//2018.05.31, czs, create in hmx, SmoothRangeAccruceSolverForm为论文而生

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geo.IO;
using Geo;


namespace Gnsser.Winform
{
    public partial class SmoothRangeAccruceSolverForm : Form
    {
        Log log = new Log(typeof(SmoothRangeAccruceSolverForm));
        public SmoothRangeAccruceSolverForm()
        {
            InitializeComponent();
        }

        private void button_solve_Click(object sender, EventArgs e)
        {
            RinexSatFrequency satFreq = GetSatFrequence();
            int n = namedIntControl_epochCount.GetValue();
            double deltaL = satFreq.GetFrequence().WaveLength * 0.01;// 0.0019;
            double deltaP = this.namedFloatControl_deltaOfP.GetValue();
            double deltaC = namedFloatControl_deltaOfC.GetValue();
            double deltaLSquared = deltaL * deltaL;
            double deltaPSquared = deltaP * deltaP;
            double deltaCSquared = deltaC * deltaC;
            int i = n;
            double deltaPs = GetDeltaOfSmoothedRange(i, deltaP, deltaL);
            double deltaCs = GetDeltaOfSmoothedRange(i, deltaC, deltaL);
            double step = this.namedFloatControl_step.GetValue();
            double stepWeight = (i - 1) * step;

            if (i >= 100)
            {
                stepWeight = 0.01;
            }

            double normalWeight = 1.0 / i;
            double deltaStepP = GetDeltaOfWeightedSmoothRange(i, stepWeight, deltaP, deltaL);
            double deltaStepC = GetDeltaOfWeightedSmoothRange(i, stepWeight, deltaC, deltaL);

            double deltaWeightedPs = GetDeltaOfWeightedSmoothRange(i, normalWeight, deltaP, deltaL);
            double deltaWeightedCs = GetDeltaOfWeightedSmoothRange(i, normalWeight, deltaC, deltaL);
            double timesOfP = deltaP / deltaPs;
            double timesOfC = deltaC / deltaCs;
            double timesOfWeightedP = deltaP / deltaWeightedPs;
            double timesOfWeightedC = deltaC / deltaWeightedCs;
            double timesOfStepWeightedP = deltaStepP / deltaWeightedPs;
            double timesOfStepWeightedC = deltaStepC / deltaWeightedCs;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("deltaPs:\t" + deltaPs);
            sb.AppendLine("deltaCs:\t" + deltaCs);
            sb.AppendLine("deltaWeightedPs:\t" + deltaWeightedPs);
            sb.AppendLine("deltaWeightedCs:\t" + deltaWeightedCs);
            sb.AppendLine("deltaStepP:\t" + deltaStepP);
            sb.AppendLine("deltaStepC:\t" + deltaStepC);
            sb.AppendLine("timesOfP:\t" + timesOfP);
            sb.AppendLine("timesOfC:\t" + timesOfC);
            sb.AppendLine("timesOfWeightedP:\t" + timesOfWeightedP);
            sb.AppendLine("timesOfWeightedC:\t" + timesOfWeightedC);
            sb.AppendLine("timesOfStepWeightedP:\t" + timesOfStepWeightedP);
            sb.AppendLine("timesOfStepWeightedC:\t" + timesOfStepWeightedC);


            this.richTextBoxControl1.Text = sb.ToString();


        }

