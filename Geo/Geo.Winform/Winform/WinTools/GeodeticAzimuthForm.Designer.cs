namespace Geo
{
    partial class GeodeticAzimuthForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBox_isGeoOrXYz = new System.Windows.Forms.CheckBox();
            this.ellipsoidSelectControl1 = new Geo.Winform.EllipsoidSelectControl();
            this.enumRadioControl_angleUnit = new Geo.Winform.EnumRadioControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBox_IsOutSplitByTab = new System.Windows.Forms.CheckBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_starts = new Geo.Winform.Controls.RichTextBoxControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_ends = new Geo.Winform.Controls.RichTextBoxControl();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.button_gaussToLonlat = new System.Windows.Forms.Button();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_lonlat = new Geo.Winform.Controls.RichTextBoxControl();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(959, 519);
            this.splitContainer1.SplitterDistance = 141;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(959, 141);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.checkBox_isGeoOrXYz);
            this.tabPage1.Controls.Add(this.ellipsoidSelectControl1);
            this.tabPage1.Controls.Add(this.enumRadioControl_angleUnit);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(951, 115);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "设置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBox_isGeoOrXYz
            // 
            this.checkBox_isGeoOrXYz.AutoSize = true;
            this.checkBox_isGeoOrXYz.Checked = true;
            this.checkBox_isGeoOrXYz.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_isGeoOrXYz.Location = new System.Drawing.Point(19, 73);
            this.checkBox_isGeoOrXYz.Name = "checkBox_isGeoOrXYz";
            this.checkBox_isGeoOrXYz.Size = new System.Drawing.Size(222, 16);
            this.checkBox_isGeoOrXYz.TabIndex = 2;
            this.checkBox_isGeoOrXYz.Text = "输入为空间直角坐标,否则为大地坐标";
            this.checkBox_isGeoOrXYz.UseVisualStyleBackColor = true;
            // 
            // ellipsoidSelectControl1
            // 
            this.ellipsoidSelectControl1.Location = new System.Drawing.Point(515, 11);
            this.ellipsoidSelectControl1.Name = "ellipsoidSelectControl1";
            this.ellipsoidSelectControl1.Size = new System.Drawing.Size(432, 93);
            this.ellipsoidSelectControl1.TabIndex = 1;
            // 
            // enumRadioControl_angleUnit
            // 
            this.enumRadioControl_angleUnit.IsReady = false;
            this.enumRadioControl_angleUnit.Location = new System.Drawing.Point(8, 11);
            this.enumRadioControl_angleUnit.Name = "enumRadioControl_angleUnit";
            this.enumRadioControl_angleUnit.Size = new System.Drawing.Size(501, 44);
            this.enumRadioControl_angleUnit.TabIndex = 0;
            this.enumRadioControl_angleUnit.Title = "角度单位";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkBox_IsOutSplitByTab);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(951, 115);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "其它设置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsOutSplitByTab
            // 
            this.checkBox_IsOutSplitByTab.AutoSize = true;
            this.checkBox_IsOutSplitByTab.Checked = true;
            this.checkBox_IsOutSplitByTab.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_IsOutSplitByTab.Location = new System.Drawing.Point(24, 24);
            this.checkBox_IsOutSplitByTab.Name = "checkBox_IsOutSplitByTab";
            this.checkBox_IsOutSplitByTab.Size = new System.Drawing.Size(192, 16);
            this.checkBox_IsOutSplitByTab.TabIndex = 0;
            this.checkBox_IsOutSplitByTab.Text = "输出间隔为制表符，否则为逗号";
            this.checkBox_IsOutSplitByTab.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabControl2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(959, 374);
            this.splitContainer2.SplitterDistance = 364;
            this.splitContainer2.TabIndex = 0;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(364, 374);
            this.tabControl2.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.richTextBoxControl_starts);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(356, 348);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "起始坐标";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_starts
            // 
            this.richTextBoxControl_starts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_starts.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_starts.MaxAppendLineCount = 5000;
            this.richTextBoxControl_starts.Name = "richTextBoxControl_starts";
            this.richTextBoxControl_starts.Size = new System.Drawing.Size(350, 342);
            this.richTextBoxControl_starts.TabIndex = 0;
            this.richTextBoxControl_starts.Text = "2799263.391, 484933.975, 1111111";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.richTextBoxControl_ends);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(356, 348);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "结束坐标";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_ends
            // 
            this.richTextBoxControl_ends.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_ends.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_ends.MaxAppendLineCount = 5000;
            this.richTextBoxControl_ends.Name = "richTextBoxControl_ends";
            this.richTextBoxControl_ends.Size = new System.Drawing.Size(350, 342);
            this.richTextBoxControl_ends.TabIndex = 1;
            this.richTextBoxControl_ends.Text = "2799263.391, 484933.975, 11111112";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.button_gaussToLonlat);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.tabControl3);
            this.splitContainer3.Size = new System.Drawing.Size(591, 374);
            this.splitContainer3.SplitterDistance = 121;
            this.splitContainer3.TabIndex = 0;
            // 
            // button_gaussToLonlat
            // 
            this.button_gaussToLonlat.Location = new System.Drawing.Point(24, 28);
            this.button_gaussToLonlat.Name = "button_gaussToLonlat";
            this.button_gaussToLonlat.Size = new System.Drawing.Size(75, 37);
            this.button_gaussToLonlat.TabIndex = 0;
            this.button_gaussToLonlat.Text = "->";
            this.button_gaussToLonlat.UseVisualStyleBackColor = true;
            this.button_gaussToLonlat.Click += new System.EventHandler(this.button_gaussToLonlat_Click);
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage4);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(0, 0);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(466, 374);
            this.tabControl3.TabIndex = 2;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.richTextBoxControl_lonlat);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(458, 348);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "计算结果";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_lonlat
            // 
            this.richTextBoxControl_lonlat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_lonlat.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_lonlat.MaxAppendLineCount = 5000;
            this.richTextBoxControl_lonlat.Name = "richTextBoxControl_lonlat";
            this.richTextBoxControl_lonlat.Size = new System.Drawing.Size(452, 342);
            this.richTextBoxControl_lonlat.TabIndex = 0;
            this.richTextBoxControl_lonlat.Text = "10";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(273, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "暂时只能用XYZ，角度设置无效";
            // 
            // GeodeticAzimuthForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 519);
            this.Controls.Add(this.splitContainer1);
            this.Name = "GeodeticAzimuthForm";
            this.Text = "大地方位角计算";
            this.Load += new System.EventHandler(this.GaussLonlatConvertForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Winform.Controls.RichTextBoxControl richTextBoxControl_starts;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Button button_gaussToLonlat;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage4;
        private Winform.Controls.RichTextBoxControl richTextBoxControl_lonlat;
        private Winform.EnumRadioControl enumRadioControl_angleUnit;
        private Winform.EllipsoidSelectControl ellipsoidSelectControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox checkBox_IsOutSplitByTab;
        private System.Windows.Forms.TabPage tabPage5;
        private Winform.Controls.RichTextBoxControl richTextBoxControl_ends;
        private System.Windows.Forms.CheckBox checkBox_isGeoOrXYz;
        private System.Windows.Forms.Label label1;
    }
}