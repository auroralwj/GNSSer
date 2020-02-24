namespace Gnsser.Winform
{
    partial class ClockPredictionBasedonClockFileExportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClockPredictionBasedonClockFileExportForm));
            this.openFileDialog_nav = new System.Windows.Forms.OpenFileDialog();
            this.button_export = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button_getPath = new System.Windows.Forms.Button();
            this.textBox_Pathes = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_satPrns = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button_predictedresult = new System.Windows.Forms.Button();
            this.textBox_predictedresultPath = new System.Windows.Forms.TextBox();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.openFileDialog_predictedresult = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog_nav
            // 
            this.openFileDialog_nav.Filter = "Rinex导航、星历文件|*.*N;*.*R;*.*G;*.SP3|所有文件|*.*";
            this.openFileDialog_nav.Multiselect = true;
            this.openFileDialog_nav.Title = "请选择文件";
            // 
            // button_export
            // 
            this.button_export.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_export.Location = new System.Drawing.Point(723, 370);
            this.button_export.Name = "button_export";
            this.button_export.Size = new System.Drawing.Size(75, 23);
            this.button_export.TabIndex = 31;
            this.button_export.Text = "导出";
            this.button_export.UseVisualStyleBackColor = true;
            this.button_export.Click += new System.EventHandler(this.button_export_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 17;
            this.label1.Text = "导航文件：";
            // 
            // button_getPath
            // 
            this.button_getPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getPath.Location = new System.Drawing.Point(736, 11);
            this.button_getPath.Name = "button_getPath";
            this.button_getPath.Size = new System.Drawing.Size(50, 47);
            this.button_getPath.TabIndex = 16;
            this.button_getPath.Text = "...";
            this.button_getPath.UseVisualStyleBackColor = true;
            this.button_getPath.Click += new System.EventHandler(this.button_getPath_Click);
            // 
            // textBox_Pathes
            // 
            this.textBox_Pathes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Pathes.Location = new System.Drawing.Point(131, 13);
            this.textBox_Pathes.Multiline = true;
            this.textBox_Pathes.Name = "textBox_Pathes";
            this.textBox_Pathes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Pathes.Size = new System.Drawing.Size(589, 45);
            this.textBox_Pathes.TabIndex = 18;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_satPrns);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.button_predictedresult);
            this.groupBox1.Controls.Add(this.textBox_predictedresultPath);
            this.groupBox1.Controls.Add(this.directorySelectionControl1);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button_getPath);
            this.groupBox1.Controls.Add(this.textBox_Pathes);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(801, 367);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 202);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 25;
            this.label2.Text = "待导出卫星：";
            // 
            // textBox_satPrns
            // 
            this.textBox_satPrns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_satPrns.Location = new System.Drawing.Point(131, 195);
            this.textBox_satPrns.Multiline = true;
            this.textBox_satPrns.Name = "textBox_satPrns";
            this.textBox_satPrns.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_satPrns.Size = new System.Drawing.Size(589, 82);
            this.textBox_satPrns.TabIndex = 26;
            this.textBox_satPrns.Text = resources.GetString("textBox_satPrns.Text");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 108);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 12);
            this.label5.TabIndex = 23;
            this.label5.Text = "钟差预报结果文件：";
            // 
            // button_predictedresult
            // 
            this.button_predictedresult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_predictedresult.Location = new System.Drawing.Point(736, 103);
            this.button_predictedresult.Name = "button_predictedresult";
            this.button_predictedresult.Size = new System.Drawing.Size(50, 81);
            this.button_predictedresult.TabIndex = 22;
            this.button_predictedresult.Text = "...";
            this.button_predictedresult.UseVisualStyleBackColor = true;
            this.button_predictedresult.Click += new System.EventHandler(this.button_predictedresult_Click);
            // 
            // textBox_predictedresultPath
            // 
            this.textBox_predictedresultPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_predictedresultPath.Location = new System.Drawing.Point(131, 105);
            this.textBox_predictedresultPath.Multiline = true;
            this.textBox_predictedresultPath.Name = "textBox_predictedresultPath";
            this.textBox_predictedresultPath.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_predictedresultPath.Size = new System.Drawing.Size(589, 79);
            this.textBox_predictedresultPath.TabIndex = 24;
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "输出目录：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(66, 297);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "D:\\Temp";
            this.directorySelectionControl1.Pathes = new string[] {
        "D:\\Temp"};
            this.directorySelectionControl1.Size = new System.Drawing.Size(720, 22);
            this.directorySelectionControl1.TabIndex = 21;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(647, 64);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(126, 16);
            this.checkBox1.TabIndex = 19;
            this.checkBox1.Text = "若无数据，以0填充";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // openFileDialog_predictedresult
            // 
            this.openFileDialog_predictedresult.FileName = "openFileDialog1";
            this.openFileDialog_predictedresult.Filter = "钟差预报结果文件|*.*xls;*.*xlsx|所有文件|*.*";
            // 
            // ClockPredictionBasedonEphemerisExportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 445);
            this.Controls.Add(this.button_export);
            this.Controls.Add(this.groupBox1);
            this.Name = "ClockPredictionBasedonEphemerisExportForm";
            this.Text = "生成Clk文件的钟差预报";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog_nav;
        private System.Windows.Forms.Button button_export;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_getPath;
        private System.Windows.Forms.TextBox textBox_Pathes;
        private System.Windows.Forms.GroupBox groupBox1;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_predictedresult;
        private System.Windows.Forms.TextBox textBox_predictedresultPath;
        private System.Windows.Forms.OpenFileDialog openFileDialog_predictedresult;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_satPrns;
    }
}