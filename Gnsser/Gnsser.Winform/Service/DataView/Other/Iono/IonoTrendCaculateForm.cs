//2018.07.06, czs, create in HMX, 站星电离层趋势计算


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
using Gnsser.Checkers;
using Geo;
using Geo.IO;
using Gnsser.Domain;
using Geo.Draw;
using Geo.Winform;
using Gnsser.Data;
using Gnsser.Correction;
using System.Threading.Tasks;
using Geo.Algorithm; 

namespace Gnsser.Winform
{
    /// <summary>
    /// 站星电离层趋势计算
    /// </summary>
    public partial class IonoTrendCaculateForm : Form, Gnsser.Winform.IShowLayer
    {
        public Log log = new Log(typeof(RangeSmootherForm));
        public event ShowLayerHandler ShowLayer;
        /// <summary>
        /// 观测文件
        /// </summary>
        public Data.Rinex.RinexObsFile ObsFile { get; set; }

        public string ObsPath { get { return fileOpenControl1.FilePath; } set { fileOpenControl1.FilePath = value; } }

        public IonoTrendCaculateForm()
        {
            InitializeComponent();

            enumRadioControl_fitType.Init<PolyfitType>();
        }
        RinexObsFileReader obsFileReader;

        GlobalNavEphemerisService EphemerisService = GlobalNavEphemerisService.Instance;
        /// <summary>
        /// 载波相位平滑伪距
        /// </summary>
        public CarrierSmoothedRangeBuilderManager SmoothedRangeBuilderManagerP1 { get; set; }

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


        private void button_view_Click(object sender, EventArgs e)
        {
            ViewRinex();
        }
        private void ViewRinex()
        {
            CheckAndReadObsFile();
            SatelliteNumber prn = (SatelliteNumber)this.bindingSource_sat.Current;
            ObjectTableStorage table = BuildObjectTable(prn);
            BindDataSource(table);
        }

        private void ObsFileViewerForm_Load(object sender, EventArgs e)
        {
            enumRadioControl1.Init<SmoothRangeSuperpositionType>(true);
            enumRadioControl_ionDifferType.Init<IonoDifferCorrectionType>();
            fileOpenControl_multiPath.Filter = Setting.RinexOFileFilter;
            fileOpenControl_multiPath.FilePath = Setting.GnsserConfig.SampleOFileA;
            this.ObsPath = Setting.GnsserConfig.SampleOFileA;
            directorySelectionControlOutDir.Path = Setting.TempDirectory;
        }

        private void button_viewObs_Click(object sender, EventArgs e)
        {
            if (ObsFile == null) { MessageBox.Show("请先读取数据！"); return; }

            EnableRunButton(false);

            double cutOff = namedFloatControl_satCutoff.GetValue();
            int smoothWindow = this.namedIntControl_smoothWindow.GetValue();
            var EpochInfoBuilder = new RinexEpochInfoBuilder(ObsFile.Header);

            var _obsDataSource = new RinexFileObsDataSource(ObsPath);

            SmoothedRangeBuilderManagerP1 = BuildPhaseSmoothRangeBulider();


            ObjectTableManager txtManager = new ObjectTableManager(10000000, OutDirectory);
            ObjectTableStorage table = txtManager.GetOrCreate(System.IO.Path.GetFileNameWithoutExtension(ObsPath) + "_Data");// new TableTextStorage();

            var option = new GnssProcessOption();
            option.VertAngleCut = this.namedFloatControl_satCutoff.GetValue();
            var context = DataSourceContext.LoadDefault(option, _obsDataSource);

            var bufferedStream = new BufferedStreamService<EpochInformation>(_obsDataSource, option.BufferSize);

            //var EphemerisEndTime = context.EphemerisService.TimePeriod.BufferedEnd;
            SatTimeInfoManager SatTimeInfoManager = new Gnsser.SatTimeInfoManager(_obsDataSource.ObsInfo.Interval);
            //var Reviser = EpochInfoReviseManager.GetDefaultCycleSlipDetectReviser(context, option);

            var Reviser = new BufferPolyRangeSmoothReviser(option);
            //.GetDefaultEpochInfoReviser(context, option, SatTimeInfoManager);
            var checker = EpochCheckingManager.GetDefaultCheckers(context, option);
            int i = -1;

            progressBarComponent1.InitProcess(ObsFile.Count);
            foreach (var item in bufferedStream)
            {
                i++;

                progressBarComponent1.PerformProcessStep();

                //原始数据检核
                var epochInfo = item;
                if (!checker.Check(item))
                {
                    continue;
                }
                //数据矫正
                Reviser.Buffers = bufferedStream.MaterialBuffers;
                bool result = Reviser.Revise(ref epochInfo);
                if (!result)
                {
                    continue;
                }

                table.NewRow();
                table.AddItem("Epoch", epochInfo.ReceiverTime.ToShortTimeString());
                //计算伪距平滑值
                foreach (var sat in epochInfo)
                {
                    //观测值，或组合值
                    var rangeVal = sat.FrequenceA.PseudoRange.Value;// sat[type].CorrectedValue;
                    var phaseVal = sat.FrequenceA.PhaseRange.Value;// sat[option.PhaseTypeToSmoothRange].CorrectedValue;
                    var manager = SmoothedRangeBuilderManagerP1.GetOrCreate(sat.Prn);
                    var smoothRangeVal = manager.SetRawValue(sat.ReceiverTime, rangeVal, phaseVal, 0)
                        .SetReset(sat.IsUnstable).Build().Value;

                    table.AddItem(sat.Prn + "_P1_Raw", rangeVal);
                    table.AddItem(sat.Prn + "_P1_PolySmooth", sat.FrequenceA.PseudoRange.CorrectedValue);
                    table.AddItem(sat.Prn + "_P1_PhaseSmooth", smoothRangeVal);
                }

                i++;
            }
            progressBarComponent1.Full();

            table.EndRow();

            this.objectTableControl1.DataBind(table);
            txtManager.WriteAllToFileAndCloseStream();


            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(OutDirectory);


            EnableRunButton(true);
        }

