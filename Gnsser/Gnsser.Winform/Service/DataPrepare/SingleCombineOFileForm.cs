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

namespace Gnsser.Winform
{
    public partial class SingleCombineOFileForm : Form
    {
        public SingleCombineOFileForm()
        {
            InitializeComponent();
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            var outDir = directorySelectionControl_outputDir.Path;
            var outVersion = namedFloatControl_outVersion.GetValue();
            List<string> files = new List<string>(fileOpenControl_inputs.FilePathes);
            if(files.Count == 0)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("巧妇难为无米之炊！");
                return;
            }
            progressBarComponent1.InitProcess(files.Count);
            string currentSiteName = null;

            RinexObsFile currentFile = null;
            string readiedFilePath = null;

            foreach (var path in files)
            {
                progressBarComponent1.PerformProcessStep();
                var siteName =Path.GetFileName( path).Substring(0, 4);
                if(currentSiteName == null) { currentSiteName = siteName; }
               
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

            progressBarComponent1.Full();

            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(outDir);
                    
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

        private void button_sort_Click(object sender, EventArgs e)
        {
            List<string> files = new List<string>(fileOpenControl_inputs.FilePathes);
            files.Sort();
            fileOpenControl_inputs.FilePathes = files.ToArray();
        }

        private void CombineOFileForm_Load(object sender, EventArgs e)
        {
            fileOpenControl_inputs.Filter = Setting.RinexOFileFilter;
            directorySelectionControl_outputDir.Path = Setting.TempDirectory;
        }
    }
}
