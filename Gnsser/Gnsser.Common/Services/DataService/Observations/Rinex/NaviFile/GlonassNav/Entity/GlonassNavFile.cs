//2016.10.17, czs & double , edit in hongqing, 重构为字典模式

using System;
using System.Collections.Generic;

using System.Text;
using System.IO;
using Geo.Coordinates;

using Geo.Times;
using Gnsser.Times;
using Gnsser.Service;
using Geo;

namespace Gnsser.Data.Rinex
{
    /// <summary>
    ///  RINEX GLONASS 导航文件。
    /// </summary>
    public class GlonassNavFile : BaseDictionary<SatelliteNumber,  List<GlonassNavRecord> >
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GlonassNavFile() { }
        /// <summary>
        /// 头文件
        /// </summary>
        public NavFileHeader Header { get; set; }
        /// <summary>
        /// 记录.
        /// </summary>
        public List<GlonassNavRecord> NavRecords
        {
            get
            {
                var list = new List<GlonassNavRecord>(); 
                foreach (var item in this)
                {
                    list.AddRange(item);
                } 
                return list;
            }
        }

        /// <summary>
        /// 开始时间，不代表所有为卫星。
        /// </summary>
        public Time StartTime { get; set; }
        /// <summary>
        /// 结束时间，不代表所有为卫星。
        /// </summary>
        public Time EndTime { get; set; }
        #region 扩展属性和方法
        /// <summary>
        /// 卫星编号集合
        /// </summary>
        public List<SatelliteNumber> Prns { get { return new List<SatelliteNumber>(this.Keys); } }
      
        /// <summary>
        /// 添加一个星历
        /// </summary>
        /// <param name="gloEph"></param>
        public void Add(GlonassNavRecord gloEph) { this.SetTime(gloEph.Time); this.GetOrCreate(gloEph.Prn).Add(gloEph); }
 
        public override List<GlonassNavRecord> Create(SatelliteNumber key)
        {
            return new  List<GlonassNavRecord>();
        }

        private void SetTime(Time time)
        { 
            if (StartTime > time)
            {
                StartTime = time;
            }
            if (EndTime < time)
            {
                EndTime = time;
            }
        }
        #endregion
    }
}