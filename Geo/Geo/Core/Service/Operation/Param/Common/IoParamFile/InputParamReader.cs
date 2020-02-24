//2015.09.30, czs, create in K385宝鸡到成都列车上, 删除
//2015.10.22, czs, edit in xi'an hongqing, 顶层输入读取器

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
    ///顶层输入读取器
    /// </summary>
    public class InputParamReader : LineFileReader<InputParam>
    {       /// <summary>
        /// 默认构造函数
        /// </summary>
        public InputParamReader() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public InputParamReader(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public InputParamReader(string filePath, Gmetadata Gmetadata)
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
            data.PropertyNames = new string[] {Geo.Utils.ObjectUtil.GetPropertyName<InputParam>(m=>m.InputPath)};
            return data;
        }

        /// <summary>
        /// 字符串列表解析为属性
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public override InputParam Parse(string[] items)
        {
            var from = items[PropertyIndexes[Geo.Utils.ObjectUtil.GetPropertyName<InputParam>(m => m.InputPath)]];
            var row = new InputParam()
            {
               InputPath = from
            }; 
            return row;
        }

    }
}
