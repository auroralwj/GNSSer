namespace Gnsser.Winform
{
    partial class TwoLineEleOrbitForm
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
            this.textBox_count = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_intervalMin = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_solve = new System.Windows.Forms.Button();
            this.button_showOnMap = new System.Windows.Forms.Button();
            this.textBox_show = new System.Windows.Forms.TextBox();
            this.textBox_input = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button_radarCaculate = new System.Windows.Forms.Button();
            this.radioButton_geoCoord = new System.Windows.Forms.RadioButton();
            this.radioButton_xyz = new System.Windows.Forms.RadioButton();
            this.textBox_coord = new System.Windows.Forms.TextBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_count);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_intervalMin);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button_solve);
            this.groupBox1.Controls.Add(this.button_showOnMap);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(2, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(601, 72);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置";
            // 
            // textBox_count
            // 
            this.textBox_count.Location = new System.Drawing.Point(178, 44);
            this.textBox_count.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_count.Name = "textBox_count";
            this.textBox_count.Size = new System.Drawing.Size(61, 21);
            this.textBox_count.TabIndex = 6;
            this.textBox_count.Text = "100";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(106, 52);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "计算次数：";
            // 
            // textBox_intervalMin
            // 
            this.textBox_intervalMin.Location = new System.Drawing.Point(178, 19);
            this.textBox_intervalMin.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_intervalMin.Name = "textBox_intervalMin";
            this.textBox_intervalMin.Size = new System.Drawing.Size(61, 21);
            this.textBox_intervalMin.TabIndex = 6;
            this.textBox_intervalMin.Text = "2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "从观测历元开始间隔（分钟）：";
            // 
            // button_solve
            // 
            this.button_solve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_solve.Location = new System.Drawing.Point(431, 19);
            this.button_solve.Margin = new System.Windows.Forms.Padding(2);
            this.button_solve.Name = "button_solve";
            this.button_solve.Size = new System.Drawing.Size(88, 27);
            this.button_solve.TabIndex = 4;
            this.button_solve.Text = "计算卫星轨迹";
            this.button_solve.UseVisualStyleBackColor = true;
            this.button_solve.Click += new System.EventHandler(this.button_solve_Click);
            // 
            // button_showOnMap
            // 
            this.button_showOnMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_showOnMap.Location = new System.Drawing.Point(523, 19);
            this.button_showOnMap.Margin = new System.Windows.Forms.Padding(2);
            this.button_showOnMap.Name = "button_showOnMap";
            this.button_showOnMap.Size = new System.Drawing.Size(74, 26);
            this.button_showOnMap.TabIndex = 3;
            this.button_showOnMap.Text = "地图显示";
            this.button_showOnMap.UseVisualStyleBackColor = true;
            this.button_showOnMap.Click += new System.EventHandler(this.button_showOnMap_Click);
            // 
            // textBox_show
            // 
            this.textBox_show.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_show.Location = new System.Drawing.Point(2, 16);
            this.textBox_show.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_show.Multiline = true;
            this.textBox_show.Name = "textBox_show";
            this.textBox_show.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_show.Size = new System.Drawing.Size(604, 208);
            this.textBox_show.TabIndex = 1;
            // 
            // textBox_input
            // 
            this.textBox_input.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_input.Location = new System.Drawing.Point(2, 16);
            this.textBox_input.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_input.Multiline = true;
            this.textBox_input.Name = "textBox_input";
            this.textBox_input.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_input.Size = new System.Drawing.Size(604, 64);
            this.textBox_input.TabIndex = 1;
            this.textBox_input.Text = "12388\r\n1 12388U 81033A   14161.84868222  .00000918  00000-0  59467-4 0  4392\r\n2 1" +
    "2388 082.9540 128.5597 0697173 252.5551 099.8292 14.01504380652049";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.textBox_input);
            this.groupBox2.Location = new System.Drawing.Point(12, 108);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(608, 82);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "两行根数";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.textBox_show);
            this.groupBox3.Location = new System.Drawing.Point(10, 195);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(608, 226);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "结果输出";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(9, 1);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(613, 102);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(605, 76);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "卫星轨迹计算";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button_radarCaculate);
            this.tabPage2.Controls.Add(this.radioButton_geoCoord);
            this.tabPage2.Controls.Add(this.radioButton_xyz);
            this.tabPage2.Controls.Add(this.textBox_coord);
            this.tabPage2.Controls.Add(this.dateTimePicker1);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(605, 76);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "雷达（站心坐标）观测计算";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button_radarCaculate
            // 
            this.button_radarCaculate.Location = new System.Drawing.Point(532, 17);
            this.button_radarCaculate.Margin = new System.Windows.Forms.Padding(2);
            this.button_radarCaculate.Name = "button_radarCaculate";
            this.button_radarCaculate.Size = new System.Drawing.Size(56, 43);
            this.button_radarCaculate.TabIndex = 9;
            this.button_radarCaculate.Text = "计算";
            this.button_radarCaculate.UseVisualStyleBackColor = true;
            this.button_radarCaculate.Click += new System.EventHandler(this.button_radarCaculate_Click);
            // 
            // radioButton_geoCoord
            // 
            this.radioButton_geoCoord.AutoSize = true;
            this.radioButton_geoCoord.Checked = true;
            this.radioButton_geoCoord.Location = new System.Drawing.Point(289, 16);
            this.radioButton_geoCoord.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton_geoCoord.Name = "radioButton_geoCoord";
            this.radioButton_geoCoord.Size = new System.Drawing.Size(95, 16);
            this.radioButton_geoCoord.TabIndex = 8;
            this.radioButton_geoCoord.TabStop = true;
            this.radioButton_geoCoord.Text = "大地坐标(度)";
            this.radioButton_geoCoord.UseVisualStyleBackColor = true;
            // 
            // radioButton_xyz
            // 
            this.radioButton_xyz.AutoSize = true;
            this.radioButton_xyz.Location = new System.Drawing.Point(392, 17);
            this.radioButton_xyz.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton_xyz.Name = "radioButton_xyz";
            this.radioButton_xyz.Size = new System.Drawing.Size(119, 16);
            this.radioButton_xyz.TabIndex = 8;
            this.radioButton_xyz.Text = "空间直角坐标(米)";
            this.radioButton_xyz.UseVisualStyleBackColor = true;
            // 
            // textBox_coord
            // 
            this.textBox_coord.Location = new System.Drawing.Point(70, 46);
            this.textBox_coord.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_coord.Name = "textBox_coord";
            this.textBox_coord.Size = new System.Drawing.Size(289, 21);
            this.textBox_coord.TabIndex = 7;
            this.textBox_coord.Text = "123.2075,   43.1904,   186.0";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(70, 13);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(2);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(192, 21);
            this.dateTimePicker1.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 48);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "测站坐标：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 16);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "观测时间：";
            // 
            // TwoLineEleOrbitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 430);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TwoLineEleOrbitForm";
            this.Text = "两行根数轨道计算";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_show;
        private System.Windows.Forms.Button button_showOnMap;
        private System.Windows.Forms.TextBox textBox_input;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_solve;
        private System.Windows.Forms.TextBox textBox_count;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_intervalMin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radioButton_geoCoord;
        private System.Windows.Forms.RadioButton radioButton_xyz;
        private System.Windows.Forms.TextBox textBox_coord;
        private System.Windows.Forms.Button button_radarCaculate;
    }
}