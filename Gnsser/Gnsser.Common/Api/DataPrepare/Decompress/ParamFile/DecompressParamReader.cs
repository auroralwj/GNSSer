//2015.10.01, czs, create in K385宝鸡到成都列车上, 解压

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
    public class DecompressParamReader : LineFileReader<DecompressParam>
    {       /// <summary>
        /// 默认构造函数
        /// </summary>
        public DecompressParamReader() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public DecompressParamReader(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public DecompressParamReader(string filePath, Gmetadata Gmetadata)
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
                Geo.Utils.ObjectUtil.GetPropertyName<DecompressParam>(m=>m.InputPath),
                Geo.Utils.ObjectUtil.GetPropertyName<DecompressParam>(m=>m.OutputPath),
                Geo.Utils.ObjectUtil.GetPropertyName<DecompressParam>(m=>m.IsDeleteSource),
                Geo.Utils.ObjectUtil.GetPropertyName<DecompressParam>(m=>m.IsOverwrite)
            };
            return data;
        }

        /// <summary>
        /// 字符串列表解析为属性
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public override DecompressParam Parse(string[] items)
        {
            bool isFull = items.Length == PropertyIndexes.Count;

            var propertyName= Geo.Utils.ObjectUtil.GetPropertyName<DecompressParam>(m => m.InputPath);

            var from = items[PropertyIndexes[propertyName]];
            var row = new DecompressParam()
            {
               InputPath = from
            };

            propertyName = Geo.Utils.ObjectUtil.GetPropertyName<DecompressParam>(m => m.OutputPath);
            row.OutputPath = GetPropertyValue(items, propertyName);
            if (row.OutputPath == null && PreviousObject != null)
            {
                row.OutputPath = PreviousObject.OutputPath;
            }

            propertyName = Geo.Utils.ObjectUtil.GetPropertyName<DecompressParam>(m => m.IsOverwrite);
            row.IsOverwrite = bool.Parse(GetPropertyValue(items, propertyName));
            if (row.OutputPath == null && PreviousObject != null)
            {
                row.IsOverwrite = PreviousObject.IsOverwrite;
            }

            propertyName = Geo.Utils.ObjectUtil.GetPropertyName<DecompressParam>(m => m.IsDeleteSource);
            row.IsDeleteSource = bool.Parse(GetPropertyValue(items, propertyName));
            if (row.OutputPath == null && PreviousObject != null)
            {
                row.IsDeleteSource = PreviousObject.IsDeleteSource;
            }

            if (isFull)
            {
                PreviousObject = row;
            }
            return row;
        }

    }
}
