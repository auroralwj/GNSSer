//2018.09.30, czs, create in hmx,收敛时间计算器
//2018.11.10, czs, edit in hmx, 提取单独的结果分析类

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
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
using Geo.Common;
using Gnsser.Checkers;

namespace Gnsser.Winform
{
    public partial class ConvergenceTimeCalculatorForm : ParalleledFileForm
    {
        public ConvergenceTimeCalculatorForm()
        {
            InitializeComponent();

            this.fileOpenControl_inputPathes.Filter = Setting.EpochParamFileFilter;
            namedStringControl_paramNames.SetValue("De,Dn,Du");
            namedFloatControl_maxDiffer.SetValue(0.1);
            namedIntControl_epochCount.SetValue(20);

            //SetExtendTabPageCount(0, 0);
            //   SetEnableMultiSysSelection(false);
            SetEnableDetailSettingButton(false);
        }



        #region 属性
        int SequentialEpochCount { get; set; }
        double MaxDiffer { get; set; }
        double MaxAllowedRms { get; set; }
        double MaxAllowedDifferAfterConvergence { get; set; }
        double MaxAllowedConvergenceTime { get; set; }
        int KeyLabelCharCount { get; set; }
        string ParamNamesString { get; set; }
        EpochParamAnalyzer paramAnalyzer { get; set; }
        #endregion

        BaseConcurrentDictionary<string, BaseDictionary<string, Time>> Result { get; set; }
        BaseDictionary<string, Time> StartTimes { get; set; }

        public override void PreRun()
        {
            base.PreRun();
            this.MaxAllowedRms = namedFloatControl_maxAllowedRms.GetValue();
            MaxAllowedConvergenceTime = namedFloatControl_maxAllowConvergTime.GetValue();
            MaxAllowedDifferAfterConvergence = namedFloatControl1MaxAllowedDifferAfterConvergence.GetValue();
            MaxDiffer =this.namedFloatControl_maxDiffer.GetValue();
            SequentialEpochCount= this.namedIntControl_epochCount.GetValue();
            KeyLabelCharCount = namedIntControl_labelCharCount.GetValue();
            ParamNamesString = namedStringControl_paramNames.GetValue();

            Result = new BaseConcurrentDictionary<string, BaseDictionary<string, Time>>("结果", (key) => new BaseDictionary<string, Time>());
            StartTimes = new BaseDictionary<string, Time>();

            var paramNames = ParamNamesString.Split(new char[] { ',', ';', '，', '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            paramAnalyzer = new EpochParamAnalyzer(new List<string>(paramNames),
                SequentialEpochCount,
                MaxDiffer, MaxAllowedConvergenceTime, 
                KeyLabelCharCount, MaxAllowedDifferAfterConvergence, MaxAllowedRms); 

            //foreach (var inputPath in this.TotalPathes)
            //{ 
            //    var fileName = Path.GetFileName(inputPath);
            //    var dic = Result.GetOrCreate(fileName);//单线程建立，避免冲突
            //}
        }

        protected override List<string> ParseInputPathes(string[] inputPathes)
        {
           return inputPathes.ToList();
        }
        static object locker = new object();
        /// <summary>
        /// 执行单个,判断是否取消，更新进度条。
        /// </summary>
        /// <param name="inputPath"></param>
        protected override void Run(string inputPath)
        {
            base.Run(inputPath);
            paramAnalyzer.Add(inputPath);
            log.Info("计算完成：" + inputPath);
        }

        public override void  Complete()
        {
            base.Complete();

            var table = paramAnalyzer.GetTotalFileParamConvergenceTable();
            var table2 = paramAnalyzer.GetTotalFileParamRmsTable();
            this.BindTableA(table);
            this.BindTableB(table2);

            var path = Path.Combine(this.OutputDirectory, table.Name + Geo.Setting.TextTableFileExtension); 
            ObjectTableWriter.Write(table, path);
            path = Path.Combine(this.OutputDirectory, table2.Name + Geo.Setting.TextTableFileExtension);
            ObjectTableWriter.Write(table2, path);
        }

        protected override string BuildInitPathString()
        {
            return "";
        }

        private void fileOpenControl1_FilePathSetted(object sender, EventArgs e)
        {
        }

        private void ConvergenceTimeCalculatorForm_Load(object sender, EventArgs e)
        {
            var files = Directory.GetFiles(Setting.TempDirectory, "*" + Setting.EpochParamFileExtension);
            this.SetFilePathes(files);
        }
    }

}
