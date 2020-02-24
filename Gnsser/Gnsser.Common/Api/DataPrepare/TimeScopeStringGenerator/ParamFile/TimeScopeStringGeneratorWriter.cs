//2015.10.23, czs, create in 西安五路口 凉皮店,具有时段的字符串参数

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
    ///具有时段的字符串参数
    /// </summary>
    public class TimeScopeStringGeneratorWriter : LineFileWriter<TimeScopeStringGeneratorParam>
    {       /// <summary>
        /// 默认构造函数
        /// </summary>
        public TimeScopeStringGeneratorWriter() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public TimeScopeStringGeneratorWriter(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public TimeScopeStringGeneratorWriter(string filePath, Gmetadata Gmetadata)
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
            data.OutputFilePath = @"Data\OutputFilePath_"+DateTime.Now.Ticks+".url";
            data.PropertyNames = new string[] { 
                Geo.Utils.ObjectUtil.GetPropertyName<TimeScopeStringGeneratorParam>(m=>m.Pattern),
                Geo.Utils.ObjectUtil.GetPropertyName<Sp3UrlGeneratorParam>(m=>m.StartTime),
                Geo.Utils.ObjectUtil.GetPropertyName<Sp3UrlGeneratorParam>(m=>m.EndTime),
                Geo.Utils.ObjectUtil.GetPropertyName<Sp3UrlGeneratorParam>(m=>m.Interval),
            };
            return data;
        }
         
    }
}
