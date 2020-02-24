//2014.05.22, Cui Yang, created
//2014.07.04, Cui Yang, 增加多映射通用集合类，添加了MultiMap引用,注：已经取消。
//2014.07.05, czs, edit, 进行了代码重构

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
using Geo.Times; 

namespace Gnsser.Data
{

    /// Jet Propulsion Laboratory (JPL) provides a file called "PRN_GPS"
    ///* with satellite information such as launch and deactivation dates,
    ///* block type GPS number, etc. This information is important for some
    ///* precise GPS satData processing algorithms, and is used in Gipsy/OASIS
    ///* software.
    /// <summary>
    /// 卫星基础信息。
    /// 说明：喷气推进实验室Jet Propulsion Laboratory (JPL)发布了 "PRN_GPS"文件，用于记录卫星信息，包括有效时间，编号，钟类型等。
    /// 该文件对于精密定位具有重要的作用。
    /// </summary>
    public class SatInfo
    {
        /// <summary>
        /// 初始化一个卫星信息
        /// </summary>
        public SatInfo()
        {
            TimePeriod = new TimePeriod(Time.MinValue, Time.MaxValue);
        }
        /// <summary>
        /// 卫星编号
        /// </summary>
        public SatelliteNumber Prn { get; set; }
        /// <summary>
        /// 有效时段
        /// </summary>
        public TimePeriod TimePeriod { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int GpsNumber;  //GPS number.
        /// <summary>
        /// 天线类型
        /// </summary>
        public string Block;   //Block the SV belongs to
        /// <summary>
        /// 轨道
        /// </summary>
        public string Orbit { get; set; }
        /// <summary>
        /// 卫星钟类型
        /// </summary>
        public string Clock { get; set; }

        public override string ToString()
        {
            return TimePeriod.ToString() + "  " + GpsNumber + " " + Prn + " " + Block + " " + Orbit + " " + Clock;
        }
    }
}