        public string OutDirectory { get => directorySelectionControlOutDir.Path; }
        NamedCarrierSmoothedRangeBuilderManager PhaseSmoothRangeBulider;
        private void button_smoothCurrent_Click(object sender, EventArgs e)
        {
            EnableRunButton(false);

            CheckAndReadObsFile();
            DateTime start = DateTime.Now;
            ObjectTableManager tableObjectStorages;
            ObjectTableStorage IonoFitTable;


            var outDir = directorySelectionControlOutDir.Path;
            int bufferSize = namedIntControl_bufferCount.GetValue();
            bool isShowPoly = checkBox_showPoly.Checked;
            double cutOff = namedFloatControl_satCutoff.GetValue();
            bool isShowL1Only = checkBox_isShowL1Only.Checked;

            PhaseSmoothRangeBulider = BuildPhaseSmoothRangeBulider();
            int smoothWindow = this.namedIntControl_smoothWindow.GetValue();

            SatelliteNumber prn = (SatelliteNumber)this.bindingSource_sat.Current;

            tableObjectStorages = new ObjectTableManager(outDir);
            IonoFitTable = tableObjectStorages.AddTable("IonoFit");
            GlobalIgsGridIonoService ionoService = GlobalIgsGridIonoService.Instance;
            GlobalIgsGridIonoDcbService ionoDcbService = GlobalIgsGridIonoDcbService.Instance;
            var TimedSmoothValueBuilderManager = new TimedSmoothValueBuilderManager(smoothWindow);

            List<TimedRinexSatObsData> records = ObsFile.GetEpochTimedObservations(prn);
            ObjectTableStorage table = new ObjectTableStorage(prn + "_平滑");

            var firtTime = records[0].Time;

            double P1_P2sat = ionoDcbService.GetDcb(firtTime, prn).Value * GnssConst.MeterPerNano;

            double P1_P2recMeter = ionoDcbService.GetDcbMeterForP1(firtTime, Path.GetFileName(ObsPath).Substring(0, 4).ToLower());
            var siteXyz = ObsFile.Header.ApproxXyz;

            var epochSatBuilder = new RinexEpochSatBuilder();
            progressBarComponent1.InitProcess(records.Count);

            BufferedStreamService<TimedRinexSatObsData> bufferStream = new BufferedStreamService<TimedRinexSatObsData>(records, bufferSize);
            bufferStream.MaterialInputted += PhaseSmoothBufferStream_MaterialInputted;
            foreach (var record in bufferStream)
            {
                progressBarComponent1.PerformProcessStep();

                var time = record.Time;
                var data = record.SatObsData;


                var eph = EphemerisService.Get(prn, time);
                if (eph == null) { continue; }

                var satXyz = eph.XYZ;

                var polar = Geo.Coordinates.CoordTransformer.XyzToGeoPolar(satXyz, siteXyz, AngleUnit.Degree);
                if (polar.Elevation < cutOff)
                {
                    continue;
                }
                var waveLenL1 = Frequence.GetFrequenceA(prn, time).WaveLength;
                var waveLenL2 = Frequence.GetFrequenceB(prn, time).WaveLength;

                double L1 = data.PhaseA.Value * waveLenL1;
                double P1 = data.RangeA.Value;
                double L2 = data.PhaseB != null ? record.SatObsData.PhaseB.Value * waveLenL2 : 0;
                double P2 = data.RangeB != null ? record.SatObsData.RangeB.Value : 0;

                //扩展
                var sat = epochSatBuilder.SetPrn(record.SatObsData.Prn).SetTime(time).Build(record.SatObsData);

                #region  载波相位平滑伪距
                var smootherP1 = PhaseSmoothRangeBulider.GetOrCreate("P1");
                var smootherP2 = PhaseSmoothRangeBulider.GetOrCreate("P2");

                var P1s = smootherP1
                    .SetReset(data.PhaseA.IsLossLock)
                    .SetRawValue(record.Time, P1, L1, sat.IonoLenOfL1ByDifferL1L2)
                    .Build().Value;
                IonoFitTable.NewRow();
                IonoFitTable.AddItem("Epoch", time);
                IonoFitTable.AddItem("FittedIonoAndAmbiValue", smootherP1.CurrentRaw.FittedIonoAndAmbiValue);
                //var test = TestSmoothRangeBuilder.GetOrCreate("Test")
                //    .SetReset(data.PhaseA.IsLossLock)
                //    .SetRawValue(record.Time, P1, L1)
                //    .Build().Value;

                //var differ = test - P1s;
                //int iii = 0;

                var P2s = 0.0;
                if (!isShowL1Only) P2s = smootherP2
                   .SetReset(data.PhaseB.IsLossLock)
                   .SetRawValue(record.Time, P2, L2, sat.IonoLenOfL2ByDifferL1L2)
                   .Build().Value;
                #endregion

                //二次多项式平滑比较
                var lsSP1Smoother = TimedSmoothValueBuilderManager.GetOrCreate("P1");
                var lsSP2Smoother = TimedSmoothValueBuilderManager.GetOrCreate("P2");
                double lsSP1 = 0, lsSP2 = 0;
                if (isShowPoly)
                {
                    foreach (var item in bufferStream.MaterialBuffers)
                    {
                        lsSP1Smoother.SetRawValue(item.Time, item.SatObsData.RangeA.Value);
                        lsSP2Smoother.SetRawValue(item.Time, item.SatObsData.RangeB.Value);
                    }

                    if (data.PhaseA.IsLossLock)
                    {
                        lsSP1 = P1;
                        lsSP1Smoother.SetReset(data.PhaseA.IsLossLock);
                    }
                    else
                    {
                        lsSP1 = lsSP1Smoother.SetReset(data.PhaseA.IsLossLock)
                         .SetRawValue(time, P1)
                         .SetSmoothTime(time)
                         .Build();
                    }
                    if (data.PhaseB.IsLossLock)
                    {
                        lsSP2 = P2;
                        lsSP2Smoother.SetReset(data.PhaseB.IsLossLock);
                    }
                    else
                    {
                        lsSP2 = lsSP2Smoother.SetReset(data.PhaseB.IsLossLock)
                         .SetRawValue(time, P2)
                         .SetSmoothTime(time)
                         .Build();
                    }
                }


                table.NewRow();
                table.AddItem("Epoch", record.Time);

                //table.AddItem("L1", L1);
                //table.AddItem("L2", L2);
                table.AddItem("P1", P1);
                if (!isShowL1Only) table.AddItem("P2", P2);
                table.AddItem("P1S", P1s);
                if (!isShowL1Only && isShowPoly) table.AddItem("P2S", P2s);

                if (isShowPoly) table.AddItem("LsP1S", lsSP1);
                if (!isShowL1Only) table.AddItem("LsP2S", lsSP2);

            }
            progressBarComponent1.Full();

            BindDataSource(table);

            EnableRunButton(true);

            Geo.Winform.TableObjectViewForm form = new Geo.Winform.TableObjectViewForm(IonoFitTable);
            form.Show();

            // tableObjectStorages.WriteAllToFileAndClearBuffer();

            var span = DateTime.Now - start;// = DateTime.Now;
            log.Info("计算完毕，耗时 ： " + span);
        }
        public SatelliteNumber CurrentPrn { get { return this.bindingSource_sat.Current == null ? SatelliteNumber.Default : (SatelliteNumber)this.bindingSource_sat.Current; } }
        private void PhaseSmoothBufferStream_MaterialInputted(TimedRinexSatObsData record)
        {
            SatelliteNumber prn = CurrentPrn;
            var smootherP1 = PhaseSmoothRangeBulider.GetOrCreate("P1");

            var time = record.Time;
            var data = record.SatObsData;

            var waveLenL1 = Frequence.GetFrequence(prn.SatelliteType, 1).WaveLength;
            var waveLenL2 = Frequence.GetFrequence(prn.SatelliteType, 2).WaveLength;

            double L1 = data.PhaseA.Value * waveLenL1;
            double P1 = data.RangeA.Value;

            smootherP1.SetBufferValue(record.Time, P1, L1);
        }

