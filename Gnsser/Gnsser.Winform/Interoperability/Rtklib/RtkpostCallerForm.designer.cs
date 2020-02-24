namespace Gnsser.Winform
{
    partial class RtkpostCallerForm
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

        #region Windows Form Designer generated obsCodeode

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the obsCodeode editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_setclk = new System.Windows.Forms.Button();
            this.button_setsp3 = new System.Windows.Forms.Button();
            this.button_setNavPath = new System.Windows.Forms.Button();
            this.button_obsPathSet = new System.Windows.Forms.Button();
            this.textBox_clk = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_sp3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_nav = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_obs = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog_obs = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_nav = new System.Windows.Forms.OpenFileDialog();
            this.richTextBoxControl1 = new Geo.Winform.Controls.RichTextBoxControl();
            this.button_caculate = new System.Windows.Forms.Button();
            this.button_readConfig = new System.Windows.Forms.Button();
            this.openFileDialog_clk = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl2 = new Geo.Winform.Controls.RichTextBoxControl();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button_setclk);
            this.groupBox1.Controls.Add(this.button_setsp3);
            this.groupBox1.Controls.Add(this.button_setNavPath);
            this.groupBox1.Controls.Add(this.button_obsPathSet);
            this.groupBox1.Controls.Add(this.textBox_clk);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBox_sp3);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox_nav);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_obs);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(9, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(696, 123);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置";
            // 
            // button_setclk
            // 
            this.button_setclk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setclk.Location = new System.Drawing.Point(645, 92);
            this.button_setclk.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_setclk.Name = "button_setclk";
            this.button_setclk.Size = new System.Drawing.Size(47, 20);
            this.button_setclk.TabIndex = 2;
            this.button_setclk.Text = "...";
            this.button_setclk.UseVisualStyleBackColor = true;
            this.button_setclk.Click += new System.EventHandler(this.button_setclk_Click);
            // 
            // button_setsp3
            // 
            this.button_setsp3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setsp3.Location = new System.Drawing.Point(645, 67);
            this.button_setsp3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_setsp3.Name = "button_setsp3";
            this.button_setsp3.Size = new System.Drawing.Size(47, 20);
            this.button_setsp3.TabIndex = 2;
            this.button_setsp3.Text = "...";
            this.button_setsp3.UseVisualStyleBackColor = true;
            this.button_setsp3.Click += new System.EventHandler(this.button_setsp3_Click);
            // 
            // button_setNavPath
            // 
            this.button_setNavPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setNavPath.Location = new System.Drawing.Point(645, 42);
            this.button_setNavPath.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_setNavPath.Name = "button_setNavPath";
            this.button_setNavPath.Size = new System.Drawing.Size(47, 20);
            this.button_setNavPath.TabIndex = 2;
            this.button_setNavPath.Text = "...";
            this.button_setNavPath.UseVisualStyleBackColor = true;
            this.button_setNavPath.Click += new System.EventHandler(this.button_setNavPath_Click);
            // 
            // button_obsPathSet
            // 
            this.button_obsPathSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_obsPathSet.Location = new System.Drawing.Point(645, 18);
            this.button_obsPathSet.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_obsPathSet.Name = "button_obsPathSet";
            this.button_obsPathSet.Size = new System.Drawing.Size(47, 20);
            this.button_obsPathSet.TabIndex = 2;
            this.button_obsPathSet.Text = "...";
            this.button_obsPathSet.UseVisualStyleBackColor = true;
            this.button_obsPathSet.Click += new System.EventHandler(this.button_obsPathSet_Click);
            // 
            // textBox_clk
            // 
            this.textBox_clk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_clk.Location = new System.Drawing.Point(105, 92);
            this.textBox_clk.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox_clk.Name = "textBox_clk";
            this.textBox_clk.Size = new System.Drawing.Size(536, 21);
            this.textBox_clk.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 94);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "精密钟差(可选)：";
            // 
            // textBox_sp3
            // 
            this.textBox_sp3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_sp3.Location = new System.Drawing.Point(105, 67);
            this.textBox_sp3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox_sp3.Name = "textBox_sp3";
            this.textBox_sp3.Size = new System.Drawing.Size(536, 21);
            this.textBox_sp3.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 70);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "精密星历(可选)：";
            // 
            // textBox_nav
            // 
            this.textBox_nav.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_nav.Location = new System.Drawing.Point(105, 42);
            this.textBox_nav.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox_nav.Name = "textBox_nav";
            this.textBox_nav.Size = new System.Drawing.Size(536, 21);
            this.textBox_nav.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 45);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "导航(N)文件：";
            // 
            // textBox_obs
            // 
            this.textBox_obs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_obs.Location = new System.Drawing.Point(105, 18);
            this.textBox_obs.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox_obs.Name = "textBox_obs";
            this.textBox_obs.Size = new System.Drawing.Size(536, 21);
            this.textBox_obs.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "观测(O)文件：";
            // 
            // openFileDialog_obs
            // 
            this.openFileDialog_obs.Filter = "Rinex观测文件|*.*o|所有文件|*.*";
            // 
            // openFileDialog_nav
            // 
            this.openFileDialog_nav.FileName = "openFileDialog2";
            this.openFileDialog_nav.Filter = "Rinex导航、星历文件|*.*N;*.*R;*.*G;*.SP3|所有文件|*.*";
            // 
            // richTextBoxControl1
            // 
            this.richTextBoxControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl1.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.richTextBoxControl1.Name = "richTextBoxControl1";
            this.richTextBoxControl1.Size = new System.Drawing.Size(682, 245);
            this.richTextBoxControl1.TabIndex = 0;
            this.richTextBoxControl1.Text = "";
            // 
            // button_caculate
            // 
            this.button_caculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_caculate.Location = new System.Drawing.Point(637, 138);
            this.button_caculate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_caculate.Name = "button_caculate";
            this.button_caculate.Size = new System.Drawing.Size(68, 26);
            this.button_caculate.TabIndex = 2;
            this.button_caculate.Text = "计算";
            this.button_caculate.UseVisualStyleBackColor = true;
            this.button_caculate.Click += new System.EventHandler(this.button_caculate_Click);
            // 
            // button_readConfig
            // 
            this.button_readConfig.Location = new System.Drawing.Point(14, 138);
            this.button_readConfig.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_readConfig.Name = "button_readConfig";
            this.button_readConfig.Size = new System.Drawing.Size(140, 26);
            this.button_readConfig.TabIndex = 2;
            this.button_readConfig.Text = "查看修改配置文件";
            this.button_readConfig.UseVisualStyleBackColor = true;
            this.button_readConfig.Click += new System.EventHandler(this.button_readConfig_Click);
            // 
            // openFileDialog_clk
            // 
            this.openFileDialog_clk.FileName = "钟差";
            this.openFileDialog_clk.Filter = "精密钟差文件|*.clk*|所有文件|*.*";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(9, 182);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(696, 277);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextBoxControl1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(688, 251);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "计算结果";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.richTextBoxControl2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(688, 251);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "其它信息";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl2
            // 
            this.richTextBoxControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl2.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl2.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBoxControl2.Name = "richTextBoxControl2";
            this.richTextBoxControl2.Size = new System.Drawing.Size(682, 245);
            this.richTextBoxControl2.TabIndex = 1;
            this.richTextBoxControl2.Text = "";
            // 
            // RtkpostCallerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 459);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button_readConfig);
            this.Controls.Add(this.button_caculate);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "RtkpostCallerForm";
            this.Text = "Rtkpost事后定位计算";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_obsPathSet;
        private System.Windows.Forms.TextBox textBox_obs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog_obs;
        private System.Windows.Forms.OpenFileDialog openFileDialog_nav;
        private System.Windows.Forms.Button button_setNavPath;
        private System.Windows.Forms.TextBox textBox_nav;
        private System.Windows.Forms.Label label2;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl1;
        private System.Windows.Forms.Button button_caculate;
        private System.Windows.Forms.Button button_readConfig;
        private System.Windows.Forms.Button button_setclk;
        private System.Windows.Forms.Button button_setsp3;
        private System.Windows.Forms.TextBox textBox_clk;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_sp3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.OpenFileDialog openFileDialog_clk;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl2;
    }
}