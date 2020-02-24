//2016.08.29, czs, create in hongqing, 文件稀疏
//2016.11.19, czs, edit in hongqing, 格式化转换Rinex观测文件
//2017.04.22.06, czs, create in hongqing，用于转换为表格

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Geo.Times;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo.Coordinates;
using Geo;
using Geo.IO;
using Gnsser.Data;

namespace Gnsser.Winform
{
    /// <summary>
    /// 格式化转换Rinex观测文件用于转换为表格
    /// </summary>
    public partial class TableToOFileForm : Gnsser.Winform.ParalleledFileForm
    {
        public TableToOFileForm()
        {
            InitializeComponent();
            this.SetExtendTabPageCount(0, 0);
            this.SetEnableMultiSysSelection(false);
            this.SetEnableDetailSettingButton(false);

            this.InputFileFilter = "表文件(*" + FileNames.RinexOFileExtension + FileNames.TextExcelFileExtension + ")|*" + FileNames.RinexOFileExtension + FileNames.TextExcelFileExtension + "|所有文件|*.*";
        }

        protected override void Init()
        {
            base.Init();
        }


        protected override void DetailSetting()
        {
        }

        /// <summary>
        /// 解析输入路径
        /// </summary>
        protected override List<string> ParseInputPathes(string[] inputPathes)
        {
            return new List<string>(inputPathes);
        }
        protected override void Run(string inputPath)
        {
            TableObsFileReader reader = new TableObsFileReader(inputPath);

            var ObsFile = new TableObsFileReader(inputPath).Read();
            var outPath = Path.Combine(this.OutputDirectory, Path.GetFileName(inputPath).Replace(FileNames.TextExcelFileExtension, ""));

            var Writer = new RinexObsFileWriter(outPath, namedFloatControl1Vertion.Value);


            Writer.WriteHeader(ObsFile.Header);
            foreach (var item in ObsFile)
            {
                Writer.WriteEpochObservation(item);
            }
            Writer.Dispose();

            base.Run(inputPath);
        }

        private void TableToOFileForm_Load(object sender, EventArgs e)
        {
            this.fileOpenControl_inputPathes.FilePath = Setting.GnsserConfig.SampleOTableFile;
        }

    }
}