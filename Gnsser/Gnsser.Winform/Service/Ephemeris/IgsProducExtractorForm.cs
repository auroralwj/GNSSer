//2018.05.12, czs, create in hmx, 本地IGS产品提取

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
    public partial class IgsProducExtractorForm : Form
    {
        Log log = new Log(typeof(IgsProducExtractorForm));
        public IgsProducExtractorForm()
        {
            InitializeComponent();
        }

        private void FilemaintainForm_Load(object sender, EventArgs e)
        {
            fileOpenControl1.FilePathes = Setting.GnsserConfig.IgsProductLocalDirectories.ToArray();
            directorySelectionControl1.Path = Setting.TempDirectory;//.SetValue(".sp3.Z");
            namedStringControl_nameOfIgs.SetValue("igs");

            this.namedStringControl1_extension.SetValue("*.sp3;*.clk_30s;*.clk;*.erp;*.??I");
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            button_run.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
          
        }

        private void Run()
        {
            DateTime start = DateTime.Now;
            var onlyOne = checkBox_onlyOne.Checked;
            var sourceFolder = fileOpenControl1.FilePathes;
            var isParse = this.checkBox_moveOrCopy.Checked;
            var extension = this.namedStringControl1_extension.GetValue();
            var timePerid = timePeriodControl1.TimePeriod;
            var igsName = namedStringControl_nameOfIgs.GetValue();
            var isMoveOrCopy = checkBox_moveOrCopy.Checked;
            var outDir = this.directorySelectionControl1.Path;
            var isOverride = checkBox_override.Checked;

            var extensions = extension.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            //首先获取所有可能的文件，一个文件可能有多个名称，如未解压的Z文件
            Dictionary<string, List<string>> allNames = new Dictionary<string, List<string>>();
            for (var time = timePerid.Start; time <= timePerid.End; time += TimeSpan.FromDays(1))
            {
                foreach (var ext in extensions)
                {
                    var fileNames = new IgsProductNameBuilder(igsName, ext, false).Get(time).FilePathes;
                    allNames[fileNames[0]] = fileNames;
                }
            }

            List<string> failedNames = new List<string>();
            List<string> okPath = new List<string>();
            foreach (var kv in allNames)
            {
                bool isOk = false;
                foreach (var fileName in kv.Value)//便利所有文件名
                {
                    foreach (var folder in sourceFolder)//遍历所有数据源目录
                    {
                        var localPath = Path.Combine(folder, fileName);

                        if (Geo.Utils.FileUtil.IsValid(localPath))
                        {
                            okPath.Add(localPath);
                            isOk = true;
                            if (onlyOne)
                            {
                                break;
                            }
                        }
                    }
                    if (isOk && onlyOne) { break; }
                }
                if (!isOk)
                {
                    failedNames.Add(kv.Key);
                }
            }



            StringBuilder sb = new StringBuilder();
            sb.AppendLine("一共生成 " + allNames.Count + " 个文件名称");
            sb.AppendLine("查找总共到 " + (allNames.Count - failedNames.Count) + " 个文件，包括 " + okPath.Count + " 个压缩或解压的文件");
            sb.AppendLine("其它 " + failedNames.Count + " 个文件未找到：");
            foreach (var item in failedNames)
            {
                sb.Append(item + ", ");
            }
            log.Info(sb.ToString());
            sb.AppendLine();
            log.Info("准备操作这些数据。");

            this.progressBarComponent1.InitProcess(okPath.Count);
            log.Info("  " + okPath.Count + " 个文件！");

            Parallel.ForEach(okPath, new Action<string, ParallelLoopState>((path, state) =>
            {
                this.progressBarComponent1.PerformProcessStep();

                DoOneFile(isMoveOrCopy, outDir, isOverride, path);
            }));

            //    foreach (var path in okPath)
            //{
            //    this.progressBarComponent1.PerformProcessStep();

            //    DoOneFile(isMoveOrCopy, outDir, isOverride, path);
            //}


            var span = DateTime.Now - start;
            var msg = "执行完毕，" + sb.ToString() + ", 耗时 " + span.TotalMinutes + "分钟, 是否打开输出目录？";
            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(outDir, msg);
            this.progressBarComponent1.Full();
        }

        private static void DoOneFile(bool isMoveOrCopy, string outDir, bool isOverride, string path)
        {
            var fileName = Path.GetFileName(path);
            var destPath = Path.Combine(outDir, fileName);
            if (isOverride && Geo.Utils.FileUtil.IsValid(destPath))
            {
                if (isMoveOrCopy)
                {
                    File.Delete(path);
                }
                return;
            }

            if (isMoveOrCopy)
            {
                File.Move(path, destPath);
            }
            else
            {
                File.Copy(path, destPath, true);
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
