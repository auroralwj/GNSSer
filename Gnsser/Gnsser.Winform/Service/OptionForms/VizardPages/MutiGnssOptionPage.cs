//2018.09.26, czs, create in HMX, 多频多系统

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
    public partial class MutiGnssOptionPage : BaseGnssProcessOptionPage
    {
        public MutiGnssOptionPage()
        {
            InitializeComponent();

            this.Name = "多频多系统";
        }



        public override void UiToEntity()
        {
            base.UiToEntity(); 
            Option.IsSameTimeSystemInMultiGnss = this.checkBox_isSameTimeSystem.Checked;
            this.Option.SatelliteStdDevsString = this.namedStringControl_satStdDev.GetValue();
            this.Option.SystemStdDevFactorString = this.namedStringControl_sysStdDev.GetValue();
            this.Option.SatelliteTypes = this.multiGnssSystemSelectControl1.SatelliteTypes;
        }

        public override void EntityToUi()
        {
            base.UiToEntity(); 
            this.checkBox_isSameTimeSystem.Checked = Option.IsSameTimeSystemInMultiGnss; 
            this.namedStringControl_satStdDev.SetValue( this.Option.SatelliteStdDevsString);
            this.namedStringControl_sysStdDev.SetValue(this.Option.SystemStdDevFactorString );
            this.multiGnssSystemSelectControl1.SetSatelliteTypes(this.Option.SatelliteTypes);
        }

        private void GnssOptionForm_Load(object sender, EventArgs e)
        {   
        }
         

    }
}