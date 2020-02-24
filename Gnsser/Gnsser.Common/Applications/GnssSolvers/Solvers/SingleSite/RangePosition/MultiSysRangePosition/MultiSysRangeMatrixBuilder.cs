// 2014.09.26, czs, create, 用于计算无多余数据源，无初始坐标，无多余改正的数据的“三无”数据。
//2017.09.04, lly, edit in zz, 增加多系统定位钟差
//2017.09.05, czs, edit in hongqing, 整理，单独提出类，多系统伪距单点定位，顾及系统时间偏差
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
using Geo.Algorithm;

namespace Gnsser.Service
{
    /// <summary>
    /// 多系统伪距单点定位，顾及系统时间偏差。
    /// </summary>
    public class MultiSysRangeMatrixBuilder : SingleSiteGnssMatrixBuilder
    {
        #region 构造函数
        /// <summary>
        /// 多系统伪距单点定位，顾及系统时间偏差，构造函数。
        /// </summary>
        /// <param name="option">解算选项</param>
        public MultiSysRangeMatrixBuilder(GnssProcessOption option)
            : base(option)
        {
            this.IonosphereBuilderManager = new IonophereBuilderManager();
            this.ParamNameBuilder = new MultiSysRangeParamNameBuilder(option);//option中包含了几个系统
        }

        #endregion

        #region 全局基础属性

