namespace Geo
{
    partial class GifMakerForm
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
            this.button_run = new System.Windows.Forms.Button();
            this.checkBox_repeat = new System.Windows.Forms.CheckBox();
            this.fileOutputControl_outGif = new Geo.Winform.Controls.FileOutputControl();
            this.fileOpenControl_input = new Geo.Winform.Controls.FileOpenControl();
            this.namedIntControl_delayMs = new Geo.Winform.Controls.NamedIntControl();
            this.SuspendLayout();
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(475, 157);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 23);
            this.button_run.TabIndex = 1;
            this.button_run.Text = "运行";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // checkBox_repeat
            // 
            this.checkBox_repeat.AutoSize = true;
            this.checkBox_repeat.Checked = true;
            this.checkBox_repeat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_repeat.Location = new System.Drawing.Point(53, 93);
            this.checkBox_repeat.Name = "checkBox_repeat";
            this.checkBox_repeat.Size = new System.Drawing.Size(72, 16);
            this.checkBox_repeat.TabIndex = 3;
            this.checkBox_repeat.Text = "重复播放";
            this.checkBox_repeat.UseVisualStyleBackColor = true;
            // 
            // fileOutputControl_outGif
            // 
            this.fileOutputControl_outGif.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOutputControl_outGif.FilePath = "";
            this.fileOutputControl_outGif.Filter = "GIF文件|*.gif|所有文件|*.*";
            this.fileOutputControl_outGif.LabelName = "输出GIF路径：";
            this.fileOutputControl_outGif.Location = new System.Drawing.Point(12, 128);
            this.fileOutputControl_outGif.Name = "fileOutputControl_outGif";
            this.fileOutputControl_outGif.Size = new System.Drawing.Size(538, 22);
            this.fileOutputControl_outGif.TabIndex = 2;
            // 
            // fileOpenControl_input
            // 
            this.fileOpenControl_input.AllowDrop = true;
            this.fileOpenControl_input.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_input.FilePath = "";
            this.fileOpenControl_input.FilePathes = new string[0];
            this.fileOpenControl_input.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_input.FirstPath = "";
            this.fileOpenControl_input.IsMultiSelect = true;
            this.fileOpenControl_input.LabelName = "输入图片路径：";
            this.fileOpenControl_input.Location = new System.Drawing.Point(12, 12);
            this.fileOpenControl_input.Name = "fileOpenControl_input";
            this.fileOpenControl_input.Size = new System.Drawing.Size(538, 74);
            this.fileOpenControl_input.TabIndex = 0;
            this.fileOpenControl_input.FilePathSetted += new System.EventHandler(this.fileOpenControl1_FilePathSetted);
            // 
            // namedIntControl_delayMs
            // 
            this.namedIntControl_delayMs.Location = new System.Drawing.Point(140, 92);
            this.namedIntControl_delayMs.Name = "namedIntControl_delayMs";
            this.namedIntControl_delayMs.Size = new System.Drawing.Size(141, 23);
            this.namedIntControl_delayMs.TabIndex = 5;
            this.namedIntControl_delayMs.Title = "延迟（ms）：";
            this.namedIntControl_delayMs.Value = 500;
            // 
            // GifMakerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 191);
            this.Controls.Add(this.namedIntControl_delayMs);
            this.Controls.Add(this.checkBox_repeat);
            this.Controls.Add(this.fileOutputControl_outGif);
            this.Controls.Add(this.button_run);
            this.Controls.Add(this.fileOpenControl_input);
            this.Name = "GifMakerForm";
            this.Text = "GifMakerForm";
            this.Load += new System.EventHandler(this.GifMakerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Winform.Controls.FileOpenControl fileOpenControl_input;
        private System.Windows.Forms.Button button_run;
        private Winform.Controls.FileOutputControl fileOutputControl_outGif;
        private System.Windows.Forms.CheckBox checkBox_repeat;
        private Winform.Controls.NamedIntControl namedIntControl_delayMs;
    }
}