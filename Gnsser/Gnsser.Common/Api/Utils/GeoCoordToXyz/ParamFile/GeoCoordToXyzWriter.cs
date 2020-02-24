//2015.10.23, czs, create in 西安五路口 凉皮店,坐标转换

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
    ///坐标转换
    /// </summary>
    public class GeoCoordToXyzWriter : LineFileWriter<GeoCoordToXyzParam>
    {       /// <summary>
        /// 默认构造函数
        /// </summary>
        public GeoCoordToXyzWriter() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public GeoCoordToXyzWriter(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public GeoCoordToXyzWriter(string filePath, Gmetadata Gmetadata)
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
         
    }
}
