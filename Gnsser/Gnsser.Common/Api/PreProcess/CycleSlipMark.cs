//2015.10.09, czs, create in  xi'an hongqing, 周跳探测
//2015.10.09, czs, refactor in 彭州, 重构为通用文件输入输出执行类
//2015.10.19, czs, refactor in hongqing, 周跳标记器，输出新的文件，并标记周跳。

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.IO;
using Geo;
using Gnsser.Data.Rinex;
using Gnsser.Domain;

namespace Gnsser.Api
{
    /// <summary>
    /// 复制文件
    /// </summary>
    public class CycleSlipMark :  AbstractVersionedIoOperation
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CycleSlipMark()
        {
        }

        /// <summary>
        /// 建立输出文件路径
        /// </summary>
        /// <param name="outPath"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        protected override string BuildOutputFilePath(string outPath, string file)
        {
            var outFile = Geo.Utils.FileUtil.GetOutputFilePath(outPath, file);
            return outFile;
        }

        /// <summary>
        /// 具体的执行.
        /// </summary>
        /// <param name="fileInPath"></param>
        /// <param name="fileOutPath"></param>
        protected override void Execute(string fileInPath, string fileOutPath)
        {
            Gnsser.Data.Rinex.RinexObsFileReader reader = new Data.Rinex.RinexObsFileReader(fileInPath);
            var oldHeader = reader.GetHeader();
            var processer = new SatCycleSlipAnalyst(new List<SatelliteType>() { SatelliteType.G }, oldHeader.Interval);
           
            //写入到流
            EpochInfoToRinex EpochInfoToRinex = new Domain.EpochInfoToRinex(this.CurrentParam.OutputVersion, false);
            Gnsser.Data.Rinex.RinexObsFileWriter writer = new Data.Rinex.RinexObsFileWriter(fileOutPath, this.CurrentParam.OutputVersion);
            Gnsser.Data.Rinex.RinexObsFileHeader newHeader = null; 
            int maxBufferEpoch = 200;
            int i = 0;
            foreach (var item in reader)
            {
                //标记
                var obs = Domain.EpochInformation.Parse(item, item.Header.SatelliteTypes);
                processer.Revise(ref obs);
                
                //写入文件
                var epochObs = EpochInfoToRinex.Build(obs);
                if (newHeader == null)
                {
                    newHeader = epochObs.Header;
                    writer.WriteHeader(newHeader);
                }
                writer.WriteEpochObservation(EpochInfoToRinex.Build(obs));              

                if (i > maxBufferEpoch)
                {
                    writer.Writer.Flush();
                }
            }
            writer.Writer.Close();

            processer.SatSequentialPeriod.SaveSatPeriodText(fileOutPath + "_CycleSlip.txt"); 
            File.WriteAllText(fileOutPath + "__cycleSlipPeriod.txt", processer.SatSequentialPeriod.ToFormatedString(), Encoding.UTF8);
        }
    }
}
