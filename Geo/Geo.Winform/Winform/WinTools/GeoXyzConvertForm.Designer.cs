namespace Geo.WinTools
{
    partial class GeoXyzConvertForm
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
            this.components = new System.ComponentModel.Container();
            this.label5 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBox_xyz = new Geo.Winform.Controls.RichTextBoxControl();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.button_geoToxyz = new System.Windows.Forms.Button();
            this.button_xyzTogeo = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBox_geo = new Geo.Winform.Controls.RichTextBoxControl();
            this.enumRadioControl_angleUnit = new Geo.Winform.EnumRadioControl();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.ellipsoidSelectControl1 = new Geo.Winform.EllipsoidSelectControl();
            this.checkBox_IsOutSplitByTab = new System.Windows.Forms.CheckBox();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(225, 103);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(287, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "坐标分量之间的分隔符为：英文逗号\",\"或制表符\"\\t\"";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(791, 366);
            this.splitContainer1.SplitterDistance = 330;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(330, 366);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBox_xyz);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(322, 340);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "空间直角坐标";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBox_xyz
            // 
            this.textBox_xyz.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_xyz.Location = new System.Drawing.Point(3, 3);
            this.textBox_xyz.MaxAppendLineCount = 5000;
            this.textBox_xyz.Name = "textBox_xyz";
            this.textBox_xyz.Size = new System.Drawing.Size(316, 334);
            this.textBox_xyz.TabIndex = 0;
            this.textBox_xyz.Text = "100000,100000,100000";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.button_geoToxyz);
            this.splitContainer2.Panel1.Controls.Add(this.button_xyzTogeo);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControl2);
            this.splitContainer2.Size = new System.Drawing.Size(458, 366);
            this.splitContainer2.SplitterDistance = 109;
            this.splitContainer2.SplitterWidth = 3;
            this.splitContainer2.TabIndex = 0;
            // 
            // button_geoToxyz
            // 
            this.button_geoToxyz.Location = new System.Drawing.Point(17, 146);
            this.button_geoToxyz.Margin = new System.Windows.Forms.Padding(2);
            this.button_geoToxyz.Name = "button_geoToxyz";
            this.button_geoToxyz.Size = new System.Drawing.Size(67, 34);
            this.button_geoToxyz.TabIndex = 3;
            this.button_geoToxyz.Text = "<<";
            this.button_geoToxyz.UseVisualStyleBackColor = true;
            this.button_geoToxyz.Click += new System.EventHandler(this.button_geoToxyz_Click);
            // 
            // button_xyzTogeo
            // 
            this.button_xyzTogeo.Location = new System.Drawing.Point(17, 70);
            this.button_xyzTogeo.Margin = new System.Windows.Forms.Padding(2);
            this.button_xyzTogeo.Name = "button_xyzTogeo";
            this.button_xyzTogeo.Size = new System.Drawing.Size(67, 34);
            this.button_xyzTogeo.TabIndex = 2;
            this.button_xyzTogeo.Text = ">>";
            this.button_xyzTogeo.UseVisualStyleBackColor = true;
            this.button_xyzTogeo.Click += new System.EventHandler(this.button_xyzTogeo_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(346, 366);
            this.tabControl2.TabIndex = 0;
            this.tabControl2.SelectedIndexChanged += new System.EventHandler(this.tabControl2_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textBox_geo);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(338, 340);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "大地坐标";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBox_geo
            // 
            this.textBox_geo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_geo.Location = new System.Drawing.Point(3, 3);
            this.textBox_geo.MaxAppendLineCount = 5000;
            this.textBox_geo.Name = "textBox_geo";
            this.textBox_geo.Size = new System.Drawing.Size(332, 334);
            this.textBox_geo.TabIndex = 0;
            this.textBox_geo.Text = "120,30,100";
            // 
            // enumRadioControl_angleUnit
            // 
            this.enumRadioControl_angleUnit.IsReady = false;
            this.enumRadioControl_angleUnit.Location = new System.Drawing.Point(-1, 2);
            this.enumRadioControl_angleUnit.Name = "enumRadioControl_angleUnit";
            this.enumRadioControl_angleUnit.Size = new System.Drawing.Size(307, 93);
            this.enumRadioControl_angleUnit.TabIndex = 6;
            this.enumRadioControl_angleUnit.Title = "角度单位";
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.tabControl3);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainerMain.Size = new System.Drawing.Size(791, 520);
            this.splitContainerMain.SplitterDistance = 150;
            this.splitContainerMain.TabIndex = 8;
            // 
            // ellipsoidSelectControl1
            // 
            this.ellipsoidSelectControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ellipsoidSelectControl1.Location = new System.Drawing.Point(312, 3);
            this.ellipsoidSelectControl1.Name = "ellipsoidSelectControl1";
            this.ellipsoidSelectControl1.Size = new System.Drawing.Size(524, 93);
            this.ellipsoidSelectControl1.TabIndex = 7;
            // 
            // checkBox_IsOutSplitByTab
            // 
            this.checkBox_IsOutSplitByTab.AutoSize = true;
            this.checkBox_IsOutSplitByTab.Checked = true;
            this.checkBox_IsOutSplitByTab.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_IsOutSplitByTab.Location = new System.Drawing.Point(14, 103);
            this.checkBox_IsOutSplitByTab.Name = "checkBox_IsOutSplitByTab";
            this.checkBox_IsOutSplitByTab.Size = new System.Drawing.Size(192, 16);
            this.checkBox_IsOutSplitByTab.TabIndex = 8;
            this.checkBox_IsOutSplitByTab.Text = "输出间隔为制表符，否则为逗号";
            this.checkBox_IsOutSplitByTab.UseVisualStyleBackColor = true;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage3);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(0, 0);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(791, 150);
            this.tabControl3.TabIndex = 9;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.checkBox_IsOutSplitByTab);
            this.tabPage3.Controls.Add(this.ellipsoidSelectControl1);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.enumRadioControl_angleUnit);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(783, 124);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "设置";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // GeoXyzConvertForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 520);
            this.Controls.Add(this.splitContainerMain);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "GeoXyzConvertForm";
            this.Text = "空间直角坐标与大地坐标的转换";
            this.Load += new System.EventHandler(this.GeoXyzConvertForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button button_geoToxyz;
        private System.Windows.Forms.Button button_xyzTogeo;
        private System.Windows.Forms.Label label5;
        private Winform.EllipsoidSelectControl ellipsoidSelectControl1;
        private Winform.EnumRadioControl enumRadioControl_angleUnit;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Winform.Controls.RichTextBoxControl textBox_xyz;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage2;
        private Winform.Controls.RichTextBoxControl textBox_geo;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.CheckBox checkBox_IsOutSplitByTab;
    }
}