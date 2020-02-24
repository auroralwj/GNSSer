//2018.06.05, czs, create in HMX, 合并O文件
//2018.07.03, czs, edit in HMX, 增加 RINEX 3 文件名称输出支持


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gnsser.Data.Rinex;
using System.IO;
using System.Threading.Tasks;
using Geo;

namespace Gnsser.Winform
{
    public partial class MultiCombineOFileForm : Form
    {
        public MultiCombineOFileForm()
        {
            InitializeComponent();
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            button_run.Enabled = false;
            backgroundWorker1.RunWorkerAsync(); 
        }

        private void CombineFilesInOneSite(int keyCount, string outDir, double outVersion,  List<string> oneSiteFiles)
        {
            RinexObsFile currentFile = null;
            string readiedFilePath = null;
            string currentSiteName = null;
            oneSiteFiles.Sort();
            foreach (var path in oneSiteFiles)
            {
                var siteName = Path.GetFileName(path).Substring(0, keyCount);
                if (currentSiteName == null) { currentSiteName = siteName; }

                if (!String.Equals(currentSiteName, siteName, StringComparison.CurrentCultureIgnoreCase))
                {
                    OutputFile(outDir, outVersion, currentSiteName, currentFile, readiedFilePath);

                    currentSiteName = siteName;
                    currentFile = null;
                }

                RinexObsFile file = new RinexObsFileReader(path, true).ReadObsFile();

                readiedFilePath = path;//存储刚刚读取的路径，用户获取后缀名
                if (currentFile == null) { currentFile = file; }
                else//拼接
                {
                    currentFile.Add(file);
                }
            }
            OutputFile(outDir, outVersion, currentSiteName, currentFile, readiedFilePath);
            currentFile = null;
        }

        private static void OutputFile(string outDir, double outVersion, string currentSiteName, RinexObsFile currentFile, string readiedFilePath)
        {
            var extension = Path.GetExtension(readiedFilePath); 
            RinexFileNameBuilder builder = new RinexFileNameBuilder(outVersion);
            var fileName = builder.Build(currentFile);

            string outputPath = Path.Combine(outDir, fileName);
            using (RinexObsFileWriter writer = new RinexObsFileWriter(outputPath, outVersion))
            {
                writer.Write(currentFile);
            }
        }

        private void CombineOFileForm_Load(object sender, EventArgs e)
        {
            fileOpenControl_inputs.Filter = Setting.RinexOFileFilter;
            directorySelectionControl_outputDir.Path = Setting.TempDirectory;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int keyCount = namedIntControl_keyCount.Value;
            string outDir = directorySelectionControl_outputDir.Path;
            double outVersion = namedFloatControl_outVersion.GetValue();
            List<string> inputFiles = new List<string>(fileOpenControl_inputs.FilePathes);
            if (inputFiles.Count == 0)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("巧妇难为无米之炊！");
                return;
            }


            MultiPathDistinguisher multiPathDistinguisher = new MultiPathDistinguisher(inputFiles, keyCount);
            var fileDics = multiPathDistinguisher.GetDistinguishedPathes();

            progressBarComponent1.InitProcess(fileDics.Count);

            Parallel.ForEach(fileDics.Data, (files, state) =>
            {
                if (this.backgroundWorker1.CancellationPending) { state.Stop(); }


                CombineFilesInOneSite(keyCount, outDir, outVersion, files.Value);

                progressBarComponent1.PerformProcessStep();
            });


            progressBarComponent1.Full();
            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(outDir);

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        { 
            button_run.Enabled = true;
        }
    }


    /// <summary>
    /// 批量文件区分器
    /// </summary>
    public class MultiPathDistinguisher
    {

        public MultiPathDistinguisher(List<String> pathes, int keyCount = 4)
        {
            this.Pathes = pathes;
            this.KeyCount = keyCount;
        }

        public List<string> Pathes { get; set; }

        public int KeyCount { get; set; }

        public BaseDictionary<string, List<string>> GetDistinguishedPathes()
        {
            this.Pathes.Sort();
            BaseDictionary<String, List<string>> data = new BaseDictionary<string, List<string>>("test", m=> new  List<string>());  
            string currentKey = "";
            foreach (var path in this.Pathes)
            {
                currentKey = Path.GetFileName( path).Substring(0, KeyCount);
                data.GetOrCreate(currentKey).Add(path);
            }

            return data;
        }

    }









}
