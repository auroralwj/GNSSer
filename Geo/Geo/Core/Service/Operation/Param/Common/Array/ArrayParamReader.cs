//2016.02.16, czs, create in xi'an hongqing, 通用数组

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
    ///通用数组
    /// </summary>
    public class ArrayParamReader : LineFileReader<ArrayParam>
    {       /// <summary>
        /// 默认构造函数
        /// </summary>
        public ArrayParamReader() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public ArrayParamReader(string filePath, string metaFilePath = null) : base(filePath, metaFilePath)
        { 
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public ArrayParamReader(string filePath, Gmetadata Gmetadata)
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
        public override ArrayParam Parse(string[] items)
        { 
            return new ArrayParam(items);
        } 
    }
}
