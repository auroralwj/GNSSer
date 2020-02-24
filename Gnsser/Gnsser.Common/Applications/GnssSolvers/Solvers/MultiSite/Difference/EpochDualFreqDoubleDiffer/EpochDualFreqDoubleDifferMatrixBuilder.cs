//2018.07.26, czs, create in HMX, 简易近距离单历元载波相位双差

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm; 
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Times;
using Gnsser.Times;
using Gnsser.Service;
using Gnsser.Domain;
using Gnsser.Checkers;
using Gnsser.Models;
using Gnsser.Data.Rinex;

namespace Gnsser.Service
{
    /// <summary>
    /// 简易近距离单基线单历元载波相位双差
    /// </summary>
    public class EpochDualFreqDoubleDifferMatrixBuilder : MultiSiteMatrixBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        /// <param name="BaseParamCount">基础参数数量</param>
        public EpochDualFreqDoubleDifferMatrixBuilder(GnssProcessOption option, int BaseParamCount)
            : base(option)
        {
            this.BaseParamCount = BaseParamCount;// 
            this.ParamNameBuilder = new EpochDualFreqDoubleDifferParamNameBuilder(this.Option, BaseParamCount);
        }
        /// <summary>
        /// 不变参数数量
        /// </summary>
        public int BaseParamCount { get; set; }
        /// <summary>
        /// 参数是否改变。
        /// </summary>
        public override bool IsParamsChanged { get { return base.IsParamsChanged || this.IsBaseSatUnstable; } }
        /// <summary>
        /// 参考站信息
        /// </summary>
        EpochInformation EpochInfoOfRef { get { return CurrentMaterial.BaseEpochInfo; } }
        /// <summary>
        /// 流动站
        /// </summary>
        EpochInformation EpochInfoOfRov { get { return CurrentMaterial.OtherEpochInfo; } }

        /// <summary>
        /// 构建
        /// </summary>
        public override void Build()
        {
            this.BaseSiteName = this.CurrentMaterial.BaseEpochInfo.SiteName;
           

            //本类所独有的
            IsBaseSatUnstable = false;

            if (!IsBaseSatUnstable && (this.EpochInfoOfRef[CurrentBasePrn].IsUnstable || this.EpochInfoOfRov[CurrentBasePrn].IsUnstable))
            {
                IsBaseSatUnstable = true;
            }
            base.Build();
        }
        
        /// <summary>
        /// 方程数量
        /// </summary>
        public override int ObsCount { get=>(EnabledSatCount - 1) * 2 * 2; } 

        public override int SiteCount { get => 2; }


        #region 创建观测信息
        /// <summary>
        /// 具有权值的观测值。
        /// </summary> 
        public override WeightedVector Observation
        {
            get
            {
                SatObsDataType rangeSatObsDataType = SatObsDataType.PseudoRangeA;
                SatObsDataType phaseSatObsDataType = SatObsDataType.PhaseRangeA;

                SatApproxDataType rangeSatApproxDataType = SatApproxDataType.ApproxPseudoRangeA;
                SatApproxDataType phaseSatApproxDataType = SatApproxDataType.ApproxPhaseRangeA;


                var frequenceType2 = FrequenceType.B;
                var rangeSatObsDataType2 = SatObsDataType.PseudoRangeB;
                var phaseSatObsDataType2 = SatObsDataType.PhaseRangeB;
                var rangeSatApproxDataType2 = SatApproxDataType.ApproxPseudoRangeB;
                var phaseSatApproxDataType2 = SatApproxDataType.ApproxPhaseRangeB;

                //update
                Option.ApproxDataType = phaseSatApproxDataType;
                Option.ObsDataType = phaseSatObsDataType;


                Vector phaseObs = CurrentMaterial.GetTwoSiteDoubleDifferResidualVector(phaseSatObsDataType, phaseSatApproxDataType, this.EnabledPrns, CurrentBasePrn);
                Vector rangeObs = CurrentMaterial.GetTwoSiteDoubleDifferResidualVector(rangeSatObsDataType, rangeSatApproxDataType, this.EnabledPrns, CurrentBasePrn);

                Vector phaseObs2 = CurrentMaterial.GetTwoSiteDoubleDifferResidualVector(phaseSatObsDataType2, phaseSatApproxDataType2, this.EnabledPrns, CurrentBasePrn);
                Vector rangeObs2 = CurrentMaterial.GetTwoSiteDoubleDifferResidualVector(rangeSatObsDataType2, rangeSatApproxDataType2, this.EnabledPrns, CurrentBasePrn);
                

                var prns = this.EnabledPrns;
                Vector obsMinusAppMain = new Vector(this.ObsCount);
                //L1
                for (int i = 0; i < ObsCount/2; i++)
                {
                    if (i < rangeObs.Count)
                    {
                        obsMinusAppMain[i] = rangeObs[i];

                    }
                    else
                    {
                        obsMinusAppMain[i] = phaseObs[i - phaseObs.Count];
                    }
                }
                //L2
                int halfObsCount = ObsCount / 2;
                for (int i = halfObsCount; i < ObsCount; i++)
                {
                    if (i < halfObsCount + rangeObs2.Count)
                    {
                        obsMinusAppMain[i] = rangeObs2[i - halfObsCount];

                    }
                    else
                    {
                        obsMinusAppMain[i] = phaseObs2[i - halfObsCount- phaseObs2.Count];
                    }
                }


                WeightedVector deltaObs = new WeightedVector(obsMinusAppMain, BulidInverseWeightOfObs());
                deltaObs.ParamNames = GetObsNames();
                return deltaObs;
            }
        }

