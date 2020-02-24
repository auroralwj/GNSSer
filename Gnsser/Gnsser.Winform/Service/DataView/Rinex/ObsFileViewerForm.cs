//2014.06.24, czs, add， 增加RINEX3.0的显示
//2015.05.11, czs, add in namu, 增加RINEX数据瘦身、导出功能，各版本数据在同一个数据表中显示
//2017.09.29, czs, edit in hongqing, 重构，修改完善分析功能
//2018.04.27, czs, edit in hmx, 增加rnx观测文件的支持
//2018.05.20, czs, edti in hmx, 增加平滑伪距的支持

using Gnsser.Times;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Geo.Utils;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo.Coordinates; 
using Geo.Times;
using Geo;
using Gnsser.Domain;
using Geo.Draw;
using Gnsser.Data;

namespace Gnsser.Winform
{
    /// <summary>
    /// 观测文件查看器
    /// </summary>
    public partial class ObsFileViewerForm : Form, Gnsser.Winform.IShowLayer
    {
        public ObsFileViewerForm()
        {
            InitializeComponent();
        }
        #region 属性
        public event ShowLayerHandler ShowLayer;
        /// <summary>
        /// 观测文件
        /// </summary>
        public Data.Rinex.RinexObsFile ObsFile { get; set; }
        public string ObsPath { get { return fileOpenControl1.FilePath; } set { fileOpenControl1.FilePath = value; } }

        RinexObsFileReader obsFileReader;
        /// <summary>
        /// 当前表
        /// </summary>
        ObjectTableStorage TableObjectStorage { get; set; }
        #endregion

        private void button_read_Click(object sender, EventArgs e)
        {
            if (!File.Exists(ObsPath))
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("文件不存在！");
                return;
            }
            ReadFile();
            button_view_Click(sender, e);
        }

        private void button_slim_Click(object sender, EventArgs e)
        {
            CheckAndReadObsFile();

            double per = double.Parse(this.textBox_maxPercentage.Text);
            string info = null;// ObsFileTypeManager.RemoveObserversInfo(ref obsFile, per * 0.01);

            ObsFileProcesserChain chain = new ObsFileProcesserChain();
            var process = new ObsFileCodeFilterProcesser() { MaxPercentage = per };
            chain.AddProcessor(process);
            var oFile = ObsFile;
            chain.Revise(ref oFile);

            info = process.Message;

            MessageBox.Show("移除了 ：" + info);
            button_view_Click(sender, e);
        }

        /// <summary>
        /// 数据绑定
        /// </summary>
        /// <param name="table"></param>
        private void BindDataSource(ObjectTableStorage table)
        {
            this.objectTableControl1.DataBind(table);
        }
        private void ObsFileViewerForm_Load(object sender, EventArgs e) { this.ObsPath = Setting.GnsserConfig.SampleOFileA; }

        #region 查看
        private void button_view_Click(object sender, EventArgs e)
        {
            CheckAndReadObsFile();
            var isShowOnly = checkBox_show1Only.Checked;
            SatelliteNumber prn = (SatelliteNumber)this.bindingSource_sat.Current;
            ObjectTableStorage table = BuildObjectTable(prn, isShowOnly);
            BindDataSource(table);
        }
        private void button_viewPeriodOnMap_Click(object sender, EventArgs e)
        {
            CheckAndReadObsFile();
            if (ShowLayer != null && obsFileReader != null)
            {
                obsFileReader.Reset();

                SatConsecutiveAnalyst processer = null;
                foreach (var item in ObsFile)
                {
                    if (processer == null) { processer = new SatConsecutiveAnalyst(item.Header.Interval); }
                    var obs = Domain.EpochInformation.Parse(item, item.Header.SatelliteTypes);
                    processer.Revise(ref obs);
                }
                TimePeriodToLayerBuilder builder = new TimePeriodToLayerBuilder(processer.SatSequentialPeriod);
                var layer = builder.Build();
                ShowLayer(layer);
            }
        }
        private void button_viewObs_Click(object sender, EventArgs e)
        {
            CheckAndReadObsFile();
            var type = this.satObsDataTypeControl1.CurrentdType;

            ObjectTableStorage table = BuildObjectTable(type);
            BindDataSource(table);
        }


