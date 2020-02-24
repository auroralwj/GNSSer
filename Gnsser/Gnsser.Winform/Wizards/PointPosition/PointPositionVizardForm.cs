//2017.07.24, czs, create in hongqing, 向导式
//2018.11.12, czs, edit in hmx, 增加动态坐标更功能


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
using Geo.IO;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser;


namespace Gnsser.Winform
{
    /// <summary>
    /// 向导式
    /// </summary>
    public class PointPositionVizardForm : WizardForm
    {
        Log log = new Log(typeof(PointPositionVizardForm));
        public PointPositionVizardForm()
        {
            InitializeComponent();
            FinalStepText = "执行";
            SelectFilePageControl = new Winform.SelectRinexFileWizardPage();

            SiteInfoSetingPage = new Winform.SiteInfoSetingPage();

            SelectPointPositionTypePage = new EnumRadioWizardPage();
            SelectPointPositionTypePage.Init<SingleSiteGnssSolverType>("选择定位模型");
            SelectPointPositionTypePage.SetCurrent<SingleSiteGnssSolverType>(SingleSiteGnssSolverType.无电离层组合PPP, (VersionType.Public != Setting.VersionType) );

            SelectAdjustmentPage = new EnumRadioWizardPage();
            SelectAdjustmentPage.Init<AdjustmentType>("选择平差器");
            SelectAdjustmentPage.SetCurrent<AdjustmentType>(AdjustmentType.卡尔曼滤波);

            ProgressBarWizardPage = new Geo.Winform.Wizards.ProgressBarWizardPage();
            //ProgressBarWizardPage.InitDetect("执行进度", 1);

            var WizardPages = new WizardPageCollection();
            WizardPages.Add(1, SelectFilePageControl);
            WizardPages.Add(2, SiteInfoSetingPage);
            WizardPages.Add(3, SelectPointPositionTypePage);
            WizardPages.Add(4, SelectAdjustmentPage);
            WizardPages.Add(5, ProgressBarWizardPage);  


            this.Init(WizardPages);
        }

        public GnssProcessOption Option { get; set; }

        EnumRadioWizardPage SelectAdjustmentPage;
        EnumRadioWizardPage SelectPointPositionTypePage;
        private BackgroundWorker backgroundWorker1;
        SelectRinexFileWizardPage SelectFilePageControl;
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

            ProgressBarWizardPage.Init(this.SelectFilePageControl.FilePathes.Length);
            nFilePath = SelectFilePageControl.NFilePath;

            var singlePpType = SelectPointPositionTypePage.GetCurrent<SingleSiteGnssSolverType>();
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
            ProgressBarWizardPage.Init(this.SelectFilePageControl.FilePathes.Length);
            backgroundWorker1.RunWorkerAsync();
        }

        string nFilePath = null;
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Geo.Utils.FormUtil.ShowWaittingForm("正在处理，请耐心等待！");

            var filePathes = SelectFilePageControl.FilePathes;
           
            PointPositionBackGroundRunner runner = new PointPositionBackGroundRunner(this.Option, filePathes);
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
            this.Text = "单点定位向导";
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
