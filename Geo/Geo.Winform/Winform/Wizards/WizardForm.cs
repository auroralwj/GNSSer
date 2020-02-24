//2015.12.06, czs, add in hongqing, 添加树形导航菜单
//2017.06.02, czs, edit in hongqing, 修改按钮名称，将委托提出到外部

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Geo.Winform.Wizards
{

    public delegate void WizardCompletedEventHandler();

    public partial class WizardForm : Form
    {
        private const string VALIDATION_MESSAGE = "当前页面尚未执行完成";

        #region Constructor & Window Event Handlers
        public WizardForm()
        {
            InitializeComponent();
            FinalStepText = "完成";
        }

        public WizardForm(WizardPageCollection WizardPages)
        {
            InitializeComponent();

            FinalStepText = "完成";
            Init(WizardPages);
        }

        protected void Init(WizardPageCollection WizardPages)
        {
            this.WizardPages = WizardPages;
            Init();
        }

        protected void Init()
        {
            this.WizardPages.WizardPageLocationChanged
                += new WizardPageLocationChangedEventHanlder(OnWizardPageLocationChanged);

            //init treeview
            this.treeView1.Nodes.Clear();
            var pages = WizardPages;
            foreach (var item in WizardPages)
            {
                TreeNode node = new TreeNode(item.Value.Content.Name);
                node.Tag = item;
                this.treeView1.Nodes.Add(node);
            }
            this.LoadWizard();
            //pages.ForEach(new Action<string>(delegate(string name)
            //{
            //    this.treeView1.Nodes.Add(name);
            //}));         
        }

        protected virtual void OnWizardPageLocationChanged(WizardPageLocationChangedEventArgs e)
        {
            LoadNextPageByNavButton(e.PageIndex, e.PreviousPageIndex, true);
        }

        #endregion

        #region Properties
        /// <summary>
        ///  最后一步的文字，一般为完成，执行等。
        /// </summary>
        public string FinalStepText { get; set; }

        protected WizardPageCollection WizardPages { get; set; }
        public bool ShowFirstButton
        {
            get { return btnFirst.Visible; }
            set { btnFirst.Visible = value; }
        }
        public bool ShowLastButton { get; set; }

        //public bool ShowLastButton
        //{
        //    get { return btnLast.Visible; }
        //    set { btnLast.Visible = value; }
        //}

        private bool navigationEnabled = true;
        public bool NavigationEnabled
        {
            get { return navigationEnabled; }
            set
            {
                btnFirst.Enabled = value;
                btnPrevious.Enabled = value;
                btnNext.Enabled = value;
                btnLast.Enabled = value;
                navigationEnabled = value;
            }
        }

        #endregion

        #region Delegates & Events

        public event WizardCompletedEventHandler WizardCompleted; 

        #endregion

        #region Private Methods

        private void NotifyWizardCompleted()
        {

            OnWizardCompleted();
        }
        protected virtual void OnWizardCompleted()
        {
            WizardPages.LastPage.Save();

            if (WizardCompleted != null)
            {
                WizardCompleted();
                WizardPages.Reset();
                this.DialogResult = DialogResult.OK;  
            }   
        }

        public void UpdateNavigationButton()
        {
            #region Reset

            btnNext.Enabled = true;
            btnNext.Visible = true;

            btnLast.Text = "末页 >";
            if (ShowLastButton)
            {
                btnLast.Enabled = true;
            }
            else
            {
                btnLast.Enabled = false;
            }

            #endregion

            bool canMoveNext = WizardPages.CanMoveNext;
            bool canMovePrevious = WizardPages.CanMovePrevious;

            btnPrevious.Enabled = canMovePrevious;
            btnFirst.Enabled = canMovePrevious;

            if (canMoveNext)
            {
                btnNext.Text = "下页 >";
                btnNext.Enabled = true;

                if (ShowLastButton)
                {
                    btnLast.Enabled = true;
                }
            }
            else
            {
                if (ShowLastButton)
                {
                    btnLast.Text = FinalStepText;
                    btnNext.Visible = false;
                }
                else
                {
                    btnNext.Text = FinalStepText;
                    btnNext.Visible = true;
                }
            }
        }

        private bool CheckPageIsValid()
        {
            if ( WizardPages.CurrentPage != null && !WizardPages.CurrentPage.PageValid)
            {
                MessageBox.Show(
                    string.Concat(VALIDATION_MESSAGE, Environment.NewLine, Environment.NewLine, WizardPages.CurrentPage.ValidationMessage),
                    "信息详情",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// 加载
        /// </summary>
        public void LoadWizard()
        {
            LoadNextPageByNavButton(1, -1, false);
        //    WizardPages.MoveTo(1);//.MovePageFirst();
        }
        /// <summary>
        /// 加载下一个
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="previousPageIndex"></param>
        /// <param name="savePreviousPage"></param>
        public void LoadNextPageByNavButton(int pageIndex, int previousPageIndex, bool savePreviousPage)
        {
            LoadNextPageByNavTree(pageIndex, previousPageIndex, savePreviousPage);
            //if (pageIndex != -1)
            //{
            //    contentPanel.Controls.Clear();
            //    contentPanel.Controls.Add(WizardPages[pageIndex].Content);
            //    if (savePreviousPage && previousPageIndex != -1)
            //    {
            //        WizardPages[previousPageIndex].Save();
            //    }
            //    WizardPages[pageIndex].Load();
               
            //}
            UpdateTreeNavigation();
        }
        /// <summary>
        /// 加载树形导航
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="previousPageIndex"></param>
        /// <param name="savePreviousPage"></param>
        public void LoadNextPageByNavTree(int pageIndex, int previousPageIndex, bool savePreviousPage)
        {
            if (pageIndex != -1)
            {
                contentPanel.Controls.Clear();
                WizardPages.MoveTo(pageIndex);
                var page = WizardPages.CurrentPage;

                contentPanel.Controls.Add(page.Content);
                page.Content.Dock = DockStyle.Fill;

                
                if (savePreviousPage && previousPageIndex != -1)
                {
                    WizardPages[previousPageIndex].Save();
                }
                page.LoadPage();
                UpdateNavigationButton();
            }
        }
        
        private void UpdateTreeNavigation()
        {
            foreach (TreeNode item in treeView1.Nodes)
            {
                if (item.Text == WizardPages.CurrentPage.Content.Name)
                {
                    if (treeView1.SelectedNode != item)
                    {
                        treeView1.SelectedNode = item;
                        treeView1.Focus();
                    }
                    break;
                }
            }
        }

        #endregion

        #region Event Handlers

        private void btnFirst_Click(object sender, EventArgs e)
        {
            //if (!CheckPageIsValid()) //Maybe doesn't matter if move back; only matters if move forward
            //{ return; }

            WizardPages.MovePageFirst();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            //if (!CheckPageIsValid()) //Maybe doesn't matter if move back; only matters if move forward
            //{ return; }

            WizardPages.MovePagePrevious();
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (!CheckPageIsValid())
            { return; }

            if (WizardPages.CanMoveNext)
            {
                WizardPages.MovePageNext();
            }
            else
            {
                //This is the finish button and it has been clicked
                NotifyWizardCompleted();
            }
        }
        private void btnLast_Click(object sender, EventArgs e)
        {
            if (!CheckPageIsValid())
            { return; }

            if (WizardPages.CanMoveNext)
            {
                WizardPages.MovePageLast();
            }
            else
            {
                //This is the finish button and it has been clicked
                NotifyWizardCompleted();
            }
        }

        #endregion

        private void StepForm_Load(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var val = (KeyValuePair<int, IWizardPage>)e.Node.Tag;
         //   if (currentVal.Key != WizardPages.CurrentPageIndex)
            {
                this.WizardPages.MoveTo(val.Key);
            //   LoadNextPageByNavTree(currentVal.Key, WizardPages.CurrentPageIndex, true);
            }
        }

        private void WizardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WizardPages.IsBusy)
            {
                if (Geo.Utils.FormUtil.ShowYesNoMessageBox("后台正忙，是否强制退出？") == System.Windows.Forms.DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}