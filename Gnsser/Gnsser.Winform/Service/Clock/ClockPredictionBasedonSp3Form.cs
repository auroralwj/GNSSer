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
    public partial class ClockPredictionBasedonSp3Form  : Form
    {
        public ClockPredictionBasedonSp3Form()
        {
            InitializeComponent();
        }

        private void button_getPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog_sp3.ShowDialog() == DialogResult.OK)
                this.textBox_Pathes.Lines = openFileDialog_sp3.FileNames;
        }
        private class SSE
        {
            public double SSE3h;
            public double SSE6h;
            public double SSE12h;
            public double SSE24h;
        }
        Sp3File sp3ofPredicted;
        SequentialFileEphemerisService coll;
        private void button_export_Click(object sender, EventArgs e)
        {
            bool fillWithZero = checkBox1.Checked;
            int intervalSec = (int)(double.Parse(textBox_interval.Text) * 60);
            var ModelLength = (int)(24 * double.Parse(textBox_ModelLength.Text)) * 3600 / intervalSec;
            var PredictedLength = (int)(24 * double.Parse(textBox_PredictedLength.Text)) * 3600 / intervalSec;
            var directory = this.directorySelectionControl1.Path;
            Geo.Utils.FileUtil.CheckOrCreateDirectory(directory);
            
            var PredictedNumber = int.Parse(textBox_PredictedNumber.Text);            

            string[] pathes = this.textBox_Pathes.Lines;
            EphemerisDataSourceFactory fac = new EphemerisDataSourceFactory();
            coll = new SequentialFileEphemerisService(fac, pathes);
            int allSec = (int)coll.TimePeriod.Span;            
            Time end = coll.TimePeriod.End + intervalSec;
            
            List<SatelliteNumber> prns=new List<SatelliteNumber> ();
            if (this.IsSelectedPrn.Checked) { prns = SatelliteNumber.ParsePRNsBySplliter(textBox_satPrns.Text, new char[] { ',' }); }
            else { prns = coll.Prns; }
            
            List<double> list = new List<double>();
            Dictionary<string, SSE> ListOfSSE = new Dictionary<string, SSE>();
            string approach = null;
            #region 逐个卫星进行钟差预报
            for (int number = 0; number < PredictedNumber; number++)
            {
                sp3ofPredicted = new Sp3File();
                foreach (var prn in prns)
                {
                    int GlideEpochNumber = int.Parse(this.textBox1_GlideEpochNumber.Text);//滑动历元历元数
                    ArrayMatrix OringalClock = new ArrayMatrix(ModelLength, 1);
                    ArrayMatrix CompareClock = new ArrayMatrix(PredictedLength, 1);

                    #region 获取建模，比较数据等
                    Time StartOfModel = coll.TimePeriod.Start + number * GlideEpochNumber * intervalSec;
                    Time EndOfModel = StartOfModel + (ModelLength - 1) * intervalSec;
                    Time StartOfPredicted = EndOfModel + intervalSec;
                    Time EndOfPredicted = StartOfPredicted + (PredictedLength - 1) * intervalSec;
                    var OriginalClockData = coll.Gets(prn, StartOfModel, EndOfModel);
                    var PredictedClockData = coll.Gets(prn, StartOfPredicted, EndOfPredicted);

                    int ModelDataNumberIndex = 0;
                    int PredictedDataNumberIndex = 0;
                    bool IsNeedFilling = false;
                    for (Time i = StartOfModel; i <= EndOfModel; i = i + intervalSec)
                    {
                        var findofmodel = OriginalClockData.SingleOrDefault(m => m.Time == i);
                        if (findofmodel == null)
                        {
                            if (!fillWithZero) continue;
                            IsNeedFilling = true;
                            findofmodel = new Ephemeris() { Time = i, Prn = prn };
                        }
                        OringalClock[ModelDataNumberIndex, 0] = findofmodel.ClockBias;
                        ModelDataNumberIndex++;
                    }
                    if (IsNeedFilling)
                    {
                        QuadraticPolynomialModel Filling = new QuadraticPolynomialModel();
                        Filling.FillingCalculate(OringalClock,intervalSec); 
                    }
                    for (Time i = StartOfPredicted; i <= EndOfPredicted; i = i + intervalSec)
                    {
                        var findofpredicted = PredictedClockData.SingleOrDefault(m => m.Time == i);
                        if (findofpredicted == null)
                        {
                            if (!fillWithZero) continue;

                            findofpredicted = new Ephemeris() { Time = i, Prn = prn };
                        }
                        CompareClock[PredictedDataNumberIndex, 0] = findofpredicted.ClockBias;
                        PredictedDataNumberIndex++;
                    }
                    #endregion
                    ArrayMatrix Data = null;
                    if (radioButton_QP.Checked){approach = "QP";QuadraticPolynomialModel QuadraticPolynomialModel = new QuadraticPolynomialModel(); QuadraticPolynomialModel.Calculate(OringalClock, CompareClock, intervalSec);Data = QuadraticPolynomialModel.PredictedData;}
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
                        ArrayMatrix GMOringalClock1 = new ArrayMatrix(8, 1);
                        for (int GMIndex = 0; GMIndex < 8; GMIndex++)
                        {
                            GMOringalClock1[GMIndex, 0] = OringalClock[ModelLength - (8 - GMIndex), 0];
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
                        var sat = new Ephemeris();
                        sat.Prn = prn;
                        sat.Time = StartOfPredicted + i0 * intervalSec;
                        sat.ClockBias = Data[i0, 0];
                        if (!(sp3ofPredicted.Contains(sat.Time)))
                            sp3ofPredicted.Add(sat.Time, new Sp3Section());
                        sp3ofPredicted[sat.Time].Add(sat.Prn, sat);
                    }
                    sp3ofPredicted.Prns.Add(prn);
                }
                OutputOfPredictedClock(directory, approach, number);
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

        private void OutputOfPredictedClock(string directory, string approach, int number)
        {
            sp3ofPredicted.Header = new Sp3Header();
            sp3ofPredicted.Header.StartTime = sp3ofPredicted.First.Time;
            sp3ofPredicted.Header.NumberOfEpochs = sp3ofPredicted.Count;
            sp3ofPredicted.Header.PRNs = sp3ofPredicted.Prns;
            var path = Path.Combine(directory, "PredictedBasedon" + approach + number + ".sp3");
            Sp3Writer Sp3Writer = new Sp3Writer(path, sp3ofPredicted);
            Sp3Writer.SaveToFile();
            var pathOfXls = Path.Combine(directory, "PredictedBasedon" + approach + number + ".xls");
            FileInfo aFileOfPredicted = new FileInfo(pathOfXls);
            StreamWriter SWOfPredicted = aFileOfPredicted.CreateText();
            SWOfPredicted.Write("Time");
            SWOfPredicted.Write("\t");
            foreach (var item in sp3ofPredicted.Prns)
            {
                SWOfPredicted.Write(item.ToString());
                SWOfPredicted.Write("\t");
            }
            SWOfPredicted.Write("\n");
            foreach (var item in sp3ofPredicted)
            {
                var qq = item.Time.ToDateAndHourMinitePathString();
                SWOfPredicted.Write(item.Time.ToDateAndHourMinitePathString());
                SWOfPredicted.Write("\t");
                foreach (var sat in item)
                {
                    var aa = sat.ClockBias.ToString();
                    SWOfPredicted.Write(sat.ClockBias.ToString());
                    SWOfPredicted.Write("\t");
                }
                SWOfPredicted.Write("\n");
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
