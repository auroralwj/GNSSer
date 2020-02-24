//2015.10.09, czs, create in  xi'an hongqing, 周跳探测
//2015.10.28, czs, edit in hongqing, 具有输出版本的输入输出参数文件读取

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
    /// 具有输出版本的输入输出参数文件读取
    /// </summary>
    public class VersionedIoParamReader :   LineFileReader<VersionedIoParam>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public VersionedIoParamReader() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public VersionedIoParamReader(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public VersionedIoParamReader(string filePath, Gmetadata Gmetadata)
            : base(filePath, Gmetadata)
        {
        }
        
        /// <summary>
        /// 字符串列表解析为属性
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public override VersionedIoParam Parse(string[] items)
        {
            bool isFull = items.Length == PropertyIndexes.Count;

            var from = items[PropertyIndexes[Geo.Utils.ObjectUtil.GetPropertyName<IoParam>(m => m.InputPath)]];
            var row = new VersionedIoParam()
            {
                InputPath = from
            };

            row.OutputPath = GetPropertyValue(items, Geo.Utils.ObjectUtil.GetPropertyName<IoParam>(m => m.OutputPath));
            if (row.OutputPath == null && PreviousObject != null)
            {
                row.OutputPath = PreviousObject.OutputPath;
            }

            var Overwrite = GetPropertyValue(items, Geo.Utils.ObjectUtil.GetPropertyName<IoParam>(m => m.IsOverwrite));
            if (String.IsNullOrWhiteSpace(Overwrite))
            {
                if (PreviousObject != null)
                {
                    row.IsOverwrite = PreviousObject.IsOverwrite;
                }
            }
            else
            {
                row.IsOverwrite = bool.Parse(Overwrite);
            }

            var val = GetPropertyValue(items, Geo.Utils.ObjectUtil.GetPropertyName<VersionedIoParam>(m => m.OutputVersion));
            if (String.IsNullOrWhiteSpace(val))
            {
                if (PreviousObject != null)
                {
                    row.OutputVersion = PreviousObject.OutputVersion;
                }
            }
            else
            {
                row.OutputVersion = Double.Parse(val);
            } 

            PreviousObject = row;
            return row;
        }

    }
}
