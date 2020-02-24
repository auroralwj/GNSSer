//2017.08.11, czs, edit in hongqing, 单独提出
//2018.04.27, czs, edit in hmx, 移走手动设置星历部分
//2018.05.29, czs, edit in hmx, 从数据源中移出

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
    public partial class CorrectionOptionPage : BaseGnssProcessOptionPage
    {
        public CorrectionOptionPage()
        {
            InitializeComponent();
            this.Name = "改正数";
            this.enumRadioControl_IonoSourceTypeForCorrection.Init<IonoSourceType>();
        }


        public override void UiToEntity()
        {
            base.UiToEntity();

            Option.IonoSourceTypeForCorrection = enumRadioControl_IonoSourceTypeForCorrection.GetCurrent<IonoSourceType>();

            #region  改正数 
            Option.IsRecAntPcoCorrectionRequired = checkBox_IsRecAntennaPcoCorrectionRequired.Checked;
            Option.IsReceiverAntSiteBiasCorrectionRequired = checkBox_IsReceiverAntSiteBiasCorrectionRequired.Checked;

            Option.IsDcbOfP1P2Enabled = this.checkBox_IsDcbOfP1P2Enabled.Checked;
            Option.IsP1DcbToLcOfGridIonoRequired = this.checkBox_IsP1DcbToLcRequired.Checked;
            Option.IsObsCorrectionRequired = checkBox_IsObsCorrectionRequired.Checked;
            Option.IsApproxModelCorrectionRequired = checkBox_IsApproxModelCorrectionRequired.Checked;
            Option.IsDcbCorrectionRequired = checkBox_IsDcbCorrectionRequired.Checked;
            Option.IsOceanTideCorrectionRequired = checkBox_IsOceanTideCorrectionRequired.Checked;
            Option.IsSolidTideCorrectionRequired = checkBox_IsSolidTideCorrectionRequired.Checked;
            Option.IsPoleTideCorrectionRequired = checkBox_IsPoleTideCorrectionRequired.Checked;
            Option.IsSatClockBiasCorrectionRequired = checkBox_IsSatClockBiasCorrectionRequired.Checked;
            Option.IsTropCorrectionRequired = checkBox_IsTropCorrectionRequired.Checked;
            Option.IsGravitationalDelayCorrectionRequired = checkBox_IsGravitationalDelayCorrectionRequired.Checked;
            Option.IsSatAntPcoCorrectionRequired = checkBox_IsSatAntPcoCorrectionRequired.Checked;
            Option.IsSatAntPvcCorrectionRequired = checkBox_IsSatAntPvcCorrectionRequired.Checked;
            Option.IsRecAntPcvCorrectionRequired = checkBox_IsRecAntennaPcvRequired.Checked;
            Option.IsPhaseWindUpCorrectionRequired = checkBox_IsPhaseWindUpCorrectionRequired.Checked;
            Option.IsSiteCorrectionsRequired = checkBox_IsSiteCorrectionsRequired.Checked;
            Option.IsRangeCorrectionsRequired = checkBox_IsRangeCorrectionsRequired.Checked;
            Option.IsFrequencyCorrectionsRequired = checkBox_IsFrequencyCorrectionsRequired.Checked;

            Option.IsIonoCorretionRequired = this.checkBox_ionoCorrection.Checked;
            Option.IsSmoothRange = this.checkBox_IsSmoothingRange.Checked;
              
            #endregion 

        }
        public override void EntityToUi()
        {
            base.EntityToUi();

            enumRadioControl_IonoSourceTypeForCorrection.SetCurrent< IonoSourceType>( Option.IonoSourceTypeForCorrection);


            #region  改正数
            checkBox_IsSatAntPvcCorrectionRequired.Checked = Option.IsSatAntPvcCorrectionRequired;
            checkBox_IsRecAntennaPcoCorrectionRequired.Checked = Option.IsRecAntPcoCorrectionRequired;
            checkBox_IsReceiverAntSiteBiasCorrectionRequired.Checked = Option.IsReceiverAntSiteBiasCorrectionRequired;

            this.checkBox_IsDcbOfP1P2Enabled.Checked = Option.IsDcbOfP1P2Enabled;
            this.checkBox_IsP1DcbToLcRequired.Checked = Option.IsP1DcbToLcOfGridIonoRequired;
            checkBox_IsObsCorrectionRequired.Checked = Option.IsObsCorrectionRequired;
            checkBox_IsApproxModelCorrectionRequired.Checked = Option.IsApproxModelCorrectionRequired;
            checkBox_IsDcbCorrectionRequired.Checked = Option.IsDcbCorrectionRequired;
            checkBox_IsOceanTideCorrectionRequired.Checked = Option.IsOceanTideCorrectionRequired;
            checkBox_IsSolidTideCorrectionRequired.Checked = Option.IsSolidTideCorrectionRequired;
            checkBox_IsPoleTideCorrectionRequired.Checked = Option.IsPoleTideCorrectionRequired;
            checkBox_IsSatClockBiasCorrectionRequired.Checked = Option.IsSatClockBiasCorrectionRequired;
            checkBox_IsTropCorrectionRequired.Checked = Option.IsTropCorrectionRequired;
            checkBox_IsGravitationalDelayCorrectionRequired.Checked = Option.IsGravitationalDelayCorrectionRequired;
            checkBox_IsSatAntPcoCorrectionRequired.Checked = Option.IsSatAntPcoCorrectionRequired;
            checkBox_IsRecAntennaPcvRequired.Checked = Option.IsRecAntPcvCorrectionRequired;
            checkBox_IsPhaseWindUpCorrectionRequired.Checked = Option.IsPhaseWindUpCorrectionRequired;
            checkBox_IsSiteCorrectionsRequired.Checked = Option.IsSiteCorrectionsRequired;
            checkBox_IsRangeCorrectionsRequired.Checked = Option.IsRangeCorrectionsRequired;
            checkBox_IsFrequencyCorrectionsRequired.Checked = Option.IsFrequencyCorrectionsRequired;

            this.checkBox_ionoCorrection.Checked = Option.IsIonoCorretionRequired;


            this.checkBox_IsSmoothingRange.Checked = Option.IsSmoothRange;
            #endregion


        }


        private void GnssOptionForm_Load(object sender, EventArgs e)
        {
        }

        private void tabPage_datasource_Click(object sender, EventArgs e)
        {

        }
    }
}