//2018.05.19, czs, create in HMX， 电离层DCB计算 

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
    /// 电离层DCB计算
    /// </summary>
    public partial class IonoDcbSolveForm : Form, Gnsser.Winform.IShowLayer
    {
        public Log log = new Log(typeof(RangeSmootherForm));
        public event ShowLayerHandler ShowLayer;
        /// <summary>
        /// 观测文件
        /// </summary>
        public Data.Rinex.RinexObsFile ObsFile { get; set; }

        public IonoDcbSolveForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 载波相位平滑伪距
        /// </summary>
        public CarrierSmoothedRangeBuilderManager SmoothedRangeBuilderManager { get; set; }
        public string ObsPath { get { return fileOpenControl1.FilePath; } set { fileOpenControl1.FilePath = value; } }
   
        RinexObsFileReader obsFileReader;
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

 
        private void ObsFileViewerForm_Load(object sender, EventArgs e) { this.ObsPath = Setting.GnsserConfig.SampleOFileA; }

        private void button_viewObs_Click(object sender, EventArgs e)
        {
            if (ObsFile == null) { MessageBox.Show("请先读取数据！"); return; }
            var window = this.namedIntControl_smoothWindow.GetValue();

            SmoothedRangeBuilderManager = new CarrierSmoothedRangeBuilderManager(true, window,true, IonoDifferCorrectionType.DualFreqCarrier);
 
            ObjectTableStorage table = new ObjectTableStorage();
            var EpochInfoBuilder = new RinexEpochInfoBuilder( ObsFile.Header);
            var _obsDataSource = new RinexFileObsDataSource(ObsPath); 
            var option = new GnssProcessOption();
            var context = DataSourceContext.LoadDefault(option, _obsDataSource);
            var bufferedStream = new BufferedStreamService<EpochInformation>(_obsDataSource, option.BufferSize);
            SatTimeInfoManager SatTimeInfoManager = new Gnsser.SatTimeInfoManager(_obsDataSource.ObsInfo.Interval);
            var Reviser = EpochInfoReviseManager.GetDefaultEpochInfoReviser(context, option, SatTimeInfoManager);
            var checker = EpochCheckingManager.GetDefaultCheckers(context, option);
            int i = -1;
            foreach (var item in bufferedStream)
            {
                i++; 

                //原始数据检核
                var epochInfo = item;
                if (!checker.Check(item))
                {
                    continue;
                }
                //数据矫正
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
                    var rangeVal = sat.FrequenceA.PseudoRange.CorrectedValue;// sat[type].CorrectedValue;

                    var phaseVal = sat.FrequenceA.PhaseRange.CorrectedValue;// sat[option.PhaseTypeToSmoothRange].CorrectedValue;
                    var manager = SmoothedRangeBuilderManager.GetOrCreate(sat.Prn);
                    var smoothRangeVal = manager.SetRawValue(sat.ReceiverTime, rangeVal, phaseVal, sat.IonoLenOfL1ByDifferL1L2 ).SetReset(sat.IsUnstable).Build();

                    table.AddItem(sat.Prn + "_Raw", rangeVal + "");
                    table.AddItem(sat.Prn + "_Smooth", smoothRangeVal + "");
                }

                i++;
            }

            table.EndRow();
            this.BindDataSource(table);
        }


        private void button_multiSolve_Click(object sender, EventArgs e)
        {
            CheckAndReadObsFile();
            double cutOff = namedFloatControl_satCutoff.GetValue();
            int smoothWindow = this.namedIntControl_smoothWindow.GetValue();
            bool isUserRawValue = this.radioButton_isUserRawValue.Checked;
            bool isUseSphaseSmoothP = this.radioButton_isUsePhaaseSmoothP.Checked;
            bool isUserPolyFitValue = this.radioButton_isUserPolyFitVal.Checked;

            var PhaseSmoothRangeBulider = new NamedCarrierSmoothedRangeBuilderManager(true, smoothWindow, true, IonoDifferCorrectionType.DualFreqCarrier);


            GlobalIgsGridIonoService ionoService = GlobalIgsGridIonoService.Instance;
            GlobalIgsGridIonoDcbService ionoDcbService = GlobalIgsGridIonoDcbService.Instance;
            GlobalIgsEphemerisService ephemerisService = GlobalIgsEphemerisService.Instance;

            ObjectTableStorage table = new ObjectTableStorage( Path.GetFileName(ObsPath) + "_硬件延迟");
            var EpochInfoBuilder = new RinexEpochInfoBuilder(ObsFile.Header);
            var _obsDataSource = new RinexFileObsDataSource(ObsPath);
            var option = new GnssProcessOption();
            var context = DataSourceContext.LoadDefault(option, _obsDataSource);
            var bufferedStream = new BufferedStreamService<EpochInformation>(_obsDataSource, option.BufferSize);
            SatTimeInfoManager SatTimeInfoManager = new Gnsser.SatTimeInfoManager(_obsDataSource.ObsInfo.Interval);
            var CycleReviser = EpochInfoReviseManager.GetDefaultCycleSlipDetectReviser(context, option);
            var Reviser = new BufferPolyRangeSmoothReviser(option);
                //EpochInfoReviseManager.GetDefaultEpochInfoReviser(context, option, SatTimeInfoManager);
            var checker = EpochCheckingManager.GetDefaultCheckers(context, option);
            int i = -1;
            var siteXyz = ObsFile.Header.ApproxXyz;
            progressBarComponent1.InitProcess( ObsFile.Count);
            foreach (var item in bufferedStream)
            {
                i++;
                progressBarComponent1.PerformProcessStep();
                //原始数据检核
                var epochInfo = item;
                //if (!checker.Check(item))
                //{
                //    continue;
                //}
                ////数据矫正

                if (isUseSphaseSmoothP) //需要周跳探测
                {
                    bool result = CycleReviser.Revise(ref epochInfo);
                    if (!result)
                    {
                        continue;
                    }
                }

                if (isUserPolyFitValue)//缓存与多项式拟合改正
                {
                    Reviser.Buffers = bufferedStream.MaterialBuffers;
                    bool result =  Reviser.Revise(ref epochInfo);
                    if (!result)
                    {
                        continue;
                    }
                }

                table.NewRow();
                table.AddItem("Epoch", epochInfo.ReceiverTime.ToShortTimeString());
                //计算伪距平滑值
                foreach (var sat in epochInfo)
                {
                    var prn = sat.Prn;
                    var time = sat.ReceiverTime;
                    var eph = ephemerisService.Get(prn, time);
                    if (eph == null) { continue; }

                    var satXyz = eph.XYZ;

                    var polar = Geo.Coordinates.CoordTransformer.XyzToGeoPolar(satXyz, siteXyz, AngleUnit.Degree);
                    if (polar.Elevation < cutOff)
                    {
                        continue;
                    }

                    double P1_P2sat = ionoDcbService.GetDcb(time, prn).Value * GnssConst.MeterPerNano;
                    // double P1_P2recMeter = ionoDcbService.GetDcbMeterForP1(time, Path.GetFileName(ObsPath).Substring(0, 4).ToLower());

                    var F1 = sat.FrequenceA.Frequence;// Frequence.GetFrequence(prn, 1, time);
                    var F2 = sat.FrequenceB.Frequence;//Frequence.GetFrequence(prn, 2, time);

                    var f1 = F1.Value * 1e6;//恢复单位
                    var f2 = F2.Value * 1e6;

                    double f1f1 = f1 * f1;
                    double f2f2 = f2 * f2;

                    double a = -40.28 * (f2f2 - f1f1) / (f1f1 * f2f2);

                    //观测值，或组合值
                    var P1 = sat.FrequenceA.PseudoRange.Value;
                    var P2 = sat.FrequenceB.PseudoRange.Value;
                    var L1 = sat.FrequenceA.PhaseRange.Value;
                    var L2 = sat.FrequenceB.PhaseRange.Value;

                    double differP = P1 - P2;//raw Value
                    if (isUseSphaseSmoothP)
                    {
                        var smootherP1 = PhaseSmoothRangeBulider.GetOrCreate(prn + "_P1");
                        var smootherP2 = PhaseSmoothRangeBulider.GetOrCreate(prn + "_P2");

                        var P1s = smootherP1
                            .SetReset(sat.IsUnstable)
                            .SetRawValue(sat.ReceiverTime, P1, L1, sat.IonoLenOfL1ByDifferL1L2)
                            .Build();
                        var P2s = smootherP2
                            .SetReset(sat.IsUnstable)
                            .SetRawValue(sat.ReceiverTime, P2, L2, sat.IonoLenOfL2ByDifferL1L2)
                            .Build();

                        differP = P1s.Value - P2s.Value;
                    }

                    if (isUserPolyFitValue)
                    {
                        differP = sat.FrequenceA.PseudoRange.CorrectedValue - sat.FrequenceB.PseudoRange.CorrectedValue;
                    }

                    var cTEC = ionoService.GetSlope(time, siteXyz, satXyz);
                    double ionoRange = a * cTEC.Value * 1e16; // 单位是1e16
                    double rawDifferP = P1 - P2;  

                    double ionoP1 = ionoService.GetSlopeDelayRange(time, siteXyz, satXyz, F1.Value);
                    double ionoP2 = ionoService.GetSlopeDelayRange(time, siteXyz, satXyz, F2.Value);
                    double rawDifferIonoP = ionoP1 - ionoP2;

                    var recDcb = differP - P1_P2sat - rawDifferIonoP; 


                    //table.AddItem("L1", L1);
                    //table.AddItem("L2", L2); 
                    //table.AddItem("P1", P1);
                    //table.AddItem("P2", P2);
                    //table.AddItem("P1S", P1s); 
                    //table.AddItem("P2S", P2s);
                    //table.AddItem("P1-P2", rawDifferP);
                    //table.AddItem("IonoDiffer", rawDifferIonoP);
                    //table.AddItem("SatDcb", P1_P2sat);
                    //table.AddItem("P1-P2_rec", P1_P2recMeter);
                    table.AddItem(prn+"", recDcb);
                    //table.AddItem("SmRecDcb", smRecDcb);
                    //table.AddItem(sat.Prn + "_Raw", rangeVal + "");
                    //table.AddItem(sat.Prn + "_Smooth", smoothRangeVal + "");
                }

                i++;
            }
            progressBarComponent1.Full();

            table.EndRow();
            this.BindDataSource(table);
        }

        private void button_calculate_Click(object sender, EventArgs e)
        {
            CheckAndReadObsFile();
            double cutOff = namedFloatControl_satCutoff.GetValue();
            SatelliteNumber prn = (SatelliteNumber)this.bindingSource_sat.Current;

            var F1 = Frequence.GetFrequence(prn, 1);
            var F2 = Frequence.GetFrequence(prn, 2);

            var f1 = F1.Value * 1e6;//恢复单位
            var f2 = F2.Value * 1e6;

            double f1f1 = f1 * f1;
            double f2f2 = f2 * f2;

            double a = -40.28 * (f2f2 - f1f1) / (f1f1 * f2f2);

            int smoothWindow = this.namedIntControl_smoothWindow.GetValue();

            GlobalIgsGridIonoService ionoService = GlobalIgsGridIonoService.Instance;
            GlobalIgsGridIonoDcbService  ionoDcbService = GlobalIgsGridIonoDcbService.Instance;
            GlobalIgsEphemerisService ephemerisService = GlobalIgsEphemerisService.Instance;


            var PhaseSmoothRangeBulider = new NamedCarrierSmoothedRangeBuilderManager(true, smoothWindow, true, IonoDifferCorrectionType.DualFreqCarrier);

            List<TimedRinexSatObsData> records = ObsFile.GetEpochTimedObservations(prn);
            ObjectTableStorage table = new ObjectTableStorage(prn+"_硬件延迟");

            var firtTime = records[0].Time;

            double P1_P2sat = ionoDcbService.GetDcb(firtTime, prn).Value * GnssConst.MeterPerNano;

            double P1_P2recMeter =  ionoDcbService.GetDcbMeterForP1(firtTime, Path.GetFileName(ObsPath).Substring(0, 4).ToLower());
            var siteXyz = ObsFile.Header.ApproxXyz;

            progressBarComponent1.InitProcess(records.Count);
            foreach (var record in records)
            {
                progressBarComponent1.PerformProcessStep();

                var time = record.Time;
                var data = record.SatObsData;

                var waveLenL1 = Frequence.GetFrequenceA(prn, time).WaveLength;
                var waveLenL2 = Frequence.GetFrequenceB(prn, time).WaveLength;


                double L1 = data.PhaseA.Value * waveLenL1;
                double P1 = data.RangeA.Value;
                double L2 = data.PhaseB != null ? record.SatObsData.PhaseB.Value * waveLenL2 : 0;
                double P2 = data.RangeB != null ? record.SatObsData.RangeB.Value : 0;


                var smootherP1 = PhaseSmoothRangeBulider.GetOrCreate("P1");
                var smootherP2 = PhaseSmoothRangeBulider.GetOrCreate("P2");

                var P1s = smootherP1
                    .SetReset(data.PhaseA.IsLossLock)
                    .SetRawValue(record.Time, P1, L1,  0)
                    .Build();
                var P2s = smootherP2
                    .SetReset(data.PhaseB.IsLossLock)
                    .SetRawValue(record.Time, P2, L2, 0)
                    .Build();
                //var smNew = smoother.GetSmoothedRange();

                var eph = ephemerisService.Get(prn, time);
                if(eph == null) { continue; }

                var satXyz = eph.XYZ;

                var polar =  Geo.Coordinates.CoordTransformer.XyzToGeoPolar(satXyz, siteXyz, AngleUnit.Degree);
                if(polar.Elevation < cutOff)
                {
                    continue;
                }


                var cTEC = ionoService.GetSlope(time, siteXyz, satXyz);
                double ionoRange = a * cTEC.Value * 1e16; // 单位是1e16
                double rawDifferP = P1 - P2;
                double smDifferP = P1s.Value - P2s.Value;

                //电离层倾斜延迟
                double ionoP1 = ionoService.GetSlopeDelayRange(time, siteXyz, satXyz, F1.Value);
                double ionoP2 = ionoService.GetSlopeDelayRange(time, siteXyz, satXyz, F2.Value);
                double rawDifferIonoP = ionoP1 - ionoP2;

                var rawRecDcb = rawDifferP - P1_P2sat - rawDifferIonoP;
                var smRecDcb  = smDifferP  - P1_P2sat - rawDifferIonoP;


                table.NewRow();
                table.AddItem("Epoch", record.Time);
                //table.AddItem("L1", L1);
                //table.AddItem("L2", L2); 
                //table.AddItem("P1", P1);
                //table.AddItem("P2", P2);
                //table.AddItem("P1S", P1s); 
                //table.AddItem("P2S", P2s);
                table.AddItem("P1-P2", rawDifferP);
                table.AddItem("SmP1-P2", smDifferP);
                table.AddItem("IonoDiffer", rawDifferIonoP);
                table.AddItem("SatDcb", P1_P2sat);
                //table.AddItem("P1-P2_rec", P1_P2recMeter);
                table.AddItem("RawRecDcb", rawRecDcb);
                table.AddItem("SmRecDcb", smRecDcb);
            }
            progressBarComponent1.Full();

            BindDataSource(table);


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
            List<TimedRinexSatObsData> records = ObsFile.GetEpochTimedObservations(prn);
            ObjectTableStorage table = new ObjectTableStorage();
            foreach (var record in records)
            {
                table.NewRow();
                table.AddItem("Epoch", record.Time);
                foreach (var item in record.SatObsData)
                {
                    table.AddItem(item.Key, item.Value.Value);
                }
            }
            return table;
        }

    }
}