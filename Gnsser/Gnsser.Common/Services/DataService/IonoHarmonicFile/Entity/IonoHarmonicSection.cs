//2018.05.25, czs, create in HMX, CODE电离层球谐函数

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Gnsser.Times;
using Geo.Times;
using Geo;
using Geo.Coordinates;

namespace Gnsser.Data
{
    /// <summary>
    /// 电离层球谐函数数据
    /// </summary>
    public class IonoHarmonicSection : SphericalHarmonicsFile// Geo.BaseDictionary<int, Geo.BaseDictionary<int, RmsedNumeral>>
    {
        /// <summary>
        /// 构造函数,关键字为纬度，记录值为经度和数值
        /// </summary>
        public IonoHarmonicSection()
        { 
           // CreateFunc = new Func<int, BaseDictionary<int, RmsedNumeral>>(key => new BaseDictionary<int, RmsedNumeral>(key + "") );
        }   
        /// <summary>
        /// 头部
        /// </summary>
        public IonoHarmonicHeader Header { get; set; }
        /// <summary>
        /// 当前历元
        /// </summary>
        public Time Time { get { return Header.ValidPeroid.Start; } }
        /// <summary>
        /// 字符串显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Header.ModelNumberStationName + ", " + Time + ", Max  " + Header.MaxTec ;
        }
    }
   
}
