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
    public class Gpt2Res1Degree:Gpt2Res
    {
        public Gpt2Res1Degree(Time time, double lat, double lon, double hgt, double p, double T, double dT, double Tm, double e, double ah, double aw, double la, double undu)
            : base(time, lat, lon, hgt, p, T, dT, e, ah, aw, undu)
        {
            this.Tm = Tm;
            this.la = la;
        }        
        /// <summary>
        /// water vapor decrease factor 
        /// </summary>
        public double la;
        /// <summary>
        /// mean temperature of the water vapor in degrees Kelvin 
        /// </summary>
        public double Tm;
    }
}
