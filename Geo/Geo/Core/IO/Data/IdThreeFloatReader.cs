//2017.02.03, czs,  create in hongqing, 3 值文件读取


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
    /// 3 值文件读取
    /// </summary>
    public class IdThreeFloatReader :  LineFileReader<IdThreeFloatRow>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public IdThreeFloatReader(string filePath, string metaFilePath = null) : base(filePath, metaFilePath)
        { 
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public IdThreeFloatReader(string filePath, Gmetadata Gmetadata)
            : base(filePath, Gmetadata)
        { 
        }

        /// <summary>
        /// 默认的元数据
        /// </summary>
        /// <returns></returns>
        public override Gmetadata GetDefaultMetadata()
        {
            return Gmetadata.DefaultThreeValueMetadata;
        }

        /// <summary>
        /// 字符串列表解析为属性
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public override IdThreeFloatRow Parse(string[] items)
        {
            var name = items[PropertyIndexes[VariableNames.Id]];
            var val = Double.Parse(items[PropertyIndexes[VariableNames.Value]]);
            var val2 = Double.Parse(items[PropertyIndexes[VariableNames.Value2]]);
            var val3 = Double.Parse(items[PropertyIndexes[VariableNames.Value3]]);
            return new IdThreeFloatRow(name, val, val2, val3);
        } 
    }
}
