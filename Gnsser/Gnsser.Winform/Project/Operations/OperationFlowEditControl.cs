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

namespace Gnsser.Winform
{
    public delegate void OperationInfoChangedEventHandler(OperationInfo OperationInfo);
       

    /// <summary>
    /// 编辑操作流文件夹 GOF Gnsser Operation Flow
    /// </summary>
    public partial class OperationFlowEditControl : UserControl, Geo.Winform.IEntityEditForm<OperationFlow>
    {
        public event OperationInfoChangedEventHandler OperationInfoChanged;

        public OperationFlowEditControl()
        { 
            InitializeComponent();
            this.IsChangeSaved = true;
            InitTable(typeof(OperationInfo));
        }
        public void SetOperationFlow(OperationFlow OperationFlow)
        {
            if (OperationFlow != null)
            {
                if (Entity != null && Entity.Equals(OperationFlow))
                {
                    return;
                } 

                OperationFlow.Clear();
                var path = Setting.GnsserConfig.CurrentProject.GetAbsScriptPath(OperationFlow.FileName);

                OperationInfoReader reader = new OperationInfoReader(path);

                OperationFlow.Add(reader.ReadAll()); 
            }

            this.Entity = OperationFlow;
            EntityToUi();
        }
        /// <summary>
        /// 数据是否改变，是否提示保存
        /// </summary>
        public bool IsChangeSaved { get; set; }
        /// <summary>
        /// 操作信息
        /// </summary>
        public OperationFlow Entity { get;   set; }

        public void InitTable(Type type)
        {
            this.dataGridView1.Columns.Clear();
            Geo.Utils.DataGridViewUtil.SetDataGridViewCloumnsWithProperties(this.dataGridView1,type, true); 
        }  

        public void EntityToUi()
        {
            if (Entity == null) { Entity = new OperationFlow(); }
          
            this.bindingSource_dataview.DataSource = null;
            this.bindingSource_dataview.DataSource = Entity.Data; 
        } 

        private void toolStripButtonAddNew_Click(object sender, EventArgs e)
        {
            OperationInfoCreationForm form = new OperationInfoCreationForm();
            form.BaseDirectory = Setting.GnsserConfig.CurrentProject.ParamDirectory;
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.Entity.Add(form.Entity);                 
                EntityToUi();
                //this.IsChangeSaved = false;
                this.SaveChanges();
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            var obj =  Geo.Utils.DataGridViewUtil.GetSelectedObject<OperationInfo>(this.dataGridView1);
            if (obj == null)
            {
                MessageBox.Show("请选中后再试！");
                return;
            }
            if (this.Entity.Remove(obj))
            {
                EntityToUi();

                this.IsChangeSaved = false;

                this.SaveChanges();
            }
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
            EditCurrent();
        }

        private void EditCurrent()
        {
            var obj = Geo.Utils.DataGridViewUtil.GetSelectedObject<OperationInfo>(this.dataGridView1);
            if (obj == null)
            {
                MessageBox.Show("请选中后再试！");
                return;
            }

            OperationInfoEditForm form = new OperationInfoEditForm(obj);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                EntityToUi();

                this.IsChangeSaved = false;
                //this.Object.Add(form.Object);
                this.SaveChanges();
            }
        }

        private void 保存SToolStripButton_Click(object sender, EventArgs e)
        {
            SaveChanges();
            MessageBox.Show("已成功保存到文件。" + Entity.FileName);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EditCurrent();
        }

        internal void SaveChanges()
        {
            this.Entity.SaveToDirectory(Setting.GnsserConfig.CurrentProject.ScriptDirectory);

            this.IsChangeSaved = true;
        }

        private void bindingSource_dataview_CurrentChanged(object sender, EventArgs e)
        {
            var val = (OperationInfo)this.bindingSource_dataview.Current;
            OnOperationInfoChanged(val);
        }

        public void OnOperationInfoChanged(OperationInfo val)
        {
            if (OperationInfoChanged != null) { OperationInfoChanged(val); }
        }

        private void toolStripButton_openOutSide_Click(object sender, EventArgs e)
        {
            var obj = Geo.Utils.DataGridViewUtil.GetSelectedObject<OperationInfo>(this.dataGridView1);
            if (obj == null)
            {
                MessageBox.Show("请选中后再试！");
                return;
            }
            var path = Setting.GnsserConfig.CurrentProject.GetAbsPath(obj.ParamFilePath);
            if (System.IO.File.Exists(path))
            { 
                Geo.Utils.FileUtil.OpenFile(path);
            }
            else
            {
                MessageBox.Show("文件不存在，请检查路径。" + path);
            }
        }


        public void UiToEntity()
        {
            throw new NotImplementedException();
        }

        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            EntityToUi();
        }

        private void 导入OToolStripButton_Click(object sender, EventArgs e)
        {
            OperationInfoEditForm form = new OperationInfoEditForm();
            //form.BaseDirectory = Setting.GnsserConfig.CurrentProject.ParamDirectory;
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.Entity.Add(form.Entity);
                EntityToUi();               
                this.IsChangeSaved = false;

                this.SaveChanges();
            }
        }

        private void 往上移动UToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var info = Geo.Utils.DataGridViewUtil.GetSelectedObject<OperationInfo>(this.dataGridView1);

            this.Entity.MoveUp(info);
            SaveChanges();
            this.EntityToUi();
        }

        private void 往下移动DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var info = Geo.Utils.DataGridViewUtil.GetSelectedObject<OperationInfo>(this.dataGridView1);

            this.Entity.MoveDown(info);
            SaveChanges();
            this.EntityToUi();
        }
    }

}