        /// <summary>
        /// 观测名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetObsNames()
        {
            var names = new string[ObsCount];
            int rangeRow = 0;
            int siteCount = CurrentMaterial.Count;
            int range1RowCount = this.ObsCount / 4;

            foreach (var prn in this.EnabledPrns)
            {
                if (prn == CurrentBasePrn) { continue; }

                int phaseRow = rangeRow + range1RowCount;
                names[rangeRow] = GnssParamNameBuilder.GetDoubleDifferObsCodeNameOf(prn, CurrentBasePrn, Gnsser.ParamNames.P1Code);
                names[phaseRow] = GnssParamNameBuilder.GetDoubleDifferObsCodeNameOf(prn, CurrentBasePrn, Gnsser.ParamNames.L1Code);
                rangeRow++;
            }
            rangeRow += range1RowCount;
            foreach (var prn in this.EnabledPrns)
            {
                if (prn == CurrentBasePrn) { continue; }

                int phaseRow = rangeRow + range1RowCount; 
                names[rangeRow] = GnssParamNameBuilder.GetDoubleDifferObsCodeNameOf(prn, CurrentBasePrn, Gnsser.ParamNames.P2Code);
                names[phaseRow] = GnssParamNameBuilder.GetDoubleDifferObsCodeNameOf(prn, CurrentBasePrn, Gnsser.ParamNames.L2Code);

                rangeRow++;
            }
            return new List<string>(names);
        }


        /// <summary>
        /// 观测量的权逆阵。按照先伪距，后载波的顺序排列。
        /// 标准差由常数确定，载波比伪距标准差通常是 1:100，其方差比是 1:10000.
        /// 1.0/140*140=5.10e-5
        /// </summary> 
        /// <returns></returns>
        protected IMatrix BulidInverseWeightOfObs()
        {
            ObsCovaMatrixBuilder obsCovaMatrixBuilder = new ObsCovaMatrixBuilder(this.SatWeightProvider);
            return obsCovaMatrixBuilder.BulidDoubleInverseWeightOfObs(CurrentMaterial, BaseSiteName, CurrentBasePrn, Option.PhaseCovaProportionToRange, false, true);
        }

        #endregion

