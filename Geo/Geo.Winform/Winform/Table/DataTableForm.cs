//2015.06.17, czs, create in namu, 表显示器
//2017.02.06, czs, edit in hongqing, 增加一些显示设置

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Utils;

namespace Geo.Winform
{
    /// <summary>
    /// 通用表格数据显示窗口。
    /// </summary>
    public partial class DataTableForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="isShowToolStrip"></param>
        /// <param name="isShowNavigator"></param>
        public DataTableForm(object dataSource = null, bool isShowToolStrip = true, bool isShowNavigator = true)
        {
            InitializeComponent();
            this.bindingNavigator1.Visible = isShowNavigator;
            this.toolStrip1.Visible = isShowToolStrip;
            
            BindDataSource(dataSource);
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="dataSource"></param>
        public void BindDataSource(object dataSource)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = dataSource;
            bindingNavigator1.BindingSource = bs;
            dataGridView1.DataSource = bs;
        }



        private void toolStripButton_toExcel_Click(object sender, EventArgs e) { ReportUtil.SaveToExcel(this.dataGridView1); }

        private void toolStripButton_toWord_Click(object sender, EventArgs e) { ReportUtil.SaveToWord(this.dataGridView1); }
       
    }
}
