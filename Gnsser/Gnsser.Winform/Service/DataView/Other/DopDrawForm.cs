//2017.10.15, czs, create in hongqing, 电离层产品读取查看器
//2017.10.15, czs, edit in hongqing, 改进为批量绘制
//2019.04.25, czs, edit in hongqing, 增加绘图范围控制

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
using Geo.IO;
using Geo.Algorithm;
using Geo.Draw;

namespace Gnsser.Winform
{
    public partial class DopDrawForm : Form
    {
        Log log = new Log(typeof(DopDrawForm));
        public DopDrawForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 数据
        /// </summary>
        List<ObjectTableStorage> Tables { get; set; }
        private void button_read_Click(object sender, EventArgs e)
        {
            Tables = new List<ObjectTableStorage>();
            int i = 0;
            foreach (var path in fileOpenControl1.FilePathes)
            { 
                ObjectTableReader reader = new ObjectTableReader(path);
                var table = reader.Read();
                if (i == 0)
                {
                    this.objectTableControl1.DataBind(table);
                }
                Tables.Add(table);
                i++;
            }         
        } 

        private void IgsFcbViewerForm_Load(object sender, EventArgs e)
        {
            this.fileOpenControl1.FilePath = Setting.GnsserConfig.DopFilePath;
        }

        private void button_draw_Click(object sender, EventArgs e)
        {       
            var valueName = "SatCount";
            DrawChart(valueName);       
        }

        private void DrawChart(string valueName)
        {
            if (Tables == null) { MessageBox.Show("请先读取！"); return; }
            var maxVal = enabledFloatControl_max.Value;
            var minVal = enabledFloatControl_min.Value;
            var enableMax = enabledFloatControl_max.GetEnabledValue().Enabled;
            var enableMin = enabledFloatControl_min.GetEnabledValue().Enabled;

            foreach (var table in Tables)
            {
                var data = table.GetNumeralKeyDic("Lon", "Lat", valueName);
                var span = data.ValueSpan;
                if (enableMax)
                {
                    log.Info("数值范围 " + span + ", 最大值调整到 “" + maxVal + "” ");
                    span.End = maxVal;
                }
                if (enableMin)
                {
                    log.Info("数值范围 " + span + ", 最小值调整到 “" + minVal + "” ");
                    span.Start = minVal;
                }
                TwoDimColorChartForm form = new TwoDimColorChartForm(data, span);
                form.Text = valueName + " of " + table.Name;
                form.Show();
            }
        }

        private void button_drawRms_Click(object sender, EventArgs e)
        {
            DrawChart("GDOP");     
        }

        private void button_pod_Click(object sender, EventArgs e)
        {
            DrawChart("PDOP");     
        }

        private void button_hdop_Click(object sender, EventArgs e)
        {
            DrawChart("HDOP");     
        }

        private void buttonvdop_Click(object sender, EventArgs e)
        {
            DrawChart("VDOP");  
        }

        private void button_tdop_Click(object sender, EventArgs e)
        {
            DrawChart("TDOP");  
        } 
    }
}

