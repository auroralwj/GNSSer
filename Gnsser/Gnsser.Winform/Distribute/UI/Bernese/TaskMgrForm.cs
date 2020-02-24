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
    /// <summary>
    /// 任务管理器
    /// </summary>
    public partial class TaskMgrForm : Form
    {
        TaskMgr mgr;
        SiteMgr siteManager;
        /// <summary>
        /// 构造
        /// </summary>
        public TaskMgrForm()
        {
            InitializeComponent();
        }

        private void SiteNodeMgrForm_Load(object sender, EventArgs e)
        {
            if (File.Exists(Setting.GnsserConfig.SiteFilePath))
            {
                siteManager = new SiteMgr(Setting.GnsserConfig.SiteFilePath);
                siteNames = siteManager.GetSiteNames().ToArray();
            }
         
            BindMain();
        }

        private void BindMain()
        {
            mgr = new TaskMgr();

            List<Task> list = new TaskMgr(Setting.GnsserConfig.TaskFilePath, Setting.GnsserConfig.SiteFilePath).GetAllTasks();
            if (list.Count != 0)
            {
                TaskEditForm.BasicId = list[list.Count - 1].Id;
                BuildtBaseLineStakForm.BasicId = list[list.Count - 1].Id;
            }
            this.bindingSource_task.DataSource = list;
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            BindSub();
        }

        private void BindSub()
        {
            Task curent = this.bindingSource_task.Current as Task;
            if (curent != null)
            {
                this.bindingSource_sites.DataSource = null;
                this.bindingSource_sites.DataSource = curent.Sites;
            }
        }


        private void toolStripLabel_save_Click(object sender, EventArgs e)
        {
            mgr.Save();
        }

        //增加计算任务 
        private void 增加计算节点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaskEditForm form = new TaskEditForm();
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //this.bindingSource_raw.Add(form.task);
                mgr.Add(form.Task);
                mgr.Save();
                BindMain();
            }
        }

        private void 修改计算节点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Edit();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Edit();
        }

        private void Edit()
        {
            Task curent = this.bindingSource_task.Current as Task;
            TaskEditForm form = new TaskEditForm(curent);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                mgr.Edit(curent, this.bindingSource_task.IndexOf(curent));
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

        private void 增加测站ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (siteNames == null)
            {
                MessageBox.Show("请先设置任务名称！"); return;
            }
            Task curent = this.bindingSource_task.Current as Task;
            if (curent == null)
            {
                MessageBox.Show("请先选中任务！"); return;
            }
            Geo.Winform.Controls.ListItemSelecterForm form =
                new Geo.Winform.Controls.ListItemSelecterForm(siteNames);

            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                curent.Sites.Clear();

                foreach (object o in form.SelectedItems)
                    curent.Sites.Add(new Site() { Name = o.ToString() });

                mgr.Edit(curent, this.bindingSource_task.IndexOf(curent));
                mgr.Save();
                BindSub();
            }

        }


        private void 删除测站ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.bindingSource_sites.Current == null) return;

            Task curent1 = this.bindingSource_task.Current as Task;
            List<DataGridViewRow> rows = Geo.Utils.DataGridViewUtil.GetSelectedRows(this.dataGridView2);
            foreach (var item in rows)
            {
                Site curent = item.DataBoundItem as Site;
                curent1.Sites.Remove(curent);
            }

            mgr.Edit(curent1, this.bindingSource_task.IndexOf(curent1));

            mgr.Save();

            BindSub();
        }

        string[] siteNames;

        private void 手动输入测站ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task curent = this.bindingSource_task.Current as Task;
            if (curent == null)
            {
                MessageBox.Show("请先选中任务！"); return;
            }

            string[] lines;
            if (Geo.Utils.FormUtil.ShowInputLineForm("输入行或以逗号\",\"隔开", out lines))
            {
                curent.Sites.Clear();

                foreach (var o in lines)
                {
                    string[] sites = o.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in sites)
                    { 
                      curent.Sites.Add(new Site() { Name =item});
                    }
                }
                mgr.Edit(curent, this.bindingSource_task.IndexOf(curent));
                mgr.Save();
                BindSub();

            }

        }

        private void 建立基线任务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (new BuildtBaseLineStakForm().ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                BindMain();
            }
        }

        
    }

}
