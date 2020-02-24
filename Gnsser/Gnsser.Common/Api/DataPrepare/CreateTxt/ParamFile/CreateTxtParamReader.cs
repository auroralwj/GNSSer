//2015.10.02, czs, create in 彭州, URL生成器
//2015.10.06, czs, edit in 彭州到成都动车C6186, 时间段字符串生成器
//2015.10.07, czs, edit in 安康到西安临客K8182, 星历地址生成器

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
    ///复制的参数文件读取
    /// </summary>
    public class CreateTxtParamReader : LineFileReader<CreateTxtParam>
    {       /// <summary>
        /// 默认构造函数
        /// </summary>
        public CreateTxtParamReader() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public CreateTxtParamReader(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public CreateTxtParamReader(string filePath, Gmetadata Gmetadata)
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


        /// <summary>
        /// 字符串列表解析为属性
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public override CreateTxtParam Parse(string[] items)
        {
            bool isFull = items.Length == PropertyIndexes.Count;

            var pattern = items[PropertyIndexes[Geo.Utils.ObjectUtil.GetPropertyName<CreateTxtParam>(m => m.OutputPath)]];
            var row = new CreateTxtParam()
            {
                 OutputPath = pattern
            }; 
             
            
            var item = GetPropertyValue(items, Geo.Utils.ObjectUtil.GetPropertyName<CreateTxtParam>(m => m.Content));
            if (item == null && PreviousObject != null)
            {
                row.Content = PreviousObject.Content;
            }else{
                row.Content = (item);
            }
             

            if (isFull)
            {
                PreviousObject = row;
            }
            return row;
        }

    }
}
