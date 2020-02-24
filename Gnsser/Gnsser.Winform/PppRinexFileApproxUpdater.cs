//2019.01.17, czs, create in hmx, PPP更新头坐标，实时更新，并写入文件。

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using AnyInfo;
using AnyInfo.Features;
using AnyInfo.Geometries;
using Gnsser;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Domain;
using Gnsser.Data.Rinex;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Times;
using Gnsser.Service;
using Geo.IO;
using Geo;
using Geo.Winform;
using System.Threading;
using Geo.Draw;


namespace Gnsser.Winform
{

    /// <summary>
    /// PPP更新头坐标，实时更新，并写入文件。
    /// </summary>
    public class PppRinexFileApproxUpdater
    {
        Log log = new Log(typeof(PppRinexFileApproxUpdater));

        public PppRinexFileApproxUpdater(GnssProcessOption option, bool IsReplaceApproxCoordWhenPPP)
        {
            this.Option = option;
            this.IsReplaceApproxCoordWhenPPP = IsReplaceApproxCoordWhenPPP;
            this.ParallelConfig = new ParallelConfig();
        }
        /// <summary>
        /// PPP更新源文件坐标
        /// </summary>
        public bool IsReplaceApproxCoordWhenPPP { get; set; }
        public ObsSiteInfos ObsSiteInfos { get; set; }
        /// <summary>
        /// 定位设置
        /// </summary>
        public GnssProcessOption Option { get; set; }
        /// <summary>
        /// 并行设置
        /// </summary>
        public ParallelConfig ParallelConfig { get; set; }

        /// <summary>
        /// 进度通知接口
        /// </summary>
        public IProgressViewer ProgressViewer { get; set; }

        public void Update(List<ObsSiteInfo> filePathes)
        {
            RunPpp(filePathes);
        }

        public void Update(ObsSiteInfo filePath)
        {
            Update(new List<ObsSiteInfo>() { filePath });
        }
        #region  PPP 计算 

        public void Update(MultiPeriodObsFileManager filePathes)
        {
            Run(filePathes);
        }
        public void Run(MultiPeriodObsFileManager obsSiteInfos )
        {
            DateTime start = DateTime.Now;

            if (ProgressViewer != null) ProgressViewer.InitProcess(obsSiteInfos.Count);
            //分时段计算 ,时段之间采用串行算法
            foreach (var item in obsSiteInfos.KeyValues)
            {
                var sitebaseLines = item.Value.Values;
                var netTimePeriod = item.Key;

                RunPpp(sitebaseLines, Option, netTimePeriod); 

                ProgressViewer.PerformProcessStep();
            }
            if (ProgressViewer != null) ProgressViewer.Full();

            var span = DateTime.Now - start;

            var perSec = span.TotalSeconds / obsSiteInfos.SiteCount;

            log.Fatal("计算完毕，耗时 ： " + span.ToString() + " = " 
                + span.TotalMinutes.ToString("0.000") + " 分钟, 平均 "
                + perSec.ToString("0.000") + " 秒/个。 "
                );
        }

        /// <summary>
        /// 时段计算
        /// </summary>
        /// <param name="files"></param>
        /// <param name="Option"></param>
        /// <param name="netPeriod"></param>
        private void RunPpp(List<ObsSiteInfo> files, GnssProcessOption Option,TimePeriod netPeriod)
        {
            if (files.Count == 0) { log.Warn("没有文件！"); return; }

            //设置独立的输出目录
            var OriginalDirectory = Option.OutputDirectory;
            Option.OutputDirectory = Option.GetSolverDirectory(netPeriod);
            Geo.Utils.FileUtil.CheckOrCreateDirectory(Option.OutputDirectory);
             
            this.ObsSiteInfos = new ObsSiteInfos(files);
            var pppRunner = new PointPositionBackGroundRunner(Option, ObsSiteInfos.GetFilePathes().ToArray());
            pppRunner.ParallelConfig = ParallelConfig;
            pppRunner.ProgressViewer = ProgressViewer;
            pppRunner.Processed += PppRunner_Processed;
            pppRunner.Completed += PppRunner_Completed;
            pppRunner.Init();
            pppRunner.Run();
            
            //恢复目录
            Option.OutputDirectory = OriginalDirectory;
        }

