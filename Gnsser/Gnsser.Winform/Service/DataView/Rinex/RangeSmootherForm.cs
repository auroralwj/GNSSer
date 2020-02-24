//2014.06.24, czs, add， 增加RINEX3.0的显示
//2015.05.11, czs, add in namu, 增加RINEX数据瘦身、导出功能，各版本数据在同一个数据表中显示
//2018.5.20, czs, edit in HMX, 平滑伪距改进


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
using Gnsser.Data;
using Gnsser.Correction;

namespace Gnsser.Winform
{
    /// <summary>
    /// 观测文件查看器
    /// </summary>
    public partial class RangeSmootherForm : Form, Gnsser.Winform.IShowLayer
    {
        public Log log = new Log(typeof(RangeSmootherForm));
        public event ShowLayerHandler ShowLayer;
        /// <summary>
        /// 观测文件
        /// </summary>
        public Data.Rinex.RinexObsFile ObsFile { get; set; }

        public string ObsPath { get { return fileOpenControl1.FilePath; } set { fileOpenControl1.FilePath = value; } }

        public RangeSmootherForm()
        {
            InitializeComponent();
        } 
        RinexObsFileReader obsFileReader;

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

        private void ObsFileViewerForm_Load(object sender, EventArgs e) {
            enumRadioControl1.Init<SmoothRangeSuperpositionType>(true);
            enumRadioControl_ionDifferType.Init<IonoDifferCorrectionType>();

            this.ObsPath = Setting.GnsserConfig.SampleOFileA; }

