namespace Gnsser.Winform
{
    partial class GnssNetAccuracyForm
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
            this.button_run = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_result = new Geo.Winform.Controls.RichTextBoxControl();
            this.namedFloatControl_distance = new Geo.Winform.Controls.NamedFloatControl();
            this.enumRadioControl_gnssGrade = new Geo.Winform.EnumRadioControl();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.enumRadioControl_gnssReceiverType = new Geo.Winform.EnumRadioControl();
            this.namedStringControl_reveicerVertion = new Geo.Winform.Controls.NamedStringControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.namedFloatControl_fixedErrorLevel = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_levelCoefOfProprotion = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_fixedErrorVertical = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_verticalCoefOfProprotion = new Geo.Winform.Controls.NamedFloatControl();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage2.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.tabControl2);
            this.splitContainer1.Panel1.Controls.Add(this.namedFloatControl_distance);
            this.splitContainer1.Panel1.Controls.Add(this.button_run);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(624, 458);
            this.splitContainer1.SplitterDistance = 161;
            this.splitContainer1.TabIndex = 0;
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(537, 119);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 23);
            this.button_run.TabIndex = 0;
            this.button_run.Text = "计算";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(624, 293);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextBoxControl_result);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(616, 267);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "结果";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_result
            // 
            this.richTextBoxControl_result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_result.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_result.MaxAppendLineCount = 5000;
            this.richTextBoxControl_result.Name = "richTextBoxControl_result";
            this.richTextBoxControl_result.Size = new System.Drawing.Size(610, 261);
            this.richTextBoxControl_result.TabIndex = 0;
            this.richTextBoxControl_result.Text = "";
            // 
            // namedFloatControl_distance
            // 
            this.namedFloatControl_distance.Location = new System.Drawing.Point(17, 119);
            this.namedFloatControl_distance.Name = "namedFloatControl_distance";
            this.namedFloatControl_distance.Size = new System.Drawing.Size(203, 23);
            this.namedFloatControl_distance.TabIndex = 1;
            this.namedFloatControl_distance.Title = "距离(m)：";
            this.namedFloatControl_distance.Value = 10000D;
            // 
            // enumRadioControl_gnssGrade
            // 
            this.enumRadioControl_gnssGrade.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.enumRadioControl_gnssGrade.Location = new System.Drawing.Point(0, 3);
            this.enumRadioControl_gnssGrade.Name = "enumRadioControl_gnssGrade";
            this.enumRadioControl_gnssGrade.Size = new System.Drawing.Size(500, 78);
            this.enumRadioControl_gnssGrade.TabIndex = 2;
            this.enumRadioControl_gnssGrade.Title = "GNSS等级";
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Location = new System.Drawing.Point(7, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(610, 113);
            this.tabControl2.TabIndex = 3;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.namedStringControl_reveicerVertion);
            this.tabPage3.Controls.Add(this.enumRadioControl_gnssReceiverType);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(602, 87);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "接收机标称精度";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.enumRadioControl_gnssGrade);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(602, 87);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "GNSS国标精度";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // enumRadioControl_gnssReceiverType
            // 
            this.enumRadioControl_gnssReceiverType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.enumRadioControl_gnssReceiverType.Location = new System.Drawing.Point(6, 6);
            this.enumRadioControl_gnssReceiverType.Name = "enumRadioControl_gnssReceiverType";
            this.enumRadioControl_gnssReceiverType.Size = new System.Drawing.Size(353, 75);
            this.enumRadioControl_gnssReceiverType.TabIndex = 3;
            this.enumRadioControl_gnssReceiverType.Title = "接收机类型";
            // 
            // namedStringControl_reveicerVertion
            // 
            this.namedStringControl_reveicerVertion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.namedStringControl_reveicerVertion.Location = new System.Drawing.Point(379, 6);
            this.namedStringControl_reveicerVertion.Name = "namedStringControl_reveicerVertion";
            this.namedStringControl_reveicerVertion.Size = new System.Drawing.Size(217, 23);
            this.namedStringControl_reveicerVertion.TabIndex = 4;
            this.namedStringControl_reveicerVertion.Title = "接收机类型(可选)：";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.namedFloatControl_verticalCoefOfProprotion);
            this.tabPage2.Controls.Add(this.namedFloatControl_fixedErrorVertical);
            this.tabPage2.Controls.Add(this.namedFloatControl_levelCoefOfProprotion);
            this.tabPage2.Controls.Add(this.namedFloatControl_fixedErrorLevel);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(602, 87);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "手动输入";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // namedFloatControl_fixedErrorLevel
            // 
            this.namedFloatControl_fixedErrorLevel.Location = new System.Drawing.Point(6, 6);
            this.namedFloatControl_fixedErrorLevel.Name = "namedFloatControl_fixedErrorLevel";
            this.namedFloatControl_fixedErrorLevel.Size = new System.Drawing.Size(203, 23);
            this.namedFloatControl_fixedErrorLevel.TabIndex = 1;
            this.namedFloatControl_fixedErrorLevel.Title = "水平固定误差(mm)：";
            this.namedFloatControl_fixedErrorLevel.Value = 5D;
            // 
            // namedFloatControl_levelCoefOfProprotion
            // 
            this.namedFloatControl_levelCoefOfProprotion.Location = new System.Drawing.Point(6, 35);
            this.namedFloatControl_levelCoefOfProprotion.Name = "namedFloatControl_levelCoefOfProprotion";
            this.namedFloatControl_levelCoefOfProprotion.Size = new System.Drawing.Size(203, 23);
            this.namedFloatControl_levelCoefOfProprotion.TabIndex = 1;
            this.namedFloatControl_levelCoefOfProprotion.Title = "水平比例系数(ppm)：";
            this.namedFloatControl_levelCoefOfProprotion.Value = 1D;
            // 
            // namedFloatControl_fixedErrorVertical
            // 
            this.namedFloatControl_fixedErrorVertical.Location = new System.Drawing.Point(225, 6);
            this.namedFloatControl_fixedErrorVertical.Name = "namedFloatControl_fixedErrorVertical";
            this.namedFloatControl_fixedErrorVertical.Size = new System.Drawing.Size(203, 23);
            this.namedFloatControl_fixedErrorVertical.TabIndex = 1;
            this.namedFloatControl_fixedErrorVertical.Title = "垂直固定误差(mm)：";
            this.namedFloatControl_fixedErrorVertical.Value = 10D;
            // 
            // namedFloatControl_verticalCoefOfProprotion
            // 
            this.namedFloatControl_verticalCoefOfProprotion.Location = new System.Drawing.Point(225, 35);
            this.namedFloatControl_verticalCoefOfProprotion.Name = "namedFloatControl_verticalCoefOfProprotion";
            this.namedFloatControl_verticalCoefOfProprotion.Size = new System.Drawing.Size(203, 23);
            this.namedFloatControl_verticalCoefOfProprotion.TabIndex = 1;
            this.namedFloatControl_verticalCoefOfProprotion.Title = "垂直比例系数(ppm)：";
            this.namedFloatControl_verticalCoefOfProprotion.Value = 1D;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "注意：ppm 为百万分之一";
            // 
            // GnssNetAccuracyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 458);
            this.Controls.Add(this.splitContainer1);
            this.Name = "GnssNetAccuracyForm";
            this.Text = "GNSS网精度查询";
            this.Load += new System.EventHandler(this.GnssNetAccuracyForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button button_run;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl_result;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_distance;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private Geo.Winform.EnumRadioControl enumRadioControl_gnssReceiverType;
        private System.Windows.Forms.TabPage tabPage4;
        private Geo.Winform.EnumRadioControl enumRadioControl_gnssGrade;
        private Geo.Winform.Controls.NamedStringControl namedStringControl_reveicerVertion;
        private System.Windows.Forms.TabPage tabPage2;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_fixedErrorLevel;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_levelCoefOfProprotion;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_verticalCoefOfProprotion;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_fixedErrorVertical;
        private System.Windows.Forms.Label label1;
    }
}