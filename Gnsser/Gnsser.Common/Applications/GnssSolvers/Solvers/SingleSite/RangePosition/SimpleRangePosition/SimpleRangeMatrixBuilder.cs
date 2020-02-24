//2014.09.26, czs, create, 用于计算无多余数据源，无初始坐标，无多余改正的数据的“三无”数据。
//2017.09.05, czs, edit in hongqing,最简单的4参数伪距定位，不考虑伪距平滑，系统时间偏差等。
//2018.05.29, czs, eit in HMX, 移除载波平滑伪距算法，统一放到改正数里面

using System;
using System.Collections.Generic;
using Gnsser.Domain;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Checkers;

namespace Gnsser.Service
{
    /// <summary>
    /// 最简单的4参数伪距单点定位的矩阵生成器。
    /// 本类主要保证能出一个定位结果（即使数据不是很理想的情况下，如非双频）。
    /// 不考虑伪距平滑，系统时间偏差等。
    /// </summary>
    public class SimpleRangeMatrixBuilder : SingleSiteGnssMatrixBuilder
    {
        #region 构造函数
        /// <summary>
        /// 伪距单点定位矩阵生成器，构造函数。
        /// </summary>
        /// <param name="option">解算选项</param>
        public SimpleRangeMatrixBuilder(GnssProcessOption option)
            : base(option)
        {
            this.SatWeightProvider = new SatElevateAndRangeWeightProvider(); 
        }

        #endregion
        #region 全局基础属性 
        /// <summary>
        /// 参数数量
        /// </summary>
        public override int ParamCount { get { return 4; } }
        /// <summary>
        /// 观测数量
        /// </summary>
        public override int ObsCount { get { return CurrentMaterial.EnabledSatCount; } }
        /// <summary>
        /// 参数列表
        /// </summary>
        public override List<string> ParamNames { get { return Gnsser.ParamNames.DxyzClk; } }
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        public override List<string> BuildParamNames() { return ParamNames; } 
        #endregion


        #region 通用矩阵，误差方程系数阵，设计矩阵。

        /// <summary>
        /// 误差方程系数阵，设计矩阵。
        /// 有多少颗卫星就有多少个观测量，只有4个参数，X,Y,Z和接收机钟差等效距离偏差。
        /// </summary> 
        public override Matrix Coefficient
        {
            get
            {
                Matrix A = new Matrix(ObsCount, ParamCount);
                int satIndex = 0;
                foreach (var sat in CurrentMaterial.EnabledSats)
                {
                    XYZ vector = sat.EstmatedVector; 

                    A[satIndex, 0] = -vector.CosX;
                    A[satIndex, 1] = -vector.CosY;
                    A[satIndex, 2] = -vector.CosZ;
                    A[satIndex, 3] = 1.0;////*** 接收机 钟差 in units of meters
                    satIndex++;
                }
                return A;
            }
        }
        #endregion

        
        #region 观测值，观测值残差

        /// <summary>
        /// 残差
        /// </summary>
        public override WeightedVector Observation
        {
            get
            {
                Vector obs = new Vector(this.ObsCount);
                int i = 0;
                foreach (var sat in this.CurrentMaterial.EnabledSats)
                {
                    var obsObj = sat.GetDataValue(Option.ObsDataType);
                    if (obsObj != null && obsObj.Value != 0)
                    {
                        obs[i] = obsObj.CorrectedValue;
                    }
                    else//最低定位条件，确保获取一个定位结果。
                    {
                        obs[i] = sat.AvailablePseudoRange.CorrectedValue; //近似值 
                    }
                    obs.ParamNames[i] = sat.Prn + "";
                     
                    i++;
                } 
                var cova = BulidInverseWeightOfObs();
                return new WeightedVector(obs, cova);
            }
        }
        /// <summary>
        ///  自由项D，B0等等。则参数平差中，满足满足 l = L - (AX0 + D).
        ///  此处，FreeVector = (AX0 + D)。
        /// </summary>
        public override Vector FreeVector
        {
            get
            {
                Vector app = new Vector(this.ObsCount);
                int i = 0;
                foreach (var sat in this.CurrentMaterial.EnabledSats)
                {
                    var obsObj = sat.GetDataValue(Option.ApproxDataType);
                    if(obsObj != null && obsObj.Value != 0)
                    {
                        app[i] = obsObj.CorrectedValue; 
                    }
                    else//最低定位条件，确保获取一个定位结果。
                    {                  
                        app[i] = sat.AvailableApproxPseudoRange.CorrectedValue; //近似值 
                    }
                    app.ParamNames[i] = sat.Prn + "";

                    i++;
                } 
                return app;
            }
        }

        /// <summary>
        /// 观测量的权逆阵，一个对角阵。
        /// </summary> 
        /// <returns></returns>
        protected IMatrix BulidInverseWeightOfObs()
        {
            double[][] inverseWeight = Geo.Utils.MatrixUtil.Create(EnabledSatCount);
            bool hasEstCoord = !XYZ.IsZeroOrEmpty(CurrentMaterial.SiteInfo.ApproxXyz);
            int i = 0;
            foreach (var sat in CurrentMaterial.EnabledSats)// 一颗卫星1行 
            { 
                double inverseWeightOfSat = 1;
                if (hasEstCoord)//高度角有关，如果测站坐标为 0，则高度角计算错误。
                {
                    inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(sat);
                } 

                inverseWeight[i][i] = inverseWeightOfSat + (sat.StdDevOfRange * sat.StdDevOfRange);

                i++;
            }
            return new Matrix(inverseWeight);
        }
        #endregion

    }
}