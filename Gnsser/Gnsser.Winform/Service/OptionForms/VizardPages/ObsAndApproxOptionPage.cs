//2017.08.11, czs, edit in hongqing, 单独提出
//2018.05.29, czs, edti in hmx, 单独提出

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
    public partial class ObsAndApproxOptionPage : BaseGnssProcessOptionPage
    {
        public ObsAndApproxOptionPage()
        {
            InitializeComponent();

            this.Name = "数值";
            this.enumRadioControl_ObsPhaseType.Init<ObsPhaseType>();
        }



        public override void UiToEntity()
        {
            base.UiToEntity();
            Option.ObsPhaseType =     this.enumRadioControl_ObsPhaseType.GetCurrent<ObsPhaseType>();
            Option.IsEnableInitApriori = this.checkBox_enableApriori.Checked;
            Option.InitApriori = new Geo.Algorithm.Adjust.WeightedVector(
                new Vector(this.namedArrayControl_apriori.GetValue()),
                new DiagonalMatrix(this.namedArrayControl_rmsOfApriori.GetValue()).Pow(2.0)
                    );

            Option.ObsDataType = this.satObsDataTypeControl1.CurrentdType; 
            Option.ApproxDataType = this.satApproxDataTypeControl1.CurrentdType;
            this.Option.BaseSiteSelectType = this.enumRadioControl_BaseSiteSelectType.GetCurrent<BaseSiteSelectType>();

            Option.MultiEpochCount = int.Parse(this.textBox_multiEpochCount.Text);
            Option.MutliEpochSameSatCount = int.Parse(this.textBox_MultiEpochSameSatCount.Text);
            Option.IsSameSatRequired = this.checkBox_isRequireSameSats.Checked;
            Option.IsSmoothMoveInMultiEpoches = this.checkBox_isSmoothEpcohes.Checked;
        }

        public override void EntityToUi()
        {
            base.UiToEntity();
            this.enumRadioControl_ObsPhaseType.SetCurrent<ObsPhaseType>(Option.ObsPhaseType);
            this.satObsDataTypeControl1.CurrentdType = Option.ObsDataType; 
            this.satApproxDataTypeControl1.CurrentdType = Option.ApproxDataType;

            this.textBox_multiEpochCount.Text = Option.MultiEpochCount + "";
            this.textBox_MultiEpochSameSatCount.Text = Option.MutliEpochSameSatCount + "";
            this.checkBox_isRequireSameSats.Checked = Option.IsSameSatRequired;
            this.checkBox_isSmoothEpcohes.Checked = Option.IsSmoothMoveInMultiEpoches;

            this.enumRadioControl_BaseSiteSelectType.SetCurrent<BaseSiteSelectType>(this.Option.BaseSiteSelectType);

            this.checkBox_enableApriori.Checked = Option.IsEnableInitApriori;
            if (this.Option.InitApriori != null)
            {
                this.namedArrayControl_apriori.SetValue(this.Option.InitApriori.OneDimArray);
                if (this.Option.InitApriori.InverseWeight != null)
                {
                    this.namedArrayControl_rmsOfApriori.SetValue(this.Option.InitApriori.GetRmsVector().OneDimArray);
                }
            }
        }
         

        private void GnssOptionForm_Load(object sender, EventArgs e)
        {
            this.enumRadioControl_BaseSiteSelectType.Init<BaseSiteSelectType>();
        }
         
    }
}