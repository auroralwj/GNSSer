//2018.12.09, czs, create in hmx, 基线向导计算


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
    /// 基线向导计算
    /// </summary>
    public class BaselineSolverVizardForm : WizardForm
    {
        Log log = new Log(typeof(BaselineSolverVizardForm));
        public BaselineSolverVizardForm()
        {
            InitializeComponent();
            FinalStepText = "执行";

            SiteInfoSetingPage = new Winform.SiteInfoSetingPage();
            GroupRinexFileWizardPage = new Winform.GroupRinexFileWizardPage();

            SelectPointPositionTypePage = new EnumRadioWizardPage();
            SelectPointPositionTypePage.Init<TwoSiteSolverType>("选择定位模型");
            SelectPointPositionTypePage.SetCurrent<TwoSiteSolverType>(TwoSiteSolverType.单历元单频双差, (VersionType.Public != Setting.VersionType) );

            SelectAdjustmentPage = new EnumRadioWizardPage();
            SelectAdjustmentPage.Init<AdjustmentType>("选择平差器");
            SelectAdjustmentPage.SetCurrent<AdjustmentType>(AdjustmentType.卡尔曼滤波);

            ProgressBarWizardPage = new Geo.Winform.Wizards.ProgressBarWizardPage();
            //ProgressBarWizardPage.InitDetect("执行进度", 1);

            var WizardPages = new WizardPageCollection();
            int i = 1;
            WizardPages.Add(i++, GroupRinexFileWizardPage);
            WizardPages.Add(i++, SiteInfoSetingPage);
            WizardPages.Add(i++, SelectPointPositionTypePage);
            WizardPages.Add(i++, SelectAdjustmentPage);
            WizardPages.Add(i++, ProgressBarWizardPage);  


            this.Init(WizardPages);
        }

        public GnssProcessOption Option { get; set; }

        GroupRinexFileWizardPage GroupRinexFileWizardPage;
        EnumRadioWizardPage SelectAdjustmentPage;
        EnumRadioWizardPage SelectPointPositionTypePage;
        private BackgroundWorker backgroundWorker1;
        ProgressBarWizardPage ProgressBarWizardPage;
        SiteInfoSetingPage SiteInfoSetingPage;

        protected override void OnWizardPageLocationChanged(WizardPageLocationChangedEventArgs e)
        { 
            base.OnWizardPageLocationChanged(e);
        }

        protected override void OnWizardCompleted()
        {
            base.OnWizardCompleted();
            this.Enabled = false;

            var netFiles = GroupRinexFileWizardPage.Result;









            //ProgressBarWizardPage.Init(this.SelectFilePageControl.FilePathes.Length);
            //nFilePath = SelectFilePageControl.NFilePath;

            var singlePpType = SelectPointPositionTypePage.GetCurrent< TwoSiteSolverType >();
            GnssSolverType type = GnssSolverTypeHelper.GetGnssSolverType(singlePpType);
            if (Option == null)
            {
               Option = GnssProcessOptionManager.Instance[type];
            }
            Option.AdjustmentType = SelectAdjustmentPage.GetCurrent<AdjustmentType>();
            this.Option.IsUpdateStationInfo = SiteInfoSetingPage.IsUpdateSiteInfo;
            this.Option.IsStationInfoRequired = SiteInfoSetingPage.IsUpdateSiteInfo;
            this.Option.IsUpdateEstimatePostition = SiteInfoSetingPage.IsUpdateEpochSiteCoord;
            this.Option.PositionType = SiteInfoSetingPage.PositionType;

            //this.Option.isup
            //ProgressBarWizardPage.Init(this.SelectFilePageControl.FilePathes.Length);
            backgroundWorker1.RunWorkerAsync();
        }

        string nFilePath = null;
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Geo.Utils.FormUtil.ShowWaittingForm("正在处理，请耐心等待！");

            var filePathes = new string []{ };// SelectFilePageControl.FilePathes;

            TwoSiteBackGroundRunner runner = new TwoSiteBackGroundRunner(this.Option, filePathes);
            runner.ProgressViewer = ProgressBarWizardPage.ProgressBarComponent;
            runner.Init();

            runner.Run();

            this.Invoke(new Action(delegate()
            {
                this.Enabled = true;              
                if (Geo.Utils.FormUtil.ShowYesNoMessageBox("计算完毕,是否关闭窗口？") == System.Windows.Forms.DialogResult.Yes)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }              
            }));      
        }

        private void InitializeComponent()
        {
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // PointPositionVizardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(605, 423);
            this.Name = "PointPositionVizardForm";
            this.ShowFirstButton = true;
            this.ShowLastButton = true;
            this.Text = "基线向导";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SinexImportVizardForm_FormClosing);
            this.ResumeLayout(false);

        }

        private void SinexImportVizardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
           // if (backgroundWorker1.IsBusy && Geo.Utils.FormUtil.ShowYesNoMessageBox("请留步，后台没有运行完毕。是否一定要退出？") == DialogResult.No) { e.Cancel = true; }
           // else { IsClosed = true; }
        }

    }
}
