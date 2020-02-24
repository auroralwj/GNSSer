//2016.10.27, czs,  create in hongqing, 更新坐标文件

using AnyInfo;
using AnyInfo.Features;
using AnyInfo.Geometries;
using Geo;
using Geo.Algorithm.Adjust;
using Geo.Coordinates;
using Geo.IO;
using Geo.Referencing;
using Geo.Times;
using Geo.Utils;
using Gnsser;
using Gnsser.Checkers;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using  System.Threading.Tasks;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Gnsser.Winform
{
    /// <summary>
    /// 更新坐标文件。
    /// </summary>
    public partial class ObsFileSelectForm :  Gnsser.Winform.ParalleledFileForm// ObsFileStreamerForm
    {
        Log log = new Log(typeof(ObsFileSelectForm));
        public ObsFileSelectForm()
        {
            InitializeComponent();
            //this.SetIsMultiObsFile(false);
            //positonResultRenderControl11.IsHasIndexParamName = true;
            this.SetExtendTabPageCount(0, 0);
            this.SetEnableMultiSysSelection(false);

        }

        #region 属性 
        /// <summary>
        /// 失败的数量
        /// </summary>
        public int SelectedCount { get; set; }
        ObsFileSelectOption ObsFileSelectOption { get; set; }
        ObsFileSelector ObsFileSelector { get; set; }
        #endregion

        #region 方法
        protected override void Init()
        {
            base.Init();
            SelectedCount = 0;

            ShowInfo("开始执行");
            this.ObsFileSelectOption = GetOrInitObsFileSelectOption();



            ObsFileSelector = new ObsFileSelector(ObsFileSelectOption, OutputDirectory);
        }
         

        protected override void Run(string inputPath)
        {
            var subDirectory = Geo.Utils.PathUtil.GetSubDirectory(this.InputRawPathes, inputPath);
            if (ObsFileSelector.Select(inputPath, subDirectory))
            {
                SelectedCount++;
            }

            base.Run(inputPath);
        }

  
        protected override void DetailSetting()
        {
           this.ObsFileSelectOption = GetOrInitObsFileSelectOption();
           var form = new ObsFileSelectOptionForm(ObsFileSelectOption);
           if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
           {
               this.ObsFileSelectOption = form.Option;
           }
        }
        public ObsFileSelectOption GetOrInitObsFileSelectOption()
        {
            if (ObsFileSelectOption == null) { return new ObsFileSelectOption(); }
            else return ObsFileSelectOption;
        }

        protected override string BuildFinalInfo()
        {
            var info = new StringBuilder();
            info.AppendLine("选择数量: " + SelectedCount);
            info.AppendLine("失败数量: " + ObsFileSelector.FailedPathes.Count);
            foreach (var item in this.ObsFileSelector.FailedPathes)
            {
                info.AppendLine(item);

            }
            return base.BuildFinalInfo() + info;
        }
        
        #endregion
    }
  
}