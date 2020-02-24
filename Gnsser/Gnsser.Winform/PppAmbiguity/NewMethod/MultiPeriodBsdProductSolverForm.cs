//2017.02.09, czs, create in hongqing,  BSD 自动计算器。Between Satellite Difference
//2017.03.24, czs, edit in hongqing, 改造为多历元同时计算

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
    public partial class MultiPeriodBsdProductSolverForm : MultiFileStreamerForm
    {
        public MultiPeriodBsdProductSolverForm()
        {
            InitializeComponent();
             
            SetExtendTabPageCount(0,0);
          //  SetEnableMultiSysSelection(false);
            //SetEnableDetailSettingButton(false);
             

            this.Option = GnssProcessOption.GetEphemerisSourceOption(); 
        } 


        #region 属性
        /// <summary>
        /// 是否扩展相同卫星的时段。
        /// </summary>
        public bool IsExpandPeriodOfSameSat { get { return checkBoxIsExpandPeriod.Checked; } set { checkBoxIsExpandPeriod.Checked = value; } }

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
        double AngleCut { get { return double.Parse(textBox_angleCut.Text); } }
        #endregion                  

        protected override void DetailSetting()
        { 
        }         

        protected override void Run(string[] obsPathes)
        {
           // var satEleDir = fileOpenControl_satElevation.FilePath;// @"D:\Data\2013[甘肃CORS数据]\1103\GpsOnly\GPS卫星高度角\";
            var pppResults = fileOpenControl_pppResults.GetFilePathes("_Params" + Setting.TextTableFileExtension);// Directory.GetFiles(satEleDir);
            var angleCut = double.Parse( textBox_angleCut.Text);
            var abmiResultInCycle = PppTableResultFileReader.ReadPppAmbiResultInCycle(pppResults);

            var SatElevatoinTableBuilder = new SatElevatoinTableBuilder(obsPathes, SatelliteTypes, Interval);
            var satEleTables = SatElevatoinTableBuilder.Build();
            var mw = new MwTableBuilder(obsPathes, AngleCut, namedIntControl_emptyRowCount.Value, OutputDirectory, this.SatelliteTypes).Build();


            PeriodPrnManager PeriodPrnManager = BuildPeriodBasePrn(satEleTables);

            MultiPeriodBsdProductSolver solver = new MultiPeriodBsdProductSolver(abmiResultInCycle, mw, PeriodPrnManager);
            solver.MaxAllowedDiffer = namedFloatControl1maxAllowedDiffer.Value;
            solver.IsOutputFraction = IsOutputFraction;
            solver.IsOutputInt = IsOutputInt;
            solver.IsOutputSummary = IsOutputSummary;
            solver.Run();
            
            //var solver = new BsdAmbiSolver(obsPathes, pppResults, this.SatelliteTypes[0], angleCut, namedIntControl_minSiteCount.Value, this.namedIntControl_minEpoch.Value, OutputDirectory);

            //solver.Run();
        }

        /// <summary>
        /// 构建基准星时段对象。
        /// </summary>
        /// <param name="satEleTables"></param>
        /// <returns></returns>
        private PeriodPrnManager BuildPeriodBasePrn(ObjectTableManager satEleTables)
        {
            PeriodPrnManager PeriodPrnManager = null;
            var minSat = namedIntControl_minSiteCount.GetValue();
            var minEpoch = namedIntControl_minEpoch.GetValue();
            var baseSiteEle = fileOpenControl_satEleOfBaseSite.FilePath;

            if (baseSiteEle.Contains("SatEle.xls"))
            {
                ObjectTableReader reader = new ObjectTableReader(baseSiteEle);
                var satEleTable = reader.Read();
                log.Info("成功读取卫星高度角文件");
                PeriodFixedSatSelector BasePrnSelector = new PeriodFixedSatSelector(satEleTable, namedFloatControl1AngleCut.Value, TimePeriodCount, IsExpandPeriodOfSameSat);
                PeriodPrnManager = BasePrnSelector.Select();
            }
            else if (baseSiteEle.Contains(".xls"))
            {
                PeriodPrnManager = PeriodPrnManager.ReadFromFile(baseSiteEle);
            }
            else
            {
                PeriodPrnManager = new PeriodFixedSatSelector(satEleTables.First, namedFloatControl1AngleCut.Value, TimePeriodCount, IsExpandPeriodOfSameSat).Select();
            }
            return PeriodPrnManager;
        }


        private void WideLaneSolverForm_Load(object sender, EventArgs e)
        {
          
        }

        private void fileOpenControl1_FilePathSetted(object sender, EventArgs e)
        {
            var file = fileOpenControl_inputPathes.GetFilePath("*.*O");
            if (File.Exists(file))
            {
                var oneHeader = new RinexObsFileReader(file, false).GetHeader();
                this.textBox_interval.Text = oneHeader.Interval + "";
            }
        }

        public double Interval { get { return int.Parse(this.textBox_interval.Text); } }

        private void fileOpenControl_nav_FilePathSetted(object sender, EventArgs e)
        {
            //星历检查
            string navFile = fileOpenControl_nav.GetFilePath();
            if (File.Exists(navFile))
            {
                this.Option.EphemerisFilePath = navFile;
                this.Option.IsIndicatingEphemerisFile = true;
            }
        }
    }
}
