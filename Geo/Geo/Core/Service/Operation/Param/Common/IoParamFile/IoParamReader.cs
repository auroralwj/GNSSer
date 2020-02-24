//2015.10.09, czs, create in  xi'an hongqing, 周跳探测
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
    ///复制的参数文件读取
    /// </summary>
    public class IoParamReader : LineFileReader<IoParam>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public IoParamReader() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public IoParamReader(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
           
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public IoParamReader(string filePath, Gmetadata Gmetadata)
            : base(filePath, Gmetadata)
        {
        }
        
        /// <summary>
        /// 字符串列表解析为属性
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public override IoParam Parse(string[] items)
        {
            var initRow = base.Parse(items);
            //只更新路径的绝对位置
            initRow.OutputPath = Geo.Utils.PathUtil.GetAbsPath(initRow.OutputPath, BaseDirectory);
            initRow.InputPath = Geo.Utils.PathUtil.GetAbsPath(initRow.InputPath, BaseDirectory);
             
            return initRow;
        }

    }
}