        /// <summary>
        /// 误差方程系数阵，设计矩阵。
        /// </summary> 
        public override Matrix Coefficient
        {
            get
            {
                //相位模糊度系数，可以为米和周，按照设置文件决定。
                double coeefOfPhase = CheckAndGetCoeefOfPhase();
                double coeefOfPhaseL2 = CheckAndGetCoeefOfPhase2();

                Matrix A = new Matrix(ObsCount, ParamCount);
                var prns = CurrentMaterial.EnabledPrns;
                int satCount = prns.Count;
                int satIndex = 0;
                var rovSiteRefSat = EpochInfoOfRov[CurrentBasePrn];
                int halfObsCount = ObsCount / 2;
                //基准星
                XYZ baseSatRovVector = rovSiteRefSat.EstmatedVector;
                foreach (var prn in prns)//逐个卫星遍历
                {
                    if (prn.Equals(CurrentBasePrn)) { continue; }

                    var sat = EpochInfoOfRov[prn];

                    int rowIndex = satIndex;
                    XYZ rovVector = sat.EstmatedVector;
                    //频率1 伪距
                    A[rowIndex, 0] = -(rovVector.CosX - baseSatRovVector.CosX);//负负得正
                    A[rowIndex, 1] = -(rovVector.CosY - baseSatRovVector.CosY);
                    A[rowIndex, 2] = -(rovVector.CosZ - baseSatRovVector.CosZ);
                    //A[rowIndex, 3] = 1;//增加一个误差吸收量
                    //频率1载波
                    int phaseRowIndex = rowIndex + satCount - 1;
                    A[phaseRowIndex, 0] = A[rowIndex, 0];
                    A[phaseRowIndex, 1] = A[rowIndex, 1];
                    A[phaseRowIndex, 2] = A[rowIndex, 2];
                    //A[phaseRowIndex, 3] = 1;

                    //频率2伪距
                    int rowIndexOfP2 = halfObsCount + rowIndex;
                    A[rowIndexOfP2, 0] = A[rowIndex, 0];
                    A[rowIndexOfP2, 1] = A[rowIndex, 1];
                    A[rowIndexOfP2, 2] = A[rowIndex, 2];

                    //频率2载波
                    int phaseRowIndexOfL2 = halfObsCount + rowIndex + satCount - 1;
                    A[phaseRowIndexOfL2, 0] = A[rowIndex, 0];
                    A[phaseRowIndexOfL2, 1] = A[rowIndex, 1];
                    A[phaseRowIndexOfL2, 2] = A[rowIndex, 2];


                    if (BaseParamCount == 4)
                    {
                        double wetMap = sat.WetMap - rovSiteRefSat.WetMap;
                        A[rowIndex, 3] = wetMap;
                        A[phaseRowIndex, 3] = wetMap;// DryWetM[1];   //流动站对流层湿延迟
                        A[rowIndexOfP2, 3] = wetMap;
                        A[phaseRowIndexOfL2, 3] = wetMap;

                    }
                    else if (BaseParamCount == 5)
                    {
                        var refSiteRovSat = EpochInfoOfRef[prn];
                        double refwetMap = refSiteRovSat.WetMap - rovSiteRefSat.WetMap; //参考站对流层差分
                        double wetMap = sat.WetMap - rovSiteRefSat.WetMap;//流动站对流层差分
                        A[rowIndex, 3] = wetMap;
                        A[rowIndex, 4] = -refwetMap;
                        A[phaseRowIndex, 3] = wetMap;// DryWetM[1];   //流动站对流层湿延迟
                        A[phaseRowIndex, 4] = -refwetMap;// DryWetM[1];   //基准站对流层湿延迟


                        A[rowIndexOfP2, 3] = wetMap;
                        A[rowIndexOfP2, 4] = -refwetMap;
                        A[phaseRowIndexOfL2, 3] = wetMap;
                        A[phaseRowIndexOfL2, 4] = -refwetMap;
                    }

                    A[phaseRowIndex, this.BaseParamCount + satIndex] = coeefOfPhase; //  1;//        //模糊度互差距离偏差

                    A[phaseRowIndexOfL2, this.BaseParamCount + (this.EnabledSatCount -1)+ satIndex] = coeefOfPhaseL2; //  1;//        //模糊度互差距离偏差
                    satIndex++;
                }

                return A;
            }
        }
        protected override double CheckAndGetCoeefOfPhase()
        {
            if (IsCoeefOfPhaseSetted)
            {
                return CoeefOfPhase;
            }

            CoeefOfPhase = 1;
            if (!Option.IsPhaseInMetterOrCycle)
            {
                //获取当前单频的信号频率,若是无电离层猪组合，则采用L1的频率
                Frequence Frequence = Gnsser.Frequence.GetFrequence(this.CurrentBasePrn, FrequenceType.A, this.CurrentMaterial.ReceiverTime);
                CoeefOfPhase = Frequence.WaveLength;
                IsCoeefOfPhaseSetted = true;
            }

            return CoeefOfPhase;
        }

        protected bool IsCoeefOfPhase2Setted = false;
        double CoeefOfPhaseL2 = 0;
        protected   double CheckAndGetCoeefOfPhase2()
        {
            if (IsCoeefOfPhase2Setted)
            {
                return CoeefOfPhaseL2;
            }

            CoeefOfPhaseL2 = 1;
            if (!Option.IsPhaseInMetterOrCycle)
            {
                //获取当前单频的信号频率,若是无电离层猪组合，则采用L1的频率
                Frequence Frequence = Gnsser.Frequence.GetFrequence(this.CurrentBasePrn, FrequenceType.B, this.CurrentMaterial.ReceiverTime);
                CoeefOfPhaseL2 = Frequence.WaveLength;
                IsCoeefOfPhase2Setted = true;
            }

            return CoeefOfPhaseL2;
        }

        public override WeightedMatrix Transfer
        {
            get
            {
                return base.Transfer;
            }
        }

    }//end class
}
