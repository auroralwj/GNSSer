//2018.12.10, czs, create in hmx, md5 文件校核

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geo.Utils;

namespace Gnsser.Winform
{
    public partial class Md5CheckerForm : Form
    {
        public Md5CheckerForm()
        {
            InitializeComponent();
        }

        private void button_check_Click(object sender, EventArgs e)
        {
            var md5FilePath = this.fileOpenControl1_md5File.FilePath;
            var md5text = richTextBoxControl_inputText.Text;
            var filePath = this.fileOpenControl1_filePath.FilePath;

            var md5Result = FileUtil.GetMD5(filePath);

            if(this.tabPage_inputMd5Text == this.tabControl_input.SelectedTab)
            {
            }
            else
            {
                md5text = System.IO.File.ReadAllText(md5FilePath);
            }

            if (md5Result == md5text)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("校核通过！");
            }
            else
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("校核不通过！");
            }

            this.richTextBoxControl_md5Result.Text = md5Result;
        }
    }
}
