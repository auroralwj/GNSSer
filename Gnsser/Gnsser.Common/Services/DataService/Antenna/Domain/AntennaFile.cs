//2014.05.22, Cui Yang, created,改进自GPSTk。
//2014.07.22, czs, Refactoring，去掉了大量冗余代码.
//2014.08.18, czs, edit，模块化程度提高，改 抛出异常 为返回null。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gnsser.Times;
using Geo.Utils;
using Gnsser.Data.Rinex;

namespace Gnsser.Data
{

    /// <summary>
    /// 天线文件
    /// </summary>
    public class AntennaFile
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public AntennaFile()
        {
            this.Antennas = new List<Antenna>();
        }

        /// <summary>
        /// 用于存储已经搜寻过的天线数据，下一次使用时，可以直接获取之。
        /// </summary>
        public List<Antenna> Antennas { get; set; }
        /// <summary>
        /// 头部信息。
        /// </summary>
        public AntennaHeader Header { get; set; }

    }
}
