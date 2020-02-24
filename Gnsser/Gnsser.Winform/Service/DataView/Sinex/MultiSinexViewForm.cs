using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

using Gnsser.Data.Sinex;

using Geo.Utils;
using Geo.Coordinates;
using AnyInfo;

namespace Gnsser.Winform
{
    /// <summary>
    /// 批量查看SIENX，在地图上显示不同SINEX文件信息。
    /// </summary>
    public partial class MultiSinexViewForm : Form,IShowLayer
    {
        public MultiSinexViewForm()
        {
            InitializeComponent();
        }

        public event ShowLayerHandler ShowLayer;
        List<SinexFile> files = new List<SinexFile>();
        private void button_getPath_Click(object sender, EventArgs e)        {            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)  this.textBox_Path.Lines = this.openFileDialog1.FileNames;         }

        private void button_read_Click(object sender, EventArgs e)
        {
            if (this.textBox_Path.Lines.Length == 0)
            {
                FormUtil.ShowFileNotExistBox("");
                return;
            }

            files = SinexReader.Read(this.textBox_Path.Lines); 

            ShowFile();
        }

        private void ShowFile()
        {
            List<SinexSiteDetail> list = new List<SinexSiteDetail>();
            this.textBox_statistic.Text = "";
            foreach (var item in files)
            {
                this.textBox_statistic.Text += item.Name + "\r\n";
                this.textBox_statistic.Text += item.GetStatistic().ToString();
                this.textBox_statistic.Text += "----------------------------------------\r\n";

                list.AddRange(item.GetSinexSites());
            }

            this.bindingSource1.DataSource = list;
            this.bindingSource2.DataSource = list;
        }


        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            if (ShowLayer != null && files != null)
            {
                List<AnyInfo.Geometries.Point> lonlats = new List<AnyInfo.Geometries.Point>();
                List<SolutionEstimateGroup> groups = new List<SolutionEstimateGroup>();
                foreach (var item in files)
                {
                    List<SolutionEstimateGroup> group = SolutionEstimateGroup.GetGroups(item.SolutionEstimateBlock.Items);
                    foreach (SolutionEstimateGroup g in group)
                    {
                        lonlats.Add(new AnyInfo.Geometries.Point(g.GeoCoord, g.Items[0].SiteCode + "(" + item.Name + ")"));
                    }

                    groups.AddRange(group);
                }
                Layer layer = LayerFactory.CreatePointLayer(lonlats);
                ShowLayer(layer);
            }
        } 
       

        private void button_cleanNonCoord_Click(object sender, EventArgs e)
        {
            foreach (var item in files)
            {
                item.CleanNonCoordSolutionValue();
            }
            ShowFile();
        }

        private void button_toExcel_Click(object sender, EventArgs e) { Geo.Utils.ReportUtil.SaveToExcel(this.dataGridView1); }
        private void button_detailtoExcel_Click(object sender, EventArgs e) { Geo.Utils.ReportUtil.SaveToExcel(this.dataGridView2); }

       

        private void SinexViewForm_Load(object sender, EventArgs e)
        {
            this.textBox_Path.Text = Application.StartupPath + System.Configuration.ConfigurationManager.AppSettings["SampleSinexFile"];
        }

    }
}
