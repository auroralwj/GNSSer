//2015.10.22, czs, create in xi'an hongqing, 顶层输入写入器

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
    ///顶层输入写入器
    /// </summary>
    public class InputParamWriter : LineFileWriter<InputParam>
    {       /// <summary>
        /// 默认构造函数
        /// </summary>
        public InputParamWriter() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public InputParamWriter(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public InputParamWriter(string filePath, Gmetadata Gmetadata)
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
            data.PropertyNames = new string[] {Geo.Utils.ObjectUtil.GetPropertyName<InputParam>(m=>m.InputPath)};
            return data;
        } 

    }
}
