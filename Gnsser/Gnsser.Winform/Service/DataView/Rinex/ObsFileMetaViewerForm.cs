using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.IO;

using System.Text;
using System.Windows.Forms;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo.Coordinates;

namespace Gnsser.Winform
{

    public partial class ObsFileMetaViewerForm : Form
    {
        Data.Rinex.RinexObsFile obsFile;

        public ObsFileMetaViewerForm()
        {
            InitializeComponent();
        }

        private void button_getObsPath_Click(object sender, EventArgs e)
        {
            if (openFileDialog_obs.ShowDialog() == DialogResult.OK) textBox_obsPath.Lines = openFileDialog_obs.FileNames;
        }



        private void button_read_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime from = DateTime.Now;

                string[] filePathes = this.textBox_obsPath.Lines;
                if (this.textBox_obsPath.Lines.Length == 0) throw new ArgumentNullException("请输入文件！");

                StringBuilder sb = new StringBuilder();
                if (this.checkBox_Gnsser.Checked)
                {
                    sb.AppendLine("-------- Gnsser -------------");
                    foreach (var item in filePathes)
                    {
                        sb.AppendLine(new Data.Rinex.RinexObsFileReader(item).GetHeader().ToString());
                    }
                }
                if (this.checkBox_teqc.Checked)
                {
                    string teqcPath = Path.Combine(Setting.ExeFolder, "teqc.exe");
                    Gnsser.Interoperation.Teqc.TeqcFunctionCaller caller = new Gnsser.Interoperation.Teqc.TeqcFunctionCaller(teqcPath);
                    sb.AppendLine("-------- teqc -------------");
                    foreach (var item in filePathes)
                    {
                        sb.AppendLine(caller.Run(Gnsser.Interoperation.Teqc.TeqcFunction.ViewMetadata, item)[0]);
                    }
                }
                if (this.checkBox_source.Checked)
                {
                    sb.AppendLine("-------- SOURCE -------------");
                    foreach (var item in filePathes)
                    {
                        sb.AppendLine(Gnsser.Data.Rinex.RinexObsFileHeader.ReadText(item).ToString());
                    }
                }

                TimeSpan span = DateTime.Now - from;
                string str = "耗时：" + span.ToString() + "\r\n";
                sb.Insert(0, str);
                this.textBox_out.Text = sb.ToString();
                Geo.Utils.FormUtil.ShowOkAndOpenDirectory(Path.GetDirectoryName(filePathes[0]));
            }
            catch (Exception ex) { MessageBox.Show("出错了！ " + ex.Message); }
        }

        private void ObsFileMetaViewerForm_Load(object sender, EventArgs e)
        {
            this.textBox_obsPath.Text = Setting.GnsserConfig.SampleOFile;
        }

        private void button_saveAs_Click(object sender, EventArgs e)
        {
            Geo.Utils.FormUtil.ShowFormSaveTextFileAndIfOpenFolder(this.textBox_out.Text);
        }
    }

}
