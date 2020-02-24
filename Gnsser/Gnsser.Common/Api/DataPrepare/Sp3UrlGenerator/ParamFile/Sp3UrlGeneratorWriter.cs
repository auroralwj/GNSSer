//2015.10.23, czs, create in 西安五路口 凉皮店,URL生成器

using System;
using System.IO;
using Geo.Common;
using Geo.Coordinates;
using System.Collections;
using System.Collections.Generic;
using Geo.IO;

namespace Gnsser.Api
{
    /// <summary>
    ///URL生成器
    /// </summary>
    public class Sp3UrlGeneratorWriter : LineFileWriter<Sp3UrlGeneratorParam>
    {       /// <summary>
        /// 默认构造函数
        /// </summary>
        public Sp3UrlGeneratorWriter() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public Sp3UrlGeneratorWriter(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public Sp3UrlGeneratorWriter(string filePath, Gmetadata Gmetadata)
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
                Geo.Utils.ObjectUtil.GetPropertyName<Sp3UrlGeneratorParam>(m=>m.Pattern),
                Geo.Utils.ObjectUtil.GetPropertyName<Sp3UrlGeneratorParam>(m=>m.SourceName),
                Geo.Utils.ObjectUtil.GetPropertyName<Sp3UrlGeneratorParam>(m=>m.StartTime),
                Geo.Utils.ObjectUtil.GetPropertyName<Sp3UrlGeneratorParam>(m=>m.EndTime),
                Geo.Utils.ObjectUtil.GetPropertyName<Sp3UrlGeneratorParam>(m=>m.Interval),
                Geo.Utils.ObjectUtil.GetPropertyName<Sp3UrlGeneratorParam>(m=>m.OutputPath),
                Geo.Utils.ObjectUtil.GetPropertyName<Sp3UrlGeneratorParam>(m=>m.LocalDirectory),
            };
            return data;
        }
         

    }
}
