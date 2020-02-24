using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

using Gnsser.Service; 
using Gnsser.Data.Sinex;

using Geo.Utils;
using Geo.Coordinates;
using AnyInfo;

namespace Gnsser.Winform
{
    /// <summary>
    ///  具有公共点的子网进行联合平差。
    /// </summary>
    public partial class SubNetUnionForm : Form, Gnsser.Winform.IShowLayer
    {
        public SubNetUnionForm()
        {
            InitializeComponent();
        }

        public event ShowLayerHandler ShowLayer;

        private void SubNetUnionForm_Load(object sender, EventArgs e)
        {
            this.textBox_Path.Lines = new string[]{
                Application.StartupPath + System.Configuration.ConfigurationManager.AppSettings["SampleSinexFile"],
                Application.StartupPath + System.Configuration.ConfigurationManager.AppSettings["SampleSinexFile2"],
                Application.StartupPath + System.Configuration.ConfigurationManager.AppSettings["SampleSinexFile3"]
            };
        }
        private void button_getPath_Click(object sender, EventArgs e) { if (this.openFileDialog1.ShowDialog() == DialogResult.OK)  this.textBox_Path.Lines = this.openFileDialog1.FileNames; }

        private void button_read_Click(object sender, EventArgs e) { Read(); }

        SinexFile current = null;
        private void Read()
        {
            string[] filePathes = this.textBox_Path.Lines;
            List<SinexFile> sinexFiles = SinexReader.Read(filePathes);
            current = null;

            foreach (var item in sinexFiles)
            {
                if (current == null) current = item;
                else
                { 
                    SinexSubNetsUnion just = new SinexSubNetsUnion(current, item);
                    current = just.ResultSinexFile;
                }
            }
            this.textBox_info.Text = current.ToString();             
        }

        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            if (ShowLayer != null && current != null)
            {
                List<AnyInfo.Geometries.Point> lonlats = new List<AnyInfo.Geometries.Point>();
                List<SolutionEstimateGroup> group = SolutionEstimateGroup.GetGroups(current.SolutionEstimateBlock.Items);

                foreach (SolutionEstimateGroup g in group)
                {
                    lonlats.Add(new AnyInfo.Geometries.Point(g.GeoCoord, g.Items[0].SiteCode));
                }
                Layer layer = LayerFactory.CreatePointLayer(lonlats);
                ShowLayer(layer);
            }
        }

        private void button_saveTofile_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, this.textBox_info.Text);

                FormUtil.ShowIfOpenDirMessageBox(saveFileDialog1.FileName);
            }
        }
    }
}
