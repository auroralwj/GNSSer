//2014.09.24, czs, create, 卫星可用性,采用 Gamit 文档 svs_exclude.dat 格式


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gnsser.Service;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Gnsser.Times;
using Geo.Utils;
using Gnsser;
using Geo;
using Geo.Times; 
namespace Gnsser.Data
{

    /// <summary>
    /// 卫星信息读取器
    /// </summary>
    public class SatExcludeFileService : GnssFileService<Boolean>
    {
        /// <summary>
        /// 构造函数。可以指定文件路径，但此处并不读取，需要调用Read()方法才读取。
        /// </summary>
        /// <param name="filePath"></param>
        public SatExcludeFileService(FileOption filePath) : base(filePath)
        {
            SatExcludeReader reader = new SatExcludeReader(filePath);
            reader.Read();
            this.ExcludeSats = reader.ExcludeSats;
        }
        /// <summary>
        /// 构造函数。可以指定文件路径，但此处并不读取，需要调用Read()方法才读取。
        /// </summary>
        /// <param name="filePath"></param>
        public SatExcludeFileService(string filePath) : base(filePath)
        {
            SatExcludeReader reader = new SatExcludeReader(filePath);
            reader.Read();
            this.ExcludeSats = reader.ExcludeSats;
        }

        /// <summary>
        /// 排除的卫星列表
        /// </summary>
        public Dictionary<Time, SatExcludeItem> ExcludeSats { get; set; }
        /// <summary>
        /// 指定日期指定编号是否包含。
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="satelliteType">卫星编号</param>
        /// <returns></returns>
        public bool IsExcluded(Time date, SatelliteNumber prn)
        {
            bool isDay = ExcludeSats.ContainsKey(date);
            if (!isDay) 
                return false;
            bool exclude = ExcludeSats[date].Prns.Contains(prn);
            return exclude;
        }




    }//End SatDataReader
}
