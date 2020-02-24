//2015.10.09, czs, create in  xi'an hongqing, 周跳探测

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
    /// 复制文件
    /// </summary>
    public class CycleSlipDetect : AbstractIoOperation
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CycleSlipDetect()
        {
        }
        

        protected override void Execute(string fileInPath, string fileOutPath)
        {
            Gnsser.Data.Rinex.RinexObsFileReader reader = new Data.Rinex.RinexObsFileReader(fileInPath);
            var header = reader.GetHeader();
            var Analyst = new SatCycleSlipAnalyst(new List<SatelliteType>() { SatelliteType.G, SatelliteType.C, SatelliteType.R }, header.Interval);

            foreach (var item in reader)
            {
                var obs = Domain.EpochInformation.Parse(item, item.Header.SatelliteTypes);
                
                Analyst.Revise(ref obs);
            } 

            Analyst.SatSequentialPeriod.SaveSatPeriodText(fileOutPath + "_cycleSlipChart.txt");

            File.WriteAllText(fileOutPath + "__cycleSlipPeriod.txt", Analyst.SatSequentialPeriod.ToFormatedString(), Encoding.UTF8);
  
        }
    }
}