        private CarrierSmoothedRangeBuilderManager BuildPhaseSmoothRangeBulider()
        {
            var isApproved = checkBox_isApproved.Checked;
            int smoothWindow = this.namedIntControl_smoothWindow.GetValue();
            bool isWeighted = checkBox_isWeighted.Checked;
            int deltaIonoOrder = this.namedIntControl_deltaIonoOrder.GetValue();
            var type = enumRadioControl1.GetCurrent<SmoothRangeSuperpositionType>();
            int bufferSize = namedIntControl_bufferCount.GetValue();
            int ionoFitEpochCount = namedIntControl1_ionoFitEpochCount.GetValue();
            var Ionotype = enumRadioControl_ionDifferType.GetCurrent<IonoDifferCorrectionType>();
            return new CarrierSmoothedRangeBuilderManager(isApproved, smoothWindow, isWeighted, Ionotype, deltaIonoOrder, bufferSize, ionoFitEpochCount, type);
        }

        private void EnableRunButton(bool trueOrFalse)
        {
            this.Invoke(new Action(() =>
            {
                this.button_smoothCurrent.Enabled = trueOrFalse;
                this.button_MultiRun.Enabled = trueOrFalse;
                this.button_solveAllRate.Enabled = trueOrFalse;
                this.button_ionoDifferByLL.Enabled = trueOrFalse;
                this.button_p_L_div2.Enabled = trueOrFalse;

            }));
        }



