namespace Geo.Winform
{
    partial class DrawObjectOptionForm
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
            this.paramVectorRenderControl1 = new Geo.Winform.Controls.ParamVectorRenderControl();
            this.button_draw = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // paramVectorRenderControl1
            // 
            this.paramVectorRenderControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.paramVectorRenderControl1.Location = new System.Drawing.Point(11, 11);
            this.paramVectorRenderControl1.Margin = new System.Windows.Forms.Padding(2);
            this.paramVectorRenderControl1.Name = "paramVectorRenderControl1";
            this.paramVectorRenderControl1.Size = new System.Drawing.Size(598, 123);
            this.paramVectorRenderControl1.TabIndex = 0;
            // 
            // button_draw
            // 
            this.button_draw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_draw.Location = new System.Drawing.Point(431, 139);
            this.button_draw.Name = "button_draw";
            this.button_draw.Size = new System.Drawing.Size(75, 37);
            this.button_draw.TabIndex = 1;
            this.button_draw.Text = "绘图";
            this.button_draw.UseVisualStyleBackColor = true;
            this.button_draw.Click += new System.EventHandler(this.button_draw_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(522, 139);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 37);
            this.button_cancel.TabIndex = 1;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // DrawObjectOptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 188);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_draw);
            this.Controls.Add(this.paramVectorRenderControl1);
            this.Name = "DrawObjectOptionForm";
            this.Text = "数据表绘图选星";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ParamVectorRenderControl paramVectorRenderControl1;
        private System.Windows.Forms.Button button_draw;
        private System.Windows.Forms.Button button_cancel;
    }
}