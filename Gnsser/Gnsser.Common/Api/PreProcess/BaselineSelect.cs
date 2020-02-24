//2015.11.05, czs & cy, create in  xi'an hongqing,基线选择

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.IO;
using Geo;
using Geo.Times;

namespace Gnsser.Api
{
    /// <summary>
    /// 基线选择
    /// </summary>
    public class BaselineSelect : AbstractIoOperation
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaselineSelect()
        {
        }

        protected override string BuildOutputFilePath(string outPath, string file)
        {
            var name = DateTime.Now.ToString("yyyy-MM-dd_HH");
            return base.BuildOutputFilePath(outPath, name + ".Baseline.param");
        }
        /// <summary>
        /// 历元卫星数据
        /// </summary>
        public class EpochSatData
        {
            public string SiteName { get; set; }

            public Dictionary<Time, IEpochInfo> Data = new Dictionary<Time, IEpochInfo>();

        }
        Dictionary<string, EpochSatData> Data = new Dictionary<string, EpochSatData>();
        protected override void Execute(string inPath, string outPath)
        {
            EpochInfoReader reader = new EpochInfoReader(inPath);
            Data = new Dictionary<string, EpochSatData>();
            EpochSatData EpochSatData; 
            foreach (var item in reader)
            { 
                var siteName = item.SiteName.ToUpper();
                if (!Data.ContainsKey(siteName))
                {
                    Data[siteName] = new EpochSatData();
                }
                Data[siteName].Data.Add(item.ReceiverTime, item); 
            }
            //比较计算



            BaselineWriter writer = new BaselineWriter(outPath);
        }

    }
}
