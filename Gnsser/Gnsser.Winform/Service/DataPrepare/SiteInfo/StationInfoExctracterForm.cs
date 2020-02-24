//2016.12.28, czs, create in hongqing, 测站信息提取器

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
    ///测站信息提取器
    /// </summary>
    public partial class StationInfoExctracterForm : Gnsser.Winform.ParalleledFileForm
    {
        public StationInfoExctracterForm()
        {
            InitializeComponent();
            this.SetExtendTabPageCount(0, 0);
            this.SetEnableMultiSysSelection(false);

        }

        protected override void Init()
        {
            base.Init();
            if (StationInfoWriter == null)
            {
                StationInfoWriter = new StationInfoWriter(this.OutputDirectory + "\\StationInfo.StaInfo");
                StationInfoWriter.WriteHeaderLine();
            }
        }

        ObsFileConvertOption ObsFileConvertOption { get; set; }
        StationInfoWriter StationInfoWriter { get; set; }
        protected override void DetailSetting()
        {
            this.ObsFileConvertOption = GetOrInitObsFileFormatOption();
            var form = new ObsFileConvertOptionForm(ObsFileConvertOption);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.ObsFileConvertOption = form.Option;
            }
        }
        public ObsFileConvertOption GetOrInitObsFileFormatOption()
        {
            if (ObsFileConvertOption == null) { ObsFileConvertOption = new ObsFileConvertOption(); }
            ObsFileConvertOption.OutputDirectory = this.OutputDirectory;
            return ObsFileConvertOption;
        }

        static Object locker = new object();
        protected override void Run(string inputPath)
        {

            RinexObsFileReader reader = new RinexObsFileReader(inputPath, false);
            var header = reader.GetHeader();

            StationInfo info = new StationInfo(header.SiteInfo, header.ObsInfo);
            lock (locker)
            { 
                StationInfoWriter.Write(info);

            } 

            base.Run(inputPath);
        }

        protected override void Run(string[] inputPathes)
        {
            base.Run(inputPathes);
            StationInfoWriter.Flush();
        }

        private void RinexSparsityForm_Load(object sender, EventArgs e)
        {
        }

        protected override void FormWillClosing(object sender, FormClosingEventArgs e)
        {
            if (StationInfoWriter != null)
            {
                StationInfoWriter.Close();
                StationInfoWriter.Dispose();
            }
            base.FormWillClosing(sender, e);
        }
    }
}