//2017.08.17, czs, create in hongqing, 电离层产品读取查看器
//2017.10.16, czs, edit in hongqing, 增加绘图，批量绘图

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Gnsser.Data.Rinex;
using Geo.Coordinates;
using AnyInfo;
using Geo.Times;
using Gnsser.Data;
using System.Drawing.Drawing2D;
using System.Windows.Forms.DataVisualization.Charting;
using Gnsser; 
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo;
using Geo.Algorithm;
using Geo.Draw;
using Geo.IO;

namespace Gnsser.Winform
{
    public partial class IonoServiceForm : Form
    {
        Log log = new Log(typeof(IonoServiceForm));
        public IonoServiceForm()
        {
            InitializeComponent();
          
        }
        IonoFile File;
        GridIonoFileService IonoFileService; 
        ObjectTableStorage CurrentTable;
        private void button_read_Click(object sender, EventArgs e)
        {
            var path = this.fileOpenControl1.FilePath;
            IonoReader reader = new IonoReader(path);

             this.File = reader.ReadAll();
             this.IonoFileService = new Data.GridIonoFileService(File);

             this.namedTimeControl1.SetValue(File.TimePeriod.Start.DateTime);
             namedTimeControl2.SetValue(File.TimePeriod.Start.DateTime);

             bindingSource_times.DataSource = File.Keys; 
             bindingSource_sections.DataSource = File.Values;
    
             log.Info("加载完毕！" + path);
        } 
        private void IgsFcbViewerForm_Load(object sender, EventArgs e)
        {
            this.namedStringControl1_geoCoord.SetValue("120.0,30.0");
            this.fileOpenControl1.FilePath = Setting.GnsserConfig.IonoFilePath;
        }

        private void button_filterTime_Click(object sender, EventArgs e)
        {
            bindingSource_times_CurrentChanged(null, null);
       }
         

        private void bindingSource_times_CurrentChanged(object sender, EventArgs e)
        {
            if (this.bindingSource_times.Current == null)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("请先读取数据。");
                return;
            }
            Time time = (Time)this.bindingSource_times.Current;
            var section = File[time];
            bindingSource_lats.DataSource = section.Keys;
            bindingSource_records.DataSource = section.Values;
            if (section != null)
            { 
                this.CurrentTable = section.GetTable(this.checkBox_showRMS.Checked);
                CurrentTable.Name = File.Name + "_" + Geo.Utils.DateTimeUtil.GetDateTimePathString(section.Time.DateTime);
              
                objectTableControl1.DataBind(CurrentTable);
             //   this.dataGridView_data.DataSource = table.GetDataTable();
            }
        }

        private void button_multiCalcu_Click(object sender, EventArgs e)
        {
            tableData = new TwoNumeralKeyAndValueDictionary();
            tableRmsData = new TwoNumeralKeyAndValueDictionary();

            time = new Time(this.namedTimeControl2.GetValue());
            var looper = this.geoGridLoopControl1.GetGridLooper();
            looper.ProgressViewer = this.progressBarComponent1;
            looper.Looping += looper_Looping;
            looper.Completed += looper_Completed;
            looper.Init();
            looper.Run();

        }

        void looper_Completed(object obj, EventArgs EventArgs)
        {
            tableData.Init();
            tableRmsData.Init();
            this.CurrentTable = tableData.GetTable("Lat", false);
            CurrentTable.Name = "Iono_Calculated_" + time.ToPathString();

            objectTableControl1.DataBind(CurrentTable);
            log.Info("批量计算完成！");
            MessageBox.Show("批量计算完成！");
        }

        TwoNumeralKeyAndValueDictionary tableData;

        TwoNumeralKeyAndValueDictionary tableRmsData;
        Time time;
        void looper_Looping(LonLat geoCoord)
        {  
            var result = this.IonoFileService.Get(time, geoCoord);
            tableData.Set(geoCoord.Lon, geoCoord.Lat, result.Value);
            tableRmsData.Set(geoCoord.Lon, geoCoord.Lat, result.Rms);
          //  this.richTextBoxControl1.Text = result.ToString();
        } 

        private void bindingSource_lats_CurrentChanged(object sender, EventArgs e)
        {
            //Time time = (Time)this.bindingSource_times.Current;
            //var section = File[time];
            //double lat = (double)this.bindingSource_lats.Current;
            //var record = section[lat];
            //bindingSource_lats.DataSource = section.Keys;
        }

        private void button_calculate_Click(object sender, EventArgs e)
        {
            Time time = new Time(this.namedTimeControl1.GetValue());
            LonLat geoCoord = LonLat.Parse(this.namedStringControl1_geoCoord.GetValue());
           var result =   this.IonoFileService.Get(time, geoCoord);

           this.richTextBoxControl1.Text = result.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        #region  绘图

        private void button_drawMultiCacu_Click(object sender, EventArgs e)
        {
            if (tableData == null) { return; }

            var form = new TwoDimColorChartForm(tableData);
            form.Text = "电离层 Of 计算结果 " + time;
            form.Show();
        }

        private void button_drawMultiRms_Click(object sender, EventArgs e)
        {
            if (tableRmsData == null) { return; }

            var form = new TwoDimColorChartForm(tableRmsData);
            form.Text = "电离层 RMS Of 计算结果 " + time;
            form.Show();
        }
        private void button_draw_Click(object sender, EventArgs e)
        {
            if (this.bindingSource_times.Current == null || File ==null) { return; }

            Time time = (Time)this.bindingSource_times.Current;
            var section = File[time];

            ////var form = new TwoDimColorChartForm(section.GetGeoCoords() );
            ////form.Show();
            var form = new TwoDimColorChartForm(section.GetNumeralKeyDic());
            form.Text = "电离层 Of " + time.ToString();
            form.Show();
        }

        private void button_drawRms_Click(object sender, EventArgs e)
        {
            if (this.bindingSource_times.Current == null || File == null) { return; }

            Time time = (Time)this.bindingSource_times.Current;
            var section = File[time];

            ////var form = new TwoDimColorChartForm(section.GetGeoCoords() );
            ////form.Show();
            var form = new TwoDimColorChartForm(section.GetNumeralKeyDic(true));
            form.Text = "电离层 RMS " + time.ToString();
            form.Show();
        }
        private void button_drawAll_Click(object sender, EventArgs e)
        {
            if (File == null) { return; }

            foreach (var time in File.Keys)
            {
                var form = new TwoDimColorChartForm(File[time].GetNumeralKeyDic());
                form.Text = "电离层 Of " + time.ToString();
                form.Show();
            }
        }

        private void button_drawAllRms_Click(object sender, EventArgs e)
        {
            if (File == null) { return; }

            foreach (var time in File.Keys)
            {
                var form = new TwoDimColorChartForm(File[time].GetNumeralKeyDic(true));
                form.Text = "电离层 RMS Of " + time.ToString();
                form.Show();
            }
        }
        #endregion

        private void button_exortTable_Click(object sender, EventArgs e)
        {
            if (objectTableControl1.TableObjectStorage == null) { return; }
            var TableManager = new ObjectTableManager();
            TableManager.Add(objectTableControl1.TableObjectStorage);
            TableManager.WriteAllToFileAndCloseStream();
            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(TableManager.OutputDirectory);
        }


    }
}

