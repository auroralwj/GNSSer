//2014.09.04, czs, edit, 基本理清思路
//2014.12.11， czs, edit in jinxinliangmao shuangliao, 差分定位矩阵生成器

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
using Geo.Algorithm.Adjust;
using Gnsser.Models;

namespace Gnsser.Service
{ 
    /// <summary>
    /// 载波相位单差定位的矩阵生成器。适用于参数平差、卡尔曼滤波等。
    /// </summary>
    public class SingleDifferNoRelevantMatrixBuilder : AbstractMultiSitePeriodMatrixBuilder
    {
        #region 构造函数
        /// <summary>
        /// 载波相位单差定位的矩阵生成器，构造函数。
        /// </summary> 
        /// <param name="EpochDifferInfo">历元差分信息，传入的数据就表明可以使用了，不再做任何检查<</param>
        /// <param name="model">解算选项</param>
        /// <param name="lastResult">上一次观测记录</param>
        public SingleDifferNoRelevantMatrixBuilder( 
            GnssProcessOption model)
            : base( model)
        {
            this.ParamNameBuilder = new SingleDifferNoRelevantParamNameBuilder(Option);
        } 

        #endregion 
         
        /// <summary>
        /// 参数数量
        /// </summary>
        public override int ParamCount { get { return 3 + EnabledSatCount + EpochCount - 1; } }
        /// <summary>
        /// 方程数量
        /// </summary>
        public override int ObsCount { get { return EnabledSatCount * EpochCount; } }
          

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

                foreach (var epoch in CurrentMaterial)
                {
                    int satIndex = 0;
                    XYZ baseVector = epoch.OtherEpochInfo[this.CurrentBasePrn].ApproxVector;
                    int rowIndex = GetRowIndex(epochIndex, satIndex);
                    A[rowIndex, 0] = -baseVector.CosX;
                    A[rowIndex, 1] = -baseVector.CosY;
                    A[rowIndex, 2] = -baseVector.CosZ; //第无相关变换后，第一历元为 0.
                    if (epochIndex != 0) { A[rowIndex, 3 + epochIndex - 1] = -1.0; }   //*** 接收机钟差等效距离偏差 in units of meters

                    A[rowIndex, 3 + EpochCount + satIndex - 1] = 1.0; // 模糊度互差距离偏差
                    
                    satIndex++;

                    foreach (var prn in prns)
                    {
                        var sat = epoch.OtherEpochInfo[prn];
                        if (prn.Equals(CurrentBasePrn)) { continue; }

                        rowIndex = GetRowIndex(epochIndex, satIndex);
                        XYZ satToRev = sat.ApproxVector;

                        A[rowIndex, 0] = -satToRev.CosX;
                        A[rowIndex, 1] = -satToRev.CosY;
                        A[rowIndex, 2] = -satToRev.CosZ;
                        //第无相关变换后，第一历元为 0.
                        if (epochIndex != 0) { A[rowIndex, 3 + epochIndex - 1] = -1.0; }   //*** 接收机钟差等效距离偏差 in units of meters
                       
                        A[rowIndex, 3 + EpochCount + satIndex - 1] = 1.0; // 模糊度互差距离偏差
                        satIndex++;
                    }
                    epochIndex++;
                }

             //   System.IO.File.WriteAllText("C:\\GnsserOutput\\MatrixA.txt", MatrixUtil.GetFormatedText(A.Array));
                return A;
            }
        }
        /// <summary>
        /// 获取行编号
        /// </summary>
        /// <param name="epochIndex"></param>
        /// <param name="satIndex"></param>
        /// <returns></returns>
        private int GetRowIndex(int epochIndex, int satIndex)
        {
            int rowIndex = epochIndex * SatCount + satIndex;
            return rowIndex;
        }


        public override void Build()
        {
            base.Build(); 
        }

        #region 观测值，观测值残差

        /// <summary>
        /// 平差的常数项，通常是观测值减去近似值
        /// </summary> 
        public override WeightedVector Observation
        {
            get
            {
                Vector obsMinusApp = CurrentMaterial.GetSingleDifferResidualVector(this.Option.ObsDataType, this.Option.ApproxDataType,  this.CurrentBasePrn);
      
                return new WeightedVector(obsMinusApp, BulidInverseWeightOfObs());
            }
        }
        /// <summary>
        /// 观测量的权逆阵，一个对角阵。
        /// </summary> 
        /// <returns></returns>
        private IMatrix BulidInverseWeightOfObs()
        { 
            int row = this.ObsCount;
            DiagonalMatrix inverseWeight = DiagonalMatrix.GetIdentity(row); 

            return inverseWeight.Multiply(0.1 * 0.1);
        }

        #endregion

        public override WeightedMatrix Transfer
        {
            get
            {
                var nameBuilder = (SingleDifferNoRelevantParamNameBuilder)ParamNameBuilder;

                //设置参数状态转移模型
                foreach (var item in this.CurrentMaterial.EnabledPrns)
                {
                    var key = nameBuilder.GetSingleDifferSatAmbiParamName(item);
                    if (!ParamStateTransferModelManager.Contains(key))
                    {
                        ParamStateTransferModelManager.Add(key, new SingleSatPhaseAmbiguityModel(item, Option.StdDevOfPhaseModel, Option.StdDevOfCycledPhaseModel));
                    }
                }
                foreach (var item in this.CurrentMaterial.Epoches)
                {
                    var key = nameBuilder.GetSingleDifferTimedClockParamName(item);
                    if (!ParamStateTransferModelManager.Contains(key))
                    {
                        ParamStateTransferModelManager.Add(key, new WhiteNoiseModel(Option.StdDevOfRevClockWhiteNoiseModel));
                    }
                } 
                return base.Transfer;
            }
        } 

    }
}
