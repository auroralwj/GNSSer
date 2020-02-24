//2017.08.11, czs, edit in hongqing, 单独提出

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser.Service;
using Gnsser.Times;
using Geo.Times;
using Geo.Coordinates;
using Geo.Algorithm;
using Geo.Winform.Wizards;
 

namespace Gnsser.Winform
{
    public partial class CycleSlipOptionPage : BaseGnssProcessOptionPage
    {
        public CycleSlipOptionPage()
        {
            InitializeComponent();

            this.Name = "周跳和钟跳";

            fileOpenControl_clockJumpFilePath.Filter = Setting.ClockJumpFileFilter;
        }


        public override void UiToEntity()
        {
            base.UiToEntity();

            #region 周跳
            Option.CycleSlipDetectSwitcher = enumCheckBoxControl1.UiToEntity<CycleSlipDetectorType>( Option.CycleSlipDetectSwitcher);

            Option.IsCycleSlipDetectionRequired = this.checkBox_IsCycleSlipDetectionRequired.Checked;

            Option.IsEnableRealTimeCs = this.checkBoxRealTimeCs.Checked;
            Option.IsEnableBufferCs  =  this.checkBoxBufferCs.Checked;

            Option.IsReverseCycleSlipeRevise = checkBox_IsReverseCycleSlipeRevise.Checked;
            Option.IsUsingRecordedCycleSlipInfo = this.checkBox1IsUsingRecordedCycleSlipInfo.Checked;
            Option.MaxDifferValueOfMwCs = this.namedFloatControl1MaxDifferValueOfMwCs.Value;
            Option.MaxBreakingEpochCount = this.namedFloatControlMaxBreakingEpochCount.Value;
            Option.MaxValueDifferOfHigherDifferCs = this.namedFloatControl1MaxValueDifferOfHeigherDifferCs.Value;
            Option.MaxRmsTimesOfLsPolyCs = this.namedFloatControl1MaxDifferOfLsPolyCs.Value;           
            Option.MinWindowSizeOfCs = this.namedIntControl2MinWindowSizeOfBufferCs.Value;
            //缓存探测
            Option.MaxErrorTimesOfBufferCs = this.namedFloatControl3MaxErrorTimesOfBufferCs.Value;
            Option.DifferTimesOfBufferCs = this.namedIntControl3DifferTimesOfBufferCs.Value;
            Option.PolyFitOrderOfBufferCs = this.namedIntControl1PolyFitOrderOfBufferCs.Value;
            Option.IgnoreCsedOfBufferCs = checkBox1IgnoreCsedOfBufferCs.Checked;

            #endregion

            #region  钟跳
            Option.IsDetectClockJump = this.checkBox_IsDetectClockJump.Checked;
            Option.IsClockJumpReparationRequired = this.checkBox_isClockJumpRepaired.Checked;
            Option.IsOpenClockJumpSwitcher = checkBox_clockJumpSwitsher.Checked;
            Option.OuterClockJumpFile = this.fileOpenControl_clockJumpFilePath.FilePath;
            #endregion
        } 
        public override void EntityToUi()
        {
            base.UiToEntity();  

            #region 周跳
            enumCheckBoxControl1.EntityToUi<CycleSlipDetectorType>(Option.CycleSlipDetectSwitcher);


            this.checkBox_IsCycleSlipDetectionRequired.Checked = Option.IsCycleSlipDetectionRequired;

            this.checkBoxRealTimeCs.Checked = Option.IsEnableRealTimeCs ;
            this.checkBoxBufferCs.Checked = Option.IsEnableBufferCs;


            checkBox_IsReverseCycleSlipeRevise.Checked = Option.IsReverseCycleSlipeRevise;
            this.checkBox1IsUsingRecordedCycleSlipInfo.Checked = Option.IsUsingRecordedCycleSlipInfo ;
            this.namedFloatControl1MaxDifferValueOfMwCs.Value = Option.MaxDifferValueOfMwCs;
            this.namedFloatControlMaxBreakingEpochCount.Value = Option.MaxBreakingEpochCount;
            this.namedFloatControl1MaxValueDifferOfHeigherDifferCs.Value = Option.MaxValueDifferOfHigherDifferCs;
            this.namedFloatControl1MaxDifferOfLsPolyCs.Value = Option.MaxRmsTimesOfLsPolyCs;
            this.namedIntControl2MinWindowSizeOfBufferCs.Value = Option.MinWindowSizeOfCs;
            //缓存探测
            this.namedFloatControl3MaxErrorTimesOfBufferCs.Value = Option.MaxErrorTimesOfBufferCs;
            this.namedIntControl3DifferTimesOfBufferCs.Value = Option.DifferTimesOfBufferCs;
            this.namedIntControl1PolyFitOrderOfBufferCs.Value = Option.PolyFitOrderOfBufferCs;
            checkBox1IgnoreCsedOfBufferCs.Checked = Option.IgnoreCsedOfBufferCs;
            #endregion

            #region  钟跳
            this.checkBox_IsDetectClockJump.Checked = Option.IsDetectClockJump;
            this.checkBox_isClockJumpRepaired.Checked = Option.IsClockJumpReparationRequired;
            checkBox_clockJumpSwitsher.Checked = Option.IsOpenClockJumpSwitcher;
            this.fileOpenControl_clockJumpFilePath.FilePath = Option.OuterClockJumpFile;
            #endregion

        }
          
        private void GnssOptionForm_Load(object sender, EventArgs e)
        {
            enumCheckBoxControl1.Init<CycleSlipDetectorType>();
        } 

    }
}