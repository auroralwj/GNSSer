//2017.08.09, czs, create in hongqing, GLogger file to O file


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

namespace Gnsser.Winform
{
    /// <summary>
    /// GLogger file to O file
    /// </summary>
    public partial class GLoggerFileConvertForm : Gnsser.Winform.ParalleledFileForm
    {
        public GLoggerFileConvertForm()
        {
            InitializeComponent();
            this.SetExtendTabPageCount(0, 0);
            this.SetEnableMultiSysSelection(false);
            this.SetEnableDetailSettingButton(false);
        }
        AndroidMeasureDecoderOption option { get; set; }

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

        AndroidMeasureDecoder decoder;

        protected override void Run(string inputPath) 
        {
            var reader = new ObjectTableManagerReader(inputPath, Encoding.Default);
            reader.IsIntOrFloatFirst = true;
            reader.Spliters = new string[] { "," };
            reader.HeaderMarkers = new string[] { "#" };
            var tables = reader.Read();//.GetDataTable();  
            var fileName = System.IO.Path.GetFileName(inputPath);

            foreach (var table in tables)
            {
                //var form = new Geo.Winform.DataTableViewForm(table) { Text = table.Name };
                //form.Show();
                var name = table.Name;
                //OpenMidForm(new DataTableViewForm(table) );

                if (String.Equals(name, "Raw", StringComparison.CurrentCultureIgnoreCase))
                {
                    UiToOption();

                    decoder = new AndroidMeasureDecoder(table, option);
                    decoder.ProgressViewer = this.ProgressBar;
                    decoder.RecordProcessed += decoder_RecordProcessed;
                    var opath = Path.Combine( OutputDirectory, Path.GetFileNameWithoutExtension(inputPath) );
                    decoder.Run(opath);
                    //Geo.Utils.FormUtil.ShowOkAndOpenDirectory(OutputDirectory);
                }
            }

            base.Run(inputPath);
        }
        protected   void UiToOption()
        { 
            if (option == null) { option = new AndroidMeasureDecoderOption(); }
            option.IsSkipZeroPhase = checkBox_skipZeroPhase.Checked;
            option.IsSkipZeroPseudorange = checkBox_skipZeroPsuedorange.Checked;
            option.IsFromFirstEpoch = checkBox_fromFirstEpoch.Checked;
            option.IsToCylePhase = checkBox1IsToCylePhase.Checked;
            option.IsAligningPhase = checkBox1IsAligningPhase.Checked;
        }

        protected void OptionToUi(AndroidMeasureDecoderOption option)
        { 
            if (option == null) { option = new AndroidMeasureDecoderOption(); }
            checkBox_skipZeroPhase.Checked = option.IsSkipZeroPhase;
            checkBox_skipZeroPsuedorange.Checked = option.IsSkipZeroPseudorange;
            checkBox_fromFirstEpoch.Checked = option.IsFromFirstEpoch;
            checkBox1IsToCylePhase.Checked = option.IsToCylePhase;
            checkBox1IsAligningPhase.Checked = option.IsAligningPhase;
        }

        void decoder_RecordProcessed(int obj)
        {
            decoder.IsCancel = this.IsCancel;
        }

        private void RinexSparsityForm_Load(object sender, EventArgs e)
        {
            OptionToUi(option);
        }
    }
}