namespace Gnsser.Winform
{
    partial class SelectCommonFilesForm
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
            this.checkBox_copyOrMove = new System.Windows.Forms.CheckBox();
            this.checkBox_loopSub = new System.Windows.Forms.CheckBox();
            this.directorySelectionControl2 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.button_ok = new System.Windows.Forms.Button();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkBox_copyOrMove);
            this.groupBox1.Controls.Add(this.checkBox_loopSub);
            this.groupBox1.Controls.Add(this.directorySelectionControl2);
            this.groupBox1.Controls.Add(this.directorySelectionControl1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(546, 184);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选项";
            // 
            // checkBox_copyOrMove
            // 
            this.checkBox_copyOrMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_copyOrMove.AutoSize = true;
            this.checkBox_copyOrMove.Checked = true;
            this.checkBox_copyOrMove.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_copyOrMove.Location = new System.Drawing.Point(192, 134);
            this.checkBox_copyOrMove.Name = "checkBox_copyOrMove";
            this.checkBox_copyOrMove.Size = new System.Drawing.Size(84, 16);
            this.checkBox_copyOrMove.TabIndex = 5;
            this.checkBox_copyOrMove.Text = "复制或移动";
            this.checkBox_copyOrMove.UseVisualStyleBackColor = true;
            // 
            // checkBox_loopSub
            // 
            this.checkBox_loopSub.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_loopSub.AutoSize = true;
            this.checkBox_loopSub.Checked = true;
            this.checkBox_loopSub.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_loopSub.Location = new System.Drawing.Point(87, 134);
            this.checkBox_loopSub.Name = "checkBox_loopSub";
            this.checkBox_loopSub.Size = new System.Drawing.Size(84, 16);
            this.checkBox_loopSub.TabIndex = 5;
            this.checkBox_loopSub.Text = "包含子目录";
            this.checkBox_loopSub.UseVisualStyleBackColor = true;
            // 
            // directorySelectionControl2
            // 
            this.directorySelectionControl2.AllowDrop = true;
            this.directorySelectionControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl2.IsMultiPathes = false;
            this.directorySelectionControl2.LabelName = "目标文件夹：";
            this.directorySelectionControl2.Location = new System.Drawing.Point(14, 156);
            this.directorySelectionControl2.Name = "directorySelectionControl2";
            this.directorySelectionControl2.Path = "";
            this.directorySelectionControl2.Pathes = new string[0];
            this.directorySelectionControl2.Size = new System.Drawing.Size(511, 22);
            this.directorySelectionControl2.TabIndex = 3;
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl1.IsMultiPathes = true;
            this.directorySelectionControl1.LabelName = "文件夹：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(14, 22);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "";
            this.directorySelectionControl1.Pathes = new string[0];
            this.directorySelectionControl1.Size = new System.Drawing.Size(511, 106);
            this.directorySelectionControl1.TabIndex = 3;
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ok.Location = new System.Drawing.Point(475, 203);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 38);
            this.button_ok.TabIndex = 1;
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.IsBackwardProcess = false;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(27, 206);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(437, 34);
            this.progressBarComponent1.TabIndex = 2;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // SelectCommonFilesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 252);
            this.Controls.Add(this.progressBarComponent1);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.groupBox1);
            this.Name = "SelectCommonFilesForm";
            this.Text = "共同文件选择器";
            this.Load += new System.EventHandler(this.MoveFileByKeyForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_ok;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private System.Windows.Forms.CheckBox checkBox_loopSub;
        private Geo.Winform.Controls.ProgressBarComponent progressBarComponent1;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl2;
        private System.Windows.Forms.CheckBox checkBox_copyOrMove;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}