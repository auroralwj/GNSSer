namespace Geo.Winform.Controls
{
    partial class ParamVectorRenderControl
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage_basic = new System.Windows.Forms.TabPage();
            this.enabledFloatSpanControl_filterVal = new Geo.Winform.Controls.EnabledFloatSpanControl();
            this.textBox_differ = new System.Windows.Forms.TextBox();
            this.textBox_startEpoch = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.textBox_Count = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage_drawStyle = new System.Windows.Forms.TabPage();
            this.checkBox_autoYSpan = new System.Windows.Forms.CheckBox();
            this.enabledFloatControl_upMargin = new Geo.Winform.Controls.EnabledFloatControl();
            this.enabledFloatControl_downMargin = new Geo.Winform.Controls.EnabledFloatControl();
            this.enabledFloatSpanControl_YSpan = new Geo.Winform.Controls.EnabledFloatSpanControl();
            this.checkBox_isMaxWindow = new System.Windows.Forms.CheckBox();
            this.textBoxMarkerSize = new System.Windows.Forms.TextBox();
            this.textBox2BorderWidth = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox_chartType = new System.Windows.Forms.ComboBox();
            this.checkBox_isTakeXAsTime = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage_paramCount = new System.Windows.Forms.TabPage();
            this.textBox_paramCount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_startParamIndex = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage_ManualSelect = new System.Windows.Forms.TabPage();
            this.arrayCheckBoxControl1 = new Geo.Winform.ArrayCheckBoxControl();
            this.tabControl.SuspendLayout();
            this.tabPage_basic.SuspendLayout();
            this.tabPage_drawStyle.SuspendLayout();
            this.tabPage_paramCount.SuspendLayout();
            this.tabPage_ManualSelect.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage_basic);
            this.tabControl.Controls.Add(this.tabPage_drawStyle);
            this.tabControl.Controls.Add(this.tabPage_paramCount);
            this.tabControl.Controls.Add(this.tabPage_ManualSelect);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(562, 109);
            this.tabControl.TabIndex = 39;
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl_Selected);
            // 
            // tabPage_basic
            // 
            this.tabPage_basic.Controls.Add(this.enabledFloatSpanControl_filterVal);
            this.tabPage_basic.Controls.Add(this.textBox_differ);
            this.tabPage_basic.Controls.Add(this.textBox_startEpoch);
            this.tabPage_basic.Controls.Add(this.label15);
            this.tabPage_basic.Controls.Add(this.textBox_Count);
            this.tabPage_basic.Controls.Add(this.label14);
            this.tabPage_basic.Controls.Add(this.label1);
            this.tabPage_basic.Location = new System.Drawing.Point(4, 22);
            this.tabPage_basic.Name = "tabPage_basic";
            this.tabPage_basic.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_basic.Size = new System.Drawing.Size(554, 83);
            this.tabPage_basic.TabIndex = 0;
            this.tabPage_basic.Text = "绘图基本设置";
            this.tabPage_basic.UseVisualStyleBackColor = true;
            // 
            // enabledFloatSpanControl_filterVal
            // 
            this.enabledFloatSpanControl_filterVal.From = 0.1D;
            this.enabledFloatSpanControl_filterVal.Location = new System.Drawing.Point(227, 32);
            this.enabledFloatSpanControl_filterVal.Name = "enabledFloatSpanControl_filterVal";
            this.enabledFloatSpanControl_filterVal.Size = new System.Drawing.Size(231, 24);
            this.enabledFloatSpanControl_filterVal.TabIndex = 41;
            this.enabledFloatSpanControl_filterVal.Title = "数据过滤：";
            this.enabledFloatSpanControl_filterVal.To = 0.1D;
            // 
            // textBox_differ
            // 
            this.textBox_differ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_differ.Location = new System.Drawing.Point(82, 5);
            this.textBox_differ.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_differ.Name = "textBox_differ";
            this.textBox_differ.Size = new System.Drawing.Size(467, 21);
            this.textBox_differ.TabIndex = 38;
            this.textBox_differ.Text = "0, 0, 0, 0, 0, 0, 0";
            // 
            // textBox_startEpoch
            // 
            this.textBox_startEpoch.Location = new System.Drawing.Point(81, 32);
            this.textBox_startEpoch.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_startEpoch.Name = "textBox_startEpoch";
            this.textBox_startEpoch.Size = new System.Drawing.Size(32, 21);
            this.textBox_startEpoch.TabIndex = 35;
            this.textBox_startEpoch.Text = "0";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(0, 7);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(77, 12);
            this.label15.TabIndex = 37;
            this.label15.Text = "参数中心值：";
            // 
            // textBox_Count
            // 
            this.textBox_Count.Location = new System.Drawing.Point(164, 32);
            this.textBox_Count.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_Count.Name = "textBox_Count";
            this.textBox_Count.Size = new System.Drawing.Size(48, 21);
            this.textBox_Count.TabIndex = 35;
            this.textBox_Count.Text = "100000";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(10, 36);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 12);
            this.label14.TabIndex = 36;
            this.label14.Text = "起始历元：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(119, 36);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 36;
            this.label1.Text = "数量：";
            // 
            // tabPage_drawStyle
            // 
            this.tabPage_drawStyle.Controls.Add(this.checkBox_autoYSpan);
            this.tabPage_drawStyle.Controls.Add(this.enabledFloatControl_upMargin);
            this.tabPage_drawStyle.Controls.Add(this.enabledFloatControl_downMargin);
            this.tabPage_drawStyle.Controls.Add(this.enabledFloatSpanControl_YSpan);
            this.tabPage_drawStyle.Controls.Add(this.checkBox_isMaxWindow);
            this.tabPage_drawStyle.Controls.Add(this.textBoxMarkerSize);
            this.tabPage_drawStyle.Controls.Add(this.textBox2BorderWidth);
            this.tabPage_drawStyle.Controls.Add(this.label5);
            this.tabPage_drawStyle.Controls.Add(this.label6);
            this.tabPage_drawStyle.Controls.Add(this.comboBox_chartType);
            this.tabPage_drawStyle.Controls.Add(this.checkBox_isTakeXAsTime);
            this.tabPage_drawStyle.Controls.Add(this.label4);
            this.tabPage_drawStyle.Location = new System.Drawing.Point(4, 22);
            this.tabPage_drawStyle.Name = "tabPage_drawStyle";
            this.tabPage_drawStyle.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_drawStyle.Size = new System.Drawing.Size(554, 83);
            this.tabPage_drawStyle.TabIndex = 3;
            this.tabPage_drawStyle.Text = "样式设置";
            this.tabPage_drawStyle.UseVisualStyleBackColor = true;
            // 
            // checkBox_autoYSpan
            // 
            this.checkBox_autoYSpan.AutoSize = true;
            this.checkBox_autoYSpan.Checked = true;
            this.checkBox_autoYSpan.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_autoYSpan.Location = new System.Drawing.Point(337, 33);
            this.checkBox_autoYSpan.Name = "checkBox_autoYSpan";
            this.checkBox_autoYSpan.Size = new System.Drawing.Size(192, 16);
            this.checkBox_autoYSpan.TabIndex = 51;
            this.checkBox_autoYSpan.Text = "自动 Y 轴边界，可显示 0 刻度";
            this.checkBox_autoYSpan.UseVisualStyleBackColor = true;
            // 
            // enabledFloatControl_upMargin
            // 
            this.enabledFloatControl_upMargin.Location = new System.Drawing.Point(416, 61);
            this.enabledFloatControl_upMargin.Name = "enabledFloatControl_upMargin";
            this.enabledFloatControl_upMargin.Size = new System.Drawing.Size(132, 23);
            this.enabledFloatControl_upMargin.TabIndex = 50;
            this.enabledFloatControl_upMargin.Title = "上边界：";
            this.enabledFloatControl_upMargin.Value = 0.2D;
            // 
            // enabledFloatControl_downMargin
            // 
            this.enabledFloatControl_downMargin.Location = new System.Drawing.Point(260, 60);
            this.enabledFloatControl_downMargin.Name = "enabledFloatControl_downMargin";
            this.enabledFloatControl_downMargin.Size = new System.Drawing.Size(132, 23);
            this.enabledFloatControl_downMargin.TabIndex = 50;
            this.enabledFloatControl_downMargin.Title = "下边界：";
            this.enabledFloatControl_downMargin.Value = 0.2D;
            // 
            // enabledFloatSpanControl_YSpan
            // 
            this.enabledFloatSpanControl_YSpan.From = -1D;
            this.enabledFloatSpanControl_YSpan.Location = new System.Drawing.Point(10, 59);
            this.enabledFloatSpanControl_YSpan.Name = "enabledFloatSpanControl_YSpan";
            this.enabledFloatSpanControl_YSpan.Size = new System.Drawing.Size(244, 24);
            this.enabledFloatSpanControl_YSpan.TabIndex = 49;
            this.enabledFloatSpanControl_YSpan.Title = "窗口数值范围:";
            this.enabledFloatSpanControl_YSpan.To = 1D;
            // 
            // checkBox_isMaxWindow
            // 
            this.checkBox_isMaxWindow.AutoSize = true;
            this.checkBox_isMaxWindow.Location = new System.Drawing.Point(223, 33);
            this.checkBox_isMaxWindow.Name = "checkBox_isMaxWindow";
            this.checkBox_isMaxWindow.Size = new System.Drawing.Size(108, 16);
            this.checkBox_isMaxWindow.TabIndex = 48;
            this.checkBox_isMaxWindow.Text = "最大化绘图窗口";
            this.checkBox_isMaxWindow.UseVisualStyleBackColor = true;
            // 
            // textBoxMarkerSize
            // 
            this.textBoxMarkerSize.Location = new System.Drawing.Point(79, 33);
            this.textBoxMarkerSize.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxMarkerSize.Name = "textBoxMarkerSize";
            this.textBoxMarkerSize.Size = new System.Drawing.Size(32, 21);
            this.textBoxMarkerSize.TabIndex = 44;
            this.textBoxMarkerSize.Text = "5";
            // 
            // textBox2BorderWidth
            // 
            this.textBox2BorderWidth.Location = new System.Drawing.Point(162, 33);
            this.textBox2BorderWidth.Margin = new System.Windows.Forms.Padding(2);
            this.textBox2BorderWidth.Name = "textBox2BorderWidth";
            this.textBox2BorderWidth.Size = new System.Drawing.Size(34, 21);
            this.textBox2BorderWidth.TabIndex = 45;
            this.textBox2BorderWidth.Text = "3";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(117, 37);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 46;
            this.label5.Text = "边宽：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 37);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 47;
            this.label6.Text = "符号大小：";
            // 
            // comboBox_chartType
            // 
            this.comboBox_chartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_chartType.FormattingEnabled = true;
            this.comboBox_chartType.Location = new System.Drawing.Point(78, 6);
            this.comboBox_chartType.Name = "comboBox_chartType";
            this.comboBox_chartType.Size = new System.Drawing.Size(118, 20);
            this.comboBox_chartType.TabIndex = 43;
            this.comboBox_chartType.SelectedIndexChanged += new System.EventHandler(this.comboBox_chartType_SelectedIndexChanged);
            // 
            // checkBox_isTakeXAsTime
            // 
            this.checkBox_isTakeXAsTime.AutoSize = true;
            this.checkBox_isTakeXAsTime.Checked = true;
            this.checkBox_isTakeXAsTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_isTakeXAsTime.Location = new System.Drawing.Point(206, 10);
            this.checkBox_isTakeXAsTime.Name = "checkBox_isTakeXAsTime";
            this.checkBox_isTakeXAsTime.Size = new System.Drawing.Size(162, 16);
            this.checkBox_isTakeXAsTime.TabIndex = 42;
            this.checkBox_isTakeXAsTime.Text = "若X轴为时间，则以其分隔";
            this.checkBox_isTakeXAsTime.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 10);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 41;
            this.label4.Text = "图类型：";
            // 
            // tabPage_paramCount
            // 
            this.tabPage_paramCount.Controls.Add(this.textBox_paramCount);
            this.tabPage_paramCount.Controls.Add(this.label3);
            this.tabPage_paramCount.Controls.Add(this.textBox_startParamIndex);
            this.tabPage_paramCount.Controls.Add(this.label2);
            this.tabPage_paramCount.Location = new System.Drawing.Point(4, 22);
            this.tabPage_paramCount.Name = "tabPage_paramCount";
            this.tabPage_paramCount.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_paramCount.Size = new System.Drawing.Size(554, 83);
            this.tabPage_paramCount.TabIndex = 1;
            this.tabPage_paramCount.Text = "参数顺序选择";
            this.tabPage_paramCount.UseVisualStyleBackColor = true;
            // 
            // textBox_paramCount
            // 
            this.textBox_paramCount.Location = new System.Drawing.Point(334, 5);
            this.textBox_paramCount.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_paramCount.Name = "textBox_paramCount";
            this.textBox_paramCount.Size = new System.Drawing.Size(37, 21);
            this.textBox_paramCount.TabIndex = 35;
            this.textBox_paramCount.Text = "3";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 9);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(245, 12);
            this.label3.TabIndex = 36;
            this.label3.Text = "起始参数索引（从0开始,检索列自动跳过）：";
            // 
            // textBox_startParamIndex
            // 
            this.textBox_startParamIndex.Location = new System.Drawing.Point(248, 5);
            this.textBox_startParamIndex.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_startParamIndex.Name = "textBox_startParamIndex";
            this.textBox_startParamIndex.Size = new System.Drawing.Size(30, 21);
            this.textBox_startParamIndex.TabIndex = 35;
            this.textBox_startParamIndex.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(287, 9);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 36;
            this.label2.Text = "个数：";
            // 
            // tabPage_ManualSelect
            // 
            this.tabPage_ManualSelect.Controls.Add(this.arrayCheckBoxControl1);
            this.tabPage_ManualSelect.Location = new System.Drawing.Point(4, 22);
            this.tabPage_ManualSelect.Name = "tabPage_ManualSelect";
            this.tabPage_ManualSelect.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_ManualSelect.Size = new System.Drawing.Size(554, 83);
            this.tabPage_ManualSelect.TabIndex = 2;
            this.tabPage_ManualSelect.Text = "参数手动选择";
            this.tabPage_ManualSelect.UseVisualStyleBackColor = true;
            // 
            // arrayCheckBoxControl1
            // 
            this.arrayCheckBoxControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.arrayCheckBoxControl1.Location = new System.Drawing.Point(3, 3);
            this.arrayCheckBoxControl1.Name = "arrayCheckBoxControl1";
            this.arrayCheckBoxControl1.Size = new System.Drawing.Size(548, 77);
            this.arrayCheckBoxControl1.TabIndex = 0;
            this.arrayCheckBoxControl1.Title = "选项";
            // 
            // ParamVectorRenderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ParamVectorRenderControl";
            this.Size = new System.Drawing.Size(562, 109);
            this.tabControl.ResumeLayout(false);
            this.tabPage_basic.ResumeLayout(false);
            this.tabPage_basic.PerformLayout();
            this.tabPage_drawStyle.ResumeLayout(false);
            this.tabPage_drawStyle.PerformLayout();
            this.tabPage_paramCount.ResumeLayout(false);
            this.tabPage_paramCount.PerformLayout();
            this.tabPage_ManualSelect.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_differ;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox_Count;
        private System.Windows.Forms.TextBox textBox_startEpoch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_paramCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_startParamIndex;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage_basic;
        private System.Windows.Forms.TabPage tabPage_paramCount;
        private System.Windows.Forms.TabPage tabPage_ManualSelect;
        private ArrayCheckBoxControl arrayCheckBoxControl1;
        private EnabledFloatSpanControl enabledFloatSpanControl_filterVal;
        private System.Windows.Forms.TabPage tabPage_drawStyle;
        private System.Windows.Forms.TextBox textBoxMarkerSize;
        private System.Windows.Forms.TextBox textBox2BorderWidth;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBox_chartType;
        private System.Windows.Forms.CheckBox checkBox_isTakeXAsTime;
        private System.Windows.Forms.Label label4;
        private EnabledFloatSpanControl enabledFloatSpanControl_YSpan;
        private System.Windows.Forms.CheckBox checkBox_isMaxWindow;
        private EnabledFloatControl enabledFloatControl_upMargin;
        private EnabledFloatControl enabledFloatControl_downMargin;
        private System.Windows.Forms.CheckBox checkBox_autoYSpan;
    }
}
