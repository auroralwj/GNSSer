//2017.03.20, czs, create in hongqing,选星
//2018.08.03, czs, edit in hmx, 增加表格输出

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
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
using Geo.Coordinates;
using Geo;
using Gnsser.Checkers;

namespace Gnsser.Winform
{
    public partial class SatelliteSelectorForm : MultiFileStreamerForm
    {
        public SatelliteSelectorForm()
        {
            InitializeComponent();

            fileOpenControl_inputPathes.Filter =  Setting.SatElevationFileFilter;
        
            SetExtendTabPageCount(0, 5);
            SetEnableMultiSysSelection(false);
            SetEnableDetailSettingButton(false);
        }


        #region 属性
        /// <summary>
        /// 时段分段数量
        /// </summary>
        public int TimePeriodCount { get { return namedIntControl_timePeriodCount.Value; } }
        /// <summary>
        /// 是否扩展相同PRN的区域。
        /// </summary>
        public bool IsExpandPeriodOfSamePrn { get { return checkBoxIsExpandPeriod.Checked; } }
        /// <summary>
        /// 是否采用灵活分段方法。
        /// </summary>
        public bool IsFlexibleSegmentaion { get { return radioButton_unfixed.Checked; } }
        #endregion

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
        protected override void Run(string[] inputPathes)
        {
            ObjectTableStorage satEleTable = GetSatEleTable(inputPathes);

            if(satEleTable == null)
            {
                return;
            }
            this.BindTableC(satEleTable);

            log.Info("成功读取卫星高度角文件");
            AbstractPeriodBaseSatSelector BasePrnSelector = null;
            PeriodPrnManager PeriodPrnManager = null;
            string detailTableName = "BasePrnChainDetails";
            if (!IsFlexibleSegmentaion)
            {
                var type = enumRadioControl1.GetCurrent<BaseSatSelectionType>();
                detailTableName += "Of" + type + "_" + TimePeriodCount + "Count";
                BasePrnSelector = new PeriodFixedSatSelector(satEleTable, namedFloatControl1AngleCut.Value, TimePeriodCount, IsExpandPeriodOfSamePrn, type);
            }
            else
            {
                detailTableName += "OfFlexible_" + namedFloatControl1AngleCut.Value + "deg";
                BasePrnSelector = new FlexiblePeriodSatSelector(satEleTable, namedFloatControl1AngleCut.Value);
            }
            PeriodPrnManager = BasePrnSelector.Select();
            var outPath = Path.Combine(this.OutputDirectory, Setting.GnsserConfig.BasePrnFileName);
            PeriodPrnManager.WriteToFile(outPath);
            //基准星接力
            var mgr = new ObjectTableManager(this.OutputDirectory);
            var basePrnChain = mgr.AddTable(detailTableName);
            foreach (var item in PeriodPrnManager)
            {
                var vector = satEleTable.GetColObjectDicByObjIndex(item.Value.ToString(), item.TimePeriod.Start, item.TimePeriod.End);
                if (vector != null)
                    foreach (var prnVal in vector)
                    {
                        basePrnChain.NewRow();
                        basePrnChain.AddItem("Epoch", prnVal.Key);
                        basePrnChain.AddItem(item.Value.ToString(), prnVal.Value);
                        basePrnChain.EndRow();
                    }
            }

            this.BindTableA(basePrnChain);
            this.BindTableB(BasePrnSelector.DetailResultTable);

            mgr.Add(BasePrnSelector.DetailResultTable);
            mgr.WriteAllToFileAndCloseStream();

            var text = mgr.First.GetTextTable();// PeriodPrnManager.GetText();
            ShowInfo(text);


        }

        private ObjectTableStorage GetSatEleTable(string[] inputPathes)
        {
            ObjectTableStorage satEleTable = null;
            bool isInputCoord = this.checkBox_inputCoord.Checked;
            var cutOffAngle = namedFloatControl1AngleCut.GetValue();
            if (!isInputCoord)
            {
                log.Info("采用了文件输入高度角");
                if (inputPathes.Length == 0)
                {
                    Geo.Utils.FormUtil.ShowWarningMessageBox("请输入文件"); return satEleTable;
                }
                var baseSiteEle = inputPathes[0];
                if (baseSiteEle.ToLower().EndsWith(Setting.SatElevationFileExtension.ToLower()))
                {
                    var reader = new ObjectTableReader(baseSiteEle);
                    satEleTable = reader.Read();
                }
                else if (baseSiteEle.ToLower().EndsWith("rnx") || baseSiteEle.ToLower().EndsWith("o"))//O文件
                {
                    satEleTable = SatElevatoinTableBuilder.BuildTable(baseSiteEle, cutOffAngle, GlobalNavEphemerisService.Instance);
                }
            }
            else//手动输入
            {
                log.Info("即将跟输入计算高度角信息");

                //输出的采样间隔 
                TimeLooper looper = timeLoopControl1.GetTimeLooper();
                var xyz = XYZ.Parse(this.namedStringControl_coord.GetValue());
                satEleTable = SatElevatoinTableBuilder.BuildTable(looper, xyz, this.SatelliteTypes, cutOffAngle, GlobalNavEphemerisService.Instance, "卫星高度角");
            }

            return satEleTable;
        }

        private void WideLaneSolverForm_Load(object sender, EventArgs e)
        {
            namedFloatControl1AngleCut.SetValue(10);
            checkBox_inputCoord.Checked = false;
            UpdateEnableInputCoord(checkBox_inputCoord.Checked);

            this.namedStringControl_coord.SetValue("5105515.7657  -555145.6601  3769804.8319 ");
            this.fileOpenControl_inputPathes.FilePath = Setting.GnsserConfig.SatElevationPath;
            enumRadioControl1.Init<BaseSatSelectionType>();
        }

        private void radioButton_fixed_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton_unfixed_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button_coordSet_Click(object sender, EventArgs e)
        {
            CoordSelectForm form = new CoordSelectForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.namedStringControl_coord.SetValue(form.XYZ.ToString());
            }
        }

        private void checkBox_inputCoord_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEnableInputCoord(checkBox_inputCoord.Checked);
        }

        private void UpdateEnableInputCoord(bool isEnable)
        {
            timeLoopControl1.Enabled = isEnable;
            namedStringControl_coord.Enabled = isEnable;
            button_coordSet.Enabled = isEnable;

            fileOpenControl_inputPathes.Enabled = !isEnable;

        }
    }
}
