namespace Gnsser.Winform.Controls
{
    partial class TimePeriodUserControl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.timePeriodControl1 = new Geo.Winform.Controls.TimePeriodControl();
            this.button_loadFromObsFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // timePeriodControl1
            // 
            this.timePeriodControl1.Location = new System.Drawing.Point(3, 4);
            this.timePeriodControl1.Margin = new System.Windows.Forms.Padding(2);
            this.timePeriodControl1.Name = "timePeriodControl1";
            this.timePeriodControl1.Size = new System.Drawing.Size(414, 24);
            this.timePeriodControl1.TabIndex = 0;
            this.timePeriodControl1.TimeFrom = new System.DateTime(2019, 1, 4, 20, 45, 44, 439);
            this.timePeriodControl1.TimeStringFormat = "yyyy-MM-dd HH:mm:ss";
            this.timePeriodControl1.TimeTo = new System.DateTime(2019, 1, 4, 20, 45, 44, 445);
            this.timePeriodControl1.Title = "时段：";
            // 
            // button_loadFromObsFile
            // 
            this.button_loadFromObsFile.Location = new System.Drawing.Point(414, 6);
            this.button_loadFromObsFile.Name = "button_loadFromObsFile";
            this.button_loadFromObsFile.Size = new System.Drawing.Size(75, 23);
            this.button_loadFromObsFile.TabIndex = 1;
            this.button_loadFromObsFile.Text = "从观测文件加载";
            this.button_loadFromObsFile.UseVisualStyleBackColor = true;
            this.button_loadFromObsFile.Click += new System.EventHandler(this.button_loadFromObsFile_Click);
            // 
            // TimePeriodUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button_loadFromObsFile);
            this.Controls.Add(this.timePeriodControl1);
            this.Name = "TimePeriodUserControl";
            this.Size = new System.Drawing.Size(492, 32);
            this.ResumeLayout(false);

        }

        #endregion

        private Geo.Winform.Controls.TimePeriodControl timePeriodControl1;
        private System.Windows.Forms.Button button_loadFromObsFile;
    }
}