        private void button_viewObs_Click(object sender, EventArgs e)
        {
            if (ObsFile == null) { MessageBox.Show("请先读取数据！"); return; }

            EnableRunButton(false);

            double cutOff = namedFloatControl_satCutoff.GetValue();
            int smoothWindow = this.namedIntControl_smoothWindow.GetValue();
            var EpochInfoBuilder = new RinexEpochInfoBuilder(ObsFile.Header);
             
            var _obsDataSource = new RinexFileObsDataSource(ObsPath);
           
            SmoothedRangeBuilderManagerP1 = BuildPhaseSmoothRangeBulider();

             
            ObjectTableManager txtManager = new ObjectTableManager(10000000, Gnsser.Setting.GnsserConfig.TempDirectory);
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
                    var smoothRangeVal = manager .SetReset(sat.IsUnstable).SetRawValue(sat.ReceiverTime, rangeVal, phaseVal, 0)
                       .Build().Value;

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
            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(Gnsser.Setting.GnsserConfig.TempDirectory);


            EnableRunButton(true);
        }
        NamedCarrierSmoothedRangeBuilderManager PhaseSmoothRangeBulider;
        private void button_smoothCurrent_Click(object sender, EventArgs e)
        {
            EnableRunButton(false);

            CheckAndReadObsFile();




            ObjectTableManager tableObjectStorages;
            ObjectTableStorage IonoFitTable;

            tableObjectStorages = new ObjectTableManager(Setting.TempDirectory);
            IonoFitTable = tableObjectStorages.AddTable("IonoFit");



            DateTime start = DateTime.Now;
            int bufferSize = namedIntControl_bufferCount.GetValue();
            bool isShowPoly = checkBox_showPoly.Checked;
            PhaseSmoothRangeBulider = BuildPhaseSmoothRangeBulider();
            int smoothWindow = this.namedIntControl_smoothWindow.GetValue();

            double cutOff = namedFloatControl_satCutoff.GetValue();
            bool isShowL1Only = checkBox_isShowL1Only.Checked;
            SatelliteNumber prn = (SatelliteNumber)this.bindingSource_sat.Current; 

            GlobalIgsGridIonoService ionoService = GlobalIgsGridIonoService.Instance;
            GlobalIgsGridIonoDcbService ionoDcbService = GlobalIgsGridIonoDcbService.Instance;
            GlobalIgsEphemerisService ephemerisService = GlobalIgsEphemerisService.Instance;

            var TimedSmoothValueBuilderManager = new TimedSmoothValueBuilderManager(smoothWindow);

            List<TimedRinexSatObsData> records = ObsFile.GetEpochTimedObservations(prn);
            ObjectTableStorage table = new ObjectTableStorage(prn + "_平滑伪距");

            var firtTime = records[0].Time;

            double P1_P2sat = ionoDcbService.GetDcb(firtTime, prn).Value * GnssConst.MeterPerNano;

            double P1_P2recMeter = ionoDcbService.GetDcbMeterForP1(firtTime, Path.GetFileName(ObsPath).Substring(0, 4).ToLower());
            var siteXyz = ObsFile.Header.ApproxXyz;

            double prevIonoAmbiDcb = 0;
            double prevIonoAmbiDcbL2 = 0;
            RinexFreqObsBuilder FreqObsBuilder = new RinexFreqObsBuilder();
            var epochSatBuilder = new RinexEpochSatBuilder(FreqObsBuilder);
            progressBarComponent1.InitProcess(records.Count);

            BufferedStreamService<TimedRinexSatObsData> bufferStream = new BufferedStreamService<TimedRinexSatObsData>(records, bufferSize);
            bufferStream.MaterialInputted += BufferStream_MaterialInputted;
            foreach (var record in bufferStream)
            {
                if(record == null) { continue; }

                progressBarComponent1.PerformProcessStep();

                var time = record.Time;
                var data = record.SatObsData;


                var eph = ephemerisService.Get(prn, time);
                if (eph == null) { continue; }

                var satXyz = eph.XYZ;

                var polar = Geo.Coordinates.CoordTransformer.XyzToGeoPolar(satXyz, siteXyz, AngleUnit.Degree);
                if (polar.Elevation < cutOff)
                {
                    continue;
                }




                var waveLenL1 = Frequence.GetFrequence(prn, FrequenceType.A, time).WaveLength;
                var waveLenL2 = Frequence.GetFrequence(prn, FrequenceType.B, time).WaveLength;

                double L1 = data.PhaseA.Value * waveLenL1;
                double P1 = data.RangeA.Value;
                double L2 = data.PhaseB != null ? record.SatObsData.PhaseB.Value * waveLenL2 : 0;
                double P2 = data.RangeB != null ? record.SatObsData.RangeB.Value : 0;

                //扩展
                var sat = epochSatBuilder.SetPrn(record.SatObsData.Prn).Build(record.SatObsData);

                //  get => (this.FrequenceA.PhaseRange.Value - this.FrequenceB.PhaseRange.Value) * Frequence.GetIonoAndDcbCoeffL1L2(this.Prn.SatelliteType);

                //double differIonoL1 = 0;
                //double differIonoL2 = 0;
                //if (prevIonoAmbiDcb == 0)
                //{
                //    prevIonoAmbiDcb = sat.IonoLenOfL1ByDifferL1L2;
                //    prevIonoAmbiDcbL2 = sat.IonoLenOfL2ByDifferL1L2;
                //}
                //differIonoL1 = sat.IonoLenOfL1ByDifferL1L2 - prevIonoAmbiDcb;
                //differIonoL1 = sat.IonoLenOfL1ByDifferL1L2 - prevIonoAmbiDcb;
               // L1 = L1 + 2 * differIono;

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

        private void BufferStream_MaterialInputted(TimedRinexSatObsData record)
        {
            SatelliteNumber prn = (SatelliteNumber)this.bindingSource_sat.Current;
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
            int  smoothWindow = this.namedIntControl_smoothWindow.GetValue();
            bool isWeighted = checkBox_isWeighted.Checked;
            int deltaIonoOrder = this.namedIntControl_deltaIonoOrder.GetValue();
            var type = enumRadioControl1.GetCurrent<SmoothRangeSuperpositionType>();
            int bufferSize = namedIntControl_bufferCount.GetValue();
            int ionoFitEpochCount = namedIntControl1_ionoFitEpochCount.GetValue();
            var Ionotype = enumRadioControl_ionDifferType.GetCurrent<IonoDifferCorrectionType>(); 
            return  new CarrierSmoothedRangeBuilderManager(isApproved, smoothWindow, isWeighted, Ionotype, deltaIonoOrder, bufferSize, ionoFitEpochCount, type);
        }

        private void EnableRunButton(bool trueOrFalse)
        {
            this.button_smoothCurrent.Enabled = trueOrFalse;
            this.button_MultiRun.Enabled = trueOrFalse;
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
                Geo.IO.InputFileManager inputFileManager = new Geo.IO.InputFileManager(Setting.TempDirectory);
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
            prns.Sort();
            this.bindingSource_sat.DataSource = prns;

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
                    if(isShowL1Only && !item.Key.Contains("1")){ continue; }
                    if (isCarrierInLen && item.Key.Contains("L1"))
                    {
                        var freq = Frequence.GetFrequence(prn, FrequenceType.A, record.Time);
                        table.AddItem(item.Key, item.Value.Value * freq.WaveLength);
                    }
                    else if (isCarrierInLen && item.Key.Contains("L2"))
                    {
                        var freq = Frequence.GetFrequence(prn, FrequenceType.B, record.Time);
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
    }
}