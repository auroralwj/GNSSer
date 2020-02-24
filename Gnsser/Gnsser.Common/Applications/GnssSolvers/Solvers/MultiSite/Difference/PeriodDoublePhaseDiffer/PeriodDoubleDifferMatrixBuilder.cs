//2014.09.04, czs, edit, 基本理清思路
//2014.12.11，czs, edit in jinxinliangmao shuangliao, 差分定位矩阵生成器
//2014.12.13，czs, edit in namu shuangliao, 差分定位矩阵生成器,双差
//2016.04.30, czs, edit in hongqing, 重构
//2018.07.31, czs, edit in hmx, 名称前冠名Period

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Data.Rinex;
using Gnsser.Checkers;
using Geo.Utils;
using Geo.Coordinates;
using Gnsser.Models;
using Geo.Algorithm.Adjust;

namespace Gnsser.Service
{
    /// <summary>
    /// 双差矩阵生成器。适用于参数平差、卡尔曼滤波等。
    /// </summary>
    public class PeriodDoubleDifferMatrixBuilder :  AbstractMultiSitePeriodMatrixBuilder 
    {
        #region 构造函数
        /// <summary>
        /// 载波相位双差定位矩阵生成器，构造函数。
        /// </summary>  
        /// <param name="model">解算选项</param> 
        public PeriodDoubleDifferMatrixBuilder(
            GnssProcessOption model)
            : base(model)
        { 
            this.ParamNameBuilder = new PeriodDoubleDifferParamNameBuilder(model);
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

                //相位模糊度系数，可以为米和周，按照设置文件决定。
                double coeefOfPhase = CheckAndGetCoeefOfPhase(); 

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
                        A[rowIndex, 3 + satIndex] = coeefOfPhase;            //模糊度互差距离偏差
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

                var result = new WeightedVector(obsMinusApp, BulidInverseWeightOfObs());
                result.ParamNames = this.GetObsNames();

                return result;
            }
        }

        /// <summary>
        /// 观测名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetObsNames()
        {
            var names = new string[ObsCount]; 
            int siteCount = CurrentMaterial.Count;
            var epochCount = this.EpochCount;
            int rowIndex = 0;
            for (int epochIndex = 0; epochIndex < epochCount; epochIndex++)
            { 
                foreach (var prn in this.EnabledPrns)
                {
                    if (prn == CurrentBasePrn) { continue; }
                      
                    names[rowIndex] = GnssParamNameBuilder.GetDoubleDifferObsCodeNameOfPhase(prn, CurrentBasePrn, epochIndex);
                    rowIndex++;
                }
            }
            return new List<string>(names);
        }
        
        /// <summary>
        /// 观测量的权逆阵，双差的第一颗卫星相关，需要降权
        /// </summary> 
        /// <returns></returns>
        private IMatrix BulidInverseWeightOfObs()
        {

            this.BaseSiteName = this.CurrentMaterial[0].BaseSiteName;
            ObsCovaMatrixBuilder obsCovaMatrixBuilder = new ObsCovaMatrixBuilder(this.SatWeightProvider);
            return obsCovaMatrixBuilder.BulidDoubleInverseWeightOfObs(CurrentMaterial, BaseSiteName, CurrentBasePrn, Option.PhaseCovaProportionToRange, true, false);



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
