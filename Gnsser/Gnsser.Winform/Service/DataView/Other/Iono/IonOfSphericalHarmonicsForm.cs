//2018.05.25, czs, create, 球谐系数电离层

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Gnsser.Data;
using Geo.Coordinates;
using Geo.IO;
using Geo;
using Geo.Times;
using Geo.Draw;

namespace Gnsser.Winform
{
    public partial class IonOfSphericalHarmonicsForm : Form
    {
        Log log = new Log(typeof(IonOfSphericalHarmonicsForm));

        public IonOfSphericalHarmonicsForm()
        {
            InitializeComponent();

        }
        ObjectTableManager tables;
        SphericalHarmonicsCalculater calculater;
        ObjectTableStorage table;
        int maxOrder;
        /// <summary>
        /// 球面高
        /// </summary>
        double HeightOnSphere { get; set; }
        IonoHarmonicSection CurrentHarmanicsData;
        double AveRadiusOfEarch = 6378136.3;
        IonoHarmonicFile IonoHarmonicFile { get; set; }
        /// <summary>
        /// 是否已经结束
        /// </summary>
        public bool IsCompleted { get; set; }
        DateTime Start;
        private void button_read_Click(object sender, EventArgs e)
        {
            UiToInitParams();

            var path = this.fileOpenControl1.FilePath;
            DateTime start = DateTime.Now;

            var reader = new IonoHarmonicReader(path);
            this.IonoHarmonicFile = reader.ReadAll();


            bindingSource_epochs.DataSource = this.IonoHarmonicFile.Keys;
            SetCurrent();

            namedIntControl_order.SetValue(CurrentHarmanicsData.MaxDegree);

            label_maxDegree.Text = "最大阶数：" + CurrentHarmanicsData.MaxDegree;
            //file = reader.Read(maxOrder + 2);

            var span = DateTime.Now - start;
            log.Info("数据 " + IonoHarmonicFile.Count + " ，读取时间 " + span + " ");

            log.Info(IonoHarmonicFile.ToString());
        }

        private void SetCurrent()
        {
            if (comboBox_epochs.SelectedValue != null)
            {
                var currentEpoch = (Time)comboBox_epochs.SelectedValue;
                CurrentHarmanicsData = IonoHarmonicFile[currentEpoch];
                calculater = new SphericalHarmonicsCalculater(CurrentHarmanicsData);
                log.Info(CurrentHarmanicsData.ToString());
            }
        }

        private void UiToInitParams()
        {
            Start = DateTime.Now;
            maxOrder = namedIntControl_order.Value;
            HeightOnSphere = namedFloatControl_height.Value; 
        }
        #region 批量计算
        private void button_calculate_Click(object sender, EventArgs e)
        {
            if (calculater == null)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("请先读取数据。");
                return;
            }
            button_calculate.Enabled = false;
            UiToInitParams();
            var dir = directorySelectionControl_output.Path;
            tables = new ObjectTableManager(dir);
            table = tables.AddTable("SphericalHarmonics");

            backgroundWorker1.RunWorkerAsync();
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            UiToInitParams();

            if (CurrentHarmanicsData.Count < maxOrder)
            {
                MessageBox.Show("文件阶次不够，请重新读取！");
                return;
            }


            Start = DateTime.Now;
            gridLooper = this.geoGridLoopControl1.GetGridLooper();
            gridLooper.ProgressViewer = this.progressBarComponent1;
            IsCompleted = false;
            gridLooper.Looping += gridLooper_Looping;
            gridLooper.Completed += gridLooper_Completed;
            if (parallelConfigControl1.EnableParallel)
            {
                gridLooper.ParallelableParam = parallelConfigControl1;
            }
            gridLooper.Init();
            gridLooper.Run();
        }
        GeoGridLooper gridLooper;
        void gridLooper_Completed(object sender, EventArgs EventArgs)
        {
            tables.WriteAllToFileAndCloseStream();
            IsCompleted = true;
            var span = DateTime.Now - Start;
            log.Info("计算完毕！ 耗时 ： " + span);          
        }
        static object locker = new object();

