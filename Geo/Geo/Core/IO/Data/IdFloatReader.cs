//2015.09.27, czs, create in xi'an hongqing, XYZ 坐标文件读取
//2016.02.10, czs, create in hongqing, 单值文件读取


using System;
using System.IO;
using Geo.Common;
using Geo.Coordinates;
using System.Collections;
using System.Collections.Generic;
using Geo.IO;

namespace Geo.IO
{
    /// <summary>
    /// 单值文件读取
    /// </summary>
    public class IdFloatReader :  LineFileReader<IdFloatRow>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public IdFloatReader(string filePath, string metaFilePath = null) : base(filePath, metaFilePath)
        { 
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public IdFloatReader(string filePath, Gmetadata Gmetadata)
            : base(filePath, Gmetadata)
        { 
        }

        /// <summary>
        /// 默认的元数据
        /// </summary>
        /// <returns></returns>
        public override Gmetadata GetDefaultMetadata()
        {
            return Gmetadata.DefaultSingleValueMetadata;
        }

        /// <summary>
        /// 字符串列表解析为属性
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public override IdFloatRow Parse(string[] items)
        {
            var name = items[PropertyIndexes[VariableNames.Id]];
            var val =  Double.Parse(items[PropertyIndexes[VariableNames.Value]]);
            return new IdFloatRow(name, val);
        } 
    }
}
