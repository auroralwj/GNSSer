//2017.02.09, czs, create in hongqing, 新宽巷数据处理，Between Satellite Difference
//2017.03.08, czs, create in hongqing, MW生成器，单独提出
//2018.11.06, czs, edit hmx, MwTableBuilderForm 更名为 MwFractionTableBuilderForm

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
    public partial class MwFractionTableBuilderForm : MultiFileStreamerForm
    {
        public MwFractionTableBuilderForm()
        {
            InitializeComponent();

            this.Option = GnssProcessOption.GetEphemerisSourceOption();

            //SetExtendTabPageCount(0, 0);
            //   SetEnableMultiSysSelection(false);
            //  SetEnableDetailSettingButton(false);

            baseSatSelectingControl1.SetSatelliteType(SatelliteType.G);
        }


        #region 属性
        bool IsOutputFraction { get { return checkBox1IsOutputFractionOfSmoothedMw.Checked; } }
        bool IsOutputRawMw => this.checkBox_outputRaw.Checked;
        bool IsOutputSmoothed => this.checkBox_outputSmooth.Checked;

        double AngleCut { get { return double.Parse(textBox_angleCut.Text); } }

        #endregion

        protected override void Run(string[] oPathes)
        {
            this.BasePrn = baseSatSelectingControl1.SelectedPrn;
            //if (BasePrn.SatelliteType != SatelliteType.G)
            //{
            //    Geo.Utils.FormUtil.ShowWarningMessageBox("暂只支持GPS系统");
            //    return;
            //}

            this.ProgressBar.InitProcess(oPathes.LongLength);

            //输出的采样间隔
            var isAnasysOrTable = checkBox_anasysOrTable.Checked;
            var isSmoothRange = checkBox_IsSmoothRange.Checked;

            if (isAnasysOrTable)//平均法
            {
                var builder = new SiteMwValueManagerBuilder(oPathes, AngleCut);
                builder.IsSmoothRange = isSmoothRange;
                builder.OneFileProcessed += Builder_OneFileProcessed;

                var result = builder.Build();
                ProduceAndShow(result);

            }
            else //表格法，序贯平差法
            {
                var builder = new MwTableBuilder(oPathes, AngleCut, namedIntControl_emptyRowCount.Value, this.OutputDirectory, this.SatelliteTypes, IsOutputSmoothed);
                builder.AveMaxBreakCount = namedIntControl_aveMaxBeakCount.GetValue();
                builder.AveMaxDiffer = namedFloatControl_aveMaxDiffer.GetValue();
                builder.AveMinCount = namedIntControl_aveMinCount.GetValue();

                builder.IsSmoothRange = isSmoothRange;
                builder.OneFileProcessed += Builder_OneFileProcessed;
                builder.Build();
                //输出
                log.Info("生成完毕，准备输出。");

                this.BindTableA(builder.RawMwValue.First);
                this.BindTableB(builder.MwPeriodAverage);

                //是否生成和输出平滑MW
                if (IsOutputSmoothed)
                {
                    this.BindTableB(builder.SmoothedMwValue.First);
                    this.BindTableC(builder.FractionOfSmoothedMwValue.First);

                    builder.SmoothedMwValue.WriteAllToFileAndClearBuffer(builder.SmoothedMwValue.First);
                }
                //是否输出原始数据
                if (IsOutputRawMw)
                {
                    builder.RawMwValue.WriteAllToFileAndClearBuffer(builder.RawMwValue.First);
                }

                //输出时段结果
                var periodAvePath = Path.Combine(OutputDirectory, builder.MwPeriodAverage.Name + Setting.TextGroupFileExtension);
                ObjectTableWriter.Write(builder.MwPeriodAverage, periodAvePath);

                if (IsOutputFraction)
                {
                    builder.FractionOfSmoothedMwValue.WriteAllToFileAndClearBuffer(builder.FractionOfSmoothedMwValue.First);
                }
            }
            this.ProgressBar.Full();
        }
       
        SatelliteNumber BasePrn { get; set; }

        private void ProduceAndShow(MultiSitePeriodValueStorage result)
        {
            //计算历元产品，可以查看变化。
            var basePrn = baseSatSelectingControl1.SelectedPrn;
            if (basePrn.SatelliteType != SatelliteType.G)
            {
                MessageBox.Show("请选择GPS系统！目前似乎还不支持其它系统，如果支持了请尝试高版本先，若还没有请 Email To： gnsser@163.com");
                return;
            }
            var maxRms = namedFloatControl_maxRms.GetValue();
            var outIntervalSec = namedFloatControl_intervalOFProduct.GetValue() * 60.0;

            var table = result.GetTable();
            var product = result.GetProductTableOfAllDiffer();
            var fracTable = result.GetAverageRoundFractionTable();
            var detailTable = result.GetDetailTable();
            var finalOne = result.GetFinalFcbOfBsd(BasePrn);
            var bsdProduct = result.GetWideLaneFcb(BasePrn);


            ObjectTableManager.WriteTable(table, this.OutputDirectory);
            ObjectTableManager.WriteTable(fracTable, this.OutputDirectory);
            ObjectTableManager.WriteTable(detailTable, this.OutputDirectory);
            ObjectTableManager.WriteTable(product, this.OutputDirectory);

            this.ShowInfo("Final wide fcb:\r\n" + Geo.Utils.StringUtil.ToString(finalOne));
            this.BindTableA(table);
            this.BindTableB(product);
            this.BindTableC(fracTable);

            var differInts = result.GetAllPossibleDifferInts();
            foreach (var item in differInts.GetData())
            {
                var tab = item.Value.GetDetailTable();
                //tab.Name = item.Key + "_宽巷星间单差整数";
                tab.Name = item.Key + "_Bsd.WLInt";
                ObjectTableManager.WriteTable(tab, this.OutputDirectory);
            }
            //计算归算的最终产品
            WriteFinal(bsdProduct);

            WriteEpochProducts(result, basePrn, maxRms, outIntervalSec);

            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(Setting.TempDirectory);
        }

        private static void WriteEpochProducts(MultiSitePeriodValueStorage result,  SatelliteNumber basePrn, double maxRms, double outIntervalSec)
        {
            var list1 = result.GetWideLaneFcb(basePrn, outIntervalSec, maxRms);
            WriteEpochProducts(list1, "EpochFcbOfDcb");

            var list2 = result.GetWideLaneFcbOfAllSatAverage(basePrn, outIntervalSec, maxRms);
            WriteEpochProducts(list2, "EpochFcbOfDcbOfAll");
        }

        private static void WriteEpochProducts(List<FcbOfUpd> list, string name = "EpochFcbOfDcb")
        {
            var toPath = Path.Combine(Setting.TempDirectory, name + Gnsser.Setting.FcbExtension);
            FcbOfUpdWriter writer = new FcbOfUpdWriter(toPath);

            foreach (var fcb in list)
            {
                if (fcb == null) { continue; }
                writer.Write(fcb);
            }
            writer.Dispose();
        }

        private FcbOfUpdWriter WriteFinal(FcbOfUpd bsdProduct)
        {
            //写最终FCB 产品
            var fcbPath = Path.Combine(this.OutputDirectory, "FcbProduct" + Setting.FcbExtension);
            FcbOfUpdWriter writer = new FcbOfUpdWriter(fcbPath);
            writer.Write(bsdProduct);
            writer.Dispose();
            return writer;
        }

        private void Builder_OneFileProcessed()
        {
            this.ProgressBar.PerformProcessStep();
        }

        private void fileOpenControl1_FilePathSetted(object sender, EventArgs e)
        {
            var file = fileOpenControl_inputPathes.GetFilePath("*.*o");
            if (File.Exists(file))
            {
                var oneHeader = new RinexObsFileReader(file, false).GetHeader();
            }
        }

        private void button_buidWideInts_Click(object sender, EventArgs e)
        {
            this.button_buidWideInts.Enabled = false;
            log.Info("计算开始，请稍等....");
            this.BasePrn = baseSatSelectingControl1.SelectedPrn;
            if (BasePrn.SatelliteType != SatelliteType.G)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("暂只支持GPS系统");
                return;
            }
            var start = DateTime.Now;
            var path = fileOpenControl_periodDetails.GetFilePath(); 

            var result = MultiSitePeriodValueStorage.ParseDetailTable(path);
             
            ProduceAndShow(result);
            var span = DateTime.Now - start;
            log.Info("执行完毕，总共耗时：" + span);

            this.button_buidWideInts.Enabled = true;
            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(this.OutputDirectory);
        }

        private void MwTableBuilderForm_Load(object sender, EventArgs e)
        {
            fileOpenControl_periodDetails.Filter = Setting.TextTableFileFilter;
        }

        private void namedFloatControl1_Load(object sender, EventArgs e)
        {

        }
    }
}
