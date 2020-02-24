namespace Gnsser.Winform
{
    partial class FileNameExtractForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.namedIntControlcharCount = new Geo.Winform.Controls.NamedIntControl();
            this.namedIntControl_startIndex = new Geo.Winform.Controls.NamedIntControl();
            this.fileOpenControl1 = new Geo.Winform.Controls.FileOpenControl();
            this.button_ok = new System.Windows.Forms.Button();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.richTextBoxControl1 = new Geo.Winform.Controls.RichTextBoxControl();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.namedIntControlcharCount);
            this.groupBox1.Controls.Add(this.namedIntControl_startIndex);
            this.groupBox1.Controls.Add(this.fileOpenControl1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(580, 149);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选项";
            // 
            // namedIntControlcharCount
            // 
            this.namedIntControlcharCount.Location = new System.Drawing.Point(191, 114);
            this.namedIntControlcharCount.Name = "namedIntControlcharCount";
            this.namedIntControlcharCount.Size = new System.Drawing.Size(119, 23);
            this.namedIntControlcharCount.TabIndex = 7;
            this.namedIntControlcharCount.Title = "字符数量：";
            this.namedIntControlcharCount.Value = 4;
            // 
            // namedIntControl_startIndex
            // 
            this.namedIntControl_startIndex.Location = new System.Drawing.Point(38, 114);
            this.namedIntControl_startIndex.Name = "namedIntControl_startIndex";
            this.namedIntControl_startIndex.Size = new System.Drawing.Size(119, 23);
            this.namedIntControl_startIndex.TabIndex = 7;
            this.namedIntControl_startIndex.Title = "起始编号：";
            this.namedIntControl_startIndex.Value = 0;
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
            this.fileOpenControl1.IsMultiSelect = true;
            this.fileOpenControl1.LabelName = "文件：";
            this.fileOpenControl1.Location = new System.Drawing.Point(6, 20);
            this.fileOpenControl1.Name = "fileOpenControl1";
            this.fileOpenControl1.Size = new System.Drawing.Size(561, 88);
            this.fileOpenControl1.TabIndex = 6;
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ok.Location = new System.Drawing.Point(513, 183);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 38);
            this.button_ok.TabIndex = 1;
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.IsBackwardProcess = false;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(31, 186);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(471, 34);
            this.progressBarComponent1.TabIndex = 2;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // richTextBoxControl1
            // 
            this.richTextBoxControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxControl1.Location = new System.Drawing.Point(13, 249);
            this.richTextBoxControl1.MaxAppendLineCount = 5000;
            this.richTextBoxControl1.Name = "richTextBoxControl1";
            this.richTextBoxControl1.Size = new System.Drawing.Size(575, 235);
            this.richTextBoxControl1.TabIndex = 3;
            this.richTextBoxControl1.Text = "";
            // 
            // FileNameExtractForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 496);
            this.Controls.Add(this.richTextBoxControl1);
            this.Controls.Add(this.progressBarComponent1);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.groupBox1);
            this.Name = "FileNameExtractForm";
            this.Text = "文件名称提取器";
            this.Load += new System.EventHandler(this.MoveFileByKeyForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_ok;
        private Geo.Winform.Controls.ProgressBarComponent progressBarComponent1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl1;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl1;
        private Geo.Winform.Controls.NamedIntControl namedIntControlcharCount;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_startIndex;
    }
}