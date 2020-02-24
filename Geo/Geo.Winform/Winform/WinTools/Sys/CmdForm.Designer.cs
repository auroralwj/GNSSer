namespace Geo.Winform.Sys
{
    partial class CmdForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_cmd = new System.Windows.Forms.TextBox();
            this.button_run = new System.Windows.Forms.Button();
            this.textBox_result = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_runAsyn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "命令：";
            // 
            // textBox_cmd
            // 
            this.textBox_cmd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_cmd.Location = new System.Drawing.Point(75, 13);
            this.textBox_cmd.Multiline = true;
            this.textBox_cmd.Name = "textBox_cmd";
            this.textBox_cmd.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_cmd.Size = new System.Drawing.Size(515, 69);
            this.textBox_cmd.TabIndex = 1;
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(515, 88);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 23);
            this.button_run.TabIndex = 2;
            this.button_run.Text = "异步运行";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // textBox_result
            // 
            this.textBox_result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_result.Location = new System.Drawing.Point(75, 125);
            this.textBox_result.Multiline = true;
            this.textBox_result.Name = "textBox_result";
            this.textBox_result.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_result.Size = new System.Drawing.Size(515, 182);
            this.textBox_result.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "结果：";
            // 
            // button_runAsyn
            // 
            this.button_runAsyn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_runAsyn.Location = new System.Drawing.Point(409, 88);
            this.button_runAsyn.Name = "button_runAsyn";
            this.button_runAsyn.Size = new System.Drawing.Size(75, 23);
            this.button_runAsyn.TabIndex = 4;
            this.button_runAsyn.Text = "同步运行";
            this.button_runAsyn.UseVisualStyleBackColor = true;
            this.button_runAsyn.Click += new System.EventHandler(this.button_runAsyn_Click);
            // 
            // CmdForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 319);
            this.Controls.Add(this.button_runAsyn);
            this.Controls.Add(this.textBox_result);
            this.Controls.Add(this.button_run);
            this.Controls.Add(this.textBox_cmd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "CmdForm";
            this.Text = "命令行测试";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_cmd;
        private System.Windows.Forms.Button button_run;
        private System.Windows.Forms.TextBox textBox_result;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_runAsyn;
    }
}