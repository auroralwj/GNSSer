namespace Gnsser.Winform
{
    partial class DegForamatConvertForm
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
            this.textBox_degree = new System.Windows.Forms.TextBox();
            this.textBox_rad = new System.Windows.Forms.TextBox();
            this.button_degreeToRad = new System.Windows.Forms.Button();
            this.button_radToDegree = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_degree
            // 
            this.textBox_degree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_degree.Location = new System.Drawing.Point(3, 17);
            this.textBox_degree.Multiline = true;
            this.textBox_degree.Name = "textBox_degree";
            this.textBox_degree.Size = new System.Drawing.Size(224, 399);
            this.textBox_degree.TabIndex = 0;
            this.textBox_degree.Text = "35°00\'0.22˝\r\n90°00\'00.11˝";
            // 
            // textBox_rad
            // 
            this.textBox_rad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_rad.Location = new System.Drawing.Point(3, 17);
            this.textBox_rad.Multiline = true;
            this.textBox_rad.Name = "textBox_rad";
            this.textBox_rad.Size = new System.Drawing.Size(337, 399);
            this.textBox_rad.TabIndex = 0;
            // 
            // button_degreeToRad
            // 
            this.button_degreeToRad.Location = new System.Drawing.Point(16, 65);
            this.button_degreeToRad.Name = "button_degreeToRad";
            this.button_degreeToRad.Size = new System.Drawing.Size(75, 48);
            this.button_degreeToRad.TabIndex = 1;
            this.button_degreeToRad.Text = "->>";
            this.button_degreeToRad.UseVisualStyleBackColor = true;
            this.button_degreeToRad.Click += new System.EventHandler(this.button_degreeToRad_Click);
            // 
            // button_radToDegree
            // 
            this.button_radToDegree.Location = new System.Drawing.Point(16, 168);
            this.button_radToDegree.Name = "button_radToDegree";
            this.button_radToDegree.Size = new System.Drawing.Size(75, 48);
            this.button_radToDegree.TabIndex = 1;
            this.button_radToDegree.Text = "<<-";
            this.button_radToDegree.UseVisualStyleBackColor = true;
            this.button_radToDegree.Click += new System.EventHandler(this.button_radToDegree_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_degree);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(230, 419);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "度分秒";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox_rad);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(343, 419);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "度小数";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(690, 419);
            this.splitContainer1.SplitterDistance = 230;
            this.splitContainer1.TabIndex = 5;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.button_degreeToRad);
            this.splitContainer2.Panel1.Controls.Add(this.button_radToDegree);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer2.Size = new System.Drawing.Size(456, 419);
            this.splitContainer2.SplitterDistance = 109;
            this.splitContainer2.TabIndex = 0;
            // 
            // DegForamatConvertForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 419);
            this.Controls.Add(this.splitContainer1);
            this.Name = "DegForamatConvertForm";
            this.Text = "角度单位转换";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_degree;
        private System.Windows.Forms.TextBox textBox_rad;
        private System.Windows.Forms.Button button_degreeToRad;
        private System.Windows.Forms.Button button_radToDegree;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;

    }
}