        /// <summary>
        /// 检查，如果为null，则读取数据
        /// </summary>
        private void CheckAndReadObsFile()
        {
            if (ObsFile == null) { ReadFile(); }
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        private void ReadFile()
        {
            string lastChar = Geo.Utils.StringUtil.GetLastChar(ObsPath);
            string lastChar3 = Geo.Utils.StringUtil.GetLastChar(ObsPath, 3);
            string lastChar5 = Geo.Utils.StringUtil.GetLastChar(ObsPath, 5);
            if (String.Equals(lastChar, "o", StringComparison.CurrentCultureIgnoreCase) || String.Equals(lastChar3, "rnx", StringComparison.CurrentCultureIgnoreCase))
            {
                obsFileReader = new RinexObsFileReader(ObsPath);
                ObsFile = obsFileReader.ReadObsFile();
            }

            if (String.Equals(lastChar, "z", StringComparison.CurrentCultureIgnoreCase)
                || String.Equals(lastChar3, "crx", StringComparison.CurrentCultureIgnoreCase)
                || String.Equals(lastChar5, "crx.gz", StringComparison.CurrentCultureIgnoreCase)
                )
            {
                Geo.IO.InputFileManager inputFileManager = new Geo.IO.InputFileManager(OutDirectory);
                this.ObsPath = inputFileManager.GetLocalFilePath(ObsPath, "*.*o;*.rnx", "*.*");
                obsFileReader = new RinexObsFileReader(ObsPath);
                ObsFile = obsFileReader.ReadObsFile();
            }

            if (String.Equals(lastChar, "s", StringComparison.CurrentCultureIgnoreCase))
            {
                ObsFile = new TableObsFileReader(ObsPath).Read();
            }
            if (ObsFile == null)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("不支持输入文件格式！");
                return;
            }

            this.bindingSource_obsInfo.DataSource = ObsFile;
            var prns = ObsFile.GetPrns();
            if (prns != null && prns.Count > 0)
            {
                prns.Sort();
                this.bindingSource_sat.DataSource = prns;
            }
            this.attributeBox1.DataGridView.DataSource = Geo.Utils.ObjectUtil.GetAttributes(ObsFile.Header, false);

            string msg = "";
            msg += "首次观测时间：" + ObsFile.Header.StartTime + "\r\n";
            msg += "最后观测时间：" + ObsFile.Header.EndTime + "\r\n";
            msg += "采样间隔：" + ObsFile.Header.Interval + " 秒" + "\r\n";

            this.textBox_show.Text = msg;
        }
        /// <summary>
        /// 数据绑定
        /// </summary>
        /// <param name="table"></param>
        private void BindDataSource(ObjectTableStorage table)
        {
            this.objectTableControl1.DataBind(table);
        }

        /// <summary>
        /// 针对某一个卫星绘制
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        private ObjectTableStorage BuildObjectTable(SatelliteNumber prn)
        {
            bool isCarrierInLen = checkBox_carrrierInLen.Checked;
            bool isShowL1Only = checkBox_showL1Only.Checked;
            List<TimedRinexSatObsData> records = ObsFile.GetEpochTimedObservations(prn);
            ObjectTableStorage table = new ObjectTableStorage();
            foreach (var record in records)
            {
                table.NewRow();
                table.AddItem("Epoch", record.Time);
                foreach (var item in record.SatObsData)
                {
                    if (isShowL1Only && !item.Key.Contains("1")) { continue; }
                    if (isCarrierInLen && item.Key.Contains("L1"))
                    {
                        var freq = Frequence.GetFrequenceA(prn.SatelliteType);
                        table.AddItem(item.Key, item.Value.Value * freq.WaveLength);
                    }
                    else if (isCarrierInLen && item.Key.Contains("L2"))
                    {
                        var freq = Frequence.GetFrequenceA(prn.SatelliteType);
                        table.AddItem(item.Key, item.Value.Value * freq.WaveLength);
                    }
                    else
                    {
                        table.AddItem(item.Key, item.Value.Value);
                    }
                }
            }
            return table;
        }
        TimeNumeralWindowDataManager<SatelliteNumber> PolyfitTimeNumeralWindowDataManager;

