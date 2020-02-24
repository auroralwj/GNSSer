//2017.08.11, czs, edit in hongqing, 单独提出
//2018.04.27, czs, edit in hmx, 移走手动设置星历部分

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
    public partial class DataSourceOptionPage : BaseGnssProcessOptionPage
    {
        public DataSourceOptionPage()
        {
            InitializeComponent();
            this.Name = "数据源"; 
        }


        public override void UiToEntity()
        {
            base.UiToEntity();
             
            Option.SatelliteTypes = this.multiGnssSystemSelectControl1.SatelliteTypes; 
            Option.IsPhaseValueRequired = this.checkBox_IsPhaseValueRequired.Checked;
            Option.IsRangeValueRequired = this.checkBox_IsRangeValueRequired.Checked; 
            Option.IsDopplerShiftRequired = this.checkBox_IsDopplerShiftRequired.Checked;


            Option.IsP2C2Enabled= this.checkBox_IsP2C2Enabled.Checked; 

            Option.IsLengthPhaseValue = this.checkBox1IsLengthPhaseValue.Checked;
            
            #region 数据源配置
            Option.IsPreciseClockFileRequired = this.checkBox_isClockFileRequired.Checked;
            Option.IsPreciseEphemerisFileRequired = this.checkBox_ispreciseEphemerisFileRequired.Checked;
            Option.IsEphemerisRequired = this.checkBox_isEphemerisFileRequired.Checked;
            Option.IsAntennaFileRequired = this.checkBox_IsAntennaFileRequired.Checked;
            Option.IsSatStateFileRequired = this.checkBox_IsSatStateFileRequired.Checked;
            Option.IsSatInfoFileRequired = this.checkBox_IsSatInfoFileRequired.Checked;
            Option.IsOceanLoadingFileRequired = this.checkBox_IsOceanLoadingFileRequired.Checked;
            Option.IsDCBFileRequired = this.checkBox_IsDCBFileRequired.Checked;
            Option.IsVMF1FileRequired = this.checkBox_IsVMF1FileRequired.Checked;
            Option.IsErpFileRequired = this.checkBox_IsErpFileRequired.Checked;
               Option.IsIgsIonoFileRequired = this.checkBox_igsIonoFile.Checked;
              Option.Isgpt2File1DegreeRequired = this.checkBox_Isgpt2File1DegreeRequired.Checked;
              Option.IsEnableNgaEphemerisSource = this.checkBox1IsEnableNgaEphemerisSource.Checked;
            #endregion
             
            Option.IsSiteCoordServiceRequired = checkBox_IsSiteCoordServiceRequired.Checked; 
            Option.IsStationInfoRequired = this.checkBox_stationInfo.Checked;   
        }
        public override void EntityToUi()
        {
            base.EntityToUi();
              
            this.multiGnssSystemSelectControl1.SetSatelliteTypes(Option.SatelliteTypes);
           
            this.checkBox_IsPhaseValueRequired.Checked = Option.IsPhaseValueRequired;
            this.checkBox_IsRangeValueRequired.Checked = Option.IsRangeValueRequired; 
            this.checkBox_IsDopplerShiftRequired.Checked = Option.IsDopplerShiftRequired; 
             
            this.checkBox_IsP2C2Enabled.Checked = Option.IsP2C2Enabled;  
            this.checkBox1IsLengthPhaseValue.Checked = Option.IsLengthPhaseValue;

            #region 数据源配置 

            this.checkBox_isClockFileRequired.Checked = Option.IsPreciseClockFileRequired;
            this.checkBox_ispreciseEphemerisFileRequired.Checked = Option.IsPreciseEphemerisFileRequired;
            this.checkBox_isEphemerisFileRequired.Checked = Option.IsEphemerisRequired;

            this.checkBox_IsAntennaFileRequired.Checked = Option.IsAntennaFileRequired;
            this.checkBox_IsSatStateFileRequired.Checked = Option.IsSatStateFileRequired;
            this.checkBox_IsSatInfoFileRequired.Checked = Option.IsSatInfoFileRequired;
            this.checkBox_IsOceanLoadingFileRequired.Checked = Option.IsOceanLoadingFileRequired;
            this.checkBox_IsDCBFileRequired.Checked = Option.IsDCBFileRequired;
            this.checkBox_IsVMF1FileRequired.Checked = Option.IsVMF1FileRequired;
            this.checkBox_IsErpFileRequired.Checked = Option.IsErpFileRequired;
            this.checkBox_igsIonoFile.Checked = Option.IsIgsIonoFileRequired;
             
            this.checkBox_Isgpt2File1DegreeRequired.Checked = Option.Isgpt2File1DegreeRequired;

            this.checkBox1IsEnableNgaEphemerisSource.Checked = Option.IsEnableNgaEphemerisSource;
            #endregion
              
             
            checkBox_IsSiteCoordServiceRequired.Checked = Option.IsSiteCoordServiceRequired; 
            this.checkBox_stationInfo.Checked = Option.IsStationInfoRequired; 
        }
                 

        private void GnssOptionForm_Load(object sender, EventArgs e)
        {
        } 

        private void button_commonDatasource_Click(object sender, EventArgs e) { new ComonSourceSettingForm().ShowDialog(); }
         
 
 
        private void checkBox_isClockFileRequired_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox_isNetEnabled_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}