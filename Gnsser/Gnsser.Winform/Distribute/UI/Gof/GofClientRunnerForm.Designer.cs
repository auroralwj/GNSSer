namespace Gnsser.Winform
{
    partial class GofClientRunnerForm
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
            this.components = new System.ComponentModel.Container();
            this.button_runOrStop = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.button_listenOrStop = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox_nodeName = new System.Windows.Forms.TextBox();
            this.textBox_controlPort = new System.Windows.Forms.TextBox();
            this.textBox_controlIp = new System.Windows.Forms.TextBox();
            this.textBox_listenPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label_state_info = new System.Windows.Forms.Label();
            this.fileOpenControl1 = new Geo.Winform.Controls.FileOpenControl();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.richTextBoxControl1 = new Geo.Winform.Controls.RichTextBoxControl();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_runOrStop
            // 
            this.button_runOrStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_runOrStop.Location = new System.Drawing.Point(609, 115);
            this.button_runOrStop.Name = "button_runOrStop";
            this.button_runOrStop.Size = new System.Drawing.Size(75, 35);
            this.button_runOrStop.TabIndex = 1;
            this.button_runOrStop.Text = "运行/停止";
            this.button_runOrStop.UseVisualStyleBackColor = true;
            this.button_runOrStop.Click += new System.EventHandler(this.button_runOrStop_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // button_listenOrStop
            // 
            this.button_listenOrStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_listenOrStop.Location = new System.Drawing.Point(519, 115);
            this.button_listenOrStop.Name = "button_listenOrStop";
            this.button_listenOrStop.Size = new System.Drawing.Size(84, 35);
            this.button_listenOrStop.TabIndex = 1;
            this.button_listenOrStop.Text = "监听";
            this.button_listenOrStop.UseVisualStyleBackColor = true;
            this.button_listenOrStop.Click += new System.EventHandler(this.button_listenOrStop_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.textBox_nodeName);
            this.groupBox2.Controls.Add(this.textBox_controlPort);
            this.groupBox2.Controls.Add(this.textBox_controlIp);
            this.groupBox2.Controls.Add(this.textBox_listenPort);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(9, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(679, 47);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "网络设置";
            // 
            // textBox_nodeName
            // 
            this.textBox_nodeName.Location = new System.Drawing.Point(552, 18);
            this.textBox_nodeName.Name = "textBox_nodeName";
            this.textBox_nodeName.Size = new System.Drawing.Size(108, 21);
            this.textBox_nodeName.TabIndex = 1;
            this.textBox_nodeName.Text = "127.0.0.1";
            // 
            // textBox_controlPort
            // 
            this.textBox_controlPort.Location = new System.Drawing.Point(407, 18);
            this.textBox_controlPort.Name = "textBox_controlPort";
            this.textBox_controlPort.Size = new System.Drawing.Size(60, 21);
            this.textBox_controlPort.TabIndex = 1;
            this.textBox_controlPort.Text = "10009";
            // 
            // textBox_controlIp
            // 
            this.textBox_controlIp.Location = new System.Drawing.Point(237, 18);
            this.textBox_controlIp.Name = "textBox_controlIp";
            this.textBox_controlIp.Size = new System.Drawing.Size(110, 21);
            this.textBox_controlIp.TabIndex = 1;
            this.textBox_controlIp.Text = "127.0.0.1";
            // 
            // textBox_listenPort
            // 
            this.textBox_listenPort.Location = new System.Drawing.Point(89, 18);
            this.textBox_listenPort.Name = "textBox_listenPort";
            this.textBox_listenPort.Size = new System.Drawing.Size(59, 21);
            this.textBox_listenPort.TabIndex = 1;
            this.textBox_listenPort.Text = "10010";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(481, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "本机标识：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(362, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "端口：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(166, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "控制端IP：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "接收端口：";
            // 
            // label_state_info
            // 
            this.label_state_info.AutoSize = true;
            this.label_state_info.Location = new System.Drawing.Point(8, 158);
            this.label_state_info.Name = "label_state_info";
            this.label_state_info.Size = new System.Drawing.Size(53, 12);
            this.label_state_info.TabIndex = 18;
            this.label_state_info.Text = "状态信息";
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl1.AllowDrop = true;
            this.fileOpenControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl1.FilePath = "";
            this.fileOpenControl1.FilePathes = new string[0];
            this.fileOpenControl1.Filter = "操作流文件(GOF)|*.gof|文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl1.IsMultiSelect = true;
            this.fileOpenControl1.LabelName = "GOF文件：";
            this.fileOpenControl1.Location = new System.Drawing.Point(9, 65);
            this.fileOpenControl1.Margin = new System.Windows.Forms.Padding(4);
            this.fileOpenControl1.Name = "fileOpenControl1";
            this.fileOpenControl1.Size = new System.Drawing.Size(675, 43);
            this.fileOpenControl1.TabIndex = 1;
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(10, 115);
            this.progressBarComponent1.Margin = new System.Windows.Forms.Padding(4);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(502, 35);
            this.progressBarComponent1.TabIndex = 4;
            // 
            // richTextBoxControl1
            // 
            this.richTextBoxControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxControl1.Location = new System.Drawing.Point(12, 181);
            this.richTextBoxControl1.Name = "richTextBoxControl1";
            this.richTextBoxControl1.Size = new System.Drawing.Size(673, 202);
            this.richTextBoxControl1.TabIndex = 3;
            this.richTextBoxControl1.Text = "";
            // 
            // GofClientRunnerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 395);
            this.Controls.Add(this.label_state_info);
            this.Controls.Add(this.fileOpenControl1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.progressBarComponent1);
            this.Controls.Add(this.richTextBoxControl1);
            this.Controls.Add(this.button_listenOrStop);
            this.Controls.Add(this.button_runOrStop);
            this.Name = "GofClientRunnerForm";
            this.Text = "操作流运行器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WorkflowRunnerForm_FormClosing);
            this.Load += new System.EventHandler(this.GofClientRunnerForm_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Geo.Winform.Controls.FileOpenControl fileOpenControl1;
        private System.Windows.Forms.Button button_runOrStop;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl1;
        private Geo.Winform.Controls.ProgressBarComponent progressBarComponent1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button button_listenOrStop;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_controlPort;
        private System.Windows.Forms.TextBox textBox_controlIp;
        private System.Windows.Forms.TextBox textBox_listenPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_state_info;
        private System.Windows.Forms.TextBox textBox_nodeName;
        private System.Windows.Forms.Label label4;
    }
}