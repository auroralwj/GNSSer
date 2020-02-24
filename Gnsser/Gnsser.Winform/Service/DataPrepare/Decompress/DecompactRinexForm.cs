using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;

using System.Diagnostics;

namespace Gnsser.Winform
{
    /// <summary>
    /// 好像多线程没用起来。
    /// </summary>
    public partial class DecompactRinexForm : Form
    {
        public DecompactRinexForm()
        {
            InitializeComponent();
        }
         
        static int _threadCount = 0;
        Stopwatch stopwatch = new Stopwatch();
        private void button_convert_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
            button_convert.Enabled = false;
            button_cancel.Enabled = false;
        }



        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DecompactRinexForm_Load(object sender, EventArgs e)
        {
            namedStringControl_extension.SetValue("*.**d;*.crx");
            this.directorySelectionControl1.Path = Setting.TempDirectory;
            this.directorySelectionControl2.Path = Setting.TempDirectory;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            stopwatch.Stop();
            stopwatch.Start();
            var extensions = namedStringControl_extension.GetValue();
            var extens = extensions.Split(new char[] { ';', ',' },   StringSplitOptions.RemoveEmptyEntries);
            SearchOption searchOption = SearchOption.TopDirectoryOnly;
            var isSubDirIncluded =  checkBox_subDirIncluded.Checked;
            if (isSubDirIncluded)
            {
                searchOption = SearchOption.AllDirectories;
            }
            string sourseDir = this.directorySelectionControl1.Path;//.Text;
            string destDir = this.directorySelectionControl2.Path;//.Text;

            List<string> files = new List<string>();

            foreach (var item in extens)
            {
                string[] onefiles = Directory.GetFiles(sourseDir, item, searchOption);
                files.AddRange(onefiles);
            }          

            int maxThreadCount = int.Parse(textBox_max_thread_count.Text);
            progressBarComponent1.InitProcess( files.Count );
            
            Geo.Utils.DecompressRinexer de = new Geo.Utils.DecompressRinexer(Setting.PathOfCrx2rnx);
            var delete_sourse = checkBox_delete_sourse.Checked;
            foreach (string file in files)
            {
                de.Decompress(file, destDir, delete_sourse);

                _threadCount++;

               progressBarComponent1.PerformProcessStep();
            }
            string msg = "转换完毕，耗时 " + stopwatch.Elapsed.ToString();

            stopwatch.Reset();

            msg += "\r\n是否打开？";

            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(destDir, msg);

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            button_cancel.Enabled = true;
            button_convert.Enabled = true;
        }
    }
}
