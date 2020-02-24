namespace Gnsser.Winform
{
    partial class GLoggerFileConvertForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        { 
            this.checkBox_skipZeroPsuedorange = new System.Windows.Forms.CheckBox();
            this.checkBox_skipZeroPhase = new System.Windows.Forms.CheckBox();
            this.checkBox1IsToCylePhase = new System.Windows.Forms.CheckBox();
            this.checkBox_fromFirstEpoch = new System.Windows.Forms.CheckBox();
            this.checkBox1IsAligningPhase = new System.Windows.Forms.CheckBox();
            this.panel4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage0
            // 
            this.tabPage0.Size = new System.Drawing.Size(567, 262);
            // 
            // panel3
            // 
            this.panel3.Size = new System.Drawing.Size(593, 120);
            // 
            // panel2
            // 
            this.panel2.Size = new System.Drawing.Size(593, 120);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.checkBox1IsAligningPhase);
            this.panel4.Controls.Add(this.checkBox1IsToCylePhase);
            this.panel4.Controls.Add(this.checkBox_fromFirstEpoch);
            this.panel4.Controls.Add(this.checkBox_skipZeroPhase);
            this.panel4.Controls.Add(this.checkBox_skipZeroPsuedorange);
            this.panel4.Size = new System.Drawing.Size(561, 120);
            this.panel4.Controls.SetChildIndex(this.checkBox_skipZeroPsuedorange, 0);
            this.panel4.Controls.SetChildIndex(this.checkBox_skipZeroPhase, 0);
            this.panel4.Controls.SetChildIndex(this.checkBox_fromFirstEpoch, 0);
            this.panel4.Controls.SetChildIndex(this.checkBox1IsToCylePhase, 0);
            this.panel4.Controls.SetChildIndex(this.checkBox1IsAligningPhase, 0);
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl_inputPathes.FilePathes = new string[0];
            this.fileOpenControl_inputPathes.Filter = "\"Glog文本表格文件(*.txt;*.txt.xls;*.txt;*.xls)|*.glog;*.txt.xls;*.txt;*.xls|所有文件(*.*)|*" +
    ".*\";";
            this.fileOpenControl_inputPathes.Size = new System.Drawing.Size(561, 98);
            // 
            // tabPage1
            // 
            this.tabPage1.Size = new System.Drawing.Size(567, 126);
            // 
            // checkBox_skipZeroPsuedorange
            // 
            this.checkBox_skipZeroPsuedorange.AutoSize = true;
            this.checkBox_skipZeroPsuedorange.Checked = true;
            this.checkBox_skipZeroPsuedorange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_skipZeroPsuedorange.Location = new System.Drawing.Point(17, 57);
            this.checkBox_skipZeroPsuedorange.Name = "checkBox_skipZeroPsuedorange";
            this.checkBox_skipZeroPsuedorange.Size = new System.Drawing.Size(90, 16);
            this.checkBox_skipZeroPsuedorange.TabIndex = 57;
            this.checkBox_skipZeroPsuedorange.Text = "过滤0值伪距";
            this.checkBox_skipZeroPsuedorange.UseVisualStyleBackColor = true;
            // 
            // checkBox_skipZeroPhase
            // 
            this.checkBox_skipZeroPhase.AutoSize = true;
            this.checkBox_skipZeroPhase.Checked = true;
            this.checkBox_skipZeroPhase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_skipZeroPhase.Location = new System.Drawing.Point(128, 57);
            this.checkBox_skipZeroPhase.Name = "checkBox_skipZeroPhase";
            this.checkBox_skipZeroPhase.Size = new System.Drawing.Size(90, 16);
            this.checkBox_skipZeroPhase.TabIndex = 58;
            this.checkBox_skipZeroPhase.Text = "过滤0值载波";
            this.checkBox_skipZeroPhase.UseVisualStyleBackColor = true;
            // 
            // checkBox1IsToCylePhase
            // 
            this.checkBox1IsToCylePhase.AutoSize = true;
            this.checkBox1IsToCylePhase.Checked = true;
            this.checkBox1IsToCylePhase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1IsToCylePhase.Location = new System.Drawing.Point(17, 90);
            this.checkBox1IsToCylePhase.Name = "checkBox1IsToCylePhase";
            this.checkBox1IsToCylePhase.Size = new System.Drawing.Size(120, 16);
            this.checkBox1IsToCylePhase.TabIndex = 59;
            this.checkBox1IsToCylePhase.Text = "相位单位转换为周";
            this.checkBox1IsToCylePhase.UseVisualStyleBackColor = true;
            // 
            // checkBox_fromFirstEpoch
            // 
            this.checkBox_fromFirstEpoch.AutoSize = true;
            this.checkBox_fromFirstEpoch.Location = new System.Drawing.Point(243, 57);
            this.checkBox_fromFirstEpoch.Name = "checkBox_fromFirstEpoch";
            this.checkBox_fromFirstEpoch.Size = new System.Drawing.Size(108, 16);
            this.checkBox_fromFirstEpoch.TabIndex = 58;
            this.checkBox_fromFirstEpoch.Text = "从第一历元递增";
            this.checkBox_fromFirstEpoch.UseVisualStyleBackColor = true;
            // 
            // checkBox1IsAligningPhase
            // 
            this.checkBox1IsAligningPhase.AutoSize = true;
            this.checkBox1IsAligningPhase.Checked = true;
            this.checkBox1IsAligningPhase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1IsAligningPhase.Location = new System.Drawing.Point(157, 90);
            this.checkBox1IsAligningPhase.Name = "checkBox1IsAligningPhase";
            this.checkBox1IsAligningPhase.Size = new System.Drawing.Size(84, 16);
            this.checkBox1IsAligningPhase.TabIndex = 59;
            this.checkBox1IsAligningPhase.Text = "相初始对齐";
            this.checkBox1IsAligningPhase.UseVisualStyleBackColor = true;
            // 
            // GLoggerFileConvertForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 508);
            this.IsShowProgressBar = true;
            this.Name = "GLoggerFileConvertForm";
            this.Text = "Gloger转换Rinex观测文件";
            this.Load += new System.EventHandler(this.RinexSparsityForm_Load);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_skipZeroPhase;
        private System.Windows.Forms.CheckBox checkBox_skipZeroPsuedorange;
        private System.Windows.Forms.CheckBox checkBox1IsToCylePhase;
        private System.Windows.Forms.CheckBox checkBox_fromFirstEpoch;
        private System.Windows.Forms.CheckBox checkBox1IsAligningPhase;


    }
}

