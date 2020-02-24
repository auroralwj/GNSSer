//2017.08.11, czs, edit in hongqing, 单独提出
//2017.09.08, czs, edit in hongqing, 增加电离层、对流层，及汇总输出。

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
    public partial class OutputOptionPage : BaseGnssProcessOptionPage
    {
        public OutputOptionPage( )
        {
            InitializeComponent();

            this.Name = "输出"; 
        }

        public override void UiToEntity()
        {
            base.UiToEntity();

            Option.IsOutputJumpClockFile = checkBox_IsOutputJumpClockFile.Checked;
            Option.IsOutputInGnsserFormat = checkBox_IsOutputInGnsserFormat.Checked;
            Option.IsOpenReportWhenCompleted = checkBox_IsOpenReportWhenCompleted.Checked;
            Option.OutputBufferCount = int.Parse(this.textBox_OutputBufferCount.Text);
           
            Option.OutputDirectory = this.directorySelectionControl1.Path; 
            Option.IsOutputCycleSlipFile = this.checkBox_IsOutputCycleSlipFile.Checked;
            Option.IsOutputEpochSatInfo = checkBox_outputSiteSat.Checked;
            Option.IsOutputEpochResult = checkBox_outputEpochInfo.Checked;
            Option.IsOutputAdjust = checkBox_outputAdjust.Checked;
            Option.IsOutputResult = checkBox_outputResult.Checked;
            Option.IsOutputResidual = this.checkBox_residual.Checked;
            Option.IsOutputAdjustMatrix = checkBox_IsOutputAdjustMatrix.Checked;

            Option.IsOutputIono = this.checkBoxIonoOutput.Checked;
            Option.IsOutputWetTrop = this.checkBoxIsOutputWetTrop.Checked;

            Option.IsOutputSummery = this.checkBox_IsOutputSummery.Checked;
            Option.IsOutputSinex = this.checkBox_IsOutputSinex.Checked;

            Option.IsOutputEpochParam = this.checkBox_IsOutputEpochParam.Checked;
            Option.IsOutputEpochParamRms = this.checkBox_IsOutputEpochParamRms.Checked;
            Option.IsOutputObservation = this.checkBox_observation.Checked;
            Option.IsOutputEpochCoord = this.checkBox_EpochCoord.Checked;
            Option.IsOutputObsEquation =  this.checkBox_IsOutputObsEquation.Checked;

            Option.IsOutputEpochDop =  this.checkBox_epochDops.Checked;
            Option.OutputMinInterval = namedFloatControl_outEpochInterval.GetValue();
        }

        public override void EntityToUi()
        {
            this.checkBox_IsOutputEpochParam.Checked = Option.IsOutputEpochParam;
            this.checkBox_IsOutputEpochParamRms.Checked = Option.IsOutputEpochParamRms;
            checkBox_IsOutputJumpClockFile.Checked = Option.IsOutputJumpClockFile;
            checkBox_IsOutputInGnsserFormat.Checked = Option.IsOutputInGnsserFormat;
            this.textBox_OutputBufferCount.Text = Option.OutputBufferCount + "";
            this.directorySelectionControl1.Path = Option.OutputDirectory;
            this.checkBoxIsOutputWetTrop.Checked = Option.IsOutputWetTrop;
            checkBox_IsOpenReportWhenCompleted.Checked = Option.IsOpenReportWhenCompleted;

            this.checkBox_residual.Checked = Option.IsOutputResidual;
            this.checkBoxIonoOutput.Checked = Option.IsOutputIono;
            this.checkBox_IsOutputCycleSlipFile.Checked = Option.IsOutputCycleSlipFile;
            checkBox_IsOutputAdjustMatrix.Checked = Option.IsOutputAdjustMatrix;
            checkBox_outputSiteSat.Checked = Option.IsOutputEpochSatInfo;
            checkBox_outputEpochInfo.Checked = Option.IsOutputEpochResult;
            checkBox_outputAdjust.Checked = Option.IsOutputAdjust;
            checkBox_outputResult.Checked = Option.IsOutputResult;
            this.checkBox_IsOutputSummery.Checked = Option.IsOutputSummery;
            this.checkBox_IsOutputSinex.Checked = Option.IsOutputSinex;

            this.checkBox_EpochCoord.Checked = Option.IsOutputEpochCoord;
            this.checkBox_epochDops.Checked = Option.IsOutputEpochDop;
            this.checkBox_observation.Checked = Option.IsOutputObservation;
            this.checkBox_IsOutputObsEquation.Checked = Option.IsOutputObsEquation;
      
            namedFloatControl_outEpochInterval.SetValue(Option.OutputMinInterval);
        }
         
        private void GnssOptionForm_Load(object sender, EventArgs e)
        {
            
        }  
    }
}