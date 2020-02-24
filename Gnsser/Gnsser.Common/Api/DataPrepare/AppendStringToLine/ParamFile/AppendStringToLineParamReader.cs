//2015.10.07, create in xi'an, 追加参数到Gnsser参数文件

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
    public class AppendStringToLineParamReader : LineFileReader<AppendStringToLineParam>
    {       /// <summary>
        /// 默认构造函数
        /// </summary>
        public AppendStringToLineParamReader() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public AppendStringToLineParamReader(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public AppendStringToLineParamReader(string filePath, Gmetadata Gmetadata)
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
                Geo.Utils.ObjectUtil.GetPropertyName<AppendStringToLineParam>(m => m.FileToAppdend),
                Geo.Utils.ObjectUtil.GetPropertyName<AppendStringToLineParam>(m => m.Content), 
            };
            return data;
        }

        /// <summary>
        /// 字符串列表解析为属性
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public override AppendStringToLineParam Parse(string[] items)
        {
            bool isFull = items.Length == PropertyIndexes.Count;

            var from = items[PropertyIndexes[Geo.Utils.ObjectUtil.GetPropertyName<AppendStringToLineParam>(m => m.FileToAppdend)]];
            var row = new AppendStringToLineParam()
            {
                FileToAppdend = from
            };

            row.Content = GetPropertyValue(items,Geo.Utils.ObjectUtil.GetPropertyName<AppendStringToLineParam>(m => m.Content));
            if (row.Content == null && PreviousObject != null)
            {
                row.Content = PreviousObject.Content;
            }

            PreviousObject = row;
            return row;
        }

    }
}
