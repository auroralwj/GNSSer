//2016.07.14, czs, create in xi'an hongqing, IdAzimuth 文件读取

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
    /// IdAzimuth 文件读取
    /// </summary>
    public class IdAzimuthReader : LineFileReader<IdAzimuth>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public IdAzimuthReader(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public IdAzimuthReader(string filePath, Gmetadata Gmetadata)
            : base(filePath, Gmetadata)
        {
        }
  

        /// <summary>
        /// 默认的元数据
        /// </summary>
        /// <returns></returns>
        public override Gmetadata GetDefaultMetadata()
        {
            return Gmetadata.DefaultAzimuthMetadata;
        }


        /// <summary>
        /// 字符串列表解析为属性
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public override IdAzimuth Parse(string[] items)
        {
            var id = items[PropertyIndexes[VariableNames.Id]];

            var Azimuth = Double.Parse(items[PropertyIndexes[VariableNames.Azimuth]]);
            var destId = items[PropertyIndexes[VariableNames.ToId]];
            if (this.IsPropertyUnitChanged)
            {
                Azimuth = Convert(VariableNames.Azimuth, Azimuth);
            }

            IdAzimuth citem = new IdAzimuth(id, destId, Azimuth);
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