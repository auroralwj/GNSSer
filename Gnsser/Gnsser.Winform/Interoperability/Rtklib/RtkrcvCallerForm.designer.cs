namespace Gnsser.Winform
{
    partial class RtkrcvCallerForm
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

        #region Windows Form Designer generated obsCode

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the obsCode editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button_start = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.richTextBoxControl_info = new Geo.Winform.Controls.RichTextBoxControl();
            this.richTextBoxControl_result = new Geo.Winform.Controls.RichTextBoxControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox_stateInfo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_stop = new System.Windows.Forms.Button();
            this.button_readConfig = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button_openHisDir = new System.Windows.Forms.Button();
            this.checkBox_showResult = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_startFromPipe = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_start
            // 
            this.button_start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_start.Location = new System.Drawing.Point(660, 10);
            this.button_start.Margin = new System.Windows.Forms.Padding(2);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(80, 38);
            this.button_start.TabIndex = 2;
            this.button_start.Text = "直接启动";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.splitContainer1);
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Location = new System.Drawing.Point(9, 66);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(807, 324);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "结果";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(2, 42);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.richTextBoxControl_info);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.richTextBoxControl_result);
            this.splitContainer1.Size = new System.Drawing.Size(803, 280);
            this.splitContainer1.SplitterDistance = 76;
            this.splitContainer1.TabIndex = 5;
            // 
            // richTextBoxControl_info
            // 
            this.richTextBoxControl_info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_info.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxControl_info.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBoxControl_info.Name = "richTextBoxControl_info";
            this.richTextBoxControl_info.Size = new System.Drawing.Size(803, 76);
            this.richTextBoxControl_info.TabIndex = 1;
            this.richTextBoxControl_info.Text = "";
            // 
            // richTextBoxControl_result
            // 
            this.richTextBoxControl_result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_result.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxControl_result.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBoxControl_result.Name = "richTextBoxControl_result";
            this.richTextBoxControl_result.Size = new System.Drawing.Size(803, 200);
            this.richTextBoxControl_result.TabIndex = 0;
            this.richTextBoxControl_result.Text = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBox_stateInfo);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(2, 16);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(803, 26);
            this.panel1.TabIndex = 4;
            // 
            // textBox_stateInfo
            // 
            this.textBox_stateInfo.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_stateInfo.ForeColor = System.Drawing.Color.Red;
            this.textBox_stateInfo.Location = new System.Drawing.Point(69, 3);
            this.textBox_stateInfo.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_stateInfo.Name = "textBox_stateInfo";
            this.textBox_stateInfo.Size = new System.Drawing.Size(367, 21);
            this.textBox_stateInfo.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "当前状态：";
            // 
            // button_stop
            // 
            this.button_stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_stop.Location = new System.Drawing.Point(744, 10);
            this.button_stop.Margin = new System.Windows.Forms.Padding(2);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(69, 38);
            this.button_stop.TabIndex = 3;
            this.button_stop.Text = "停止";
            this.button_stop.UseVisualStyleBackColor = true;
            this.button_stop.Click += new System.EventHandler(this.button_stop_Click);
            // 
            // button_readConfig
            // 
            this.button_readConfig.Location = new System.Drawing.Point(16, 10);
            this.button_readConfig.Margin = new System.Windows.Forms.Padding(2);
            this.button_readConfig.Name = "button_readConfig";
            this.button_readConfig.Size = new System.Drawing.Size(90, 26);
            this.button_readConfig.TabIndex = 4;
            this.button_readConfig.Text = "修改配置文件";
            this.button_readConfig.UseVisualStyleBackColor = true;
            this.button_readConfig.Click += new System.EventHandler(this.button_readConfig_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button_openHisDir
            // 
            this.button_openHisDir.Location = new System.Drawing.Point(111, 10);
            this.button_openHisDir.Margin = new System.Windows.Forms.Padding(2);
            this.button_openHisDir.Name = "button_openHisDir";
            this.button_openHisDir.Size = new System.Drawing.Size(95, 26);
            this.button_openHisDir.TabIndex = 2;
            this.button_openHisDir.Text = "查看历史结果";
            this.button_openHisDir.UseVisualStyleBackColor = true;
            this.button_openHisDir.Click += new System.EventHandler(this.button_openHisDir_Click);
            // 
            // checkBox_showResult
            // 
            this.checkBox_showResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_showResult.AutoSize = true;
            this.checkBox_showResult.Checked = true;
            this.checkBox_showResult.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_showResult.Location = new System.Drawing.Point(234, 16);
            this.checkBox_showResult.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_showResult.Name = "checkBox_showResult";
            this.checkBox_showResult.Size = new System.Drawing.Size(186, 16);
            this.checkBox_showResult.TabIndex = 5;
            this.checkBox_showResult.Text = "实时输出（显示最新1万字符）";
            this.checkBox_showResult.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 46);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(329, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "注意：实时计算停止运行后，将自动把结果保存到历史目录。";
            // 
            // button_startFromPipe
            // 
            this.button_startFromPipe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_startFromPipe.Location = new System.Drawing.Point(576, 10);
            this.button_startFromPipe.Margin = new System.Windows.Forms.Padding(2);
            this.button_startFromPipe.Name = "button_startFromPipe";
            this.button_startFromPipe.Size = new System.Drawing.Size(80, 38);
            this.button_startFromPipe.TabIndex = 2;
            this.button_startFromPipe.Text = "管道启动";
            this.button_startFromPipe.UseVisualStyleBackColor = true;
            this.button_startFromPipe.Click += new System.EventHandler(this.button_startFromPipe_Click);
            // 
            // RtkrcvCallerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 403);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBox_showResult);
            this.Controls.Add(this.button_readConfig);
            this.Controls.Add(this.button_stop);
            this.Controls.Add(this.button_openHisDir);
            this.Controls.Add(this.button_startFromPipe);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.groupBox2);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "RtkrcvCallerForm";
            this.Text = "Rtklib实时计算调用器";
            this.groupBox2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.GroupBox groupBox2;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl_result;
        private System.Windows.Forms.Button button_stop;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox_stateInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_readConfig;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button_openHisDir;
        private System.Windows.Forms.CheckBox checkBox_showResult;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl_info;
        private System.Windows.Forms.Button button_startFromPipe;
    }
}