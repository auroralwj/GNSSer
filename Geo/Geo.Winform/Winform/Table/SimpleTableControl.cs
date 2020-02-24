//2015.05.31， czs , edit in namu, 增加注释，稍微调整。

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Geo.Utils;
using System.Runtime.InteropServices;
using System.IO; 
using Geo;

namespace Geo.Winform
{

    public delegate void EntitySelectedEventHandler<T>(List<T> entities);
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperationType { Add, Delete, Edit, Select }
    /// <summary>
    /// 发生了操作
    /// </summary>
    /// <param name="type"></param>
    public delegate void OperationEventHandler(OperationType type, object sender);

    /// <summary>
    /// 简单数据表控件，只负责显示。先调用 Init，再调用 DataBind
    /// </summary>
    public partial class SimpleTableControl : UserControl
    {

        ISimpleTableManager manager;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SimpleTableControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 排序
        /// </summary>
        public event Action<string, SortOrder> ColumnSortEventHandler;
        public event  OperationEventHandler Operation;

        protected void OnOperation(OperationType type, object sender)
        {
            if (Operation != null) { Operation(type, sender); }
        }
        #region API 
        #region 属性
        /// <summary>
        /// 图标快捷方式
        /// </summary>
        public ToolStrip ToolStrip { get { return this.toolStrip1; } }
        /// <summary>
        ///  数据表
        /// </summary>
        public DataGridView DataGridView { get { return this.dataGridView1; } }
        /// <summary>
        /// 不在表格中显示的字段。
        /// </summary>
        public List<string> HiddenColumns { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get { return this.label_title.Text; } set { this.label_title.Text = value; } }
        /// <summary>
        ///是否显示标题
        /// </summary>
        public bool IsShowTitle { get { return this.label_title.Visible; } set { this.label_title.Visible = value; } }
        /// <summary>
        /// 是否启用导出功能
        /// </summary>
        public bool EnableExport
        {
            get
            {
                return this.word导出ToolStripMenuItem.Visible;
            }
            set
            {
                this.word导出ToolStripMenuItem.Visible = value;
                this.excel导出ToolStripMenuItem.Visible = value;
                this.toolStripButton_toWord.Visible = value;
                this.toolStripButton_toExcel.Visible = value;  
            }
        }

        public bool ShowToolStrip { get { return ToolStrip.Visible; } set { this.ToolStrip.Visible = value; } }

        #endregion

        #region 方法
        /// <summary>
        /// 初始化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="useDisplayName"></param>
        /// <param name="HiddenColumns"></param>
        public void Init<T>(bool useDisplayName = true, List<string> HiddenColumns = null) where T : class
        {
            this.manager = new BaseSimpleTableManager<T>(useDisplayName);
            this.HiddenColumns = HiddenColumns;

            SetColumns();
        }
        /// <summary>
        /// 绑定数据，同时将显示。
        /// </summary>
        /// <param name="dataSource"></param>
        public void DataBind(object dataSource)
        {
            this.Invoke(new Action(delegate()
            {
                this.bindingSource1.DataSource = dataSource;
                // this.dataGridView1.DataSource = dataSource;

            }));
        }
        #endregion
        #endregion

        #region 内部方法
        /// <summary>
        /// 设置数据表列。可在 UseDisplayName 赋值后调用。
        /// </summary>
        private void SetColumns()
        {
            if (manager != null)//设计器支持
            {
                this.dataGridView1.Columns.Clear();
                var cols = manager.GetDataGridViewColumns(HiddenColumns, manager.UseDisplayName);
                this.dataGridView1.Columns.AddRange(cols);
            }
        }

        private void toolStripButton_toExcel_Click(object sender, EventArgs e) { ReportUtil.SaveToExcel(this.dataGridView1); }

        private void toolStripButton_toWord_Click(object sender, EventArgs e) { ReportUtil.SaveToWord(this.dataGridView1); }
        #endregion

        /// <summary>
        /// 设置排序模式
        /// </summary>
        /// <param name="torf"></param>
        public void SetColumnSortModel(DataGridViewColumnSortMode model = DataGridViewColumnSortMode.Automatic)
        {
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            { 
                this.dataGridView1.Columns[i].SortMode = model;
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var col = this.dataGridView1.Columns[e.ColumnIndex]; 
            if (col.SortMode!= DataGridViewColumnSortMode.Programmatic)
            {
                return;
            }

            switch (col.HeaderCell.SortGlyphDirection)
            {
                case SortOrder.None:
                case SortOrder.Ascending:
                    //在这里加入排序的逻辑 //设置列标题的状体 
                    OnColumnSortEventHandler(col.HeaderText, SortOrder.Descending);
                    this.dataGridView1.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    break;
                case SortOrder.Descending:
                    OnColumnSortEventHandler(col.HeaderText, SortOrder.Ascending);
                    this.dataGridView1.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    break;
                default:
                    OnColumnSortEventHandler(col.HeaderText, SortOrder.Descending);
                    this.dataGridView1.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Descending;
                    break;
            }
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="SortOrder"></param>
        protected void OnColumnSortEventHandler(string colName, SortOrder SortOrder)
        {
            if (ColumnSortEventHandler != null) { ColumnSortEventHandler(colName, SortOrder); }
        }

        private void 选择SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnOperation(OperationType.Select, this.dataGridView1);
        }

        public List<T> GetSelectedObjects<T>()
        {
           return  Geo.Utils.DataGridViewUtil.GetSelectedObjects<T>(this.dataGridView1); 
        }
    }
}
