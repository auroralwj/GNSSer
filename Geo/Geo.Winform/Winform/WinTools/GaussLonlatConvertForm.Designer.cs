namespace Geo
{
    partial class GaussLonlatConvertForm
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
            this.checkBox_indicated = new System.Windows.Forms.CheckBox();
            this.checkBox_isWithBeltNum = new System.Windows.Forms.CheckBox();
            this.checkBox_is3Belt = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBox_IsOutSplitByTab = new System.Windows.Forms.CheckBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.button_lonlatToGauss = new System.Windows.Forms.Button();
            this.button_gaussToLonlat = new System.Windows.Forms.Button();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.namedFloatControl_orinalLonDeg = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_aveGeoHeight = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControlYConst = new Geo.Winform.Controls.NamedFloatControl();
            this.ellipsoidSelectControl1 = new Geo.Winform.EllipsoidSelectControl();
            this.enumRadioControl_angleUnit = new Geo.Winform.EnumRadioControl();
            this.richTextBoxControl_gauss = new Geo.Winform.Controls.RichTextBoxControl();
            this.richTextBoxControl_lonlat = new Geo.Winform.Controls.RichTextBoxControl();
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
            this.tabPage1.Controls.Add(this.namedFloatControl_orinalLonDeg);
            this.tabPage1.Controls.Add(this.namedFloatControl_aveGeoHeight);
            this.tabPage1.Controls.Add(this.namedFloatControlYConst);
            this.tabPage1.Controls.Add(this.checkBox_indicated);
            this.tabPage1.Controls.Add(this.checkBox_isWithBeltNum);
            this.tabPage1.Controls.Add(this.checkBox_is3Belt);
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
            // checkBox_indicated
            // 
            this.checkBox_indicated.AutoSize = true;
            this.checkBox_indicated.Location = new System.Drawing.Point(339, 86);
            this.checkBox_indicated.Name = "checkBox_indicated";
            this.checkBox_indicated.Size = new System.Drawing.Size(108, 16);
            this.checkBox_indicated.TabIndex = 2;
            this.checkBox_indicated.Text = "指定中央子午线";
            this.checkBox_indicated.UseVisualStyleBackColor = true;
            // 
            // checkBox_isWithBeltNum
            // 
            this.checkBox_isWithBeltNum.AutoSize = true;
            this.checkBox_isWithBeltNum.Location = new System.Drawing.Point(8, 88);
            this.checkBox_isWithBeltNum.Name = "checkBox_isWithBeltNum";
            this.checkBox_isWithBeltNum.Size = new System.Drawing.Size(78, 16);
            this.checkBox_isWithBeltNum.TabIndex = 2;
            this.checkBox_isWithBeltNum.Text = "Y轴加带号";
            this.checkBox_isWithBeltNum.UseVisualStyleBackColor = true;
            // 
            // checkBox_is3Belt
            // 
            this.checkBox_is3Belt.AutoSize = true;
            this.checkBox_is3Belt.Checked = true;
            this.checkBox_is3Belt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_is3Belt.Location = new System.Drawing.Point(8, 61);
            this.checkBox_is3Belt.Name = "checkBox_is3Belt";
            this.checkBox_is3Belt.Size = new System.Drawing.Size(132, 16);
            this.checkBox_is3Belt.TabIndex = 2;
            this.checkBox_is3Belt.Text = "三度带，否则六度带";
            this.checkBox_is3Belt.UseVisualStyleBackColor = true;
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
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(364, 374);
            this.tabControl2.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.richTextBoxControl_gauss);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(356, 348);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "高斯坐标";
            this.tabPage3.UseVisualStyleBackColor = true;
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
            this.splitContainer3.Panel1.Controls.Add(this.button_lonlatToGauss);
            this.splitContainer3.Panel1.Controls.Add(this.button_gaussToLonlat);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.tabControl3);
            this.splitContainer3.Size = new System.Drawing.Size(591, 374);
            this.splitContainer3.SplitterDistance = 121;
            this.splitContainer3.TabIndex = 0;
            // 
            // button_lonlatToGauss
            // 
            this.button_lonlatToGauss.Location = new System.Drawing.Point(24, 94);
            this.button_lonlatToGauss.Name = "button_lonlatToGauss";
            this.button_lonlatToGauss.Size = new System.Drawing.Size(75, 23);
            this.button_lonlatToGauss.TabIndex = 1;
            this.button_lonlatToGauss.Text = "<-";
            this.button_lonlatToGauss.UseVisualStyleBackColor = true;
            this.button_lonlatToGauss.Click += new System.EventHandler(this.button_lonlatToGauss_Click);
            // 
            // button_gaussToLonlat
            // 
            this.button_gaussToLonlat.Location = new System.Drawing.Point(24, 28);
            this.button_gaussToLonlat.Name = "button_gaussToLonlat";
            this.button_gaussToLonlat.Size = new System.Drawing.Size(75, 23);
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
            this.tabPage4.Text = "经纬度";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // namedFloatControl_orinalLonDeg
            // 
            this.namedFloatControl_orinalLonDeg.Location = new System.Drawing.Point(119, 86);
            this.namedFloatControl_orinalLonDeg.Name = "namedFloatControl_orinalLonDeg";
            this.namedFloatControl_orinalLonDeg.Size = new System.Drawing.Size(214, 23);
            this.namedFloatControl_orinalLonDeg.TabIndex = 3;
            this.namedFloatControl_orinalLonDeg.Title = "中央子午线(度小数)：";
            this.namedFloatControl_orinalLonDeg.Value = 99.5D;
            // 
            // namedFloatControl_aveGeoHeight
            // 
            this.namedFloatControl_aveGeoHeight.Location = new System.Drawing.Point(338, 61);
            this.namedFloatControl_aveGeoHeight.Name = "namedFloatControl_aveGeoHeight";
            this.namedFloatControl_aveGeoHeight.Size = new System.Drawing.Size(171, 23);
            this.namedFloatControl_aveGeoHeight.TabIndex = 3;
            this.namedFloatControl_aveGeoHeight.Title = "投影面大地高(m)：";
            this.namedFloatControl_aveGeoHeight.Value = 1500D;
            // 
            // namedFloatControlYConst
            // 
            this.namedFloatControlYConst.Location = new System.Drawing.Point(162, 61);
            this.namedFloatControlYConst.Name = "namedFloatControlYConst";
            this.namedFloatControlYConst.Size = new System.Drawing.Size(171, 23);
            this.namedFloatControlYConst.TabIndex = 3;
            this.namedFloatControlYConst.Title = "横轴Y加常数：";
            this.namedFloatControlYConst.Value = 500000D;
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
            // richTextBoxControl_gauss
            // 
            this.richTextBoxControl_gauss.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_gauss.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_gauss.MaxAppendLineCount = 5000;
            this.richTextBoxControl_gauss.Name = "richTextBoxControl_gauss";
            this.richTextBoxControl_gauss.Size = new System.Drawing.Size(350, 342);
            this.richTextBoxControl_gauss.TabIndex = 0;
            this.richTextBoxControl_gauss.Text = "2799263.391, 484933.975";
            // 
            // richTextBoxControl_lonlat
            // 
            this.richTextBoxControl_lonlat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_lonlat.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_lonlat.MaxAppendLineCount = 5000;
            this.richTextBoxControl_lonlat.Name = "richTextBoxControl_lonlat";
            this.richTextBoxControl_lonlat.Size = new System.Drawing.Size(452, 342);
            this.richTextBoxControl_lonlat.TabIndex = 0;
            this.richTextBoxControl_lonlat.Text = "120, 30";
            // 
            // GaussLonlatConvertForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 519);
            this.Controls.Add(this.splitContainer1);
            this.Name = "GaussLonlatConvertForm";
            this.Text = "GaussLonlatConvertForm";
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
        private Winform.Controls.RichTextBoxControl richTextBoxControl_gauss;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Button button_lonlatToGauss;
        private System.Windows.Forms.Button button_gaussToLonlat;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage4;
        private Winform.Controls.RichTextBoxControl richTextBoxControl_lonlat;
        private Winform.EnumRadioControl enumRadioControl_angleUnit;
        private Winform.EllipsoidSelectControl ellipsoidSelectControl1;
        private System.Windows.Forms.CheckBox checkBox_is3Belt;
        private Winform.Controls.NamedFloatControl namedFloatControlYConst;
        private Winform.Controls.NamedFloatControl namedFloatControl_orinalLonDeg;
        private System.Windows.Forms.CheckBox checkBox_isWithBeltNum;
        private System.Windows.Forms.CheckBox checkBox_indicated;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox checkBox_IsOutSplitByTab;
        private Winform.Controls.NamedFloatControl namedFloatControl_aveGeoHeight;
    }
}