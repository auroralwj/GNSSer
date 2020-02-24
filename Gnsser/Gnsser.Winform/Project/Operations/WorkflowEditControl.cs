//2015.10.19, czs, create in hongqing, 操作编辑文件
//2016.11.28, czs, edit in hongqing, 设置为自动保存结果

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser.Api;
using Geo;
using Gnsser;
using System.IO;
using Geo.IO;
using Geo.Winform;

namespace Gnsser.Winform
{
    /// <summary>
    /// 当前操作流文件已经修改
    /// </summary>
    /// <param name="OperationFlow"></param>
    public delegate void CurrentGofChangedEventHandler(OperationFlow OperationFlow);

    /// <summary>
    /// 工作流编辑器
    /// </summary>
    public partial class WorkflowEditControl : UserControl, Geo.Winform.IEntityEditForm<Workflow>, IWithMainForm
    {
        public event CurrentGofChangedEventHandler CurrentGofChanged;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public WorkflowEditControl()
        { 
            InitializeComponent();
            InitTable(typeof(OperationFlow));
            this.IsChangeSaved = true; 
        }
        /// <summary>
        ///  设置当前工作流
        /// </summary>
        /// <param name="Workflow"></param>
        public void SetWorkflow(Workflow Workflow)
        {
            if (Entity != null && Entity.Equals(Workflow))
            {
                return;
            }

            this.Entity = Workflow;
            this.EntityToUi();
        }


        public IMainForm MainForm { get; set; }

        /// <summary>
        /// 数据是否改变，是否提示保存
        /// </summary>
        public bool IsChangeSaved { get; set; }
        /// <summary>
        /// 操作信息
        /// </summary>
        public Workflow Entity { get; set; }

        /// <summary>
        ///  初始化表格
        /// </summary>
        /// <param name="type"></param>
        public void InitTable(Type type)
        {
            Geo.Utils.DataGridViewUtil.SetDataGridViewCloumnsWithProperties( this.dataGridView1,type, true); 
        }

        public void EntityToUi()
        {
            if (Entity == null) { Entity = new Workflow(); }
             
            this.bindingSource_dataview.DataSource = null;
            this.bindingSource_dataview.DataSource = Entity.Data; 
        }

        private void toolStripButtonAddNew_Click(object sender, EventArgs e) {  AddNew();  }


        private void 导入OToolStripButton_Click(object sender, EventArgs e)
        {
            var absPath = Geo.Utils.FormUtil.ShowFormGetFilePath("操作文件|*.gof");
            if (absPath == null) { return; }
            var result =  Setting.GnsserConfig.CurrentProject.ImportGofFile(absPath);
            if (result.ProcessState == ProcessState.Sucessed)
            {
                this.Entity = Setting.GnsserConfig.CurrentProject.Workflow;
                this.EntityToUi();

                SaveChanges();
            } 
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            var obj = Geo.Utils.DataGridViewUtil.GetSelectedObject<OperationFlow>(this.dataGridView1);
            if (obj == null)
            {
                MessageBox.Show("请选中后再试！");
                return;
            }
            bool dele = (Geo.Utils.FormUtil.ShowYesNoMessageBox("是否删除文件?删除后不可恢复!" + obj.FileName) == DialogResult.Yes);

            var result = Setting.GnsserConfig.CurrentProject.DeleteOperatonFlow(obj, dele);
            if (result.ProcessState == ProcessState.Sucessed)
            {
                this.Entity = Setting.GnsserConfig.CurrentProject.Workflow;
                this.EntityToUi();

                SaveChanges();
            }

        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)  {   EditContent();   }


        private void 保存SToolStripButton_Click(object sender, EventArgs e)
        {
            SaveChanges();
            MessageBox.Show("已成功保存到当前项目。");
        }

        //对选中重命名
        private void EditContent()
        {
            var obj = Geo.Utils.DataGridViewUtil.GetSelectedObject<OperationFlow>(this.dataGridView1);
            if (obj == null)
            {
                MessageBox.Show("请选中后再试！");
                return;
            }
            String name = null;
            if (Geo.Utils.FormUtil.ShowInputForm("修改工作流文件名称", obj.FileName, Entity.GofFileNames, out name))
            { 
                Setting.GnsserConfig.CurrentProject.RenameOperationFlow(obj, name);
                this.SaveChanges();
            }
        }

