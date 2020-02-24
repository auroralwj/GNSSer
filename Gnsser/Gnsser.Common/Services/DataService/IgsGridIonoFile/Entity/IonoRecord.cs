//2017.08.16, czs, create in hongqing, 电离层文件的读取

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Gnsser.Times;
using Geo.Times;
using Geo;


namespace Gnsser.Data
{
    /// <summary>
    /// 电离层记录,一个纬度对应全部经度。
    /// </summary>
    public class IonoRecord : BaseDictionary<double, RmsedNumeral>
    {
        public IonoRecord()
        {
            LonRange = new IncreaseValue(); 
        }
        public IncreaseValue LonRange { get; set; }               

        public double Height { get; set; } 
        public double Lat { get; set; }
    }
}
