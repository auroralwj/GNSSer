//2017.10.21, czs, create in hongqing, 探测修复Rinex观测文件

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
    /// 探测修复Rinex观测文件
    /// </summary>
    public partial class OFileFixingForm : Gnsser.Winform.ParalleledFileForm
    {
        public OFileFixingForm()
        {
            InitializeComponent();
            this.SetExtendTabPageCount(0, 0);
            this.SetEnableMultiSysSelection(false);
        }

        protected override void Init()
        {
            base.Init();
            ObsFileFixers = new List<ObsFileFixer>();
        }

        ObsFileFixOption ObsFileConvertOption { get; set; }

        protected override void DetailSetting()
        {
            this.ObsFileConvertOption = GetOrInitObsFileFormatOption();
            var form = new ObsFileFixOptionForm(ObsFileConvertOption);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.ObsFileConvertOption = form.Option;
            }
        }
        public ObsFileFixOption GetOrInitObsFileFormatOption()
        {
            if (ObsFileConvertOption == null) { ObsFileConvertOption = new ObsFileFixOption(); }
            ObsFileConvertOption.OutputDirectory = this.OutputDirectory;
            return ObsFileConvertOption; 
        }
        List< ObsFileFixer> ObsFileFixers{get;set;}
              
      protected   override  void  OnProcessCommandChanged(ProcessCommandType type){
          base.OnProcessCommandChanged(type);
          foreach (var item in ObsFileFixers)
          {
              if (type== ProcessCommandType.Cancel)
              {
                  item.IsCancel = true;
              }
          }
      }

        protected override void Run(string inputPath)
        {
            ObsFileFixer ObsFileFormater = new ObsFileFixer(GetOrInitObsFileFormatOption(), inputPath);
            ObsFileFixers.Add(ObsFileFormater);

            ObsFileFormater.Init();
            ObsFileFormater.Run();

            base.Run(inputPath);
        }
    }
}