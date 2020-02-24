//2016.08.29, czs, create in hongqing, 文件稀疏
//2016.11.19, czs, edit in hongqing, 格式化转换Rinex观测文件

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
    /// 格式化转换Rinex观测文件
    /// </summary>
    public partial class OFileFormaterForm : Gnsser.Winform.ParalleledFileForm
    {
        public OFileFormaterForm()
        {
            InitializeComponent();
            this.SetExtendTabPageCount(0, 0);
            this.SetEnableMultiSysSelection(false);
        }

        protected override void Init()
        {
            base.Init();
        }

        ObsFileConvertOption ObsFileConvertOption { get; set; }

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


        protected override void Run(string inputPath)
        {
            ObsFileConvertOption opt = GetOrInitObsFileFormatOption();
            //opt.OutputDirectory = this.OutputDirectory;
            string subDir = Geo.Utils.PathUtil.GetSubDirectory(InputRawPathes, inputPath);

            ObsFileFormater ObsFileFormater = new ObsFileFormater(opt, inputPath);
            ObsFileFormater.SubDirectory = subDir;
            ObsFileFormater.Init();
            ObsFileFormater.Run();

            base.Run(inputPath);
        }

    }
}