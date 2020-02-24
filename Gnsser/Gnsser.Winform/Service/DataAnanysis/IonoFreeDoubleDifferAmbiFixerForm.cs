//2018.11.07, czs, create in hmx, 无电离层组合双差模糊度固定

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geo;
using Geo.IO;
using Gnsser.Correction;
using Gnsser.Data.Rinex;

namespace Gnsser.Winform
{
    public partial class IonoFreeDoubleDifferAmbiFixerForm : Form
    {
        Log log = new Log(typeof(IonoFreeDoubleDifferAmbiFixerForm));
        public IonoFreeDoubleDifferAmbiFixerForm()
        {
            InitializeComponent();
            fileOpenControl_floatAmbiFiles.Filter = Setting.TextTableFileFilter;
            fileOpenContol_obsFiles.Filter = Setting.RinexOFileFilter;
            fileOpenContol_obsFiles.FilePathes = new string[] { Gnsser.Setting.GnsserConfig.SampleOFileA, Gnsser.Setting.GnsserConfig.SampleOFileB };
        }

        public List<SatelliteType> SatelliteTypes { get => multiGnssSystemSelectControl1.SatelliteTypes; }

        double AngleCut { get { return double.Parse(textBox_angleCut.Text); } }
        private void button_run_Click(object sender, EventArgs e)
        {
            button_run.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
        }
        private void Builder_OneFileProcessed()
        {
            this.progressBarComponent1.PerformProcessStep();
        }