        private void button_multiDelta_Click(object sender, EventArgs e)
        {
            RinexSatFrequency satFreq = GetSatFrequence();

            ObjectTableStorage table = new ObjectTableStorage();

            bool isShowCA = checkBox_IsShowCA.Checked;
            double step = this.namedFloatControl_step.GetValue();
            double n = namedIntControl_epochCount.GetValue();
            var deltaL = satFreq.GetFrequence().WaveLength * 0.01;// 0.0019;
            double deltaP = this.namedFloatControl_deltaOfP.GetValue();
            double deltaC = namedFloatControl_deltaOfC.GetValue();

            var deltaLSquared = deltaL * deltaL;
            var deltaPSquared = deltaP * deltaP;
            var deltaCSquared = deltaC * deltaC;
            double stepCount = 1 / step;
            for (int i = 1; i < n; i++)
            {
                //原始
                double deltaPs = GetDeltaOfSmoothedRange(i, deltaP, deltaL);
                double deltaCs = GetDeltaOfSmoothedRange(i, deltaC, deltaL);
                //加权平均
                double normalWeight = 1.0 / i;
                double deltaWeightedPs = GetDeltaOfWeightedSmoothRange(i, normalWeight, deltaP, deltaL);
                double deltaWeightedCs = GetDeltaOfWeightedSmoothRange(i, normalWeight, deltaC, deltaL);
                //步长加权
                double stepWeight = 1 - (i - 1) * step; // stepCount;// (i - 1) * step;
                if (i >= stepCount)
                {
                    stepWeight = step;
                }
                double deltaStepP2 = GetDeltaOfStepWeightedSmoothRange(i, step, deltaP, deltaL);
                double deltaStepP = GetDeltaOfWeightedSmoothRange(i, stepWeight, deltaP, deltaL);
                double deltaStepC = GetDeltaOfWeightedSmoothRange(i, stepWeight, deltaC, deltaL);


                double timesOfP = deltaP / deltaPs;
                double timesOfC = deltaC / deltaCs;
                double timesOfWeightedP = deltaP / deltaWeightedPs;
                double timesOfWeightedC = deltaC / deltaWeightedCs;
                if (i < 1000)
                {
                    table.NewRow();
                    table.AddItem("Epoch", i);
                    table.AddItem("σRawSP", deltaPs);
                    if (isShowCA) table.AddItem("σRawSC", deltaCs);
                    table.AddItem("σWSP", deltaWeightedPs);
                    if (isShowCA) table.AddItem("σWSC", deltaWeightedCs);
                    table.AddItem("σStepSP", deltaStepP);
                    table.AddItem("σStepSP2", deltaStepP2);
                    if (isShowCA) table.AddItem("σStepSC", deltaStepC);
                }
                // 精度提高倍数
                else if (
                             //i <= 10
                             //         || (i > 10 && i <= 100 && i % 10 == 0)
                             //          || (i > 100 && i <= 1000 && i % 100 == 0)
                             //            || 
                             (i > 1000 && i % 1000 == 0)
                             || (i > 10000 && i % 10000 == 0)
                             || (i > 100000 && i % 100000 == 0))
                {
                    table.NewRow();
                    table.AddItem("Epoch", i);
                    table.AddItem("σRawSP", deltaPs);
                    if (isShowCA) table.AddItem("σRawSC", deltaCs);
                    table.AddItem("σWSP", deltaWeightedPs);
                    if (isShowCA) table.AddItem("σWSC", deltaWeightedCs);
                    table.AddItem("σStepSP", deltaStepP);
                    if (isShowCA) table.AddItem("σStepSC", deltaStepP);
                }
            }
            table.IndexColName = "Epoch";
            this.objectTableControl1.DataBind(table);
        }
        private void button_multiSolve_Click(object sender, EventArgs e)
        {
            ObjectTableStorage table = new ObjectTableStorage();


            bool isShowCA = checkBox_IsShowCA.Checked;
            RinexSatFrequency satFreq = GetSatFrequence();
            double n = namedIntControl_epochCount.GetValue();
            var deltaL = satFreq.GetFrequence().WaveLength * 0.01;// 0.0019;
            double deltaP = this.namedFloatControl_deltaOfP.GetValue();
            double deltaC = namedFloatControl_deltaOfC.GetValue();
            var deltaLSquared = deltaL * deltaL;
            var deltaPSquared = deltaP * deltaP;
            var deltaCSquared = deltaC * deltaC;
            double step = this.namedFloatControl_step.GetValue();
            for (int i = 1; i < n; i++)
            {
                double deltaPs = GetDeltaOfSmoothedRange(i, deltaP, deltaL);
                double deltaCs = GetDeltaOfSmoothedRange(i, deltaC, deltaL);

                double stepWeight = (i - 1) * step;

                if (i >= 100)
                {
                    stepWeight = 0.01;
                }

                double normalWeight = 1.0 / i;


                double deltaStepP = GetDeltaOfWeightedSmoothRange(i, stepWeight, deltaP, deltaL);
                double deltaStepC = GetDeltaOfWeightedSmoothRange(i, stepWeight, deltaC, deltaL);

                double deltaWeightedPs = GetDeltaOfWeightedSmoothRange(i, normalWeight, deltaP, deltaL);
                double deltaWeightedCs = GetDeltaOfWeightedSmoothRange(i, normalWeight, deltaC, deltaL);
                double timesOfP = deltaP / deltaPs;
                double timesOfC = deltaC / deltaCs;
                double timesOfWeightedP = deltaP / deltaWeightedPs;
                double timesOfWeightedC = deltaC / deltaWeightedCs;
                if (i < 1000)
                {
                    table.NewRow();
                    table.AddItem("Epoch", i);
                    table.AddItem("timesOfP", timesOfP);
                    if (isShowCA) table.AddItem("timesOfC", timesOfC);
                    table.AddItem("timesOfWeightedP", timesOfWeightedP);
                    if (isShowCA) table.AddItem("timesOfWeightedC", timesOfWeightedC);
                }
                // 精度提高倍数
                else if (
                             //i <= 10
                             //         || (i > 10 && i <= 100 && i % 10 == 0)
                             //          || (i > 100 && i <= 1000 && i % 100 == 0)
                             //            || 
                             (i > 1000 && i % 1000 == 0)
                             || (i > 10000 && i % 10000 == 0)
                             || (i > 100000 && i % 100000 == 0))
                {
                    table.NewRow();
                    table.AddItem("Epoch", i);
                    table.AddItem("timesOfP", timesOfP);
                    if (isShowCA) table.AddItem("timesOfC", timesOfC);
                    table.AddItem("timesOfWeightedP", timesOfWeightedP);
                    if (isShowCA) table.AddItem("timesOfWeightedC", timesOfWeightedC);
                }
            }
            table.IndexColName = "Epoch";
            this.objectTableControl1.DataBind(table);
        }
        /// <summary>
        /// 原始平滑伪距的标准差
        /// </summary>
        /// <param name="epochCount"></param> 
        /// <param name="deltaP"></param>
        /// <param name="deltaL"></param>
        /// <returns></returns>
        public double GetDeltaOfSmoothedRange(int epochCount, double deltaP = 0.3, double deltaL = 0.0019)
        {
            if (epochCount == 1) return deltaP;

            var deltaLSquared = deltaL * deltaL;
            var deltaPSquared = deltaP * deltaP;
            double sigmaOfSmoothRange = deltaLSquared + (deltaLSquared + deltaPSquared) / epochCount;
            return Math.Sqrt(sigmaOfSmoothRange);
        }
        /// <summary>
        /// 计算加权后平滑伪距的标准差，Step for check，算法验证。
        /// </summary>
        /// <param name="epochCount"></param>
        /// <param name="step"></param>
        /// <param name="deltaP"></param>
        /// <param name="deltaL"></param>
        /// <returns></returns>
        public double GetDeltaOfStepWeightedSmoothRange(int epochCount, double step, double deltaP = 0.3, double deltaL = 0.0019)
        {
            if (epochCount == 1) return deltaP;

            var deltaLSquared = deltaL * deltaL;
            var deltaPSquared = deltaP * deltaP;
            var ss = step * step;

            double prevEpochCount = (epochCount - 1);
            double sigmaOfWeightedSmoothRange =
                (Math.Pow(1 - prevEpochCount * step, 2) + prevEpochCount * ss) * deltaPSquared
                + (3 * prevEpochCount + 1) * prevEpochCount * ss * deltaLSquared;
            return Math.Sqrt(sigmaOfWeightedSmoothRange);
        }
        /// <summary>
        /// 计算加权后平滑伪距的标准差
        /// </summary>
        /// <param name="epochCount"></param>
        /// <param name="weightOfRawP"></param>
        /// <param name="deltaP"></param>
        /// <param name="deltaL"></param>
        /// <returns></returns>
        public double GetDeltaOfWeightedSmoothRange(int epochCount, double weightOfRawP, double deltaP = 0.3, double deltaL = 0.0019)
        {
            if (epochCount == 1) return deltaP;

            var deltaLSquared = deltaL * deltaL;
            var deltaPSquared = deltaP * deltaP;
            double weightOfRawPSquared = weightOfRawP * weightOfRawP;
            double temp = Math.Pow(1 - weightOfRawP, 2);
            double prevEpochCount = (epochCount - 1);
            double sigmaOfWeightedSmoothRange =
                (weightOfRawPSquared + temp / prevEpochCount) * deltaPSquared
                + temp * (epochCount / prevEpochCount + 2) * deltaLSquared;
            return Math.Sqrt(sigmaOfWeightedSmoothRange);
        }
        /// <summary>
        /// 计算加权后平滑伪距的标准差
        /// </summary>
        /// <param name="epochCount"></param>
        /// <param name="weightOfRawP"></param>
        /// <param name="deltaP"></param>
        /// <param name="deltaL"></param>
        /// <returns></returns>
        public double GetDeltaOfWeightedSmoothRangeSimple(int epochCount, double weightOfRawP, double deltaP = 0.3, double deltaL = 0.0019)
        {
            if (epochCount == 1) return deltaP;

            var deltaLSquared = deltaL * deltaL;
            var deltaPSquared = deltaP * deltaP;
            double weightOfRawPSquared = weightOfRawP * weightOfRawP;
            double temp = Math.Pow(1 - weightOfRawP, 2);
            double sigmaOfWeightedSmoothRange = (weightOfRawPSquared + temp / (epochCount - 1)) * deltaPSquared + 3 * temp * deltaLSquared;
            return Math.Sqrt(sigmaOfWeightedSmoothRange);
        }


