
//2018.09.31, czs, create in fujian yong'an, 法国宽项读取器

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Times;
using Geo.IO;
using Gnsser;
using Gnsser.Checkers;
using Geo.Referencing;
using Geo.Utils;

namespace Gnsser
{
    /// <summary>
    /// 法国宽项读取器。 
    /// ftp://ftpsedr.cls.fr/pub/igsac/Wide_lane_GPS_satellite_biais.wsb
    /// </summary>
    public class WideLaneBiasService : IService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gofFilePath"></param>
        /// <param name="metaFilePath"></param>
        public WideLaneBiasService(string gofFilePath, string metaFilePath = null)
        {
            Reader = new WideLaneBiasItemReader(gofFilePath, metaFilePath);
            DataFile = new WideLaneBiasFile();
            foreach (var item in Reader)
            {
                DataFile[item.Time] = item;
            }
        }
        /// <summary>
        /// 读取
        /// </summary>
        WideLaneBiasItemReader Reader { get; set; }
        WideLaneBiasFile DataFile { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 如果获取失败，则返回 0 。
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public double Get(SatelliteNumber prn, Time time)
        {
            var items = Get(time); 

            if(items ==null || !items.Data.Contains(prn))
            {
                return 0;
            }

            return items.Data[prn];
        }
        /// <summary>
        /// 获取当天的
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public WideLaneBiasItem Get(Time time)
        {
            var date = time.Date + TimeSpan.FromHours(12);
            if (DataFile.Contains(date)) { return DataFile[date]; }
             
            //foreach (var item in Reader)
            //{
            //    if (item.Time.Date == date)
            //    {
            //        DataFile[date] = item;
            //        return item; 
            //    }
            //}
            return null;
        }
    }

}