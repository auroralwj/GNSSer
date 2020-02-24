//2015.10.23, czs, create in 西安五路口 凉皮店, 追加参数到Gnsser参数文件

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
    public class AppendStringToLineParamWriter : LineFileWriter<AppendStringToLineParam>
    {       /// <summary>
        /// 默认构造函数
        /// </summary>
        public AppendStringToLineParamWriter() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public AppendStringToLineParamWriter(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public AppendStringToLineParamWriter(string filePath, Gmetadata Gmetadata)
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
                Geo.Utils.ObjectUtil.GetPropertyName<AppendStringToLineParam>(m => m.FileToAppdend),
                Geo.Utils.ObjectUtil.GetPropertyName<AppendStringToLineParam>(m => m.Content), 
            };
            return data;
        }

    }
}
