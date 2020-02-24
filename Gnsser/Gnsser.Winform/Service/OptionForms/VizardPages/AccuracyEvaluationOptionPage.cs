//2018.12.10, czs, create in hmx, 静态精度评估

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
    public partial class AccuracyEvaluationOptionPage : BaseGnssProcessOptionPage
    {
        public AccuracyEvaluationOptionPage( )
        {
            InitializeComponent();

            this.Name = "静态精度评估"; 
        }


        public override void UiToEntity()
        {
            base.UiToEntity();

            Option.IsEnableEpochParamAnalysis = this.enabledStringControl_analysisParam.GetEnabledValue().Enabled;
            Option.AnalysisParamNamesString = this.enabledStringControl_analysisParam.GetEnabledValue().Value;

            Option.MaxAllowedRmsOfAccuEval = namedFloatControl_maxAllowedRms.GetValue();
            Option.MaxAllowedConvergenceTime = namedFloatControl_maxAllowConvergTime.GetValue();
            Option.MaxAllowedDifferAfterConvergence = namedFloatControl1MaxAllowedDifferAfterConvergence.GetValue();
            Option.MaxDifferOfAccuEval = this.namedFloatControl_maxDiffer.GetValue();
            Option.SequentialEpochCountOfAccuEval = this.namedIntControl_epochCount.GetValue();
            Option.KeyLabelCharCount = namedIntControl_labelCharCount.GetValue();
        }

        public override void EntityToUi()
        {
            base.UiToEntity();
             
            enabledStringControl_analysisParam.SetEnabledValue(new Geo.EnableString(Option.AnalysisParamNamesString, Option.IsEnableEpochParamAnalysis));


            namedFloatControl_maxAllowedRms.SetValue(Option.MaxAllowedRmsOfAccuEval );
            namedFloatControl_maxAllowConvergTime.SetValue(Option.MaxAllowedConvergenceTime );
            namedFloatControl1MaxAllowedDifferAfterConvergence.SetValue(Option.MaxAllowedDifferAfterConvergence);
            this.namedFloatControl_maxDiffer.SetValue(Option.MaxDifferOfAccuEval );
            this.namedIntControl_epochCount.SetValue(Option.SequentialEpochCountOfAccuEval );
            namedIntControl_labelCharCount.SetValue(Option.KeyLabelCharCount);

        }
          
        private void GnssOptionForm_Load(object sender, EventArgs e)
        {
           
        } 

    }
}