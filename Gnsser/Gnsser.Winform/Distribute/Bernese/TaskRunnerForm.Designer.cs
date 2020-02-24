namespace Gnsser.Winform
{
    partial class TaskRunnerForm
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
            this.textBox_info = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button_run = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_ftpResult = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_resultDir = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_gnssUrlFilePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_solveType = new System.Windows.Forms.ComboBox();
            this.dateTimePicker_date = new System.Windows.Forms.DateTimePicker();
            this.textBox_campaign = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBox_asyn = new System.Windows.Forms.CheckBox();
            this.button_viewState = new System.Windows.Forms.Button();
            this.textBox_taskXml = new System.Windows.Forms.TextBox();
            this.button_genXml = new System.Windows.Forms.Button();
            this.button_parseXml = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_info
            // 
            this.textBox_info.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_info.Location = new System.Drawing.Point(12, 253);
            this.textBox_info.Multiline = true;
            this.textBox_info.Name = "textBox_info";
            this.textBox_info.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_info.Size = new System.Drawing.Size(756, 357);
            this.textBox_info.TabIndex = 3;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(693, 217);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 30);
            this.button_run.TabIndex = 2;
            this.button_run.Text = "运行";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_ftpResult);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox_resultDir);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_gnssUrlFilePath);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(344, 107);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据信息";
            // 
            // textBox_ftpResult
            // 
            this.textBox_ftpResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_ftpResult.Location = new System.Drawing.Point(96, 71);
            this.textBox_ftpResult.Name = "textBox_ftpResult";
            this.textBox_ftpResult.Size = new System.Drawing.Size(236, 21);
            this.textBox_ftpResult.TabIndex = 4;
            this.textBox_ftpResult.Text = "ftp://25.20.220.196/TestData/Result/";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "ftp结果：";
            // 
            // textBox_resultDir
            // 
            this.textBox_resultDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_resultDir.Location = new System.Drawing.Point(96, 47);
            this.textBox_resultDir.Name = "textBox_resultDir";
            this.textBox_resultDir.Size = new System.Drawing.Size(236, 21);
            this.textBox_resultDir.TabIndex = 4;
            this.textBox_resultDir.Text = ".\\Data\\Result\\";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "本地结果目录：";
            // 
            // textBox_gnssUrlFilePath
            // 
            this.textBox_gnssUrlFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_gnssUrlFilePath.Location = new System.Drawing.Point(96, 20);
            this.textBox_gnssUrlFilePath.Name = "textBox_gnssUrlFilePath";
            this.textBox_gnssUrlFilePath.Size = new System.Drawing.Size(236, 21);
            this.textBox_gnssUrlFilePath.TabIndex = 4;
            this.textBox_gnssUrlFilePath.Text = ".\\Data\\GNSS数据\\文件地址.txt";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "观测数据地址：";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.comboBox_solveType);
            this.groupBox2.Controls.Add(this.dateTimePicker_date);
            this.groupBox2.Controls.Add(this.textBox_campaign);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(362, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(406, 107);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "工程任务";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "算法：";
            // 
            // comboBox_solveType
            // 
            this.comboBox_solveType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_solveType.FormattingEnabled = true;
            this.comboBox_solveType.Location = new System.Drawing.Point(88, 74);
            this.comboBox_solveType.Name = "comboBox_solveType";
            this.comboBox_solveType.Size = new System.Drawing.Size(139, 20);
            this.comboBox_solveType.TabIndex = 4;
            // 
            // dateTimePicker_date
            // 
            this.dateTimePicker_date.Location = new System.Drawing.Point(88, 45);
            this.dateTimePicker_date.Name = "dateTimePicker_date";
            this.dateTimePicker_date.Size = new System.Drawing.Size(194, 21);
            this.dateTimePicker_date.TabIndex = 3;
            this.dateTimePicker_date.Value = new System.DateTime(2002, 5, 23, 0, 0, 0, 0);
            // 
            // textBox_campaign
            // 
            this.textBox_campaign.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_campaign.Location = new System.Drawing.Point(88, 19);
            this.textBox_campaign.Name = "textBox_campaign";
            this.textBox_campaign.Size = new System.Drawing.Size(274, 21);
            this.textBox_campaign.TabIndex = 2;
            this.textBox_campaign.Text = "EXAMPLE";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "历元：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(41, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "工程：";
            // 
            // checkBox_asyn
            // 
            this.checkBox_asyn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_asyn.AutoSize = true;
            this.checkBox_asyn.Checked = true;
            this.checkBox_asyn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_asyn.Location = new System.Drawing.Point(639, 225);
            this.checkBox_asyn.Name = "checkBox_asyn";
            this.checkBox_asyn.Size = new System.Drawing.Size(48, 16);
            this.checkBox_asyn.TabIndex = 7;
            this.checkBox_asyn.Text = "异步";
            this.checkBox_asyn.UseVisualStyleBackColor = true;
            // 
            // button_viewState
            // 
            this.button_viewState.Location = new System.Drawing.Point(12, 214);
            this.button_viewState.Name = "button_viewState";
            this.button_viewState.Size = new System.Drawing.Size(96, 33);
            this.button_viewState.TabIndex = 6;
            this.button_viewState.Text = "查看执行状态";
            this.button_viewState.UseVisualStyleBackColor = true;
            this.button_viewState.Click += new System.EventHandler(this.button_viewState_Click);
            // 
            // textBox_taskXml
            // 
            this.textBox_taskXml.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_taskXml.Location = new System.Drawing.Point(13, 127);
            this.textBox_taskXml.Multiline = true;
            this.textBox_taskXml.Name = "textBox_taskXml";
            this.textBox_taskXml.Size = new System.Drawing.Size(642, 81);
            this.textBox_taskXml.TabIndex = 8;
            // 
            // button_genXml
            // 
            this.button_genXml.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_genXml.Location = new System.Drawing.Point(693, 127);
            this.button_genXml.Name = "button_genXml";
            this.button_genXml.Size = new System.Drawing.Size(75, 27);
            this.button_genXml.TabIndex = 9;
            this.button_genXml.Text = "生成XML";
            this.button_genXml.UseVisualStyleBackColor = true;
            this.button_genXml.Click += new System.EventHandler(this.button_genXml_Click);
            // 
            // button_parseXml
            // 
            this.button_parseXml.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_parseXml.Location = new System.Drawing.Point(693, 160);
            this.button_parseXml.Name = "button_parseXml";
            this.button_parseXml.Size = new System.Drawing.Size(75, 28);
            this.button_parseXml.TabIndex = 10;
            this.button_parseXml.Text = "解析XML";
            this.button_parseXml.UseVisualStyleBackColor = true;
            this.button_parseXml.Click += new System.EventHandler(this.button_parseXml_Click);
            // 
            // TaskRunnerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 622);
            this.Controls.Add(this.button_parseXml);
            this.Controls.Add(this.button_genXml);
            this.Controls.Add(this.textBox_taskXml);
            this.Controls.Add(this.checkBox_asyn);
            this.Controls.Add(this.button_viewState);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.textBox_info);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_run);
            this.Name = "TaskRunnerForm";
            this.Text = "Bernese 任务解析执行";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_run;
        private System.Windows.Forms.TextBox textBox_info;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_gnssUrlFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_resultDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_ftpResult;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox_solveType;
        private System.Windows.Forms.DateTimePicker dateTimePicker_date;
        private System.Windows.Forms.TextBox textBox_campaign;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBox_asyn;
        private System.Windows.Forms.Button button_viewState;
        private System.Windows.Forms.TextBox textBox_taskXml;
        private System.Windows.Forms.Button button_genXml;
        private System.Windows.Forms.Button button_parseXml;
    }
}