        private void button_p_L_div2_Click(object sender, EventArgs e)
        {
            EnableRunButton(false);

            CheckAndReadObsFile();

            int bufferSize = namedIntControl_bufferCount.GetValue();
            bool isShowPoly = checkBox_showPoly.Checked;
            int smoothWindow = this.namedIntControl_smoothWindow.GetValue();
            double cutOff = namedFloatControl_satCutoff.GetValue();
            bool isShowL1Only = checkBox_isShowL1Only.Checked;
            SatelliteNumber prn = (SatelliteNumber)this.bindingSource_sat.Current;
            int deltaIonoOrder = this.namedIntControl_deltaIonoOrder.GetValue();
            int ionFitWindow = this.namedIntControl1_ionoFitEpochCount.GetValue();

            var siteXyz = ObsFile.Header.ApproxXyz;

            double interval = ObsFile.Interval;
            PolyfitType polyfitType = enumRadioControl_fitType.GetCurrent<PolyfitType>();
            ProcessOneOrder(bufferSize, cutOff, prn, ionFitWindow, siteXyz, interval, deltaIonoOrder, true, polyfitType);

            EnableRunButton(true);
        }
        //注意不支持并行计算
        private void PolyfitBufferStream_MaterialInputted(TimedRinexSatObsData material)
        {
            var prn = material.SatObsData.Prn;
            var data = material.SatObsData;
            var waveLenL1 = Frequence.GetFrequence(prn.SatelliteType, 1).WaveLength;
            var waveLenL2 = Frequence.GetFrequence(prn.SatelliteType, 2).WaveLength;

            double L1 = data.PhaseA.Value * waveLenL1;
            double P1 = data.RangeA.Value;
            double ionOfPL1 = (P1 - L1) / 2;

            PolyfitTimeNumeralWindowDataManager.GetOrCreate(material.SatObsData.Prn).Add(material.Time, ionOfPL1);
        } 
        private void button_ionoDifferByLL_Click(object sender, EventArgs e)
        {

            EnableRunButton(false);

            CheckAndReadObsFile();


            DateTime start = DateTime.Now;
            int bufferSize = namedIntControl_bufferCount.GetValue();
            bool isShowPoly = checkBox_showPoly.Checked;
            int smoothWindow = this.namedIntControl_smoothWindow.GetValue();
            double cutOff = namedFloatControl_satCutoff.GetValue();
            bool isShowL1Only = checkBox_isShowL1Only.Checked;
            SatelliteNumber prn = (SatelliteNumber)this.bindingSource_sat.Current;

            List<TimedRinexSatObsData> records = ObsFile.GetEpochTimedObservations(prn);
            ObjectTableStorage table = new ObjectTableStorage(prn + "_电离层变化率");

            var firtTime = records[0].Time;
            int deltaIonoOrder = this.namedIntControl_deltaIonoOrder.GetValue();
            var siteXyz = ObsFile.Header.ApproxXyz;

            double interval = ObsFile.Interval;

            TimeNumeralWindowData polyFitWindowData = new TimeNumeralWindowData(smoothWindow, interval * 5);
            var epochSatBuilder = new RinexEpochSatBuilder();
            progressBarComponent1.InitProcess(records.Count);
            TimedEpochChangeRateSolverManager<string> manager = new TimedEpochChangeRateSolverManager<string>(interval, 5);

            foreach (var record in records)
            {
                progressBarComponent1.PerformProcessStep();

                var time = record.Time;
                var data = record.SatObsData;


                var eph = EphemerisService.Get(prn, time);
                if (eph == null) { continue; }

                var satXyz = eph.XYZ;

                var polar = Geo.Coordinates.CoordTransformer.XyzToGeoPolar(satXyz, siteXyz, AngleUnit.Degree);
                if (polar.Elevation < cutOff)
                {
                    continue;
                }

                //扩展
                var sat = epochSatBuilder.SetPrn(record.SatObsData.Prn).SetTime(time).Build(record.SatObsData);

                table.NewRow();
                table.AddItem("Epoch", record.Time);
                //L1 
                var deltaI = manager.GetOrCreate(prn + "_L1").GetChangeRate(time, sat.IonoLenOfL1ByDifferL1L2);
                table.AddItem("△IofL1", deltaI);

                if (!isShowL1Only)
                {
                    var deltaI2 = manager.GetOrCreate(prn + "_L2").GetChangeRate(time, sat.IonoLenOfL2ByDifferL1L2);
                    table.AddItem("△IofL2", deltaI2);
                }
            }
            progressBarComponent1.Full();
            BindDataSource(table);
            EnableRunButton(true);

            var span = DateTime.Now - start;// = DateTime.Now;
            log.Info("计算完毕，耗时 ： " + span);
        }

        private void button_solveAllRate_Click(object sender, EventArgs e)
        {
            if (ObsFile == null) { MessageBox.Show("请先读取数据！"); return; }

            EnableRunButton(false);
            string filePath = ObsPath;

            CaculateAllIonoRate(filePath);

            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(OutDirectory);
            EnableRunButton(true);
        }

        bool isDrawChart { get; set; }
        bool IsShowTable { get; set; }

        private void button_multiFile_Click(object sender, EventArgs e)
        {
            IsShowTable = checkBox_showTable.Checked;
            isDrawChart = checkBox_openChart.Checked;
            backgroundWorker1.RunWorkerAsync();
        }

        public List<SatelliteType> SatelliteTypes { get => multiGnssSystemSelectControl1.SatelliteTypes; }

