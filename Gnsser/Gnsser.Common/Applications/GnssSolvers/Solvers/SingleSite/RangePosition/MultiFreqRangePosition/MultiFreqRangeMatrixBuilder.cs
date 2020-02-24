//2018.08.12, czs, create in hmx, 多频率伪距定位


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
    /// 多系统伪距单点定位，顾及系统时间偏差。
    /// </summary>
    public class MultiFreqRangeMatrixBuilder : SingleSiteGnssMatrixBuilder
    {
        #region 构造函数
        /// <summary>
        /// 多系统伪距单点定位，顾及系统时间偏差，构造函数。
        /// </summary>
        /// <param name="option">解算选项</param>
        public MultiFreqRangeMatrixBuilder(GnssProcessOption option)
            : base(option)
        {
            this.IsEstIonoParamOfL1 = false; 
            this.ParamNameBuilder = new MultiFreqRangeParamNameBuilder(option,   IsEstIonoParamOfL1);//option中包含了几个系统
        }

        #endregion

        #region 全局基础属性
        /// <summary>
        /// 是否估计 L1 的电离层参数
        /// </summary>
        public bool IsEstIonoParamOfL1 { get; set; } 
        /// <summary>
        /// 观测数量
        /// </summary>
        public override int ObsCount { get { return CurrentMaterial.EnabledSatCount * 2; } }
        
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
                int row = 0;
                int satCount = this.EnabledSatCount; 
                #region 1个系统
                if (Option.SatelliteTypes.Count == 1)
                {
                    foreach (var sat in CurrentMaterial.EnabledSats)
                    {
                        XYZ vector = sat.EstmatedVector;

                        int colIndex = 0;
                        int nextRow = satCount + row;

                        if (!IsFixingCoord)
                        {
                            A[row, 0] = -vector.CosX;
                            A[row, 1] = -vector.CosY;
                            A[row, 2] = -vector.CosZ;

                            A[nextRow, 0] = A[row, 0];
                            A[nextRow, 1] = A[row, 1];
                            A[nextRow, 2] = A[row, 2];

                            colIndex = 3;
                        }

                        A[row, colIndex] = 1.0;////*** 接收机 钟差改正 in units of meters
                        A[nextRow, colIndex] = A[row, colIndex];////*** 接收机 钟差改正 in units of meters

                        colIndex++;
                        A[nextRow, colIndex] = -1.0;//DCB 


                        colIndex++;
                        if (IsEstIonoParamOfL1)
                        {
                            double IonoCoeOfP2 = GetIonoCoeOfP2(sat.Prn.SatelliteType);

                            int ionoColIndex = colIndex + row;

                            A[row, ionoColIndex] = 1;// P1的电离层参数的系数
                            A[nextRow, ionoColIndex] = IonoCoeOfP2;  //P2的电离层参数的系数
                        } 

                        row++;
                    }
                }
                #endregion
                //------注意：尚未实现多系统！！2018.08.12， czs， hmx

                #region 2个系统
                //默认必须有GPS
                if (Option.SatelliteTypes.Count == 2)
                {
                    foreach (var sat in CurrentMaterial.EnabledSats)
                    {
                        if (sat.Prn.SatelliteType == SatelliteType.G)
                        {
                            XYZ satToStaVector = sat.Ephemeris.XYZ - this.CurrentMaterial.SiteInfo.EstimatedXyz;
                            A[row, 0] = -satToStaVector.CosX;
                            A[row, 1] = -satToStaVector.CosY;
                            A[row, 2] = -satToStaVector.CosZ;
                            A[row, 3] = 1.0;////*** 接收机 钟差改正 in units of meters
                            A[row, 4] = -0.0;////系统时间偏差
                        }
                        if (sat.Prn.SatelliteType == SatelliteType.E)
                        {
                            XYZ satToStaVector = sat.Ephemeris.XYZ - this.CurrentMaterial.SiteInfo.EstimatedXyz;
                            A[row, 0] = -satToStaVector.CosX;
                            A[row, 1] = -satToStaVector.CosY;
                            A[row, 2] = -satToStaVector.CosZ;
                            A[row, 3] = 1.0;////*** 接收机 钟差改正 in units of meters
                            A[row, 4] = -1.0;////系统时间偏差
                        }
                        if (sat.Prn.SatelliteType == SatelliteType.C)
                        {
                            XYZ satToStaVector = sat.Ephemeris.XYZ - this.CurrentMaterial.SiteInfo.EstimatedXyz;
                            A[row, 0] = -satToStaVector.CosX;
                            A[row, 1] = -satToStaVector.CosY;
                            A[row, 2] = -satToStaVector.CosZ;
                            A[row, 3] = 1.0;////*** 接收机 钟差改正 in units of meters
                            A[row, 4] = -1.0;////系统时间偏差
                        }
                        row++;
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
                            A[row, 0] = -satToStaVector.CosX;
                            A[row, 1] = -satToStaVector.CosY;
                            A[row, 2] = -satToStaVector.CosZ;
                            A[row, 3] = 1.0;////*** 接收机 钟差改正 in units of meters
                            A[row, 4] = -0.0;////系统时间偏差
                            A[row, 5] = -0.0;////系统时间偏差
                        }
                        if (sat.Prn.SatelliteType == SatelliteType.E)
                        {
                            XYZ satToStaVector = sat.Ephemeris.XYZ - this.CurrentMaterial.SiteInfo.EstimatedXyz;
                            A[row, 0] = -satToStaVector.CosX;
                            A[row, 1] = -satToStaVector.CosY;
                            A[row, 2] = -satToStaVector.CosZ;
                            A[row, 3] = 1.0;////*** 接收机 钟差改正 in units of meters
                            A[row, 4] = -1.0;////系统时间偏差
                            A[row, 5] = -0.0;////系统时间偏差
                        }
                        if (sat.Prn.SatelliteType == SatelliteType.C)
                        {
                            XYZ satToStaVector = sat.Ephemeris.XYZ - this.CurrentMaterial.SiteInfo.EstimatedXyz;
                            A[row, 0] = -satToStaVector.CosX;
                            A[row, 1] = -satToStaVector.CosY;
                            A[row, 2] = -satToStaVector.CosZ;
                            A[row, 3] = 1.0;////*** 接收机 钟差改正 in units of meters
                            A[row, 4] = -0.0;////系统时间偏差
                            A[row, 5] = -1.0;////系统时间偏差
                        }
                        row++;
                    }
                }
                #endregion
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
                    obs[i] = sat.FrequenceA.PseudoRange.CorrectedValue;
                    obs[i+1] = sat.FrequenceB.PseudoRange.CorrectedValue;


                    obs.ParamNames[i] = sat.Prn + "OfP1";
                    obs.ParamNames[i+1] = sat.Prn + "OfP2";

                    i = i+2;
                } 
                var cova = BulidInverseWeightOfObs();

                return new WeightedVector(obs, cova);
            }
        }
        /// <summary>
        /// 观测量的权逆阵，一个对角阵。
        /// </summary> 
        /// <returns></returns>
        protected IMatrix BulidInverseWeightOfObs()
        {
            double[][] inverseWeight = Geo.Utils.MatrixUtil.Create(this.ObsCount);

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
                inverseWeight[i+1][i+1] = inverseWeightOfSat * invFactorOfRange;

                i +=2;
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
                    app[i] = sat.GetApproxRange(SatObsDataType.PseudoRangeA);
                    app[i+1] = sat.GetApproxRange(SatObsDataType.PseudoRangeB);

                    app.ParamNames[i] = sat.Prn + "OfP1";
                    app.ParamNames[i+1] = sat.Prn + "OfP2";

                    i += 2;
                }
                return app;
            }
        }
        #endregion


        public override WeightedMatrix Transfer => base.Transfer;
    }
}