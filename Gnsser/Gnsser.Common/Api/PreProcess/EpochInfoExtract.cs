//2015.11.06, czs & cy, create in  xi'an hongqing, 历元信息提取

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
    /// 历元信息提取
    /// </summary>
    public class EpochInfoExtract : AbstractIoOperation
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EpochInfoExtract()
        {
        }

        protected override string BuildOutputFilePath(string outPath, string file)
        {
            var name = DateTime.Now.ToString("yyyy-MM-dd_HH");
            return base.BuildOutputFilePath(outPath, name + ".SiteInfo" + ".param");
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public override bool Do()
        {
            var reader = new IoParamReader(this.OperationInfo.ParamFilePath);
            foreach (var item in reader)
            {
                this.CurrentParam =  item;

                var inPath = item.InputPath;
                var outPath = item.OutputPath;

                var outFile = BuildOutputFilePath(outPath, inPath);
                    CheckOrExecute(inPath, outFile, item.IsOverwrite);
               

                var Message = "已执行 " + item.InputPath + " 到 " + item.OutputPath;
                this.OnStatedMessageProduced(StatedMessage.GetProcessed(Message));
            }
            return true;
        }



        EpochInfoWriter writer;

        protected override void Execute(string fileInPath, string fileOutPath)
        {
            Gnsser.Data.Rinex.RinexFileObsDataSource reader = new Data.Rinex.RinexFileObsDataSource(fileInPath, false);
            if (writer == null)
            {
                writer = new EpochInfoWriter(fileOutPath);
                writer.WriteCommentLine("SiteName\tReceiverTime\tTotalPrns");
            }

            foreach (var item in reader)
            {
                writer.Write(item);
            } 
        }
    }
}
