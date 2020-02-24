using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Utils;
using Gnsser.Data.Rinex;

namespace Gnsser
{
    /// <summary>
    /// 天线文件头文件。
    /// Antenna File Header
    /// </summary>
    public class AntennaHeader
    {
        public AntennaHeader()
        {
            this.Comments = new List<string>();
        }

        //头文件信息
        /// <summary>
        /// 版本
        /// </summary>
        public double Version { get; set; }
        /// <summary>
        /// 卫星系统
        /// </summary>
        public SatelliteSystem SatelliteSystem { get; set; }//Satellite system
        /// <summary>
        /// 注释
        /// </summary>
        public List<string> Comments { get; set; }
        /// <summary>
        /// Phase Center Variation（PCV）天线相位中心变化类型。
        /// </summary>
        public PcvType PcvType { get; set; }

        /// <summary>
        /// Reference antenna type(relative)
        /// </summary>
        public string ReferenceAntena { get; set; }
        /// <summary>
        /// Reference antenna serial(relative)
        /// </summary>
        public string ReferenceAntenaSerial { get; set; }

        public bool IsAbsolute { get { return PcvType.Absolute == PcvType; } }
    }
}
