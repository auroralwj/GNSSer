// 2014.09.12, czs, create, 卫星定权。

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Geo.Algorithm.Adjust;
using Gnsser.Service;

namespace Gnsser
{
    /// <summary>
    /// 卫星高度角定权
    /// </summary>
    public class SatElevateWeightProvider : ISatWeightProvider
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Option"></param>
         public SatElevateWeightProvider(GnssProcessOption Option)
        {
            this.SystemStdDevFactors = Option.SystemStdDevFactors;
            this.SatelliteStdDevs = Option.SatelliteStdDevs;
            this.IsSpeedTopModel = Option.TopSpeedModel;
        } 

        #region 基本属性
        public bool IsSpeedTopModel { get; set; }
        /// <summary>
        /// 不同系统的权值
        /// </summary>
        protected Dictionary<SatelliteType, double> SystemStdDevFactors { get; set; }
        /// <summary>
        /// 不同卫星的权值
        /// </summary>
        protected Dictionary<SatelliteNumber, double> SatelliteStdDevs { get; set; }
        /// <summary>
        /// 经验值，用于改进高度角模型对低仰角卫星精度较差的不足
        /// 单位：mm
        /// </summary>
        public double a = 4;

        /// <summary>
        /// 经验值，用于改进高度角模型对低仰角卫星精度较差的不足
        /// 单位：mm
        /// </summary>
        public double b = 3;
        #endregion

        /// <summary>
        /// 观测值精度
        /// </summary>
        /// <param name="satellite"></param>
        /// <returns></returns>
        public double GetInverseWeightOfRange(EpochSatellite satellite)
        {
            var prn = satellite.Prn;
        //    double sinE = 0.5 + 0.5 * Math.Sin(satellite.GeoElevation * AngularConvert.DegToRadMultiplier);
            double sinE = 0.01 + 0.99 * Math.Sin(satellite.GeoElevation * AngularConvert.DegToRadMultiplier);

            //防止测站坐标为 0 的情况
            if (!Geo.Utils.DoubleUtil.IsValid(sinE)) { sinE =1.0; }

            if (IsSpeedTopModel) { return sinE; }


            //不同系统的权值
            double stdDevFactorOfSystem = 1;// 默认 1m 精度
            if ( this.SystemStdDevFactors.ContainsKey(prn.SatelliteType))
            {
                stdDevFactorOfSystem = this.SystemStdDevFactors[prn.SatelliteType]; 
            }

            //不同卫星的权值
            double stdDevFactorOfSat = 1;// 默认 1m 精度
            if (this.SatelliteStdDevs.ContainsKey(prn))
            {
                stdDevFactorOfSat = this.SatelliteStdDevs[prn];
            }
            else
            {
                GnssSystem sys = GnssSystem.GetGnssSystem(satellite.Prn.SatelliteType);
                double factor = 1.0 * satellite.AvailablePseudoRange.Value / sys.ApproxRadius; //如果是静止轨道卫星，此值偏大
                stdDevFactorOfSat = factor;
            }

            double stdDev = stdDevFactorOfSat * stdDevFactorOfSystem / sinE;//标准差
            double inverseWeight = stdDev * stdDev;//方差

            return inverseWeight;
        }
    }
}