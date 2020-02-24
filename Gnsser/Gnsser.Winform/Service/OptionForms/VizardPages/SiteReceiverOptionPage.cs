//2017.08.11, czs, edit in hongqing, 单独提出
//2018.06.29, czs, edit in HMX, 更名为 SiteReceiverOptionPage

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
    public partial class SiteReceiverOptionPage : BaseGnssProcessOptionPage
    {
        public SiteReceiverOptionPage()
        {
            InitializeComponent();

            this.Name = "测站/接收机";
        }



        public override void UiToEntity()
        {
            base.UiToEntity();
            Option.PositionType = enumRadioControl_positionType.GetCurrent<PositionType>();
            Option.IsNeedPseudorangePositionWhenProcess = this.checkBox_IsNeedPseudorangePositionWhenProcess.Checked;
            Option.IsSmoothRangeWhenPrevPseudorangePosition = this.checkBox_IsSmoothRangeWhenPrevPseudorangePosition.Checked;

            Option.IsEstimateTropWetZpd = this.checkBox1IsEstimateTropWetZpd.Checked;
            Option.IsFixingCoord = this.checkBox_IsFixingCoord.Checked;
            Option.IsApproxXyzRequired = this.checkBox_IsApproxXyzRequired.Checked;
            Option.IsUpdateEstimatePostition = this.checkBox_IsUpdateEstimatePostition.Checked;
            Option.IsUpdateStationInfo = checkBox_updateStationInfo.Checked;
            if (Option.IsUpdateStationInfo)
            {
                Option.IsStationInfoRequired = Option.IsUpdateStationInfo;
            }


            Option.IsIndicatingApproxXyz = this.checkBox_approxPos.Checked;
            Option.IsIndicatingApproxXyzRms = this.checkBox_rmsIndicated.Checked;
            Option.IsSetApproxXyzWithCoordService = this.checkBox_IsSetApproxXyzWithCoordService.Checked;

            Option.StdDevOfWhiteNoiseOfDynamicPosition = this.namedFloatControl_rmsOfWhitenoiceOfDynamicPos.Value;
            Option.IsEstDcbOfRceiver = checkBox_IsEstDcbOfRceiver.Checked;

            if (!String.IsNullOrWhiteSpace(this.textBox_approxPosRms.Text))
            {
                Option.InitApproxXyzRms = XYZ.Parse(this.textBox_approxPosRms.Text);
            }
            if (!String.IsNullOrWhiteSpace(this.textBox_aprioriPos.Text))
            {
                Option.InitApproxXyz = XYZ.Parse(this.textBox_aprioriPos.Text);
            }
            //手动输入
            double levelFixed = this.namedFloatControl_fixedErrorLevel.GetValue();
            double verticalFixed = this.namedFloatControl_fixedErrorVertical.GetValue();
            double levelCoeef = this.namedFloatControl_levelCoefOfProprotion.GetValue();
            double verticalCoeef = this.namedFloatControl_verticalCoefOfProprotion.GetValue();
            Option.GnssReveiverNominalAccuracy = new GnssReveiverNominalAccuracy(levelFixed, verticalFixed, levelCoeef, verticalCoeef);


            //测站坐标设置
            Option.IsSiteCoordServiceRequired = this.checkBox_IsSiteCoordServiceRequired.Checked;
            Option.CoordFilePath = this.fileOpenControl_coordPath.FilePath;
            Option.IsIndicatingCoordFile = this.checkBox_indicateCoordfile.Checked;
            Option.MinAllowedApproxXyzLen = this.namedFloatControl_MinAllowedApproxXyzLen.GetValue();
        }

        public override void EntityToUi()
        {
            base.UiToEntity();
            enumRadioControl_positionType.SetCurrent<PositionType>(Option.PositionType);
            this.checkBox_IsNeedPseudorangePositionWhenProcess.Checked = Option.IsNeedPseudorangePositionWhenProcess;
            this.checkBox_IsSmoothRangeWhenPrevPseudorangePosition.Checked = Option.IsSmoothRangeWhenPrevPseudorangePosition;
            this.checkBox_IsFixingCoord.Checked = Option.IsFixingCoord;
            this.checkBox1IsEstimateTropWetZpd.Checked = Option.IsEstimateTropWetZpd;
            this.checkBox_IsApproxXyzRequired.Checked = Option.IsApproxXyzRequired;
            this.checkBox_updateStationInfo.Checked = Option.IsUpdateStationInfo;
            this.checkBox_IsSetApproxXyzWithCoordService.Checked = Option.IsSetApproxXyzWithCoordService;
            this.checkBox_IsUpdateEstimatePostition.Checked = Option.IsUpdateEstimatePostition;
            this.textBox_approxPosRms.Text = Option.InitApproxXyzRms + "";
            this.textBox_aprioriPos.Text = Option.InitApproxXyz + "";
            this.checkBox_approxPos.Checked = Option.IsIndicatingApproxXyz;
            this.checkBox_rmsIndicated.Checked = Option.IsIndicatingApproxXyzRms;
            checkBox_IsEstDcbOfRceiver.Checked = Option.IsEstDcbOfRceiver;
            this.namedFloatControl_rmsOfWhitenoiceOfDynamicPos.Value = Option.StdDevOfWhiteNoiseOfDynamicPosition;

            this.namedFloatControl_fixedErrorLevel.SetValue(Option.GnssReveiverNominalAccuracy.FixedValue.Level);
            this.namedFloatControl_fixedErrorVertical.SetValue(Option.GnssReveiverNominalAccuracy.FixedValue.Vertical);
            this.namedFloatControl_levelCoefOfProprotion.SetValue(Option.GnssReveiverNominalAccuracy.CoefOfProportion.Level);
            this.namedFloatControl_verticalCoefOfProprotion.SetValue(Option.GnssReveiverNominalAccuracy.CoefOfProportion.Vertical);

            //测站坐标设置
            this.checkBox_IsSiteCoordServiceRequired.Checked = Option.IsSiteCoordServiceRequired;
            this.checkBox_indicateCoordfile.Checked = Option.IsIndicatingCoordFile;
            this.fileOpenControl_coordPath.FilePath = Option.CoordFilePath;
            this.namedFloatControl_MinAllowedApproxXyzLen.SetValue(Option.MinAllowedApproxXyzLen);
        }

        private void GnssOptionForm_Load(object sender, EventArgs e)
        {
            enumRadioControl_positionType.Init<PositionType>();       
        }

        private void radioButton_highPrecise_CheckedChanged(object sender, EventArgs e) { this.textBox_approxPosRms.Text = new XYZ(0.1, 0.1, 0.1).ToString(); }

        private void radioButton_commonPrecise_CheckedChanged(object sender, EventArgs e) { this.textBox_approxPosRms.Text = new XYZ(10, 10, 10).ToString(); }

        private void radioButton_loosePrecise_CheckedChanged(object sender, EventArgs e) { this.textBox_approxPosRms.Text = new XYZ(1000, 1000, 1000).ToString(); }



    }
}