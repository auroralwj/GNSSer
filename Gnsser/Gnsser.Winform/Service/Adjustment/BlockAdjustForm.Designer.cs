namespace Gnsser.Winform
{
    partial class BlockAdjustForm
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
            this.panel_parallel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_degreeOfParallel = new System.Windows.Forms.TextBox();
            this.label_processorCount = new System.Windows.Forms.Label();
            this.checkBox_parallel = new System.Windows.Forms.CheckBox();
            this.textBox_rinexPathes = new System.Windows.Forms.TextBox();
            this.button_setBlockFilesPath = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button_solve = new System.Windows.Forms.Button();
            this.textBox_info = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBox_resultSinex = new System.Windows.Forms.TextBox();
            this.button_saveSnx = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.button_showOnMap = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.panel_parallel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.panel_parallel);
            this.groupBox1.Controls.Add(this.checkBox_parallel);
            this.groupBox1.Controls.Add(this.textBox_rinexPathes);
            this.groupBox1.Controls.Add(this.button_setBlockFilesPath);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 12);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(955, 131);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息";
            // 
            // panel_parallel
            // 
            this.panel_parallel.Controls.Add(this.label2);
            this.panel_parallel.Controls.Add(this.label3);
            this.panel_parallel.Controls.Add(this.textBox_degreeOfParallel);
            this.panel_parallel.Controls.Add(this.label_processorCount);
            this.panel_parallel.Location = new System.Drawing.Point(259, 85);
            this.panel_parallel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel_parallel.Name = "panel_parallel";
            this.panel_parallel.Size = new System.Drawing.Size(456, 42);
            this.panel_parallel.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "并行度：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(237, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "CPU核数量：";
            // 
            // textBox_degreeOfParallel
            // 
            this.textBox_degreeOfParallel.Location = new System.Drawing.Point(99, 5);
            this.textBox_degreeOfParallel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_degreeOfParallel.Name = "textBox_degreeOfParallel";
            this.textBox_degreeOfParallel.Size = new System.Drawing.Size(100, 25);
            this.textBox_degreeOfParallel.TabIndex = 4;
            // 
            // label_processorCount
            // 
            this.label_processorCount.AutoSize = true;
            this.label_processorCount.Location = new System.Drawing.Point(335, 12);
            this.label_processorCount.Name = "label_processorCount";
            this.label_processorCount.Size = new System.Drawing.Size(91, 15);
            this.label_processorCount.TabIndex = 1;
            this.label_processorCount.Text = "CPU核数量：";
            // 
            // checkBox_parallel
            // 
            this.checkBox_parallel.AutoSize = true;
            this.checkBox_parallel.Checked = true;
            this.checkBox_parallel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_parallel.Location = new System.Drawing.Point(140, 95);
            this.checkBox_parallel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_parallel.Name = "checkBox_parallel";
            this.checkBox_parallel.Size = new System.Drawing.Size(119, 19);
            this.checkBox_parallel.TabIndex = 5;
            this.checkBox_parallel.Text = "多核并行计算";
            this.checkBox_parallel.UseVisualStyleBackColor = true;
            this.checkBox_parallel.CheckedChanged += new System.EventHandler(this.checkBox_parallel_CheckedChanged);
            // 
            // textBox_rinexPathes
            // 
            this.textBox_rinexPathes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_rinexPathes.Location = new System.Drawing.Point(140, 20);
            this.textBox_rinexPathes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_rinexPathes.Multiline = true;
            this.textBox_rinexPathes.Name = "textBox_rinexPathes";
            this.textBox_rinexPathes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_rinexPathes.Size = new System.Drawing.Size(716, 59);
            this.textBox_rinexPathes.TabIndex = 3;
            // 
            // button_setBlockFilesPath
            // 
            this.button_setBlockFilesPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setBlockFilesPath.Location = new System.Drawing.Point(873, 20);
            this.button_setBlockFilesPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_setBlockFilesPath.Name = "button_setBlockFilesPath";
            this.button_setBlockFilesPath.Size = new System.Drawing.Size(75, 59);
            this.button_setBlockFilesPath.TabIndex = 2;
            this.button_setBlockFilesPath.Text = "...";
            this.button_setBlockFilesPath.UseVisualStyleBackColor = true;
            this.button_setBlockFilesPath.Click += new System.EventHandler(this.button_setBlockFilesPath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "分区Sinex文件：";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "SINEX文件(*.SNX)|*.SNX|所有文件|*.*";
            this.openFileDialog1.Multiselect = true;
            // 
            // button_solve
            // 
            this.button_solve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_solve.Location = new System.Drawing.Point(892, 162);
            this.button_solve.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_solve.Name = "button_solve";
            this.button_solve.Size = new System.Drawing.Size(75, 61);
            this.button_solve.TabIndex = 1;
            this.button_solve.Text = "计算";
            this.button_solve.UseVisualStyleBackColor = true;
            this.button_solve.Click += new System.EventHandler(this.button_solve_Click);
            // 
            // textBox_info
            // 
            this.textBox_info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_info.Location = new System.Drawing.Point(4, 4);
            this.textBox_info.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_info.Multiline = true;
            this.textBox_info.Name = "textBox_info";
            this.textBox_info.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_info.Size = new System.Drawing.Size(939, 352);
            this.textBox_info.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(13, 230);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(955, 389);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBox_info);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Size = new System.Drawing.Size(947, 360);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "解算信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textBox_resultSinex);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Size = new System.Drawing.Size(947, 360);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "SINEX结果";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBox_resultSinex
            // 
            this.textBox_resultSinex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_resultSinex.Location = new System.Drawing.Point(4, 4);
            this.textBox_resultSinex.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_resultSinex.Multiline = true;
            this.textBox_resultSinex.Name = "textBox_resultSinex";
            this.textBox_resultSinex.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_resultSinex.Size = new System.Drawing.Size(939, 352);
            this.textBox_resultSinex.TabIndex = 3;
            // 
            // button_saveSnx
            // 
            this.button_saveSnx.Location = new System.Drawing.Point(13, 179);
            this.button_saveSnx.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_saveSnx.Name = "button_saveSnx";
            this.button_saveSnx.Size = new System.Drawing.Size(100, 29);
            this.button_saveSnx.TabIndex = 4;
            this.button_saveSnx.Text = "保存SINEX";
            this.button_saveSnx.UseVisualStyleBackColor = true;
            this.button_saveSnx.Click += new System.EventHandler(this.button_saveSnx_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "SINEX文件(*.SNX)|*.SNX|所有文件|*.*";
            // 
            // button_showOnMap
            // 
            this.button_showOnMap.Location = new System.Drawing.Point(153, 179);
            this.button_showOnMap.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_showOnMap.Name = "button_showOnMap";
            this.button_showOnMap.Size = new System.Drawing.Size(100, 29);
            this.button_showOnMap.TabIndex = 5;
            this.button_showOnMap.Text = "地图上查看";
            this.button_showOnMap.UseVisualStyleBackColor = true;
            this.button_showOnMap.Click += new System.EventHandler(this.button_showOnMap_Click);
            // 
            // BlockAdjustForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(979, 634);
            this.Controls.Add(this.button_showOnMap);
            this.Controls.Add(this.button_saveSnx);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button_solve);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "BlockAdjustForm";
            this.Text = "分区平差";
            this.Load += new System.EventHandler(this.BlockAdjustForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel_parallel.ResumeLayout(false);
            this.panel_parallel.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_rinexPathes;
        private System.Windows.Forms.Button button_setBlockFilesPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_solve;
        private System.Windows.Forms.TextBox textBox_info;
        private System.Windows.Forms.TextBox textBox_degreeOfParallel;
        private System.Windows.Forms.Label label_processorCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel_parallel;
        private System.Windows.Forms.CheckBox checkBox_parallel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox textBox_resultSinex;
        private System.Windows.Forms.Button button_saveSnx;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button_showOnMap;
    }
}