namespace Gnsser.Winform
{
    partial class SatelliteSelectorForm
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
            this.namedFloatControl1AngleCut = new Geo.Winform.Controls.NamedFloatControl();
            this.checkBoxIsExpandPeriod = new System.Windows.Forms.CheckBox();
            this.namedIntControl_timePeriodCount = new Geo.Winform.Controls.NamedIntControl();
            this.enumRadioControl1 = new Geo.Winform.EnumRadioControl();
            this.radioButton_fixed = new System.Windows.Forms.RadioButton();
            this.radioButton_unfixed = new System.Windows.Forms.RadioButton();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.timeLoopControl1 = new Geo.Winform.Controls.TimeLoopControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_coordSet = new System.Windows.Forms.Button();
            this.namedStringControl_coord = new Geo.Winform.Controls.NamedStringControl();
            this.checkBox_inputCoord = new System.Windows.Forms.CheckBox();
            this.panel4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel_buttons.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage0
            // 
            this.tabPage0.Size = new System.Drawing.Size(875, 148);
            // 
            // panel3
            // 
            this.panel3.Size = new System.Drawing.Size(869, 123);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.radioButton_unfixed);
            this.panel4.Controls.Add(this.radioButton_fixed);
            this.panel4.Controls.Add(this.tabControl1);
            this.panel4.Size = new System.Drawing.Size(869, 123);
            this.panel4.Controls.SetChildIndex(this.tabControl1, 0);
            this.panel4.Controls.SetChildIndex(this.radioButton_fixed, 0);
            this.panel4.Controls.SetChildIndex(this.radioButton_unfixed, 0);
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl_inputPathes.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileOpenControl_inputPathes.FilePathes = new string[0];
            this.fileOpenControl_inputPathes.IsMultiSelect = false;
            this.fileOpenControl_inputPathes.LabelName = "观测文件或卫星高度角文件：";
            this.fileOpenControl_inputPathes.Size = new System.Drawing.Size(869, 24);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.namedFloatControl1AngleCut);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Size = new System.Drawing.Size(875, 129);
            this.tabPage1.Controls.SetChildIndex(this.fileOpenControl_inputPathes, 0);
            this.tabPage1.Controls.SetChildIndex(this.groupBox2, 0);
            this.tabPage1.Controls.SetChildIndex(this.namedFloatControl1AngleCut, 0);
            // 
            // panel_buttons
            // 
            this.panel_buttons.Location = new System.Drawing.Point(616, 0);
            // 
            // namedFloatControl1AngleCut
            // 
            this.namedFloatControl1AngleCut.Location = new System.Drawing.Point(672, 41);
            this.namedFloatControl1AngleCut.Name = "namedFloatControl1AngleCut";
            this.namedFloatControl1AngleCut.Size = new System.Drawing.Size(146, 23);
            this.namedFloatControl1AngleCut.TabIndex = 70;
            this.namedFloatControl1AngleCut.Title = "高度截止角(度)：";
            this.namedFloatControl1AngleCut.Value = 5D;
            // 
            // checkBoxIsExpandPeriod
            // 
            this.checkBoxIsExpandPeriod.AutoSize = true;
            this.checkBoxIsExpandPeriod.Location = new System.Drawing.Point(186, 45);
            this.checkBoxIsExpandPeriod.Name = "checkBoxIsExpandPeriod";
            this.checkBoxIsExpandPeriod.Size = new System.Drawing.Size(180, 16);
            this.checkBoxIsExpandPeriod.TabIndex = 69;
            this.checkBoxIsExpandPeriod.Text = "是否扩展相同卫星编号的时段";
            this.checkBoxIsExpandPeriod.UseVisualStyleBackColor = true;
            // 
            // namedIntControl_timePeriodCount
            // 
            this.namedIntControl_timePeriodCount.Location = new System.Drawing.Point(14, 45);
            this.namedIntControl_timePeriodCount.Name = "namedIntControl_timePeriodCount";
            this.namedIntControl_timePeriodCount.Size = new System.Drawing.Size(146, 23);
            this.namedIntControl_timePeriodCount.TabIndex = 68;
            this.namedIntControl_timePeriodCount.Title = "固定时段分段数：";
            this.namedIntControl_timePeriodCount.Value = 12;
            // 
            // enumRadioControl1
            // 
            this.enumRadioControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.enumRadioControl1.Location = new System.Drawing.Point(3, 3);
            this.enumRadioControl1.Name = "enumRadioControl1";
            this.enumRadioControl1.Size = new System.Drawing.Size(590, 38);
            this.enumRadioControl1.TabIndex = 72;
            this.enumRadioControl1.Title = "固定时段选星器类型";
            // 
            // radioButton_fixed
            // 
            this.radioButton_fixed.AutoSize = true;
            this.radioButton_fixed.Checked = true;
            this.radioButton_fixed.Location = new System.Drawing.Point(145, 15);
            this.radioButton_fixed.Name = "radioButton_fixed";
            this.radioButton_fixed.Size = new System.Drawing.Size(71, 16);
            this.radioButton_fixed.TabIndex = 73;
            this.radioButton_fixed.TabStop = true;
            this.radioButton_fixed.Text = "固定时段";
            this.radioButton_fixed.UseVisualStyleBackColor = true;
            this.radioButton_fixed.CheckedChanged += new System.EventHandler(this.radioButton_fixed_CheckedChanged);
            // 
            // radioButton_unfixed
            // 
            this.radioButton_unfixed.AutoSize = true;
            this.radioButton_unfixed.Location = new System.Drawing.Point(222, 15);
            this.radioButton_unfixed.Name = "radioButton_unfixed";
            this.radioButton_unfixed.Size = new System.Drawing.Size(71, 16);
            this.radioButton_unfixed.TabIndex = 74;
            this.radioButton_unfixed.Text = "灵活分段";
            this.radioButton_unfixed.UseVisualStyleBackColor = true;
            this.radioButton_unfixed.CheckedChanged += new System.EventHandler(this.radioButton_unfixed_CheckedChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.namedIntControl_timePeriodCount);
            this.tabPage4.Controls.Add(this.checkBoxIsExpandPeriod);
            this.tabPage4.Controls.Add(this.enumRadioControl1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(596, 66);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "固定时段设置";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(5, 28);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(604, 92);
            this.tabControl1.TabIndex = 75;
            // 
            // timeLoopControl1
            // 
            this.timeLoopControl1.Location = new System.Drawing.Point(72, 15);
            this.timeLoopControl1.Margin = new System.Windows.Forms.Padding(2);
            this.timeLoopControl1.Name = "timeLoopControl1";
            this.timeLoopControl1.Size = new System.Drawing.Size(578, 30);
            this.timeLoopControl1.TabIndex = 70;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.button_coordSet);
            this.groupBox2.Controls.Add(this.namedStringControl_coord);
            this.groupBox2.Controls.Add(this.checkBox_inputCoord);
            this.groupBox2.Controls.Add(this.timeLoopControl1);
            this.groupBox2.Location = new System.Drawing.Point(8, 26);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(644, 74);
            this.groupBox2.TabIndex = 71;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "或手动指定测站坐标和采样率";
            // 
            // button_coordSet
            // 
            this.button_coordSet.Location = new System.Drawing.Point(513, 45);
            this.button_coordSet.Name = "button_coordSet";
            this.button_coordSet.Size = new System.Drawing.Size(75, 23);
            this.button_coordSet.TabIndex = 73;
            this.button_coordSet.Text = "设置坐标";
            this.button_coordSet.UseVisualStyleBackColor = true;
            this.button_coordSet.Click += new System.EventHandler(this.button_coordSet_Click);
            // 
            // namedStringControl_coord
            // 
            this.namedStringControl_coord.Location = new System.Drawing.Point(14, 45);
            this.namedStringControl_coord.Name = "namedStringControl_coord";
            this.namedStringControl_coord.Size = new System.Drawing.Size(493, 23);
            this.namedStringControl_coord.TabIndex = 72;
            this.namedStringControl_coord.Title = "测站坐标(XYZ)：";
            // 
            // checkBox_inputCoord
            // 
            this.checkBox_inputCoord.AutoSize = true;
            this.checkBox_inputCoord.Location = new System.Drawing.Point(14, 17);
            this.checkBox_inputCoord.Name = "checkBox_inputCoord";
            this.checkBox_inputCoord.Size = new System.Drawing.Size(48, 16);
            this.checkBox_inputCoord.TabIndex = 71;
            this.checkBox_inputCoord.Text = "启用";
            this.checkBox_inputCoord.UseVisualStyleBackColor = true;
            this.checkBox_inputCoord.CheckedChanged += new System.EventHandler(this.checkBox_inputCoord_CheckedChanged);
            // 
            // SatelliteSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 405);
            this.IsShowProgressBar = true;
            this.Name = "SatelliteSelectorForm";
            this.Text = "时段基准卫星选择器";
            this.Load += new System.EventHandler(this.WideLaneSolverForm_Load);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.panel_buttons.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Geo.Winform.Controls.NamedFloatControl namedFloatControl1AngleCut;
        private System.Windows.Forms.CheckBox checkBoxIsExpandPeriod;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_timePeriodCount;
        private Geo.Winform.EnumRadioControl enumRadioControl1;
        private System.Windows.Forms.RadioButton radioButton_unfixed;
        private System.Windows.Forms.RadioButton radioButton_fixed;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox_inputCoord;
        private Geo.Winform.Controls.TimeLoopControl timeLoopControl1;
        private System.Windows.Forms.Button button_coordSet;
        private Geo.Winform.Controls.NamedStringControl namedStringControl_coord;
    }
}