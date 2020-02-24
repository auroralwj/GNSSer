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
using Geo.Times; 
using Geo;

namespace Gnsser.Data
{

    /// <summary>
    /// 卫星信息读取器
    /// </summary>
    public class SatExcludeReader
    {
        /// <summary>
        /// 构造函数。可以指定文件路径，但此处并不读取，需要调用Read()方法才读取。
        /// </summary>
        /// <param name="filePath"></param>
        public SatExcludeReader(FileOption filePath)
        {
            this.path = filePath.FilePath;
        }
        /// <summary>
        /// 构造函数。可以指定文件路径，但此处并不读取，需要调用Read()方法才读取。
        /// </summary>
        /// <param name="filePath"></param>
        public SatExcludeReader(string filePath)
        {
            this.path = filePath;
        }

        string path;

        /// <summary>
        /// 排除的卫星列表
        /// </summary>
        public Dictionary<Time, SatExcludeItem> ExcludeSats { get; set; }
        /// <summary>
        /// 读取卫星信息。
        /// 由于卫星信息文件较小，这里一次性读取完毕。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public void Read()
        {
            Dictionary<Time, SatExcludeItem> sats = new Dictionary<Time, SatExcludeItem>();
            using (StreamReader sr = new StreamReader(path))
            {
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] strs = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    // SatExcludeItem 
                    int year = int.Parse(strs[0]);
                    int dayOfYear = int.Parse(strs[1]);
                     
                    Time gpstime = new Time(year, dayOfYear);

                    int length = strs.Length;
                    List<SatelliteNumber> prns = new List<SatelliteNumber>();
                    for (int i = 2; i < length; i++)
                    {
                        SatelliteNumber prn = SatelliteNumber.Parse(strs[i]);
                        prns.Add(prn);
                    }
                    SatExcludeItem item = new SatExcludeItem(gpstime, prns);

                    if(!sats.ContainsKey(item.Date))
                        sats.Add(item.Date, item);
                }
            }
            ExcludeSats =  sats; 
        }

    }//End SatDataReader
}
