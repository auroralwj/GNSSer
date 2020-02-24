//2015.10.23, czs, create in 西安五路口 凉皮店,解压的参数文件写入

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
    ///解压的参数文件写入
    /// </summary>
    public class DecompressParamWriter : LineFileWriter<DecompressParam>
    {       /// <summary>
        /// 默认构造函数
        /// </summary>
        public DecompressParamWriter() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public DecompressParamWriter(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public DecompressParamWriter(string filePath, Gmetadata Gmetadata)
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
                Geo.Utils.ObjectUtil.GetPropertyName<DecompressParam>(m=>m.InputPath),
                Geo.Utils.ObjectUtil.GetPropertyName<DecompressParam>(m=>m.OutputPath),
                Geo.Utils.ObjectUtil.GetPropertyName<DecompressParam>(m=>m.IsDeleteSource),
                Geo.Utils.ObjectUtil.GetPropertyName<DecompressParam>(m=>m.IsOverwrite)
            };
            return data;
        }
         

    }
}
