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
    public partial class SinexViewForm : Form,IShowLayer
    {
        public SinexViewForm()
        {
            InitializeComponent();
        }

        public event ShowLayerHandler ShowLayer;
        SinexFile file;
        private void button_getPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_Path.Text = this.openFileDialog1.FileName;  
               //  CreateFromODir();
            }
        }

        private void button_read_Click(object sender, EventArgs e)
        {
            string path = this.textBox_Path.Text;
            if (!File.Exists(path))
            {
                FormUtil.ShowFileNotExistBox(path);
                return;
            }

            file = SinexReader.Read(path, this.checkBox_notReadMatix.Checked);

            ShowFile();
        }

        private void ShowFile()
        {          
            if (!this.checkBox_notShowTxt.Checked)
                this.textBox_info.Text = file.ToString();
            this.textBox_statistic.Text = file.GetStatistic().ToString();

            List<SinexSiteDetail> list = file.GetSinexSites();

            this.bindingSource1.DataSource = list;
            this.bindingSource2.DataSource = list;
        }


        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            if (ShowLayer != null && file != null)
            {
                List<AnyInfo.Geometries.Point> lonlats = new List<AnyInfo.Geometries.Point>();
                List<SolutionEstimateGroup> group = SolutionEstimateGroup.GetGroups(file.SolutionEstimateBlock.Items);

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

        private void button_outport_matrix_Click(object sender, EventArgs e)
        {
            if (!file.HasAprioriCovaMatrix) { MessageBox.Show("先验协方差为空！"); return; }
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "AprioriCova.txt";
            saveFileDialog1.Filter = "文本文档(*.txt)|*.txt";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MatrixUtil.Print(new FileStream(saveFileDialog1.FileName, FileMode.Create),
                    file.GetAprioriCovaMatrix());
                FormUtil.ShowIfOpenDirMessageBox(saveFileDialog1.FileName);
            }
        }

        private void button_exportEstimateLCova_Click(object sender, EventArgs e)
        {
            if (!file.HasEstimateCovaMatrix) { MessageBox.Show("估值协方差为空！"); return; }

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "EstimateCova.txt";
            saveFileDialog1.Filter = "文本文档(*.txt)|*.txt";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MatrixUtil.Print(new FileStream(saveFileDialog1.FileName, FileMode.Create),
                    file.GetEstimateCovaMatrix());
                FormUtil.ShowIfOpenDirMessageBox(saveFileDialog1.FileName);
            }
        }

        private void button_cleanNonCoord_Click(object sender, EventArgs e)
        {
            file.CleanNonCoordSolutionValue();
            ShowFile();
        }

        private void button_toExcel_Click(object sender, EventArgs e) { Geo.Utils.ReportUtil.SaveToExcel(this.dataGridView1); }

        private void button_detailtoExcel_Click(object sender, EventArgs e) { Geo.Utils.ReportUtil.SaveToExcel(this.dataGridView2); }

        private void button_exportNormalCoeff_Click(object sender, EventArgs e)
        {
            if (!file.HasNormalEquationMatrix) { MessageBox.Show("法方程系数阵为空！"); return; }

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "NormalCoeff.txt";
            saveFileDialog1.Filter = "文本文档(*.txt)|*.txt";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MatrixUtil.Print(new FileStream(saveFileDialog1.FileName, FileMode.Create),
                    file.GetNormalEquationMatrix());
                FormUtil.ShowIfOpenDirMessageBox(saveFileDialog1.FileName);
            }
        }

        private void button_exportNormaRightHand_Click(object sender, EventArgs e)
        {
            if (!file.HasNormalEquationVectorMatrix) { MessageBox.Show("法方程右手边为空！"); return; }

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "NormaRightHand.txt";
            saveFileDialog1.Filter = "文本文档(*.txt)|*.txt";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MatrixUtil.Print(new FileStream(saveFileDialog1.FileName, FileMode.Create),
                   MatrixUtil.Create( file.GetNormalEquationVector()));
                FormUtil.ShowIfOpenDirMessageBox(saveFileDialog1.FileName);
            }
        }

        private void SinexViewForm_Load(object sender, EventArgs e)
        {
            this.textBox_Path.Text = Setting.GnsserConfig.SampleSinexFile;
        }

    }
}
