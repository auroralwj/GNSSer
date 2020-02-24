using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
 using Gnsser.Interoperation.Bernese;

namespace Gnsser.Winform
{
    public partial class BerFileMergeForm : Form
    {
        public BerFileMergeForm()
        {
            InitializeComponent();
        } 
        
        public BerFileMergeForm(BerFileType berFileType)
        {
            InitializeComponent();

            this.BerFileType = berFileType;
            this.Text = berFileType + "文件合成";
            this.saveFileDialog1.Filter = String.Format("ABB文件(*.{0})|*.{0}|所有文件|*.*", berFileType);
            textBox_fileA.Text = "C:\\EXAMPLE." + berFileType;
            textBox_fileB.Text = @"C:\GPSDATA\EXAMPLE\STA\EXAMPLE." + berFileType;

            this.textBox_mergePath.Text = "C:\\MERGE." + berFileType;
            this.openFileDialog1.Filter = String.Format("ABB文件(*.{0})|*.{0}|所有文件|*.*", berFileType);
            this.saveFileDialog1.Filter = String.Format("ABB文件(*.{0})|*.{0}|所有文件|*.*", berFileType);
        }

        BerFileType berFileType;

        public BerFileType BerFileType
        {
            get { return berFileType; }
            set { berFileType = value; }
        }

        private void button_setStaPath_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_fileA.Text = openFileDialog1.FileName;
                SetOutPath();
            }
        }
        private void button_setfileBPath_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_fileB.Text = openFileDialog1.FileName;
                SetOutPath();
            }
        }

        private void SetOutPath()
        {
            string pathA = this.textBox_fileA.Text.Trim();
            string pathB = this.textBox_fileB.Text.Trim();
            if (pathA != "" && pathB != "")
                this.textBox_mergePath.Text = Path.Combine(Path.GetDirectoryName(pathA),
                     Path.GetFileNameWithoutExtension(pathA) + "_" + Path.GetFileNameWithoutExtension(pathB) + "_MERGED." + berFileType);
        }
        private void button_readA_Click(object sender, EventArgs e)
        {
            string path = this.textBox_fileA.Text;
            this.textBox_A.Lines = File.ReadAllLines(path);
        }
        private void button_readB_Click(object sender, EventArgs e)
        {
            string path = this.textBox_fileB.Text;
            this.textBox_B.Lines = File.ReadAllLines(path);
        }

        private void button_setMerPath_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_mergePath.Text = saveFileDialog1.FileName;
            }
        }

        private void button_merge_Click(object sender, EventArgs e)
        {
            Gnsser.Interoperation.Bernese.IBerFile fileC = BerFileFactory.Merge(this.textBox_A.Text, this.textBox_B.Text, berFileType);
           
            this.textBox_C.Text = fileC.ToString();
            //Save to File.
            string staPath = this.textBox_mergePath.Text;
            File.WriteAllText(staPath, fileC.ToString());
            string msg = String.Format("合并完成，是否打开 {0} ?", 
                Path.GetDirectoryName(staPath));
            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(Path.GetDirectoryName(staPath), msg);
        }


    }
}