        /// <summary>
        /// 电离层延迟计算
        /// </summary>
        public IonophereBuilderManager IonosphereBuilderManager { get; set; }
        /// <summary>
        /// 参数数量
        /// </summary>
        public override int ParamCount { get { return 3 + Option.SatelliteTypes.Count; } }
        /// <summary>
        /// 观测数量
        /// </summary>
        public override int ObsCount { get { return CurrentMaterial.EnabledSatCount; } }
        
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
                #region 1个系统
                if (Option.SatelliteTypes.Count == 1)
                {
                    foreach (var sat in CurrentMaterial.EnabledSats)
                    {
                        XYZ satToStaVector = sat.Ephemeris.XYZ - this.CurrentMaterial.SiteInfo.EstimatedXyz;

                        A[satIndex, 0] = -satToStaVector.CosX;
                        A[satIndex, 1] = -satToStaVector.CosY;
                        A[satIndex, 2] = -satToStaVector.CosZ;
                        A[satIndex, 3] = -1.0;////*** 接收机 钟差改正 in units of meters

                        satIndex++;
                    }
                }
                #endregion
                #region 2个系统
                //默认必须有GPS
                if (Option.SatelliteTypes.Count == 2)
                {
                    foreach (var sat in CurrentMaterial.EnabledSats)
                    {
                        if (sat.Prn.SatelliteType == SatelliteType.G)
                        {
                            XYZ satToStaVector = sat.Ephemeris.XYZ - this.CurrentMaterial.SiteInfo.EstimatedXyz;
                            A[satIndex, 0] = -satToStaVector.CosX;
                            A[satIndex, 1] = -satToStaVector.CosY;
                            A[satIndex, 2] = -satToStaVector.CosZ;
                            A[satIndex, 3] = -1.0;////*** 接收机 钟差改正 in units of meters
                            A[satIndex, 4] = -0.0;////系统时间偏差
                        }
                        if (sat.Prn.SatelliteType == SatelliteType.E)
                        {
                            XYZ satToStaVector = sat.Ephemeris.XYZ - this.CurrentMaterial.SiteInfo.EstimatedXyz;
                            A[satIndex, 0] = -satToStaVector.CosX;
                            A[satIndex, 1] = -satToStaVector.CosY;
                            A[satIndex, 2] = -satToStaVector.CosZ;
                            A[satIndex, 3] = -1.0;////*** 接收机 钟差改正 in units of meters
                            A[satIndex, 4] = -1.0;////系统时间偏差
                        }
                        if (sat.Prn.SatelliteType == SatelliteType.C)
                        {
                            XYZ satToStaVector = sat.Ephemeris.XYZ - this.CurrentMaterial.SiteInfo.EstimatedXyz;
                            A[satIndex, 0] = -satToStaVector.CosX;
                            A[satIndex, 1] = -satToStaVector.CosY;
                            A[satIndex, 2] = -satToStaVector.CosZ;
                            A[satIndex, 3] = -1.0;////*** 接收机 钟差改正 in units of meters
                            A[satIndex, 4] = -1.0;////系统时间偏差
                        }
                        satIndex++;
                    }
                }
                #endregion
                #region 3个系统
                //默认 G E C
                if (Option.SatelliteTypes.Count == 3)
                {
                    foreach (var sat in CurrentMaterial.EnabledSats)
                    {
                        if (sat.Prn.SatelliteType == SatelliteType.G)
                        {
                            XYZ satToStaVector = sat.Ephemeris.XYZ - this.CurrentMaterial.SiteInfo.EstimatedXyz;
                            A[satIndex, 0] = -satToStaVector.CosX;
                            A[satIndex, 1] = -satToStaVector.CosY;
                            A[satIndex, 2] = -satToStaVector.CosZ;
                            A[satIndex, 3] = -1.0;////*** 接收机 钟差改正 in units of meters
                            A[satIndex, 4] = -0.0;////系统时间偏差
                            A[satIndex, 5] = -0.0;////系统时间偏差
                        }
                        if (sat.Prn.SatelliteType == SatelliteType.E)
                        {
                            XYZ satToStaVector = sat.Ephemeris.XYZ - this.CurrentMaterial.SiteInfo.EstimatedXyz;
                            A[satIndex, 0] = -satToStaVector.CosX;
                            A[satIndex, 1] = -satToStaVector.CosY;
                            A[satIndex, 2] = -satToStaVector.CosZ;
                            A[satIndex, 3] = -1.0;////*** 接收机 钟差改正 in units of meters
                            A[satIndex, 4] = -1.0;////系统时间偏差
                            A[satIndex, 5] = -0.0;////系统时间偏差
                        }
                        if (sat.Prn.SatelliteType == SatelliteType.C)
                        {
                            XYZ satToStaVector = sat.Ephemeris.XYZ - this.CurrentMaterial.SiteInfo.EstimatedXyz;
                            A[satIndex, 0] = -satToStaVector.CosX;
                            A[satIndex, 1] = -satToStaVector.CosY;
                            A[satIndex, 2] = -satToStaVector.CosZ;
                            A[satIndex, 3] = -1.0;////*** 接收机 钟差改正 in units of meters
                            A[satIndex, 4] = -0.0;////系统时间偏差
                            A[satIndex, 5] = -1.0;////系统时间偏差
                        }
                        satIndex++;
                    }
                }
                #endregion
                return A;
            }
        }
        #endregion

        #region 参数先验值，先验值设置为 0 ，主要需要其权逆阵

       /// <summary>
        /// 先验参数
        /// </summary> 
        protected override WeightedVector CreateInitAprioriParam()
        {
            double[] residualVec = new double[ParamCount];//近似差为 0 
            double[][] inverseWeight = MatrixUtil.CreateIdentity(ParamCount);

            if (Option.IsIndicatingApproxXyzRms)
            {
                int i = 0;
                for (i = 0; i < 3; i++)
                {
                    var item = Option.InitApproxXyzRms[i];
                    inverseWeight[i][i] = item * item;
                }
            }
            else if (CurrentMaterial.SiteInfo.EstimatedXyz.IsZero)
            {
                for (int i = 0; i < 3; i++) { inverseWeight[i][i] = 1e10; }
            }

            else
            {
                for (int i = 0; i < 3; i++) { inverseWeight[i][i] = 100000; }
            }
            inverseWeight[3][3] = 1000000;//钟差
            for (int i = 4; i < 3 + Option.SatelliteTypes.Count; i++)
            {
                inverseWeight[i][i] = 0.25;//系统时间偏差
            }
            return new WeightedVector(residualVec, inverseWeight);
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
                Vector app = new Vector(this.ObsCount);
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
                var vector = new AdjustVector(obs, app);

                var cova = BulidInverseWeightOfObs();

                return new WeightedVector(vector, cova);


                return new WeightedVector(this.CurrentMaterial.GetAdjustVector(SatObsDataType.IonoFreeRange), DiagonalMatrix.GetIdentity(CurrentMaterial.EnabledSatCount));
            }
        }
        /// <summary>
        /// 观测量的权逆阵，一个对角阵。
        /// </summary> 
        /// <returns></returns>
        protected IMatrix BulidInverseWeightOfObs()
        {
            double[][] inverseWeight = Geo.Utils.MatrixUtil.Create(EnabledSatCount);

            double invFactorOfRange = 10;//URE

            int i = 0;
            foreach (var prn in CurrentMaterial.EnabledPrns)// 一颗卫星1行 
            {
                EpochSatellite e = this.CurrentMaterial[prn];
                double inverseWeightOfSat = 1;
                if (e.SiteInfo.ApproxXyz.IsZero)//高度角有关，如果测站坐标为 0，则高度角计算错误。
                {
                    inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(e);
                } 

                inverseWeight[i][i] = inverseWeightOfSat * invFactorOfRange;

                i++;
            }
            return new Matrix(inverseWeight);
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
                    if (obsObj != null && obsObj.Value != 0)
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
        #endregion

    }
}