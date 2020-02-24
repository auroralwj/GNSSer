//2016.04.26 double create on the train of xi'an-Zhengzhou 初步完成各种模型，模型的计算性能有待验证
//2016.10.08.00 double edit in hongqing
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser.Data.Rinex;
using Gnsser;
using Gnsser.Data;
using Geo.Coordinates;
using Geo.Referencing;
using AnyInfo;
using Geo.Algorithm;
using Geo.Times;
using Gnsser.Core;
using System.IO;
using Gnsser.Service;
using Geo.Algorithm;

namespace Gnsser.Winform
{
    public partial class ClockPredictionBasedonClockFileForm : Form
    {
        public ClockPredictionBasedonClockFileForm()
        {
            InitializeComponent();
        }

        private void button_getPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_clk.ShowDialog() == DialogResult.OK)
                this.textBox_Pathes.Lines = openFileDialog_clk.FileNames;
        }
        MultiFileClockService coll;
        private class SSE
        {
            public double SSE3h;
            public double SSE6h;
            public double SSE12h;
            public double SSE24h;
        }
        ClockFile clkofPredicted;
        bool IsNeedFilling;
        private void button_export_Click(object sender, EventArgs e)
        {
            bool fillWithZero = checkBox1.Checked;
            var intervalSec = int.Parse(textBox_interval.Text) * 60;
            var directory = this.directorySelectionControl1.Path;
            Geo.Utils.FileUtil.CheckOrCreateDirectory(directory);
            var ModelLength = (int)(24 * double.Parse(textBox_ModelLength.Text)) * 3600 / intervalSec;
            var PredictedLength = (int)(24 * double.Parse(textBox_PredictedLength.Text)) * 3600 / intervalSec;
            var PredictedNumber = int.Parse(textBox_PredictedNumber.Text);

            var prns = SatelliteNumber.ParsePRNsBySplliter(textBox_satPrns.Text, new char[] { ',' });
            Dictionary<SatelliteNumber, StreamWriter> prnWriters = new Dictionary<SatelliteNumber, StreamWriter>();
            foreach (var item in prns)
            {
                var path = Path.Combine(directory, item.ToString() + ".xls");
                prnWriters[item] = new StreamWriter(new FileStream(path, FileMode.Create));
            }

            string[] pathes = this.textBox_Pathes.Lines;
            coll = new MultiFileClockService(pathes);

            int allSec = (int)coll.TimePeriod.Span;
            Time end = coll.TimePeriod.End + intervalSec;
            List<double> list = new List<double>();
            Dictionary<string, SSE> ListOfSSE = new Dictionary<string, SSE>();
            string approach = null;
            #region 逐个卫星进行钟差预报
            clkofPredicted = new ClockFile();
            clkofPredicted.Header = new ClockFileHeader();
            clkofPredicted.Header.PrnList = new List<SatelliteNumber>();
            for (int number = 0; number < PredictedNumber; number++)
            {
                foreach (var prn in prns)
                {
                    var writer = prnWriters[prn];
                    //List<double> colName = new List<double>();  
                    var all = coll.Gets(prn, coll.TimePeriod.Start, end);
                    ArrayMatrix OringalClock = new ArrayMatrix(ModelLength, 1);
                    ArrayMatrix CompareClock = new ArrayMatrix(PredictedLength, 1);

                    Time StartOfModel = coll.TimePeriod.Start;
                    Time EndOfModel = StartOfModel + (ModelLength - 1) * intervalSec;
                    Time StartOfPredicted = EndOfModel + intervalSec;
                    Time EndOfPredicted = StartOfPredicted + (PredictedLength - 1) * intervalSec;

                    int DataNumberIndex = 0;
                    int ModelDataNumberIndex = 0;
                    int PredictedDataNumberIndex = 0;
                    GetClockData(fillWithZero, intervalSec, ModelLength, PredictedLength, end, prn, all, OringalClock, CompareClock, number, DataNumberIndex, ModelDataNumberIndex, PredictedDataNumberIndex);
                    if (IsNeedFilling)
                    {
                        QuadraticPolynomialModel Filling = new QuadraticPolynomialModel();
                        Filling.FillingCalculate(OringalClock, intervalSec);
                    }


                    ArrayMatrix Data = null;
                    if (radioButton_QP.Checked) { approach = "QP"; QuadraticPolynomialModel QuadraticPolynomialModel = new QuadraticPolynomialModel(); QuadraticPolynomialModel.Calculate(OringalClock, CompareClock, intervalSec); Data = QuadraticPolynomialModel.PredictedData; }
                    if (radioButton_QPT1.Checked) { approach = "QPT1"; QuadraticPolynomialT1 QuadraticPolynomialT1 = new QuadraticPolynomialT1(); QuadraticPolynomialT1.Calculate(OringalClock, CompareClock, intervalSec); list.Add(QuadraticPolynomialT1.PredictedRms); Data = QuadraticPolynomialT1.PredictedData; }
                    if (radioButton_QPT2.Checked) { approach = "QPT2"; QuadraticPolynomialT2 QuadraticPolynomialT2 = new QuadraticPolynomialT2(); QuadraticPolynomialT2.Calculate(OringalClock, CompareClock, intervalSec, prn); list.Add(QuadraticPolynomialT2.PredictedRms); Data = QuadraticPolynomialT2.PredictedData; }
                    if (radioButton_QPT3.Checked) { approach = "QPT3"; QuadraticPolynomialT3 QuadraticPolynomialT3 = new QuadraticPolynomialT3(); QuadraticPolynomialT3.Calculate(OringalClock, CompareClock, intervalSec); list.Add(QuadraticPolynomialT3.PredictedRms); Data = QuadraticPolynomialT3.PredictedData; }
                    if (radioButton_QPT4.Checked) { approach = "QPT4"; QuadraticPolynomialT4 QuadraticPolynomialT4 = new QuadraticPolynomialT4(); QuadraticPolynomialT4.Calculate(OringalClock, CompareClock, intervalSec); list.Add(QuadraticPolynomialT4.PredictedRms); Data = QuadraticPolynomialT4.PredictedData; }
                    if (radioButton_QPGM.Checked) { approach = "QPGM"; QuadraticPolynomialGM QuadraticPolynomialGM = new QuadraticPolynomialGM(); QuadraticPolynomialGM.Calculate(OringalClock, CompareClock, intervalSec); list.Add(QuadraticPolynomialGM.PredictedRms); Data = QuadraticPolynomialGM.PredictedData; }
                    if (radioButton_QPT2GM.Checked) { approach = "QPT2GM"; QuadraticPolynomialT2GM QuadraticPolynomialT2GM = new QuadraticPolynomialT2GM(); QuadraticPolynomialT2GM.Calculate(OringalClock, CompareClock, intervalSec, prn); list.Add(QuadraticPolynomialT2GM.PredictedRms); Data = QuadraticPolynomialT2GM.PredictedData; }
                    if (radioButton_QPT4GM.Checked) { approach = "QPT4GM"; QuadraticPolynomialT4GM QuadraticPolynomialT4GM = new QuadraticPolynomialT4GM(); QuadraticPolynomialT4GM.Calculate(OringalClock, CompareClock, intervalSec); list.Add(QuadraticPolynomialT4GM.PredictedRms); Data = QuadraticPolynomialT4GM.PredictedData; }

                    if (radioButton_LP.Checked) { approach = "LP"; LinearPolynomialModel LinearPolynomialModel = new LinearPolynomialModel(); LinearPolynomialModel.Calculate(OringalClock, CompareClock, intervalSec); list.Add(LinearPolynomialModel.PredictedRms); Data = LinearPolynomialModel.PredictedData; }
                    if (radioButton_DLP.Checked) { approach = "DLP"; DLinearPolynomialModel DLinearPolynomialModel = new DLinearPolynomialModel(); DLinearPolynomialModel.Calculate(OringalClock, CompareClock, intervalSec); list.Add(DLinearPolynomialModel.PredictedRms); Data = DLinearPolynomialModel.PredictedData; }
                    if (radioButton_RobustDLP.Checked)
                    {
                        approach = "RobustDLP";
                        RobustDLinearPolynomial RobustDLinearPolynomial = new RobustDLinearPolynomial();
                        RobustDLinearPolynomial.Calculate(OringalClock, CompareClock, intervalSec);
                        list.Add(RobustDLinearPolynomial.PredictedRms);
                        Data = RobustDLinearPolynomial.PredictedData;
                        SSE a = new SSE();
                        a = GetSSE(RobustDLinearPolynomial);
                        ListOfSSE.Add(prn.ToString() + '-' + number, a);
                    } 
                    if (radioButton_RobustLP.Checked) { approach = "RobustLP"; RobustLinearPolynomial RobustLinearPolynomial = new RobustLinearPolynomial(); RobustLinearPolynomial.Calculate(OringalClock, CompareClock, intervalSec); list.Add(RobustLinearPolynomial.PredictedRms); Data = RobustLinearPolynomial.PredictedData; }



                    if (radioButton_GM.Checked)
                    {
                        approach = "GM";
                        ArrayMatrix GMOringalClock1 = new ArrayMatrix(ModelLength, 1);
                        for (int GMIndex = 0; GMIndex < ModelLength; GMIndex++)
                        {
                            GMOringalClock1[GMIndex, 0] = OringalClock[ModelLength - (ModelLength - GMIndex), 0];
                        }
                        GreyModel GreyModel1 = new GreyModel();
                        SSE a = new SSE();
                        GreyModel1.Calculate(GMOringalClock1, CompareClock, intervalSec);
                        a = GetSSE(GreyModel1);
                        ListOfSSE.Add(prn.ToString() + '-' + number, a);
                        Data = GreyModel1.PredictedData;
                    }
                    if (radioButton_KFAllan.Checked) { approach = "KFAllan"; KalmanAllan KalmanAllan = new KalmanAllan(); KalmanAllan.Calculate(OringalClock, CompareClock, intervalSec); list.Add(KalmanAllan.PredictedRms); Data = KalmanAllan.PredictedData; }
                    if (radioButton_KFHardamard.Checked) { approach = "KFHardamard"; KalmanHardamard KalmanHardamard = new KalmanHardamard(); KalmanHardamard.Calculate(OringalClock, CompareClock, intervalSec); list.Add(KalmanHardamard.PredictedRms); Data = KalmanHardamard.PredictedData; }
                    if (radioButton_KFReHardamard.Checked) { approach = "KFReHardamard"; KalmanRecursionHardamard KalmanRecursionHardamard = new KalmanRecursionHardamard(); KalmanRecursionHardamard.Calculate(OringalClock, CompareClock, intervalSec); list.Add(KalmanRecursionHardamard.PredictedRms); Data = KalmanRecursionHardamard.PredictedData; }
                    if (radioButton_KFReAllan.Checked) { approach = "KFReAllan"; KalmanRecursionAllan KalmanRecursionAllan = new KalmanRecursionAllan(); KalmanRecursionAllan.Calculate(OringalClock, CompareClock, intervalSec); list.Add(KalmanRecursionAllan.PredictedRms); Data = KalmanRecursionAllan.PredictedData; }
                    for (int i0 = 0; i0 < Data.RowCount; i0++)
                    {
                        AtomicClock sat = new AtomicClock();
                        sat.Prn = prn;
                        sat.Time = StartOfPredicted + i0 * intervalSec;
                        sat.ClockBias = Data[i0, 0];
                        sat.ClockType = ClockType.Satellite;
                        if (!(clkofPredicted.Contains(sat.Prn.ToString())))
                            clkofPredicted.Add(sat.Prn.ToString(), new List<AtomicClock>());
                        clkofPredicted[sat.Prn.ToString()].Add(sat);
                    }
                    clkofPredicted.Header.PrnList.Add(prn);

                }
                OutputOfPredictedClock(directory, approach,intervalSec);
            }
            #endregion
            OutputOfSSE(directory, ListOfSSE, approach);

            Geo.Utils.FileUtil.OpenDirectory(directory);
        }
        private static void OutputOfSSE(string directory, Dictionary<string, SSE> ListOfSSE, string approach)
        {
            var pathOfSSE = Path.Combine(directory, "PredictedBasedon" + approach + ".xls");
            FileInfo SSE = new FileInfo(pathOfSSE);
            StreamWriter SWOfSSE = SSE.CreateText();
            foreach (var item in ListOfSSE)
            {
                SWOfSSE.Write(item.Key);
                SWOfSSE.Write("\t");
                SWOfSSE.Write(item.Value.SSE3h);
                SWOfSSE.Write("\t");
                SWOfSSE.Write(item.Value.SSE6h);
                SWOfSSE.Write("\t");
                SWOfSSE.Write(item.Value.SSE12h);
                SWOfSSE.Write("\t");
                SWOfSSE.Write(item.Value.SSE24h);
                SWOfSSE.Write("\n");
            }
        }
        private void OutputOfPredictedClock(string directory, string approach, int intervalSec)
        {
            var path = Path.Combine(directory, "PredictedBasedon" + approach + ".clk");
            ClockFileWriter ClockFileWriter = new ClockFileWriter(path, clkofPredicted);
            ClockFileWriter.SaveToFile();
            var pathOfXls = Path.Combine(directory, "PredictedBasedon" + approach + ".xls");
            FileInfo aFileOfPredicted = new FileInfo(pathOfXls);
            StreamWriter SWOfPredicted = aFileOfPredicted.CreateText();
            SWOfPredicted.Write("Time");
            SWOfPredicted.Write("\t");
            foreach (var item in clkofPredicted.Header.PrnList)
            {
                SWOfPredicted.Write(item.ToString());
                SWOfPredicted.Write("\t");
            }
            SWOfPredicted.Write("\n");
            #region 逐历元输出钟差结果
            int count = clkofPredicted.AllItems.Count / clkofPredicted.ClockCount;
            for (int i = 0; i < count; i++)
            {
                Time currentTime = clkofPredicted.First.First().Time + i * intervalSec;
                SWOfPredicted.Write(currentTime.ToDateAndHourMinitePathString());
                //按卫星逐个输出
                foreach (var item1 in clkofPredicted.Header.PrnList)
                {
                    SWOfPredicted.Write("\t");
                    var sat = clkofPredicted.GetClockItem(item1.ToString(),currentTime);
                    SWOfPredicted.Write(sat.ClockBias);                    
                }
                SWOfPredicted.Write("\n");
            }
            #endregion
        }

        private void GetClockData(bool fillWithZero, int intervalSec, int ModelLength, int PredictedLength, Time end,  SatelliteNumber prn, List<AtomicClock> all, ArrayMatrix OringalClock, ArrayMatrix CompareClock, int number,  int DataNumberIndex,  int ModelDataNumberIndex,  int PredictedDataNumberIndex)
        {
            IsNeedFilling = false;
            for (Time i = coll.TimePeriod.Start + number * 24 * 3600; i <= end; i = i + intervalSec)
            {
                var find = all.SingleOrDefault(m => m.Time == i);
                if (find == null)
                {
                    if (!fillWithZero) continue;
                    IsNeedFilling = true;
                    find = new AtomicClock() { Time = i, Prn = prn };
                }
                if (DataNumberIndex < ModelLength)
                {
                    OringalClock[ModelDataNumberIndex, 0] = find.ClockBias;
                    ModelDataNumberIndex++;
                    DataNumberIndex++;
                }
                else if (DataNumberIndex < ModelLength + PredictedLength)
                {
                    CompareClock[PredictedDataNumberIndex, 0] = find.ClockBias;
                    PredictedDataNumberIndex++;
                    DataNumberIndex++;
                }
                //writer.WriteLine(find.GetTabValues());
            }
        }
        private static SSE GetSSE(BasicFunctionModel Model)
        {
            SSE a = new SSE();
            a.SSE3h = Model.Predicted3hRms;
            a.SSE6h = Model.Predicted6hRms;
            a.SSE12h = Model.Predicted12hRms;
            a.SSE24h = Model.Predicted24hRms;
            return a;
        }    
    }
}