        /// <summary>
        /// 时段计算
        /// </summary>
        /// <param name="files"></param>
        /// <param name="Option"></param>
        /// <param name="netPeriod"></param>
        private void RunPpp(List<ObsSiteInfo> files)
        {
            if (files.Count == 0) { log.Warn("没有文件！"); return; }

            //设置独立的输出目录 
            this.ObsSiteInfos = new ObsSiteInfos(files);
            var pppRunner = new PointPositionBackGroundRunner(Option, ObsSiteInfos.GetFilePathes().ToArray());
            pppRunner.ParallelConfig = ParallelConfig;
            pppRunner.ProgressViewer = ProgressViewer;
            pppRunner.Processed += PppRunner_Processed;
            pppRunner.Completed += PppRunner_Completed;
            pppRunner.Init();
            pppRunner.Run(); 
        }


        private void PppRunner_Processed(SingleSiteGnssSolveStreamer Solver)
        {
            var site = this.ObsSiteInfos.Get(Solver.Path);
            if (site == null) { return; }
            var entity = Solver.CurrentGnssResult as SingleSiteGnssResult; if (entity == null) { return; }
            var xyz = entity.EstimatedXyz;

            site.SiteObsInfo.ApproxXyz = xyz; //实时更新
            site.EstimatedSite = new EstimatedSite(site.SiteName, entity.EstRmsedXYZ, entity.MaterialObj.ReceiverTime);
          

            var temptempDir = Path.Combine(site.TempDirectory, "Temp");
            Geo.Utils.FileUtil.CheckOrCreateDirectory(temptempDir);

            var outPath = Path.Combine(temptempDir, site.SiteObsInfo.FileInfo.FileName);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic[RinexHeaderLabel.APPROX_POSITION_XYZ] = RinexObsFileWriter.BuildApproxXyzLine(xyz);
            var replacer = new LineFileReplacer(site.FilePath, outPath, dic);
            replacer.EndMarkers.Add(RinexHeaderLabel.END_OF_HEADER);
            //replacer.AddingLines.Add(RinexObsFileWriter.BuildGnsserCommentLines());
            replacer.AddingLines.Add(RinexObsFileWriter.BuildCommentLine("Approx XYZ updated with GNSSer PPP " + Geo.Utils.DateTimeUtil.GetFormatedDateTimeNow()));
            replacer.Run();

            Geo.Utils.FileUtil.MoveFile(outPath, site.TempFilePath, true);
            log.Info("更新成功！ 输出到 ： " + site.TempFilePath);

            log.Info(entity.Name + ", " + entity.ReceiverTime + "， 输出到结果文件");
            var writer = new GnssResultWriter(Solver.Option, Solver.Option.IsOutputEpochResult,
                Solver.Option.IsOutputEpochSatInfo);

            writer.WriteFinal(entity);


            //更新到对象



        }

        private void PppRunner_Completed(object sender, EventArgs e)
        {
            var site = this.ObsSiteInfos.GetFirst(); if (site == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中测站再试！"); return; }

            var temptempDir = Path.Combine(site.TempDirectory, "Temp");
            Geo.Utils.FileUtil.TryDeleteFileOrDirectory(temptempDir);
            if (IsReplaceApproxCoordWhenPPP)
            {
                var pathes = Directory.GetFiles(site.TempDirectory);
                foreach (var result in pathes)
                {
                    var original = Path.Combine(site.Directory, Path.GetFileName(result));
                    Geo.Utils.FileUtil.MoveFile(result, original, true);
                    log.Info("已替换 ： " + original);
                }
            }
        }

        #endregion
    }
}
