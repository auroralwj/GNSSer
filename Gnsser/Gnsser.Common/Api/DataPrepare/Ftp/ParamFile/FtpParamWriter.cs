//2015.10.23, czs, create in 西安五路口 凉皮店,FTP 下载参数
//2016.11.28, czs, edit in hongqing, 通用 FtpParam 参数


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
    ///FTP 下载参数
    /// </summary>
    public class FtpParamWriter : LineFileWriter<FtpParam>
    {       /// <summary>
        /// 默认构造函数
        /// </summary>
        public FtpParamWriter() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public FtpParamWriter(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public FtpParamWriter(string filePath, Gmetadata Gmetadata)
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
                Geo.Utils.ObjectUtil.GetPropertyName<FtpParam>(m => m.InputPath),
                Geo.Utils.ObjectUtil.GetPropertyName<FtpParam>(m => m.OutputPath),
                Geo.Utils.ObjectUtil.GetPropertyName<FtpParam>(m => m.IsOverwrite), 
                Geo.Utils.ObjectUtil.GetPropertyName<FtpParam>(m => m.Extension), 
                VariableNames.UserName, 
                VariableNames.Password };
            return data;
        }
      

    }
}