        #region  计算细节
        private void CaculateAllIonoRate(string filePath, bool isSingleFile = true)
        {
            bool isShowL1Only = checkBox_isShowL1Only.Checked;
            double cutOff = namedFloatControl_satCutoff.GetValue();
            int smoothWindow = this.namedIntControl_smoothWindow.GetValue();
            var _obsDataSource = new RinexFileObsDataSource(filePath);
            var satCut = this.namedFloatControl_satCutoff.GetValue();


            ObjectTableManager txtManager = new ObjectTableManager(10000000, OutDirectory);
            ObjectTableStorage table = txtManager.GetOrCreate(System.IO.Path.GetFileNameWithoutExtension(filePath) + "_Data");// new TableTextStorage();

            var option = new GnssProcessOption();
            option.MinSatCount = 1;
            option.VertAngleCut = satCut;
            option.MinFrequenceCount = 2;
            var context = DataSourceContext.LoadDefault(option, _obsDataSource);

            var bufferedStream = new BufferedStreamService<EpochInformation>(_obsDataSource, option.BufferSize);
            SatTimeInfoManager SatTimeInfoManager = new Gnsser.SatTimeInfoManager(_obsDataSource.ObsInfo.Interval);
            var Reviser = EpochInfoReviseManager.GetDefaultCycleSlipDetectReviser(context, option);

            var checker = EpochCheckingManager.GetDefaultCheckers(context, option);
            int i = -1;

            double interval = _obsDataSource.Header.Interval;//ObsFile.Interval;
            TimedEpochChangeRateSolverManager<string> manager = new TimedEpochChangeRateSolverManager<string>(interval, 5);
            if (isSingleFile)
            {
                progressBarComponent1.InitProcess((int)(_obsDataSource.Header.TimePeriod.TimeSpan.TotalSeconds / interval));
            }
            var siteXyz = _obsDataSource.Header.ApproxXyz;
            foreach (var item in bufferedStream)
            {
                i++;

                if (isSingleFile)
                {
                    progressBarComponent1.PerformProcessStep();
                }
                //原始数据检核
                var epochInfo = item;
                epochInfo.RemoveOtherGnssSystem(SatelliteTypes);
                if (!checker.Check(item))
                {
                    continue;
                }

                //数据矫正
                Reviser.Buffers = bufferedStream.MaterialBuffers;
                bool result = Reviser.Revise(ref epochInfo);
                if (!result)
                {
                    continue;
                }
                var time = epochInfo.ReceiverTime;
                table.NewRow();
                table.AddItem("Epoch", epochInfo.ReceiverTime);
                //计算伪距平滑值
                foreach (var sat in epochInfo)
                {
                    var prn = sat.Prn;
                    var eph = EphemerisService.Get(prn, time);
                    if (eph == null) { continue; }

                    var satXyz = eph.XYZ;

                    var polar = Geo.Coordinates.CoordTransformer.XyzToGeoPolar(satXyz, siteXyz, AngleUnit.Degree);
                    if (polar.Elevation < cutOff)
                    {
                        continue;
                    }

                    //L1 
                    var deltaI = manager.GetOrCreate(prn + "_L1").SetIsReset(sat.IsUnstable).GetChangeRate(time, sat.IonoLenOfL1ByDifferL1L2);
                    table.AddItem(sat.Prn + "_△IofL1", deltaI);

                    if (!isShowL1Only)
                    {
                        var deltaI2 = manager.GetOrCreate(prn + "_L2").SetIsReset(sat.IsUnstable).GetChangeRate(time, sat.IonoLenOfL2ByDifferL1L2);
                        table.AddItem(sat.Prn + "_△IofL2", deltaI2);
                    }
                }

                i++;
            }
            if (isSingleFile)
            {
                progressBarComponent1.Full();
                this.objectTableControl1.DataBind(table);
            }
            if (isDrawChart)
            {
                this.Invoke(new Action(() =>
                {
                    var chartForm = new Geo.Winform.CommonChartForm(table, System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point);
                    chartForm.Text = "" + table.Name;
                    chartForm.Show();
                }));
            }
            if (IsShowTable)
            {
                this.Invoke(new Action(() =>
                {
                    TableObjectViewForm form = new TableObjectViewForm(table);
                    form.Show();
                }));
            }

            txtManager.WriteAllToFileAndCloseStream();
        }

        #endregion

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var inputPathes = this.fileOpenControl_multiPath.FilePathes;
            if (inputPathes == null || inputPathes.Length == 0) { MessageBox.Show("请加入批量数据！"); return; }

            EnableRunButton(false);

            this.progressBarComponent1.InitProcess(inputPathes.Length);
            this.progressBarComponent1.ShowInfo("正在计算！");

            Parallel.ForEach(inputPathes, (inputPath, state) =>
            {

                log.Info("处理\t" + inputPath);

                CaculateAllIonoRate(inputPath, false);

                this.progressBarComponent1.PerformProcessStep();
            });
            progressBarComponent1.Full();

            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(OutDirectory);
            EnableRunButton(true);
        }

