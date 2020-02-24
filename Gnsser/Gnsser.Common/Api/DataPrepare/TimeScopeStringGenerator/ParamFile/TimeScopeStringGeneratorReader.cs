//2015.10.02, czs, create in 彭州, URL生成器
//2015.10.06, czs, edit in 彭州到成都动车C6186, 时间段字符串生成器

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
    ///复制的参数文件读取
    /// </summary>
    public class TimeScopeStringGeneratorReader : LineFileReader<TimeScopeStringGeneratorParam>
    {       /// <summary>
        /// 默认构造函数
        /// </summary>
        public TimeScopeStringGeneratorReader() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public TimeScopeStringGeneratorReader(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public TimeScopeStringGeneratorReader(string filePath, Gmetadata Gmetadata)
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


        /// <summary>
        /// 字符串列表解析为属性
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public override TimeScopeStringGeneratorParam Parse(string[] items)
        {
            bool isFull = items.Length == PropertyIndexes.Count;

            var pattern = items[PropertyIndexes[Geo.Utils.ObjectUtil.GetPropertyName<TimeScopeStringGeneratorParam>(m => m.Pattern)]];
            var row = new TimeScopeStringGeneratorParam()
            {
                Pattern = pattern
            };


            var item = GetPropertyValue(items, Geo.Utils.ObjectUtil.GetPropertyName<Sp3UrlGeneratorParam>(m => m.StartTime));
            if (item == null && PreviousObject != null)
            {
                row.StartTime = PreviousObject.StartTime;
            }
            else
            {
                row.StartTime = DateTime.Parse(item);
            }

            item = GetPropertyValue(items, Geo.Utils.ObjectUtil.GetPropertyName<Sp3UrlGeneratorParam>(m => m.EndTime));
            if (item == null && PreviousObject != null)
            {
                row.EndTime = PreviousObject.EndTime;
            }
            else
            {
                row.EndTime = DateTime.Parse(item);
            }

            item = GetPropertyValue(items, Geo.Utils.ObjectUtil.GetPropertyName<Sp3UrlGeneratorParam>(m => m.Interval));
            if (!String.IsNullOrWhiteSpace(item))
            {
                row.Interval = Int32.Parse(item);
            }
            else if (PreviousObject != null)
            {
                row.Interval = PreviousObject.Interval;
            }

            item = GetPropertyValue(items, Geo.Utils.ObjectUtil.GetPropertyName<Sp3UrlGeneratorParam>(m => m.OutputPath));
            if (!String.IsNullOrWhiteSpace(item))
            {
                row.OutputPath = (item);
            }
            else if (PreviousObject != null)
            {
                row.OutputPath = PreviousObject.OutputPath;
            }

            return row;
        }

    }
}
