//2018.07.29, czs, edit in HMX, BaseLineSelectionType 基线
//2018.10.16, czs, edit in hmx, 提取修改为模糊度或固定参数专用
//2018.12.28, czs, edit in hmx, 启用时段卫星数据服务

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
    public partial class AmbiguityOptionPage : BaseGnssProcessOptionPage
    {
        public AmbiguityOptionPage()
        {
            InitializeComponent();

            this.Name = "模糊度/固定参数";
        }
 

        public override void UiToEntity()
        {
            base.UiToEntity(); 
            Option.MaxAllowedRmsOfMw = this.namedFloatControl_MaxAllowedRmsOfMw.Value;
            Option.IsOutputPeriodData = this.checkBox_IsOutputPeriodData.Checked;
            Option.IsEnableSiteSatPeriodDataService = this.checkBox_IsEnableSiteSatPeriodDataService.Checked;
            Option.IsRealTimeAmbiFixWhenOuterAmbiFileFailed = this.checkBox_IsRealTimeAmbiFixWhenOuterAmbiFileFailed.Checked;
            Option.IsFixingAmbiguity = this.checkBox_IsFixingAmbiguity.Checked;
            Option.IsFixParamByConditionOrHugeWeight = this.checkBox_IsFixParamByConditionOrHugeWeight.Checked;
            Option.IsPhaseInMetterOrCycle = this.checkBox_IsPhaseUnitMetterOrCycle.Checked;
            Option.IsUseFixedParamDirectly = this.checkBox_IsUseFixedParamDirectly.Checked;

            Option.MaxRatioOfLambda = this.namedFloatControl_MaxRatioOfLambda.Value;
            Option.MinFixedAmbiRatio = this.namedFloatControl_MinFixedAmbiRatio.Value;
            Option.MaxFloatRmsNormToFixAmbiguity = this.namedFloatControl_MaxFloatRmsNormToFixAmbiguity.Value;
            Option.AmbiguityFilePath = this.fileOpenControl_ambiguityFile.FilePath;
            Option.IsUsingAmbiguityFile = this.checkBox_ambiguityFile.Checked;
            Option.MaxAmbiDifferOfIntAndFloat = this.namedFloatControl1MaxAmbiDifferOfIntAndFloat.GetValue();
            Option.MaxRoundAmbiDifferOfIntAndFloat = this.namedFloatControl_MaxRoundAmbiDifferOfIntAndFloat.GetValue();
        }

        public override void EntityToUi()
        {
            base.UiToEntity();
            this.namedFloatControl_MaxAllowedRmsOfMw.Value = Option.MaxAllowedRmsOfMw;
            this.checkBox_IsOutputPeriodData.Checked = Option.IsOutputPeriodData;
            this.checkBox_IsEnableSiteSatPeriodDataService.Checked = Option.IsEnableSiteSatPeriodDataService;
            this.checkBox_IsRealTimeAmbiFixWhenOuterAmbiFileFailed.Checked = Option.IsRealTimeAmbiFixWhenOuterAmbiFileFailed;
            this.checkBox_IsFixParamByConditionOrHugeWeight.Checked = Option.IsFixParamByConditionOrHugeWeight;
            this.checkBox_IsUseFixedParamDirectly.Checked= Option.IsUseFixedParamDirectly;
            this.namedFloatControl_MinFixedAmbiRatio.Value = Option.MinFixedAmbiRatio;
            this.namedFloatControl_MaxRatioOfLambda.Value = Option.MaxRatioOfLambda;
            this.namedFloatControl_MaxFloatRmsNormToFixAmbiguity.Value = Option.MaxFloatRmsNormToFixAmbiguity;
            this.checkBox_IsFixingAmbiguity.Checked = Option.IsFixingAmbiguity;
            this.checkBox_IsPhaseUnitMetterOrCycle.Checked = Option.IsPhaseInMetterOrCycle;

            this.fileOpenControl_ambiguityFile.FilePath = Option.AmbiguityFilePath;
            this.checkBox_ambiguityFile.Checked = Option.IsUsingAmbiguityFile;
            this.namedFloatControl1MaxAmbiDifferOfIntAndFloat.SetValue(Option.MaxAmbiDifferOfIntAndFloat);
            this.namedFloatControl_MaxRoundAmbiDifferOfIntAndFloat.SetValue(Option.MaxRoundAmbiDifferOfIntAndFloat);
        }

        private void GnssOptionForm_Load(object sender, EventArgs e)
        { 
        } 
    }
}