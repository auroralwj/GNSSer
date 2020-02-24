//2015.10.18, czs, edit in pengzhou railway station, 连续性分析

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
    public class SatConsecutiveAnalysis : AbstractIoOperation
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SatConsecutiveAnalysis()
        {
        } 

         protected override void Execute(string inPath, string outPath)
         {
            Gnsser.Data.Rinex.RinexObsFileReader reader = new Data.Rinex.RinexObsFileReader(inPath);
            var header = reader.GetHeader();
            var processer = new SatConsecutiveAnalyst(header.Interval);

            foreach (var item in reader)
            {
                var obs = Domain.EpochInformation.Parse(item, item.Header.SatelliteTypes);
                
                processer.Revise(ref obs);
            }

            processer.SatSequentialPeriod.SaveSatPeriodText(outPath + "_SatSequentialPeriodChart.txt");
            processer.SatSequentialPeriod.Opposite.SaveSatPeriodText(outPath + "_SatSequentialPeriodChartOpposite.txt");

            File.WriteAllText(outPath + "__SatSequentialPeriod.txt", processer.SatSequentialPeriod.ToFormatedString(), Encoding.UTF8);

            File.WriteAllText(outPath + "__SatSequentialPeriodOpposite.txt", processer.SatSequentialPeriod.Opposite.ToFormatedString(), Encoding.UTF8);
         }

    }
}
