//2017.07.23,czs, create in hongqing,  混合类型的导航文件

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Gnsser.Times;
using Gnsser.Service;
using Geo.Times; 
using Geo;

namespace Gnsser.Data.Rinex
{
    /// <summary>
    /// RINEX GnssNavFile 导航文件。
    /// 以导航星历参数表达的导航文件，包括北斗、GPS、伽利略。
    /// 一个类的实例代表一个文件。
    /// </summary>
    public class MixedNavFile : IDisposable
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MixedNavFile() {
            this.ParamNavFile = new ParamNavFile();
            this.GlonassNavFile = new GlonassNavFile();
        } 
        
        #region 属性

        public ParamNavFile ParamNavFile { get; set; }

        public GlonassNavFile GlonassNavFile { get; set; } 
        /// <summary>
        /// 导航头部信息
        /// </summary>
        public NavFileHeader Header { get; set; }
        public void SetHeader(NavFileHeader Header) { this.Header = Header; this.ParamNavFile.Header = Header; this.GlonassNavFile.Header = Header; } 
        /// <summary>
        /// 卫星编号集合
        /// </summary>
        public List<SatelliteNumber> Prns { get { var prns = new List<SatelliteNumber>(ParamNavFile.Prns); prns.AddRange(GlonassNavFile.Prns); return prns; } }
        /// <summary>
        /// 文件名称
        /// </summary>
        public  string Name
        {
            get { return Header.Name; }
            set { if(Header !=null)  Header.Name = value; }
        }
         
        /// <summary>
        /// 开始时间，不代表所有为卫星。
        /// </summary>
        public Time StartTime { get { return Time.Max( this.ParamNavFile.StartTime, this.GlonassNavFile.StartTime); } }
        /// <summary>
        /// 结束时间，不代表所有为卫星。
        /// </summary>
        public Time EndTime { get { return Time.Max(this.ParamNavFile.EndTime, this.GlonassNavFile.EndTime); } }
        #endregion

        #region 方法

        /// <summary>
        /// 添加星历参数,重复星历将失败。
        /// </summary>
        /// <param name="EphemerisParam"></param>
        public void Add(EphemerisParam EphemerisParam)
        {
            this.ParamNavFile.Add(EphemerisParam); 
        }

        internal void Add(GlonassNavRecord record2)
        {
             this.GlonassNavFile.Add(record2);
        }
         
           
        #endregion 
        /// <summary>
        /// 清空，释放资源
        /// </summary>
        public void Dispose()
        {
            this.GlonassNavFile.Dispose();
            this.ParamNavFile.Dispose();
        }
    } 
}
