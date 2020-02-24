namespace Gnsser.Winform
{
    partial class WorkOperationControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            Geo.Workflow workflow1 = new Geo.Workflow();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.workflowEditControl1 = new Gnsser.Winform.WorkflowEditControl();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.operationFlowEditControl1 = new Gnsser.Winform.OperationFlowEditControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.paramEditControl1 = new Gnsser.Winform.ParamEditControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(699, 568);
            this.splitContainer1.SplitterDistance = 133;
            this.splitContainer1.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.workflowEditControl1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(133, 568);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "工作流程";
            // 
            // workflowEditControl1
            // 
            this.workflowEditControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            workflow1.Name = "";
            this.workflowEditControl1.Entity = workflow1;
            this.workflowEditControl1.IsChangeSaved = true;
            this.workflowEditControl1.Location = new System.Drawing.Point(3, 21);
            this.workflowEditControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.workflowEditControl1.Name = "workflowEditControl1";
            this.workflowEditControl1.Size = new System.Drawing.Size(127, 544);
            this.workflowEditControl1.TabIndex = 0;
            this.workflowEditControl1.CurrentGofChanged += new Gnsser.Winform.CurrentGofChangedEventHandler(this.workflowEditControl1_CurrentGofChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer2.Size = new System.Drawing.Size(562, 568);
            this.splitContainer2.SplitterDistance = 209;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.operationFlowEditControl1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(209, 568);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "操作流程";
            // 
            // operationFlowEditControl1
            // 
            this.operationFlowEditControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.operationFlowEditControl1.Entity = null;
            this.operationFlowEditControl1.IsChangeSaved = true;
            this.operationFlowEditControl1.Location = new System.Drawing.Point(3, 21);
            this.operationFlowEditControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.operationFlowEditControl1.Name = "operationFlowEditControl1";
            this.operationFlowEditControl1.Size = new System.Drawing.Size(203, 544);
            this.operationFlowEditControl1.TabIndex = 0;
            this.operationFlowEditControl1.OperationInfoChanged += new Gnsser.Winform.OperationInfoChangedEventHandler(this.operationFlowEditControl1_OperationInfoChaned);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.paramEditControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(349, 568);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "操作参数";
            // 
            // paramEditControl1
            // 
            this.paramEditControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paramEditControl1.IsChangeSaved = true;
            this.paramEditControl1.Location = new System.Drawing.Point(3, 21);
            this.paramEditControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.paramEditControl1.Name = "paramEditControl1";
            this.paramEditControl1.ParamTypeName = null;
            this.paramEditControl1.Size = new System.Drawing.Size(343, 544);
            this.paramEditControl1.TabIndex = 0;
            this.paramEditControl1.Type = null;
            // 
            // WorkOperationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "WorkOperationControl";
            this.Size = new System.Drawing.Size(699, 568);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox2;
        private WorkflowEditControl workflowEditControl1;
        private System.Windows.Forms.GroupBox groupBox3;
        private OperationFlowEditControl operationFlowEditControl1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox1;
        private ParamEditControl paramEditControl1;
    }
}
