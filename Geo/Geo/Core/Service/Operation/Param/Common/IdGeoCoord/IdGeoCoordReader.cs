//2015.09.27, czs, create in xi'an hongqing, 大地坐标文件读取

using System;
using System.IO;
using Geo.Common;
using Geo.Coordinates;
using System.Collections;
using System.Collections.Generic;
using Geo.IO;
using Geo;

namespace Geo.IO
{
    /// <summary>
    ///大地坐标文件读取
    /// </summary>
    public class IdGeoCoordReader : LineFileReader<IdGeoCoord>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public IdGeoCoordReader(string filePath, string metaFilePath = null) : base(filePath, metaFilePath)
        { 
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public IdGeoCoordReader(string filePath, Gmetadata Gmetadata)
            : base(filePath, Gmetadata)
        { 
        }

        /// <summary>
        /// 默认的元数据
        /// </summary>
        /// <returns></returns>
        public override Gmetadata GetDefaultMetadata()
        {
            return Gmetadata.DefaultLbhMetadata;
        }


        /// <summary>
        /// 字符串列表解析为属性
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public override IdGeoCoord Parse(string[] items)
        {
            var name = items[PropertyIndexes[VariableNames.Id]];
            double lon = Double.Parse(items[PropertyIndexes[VariableNames.Lon]]);
            double lat = Double.Parse(items[PropertyIndexes[VariableNames.Lat]]);
            double height = 0;
            if (PropertyIndexes.ContainsKey(VariableNames.Height) && items.Length > PropertyIndexes[VariableNames.Height])
            {
                height =  Double.Parse(items[PropertyIndexes[VariableNames.Height]]);
            }          

            var coord = new GeoCoord(lon, lat, height);

            if (this.IsPropertyUnitChanged)
            {
                coord.Lon = Convert(VariableNames.Lon, coord.Lon);
                coord.Lat = Convert(VariableNames.Lat, coord.Lat);
                coord.Height = Convert(VariableNames.Height, coord.Height);
            }

            return new IdGeoCoord(name, coord);
        }

        /// <summary>
        /// 单位转换
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public override double Convert(string propertyName, double val)
        {
            if (PropertyUnits.ContainsKey(propertyName) && DestPropertyUnits.ContainsKey(propertyName))
            {
                return UnitConverter.Convert(val, PropertyUnits[propertyName], DestPropertyUnits[propertyName]);
            }
            return val;
        }
    }
}
