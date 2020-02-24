namespace Gnsser.Winform
{
    partial class SameSiteSelectForm
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
            this.namedIntControl_labelCharCount = new Geo.Winform.Controls.NamedIntControl();
            this.checkBox_ignoreCase = new System.Windows.Forms.CheckBox();
            this.checkBox1_moveOrCopy = new System.Windows.Forms.CheckBox();
            this.panel4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel_buttons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage0
            // 
            this.tabPage0.Size = new System.Drawing.Size(855, 273);
            // 
            // panel3
            // 
            this.panel3.Size = new System.Drawing.Size(645, 123);
            // 
            // panel2
            // 
            this.panel2.Size = new System.Drawing.Size(645, 123);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.checkBox1_moveOrCopy);
            this.panel4.Controls.Add(this.checkBox_ignoreCase);
            this.panel4.Controls.Add(this.namedIntControl_labelCharCount);
            this.panel4.Size = new System.Drawing.Size(849, 123);
            this.panel4.Controls.SetChildIndex(this.namedIntControl_labelCharCount, 0);
            this.panel4.Controls.SetChildIndex(this.checkBox_ignoreCase, 0);
            this.panel4.Controls.SetChildIndex(this.checkBox1_moveOrCopy, 0);
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl_inputPathes.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileOpenControl_inputPathes.FilePathes = new string[0];
            this.fileOpenControl_inputPathes.Size = new System.Drawing.Size(849, 93);
            this.fileOpenControl_inputPathes.FilePathSetted += new System.EventHandler(this.fileOpenControl1_FilePathSetted);
            // 
            // tabPage1
            // 
            this.tabPage1.Size = new System.Drawing.Size(855, 129);
            // 
            // panel_buttons
            // 
            this.panel_buttons.Location = new System.Drawing.Point(596, 0);
            // 
            // namedIntControl_labelCharCount
            // 
            this.namedIntControl_labelCharCount.Location = new System.Drawing.Point(98, 18);
            this.namedIntControl_labelCharCount.Name = "namedIntControl_labelCharCount";
            this.namedIntControl_labelCharCount.Size = new System.Drawing.Size(165, 23);
            this.namedIntControl_labelCharCount.TabIndex = 67;
            this.namedIntControl_labelCharCount.Title = "匹配字符数量：";
            this.namedIntControl_labelCharCount.Value = 4;
            // 
            // checkBox_ignoreCase
            // 
            this.checkBox_ignoreCase.AutoSize = true;
            this.checkBox_ignoreCase.Checked = true;
            this.checkBox_ignoreCase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_ignoreCase.Location = new System.Drawing.Point(100, 50);
            this.checkBox_ignoreCase.Name = "checkBox_ignoreCase";
            this.checkBox_ignoreCase.Size = new System.Drawing.Size(84, 16);
            this.checkBox_ignoreCase.TabIndex = 70;
            this.checkBox_ignoreCase.Text = "忽略大小写";
            this.checkBox_ignoreCase.UseVisualStyleBackColor = true;
            // 
            // checkBox1_moveOrCopy
            // 
            this.checkBox1_moveOrCopy.AutoSize = true;
            this.checkBox1_moveOrCopy.Checked = true;
            this.checkBox1_moveOrCopy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1_moveOrCopy.Location = new System.Drawing.Point(100, 85);
            this.checkBox1_moveOrCopy.Name = "checkBox1_moveOrCopy";
            this.checkBox1_moveOrCopy.Size = new System.Drawing.Size(84, 16);
            this.checkBox1_moveOrCopy.TabIndex = 70;
            this.checkBox1_moveOrCopy.Text = "移动或复制";
            this.checkBox1_moveOrCopy.UseVisualStyleBackColor = true;
            // 
            // SameSiteSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 530);
            this.IsShowProgressBar = true;
            this.Name = "SameSiteSelectForm";
            this.OutputDirectory = "D:\\ProgramFiles\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\Temp";
            this.Text = "收敛数据统计器";
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.panel_buttons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Geo.Winform.Controls.NamedIntControl namedIntControl_labelCharCount;
        private System.Windows.Forms.CheckBox checkBox_ignoreCase;
        private System.Windows.Forms.CheckBox checkBox1_moveOrCopy;
    }
}