namespace Geo.Winform.Controls
{
    partial class TimeLoopControl
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.timePeriodControl1 = new Geo.Winform.Controls.TimePeriodControl();
            this.namedFloatControl_time = new Geo.Winform.Controls.NamedFloatControl();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timePeriodControl1
            // 
            this.timePeriodControl1.Location = new System.Drawing.Point(2, 2);
            this.timePeriodControl1.Margin = new System.Windows.Forms.Padding(2);
            this.timePeriodControl1.Name = "timePeriodControl1";
            this.timePeriodControl1.Size = new System.Drawing.Size(414, 24);
            this.timePeriodControl1.TabIndex = 10;
            this.timePeriodControl1.TimeFrom = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
            this.timePeriodControl1.TimeStringFormat = "yyyy-MM-dd HH:mm:ss";
            this.timePeriodControl1.TimeTo = new System.DateTime(2018, 1, 2, 0, 0, 0, 0);
            this.timePeriodControl1.Title = "时段：";
            // 
            // namedFloatControl_time
            // 
            this.namedFloatControl_time.Location = new System.Drawing.Point(421, 3);
            this.namedFloatControl_time.Name = "namedFloatControl_time";
            this.namedFloatControl_time.Size = new System.Drawing.Size(126, 23);
            this.namedFloatControl_time.TabIndex = 9;
            this.namedFloatControl_time.Title = "间隔（分）：";
            this.namedFloatControl_time.Value = 0.5D;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.timePeriodControl1);
            this.flowLayoutPanel1.Controls.Add(this.namedFloatControl_time);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(578, 30);
            this.flowLayoutPanel1.TabIndex = 11;
            // 
            // TimeLoopControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TimeLoopControl";
            this.Size = new System.Drawing.Size(578, 30);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TimePeriodControl timePeriodControl1;
        private NamedFloatControl namedFloatControl_time;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;


    }
}