        private void button_diffOrderPolyIonoFit_Click(object sender, EventArgs e)
        {
            EnableRunButton(false);
            CheckAndReadObsFile();
            DateTime start = DateTime.Now;

            int fromOrder = this.namedIntControl_fromOrder.GetValue();
            int toOrder = this.namedIntControl_toOrder.GetValue();


            int bufferSize = namedIntControl_bufferCount.GetValue();
            bool isShowPoly = checkBox_showPoly.Checked;
            int smoothWindow = this.namedIntControl_smoothWindow.GetValue();
            double cutOff = namedFloatControl_satCutoff.GetValue();
            bool isShowL1Only = checkBox_isShowL1Only.Checked;
            SatelliteNumber prn = (SatelliteNumber)this.bindingSource_sat.Current;
            int deltaIonoOrder = this.namedIntControl_deltaIonoOrder.GetValue();
            int ionFitWindow = this.namedIntControl1_ionoFitEpochCount.GetValue();
            var siteXyz = ObsFile.Header.ApproxXyz;
            double interval = ObsFile.Interval;

            PolyfitType polyfitType = enumRadioControl_fitType.GetCurrent<PolyfitType>();

            progressBarComponent1.InitProcess(toOrder - fromOrder + 1);


            for (int order = fromOrder; order <= toOrder; order++)
            {
                ProcessOneOrder(bufferSize, cutOff, prn, ionFitWindow, siteXyz, interval, order, false, polyfitType);

                progressBarComponent1.PerformProcessStep();

            }
            progressBarComponent1.Full();

            EnableRunButton(true);


            var span = DateTime.Now - start;// = DateTime.Now;
            log.Info("计算完毕，耗时 ： " + span);
        }

        private ObjectTableStorage ProcessOneOrder(int bufferSize, double cutOff, SatelliteNumber prn, int ionFitWindow, XYZ siteXyz, double interval, 
            int order, 
            bool isSingle, PolyfitType polyfitType)
        {
            List<TimedRinexSatObsData> records = ObsFile.GetEpochTimedObservations(prn);
            ObjectTableStorage table = new ObjectTableStorage(prn + "_多法电离层变化");


            var countCal = new MovingWindowCounter(ionFitWindow, polyfitType);
            

            PolyfitTimeNumeralWindowDataManager = new TimeNumeralWindowDataManager<SatelliteNumber>(ionFitWindow + bufferSize, interval * 5);
            TimeNumeralWindowData polyFitWindowData = PolyfitTimeNumeralWindowDataManager.GetOrCreate(prn);
            var epochSatBuilder = new RinexEpochSatBuilder();

            if (isSingle) progressBarComponent1.InitProcess(records.Count);

            BufferedStreamService<TimedRinexSatObsData> bufferStream = new BufferedStreamService<TimedRinexSatObsData>(records, bufferSize);
            bufferStream.MaterialInputted += PolyfitBufferStream_MaterialInputted;
            //foreach (var record in bufferStream)
            foreach (var record in records)
            {
                var time = record.Time;
                var data = record.SatObsData;
                var eph = EphemerisService.Get(prn, time);
                if (eph == null) { continue; }

                var satXyz = eph.XYZ;

                var polar = Geo.Coordinates.CoordTransformer.XyzToGeoPolar(satXyz, siteXyz, AngleUnit.Degree);
                if (polar.Elevation < cutOff)
                {
                    continue;
                }

                var waveLenL1 = Frequence.GetFrequence(prn.SatelliteType, 1).WaveLength;
                var waveLenL2 = Frequence.GetFrequence(prn.SatelliteType, 2).WaveLength;

                double L1 = data.PhaseA.Value * waveLenL1;
                double P1 = data.RangeA.Value;

                //扩展
                var sat = epochSatBuilder.SetPrn(record.SatObsData.Prn).SetTime(time).Build(record.SatObsData);

                table.NewRow();
                table.AddItem("Epoch", record.Time);

                double ionOfPL1 = (P1 - L1) / 2;
                double ionOfLL = sat.IonoLenOfL1ByDifferL1L2;

                table.AddItem("(P1-L1)/2", ionOfPL1);
                table.AddItem("(L1-L2)*f1", ionOfLL);

                polyFitWindowData.Add(time, ionOfPL1);

                var fitP1 = ionOfPL1;
                if (order < polyFitWindowData.Count)
                {
                   
                    if(polyfitType == PolyfitType.OverlapedWindow || polyfitType == PolyfitType.IndependentWindow)
                    { 
                        fitP1 = polyFitWindowData.GetSectedMovWindowPolyfitValue( order,time, countCal.PolyfitCount, countCal.MarginCount, true, countCal.OverlapCount).Value;

                    }
                    else// (PolyfitType == PolyfitType.MovingWindow)
                    {
                        fitP1 = polyFitWindowData.GetPolyFitValue(time, order, ionFitWindow).Value;
                    }
                }

                table.AddItem("LsFitIono(P1-L1)/2", fitP1);

                if (isSingle)
                {
                    progressBarComponent1.PerformProcessStep();
                }
            }

            //BindDataSource(table);
            //自动对齐输出
            ObjectTableStorage newTable2 = table.GetTableAllColMinusFirstValid();
            var newTable = ObjectTableUtil.GetAlignedTable(newTable2);

            newTable.UpdateAllByMinusCol("(L1-L2)*f1");

            var text = "Order: " + order + "\r\n" + newTable.GetAveragesWithStdDevTable().GetTextTable("\t", " ", "G6");
            log.Fatal(text);

            if (isSingle)
            {
                this.BindDataSource(table);
                new Geo.Winform.TableObjectViewForm(newTable).Show();

                progressBarComponent1.Full();
            }
            return newTable;
        }

