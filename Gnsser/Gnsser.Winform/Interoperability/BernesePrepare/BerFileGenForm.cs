using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using  Gnsser.Interoperation.Bernese;

namespace Gnsser.Winform
{
    public partial class BerFileGenForm : Form
    {
        public BerFileGenForm(BerFileType berFileType)
        {
            InitializeComponent();
            this.BerFileType = berFileType;
            this.Text = berFileType + "文件生成";
            this.saveFileDialog1.Filter = String.Format("ABB文件(*.{0})|*.{0}|所有文件|*.*", berFileType);
            this.textBox_outPath.Text = @"C:\EXAMPLE." + berFileType;
        }

        BerFileType berFileType;

        public BerFileType BerFileType
        {
            get { return berFileType; }
            set { berFileType = value; }
        }

        private void button_setODirPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_dir.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button_setStaPath_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_outPath.Text = saveFileDialog1.FileName;
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            string dirPath = this.textBox_dir.Text;
            string staPath = this.textBox_outPath.Text;

            IBerFile berFile = BerFileFactory.Create(dirPath, berFileType);
            berFile.Save(staPath);

            this.textBox_gen.Text = berFile.ToString();

            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(Path.GetDirectoryName(staPath));
        }

    }
}
