//2015.10.09, czs, create in  xi'an hongqing, 坐标转换

using System;
using System.IO;
using Geo.Common;
using Geo.Coordinates;
using System.Collections;
using System.Collections.Generic;
using Geo.IO;
using Geo.Referencing;


namespace Gnsser.Api
{
    /// <summary>
    ///复制的参数文件读取
    /// </summary>
    public class GeoCoordToXyzReader : LineFileReader<GeoCoordToXyzParam>
    {       /// <summary>
        /// 默认构造函数
        /// </summary>
        public GeoCoordToXyzReader() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public GeoCoordToXyzReader(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public GeoCoordToXyzReader(string filePath, Gmetadata Gmetadata)
            : base(filePath, Gmetadata)
        {
        }

        /// <summary>
        /// 默认的元数据
        /// </summary>
        /// <returns></returns>
        public override Gmetadata GetDefaultMetadata()
        {
            Gmetadata data = Gmetadata.NewInstance; 
            data.PropertyNames = new string[] {  
                Geo.Utils.ObjectUtil.GetPropertyName<GeoCoord>(m=>m.Lon),
                Geo.Utils.ObjectUtil.GetPropertyName<GeoCoord>(m=>m.Lat),
                Geo.Utils.ObjectUtil.GetPropertyName<GeoCoord>(m=>m.Height), 
                Geo.Utils.ObjectUtil.GetPropertyName<GeoCoordToXyzParam>(m=>m.OutputPath), 

                VariableNames.SemiMinorAxis,
                VariableNames.FlatteningOrInverse,
                Geo.Utils.ObjectUtil.GetPropertyName<GeoCoordToXyzParam>(m=>m.AngleUnit),
            };
            return data;
        }


        /// <summary>
        /// 字符串列表解析为属性
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public override GeoCoordToXyzParam Parse(string[] items)
        { 
            var row = new GeoCoordToXyzParam();

            var lon = GetDoubleProperty(items, VariableNames.Lon);
            var lat = GetDoubleProperty(items, VariableNames.Lat);
            var height = GetDoubleProperty(items, VariableNames.Height);

            row.GeoCoord = new GeoCoord(lon, lat, height);
            //以下是可选参数     
            var item = GetPropertyValue(items, Geo.Utils.ObjectUtil.GetPropertyName<GeoCoordToXyzParam>(m => m.OutputPath));
            if (!String.IsNullOrWhiteSpace(item))
            {
                row.OutputPath = item;
            }
            else if (PreviousObject != null)
            {
                row.OutputPath =PreviousObject.OutputPath ;
            }

            // double 
            var item1 = GetPropertyValue(items,VariableNames.SemiMinorAxis);
            var item2 = GetPropertyValue(items,VariableNames.FlatteningOrInverse);
            if (!String.IsNullOrWhiteSpace(item1) && !String.IsNullOrWhiteSpace(item2) )
            {
                var SemiMinorAxis = Double.Parse(item1);
                var FlatteningOrInverse = Double.Parse(item2);
                row.Ellipsoid = new Ellipsoid(SemiMinorAxis, FlatteningOrInverse);
            }
            else if (PreviousObject != null)
            {
                row.Ellipsoid = PreviousObject.Ellipsoid;
            }  
            //角度单位，可选。
            item = GetPropertyValue(items, Geo.Utils.ObjectUtil.GetPropertyName<GeoCoordToXyzParam>(m => m.AngleUnit));
            if (!String.IsNullOrWhiteSpace(item))
            {
                row.AngleUnit =(AngleUnit)Enum.Parse(typeof(AngleUnit), item); 
            }
            else if (PreviousObject != null)
            {
                row.AngleUnit = PreviousObject.AngleUnit;
            }

            PreviousObject = row;

            return row;
        }

    }
}
