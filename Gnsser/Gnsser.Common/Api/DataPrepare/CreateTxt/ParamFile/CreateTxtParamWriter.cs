//2015.10.23, czs, create in 西安五路口 凉皮店, 创建文档参数写入器

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
    ///创建文档参数写入器
    /// </summary>
    public class CreateTxtParamWriter : LineFileWriter<CreateTxtParam>
    {       /// <summary>
        /// 默认构造函数
        /// </summary>
        public CreateTxtParamWriter() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public CreateTxtParamWriter(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public CreateTxtParamWriter(string filePath, Gmetadata Gmetadata)
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
            data.OutputFilePath = @"Data\OutputFilePath_"+DateTime.Now.Ticks+".url";
            data.PropertyNames = new string[] { 
                Geo.Utils.ObjectUtil.GetPropertyName<CreateTxtParam>(m=>m.OutputPath),
                Geo.Utils.ObjectUtil.GetPropertyName<CreateTxtParam>(m=>m.Content), 
            };
            return data;
        }

    }
}
