//2017.08.16, czs, create in hongqing, 电离层文件的读取

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Gnsser.Times;
using Geo.Times;
using Geo;
using Geo.Coordinates;

namespace Gnsser.Data
{
    /// <summary>
    /// 电离层历元结果，关键字为纬度，记录值为经度和数值
    /// </summary>
    public class IonoSection : Geo.BaseDictionary<double, IonoRecord>
    {
        /// <summary>
        /// 构造函数,关键字为纬度，记录值为经度和数值
        /// </summary>
        public IonoSection()
        { 
        }   
        /// <summary>
        /// 头部
        /// </summary>
        public IonoHeader Header { get; set; }
        /// <summary>
        /// 当前历元
        /// </summary>
        public Time Time { get; set; }

        public ObjectTableStorage GetTable(bool isShowRms)
        {
            ObjectTableStorage table = new ObjectTableStorage();
            foreach (var lat in this.Keys)
            {
                var records = this[lat];
                table.NewRow();
                table.AddItem("Lat", lat);
                foreach (var lon in records.Keys)
                {
                    var val = records[lon];
                    object str = val.Value;
                    if (isShowRms) { str = val; }
                    table.AddItem(lon, str);
                }
            }
            return table;
        }
        /// <summary>
        /// 数据字典
        /// </summary>
        /// <param name="isRms"></param>
        /// <returns></returns>
        public TwoNumeralKeyAndValueDictionary GetNumeralKeyDic(bool isRms = false)
        {
            var dic = new TwoNumeralKeyAndValueDictionary(); 
            foreach (var lat in this.Keys)
            {
                var records = this[lat];
                foreach (var lon in records.Keys)
                {
                    var val = records[lon];
                    object str = val.Value;

                    if (isRms) { dic[lon, lat] = val.Rms; }
                    else { dic[lon, lat] = val.Value; }
                }
            }
            dic.Init();
            return dic;  
        }
        /// <summary>
        /// 获取坐标集合,其中，高程作为数据
        /// </summary>
        /// <param name="isRms"></param>
        /// <returns></returns>
        public List<GeoCoord> GetGeoCoords(bool isRms = false)
        {
            List<GeoCoord> geoCoords = new List<GeoCoord>();

            foreach (var lat in this.Keys)
            {
                var records = this[lat];
                foreach (var lon in records.Keys)
                {
                    var val = records[lon];
                    object str = val.Value;

                    var geoCoord = new GeoCoord(lon, lat, val.Value);
                    if (isRms) { geoCoord.Height = val.Rms; }
                    geoCoords.Add(geoCoord);
                }
            }
            return geoCoords; 
        }
    }
   
}
