namespace Gnsser.Winform
{
    partial class KelplerOrbitForm
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox_epochCount = new System.Windows.Forms.TextBox();
            this.textBox_interval = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_M = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_omiga = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBox_i = new System.Windows.Forms.TextBox();
            this.textBox_Ω = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_e = new System.Windows.Forms.TextBox();
            this.textBox_a = new System.Windows.Forms.TextBox();
            this.button_showOnMap = new System.Windows.Forms.Button();
            this.button_caculate = new System.Windows.Forms.Button();
            this.tabPage_result = new System.Windows.Forms.TabPage();
            this.tabPage_info = new System.Windows.Forms.TabPage();
            this.tabControl_result = new System.Windows.Forms.TabControl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.textBox_pos = new System.Windows.Forms.TextBox();
            this.textBox_vel = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.button_solveOrbitParam = new System.Windows.Forms.Button();
            this.RichTextBoxControl_processInfo = new Geo.Winform.Controls.RichTextBoxControl();
            this.textBox_show = new Geo.Winform.Controls.RichTextBoxControl();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage_result.SuspendLayout();
            this.tabPage_info.SuspendLayout();
            this.tabControl_result.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.textBox_epochCount);
            this.groupBox3.Controls.Add(this.textBox_interval);
            this.groupBox3.Location = new System.Drawing.Point(6, 163);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(280, 47);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "历元与外推信息";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 17);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 1;
            this.label8.Text = "历元次数：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(133, 17);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 12);
            this.label9.TabIndex = 1;
            this.label9.Text = "采样率(秒)：";
            // 
            // textBox_epochCount
            // 
            this.textBox_epochCount.Location = new System.Drawing.Point(71, 14);
            this.textBox_epochCount.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_epochCount.Name = "textBox_epochCount";
            this.textBox_epochCount.Size = new System.Drawing.Size(54, 21);
            this.textBox_epochCount.TabIndex = 2;
            this.textBox_epochCount.Text = "100";
            // 
            // textBox_interval
            // 
            this.textBox_interval.Location = new System.Drawing.Point(214, 14);
            this.textBox_interval.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_interval.Name = "textBox_interval";
            this.textBox_interval.Size = new System.Drawing.Size(54, 21);
            this.textBox_interval.TabIndex = 2;
            this.textBox_interval.Text = "10";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox6);
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(628, 151);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "开普勒轨道6参数";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Controls.Add(this.textBox_M);
            this.groupBox6.Location = new System.Drawing.Point(290, 103);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(287, 43);
            this.groupBox6.TabIndex = 17;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "时间参数";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 21);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(149, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "参考时刻平近点角M(rad)：";
            // 
            // textBox_M
            // 
            this.textBox_M.Location = new System.Drawing.Point(169, 16);
            this.textBox_M.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_M.Name = "textBox_M";
            this.textBox_M.Size = new System.Drawing.Size(100, 21);
            this.textBox_M.TabIndex = 13;
            this.textBox_M.Text = "1.0";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.textBox_omiga);
            this.groupBox5.Location = new System.Drawing.Point(13, 102);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(252, 44);
            this.groupBox5.TabIndex = 16;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "轨道椭圆定向参数";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 17);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "近升角距ω(rad)：";
            // 
            // textBox_omiga
            // 
            this.textBox_omiga.Location = new System.Drawing.Point(116, 14);
            this.textBox_omiga.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_omiga.Name = "textBox_omiga";
            this.textBox_omiga.Size = new System.Drawing.Size(100, 21);
            this.textBox_omiga.TabIndex = 11;
            this.textBox_omiga.Text = "1.0";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBox_i);
            this.groupBox4.Controls.Add(this.textBox_Ω);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Location = new System.Drawing.Point(290, 23);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(287, 73);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "平面参数";
            // 
            // textBox_i
            // 
            this.textBox_i.Location = new System.Drawing.Point(148, 19);
            this.textBox_i.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_i.Name = "textBox_i";
            this.textBox_i.Size = new System.Drawing.Size(100, 21);
            this.textBox_i.TabIndex = 5;
            this.textBox_i.Text = "0.8";
            // 
            // textBox_Ω
            // 
            this.textBox_Ω.Location = new System.Drawing.Point(148, 47);
            this.textBox_Ω.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_Ω.Name = "textBox_Ω";
            this.textBox_Ω.Size = new System.Drawing.Size(100, 21);
            this.textBox_Ω.TabIndex = 7;
            this.textBox_Ω.Text = "1.0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 50);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "升交点赤经Ω(rad)：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 22);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "轨道平面倾角i(rad)：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBox_e);
            this.groupBox1.Controls.Add(this.textBox_a);
            this.groupBox1.Location = new System.Drawing.Point(13, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(252, 76);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "轨道椭圆形状参数";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "轨道长半径a(米)：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 46);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "轨道椭圆离心率e：";
            // 
            // textBox_e
            // 
            this.textBox_e.Location = new System.Drawing.Point(122, 43);
            this.textBox_e.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_e.Name = "textBox_e";
            this.textBox_e.Size = new System.Drawing.Size(100, 21);
            this.textBox_e.TabIndex = 9;
            this.textBox_e.Text = "0.01";
            // 
            // textBox_a
            // 
            this.textBox_a.Location = new System.Drawing.Point(122, 15);
            this.textBox_a.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_a.Name = "textBox_a";
            this.textBox_a.Size = new System.Drawing.Size(100, 21);
            this.textBox_a.TabIndex = 2;
            this.textBox_a.Text = "20000000";
            // 
            // button_showOnMap
            // 
            this.button_showOnMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_showOnMap.Location = new System.Drawing.Point(537, 165);
            this.button_showOnMap.Margin = new System.Windows.Forms.Padding(2);
            this.button_showOnMap.Name = "button_showOnMap";
            this.button_showOnMap.Size = new System.Drawing.Size(76, 33);
            this.button_showOnMap.TabIndex = 3;
            this.button_showOnMap.Text = "地图显示";
            this.button_showOnMap.UseVisualStyleBackColor = true;
            this.button_showOnMap.Click += new System.EventHandler(this.button_showOnMap_Click);
            // 
            // button_caculate
            // 
            this.button_caculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_caculate.Location = new System.Drawing.Point(446, 164);
            this.button_caculate.Margin = new System.Windows.Forms.Padding(2);
            this.button_caculate.Name = "button_caculate";
            this.button_caculate.Size = new System.Drawing.Size(76, 34);
            this.button_caculate.TabIndex = 0;
            this.button_caculate.Text = "计算";
            this.button_caculate.UseVisualStyleBackColor = true;
            this.button_caculate.Click += new System.EventHandler(this.button_caculate_Click);
            // 
            // tabPage_result
            // 
            this.tabPage_result.Controls.Add(this.textBox_show);
            this.tabPage_result.Location = new System.Drawing.Point(4, 22);
            this.tabPage_result.Name = "tabPage_result";
            this.tabPage_result.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_result.Size = new System.Drawing.Size(627, 251);
            this.tabPage_result.TabIndex = 5;
            this.tabPage_result.Text = "计算结果";
            this.tabPage_result.UseVisualStyleBackColor = true;
            // 
            // tabPage_info
            // 
            this.tabPage_info.Controls.Add(this.RichTextBoxControl_processInfo);
            this.tabPage_info.Location = new System.Drawing.Point(4, 22);
            this.tabPage_info.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage_info.Name = "tabPage_info";
            this.tabPage_info.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage_info.Size = new System.Drawing.Size(627, 251);
            this.tabPage_info.TabIndex = 4;
            this.tabPage_info.Text = "处理信息";
            this.tabPage_info.UseVisualStyleBackColor = true;
            // 
            // tabControl_result
            // 
            this.tabControl_result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_result.Controls.Add(this.tabPage_info);
            this.tabControl_result.Controls.Add(this.tabPage_result);
            this.tabControl_result.Location = new System.Drawing.Point(10, 276);
            this.tabControl_result.Name = "tabControl_result";
            this.tabControl_result.SelectedIndex = 0;
            this.tabControl_result.Size = new System.Drawing.Size(635, 277);
            this.tabControl_result.TabIndex = 25;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(10, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(637, 245);
            this.tabControl1.TabIndex = 16;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button_showOnMap);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.button_caculate);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(629, 219);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "计算卫星坐标和速度";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox7);
            this.tabPage2.Controls.Add(this.button_solveOrbitParam);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(629, 219);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "计算轨道根数";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox7.Controls.Add(this.textBox_pos);
            this.groupBox7.Controls.Add(this.textBox_vel);
            this.groupBox7.Controls.Add(this.label2);
            this.groupBox7.Controls.Add(this.label10);
            this.groupBox7.Location = new System.Drawing.Point(17, 15);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(586, 73);
            this.groupBox7.TabIndex = 16;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "参考时刻卫星飞行状态";
            // 
            // textBox_pos
            // 
            this.textBox_pos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_pos.Location = new System.Drawing.Point(94, 19);
            this.textBox_pos.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_pos.Name = "textBox_pos";
            this.textBox_pos.Size = new System.Drawing.Size(474, 21);
            this.textBox_pos.TabIndex = 5;
            this.textBox_pos.Text = "-15158942.403045557, -467549.49942087941, 12873768.692849424";
            // 
            // textBox_vel
            // 
            this.textBox_vel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_vel.Location = new System.Drawing.Point(94, 47);
            this.textBox_vel.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_vel.Name = "textBox_vel";
            this.textBox_vel.Size = new System.Drawing.Size(474, 21);
            this.textBox_vel.TabIndex = 7;
            this.textBox_vel.Text = "-1081.1457851615357, -4136.7406242091811, -1364.6187861984311";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 50);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "速度(m/s)：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 22);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(83, 12);
            this.label10.TabIndex = 4;
            this.label10.Text = "位置向径(m)：";
            // 
            // button_solveOrbitParam
            // 
            this.button_solveOrbitParam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_solveOrbitParam.Location = new System.Drawing.Point(484, 94);
            this.button_solveOrbitParam.Name = "button_solveOrbitParam";
            this.button_solveOrbitParam.Size = new System.Drawing.Size(119, 39);
            this.button_solveOrbitParam.TabIndex = 0;
            this.button_solveOrbitParam.Text = "计算轨道根数";
            this.button_solveOrbitParam.UseVisualStyleBackColor = true;
            this.button_solveOrbitParam.Click += new System.EventHandler(this.button_solveOrbitParam_Click);
            // 
            // RichTextBoxControl_processInfo
            // 
            this.RichTextBoxControl_processInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RichTextBoxControl_processInfo.Location = new System.Drawing.Point(2, 2);
            this.RichTextBoxControl_processInfo.Name = "RichTextBoxControl_processInfo";
            this.RichTextBoxControl_processInfo.Size = new System.Drawing.Size(623, 247);
            this.RichTextBoxControl_processInfo.TabIndex = 0;
            this.RichTextBoxControl_processInfo.Text = "";
            // 
            // textBox_show
            // 
            this.textBox_show.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_show.Location = new System.Drawing.Point(3, 3);
            this.textBox_show.Name = "textBox_show";
            this.textBox_show.Size = new System.Drawing.Size(621, 245);
            this.textBox_show.TabIndex = 1;
            this.textBox_show.Text = "";
            // 
            // KelplerOrbitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 565);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.tabControl_result);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "KelplerOrbitForm";
            this.Text = "开普勒轨道计算";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage_result.ResumeLayout(false);
            this.tabPage_info.ResumeLayout(false);
            this.tabControl_result.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_a;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_caculate;
        private System.Windows.Forms.Button button_showOnMap;
        private System.Windows.Forms.TextBox textBox_i;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_M;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_omiga;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_e;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_Ω;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_interval;
        private System.Windows.Forms.TextBox textBox_epochCount;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox3;
        protected System.Windows.Forms.TabPage tabPage_result;
        protected Geo.Winform.Controls.RichTextBoxControl textBox_show;
        protected System.Windows.Forms.TabPage tabPage_info;
        protected Geo.Winform.Controls.RichTextBoxControl RichTextBoxControl_processInfo;
        protected System.Windows.Forms.TabControl tabControl_result;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox textBox_pos;
        private System.Windows.Forms.TextBox textBox_vel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button_solveOrbitParam;
    }
}