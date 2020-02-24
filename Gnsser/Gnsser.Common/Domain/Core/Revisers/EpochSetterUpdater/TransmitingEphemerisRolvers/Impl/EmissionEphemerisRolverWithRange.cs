//2014.09.15, czs, create, 设置卫星星历。
//2014.10.12, czs, edit in hailutu, 对星历赋值进行了重新设计，分解为几个不同的子算法 
//2017.08.06, czs, edit in hongqing, 代码整理，面向对象重构

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
using Geo.Common;
using Gnsser.Times;
using Geo.Times;
using Gnsser.Data;

namespace Gnsser
{
    /// <summary>
    /// 采用伪距和接收机钟面时进行卫星位置计算。
    /// 如果没有伪距，则返回 null。
    /// </summary>
    public class EmissionEphemerisRolverWithRange : EmissionEphemerisRolver
    {
        /// <summary>
        /// 构造函数。设置卫星星历，请在本类前执行 观测值的有效性检查与过滤。
        /// </summary>
        public EmissionEphemerisRolverWithRange(IEphemerisService EphemerisService, DataSourceContext DataSouceProvider, EpochSatellite sat)
            : base(EphemerisService, DataSouceProvider, sat)
        {
            this.Name = "直接伪距法计算星历"; 
        }
         

        /// <summary>
        /// 计算
        /// </summary>
        public override IEphemeris Get()
        {
            SatelliteNumber prn = EpochSat.Prn;
            Time recevingTime = EpochSat.RecevingTime;//utc?? ,接收时刻时间，含钟差改正
            if (EpochSat.Time.Correction != 0)
            {
                int i = 0;
                int j = 0;
            }
            var transTime = EpochSat.GetTransitTime();
     
            Time emissionTime = recevingTime - transTime;
            var eph= EphemerisService.Get(prn, emissionTime);
            return eph;
        }
    }

}
