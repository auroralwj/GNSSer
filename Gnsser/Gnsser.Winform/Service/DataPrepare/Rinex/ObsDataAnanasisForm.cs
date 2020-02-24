//2016.11.26, czs,  create in hongqing, 观测数据分析

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
    /// 观测数据分析。
    /// </summary>
    public partial class ObsDataAnanasisForm : ParalleledFileForm
    {
        Log log = new Log(typeof(ObsDataAnanasisForm));
        public ObsDataAnanasisForm()
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
        public int SelectedCount { get; set; }
        ObsDataAnalystOption ObsFileSelectOption { get; set; } 
        #endregion

        #region 方法
        protected override void Init()
        {
            base.Init();
            SelectedCount = 0;
            this.ObsFileSelectOption = GetOrInitObsFileSelectOption(); 
        }


        protected override void Run(string inputPath)
        {
            var ObsFileSelector = new ObsFileAnalyst(inputPath, ObsFileSelectOption, OutputDirectory);

            ObsFileSelector.Init();
            ObsFileSelector.Run();  

            base.Run(inputPath);
        }
  
        protected override void DetailSetting()
        { 
        }
        public ObsDataAnalystOption GetOrInitObsFileSelectOption()
        {
            if (ObsFileSelectOption == null) { return new ObsDataAnalystOption(); }
            else return ObsFileSelectOption;
        }

        protected override string BuildFinalInfo()
        {
            var infoAdd = " 选择数量：" + SelectedCount;
            return base.BuildFinalInfo() + infoAdd;
        }


        #region 事件响应 
        /// <summary>
        /// 是否停止计算
        /// </summary>
        /// <param name="runable"></param>
        protected override void SetRunable(bool runable) { base.SetRunable(runable); }//foreach (var key in CurrentRunningSolvers)  { key.IsCancel = this.IsCancel; } }
        #endregion
         
        #endregion
    }
  
}