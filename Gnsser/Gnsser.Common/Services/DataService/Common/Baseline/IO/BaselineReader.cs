//2015.11.05, czs & cy, create in  xi'an hongqing, Baseline 输入输出写入器

using System;
using System.IO;
using Geo.Common;
using Geo.Coordinates;
using System.Collections;
using System.Collections.Generic;
using Geo.IO;
using Geo;

namespace Gnsser
{ 

    /// <summary>
    ///Baseline文件读取
    /// </summary>
    public class BaselineReader : LineFileReader<Baseline>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaselineReader() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public BaselineReader(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
           
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public BaselineReader(string filePath, Gmetadata Gmetadata)
            : base(filePath, Gmetadata)
        {
        }
         

    }
}
