//2014.12.04, czs, create in jinxingliaomao husangliao,  GNSS 系统选择

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
    /// <summary>
    /// GNSS系统选择器
    /// </summary>
    public partial class GnssSystemSelectControl : UserControl
    {
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public GnssSystemSelectControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 卫星类型改变了。
        /// </summary>
        public event SatelliteTypeChangedEventHandler SatelliteTypeChanged;

        /// <summary>
        /// GNSS 系统类型。
        /// </summary>
        public GnssType GnssType
        {
            get
            {
                return Gnsser.GnssSystem.GetGnssType(SatelliteType);
            }
        }

        /// <summary>
        /// GNSS 系统。
        /// </summary>
        public GnssSystem GnssSystem
        {
            get
            {
                return Gnsser.GnssSystem.GetGnssSystem(SatelliteType);
            }
        }

        /// <summary>
        /// 卫星类型
        /// </summary>
        public SatelliteType SatelliteType
        {
            get
            {
                if (radioButton_beidou.Checked) return (SatelliteType.C);
                if (this.radioButton_galileo.Checked) return (SatelliteType.E);
                if (this.radioButton_gps.Checked) return (SatelliteType.G);
                if (this.radioButton_glonass.Checked) return (SatelliteType.R);
                return SatelliteType.C;
            }
        }
        public void SetSatelliteType( SatelliteType satType)
        { 
            switch(satType){
                case Gnsser.SatelliteType.C:(radioButton_beidou.Checked) = true;break;
                case Gnsser.SatelliteType.G:(radioButton_gps.Checked) = true;break;
                case Gnsser.SatelliteType.E:(radioButton_galileo.Checked) = true;break;
                case Gnsser.SatelliteType.R:(radioButton_glonass.Checked) = true;break; 
                default:
                    (radioButton_beidou.Checked) = true;break;
            } 
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (SatelliteTypeChanged != null) { SatelliteTypeChanged(SatelliteType);}
        }
    }
}
