//2017.8.1, cy, create in chongqing, 双频相位载波双差矩阵构建类

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm; 
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates;
using Geo.Algorithm;
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
    /// 单历元相位双差
    /// </summary>
    public class EpochDouFreDoubleDifferMatrixBuilder : MultiSiteMatrixBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        public EpochDouFreDoubleDifferMatrixBuilder(
            GnssProcessOption option, int baseParamCount)
            : base(option)
        {
            this.ParamStateTransferModelManager = new ParamStateTransferModelManager(option);
            foreach (var item in Gnsser.ParamNames.Dxyz)
            {
                ParamStateTransferModelManager.Add(item, new StaticTransferModel(Option.StdDevOfStaticTransferModel));
            }
            if (baseParamCount == 4)
            { ParamStateTransferModelManager.Add(Gnsser.ParamNames.WetTropZpd, new RandomWalkModel(Option.StdDevOfTropoRandomWalkModel)); }
            if (baseParamCount == 5)
            {
                ParamStateTransferModelManager.Add(Gnsser.ParamNames.WetTropZpd, new RandomWalkModel(Option.StdDevOfTropoRandomWalkModel));
                ParamStateTransferModelManager.Add(Gnsser.ParamNames.RefWetTrop, new RandomWalkModel(Option.StdDevOfTropoRandomWalkModel));
            }
            this.BaseParamCount = baseParamCount;
            this.ParamNameBuilder = new PeriodDoubleDifferParamNameBuilder(this.Option);
            
        }
        /// <summary>
        /// 基础（不变）参数个数
        /// </summary>
        public int BaseParamCount { get; set; }
        /// <summary>
        /// 参数是否改变。
        /// </summary>
        public override bool IsParamsChanged { get { return base.IsParamsChanged || this.IsBaseSatUnstable; } }
        /// <summary>
        /// 参考站信息
        /// </summary>
        EpochInformation RefInfo { get { return CurrentMaterial.BaseEpochInfo; } }

        EpochInformation RovInfo { get { return CurrentMaterial.OtherEpochInfo; } }
         

        /// <summary>
        /// 构建
        /// </summary>
        public override void Build()
        {
            //本类所独有的
            if (this.RefInfo[CurrentBasePrn].IsUnstable || this.RovInfo[CurrentBasePrn].IsUnstable)
            {
                IsBaseSatUnstable = true;
            }

            this.ParamNames = BuildParamNames();     
            base.Build();

          
        }

        public override List<string> BuildParamNames()
        {
           // this.ParamNames = GnssParamNameBuilder.SetBasePrn(this.CurrentBasePrn).SetPrns(this.EnabledPrns).Build();
            GnssParamNameBuilder.SetBasePrn(this.CurrentBasePrn).SetPrns(this.EnabledPrns).SetSatelliteTypes(this.CurrentMaterial.SatelliteTypes); 

            if (SatelliteNumber.IsNullOrDefault(this.CurrentBasePrn)) throw new Geo.ShouldNotHappenException("请设置基础卫星");
            List<SatelliteNumber> enabledPrns = new List<SatelliteNumber>();

            //双差才参数名称，只有坐标和模糊度互差
            List<string> ParamNames = new List<string>(Gnsser.ParamNames.Dxyz);
            if (this.BaseParamCount == 4) { ParamNames.Add(Gnsser.ParamNames.WetTropZpd); }
            if (this.BaseParamCount == 5) { ParamNames.Add(Gnsser.ParamNames.WetTropZpd); ParamNames.Add(Gnsser.ParamNames.RefWetTrop); }

            foreach (var prn in this.EnabledPrns)
            {
                if (prn.Equals(this.CurrentBasePrn)) { continue; }
                string paramname = prn + Gnsser.ParamNames.Pointer + this.CurrentBasePrn + Gnsser.ParamNames.DoubleDifferL1AmbiguitySuffix;
                ParamNames.Add(paramname);
            }

            foreach (var prn in this.EnabledPrns)
            {
                if (prn.Equals(this.CurrentBasePrn)) { continue; }
                string paramname = prn + Gnsser.ParamNames.Pointer + this.CurrentBasePrn + Gnsser.ParamNames.DoubleDifferL2AmbiguitySuffix;
                ParamNames.Add(paramname);
            }
            return ParamNames;
    
        }

        public override void UpdateStateTransferModels()
        {
            if (this.ParamNames.Count == 0)
            {
                foreach (var prn in EnabledPrns)
                {
                    //if (prn == this.CurrentBasePrn) continue;
                    var paramName = ((GnssParamNameBuilder)ParamNameBuilder).GetParamName(prn);
                    if (!ParamStateTransferModelManager.Contains(paramName))
                    {
                        ParamStateTransferModelManager[paramName] = new Gnsser.Models.SingleSatPhaseAmbiguityModel(prn, Option.StdDevOfPhaseModel, Option.StdDevOfCycledPhaseModel);
                    }
                }
            }
            else
            {
                foreach (var prn in EnabledPrns)
                {
                    if (prn == this.CurrentBasePrn) continue;
                    foreach(var item in ParamNames)
                    {
                        if(item.Contains(prn.ToString()))
                        {
                            if (!ParamStateTransferModelManager.Contains(item))
                            {
                                ParamStateTransferModelManager[item] = new Gnsser.Models.SingleSatPhaseAmbiguityModel(prn, Option.StdDevOfPhaseModel, Option.StdDevOfCycledPhaseModel);
                            }
                        }
                    }           
                }
            }
        }

        /// <summary>
        /// 参数数量
        /// </summary>
        public override int ParamCount { get { return BaseParamCount + (EnabledSatCount - 1) * 2; } } //双频载波双差定位

        /// <summary>
        /// 方程数量
        /// </summary>
        public override int ObsCount { get { return (EnabledSatCount - 1) * 2; } }

        #region 参数先验信息
        /// <summary>
        /// 创建先验信息
        /// </summary> 
        protected override WeightedVector CreateInitAprioriParam()
        {
            return PppMatrixHelper.GetInitDoubleAprioriParam((this.CurrentMaterial.EnabledSatCount - 1) * 2 + BaseParamCount, BaseParamCount, this.Option.InitApproxXyzRms);
        }
        #endregion

        #region 创建观测信息
        /// <summary>
        /// 具有权值的观测值。观测值顺序调整了，基准星的数据放在第一个位置。
        /// </summary> 
        public override WeightedVector Observation
        {
            get
            {
               // Vector obsMinusApp = CurrentMaterial.GetDoubleDifferResidualVector(this.Option.ObsPhaseDataType, this.Option.ApproxDataType, this.EnabledPrns, CurrentBasePrn);
                Vector obsMinusApp1 = CurrentMaterial.GetTwoSiteDoubleDifferResidualVector(SatObsDataType.PhaseRangeA, SatApproxDataType.ApproxPhaseRangeA, this.EnabledPrns, CurrentBasePrn);
                Vector obsMinusApp2 = CurrentMaterial.GetTwoSiteDoubleDifferResidualVector(SatObsDataType.PhaseRangeB, SatApproxDataType.ApproxPhaseRangeB, this.EnabledPrns, CurrentBasePrn);
                Vector obsMinusApp = new Vector(obsMinusApp1.Count + obsMinusApp2.Count);

                int i=0;
                foreach (var item in obsMinusApp1) { obsMinusApp[i] = item; i++; }
                foreach (var item in obsMinusApp2) { obsMinusApp[i] = item; i++; }

               // WeightedVector deltaObs = new WeightedVector(obsMinusApp, BuildEpochDoubleQ());
                WeightedVector deltaObs = new WeightedVector(obsMinusApp, BulidInverseWeightOfObs());
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
            int rangeRowCount = this.ObsCount / 2;

            foreach (var prn in this.EnabledPrns)
            {
                if (prn == CurrentBasePrn) { continue; }

                int phaseRow = rangeRow + rangeRowCount;
                names[rangeRow] = GnssParamNameBuilder.GetDoubleDifferObsCodeNameOf(prn, CurrentBasePrn, Gnsser.ParamNames.L1Code);
                names[phaseRow] = GnssParamNameBuilder.GetDoubleDifferObsCodeNameOf(prn, CurrentBasePrn, Gnsser.ParamNames.L2Code);
                rangeRow++;
            }

            return new List<string>(names);
        }


        /// <summary>
        /// 每个历元的权逆阵，第一颗卫星多出现了一次，要对其降权。
        /// 此处令对角线为2，其余为1。
        /// </summary>
        /// <param name="rowCol">行列数</param>
        /// <returns></returns>
        private ArrayMatrix BuildEpochDoubleQ()
        {
            int rowCol = ObsCount;
            double initVal = 1.0;
            if (IsBaseSatUnstable) { initVal = 1e3; }

            ArrayMatrix identify = new ArrayMatrix(rowCol, rowCol, initVal);
            ArrayMatrix diagonal = ArrayMatrix.Diagonal(rowCol, rowCol, initVal);

            //如果有周跳，则放大方差
            var unstatblePrns = this.CurrentMaterial.UnstablePrns;
            foreach (var prn in unstatblePrns)
            {
                if (prn == CurrentBasePrn) { continue; }
                var index = GetSatIndexExceptBasePrn(prn);
                if (index < 0) { continue; }//有可能已经排除在启用卫星内
                diagonal[index, index] = 1e6;
            }


            //for (int i = 0; i < unstatblePrns.Count; i++)
            //{
            //    var prn = unstatblePrns[i];
            //    if (prn == CurrentBasePrn) { continue; }
            //    var index = GetSatIndexExceptBasePrn(prn);
            //    if (index < 0) { continue; }//有可能已经排除在启用卫星内
            //    diagonal[index, index] = 1e6;
            //}

            return identify + diagonal;
        }


        /// <summary>
        /// 双频载波观测值的双差协方差矩阵
        /// </summary>
        /// <returns></returns>
        protected IMatrix BulidInverseWeightOfObs()
        {

            int satCount = EnabledSatCount;

            //首先,建立非差观测值的权逆阵，这里前一半是rov站的，后一半是ref站的.单频基础上扩展双频
            int row = (satCount) * 2;
            DiagonalMatrix inverseWeight = new DiagonalMatrix(row);
            double invFactorOfPhase = 1e-4;
            int index = 0;
            int baseindex = this.RovInfo.EnabledPrns.IndexOf(CurrentBasePrn);
            EpochSatellite baseRovEle = this.RovInfo.EnabledSats[baseindex];
            EpochSatellite baseRefEle = this.RefInfo.EnabledSats[baseindex];
            double baseRovSat = SatWeightProvider.GetInverseWeightOfRange(baseRovEle); //返回的伪距的方差
            double baseRefSat = SatWeightProvider.GetInverseWeightOfRange(baseRefEle); //返回的伪距的方差
            inverseWeight[index, index] = baseRovSat * invFactorOfPhase;
            inverseWeight[index + satCount, index + satCount] = baseRefSat * invFactorOfPhase;

            for (int i = 0; i < satCount; i++)
            {              
                if (i != baseindex)
                {
                    index++;
                    EpochSatellite e = this.RovInfo.EnabledSats[i];
                    double inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(e); //返回的伪距的方差
                    inverseWeight[index, index] = inverseWeightOfSat * invFactorOfPhase;

                    e = this.RefInfo[i];
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
            foreach (var item in RovInfo.EnabledSats)
            {
                if (item.Prn != CurrentBasePrn)
                {
                    doubleCoeff[satIndex][0] = -1; //参考星
                    int index0 = this.RovInfo.EnabledPrns.IndexOf(item.Prn);
                    doubleCoeff[satIndex][satIndex + 1] = 1; //流动星
                    satIndex++;
                }
            }
            IMatrix doubleCoeffMatrix = new ArrayMatrix(doubleCoeff);
            IMatrix doubleInverseWeigth = doubleCoeffMatrix.Multiply(sigleInverseWeigth).Multiply(doubleCoeffMatrix.Transposition);

            //发现了吗？上面构建的是单频载波双差的权逆阵，而双频的呢？ 
            //可以重新建立（适合认为每个频率不一样的观测噪声），或者是两个独立模块，组合在一个大矩阵里。下面用第二种方式。

            double[][] DDelements = Geo.Utils.MatrixUtil.Create((satCount - 1) * 2, (satCount - 1) * 2, 0f);
            for (int i = 0; i < (satCount - 1); i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    DDelements[i][j] = doubleInverseWeigth[i, j];
                    DDelements[i + (satCount - 1)][j + (satCount - 1)] = doubleInverseWeigth[i, j];
                }
            }
            for (int i = 0; i < (satCount - 1) * 2; i++)
            {
                for (int j = i; j < (satCount - 1) * 2; j++)
                {
                    DDelements[i][j] = DDelements[j][i];
                }
            }
            IMatrix DDInverseWeigth = new Matrix(DDelements);

            return DDInverseWeigth;
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
                //获取当前单频的信号频率
                Frequence FrequenceA = Frequence.GetFrequence(this.CurrentBasePrn, SatObsDataType.PhaseRangeA, this.RefInfo.ReceiverTime);
                Frequence FrequenceB = Frequence.GetFrequence(this.CurrentBasePrn, SatObsDataType.PhaseRangeB, this.RefInfo.ReceiverTime);

                Matrix A = new Matrix(ObsCount, ParamCount); 
                var prns = CurrentMaterial.EnabledPrns;
                int sObsCount = CurrentMaterial.EnabledSatCount - 1;
                int satIndex = 0;
                //基准星
                XYZ baseSatRovVector = RovInfo[CurrentBasePrn].ApproxVector;
                foreach (var prn in prns)//逐个卫星遍历
                {
                    if (prn.Equals(CurrentBasePrn)) { continue; }

                    var sat = RovInfo[prn];

                    int rowIndex = satIndex;
                    XYZ rovVector = sat.ApproxVector;
                    //频率1
                    A[rowIndex, 0] = -(rovVector.CosX - baseSatRovVector.CosX);//负负得正
                    A[rowIndex, 1] = -(rovVector.CosY - baseSatRovVector.CosY);
                    A[rowIndex, 2] = -(rovVector.CosZ - baseSatRovVector.CosZ);

                    //频率2
                    A[rowIndex + sObsCount, 0] = -(rovVector.CosX - baseSatRovVector.CosX);//负负得正
                    A[rowIndex + sObsCount, 1] = -(rovVector.CosY - baseSatRovVector.CosY);
                    A[rowIndex + sObsCount, 2] = -(rovVector.CosZ - baseSatRovVector.CosZ);

                    if (this.BaseParamCount == 4)
                    {
                        double wetMap = RovInfo[prn].WetMap - RovInfo[CurrentBasePrn].WetMap;
                        double refwetMap = RefInfo[prn].WetMap - RefInfo[CurrentBasePrn].WetMap; //参考站

                        A[rowIndex, BaseParamCount - 1] = wetMap - refwetMap;
                    }
                    else if (this.BaseParamCount == 5)
                    {
                        double wetMap = RovInfo[prn].WetMap - RovInfo[CurrentBasePrn].WetMap;
                        double refwetMap = RefInfo[prn].WetMap - RefInfo[CurrentBasePrn].WetMap; //参考站
                        A[rowIndex, BaseParamCount - 2] = wetMap;
                        A[rowIndex, BaseParamCount - 1] = -refwetMap;
                    }
                    //频率1
                    A[rowIndex, this.BaseParamCount + satIndex] = FrequenceA.WaveLength;//  1.0; //          //模糊度互差距离偏差
                    //频率2
                    A[rowIndex + sObsCount, this.BaseParamCount + sObsCount + satIndex] = FrequenceB.WaveLength;//  1.0; //          //模糊度互差距离偏差

                    satIndex++;
                }

                return A;
            }
        }
    }//end class
}
