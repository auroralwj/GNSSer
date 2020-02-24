//2016.10.28, czs, create  in hongqing, 观测文件选择选项。
//2016.10.29, czs, edit  in hongqing, 观测文件格式化选项。

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Common;
using Geo.Coordinates;
using Geo;
using Geo.Times;

namespace Gnsser.Winform
{
    public partial class ObsFileFixOptionForm : Form
    {
        public ObsFileFixOptionForm()
        {
            InitializeComponent();
            enumCheckBoxControlObsTypes.Init<ObsTypes>();
        }
        public ObsFileFixOptionForm( ObsFileFixOption option)
        {
            InitializeComponent(); 
            enumCheckBoxControlObsTypes.Init<ObsTypes>();
            this.SetOption(option);


        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            UiToEntity();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void SetOption(ObsFileFixOption Option)
        {
            this.Option = Option;
            EntityToUi();
        }

        /// <summary>
        /// 选项
        /// </summary>
        public ObsFileFixOption Option { get; set; }


        private void EntityToUi()
        {
            if (Option == null) { Option = CreateNewModel(); }

            checkBox1IsConvertPhaseToLength.Checked = Option.IsConvertPhaseToLength;

            var prnStr = SatelliteNumberUtils.GetString(Option.SatsToBeRemoved);
            enabledStringControl_RemoveSats.SetEnabledValue(new EnableString() { Enabled = Option.IsEnableRemoveSats, Value = prnStr });
            this.checkBox_enableCode.Checked = Option.IsEnableObsCodes;
            this.checkBox_interval.Checked = Option.IsEnableInterval;
            this.checkBox_enableTimePeriod.Checked = Option.IsEnableTimePeriod;
            this.timePeriodControl1.SetTimePerid(Option.TimePeriod);
            this.comboBox_version.Text = Option.Version.ToString();
            this.textBox_interval.Text = Option.Interval.ToString();
            this.checkBoxMinObsCodeAppearRatio.Checked = Option.IsEnableMinObsCodeAppearRatio;
            this.textBoxMinObsCodeAppearRatio.Text = Option.MinObsCodeAppearRatio.ToString();

            this.checkBoxSatelliteTypes.Checked = Option.IsEnableSatelliteTypes;
            this.multiGnssSystemSelectControl1.SetSatelliteTypes(Option.SatelliteTypes);

            textBoxNotVacantCodes.Text = Option.NotVacantCodes;
            checkBox_deleVacantSat.Checked = Option.IsDeleteVacantSat;
            Option.IsEnableObsTypes = checkBox_enableCode.Checked;
            enumCheckBoxControlObsTypes.Select<ObsTypes>(Option.ObsTypes);

            enabledFloatControl1Section.SetEnabledValue(   Option.EnabledSection);            
            checkBoxIsRemoveZeroRangeSat.Checked = Option.IsRemoveZeroRangeSat;
            this.checkBoxIsRemoveZeroPhaseSat.Checked = Option.IsRemoveZeroPhaseSat;

            this.checkBoxIsEnableAlignPhase.Checked = Option.IsEnableAlignPhase;
            this.checkBox1IsAmendBigCs.Checked = Option.IsAmendBigCs;
           
            this.checkBox1IsReomveOtherCodes.Checked = Option.IsReomveOtherCodes;
            this.textBox1OnlyCodes.Text = Option.OnlyCodesString;
            this.enabledIntControl_removeEpochCount.SetEnabledValue(new EnableInteger() { Enabled = Option.IsEnableMinEpochCount, Value = Option.MinEpochCount });
            this.namedIntControl1MaxBreakCount.SetValue(this.Option.MaxBreakCount);
        }

        public void UiToEntity()
        {
            if (Option == null) { Option = CreateNewModel(); }

            Option.IsConvertPhaseToLength = checkBox1IsConvertPhaseToLength.Checked;

            var val = enabledStringControl_RemoveSats.GetEnabledValue();
            Option.IsEnableRemoveSats = val.Enabled;
            Option.SatsToBeRemoved = SatelliteNumberUtils.ParseString(val.Value);  

            Option.IsEnableObsCodes = this.checkBox_enableCode.Checked;
            Option.IsEnableInterval = this.checkBox_interval.Checked;

            Option.IsEnableTimePeriod = this.checkBox_enableTimePeriod.Checked;
            Option.TimePeriod = this.timePeriodControl1.TimePeriod;
            Option.Version = double.Parse(this.comboBox_version.Text);
            Option.Interval = double.Parse(this.textBox_interval.Text);

            Option.IsEnableMinObsCodeAppearRatio = this.checkBoxMinObsCodeAppearRatio.Checked;
            Option.MinObsCodeAppearRatio = double.Parse(this.textBoxMinObsCodeAppearRatio.Text);

            Option.IsEnableSatelliteTypes = this.checkBoxSatelliteTypes.Checked;
            Option.SatelliteTypes =  this.multiGnssSystemSelectControl1.SatelliteTypes;

            Option.IsEnableObsTypes = checkBox_enableCode.Checked;
            Option.ObsTypes = enumCheckBoxControlObsTypes.GetSelected<ObsTypes>();

            Option.IsRemoveZeroRangeSat = checkBoxIsRemoveZeroRangeSat.Checked;
            Option.IsRemoveZeroPhaseSat = this.checkBoxIsRemoveZeroPhaseSat.Checked;

            Option.NotVacantCodes = textBoxNotVacantCodes.Text.Trim();
            Option.IsDeleteVacantSat = checkBox_deleVacantSat.Checked;
            Option.EnabledSection = enabledFloatControl1Section.GetEnabledValue();
            
            Option.IsReomveOtherCodes = this.checkBox1IsReomveOtherCodes.Checked;
            Option.OnlyCodesString = this.textBox1OnlyCodes.Text;
            Option.IsEnableAlignPhase = this.checkBoxIsEnableAlignPhase.Checked;
            Option.IsAmendBigCs = this.checkBox1IsAmendBigCs.Checked;

            Option.IsEnableMinEpochCount = this.enabledIntControl_removeEpochCount.GetEnabledValue().Enabled;
            Option.MinEpochCount = this.enabledIntControl_removeEpochCount.GetValue();
            this.Option.MaxBreakCount =  this.namedIntControl1MaxBreakCount.GetValue();
        }

        private ObsFileFixOption CreateNewModel()
        {
            return new ObsFileFixOption();
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            this.EntityToUi();
        }
    }


}
