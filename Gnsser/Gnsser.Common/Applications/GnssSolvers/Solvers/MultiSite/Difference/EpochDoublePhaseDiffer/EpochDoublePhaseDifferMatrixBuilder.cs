//2016.10.26, czs, create in hongqing, 搭建单历元相位双差计算框架
//2018.07.26, czs, create in HMX, 简易近距离单历元载波相位双差
//2018.12.30, czs, create in hmx, 单历元纯相位双差

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
    /// 单历元纯相位双差
    /// </summary>
    public class EpochDoublePhaseDifferMatrixBuilder : MultiSiteMatrixBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        /// <param name="BaseParamCount">基础参数数量</param>
        public EpochDoublePhaseDifferMatrixBuilder(GnssProcessOption option, int BaseParamCount)
            : base(option)
        {
            this.BaseParamCount = BaseParamCount;// 
            this.ParamNameBuilder = new EpochDoublePhaseDifferParamNameBuilder(this.Option, BaseParamCount);
            IsDueFrequence = Option.ObsPhaseType == ObsPhaseType.L1AndL2;
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
        public override int ObsCount { get=>IsDueFrequence ? (EnabledSatCount - 1) * 2 : (EnabledSatCount - 1); } 
        /// <summary>
        /// 测站数量
        /// </summary>
        public override int SiteCount { get => 2; }
        /// <summary>
        /// 是否双频
        /// </summary>
        public bool IsDueFrequence { get; set; }


        /// <summary>
        /// 第一次参数先验值。 创建初始先验参数值和协方差阵。只会执行一次。
        /// </summary> 
        protected override WeightedVector CreateInitAprioriParam()
        {
            var init =  new InitAprioriParamBuilder(this.ParamNames, this.Option).Build();
            //默认起始精度认为1dm
            init.InverseWeight[0, 0] = 1e-2;
            init.InverseWeight[1, 1] = 1e-2;
            init.InverseWeight[2, 2] = 1e-2;

            return init;
        }


        #region 创建观测信息
        /// <summary>
        /// 具有权值的观测值。
        /// </summary> 
        public override WeightedVector Observation
        {
            get
            { 
                SatObsDataType phaseL1SatObsDataType = SatObsDataType.PhaseRangeA;
                SatObsDataType phaseL2SatObsDataType = SatObsDataType.PhaseRangeB;

                SatApproxDataType phaseL1SatApproxDataType = SatApproxDataType.ApproxPhaseRangeA;
                SatApproxDataType phaseL2SatApproxDataType = SatApproxDataType.ApproxPhaseRangeB;
                 
                if (Option.ObsPhaseType == ObsPhaseType.L1)
                { 
                    phaseL1SatObsDataType = SatObsDataType.PhaseRangeA;
                    phaseL1SatApproxDataType = SatApproxDataType.ApproxPhaseRangeA;

                }
                else if (Option.ObsPhaseType == ObsPhaseType.L2)
                {
                    phaseL1SatObsDataType = SatObsDataType.PhaseRangeB;
                    phaseL1SatApproxDataType = SatApproxDataType.ApproxPhaseRangeB;
                }
                else if (IsDueFrequence)
                {
                    phaseL1SatObsDataType = SatObsDataType.PhaseRangeA;
                    phaseL1SatApproxDataType = SatApproxDataType.ApproxPhaseRangeA;
                    phaseL2SatObsDataType = SatObsDataType.PhaseRangeB;
                    phaseL2SatApproxDataType = SatApproxDataType.ApproxPhaseRangeB;
                }

                //update
                Option.ObsDataType = phaseL1SatObsDataType; 
                Option.ApproxDataType = phaseL1SatApproxDataType;

                Vector phaseL1Obs = CurrentMaterial.GetTwoSiteDoubleDifferResidualVector(phaseL1SatObsDataType, phaseL1SatApproxDataType, this.EnabledPrns, CurrentBasePrn);
                Vector phaseL2Obs = null;
                if (IsDueFrequence)
                {
                    phaseL2Obs = CurrentMaterial.GetTwoSiteDoubleDifferResidualVector(phaseL2SatObsDataType, phaseL2SatApproxDataType, this.EnabledPrns, CurrentBasePrn);
                }
                 
                var prns = this.EnabledPrns;
                Vector obsMinusAppMain = new Vector(this.ObsCount);
                for (int i = 0; i < ObsCount; i++)
                {
                    if (i < phaseL1Obs.Count)
                    {
                        obsMinusAppMain[i] = phaseL1Obs[i];
                    }
                    else
                    {
                        obsMinusAppMain[i] = phaseL2Obs[i - phaseL1Obs.Count];
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

            int L1 = 0;
            int siteCount = CurrentMaterial.Count;
            int halfCount = this.ObsCount / 2;

            foreach (var prn in this.EnabledPrns)
            {
                if (prn == CurrentBasePrn) { continue; }

                names[L1] = GnssParamNameBuilder.GetDoubleDifferObsCodeNameOf(prn, CurrentBasePrn, Gnsser.ParamNames.L1Code);

                if (IsDueFrequence)
                {
                    int L2 = L1 + halfCount;
                    names[L2] = GnssParamNameBuilder.GetDoubleDifferObsCodeNameOf(prn, CurrentBasePrn, Gnsser.ParamNames.L2Code);
                }
                L1++;
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
            return obsCovaMatrixBuilder.BulidDoubleInverseWeightOfObs(CurrentMaterial, BaseSiteName, CurrentBasePrn, Option.PhaseCovaProportionToRange,true, IsDueFrequence);
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
                double coeefOfPhaseL2 = CheckAndGetCoeefOfPhaseL2();

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

                    int rowIndexOfL2 = rowIndex + satCount - 1;
                    if (IsDueFrequence)
                    {
                        A[rowIndexOfL2, 0] = A[rowIndex, 0];
                        A[rowIndexOfL2, 1] = A[rowIndex, 1];
                        A[rowIndexOfL2, 2] = A[rowIndex, 2];
                    }

                    if (BaseParamCount == 4)
                    {
                        double wetMap = sat.WetMap - rovSiteRefSat.WetMap;
                        A[rowIndex, 3] = wetMap;
                        if (IsDueFrequence)
                        {
                            A[rowIndexOfL2, 3] = wetMap;// DryWetM[1];   //流动站对流层湿延迟 
                        }
                    }
                    else if (BaseParamCount == 5)
                    {
                        var refSiteRovSat = EpochInfoOfRef[prn];
                        double refwetMap = refSiteRovSat.WetMap - rovSiteRefSat.WetMap; //参考站对流层差分
                        double wetMap = sat.WetMap - rovSiteRefSat.WetMap;//流动站对流层差分
                        A[rowIndex, 3] = wetMap;
                        A[rowIndex, 4] = -refwetMap;
                        if (IsDueFrequence)
                        {
                            A[rowIndexOfL2, 3] = wetMap;// DryWetM[1];   //流动站对流层湿延迟
                            A[rowIndexOfL2, 4] = -refwetMap;// DryWetM[1];   //基准站对流层湿延迟
                        }
                    }
                    A[rowIndex, this.BaseParamCount + satIndex] = coeefOfPhase; //  1;//        //模糊度互差距离偏差
                    if (IsDueFrequence)
                    {
                        A[rowIndexOfL2, this.BaseParamCount + (satCount -1)+ satIndex] = coeefOfPhaseL2; //  1;//        //模糊度互差距离偏差
                    }
                    satIndex++;
                }

                A.RowNames = this.BuildObsNames();
                A.ColNames = this.BuildParamNames();

                return A;
            }
        }
    }//end class
}
