//2015.10.22, czs, create in  xi'an hongqing, 输入输出写入器
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
    ///输入输出写入器
    /// </summary>
    public class IoParamWriter : LineFileWriter<IoParam>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public IoParamWriter() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public IoParamWriter(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public IoParamWriter(string filePath, Gmetadata Gmetadata)
            : base(filePath, Gmetadata)
        {
        }

        public override void Write(IoParam obj)
        {
            obj.OutputPath = Geo.Utils.PathUtil.GetRelativePath(obj.OutputPath, this.BaseDirectory);
            obj.InputPath = Geo.Utils.PathUtil.GetRelativePath(obj.InputPath, this.BaseDirectory);
            base.Write(obj);
        }
    }
}
