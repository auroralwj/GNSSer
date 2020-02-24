//2018.07.26, czs, create in HMX, 简易近距离单历元载波相位双差
//2019.01.02, czs, edit in hmx, 去掉简易（Simple）更名为 单历元双差

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
    /// 近距离单基线单历元载波相位双差
    /// </summary>
    public class EpochDoubleDifferMatrixBuilder : MultiSiteMatrixBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        /// <param name="BaseParamCount">基础参数数量</param>
        public EpochDoubleDifferMatrixBuilder(GnssProcessOption option, int BaseParamCount)
            : base(option)
        {
            this.BaseParamCount = BaseParamCount;// 
            this.ParamNameBuilder = new EpochDoubleDifferParamNameBuilder(this.Option, BaseParamCount);

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
            BaseSiteName = this.CurrentMaterial.BaseEpochInfo.SiteName;
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
        public override int ObsCount { get=>(EnabledSatCount - 1) * 2; } 

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

                FrequenceType frequenceType = FrequenceType.A;
                if (Option.ObsPhaseType == ObsPhaseType.L1)
                {
                    frequenceType = FrequenceType.A;
                    rangeSatObsDataType = SatObsDataType.PseudoRangeA;
                    phaseSatObsDataType = SatObsDataType.PhaseRangeA;
                    rangeSatApproxDataType = SatApproxDataType.ApproxPseudoRangeA;
                    phaseSatApproxDataType = SatApproxDataType.ApproxPhaseRangeA;

                }
                else if (Option.ObsPhaseType == ObsPhaseType.L2)
                {
                    frequenceType = FrequenceType.B;
                    rangeSatObsDataType = SatObsDataType.PseudoRangeB;
                    phaseSatObsDataType = SatObsDataType.PhaseRangeB;
                    rangeSatApproxDataType = SatApproxDataType.ApproxPseudoRangeB;
                    phaseSatApproxDataType = SatApproxDataType.ApproxPhaseRangeB;
                }
                //update
                Option.ApproxDataType = phaseSatApproxDataType;
                Option.ObsDataType = phaseSatObsDataType;

                //if(this.EpochInfoOfRef.ReceiverTime == Time.Parse("2017-12-27 04:25:10"))
                //{
                //    int ii = 0;
                //    var e = this.EnabledPrns;
                //}

                //var bad = Geo.Utils.ListUtil.GetExcept<SatelliteNumber>(this.EnabledPrns, this.EpochInfoOfRef.EnabledPrns);
                //var bad2 = Geo.Utils.ListUtil.GetExcept<SatelliteNumber>(this.EnabledPrns, this.EpochInfoOfRov.EnabledPrns);
                //if (bad.Count > 0 || bad2.Count > 0)
                //{
                //    int i = 0;
                //    var e = this.EnabledPrns;
                //}

                Vector phaseObs = CurrentMaterial.GetTwoSiteDoubleDifferResidualVector(phaseSatObsDataType, phaseSatApproxDataType, this.EnabledPrns, CurrentBasePrn);
                Vector rangeObs = CurrentMaterial.GetTwoSiteDoubleDifferResidualVector(rangeSatObsDataType, rangeSatApproxDataType, this.EnabledPrns, CurrentBasePrn);


                //Vector phaseObs = CurrentMaterial.GetDoublePhaseDifferResidualVector(frequenceType, this.EnabledPrns, CurrentBasePrn);
                //Vector rangeObs = CurrentMaterial.GetDoubleRangeDifferResidualVector(frequenceType, this.EnabledPrns, CurrentBasePrn);
                var prns = this.EnabledPrns;
                Vector obsMinusAppMain = new Vector(this.ObsCount);
                for (int i = 0; i < ObsCount; i++)
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

                WeightedVector deltaObs = new WeightedVector(obsMinusAppMain, BulidInverseWeightOfObs());
                deltaObs.ParamNames = BuildObsNames();
                return deltaObs;
            }
        }

        /// <summary>
        /// 观测名称
        /// </summary>
        /// <returns></returns>
        public List<string> BuildObsNames()
        {
            var names = new string[ObsCount];
            int rangeRow = 0;
            int siteCount = CurrentMaterial.Count;
            int rangeRowCount = this.ObsCount / 2;

            foreach (var prn in this.EnabledPrns)
            {
                if (prn == CurrentBasePrn) { continue; }

                int phaseRow = rangeRow + rangeRowCount;
                names[rangeRow] = GnssParamNameBuilder.GetDoubleDifferObsPCodeName( prn, CurrentBasePrn);
                names[phaseRow] = GnssParamNameBuilder.GetDoubleDifferObsLCodeName( prn, CurrentBasePrn);
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
            return obsCovaMatrixBuilder.BulidDoubleInverseWeightOfObs(CurrentMaterial, BaseSiteName, CurrentBasePrn, Option.PhaseCovaProportionToRange, false, false);

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

                Matrix A = new Matrix(ObsCount, ParamCount);
                var prns = CurrentMaterial.EnabledPrns;
                int satCount = prns.Count;
                int satIndex = 0;
                var rovSiteRefSat = EpochInfoOfRov[CurrentBasePrn];
                //基准星
                XYZ baseSatRovVector = rovSiteRefSat.EstmatedVector;
                foreach (var prn in prns)//逐个卫星遍历
                {
                    if (prn.Equals(CurrentBasePrn)) { continue; }

                    var sat = EpochInfoOfRov[prn];

                    int rowIndex = satIndex;
                    XYZ rovVector = sat.EstmatedVector;

                    A[rowIndex, 0] = -(rovVector.CosX - baseSatRovVector.CosX);//负负得正
                    A[rowIndex, 1] = -(rovVector.CosY - baseSatRovVector.CosY);
                    A[rowIndex, 2] = -(rovVector.CosZ - baseSatRovVector.CosZ);
                    //A[rowIndex, 3] = 1;//增加一个误差吸收量

                    int phaseRowIndex = rowIndex + satCount - 1;
                    A[phaseRowIndex, 0] = A[rowIndex, 0];
                    A[phaseRowIndex, 1] = A[rowIndex, 1];
                    A[phaseRowIndex, 2] = A[rowIndex, 2];
                    //A[phaseRowIndex, 3] = 1;

                    if (BaseParamCount == 4)
                    {
                        double wetMap = sat.WetMap - rovSiteRefSat.WetMap;
                        A[rowIndex, 3] = wetMap;
                        A[phaseRowIndex, 3] = wetMap;// DryWetM[1];   //流动站对流层湿延迟 
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
                    }
                    A[phaseRowIndex, this.BaseParamCount + satIndex] = coeefOfPhase; //  1;//        //模糊度互差距离偏差

                    satIndex++;
                }
                A.RowNames = this.BuildObsNames();
                A.ColNames = this.BuildParamNames();

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
                Frequence Frequence = Gnsser.Frequence.GetFrequence(this.CurrentBasePrn, this.Option.ObsPhaseType, this.CurrentMaterial.ReceiverTime);
                CoeefOfPhase = Frequence.WaveLength;
                IsCoeefOfPhaseSetted = true;
            }

            return CoeefOfPhase;
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
