//2015.11.03, czs, create in hongqing, GOF任务编辑器。

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
using Geo;

namespace Gnsser.Winform
{
    /// <summary>
    /// 任务管理器
    /// </summary>
    public partial class GofTaskMgrForm : Form
    {
        GofTaskMgr mgr;
        List<GofTask> tasks;
        /// <summary>
        /// 构造
        /// </summary>
        public GofTaskMgrForm()
        {
            InitializeComponent();
        }

        private void SiteNodeMgrForm_Load(object sender, EventArgs e)
        { 
            BindMain();
        }

        private void BindMain()
        {
            mgr = new GofTaskMgr(Setting.GnsserConfig.GofTaskFilePath);
            tasks = mgr.GetAllTasks();
            this.bindingSource_task.DataSource = tasks;
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        { 
        }
         

        private void toolStripLabel_save_Click(object sender, EventArgs e)
        {
            mgr.Save();
        }

        //增加计算任务 
        private void 增加计算节点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TaskEditForm form = new TaskEditForm();
            //if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    //this.bindingSource_raw.Add(form.task);
            //    mgr.Add(form.Task);
            //    mgr.Save();
            //    BindMain();
            //}
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
            //Task curent = this.bindingSource_task.Current as Task;
            //TaskEditForm form = new TaskEditForm(curent);
            //if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    mgr.Edit(curent, this.bindingSource_task.IndexOf(curent));
            //    mgr.Save();
            //    BindMain();
            //}
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

        private void 导入OToolStripButton_Click(object sender, EventArgs e)
        { 
            var absPath = Geo.Utils.FormUtil.ShowFormGetFilePath("操作文件|*.gof");
            if (absPath == null) { return; }
            OperationFlow operFlow = OperationFlow.ReadFromFile(absPath);
            var task = new GofTask(operFlow);
            this.mgr.Add(task);
            this.mgr.Save();
          //  tasks.Add(task);
            this.bindingSource_task.Add(task);
        }  
    }

}
