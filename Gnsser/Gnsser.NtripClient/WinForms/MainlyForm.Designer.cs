namespace Gnsser.Ntrip.WinForms
{
    partial class MainlyForm
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
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button_readLocal = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(24, 32);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(96, 39);
            this.button2.TabIndex = 1;
            this.button2.Text = "Ntrip数据接收";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(279, 32);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(96, 39);
            this.button3.TabIndex = 1;
            this.button3.Text = "数据表查看器";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button_readLocal
            // 
            this.button_readLocal.Location = new System.Drawing.Point(136, 32);
            this.button_readLocal.Name = "button_readLocal";
            this.button_readLocal.Size = new System.Drawing.Size(128, 39);
            this.button_readLocal.TabIndex = 1;
            this.button_readLocal.Text = "接收本地Ntrip数据";
            this.button_readLocal.UseVisualStyleBackColor = true;
            this.button_readLocal.Click += new System.EventHandler(this.button_readLocal_Click);
            // 
            // MainlyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 305);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button_readLocal);
            this.Controls.Add(this.button2);
            this.Name = "MainlyForm";
            this.Text = "MainlyForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button_readLocal;
    }
}