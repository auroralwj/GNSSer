//2017.08.15, czs, edit in hongqing, 单独提出
//2018.12.26, czs, edit in ryd, 增加去除无法组成无电离层组合数据， 观测数据掐头去尾

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
    public partial class PreprocessOptionPage : BaseGnssProcessOptionPage
    {
        public PreprocessOptionPage()
        {
            InitializeComponent();

            this.Name = "预处理"; 
        }


        public override void UiToEntity()
        {
            base.UiToEntity();
            #region 预处理  
            Option.MaxBreakingEpochCount = this.namedFloatControlMaxBreakingEpochCount.Value;           
            Option.MinContinuouObsCount = this.namedIntControl_MinContinuouObsCount.Value;
            //缓存探测 
             
            Option.MinSatCount = int.Parse(this.textBox_minSatCount.Text);
            //Option.DataSourceOption.IsDownloadingSurplurseIgsProducts = this.checkBox1DownloadingSurplusIgsProduct.Checked;
            Option.MaxEpochSpan = int.Parse(this.textBox_epochMaxAllowedGap.Text);
            Option.MinFrequenceCount = int.Parse(this.textBox_MinFrequenceCount.Text);
            Option.IsAliningPhaseWithRange = this.checkBox_IsAliningPhaseWithRange.Checked;
            Option.MinAllowedRange = Double.Parse(this.textBox_minAllowedRange.Text);
            Option.MaxAllowedRange = Double.Parse(this.textBox_maxAllowedRange.Text);
            Option.IsAllowMissingEpochSite = this.checkBox_IsAllowMissingEpochSite.Checked;
            Option.IsRemoveSmallPartSat = checkBox_IsRemoveSmallPartSat.Checked;
            Option.IsRemoveIonoFreeUnavaliable = checkBox_IsRemoveIonoFreeUnavaliable.Checked;
            Option.IsBreakOffBothEnds = checkBox_IsBreakOffBothEnds.Checked;
            Option.MinuteOfBreakOffBothEnds = this.namedFloatControl_breakEndsMinute.GetValue();


            Option.IsEnableSatAppearenceService = checkBox_IsEnableSatAppearenceService.Checked;
            #endregion

        }
        public override void EntityToUi()
        {
            base.UiToEntity();
            #region 预处理 
            checkBox_IsRemoveSmallPartSat.Checked = Option.IsRemoveSmallPartSat;
            this.namedFloatControlMaxBreakingEpochCount.Value = Option.MaxBreakingEpochCount; 
            this.namedIntControl_MinContinuouObsCount.Value = Option.MinContinuouObsCount;
             
            this.textBox_minSatCount.Text = Option.MinSatCount + "";
            //this.checkBox1DownloadingSurplusIgsProduct.Checked = Option.DataSourceOption.IsDownloadingSurplurseIgsProducts;
            this.textBox_epochMaxAllowedGap.Text = Option.MaxEpochSpan.ToString();
            this.textBox_MinFrequenceCount.Text = Option.MinFrequenceCount + "";
            this.checkBox_IsAliningPhaseWithRange.Checked = Option.IsAliningPhaseWithRange;
            this.textBox_minAllowedRange.Text = Option.MinAllowedRange + "";
            this.textBox_maxAllowedRange.Text = Option.MaxAllowedRange + "";
            this.checkBox_IsAllowMissingEpochSite.Checked = Option.IsAllowMissingEpochSite;
            checkBox_IsRemoveIonoFreeUnavaliable.Checked = Option.IsRemoveIonoFreeUnavaliable;

            checkBox_IsBreakOffBothEnds.Checked = Option.IsBreakOffBothEnds;
            this.namedFloatControl_breakEndsMinute.SetValue(Option.MinuteOfBreakOffBothEnds);
            #endregion
            checkBox_IsEnableSatAppearenceService.Checked = Option.IsEnableSatAppearenceService;

        }
          
        private void GnssOptionForm_Load(object sender, EventArgs e)
        { 
        } 

    }
}