//2017.02.03, czs,  create in hongqing, 2值文件读取


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
    /// 2值文件读取
    /// </summary>
    public class IdTwoFloatReader :  LineFileReader<IdTwoFloatRow>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public IdTwoFloatReader(string filePath, string metaFilePath = null) : base(filePath, metaFilePath)
        { 
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public IdTwoFloatReader(string filePath, Gmetadata Gmetadata)
            : base(filePath, Gmetadata)
        { 
        }

        /// <summary>
        /// 默认的元数据
        /// </summary>
        /// <returns></returns>
        public override Gmetadata GetDefaultMetadata()
        {
            return Gmetadata.DefaultTwoValueMetadata;
        }

        /// <summary>
        /// 字符串列表解析为属性
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public override IdTwoFloatRow Parse(string[] items)
        {
            var name = items[PropertyIndexes[VariableNames.Id]];
            var val =  Double.Parse(items[PropertyIndexes[VariableNames.Value]]);
            var val2 =  Double.Parse(items[PropertyIndexes[VariableNames.Value2]]);
            return new IdTwoFloatRow(name, val, val2);
        } 
    }
}
