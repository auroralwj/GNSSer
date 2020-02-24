//2014.09.15, czs, create, 设置卫星星历。
//2014.10.12, czs, edit in hailutu, 对星历赋值进行了重新设计，分解为几个不同的子算法
//2015.02.08, 崔阳, 卫星钟差和精密星历若同时存在，则不可分割
//2017.08.06, czs, refact in hongqing, RelativeEphemerisCorrector 单独成类
//2017.08.06, czs, refact in hongqing, ClockEphemerisReviser 单独成类

using System;
using System.IO;
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
using Geo.Common;
using Gnsser.Times;
using Gnsser.Data;
using Geo.Times;
using Gnsser.Correction;

namespace Gnsser
{ 
    
    /// <summary>
    /// 钟差单独改正
    /// </summary>
    public class ClockEphemerisReviser : EphemerisReviser
    {
        /// <summary>
        /// 钟差单独改正
        /// </summary>
        public ClockEphemerisReviser(ISimpleClockService ClockService)
        {
            this.Name = "钟差单独改正";
            this.ClockDataSource = ClockService;
            Failes = new List<SatelliteNumber>();
        }

        private ISimpleClockService ClockDataSource;
        /// <summary>
        /// 失败了，则记住，不再反复尝试
        /// </summary>
       List<SatelliteNumber> Failes { get; set; }
        /// <summary>
        /// 相对论改正钟差
        /// </summary>
        /// <param name="eph"></param>
        /// <returns></returns>
        public override bool Revise(ref IEphemeris eph)
        {
            //采用精密钟差服务改正
            if (ClockDataSource != null )
            {
                if (Failes.Contains(eph.Prn)) { return true; }

                var clock = ClockDataSource.Get(eph.Prn, eph.Time);
                if (clock != null)
                {
                    if (clock.ClockBias != 0) { eph.ClockBias = clock.ClockBias; }
                    //if (clock.ClockDrift != 0) { eph.ClockDrift = clock.ClockDrift; } //如果钟漂不为 0 ，则赋值。
                }
                else//获取失败
                {
                    int i = 0;
                    //下次避免再次计算

                    Failes.Add(eph.Prn);
                }
            }
            else
            {
                log.Debug("没有钟差服务！钟差不会单独改正" + eph);
            }

            return true;
        }
    }      
}
