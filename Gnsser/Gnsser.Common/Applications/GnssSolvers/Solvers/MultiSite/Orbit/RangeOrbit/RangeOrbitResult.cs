//2018.10.27, czs, create in hmx, 简易伪距轨道确定


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gnsser.Domain;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Gnsser.Data.Sinex;
using Geo.Algorithm.Adjust;
using Geo; 
using Geo.Times;
using Gnsser.Service;

namespace Gnsser
{
    /// <summary>
    /// 简易伪距轨道确定
    /// </summary>
    public class RangeOrbitResult : MultiSiteGnssResult, IOrbitResult
    {
        /// <summary>
        /// 非差轨道确定
        /// </summary>
        /// <param name="Material">历元信息</param>
        /// <param name="ResultMatrix">平差信息</param>
        /// <param name="ParamNameBuilder">钟差估计器</param>
        /// <param name="previousResult">上一历元结果</param>
        public RangeOrbitResult(
            MultiSiteEpochInfo Material,
            AdjustResultMatrix ResultMatrix,
            RangeOrbitParamNameBuilder ParamNameBuilder,
            RangeOrbitResult previousResult = null)
            : base(Material, ResultMatrix, ParamNameBuilder)
        {
            //测站接收机钟差改正
            foreach (var site in Material)
            {
                var clkName = ParamNameBuilder.GetReceiverClockParamName(site.SiteName);
                var val = ResultMatrix.Estimated.Get(clkName);
                site.Time.Correction = GetTimeCorrectionSeconds(val.Value);
            } 

            //提取星历参数，可以用于改正卫星位置，进行迭代计算
             EphemerisResults = new BaseDictionary<SatelliteNumber, EphemerisResult>();
            foreach (var site in Material)
            {
                foreach (var sat in site)
                {
                    var prn = sat.Prn;

                    Ephemeris estimated = new Ephemeris(sat.Ephemeris.Prn, sat.Ephemeris.Time)
                    {
                        XyzDotRms = new XYZ()
                    };
                    var clkName = ParamNameBuilder.GetSatClockParamName(prn);
                    var val = ResultMatrix.Estimated.Get(clkName);
                    estimated.ClockBias = GetTimeCorrectionSeconds(val.Value);
                    estimated.ClockBiasRms = val.Rms / GnssConst.LIGHT_SPEED;

                    var names = ParamNameBuilder.GetSatDxyz(prn);
                    foreach (var item in names)
                    {
                        val = ResultMatrix.Estimated.Get(item);
                        if (item.Contains(Gnsser.ParamNames.Dx))
                        {
                            estimated.XYZ.X = val.Value;
                            estimated.XyzDotRms.X = val.Rms;
                        }
                        if (item.Contains(Gnsser.ParamNames.Dy))
                        {
                            estimated.XYZ.Y = val.Value;
                            estimated.XyzDotRms.Y = val.Rms;
                        }
                        if (item.Contains(Gnsser.ParamNames.Dz))
                        {
                            estimated.XYZ.Z = val.Value;
                            estimated.XyzDotRms.Z = val.Rms;
                        }
                    }                   

                    EphemerisResults[prn] = new EphemerisResult((Ephemeris)sat.Ephemeris, estimated);
                }
            }
        }

        /// <summary>
        /// 估值星历。可以用于改正卫星位置，进行迭代计算
        /// </summary>
        public BaseDictionary<SatelliteNumber, EphemerisResult> EphemerisResults { get; set; } 
        /// <summary>
        /// 由钟差等效距离转换成秒改正数。
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public double GetTimeCorrectionSeconds(double distance)
        {
            return distance / GnssConst.LIGHT_SPEED;
        }
    }   

    /// <summary>
    /// 星历结果
    /// </summary>
    public class EphemerisResult
    {
        public EphemerisResult(Ephemeris Original, Ephemeris Estimated)
        {
            this.Original = Original;
            this.Estimated = Estimated;
            this.Corrected = Original + Estimated;
        }
        /// <summary>
        /// 原始值
        /// </summary>
        public Ephemeris Original { get; set; }
        /// <summary>
        /// 估值
        /// </summary>
        public Ephemeris Estimated { get; set; }
        /// <summary>
        /// 改正后
        /// </summary>
        public Ephemeris Corrected { get; set; }


    }
    
}
