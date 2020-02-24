//2015.09.27, czs, create in xi'an hongqing, 基线标记文件读取

using System;
using System.IO;
using Geo.Common;
using Geo.Coordinates;
using System.Collections;
using System.Collections.Generic;
using Geo;
using Geo.IO;

namespace Geo.IO
{
    /// <summary>
    ///基线标记文件读取
    /// </summary>
    public class VectorNameReader : LineFileReader<VectorName>
    {       /// <summary>
        /// 默认构造函数
        /// </summary>
        public VectorNameReader() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public VectorNameReader(string filePath, string metaFilePath = null) : base(filePath, metaFilePath)
        { 
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public VectorNameReader(string filePath, Gmetadata Gmetadata)
            : base(filePath, Gmetadata)
        { 
        }

        /// <summary>
        /// 默认的元数据
        /// </summary>
        /// <returns></returns>
        public override Gmetadata GetDefaultMetadata()
        {
            return Gmetadata.DefaultVectorNameMetadata;
        }

        /// <summary>
        /// 字符串列表解析为属性
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public override VectorName Parse(string[] items)
        {
            var startName = items[ PropertyIndexes[ VariableNames.StartName ]];
            var endName = items[ PropertyIndexes[ VariableNames.EndName ]];
            return new VectorName(startName, endName);
        } 
    }
}
