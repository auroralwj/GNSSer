//2015.10.21, czs, create in hongqing, 操作参数编辑文件
//2016.11.28, czs, edit in hongqing, 设置为自动保存结果

using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser.Api;
using Geo;
using Geo.Winform;

namespace Gnsser.Winform
{

    public partial class ParamEditControl : UserControl
    {
        public ParamEditControl()
        {
            InitializeComponent();
            this.IsChangeSaved = true;

            ParamIoerManager = ParamIoerManager.Default;
            OperationManager = GnsserOperationManager.Default;
            ParamNameManager = ParamNameManager.Default;

            OperationParamManager = new OperationParamManager(); 
        }

        #region 属性
        ParamNameManager ParamNameManager { get; set; }
        OperationParamManager OperationParamManager { get; set; }
        BindingSource BindingSource { get { return bindingSource_dataview; } set { this.bindingSource_dataview = value; } }
        ParamIoerManager ParamIoerManager { get; set; }
        OperationManager OperationManager { get; set; }

        /// <summary>
        /// 参数类型名称
        /// </summary>
        public string ParamTypeName { get; set; }
        /// <summary>
        /// 数据是否改变，是否提示保存
        /// </summary>
        public bool IsChangeSaved { get; set; }
        /// <summary>
        /// 参数列表
        /// </summary>
        ArrayList Parameters { get; set; }
        /// <summary>
        /// 操作信息
        /// </summary>
        public OperationInfo Entity { get; private set; }
        /// <summary>
        /// 类型
        /// </summary>
        public Type Type { get; set; }
        #endregion

        /// <summary>
        /// 初始化表格。
        /// </summary>
        /// <param name="type"></param>
        public void InitTable(Type type)
        {
            this.Type = type;
            this.dataGridView1.Columns.Clear();
            Geo.Utils.DataGridViewUtil.SetDataGridViewCloumnsWithProperties(this.dataGridView1, type, true);
        }

        /// <summary>
        /// 以操作信息初始化
        /// </summary>
        /// <param name="OperationInfo"></param>
        public void SetOperationInfo2(OperationInfo OperationInfo)
        {
            if (Entity != null && Entity.Equals(OperationInfo))
            {
                return;
            }
            if (OperationInfo != null)
            {
                //第二后缀,标识参数类型，这样设计免得还要建立一个操作与参数类型的关联 //czs, 2015.10.22
                this.ParamTypeName = OperationInfo.ParamTypeName;
                //初始化表格
                InitTable(this.OperationManager.Get(OperationInfo.OperationName).ParamType);
                Parameters = OperationParamManager.GetParams(OperationInfo);
            }
            this.Entity = OperationInfo;
            ObjToUi();
        }
         

        /// <summary>
        /// 以操作信息初始化
        /// </summary>
        /// <param name="OperationInfo"></param>
        public void SetOperationInfo(OperationInfo OperationInfo)
        {
            if (Entity != null && Entity.Equals(OperationInfo))
            {
                return;
            }
            if (OperationInfo != null)
            {
                //第二后缀,标识参数类型，这样设计免得还要建立一个操作与参数类型的关联 //czs, 2015.10.22
                this.ParamTypeName = OperationInfo.ParamTypeName;
                if(! this.ParamNameManager.Contains(this.ParamTypeName)  ){
                    MessageBox.Show("参数未注册或未授权！" + this.ParamTypeName);
                    return ;
                }
                var paramName = ParamNameManager[ParamTypeName];
                var paramObj = Activator.CreateInstance(paramName.AssemblyName, paramName.FullName);
                var ParamType = paramObj.Unwrap().GetType();

                //初始化表格
              // InitTable(this.OperationManager.Get(OperationInfo.OperationName).ParamType);
                InitTable(ParamType);
                Parameters = OperationParamManager.GetParams(OperationInfo);
            }
            this.Entity = OperationInfo;
            ObjToUi();
        }

        /// <summary>
        /// 对象到界面
        /// </summary>
        public void ObjToUi()
        {
            if (Entity == null) { Entity = new OperationInfo(); }

            this.bindingSource_dataview.DataSource = null;
            this.bindingSource_dataview.DataSource = Parameters;
        }

        private void toolStripButtonAddNew_Click(object sender, EventArgs e)
        {
            if (this.Type == null) { MessageBox.Show("参数文件尚未初始化！请手动添加一个参数。"); }
            var newObj = Activator.CreateInstance(this.Type);
            DisplayPropertyEditForm form = new DisplayPropertyEditForm(newObj);

            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.Parameters.Add(newObj);
                ObjToUi();
                this.IsChangeSaved = false;
                //this.Object.Add(form.Object);
                SaveChanges();
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            var obj = Geo.Utils.DataGridViewUtil.GetSelectedObject(this.dataGridView1);
            if (obj == null)
            {
                MessageBox.Show("请选中后再试！");
                return;
            }

            if (this.Parameters.Contains(obj))
            {
                this.Parameters.Remove(obj);
                ObjToUi();
                this.IsChangeSaved = false;

                SaveChanges();
            }
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
            EditCurrent();
        }

