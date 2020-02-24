//2016.10.27, czs,  create in hongqing, 更新坐标文件

using AnyInfo;
using AnyInfo.Features;
using AnyInfo.Geometries;
using Geo;
using Geo.Algorithm.Adjust;
using Geo.Coordinates;
using Geo.IO;
using Geo.Referencing;
using Geo.Times;
using Geo.Utils;
using Gnsser;
using Gnsser.Checkers;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using  System.Threading.Tasks;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Gnsser.Winform
{
    /// <summary>
    /// 更新坐标文件。
    /// </summary>
    public partial class ObsFileCoordUpdaterForm : ParalleledFileForm
    {
        Log log = new Log(typeof(ObsFileCoordUpdaterForm));
        public ObsFileCoordUpdaterForm()
        {
            InitializeComponent();
            //this.SetIsMultiObsFile(false);
            //positonResultRenderControl11.IsHasIndexParamName = true;
            this.SetExtendTabPageCount(0, 0);
            this.SetEnableMultiSysSelection(false);
             
        }

        #region 属性 
        /// <summary>
        /// 失败的数量
        /// </summary>
        public int FailedCount { get; set; }
        #endregion

        #region 方法
        protected override void DetailSetting()
        {
            //base.DetailSetting();
        }

        SiteCoordService SiteCoordService { get; set; }

        protected override void Init()
        {
            base.Init();
            FailedCount = 0;
            SiteCoordService = new SiteCoordService(this.fileOpenControl_coordFile.FilePath);
        }
         
        /// <summary>
        /// 执行单个
        /// </summary>
        /// <param name="inputPath"></param>
        protected override void Run(string inputPath)
        {
            if (this.IsCancel) { return; }

            var reader = new RinexObsFileReader(inputPath, false);
            var header = reader.GetHeader();
            var coord = SiteCoordService.Get(header.MarkerName, header.StartTime);
            if (coord == null) {
                FailedCount++;
                ShowInfo("Faild:\t"  + inputPath);
                log.Error("坐标获取失败，本文件执行取消。 " + inputPath); return;
            } 

            var outPath = Path.Combine(this.OutputDirectory, Path.GetFileName(inputPath));
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic[RinexHeaderLabel.APPROX_POSITION_XYZ] = RinexObsFileWriter.BuildApproxXyzLine(coord.Value);
            var replacer = new LineFileReplacer(inputPath, outPath, dic);
            replacer.EndMarkers.Add(RinexHeaderLabel.END_OF_HEADER);

            replacer.AddingLines.Add(RinexObsFileWriter.BuildGnsserCommentLines());
            replacer.AddingLines.Add(RinexObsFileWriter.BuildCommentLine("Approx XYZ updated with " + SiteCoordService.Name + " " + Geo.Utils.DateTimeUtil.GetFormatedDateTimeNow()));
            replacer.Run();
            log.Info("更新成功！ " + coord.Value + ", "+ inputPath);

            this.ProgressBar.PerformProcessStep(); 
        }
        protected override string BuildFinalInfo()
        {
            var infoAdd = " 失败数量：" + FailedCount;
            return base.BuildFinalInfo() + infoAdd;
        } 
  
        #region 事件响应 
        /// <summary>
        /// 是否停止计算
        /// </summary>
        /// <param name="runable"></param>
        protected override void SetRunable(bool runable) { base.SetRunable(runable); }//foreach (var key in CurrentRunningSolvers)  { key.IsCancel = this.IsCancel; } }
        #endregion

        private void IntegralGnssFileSolveForm_Load(object sender, EventArgs e)
        {
            this.fileOpenControl_coordFile.FilePath = Setting.GnsserConfig.SiteCoordFile;
        }
        #endregion
    }
}