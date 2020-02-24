//2015.09.27, czs, create in xi'an hongqing, XYZ 坐标文件读取
//2016.02.10, czs, create in hongqing, XY 坐标文件读取


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
    /// XY 坐标文件读取
    /// </summary>
    public class IdXyReader :  LineFileReader<IdXyRow>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public IdXyReader(string filePath, string metaFilePath = null) : base(filePath, metaFilePath)
        { 
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public IdXyReader(string filePath, Gmetadata Gmetadata)
            : base(filePath, Gmetadata)
        { 
        }

        /// <summary>
        /// 默认的元数据
        /// </summary>
        /// <returns></returns>
        public override Gmetadata GetDefaultMetadata()
        {
            return Gmetadata.DefaultXyMetadata;
        }

        /// <summary>
        /// 字符串列表解析为属性
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public override IdXyRow Parse(string [] items)
        {
            var id = items[PropertyIndexes[VariableNames.Id]];
            var xyz = new XY(
               Double.Parse( items[  PropertyIndexes[ VariableNames.X ]]),
               Double.Parse( items[  PropertyIndexes[ VariableNames.Y ]]) 
                );
           return new IdXyRow(id, xyz);
        } 
    }
}
    