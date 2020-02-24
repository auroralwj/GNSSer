//2018.07.29, czs, edit in HMX, BaseLineSelectionType 基线
//2019.01.02, czs, edit in hmx, BaseSiteSelectType 测站选择方法

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
    public partial class BaseLineOptionPage : BaseGnssProcessOptionPage
    {
        public BaseLineOptionPage()
        {
            InitializeComponent();

            this.Name = "基线";
        }
 

        public override void UiToEntity()
        {
            base.UiToEntity();
            Option.IsBaseSiteRequried = this.checkBox_IsBaseSiteRequried.Checked;
            Option.IndicatedBaseSiteName = this.namedStringControl_BaseSiteName.GetValue();
            Option.IndicatedPrn = SatelliteNumber.Parse(this.enabledStringControl_IndicatedPrn.GetEnabledValue().Value);
            Option.IsIndicatedPrn = this.enabledStringControl_IndicatedPrn.GetEnabledValue().Enabled;
            Option.BaseLineSelectionType = this.enumRadioControl_BaseLineSelectionType.GetCurrent<BaseLineSelectionType>();
            Option.BaseSatSelectionType = this.enumRadioControl_baseSatSelectionType.GetCurrent<Gnsser.BaseSatSelectionType>(); 
            Option.MaxDistanceOfShortBaseLine = this.namedFloatControl_maxShotBaseLine.Value;
            Option.MinDistanceOfLongBaseLine = this.namedFloatControl_MinDistanceOfLongBaseLine.Value;

            Option.BaseLineFilePath = fileOpenControl_baselineFile.FilePath;
            Option.CenterSiteName = namedStringControl_ceterSiteName.GetValue();

            this.Option.BaseSiteSelectType = this.enumRadioControl_BaseSiteSelectType.GetCurrent<BaseSiteSelectType>();

            //手动输入
            double levelFixed = this.namedFloatControl_fixedErrorLevel.GetValue();
            double verticalFixed = this.namedFloatControl_fixedErrorVertical.GetValue();
            double levelCoeef = this.namedFloatControl_levelCoefOfProprotion.GetValue();
            double verticalCoeef = this.namedFloatControl_verticalCoefOfProprotion.GetValue();
            Option.GnssReveiverNominalAccuracy = new GnssReveiverNominalAccuracy(levelFixed, verticalFixed, levelCoeef, verticalCoeef); 
        }

        public override void EntityToUi()
        {
            base.UiToEntity();
            this.checkBox_IsBaseSiteRequried.Checked = Option.IsBaseSiteRequried;
            this.namedStringControl_BaseSiteName.SetValue(Option.IndicatedBaseSiteName);
            this.enabledStringControl_IndicatedPrn.SetEnabledValue(new Geo.EnableString() { Enabled = Option.IsIndicatedPrn, Value = Option.IndicatedPrn.ToString() });
            this.enumRadioControl_BaseLineSelectionType.SetCurrent<BaseLineSelectionType>(Option.BaseLineSelectionType);
            this.enumRadioControl_baseSatSelectionType.SetCurrent<Gnsser.BaseSatSelectionType>((Gnsser.BaseSatSelectionType)Option.BaseSatSelectionType);
            this.namedFloatControl_maxShotBaseLine.Value = Option.MaxDistanceOfShortBaseLine;
            this.namedFloatControl_MinDistanceOfLongBaseLine.Value = Option.MinDistanceOfLongBaseLine;
            this.enumRadioControl_BaseSiteSelectType.SetCurrent<BaseSiteSelectType>(this.Option.BaseSiteSelectType);

            namedStringControl_ceterSiteName.SetValue(Option.CenterSiteName);

            fileOpenControl_baselineFile.FilePath = Option.BaseLineFilePath;

            this.namedFloatControl_fixedErrorLevel.SetValue(Option.GnssReveiverNominalAccuracy.FixedValue.Level);
            this.namedFloatControl_fixedErrorVertical.SetValue(Option.GnssReveiverNominalAccuracy.FixedValue.Vertical);
            this.namedFloatControl_levelCoefOfProprotion.SetValue(Option.GnssReveiverNominalAccuracy.CoefOfProportion.Level);
            this.namedFloatControl_verticalCoefOfProprotion.SetValue(Option.GnssReveiverNominalAccuracy.CoefOfProportion.Vertical);

        }

        private void GnssOptionForm_Load(object sender, EventArgs e)
        {
            this.enumRadioControl_BaseLineSelectionType.Init<BaseLineSelectionType>();
            this.enumRadioControl_baseSatSelectionType.Init<Gnsser.BaseSatSelectionType>();
            this.enumRadioControl_BaseSiteSelectType.Init<BaseSiteSelectType>();
        }

        private void TabPage_receiver_Click(object sender, EventArgs e)
        {

        }
    }
}