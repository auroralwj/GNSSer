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
using Geo;
using Geo.Winform.Wizards; 

namespace Gnsser.Winform
{
    public partial class SatelliteOptionPage : BaseGnssProcessOptionPage
    {
        public SatelliteOptionPage( )
        {
            InitializeComponent();

            this.Name = "卫星"; 
        }


        public override void UiToEntity()
        {
            base.UiToEntity();
            var val = enabledStringControl_RemoveSats.GetEnabledValue();
            Option.IsEnableRemoveSats = val.Enabled;
            Option.SatsToBeRemoved = SatelliteNumberUtils.ParseString(val.Value);

            Option.IndicatedPrn = SatelliteNumber.Parse( this.enabledStringControl_IndicatedPrn.GetEnabledValue().Value);
            Option.IsIndicatedPrn = this.enabledStringControl_IndicatedPrn.GetEnabledValue().Enabled;
            Option.MinSuccesiveEphemerisCount = int.Parse(this.textBox_MinSuccesiveEphemerisCount.Text);
            Option.IsSwitchWhenEphemerisNull = this.checkBox_IsSwitchWhenEphemerisNull.Checked;   
            Option.VertAngleCut = double.Parse(this.textBox_angleCut.Text); 
            Option.IsEphemerisRequired = this.checkBox_IsEphemerisRequired.Checked; 
            Option.IsRemoveOrDisableNotPassedSat = this.checkBox_IsRemoveOrDisableNotPassedSat.Checked;
            Option.IsDisableEclipsedSat = this.checkBox_IsDisableEclipsedSat.Checked;
            Option.EphemerisInterpolationOrder = this.namedIntControl_ephInterOrder.GetValue();
            Option.IsExcludeMalfunctioningSat = this.checkBox_IsExcludeMalfunctioningSat.Checked;

            Option.IsConnectIgsDailyProduct = this.checkBox_IsConnectIgsDailyProduct.Checked;

        }
        public override void EntityToUi()
        {
            base.UiToEntity();

            var prnStr = SatelliteNumberUtils.GetString(Option.SatsToBeRemoved);
            enabledStringControl_RemoveSats.SetEnabledValue(new EnableString() { Enabled = Option.IsEnableRemoveSats, Value = prnStr });
       
            this.enabledStringControl_IndicatedPrn.SetEnabledValue(new Geo.EnableString() { Enabled = Option.IsIndicatedPrn, Value = Option.IndicatedPrn.ToString() });
            this.textBox_MinSuccesiveEphemerisCount.Text  = Option.MinSuccesiveEphemerisCount + "";
            this.checkBox_IsSwitchWhenEphemerisNull.Checked =Option.IsSwitchWhenEphemerisNull;
 
            this.checkBox_IsDisableEclipsedSat.Checked = Option.IsDisableEclipsedSat;
           
            this.textBox_angleCut.Text = Option.VertAngleCut + "";
            this.checkBox_IsEphemerisRequired.Checked = Option.IsEphemerisRequired;
           
            this.checkBox_IsRemoveOrDisableNotPassedSat.Checked = Option.IsRemoveOrDisableNotPassedSat;
            this.namedIntControl_ephInterOrder.SetValue(Option.EphemerisInterpolationOrder);
            this.checkBox_IsExcludeMalfunctioningSat.Checked = Option.IsExcludeMalfunctioningSat;

            this.checkBox_IsConnectIgsDailyProduct.Checked = Option.IsConnectIgsDailyProduct;


        }
         
        private void GnssOptionForm_Load(object sender, EventArgs e)
        {

        }
         

    }
}