        private void button＿viewAll_Click(object sender, EventArgs e)
        {
            CheckAndReadObsFile();

            var table = new ObsFileToTableBuilder().Build(this.ObsFile);
            BindDataSource(table);
        }
        #endregion

        #region 绘图
        private void button_drawTableView_Click(object sender, EventArgs e)
        {
            //var tables = objectTableControl1.DataGridView.DataSource as DataTable;
            //if (tables == null) { return; }
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }
            var chartForm = new Geo.Winform.CommonChartForm(TableObjectStorage, System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point);
            chartForm.Text = "" + this.bindingSource_sat.Current;
            chartForm.Show();
        }

        private void buttonViewOnChart_Click(object sender, EventArgs e)
        {
            bool isDrawAllPhase = checkBox1ViewAllPhase.Checked;
            ObjectTableStorage table = BuildObjectTable(isDrawAllPhase);
            if (checkBox_sortPrn.Checked) { table.ParamNames.Sort(); }

            EpochChartForm form = new EpochChartForm(table);
            form.Show();
        }
        private void button_selectDraw_Click(object sender, EventArgs e)
        {
            var indexName = "Epoch";
            Geo.Utils.DataGridViewUtil.SelectColsAndDraw(objectTableControl1.DataGridView, indexName, this.bindingSource_sat.Current + "");
        }
        #endregion

        #region 数据表的构建
        /// <summary>
        /// 针对某一个卫星绘制
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        private ObjectTableStorage BuildObjectTable(SatelliteNumber prn, bool show1Only)
        {
            List<TimedRinexSatObsData> records = ObsFile.GetEpochTimedObservations(prn);
            ObjectTableStorage table = new ObjectTableStorage();
            foreach (var record in records)
            {
                table.NewRow();
                table.AddItem("Epoch", record.Time);
                foreach (var item in record.SatObsData)
                {
                    if (show1Only && !item.Key.Contains("1"))
                    {
                        continue;
                    }
                    table.AddItem(item.Key, item.Value.Value);
                }
            }
            return table;
        }





        /// <summary>
        /// 构建数据表，绘制相位数据
        /// </summary>
        /// <param name="isDrawAllPhase"></param>
        /// <returns></returns>
        private ObjectTableStorage BuildObjectTable(bool isDrawAllPhase)
        {
            var ObsDataSource = new RinexFileObsDataSource(this.ObsPath);
            ObjectTableStorage table = new ObjectTableStorage();
            foreach (var epochInfo in ObsDataSource)
            {
                table.NewRow();
                //加下划线，确保排序为第一个
                table.AddItem("_Epoch", epochInfo.ReceiverTime);
                foreach (var sat in epochInfo)
                {
                    if (isDrawAllPhase)
                    {
                        foreach (var phase in sat.Data)
                        {
                            var val = phase.Value.PhaseRange.RawPhaseValue;
                            if (Geo.Utils.DoubleUtil.IsValid(val) || val == 0)
                            {
                                table.AddItem(sat.Prn + "_" + phase.Key, val);
                            }
                        }
                    }
                    else if (Geo.Utils.DoubleUtil.IsValid(sat.FirstAvailable.PhaseRange.RawPhaseValue))
                    {
                        table.AddItem(sat.Prn, sat.FirstAvailable.PhaseRange.RawPhaseValue);
                    }
                }
                table.EndRow();
            }
            return table;
        }
        /// <summary>
        /// 构建数据表格。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private ObjectTableStorage BuildObjectTable(SatObsDataType type)
        {
            var EpochInfoBuilder = new RinexEpochInfoBuilder(ObsFile.Header.SatelliteTypes);
            ObjectTableStorage table = new ObjectTableStorage();
            foreach (var obs in ObsFile)
            {
                var epochInfo = EpochInfoBuilder.Build(obs);
                table.NewRow();
                table.AddItem("Epoch", epochInfo.ReceiverTime.ToShortTimeString());
                foreach (var sat in epochInfo)
                {
                    table.AddItem(sat.Prn + "", sat[type].Value + "");
                }
            }
            table.EndRow();
            return table;
        }
        #endregion

        #region 数据读取
        /// <summary>
        /// 读取数据
        /// </summary>
        private void ReadFile()
        { 
            ObsFile = ObsFileUtil.ReadFile(ObsPath);

            if (ObsFile == null)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("不支持输入文件格式！");
                return;
            }            

            this.bindingSource_obsInfo.DataSource = ObsFile;
            var prns = ObsFile.GetPrns();
            prns.Sort();
            this.bindingSource_sat.DataSource = prns;

            this.attributeBox1.DataGridView.DataSource = Geo.Utils.ObjectUtil.GetAttributes(ObsFile.Header, false);

            string msg = "";
            msg += "首次观测时间：" + ObsFile.Header.StartTime + "\r\n";
            msg += "最后观测时间：" + ObsFile.Header.EndTime + "\r\n";
            msg += "采样间隔：" + ObsFile.Header.Interval + " 秒" + "\r\n";

            this.textBox_show.Text = msg;
            

            Data.Rinex.RinexObsFile ObsFile1 = ObsFileUtil.ReadFile(ObsPath);
            var observations = new MemoRinexFileObsDataSource(ObsFile1);
            foreach (var epochInfo in observations)
            {
                int a = 1;
                int b = 2;
            }
        }
        /// <summary>
        /// 检查，如果为null，则读取数据
        /// </summary>
        private void CheckAndReadObsFile()
        {
            if (ObsFile == null) { ReadFile(); }
        }
        #endregion

        #region 表格操作分析
        #region 输出

        private void button_outputOneTable_Click(object sender, EventArgs e)
        {
            CheckAndReadObsFile();
            var table = new ObsFileToTableBuilder().Build(this.ObsFile);
            table.Name = System.IO.Path.GetFileNameWithoutExtension(this.ObsPath);
            ObjectTableManager mgr = new ObjectTableManager(Setting.GnsserConfig.TempDirectory);
            mgr.Add(table);
            mgr.WriteAllToFileAndClearBuffer();
            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(mgr.OutputDirectory);
        }
        private void button_toRinexV3_Click(object sender, EventArgs e)
        {
            CheckAndReadObsFile();
            FormUtil.ShowFormSaveTextFileAndIfOpenFolder(new RinexObsFileWriter().GetRinexString(ObsFile, 3.02),
                System.IO.Path.GetFileName(ObsPath), "O文件|**.O|所有格式|*.*");
        }

        private void button_toRinexV2_Click(object sender, EventArgs e)
        {
            CheckAndReadObsFile();
            FormUtil.ShowFormSaveTextFileAndIfOpenFolder(new RinexObsFileWriter().GetRinexString(ObsFile, 2.11),
                System.IO.Path.GetFileName(ObsPath), "O文件|**.O|所有格式|*.*");
        }
        private void button_toExcel_Click(object sender, EventArgs e) { Geo.Utils.ReportUtil.SaveToExcel(this.objectTableControl1.DataGridView); }
        #endregion

        #endregion

        private void button_exportEpochLine_Click(object sender, EventArgs e)
        {
            var path = Path.Combine(Setting.TempDirectory, "EpochLine.txt");
            using (StreamWriter writer = new StreamWriter(path))
            {
                foreach (var item in this.ObsFile)
                {
                    writer.WriteLine(item.ToLineString());
                }
            }
            Geo.Utils.FileUtil.OpenDirectory(Setting.TempDirectory);
        }

        private void button_smoothRange_Click(object sender, EventArgs e)
        {
            CheckAndReadObsFile();
            bool isDualFreIonFree = checkBox_ionoFree.Checked;

            int smoothWindow = this.namedIntControl_smoothWindow.GetValue();
            SatelliteNumber prn = (SatelliteNumber)this.bindingSource_sat.Current;
            var PhaseSmoothRangeBulider = new NamedCarrierSmoothedRangeBuilderManager(checkBox_isApproved.Checked,  smoothWindow, true, IonoDifferCorrectionType.DualFreqCarrier);

            List<TimedRinexSatObsData> records = ObsFile.GetEpochTimedObservations(prn);
            if (isDualFreIonFree)
            {
                if (ObsFile.Header.ObsInfo.GetFrequenceCount() <= 1)
                {
                    Geo.Utils.FormUtil.ShowWarningMessageBox("只有一个频率，无法实现无电离层组合！");
                    return;
                }
                
                DualFreqPhaseSmoothRange(prn, PhaseSmoothRangeBulider);
            }
            else
            {
                SingleFreqPhaseSmoothRange(prn, PhaseSmoothRangeBulider, records);
            } 
        }
         
        /// <summary>
        /// 单频平滑伪距
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="PhaseSmoothRangeBulider"></param>
        /// <param name="records"></param>
        private void DualFreqPhaseSmoothRange(SatelliteNumber prn, NamedCarrierSmoothedRangeBuilderManager PhaseSmoothRangeBulider)
        {
            ObjectTableStorage table = new ObjectTableStorage(); 
            MemoRinexFileObsDataSource observations = new MemoRinexFileObsDataSource(this.ObsFile);
            CycleSlipDetectReviser cycleSlipDetectReviser = new CycleSlipDetectReviser(); 

            while (observations.MoveNext())
            {  
                var current = observations.Current; 
                cycleSlipDetectReviser.Revise(ref current);

                if (current.Contains(prn))
                {
                    table.NewRow();
                    table.AddItem("Epoch", current.ReceiverTime);

                    var epochSat = current[prn];

                    var smootherP1 = PhaseSmoothRangeBulider.GetOrCreate("P");

                    var PS = smootherP1
                        .SetReset(epochSat.IsUnstable)
                        .SetRawValue ( current.ReceiverTime,
                        epochSat.Combinations.IonoFreeRange.Value,
                        epochSat.Combinations.IonoFreePhaseRange.Value,
                        0
                        )
                        .Build().Value;

                    table.AddItem("P1", epochSat.FrequenceA.PseudoRange.Value);
                    table.AddItem("P2", epochSat.FrequenceB.PseudoRange.Value);
                    table.AddItem("PS", PS); 

                } 
            } 

            BindDataSource(table);
        }


        /// <summary>
        /// 单频平滑伪距
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="PhaseSmoothRangeBulider"></param>
        /// <param name="records"></param>
        private void SingleFreqPhaseSmoothRange(SatelliteNumber prn, NamedCarrierSmoothedRangeBuilderManager PhaseSmoothRangeBulider, List<TimedRinexSatObsData> records)
        {
            ObjectTableStorage table = new ObjectTableStorage();
            foreach (var record in records)
            {
                table.NewRow();
                table.AddItem("Epoch", record.Time);
                var data = record.SatObsData;

                var waveLenL1 = Frequence.GetFrequence(prn.SatelliteType, 1).WaveLength;
                var waveLenL2 = Frequence.GetFrequence(prn.SatelliteType, 2).WaveLength;


                double L1 = data.PhaseA.Value * waveLenL1;
                double P1 = data.RangeA.Value;
                double L2 = data.PhaseB != null ? record.SatObsData.PhaseB.Value * waveLenL2 : 0;
                double P2 = data.RangeB != null ? record.SatObsData.RangeB.Value : 0;


                var smootherP1 = PhaseSmoothRangeBulider.GetOrCreate("P1");
                var smootherP2 = PhaseSmoothRangeBulider.GetOrCreate("P2");

                var P1s = smootherP1
                    .SetReset(data.PhaseA.IsLossLock)
                    .SetRawValue(record.Time, P1, L1, 0 )
                    .Build().Value;
                table.AddItem("P1", P1);
                table.AddItem("P1S", P1s);

                if (data.PhaseB != null)
                {
                    var P2s = smootherP2
                        .SetReset(data.PhaseB.IsLossLock)
                        .SetRawValue(record.Time, P2, L2, 0)
                        .Build().Value;
                    table.AddItem("P2", P2);
                    //table.AddItem("P1S_old", P1s);
                    table.AddItem("P2S", P2s);
                }
            }

            BindDataSource(table);
        }
    }
}