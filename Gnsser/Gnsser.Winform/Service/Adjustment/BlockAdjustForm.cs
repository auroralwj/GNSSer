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
using Gnsser.Service; 

namespace Gnsser.Winform
{
    public partial class BlockAdjustForm : Form, Gnsser.Winform.IShowLayer
    {
        public event ShowLayerHandler ShowLayer;

        public BlockAdjustForm()
        {
            InitializeComponent();
            this.label_processorCount.Text = Environment.ProcessorCount.ToString();
            this.textBox_degreeOfParallel.Text = this.label_processorCount.Text;
        }

        private void button_setBlockFilesPath_Click(object sender, EventArgs e) { if (openFileDialog1.ShowDialog() == DialogResult.OK)  this.textBox_rinexPathes.Lines = openFileDialog1.FileNames; }
        private void checkBox_parallel_CheckedChanged(object sender, EventArgs e) { panel_parallel.Visible = checkBox_parallel.Checked; }

        private void button_solve_Click(object sender, EventArgs e)
        {
            //读取
            string[] filePathes = this.textBox_rinexPathes.Lines;
            List<SinexFile> sinexFiles = SinexReader.Read(filePathes);
            adjust = new SinexBlockAdjust(sinexFiles.ToArray());

            this.textBox_resultSinex.Text = adjust.ResultSinexFile.ToString();
            this.textBox_info.Text = adjust.ToString();
        }
        SinexBlockAdjust adjust;
        private void BlockAdjustForm_Load(object sender, EventArgs e)
        {
            this.textBox_rinexPathes.Lines = new string[]{
                //Application.StartupPath + System.Configuration.ConfigurationManager.AppSettings["SampleSinexFile"],
                Application.StartupPath + System.Configuration.ConfigurationManager.AppSettings["SampleSinexFile2"],
                Application.StartupPath + System.Configuration.ConfigurationManager.AppSettings["SampleSinexFile3"]
            };
        }

        private void button_saveSnx_Click(object sender, EventArgs e) { if (saveFileDialog1.ShowDialog() == DialogResult.OK)  File.WriteAllText(saveFileDialog1.FileName, this.textBox_resultSinex.Text); }

        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            if (ShowLayer != null && adjust != null)
            {
                List<AnyInfo.Geometries.Point> lonlats = new List<AnyInfo.Geometries.Point>();
                List<SolutionEstimateGroup> group = SolutionEstimateGroup.GetGroups(adjust.ResultSinexFile.SolutionEstimateBlock.Items);

                foreach (SolutionEstimateGroup g in group)
                {
                    lonlats.Add(new AnyInfo.Geometries.Point(g.GeoCoord, g.Items[0].SiteCode));
                }
                Layer layer = LayerFactory.CreatePointLayer(lonlats);
                ShowLayer(layer);
            }
        }

    }
}
