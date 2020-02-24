namespace Gnsser.Winform
{
    partial class ObsFileConvertOptionForm
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
            this.button_ok = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.enabledFloatControl1Section = new Geo.Winform.Controls.EnabledFloatControl();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.namedStringControl_StrOfFrequenceNumToBeRemoved = new Geo.Winform.Controls.NamedStringControl();
            this.checkBox_StrOfFrequenceNumToBeRemoved = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBox1IsReomveOtherCodes = new System.Windows.Forms.CheckBox();
            this.textBox1OnlyCodes = new System.Windows.Forms.TextBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox_deleVacantSat = new System.Windows.Forms.CheckBox();
            this.textBoxNotVacantCodes = new System.Windows.Forms.TextBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.checkBoxIsRemoveZeroPhaseSat = new System.Windows.Forms.CheckBox();
            this.checkBoxIsRemoveZeroRangeSat = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.enumRadioControl_nameType = new Geo.Winform.EnumRadioControl();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBox_XToP2Enabled = new System.Windows.Forms.CheckBox();
            this.checkBox_IsEnableRinexVertion = new System.Windows.Forms.CheckBox();
            this.comboBox_version = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxMinObsCodeAppearRatio = new System.Windows.Forms.TextBox();
            this.checkBoxMinObsCodeAppearRatio = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.multiGnssSystemSelectControl1 = new Gnsser.Winform.Controls.MultiGnssSystemSelectControl();
            this.checkBoxSatelliteTypes = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBox_interval = new System.Windows.Forms.CheckBox();
            this.textBox_interval = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.timePeriodControl1 = new Geo.Winform.Controls.TimePeriodControl();
            this.checkBox_enableTimePeriod = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.enumCheckBoxControlObsTypes = new Geo.Winform.EnumCheckBoxControl();
            this.checkBox_enableCode = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBox_IsUseFileNameAsSiteName = new System.Windows.Forms.CheckBox();
            this.fileOpenControl1IndicatedEphemeris = new Geo.Winform.Controls.FileOpenControl();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.enabledFloatControl_SatCutOffAngle = new Geo.Winform.Controls.EnabledFloatControl();
            this.checkBox1IsRemoveRedundantObsForIonoFree = new System.Windows.Forms.CheckBox();
            this.checkBox1IndicatedEphemeris = new System.Windows.Forms.CheckBox();
            this.checkBox1IsRemoveIonoFreeUnavaliable = new System.Windows.Forms.CheckBox();
            this.checkBox1IsAmendBigCs = new System.Windows.Forms.CheckBox();
            this.checkBoxIsEnableAlignPhase = new System.Windows.Forms.CheckBox();
            this.namedIntControl1MaxBreakCount = new Geo.Winform.Controls.NamedIntControl();
            this.enabledIntControl_SiteNameLength = new Geo.Winform.Controls.EnabledIntControl();
            this.enabledIntControl_removeEpochCount = new Geo.Winform.Controls.EnabledIntControl();
            this.checkBox1IsConvertPhaseToLength = new System.Windows.Forms.CheckBox();
            this.enabledStringControl_RemoveSats = new Geo.Winform.Controls.EnabledStringControl();
            this.button_reset = new System.Windows.Forms.Button();
            this.checkBox_IsUpperSiteName = new System.Windows.Forms.CheckBox();
            this.checkBox_IsUpperFileName = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ok.Location = new System.Drawing.Point(380, 618);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 30);
            this.button_ok.TabIndex = 0;
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(533, 609);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.enabledFloatControl1Section);
            this.tabPage1.Controls.Add(this.groupBox10);
            this.tabPage1.Controls.Add(this.groupBox6);
            this.tabPage1.Controls.Add(this.groupBox8);
            this.tabPage1.Controls.Add(this.groupBox9);
            this.tabPage1.Controls.Add(this.groupBox7);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox5);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(525, 583);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本选项";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // enabledFloatControl1Section
            // 
            this.enabledFloatControl1Section.Location = new System.Drawing.Point(11, 184);
            this.enabledFloatControl1Section.Name = "enabledFloatControl1Section";
            this.enabledFloatControl1Section.Size = new System.Drawing.Size(270, 23);
            this.enabledFloatControl1Section.TabIndex = 66;
            this.enabledFloatControl1Section.Title = "分时段切割（分）：";
            this.enabledFloatControl1Section.Value = 0.1D;
            // 
            // groupBox10
            // 
            this.groupBox10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox10.Controls.Add(this.namedStringControl_StrOfFrequenceNumToBeRemoved);
            this.groupBox10.Controls.Add(this.checkBox_StrOfFrequenceNumToBeRemoved);
            this.groupBox10.Location = new System.Drawing.Point(7, 502);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(510, 56);
            this.groupBox10.TabIndex = 65;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "频率编号过滤";
            // 
            // namedStringControl_StrOfFrequenceNumToBeRemoved
            // 
            this.namedStringControl_StrOfFrequenceNumToBeRemoved.Location = new System.Drawing.Point(7, 21);
            this.namedStringControl_StrOfFrequenceNumToBeRemoved.Name = "namedStringControl_StrOfFrequenceNumToBeRemoved";
            this.namedStringControl_StrOfFrequenceNumToBeRemoved.Size = new System.Drawing.Size(274, 23);
            this.namedStringControl_StrOfFrequenceNumToBeRemoved.TabIndex = 67;
            this.namedStringControl_StrOfFrequenceNumToBeRemoved.Title = "频率编号：";
            // 
            // checkBox_StrOfFrequenceNumToBeRemoved
            // 
            this.checkBox_StrOfFrequenceNumToBeRemoved.AutoSize = true;
            this.checkBox_StrOfFrequenceNumToBeRemoved.Location = new System.Drawing.Point(316, 21);
            this.checkBox_StrOfFrequenceNumToBeRemoved.Name = "checkBox_StrOfFrequenceNumToBeRemoved";
            this.checkBox_StrOfFrequenceNumToBeRemoved.Size = new System.Drawing.Size(144, 16);
            this.checkBox_StrOfFrequenceNumToBeRemoved.TabIndex = 66;
            this.checkBox_StrOfFrequenceNumToBeRemoved.Text = "是否删除指定频率编号";
            this.checkBox_StrOfFrequenceNumToBeRemoved.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Controls.Add(this.checkBox1IsReomveOtherCodes);
            this.groupBox6.Controls.Add(this.textBox1OnlyCodes);
            this.groupBox6.Location = new System.Drawing.Point(5, 450);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(510, 46);
            this.groupBox6.TabIndex = 65;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "观测码过滤";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "观测类型：";
            // 
            // checkBox1IsReomveOtherCodes
            // 
            this.checkBox1IsReomveOtherCodes.AutoSize = true;
            this.checkBox1IsReomveOtherCodes.Location = new System.Drawing.Point(321, 24);
            this.checkBox1IsReomveOtherCodes.Name = "checkBox1IsReomveOtherCodes";
            this.checkBox1IsReomveOtherCodes.Size = new System.Drawing.Size(168, 16);
            this.checkBox1IsReomveOtherCodes.TabIndex = 66;
            this.checkBox1IsReomveOtherCodes.Text = "是否删除其它观测类型数据";
            this.checkBox1IsReomveOtherCodes.UseVisualStyleBackColor = true;
            // 
            // textBox1OnlyCodes
            // 
            this.textBox1OnlyCodes.Location = new System.Drawing.Point(84, 19);
            this.textBox1OnlyCodes.Name = "textBox1OnlyCodes";
            this.textBox1OnlyCodes.Size = new System.Drawing.Size(197, 21);
            this.textBox1OnlyCodes.TabIndex = 2;
            this.textBox1OnlyCodes.Text = "L1,L2,P2,C1,P1";
            // 
            // groupBox8
            // 
            this.groupBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox8.Controls.Add(this.label2);
            this.groupBox8.Controls.Add(this.label1);
            this.groupBox8.Controls.Add(this.checkBox_deleVacantSat);
            this.groupBox8.Controls.Add(this.textBoxNotVacantCodes);
            this.groupBox8.Location = new System.Drawing.Point(5, 380);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(510, 61);
            this.groupBox8.TabIndex = 65;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "删除缺少指定观测码的卫星";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(371, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "注意：如果是RINEX3.0以上数据，需要精确指定 3 个字符的观测类型";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "观测类型：";
            // 
            // checkBox_deleVacantSat
            // 
            this.checkBox_deleVacantSat.AutoSize = true;
            this.checkBox_deleVacantSat.Location = new System.Drawing.Point(321, 19);
            this.checkBox_deleVacantSat.Name = "checkBox_deleVacantSat";
            this.checkBox_deleVacantSat.Size = new System.Drawing.Size(168, 16);
            this.checkBox_deleVacantSat.TabIndex = 66;
            this.checkBox_deleVacantSat.Text = "删除缺少指定观测码的卫星";
            this.checkBox_deleVacantSat.UseVisualStyleBackColor = true;
            // 
            // textBoxNotVacantCodes
            // 
            this.textBoxNotVacantCodes.Location = new System.Drawing.Point(87, 14);
            this.textBoxNotVacantCodes.Name = "textBoxNotVacantCodes";
            this.textBoxNotVacantCodes.Size = new System.Drawing.Size(197, 21);
            this.textBoxNotVacantCodes.TabIndex = 2;
            this.textBoxNotVacantCodes.Text = "L1,L2,P2";
            // 
            // groupBox9
            // 
            this.groupBox9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox9.Controls.Add(this.checkBoxIsRemoveZeroPhaseSat);
            this.groupBox9.Controls.Add(this.checkBoxIsRemoveZeroRangeSat);
            this.groupBox9.Location = new System.Drawing.Point(11, 334);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(508, 30);
            this.groupBox9.TabIndex = 64;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "空值过滤";
            // 
            // checkBoxIsRemoveZeroPhaseSat
            // 
            this.checkBoxIsRemoveZeroPhaseSat.AutoSize = true;
            this.checkBoxIsRemoveZeroPhaseSat.Location = new System.Drawing.Point(207, 11);
            this.checkBoxIsRemoveZeroPhaseSat.Name = "checkBoxIsRemoveZeroPhaseSat";
            this.checkBoxIsRemoveZeroPhaseSat.Size = new System.Drawing.Size(114, 16);
            this.checkBoxIsRemoveZeroPhaseSat.TabIndex = 2;
            this.checkBoxIsRemoveZeroPhaseSat.Text = "移除0值载波卫星";
            this.checkBoxIsRemoveZeroPhaseSat.UseVisualStyleBackColor = true;
            // 
            // checkBoxIsRemoveZeroRangeSat
            // 
            this.checkBoxIsRemoveZeroRangeSat.AutoSize = true;
            this.checkBoxIsRemoveZeroRangeSat.Location = new System.Drawing.Point(87, 11);
            this.checkBoxIsRemoveZeroRangeSat.Name = "checkBoxIsRemoveZeroRangeSat";
            this.checkBoxIsRemoveZeroRangeSat.Size = new System.Drawing.Size(114, 16);
            this.checkBoxIsRemoveZeroRangeSat.TabIndex = 2;
            this.checkBoxIsRemoveZeroRangeSat.Text = "移除0值伪距卫星";
            this.checkBoxIsRemoveZeroRangeSat.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox7.Controls.Add(this.enumRadioControl_nameType);
            this.groupBox7.Controls.Add(this.label6);
            this.groupBox7.Controls.Add(this.checkBox_XToP2Enabled);
            this.groupBox7.Controls.Add(this.checkBox_IsEnableRinexVertion);
            this.groupBox7.Controls.Add(this.comboBox_version);
            this.groupBox7.Location = new System.Drawing.Point(9, 256);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(508, 73);
            this.groupBox7.TabIndex = 64;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "输出版本";
            // 
            // enumRadioControl_nameType
            // 
            this.enumRadioControl_nameType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.enumRadioControl_nameType.IsReady = false;
            this.enumRadioControl_nameType.Location = new System.Drawing.Point(209, 18);
            this.enumRadioControl_nameType.Name = "enumRadioControl_nameType";
            this.enumRadioControl_nameType.Size = new System.Drawing.Size(292, 48);
            this.enumRadioControl_nameType.TabIndex = 5;
            this.enumRadioControl_nameType.Title = "文件命名方式";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "输出版本：";
            // 
            // checkBox_XToP2Enabled
            // 
            this.checkBox_XToP2Enabled.AutoSize = true;
            this.checkBox_XToP2Enabled.Location = new System.Drawing.Point(6, 51);
            this.checkBox_XToP2Enabled.Name = "checkBox_XToP2Enabled";
            this.checkBox_XToP2Enabled.Size = new System.Drawing.Size(204, 16);
            this.checkBox_XToP2Enabled.TabIndex = 2;
            this.checkBox_XToP2Enabled.Text = "若无P2/L2,以其它替代(3.x转2时)";
            this.checkBox_XToP2Enabled.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsEnableRinexVertion
            // 
            this.checkBox_IsEnableRinexVertion.AutoSize = true;
            this.checkBox_IsEnableRinexVertion.Location = new System.Drawing.Point(137, 20);
            this.checkBox_IsEnableRinexVertion.Name = "checkBox_IsEnableRinexVertion";
            this.checkBox_IsEnableRinexVertion.Size = new System.Drawing.Size(48, 16);
            this.checkBox_IsEnableRinexVertion.TabIndex = 2;
            this.checkBox_IsEnableRinexVertion.Text = "启用";
            this.checkBox_IsEnableRinexVertion.UseVisualStyleBackColor = true;
            // 
            // comboBox_version
            // 
            this.comboBox_version.FormattingEnabled = true;
            this.comboBox_version.Items.AddRange(new object[] {
            "2.01",
            "3.02"});
            this.comboBox_version.Location = new System.Drawing.Point(72, 18);
            this.comboBox_version.Name = "comboBox_version";
            this.comboBox_version.Size = new System.Drawing.Size(59, 20);
            this.comboBox_version.TabIndex = 4;
            this.comboBox_version.Text = "3.02";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.textBoxMinObsCodeAppearRatio);
            this.groupBox4.Controls.Add(this.checkBoxMinObsCodeAppearRatio);
            this.groupBox4.Location = new System.Drawing.Point(253, 210);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(248, 40);
            this.groupBox4.TabIndex = 62;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "观测码出勤率过滤";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 17);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(77, 12);
            this.label11.TabIndex = 1;
            this.label11.Text = "最小出勤率：";
            // 
            // textBoxMinObsCodeAppearRatio
            // 
            this.textBoxMinObsCodeAppearRatio.Location = new System.Drawing.Point(85, 13);
            this.textBoxMinObsCodeAppearRatio.Name = "textBoxMinObsCodeAppearRatio";
            this.textBoxMinObsCodeAppearRatio.Size = new System.Drawing.Size(95, 21);
            this.textBoxMinObsCodeAppearRatio.TabIndex = 5;
            this.textBoxMinObsCodeAppearRatio.Text = "0.5";
            // 
            // checkBoxMinObsCodeAppearRatio
            // 
            this.checkBoxMinObsCodeAppearRatio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxMinObsCodeAppearRatio.AutoSize = true;
            this.checkBoxMinObsCodeAppearRatio.Location = new System.Drawing.Point(194, 15);
            this.checkBoxMinObsCodeAppearRatio.Name = "checkBoxMinObsCodeAppearRatio";
            this.checkBoxMinObsCodeAppearRatio.Size = new System.Drawing.Size(48, 16);
            this.checkBoxMinObsCodeAppearRatio.TabIndex = 2;
            this.checkBoxMinObsCodeAppearRatio.Text = "启用";
            this.checkBoxMinObsCodeAppearRatio.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.multiGnssSystemSelectControl1);
            this.groupBox2.Controls.Add(this.checkBoxSatelliteTypes);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(510, 58);
            this.groupBox2.TabIndex = 61;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "系统过滤";
            // 
            // multiGnssSystemSelectControl1
            // 
            this.multiGnssSystemSelectControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.multiGnssSystemSelectControl1.Location = new System.Drawing.Point(3, 17);
            this.multiGnssSystemSelectControl1.Margin = new System.Windows.Forms.Padding(2);
            this.multiGnssSystemSelectControl1.Name = "multiGnssSystemSelectControl1";
            this.multiGnssSystemSelectControl1.Size = new System.Drawing.Size(456, 38);
            this.multiGnssSystemSelectControl1.TabIndex = 60;
            // 
            // checkBoxSatelliteTypes
            // 
            this.checkBoxSatelliteTypes.AutoSize = true;
            this.checkBoxSatelliteTypes.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxSatelliteTypes.Location = new System.Drawing.Point(459, 17);
            this.checkBoxSatelliteTypes.Name = "checkBoxSatelliteTypes";
            this.checkBoxSatelliteTypes.Size = new System.Drawing.Size(48, 38);
            this.checkBoxSatelliteTypes.TabIndex = 2;
            this.checkBoxSatelliteTypes.Text = "启用";
            this.checkBoxSatelliteTypes.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.checkBox_interval);
            this.groupBox5.Controls.Add(this.textBox_interval);
            this.groupBox5.Location = new System.Drawing.Point(3, 210);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(249, 40);
            this.groupBox5.TabIndex = 58;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "采样率";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "采样率(秒)：";
            // 
            // checkBox_interval
            // 
            this.checkBox_interval.AutoSize = true;
            this.checkBox_interval.Location = new System.Drawing.Point(173, 13);
            this.checkBox_interval.Name = "checkBox_interval";
            this.checkBox_interval.Size = new System.Drawing.Size(48, 16);
            this.checkBox_interval.TabIndex = 2;
            this.checkBox_interval.Text = "启用";
            this.checkBox_interval.UseVisualStyleBackColor = true;
            // 
            // textBox_interval
            // 
            this.textBox_interval.Location = new System.Drawing.Point(89, 11);
            this.textBox_interval.Name = "textBox_interval";
            this.textBox_interval.Size = new System.Drawing.Size(78, 21);
            this.textBox_interval.TabIndex = 2;
            this.textBox_interval.Text = "15";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.timePeriodControl1);
            this.groupBox3.Controls.Add(this.checkBox_enableTimePeriod);
            this.groupBox3.Location = new System.Drawing.Point(3, 135);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(510, 40);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "时段过滤";
            // 
            // timePeriodControl1
            // 
            this.timePeriodControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timePeriodControl1.Location = new System.Drawing.Point(3, 17);
            this.timePeriodControl1.Margin = new System.Windows.Forms.Padding(2);
            this.timePeriodControl1.Name = "timePeriodControl1";
            this.timePeriodControl1.Size = new System.Drawing.Size(456, 20);
            this.timePeriodControl1.TabIndex = 4;
            this.timePeriodControl1.TimeFrom = new System.DateTime(2016, 10, 28, 13, 13, 20, 890);
            this.timePeriodControl1.TimeStringFormat = "yyyy-MM-dd HH:mm:ss";
            this.timePeriodControl1.TimeTo = new System.DateTime(2016, 10, 28, 13, 13, 20, 899);
            this.timePeriodControl1.Title = "时段：";
            // 
            // checkBox_enableTimePeriod
            // 
            this.checkBox_enableTimePeriod.AutoSize = true;
            this.checkBox_enableTimePeriod.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBox_enableTimePeriod.Location = new System.Drawing.Point(459, 17);
            this.checkBox_enableTimePeriod.Name = "checkBox_enableTimePeriod";
            this.checkBox_enableTimePeriod.Size = new System.Drawing.Size(48, 20);
            this.checkBox_enableTimePeriod.TabIndex = 3;
            this.checkBox_enableTimePeriod.Text = "启用";
            this.checkBox_enableTimePeriod.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.enumCheckBoxControlObsTypes);
            this.groupBox1.Controls.Add(this.checkBox_enableCode);
            this.groupBox1.Location = new System.Drawing.Point(9, 67);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(510, 64);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "观测类型过滤";
            // 
            // enumCheckBoxControlObsTypes
            // 
            this.enumCheckBoxControlObsTypes.Location = new System.Drawing.Point(3, 13);
            this.enumCheckBoxControlObsTypes.Name = "enumCheckBoxControlObsTypes";
            this.enumCheckBoxControlObsTypes.Size = new System.Drawing.Size(441, 44);
            this.enumCheckBoxControlObsTypes.TabIndex = 61;
            this.enumCheckBoxControlObsTypes.Title = "";
            // 
            // checkBox_enableCode
            // 
            this.checkBox_enableCode.AutoSize = true;
            this.checkBox_enableCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBox_enableCode.Location = new System.Drawing.Point(459, 17);
            this.checkBox_enableCode.Name = "checkBox_enableCode";
            this.checkBox_enableCode.Size = new System.Drawing.Size(48, 44);
            this.checkBox_enableCode.TabIndex = 2;
            this.checkBox_enableCode.Text = "启用";
            this.checkBox_enableCode.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkBox_IsUpperFileName);
            this.tabPage2.Controls.Add(this.checkBox_IsUpperSiteName);
            this.tabPage2.Controls.Add(this.checkBox_IsUseFileNameAsSiteName);
            this.tabPage2.Controls.Add(this.fileOpenControl1IndicatedEphemeris);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.enabledFloatControl_SatCutOffAngle);
            this.tabPage2.Controls.Add(this.checkBox1IsRemoveRedundantObsForIonoFree);
            this.tabPage2.Controls.Add(this.checkBox1IndicatedEphemeris);
            this.tabPage2.Controls.Add(this.checkBox1IsRemoveIonoFreeUnavaliable);
            this.tabPage2.Controls.Add(this.checkBox1IsAmendBigCs);
            this.tabPage2.Controls.Add(this.checkBoxIsEnableAlignPhase);
            this.tabPage2.Controls.Add(this.namedIntControl1MaxBreakCount);
            this.tabPage2.Controls.Add(this.enabledIntControl_SiteNameLength);
            this.tabPage2.Controls.Add(this.enabledIntControl_removeEpochCount);
            this.tabPage2.Controls.Add(this.checkBox1IsConvertPhaseToLength);
            this.tabPage2.Controls.Add(this.enabledStringControl_RemoveSats);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(525, 583);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "其它选项";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsUseFileNameAsSiteName
            // 
            this.checkBox_IsUseFileNameAsSiteName.AutoSize = true;
            this.checkBox_IsUseFileNameAsSiteName.Location = new System.Drawing.Point(113, 461);
            this.checkBox_IsUseFileNameAsSiteName.Name = "checkBox_IsUseFileNameAsSiteName";
            this.checkBox_IsUseFileNameAsSiteName.Size = new System.Drawing.Size(180, 16);
            this.checkBox_IsUseFileNameAsSiteName.TabIndex = 8;
            this.checkBox_IsUseFileNameAsSiteName.Text = "是否采用文件名称替代测站名";
            this.checkBox_IsUseFileNameAsSiteName.UseVisualStyleBackColor = true;
            // 
            // fileOpenControl1IndicatedEphemeris
            // 
            this.fileOpenControl1IndicatedEphemeris.AllowDrop = true;
            this.fileOpenControl1IndicatedEphemeris.FilePath = "";
            this.fileOpenControl1IndicatedEphemeris.FilePathes = new string[0];
            this.fileOpenControl1IndicatedEphemeris.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl1IndicatedEphemeris.FirstPath = "";
            this.fileOpenControl1IndicatedEphemeris.IsMultiSelect = false;
            this.fileOpenControl1IndicatedEphemeris.LabelName = "星历文件：";
            this.fileOpenControl1IndicatedEphemeris.Location = new System.Drawing.Point(20, 276);
            this.fileOpenControl1IndicatedEphemeris.Name = "fileOpenControl1IndicatedEphemeris";
            this.fileOpenControl1IndicatedEphemeris.Size = new System.Drawing.Size(413, 22);
            this.fileOpenControl1IndicatedEphemeris.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(81, 301);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(179, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "若不指定，默认使用GPS导航星历";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(171, 249);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "无星历的卫星将直接被删除";
            // 
            // enabledFloatControl_SatCutOffAngle
            // 
            this.enabledFloatControl_SatCutOffAngle.Location = new System.Drawing.Point(68, 223);
            this.enabledFloatControl_SatCutOffAngle.Name = "enabledFloatControl_SatCutOffAngle";
            this.enabledFloatControl_SatCutOffAngle.Size = new System.Drawing.Size(264, 23);
            this.enabledFloatControl_SatCutOffAngle.TabIndex = 5;
            this.enabledFloatControl_SatCutOffAngle.Title = "卫星高度截止角：";
            this.enabledFloatControl_SatCutOffAngle.Value = 10D;
            // 
            // checkBox1IsRemoveRedundantObsForIonoFree
            // 
            this.checkBox1IsRemoveRedundantObsForIonoFree.AutoSize = true;
            this.checkBox1IsRemoveRedundantObsForIonoFree.Location = new System.Drawing.Point(128, 390);
            this.checkBox1IsRemoveRedundantObsForIonoFree.Name = "checkBox1IsRemoveRedundantObsForIonoFree";
            this.checkBox1IsRemoveRedundantObsForIonoFree.Size = new System.Drawing.Size(216, 16);
            this.checkBox1IsRemoveRedundantObsForIonoFree.TabIndex = 4;
            this.checkBox1IsRemoveRedundantObsForIonoFree.Text = "移除对于无电离层组合多余的观测量";
            this.checkBox1IsRemoveRedundantObsForIonoFree.UseVisualStyleBackColor = true;
            // 
            // checkBox1IndicatedEphemeris
            // 
            this.checkBox1IndicatedEphemeris.AutoSize = true;
            this.checkBox1IndicatedEphemeris.Location = new System.Drawing.Point(439, 276);
            this.checkBox1IndicatedEphemeris.Name = "checkBox1IndicatedEphemeris";
            this.checkBox1IndicatedEphemeris.Size = new System.Drawing.Size(48, 16);
            this.checkBox1IndicatedEphemeris.TabIndex = 4;
            this.checkBox1IndicatedEphemeris.Text = "启用";
            this.checkBox1IndicatedEphemeris.UseVisualStyleBackColor = true;
            // 
            // checkBox1IsRemoveIonoFreeUnavaliable
            // 
            this.checkBox1IsRemoveIonoFreeUnavaliable.AutoSize = true;
            this.checkBox1IsRemoveIonoFreeUnavaliable.Location = new System.Drawing.Point(128, 357);
            this.checkBox1IsRemoveIonoFreeUnavaliable.Name = "checkBox1IsRemoveIonoFreeUnavaliable";
            this.checkBox1IsRemoveIonoFreeUnavaliable.Size = new System.Drawing.Size(192, 16);
            this.checkBox1IsRemoveIonoFreeUnavaliable.TabIndex = 4;
            this.checkBox1IsRemoveIonoFreeUnavaliable.Text = "移除不能组成电离层组合的卫星";
            this.checkBox1IsRemoveIonoFreeUnavaliable.UseVisualStyleBackColor = true;
            // 
            // checkBox1IsAmendBigCs
            // 
            this.checkBox1IsAmendBigCs.AutoSize = true;
            this.checkBox1IsAmendBigCs.Location = new System.Drawing.Point(173, 179);
            this.checkBox1IsAmendBigCs.Name = "checkBox1IsAmendBigCs";
            this.checkBox1IsAmendBigCs.Size = new System.Drawing.Size(180, 16);
            this.checkBox1IsAmendBigCs.TabIndex = 4;
            this.checkBox1IsAmendBigCs.Text = "全程相位对齐(后续手动跳跃)";
            this.checkBox1IsAmendBigCs.UseVisualStyleBackColor = true;
            // 
            // checkBoxIsEnableAlignPhase
            // 
            this.checkBoxIsEnableAlignPhase.AutoSize = true;
            this.checkBoxIsEnableAlignPhase.Location = new System.Drawing.Point(173, 153);
            this.checkBoxIsEnableAlignPhase.Name = "checkBoxIsEnableAlignPhase";
            this.checkBoxIsEnableAlignPhase.Size = new System.Drawing.Size(168, 16);
            this.checkBoxIsEnableAlignPhase.TabIndex = 4;
            this.checkBoxIsEnableAlignPhase.Text = "启用相位对齐(支持AB频率)";
            this.checkBoxIsEnableAlignPhase.UseVisualStyleBackColor = true;
            // 
            // namedIntControl1MaxBreakCount
            // 
            this.namedIntControl1MaxBreakCount.Location = new System.Drawing.Point(51, 110);
            this.namedIntControl1MaxBreakCount.Name = "namedIntControl1MaxBreakCount";
            this.namedIntControl1MaxBreakCount.Size = new System.Drawing.Size(179, 23);
            this.namedIntControl1MaxBreakCount.TabIndex = 3;
            this.namedIntControl1MaxBreakCount.Title = "允许最大断裂历元数：";
            this.namedIntControl1MaxBreakCount.Value = 0;
            // 
            // enabledIntControl_SiteNameLength
            // 
            this.enabledIntControl_SiteNameLength.Location = new System.Drawing.Point(25, 427);
            this.enabledIntControl_SiteNameLength.Name = "enabledIntControl_SiteNameLength";
            this.enabledIntControl_SiteNameLength.Size = new System.Drawing.Size(205, 23);
            this.enabledIntControl_SiteNameLength.TabIndex = 2;
            this.enabledIntControl_SiteNameLength.Title = "测站名称长度：";
            this.enabledIntControl_SiteNameLength.Value = 0;
            // 
            // enabledIntControl_removeEpochCount
            // 
            this.enabledIntControl_removeEpochCount.Location = new System.Drawing.Point(20, 81);
            this.enabledIntControl_removeEpochCount.Name = "enabledIntControl_removeEpochCount";
            this.enabledIntControl_removeEpochCount.Size = new System.Drawing.Size(295, 23);
            this.enabledIntControl_removeEpochCount.TabIndex = 2;
            this.enabledIntControl_removeEpochCount.Title = "移除连续历元数过小的卫星：";
            this.enabledIntControl_removeEpochCount.Value = 0;
            // 
            // checkBox1IsConvertPhaseToLength
            // 
            this.checkBox1IsConvertPhaseToLength.AutoSize = true;
            this.checkBox1IsConvertPhaseToLength.Location = new System.Drawing.Point(107, 50);
            this.checkBox1IsConvertPhaseToLength.Name = "checkBox1IsConvertPhaseToLength";
            this.checkBox1IsConvertPhaseToLength.Size = new System.Drawing.Size(132, 16);
            this.checkBox1IsConvertPhaseToLength.TabIndex = 1;
            this.checkBox1IsConvertPhaseToLength.Text = "是否将载波转成距离";
            this.checkBox1IsConvertPhaseToLength.UseVisualStyleBackColor = true;
            // 
            // enabledStringControl_RemoveSats
            // 
            this.enabledStringControl_RemoveSats.Location = new System.Drawing.Point(20, 21);
            this.enabledStringControl_RemoveSats.Name = "enabledStringControl_RemoveSats";
            this.enabledStringControl_RemoveSats.Size = new System.Drawing.Size(342, 23);
            this.enabledStringControl_RemoveSats.TabIndex = 0;
            this.enabledStringControl_RemoveSats.Title = "移除指定卫星：";
            // 
            // button_reset
            // 
            this.button_reset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_reset.Location = new System.Drawing.Point(461, 618);
            this.button_reset.Name = "button_reset";
            this.button_reset.Size = new System.Drawing.Size(75, 30);
            this.button_reset.TabIndex = 2;
            this.button_reset.Text = "重置";
            this.button_reset.UseVisualStyleBackColor = true;
            this.button_reset.Click += new System.EventHandler(this.button_reset_Click);
            // 
            // checkBox_IsUpperSiteName
            // 
            this.checkBox_IsUpperSiteName.AutoSize = true;
            this.checkBox_IsUpperSiteName.Location = new System.Drawing.Point(113, 501);
            this.checkBox_IsUpperSiteName.Name = "checkBox_IsUpperSiteName";
            this.checkBox_IsUpperSiteName.Size = new System.Drawing.Size(96, 16);
            this.checkBox_IsUpperSiteName.TabIndex = 8;
            this.checkBox_IsUpperSiteName.Text = "大写测站名称";
            this.checkBox_IsUpperSiteName.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsUpperFileName
            // 
            this.checkBox_IsUpperFileName.AutoSize = true;
            this.checkBox_IsUpperFileName.Location = new System.Drawing.Point(113, 529);
            this.checkBox_IsUpperFileName.Name = "checkBox_IsUpperFileName";
            this.checkBox_IsUpperFileName.Size = new System.Drawing.Size(96, 16);
            this.checkBox_IsUpperFileName.TabIndex = 8;
            this.checkBox_IsUpperFileName.Text = "大写文件名称";
            this.checkBox_IsUpperFileName.UseVisualStyleBackColor = true;
            // 
            // ObsFileConvertOptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 653);
            this.Controls.Add(this.button_reset);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button_ok);
            this.Name = "ObsFileConvertOptionForm";
            this.Text = "观测文件格式化转换选项";
            this.Load += new System.EventHandler(this.ObsFileConvertOptionForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox checkBox_enableCode;
        private System.Windows.Forms.Button button_reset;
        private Geo.Winform.Controls.TimePeriodControl timePeriodControl1;
        private System.Windows.Forms.CheckBox checkBox_enableTimePeriod;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox comboBox_version;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_interval;
        private System.Windows.Forms.Label label7;
        private Controls.MultiGnssSystemSelectControl multiGnssSystemSelectControl1;
        private System.Windows.Forms.CheckBox checkBox_interval;
        private System.Windows.Forms.CheckBox checkBoxSatelliteTypes;
        private System.Windows.Forms.TextBox textBoxMinObsCodeAppearRatio;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox checkBoxMinObsCodeAppearRatio;
        private Geo.Winform.EnumCheckBoxControl enumCheckBoxControlObsTypes;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.CheckBox checkBox_deleVacantSat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxNotVacantCodes;
        private System.Windows.Forms.Label label2;
        private Geo.Winform.Controls.EnabledFloatControl enabledFloatControl1Section;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBox1IsReomveOtherCodes;
        private System.Windows.Forms.TextBox textBox1OnlyCodes;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.CheckBox checkBoxIsRemoveZeroPhaseSat;
        private System.Windows.Forms.CheckBox checkBoxIsRemoveZeroRangeSat;
        private System.Windows.Forms.TabPage tabPage2;
        private Geo.Winform.Controls.EnabledStringControl enabledStringControl_RemoveSats;
        private System.Windows.Forms.CheckBox checkBox1IsConvertPhaseToLength;
        private Geo.Winform.Controls.EnabledIntControl enabledIntControl_removeEpochCount;
        private Geo.Winform.Controls.NamedIntControl namedIntControl1MaxBreakCount;
        private System.Windows.Forms.CheckBox checkBoxIsEnableAlignPhase;
        private System.Windows.Forms.CheckBox checkBox1IsAmendBigCs;
        private Geo.Winform.Controls.EnabledFloatControl enabledFloatControl_SatCutOffAngle;
        private System.Windows.Forms.CheckBox checkBox_IsEnableRinexVertion;
        private System.Windows.Forms.GroupBox groupBox10;
        private Geo.Winform.Controls.NamedStringControl namedStringControl_StrOfFrequenceNumToBeRemoved;
        private System.Windows.Forms.CheckBox checkBox_StrOfFrequenceNumToBeRemoved;
        private System.Windows.Forms.CheckBox checkBox1IsRemoveRedundantObsForIonoFree;
        private System.Windows.Forms.CheckBox checkBox1IsRemoveIonoFreeUnavaliable;
        private System.Windows.Forms.Label label3;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl1IndicatedEphemeris;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBox1IndicatedEphemeris;
        private Geo.Winform.EnumRadioControl enumRadioControl_nameType;
        private System.Windows.Forms.CheckBox checkBox_XToP2Enabled;
        private Geo.Winform.Controls.EnabledIntControl enabledIntControl_SiteNameLength;
        private System.Windows.Forms.CheckBox checkBox_IsUseFileNameAsSiteName;
        private System.Windows.Forms.CheckBox checkBox_IsUpperSiteName;
        private System.Windows.Forms.CheckBox checkBox_IsUpperFileName;
    }
}