        private void fileOpenControl1_FilePathSetted(object sender, EventArgs e)
        {
            var path = this.fileOpenContol_obsFiles.FilePath;
            if (File.Exists(path))
            {
                var file = Gnsser.Data.Rinex.RinexObsFileReader.Read(path, false);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var maxCycleDifferOfIntFloat = namedFloatControl1_maxDifferOfIntFloat.GetValue();
               var oPathes = fileOpenContol_obsFiles.FilePathes;
            var floatAmbiPathes = fileOpenControl_floatAmbiFiles.FilePathes;
            var IsPhaseInMetterOrCycle = this.checkBoxIsPhaseInMetterOrCycle.Checked;
            var outDir = Setting.TempDirectory;
            var isPhaseSmoothRange = checkBox_phaseSmoothRange.Checked;
            progressBarComponent1.Init(new List<string>() { "各站生成MW并平滑", "提取时段MW均值", "读取模糊度浮点解" }, oPathes.Length);

            double maxMwRms = 0.3;         //MW 至少的平滑精度
           // double maxDifferOfIntFloat = 0.2; //MW 取整时允许最大的偏差

            //------------------各站生成MW-----------
            var mwBbuilder = new MwTableBuilder(oPathes, AngleCut, namedIntControl_emptyRowCount.Value, outDir, this.SatelliteTypes, false);
            mwBbuilder.IsSmoothRange = isPhaseSmoothRange;
            mwBbuilder.OneFileProcessed += Builder_OneFileProcessed;
            var smothedMw = mwBbuilder.Build();
            //输出
            log.Info("MW 生成完毕。");
            //输出
            //mwBbuilder.SmoothedMwValue.WriteAllToFileAndClearBuffer(mwBbuilder.SmoothedMwValue.First);
            //mwBbuilder.RawMwValue.WriteAllToFileAndClearBuffer(mwBbuilder.RawMwValue.First);

            //---------------------提取时段MW均值-------------
            progressBarComponent1.InitNextClassProcessCount(smothedMw.Count);
            var periodMwAmbis = new BaseDictionary<string, PeriodRmsedNumeralStoarge>();
            foreach (var item in smothedMw.KeyValues)
            {
                var extractor = new PeriodParamExtractor(item.Value, null, 1);
                var peroidAmbi = extractor.Build();
                var siteName = item.Key.Substring(0, 4).ToUpper();
                periodMwAmbis[siteName] = peroidAmbi;

                progressBarComponent1.PerformProcessStep();
            }

            //--------------------读取模糊度浮点解--------
            progressBarComponent1.InitNextClassProcessCount(floatAmbiPathes.Length);
            var floatAmbiTables = IonoFreeNetDoubleDifferResultReader.ReadAmbiResultInDefaultUnit(floatAmbiPathes, true);

            //-----------提取时段浮点解--------------------
            var floatAmbiStorages = new BaseDictionary<string, PeriodRmsedNumeralStoarge>();
            foreach (var item in floatAmbiTables.KeyValues)
            {
                var extractor = new PeriodParamExtractor(item.Value, null, 1);
                var peroidAmbi = extractor.Build();
                floatAmbiStorages[item.Key] = peroidAmbi;

                progressBarComponent1.PerformProcessStep();
            }

            //----------------计算双差模糊度固定值------------------
            var result = new BaseDictionary<string, PeriodRmsedNumeralStoarge>("双差模糊度固定值", new Func<string, PeriodRmsedNumeralStoarge>(key => new PeriodRmsedNumeralStoarge()));
            foreach (var floatAmbiKv in floatAmbiStorages.KeyValues)
            {
                var siteFileName = floatAmbiKv.Key.Substring(0, 4).ToUpper();
                var wmPeriod = periodMwAmbis[siteFileName];

                var siteResult = result.GetOrCreate(siteFileName);

                foreach (var periodSto in floatAmbiKv.Value.KeyValues)
                {
                    var paramName = periodSto.Key;//参数
                    var paraNameSpliter = new ParamNameSpliter(paramName);
                    if (paraNameSpliter.ParamDifferType != ParamDifferType.Double) { continue; }

                    var refWmPeriod = periodMwAmbis[paraNameSpliter.RefSiteName];

                    var paramResult = siteResult.GetOrCreate(paramName);

                    foreach (var kv in periodSto.Value.KeyValues)
                    {
                        var timePeriod = kv.Key;
                        var ambiFloatVal = kv.Value.Value;
                        if (!IsPhaseInMetterOrCycle)
                        {
                            ambiFloatVal = ambiFloatVal * Frequence.GpsL1.WaveLength;
                        }

                        //计算MW双差，
                       var differMwVal =  wmPeriod.GetDifferValue(timePeriod.Median, paraNameSpliter.Prn.ToString(), paraNameSpliter.RefPrn.ToString());
                       var refDifferMwVal = refWmPeriod.GetDifferValue(timePeriod.Median, paraNameSpliter.Prn.ToString(), paraNameSpliter.RefPrn.ToString());
                        var doubleMwDiffer = differMwVal - refDifferMwVal;
                        var intDoubleMwDiffer = Math.Round(doubleMwDiffer.Value);
                        var intDiffer = Math.Abs(intDoubleMwDiffer - doubleMwDiffer.Value);
                        if (intDiffer > maxCycleDifferOfIntFloat)
                        {
                            log.Warn("宽巷整数与浮点数差 " + intDiffer + " > " + maxCycleDifferOfIntFloat + ",取消固定 " + paramName  + ", " + timePeriod);
                            continue;
                        }
                        
                        var narrowFloat = GetNarrowFloatValue(intDoubleMwDiffer, ambiFloatVal);
                        var intNarrow = Math.Round(narrowFloat);
                        intDiffer = Math.Abs(intNarrow - narrowFloat);
                        if (intDiffer > maxCycleDifferOfIntFloat)
                        {
                            log.Warn("窄巷整数与浮点数差 " + intDiffer + " > " + maxCycleDifferOfIntFloat + ",取消固定 " + paramName + ", " + timePeriod);
                            continue;
                        }
                        
                        //判断是否超限

                        //计算双差载波模糊度固定值
                        var fixedVal = GetIonoFreeAmbiValueLen(intDoubleMwDiffer, intNarrow);

                        paramResult[timePeriod] = new RmsedNumeral(fixedVal, 1e-8);
                    }
                }
            }

            //输出结果
            foreach (var item in result.KeyValues)
            {
                var table = item.Value.ToTable();
                var path = Path.Combine(Setting.TempDirectory, item.Key+Geo.Setting.AmbiguityFileExtension);
                ObjectTableWriter.Write(table, path);
            }


            //var floatAmbiSotrage = IonoFreeNetDoubleDifferResultReader.ReadToEpochStorage(floatAmbiTables);
            // floatAmbiSotrage.GetRawDiffer()

            this.Invoke(new Action(() =>
            {
                objectTableControlA.DataBind(mwBbuilder.SmoothedMwValue.First);
                objectTableControlB.DataBind(floatAmbiTables.First);
                objectTableControlC.DataBind(floatAmbiStorages.First.ToTable());
                objectTableControlD.DataBind(result.First.ToTable());
            }
            ));

            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(outDir);
        }
        /// <summary>
        /// 计算窄巷模糊度浮点解
        /// </summary>
        /// <param name="wideIntCyle"></param>
        /// <param name="floatIfAmbiLen"></param>
        /// <returns></returns>
        public double GetNarrowFloatValue(double wideIntCyle, double floatIfAmbiLen)
        {
            //基本参数定义
            var CurrentBasePrn = SatelliteNumber.G01;
            var time = Geo.Times.Time.Now;

            var f1 = Frequence.GetFrequenceA(CurrentBasePrn, time).Value;// 1575.42;
            var f2 = Frequence.GetFrequenceB(CurrentBasePrn, time).Value; //  1227.60;
            var wideWaveLen = Frequence.GetMwFrequence(CurrentBasePrn, time).WaveLength;
            var narrowWaveLen = Frequence.GetNarrowLaneFrequence(CurrentBasePrn, time).WaveLength;

            var tempCoeef = f2 / (f1 + f2);                          //算法1
            var tempCoeef2 = (f1 + f2) * 1e6 / GnssConst.LIGHT_SPEED;//算法2：单位转换,窄巷波长的倒数
            var tempCoeef3 = f2 / (f1 - f2);                         //算法2

            var narrowFloat = (floatIfAmbiLen - tempCoeef * wideWaveLen * wideIntCyle) / narrowWaveLen;//算法1
            return narrowFloat;
        }

        /// <summary>
        /// 计算无电离层组合模糊度
        /// </summary>
        /// <param name="wideIntCyle"></param>
        /// <param name="narrowIntCyle"></param>
        /// <returns></returns>
        public double GetIonoFreeAmbiValueLen(double wideIntCyle, double narrowIntCyle)
        {
            //基本参数定义
            var CurrentBasePrn = SatelliteNumber.G01;
            var time = Geo.Times.Time.Now;

            var f1 = Frequence.GetFrequenceA(CurrentBasePrn, time).Value;// 1575.42;
            var f2 = Frequence.GetFrequenceB(CurrentBasePrn, time).Value; //  1227.60;
            var wideWaveLen = Frequence.GetMwFrequence(CurrentBasePrn, time).WaveLength;
            var narrowWaveLen = Frequence.GetNarrowLaneFrequence(CurrentBasePrn, time).WaveLength;

            var tempCoeef = f2 / (f1 + f2);                          //算法1
            var tempCoeef2 = (f1 + f2) * 1e6 / GnssConst.LIGHT_SPEED;//算法2：单位转换,窄巷波长的倒数
            var tempCoeef3 = f2 / (f1 - f2);                         //算法2

            var IonoFreeAmbiValue = (tempCoeef * wideWaveLen * wideIntCyle) + (narrowIntCyle * narrowWaveLen);//算法1
            return IonoFreeAmbiValue;
        }


        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button_run.Enabled = true;
        }

        private void IonoFreeDoubleDifferAmbiFixerForm_Load(object sender, EventArgs e)
        {
            fileOpenContol_obsFiles.FilePathes = new string[] { Setting.GnsserConfig.SampleOFileA, Setting.GnsserConfig.SampleOFileB };
        }

        private void fileOpenContol_obsFiles_Load(object sender, EventArgs e)
        {

        }
    }
}
