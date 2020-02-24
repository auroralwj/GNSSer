//2015.11.10, czs, edit in penghzou, 增加未保存提示

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser.Api;
using Geo;
using Gnsser;
using Geo.IO;
using Geo.Winform;

namespace Gnsser.Winform
{
    public partial class WorkOperationControl : UserControl, IWithMainForm
    {
        public WorkOperationControl()
        {
            InitializeComponent();
        }

        public void SetWorkflow(Workflow Workflow)
        {
            this.workflowEditControl1.SetWorkflow(Workflow);
        }

        private void workflowEditControl1_CurrentGofChanged(Geo.OperationFlow OperationFlow)
        {
            if (!this.operationFlowEditControl1.IsChangeSaved)
            {
                if (Geo.Utils.FormUtil.ShowYesNoMessageBox("操作列表未保存，是否保存？") == DialogResult.Yes)
                {
                    this.operationFlowEditControl1.SaveChanges();
                }
            }
            this.operationFlowEditControl1.SetOperationFlow(OperationFlow);
        }

        private void operationFlowEditControl1_OperationInfoChaned(OperationInfo OperationInfo)
        {
            if (!this.paramEditControl1.IsChangeSaved)
            {
                if (Geo.Utils.FormUtil.ShowYesNoMessageBox("参数列表未保存，是否保存？") == DialogResult.Yes)
                {
                    this.paramEditControl1.SaveChanges();
                }
            }
            this.paramEditControl1.SetOperationInfo(OperationInfo);
        }

        /// <summary>
        /// 改变是否更改
        /// </summary>
        public bool IsChangedSaved { get { return this.workflowEditControl1.IsChangeSaved && this.operationFlowEditControl1.IsChangeSaved; } }
        
        /// <summary>
        /// 保存更改
        /// </summary>
        public void SaveChanges()
        {
            if (!this.workflowEditControl1.IsChangeSaved)
            {
                this.workflowEditControl1.SaveChanges();
            }
            if (!this.operationFlowEditControl1.IsChangeSaved)
            {
                this.operationFlowEditControl1.SaveChanges();
            } 
        }


        public IMainForm MainForm
        {
            get
            {
              return    this.workflowEditControl1.MainForm;
            }
            set
            {
                this.workflowEditControl1.MainForm = value;
            }
        }
    }
}
