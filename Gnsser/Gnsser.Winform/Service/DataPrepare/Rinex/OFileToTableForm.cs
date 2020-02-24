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
    public partial class OFileToTableForm : Gnsser.Winform.ParalleledFileForm
    {
        public OFileToTableForm()
        {
            InitializeComponent();
            this.SetExtendTabPageCount(0, 0);
            this.SetEnableMultiSysSelection(false);
        }

        protected override void Init()
        {
            base.Init();
        }


        protected override void DetailSetting()
        {
        }


        protected override void Run(string inputPath)
        {
            base.Run(inputPath);

            var obsFileReader = new RinexObsFileReader(inputPath);
            var ObsFile = obsFileReader.ReadObsFile();
            var table = new ObsFileToTableBuilder().Build(ObsFile);

            ObjectTableManager mgr = new ObjectTableManager(this.OutputDirectory);
            mgr.Add(table);
            mgr.WriteAllToFileAndClearBuffer();
           // Geo.Utils.FormUtil.ShowOkAndOpenDirectory(mgr.OutputDirectory);
        }
    }
}