//2015.11.05, czs & cy, create in  xi'an hongqing, SiteInfo 输入输出写入器

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
    ///SiteInfo文件读取
    /// </summary>
    public class SiteInfoReader : LineFileReader<SiteInfo>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SiteInfoReader() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public SiteInfoReader(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
           
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public SiteInfoReader(string filePath, Gmetadata Gmetadata)
            : base(filePath, Gmetadata)
        {
        }
      
        /// <summary>
        /// 字符串列表解析为属性
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public override SiteInfo Parse(string[] items)
        {
            SiteInfo row = new SiteInfo();
            int i = 0;
            row.SiteName = items[i++];
            List<string> list = new List<string>(items);
            list.RemoveAt(0);

            row.EstimatedXyz = XYZ.Parse(list.GetRange(0, 3).ToArray());

            if (items.Length > 3)
            {
               row.EstimatedXyzRms = XYZ.Parse(list.GetRange(3, 3).ToArray());  
            }

            //更新上一个
            this.PreviousObject = row;

            return row;
        }

    }
}
