//2018.11.05, czs, create in HMX, 单差网解定位

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
    /// 单差网解定位
    /// </summary>
    public class NetSingleDifferPositionMatrixBuilder : MultiSiteMatrixBuilder// BasePositionMatrixBuilder
    {
        /// <summary>
        /// 无电离层双差矩阵构造器
        /// </summary>
        /// <param name="option"></param>
        /// <param name="baseParamCount"></param>
        public NetSingleDifferPositionMatrixBuilder(
            GnssProcessOption option, int baseParamCount)
            : base(option)
        {
            IsEstimateTropWetZpd = option.IsEstimateTropWetZpd;
            this.ParamNameBuilder = new NetSingleDifferPositionParamNameBuilder(option);
            this.BaseParamCount = baseParamCount;

            IsDualIonoFreeComObservation = Option.IsDualIonoFreeComObservation;
        }
        /// <summary>
        /// 是否采用双频无电离层组合观测值
        /// </summary>
        public bool IsDualIonoFreeComObservation { get; set; }
        /// <summary>
        /// 是否估计对流层湿延迟参数。
        /// </summary>
        public bool IsEstimateTropWetZpd { get; set; }

        /// <summary>
        /// 参数是否改变。
        /// </summary>
        public override bool IsParamsChanged
        {
            get
            {
                return base.IsParamsChanged || this.IsBaseSatUnstable;
            }
        }  

        /// <summary>
        /// 基础参数的总数，即除了模糊度的剩余参数的个数
        /// 长基线时是5，即三个坐标参数+两个对流层参数
        /// 短基线时是3，即三个坐标参数
        /// </summary>
        public int BaseParamCount { get; set; }

        /// <summary>
        /// 构建
        /// </summary>
        public override void Build()
        {
            if(String.IsNullOrWhiteSpace(BaseSiteName))
            {
                BaseSiteName = CurrentMaterial.BaseSiteName;
            }

            //本类所独有的
            GnssParamNameBuilder.BaseParamCount = BaseParamCount;

            foreach (var item in CurrentMaterial)
            {
                if (item[CurrentBasePrn].IsUnstable)
                {
                    IsBaseSatUnstable = true;
                }
            }

            base.Build();
        }

        /// <summary>
        /// 观测值数量,伪距加载波，除去一个测站，卫星保持完整。
        /// </summary>
        public override int ObsCount { get { return 2 * (EnabledSatCount) * (SiteCount - 1 ); } }
        /// <summary>
        /// 测站数量
        /// </summary>
        public int SiteCount { get => CurrentMaterial.Count; }


        #region 创建观测信息 
        /// <summary>
        /// 具有权值的观测值。
        /// </summary> 
        public override WeightedVector Observation
        {
            get
            {
                IMatrix inverseWeightOfObs = BulidInverseWeightOfObs();
                WeightedVector deltaObs = new WeightedVector(ObservationVector, inverseWeightOfObs);
                return deltaObs;
            }
        }

        /// <summary>
        /// 观测值。定位采用站间单差。
        /// 自由项 l，观测值减去先验值或估计值。
        /// 常数项，观测误差方程的常数项,或称自由项
        /// </summary>
        public virtual IVector ObservationVector
        {
            get
            {
                int enableSatCount = this.EnabledSatCount;
                SatObsDataType rangeSatObsDataType = SatObsDataType.IonoFreeRange;
                SatObsDataType phaseSatObsDataType = SatObsDataType.IonoFreePhaseRange;
                SatApproxDataType rangeSatApproxDataType = SatApproxDataType.IonoFreeApproxPseudoRange;
                 SatApproxDataType phaseSatApproxDataType = SatApproxDataType.IonoFreeApproxPhaseRange;

                //若不采用无电离层组合，则采用单频计算。适用于小区域布网,如阵地
                if (!IsDualIonoFreeComObservation)
                {
                    FrequenceType frequenceType = FrequenceType.A;

                    if (Option.ObsDataType.ToString().Contains(FrequenceType.A.ToString()))
                    {
                        frequenceType = FrequenceType.A;
                        rangeSatObsDataType = SatObsDataType.PseudoRangeA;
                        phaseSatObsDataType = SatObsDataType.PhaseRangeA;
                        rangeSatApproxDataType = SatApproxDataType.ApproxPseudoRangeA;
                        phaseSatApproxDataType = SatApproxDataType.ApproxPhaseRangeA;

                    }
                    else if (Option.ObsDataType.ToString().Contains(FrequenceType.B.ToString()))
                    {
                        frequenceType = FrequenceType.B;
                        rangeSatObsDataType = SatObsDataType.PseudoRangeB;
                        phaseSatObsDataType = SatObsDataType.PhaseRangeB;
                        rangeSatApproxDataType = SatApproxDataType.ApproxPseudoRangeB;
                        phaseSatApproxDataType = SatApproxDataType.ApproxPhaseRangeB;
                    }
                }

                Vector rangeObs = CurrentMaterial.GetNetDifferResidualVectorBetweenSites(rangeSatObsDataType, rangeSatApproxDataType, this.BaseSiteName, CurrentBasePrn);
                Vector phaseObs = CurrentMaterial.GetNetDifferResidualVectorBetweenSites(phaseSatObsDataType, phaseSatApproxDataType, this.BaseSiteName, CurrentBasePrn);

                //--------------------check----------------
                if (false)
                {
                    Vector rangeObs1 = CurrentMaterial.GetNetDoubleDifferResidualVectorBeweenSites(rangeSatObsDataType, rangeSatApproxDataType, this.BaseSiteName, CurrentBasePrn);
                    if (rangeObs1.Equals(rangeObs))//【校验通过】
                    {
                        int iii = 0;
                    }
                }
                //-----------------end check -------------------

                int halfCount = this.ObsCount / 2;
                Vector obsMinusAppMain = new Vector(this.ObsCount);
                int siteCount = SiteCount;

                for (int i = 0; i < this.ObsCount; i++)
                {
                    if (i < halfCount)
                    {
                        obsMinusAppMain[i] = rangeObs[i];
                    }
                    else
                    {
                        int index = i - halfCount;
                        obsMinusAppMain[i] = phaseObs[index];
                    }
                }
                WeightedVector deltaObs = new WeightedVector(obsMinusAppMain, BulidInverseWeightOfObs()) { ParamNames = GetObsNames() } ;
                return deltaObs;
            }
        }

        /// <summary>
        /// 观测量的权逆阵，一个对称阵。
        /// 双差对角线应该加倍？？？？？？此处先忽略/。。。。。。。。。。。
        /// </summary>  
        /// <returns></returns>
        public IMatrix BulidInverseWeightOfObs()
        {
            DiagonalMatrix inverseWeight = new DiagonalMatrix(this.ObsCount);
            double[] inverseWeightVector = inverseWeight.Vector;
            double invFactorOfRange = 1;
            double invFactorOfPhase = Option.PhaseCovaProportionToRange;
            int rangeRowCount = ObsCount / 2;
            int rangeRow = 0;
            foreach (var site in this.CurrentMaterial)
            {
                var siteName = site.SiteName;
                if (String.Equals(siteName, BaseSiteName, StringComparison.CurrentCultureIgnoreCase)) { continue; }

                foreach (var sat in site.EnabledSats)
                {
                    int phaseRow = rangeRow + rangeRowCount;
                    double inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(sat);
                    inverseWeight[rangeRow] = inverseWeightOfSat * invFactorOfRange;
                    inverseWeight[phaseRow] = inverseWeightOfSat * invFactorOfPhase;
                    rangeRow++;
                }
            }
            return inverseWeight;
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
            int rangeRowCount = this.ObsCount / 2;
            foreach (var site in CurrentMaterial)
            {
                var siteName = site.SiteName;
                if( String.Equals( siteName, BaseSiteName , StringComparison.CurrentCultureIgnoreCase)){ continue; }

                foreach (var prn in this.EnabledPrns)
                { 
                    int phaseRow = rangeRow + rangeRowCount;
                    names[rangeRow] = GnssParamNameBuilder.GetDifferObsPCodeName(siteName , BaseSiteName, prn);
                    names[phaseRow] = GnssParamNameBuilder.GetDifferObsLCodeName(siteName, BaseSiteName, prn);
                    rangeRow++;
                }
            }
            return new List<string>(names);
        }

        #endregion

        #region 公共矩阵生成

        #region 系数矩阵生成 

        /// <summary>
        /// 参数平差系数阵 A。前一半行数为伪距观测量，后一半为载波观测量。构建为稀疏矩阵
        /// </summary>
        public override Matrix Coefficient
        {
            get
            {
                var paramNameBuilder = (NetSingleDifferPositionParamNameBuilder)this.ParamNameBuilder;

                var A = new Matrix(ObsCount, ParamCount);
                if (ObsCount < ParamCount)
                {
                    log.Warn("小心：观测方程少于参数数量： " + ObsCount + " < " + ParamCount + ", " + this.CurrentMaterial);
                }
                //相位模糊度系数，可以为米和周，按照设置文件决定。
                double coeefOfPhase = CheckAndGetCoeefOfPhase();

                int rangeRowCount = this.ObsCount / 2;
                int rangeRow = 0;//行的索引号，对应观测方程行
                int colIndex = 0;//列索引，对应参数编号
                                 //第一次为伪距，第二次为载波
                var baseZpdname = paramNameBuilder.GetSiteWetTropZpdName(BaseSiteName);
                int colIndexOfBaseZpdname = this.ParamNames.IndexOf(baseZpdname); //基准站对流层位置相同，但是各颗卫星系数（映射函数）不同
                var baseSite = this.CurrentMaterial[BaseSiteName];
                foreach (var site in CurrentMaterial)
                {
                    var siteName = site.SiteName;
                    if (siteName == BaseSiteName) { continue; }//基准站已经被差分掉了

                    //1.测站相关 
                    //基准站对流层
                    var siteZpdName = paramNameBuilder.GetSiteWetTropZpdName(siteName);
                    int colIndexOfZpd = this.ParamNames.IndexOf(siteZpdName);

                    var clkName = paramNameBuilder.GetSiteClockDiffer(siteName, BaseSiteName);
                    int colIndexOfClk = this.ParamNames.IndexOf(clkName);

                    //2.卫星相关，或站星相关
                    foreach (var sat in site.EnabledSats)
                    {
                        var prn = sat.Prn;

                        int phaseRow = rangeRow + rangeRowCount;
                        var baseSat = baseSite[prn];//基准站的对应卫星
                        //var baseVector = baseSat.EstmatedVector;
                        var vector = sat.EstmatedVector;

                        //测站坐标
                        var satXyzNames = paramNameBuilder.GetSiteDxyz(site.SiteName);
                        colIndex = this.ParamNames.IndexOf(satXyzNames[0]);
                        A[rangeRow, colIndex + 0] = -vector.CosX ;// - (vector.CosX - baseVector.CosX);
                        A[rangeRow, colIndex + 1] = -vector.CosY;// -(vector.CosY - baseVector.CosY);
                        A[rangeRow, colIndex + 2] = -vector.CosZ;// -(vector.CosZ - baseVector.CosZ);
                        A[phaseRow, colIndex + 0] = A[rangeRow, colIndex + 0];
                        A[phaseRow, colIndex + 1] = A[rangeRow, colIndex + 1];
                        A[phaseRow, colIndex + 2] = A[rangeRow, colIndex + 2];

                        //测站接收机钟差
                        A[rangeRow, colIndexOfClk] = 1;
                        A[phaseRow, colIndexOfClk] = 1;

                        //对流层湿延迟天顶距
                        if (IsEstimateTropWetZpd)
                        {
                            //测站接收对流层
                            A[rangeRow, colIndexOfBaseZpdname] = -baseSat.WetMap;
                            A[rangeRow, colIndexOfZpd] = sat.WetMap;
                            A[phaseRow, colIndexOfBaseZpdname] = A[rangeRow, colIndexOfBaseZpdname];// - baseSat.WetMap;
                            A[phaseRow, colIndexOfZpd] = A[rangeRow, colIndexOfZpd];// sat.WetMap;
                        }

                        //载波相位模糊度，只有相位才有此参数
                        var ambi = paramNameBuilder.GetSingleDifferAmbiParamName(siteName, sat.Prn);
                        colIndex = this.ParamNames.IndexOf(ambi);
                        A[phaseRow, colIndex] = coeefOfPhase;

                        rangeRow++;
                    }
                }
                A.ColNames = ParamNames;
                A.RowNames = GetObsNames();
                return A;
            }
        }

        #endregion
        
        #endregion 
    }//end class
}