        private void SmoothRangeAccruceSolverForm_Load(object sender, EventArgs e)
        {
            enumRadioControl_satelliteType.Init<SatelliteType>(true);
            this.enumRadioControl_frequenceTypes.Init<FrequenceType>(true);

            enumRadioControl_satelliteType.SetCurrent(SatelliteType.G);
            enumRadioControl_frequenceTypes.SetCurrent(FrequenceType.A);
        }

        private void enumRadioControl_satelliteType_EnumItemSelected(string arg1, bool arg2)
        {
            ShowFrequence();
        }

        private void enumRadioControl_frequenceTypes_EnumItemSelected(string arg1, bool arg2)
        {
            ShowFrequence();
        }

        private void ShowFrequence()
        {
            RinexSatFrequency satelliteFrequency = GetSatFrequence();
            if (satelliteFrequency == null) { return; }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("当前选中了: " + satelliteFrequency.ToString());
            if (satelliteFrequency.GetFrequence() != null)
            {
                sb.AppendLine("频率: " + satelliteFrequency.GetFrequence().ToString());
            }
            log.Info(sb.ToString());
        }

        private RinexSatFrequency GetSatFrequence()
        {
            if (enumRadioControl_frequenceTypes.IsReady && enumRadioControl_satelliteType.IsReady)
            {
                var satType = enumRadioControl_satelliteType.GetCurrent<SatelliteType>();
                var freqType = enumRadioControl_frequenceTypes.GetCurrent<FrequenceType>();
                RinexSatFrequency satelliteFrequency = new RinexSatFrequency(satType, (int)freqType);
                return satelliteFrequency;
            }
            return null;
        }

        private void button_solveWantedEpochCount_Click(object sender, EventArgs e)
        {
            var rmsSp = this.namedFloatControl_wantedRms.GetValue();
            var rmsP = namedFloatControl_deltaOfP.GetValue();
            var rmsCA = namedFloatControl_deltaOfC.GetValue();

            var countOfP = GetEpochCount(rmsSp, rmsP);
            var countOfCa = GetEpochCount(rmsSp, rmsCA);

            var info = "CountOfP: " + countOfP;
            info += " CountOfC: " + countOfCa;
            log.Info(info);
        }


        public double GetEpochCount(double deltaSP = 0.1, double deltaP = 0.3)
        {
            var deltaSpSquared = deltaSP * deltaSP;
            var deltaPSquared = deltaP * deltaP;

            double epochCount = deltaPSquared / deltaSpSquared;


            //电源线，卫生纸，牙膏，香皂
            return epochCount;
        }
    }
}