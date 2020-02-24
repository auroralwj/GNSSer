namespace Gnsser.Winform
{
    partial class AveragingXyzForm
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
            this.fileOutputControl1 = new Geo.Winform.Controls.FileOutputControl();
            this.fileOpenControlA = new Geo.Winform.Controls.FileOpenControl();
            this.button_run = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.fileOutputControl1);
            this.groupBox1.Controls.Add(this.fileOpenControlA);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(710, 127);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "输入文件";
            // 
            // fileOutputControl1
            // 
            this.fileOutputControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOutputControl1.FilePath = "";
            this.fileOutputControl1.Filter = "坐标文件(文本Excel文件)|*.xls|所有文件(*.*)|*.*";
            this.fileOutputControl1.LabelName = "输出：";
            this.fileOutputControl1.Location = new System.Drawing.Point(6, 94);
            this.fileOutputControl1.Name = "fileOutputControl1";
            this.fileOutputControl1.Size = new System.Drawing.Size(698, 22);
            this.fileOutputControl1.TabIndex = 22;
            // 
            // fileOpenControlA
            // 
            this.fileOpenControlA.AllowDrop = true;
            this.fileOpenControlA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControlA.FilePath = "";
            this.fileOpenControlA.FilePathes = new string[0];
            this.fileOpenControlA.Filter = "GNSSer坐标文件|*.*";
            this.fileOpenControlA.FirstPath = "";
            this.fileOpenControlA.IsMultiSelect = true;
            this.fileOpenControlA.LabelName = "输入：";
            this.fileOpenControlA.Location = new System.Drawing.Point(6, 20);
            this.fileOpenControlA.Name = "fileOpenControlA";
            this.fileOpenControlA.Size = new System.Drawing.Size(698, 68);
            this.fileOpenControlA.TabIndex = 21;
            this.fileOpenControlA.FilePathSetted += new System.EventHandler(this.fileOpenControlA_FilePathSetted);
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(641, 145);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 44);
            this.button_run.TabIndex = 1;
            this.button_run.Text = "执行";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 161);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(473, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "说明：当前只适用于GNSSerXYZ文件，分别以Name、X、Y、Z为列名制表位分隔的文本文件";
            // 
            // AveragingXyzForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 213);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_run);
            this.Controls.Add(this.groupBox1);
            this.Name = "AveragingXyzForm";
            this.Text = "坐标求平均";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Geo.Winform.Controls.FileOutputControl fileOutputControl1;
        private Geo.Winform.Controls.FileOpenControl fileOpenControlA;
        private System.Windows.Forms.Button button_run;
        private System.Windows.Forms.Label label1;
    }
}