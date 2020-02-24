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
    public class BreakingDetect : AbstractIoOperation
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BreakingDetect()
        {
        } 

         protected override void Execute(string inPath, string outPath)
         {
            Gnsser.Data.Rinex.RinexObsFileReader reader = new Data.Rinex.RinexObsFileReader(inPath);
            var header = reader.GetHeader();
            var processer = new SatConsecutionAnalyst(40, 0 , header.Interval);

            foreach (var item in reader)
            {
                var obs = Domain.EpochInformation.Parse(item, item.Header.SatelliteTypes);
                
                processer.Revise(ref obs);
            }

            processer.SatSequentialPeriod.SaveSatPeriodText(outPath + "_BreakingChart.txt");

            File.WriteAllText(outPath + "__BreakingPeriod.txt", processer.SatSequentialPeriod.ToFormatedString(), Encoding.UTF8);
        }

    }
}
