//2018.11.27, czs, create in HMX, 模糊度固定的纯载波双差

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
using Geo;

namespace Gnsser.Service
{
    /// <summary>
    /// 模糊度固定的纯载波双差
    /// </summary>
    public class AmbiFixedEpochDoublePhaseOnlytDifferMatrixBuilder : MultiSiteMatrixBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        /// <param name="BaseParamCount">基础参数数量</param>
        public AmbiFixedEpochDoublePhaseOnlytDifferMatrixBuilder(GnssProcessOption option, int BaseParamCount)
            : base(option)
        {
            this.BaseParamCount = BaseParamCount;// 
            this.ParamNameBuilder = new AmbiFixedEpochDoublePhaseOnlytDifferParamNameBuilder(this.Option, BaseParamCount);


            AmbiguityManager = new AmbiguityManager(Option);


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
        /// 模糊度管理器
        /// </summary>
        public AmbiguityManager AmbiguityManager { get; set; }
        /// <summary>
        /// 模糊度存储
        /// </summary>
        public PeriodRmsedNumeralStoarge AmbiguityStoarge { get => AmbiguityManager.AmbiguityProduct; }

        /// <summary>
        /// 构建
        /// </summary>
        public override void Build()
        {
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
        public override int ObsCount { get=>(EnabledSatCount - 1); } 


        #region 创建观测信息
        /// <summary>
        /// 具有权值的观测值。
        /// </summary> 
        public override WeightedVector Observation
        {
            get
            {
                SatObsDataType phaseSatObsDataType = SatObsDataType.PhaseRangeA;
                SatApproxDataType phaseSatApproxDataType = SatApproxDataType.ApproxPhaseRangeA;

                FrequenceType frequenceType = FrequenceType.A;
                if (Option.ObsDataType.ToString().Contains(FrequenceType.A.ToString()))
                {
                    frequenceType = FrequenceType.A;
                    phaseSatObsDataType = SatObsDataType.PhaseRangeA;
                    phaseSatApproxDataType = SatApproxDataType.ApproxPhaseRangeA;

                }
                else if (Option.ObsDataType.ToString().Contains(FrequenceType.B.ToString()))
                {
                    frequenceType = FrequenceType.B;
                    phaseSatObsDataType = SatObsDataType.PhaseRangeB;
                    phaseSatApproxDataType = SatApproxDataType.ApproxPhaseRangeB;
                }
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

                //Vector phaseObs = CurrentMaterial.GetDoublePhaseDifferResidualVector(frequenceType, this.EnabledPrns, CurrentBasePrn);
                //Vector rangeObs = CurrentMaterial.GetDoubleRangeDifferResidualVector(frequenceType, this.EnabledPrns, CurrentBasePrn);



                var prns = this.EnabledPrns;
                Dictionary<SatelliteNumber, double> ambiValues = new Dictionary<SatelliteNumber, double>();
                foreach (var prn in prns)
                {
                    var name = this.GnssParamNameBuilder.GetParamName(prn);
                    var val = AmbiguityStoarge.Get(name, this.CurrentMaterial.ReceiverTime);
                    if (RmsedNumeral.IsValid(val))
                    {
                        Frequence Frequence = Gnsser.Frequence.GetFrequence(this.CurrentBasePrn, this.Option.ObsDataType, this.CurrentMaterial.ReceiverTime);
                        var valu = val.Value * Frequence.WaveLength;


                        ambiValues.Add(prn, valu);
                    }
                }

                this.EnabledPrns = new List<SatelliteNumber>(ambiValues.Keys);
                prns = this.EnabledPrns;
                prns.Add(this.CurrentBasePrn);
                this.EnabledPrns = prns;
                this.EnabledSatCount = this.EnabledPrns.Count; //update

                Vector phaseObs = CurrentMaterial.GetTwoSiteDoubleDifferResidualVector(phaseSatObsDataType, phaseSatApproxDataType, this.EnabledPrns, CurrentBasePrn);


                Vector obsMinusAppMain = new Vector(this.ObsCount);
                for (int i = 0; i < ObsCount; i++)
                {
                    var prn = prns[i];
                    if (this.CurrentBasePrn == prn) { continue; }

                    var ambi = ambiValues[prn];
                    obsMinusAppMain[i] = phaseObs[i] - ambi;

                    var name = this.GnssParamNameBuilder.GetParamName(prn);
                    obsMinusAppMain.ParamNames[i] = name;

                }
                WeightedVector deltaObs = new WeightedVector(obsMinusAppMain, BuildEpochDoubleQ());
                deltaObs.ParamNames = GetObsNames();
                return deltaObs;
                //WeightedVector deltaObs = new WeightedVector(obsMinusApp, BulidInverseWeightOfObs());
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
            int rangeRowCount = this.ObsCount;
            foreach (var prn in this.EnabledPrns)
            {
                if (prn == CurrentBasePrn) { continue; }

                int phaseRow = rangeRow + rangeRowCount;
                names[rangeRow] = GnssParamNameBuilder.GetDoubleDifferObsPCodeName("", BaseSiteName, prn, CurrentBasePrn);
                rangeRow++;
            }
            return new List<string>(names);
        }

/// <summary>
/// 每个历元的权逆阵，第一颗卫星多出现了一次，不再独立。
/// 此处令对角线为2，其余为1。
/// </summary>
/// <returns></returns>
private ArrayMatrix BuildEpochDoubleQ()
        {
            double initVal = 1.0;
            int rowCol = ObsCount;
            int rangeCount = rowCol;
            double invFactorOfPhase = this.Option.PhaseCovaProportionToRange;// 1e-6;
            if (IsBaseSatUnstable)
            {
                invFactorOfPhase = 100;
            }
            ArrayMatrix identify = new ArrayMatrix(rangeCount, rangeCount, initVal);
            ArrayMatrix diagonal = ArrayMatrix.Diagonal(rangeCount, rangeCount, initVal);

            var subM = (identify + diagonal) * invFactorOfPhase;

            //matrixQ.Set(subM); //range
            //matrixQ.Set(subM * invFactorOfPhase, rangeCount, rangeCount); //phase

            return subM;
        }

        /// <summary>
        /// 单频载波观测值的双差协方差矩阵
        /// </summary>
        /// <returns></returns>
        protected IMatrix BulidInverseWeightOfObs()
        {
            int satCount = EnabledSatCount;
            //首先建立非差观测值的权逆阵，这里前一半是rov站的，后一半是ref站的
            int row = (satCount) * 2;
            DiagonalMatrix inverseWeight = new DiagonalMatrix(row);
            double invFactorOfPhase = 1e-4;
            int index = 0;
            int baseindex = this.EpochInfoOfRov.EnabledPrns.IndexOf(CurrentBasePrn);
            EpochSatellite baseRovEle = this.EpochInfoOfRov.EnabledSats[baseindex];
            EpochSatellite baseRefEle = this.EpochInfoOfRef.EnabledSats[baseindex];
            double baseRovSat = SatWeightProvider.GetInverseWeightOfRange(baseRovEle); //返回的伪距的方差
            double baseRefSat = SatWeightProvider.GetInverseWeightOfRange(baseRefEle); //返回的伪距的方差
            inverseWeight[index, index] = baseRovSat * invFactorOfPhase;
            inverseWeight[index + satCount, index + satCount] = baseRefSat * invFactorOfPhase;

            for (int i = 0; i < satCount; i++)
            {
                if (i != baseindex)
                {
                    index++;
                    EpochSatellite e = this.EpochInfoOfRov.EnabledSats[i];
                    double inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(e); //返回的伪距的方差
                    inverseWeight[index, index] = inverseWeightOfSat * invFactorOfPhase;

                    e = this.EpochInfoOfRef[i];
                    inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(e);
                    inverseWeight[index + satCount, index + satCount] = inverseWeightOfSat * invFactorOfPhase;
                }

            }
            DiagonalMatrix undiffInverseWeigth = (inverseWeight);

            //然后，建立单差的权逆阵
            double[][] sigleCoeff = Geo.Utils.MatrixUtil.Create(satCount, satCount * 2, 0f);
            for (int i = 0; i < satCount; i++)
            {
                sigleCoeff[i][i] = 1; //流动站
                sigleCoeff[i][satCount + i] = -1; //参考站
            }
            IMatrix sigleCoeffMatrix = new ArrayMatrix(sigleCoeff);
            IMatrix sigleInverseWeigth = sigleCoeffMatrix.Multiply(undiffInverseWeigth).Multiply(sigleCoeffMatrix.Transposition);


            //最后，建立双差的权逆阵
            double[][] doubleCoeff = Geo.Utils.MatrixUtil.Create((satCount - 1), satCount, 0f);
            // int baseindex = this.RovInfo.EnabledPrns.IndexOf(CurrentBasePrn);
            int satIndex = 0;
            foreach (var item in EpochInfoOfRov.EnabledSats)
            {
                if (item.Prn != CurrentBasePrn)
                {
                    doubleCoeff[satIndex][0] = -1; //参考星
                    int index0 = this.EpochInfoOfRov.EnabledPrns.IndexOf(item.Prn);
                    doubleCoeff[satIndex][satIndex + 1] = 1; //流动星
                    satIndex++;
                }
            }
            IMatrix doubleCoeffMatrix = new ArrayMatrix(doubleCoeff);
            IMatrix doubleInverseWeigth = doubleCoeffMatrix.Multiply(sigleInverseWeigth).Multiply(doubleCoeffMatrix.Transposition);

            return doubleInverseWeigth;
        }

        /// <summary>
        /// 获取卫星在卫星参数中的编号。自动跳过基准星。
        /// </summary>
        /// <param name="prn"></param> 
        /// <returns></returns>
        public int GetSatIndexExceptBasePrn(SatelliteNumber prn)
        {
            var prns = EnabledPrns;
            int basePrnIndex = prns.IndexOf(this.CurrentBasePrn);
            int index = (prns.IndexOf(prn));

            if (index > basePrnIndex) { return index - 1; }
            return index;
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
                var prns = EnabledPrns;
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

                    if (BaseParamCount == 4)
                    {
                        double wetMap = sat.WetMap - rovSiteRefSat.WetMap;
                        A[rowIndex, 3] = wetMap;
                    }
                    else if (BaseParamCount == 5)
                    {
                        var refSiteRovSat = EpochInfoOfRef[prn];
                        double refwetMap = refSiteRovSat.WetMap - rovSiteRefSat.WetMap; //参考站对流层差分
                        double wetMap = sat.WetMap - rovSiteRefSat.WetMap;//流动站对流层差分
                        A[rowIndex, 3] = wetMap;
                        A[rowIndex, 4] = -refwetMap;
                    }
                    //A[phaseRowIndex, this.BaseParamCount + satIndex] = coeefOfPhase; //  1;//        //模糊度互差距离偏差

                    satIndex++;
                }

                return A;
            }
        }

    }//end class
}
