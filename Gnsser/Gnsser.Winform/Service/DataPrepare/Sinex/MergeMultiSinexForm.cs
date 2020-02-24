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
    public partial class MergeMultiSinexForm : Form
    {
        public MergeMultiSinexForm()
        {
            InitializeComponent();
        }

        private void button_setFilePath_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_pathes.Lines = openFileDialog1.FileNames;
        }

        private void button_merge_Click(object sender, EventArgs e)
        {
            SinexFile fileMerged = null;
            foreach (var item in this.textBox_pathes.Lines)
            {
                SinexFile file = SinexReader.Read(item);
                if (fileMerged == null) fileMerged = file;
                else fileMerged = SinexMerger.Merge(fileMerged, file, this.checkBox_eraseNonCoord.Checked);
            }
            if(checkBox_showresult.Checked)
            this.textBox_result.Text = fileMerged.ToString();

            File.WriteAllText(this.textBox_savepath.Text, fileMerged.ToString());

            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(this.textBox_savepath.Text);

        }

        private void button_setSavePath_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_savepath.Text = this.saveFileDialog1.FileName;
        }

        private void MergeMultiSinexForm_Load(object sender, EventArgs e)
        {
            textBox_savepath.Text = Path.Combine(Application.StartupPath, "Temp/Merged.Snx");
        }
    }
}
