//2015.09.29, czs, create in xi'an hongqing,  FTP 下载
//2016.11.28, czs, edit in hongqing, 修改，实现FTP目录下载

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
    ///FTP 下载
    /// </summary>
    public class FtpParamReader : LineFileReader<FtpParam>
    {       /// <summary>
        /// 默认构造函数
        /// </summary>
        public FtpParamReader() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public FtpParamReader(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public FtpParamReader(string filePath, Gmetadata Gmetadata)
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

        /// <summary>
        /// 字符串列表解析为属性
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public override FtpParam Parse(string[] items)
        {
            bool isFull = items.Length == PropertyIndexes.Count;

            var url = items[PropertyIndexes[Geo.Utils.ObjectUtil.GetPropertyName<FtpParam>(m => m.InputPath)]];
            var row = new FtpParam()
            {
                InputPath = url
            };

            var item = GetPropertyValue(items, Geo.Utils.ObjectUtil.GetPropertyName<FtpParam>(m => m.OutputPath));
            if (!String.IsNullOrWhiteSpace(item))
            {
                row.OutputPath = Geo.Utils.PathUtil.GetAbsPath(item, BaseDirectory);  
            }
            else if (PreviousObject != null)
            {
                row.OutputPath = PreviousObject.OutputPath; ;
            }
            item = GetPropertyValue(items, Geo.Utils.ObjectUtil.GetPropertyName<FtpParam>(m => m.Extension));
            if (!String.IsNullOrWhiteSpace(item))
            {
                row.Extension = item;
            }
            else if (PreviousObject != null)
            {
                row.Extension = PreviousObject.Extension; ;
            }
            item = GetPropertyValue(items, Geo.Utils.ObjectUtil.GetPropertyName<FtpParam>(m => m.UserName));
            if (!String.IsNullOrWhiteSpace(item))
            {
                row.UserName = item;
            }
            else if (PreviousObject != null)
            {
                row.UserName = PreviousObject.UserName; ;
            }
            item = GetPropertyValue(items, Geo.Utils.ObjectUtil.GetPropertyName<FtpParam>(m => m.Password));
            if (!String.IsNullOrWhiteSpace(item))
            {
                row.Password = item;
            }
            else if (PreviousObject != null)
            {
                row.Password = PreviousObject.Password; ;
            }
             
            PreviousObject = row;

            return row;
        }

    }
}
