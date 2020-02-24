//2017.08.11, czs, create in hongqing, 向导式


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.ComponentModel;
using System.Data;
using System.Drawing; 
using System.Windows.Forms; 
using Geo.Winform.Demo;
using Geo.Winform.Wizards;
using Geodesy.Winform;
using AnyInfo;
using Geo.Coordinates;
using Geo;
using Geo.Algorithm;
using Geo.IO;
using System.Net; 
using Gnsser.Service;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser;


namespace Gnsser.Winform
{
    /// <summary>
    /// 向导式
    /// </summary>
    public class OptionVizardForm : WizardForm
    {
        Log log = new Log(typeof(OptionVizardForm));
        public OptionVizardForm(GnssProcessOption Option)
        {
            InitializeComponent();
            this.Option = Option;
            FinalStepText = "完成";
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public GnssProcessOption Init(OptionVizardBuilder builder = null)
        {
            if (builder == null) builder = new OptionVizardBuilder();

            AccuracyEvaluationOptionPage = new Winform.AccuracyEvaluationOptionPage();
            IndicateDataSourceOptionPage = new Winform.IndicateDataSourceOptionPage();
            StateTransferModelOptionPage = new Winform.StateTransferModelOptionPage();
            DataSourceOptionPage = new Winform.DataSourceOptionPage();
            CorrectionOptionPage = new Winform.CorrectionOptionPage();
            BaseLineOptionPage = new Winform.BaseLineOptionPage();
            AmbiguityOptionPage = new Winform.AmbiguityOptionPage();
            StreamOptionPage = new Winform.StreamOptionPage();
            MutiGnssOptionPage = new Winform.MutiGnssOptionPage();
            CycleSlipOptionPage = new Winform.CycleSlipOptionPage();
            PreprocessOptionPage = new Winform.PreprocessOptionPage();
            ParamOptionPage = new Winform.ParamOptionPage();
            ResultCheckOptionPage = new Winform.ResultCheckOptionPage();
            GnssCaculatorOptionPage = new Winform.GnssCaculatorOptionPage();
            PsuedoRangeOptionPage = new Winform.PsuedoRangeSmoothOptionPage();
            ReceiverOptionPage = new Winform.SiteReceiverOptionPage();
            OutputOptionPage = new Winform.OutputOptionPage();
            SatelliteOptionPage = new SatelliteOptionPage();
            ObsAndApproxOptionPage = new ObsAndApproxOptionPage();
            this.WizardPages = new WizardPageCollection();
            if (Option == null)
            {
                Option = new GnssProcessOption();
            }

            int i = 1;
            if (VersionType.Development == Setting.VersionType)
            {
                if (builder.IsShowCorrectionOptionPage) WizardPages.Add(i++, CorrectionOptionPage);
            }
            if (VersionType.Development == Setting.VersionType)
            {
                if (builder.IsShowDataSourceOptionPage) WizardPages.Add(i++, DataSourceOptionPage);
            }
            //if(VersionType.Public !=Setting.VersionType) 
            if (builder.IsShowDataSourceOptionPage)
            {
                WizardPages.Add(i++, IndicateDataSourceOptionPage);
            }
            if (VersionType.Development == Setting.VersionType)
            {
                if (builder.IsShowGnssCaculatorOptionPage) WizardPages.Add(i++, GnssCaculatorOptionPage);
            }
            if (VersionType.Public != Setting.VersionType)
            {
                if (builder.IsShowObsAndApproxOptionPage) WizardPages.Add(i++, ObsAndApproxOptionPage);
            }
            if (VersionType.Development == Setting.VersionType)
            {
                if (builder.IsShowStateTransferModelOptionPage) WizardPages.Add(i++, StateTransferModelOptionPage);
            }
            if (VersionType.Development == Setting.VersionType)
            {
                if (builder.IsShowStreamOptionPage) WizardPages.Add(i++, StreamOptionPage);
            }

            if (VersionType.Public != Setting.VersionType)
            {
                if (builder.IsShowPreprocessOptionPage)
                    WizardPages.Add(i++, PreprocessOptionPage);
            }
           
            if (VersionType.Public != Setting.VersionType)
            {
                if (builder.IsShowParamOptionPage)
                    WizardPages.Add(i++, ParamOptionPage); 
            }
            if (VersionType.Development == Setting.VersionType || VersionType.BaselineNet == Setting.VersionType)
            {
                if (builder.IsShowCycleSlipOptionPage)
                    WizardPages.Add(i++, CycleSlipOptionPage);
            }
            if (builder.IsShowReceiverOptionPage)
            {
                WizardPages.Add(i++, SatelliteOptionPage);
            }
            if (VersionType.Development == Setting.VersionType || VersionType.BaselineNet == Setting.VersionType)
            {
                if (builder.IsShowReceiverOptionPage) WizardPages.Add(i++, ReceiverOptionPage);
            }
            if (VersionType.Development == Setting.VersionType || VersionType.BaselineNet == Setting.VersionType)
            {
                if (builder.IsShowBaseLineOptionPage)
                    WizardPages.Add(i++, BaseLineOptionPage);
            }
            if (VersionType.Development == Setting.VersionType || VersionType.BaselineNet == Setting.VersionType)
            {
                if (builder.IsShowAmbiguityOptionPage)
                    WizardPages.Add(i++, AmbiguityOptionPage);
            }
            if (VersionType.Development == Setting.VersionType)
            {
                if (builder.IsShowPsuedoRangeOptionPage) WizardPages.Add(i++, PsuedoRangeOptionPage);
            }
            if (VersionType.Development == Setting.VersionType)
            {
                if (builder.IsShowMutiGnssOptionPage)
                    WizardPages.Add(i++, MutiGnssOptionPage);
            }

            if (builder.IsShowResultCheckOptionPage)
                WizardPages.Add(i++, ResultCheckOptionPage);


            if (VersionType.Development == Setting.VersionType || VersionType.BaselineNet == Setting.VersionType)
            {
                if (builder.IsShowAccuracyEvaluationOptionPage)
                    WizardPages.Add(i++, AccuracyEvaluationOptionPage);
            }

            if (VersionType.Development == Setting.VersionType || VersionType.BaselineNet == Setting.VersionType)
            {
                if (builder.IsShowOutputOptionPage) WizardPages.Add(i++, OutputOptionPage);
            }
            this.Init(WizardPages);
            return Option;
        }

        public GnssProcessOption Option { get; set; }

        AccuracyEvaluationOptionPage AccuracyEvaluationOptionPage { get; set; }
        IndicateDataSourceOptionPage IndicateDataSourceOptionPage { get; set; }
        DataSourceOptionPage DataSourceOptionPage { get; set; }
        CorrectionOptionPage CorrectionOptionPage { get; set; }
        BaseLineOptionPage BaseLineOptionPage { get; set; }
        AmbiguityOptionPage AmbiguityOptionPage { get; set; }
        StreamOptionPage StreamOptionPage { get; set; }
        CycleSlipOptionPage CycleSlipOptionPage { get; set; }
        MutiGnssOptionPage MutiGnssOptionPage { get; set; }
        ResultCheckOptionPage ResultCheckOptionPage { get; set; }
        ParamOptionPage ParamOptionPage { get; set; }
        PreprocessOptionPage PreprocessOptionPage { get; set; }
        GnssCaculatorOptionPage GnssCaculatorOptionPage { get; set; }
        ObsAndApproxOptionPage ObsAndApproxOptionPage { get; set; }
        PsuedoRangeSmoothOptionPage PsuedoRangeOptionPage { get; set; }
        SiteReceiverOptionPage ReceiverOptionPage { get; set; }
        OutputOptionPage OutputOptionPage { get; set; }
        StateTransferModelOptionPage StateTransferModelOptionPage { get; set; }
        SatelliteOptionPage SatelliteOptionPage { get; set; }

        protected override void OnWizardPageLocationChanged(WizardPageLocationChangedEventArgs e)
        {
            if (e.PreviousPageIndex != -1)
            {
                var prePage = WizardPages[e.PreviousPageIndex] as BaseGnssProcessOptionPage;
                this.Option = prePage.Option;
            }

            var page = WizardPages[e.PageIndex] as BaseGnssProcessOptionPage;
            page.SetOption(this.Option);

            base.OnWizardPageLocationChanged(e);
        }

        protected override void OnWizardCompleted()
        {
            base.OnWizardCompleted();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void OptionVizardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                var prePage = this.WizardPages.CurrentPage as BaseGnssProcessOptionPage;
              
                prePage.Save();
                this.Option = prePage.Option;
            }
            catch (Exception ex)
            {
                Geo.Utils.FormUtil.ShowErrorMessageBox("界面输入不合法！" + ex.Message);
                e.Cancel = true;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // OptionVizardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Name = "OptionVizardForm";
            this.ShowFirstButton = true;
            this.ShowLastButton = true;
            this.Text = "GNSS数据处理选项向导";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionVizardForm_FormClosing);
            this.ResumeLayout(false);

        }
    }

