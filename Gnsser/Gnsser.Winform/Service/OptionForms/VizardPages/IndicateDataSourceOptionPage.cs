//2018.04.27, czs, create in hmx, 从数据源单独提出，指定路径的多系统星历服务

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
    public partial class IndicateDataSourceOptionPage : BaseGnssProcessOptionPage
    {
        public IndicateDataSourceOptionPage()
        {
            InitializeComponent();
            this.Name = "手动数据源";
        }


        public override void UiToEntity()
        {
            base.UiToEntity();

            Option.IsUseUniqueEphemerisFile = this.checkBox_IsUseUniqueEphemerisFile.Checked;
            Option.IsIndicatingEphemerisFile = this.checkBox_setEphemerisFile.Checked;
            Option.IsIndicatingBdsEphemerisFile = this.checkBox_bdsEph.Checked;
            Option.IsIndicatingGloEphemerisFile = this.checkBox_gloEph.Checked;
            Option.IsIndicatingGalEphemerisFile = this.checkBox_galEph.Checked;

            Option.IsIndicatingClockFile = this.checkBox_enableClockFile.Checked;

            Option.EphemerisFilePath = this.fileOpenControl_eph.FilePath;
            Option.BdsEphemerisFilePath = this.fileOpenControl_bdsEph.FilePath;
            Option.GloEphemerisFilePath = this.fileOpenControl_gloEph.FilePath;
            Option.GalEphemerisFilePath = this.fileOpenControl_galEph.FilePath;

            Option.ClockFilePath = this.fileOpenControl_clk.FilePath;

            Option.IsGnsserFcbOfDcbRequired = this.checkBox_IsGnsserFcbOfDcbRequired.Checked;
            Option.GnsserFcbFilePath = this.fileOpenControl_fcbOfDcb.FilePath;

            #region 数据源配置
            #endregion

            #region  改正数
            Option.IsGnsserEpochIonoFileRequired = checkBox1IsGnsserEpochIonoFileRequired.Checked;

            Option.IsNavIonoModelCorrectionRequired = this.checkBox_ionoParamCorrection.Checked;
            #endregion
            Option.CoordFilePath = this.fileOpenControl_coordPath.FilePath;
            Option.TropAugmentFilePath = this.fileOpenControl_TropAugmentFilePath.FilePath;

            Option.IsIndicatingCoordFile = checkBox_indicateCoordfile.Checked;

            Option.NavIonoModelPath = this.fileOpenControl_navIonoModel.FilePath;
            Option.StationInfoPath = this.fileOpenControl_stainfo.FilePath;


            Option.GnsserEpochIonoFilePath = this.fileOpenControl1GnsserEpochIonoParamFilePath.FilePath;
            Option.IonoGridFilePath = this.fileOpenControl_ion.FilePath;
            Option.IsIndicatingGridIonoFile = this.checkBoxIsIndicateGridIono.Checked;

            Option.IsIndicatingStationInfoFile= this.checkBox_indicateStainfo.Checked;


        }
        public override void EntityToUi()
        {
            base.EntityToUi();

            this.checkBox_IsUseUniqueEphemerisFile.Checked = Option.IsUseUniqueEphemerisFile;
            this.checkBox_IsTropAugmentEnabled.Checked = Option.IsTropAugmentEnabled;
            checkBox1IsGnsserEpochIonoFileRequired.Checked = Option.IsGnsserEpochIonoFileRequired;

            this.checkBox_setEphemerisFile.Checked = Option.IsIndicatingEphemerisFile;
            this.checkBox_bdsEph.Checked = Option.IsIndicatingBdsEphemerisFile;
            this.checkBox_gloEph.Checked = Option.IsIndicatingGloEphemerisFile;
            this.checkBox_galEph.Checked = Option.IsIndicatingGalEphemerisFile;

            this.checkBox_enableClockFile.Checked = Option.IsIndicatingClockFile;

            this.fileOpenControl_eph.FilePath = Option.EphemerisFilePath;
            this.fileOpenControl_bdsEph.FilePath = Option.BdsEphemerisFilePath;
            this.fileOpenControl_gloEph.FilePath = Option.GloEphemerisFilePath;
            this.fileOpenControl_galEph.FilePath = Option.GalEphemerisFilePath;

            this.fileOpenControl_clk.FilePath = Option.ClockFilePath;

            this.fileOpenControl_navIonoModel.FilePath = Option.NavIonoModelPath;
            this.fileOpenControl_ion.FilePath = Option.IonoGridFilePath;

            this.checkBox_IsGnsserFcbOfDcbRequired.Checked = Option.IsGnsserFcbOfDcbRequired;
            this.fileOpenControl_fcbOfDcb.FilePath = Option.GnsserFcbFilePath;

            #region 数据源配置

            this.checkBoxIsIndicateGridIono.Checked = Option.IsIndicatingGridIonoFile;
            #endregion

            #region  改正数


            this.checkBox_ionoParamCorrection.Checked = Option.IsNavIonoModelCorrectionRequired;

            #endregion

            this.fileOpenControl_TropAugmentFilePath.FilePath = Option.TropAugmentFilePath;

            this.fileOpenControl_coordPath.FilePath = Option.CoordFilePath;
            checkBox_indicateCoordfile.Checked = Option.IsIndicatingCoordFile;
            this.fileOpenControl1GnsserEpochIonoParamFilePath.FilePath = Option.GnsserEpochIonoFilePath;
            this.fileOpenControl_stainfo.FilePath = Option.StationInfoPath;
            this.checkBox_indicateStainfo.Checked = Option.IsIndicatingStationInfoFile;
        }


        private void GnssOptionForm_Load(object sender, EventArgs e)
        {
            fileOpenControl_clk.Filter = Gnsser.Setting.RinexClkFileFilter;


            if(!Setting.IsMultiSystemEnabled)
            {
                this.groupBox_ionoSource.Visible = false;
                this.groupBox_otherSource.Visible = false;
                EnableOtherEphePathes(false);
            }

        }

        private void checkBox_IsUseUniqueEphemerisFile_CheckedChanged(object sender, EventArgs e)
        {
            var enalbeOthers = !checkBox_IsUseUniqueEphemerisFile.Checked;

            EnableOtherEphePathes(enalbeOthers);
        }

        private void EnableOtherEphePathes(bool enalbeOthers)
        {
            this.fileOpenControl_bdsEph.Enabled = enalbeOthers;
            this.fileOpenControl_gloEph.Enabled = enalbeOthers;
            this.fileOpenControl_galEph.Enabled = enalbeOthers;
            checkBox_galEph.Enabled = enalbeOthers;
            checkBox_gloEph.Enabled = enalbeOthers;
            checkBox_bdsEph.Enabled = enalbeOthers;
            checkBox_setEphemerisFile.Enabled = enalbeOthers;
        }
    }
}