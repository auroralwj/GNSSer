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
using Geo.Algorithm.Adjust;

namespace Gnsser.Winform
{
    public partial class GnssCaculatorOptionPage : BaseGnssProcessOptionPage
    {
        public GnssCaculatorOptionPage()
        {
            InitializeComponent();

            this.Name = "计算器"; 
            enumRadioControl1AdjustmentType.Init<AdjustmentType>();
            this.enumRadioControl1StepOfRecursive.Init<StepOfRecursive>();
        }

        private void GnssSolverSelectionControl1_RadioSelected(GnssSolverType obj)
        {
            if (Option.GnssSolverType  != gnssSolverSelectionControl1.CurrentdType
                && Geo.Utils.FormUtil.ShowYesNoMessageBox(" 已选择 “" + obj + "”，是否加载默认其设置？") == DialogResult.Yes)
            {
                button_loadDefault_Click(null, null);
            }
        }

        public override void UiToEntity()
        {
            base.UiToEntity();
            Option.IsAdjustEnabled = checkBox_enableAdjust.Checked;

            Option.StepOfRecursive = this.enumRadioControl1StepOfRecursive.GetCurrent<StepOfRecursive>();
            Option.AdjustmentType = enumRadioControl1AdjustmentType.GetCurrent<AdjustmentType>();           
            Option.CaculateType = this.radioButton_cacuWithKalmanFilter.Checked ? CaculateType.Filter : CaculateType.Independent;
            Option.GnssSolverType = gnssSolverSelectionControl1.CurrentdType;
        }

        public override void EntityToUi()
        {
            base.UiToEntity();

            checkBox_enableAdjust.Checked = Option.IsAdjustEnabled;
            this.enumRadioControl1StepOfRecursive.SetCurrent<StepOfRecursive>(Option.StepOfRecursive);
            enumRadioControl1AdjustmentType.SetCurrent<AdjustmentType>(Option.AdjustmentType);    
            this.radioButton_cacuWithKalmanFilter.Checked = Option.CaculateType == CaculateType.Filter;
            this.radioButton_cacu_noFilter.Checked = Option.CaculateType == CaculateType.Independent;             
            gnssSolverSelectionControl1.CurrentdType = Option.GnssSolverType;
        }
         

        private void GnssOptionForm_Load(object sender, EventArgs e)
        {
            gnssSolverSelectionControl1.RadioSelected += GnssSolverSelectionControl1_RadioSelected;
        }

        private void button_loadDefault_Click(object sender, EventArgs e)
        {
            var CurrentdType = gnssSolverSelectionControl1.CurrentdType; 
            this.Option = GnssProcessOptionManager.Instance[CurrentdType];
            this.EntityToUi();
        }

        private void checkBox_enableAdjust_CheckedChanged(object sender, EventArgs e)
        {
            enumRadioControl1AdjustmentType.Enabled = this.checkBox_enableAdjust.Checked;
        }
    }
}