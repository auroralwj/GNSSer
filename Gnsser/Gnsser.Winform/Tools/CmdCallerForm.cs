//2016.02.21, czs, create in hognqing, CMD 执行器

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Utils;
using System.IO;

namespace Gnsser.Winform 
{
    public partial class CmdCallerForm : Form
    {
        public CmdCallerForm()
        {
            InitializeComponent();
        }

        private void button_run_Click(object sender, EventArgs e)
        {

            string exeFilePath = this.fileOpenControl1.FilePath;
            string[] argsPathes = this.directorySelectionControl1.Pathes;

            foreach (var path in argsPathes)
            {
              
                if (File.Exists(path))
                {
                    ShowInfo("处理：" + path);
                    Geo.Common.ProcessRunner cmd = new Geo.Common.ProcessRunner(exeFilePath);
                    cmd.ExitedOrDisposed += cmd_ExitedOrDisposed;
                    cmd.Run(path);
                }
                if (Directory.Exists(path))
                {
                    var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        if (File.Exists(file))
                        {
                            ShowInfo("处理：" + file);
                            Geo.Common.ProcessRunner cmd = new Geo.Common.ProcessRunner(exeFilePath);
                            cmd.ExitedOrDisposed += cmd_ExitedOrDisposed;
                            cmd.Run(path);
                        }
                    }
                }
            }

            MessageBox.Show("执行完毕！");

        }

        void cmd_ExitedOrDisposed(object sender, EventArgs e)
        {
            ShowInfo("线程退出");
        }
        public void ShowInfo(string msg)
        {
            try
            {
                this.Invoke(new Action(delegate()
                {
                    var info = DateTimeUtil.GetFormatedTimeNow(true) + ":\t" + msg;
                    FormUtil.InsertLineToTextBox(this.richTextBoxControl1, info);
                }));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
               // log.Error(ex.Message);
            }
        }
    }
}
