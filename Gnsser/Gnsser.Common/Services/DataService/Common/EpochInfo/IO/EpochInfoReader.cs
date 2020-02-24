//2015.11.06, czs & cy, create in  xi'an hongqing, EpochInfo 输入输出写入器

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
    ///EpochInfo文件读取
    /// </summary>
    public class EpochInfoReader : LineFileReader<IEpochInfo>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public EpochInfoReader() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePath"></param>
        public EpochInfoReader(string filePath, string metaFilePath = null)
            : base(filePath, metaFilePath)
        {
           
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        public EpochInfoReader(string filePath, Gmetadata Gmetadata)
            : base(filePath, Gmetadata)
        {
        }
        protected override  void Init(Gmetadata Gmetadata)
        {
            this.ItemSpliters = Gmetadata.ItemSplliter;

            PropertyIndexes = new Dictionary<string, int>();
            int i = 0;
            foreach (var name in this.Metadata.PropertyNames)
            {
                PropertyIndexes.Add(name.Trim(), i++);
            }
            //采用默认序列
            if (PropertyIndexes.Count == 0)
            {
                var defaultObj = Activator.CreateInstance<EpochInfo>();// typeof(TLineClass);

                var names = defaultObj.OrderedProperties;
                this.Metadata.PropertyNames = names.ToArray();
                i = 0;
                foreach (var name in this.Metadata.PropertyNames)
                {
                    PropertyIndexes.Add(name.Trim(), i++);
                }
            }


            InitStreamReader();
        }
        /// <summary>
        /// 字符串列表解析为属性
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public override IEpochInfo Parse(string[] items)
        {
            EpochInfo row = new EpochInfo();
            int i = 0;
            row.SiteName = items[i++];
            row.ReceiverTime = Geo.Times.Time.ParseLinePropertyString(items[i++]);

            var prnStirng = items[i++];
            row.TotalPrns = SatelliteNumber.ParsePRNs(prnStirng);
            //更新上一个
            this.PreviousObject = row;

            return row;
        }

    }
}
