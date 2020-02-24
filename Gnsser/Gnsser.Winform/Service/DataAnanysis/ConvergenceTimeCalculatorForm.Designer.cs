namespace Gnsser.Winform
{
    partial class ConvergenceTimeCalculatorForm
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
            this.namedIntControl_epochCount = new Geo.Winform.Controls.NamedIntControl();
            this.namedFloatControl_maxDiffer = new Geo.Winform.Controls.NamedFloatControl();
            this.namedStringControl_paramNames = new Geo.Winform.Controls.NamedStringControl();
            this.namedIntControl_labelCharCount = new Geo.Winform.Controls.NamedIntControl();
            this.namedFloatControl_maxAllowConvergTime = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl1MaxAllowedDifferAfterConvergence = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_maxAllowedRms = new Geo.Winform.Controls.NamedFloatControl();
            this.panel4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel_buttons.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage0
            // 
            this.tabPage0.Location = new System.Drawing.Point(4, 22);
            this.tabPage0.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.tabPage0.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.tabPage0.Size = new System.Drawing.Size(762, 179);
            // 
            // panel3
            // 
            this.panel3.Location = new System.Drawing.Point(2, 2);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.panel3.Size = new System.Drawing.Size(647, 122);
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(2, 2);
            this.panel2.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.panel2.Size = new System.Drawing.Size(647, 122);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.namedStringControl_paramNames);
            this.panel4.Controls.Add(this.namedFloatControl1MaxAllowedDifferAfterConvergence);
            this.panel4.Controls.Add(this.namedFloatControl_maxAllowConvergTime);
            this.panel4.Controls.Add(this.namedFloatControl_maxAllowedRms);
            this.panel4.Controls.Add(this.namedFloatControl_maxDiffer);
            this.panel4.Location = new System.Drawing.Point(2, 2);
            this.panel4.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.panel4.Size = new System.Drawing.Size(758, 122);
            this.panel4.Controls.SetChildIndex(this.panel5, 0);
            this.panel4.Controls.SetChildIndex(this.namedFloatControl_maxDiffer, 0);
            this.panel4.Controls.SetChildIndex(this.namedFloatControl_maxAllowedRms, 0);
            this.panel4.Controls.SetChildIndex(this.namedFloatControl_maxAllowConvergTime, 0);
            this.panel4.Controls.SetChildIndex(this.namedFloatControl1MaxAllowedDifferAfterConvergence, 0);
            this.panel4.Controls.SetChildIndex(this.namedStringControl_paramNames, 0);
            // 
            // panel_buttonExtends
            // 
            this.panel_buttonExtends.Location = new System.Drawing.Point(0, 43);
            // 
            // fileOpenControl_inputPathes
            // 
            this.fileOpenControl_inputPathes.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileOpenControl_inputPathes.FilePathes = new string[0];
            this.fileOpenControl_inputPathes.Location = new System.Drawing.Point(5, 5);
            this.fileOpenControl_inputPathes.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.fileOpenControl_inputPathes.Size = new System.Drawing.Size(752, 93);
            this.fileOpenControl_inputPathes.FilePathSetted += new System.EventHandler(this.fileOpenControl1_FilePathSetted);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.tabPage1.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.tabPage1.Size = new System.Drawing.Size(762, 126);
            // 
            // panel_buttons
            // 
            this.panel_buttons.Location = new System.Drawing.Point(460, 0);
            this.panel_buttons.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.namedIntControl_epochCount);
            this.panel5.Controls.Add(this.namedIntControl_labelCharCount);
            this.panel5.Size = new System.Drawing.Size(517, 24);
            this.panel5.Controls.SetChildIndex(this.namedIntControl_labelCharCount, 0);
            this.panel5.Controls.SetChildIndex(this.namedIntControl_epochCount, 0);
            // 
            // namedIntControl_epochCount
            // 
            this.namedIntControl_epochCount.Location = new System.Drawing.Point(257, 4);
            this.namedIntControl_epochCount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.namedIntControl_epochCount.Name = "namedIntControl_epochCount";
            this.namedIntControl_epochCount.Size = new System.Drawing.Size(165, 23);
            this.namedIntControl_epochCount.TabIndex = 67;
            this.namedIntControl_epochCount.Title = "比较历元数量：";
            this.namedIntControl_epochCount.Value = 20;
            // 
            // namedFloatControl_maxDiffer
            // 
            this.namedFloatControl_maxDiffer.Location = new System.Drawing.Point(4, 30);
            this.namedFloatControl_maxDiffer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.namedFloatControl_maxDiffer.Name = "namedFloatControl_maxDiffer";
            this.namedFloatControl_maxDiffer.Size = new System.Drawing.Size(140, 23);
            this.namedFloatControl_maxDiffer.TabIndex = 69;
            this.namedFloatControl_maxDiffer.Title = "最大允许偏差：";
            this.namedFloatControl_maxDiffer.Value = 0.1D;
            // 
            // namedStringControl_paramNames
            // 
            this.namedStringControl_paramNames.Location = new System.Drawing.Point(3, 72);
            this.namedStringControl_paramNames.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.namedStringControl_paramNames.Name = "namedStringControl_paramNames";
            this.namedStringControl_paramNames.Size = new System.Drawing.Size(504, 23);
            this.namedStringControl_paramNames.TabIndex = 70;
            this.namedStringControl_paramNames.Title = "待统计参数名称：";
            // 
            // namedIntControl_labelCharCount
            // 
            this.namedIntControl_labelCharCount.Location = new System.Drawing.Point(85, 4);
            this.namedIntControl_labelCharCount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.namedIntControl_labelCharCount.Name = "namedIntControl_labelCharCount";
            this.namedIntControl_labelCharCount.Size = new System.Drawing.Size(165, 23);
            this.namedIntControl_labelCharCount.TabIndex = 67;
            this.namedIntControl_labelCharCount.Title = "显示字符数量：";
            this.namedIntControl_labelCharCount.Value = 4;
            // 
            // namedFloatControl_maxAllowConvergTime
            // 
            this.namedFloatControl_maxAllowConvergTime.Location = new System.Drawing.Point(152, 30);
            this.namedFloatControl_maxAllowConvergTime.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.namedFloatControl_maxAllowConvergTime.Name = "namedFloatControl_maxAllowConvergTime";
            this.namedFloatControl_maxAllowConvergTime.Size = new System.Drawing.Size(183, 23);
            this.namedFloatControl_maxAllowConvergTime.TabIndex = 69;
            this.namedFloatControl_maxAllowConvergTime.Title = "最大允许收敛时间(分)：";
            this.namedFloatControl_maxAllowConvergTime.Value = 240D;
            // 
            // namedFloatControl1MaxAllowedDifferAfterConvergence
            // 
            this.namedFloatControl1MaxAllowedDifferAfterConvergence.Location = new System.Drawing.Point(343, 30);
            this.namedFloatControl1MaxAllowedDifferAfterConvergence.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.namedFloatControl1MaxAllowedDifferAfterConvergence.Name = "namedFloatControl1MaxAllowedDifferAfterConvergence";
            this.namedFloatControl1MaxAllowedDifferAfterConvergence.Size = new System.Drawing.Size(183, 23);
            this.namedFloatControl1MaxAllowedDifferAfterConvergence.TabIndex = 69;
            this.namedFloatControl1MaxAllowedDifferAfterConvergence.Title = "收敛后允许最大偏差：";
            this.namedFloatControl1MaxAllowedDifferAfterConvergence.Value = 0.25D;
            // 
            // namedFloatControl_maxAllowedRms
            // 
            this.namedFloatControl_maxAllowedRms.Location = new System.Drawing.Point(377, 54);
            this.namedFloatControl_maxAllowedRms.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.namedFloatControl_maxAllowedRms.Name = "namedFloatControl_maxAllowedRms";
            this.namedFloatControl_maxAllowedRms.Size = new System.Drawing.Size(140, 23);
            this.namedFloatControl_maxAllowedRms.TabIndex = 69;
            this.namedFloatControl_maxAllowedRms.Title = "最大允许RMS：";
            this.namedFloatControl_maxAllowedRms.Value = 0.05D;
            // 
            // ConvergenceTimeCalculatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 436);
            this.IsShowProgressBar = true;
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "ConvergenceTimeCalculatorForm";
            this.Text = "收敛数据统计器";
            this.Load += new System.EventHandler(this.ConvergenceTimeCalculatorForm_Load);
            this.panel4.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel_buttons.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Geo.Winform.Controls.NamedIntControl namedIntControl_epochCount;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_maxDiffer;
        private Geo.Winform.Controls.NamedStringControl namedStringControl_paramNames;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_labelCharCount;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_maxAllowConvergTime;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl1MaxAllowedDifferAfterConvergence;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_maxAllowedRms;
    }
}