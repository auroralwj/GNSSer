//2017.08.28, czs & kyc, create in hongqing, 多站多历元GNSS计算预留测试类

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Data.Rinex;
using Gnsser.Checkers;
using Geo.Utils;
using Geo.Algorithm;
using Geo.Coordinates;
using Gnsser.Models;
using Geo.Algorithm.Adjust;

namespace Gnsser.Service
{
    /// <summary>
    /// 多站多历元GNSS计算预留测试类。适用于参数平差、卡尔曼滤波等。
    /// </summary>
    public class PeriodMultiSiteGnssExtentMatrixBuilder :  AbstractMultiSitePeriodMatrixBuilder 
    {
        #region 构造函数
        /// <summary>
        /// 载波相位双差定位矩阵生成器，构造函数。
        /// </summary>  
        /// <param name="model">解算选项</param> 
        public PeriodMultiSiteGnssExtentMatrixBuilder(
            GnssProcessOption model)
            : base(model)
        {
            this.ParamNameBuilder = new PeriodMultiSiteGnssExtentParamNameBuilder(this.Option);
        }
        #endregion
         
        #region 全局基础属性 
        /// <summary>
        /// 每个历元的方程数量
        /// </summary>
        public int EpochObsCount { get { return this.EnabledSatCount - 1; } }

        /// <summary>
        /// 参数数量
        /// </summary>
        public override int ParamCount { get { return 2 + EnabledSatCount; } }

        /// <summary>
        /// 方程数量
        /// </summary>
        public override int ObsCount { get { return (EnabledSatCount - 1) * EpochCount; } } 
        #endregion 

        /// <summary>
        /// 误差方程系数阵，设计矩阵。
        /// </summary> 
        public override Matrix Coefficient
        {
            get
            {
                Matrix A = new Matrix(ObsCount, ParamCount);
                int epochIndex = 0;
                var prns = CurrentMaterial.EnabledPrns;
                foreach (var epoch in CurrentMaterial) //逐个历元遍历
                {
                    int satIndex = 0;
                    //基准星
                    XYZ baseSatRovVector = epoch.OtherEpochInfo[CurrentBasePrn].ApproxVector;
                    foreach (var prn in prns)//逐个卫星遍历
                    {
                        if (prn.Equals(CurrentBasePrn)) { continue; }

                        var sat = epoch.OtherEpochInfo[prn];

                        int rowIndex = epochIndex * EpochObsCount + satIndex;
                        XYZ rovVector = sat.ApproxVector;

                        A[rowIndex, 0] = -(rovVector.CosX - baseSatRovVector.CosX);//负负得正
                        A[rowIndex, 1] = -(rovVector.CosY - baseSatRovVector.CosY);
                        A[rowIndex, 2] = -(rovVector.CosZ - baseSatRovVector.CosZ);
                        A[rowIndex, 3 + satIndex] = 1.0;            //模糊度互差距离偏差
                        satIndex++; 
                    }
                    epochIndex++;
                }

                return A;
            }
        }         

        #region 观测值，观测值残差

        /// <summary>
        /// 平差的常数项，通常是观测值减去近似值
        /// </summary> 
        public override WeightedVector Observation
        {
            get
            {
               // AdjustVector obsMinusApp = PeriodDifferInfo.GetDoubleDifferAdjustVector(this.PositionOption.ObsDataType, this.PositionOption.ApproxDataType, BasePrn);
                 Vector obsMinusApp = CurrentMaterial.GetDoubleDifferResidualVector(this.Option.ObsDataType, this.Option.ApproxDataType, CurrentBasePrn);

                return new WeightedVector(obsMinusApp, BulidInverseWeightOfObs());
            }
        }
        /// <summary>
        /// 观测量的权逆阵，双差的第一颗卫星相关，需要降权
        /// </summary> 
        /// <returns></returns>
        private IMatrix BulidInverseWeightOfObs()
        {
            ArrayMatrix q = new ArrayMatrix(ObsCount, ObsCount);
            for (int i = 0; i < EpochCount; i++)
            {
                ArrayMatrix sub = BuildEpochDoubleQ(EpochObsCount);
                int fromRowCol = i * EpochObsCount;
                q.Set(sub, fromRowCol, fromRowCol); ;
            }

            return q;
        }
        /// <summary>
        /// 每个历元的权逆阵，第一颗卫星多出现了一次，要对其降权。
        /// 此处令对角线为2，其余为1。
        /// </summary>
        /// <param name="rowCol">行列数</param>
        /// <returns></returns>
        private ArrayMatrix BuildEpochDoubleQ(int rowCol)
        {
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
    }
}
