namespace Geo.WinTools
{
    partial class FileExecutingForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_ignoreFirstCol = new System.Windows.Forms.CheckBox();
            this.checkBox_ignoreFirstRow = new System.Windows.Forms.CheckBox();
            this.fileOutputControl1 = new Geo.Winform.Controls.FileOutputControl();
            this.fileOpenControl1 = new Geo.Winform.Controls.FileOpenControl();
            this.button_run = new System.Windows.Forms.Button();
            this.label_info = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkBox_ignoreFirstCol);
            this.groupBox1.Controls.Add(this.checkBox_ignoreFirstRow);
            this.groupBox1.Controls.Add(this.fileOutputControl1);
            this.groupBox1.Controls.Add(this.fileOpenControl1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(478, 111);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息";
            // 
            // checkBox_ignoreFirstCol
            // 
            this.checkBox_ignoreFirstCol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_ignoreFirstCol.AutoSize = true;
            this.checkBox_ignoreFirstCol.Location = new System.Drawing.Point(135, 49);
            this.checkBox_ignoreFirstCol.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBox_ignoreFirstCol.Name = "checkBox_ignoreFirstCol";
            this.checkBox_ignoreFirstCol.Size = new System.Drawing.Size(72, 16);
            this.checkBox_ignoreFirstCol.TabIndex = 11;
            this.checkBox_ignoreFirstCol.Text = "忽略首列";
            this.checkBox_ignoreFirstCol.UseVisualStyleBackColor = true;
            // 
            // checkBox_ignoreFirstRow
            // 
            this.checkBox_ignoreFirstRow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_ignoreFirstRow.AutoSize = true;
            this.checkBox_ignoreFirstRow.Location = new System.Drawing.Point(64, 49);
            this.checkBox_ignoreFirstRow.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBox_ignoreFirstRow.Name = "checkBox_ignoreFirstRow";
            this.checkBox_ignoreFirstRow.Size = new System.Drawing.Size(72, 16);
            this.checkBox_ignoreFirstRow.TabIndex = 12;
            this.checkBox_ignoreFirstRow.Text = "忽略首行";
            this.checkBox_ignoreFirstRow.UseVisualStyleBackColor = true;
            // 
            // fileOutputControl1
            // 
            this.fileOutputControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOutputControl1.FilePath = "";
            this.fileOutputControl1.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOutputControl1.LabelName = "输出文件：";
            this.fileOutputControl1.Location = new System.Drawing.Point(8, 71);
            this.fileOutputControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.fileOutputControl1.Name = "fileOutputControl1";
            this.fileOutputControl1.Size = new System.Drawing.Size(464, 22);
            this.fileOutputControl1.TabIndex = 1;
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl1.AllowDrop = true;
            this.fileOpenControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl1.FilePath = "";
            this.fileOpenControl1.FilePathes = new string[0];
            this.fileOpenControl1.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl1.FirstPath = "";
            this.fileOpenControl1.IsMultiSelect = false;
            this.fileOpenControl1.LabelName = "输入文件：";
            this.fileOpenControl1.Location = new System.Drawing.Point(9, 20);
            this.fileOpenControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.fileOpenControl1.Name = "fileOpenControl1";
            this.fileOpenControl1.Size = new System.Drawing.Size(464, 22);
            this.fileOpenControl1.TabIndex = 0;
            this.fileOpenControl1.FilePathSetted += new System.EventHandler(this.fileOpenControl1_FilePathSetted);
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(415, 130);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 23);
            this.button_run.TabIndex = 1;
            this.button_run.Text = "执行";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // label_info
            // 
            this.label_info.AutoSize = true;
            this.label_info.Location = new System.Drawing.Point(12, 130);
            this.label_info.Name = "label_info";
            this.label_info.Size = new System.Drawing.Size(53, 12);
            this.label_info.TabIndex = 2;
            this.label_info.Text = "执行情况";
            // 
            // FileExecutingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 163);
            this.Controls.Add(this.label_info);
            this.Controls.Add(this.button_run);
            this.Controls.Add(this.groupBox1);
            this.Name = "FileExecutingForm";
            this.Text = "文件执行";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Winform.Controls.FileOpenControl fileOpenControl1;
        private Winform.Controls.FileOutputControl fileOutputControl1;
        private System.Windows.Forms.Button button_run;
        private System.Windows.Forms.Label label_info;
        private System.Windows.Forms.CheckBox checkBox_ignoreFirstCol;
        private System.Windows.Forms.CheckBox checkBox_ignoreFirstRow;
    }
}