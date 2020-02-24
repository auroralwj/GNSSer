//2017.08.12, czs, create in hongqing, 状态转移模型参数设置

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
    /// <summary>
    /// 状态转移模型参数设置
    /// </summary>
    public partial class StateTransferModelOptionPage : BaseGnssProcessOptionPage
    {
        public StateTransferModelOptionPage()
        {
            InitializeComponent();

            this.Name = "状态转移参数";
        }

        public override void UiToEntity()
        {
            base.UiToEntity();

            Option.PhaseCovaProportionToRange = namedFloatControl_PhaseCovaProportionToRange.Value;
            Option.IsPromoteTransWhenResultValueBreak = this.checkBox_IsPromoteTransWhenResultValueBreak.Checked;

            #region 随机模型参数默认值
            Option.StdDevOfSysTimeRandomWalkModel = this.namedFloatControl1StdDevOfSysTimeRandomWalkModel.Value;// 3e-8;
            Option.StdDevOfRandomWalkModel = this.namedFloatControl1StdDevOfRandomWalkModel.Value;// 3e-8;
            Option.StdDevOfPhaseModel = this.namedFloatControl2StdDevOfPhaseAmbiguityModel.Value;// 2e7;
            Option.StdDevOfCycledPhaseModel = this.namedFloatControl1StdDevOfCycledPhaseModel.Value;// 2e7;
            Option.StdDevOfIonoRandomWalkModel = this.namedFloatControl3StdDevOfIonoRandomWalkModel.Value;//1.7e-2;
            Option.StdDevOfStaticTransferModel = this.namedFloatControl4StdDevOfStaticTransferModel.Value;//1e-10;
            Option.StdDevOfTropoRandomWalkModel = this.namedFloatControl5StdDevOfTropoRandomWalkModel.Value;// 1.7e-7;
            Option.StdDevOfRevClockWhiteNoiseModel = this.namedFloatControl6StdDevOfWhiteNoiseModel.Value;// 2e7;
            Option.StdDevOfSatClockWhiteNoiseModel = this.namedFloatControl_StdDevOfSatClockWhiteNoiseModel.Value;// 2e7;

            Option.StdDevOfWhiteNoiseOfDynamicPosition = this.namedFloatControl_rmsOfWhitenoiceOfDynamicPos.Value;
            #endregion
        }
        public override void EntityToUi()
        {
            base.UiToEntity();
            this.checkBox_IsPromoteTransWhenResultValueBreak.Checked = Option.IsPromoteTransWhenResultValueBreak;
            namedFloatControl_PhaseCovaProportionToRange.Value =  Option.PhaseCovaProportionToRange; 

            #region 随机模型参数默认值
            this.namedFloatControl1StdDevOfSysTimeRandomWalkModel.Value = Option.StdDevOfSysTimeRandomWalkModel;
            this.namedFloatControl1StdDevOfRandomWalkModel.Value = Option.StdDevOfRandomWalkModel;// 3e-8;
            this.namedFloatControl2StdDevOfPhaseAmbiguityModel.Value = Option.StdDevOfPhaseModel;// 2e7;
            this.namedFloatControl1StdDevOfCycledPhaseModel.Value = Option.StdDevOfCycledPhaseModel;// 2e7;
            this.namedFloatControl3StdDevOfIonoRandomWalkModel.Value = Option.StdDevOfIonoRandomWalkModel;//1.7e-2;
            this.namedFloatControl4StdDevOfStaticTransferModel.Value = Option.StdDevOfStaticTransferModel;//1e-10;
            this.namedFloatControl5StdDevOfTropoRandomWalkModel.Value = Option.StdDevOfTropoRandomWalkModel;// 1.7e-7;
            this.namedFloatControl6StdDevOfWhiteNoiseModel.Value = Option.StdDevOfRevClockWhiteNoiseModel;// 2e7;
            this.namedFloatControl_StdDevOfSatClockWhiteNoiseModel.Value = Option.StdDevOfSatClockWhiteNoiseModel;// 2e7;
            this.namedFloatControl_rmsOfWhitenoiceOfDynamicPos.Value =  Option.StdDevOfWhiteNoiseOfDynamicPosition;
            #endregion
        }

        private void GnssOptionForm_Load(object sender, EventArgs e)
        {

        }
    }
}