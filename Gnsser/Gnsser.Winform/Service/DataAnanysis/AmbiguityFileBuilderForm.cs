//2018.10.16, czs, create in hmx, 参数数值提取
//2018.10.19, czs, edit in hmx， 模糊度增加RMS信息

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geo;
using Geo.IO;
using Geo.Times;


namespace Gnsser.Winform
{
    public partial class AmbiguityFileBuilderForm : Form
    {
        Log log = new Log(typeof(AmbiguityFileBuilderForm));
        public AmbiguityFileBuilderForm()
        {
            InitializeComponent();
            fileOpenControl_EpochParam.Filter = Setting.EpochParamFileFilter;
            fileOpenControl_EpochParamRms.Filter = Setting.EpochParamRmsFileFilter;
        }

        private void button_combine_Click(object sender, EventArgs e)
        {
            var epochParamPath = this.fileOpenControl_EpochParam.FilePath;
            var epochParamRmsPath = this.fileOpenControl_EpochParamRms.FilePath;
            var maxBreak = namedIntControl_allowBreakCount.GetValue();
            if (!File.Exists(epochParamPath))
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("请选择参数文件后再试！");
                return;
            }
            double defaultRms = namedFloatControl_defaultRms.GetValue();
            PeriodParamExtractor periodParamExtractor = new PeriodParamExtractor(epochParamPath, epochParamRmsPath, maxBreak, defaultRms);
            var result = periodParamExtractor. Build().ToTable();

            var outputPath = Path.Combine(Setting.TempDirectory, Path.GetFileNameWithoutExtension(epochParamPath) + Setting.AmbiguityFileExtension);
        
            this.objectTableControlB.DataBind(result);
            ObjectTableWriter.Write(result, outputPath);

            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(Setting.TempDirectory);
        }


        private void AmbiguityCombineerForm_Load(object sender, EventArgs e)
        {
            var extension = "*" + Setting.EpochParamFileExtension;
            var files = Directory.GetFiles(Setting.TempDirectory, extension); 
            if (files.Length > 0)
            {
                this.fileOpenControl_EpochParam.FilePath = files[0];
            }
            extension = "*" + Setting.EpochParamRmsFileExtension;
            files = Directory.GetFiles(Setting.TempDirectory, extension);
            if (files.Length > 0)
            {
                this.fileOpenControl_EpochParamRms.FilePath = files[0];
            }
        }
    }
}
