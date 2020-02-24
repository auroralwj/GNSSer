//2018.10.01, czs , create in hmx, 选择共同文件


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
    /// 选择共同文件
    /// </summary>
    public partial class SelectCommonFilesForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SelectCommonFilesForm()
        {
            InitializeComponent();
            this.directorySelectionControl1.IsAddOrReplase = true;
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            this.button_ok.Enabled = false;
            var isLoop = this.checkBox_loopSub.Checked;
            var toDir = directorySelectionControl2.Path;
            var isCopy = checkBox_copyOrMove.Checked;
            var dirs = directorySelectionControl1.Pathes;

            List<string> firstFiles = new List<string>();
            string firstTopDir = null;
            Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();
            foreach (var dir in dirs)
            {
                var files = Directory.GetFiles(dir, "*.*", isLoop ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                if (firstFiles.Count == 0)
                {
                    firstFiles.AddRange(files);
                    firstTopDir = dir;
                }
                List<string> list = new List<string>();
                foreach (var file in files)
                {
                    var name = Path.GetFileName(file).ToUpper();
                    list.Add(name);
                }
                data[dir] = list;
            }

            List<string> common = new List<string>();
            foreach (var item in data)
            {
                if (common.Count == 0) { common.AddRange(item.Value); continue; }

                common = Geo.Utils.ListUtil.GetCommons(common, item.Value);
            }

            this.progressBarComponent1.InitProcess(common.Count);
            int counter = 0;
             //按照此架构保存文件
            foreach (var file in firstFiles)
            { 
                var name = Path.GetFileName(file).ToUpper();

                if (common.Contains(name))
                {
                    var toPath = file.Replace(firstTopDir, toDir);
                    Geo.Utils.FileUtil.CopyOrMoveFile(file, toPath, isCopy);

                    counter++;
                    this.progressBarComponent1.PerformProcessStep();
                }
            }


            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(toDir, "OK " + counter + " done! Open or not");
            this.button_ok.Enabled = true;
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
