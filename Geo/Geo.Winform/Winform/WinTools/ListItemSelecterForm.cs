using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Geo.Winform.Controls
{
    public partial class ListItemSelecterForm : Form
    {
        //public ListItemSelecterForm(string[] names)
        //{
        //    InitializeComponent();
        //    this.listView1.Items.Clear();

        //    foreach (var name in names)
        //    {
        //        ListViewItem key = new ListViewItem(name);
        //        key.Tag = name;
        //        this.listView1.Items.Add(key);
        //    }
        //}

        public ListItemSelecterForm(object[] names)
        {
            InitializeComponent();
            this.listView1.Items.Clear();

            foreach (var name in names)
            {
                ListViewItem item = new ListViewItem(name.ToString());
                item.Tag = name;
                this.listView1.Items.Add(item);
            }
        }
        public object [] SelectedItems { get; set; }
        private void button_ok_Click(object sender, EventArgs e)
        {
            List<Object> ls = new List<object>();
            foreach (ListViewItem item in this.listView1.SelectedItems)
            {
                ls.Add(item.Tag);
            }
            SelectedItems = ls.ToArray();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.listView1.SelectedItems != null)
            this.label_info.Text = "选择了 " + this.listView1.SelectedItems.Count + " 个";
        }

        #region 设置文件视图方式
        private void 缩略图ToolStripMenuItem_Click(object sender, EventArgs e) { this.listView1.View = View.LargeIcon; }
        private void 图标ToolStripMenuItem_Click(object sender, EventArgs e) { this.listView1.View = View.SmallIcon; }
        private void 平铺ToolStripMenuItem_Click(object sender, EventArgs e) { this.listView1.View = View.Tile; }
        private void 列表ToolStripMenuItem_Click(object sender, EventArgs e) { this.listView1.View = View.List; }
        private void 详细信息ToolStripMenuItem_Click(object sender, EventArgs e) { this.listView1.View = View.Details; }
        #endregion


    }
}
