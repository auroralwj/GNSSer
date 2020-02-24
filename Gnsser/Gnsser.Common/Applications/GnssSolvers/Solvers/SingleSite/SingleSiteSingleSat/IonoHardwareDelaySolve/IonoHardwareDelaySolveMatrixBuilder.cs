//2018.05.15, czs, create in hmx, 电离层硬件延迟计算

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Times;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Data.Rinex;
using Gnsser.Checkers;
using Gnsser.Models;
using Geo.Times;
using Gnsser.Data;
using Geo;

namespace Gnsser.Service
{
    /// <summary>
    /// 电离层硬件延迟计算
    /// </summary>
    public class IonoHardwareDelaySolveMatrixBuilder : SingleSiteSingleSatGnssMatrixBuilder
    {
        /// <summary>
        /// 先构造，再设置历元值。
        /// </summary>
        /// <param name="option">解算选项</param> 
        public IonoHardwareDelaySolveMatrixBuilder(
            GnssProcessOption option)
            : base(option)
        {
            ParamNameBuilder = new IonoHardwareDelaySolveParamNameBuilder(option);      
        }

        #region 全局基础属性 
        /// <summary>
        /// 选定的卫星
        /// </summary>
        public SatelliteNumber Prn { get; set; }
        /// <summary>
        /// 参数数量
        /// </summary>
        public override int ParamCount { get { return 4; } }


        /// <summary>
        /// 参数列表
        /// </summary>
        public override List<string> ParamNames { get { return ParamNameBuilder.Build(); } }
        #endregion

        public override void Build()
        {
            if(Prn  != this.CurrentMaterial.Prn)
            {
                log.Info("当前卫星已改变，由 " + Prn + " 变成 " + this.CurrentMaterial.Prn);
                this.Prn = this.CurrentMaterial.Prn;
            }
            base.Build();
        }

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
        /// 观测值。
        /// 自由项 l，观测值减去先验值或估计值。
        /// 常数项，观测误差方程的常数项,或称自由项
        /// </summary>
        public virtual IVector ObservationVector
        {
            get
            {
                 var m = this.CurrentMaterial;

                double P1 = m.FrequenceA.PseudoRange.CorrectedValue;
                double P2 = m.FrequenceB.PseudoRange.CorrectedValue;
                double L1 = m.FrequenceA.PhaseRange.CorrectedValue;
                double L2 = m.FrequenceB.PhaseRange.CorrectedValue;

                double dL = (L1 - L2);
                double dP = (P2 - P1);

                double B = dL - dP;
                double l = dL + B;
                Vector L = new Vector(1);

                L[0] = l; 

                return L;
            }
        }

        /// <summary>
        /// 观测量的权逆阵，一个对角阵。按照先伪距，后载波的顺序排列。
        /// </summary> 
        /// <returns></returns>
        protected IMatrix BulidInverseWeightOfObs()
        {
            DiagonalMatrix inverseWeight = new DiagonalMatrix(1, 1);
            inverseWeight[0,0] = SatWeightProvider.GetInverseWeightOfRange( this.CurrentMaterial);
            return (inverseWeight);
        }

        #endregion

        #region 公共矩阵生成

        /// <summary>
        /// 参数平差系数阵 A。前一半行数为伪距观测量，后一半为载波观测量。
        /// </summary> 
        /// <returns></returns>
        public override Matrix Coefficient
        {
            get
            {
                double f1f1 = Frequence.GpsL1.Value * Frequence.GpsL1.Value * 1e-6;
                double f2f2 = Frequence.GpsL2.Value * Frequence.GpsL2.Value * 1e-6;
                double a =  40.28 * (f1f1 - f2f2) / (f1f1 * f2f2); 

                 //夹角
               double beta= GridIonoFileService.GetSatZenithAngleOfPuncturePointInSphereRad(CurrentMaterial.SphereElevation, GlobalIgsGridIonoService.Instance.HeightOfModel);


                var satSunFixXyz = CoordTransformer.EarthXyzToSunFixedXyz(CurrentMaterial.Ephemeris.XYZ, CurrentMaterial.ReceiverTime.DateTime);
                var satPolar = Geo.Coordinates.CoordTransformer.XyzToPolar(satSunFixXyz, AngleUnit.Radian);
                var siteSunFixXyz = CoordTransformer.EarthXyzToSunFixedXyz(this.CurrentMaterial.SiteInfo.ApproxXyz, CurrentMaterial.ReceiverTime.DateTime);
                var sitePolar = Geo.Coordinates.CoordTransformer.XyzToPolar(siteSunFixXyz, AngleUnit.Radian);

                double differLon = sitePolar.Azimuth - satPolar.Azimuth;
                double differLat = sitePolar.Elevation - satPolar.Elevation;

                double aSecBeta = a * 1.0 / Math.Cos(beta);
                int rowCount = 1;
                int colCount = 4;
                Matrix A = new Matrix(rowCount, colCount);
                A[0, 0] = aSecBeta;
                A[0, 1] = aSecBeta * differLon;
                A[0, 2] = aSecBeta * differLat;
                A[0, 3] = 1.0;

                return (A);
            }
        }
         
        /// <summary>
        /// 只有四个参数且不变。
        /// </summary>
        public override bool IsParamsChanged { get { return false; } }

        /// <summary>
        /// 状态转移矩阵
        /// </summary>
        public override WeightedMatrix Transfer {
            get
            {
                double differLon = 0;
                double differLat = 0;
                if (PreviousProduct != null)
                {
                    var sunFixXyz = CoordTransformer.EarthXyzToSunFixedXyz(PreviousProduct.Material.Ephemeris.XYZ, PreviousProduct.Material.ReceiverTime.DateTime );

                    var lastPolar = Geo.Coordinates.CoordTransformer.XyzToPolar(sunFixXyz, AngleUnit.Radian);

                    var newsunFixXyz = CoordTransformer.EarthXyzToSunFixedXyz(CurrentMaterial.Ephemeris.XYZ, CurrentMaterial.ReceiverTime.DateTime);

                    var polar = Geo.Coordinates.CoordTransformer.XyzToPolar(newsunFixXyz, AngleUnit.Radian);
                    differLon = polar.Azimuth - lastPolar.Azimuth;
                    differLat = polar.Elevation - lastPolar.Elevation;
                }

                Matrix matrix = Matrix.CreateIdentity(ParamCount);
                matrix[0, 1] = differLon;
                matrix[0, 2] = differLat;

                Matrix moise = Matrix.CreateIdentity(ParamCount) * 100;
                moise[0, 0] = 10;
                moise[3, 3] = 0.01;//硬件延迟稳定
                WeightedMatrix weightedMatrix = new WeightedMatrix(matrix, moise);

                return weightedMatrix;
            }
            protected set => base.Transfer = value;
        }


        #endregion

    }//end class
}