//2018.05.25, czs, create in HMX, CODE电离层球谐函数文件的读取

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
    /// 电离层球谐函数文件。
    /// TEC 单位为TECU，1 TECU= 1E16 个电子
    /// </summary>
    public class IonoHarmonicFile : BaseDictionary<Time, IonoHarmonicSection>, IIgsProductFile
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public IonoHarmonicFile()
        { 
        }
        /// <summary>
        /// 代码
        /// </summary>
        public string SourceCode { get { return this.Name.Substring(0, 2); } }
        
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
        internal void Add(IonoHarmonicSection section)
        {
            if(!Contains(section.Time))
            base.Add(section.Time, section); 
        }

    }
   
}
