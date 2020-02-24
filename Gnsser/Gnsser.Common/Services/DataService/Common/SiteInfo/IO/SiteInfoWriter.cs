//2015.11.05, czs & cy, create in  xi'an hongqing, SiteInfo 输入输出写入器

using System;
using System.IO;
using Geo.Common;
using Geo.Coordinates;
using System.Collections;
using System.Collections.Generic;
using Geo.IO;
using Geo;
using System.Text;

namespace Gnsser
{
    /// <summary>
    ///SiteInfo写入器
    /// </summary>
    public class SiteInfoWriter : LineFileWriter<SiteInfo>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SiteInfoWriter() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public SiteInfoWriter(string filePath, Encoding Encoding = null, string metaFilePath = null)
            : base(filePath, metaFilePath, Encoding)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public SiteInfoWriter(string filePath, Gmetadata Gmetadata, Encoding Encoding = null)
            : base(filePath, Gmetadata, Encoding)
        {
        }

        public override void Write(SiteInfo obj)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(obj.SiteName);
            sb.Append(this.ItemSpliter);
            sb.Append(obj.EstimatedXyz.X);
            sb.Append(this.ItemSpliter);
            sb.Append(obj.EstimatedXyz.Y);
            sb.Append(this.ItemSpliter);
            sb.Append(obj.EstimatedXyz.Z);
            sb.Append(this.ItemSpliter);
            sb.Append(obj.EstimatedXyzRms.X);
            sb.Append(this.ItemSpliter);
            sb.Append(obj.EstimatedXyzRms.Y);
            sb.Append(this.ItemSpliter);
            sb.Append(obj.EstimatedXyzRms.Z); 
             
            StreamWriter.WriteLine(sb.ToString());

            CurrentIndex++;
        }
    }
}
