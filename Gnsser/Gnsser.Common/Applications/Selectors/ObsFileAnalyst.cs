//2016.11.25, czs & cuiyang, create  in hongqing, 观测文件分析专家

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Geo.Common;
using Geo.Coordinates;
using Geo;
using Geo.IO;
using Geo.Times;
using Gnsser.Data.Rinex;
using Gnsser.Models;
using Gnsser.Domain;

namespace Gnsser
{

    /// <summary>
    /// 观测文件分析专家
    /// </summary>
    public class ObsFileAnalyst : ObsFileEpochRunner<EpochInformation>
    {
        Log log = new Log(typeof(ObsFileAnalyst));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Option"></param>
        /// <param name="OutputDirectory"></param>
        public ObsFileAnalyst(string fileName,  ObsDataAnalystOption Option, string OutputDirectory)
        {
            this.FilePath = fileName;
            this.OutputDirectory = OutputDirectory;
            this.Option = Option;
            ObsAnalysisInfoCollection = new ObsAnalysisInfoCollection(fileName);
        }
        #region 属性
        /// <summary>
        /// 待分析的文件路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 选项
        /// </summary>
        public ObsDataAnalystOption Option { get; set; }
        /// <summary>
        /// 观测文件分析结果
        /// </summary>
        public ObsAnalysisInfoCollection ObsAnalysisInfoCollection { get; set; }
        #endregion

        public override void Process(EpochInformation epochInfo)
        {
            EpochAnlysisInfo EpochAnlysisInfo = new EpochAnlysisInfo() { Name = epochInfo.ReceiverTime.ToShortTimeString() };
            foreach (var sat in epochInfo)
            {
                var ana = new EpochSatAnlysisInfo(sat);

                EpochAnlysisInfo.Add(ana.Prn, ana);
            }

            ObsAnalysisInfoCollection.Add(epochInfo.ReceiverTime, EpochAnlysisInfo);
        }

        public override void PostRun()
        {
            base.PostRun();
            var fileName = Path.GetFileName(FilePath);
            var outFile = Path.Combine(OutputDirectory, Path.GetFileName(FilePath));

           var result = ObsAnalysisInfoCollection.BuildResult();
           result.WriteAsRinexCommentFile(Path.Combine(OutputDirectory, fileName + ".Anasis"));

           result.UpdateToRinexOFileHeader(FilePath, outFile);
          var resultFromFile =  ObsAnalysisInfo.ParseRinexCommentFile(outFile);
        }

        /// <summary>
        /// 缓存数据流
        /// </summary>
        /// <returns></returns>
        protected override BufferedStreamService<EpochInformation> BuildBufferedStream()
        {
            var DataSource = new RinexFileObsDataSource(FilePath);
            var bufferStream = new BufferedStreamService<EpochInformation>(DataSource, 20);
            return bufferStream;
        }
         
    }

 
}