        private void EditCurrent()
        {
            var obj = Geo.Utils.DataGridViewUtil.GetSelectedObject(this.dataGridView1);
            if (obj == null)
            {
                MessageBox.Show("请选中后再试！");
                return;
            }

            DisplayPropertyEditForm form = new DisplayPropertyEditForm(obj);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ObjToUi();
                this.IsChangeSaved = false;
                //this.Object.Add(form.Object);
                SaveChanges();
            }
        }

        private void 保存SToolStripButton_Click(object sender, EventArgs e)
        {
            SaveChanges();
            MessageBox.Show("已成功保存到文件。" + Entity.ParamFilePath);
        }

        public void SaveChanges()
        {
            OperationParamManager.SaveChanges();
            this.IsChangeSaved = true;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EditCurrent();
        }
 

        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            ObjToUi();
        }


        private void 往上移动UToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var info = Geo.Utils.DataGridViewUtil.GetSelectedObject<RowClass>(this.dataGridView1);

            var index = this.Parameters.IndexOf(info);
            if (index > 0)
            {
                Parameters.RemoveAt(index);
                Parameters.Insert(index - 1, info);
            }

            SaveChanges();
            ObjToUi();
        }

        private void 往下移动DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var val = Geo.Utils.DataGridViewUtil.GetSelectedObject<RowClass>(this.dataGridView1);

            var index = this.Parameters.IndexOf(val);
            if (index != -1 && index < this.Parameters.Count - 1)
            {
                Parameters.RemoveAt(index);
                Parameters.Insert(index + 1, val);
            }
            SaveChanges();
            ObjToUi();
        }
    }

    //2015.11.10, czs, create in pengzhou, 操作参数管理器
    /// <summary>
    /// 操作参数管理器
    /// </summary>
    public class OperationParamManager
    {
        public OperationParamManager()
        {
            ParamIoerManager = ParamIoerManager.Default;
            OperationManager = GnsserOperationManager.Default;
        }


        /// <summary>
        /// 参数
        /// </summary>
        public ArrayList Parameters { get; set; }
        public OperationInfo Entity { get; set; }
        public OperationManager OperationManager { get; set; }
        public ParamIoerManager ParamIoerManager { get; set; }
        /// <summary>
        /// 参数类型名称
        /// </summary>
        public string ParamTypeName { get; set; }
        /// <summary>
        /// 从参数文件读取
        /// </summary>
        /// <param name="OperationInfo"></param>
        /// <returns></returns>
        public ArrayList GetParams(OperationInfo OperationInfo)
        {
            Parameters = new ArrayList();

            if (Entity != null && Entity.Equals(OperationInfo))
            {
                return Parameters;
            }

            this.Entity = OperationInfo;

            if (OperationInfo != null)
            {

                //第二后缀,标识参数类型，这样设计免得还要建立一个操作与参数类型的关联 //czs, 2015.10.22
                this.ParamTypeName = Entity.ParamTypeName;
                //初始化表格
                //    InitTable(this.OperationManager.Get(Entity.OperationName).ParamType);

                var absPath = Setting.GnsserConfig.CurrentProject.GetAbsScriptPath(Entity.ParamFilePath);

                //try
                //{
                int i = 0;
                if (ParamIoerManager.Contains(ParamTypeName))
                {
                    var reader = ParamIoerManager[ParamTypeName];

                    if (File.Exists(absPath))
                    {
                        reader.Reader.Init(absPath);

                        foreach (var item in reader.Reader)
                        {
                            Parameters.Add(item);
                        }
                        i++;
                    }
                    else
                    {
                        MessageBox.Show("参数文件不存在。" + absPath);
                    }
                }
            }
            return Parameters;
        }

        /// <summary>
        /// 保存到参数文件
        /// </summary>
        public void SaveChanges()
        {
            int i = 0;
            if (ParamIoerManager.Contains(ParamTypeName))
            {
                var reader = ParamIoerManager[ParamTypeName];

                var absPath = Setting.GnsserConfig.CurrentProject.GetAbsScriptPath(this.Entity.ParamFilePath);

                reader.Writer.Init(absPath);
                foreach (var item in Parameters)
                {
                    reader.Writer.Write(item as IRowClass);
                }
                reader.Writer.Close();
                i++;
            } 
        }
    }
}