//2017.08.30, czs, create in hongqing, 通用平差器

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;
using Geo; 

namespace Geo.Winform
{
    public partial class CommonAdjusterForm : LogListenerForm
    {
        public CommonAdjusterForm()
        {
            InitializeComponent();
            enumRadioControl1AdjustType.Init<AdjustmentType>(true);
            directorySelectionControl1.Path = Setting.TempDirectory;
        }
        CommonFileAdjuster adjuster { get; set; }
        private void button_run_Click(object sender, EventArgs e)
        {
            var filePath = this.fileOpenControl_file.FilePath;
            AdjustmentType adjustType = enumRadioControl1AdjustType.GetCurrent<AdjustmentType>();
            String outDirectory = directorySelectionControl1.Path;
            adjuster = new CommonFileAdjuster(adjustType, outDirectory, progressBarComponent1);

            adjuster.Run(filePath);

            var result = adjuster.ResultTables.First;
            dataGridView_param.DataSource = result.GetDataTable();
            var resultRms = adjuster.ResultTables.Second;
            dataGridView_rms.DataSource = resultRms.GetDataTable();

            adjuster.OutputResult();
            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(outDirectory);
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            if (adjuster != null)
            {
                adjuster.IsCancel = true;
            }

        }
    }

}