        private void button_CacuIonoDelta_Click(object sender, EventArgs e)
        {

            EnableRunButton(false);

            CheckAndReadObsFile();
            int order = 1;
            int bufferSize = namedIntControl_bufferCount.GetValue();
            bool isShowPoly = checkBox_showPoly.Checked;
            int smoothWindow = this.namedIntControl_smoothWindow.GetValue();
            double cutOff = namedFloatControl_satCutoff.GetValue();
            bool isShowL1Only = checkBox_isShowL1Only.Checked;
            int deltaIonoOrder = this.namedIntControl_deltaIonoOrder.GetValue();
            int ionFitWindow = this.namedIntControl1_ionoFitEpochCount.GetValue();

            var siteXyz = ObsFile.Header.ApproxXyz;

            double interval = ObsFile.Interval;

            var Ionotype = enumRadioControl_ionDifferType.GetCurrent<IonoDifferCorrectionType>();

            ObjectTableStorage table = new ObjectTableStorage(Ionotype + "Of_" + ObsFile.SiteInfo.SiteName + "_电离层变化");
            PolyfitTimeNumeralWindowDataManager = new TimeNumeralWindowDataManager<SatelliteNumber>(ionFitWindow + bufferSize, interval * 5);

            progressBarComponent1.InitProcess(ObsFile.Count);

            var epochSatBuilder = new RinexEpochSatBuilder();
            var bufferStream = new BufferedStreamService<RinexEpochObservation>(ObsFile, bufferSize);
            bufferStream.MaterialInputted += BufferStream_MaterialInputted;

            foreach (var epochObs in bufferStream)
            { 
                var time = epochObs.ReceiverTime;

                table.NewRow();
                table.AddItem("Epoch", time);
                foreach (var satObs in epochObs)
                {
                    var data = satObs;
                    SatelliteNumber prn = satObs.Prn;// (SatelliteNumber)this.bindingSource_sat.Current;
                    var eph = EphemerisService.Get(prn, time);
                    if (eph == null) { continue; }

                    var satXyz = eph.XYZ;

                    var polar = Geo.Coordinates.CoordTransformer.XyzToGeoPolar(satXyz, siteXyz, AngleUnit.Degree);
                    if (polar.Elevation < cutOff)
                    {
                        continue;
                    }

                    var polyFitWindowData = PolyfitTimeNumeralWindowDataManager.GetOrCreate(prn);
                    var waveLenL1 = Frequence.GetFrequence(prn.SatelliteType, 1).WaveLength;
                    var waveLenL2 = Frequence.GetFrequence(prn.SatelliteType, 2).WaveLength;

                    double L1 = data.PhaseA.Value * waveLenL1;
                    double P1 = data.RangeA.Value;
                    double ionOfPL1 = (P1 - L1) / 2;


                    double ionoVal = ionOfPL1;
                    switch (Ionotype)
                    {
                        case IonoDifferCorrectionType.No://原始数据
                            ionoVal = ionOfPL1;
                            break;
                        case IonoDifferCorrectionType.DualFreqCarrier:

                            var sat = epochSatBuilder.SetPrn(satObs.Prn).SetTime(time).Build(satObs);
                            double ionOfLL = sat.IonoLenOfL1ByDifferL1L2;
                            ionoVal = ionOfLL;
                            break;
                        case IonoDifferCorrectionType.WindowPolyfit:
                            { 
                                polyFitWindowData.Add(time, ionOfPL1);

                                if (order < polyFitWindowData.Count)
                                {
                                    ionoVal = polyFitWindowData.GetPolyFitValue(time, order, ionFitWindow).Value;
                                }
                            }
                            break;
                        case IonoDifferCorrectionType.WindowWeightedAverage:
                            {
                                polyFitWindowData.Add(time, ionOfPL1);

                                if (order < polyFitWindowData.Count)
                                {
                                    ionoVal = polyFitWindowData.GetAdaptiveLinearFitValue(time, order, false).Value;
                                }
                            }
                            break;
                        default:
                            break;
                    }


                    table.AddItem(satObs.Prn, ionoVal); 
                }

                progressBarComponent1.PerformProcessStep();
            }
            
            this.BindDataSource(table);
            
            //自动对齐输出
            //TableObjectStorage newTable = table.GetTableAllColMinusFirstValid(); 
            //new Geo.Winform.TableObjectViewForm(newTable).Show();

            progressBarComponent1.Full();

            EnableRunButton(true);
        }

        /// <summary>
        /// 缓存
        /// </summary>
        /// <param name="material"></param>
        private void BufferStream_MaterialInputted(RinexEpochObservation material)
        {
            foreach (var sat in material)
            {
                var prn = sat.Prn;
                var data = sat;
                var waveLenL1 = Frequence.GetFrequence(prn.SatelliteType, 1).WaveLength;
                var waveLenL2 = Frequence.GetFrequence(prn.SatelliteType, 2).WaveLength;

                double L1 = data.PhaseA.Value * waveLenL1;
                double P1 = data.RangeA.Value;
                double ionOfPL1 = (P1 - L1) / 2;

                PolyfitTimeNumeralWindowDataManager.GetOrCreate(prn).Add(material.ReceiverTime, ionOfPL1); 
            } 
        }

        private void button_CacuRawIonoDelta_Click(object sender, EventArgs e)
        {

        }
    }

}