    /// <summary>
    /// 构建器
    /// </summary>
    public class OptionVizardBuilder
    {
        /// <summary>
        /// 默认构造器
        /// </summary>
        public OptionVizardBuilder()
        {
            this.IsShowGnssCaculatorOptionPage = true;
            this.IsShowStateTransferModelOptionPage = true;
            this.IsShowDataSourceOptionPage = true;
            this.IsShowStreamOptionPage = true;
            this.IsShowCycleSlipOptionPage = true;
            this.IsShowPreprocessOptionPage = true;
            this.IsShowParamOptionPage = true;
            this.IsShowBaseLineOptionPage = true;
            this.IsShowAmbiguityOptionPage = true;
            this.IsShowPsuedoRangeOptionPage = true;
            this.IsShowReceiverOptionPage = true;
            this.IsShowOutputOptionPage = true;
            this.IsShowObsAndApproxOptionPage = true;
            this.IsShowResultCheckOptionPage = true;
            this.IsShowCorrectionOptionPage = true;
            this.IsShowMutiGnssOptionPage = true;
            this.IsShowAccuracyEvaluationOptionPage = true;
        }

        public bool IsShowAccuracyEvaluationOptionPage { get; set; }
        public bool IsShowGnssCaculatorOptionPage { get; set; }
        public bool IsShowStateTransferModelOptionPage { get; set; }
        public bool IsShowDataSourceOptionPage { get; set; }
        public bool IsShowCorrectionOptionPage { get; set; }
        public bool IsShowStreamOptionPage { get; set; }
        public bool IsShowCycleSlipOptionPage { get; set; }
        public bool IsShowPreprocessOptionPage { get; set; }
        public bool IsShowParamOptionPage { get; set; }
        public bool IsShowResultCheckOptionPage { get; set; }
        public bool IsShowPsuedoRangeOptionPage { get; set; }
        public bool IsShowReceiverOptionPage { get; set; }
        public bool IsShowOutputOptionPage { get; set; }
        public bool IsShowObsAndApproxOptionPage { get; set; }
        public bool IsShowBaseLineOptionPage { get; internal set; }
        public bool IsShowAmbiguityOptionPage { get; internal set; }
        public bool IsShowMutiGnssOptionPage { get; internal set; }

