//2017.07.26, czs, create in hongqing, 选择文件页面控件
//2018.11.12, czs, edit in hmx, 增加动态选项

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms; 
using Geo.Winform.Wizards;

namespace Gnsser.Winform
{
    public partial class SiteInfoSetingPage : UserControl, IWizardPage
    {
        public SiteInfoSetingPage()
        {
            InitializeComponent();

            fileOpenControl_siteinfo.Filter = Setting.SiteInfoFilter;
            fileOpenControl_siteCoord.Filter = Setting.SinexFilter;
            this.Name = "测站信息";
        }

        public string SiteInfoPath { get { return fileOpenControl_siteinfo.FilePath; } }
        public string SiteCoordPath { get { return fileOpenControl_siteinfo.FilePath; } }
        public PositionType PositionType { get { return enumRadioControl_positionType.GetCurrent<PositionType>(); } }
  
        public bool IsUpdateSiteInfo { get { return this.checkBox_updateStationInfo.Checked; } }

        public bool IsUpdateApproxCoord { get { return this.checkBox_IsUpdateSiteCoord.Checked; } }
        public bool IsUpdateEpochSiteCoord { get { return this.checkBox_updateEpochSiteCoord.Checked; } }

        public UserControl Content
        {
            get { return this; }
        }

        public new void LoadPage()
        {
            //  throw new NotImplementedException();
        }

        public void Save()
        {
            //throw new NotImplementedException();
        }

        public void Cancel()
        {
            //throw new NotImplementedException();
        }

        public bool IsBusy
        {
            get { return false; }
        }

        public bool PageValid
        {
            get { return true; }
        }

        public string ValidationMessage
        {
            get { throw new NotImplementedException(); }
        }
         

        private void SelectFilePageControl_Load(object sender, EventArgs e)
        {
            enumRadioControl_positionType.Init<PositionType>();
            fileOpenControl_siteinfo.FilePath = Setting.GnsserConfig.StationInfoPath ;
            fileOpenControl_siteCoord.FilePath = Setting.GnsserConfig.SiteCoordFile;
        }
    }
}
