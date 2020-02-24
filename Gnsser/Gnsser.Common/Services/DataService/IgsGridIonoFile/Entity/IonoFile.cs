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
    /// 电离层文件。
    /// TEC 单位为TECU，1 TECU= 1E16 个电子
    /// </summary>
    public class IonoFile : BaseDictionary<Time, IonoSection>, IIgsProductFile
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public IonoFile()
        { 
        }
        /// <summary>
        /// 代码
        /// </summary>
        public string SourceCode { get { return this.Name.Substring(0, 2); } }
        /// <summary>
        /// DCB of Site
        /// </summary>
        public Dictionary<string, RmsedNumeral> DcbsOfSites { get { return this.Header.StationsWithBiasRms; } }
        /// <summary>
        /// DCB of PRN
        /// </summary>
        public Dictionary<SatelliteNumber, RmsedNumeral> DcbsOfSats { get { return this.Header.SatellitesWithBiasRms; } }
   
        /// <summary>
        /// 头部信息
        /// </summary>
        public IonoHeader Header { get; set; } 
        /// <summary>
        /// 名称
        /// </summary>
        public override string Name { get; set; }
        /// <summary>
        /// 时间范围读完后赋值
        /// </summary>
        public BufferedTimePeriod TimePeriod { get; set; }

        /// <summary>
        /// 增加一个
        /// </summary>
        /// <param name="section"></param>
        internal void Add(IonoSection section)
        {
            if(!Contains(section.Time))
            base.Add(section.Time, section); 
        }

    }
   
}
