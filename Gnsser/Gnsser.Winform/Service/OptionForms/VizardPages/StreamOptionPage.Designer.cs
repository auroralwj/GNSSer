namespace Gnsser.Winform
{
    partial class StreamOptionPage
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_stream = new System.Windows.Forms.TabPage();
            this.namedIntControl1ExtraStreamLoopCount = new Geo.Winform.Controls.NamedIntControl();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.checkBox1IsOnlySameParam = new System.Windows.Forms.CheckBox();
            this.checkBox1TopSpeedModel = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.namedIntControlOrdinalAndReverseCount = new Geo.Winform.Controls.NamedIntControl();
            this.checkBox1IsClearOutBufferWhenReversing = new System.Windows.Forms.CheckBox();
            this.checkBox_isReversed = new System.Windows.Forms.CheckBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_bufferSize = new System.Windows.Forms.TextBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_startEpoch = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox_caculateCount = new System.Windows.Forms.TextBox();
            this.openFileDialog_nav = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_obs = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_clock = new System.Windows.Forms.OpenFileDialog();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.namedArrayControl1 = new Geo.Winform.Controls.NamedArrayControl();
            this.namedArrayControl2 = new Geo.Winform.Controls.NamedArrayControl();
            this.tabControl1.SuspendLayout();
            this.tabPage_stream.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_stream);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(671, 473);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage_stream
            // 
            this.tabPage_stream.Controls.Add(this.namedIntControl1ExtraStreamLoopCount);
            this.tabPage_stream.Controls.Add(this.groupBox6);
            this.tabPage_stream.Controls.Add(this.groupBox2);
            this.tabPage_stream.Controls.Add(this.groupBox10);
            this.tabPage_stream.Controls.Add(this.groupBox11);
            this.tabPage_stream.Location = new System.Drawing.Point(4, 22);
            this.tabPage_stream.Name = "tabPage_stream";
            this.tabPage_stream.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_stream.Size = new System.Drawing.Size(663, 447);
            this.tabPage_stream.TabIndex = 12;
            this.tabPage_stream.Text = "流程控制";
            this.tabPage_stream.UseVisualStyleBackColor = true;
            // 
            // namedIntControl1ExtraStreamLoopCount
            // 
            this.namedIntControl1ExtraStreamLoopCount.Location = new System.Drawing.Point(13, 161);
            this.namedIntControl1ExtraStreamLoopCount.Margin = new System.Windows.Forms.Padding(4);
            this.namedIntControl1ExtraStreamLoopCount.Name = "namedIntControl1ExtraStreamLoopCount";
            this.namedIntControl1ExtraStreamLoopCount.Size = new System.Drawing.Size(184, 23);
            this.namedIntControl1ExtraStreamLoopCount.TabIndex = 71;
            this.namedIntControl1ExtraStreamLoopCount.Title = "数据流额外循环次数：";
            this.namedIntControl1ExtraStreamLoopCount.Value = 0;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.checkBox1IsOnlySameParam);
            this.groupBox6.Controls.Add(this.checkBox1TopSpeedModel);
            this.groupBox6.Location = new System.Drawing.Point(218, 126);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(228, 81);
            this.groupBox6.TabIndex = 75;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "软件测试";
            // 
            // checkBox1IsOnlySameParam
            // 
            this.checkBox1IsOnlySameParam.AutoSize = true;
            this.checkBox1IsOnlySameParam.Location = new System.Drawing.Point(18, 48);
            this.checkBox1IsOnlySameParam.Name = "checkBox1IsOnlySameParam";
            this.checkBox1IsOnlySameParam.Size = new System.Drawing.Size(144, 16);
            this.checkBox1IsOnlySameParam.TabIndex = 75;
            this.checkBox1IsOnlySameParam.Text = "只允许相同参数(卫星)";
            this.checkBox1IsOnlySameParam.UseVisualStyleBackColor = true;
            // 
            // checkBox1TopSpeedModel
            // 
            this.checkBox1TopSpeedModel.AutoSize = true;
            this.checkBox1TopSpeedModel.Location = new System.Drawing.Point(18, 26);
            this.checkBox1TopSpeedModel.Name = "checkBox1TopSpeedModel";
            this.checkBox1TopSpeedModel.Size = new System.Drawing.Size(72, 16);
            this.checkBox1TopSpeedModel.TabIndex = 74;
            this.checkBox1TopSpeedModel.Text = "极速模式";
            this.checkBox1TopSpeedModel.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.namedIntControlOrdinalAndReverseCount);
            this.groupBox2.Controls.Add(this.checkBox1IsClearOutBufferWhenReversing);
            this.groupBox2.Controls.Add(this.checkBox_isReversed);
            this.groupBox2.Location = new System.Drawing.Point(218, 21);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(228, 99);
            this.groupBox2.TabIndex = 73;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "正反数据流";
            // 
            // namedIntControlOrdinalAndReverseCount
            // 
            this.namedIntControlOrdinalAndReverseCount.Location = new System.Drawing.Point(15, 70);
            this.namedIntControlOrdinalAndReverseCount.Margin = new System.Windows.Forms.Padding(4);
            this.namedIntControlOrdinalAndReverseCount.Name = "namedIntControlOrdinalAndReverseCount";
            this.namedIntControlOrdinalAndReverseCount.Size = new System.Drawing.Size(198, 23);
            this.namedIntControlOrdinalAndReverseCount.TabIndex = 71;
            this.namedIntControlOrdinalAndReverseCount.Title = "首次后，正反算次数：";
            this.namedIntControlOrdinalAndReverseCount.Value = 0;
            // 
            // checkBox1IsClearOutBufferWhenReversing
            // 
            this.checkBox1IsClearOutBufferWhenReversing.AutoSize = true;
            this.checkBox1IsClearOutBufferWhenReversing.Location = new System.Drawing.Point(18, 47);
            this.checkBox1IsClearOutBufferWhenReversing.Name = "checkBox1IsClearOutBufferWhenReversing";
            this.checkBox1IsClearOutBufferWhenReversing.Size = new System.Drawing.Size(132, 16);
            this.checkBox1IsClearOutBufferWhenReversing.TabIndex = 70;
            this.checkBox1IsClearOutBufferWhenReversing.Text = "清空上回合输出缓存";
            this.checkBox1IsClearOutBufferWhenReversing.UseVisualStyleBackColor = true;
            // 
            // checkBox_isReversed
            // 
            this.checkBox_isReversed.AutoSize = true;
            this.checkBox_isReversed.Location = new System.Drawing.Point(18, 20);
            this.checkBox_isReversed.Name = "checkBox_isReversed";
            this.checkBox_isReversed.Size = new System.Drawing.Size(132, 16);
            this.checkBox_isReversed.TabIndex = 70;
            this.checkBox_isReversed.Text = "首次是否逆序数据流";
            this.checkBox_isReversed.UseVisualStyleBackColor = true;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.label5);
            this.groupBox10.Controls.Add(this.textBox_bufferSize);
            this.groupBox10.Location = new System.Drawing.Point(3, 94);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(188, 48);
            this.groupBox10.TabIndex = 68;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "缓存设置";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "缓存历元大小：";
            // 
            // textBox_bufferSize
            // 
            this.textBox_bufferSize.Location = new System.Drawing.Point(106, 17);
            this.textBox_bufferSize.Name = "textBox_bufferSize";
            this.textBox_bufferSize.Size = new System.Drawing.Size(49, 21);
            this.textBox_bufferSize.TabIndex = 2;
            this.textBox_bufferSize.Text = "4";
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.label11);
            this.groupBox11.Controls.Add(this.textBox_startEpoch);
            this.groupBox11.Controls.Add(this.label10);
            this.groupBox11.Controls.Add(this.textBox_caculateCount);
            this.groupBox11.Location = new System.Drawing.Point(5, 6);
            this.groupBox11.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox11.Size = new System.Drawing.Size(186, 83);
            this.groupBox11.TabIndex = 31;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "计算数量";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 19);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 17;
            this.label11.Text = "起始历元：";
            // 
            // textBox_startEpoch
            // 
            this.textBox_startEpoch.Location = new System.Drawing.Point(87, 15);
            this.textBox_startEpoch.Name = "textBox_startEpoch";
            this.textBox_startEpoch.Size = new System.Drawing.Size(66, 21);
            this.textBox_startEpoch.TabIndex = 18;
            this.textBox_startEpoch.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 51);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 17;
            this.label10.Text = "计算历元数：";
            // 
            // textBox_caculateCount
            // 
            this.textBox_caculateCount.Location = new System.Drawing.Point(87, 47);
            this.textBox_caculateCount.Name = "textBox_caculateCount";
            this.textBox_caculateCount.Size = new System.Drawing.Size(66, 21);
            this.textBox_caculateCount.TabIndex = 18;
            this.textBox_caculateCount.Text = "10000";
            // 
            // openFileDialog_nav
            // 
            this.openFileDialog_nav.FileName = "星历文件";
            this.openFileDialog_nav.Filter = "Rinex导航、星历文件|*.*N;*.*P;*.*R;*.*G;*.SP3;*.EPH|所有文件|*.*";
            // 
            // openFileDialog_obs
            // 
            this.openFileDialog_obs.Filter = "Rinex观测文件,压缩文件（*.*O;*.*D;*.*D.Z;|*.*o;*.*D.Z;*.*D|所有文件|*.*";
            // 
            // openFileDialog_clock
            // 
            this.openFileDialog_clock.FileName = "钟差文件";
            this.openFileDialog_clock.Filter = "Clock卫星钟差文件|*.clk;*.clk_30s|所有文件|*.*";
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(526, 42);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(48, 16);
            this.checkBox1.TabIndex = 47;
            this.checkBox1.Text = "启用";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // namedArrayControl1
            // 
            this.namedArrayControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.namedArrayControl1.Location = new System.Drawing.Point(15, 24);
            this.namedArrayControl1.Margin = new System.Windows.Forms.Padding(4);
            this.namedArrayControl1.Name = "namedArrayControl1";
            this.namedArrayControl1.Size = new System.Drawing.Size(508, 23);
            this.namedArrayControl1.TabIndex = 71;
            this.namedArrayControl1.Title = "初始先验值：";
            // 
            // namedArrayControl2
            // 
            this.namedArrayControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.namedArrayControl2.Location = new System.Drawing.Point(15, 49);
            this.namedArrayControl2.Margin = new System.Windows.Forms.Padding(4);
            this.namedArrayControl2.Name = "namedArrayControl2";
            this.namedArrayControl2.Size = new System.Drawing.Size(508, 23);
            this.namedArrayControl2.TabIndex = 71;
            this.namedArrayControl2.Title = "初始先验值（RMS）：";
            // 
            // StreamOptionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "StreamOptionPage";
            this.Size = new System.Drawing.Size(671, 473);
            this.Load += new System.EventHandler(this.GnssOptionForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_stream.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TextBox textBox_bufferSize;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox_startEpoch;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox_caculateCount;
        private System.Windows.Forms.OpenFileDialog openFileDialog_nav;
        private System.Windows.Forms.OpenFileDialog openFileDialog_obs;
        private System.Windows.Forms.OpenFileDialog openFileDialog_clock;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.TabPage tabPage_stream;
        private System.Windows.Forms.CheckBox checkBox_isReversed;
        private System.Windows.Forms.GroupBox groupBox2;
        private Geo.Winform.Controls.NamedIntControl namedIntControlOrdinalAndReverseCount;
        private System.Windows.Forms.CheckBox checkBox1IsClearOutBufferWhenReversing;
        private System.Windows.Forms.CheckBox checkBox1TopSpeedModel;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox checkBox1IsOnlySameParam;
        private Geo.Winform.Controls.NamedIntControl namedIntControl1ExtraStreamLoopCount;
        private System.Windows.Forms.CheckBox checkBox1;
        private Geo.Winform.Controls.NamedArrayControl namedArrayControl1;
        private Geo.Winform.Controls.NamedArrayControl namedArrayControl2;
    }
}