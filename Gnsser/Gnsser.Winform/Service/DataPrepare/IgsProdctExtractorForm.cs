//2018.10.12, czs, create in hmx, IGS产品提取器 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 
using Geo;
using Geo.Common;
using Gnsser.Service;
using Gnsser.Times;
using Geo.IO;
using System.IO;
using Gnsser.Data;
using Geo.Times;

namespace Gnsser.Winform
{
    public partial class IgsProdctExtractorForm : Form
    {
        public IgsProdctExtractorForm()
        {
            InitializeComponent();

            enumCheckBoxControl1.Init<IgsProductType>(); 
        }

       
        private void button_go_Click(object sender, EventArgs e)
        {
            button_go.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
        }

        private void IgsProdctExtractorForm_Load(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var outDir = this.directorySelectionControl1.Path;
            var timePeriod = this.timePeriodControl1.TimePeriod;
            var selects = enumCheckBoxControl1.GetSelected<IgsProductType>();
            var satTypes = multiGnssSystemSelectControl1.SatelliteTypes;

            var opt = Setting.GnsserConfig.GetIgsProductSourceOption(new BufferedTimePeriod(timePeriod), satTypes);
            opt.IsUniqueSource = false;
            //var opt = new IgsProductSourceOption(new BufferedTimePeriod(timePeriod), satTypes);

            var totalFiles = new List<string>();
            foreach (var selectType in selects)
            {
                IgsProductFileExtractor fileExtractor = new IgsProductFileExtractor(opt, selectType);
                var localFiles = fileExtractor.GetLocalFilePathes();
                totalFiles.AddRange(localFiles);
            }
            totalFiles = totalFiles.Distinct().ToList();


            Geo.Utils.FormUtil.AppendLineToTextBox(richTextBoxControl1, "恭喜！找到 " + totalFiles.Count + " 个文件\r\n" + Geo.Utils.StringUtil.ToLineString(totalFiles));

            this.progressBarComponent1.InitProcess(totalFiles.Count);
            int count = 0;
            foreach (var item in totalFiles)
            {
                var file = Path.GetFileName(item);
                var dest = Path.Combine(outDir, file);

                if (Geo.Utils.FileUtil.CopyFile(item, dest, true))
                {
                    Geo.Utils.FormUtil.AppendLineToTextBox(richTextBoxControl1, "From\t" + file + "\tTo\t" + item);
                    count++;
                    progressBarComponent1.PerformProcessStep();
                }
            }
            Geo.Utils.FormUtil.AppendLineToTextBox(richTextBoxControl1, "复制了 " + count + " 个");
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button_go.Enabled = true;
            var outDir = this.directorySelectionControl1.Path;
            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(outDir);
        }

        private void button_extractFromFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = Setting.RinexOFileFilter;
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                var path = dlg.FileName;

                var header = Gnsser.Data.Rinex.RinexObsFileReader.ReadHeader(path);
                this.timePeriodControl1.SetTimePerid(header.TimePeriod);
            }
        }
    }
}
