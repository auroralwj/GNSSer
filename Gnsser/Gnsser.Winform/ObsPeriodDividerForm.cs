//2018.11.13, czs, create in hmx, 时段文件区分

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gnsser.Data;
using Geo.Times;
using Geo.IO;
using Gnsser.Data.Rinex;

namespace Gnsser.Winform
{
    public partial class ObsPeriodDividerForm : Form
    {
        public ObsPeriodDividerForm()
        {
            InitializeComponent();
            fileOpenControl1_fiels.Filter = Setting.RinexOFileFilter;
        }
        double periodSpanMinutes => this.namedFloatControl_periodSpanMinutes.GetValue();
        string OutputDirectory => directorySelectionControl1.Path;
        string subDirectory => namedStringControl_subDirectory.GetValue();
        string NetName => namedStringControl_netName.GetValue();
        bool isMoveTo => checkBox_moveto.Checked;
        /// <summary>
        /// 结果
        /// </summary>
        public Dictionary<string, List<string>> Result { get; set; }

        private void button_run_Click(object sender, EventArgs e)
        {
            var fileNames = fileOpenControl1_fiels.FilePathes;

            ObsFilePeriodDivider obsFilePeriodDivider = new ObsFilePeriodDivider(OutputDirectory, periodSpanMinutes, subDirectory, NetName);
            obsFilePeriodDivider.IsMoveTo = isMoveTo;
            var result = obsFilePeriodDivider.Run(fileNames);
            this.Result = result;
            StringBuilder sb = new StringBuilder();
            foreach (var item in result)
            {
                sb.AppendLine();
                sb.AppendLine(item.Key);
                sb.AppendLine(
               Geo.Utils.StringUtil.ToString(item.Value, "\r\n"));
            }

            this.richTextBoxControl_result.Text = sb.ToString();
            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(OutputDirectory);
        }

        private void ObsPeriodDividerForm_Load(object sender, EventArgs e)
        {
           // namedStringControl_subDirectory.SetValue("Raw");
            this.namedStringControl_netName.SetValue("Net");
        }

        private void fileOpenControl1_fiels_FilePathSetted(object sender, EventArgs e)
        {
            if (File.Exists(fileOpenControl1_fiels.FilePath))
            {
                directorySelectionControl1.Path = Path.GetDirectoryName(fileOpenControl1_fiels.FilePath);
            }
        }
    }

}
