namespace Gnsser.Winform
{
    partial class SatEevationSolverForm
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
            this.fileOpenControl_nav = new Geo.Winform.Controls.FileOpenControl();
            this.checkBox_enableStatistics = new System.Windows.Forms.CheckBox();
            this.checkBox_sortPrnSatistics = new System.Windows.Forms.CheckBox();
            this.namedFloatControl1AngleCut = new Geo.Winform.Controls.NamedFloatControl();
            this.timeLoopControl1 = new Geo.Winform.Controls.TimeLoopControl();
            this.panel4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel_buttons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage0
            // 
            this.tabPage0.Size = new System.Drawing.Size(832, 221);
            // 
            // panel3
            // 
            this.panel3.Size = new System.Drawing.Size(826, 123);
            // 
            // panel2
            // 
            this.panel2.Size = new System.Drawing.Size(826, 123);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.timeLoopControl1);
            this.panel4.Controls.Add(this.namedFloatControl1AngleCut);
            this.panel4.Controls.Add(this.checkBox_sortPrnSatistics);
            this.panel4.Controls.Add(this.checkBox_enableStatistics);
            this.panel4.Size = new System.Drawing.Size(826, 123);
            this.panel4.Paint += new System.Windows.Forms.PaintEventHandler(this.panel4_Paint);
            this.panel4.Controls.SetChildIndex(this.checkBox_enableStatistics, 0);
            this.panel4.Controls.SetChildIndex(this.checkBox_sortPrnSatistics, 0);
            this.panel4.Controls.SetChildIndex(this.namedFloatControl1AngleCut, 0);
            this.panel4.Controls.SetChildIndex(this.timeLoopControl1, 0);
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl_inputPathes.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileOpenControl_inputPathes.FilePathes = new string[0];
            this.fileOpenControl_inputPathes.LabelName = "O文件：";
            this.fileOpenControl_inputPathes.Size = new System.Drawing.Size(826, 76);
            this.fileOpenControl_inputPathes.FilePathSetted += new System.EventHandler(this.fileOpenControl1_FilePathSetted);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.fileOpenControl_nav);
            this.tabPage1.Size = new System.Drawing.Size(832, 129);
            this.tabPage1.Controls.SetChildIndex(this.fileOpenControl_inputPathes, 0);
            this.tabPage1.Controls.SetChildIndex(this.fileOpenControl_nav, 0);
            // 
            // panel_buttons
            // 
            this.panel_buttons.Location = new System.Drawing.Point(573, 0);
            // 
            // fileOpenControl_nav
            // 
            this.fileOpenControl_nav.AllowDrop = true;
            this.fileOpenControl_nav.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileOpenControl_nav.FilePath = "";
            this.fileOpenControl_nav.FilePathes = new string[0];
            this.fileOpenControl_nav.Filter = "Rinex导航、星历文件|*.*N;*.*P;*.*R;*.*G;*.SP3;*.EPH|所有文件|*.*";
            this.fileOpenControl_nav.FirstPath = "";
            this.fileOpenControl_nav.IsMultiSelect = false;
            this.fileOpenControl_nav.LabelName = "星历(非必须，推荐导航文件，更快)：";
            this.fileOpenControl_nav.Location = new System.Drawing.Point(3, 79);
            this.fileOpenControl_nav.Name = "fileOpenControl_nav";
            this.fileOpenControl_nav.Size = new System.Drawing.Size(826, 25);
            this.fileOpenControl_nav.TabIndex = 7;
            this.fileOpenControl_nav.FilePathSetted += new System.EventHandler(this.fileOpenControl_nav_FilePathSetted);
            // 
            // checkBox_enableStatistics
            // 
            this.checkBox_enableStatistics.AutoSize = true;
            this.checkBox_enableStatistics.Location = new System.Drawing.Point(15, 90);
            this.checkBox_enableStatistics.Name = "checkBox_enableStatistics";
            this.checkBox_enableStatistics.Size = new System.Drawing.Size(216, 16);
            this.checkBox_enableStatistics.TabIndex = 62;
            this.checkBox_enableStatistics.Text = "启用卫星可见性分析，仅分析第一个";
            this.checkBox_enableStatistics.UseVisualStyleBackColor = true;
            // 
            // checkBox_sortPrnSatistics
            // 
            this.checkBox_sortPrnSatistics.AutoSize = true;
            this.checkBox_sortPrnSatistics.Location = new System.Drawing.Point(237, 90);
            this.checkBox_sortPrnSatistics.Name = "checkBox_sortPrnSatistics";
            this.checkBox_sortPrnSatistics.Size = new System.Drawing.Size(132, 16);
            this.checkBox_sortPrnSatistics.TabIndex = 63;
            this.checkBox_sortPrnSatistics.Text = "卫星排序(分析结果)";
            this.checkBox_sortPrnSatistics.UseVisualStyleBackColor = true;
            // 
            // namedFloatControl1AngleCut
            // 
            this.namedFloatControl1AngleCut.Location = new System.Drawing.Point(117, 12);
            this.namedFloatControl1AngleCut.Name = "namedFloatControl1AngleCut";
            this.namedFloatControl1AngleCut.Size = new System.Drawing.Size(146, 23);
            this.namedFloatControl1AngleCut.TabIndex = 68;
            this.namedFloatControl1AngleCut.Title = "高度截止角(度)：";
            this.namedFloatControl1AngleCut.Value = -100D;
            // 
            // timeLoopControl1
            // 
            this.timeLoopControl1.Location = new System.Drawing.Point(4, 49);
            this.timeLoopControl1.Margin = new System.Windows.Forms.Padding(2);
            this.timeLoopControl1.Name = "timeLoopControl1";
            this.timeLoopControl1.Size = new System.Drawing.Size(578, 30);
            this.timeLoopControl1.TabIndex = 69;
            // 
            // SatEevationSolverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(840, 478);
            this.IsShowProgressBar = true;
            this.Name = "SatEevationSolverForm";
            this.Text = "卫星高度角计算器";
            this.Load += new System.EventHandler(this.WideLaneSolverForm_Load);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.panel_buttons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_nav;
        private System.Windows.Forms.CheckBox checkBox_enableStatistics;
        private System.Windows.Forms.CheckBox checkBox_sortPrnSatistics;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl1AngleCut;
        private Geo.Winform.Controls.TimeLoopControl timeLoopControl1;
    }
}