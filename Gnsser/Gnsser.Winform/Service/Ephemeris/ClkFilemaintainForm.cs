//2018.05.09, czs, create in hmx, 维护星历

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using Geo.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gnsser.Data;

namespace Gnsser.Winform
{
    public partial class ClkFilemaintainForm : Form
    {
        Log log = new Log(typeof(ClkFilemaintainForm));
        public ClkFilemaintainForm()
        {
            InitializeComponent();
        }

        private void FilemaintainForm_Load(object sender, EventArgs e)
        {
            fileOpenControl1.FilePathes = Setting.GnsserConfig.IgsProductLocalDirectories.ToArray();
            directorySelectionControl1.Path = Setting.TempDirectory;//.SetValue(".sp3.Z");

            this.namedStringControl1_extension.SetValue("*.clk_30s;*.clk_05s;*.clk");
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            button_run.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
          
        }

        private void Run()
        {
            DateTime start = DateTime.Now;
            var moveNotIgs = checkBox_moveNotIgs.Checked;
            var isParse = this.checkBox_parse.Checked;
            var isMoveZ = this.checkBox_moveZ.Checked;
            var extension =   this.namedStringControl1_extension.GetValue();

            var extensions = extension.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            var inputPathes = fileOpenControl1.FilePathes;
            List<string> filePathes = new List<string>();
            foreach (var path in inputPathes)
            {

                if (Geo.Utils.FileUtil.IsDirectory(path) && Directory.Exists(path))
                {
                    foreach (var ext in extensions)
                    { 
                        var subFiles = Directory.GetFiles(path, ext, SearchOption.AllDirectories);
                        filePathes.AddRange(subFiles);
                    }

                }
                else if (File.Exists(path))
                {
                    filePathes.Add(path);
                }
            }

            if (filePathes.Count == 0)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("没有文件！");
                return;
            }

            var outDir = this.directorySelectionControl1.Path;

            this.progressBarComponent1.InitProcess(filePathes.Count);
            log.Info("载入 " + filePathes.Count + " 个文件！");
            List<string> failed = new List<string>();
            List<string> movedZFile = new List<string>();
            Parallel.ForEach(filePathes, new Action<string, ParallelLoopState>((path , state) =>
            {
                this.progressBarComponent1.PerformProcessStep();

                try
                {
                    var fileName = Path.GetFileName(path);
                    if (moveNotIgs && !String.Equals("ig", fileName.Substring(0, 2), StringComparison.CurrentCultureIgnoreCase))
                    {
                        log.Error("非IGS，移走 ： " + path);
                        Move(path, outDir, failed, movedZFile);
                        return;
                    }
                    FileInfo info = new FileInfo(path);
                    if(info.Length == 0)
                    {
                        MoveZFile(path, outDir, movedZFile);
                        return;
                    }
                     

                    if (isParse)
                        using (Data.Rinex.ClockFileReader reader = new Data.Rinex.ClockFileReader(path, true))
                        {
                             reader.ReadAll();

                            MoveZFile(path, outDir, movedZFile); 
                        }
                }
                catch (Exception ex)
                {
                    log.Error("出错 ： " + ex.Message + ", " + path);
                    Move(path, outDir, failed, movedZFile);
                }

            }));
            var span = DateTime.Now - start;
            var msg = "执行完毕，移动文件 " + failed.Count + "个, Z 压缩文件 "+ movedZFile .Count+ " 个, 耗时 " + span.TotalMinutes + "分钟, 是否打开输出目录？";
            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(outDir, msg);
            this.progressBarComponent1.Full();
        }

        private static void Move(string path, string outDir, List<string> failed, List<string> movedZFile)
        {
            var fileName = Path.GetFileName(path);
            var dest = Path.Combine(outDir, fileName);
            Geo.Utils.FileUtil.MoveFile(path, dest, true);
            failed.Add(path);

            MoveZFile(path, outDir, movedZFile);
        }

        private static void MoveZFile(string path, string outDir, List<string> movedZFile)
        {
            var zfile = path + ".Z";
            if (File.Exists(zfile))
            {
                var destZ = Path.Combine(outDir, Path.GetFileName(zfile));
                Geo.Utils.FileUtil.MoveFile(zfile + "", destZ, true);
                movedZFile.Add(zfile);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Run();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button_run.Enabled = true;
        }
    }
}
