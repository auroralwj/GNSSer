//2018.09.30, czs, create in hongqing, 同名测站选择器

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Domain;
using Gnsser.Data.Rinex;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Times;
using Gnsser.Service;
using Geo.IO;
using Geo;
using Geo.Common;
using Gnsser.Checkers;

namespace Gnsser.Winform
{
    public partial class SameSiteSelectForm : ParalleledFileForm
    {
        public SameSiteSelectForm()
        {
            InitializeComponent();

            //SetExtendTabPageCount(0, 0);
            //   SetEnableMultiSysSelection(false);
            SetEnableDetailSettingButton(false);
        }



        #region 属性  
        int LabelCharCount { get; set; } 
        bool IsIgnoreCase { get; set; }
        bool IsMoveOrCopy { get; set; }

        #endregion

        BaseConcurrentDictionary<string, List<string>> Result { get; set; }

        public override void PreRun()
        {
            base.PreRun();
            LabelCharCount = namedIntControl_labelCharCount.GetValue();
            IsIgnoreCase = this.checkBox_ignoreCase.Checked;
            IsMoveOrCopy = this.checkBox1_moveOrCopy.Checked;

            Result = new BaseConcurrentDictionary<string, List<string>>("结果", (key) => new List<string>());
        }

        protected override void Run(string[] inputPathes)
        {
            this.ProgressBar.Init(new List<string> { "统计", "复制或移动"}, inputPathes.Length);

            foreach (var path in inputPathes)
            {
                RinexObsFileHeader header = RinexObsFileReader.ReadHeader(path);
                var name = Geo.Utils.StringUtil.SubString( header.MarkerName, 0 , LabelCharCount);
                if (IsIgnoreCase) { name = name.ToUpper(); }
                Result.GetOrCreate(name).Add(path);

                this.ProgressBar.PerformProcessStep();
            }
            this.ProgressBar.PerformClassifyStep(Result.Data.Count);
            int counter = 0;
            foreach (var item in Result.Data)
            {
                if(item.Value.Count > 1)
                {
                    foreach (var path in item.Value)
                    {
                        var toPath = Path.Combine(OutputDirectory, item.Key, Path.GetFileName(path));
                        Geo.Utils.FileUtil.CopyOrMoveFile(path, toPath,!IsMoveOrCopy, true);

                        counter++;
                    }
                }
                this.ProgressBar.PerformProcessStep();
            }
            var info = "发现了 " + counter + " 个同名测站。已 " +( IsMoveOrCopy ? " 移动!!如在默认临时文件夹，请及时移出！！否则重启后将被清空。 " : " 复制 ");
            log.Info(info);
        }



        private void fileOpenControl1_FilePathSetted(object sender, EventArgs e)
        {
            if(fileOpenControl_inputPathes.FilePathes.Length > 0)
            this.OutputDirectory = Path.GetDirectoryName(fileOpenControl_inputPathes.FilePathes[0]);
        }


    }
}
