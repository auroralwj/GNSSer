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
using Gnsser.Data.Sinex;

namespace Gnsser.Winform
{
    public partial class MergeSinexForm : Form
    {
        public MergeSinexForm()
        {
            InitializeComponent();
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
                     Path.GetFileNameWithoutExtension(pathA) + "_" + Path.GetFileNameWithoutExtension(pathB) + "_MERGED.SNX");
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
            SinexFile a = SinexReader.ParseText(this.textBox_A.Text);
            SinexFile b = SinexReader.ParseText(this.textBox_B.Text);
            SinexFile c = SinexMerger.Merge(a, b);

            if(checkBox_show.Checked)   this.textBox_C.Text = c.ToString();

            File.WriteAllText(this.textBox_mergePath.Text, c.ToString());

            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(this.textBox_mergePath.Text);
        }


    }
}
