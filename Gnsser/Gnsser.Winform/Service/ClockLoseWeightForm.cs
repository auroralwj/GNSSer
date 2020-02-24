//2018.11.18, czs, create in hmx, 钟差减肥瘦身

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
    /// 钟差减肥瘦身。
    /// </summary>
    public partial class ClockLoseWeightForm : ParalleledFileForm
    {
        Log log = new Log(typeof(ClockLoseWeightForm));
        public ClockLoseWeightForm()
        {
            InitializeComponent();
            this.SetExtendTabPageCount(0, 0);
            this.SetEnableMultiSysSelection(false);
            this.SetEnableDetailSettingButton(false);
        }

        #region 属性

        #endregion

        #region 方法
         

        protected override void Run(string inputPath)
        {
            base.Run(inputPath);
             
            var clockFile = ClockFileReader.ReadFile(inputPath, false);
            clockFile.RemoveSiteColcks();

            var outPath = Path.Combine(OutputDirectory, Path.GetFileName(inputPath));
            ClockFileWriter.WriteFile(outPath, clockFile); 
        }

        #endregion

        private void ClockLoseWeightForm_Load(object sender, EventArgs e)
        {
            RunningFileExtension =  "*.clk_30s;*.clk;*.clk_05";
            InputFileFilter = Setting.RinexClkFileFilter;
        }

        /// <summary>
        /// 构建初始路径
        /// </summary>
        /// <returns></returns>
        protected override string BuildInitPathString()
        {
            if (Setting.GnsserConfig != null)
            {
                return Setting.GnsserConfig.SampleClkFile;
            }
            return "";
        }

    }
}