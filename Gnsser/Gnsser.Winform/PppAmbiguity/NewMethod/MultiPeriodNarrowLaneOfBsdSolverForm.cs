//2017.02.06, czs, create in hongqing, FCB 计算器


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
    public partial class MultiPeriodNarrowLaneOfBsdSolverForm : MultiFileStreamerForm
    {
        public MultiPeriodNarrowLaneOfBsdSolverForm()
        {
            InitializeComponent();
            SetExtendTabPageCount(2, 4);
            SetEnableMultiSysSelection(false);
            this.SetEnableDetailSettingButton(false);
        }


        #region 属性
        /// <summary>
        /// 是否各时段单独输出
        /// </summary>
        public bool IsOutputInEachDirectory { get { return (this.checkBox1IsOutputInEachDirectory.Checked); } }

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
        NarrowLaneOfBsdSolver solver { get; set; }

        #endregion


        protected override void BackgroudDoWork()
        {
            string[] inputPathes = this.fileOpenControl_inputPathes.GetFilePathes("*_Params.xls");
            TotalPathes = new List<string>();
            TotalPathes.AddRange(inputPathes);
            log.Info("解析获取了 " + inputPathes.Length + " 个文件");
            Run(inputPathes);
        }

        protected override void Run(string[] pppResultPathes)
        {
            //读取宽巷的整数解
            int startCountToRemove = this.namedIntControl_removeCountOfEachSegment.GetValue();
            double maxAllowedDiffer = this.namedFloatControl1maxAllowedDiffer.GetValue();
            bool isLoopAllSats = checkBox_calculateAllSat.Checked;
            double maxRmsTimes = namedFloatControl_maxRMSTImes.GetValue();
            int minSiteCount = this.namedIntControl1MinSiteCount.GetValue();
            double maxRms = namedFloatControl_maxRms.GetValue();
            var widelaneIntPathes = fileOpenControl_intWideLaneFiles.GetFilePathes("*_IntOfWL.xls");
            MultiPeriodNarrowLaneOfBsdSolver solver = null;
            if (widelaneIntPathes.Length > 0 && widelaneIntPathes[0].ToLower().Contains("_intofwl.xls"))
            {
                solver = new MultiPeriodNarrowLaneOfBsdSolver(widelaneIntPathes, pppResultPathes, startCountToRemove, IsOutputInEachDirectory, this.OutputDirectory);
            }
            else
            {
                var pathes = fileOpenControl_intWideLaneFiles.FilePathes;

                var windeLaneIntObjs = MultiSitePeriodValueStorage.ParsePrnTables(pathes);
                foreach (var item in windeLaneIntObjs)
                {
                    item.RemoveRmsGreaterThan(maxRms);
                }
                solver = new MultiPeriodNarrowLaneOfBsdSolver(windeLaneIntObjs, pppResultPathes, startCountToRemove, IsOutputInEachDirectory, this.OutputDirectory);
            }
            solver.MaxRms = maxRms;
            solver.MaxRmsTimes = maxRmsTimes;
            solver.MinSiteCount = minSiteCount;
            solver.IsLoopAllSats = isLoopAllSats;
            solver.MaxAllowedDiffer = maxAllowedDiffer;
            solver.IsOutputFraction = IsOutputFraction;
            solver.IsOutputInt = IsOutputInt;
            solver.IsOutputSummary = IsOutputSummary;
            solver.Run();
        }


        private void NarrowLaneOfBsdSolverForm_Load(object sender, EventArgs e)
        {
            fileOpenControl_inputPathes.FilePath = "";
            fileOpenControl_fcbPathes.Filter = Setting.FcbFileFilter;
        }

        private void button_combine_Click(object sender, EventArgs e)
        {
            var fcbPathes = fileOpenControl_fcbPathes.FilePathes;
            var basePrn = this.baseSatSelectingControl1.SelectedPrn;

            log.Info("即将转换FCB文件基准到 " + basePrn);
            //文件读取
            Dictionary<string, FcbOfUpdFile> data = new Dictionary<string, FcbOfUpdFile>();
            FcbOfUpdFile baseFile = null;
            foreach (var path in fcbPathes)
            {
                var file = new FcbOfUpdReader(path).ReadToFile();
                if(file.Count == 0) { continue; }

                var target = file.ToFile(basePrn);//基准转换到统一的卫星
                if(target ==null || target.Count == 0) { continue; }

                var fileName = Path.GetFileName(path);
                data[fileName] = target;
                if (baseFile == null) { baseFile = target; }
            }
            log.Info("文件读取完毕，数量： " + fcbPathes.Length);

            //汇集
            log.Info("即将汇集各文件到一个对象，然后方便进行平均，这个过程比较耗时。。。 ");
            EpochSatSiteValueList valList = new EpochSatSiteValueList("多基准FCB合成");
            foreach (var epochProduct in baseFile)
            {
                var epoch = epochProduct.Epoch;
                valList.GetOrCreate(epoch).GetOrCreate(basePrn).Add("BasePrn", RmsedNumeral.Zero); //基准卫星和数据不用计算。

                foreach (var kv in data)
                {
                   var epochVal =  kv.Value.Get(epoch);
                    if(epochVal == null) { continue; }

                    foreach (var item in epochVal.Data.KeyValues)
                    {
                        if(item.Key == basePrn)
                        {
                            continue;
                        }

                        if (RmsedNumeral.IsValid(item.Value))
                        {
                            valList.GetOrCreate(epoch).GetOrCreate(item.Key).Add(kv.Key,item.Value);
                        }
                    }
                }
            }
            log.Info("各文件汇集转换完成，即将求平均 ");

            //求平均
            var ave = valList.GetAverage(0, 3);
             
            //生成窄巷FCB 产品，并写入文件
            var FcbOfUpds = ave.GetFcbProduct(basePrn);

            //写入文件  //写入文件
           var outpath =  FcbOfUpdWriter.WriteEpochProducts(FcbOfUpds, basePrn + "_CombiedEpochNLFcbOfDcb");
             
            log.Info("执行完毕！ ");

            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(Path.GetDirectoryName(outpath));


            
        }
    }


}
