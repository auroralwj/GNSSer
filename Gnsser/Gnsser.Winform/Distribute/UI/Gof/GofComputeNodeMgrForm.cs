using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows.Forms;

namespace Gnsser.Winform
{
    public partial class GofComputeNodeMgrForm : Form
    {
        ComputeNodeMgr<GofTask> mgr;
        GofTaskMgr taskMgr;
        List<GofTask> tasks;

        public GofComputeNodeMgrForm()
        {
            InitializeComponent();
        }

        private void SiteNodeMgrForm_Load(object sender, EventArgs e)
        {
            DataBind();
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        private void DataBind()
        {
            taskMgr = new GofTaskMgr(Setting.GnsserConfig.GofTaskFilePath);
            tasks = taskMgr.GetAllTasks();
            BindMain();
        }

        private void BindMain()
        {
            mgr = new GofComputeNodeMgr(Setting.GnsserConfig.ComputeNodeFilePath);
            List<GofComputeNode> list = GofComputeNodeMgr.LoadGofComputeNodes(
                Setting.GnsserConfig.ComputeNodeFilePath,
                Setting.GnsserConfig.GofTaskFilePath);
            this.bindingSource1.DataSource = list;
        }

        private void toolStripLabel_save_Click(object sender, EventArgs e)
        {
            mgr.Save();
        }

        private void 增加计算节点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ComputeNodeEditForm form = new ComputeNodeEditForm();
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                mgr.Add( form.ComputeNode);
                mgr.Save();
                BindMain();
            }
        }

        private void 修改计算节点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GofComputeNode curent = this.bindingSource1.Current as GofComputeNode;
            ComputeNodeEditForm form = new ComputeNodeEditForm(curent);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                mgr.Edit(curent, this.bindingSource1.IndexOf(curent));
                mgr.Save();
                BindMain();
            }          
        }

        private void 删除计算节点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> ls = Geo.Utils.DataGridViewUtil.GetSelectedRows(this.dataGridView1);
            foreach (var item in ls)
            {
                mgr.Delete(item.Index);
                mgr.Save();
            }
            BindMain();
        }

        private void 刷新计算节点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BindMain();
        }
        
        private void 变更启用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriggerCurrentEnable();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            TriggerCurrentEnable();
        }

        private void TriggerCurrentEnable()
        {
            GofComputeNode curent = this.bindingSource1.Current as GofComputeNode;
            curent.Enabled = !curent.Enabled;
            mgr.Edit(curent, this.bindingSource1.IndexOf(curent));
            mgr.Save();
            BindMain();
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            BindSub();
        }

        private void BindSub()
        {
            GofComputeNode curent = this.bindingSource1.Current as GofComputeNode;
            if (curent != null)
            {
                this.bindingSource2.DataSource = null;
                this.bindingSource2.DataSource = curent.Tasks;
            }
        }

        private void 选择计算任务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tasks == null)
            {
                MessageBox.Show("请先设置任务名称！"); return;
            }
            GofComputeNode curent = this.bindingSource1.Current as GofComputeNode;
            if (curent == null)
            {
                MessageBox.Show("请先选中任务！"); return;
            }
            Geo.Winform.Controls.ListItemSelecterForm form =
                new Geo.Winform.Controls.ListItemSelecterForm(tasks.ToArray());

            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                curent.Tasks.Clear();

                foreach (object o in form.SelectedItems)
                    curent.Tasks.Add(o as GofTask);

                mgr.Edit(curent, this.bindingSource1.IndexOf(curent));
                mgr.Save();
                BindSub();
            }
        }

        private void 手动输入任务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GofComputeNode curent = this.bindingSource1.Current as GofComputeNode;
            if (curent == null)
            {
                MessageBox.Show("请先选中任务！"); return;
            }

            string[] lines;
            if (Geo.Utils.FormUtil.ShowInputLineForm("输入行", out lines))
            {
                curent.Tasks.Clear();

                foreach (object o in lines)
                {
                    var task = tasks.Find(m => m.Name == o.ToString() || m.Id == int.Parse(o.ToString()));
                    curent.Tasks.Add(task);
                }

                mgr.Edit(curent, this.bindingSource1.IndexOf(curent));
                mgr.Save();
                BindSub();

            }
        }

        private void 删除任务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.bindingSource2.Current == null) return;

            GofComputeNode curent1 = this.bindingSource1.Current as GofComputeNode;
            List<DataGridViewRow> rows = Geo.Utils.DataGridViewUtil.GetSelectedRows(this.dataGridView2);
            foreach (var item in rows)
            {
                GofTask curent = item.DataBoundItem as GofTask;
                curent1.Tasks.Remove(curent);
            }

            mgr.Edit(curent1, this.bindingSource1.IndexOf(curent1));

            mgr.Save();

            BindSub();

        }

        private void button_taskMaitain_Click(object sender, EventArgs e)
        {
            var form = new GofTaskMgrForm();
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.DataBind();
            } 
        }

        private void button_reload_Click(object sender, EventArgs e)
        {
            this.DataBind();
        }


    }

}
