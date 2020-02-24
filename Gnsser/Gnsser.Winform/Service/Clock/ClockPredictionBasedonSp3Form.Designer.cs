namespace Gnsser.Winform
{
    partial class ClockPredictionBasedonSp3Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClockPredictionBasedonSp3Form));
            this.radioButton_RobustDLP = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton_QPGM = new System.Windows.Forms.RadioButton();
            this.radioButton_KFReHardamard = new System.Windows.Forms.RadioButton();
            this.radioButton_KFReAllan = new System.Windows.Forms.RadioButton();
            this.radioButton_KFHardamard = new System.Windows.Forms.RadioButton();
            this.radioButton_QPT4 = new System.Windows.Forms.RadioButton();
            this.radioButton_QPT3 = new System.Windows.Forms.RadioButton();
            this.radioButton_QPT4GM = new System.Windows.Forms.RadioButton();
            this.radioButton_QPT2 = new System.Windows.Forms.RadioButton();
            this.radioButton_QPT1 = new System.Windows.Forms.RadioButton();
            this.radioButton_QPT2GM = new System.Windows.Forms.RadioButton();
            this.radioButton_GM = new System.Windows.Forms.RadioButton();
            this.radioButton_QP = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_PredictedLength = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_ModelLength = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_getPath = new System.Windows.Forms.Button();
            this.textBox_interval = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox_PredictedNumber = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox_satPrns = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.IsSelectedPrn = new System.Windows.Forms.RadioButton();
            this.textBox_Pathes = new System.Windows.Forms.TextBox();
            this.button_export = new System.Windows.Forms.Button();
            this.openFileDialog_sp3 = new System.Windows.Forms.OpenFileDialog();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox1_GlideEpochNumber = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.radioButton_DLP = new System.Windows.Forms.RadioButton();
            this.radioButton_LP = new System.Windows.Forms.RadioButton();
            this.radioButton_RobustLP = new System.Windows.Forms.RadioButton();
            this.radioButton_KFAllan = new System.Windows.Forms.RadioButton();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButton_RobustDLP
            // 
            this.radioButton_RobustDLP.AutoSize = true;
            this.radioButton_RobustDLP.Location = new System.Drawing.Point(21, 55);
            this.radioButton_RobustDLP.Name = "radioButton_RobustDLP";
            this.radioButton_RobustDLP.Size = new System.Drawing.Size(53, 16);
            this.radioButton_RobustDLP.TabIndex = 42;
            this.radioButton_RobustDLP.Text = "R-DLP";
            this.radioButton_RobustDLP.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton_KFAllan);
            this.groupBox2.Controls.Add(this.radioButton_QPGM);
            this.groupBox2.Controls.Add(this.radioButton_KFReHardamard);
            this.groupBox2.Controls.Add(this.radioButton_KFReAllan);
            this.groupBox2.Controls.Add(this.radioButton_KFHardamard);
            this.groupBox2.Controls.Add(this.radioButton_QPT4);
            this.groupBox2.Controls.Add(this.radioButton_QPT3);
            this.groupBox2.Controls.Add(this.radioButton_QPT4GM);
            this.groupBox2.Controls.Add(this.radioButton_QPT2);
            this.groupBox2.Controls.Add(this.radioButton_QPT1);
            this.groupBox2.Controls.Add(this.radioButton_QPT2GM);
            this.groupBox2.Controls.Add(this.radioButton_GM);
            this.groupBox2.Controls.Add(this.radioButton_DLP);
            this.groupBox2.Controls.Add(this.radioButton_RobustLP);
            this.groupBox2.Controls.Add(this.radioButton_RobustDLP);
            this.groupBox2.Controls.Add(this.radioButton_LP);
            this.groupBox2.Controls.Add(this.radioButton_QP);
            this.groupBox2.Location = new System.Drawing.Point(74, 261);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(406, 100);
            this.groupBox2.TabIndex = 50;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "预报模型";
            // 
            // radioButton_QPGM
            // 
            this.radioButton_QPGM.AutoSize = true;
            this.radioButton_QPGM.Location = new System.Drawing.Point(121, 60);
            this.radioButton_QPGM.Name = "radioButton_QPGM";
            this.radioButton_QPGM.Size = new System.Drawing.Size(47, 16);
            this.radioButton_QPGM.TabIndex = 55;
            this.radioButton_QPGM.Text = "QPGM";
            this.radioButton_QPGM.UseVisualStyleBackColor = true;
            // 
            // radioButton_KFReHardamard
            // 
            this.radioButton_KFReHardamard.AutoSize = true;
            this.radioButton_KFReHardamard.Location = new System.Drawing.Point(286, 62);
            this.radioButton_KFReHardamard.Name = "radioButton_KFReHardamard";
            this.radioButton_KFReHardamard.Size = new System.Drawing.Size(107, 16);
            this.radioButton_KFReHardamard.TabIndex = 54;
            this.radioButton_KFReHardamard.Text = "KF-ReHardamard";
            this.radioButton_KFReHardamard.UseVisualStyleBackColor = true;
            // 
            // radioButton_KFReAllan
            // 
            this.radioButton_KFReAllan.AutoSize = true;
            this.radioButton_KFReAllan.Location = new System.Drawing.Point(286, 42);
            this.radioButton_KFReAllan.Name = "radioButton_KFReAllan";
            this.radioButton_KFReAllan.Size = new System.Drawing.Size(83, 16);
            this.radioButton_KFReAllan.TabIndex = 53;
            this.radioButton_KFReAllan.Text = "KF-ReAllan";
            this.radioButton_KFReAllan.UseVisualStyleBackColor = true;
            // 
            // radioButton_KFHardamard
            // 
            this.radioButton_KFHardamard.AutoSize = true;
            this.radioButton_KFHardamard.Location = new System.Drawing.Point(286, 20);
            this.radioButton_KFHardamard.Name = "radioButton_KFHardamard";
            this.radioButton_KFHardamard.Size = new System.Drawing.Size(95, 16);
            this.radioButton_KFHardamard.TabIndex = 52;
            this.radioButton_KFHardamard.Text = "KF-Hardamard";
            this.radioButton_KFHardamard.UseVisualStyleBackColor = true;
            // 
            // radioButton_QPT4
            // 
            this.radioButton_QPT4.AutoSize = true;
            this.radioButton_QPT4.Location = new System.Drawing.Point(174, 78);
            this.radioButton_QPT4.Name = "radioButton_QPT4";
            this.radioButton_QPT4.Size = new System.Drawing.Size(47, 16);
            this.radioButton_QPT4.TabIndex = 51;
            this.radioButton_QPT4.Text = "QPT4";
            this.radioButton_QPT4.UseVisualStyleBackColor = true;
            // 
            // radioButton_QPT3
            // 
            this.radioButton_QPT3.AutoSize = true;
            this.radioButton_QPT3.Location = new System.Drawing.Point(174, 60);
            this.radioButton_QPT3.Name = "radioButton_QPT3";
            this.radioButton_QPT3.Size = new System.Drawing.Size(47, 16);
            this.radioButton_QPT3.TabIndex = 50;
            this.radioButton_QPT3.Text = "QPT3";
            this.radioButton_QPT3.UseVisualStyleBackColor = true;
            // 
            // radioButton_QPT4GM
            // 
            this.radioButton_QPT4GM.AutoSize = true;
            this.radioButton_QPT4GM.Location = new System.Drawing.Point(222, 42);
            this.radioButton_QPT4GM.Name = "radioButton_QPT4GM";
            this.radioButton_QPT4GM.Size = new System.Drawing.Size(59, 16);
            this.radioButton_QPT4GM.TabIndex = 49;
            this.radioButton_QPT4GM.Text = "QPT4GM";
            this.radioButton_QPT4GM.UseVisualStyleBackColor = true;
            // 
            // radioButton_QPT2
            // 
            this.radioButton_QPT2.AutoSize = true;
            this.radioButton_QPT2.Location = new System.Drawing.Point(174, 39);
            this.radioButton_QPT2.Name = "radioButton_QPT2";
            this.radioButton_QPT2.Size = new System.Drawing.Size(47, 16);
            this.radioButton_QPT2.TabIndex = 48;
            this.radioButton_QPT2.Text = "QPT2";
            this.radioButton_QPT2.UseVisualStyleBackColor = true;
            // 
            // radioButton_QPT1
            // 
            this.radioButton_QPT1.AutoSize = true;
            this.radioButton_QPT1.Location = new System.Drawing.Point(174, 17);
            this.radioButton_QPT1.Name = "radioButton_QPT1";
            this.radioButton_QPT1.Size = new System.Drawing.Size(47, 16);
            this.radioButton_QPT1.TabIndex = 47;
            this.radioButton_QPT1.Text = "QPT1";
            this.radioButton_QPT1.UseVisualStyleBackColor = true;
            // 
            // radioButton_QPT2GM
            // 
            this.radioButton_QPT2GM.AutoSize = true;
            this.radioButton_QPT2GM.Location = new System.Drawing.Point(222, 20);
            this.radioButton_QPT2GM.Name = "radioButton_QPT2GM";
            this.radioButton_QPT2GM.Size = new System.Drawing.Size(59, 16);
            this.radioButton_QPT2GM.TabIndex = 43;
            this.radioButton_QPT2GM.Text = "QPT2GM";
            this.radioButton_QPT2GM.UseVisualStyleBackColor = true;
            // 
            // radioButton_GM
            // 
            this.radioButton_GM.AutoSize = true;
            this.radioButton_GM.Location = new System.Drawing.Point(121, 39);
            this.radioButton_GM.Name = "radioButton_GM";
            this.radioButton_GM.Size = new System.Drawing.Size(35, 16);
            this.radioButton_GM.TabIndex = 31;
            this.radioButton_GM.Text = "GM";
            this.radioButton_GM.UseVisualStyleBackColor = true;
            // 
            // radioButton_QP
            // 
            this.radioButton_QP.AutoSize = true;
            this.radioButton_QP.Location = new System.Drawing.Point(123, 17);
            this.radioButton_QP.Name = "radioButton_QP";
            this.radioButton_QP.Size = new System.Drawing.Size(35, 16);
            this.radioButton_QP.TabIndex = 41;
            this.radioButton_QP.Text = "QP";
            this.radioButton_QP.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(623, 327);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 12);
            this.label7.TabIndex = 37;
            this.label7.Text = "天";
            // 
            // textBox_PredictedLength
            // 
            this.textBox_PredictedLength.Location = new System.Drawing.Point(576, 322);
            this.textBox_PredictedLength.Name = "textBox_PredictedLength";
            this.textBox_PredictedLength.Size = new System.Drawing.Size(41, 21);
            this.textBox_PredictedLength.TabIndex = 35;
            this.textBox_PredictedLength.Text = "1";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(486, 327);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 36;
            this.label8.Text = "预报数据长度";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(547, 217);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 34;
            this.label6.Text = "天";
            // 
            // textBox_ModelLength
            // 
            this.textBox_ModelLength.Location = new System.Drawing.Point(502, 212);
            this.textBox_ModelLength.Name = "textBox_ModelLength";
            this.textBox_ModelLength.Size = new System.Drawing.Size(39, 21);
            this.textBox_ModelLength.TabIndex = 32;
            this.textBox_ModelLength.Text = "1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(412, 217);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 33;
            this.label5.Text = "建模数据长度";
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "输出目录：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(28, 86);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "D:\\Temp";
            this.directorySelectionControl1.Pathes = new string[] {
        "D:\\Temp"};
            this.directorySelectionControl1.Size = new System.Drawing.Size(656, 22);
            this.directorySelectionControl1.TabIndex = 21;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(536, 64);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(126, 16);
            this.checkBox1.TabIndex = 19;
            this.checkBox1.Text = "若无数据，以0填充";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(141, 215);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 17;
            this.label4.Text = "分钟";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 217);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "采样间隔：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 17;
            this.label2.Text = "待导出卫星：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 17;
            this.label1.Text = "sp3文件：";
            // 
            // button_getPath
            // 
            this.button_getPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getPath.Location = new System.Drawing.Point(625, 11);
            this.button_getPath.Name = "button_getPath";
            this.button_getPath.Size = new System.Drawing.Size(50, 47);
            this.button_getPath.TabIndex = 16;
            this.button_getPath.Text = "...";
            this.button_getPath.UseVisualStyleBackColor = true;
            this.button_getPath.Click += new System.EventHandler(this.button_getPath_Click);
            // 
            // textBox_interval
            // 
            this.textBox_interval.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_interval.Location = new System.Drawing.Point(97, 212);
            this.textBox_interval.Name = "textBox_interval";
            this.textBox_interval.Size = new System.Drawing.Size(38, 21);
            this.textBox_interval.TabIndex = 18;
            this.textBox_interval.Text = "5";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(621, 266);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 12);
            this.label9.TabIndex = 49;
            this.label9.Text = "次";
            // 
            // textBox_PredictedNumber
            // 
            this.textBox_PredictedNumber.Location = new System.Drawing.Point(576, 261);
            this.textBox_PredictedNumber.Name = "textBox_PredictedNumber";
            this.textBox_PredictedNumber.Size = new System.Drawing.Size(39, 21);
            this.textBox_PredictedNumber.TabIndex = 47;
            this.textBox_PredictedNumber.Text = "1";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(486, 266);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 48;
            this.label10.Text = "连续预报次数";
            // 
            // textBox_satPrns
            // 
            this.textBox_satPrns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_satPrns.Location = new System.Drawing.Point(97, 124);
            this.textBox_satPrns.Multiline = true;
            this.textBox_satPrns.Name = "textBox_satPrns";
            this.textBox_satPrns.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_satPrns.Size = new System.Drawing.Size(469, 82);
            this.textBox_satPrns.TabIndex = 18;
            this.textBox_satPrns.Text = resources.GetString("textBox_satPrns.Text");
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.IsSelectedPrn);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBox_ModelLength);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.directorySelectionControl1);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button_getPath);
            this.groupBox1.Controls.Add(this.textBox_interval);
            this.groupBox1.Controls.Add(this.textBox_satPrns);
            this.groupBox1.Controls.Add(this.textBox_Pathes);
            this.groupBox1.Location = new System.Drawing.Point(74, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(690, 246);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息";
            // 
            // IsSelectedPrn
            // 
            this.IsSelectedPrn.AutoSize = true;
            this.IsSelectedPrn.Location = new System.Drawing.Point(580, 145);
            this.IsSelectedPrn.Name = "IsSelectedPrn";
            this.IsSelectedPrn.Size = new System.Drawing.Size(71, 16);
            this.IsSelectedPrn.TabIndex = 38;
            this.IsSelectedPrn.TabStop = true;
            this.IsSelectedPrn.Text = "指定卫星";
            this.IsSelectedPrn.UseVisualStyleBackColor = true;
            // 
            // textBox_Pathes
            // 
            this.textBox_Pathes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Pathes.Location = new System.Drawing.Point(97, 13);
            this.textBox_Pathes.Multiline = true;
            this.textBox_Pathes.Name = "textBox_Pathes";
            this.textBox_Pathes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Pathes.Size = new System.Drawing.Size(512, 45);
            this.textBox_Pathes.TabIndex = 18;
            // 
            // button_export
            // 
            this.button_export.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_export.Location = new System.Drawing.Point(650, 314);
            this.button_export.Name = "button_export";
            this.button_export.Size = new System.Drawing.Size(75, 23);
            this.button_export.TabIndex = 45;
            this.button_export.Text = "计算";
            this.button_export.UseVisualStyleBackColor = true;
            this.button_export.Click += new System.EventHandler(this.button_export_Click);
            // 
            // openFileDialog_sp3
            // 
            this.openFileDialog_sp3.Filter = "星历文件|*.SP3|所有文件|*.*";
            this.openFileDialog_sp3.Multiselect = true;
            this.openFileDialog_sp3.Title = "请选择文件";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(623, 299);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 12);
            this.label11.TabIndex = 40;
            this.label11.Text = "个";
            // 
            // textBox1_GlideEpochNumber
            // 
            this.textBox1_GlideEpochNumber.Location = new System.Drawing.Point(576, 294);
            this.textBox1_GlideEpochNumber.Name = "textBox1_GlideEpochNumber";
            this.textBox1_GlideEpochNumber.Size = new System.Drawing.Size(41, 21);
            this.textBox1_GlideEpochNumber.TabIndex = 38;
            this.textBox1_GlideEpochNumber.Text = "1";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(486, 299);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 39;
            this.label12.Text = "滑动历元数";
            // 
            // radioButton_DLP
            // 
            this.radioButton_DLP.AutoSize = true;
            this.radioButton_DLP.Location = new System.Drawing.Point(21, 36);
            this.radioButton_DLP.Name = "radioButton_DLP";
            this.radioButton_DLP.Size = new System.Drawing.Size(41, 16);
            this.radioButton_DLP.TabIndex = 42;
            this.radioButton_DLP.Text = "DLP";
            this.radioButton_DLP.UseVisualStyleBackColor = true;
            // 
            // radioButton_LP
            // 
            this.radioButton_LP.AutoSize = true;
            this.radioButton_LP.Location = new System.Drawing.Point(22, 17);
            this.radioButton_LP.Name = "radioButton_LP";
            this.radioButton_LP.Size = new System.Drawing.Size(35, 16);
            this.radioButton_LP.TabIndex = 41;
            this.radioButton_LP.Text = "LP";
            this.radioButton_LP.UseVisualStyleBackColor = true;
            // 
            // radioButton_RobustLP
            // 
            this.radioButton_RobustLP.AutoSize = true;
            this.radioButton_RobustLP.Location = new System.Drawing.Point(21, 76);
            this.radioButton_RobustLP.Name = "radioButton_RobustLP";
            this.radioButton_RobustLP.Size = new System.Drawing.Size(47, 16);
            this.radioButton_RobustLP.TabIndex = 42;
            this.radioButton_RobustLP.Text = "R-LP";
            this.radioButton_RobustLP.UseVisualStyleBackColor = true;
            // 
            // radioButton_KFAllan
            // 
            this.radioButton_KFAllan.AutoSize = true;
            this.radioButton_KFAllan.Location = new System.Drawing.Point(286, 80);
            this.radioButton_KFAllan.Name = "radioButton_KFAllan";
            this.radioButton_KFAllan.Size = new System.Drawing.Size(71, 16);
            this.radioButton_KFAllan.TabIndex = 56;
            this.radioButton_KFAllan.TabStop = true;
            this.radioButton_KFAllan.Text = "KF-Allan";
            this.radioButton_KFAllan.UseVisualStyleBackColor = true;
            // 
            // ClockPredictionBasedonSp3Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 365);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox1_GlideEpochNumber);
            this.Controls.Add(this.textBox_PredictedLength);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBox_PredictedNumber);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_export);
            this.Name = "ClockPredictionBasedonSp3Form";
            this.Text = "ClockPredictionForm";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButton_RobustDLP;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton_QPGM;
        private System.Windows.Forms.RadioButton radioButton_KFReHardamard;
        private System.Windows.Forms.RadioButton radioButton_KFReAllan;
        private System.Windows.Forms.RadioButton radioButton_KFHardamard;
        private System.Windows.Forms.RadioButton radioButton_QPT4;
        private System.Windows.Forms.RadioButton radioButton_QPT3;
        private System.Windows.Forms.RadioButton radioButton_QPT4GM;
        private System.Windows.Forms.RadioButton radioButton_QPT2;
        private System.Windows.Forms.RadioButton radioButton_QPT1;
        private System.Windows.Forms.RadioButton radioButton_QPT2GM;
        private System.Windows.Forms.RadioButton radioButton_GM;
        private System.Windows.Forms.RadioButton radioButton_QP;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_PredictedLength;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_ModelLength;
        private System.Windows.Forms.Label label5;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_getPath;
        private System.Windows.Forms.TextBox textBox_interval;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox_PredictedNumber;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox_satPrns;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_Pathes;
        private System.Windows.Forms.Button button_export;
        private System.Windows.Forms.OpenFileDialog openFileDialog_sp3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox1_GlideEpochNumber;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RadioButton IsSelectedPrn;
        private System.Windows.Forms.RadioButton radioButton_DLP;
        private System.Windows.Forms.RadioButton radioButton_LP;
        private System.Windows.Forms.RadioButton radioButton_RobustLP;
        private System.Windows.Forms.RadioButton radioButton_KFAllan;
    }
}