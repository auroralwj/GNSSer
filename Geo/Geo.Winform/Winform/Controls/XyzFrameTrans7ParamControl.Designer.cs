namespace Geo.Winform.Controls
{
    partial class XyzFrameTrans7ParamControl
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
            this.namedXyzControl_offset = new Geo.Winform.Controls.NamedXyzControl();
            this.namedXyzControl_rotateAngleSec = new Geo.Winform.Controls.NamedXyzControl();
            this.namedFloatControl_scaleFactor = new Geo.Winform.Controls.NamedFloatControl();
            this.SuspendLayout();
            // 
            // namedXyzControl_offset
            // 
            this.namedXyzControl_offset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.namedXyzControl_offset.Location = new System.Drawing.Point(3, 3);
            this.namedXyzControl_offset.Name = "namedXyzControl_offset";
            this.namedXyzControl_offset.Size = new System.Drawing.Size(457, 23);
            this.namedXyzControl_offset.TabIndex = 5;
            this.namedXyzControl_offset.Title = "平移参数(XYZ, m)：";
            // 
            // namedXyzControl_rotateAngleSec
            // 
            this.namedXyzControl_rotateAngleSec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.namedXyzControl_rotateAngleSec.Location = new System.Drawing.Point(14, 30);
            this.namedXyzControl_rotateAngleSec.Name = "namedXyzControl_rotateAngleSec";
            this.namedXyzControl_rotateAngleSec.Size = new System.Drawing.Size(442, 23);
            this.namedXyzControl_rotateAngleSec.TabIndex = 3;
            this.namedXyzControl_rotateAngleSec.Title = "旋转角(XYZ, s)：";
            // 
            // namedFloatControl_scaleFactor
            // 
            this.namedFloatControl_scaleFactor.Location = new System.Drawing.Point(20, 58);
            this.namedFloatControl_scaleFactor.Name = "namedFloatControl_scaleFactor";
            this.namedFloatControl_scaleFactor.Size = new System.Drawing.Size(285, 23);
            this.namedFloatControl_scaleFactor.TabIndex = 4;
            this.namedFloatControl_scaleFactor.Title = "尺度因子(ppm)：";
            this.namedFloatControl_scaleFactor.Value = 7.25784747146463E-06D;
            // 
            // XyzFrameTrans7ParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.namedXyzControl_offset);
            this.Controls.Add(this.namedXyzControl_rotateAngleSec);
            this.Controls.Add(this.namedFloatControl_scaleFactor);
            this.Name = "XyzFrameTrans7ParamControl";
            this.Size = new System.Drawing.Size(466, 84);
            this.ResumeLayout(false);

        }

        #endregion

        private NamedXyzControl namedXyzControl_offset;
        private NamedXyzControl namedXyzControl_rotateAngleSec;
        private NamedFloatControl namedFloatControl_scaleFactor;
    }
}
