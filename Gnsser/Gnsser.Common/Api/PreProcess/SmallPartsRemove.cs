//2015.10.16, czs, create in  D5181 达州到成都东, 删除非连续观测数据

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.IO;
using Geo;
using Gnsser.Domain;

namespace Gnsser.Api
{
    /// <summary>
    /// 复制文件
    /// </summary>
    public class SmallPartsRemove : AbstractVersionedIoOperation
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SmallPartsRemove(int MinSuccessiveCount = 40)
        {
            this.MinSuccessiveCount = MinSuccessiveCount;
        }

        int MinSuccessiveCount;

        protected override void Execute(string inPath, string outPath)
        {
            //先整体探测一遍。
            Gnsser.Data.Rinex.RinexObsFileReader reader = new Data.Rinex.RinexObsFileReader(inPath);
            var oldHeader = reader.GetHeader();
            var processer = new SatConsecutiveAnalyst(oldHeader.Interval);
            foreach (var item in reader)
            {
                var obs = Domain.EpochInformation.Parse(item, item.Header.SatelliteTypes);
                processer.Revise(ref obs);
            }
            var smallParts = processer.SatSequentialPeriod.GetFilteredPeriods(MinSuccessiveCount * oldHeader.Interval, true);


            //写入到流
            Gnsser.Data.Rinex.RinexObsFileWriter writer = new Data.Rinex.RinexObsFileWriter(outPath, this.CurrentParam.OutputVersion);

            EpochInfoToRinex EpochInfoToRinex = new Domain.EpochInfoToRinex(3.02, false);

            //
            Gnsser.Data.Rinex.RinexObsFileHeader newHeader = null;
            int maxBufferEpoch = 200;
            int i = 0;
            reader.Reset();
            foreach (var item in reader)
            {
                var epochInfo = Domain.EpochInformation.Parse(item, item.Header.SatelliteTypes);
                FilterSat(smallParts, epochInfo);

                var epochObs = EpochInfoToRinex.Build(epochInfo);
                if (newHeader == null)
                {
                    newHeader = epochObs.Header;
                    writer.WriteHeader(newHeader);
                }

                writer.WriteEpochObservation(epochObs);

                // throw new Exception("需要实现 EpochInformation 向 RinexEpcohObs 的转换");

                if (i > maxBufferEpoch)
                {
                    writer.Writer.Flush();
                }
                i++;
            }
            writer.Writer.Close();


            //cycleSlipProcesser.SatPeriodInfoManager.SaveSatPeriodText(outPath + "_BreakingChart.txt");

            //File.WriteAllText(outPath + "__BreakingPeriod.txt", cycleSlipProcesser.SatPeriodInfoManager.ToFormatedString(), Encoding.UTF8);

        }

        private void FilterSat(SatPeriodInfoManager smallParts, EpochInformation epochInfo)
        {
            List<SatelliteNumber> tobeRemoved = new List<SatelliteNumber>();
            foreach (var sat in epochInfo)
            {
                //是否是断开的
                if (smallParts.Contains(sat.Prn, epochInfo.ReceiverTime))
                {
                    tobeRemoved.Add(sat.Prn);
                }
            }
            if (tobeRemoved.Count == 0) { return; }

            //移除断开的卫星,不可在上面移除，提示集合已经改变 
            this.StatedMessage = StatedMessage.Processing;
            this.StatedMessage.Message = "移除小时短卫星 " + epochInfo.ReceiverTime + " ";
            this.OnStatedMessageProduced();

            epochInfo.Remove(tobeRemoved, true, "移除小时短卫星");
        }
    }
}
