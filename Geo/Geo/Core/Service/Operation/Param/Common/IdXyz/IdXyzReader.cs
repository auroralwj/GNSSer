//2015.09.27, czs, create in xi'an hongqing, XYZ 坐标文件读取

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
    /// XYZ 坐标文件读取
    /// </summary>
    public class IdXyzReader :  LineFileReader<IdXyzRow>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public IdXyzReader(string filePath, string metaFilePath = null) : base(filePath, metaFilePath)
        { 
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public IdXyzReader(string filePath, Gmetadata Gmetadata)
            : base(filePath, Gmetadata)
        { 
        }

        /// <summary>
        /// 默认的元数据
        /// </summary>
        /// <returns></returns>
        public override Gmetadata GetDefaultMetadata()
        {
            return Gmetadata.DefaultXyzMetadata;
        }

        /// <summary>
        /// 字符串列表解析为属性
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public override IdXyzRow Parse(string [] items)
        {

            var name = items[PropertyIndexes[VariableNames.Id]];
           
            var xyz = new XYZ(
               Double.Parse( items[  PropertyIndexes[ VariableNames.X ]]),
               Double.Parse( items[  PropertyIndexes[ VariableNames.Y ]]),
               Double.Parse( items[  PropertyIndexes[ VariableNames.Z ]])
                );
           return new IdXyzRow(name, xyz);
        } 
    }
}
