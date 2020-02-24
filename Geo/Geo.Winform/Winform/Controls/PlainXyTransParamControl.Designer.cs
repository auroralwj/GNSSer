namespace Geo.Winform.Controls
{
    partial class PlainXyTransParamControl
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
            this.namedXyControl1 = new Geo.Winform.Controls.NamedXyControl();
            this.namedFloatControl_rotateAngleSec = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_scaleFactor = new Geo.Winform.Controls.NamedFloatControl();
            this.SuspendLayout();
            // 
            // namedXyControl1
            // 
            this.namedXyControl1.Location = new System.Drawing.Point(3, 3);
            this.namedXyControl1.Name = "namedXyControl1";
            this.namedXyControl1.Size = new System.Drawing.Size(460, 23);
            this.namedXyControl1.TabIndex = 5;
            this.namedXyControl1.Title = "平移参数(XY, m)：";
            // 
            // namedFloatControl_rotateAngleSec
            // 
            this.namedFloatControl_rotateAngleSec.Location = new System.Drawing.Point(244, 32);
            this.namedFloatControl_rotateAngleSec.Name = "namedFloatControl_rotateAngleSec";
            this.namedFloatControl_rotateAngleSec.Size = new System.Drawing.Size(219, 23);
            this.namedFloatControl_rotateAngleSec.TabIndex = 3;
            this.namedFloatControl_rotateAngleSec.Title = "旋转角(s)：";
            this.namedFloatControl_rotateAngleSec.Value = 0.352319327046273D;
            // 
            // namedFloatControl_scaleFactor
            // 
            this.namedFloatControl_scaleFactor.Location = new System.Drawing.Point(3, 32);
            this.namedFloatControl_scaleFactor.Name = "namedFloatControl_scaleFactor";
            this.namedFloatControl_scaleFactor.Size = new System.Drawing.Size(235, 23);
            this.namedFloatControl_scaleFactor.TabIndex = 4;
            this.namedFloatControl_scaleFactor.Title = "尺度因子(ppm)：";
            this.namedFloatControl_scaleFactor.Value = 7.25784747146463E-06D;
            // 
            // PlainXyTransParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.namedXyControl1);
            this.Controls.Add(this.namedFloatControl_rotateAngleSec);
            this.Controls.Add(this.namedFloatControl_scaleFactor);
            this.Name = "PlainXyTransParamControl";
            this.Size = new System.Drawing.Size(471, 58);
            this.ResumeLayout(false);

        }

        #endregion

        private NamedXyControl namedXyControl1;
        private NamedFloatControl namedFloatControl_rotateAngleSec;
        private NamedFloatControl namedFloatControl_scaleFactor;
    }
}
