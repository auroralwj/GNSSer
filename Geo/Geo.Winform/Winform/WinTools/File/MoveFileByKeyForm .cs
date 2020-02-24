//2018.10.01, czs , create in hmx, 文件移动工具


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
    /// 文件移动
    /// </summary>
    public partial class MoveFileByKeyForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MoveFileByKeyForm()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            this.button_ok.Enabled = false;
            var dir = directorySelectionControl1.Path;
            var isLoop = this.checkBox_loopSub.Checked;
            var toDir = directorySelectionControl2.Path;
            var keyStr = this.richTextBoxControlKeys.Text;
            var keys = keyStr.Split(new char[] { '\t', ';', '；', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var isCopy = checkBox_copyOrMove.Checked;

            var files = Directory.GetFiles(dir, "*.*", isLoop ? SearchOption.AllDirectories: SearchOption.TopDirectoryOnly);

            this.progressBarComponent1.InitProcess(files.Length);
            int counter = 0;
            foreach (var fromPath in files)
            {
                counter = MatchAndSelect(dir, toDir, keys, isCopy, counter, fromPath);
                this.progressBarComponent1.PerformProcessStep();
            }
            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(toDir, "OK " + counter + " done! Open or not");
            this.button_ok.Enabled = true;
        }

        private static int MatchAndSelect(string dir, string toDir, string[] keys, bool isCopy, int counter, string fromPath)
        {
            foreach (var key in keys)
            {
                if (fromPath.ToUpper().Contains(key.ToUpper()))
                {
                    var toPath = fromPath.Replace(dir, toDir);
                    Geo.Utils.FileUtil.CopyOrMoveFile(fromPath, toPath, isCopy);

                    counter++;
                    break;
                }
            }

            return counter;
        }

        private void MoveFileByKeyForm_Load(object sender, EventArgs e)
        {
            this.directorySelectionControl2.Path = Geo.Setting.TempDirectory;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }
    }
}