        void gridLooper_Looping(Geo.Coordinates.LonLat lonlat)
        {

            //转换为日固坐标系
            LonLat lonLatSE = lonlat.GetSeLonLat(this.CurrentHarmanicsData.Time.DateTime);


            var radius = AveRadiusOfEarch + HeightOnSphere;

         //   var val = calculater.GetValue(maxOrder, lonlat, radius);
            var val = calculater.GetValue(maxOrder, lonLatSE, radius);


            AddToTable(lonlat, val);
        }

        private void AddToTable(LonLat lonlat, double val)
        {
            lock (locker)
            { 
                table.NewRow();
                table.AddItem("Lon", lonlat.Lon);
                table.AddItem("Lat", lonlat.Lat);
                table.AddItem("Val", val);
            }
        }

        private void SphericalHarmonicsForm_Load(object sender, EventArgs e)
        {
            this.fileOpenControl1.FilePath = @"D:\Codes\CSharp\Gnsser\Gnsser\Gnsser.Winform\bin\Debug\Data\Sample\COD19821.ION";
            directorySelectionControl_output.Path = Setting.TempDirectory;
            namedStringControlCoord.SetValue("120.10, 40.10");
        }

        private void button_draw_Click(object sender, EventArgs e)
        {
            if (table == null) { MessageBox.Show("请先计算！"); return; }
            if (!IsCompleted) { MessageBox.Show("请稍等，还没有计算完毕！"); return; }
            var valueName = "Val";
            var data = table.GetNumeralKeyDic("Lon", "Lat", valueName);

            TwoDimColorChartForm form = new TwoDimColorChartForm(data, null);
            form.Text =   CurrentHarmanicsData.ToString();
            form.Show();
        }

        private void buttonWrite_Click(object sender, EventArgs e)
        {
            if (CurrentHarmanicsData == null) { MessageBox.Show("请先读取！"); return; }
            var dir = directorySelectionControl_output.Path;
            var path = Path.Combine(dir, "harmonics.dat");
            SphericalHarmonicsWriter writer = new SphericalHarmonicsWriter(path);
            writer.Write(this.CurrentHarmanicsData);
            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(dir);
        }
        private void button_isCancel_Click(object sender, EventArgs e)
        {
            if (gridLooper != null)
            {
                gridLooper.IsCancel = true;
            }
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button_calculate.Enabled = true;
        }
        #endregion

        #region 单站计算
        private void buttonSinglePt_Click(object sender, EventArgs e)
        {
            buttonSinglePt.Enabled = false;
            UiToInitParams();
            if (CurrentHarmanicsData.Count < maxOrder)
            {
                MessageBox.Show("文件阶次不够，请重新读取！");
                return;
            }
            backgroundWorker2.RunWorkerAsync();
        }


        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            if (calculater == null)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("请先读取数据。");
                return;
            }
            var coordStr = this.namedStringControlCoord.GetValue();
            double val = 0;
            if (this.radioButtonLonLat.Checked)
            {
                var radius = HeightOnSphere + AveRadiusOfEarch;
                LonLat lonlat = LonLat.Parse(coordStr);
                //转换为日固坐标系
                LonLat lonLatSE = lonlat.GetSeLonLat(this.CurrentHarmanicsData.Time.DateTime);

                val = calculater.GetValue(maxOrder, lonLatSE, radius);
            }
            else
            {
                XYZ xyz = XYZ.Parse(coordStr);
                var polar = Geo.Coordinates.CoordTransformer.XyzToPolar(xyz);
                var lonlat = new LonLat(polar.Azimuth, polar.Elevation);

                LonLat lonLatSE = lonlat.GetSeLonLat(this.CurrentHarmanicsData.Time.DateTime);

                val = calculater.GetValue(maxOrder, lonLatSE, polar.Range);
            }
            var span = DateTime.Now - Start;
            Geo.Utils.FormUtil.SetText(this.richTextBoxControl1, "计算值：" + val + "，耗时 ： " + span + "");
            log.Info("计算完毕！ ，计算值：" + val + "，耗时 ： " + span + "");

        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            buttonSinglePt.Enabled = true;
        }

        #endregion

        private void comboBox_epochs_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetCurrent();
        }
    }
}
