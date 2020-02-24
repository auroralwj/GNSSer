//2017.05.10, lly, add in zz,GT2 模型
//2017.06.28, czs, edit in hongqing, 速度优化


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gnsser.Service;
using Geo.Algorithm;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Utils;
using Gnsser;
using Geo;
using Geo.Common;
using Gnsser.Times;
using Geo.Times;

namespace Gnsser.Data
{
    public class Gpt2Res
    {
        public Gpt2Res(Time time, double lat, double lon, double hgt, double p, double T, double dT, double e, double ah, double aw, double undu)
        {
            this.time = time;
            this.lat = lat;
            this.lon = lon;
            this.hgt = hgt;
            this.p = p;
            this.T = T;
            this.dT = dT;
            this.e = e;
            this.ah = ah;
            this.aw = aw;
            this.undu = undu;
            this.GeoCoord = new GeoCoord(lon, lat, hgt);
        }
        /// <summary>
        /// 大地坐标
        /// </summary>
        public GeoCoord GeoCoord { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public Time time;
        /// <summary>
        /// 纬度
        /// </summary>
        public double lat;
        /// <summary>
        /// 经度
        /// </summary>
        public double lon;
        /// <summary>
        /// 大地高
        /// </summary>
        public double hgt;

        /// <summary>
        /// 压强 pressure in hPa 
        /// </summary>
        public  double p;
        /// <summary>
        /// temperature in degrees Celsius 
        /// </summary>
        public double T;
        /// <summary>
        /// temperature lapse rate in degrees per km  
        /// </summary>
        public double dT;
        /// <summary>
        ///  water vapour pressure in hPa 
        /// </summary>
        public double e;
        /// <summary>
        /// hydrostatic mapping function coefficient at zero height (VMF1)
        /// </summary>
        public double ah;
        /// <summary>
        /// wet mapping function coefficient (VMF1)
        /// </summary>
        public double aw;
        /// <summary>
        /// geoid undulation in m (vector of length nstat)
        /// </summary>
        public double undu;
    }
}
