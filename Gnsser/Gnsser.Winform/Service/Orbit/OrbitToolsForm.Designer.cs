namespace Gnsser.Winform
{
    partial class OrbitToolsForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button_showOnMap = new System.Windows.Forms.Button();
            this.textBox_distance = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_radius = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_caculate = new System.Windows.Forms.Button();
            this.textBox_show = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.button_showOnMap);
            this.groupBox1.Controls.Add(this.textBox_distance);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_radius);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button_caculate);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(912, 127);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "计算平均距离偏差";
            // 
            // button_showOnMap
            // 
            this.button_showOnMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_showOnMap.Enabled = false;
            this.button_showOnMap.Location = new System.Drawing.Point(809, 24);
            this.button_showOnMap.Name = "button_showOnMap";
            this.button_showOnMap.Size = new System.Drawing.Size(75, 53);
            this.button_showOnMap.TabIndex = 3;
            this.button_showOnMap.Text = "地图显示";
            this.button_showOnMap.UseVisualStyleBackColor = true;
            this.button_showOnMap.Click += new System.EventHandler(this.button_showOnMap_Click);
            // 
            // textBox_time
            // 
            this.textBox_distance.Location = new System.Drawing.Point(127, 52);
            this.textBox_distance.Name = "textBox_time";
            this.textBox_distance.Size = new System.Drawing.Size(500, 25);
            this.textBox_distance.TabIndex = 2;
            this.textBox_distance.Text = "10";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "站星距离偏差：";
            // 
            // textBox_radius
            // 
            this.textBox_radius.Location = new System.Drawing.Point(89, 21);
            this.textBox_radius.Name = "textBox_radius";
            this.textBox_radius.Size = new System.Drawing.Size(500, 25);
            this.textBox_radius.TabIndex = 2;
            this.textBox_radius.Text = "22000000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "半径：";
            // 
            // button_caculate
            // 
            this.button_caculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_caculate.Location = new System.Drawing.Point(718, 24);
            this.button_caculate.Name = "button_caculate";
            this.button_caculate.Size = new System.Drawing.Size(75, 54);
            this.button_caculate.TabIndex = 0;
            this.button_caculate.Text = "计算";
            this.button_caculate.UseVisualStyleBackColor = true;
            this.button_caculate.Click += new System.EventHandler(this.button_caculate_Click);
            // 
            // textBox_show
            // 
            this.textBox_show.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_show.Location = new System.Drawing.Point(13, 164);
            this.textBox_show.Multiline = true;
            this.textBox_show.Name = "textBox_show";
            this.textBox_show.Size = new System.Drawing.Size(912, 333);
            this.textBox_show.TabIndex = 1;
            // 
            // OrbitToolsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(937, 509);
            this.Controls.Add(this.textBox_show);
            this.Controls.Add(this.groupBox1);
            this.Name = "OrbitToolsForm";
            this.Text = "轨道计算工具";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_show;
        private System.Windows.Forms.TextBox textBox_distance;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_radius;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_caculate;
        private System.Windows.Forms.Button button_showOnMap;
        private System.Windows.Forms.Label label3;
    }
}