//2016.12.17, czs, create in  xi'an hongqing, Rinex 格式转换

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
    ///  Rinex 格式转换
    /// </summary>
    public class RinexConvert : AbstractVersionedIoOperation
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RinexConvert()
        {
            this.InputFileExtension = "*.Z;*.??O";
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
