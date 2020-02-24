namespace Geo
{
    partial class SeriesSettingForm
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
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.enumRadioControl_AxisTypeOfX = new Geo.Winform.EnumRadioControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.enumRadioControl_XValueType = new Geo.Winform.EnumRadioControl();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.enumRadioControl2SeriesChartType = new Geo.Winform.EnumRadioControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.enumRadioControl_YValueType = new Geo.Winform.EnumRadioControl();
            this.enumRadioControl_AxisTypeOfY = new Geo.Winform.EnumRadioControl();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox2BorderWidth = new System.Windows.Forms.TextBox();
            this.textBoxMarkerSize = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button_apply = new System.Windows.Forms.Button();
            this.colorSelectControl1 = new Geo.Winform.ColorSelectControl();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_cancel.Location = new System.Drawing.Point(494, 324);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 0;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ok.Location = new System.Drawing.Point(413, 324);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 0;
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(0, -1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(582, 319);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox5);
            this.tabPage1.Controls.Add(this.enumRadioControl_AxisTypeOfY);
            this.tabPage1.Controls.Add(this.enumRadioControl_AxisTypeOfX);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(574, 293);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "设置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // enumRadioControl_AxisTypeOfX
            // 
            this.enumRadioControl_AxisTypeOfX.Location = new System.Drawing.Point(10, 157);
            this.enumRadioControl_AxisTypeOfX.Name = "enumRadioControl_AxisTypeOfX";
            this.enumRadioControl_AxisTypeOfX.Size = new System.Drawing.Size(166, 119);
            this.enumRadioControl_AxisTypeOfX.TabIndex = 32;
            this.enumRadioControl_AxisTypeOfX.Title = "X 轴类型";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(574, 293);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "数据类型";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // enumRadioControl_XValueType
            // 
            this.enumRadioControl_XValueType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.enumRadioControl_XValueType.Location = new System.Drawing.Point(0, 0);
            this.enumRadioControl_XValueType.Name = "enumRadioControl_XValueType";
            this.enumRadioControl_XValueType.Size = new System.Drawing.Size(291, 287);
            this.enumRadioControl_XValueType.TabIndex = 0;
            this.enumRadioControl_XValueType.Title = "选项";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.enumRadioControl2SeriesChartType);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(574, 293);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "图类型";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // enumRadioControl2SeriesChartType
            // 
            this.enumRadioControl2SeriesChartType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.enumRadioControl2SeriesChartType.Location = new System.Drawing.Point(3, 3);
            this.enumRadioControl2SeriesChartType.Name = "enumRadioControl2SeriesChartType";
            this.enumRadioControl2SeriesChartType.Size = new System.Drawing.Size(568, 287);
            this.enumRadioControl2SeriesChartType.TabIndex = 1;
            this.enumRadioControl2SeriesChartType.Title = "选项";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.enumRadioControl_XValueType);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.enumRadioControl_YValueType);
            this.splitContainer1.Size = new System.Drawing.Size(568, 287);
            this.splitContainer1.SplitterDistance = 291;
            this.splitContainer1.TabIndex = 1;
            // 
            // enumRadioControl_YValueType
            // 
            this.enumRadioControl_YValueType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.enumRadioControl_YValueType.Location = new System.Drawing.Point(0, 0);
            this.enumRadioControl_YValueType.Name = "enumRadioControl_YValueType";
            this.enumRadioControl_YValueType.Size = new System.Drawing.Size(273, 287);
            this.enumRadioControl_YValueType.TabIndex = 1;
            this.enumRadioControl_YValueType.Title = "选项";
            // 
            // enumRadioControl_AxisTypeOfY
            // 
            this.enumRadioControl_AxisTypeOfY.Location = new System.Drawing.Point(184, 157);
            this.enumRadioControl_AxisTypeOfY.Name = "enumRadioControl_AxisTypeOfY";
            this.enumRadioControl_AxisTypeOfY.Size = new System.Drawing.Size(166, 119);
            this.enumRadioControl_AxisTypeOfY.TabIndex = 32;
            this.enumRadioControl_AxisTypeOfY.Title = "Y 轴类型";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.colorSelectControl1);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.textBox2BorderWidth);
            this.groupBox5.Controls.Add(this.textBoxMarkerSize);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Location = new System.Drawing.Point(10, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(506, 70);
            this.groupBox5.TabIndex = 57;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "绘图符号";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 34);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 50;
            this.label6.Text = "符号大小：";
            // 
            // textBox2BorderWidth
            // 
            this.textBox2BorderWidth.Location = new System.Drawing.Point(159, 30);
            this.textBox2BorderWidth.Margin = new System.Windows.Forms.Padding(2);
            this.textBox2BorderWidth.Name = "textBox2BorderWidth";
            this.textBox2BorderWidth.Size = new System.Drawing.Size(34, 21);
            this.textBox2BorderWidth.TabIndex = 49;
            this.textBox2BorderWidth.Text = "3";
            // 
            // textBoxMarkerSize
            // 
            this.textBoxMarkerSize.Location = new System.Drawing.Point(76, 30);
            this.textBoxMarkerSize.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxMarkerSize.Name = "textBoxMarkerSize";
            this.textBoxMarkerSize.Size = new System.Drawing.Size(32, 21);
            this.textBoxMarkerSize.TabIndex = 48;
            this.textBoxMarkerSize.Text = "5";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(114, 34);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 51;
            this.label5.Text = "边宽：";
            // 
            // button_apply
            // 
            this.button_apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_apply.Location = new System.Drawing.Point(320, 324);
            this.button_apply.Name = "button_apply";
            this.button_apply.Size = new System.Drawing.Size(75, 23);
            this.button_apply.TabIndex = 2;
            this.button_apply.Text = "应用";
            this.button_apply.UseVisualStyleBackColor = true;
            this.button_apply.Click += new System.EventHandler(this.button_apply_Click);
            // 
            // colorSelectControl1
            // 
            this.colorSelectControl1.Location = new System.Drawing.Point(216, 30);
            this.colorSelectControl1.Name = "colorSelectControl1";
            this.colorSelectControl1.Size = new System.Drawing.Size(244, 28);
            this.colorSelectControl1.TabIndex = 52;
            // 
            // SeriesSettingForm
            // 
            this.AcceptButton = this.button_ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_cancel;
            this.ClientSize = new System.Drawing.Size(581, 359);
            this.Controls.Add(this.button_apply);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.button_cancel);
            this.Name = "SeriesSettingForm";
            this.Text = "序列设置";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.TabPage tabPage2;
        private Winform.EnumRadioControl enumRadioControl_XValueType;
        private Winform.EnumRadioControl enumRadioControl_AxisTypeOfX;
        private System.Windows.Forms.TabPage tabPage3;
        private Winform.EnumRadioControl enumRadioControl2SeriesChartType;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Winform.EnumRadioControl enumRadioControl_YValueType;
        private Winform.EnumRadioControl enumRadioControl_AxisTypeOfY;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox2BorderWidth;
        private System.Windows.Forms.TextBox textBoxMarkerSize;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_apply;
        private Winform.ColorSelectControl colorSelectControl1;
    }
}