//2018.02.12, czs, create in hmx, 选择文件,并分段页面控件

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms; 
using Geo.Winform.Wizards;

namespace Gnsser.Winform
{
    public partial class GroupRinexFileWizardPage : UserControl, IWizardPage
    {
        public GroupRinexFileWizardPage()
        {
            InitializeComponent();

            this.Name = "建立同步观测网";
            fileOpenControl1_fiels.Filter = Setting.RinexOFileFilter;
            directorySelectionControl1.Path = Setting.TempDirectory;
            this.namedStringControl_subDirectory.SetValue("Raw");
            this.namedStringControl_netName.SetValue("Net");
            this.fileOpenControl1_fiels.FilePathes = new string[] { Setting.GnsserConfig.SampleOFileA, Setting.GnsserConfig.SampleOFileB };
        }
        double periodSpanMinutes => this.namedFloatControl_periodSpanMinutes.GetValue();
        string OutputDirectory => directorySelectionControl1.Path;
        string subDirectory => namedStringControl_subDirectory.GetValue();
        string NetName => namedStringControl_netName.GetValue();
        /// <summary>
        /// 结果
        /// </summary>
        public Dictionary<string, List<string>> Result { get; set; }

        public UserControl Content
        {
            get { return this; }
        }

        public new void LoadPage()
        {
        }


        public void Save()
        {
            var fileNames = fileOpenControl1_fiels.FilePathes;

            ObsFilePeriodDivider obsFilePeriodDivider = new ObsFilePeriodDivider(OutputDirectory, periodSpanMinutes, subDirectory, NetName);

            var result = obsFilePeriodDivider.Run(fileNames);
            this.Result = result;

            StringBuilder sb = new StringBuilder();
            foreach (var item in result)
            {
                sb.AppendLine();
                sb.AppendLine(item.Key);
                sb.AppendLine(
               Geo.Utils.StringUtil.ToString(item.Value, "\r\n"));
            };

            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(OutputDirectory);
        }

        public void Cancel()
        {
            //throw new NotImplementedException();
        }

        public bool IsBusy
        {
            get { return false; }
        }

        public bool PageValid
        {
            get { return true; }
        }

        public string ValidationMessage
        {
            get { throw new NotImplementedException(); }
        }

            
        private void SelectFilePageControl_Load(object sender, EventArgs e)
        {
    
        }


    }
}
