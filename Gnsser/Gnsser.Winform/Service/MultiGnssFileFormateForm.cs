//2016.09.26, czs,  create in hongqing, GNSS多文件网解格式化

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
    /// GNSS多文件网解格式化
    /// </summary>
    public partial class MultiGnssFileFormateForm : MultiFileStreamerForm
    {
        Log log = new Log(typeof(IntegralGnssFileSolveForm));
        public MultiGnssFileFormateForm()
        {
            InitializeComponent();
            //this.SetIsMultiObsFile(false);
            //positonResultRenderControl11.IsHasIndexParamName = true;
            this.SetExtendTabPageCount(0, 0);

            MultiObsFileFormatStreamer = new MultiObsFileFormatStreamer();
            MultiObsFileFormatStreamer.EpochEntityProduced += MultiObsFileFormatStreamer_EpochEntityProduced;
            MultiObsFileFormatStreamer.InfoProduced += MultiObsFileFormatStreamer_InfoProduced;
            MultiObsFileFormatStreamer.Completed += MultiObsFileFormatStreamer_Completed;
        }

        #region 属性
        static int _resultCount = 0;
        DateTime startTime;
        MultiObsFileFormatStreamer MultiObsFileFormatStreamer;
        #endregion

        #region 方法
        protected override void OnOptionChanged(GnssProcessOption Option)
        {
            base.OnOptionChanged(Option);
            this.rinexObsFileFormatTypeControl1.CurrentdType = Option.RinexObsFileFormatType;
        }

        protected override void Run(string[] inputPathes)
        { 
            MultiObsFileFormatStreamer.Option = this.Option;
            MultiObsFileFormatStreamer.Init(inputPathes);
            this.Option = CheckOrBuildGnssOption();

            this.ProgressBar.InitProcess(MultiObsFileFormatStreamer.DataSource.BaseDataSource.ObsInfo.Count);

            MultiObsFileFormatStreamer.Run();

            this.ProgressBar.Full();
        }



        protected override GnssProcessOption CheckOrBuildGnssOption()
        {
            var type = (this.rinexObsFileFormatTypeControl1.CurrentdType);

            return CheckOrBuildGnssOption(type);
        }

        #region 事件响应
        void MultiObsFileFormatStreamer_InfoProduced(string info) { ShowNotice(info); }

        void MultiObsFileFormatStreamer_EpochEntityProduced(MultiSiteEpochInfo entity) { this.ProgressBar.PerformProcessStep();  }


        void MultiObsFileFormatStreamer_Completed(object sender, EventArgs e)
        {
            ShowInfo("执行完毕！");
        }

        /// <summary>
        /// 是否停止计算
        /// </summary>
        /// <param name="runable"></param>
        protected override void SetRunable(bool runable) { base.SetRunable(runable); if(MultiObsFileFormatStreamer !=null) MultiObsFileFormatStreamer.IsCancel = this.IsCancel; }
        #endregion

        private void MultiGnssFileFormateForm_Load(object sender, EventArgs e)
        {
            this.rinexObsFileFormatTypeControl1.CurrentdType = RinexObsFileFormatType.多站单历元;
        }
        #endregion
    }
}