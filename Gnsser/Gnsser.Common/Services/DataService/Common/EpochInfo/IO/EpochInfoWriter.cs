//2015.11.06, czs & cy, create in  xi'an hongqing, EpochInfo 输入输出写入器

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
    ///EpochInfo写入器
    /// </summary>
    public class EpochInfoWriter : LineFileWriter<IEpochInfo>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public EpochInfoWriter() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public EpochInfoWriter(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public EpochInfoWriter(string filePath, Gmetadata Gmetadata)
            : base(filePath, Gmetadata)
        {
        }
        protected override void SetProperties()
        {
            this.EntityType = typeof(EpochInfo);
            Properties = new List<System.Reflection.PropertyInfo>();

            var PropertyNames = Metadata.PropertyNames;


            //采用默认序列
            if (PropertyNames.Length == 0)
            {
                PropertyNames = Activator.CreateInstance<EpochInfo>().OrderedProperties.ToArray();
            }

            StringBuilder sb = new StringBuilder();
            foreach (var item in PropertyNames)
            {
                Properties.Add(EntityType.GetProperty(item.Trim()));
            }
        }

        public override void Write(IEpochInfo obj)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(obj.SiteName);
            sb.Append(this.ItemSpliter);
            sb.Append(obj.ReceiverTime.ToLinePropertyString());
            sb.Append(this.ItemSpliter);
            foreach (var item in obj.TotalPrns)
            {
                sb.Append(item.ToString());
            }
            StreamWriter.WriteLine(sb.ToString());

            CurrentIndex++;
        }
    }
}