        public OptionVizardBuilder SetAll(bool trueOrFalse)
        {
            return SetIsShowDataSourceOptionPage(trueOrFalse)
                .SetIsShowGnssCaculatorOptionPage(trueOrFalse)
                .SetIsShowOutputOptionPage(trueOrFalse)
                .SetIsShowPsuedoRangeOptionPage(trueOrFalse)
                .SetIsShowReceiverOptionPage(trueOrFalse)
                .SetIsShowCycleSlipOptionPage(trueOrFalse)
                .SetIsShowPreprocessOptionPage(trueOrFalse)
                .SetIsShowStreamOptionPage(trueOrFalse)
                .SetIsShowMutiGnssOptionPage(trueOrFalse)
                .SetIsShowBaseLineOptionPage(trueOrFalse)
                .SetIsShowAccuracyEvaluationOptionPage(trueOrFalse)
                .SetIsShowAmbiguityOptionPage(trueOrFalse)
                .SetIsShowCorrectionOptionPage(trueOrFalse)
                .SetIsShowObsAndApproxOptionPage(trueOrFalse)
                .SetIsShowStateTransferModelOptionPage(trueOrFalse);
        }

        public OptionVizardBuilder SetIsShowMutiGnssOptionPage(bool trueOrFalse) { this.IsShowMutiGnssOptionPage = trueOrFalse; return this; }
        public OptionVizardBuilder SetIsShowPreprocessOptionPage(bool val) { this.IsShowPreprocessOptionPage = val; return this; }
        public OptionVizardBuilder SetIsShowGnssCaculatorOptionPage(bool val) { this.IsShowGnssCaculatorOptionPage = val; return this; }
        public OptionVizardBuilder SetIsShowStateTransferModelOptionPage(bool val) { this.IsShowStateTransferModelOptionPage = val; return this; }
        public OptionVizardBuilder SetIsShowDataSourceOptionPage(bool val) { this.IsShowDataSourceOptionPage = val; return this; }
        public OptionVizardBuilder SetIsShowStreamOptionPage(bool val) { this.IsShowStreamOptionPage = val; return this; }
        public OptionVizardBuilder SetIsShowAccuracyEvaluationOptionPage(bool val) { this.IsShowAccuracyEvaluationOptionPage = val; return this; }
        public OptionVizardBuilder SetIsShowBaseLineOptionPage(bool val) { this.IsShowBaseLineOptionPage = val; return this; }
        public OptionVizardBuilder SetIsShowAmbiguityOptionPage(bool val) { this.IsShowAmbiguityOptionPage = val; return this; }
        public OptionVizardBuilder SetIsShowCycleSlipOptionPage(bool val) { this.IsShowCycleSlipOptionPage = val; return this; }
        public OptionVizardBuilder SetIsShowPsuedoRangeOptionPage(bool val) { this.IsShowPsuedoRangeOptionPage = val; return this; }
        public OptionVizardBuilder SetIsShowReceiverOptionPage(bool val) { this.IsShowReceiverOptionPage = val; return this; }
        public OptionVizardBuilder SetIsShowOutputOptionPage(bool val) { this.IsShowOutputOptionPage = val; return this; }
        public OptionVizardBuilder SetIsShowCorrectionOptionPage(bool val) { this.IsShowCorrectionOptionPage = val; return this; }

        public OptionVizardBuilder SetIsShowObsAndApproxOptionPage(bool val) { this.IsShowObsAndApproxOptionPage = val; return this; }

    }
}
