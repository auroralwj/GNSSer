namespace Gnsser.Winform
{
    partial class ProjectEditForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Gnsser.GnsserProject gnsserProject1 = new Gnsser.GnsserProject();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectEditForm));
            Geo.Workflow workflow1 = new Geo.Workflow();
            this.button_saveOrCreate = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.projectInfoControl1 = new Gnsser.Winform.ProjectInfoControl();
            this.SuspendLayout();
            // 
            // button_saveOrCreate
            // 
            this.button_saveOrCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_saveOrCreate.Location = new System.Drawing.Point(634, 475);
            this.button_saveOrCreate.Name = "button_saveOrCreate";
            this.button_saveOrCreate.Size = new System.Drawing.Size(94, 33);
            this.button_saveOrCreate.TabIndex = 2;
            this.button_saveOrCreate.Text = "创建/保存";
            this.button_saveOrCreate.UseVisualStyleBackColor = true;
            this.button_saveOrCreate.Click += new System.EventHandler(this.button_saveOrCreate_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(734, 475);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(89, 33);
            this.button_cancel.TabIndex = 6;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // projectInfoControl1
            // 
            this.projectInfoControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.projectInfoControl1.Location = new System.Drawing.Point(1, 2);
            this.projectInfoControl1.MainForm = null;
            this.projectInfoControl1.Name = "projectInfoControl1";
            gnsserProject1.Author = "Gnsser";
            gnsserProject1.Comments = ((System.Collections.Generic.List<string>)(resources.GetObject("gnsserProject1.Comments")));
            gnsserProject1.CommonDirectory = "D:\\GnsserTemp\\Common";
            gnsserProject1.CreationTime = new System.DateTime(2015, 10, 21, 10, 55, 40, 0);
            gnsserProject1.MiddleDirectory = "D:\\GnsserTemp\\Middle";
            gnsserProject1.Name = "Name";
            gnsserProject1.ObservationDirectory = "D:\\GnsserTemp\\Observation";
            gnsserProject1.OutputDirectory = "D:\\GnsserTemp\\Output";
            gnsserProject1.ParamDirectory = "D:\\GnsserTemp\\Script\\Param\\";
            gnsserProject1.ProjectDirectory = "D:\\GnsserTemp";
            gnsserProject1.ProjectName = "ProjectName";
            gnsserProject1.RevisedObsDirectory = "D:\\GnsserTemp\\RevisedObservation\\";
           // gnsserProject1.SatelliteTypes = ((System.Collections.Generic.List<Gnsser.SatelliteType>)(resources.GetObject("gnsserProject1.SatelliteTypes")));
            gnsserProject1.ScriptDirectory = "D:\\GnsserTemp\\Script";
            gnsserProject1.Version = 1D;
            workflow1.Name = "";
            gnsserProject1.Workflow = workflow1;
            this.projectInfoControl1.Entity = gnsserProject1;
            this.projectInfoControl1.Size = new System.Drawing.Size(842, 467);
            this.projectInfoControl1.TabIndex = 7;
            // 
            // ProjectEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(842, 520);
            this.Controls.Add(this.projectInfoControl1);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_saveOrCreate);
            this.Name = "ProjectEditForm";
            this.Text = "工程基础信息";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_saveOrCreate;
        private System.Windows.Forms.Button button_cancel;
        private ProjectInfoControl projectInfoControl1;
    }
}