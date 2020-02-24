//2018.10.01, czs , create in hmx, 文件名称提取器


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Gnsser.Winform
{
    /// <summary>
    ///文件名称提取器
    /// </summary>
    public partial class FileNameExtractForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FileNameExtractForm()
        {
            InitializeComponent(); 
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            this.button_ok.Enabled = false;
            int startIndex = this.namedIntControl_startIndex.GetValue();
            int charCount = this.namedIntControlcharCount.GetValue();
            
            var filePathes = this.fileOpenControl1.FilePathes;

            this.progressBarComponent1.InitProcess(filePathes.LongLength);
            List<string> result = new List<string>();
            foreach (var filePath in filePathes)
            {
                var fileName = Path.GetFileName(filePath);
                var name =  Geo.Utils.StringUtil.SubString(fileName, startIndex, charCount);
                if (!result.Contains(name))
                {
                    result.Add(name);
                }
                progressBarComponent1.PerformProcessStep();
            }

            this.richTextBoxControl1.Lines = result.ToArray();

            Geo.Utils.FormUtil.ShowOkMessageBox("提取了 " + result.Count);

            progressBarComponent1.Full();

            this.button_ok.Enabled = true;
        }

        private void MoveFileByKeyForm_Load(object sender, EventArgs e)
        {
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }
    }
}