        private void bindingSource_dataview_CurrentChanged(object sender, EventArgs e)
        {
            OnCurrentGofChanged(bindingSource_dataview.Current as OperationFlow);
        }

        public void OnCurrentGofChanged(OperationFlow current)
        {
            if (CurrentGofChanged != null)
            {
                //try
                //{
                CurrentGofChanged(current);
                //}
                //catch (Exception ex)
                //{
                //    if (Geo.Utils.FormUtil.ShowYesNoMessageBox("解析工作流文件遇到问题，是否移除？" + ex.Message) == DialogResult.Yes)
                //    {
                //        this.bindingSource_dataview.Remove(bindingSource_dataview.Current);
                //        this.Entity.Remove(current);
                //    }
                //}
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)  {    EditContent();   }

        /// <summary>
        /// 保存更改
        /// </summary>
        internal void SaveChanges()
        {
            Setting.GnsserConfig.CurrentProject.Workflow = this.Entity;

            Setting.GnsserConfig.SaveCurrentProject();
            IsChangeSaved = true;
        }

        private void AddNew()
        {
            string filePath = null;
            if (Geo.Utils.FormUtil.ShowInputForm("请输入操作流文件名称", "NewOperationName", out filePath))
            {
                if (!filePath.ToLower().Contains(".gof")) { filePath = filePath + ".gof"; }
 
                var ScriptDirectory = Setting.GnsserConfig.CurrentProject.ScriptDirectory;

                var path = Path.Combine(ScriptDirectory, filePath);
                if (File.Exists(path))
                {
                    if (Geo.Utils.FormUtil.ShowYesNoMessageBox("文件已经存在，是否覆盖？" + path) != DialogResult.Yes)
                    {
                        return;
                    }
                }
                if (Entity.Contains(path))
                {
                    MessageBox.Show("当前工程已经包含该工作流文件！" + path);
                    return;
                }

                OperationFlow flow = new OperationFlow() { FileName = filePath };
                flow.SaveToDirectory(ScriptDirectory);

                this.Entity.Add(flow);
                EntityToUi();
                IsChangeSaved = false;

                SaveChanges();
            }
        }


        public void UiToEntity()
        {
            throw new NotImplementedException();
        }

        private void toolStripButton_openOutSide_Click(object sender, EventArgs e)
        {
            var obj = Geo.Utils.DataGridViewUtil.GetSelectedObject<OperationFlow>(this.dataGridView1);
            if (obj == null)
            {
                MessageBox.Show("请选中后再试！");
                return;
            } 
            var path = Setting.GnsserConfig.CurrentProject.GetAbsScriptPath(obj.FileName);
            if (System.IO.File.Exists(path))
            {
                Geo.Utils.FileUtil.OpenFile(path);
            }
            else
            {
                MessageBox.Show("文件不存在，请检查路径。" + path);
            }
        }

        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            EntityToUi();
        }

        private void 去运行选中操作流ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var objs = Geo.Utils.DataGridViewUtil.GetSelectedObjects<OperationFlow>(this.dataGridView1);
            if (objs == null)
            {
                MessageBox.Show("请选中后再试！");
                return;
            }
            List<string> pathes = new List<string>();
            foreach (var obj in objs)
            {
                var path = Setting.GnsserConfig.CurrentProject.GetAbsScriptPath(obj.FileName);
                pathes.Add(path);
            }
            if (MainForm != null)
            { 
                GotoGpeForm(pathes.ToArray());
            }

        }
        private void GotoGpeForm(string[] gofes)
        {
            var form = new WorkflowRunnerForm(gofes);
            form.Text = Setting.GnsserConfig.CurrentProject.ProjectName + "工作流运行器";
            this.MainForm.OpenMidForm(form);
        }
        private void 往上移动UToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var obj = Geo.Utils.DataGridViewUtil.GetSelectedObject<OperationFlow>(this.dataGridView1);

            Entity.MoveUp(obj);
            this.SaveChanges();
            EntityToUi();
        }

        private void 往下移动DToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var obj = Geo.Utils.DataGridViewUtil.GetSelectedObject<OperationFlow>(this.dataGridView1);

            Entity.MoveDown(obj);
            this.SaveChanges();
            EntityToUi();
        }
    }
}
