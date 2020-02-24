//2016.02.19, czs, create in xi'an hongqing, NamedAstroCoord 文件读取

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
    ///NamedAstroCoord文件读取
    /// </summary>
    public class IdAstroProductReader : LineFileReader<IdAstroProduct>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public IdAstroProductReader(string filePath, string metaFilePath = null) : base(filePath, metaFilePath)
        { 
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public IdAstroProductReader(string filePath, Gmetadata Gmetadata)
            : base(filePath, Gmetadata)
        { 
        }

        /// <summary>
        /// 默认的元数据
        /// </summary>
        /// <returns></returns>
        public override Gmetadata GetDefaultMetadata()
        {
            return Gmetadata.DefaultAstroCoordMetadata;
        }


        /// <summary>
        /// 字符串列表解析为属性
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public override IdAstroProduct Parse(string[] items)
        { 
            var name = items[ PropertyIndexes[ VariableNames.Id ]];
            var coord = new LonLat(
               Double.Parse( items[  PropertyIndexes[ VariableNames.Lon ]]),
               Double.Parse( items[  PropertyIndexes[ VariableNames.Lat ]])
                );
           var Azimuth =   Double.Parse(items[PropertyIndexes[VariableNames.Azimuth]]);
           var DirectionPoint =    items[PropertyIndexes[VariableNames.ToId]];
            if (this.IsPropertyUnitChanged)
            {
                coord.Lon = Convert(VariableNames.Lon, coord.Lon);
                coord.Lat = Convert(VariableNames.Lat, coord.Lat);
                Azimuth = Convert(VariableNames.Azimuth, Azimuth);
            }

            IdAstroProduct citem = new IdAstroProduct(name, coord)
            {
                Azimuth = Azimuth,
                ToId = DirectionPoint
            };
            return citem;
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
