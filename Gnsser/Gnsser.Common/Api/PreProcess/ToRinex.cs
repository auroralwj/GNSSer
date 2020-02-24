//2015.10.09, czs, create in  xi'an hongqing, 格式转换

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.IO;
using Geo;

namespace Gnsser.Api
{
    /// <summary>
    /// 复制文件
    /// </summary>
    public class ToRinex : AbstractVersionedIoOperation
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ToRinex()
        {
            this.InputFileExtension = "*.Z";
            this.WorkFileExtension = Setting.RinexOFileFilter;
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="inPath">输入文件路径</param>
        /// <param name="outPath">输出文件路径</param> 
        protected override void Execute(string inPath, string outPath)
        {
            if (!File.Exists(inPath))
            {
                log.Error("输入路径不存在！ " + inPath);
                return;
            }
            Gnsser.Data.Rinex.ObsFileConverter.ToRinex(inPath, outPath, this.CurrentParam.OutputVersion);
        }
    }
}
