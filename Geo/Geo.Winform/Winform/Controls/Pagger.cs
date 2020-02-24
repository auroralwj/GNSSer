using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Geo.Winform.Controls
{
    /// <summary>
    /// Tab 面板。
    /// </summary>
    public partial class Pagger : System.Windows.Forms.ToolStrip
    {
        public event EventHandler PaggerChanged;

        public Pagger()
            : base()
        {
            //qing re pian , 5 喝 。 遥遥。
            InitializeComponent();
            //this.LayoutStyle = ToolStripLayoutStyle.Flow;
            this.GripStyle = ToolStripGripStyle.Hidden;

            this.PageLimit = 30;
            this.CurrentPage = 1;

        }

        public Pagger(BindingSource bindingSource)
        {
            InitializeComponent();
            this.BindingSource = bindingSource;
            this.PageLimit = 30;
            this.CurrentPage = 1;
        }
        private int lastPageLimit = 1;
        /// <summary>
        /// 这两个改变，则重新获取数据。
        /// </summary>
        public int PageLimit
        {
            get { return lastPageLimit; }
            set
            {
                if (lastPageLimit != value)
                {
                    this.toolStripComboBox_pagelimit.SelectedItem = value;
                    lastPageLimit = value;

                    //设置
                    this.toolStripLabel_pageCount.Text = TotalPageCount.ToString();

                    if (this.CurrentPage > TotalPageCount)
                    {
                        this.CurrentPage = this.CurrentPage;
                    }
                    else
                    {
                        if (PaggerChanged != null) PaggerChanged(this, new EventArgs());
                    }
                }
            }
        }


        /// <summary>
        /// 所有页数量，由条目总数和页条目限制决定。
        /// </summary>
        public int TotalPageCount
        {
            get { var totalPage = (int)(Math.Ceiling(AllRowCount * 1.0 / PageLimit)); return totalPage < 1 ? 1 : totalPage; }
        }

        private int lastCurrentPage = 1;
        /// <summary>
        /// 改变，则重新获取数据。
        /// </summary>
        public int CurrentPage
        {
            get
            { 
                if (lastCurrentPage > TotalPageCount)
                {
                    lastCurrentPage = TotalPageCount;
                    this.toolStripComboBox_page.SelectedItem = lastCurrentPage;
                }
                return lastCurrentPage;
            }
            set
            {
                //当前页的范围
                if (value > TotalPageCount) value = TotalPageCount;
                if (value < 1) value = 1;
                //检测当前页是否改变
                if (lastCurrentPage != value)
                {
                    lastCurrentPage = value;

                    //SetButtonsEnabled();

                    if (PaggerChanged != null) PaggerChanged(this, new EventArgs());
                }
                //同步UI。
                this.toolStripComboBox_page.SelectedItem = value;
            }
        }

        private int allRowCount = 0;
        /// <summary>
        /// 所有数量
        /// </summary>
        public int AllRowCount
        {
            get { return allRowCount; }
            set
            {
                allRowCount = value;
                this.toolStripLabel_pageCount.Text = TotalPageCount.ToString();
                this.toolStripLabel_totalRowCount.Text = value.ToString();
                //重新设置页数
                this.toolStripComboBox_page.BeginUpdate();
                this.toolStripComboBox_page.Items.Clear();
                for (int i = 1; i <= TotalPageCount; i++)
                {
                    this.toolStripComboBox_page.Items.Add(i);
                }
                this.toolStripComboBox_page.EndUpdate();
                //同步UI
                this.toolStripComboBox_page.Text = CurrentPage + "";
            }
        }

        #region 属性

        private BindingSource bindingSource;
        /// <summary>
        /// 数据源
        /// </summary>
        public BindingSource BindingSource
        {
            get { return bindingSource; }
            set
            {
                bindingSource = value;
                if (value != null)
                    this.CurrentPage = this.CurrentPage;
            }
        }
        #endregion

        private void Pagger_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;

            if (e.ClickedItem is ToolStripButton)
            {
                if (e.ClickedItem.Name == this.toolStripButton_first.Name)
                {
                    First();
                }
                if (e.ClickedItem.Name == this.toolStripButton_prev.Name)
                {
                    Prev();
                }
                if (e.ClickedItem.Name == this.toolStripButtonNext.Name)
                {
                    Next();
                }
                if (e.ClickedItem.Name == this.toolStripButton_last.Name)
                {
                    Last();
                }
            }
            // 由 toolStripComboBox_page_SelectedIndexChanged 处理
            if (e.ClickedItem is ToolStripComboBox)
            {
                if (e.ClickedItem.Name == toolStripComboBox_pagelimit.Name)
                {
                    //this.CurrentPage = int.Parse(this.toolStripComboBox_page.SelectedItem.ToString());

                }
                if (e.ClickedItem.Name == toolStripComboBox_page.Name)
                {
                    //this.CurrentPage = int.Parse(this.toolStripComboBox_page.SelectedItem.ToString());
                }
            }
        }

        public void First() { this.CurrentPage = 1; }
        public void Prev() { this.CurrentPage--; }
        public void Next() { this.CurrentPage++; }
        public void Last() { this.CurrentPage = TotalPageCount; }

        private void SetButtonsEnabled()
        {
            this.toolStripButton_prev.Enabled = true;
            this.toolStripButton_first.Enabled = true;
            this.toolStripButton_last.Enabled = true;
            this.toolStripButtonNext.Enabled = true;
            if (CurrentPage == 1)
            {
                this.toolStripButton_prev.Enabled = false;
                this.toolStripButton_first.Enabled = false;
            }

            if (CurrentPage == TotalPageCount)
            {
                this.toolStripButton_last.Enabled = false;
                this.toolStripButtonNext.Enabled = false;
            }
        }

        void toolStripComboBox_page_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CurrentPage = (int)this.toolStripComboBox_page.SelectedItem;
        }

        void toolStripComboBox_pagelimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PageLimit = int.Parse(this.toolStripComboBox_pagelimit.SelectedItem.ToString());
        }

        private void Pagger_LayoutCompleted(object sender, EventArgs e)
        {
            this.toolStripComboBox_pagelimit.Text = PageLimit + "";
        }
    }
}
