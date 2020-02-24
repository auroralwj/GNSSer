namespace Gnsser.Winform
{
    partial class CmdCallerForm
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
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.fileOpenControl1 = new Geo.Winform.Controls.FileOpenControl();
            this.richTextBoxControl1 = new Geo.Winform.Controls.RichTextBoxControl();
            this.button_run = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.directorySelectionControl1);
            this.groupBox1.Controls.Add(this.fileOpenControl1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(576, 116);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置";
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl1.IsMultiPathes = true;
            this.directorySelectionControl1.LabelName = "文件或目录：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(6, 49);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "";
            this.directorySelectionControl1.Pathes = new string[0];
            this.directorySelectionControl1.Size = new System.Drawing.Size(561, 53);
            this.directorySelectionControl1.TabIndex = 1;
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl1.AllowDrop = true;
            this.fileOpenControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl1.FilePath = "";
            this.fileOpenControl1.FilePathes = new string[0];
            this.fileOpenControl1.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl1.IsMultiSelect = false;
            this.fileOpenControl1.LabelName = "文件：";
            this.fileOpenControl1.Location = new System.Drawing.Point(20, 21);
            this.fileOpenControl1.Name = "fileOpenControl1";
            this.fileOpenControl1.Size = new System.Drawing.Size(547, 22);
            this.fileOpenControl1.TabIndex = 0;
            // 
            // richTextBoxControl1
            // 
            this.richTextBoxControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxControl1.Location = new System.Drawing.Point(12, 178);
            this.richTextBoxControl1.Name = "richTextBoxControl1";
            this.richTextBoxControl1.Size = new System.Drawing.Size(567, 127);
            this.richTextBoxControl1.TabIndex = 1;
            this.richTextBoxControl1.Text = "";
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(504, 134);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 38);
            this.button_run.TabIndex = 2;
            this.button_run.Text = "执行";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // CmdCallerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 317);
            this.Controls.Add(this.button_run);
            this.Controls.Add(this.richTextBoxControl1);
            this.Controls.Add(this.groupBox1);
            this.Name = "CmdCallerForm";
            this.Text = "CmdCallerForm";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl1;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl1;
        private System.Windows.Forms.Button button_run;
    }
}