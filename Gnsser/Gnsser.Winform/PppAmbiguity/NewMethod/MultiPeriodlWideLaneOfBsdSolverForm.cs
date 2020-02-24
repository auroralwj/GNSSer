//2017.02.09, czs, create in hongqing, 新宽巷数据处理。Between Satellite Difference
//2017.03.09, czs, edit in hongqing, 分时段处理

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Windows.Forms;
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
using Gnsser.Checkers;

namespace Gnsser.Winform
{
    public partial class MultiPeriodlWideLaneOfBsdSolverForm : MultiFileStreamerForm
    {
        public MultiPeriodlWideLaneOfBsdSolverForm()
        {
            InitializeComponent();
             
            SetExtendTabPageCount(0,0);
            SetEnableMultiSysSelection(false);
            SetEnableDetailSettingButton(false);
        } 


        #region 属性  
        /// <summary>
        /// 是否扩展相同基准卫星的相邻时段
        /// </summary>
        public bool IsExpandPeriodOfSameSat { get { return checkBoxIsExpandPeriod.Checked; } }
        /// <summary>
        /// 时段分段数量
        /// </summary>
        public int TimePeriodCount { get { return int.Parse(this.textBox_timePeriodCount.Text); } }
        /// <summary>
        /// 是否输出整数部分
        /// </summary>
        public bool IsOutputInt { get { return (this.checkBox_outputInt.Checked); } }
        /// <summary>
        /// 是否输出小数部分
        /// </summary>
        public bool IsOutputFraction { get { return (this.checkBox_outputFraction.Checked); } }
        /// <summary>
        /// 是否输出汇总文件
        /// </summary>
        public bool IsOutputSummary { get { return (this.checkBox_outputSumery.Checked); } }
        /// <summary>
        /// 是否各时段单独输出
        /// </summary>
        public bool IsOutputInEachDirectory { get { return (this.checkBox1IsOutputInEachDirectory.Checked); } }
        #endregion                  

        protected override void DetailSetting()
        { 
        }
        /// <summary>
        /// 解析输入路径
        /// </summary>
        protected override  List<string> ParseInputPathes(string[] inputPathes)
        {
            return new List<string>(inputPathes); 
        }

        protected override void Run(string[] inputPathes)
        {
            PeriodPrnManager PeriodPrnManager = null;
            var minSat = namedIntControl_minSiteCount.GetValue();
            var minEpoch = namedIntControl_minEpoch.GetValue();
            var baseSiteEle = fileOpenControl_satEleOfBaseSite.FilePath;
            if (!File.Exists(baseSiteEle))
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("必须输入时段选星文件或基准站卫星高度角文件。");
                return;
            }

            if (baseSiteEle.Contains("SatEle.xls"))
            {
                ObjectTableReader reader = new ObjectTableReader(baseSiteEle);
                var satEleTable = reader.Read();
                log.Info("成功读取卫星高度角文件");
                PeriodFixedSatSelector BasePrnSelector = new PeriodFixedSatSelector(satEleTable,namedFloatControl1AngleCut.Value, TimePeriodCount, IsExpandPeriodOfSameSat);
                PeriodPrnManager = BasePrnSelector.Select();
            }
            else
            {
                PeriodPrnManager = PeriodPrnManager.ReadFromFile(baseSiteEle);
            }



            var smoothedMws = ObjectTableManager.Read(inputPathes);
            smoothedMws.OutputDirectory = this.OutputDirectory;

            log.Info("准备计算");
            var solver = new MultiPeriodWideLaneOfBsdSolver(smoothedMws, PeriodPrnManager, minSat, minEpoch,IsOutputInEachDirectory, OutputDirectory);
            solver.IsOutputFraction = IsOutputFraction;
            solver.IsOutputInt = IsOutputInt;
            solver.IsOutputSummary = IsOutputSummary;

            solver.Run();

        }

        private void WideLaneSolverForm_Load(object sender, EventArgs e)
        {
            fileOpenControl_inputPathes.FilePath = "";
        }
    }
}
