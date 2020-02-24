namespace Gnsser.Winform
{
    partial class ObsFileAdjustStreamForm
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
            this.button_showOnMap = new System.Windows.Forms.Button();
            this.button_drawDifferLine = new System.Windows.Forms.Button();
            this.button_drawRmslines = new System.Windows.Forms.Button();
            this.checkBox_autoMatchingFile = new System.Windows.Forms.CheckBox();
            this.checkBox_enableNet = new System.Windows.Forms.CheckBox();
            this.paramVectorRenderControl1 = new Geo.Winform.Controls.ParamVectorRenderControl();
            this.checkBox_IsOpenReportWhenCompleted = new System.Windows.Forms.CheckBox();
            this.fileOpenControl_opt = new Geo.Winform.Controls.FileOpenControl();
            this.button_applyOpt = new System.Windows.Forms.Button();
            this.button_saveCurrent = new System.Windows.Forms.Button();
            this.button_optSaveAs = new System.Windows.Forms.Button();
            this.panel_opt = new System.Windows.Forms.Panel();
            this.enumRadioControl1_GnssSolverType = new Geo.Winform.EnumRadioControl();
            this.enumRadioControl_positionType = new Geo.Winform.EnumRadioControl();
            this.checkBox_clearCoords = new System.Windows.Forms.CheckBox();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel_buttonExtends.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel_buttons.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel_opt.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage0
            // 
            this.tabPage0.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage0.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage0.Size = new System.Drawing.Size(774, 201);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.enumRadioControl_positionType);
            this.panel3.Controls.Add(this.checkBox_autoMatchingFile);
            this.panel3.Controls.Add(this.checkBox_enableNet);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Size = new System.Drawing.Size(768, 119);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.paramVectorRenderControl1);
            this.panel2.Size = new System.Drawing.Size(768, 119);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.enumRadioControl1_GnssSolverType);
            this.panel4.Margin = new System.Windows.Forms.Padding(5);
            this.panel4.Controls.SetChildIndex(this.panel5, 0);
            this.panel4.Controls.SetChildIndex(this.enumRadioControl1_GnssSolverType, 0);
            // 
            // panel_buttonExtends
            // 
            this.panel_buttonExtends.Controls.Add(this.checkBox_clearCoords);
            this.panel_buttonExtends.Controls.Add(this.button_showOnMap);
            this.panel_buttonExtends.Controls.Add(this.button_drawDifferLine);
            this.panel_buttonExtends.Controls.Add(this.button_drawRmslines);
            this.panel_buttonExtends.Margin = new System.Windows.Forms.Padding(2);
            this.panel_buttonExtends.Size = new System.Drawing.Size(309, 29);
            // 
            // fileOpenControl_inputPathes
            // 
            this.fileOpenControl_inputPathes.FilePathes = new string[0];
            this.fileOpenControl_inputPathes.Location = new System.Drawing.Point(4, 4);
            this.fileOpenControl_inputPathes.Margin = new System.Windows.Forms.Padding(5);
            this.fileOpenControl_inputPathes.Size = new System.Drawing.Size(766, 95);
            // 
            // tabPage1
            // 
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(774, 125);
            // 
            // panel_buttons
            // 
            this.panel_buttons.Controls.Add(this.checkBox_IsOpenReportWhenCompleted);
            this.panel_buttons.Location = new System.Drawing.Point(473, 0);
            this.panel_buttons.Margin = new System.Windows.Forms.Padding(4);
            this.panel_buttons.Size = new System.Drawing.Size(309, 72);
            this.panel_buttons.Controls.SetChildIndex(this.panel_buttonExtends, 0);
            this.panel_buttons.Controls.SetChildIndex(this.checkBox_IsOpenReportWhenCompleted, 0);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel_opt);
            this.panel5.Controls.SetChildIndex(this.panel_opt, 0);
            // 
            // button_showOnMap
            // 
            this.button_showOnMap.Location = new System.Drawing.Point(3, 4);
            this.button_showOnMap.Name = "button_showOnMap";
            this.button_showOnMap.Size = new System.Drawing.Size(66, 25);
            this.button_showOnMap.TabIndex = 47;
            this.button_showOnMap.Text = "地图显示";
            this.button_showOnMap.UseVisualStyleBackColor = true;
            this.button_showOnMap.Click += new System.EventHandler(this.button_showOnMap_Click);
            // 
            // button_drawDifferLine
            // 
            this.button_drawDifferLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_drawDifferLine.Location = new System.Drawing.Point(232, 2);
            this.button_drawDifferLine.Name = "button_drawDifferLine";
            this.button_drawDifferLine.Size = new System.Drawing.Size(71, 23);
            this.button_drawDifferLine.TabIndex = 45;
            this.button_drawDifferLine.Text = "绘偏差图";
            this.button_drawDifferLine.UseVisualStyleBackColor = true;
            this.button_drawDifferLine.Click += new System.EventHandler(this.button_drawDifferLine_Click);
            // 
            // button_drawRmslines
            // 
            this.button_drawRmslines.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_drawRmslines.Location = new System.Drawing.Point(154, 4);
            this.button_drawRmslines.Name = "button_drawRmslines";
            this.button_drawRmslines.Size = new System.Drawing.Size(72, 23);
            this.button_drawRmslines.TabIndex = 46;
            this.button_drawRmslines.Text = "绘均方根";
            this.button_drawRmslines.UseVisualStyleBackColor = true;
            this.button_drawRmslines.Click += new System.EventHandler(this.button_drawRmslines_Click);
            // 
            // checkBox_autoMatchingFile
            // 
            this.checkBox_autoMatchingFile.AutoSize = true;
            this.checkBox_autoMatchingFile.Checked = true;
            this.checkBox_autoMatchingFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_autoMatchingFile.Location = new System.Drawing.Point(0, 2);
            this.checkBox_autoMatchingFile.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_autoMatchingFile.Name = "checkBox_autoMatchingFile";
            this.checkBox_autoMatchingFile.Size = new System.Drawing.Size(384, 16);
            this.checkBox_autoMatchingFile.TabIndex = 53;
            this.checkBox_autoMatchingFile.Text = "启用自动匹配数据源（可免部分数据源设置，如星历、钟差等文件）";
            this.checkBox_autoMatchingFile.UseVisualStyleBackColor = true;
            this.checkBox_autoMatchingFile.CheckedChanged += new System.EventHandler(this.checkBox_autoMatchingFile_CheckedChanged);
            // 
            // checkBox_enableNet
            // 
            this.checkBox_enableNet.AutoSize = true;
            this.checkBox_enableNet.Checked = true;
            this.checkBox_enableNet.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_enableNet.Location = new System.Drawing.Point(388, 2);
            this.checkBox_enableNet.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_enableNet.Name = "checkBox_enableNet";
            this.checkBox_enableNet.Size = new System.Drawing.Size(96, 16);
            this.checkBox_enableNet.TabIndex = 54;
            this.checkBox_enableNet.Text = "允许访问网络";
            this.checkBox_enableNet.UseVisualStyleBackColor = true;
            this.checkBox_enableNet.CheckedChanged += new System.EventHandler(this.checkBox_enableNet_CheckedChanged);
            // 
            // paramVectorRenderControl1
            // 
            this.paramVectorRenderControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paramVectorRenderControl1.Location = new System.Drawing.Point(0, 0);
            this.paramVectorRenderControl1.Margin = new System.Windows.Forms.Padding(2);
            this.paramVectorRenderControl1.Name = "paramVectorRenderControl1";
            this.paramVectorRenderControl1.Size = new System.Drawing.Size(768, 119);
            this.paramVectorRenderControl1.TabIndex = 0;
            // 
            // checkBox_IsOpenReportWhenCompleted
            // 
            this.checkBox_IsOpenReportWhenCompleted.AutoSize = true;
            this.checkBox_IsOpenReportWhenCompleted.Checked = true;
            this.checkBox_IsOpenReportWhenCompleted.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_IsOpenReportWhenCompleted.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBox_IsOpenReportWhenCompleted.Location = new System.Drawing.Point(0, 0);
            this.checkBox_IsOpenReportWhenCompleted.Name = "checkBox_IsOpenReportWhenCompleted";
            this.checkBox_IsOpenReportWhenCompleted.Size = new System.Drawing.Size(96, 43);
            this.checkBox_IsOpenReportWhenCompleted.TabIndex = 49;
            this.checkBox_IsOpenReportWhenCompleted.Text = "结束打开报表";
            this.checkBox_IsOpenReportWhenCompleted.UseVisualStyleBackColor = true;
            this.checkBox_IsOpenReportWhenCompleted.CheckedChanged += new System.EventHandler(this.checkBox_IsOpenReportWhenCompleted_CheckedChanged);
            // 
            // fileOpenControl_opt
            // 
            this.fileOpenControl_opt.AllowDrop = true;
            this.fileOpenControl_opt.FilePath = "";
            this.fileOpenControl_opt.FilePathes = new string[0];
            this.fileOpenControl_opt.Filter = "Opt文件|*.opt|文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_opt.FirstPath = "";
            this.fileOpenControl_opt.IsMultiSelect = false;
            this.fileOpenControl_opt.LabelName = "配置文件：";
            this.fileOpenControl_opt.Location = new System.Drawing.Point(3, 3);
            this.fileOpenControl_opt.Margin = new System.Windows.Forms.Padding(4);
            this.fileOpenControl_opt.Name = "fileOpenControl_opt";
            this.fileOpenControl_opt.Size = new System.Drawing.Size(298, 22);
            this.fileOpenControl_opt.TabIndex = 57;
            // 
            // button_applyOpt
            // 
            this.button_applyOpt.Location = new System.Drawing.Point(307, 3);
            this.button_applyOpt.Name = "button_applyOpt";
            this.button_applyOpt.Size = new System.Drawing.Size(49, 23);
            this.button_applyOpt.TabIndex = 58;
            this.button_applyOpt.Text = "应用";
            this.button_applyOpt.UseVisualStyleBackColor = true;
            this.button_applyOpt.Click += new System.EventHandler(this.button_applyOpt_Click);
            // 
            // button_saveCurrent
            // 
            this.button_saveCurrent.Location = new System.Drawing.Point(362, 3);
            this.button_saveCurrent.Name = "button_saveCurrent";
            this.button_saveCurrent.Size = new System.Drawing.Size(70, 23);
            this.button_saveCurrent.TabIndex = 58;
            this.button_saveCurrent.Text = "保存当前";
            this.button_saveCurrent.UseVisualStyleBackColor = true;
            this.button_saveCurrent.Click += new System.EventHandler(this.button_saveCurrent_Click);
            // 
            // button_optSaveAs
            // 
            this.button_optSaveAs.Location = new System.Drawing.Point(438, 3);
            this.button_optSaveAs.Name = "button_optSaveAs";
            this.button_optSaveAs.Size = new System.Drawing.Size(66, 23);
            this.button_optSaveAs.TabIndex = 58;
            this.button_optSaveAs.Text = "另存为";
            this.button_optSaveAs.UseVisualStyleBackColor = true;
            this.button_optSaveAs.Click += new System.EventHandler(this.button_optSaveAs_Click);
            // 
            // panel_opt
            // 
            this.panel_opt.Controls.Add(this.fileOpenControl_opt);
            this.panel_opt.Controls.Add(this.button_optSaveAs);
            this.panel_opt.Controls.Add(this.button_applyOpt);
            this.panel_opt.Controls.Add(this.button_saveCurrent);
            this.panel_opt.Location = new System.Drawing.Point(82, 0);
            this.panel_opt.Name = "panel_opt";
            this.panel_opt.Size = new System.Drawing.Size(513, 29);
            this.panel_opt.TabIndex = 59;
            // 
            // enumRadioControl1_GnssSolverType
            // 
            this.enumRadioControl1_GnssSolverType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.enumRadioControl1_GnssSolverType.IsReady = false;
            this.enumRadioControl1_GnssSolverType.Location = new System.Drawing.Point(0, 24);
            this.enumRadioControl1_GnssSolverType.Margin = new System.Windows.Forms.Padding(4);
            this.enumRadioControl1_GnssSolverType.Name = "enumRadioControl1_GnssSolverType";
            this.enumRadioControl1_GnssSolverType.Size = new System.Drawing.Size(537, 95);
            this.enumRadioControl1_GnssSolverType.TabIndex = 60;
            this.enumRadioControl1_GnssSolverType.Title = "选项";
            this.enumRadioControl1_GnssSolverType.EnumItemSelected += new System.Action<string, bool>(this.enumRadioControl1_EnumItemSelected);
            this.enumRadioControl1_GnssSolverType.Load += new System.EventHandler(this.enumRadioControl1_GnssSolverType_Load);
            // 
            // enumRadioControl_positionType
            // 
            this.enumRadioControl_positionType.IsReady = false;
            this.enumRadioControl_positionType.Location = new System.Drawing.Point(0, 34);
            this.enumRadioControl_positionType.Margin = new System.Windows.Forms.Padding(4);
            this.enumRadioControl_positionType.Name = "enumRadioControl_positionType";
            this.enumRadioControl_positionType.Size = new System.Drawing.Size(436, 54);
            this.enumRadioControl_positionType.TabIndex = 55;
            this.enumRadioControl_positionType.Title = "测站状态";
            this.enumRadioControl_positionType.EnumItemSelected += new System.Action<string, bool>(this.enumRadioControl_positionType_EnumItemSelected);
            // 
            // checkBox_clearCoords
            // 
            this.checkBox_clearCoords.AutoSize = true;
            this.checkBox_clearCoords.Location = new System.Drawing.Point(70, 9);
            this.checkBox_clearCoords.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_clearCoords.Name = "checkBox_clearCoords";
            this.checkBox_clearCoords.Size = new System.Drawing.Size(72, 16);
            this.checkBox_clearCoords.TabIndex = 48;
            this.checkBox_clearCoords.Text = "测站坐标";
            this.checkBox_clearCoords.UseVisualStyleBackColor = true;
            // 
            // ObsFileAdjustStreamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 458);
            this.IsShowProgressBar = true;
            this.Margin = new System.Windows.Forms.Padding(3);
            this.Name = "ObsFileAdjustStreamForm";
            this.Text = "观测文件平差数据流";
            this.Load += new System.EventHandler(this.ObsFileAdjustStreamForm_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel_buttonExtends.ResumeLayout(false);
            this.panel_buttonExtends.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.panel_buttons.ResumeLayout(false);
            this.panel_buttons.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel_opt.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_showOnMap;
        private System.Windows.Forms.Button button_drawDifferLine;
        private System.Windows.Forms.Button button_drawRmslines;
        private System.Windows.Forms.CheckBox checkBox_autoMatchingFile;
        private System.Windows.Forms.CheckBox checkBox_enableNet;
        protected Geo.Winform.Controls.ParamVectorRenderControl paramVectorRenderControl1;
        private System.Windows.Forms.CheckBox checkBox_IsOpenReportWhenCompleted;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_opt;
        private System.Windows.Forms.Button button_applyOpt;
        private System.Windows.Forms.Button button_saveCurrent;
        private System.Windows.Forms.Button button_optSaveAs;
        private System.Windows.Forms.Panel panel_opt;
        private Geo.Winform.EnumRadioControl enumRadioControl1_GnssSolverType;
        private Geo.Winform.EnumRadioControl enumRadioControl_positionType;
        private System.Windows.Forms.CheckBox checkBox_clearCoords;
    }
}