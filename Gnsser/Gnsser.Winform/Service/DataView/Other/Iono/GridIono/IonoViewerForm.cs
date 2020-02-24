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
using Geo.Draw;
using Geo.Algorithm;

namespace Gnsser.Winform
{
    /// <summary>
    /// 电离层查看
    /// </summary>
    public partial class IonoViewerForm : Form
    {
        public IonoViewerForm()
        {
            InitializeComponent();
        }

        IonoFile File;
        GridIonoFileService IonoFileService;
        ObjectTableManager mgr = new ObjectTableManager();
        ObjectTableStorage table; 
        private void button_read_Click(object sender, EventArgs e)
        {
            var path = this.fileOpenControl1.FilePath;
            IonoReader reader = new IonoReader(path);

             this.File = reader.ReadAll();
             this.IonoFileService = new Data.GridIonoFileService(File); 
             bindingSource_times.DataSource = File.Keys; 
             bindingSource_sections.DataSource = File.Values;
             mgr.Clear();
             table =  mgr.AddTable(File.Name);

             StringBuilder sb = new StringBuilder();
             sb.AppendLine("--------- Satellite  ---------");
             foreach (var item in File.DcbsOfSats)
             {
                 sb.AppendLine(item.Key + "\t" + item.Value);
             }
             sb.AppendLine("--------- Site  ---------");
             foreach (var item in File.DcbsOfSites)
             {
                 sb.AppendLine(item.Key + "\t" + item.Value);
             }


            richTextBoxControl1Dcb.Text = sb.ToString();
        }  

        private void IgsFcbViewerForm_Load(object sender, EventArgs e)
        {
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
            bindingSource_records.DataSource = section.Values;
            if (section != null )
            {
                mgr.Clear();
                this.table = section.GetTable(this.checkBox_showRMS.Checked);
                table.Name = File.Name + "_" + Geo.Utils.DateTimeUtil.GetDateTimePathString(section.Time.DateTime);
                mgr.AddTable(table);
                objectTableControl1.DataBind(table);
                //this.dataGridView_data.DataSource = table.GetDataTable();
            }
        }
           
        private void button_exportTable_Click(object sender, EventArgs e)
        {
            if (table == null)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("请先读取数据。");
                return;
            }

            mgr.WriteAllToFileAndCloseStream();
            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(mgr.OutputDirectory);
        }

        private void button_draw_Click(object sender, EventArgs e)
        {
            if (this.bindingSource_times.Current == null || File == null) { return; }

            Time time = (Time)this.bindingSource_times.Current;
            var section = File[time];

            ////var form = new TwoDimColorChartForm(section.GetGeoCoords() );
            ////form.Show();
            var form = new TwoDimColorChartForm(section.GetNumeralKeyDic() );
            form.Text = "电离层 of " + time.ToString();
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
            if (this.File == null) { return; }

            foreach (var time in File.Keys)
            {
                var form = new TwoDimColorChartForm(File[time].GetNumeralKeyDic());
                form.Text = "电离层 of " + time.ToString();
                form.Show();
            }
        }

        private void button_drawAllRms_Click(object sender, EventArgs e)
        {
            if (this.File == null) { return; }

            foreach (var time in File.Keys)
            {
                var form = new TwoDimColorChartForm(File[time].GetNumeralKeyDic(true));
                form.Text = "电离层 of " + time.ToString();
                form.Show();
            }
        } 
    }
}

