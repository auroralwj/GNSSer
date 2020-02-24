//2017.06.14, czs, create in hongqing, FCB 数据服务

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gnsser.Service;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Utils;
using Gnsser;
using Gnsser.Times;
using Geo;
using Geo.Common;
using Geo.Times;

namespace Gnsser.Data
{ 
    /// <summary>
    ///  FCB 数据服务
    /// </summary>
    public class FcbValue : RmsedNumeral//, Geo.Common.Namable
    {
        /// <summary>
        /// 初始化一个卫星信息
        /// </summary>
        public FcbValue(SatelliteNumber prn,Time Time , double val, double rms)
            : base(val, rms)
        {
            this.Prn = prn;
            this.Time = Time;
        } 
        /// <summary>
        /// 卫星编号
        /// </summary>
        public SatelliteNumber Prn { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public Time Time { get; set; } 

        public override string ToString()
        {
            return Prn + " " + Time + " " + Value + " " + Rms;
        }
    }
}