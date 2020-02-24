using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gnsser.Winform.Controls
{
    public partial class BaseSatSelectingControl : UserControl
    {
        public BaseSatSelectingControl()
        {
            InitializeComponent();
            gnssSystemSelectControl1_SatelliteTypeChanged(SatelliteType);
            SetEnable();
        }

        /// <summary>
        /// 基准卫星编号
        /// </summary>
        public SatelliteNumber SelectedPrn { get { return (SatelliteNumber)(this.bindingSource_prns.Current); } }
        /// <summary>
        /// 卫星系统类型
        /// </summary>
        public SatelliteType SatelliteType { get { return gnssSystemSelectControl1.SatelliteType; } }
        public void SetSatelliteType(SatelliteType satType) {   gnssSystemSelectControl1.SetSatelliteType(satType); }
        /// <summary> 
        /// 是否启用基准卫星
        /// </summary>
        public bool EnableBaseSat
        {
            get { return comboBox_basePrn.Visible;  }
            set { comboBox_basePrn.Visible = value; label1.Visible = value; }
        }

        private void BaseSatSelectingControl_Load(object sender, EventArgs e)
        {
      
        }

        private void gnssSystemSelectControl1_SatelliteTypeChanged(SatelliteType SatelliteType)
        {
            this.bindingSource_prns.DataSource = SatelliteNumber.GetPrns(SatelliteType, 35);
        }

        private void checkBoxIsAssignBasePrn_CheckedChanged(object sender, EventArgs e)
        {
            SetEnable();
        }

        private void SetEnable()
        {
            this.EnableBaseSat = checkBoxIsAssignBasePrn.Checked;
        }
    }
}
