﻿namespace Geo.Winform
{
    partial class InputThreeValueForm
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
            this.namedFloatControl_val1 = new Geo.Winform.Controls.NamedFloatControl();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.namedFloatControl_val2 = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_val3 = new Geo.Winform.Controls.NamedFloatControl();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // namedFloatControl_val1
            // 
            this.namedFloatControl_val1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.namedFloatControl_val1.Location = new System.Drawing.Point(16, 30);
            this.namedFloatControl_val1.Name = "namedFloatControl_val1";
            this.namedFloatControl_val1.Size = new System.Drawing.Size(255, 23);
            this.namedFloatControl_val1.TabIndex = 0;
            this.namedFloatControl_val1.Title = "名称：";
            this.namedFloatControl_val1.Value = 0.1D;
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(237, 146);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 1;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ok.Location = new System.Drawing.Point(156, 146);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 2;
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.namedFloatControl_val3);
            this.groupBox1.Controls.Add(this.namedFloatControl_val2);
            this.groupBox1.Controls.Add(this.namedFloatControl_val1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 115);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息";
            // 
            // namedFloatControl_val2
            // 
            this.namedFloatControl_val2.Location = new System.Drawing.Point(16, 59);
            this.namedFloatControl_val2.Name = "namedFloatControl_val2";
            this.namedFloatControl_val2.Size = new System.Drawing.Size(255, 23);
            this.namedFloatControl_val2.TabIndex = 0;
            this.namedFloatControl_val2.Title = "名称：";
            this.namedFloatControl_val2.Value = 0.1D;
            // 
            // namedFloatControl3
            // 
            this.namedFloatControl_val3.Location = new System.Drawing.Point(16, 86);
            this.namedFloatControl_val3.Name = "namedFloatControl3";
            this.namedFloatControl_val3.Size = new System.Drawing.Size(255, 23);
            this.namedFloatControl_val3.TabIndex = 0;
            this.namedFloatControl_val3.Title = "名称：";
            this.namedFloatControl_val3.Value = 0.1D;
            // 
            // InputThreeValueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 181);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.button_cancel);
            this.Name = "InputThreeValueForm";
            this.Text = "请输入";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.NamedFloatControl namedFloatControl_val1;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.GroupBox groupBox1;
        private Controls.NamedFloatControl namedFloatControl_val2;
        private Controls.NamedFloatControl namedFloatControl_val3;
    }
}