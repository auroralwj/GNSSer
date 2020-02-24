namespace Gnsser.Winform
{
    partial class SiteReceiverOptionPage
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
            this.tabPage_receiver = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.fileOpenControl_coordPath = new Geo.Winform.Controls.FileOpenControl();
            this.checkBox_IsApproxXyzRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_indicateCoordfile = new System.Windows.Forms.CheckBox();
            this.checkBox_IsSiteCoordServiceRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsSetApproxXyzWithCoordService = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.namedFloatControl_fixedErrorVertical = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_verticalCoefOfProprotion = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_fixedErrorLevel = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_levelCoefOfProprotion = new Geo.Winform.Controls.NamedFloatControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox_IsEstDcbOfRceiver = new System.Windows.Forms.CheckBox();
            this.checkBox1IsEstimateTropWetZpd = new System.Windows.Forms.CheckBox();
            this.checkBox_IsFixingCoord = new System.Windows.Forms.CheckBox();
            this.checkBox_updateStationInfo = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_IsSmoothRangeWhenPrevPseudorangePosition = new System.Windows.Forms.CheckBox();
            this.checkBox_IsNeedPseudorangePositionWhenProcess = new System.Windows.Forms.CheckBox();
            this.namedFloatControl_rmsOfWhitenoiceOfDynamicPos = new Geo.Winform.Controls.NamedFloatControl();
            this.checkBox_IsUpdateEstimatePostition = new System.Windows.Forms.CheckBox();
            this.enumRadioControl_positionType = new Geo.Winform.EnumRadioControl();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.radioButton_loosePrecise = new System.Windows.Forms.RadioButton();
            this.radioButton_commonPrecise = new System.Windows.Forms.RadioButton();
            this.radioButton_highPrecise = new System.Windows.Forms.RadioButton();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.checkBox_rmsIndicated = new System.Windows.Forms.CheckBox();
            this.checkBox_approxPos = new System.Windows.Forms.CheckBox();
            this.textBox_approxPosRms = new System.Windows.Forms.TextBox();
            this.textBox_aprioriPos = new System.Windows.Forms.TextBox();
            this.openFileDialog_nav = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_obs = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_clock = new System.Windows.Forms.OpenFileDialog();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.namedFloatControl_MinAllowedApproxXyzLen = new Geo.Winform.Controls.NamedFloatControl();
            this.label4 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage_receiver.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_receiver);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(582, 523);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage_receiver
            // 
            this.tabPage_receiver.Controls.Add(this.groupBox4);
            this.tabPage_receiver.Controls.Add(this.groupBox2);
            this.tabPage_receiver.Controls.Add(this.groupBox1);
            this.tabPage_receiver.Controls.Add(this.enumRadioControl_positionType);
            this.tabPage_receiver.Controls.Add(this.groupBox6);
            this.tabPage_receiver.Location = new System.Drawing.Point(4, 22);
            this.tabPage_receiver.Name = "tabPage_receiver";
            this.tabPage_receiver.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_receiver.Size = new System.Drawing.Size(574, 497);
            this.tabPage_receiver.TabIndex = 7;
            this.tabPage_receiver.Text = "测站/接收机";
            this.tabPage_receiver.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.fileOpenControl_coordPath);
            this.groupBox4.Controls.Add(this.checkBox_IsApproxXyzRequired);
            this.groupBox4.Controls.Add(this.checkBox_indicateCoordfile);
            this.groupBox4.Controls.Add(this.checkBox_IsSiteCoordServiceRequired);
            this.groupBox4.Controls.Add(this.checkBox_IsSetApproxXyzWithCoordService);
            this.groupBox4.Location = new System.Drawing.Point(7, 185);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(547, 99);
            this.groupBox4.TabIndex = 53;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "测站坐标设置";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(329, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(197, 12);
            this.label3.TabIndex = 67;
            this.label3.Text = "若不指定，则采用系统默认坐标文件";
            // 
            // fileOpenControl_coordPath
            // 
            this.fileOpenControl_coordPath.AllowDrop = true;
            this.fileOpenControl_coordPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_coordPath.FilePath = "";
            this.fileOpenControl_coordPath.FilePathes = new string[0];
            this.fileOpenControl_coordPath.Filter = "坐标文件|*.snx;*.xls|文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_coordPath.FirstPath = "";
            this.fileOpenControl_coordPath.IsMultiSelect = false;
            this.fileOpenControl_coordPath.LabelName = "坐标文件：";
            this.fileOpenControl_coordPath.Location = new System.Drawing.Point(7, 69);
            this.fileOpenControl_coordPath.Margin = new System.Windows.Forms.Padding(4);
            this.fileOpenControl_coordPath.Name = "fileOpenControl_coordPath";
            this.fileOpenControl_coordPath.Size = new System.Drawing.Size(428, 22);
            this.fileOpenControl_coordPath.TabIndex = 65;
            // 
            // checkBox_IsApproxXyzRequired
            // 
            this.checkBox_IsApproxXyzRequired.AutoSize = true;
            this.checkBox_IsApproxXyzRequired.Location = new System.Drawing.Point(7, 20);
            this.checkBox_IsApproxXyzRequired.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_IsApproxXyzRequired.Name = "checkBox_IsApproxXyzRequired";
            this.checkBox_IsApproxXyzRequired.Size = new System.Drawing.Size(216, 16);
            this.checkBox_IsApproxXyzRequired.TabIndex = 30;
            this.checkBox_IsApproxXyzRequired.Text = "观测文件需要初始坐标(计算或指定)";
            this.checkBox_IsApproxXyzRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_indicateCoordfile
            // 
            this.checkBox_indicateCoordfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_indicateCoordfile.AutoSize = true;
            this.checkBox_indicateCoordfile.Location = new System.Drawing.Point(442, 72);
            this.checkBox_indicateCoordfile.Name = "checkBox_indicateCoordfile";
            this.checkBox_indicateCoordfile.Size = new System.Drawing.Size(96, 16);
            this.checkBox_indicateCoordfile.TabIndex = 66;
            this.checkBox_indicateCoordfile.Text = "指定坐标文件";
            this.checkBox_indicateCoordfile.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsSiteCoordServiceRequired
            // 
            this.checkBox_IsSiteCoordServiceRequired.AutoSize = true;
            this.checkBox_IsSiteCoordServiceRequired.ForeColor = System.Drawing.Color.Tomato;
            this.checkBox_IsSiteCoordServiceRequired.Location = new System.Drawing.Point(8, 46);
            this.checkBox_IsSiteCoordServiceRequired.Name = "checkBox_IsSiteCoordServiceRequired";
            this.checkBox_IsSiteCoordServiceRequired.Size = new System.Drawing.Size(120, 16);
            this.checkBox_IsSiteCoordServiceRequired.TabIndex = 48;
            this.checkBox_IsSiteCoordServiceRequired.Text = "启用测站坐标文件";
            this.checkBox_IsSiteCoordServiceRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsSetApproxXyzWithCoordService
            // 
            this.checkBox_IsSetApproxXyzWithCoordService.AutoSize = true;
            this.checkBox_IsSetApproxXyzWithCoordService.Location = new System.Drawing.Point(239, 19);
            this.checkBox_IsSetApproxXyzWithCoordService.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_IsSetApproxXyzWithCoordService.Name = "checkBox_IsSetApproxXyzWithCoordService";
            this.checkBox_IsSetApproxXyzWithCoordService.Size = new System.Drawing.Size(168, 16);
            this.checkBox_IsSetApproxXyzWithCoordService.TabIndex = 30;
            this.checkBox_IsSetApproxXyzWithCoordService.Text = "采用坐标服务设置测站初值";
            this.checkBox_IsSetApproxXyzWithCoordService.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.namedFloatControl_fixedErrorVertical);
            this.groupBox3.Controls.Add(this.namedFloatControl_verticalCoefOfProprotion);
            this.groupBox3.Controls.Add(this.namedFloatControl_fixedErrorLevel);
            this.groupBox3.Controls.Add(this.namedFloatControl_levelCoefOfProprotion);
            this.groupBox3.Location = new System.Drawing.Point(3, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(558, 113);
            this.groupBox3.TabIndex = 52;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "接收机标称精度";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(421, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "注意：ppm 为百万分之一";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(389, 24);
            this.label2.TabIndex = 10;
            this.label2.Text = "按照 GB/T 18314-2009，三边同步环闭合差应满足：Wx <= √3 / 5 σ，\r\nB、C级复测基线长度较差应满足：Wx <= 2 √2 σ";
            // 
            // namedFloatControl_fixedErrorVertical
            // 
            this.namedFloatControl_fixedErrorVertical.Location = new System.Drawing.Point(212, 20);
            this.namedFloatControl_fixedErrorVertical.Name = "namedFloatControl_fixedErrorVertical";
            this.namedFloatControl_fixedErrorVertical.Size = new System.Drawing.Size(203, 23);
            this.namedFloatControl_fixedErrorVertical.TabIndex = 11;
            this.namedFloatControl_fixedErrorVertical.Title = "垂直固定误差(mm)：";
            this.namedFloatControl_fixedErrorVertical.Value = 10D;
            // 
            // namedFloatControl_verticalCoefOfProprotion
            // 
            this.namedFloatControl_verticalCoefOfProprotion.Location = new System.Drawing.Point(212, 46);
            this.namedFloatControl_verticalCoefOfProprotion.Name = "namedFloatControl_verticalCoefOfProprotion";
            this.namedFloatControl_verticalCoefOfProprotion.Size = new System.Drawing.Size(203, 23);
            this.namedFloatControl_verticalCoefOfProprotion.TabIndex = 9;
            this.namedFloatControl_verticalCoefOfProprotion.Title = "垂直比例系数(ppm)：";
            this.namedFloatControl_verticalCoefOfProprotion.Value = 1D;
            // 
            // namedFloatControl_fixedErrorLevel
            // 
            this.namedFloatControl_fixedErrorLevel.Location = new System.Drawing.Point(4, 20);
            this.namedFloatControl_fixedErrorLevel.Name = "namedFloatControl_fixedErrorLevel";
            this.namedFloatControl_fixedErrorLevel.Size = new System.Drawing.Size(203, 23);
            this.namedFloatControl_fixedErrorLevel.TabIndex = 13;
            this.namedFloatControl_fixedErrorLevel.Title = "水平固定误差(mm)：";
            this.namedFloatControl_fixedErrorLevel.Value = 5D;
            // 
            // namedFloatControl_levelCoefOfProprotion
            // 
            this.namedFloatControl_levelCoefOfProprotion.Location = new System.Drawing.Point(4, 46);
            this.namedFloatControl_levelCoefOfProprotion.Name = "namedFloatControl_levelCoefOfProprotion";
            this.namedFloatControl_levelCoefOfProprotion.Size = new System.Drawing.Size(203, 23);
            this.namedFloatControl_levelCoefOfProprotion.TabIndex = 12;
            this.namedFloatControl_levelCoefOfProprotion.Title = "水平比例系数(ppm)：";
            this.namedFloatControl_levelCoefOfProprotion.Value = 1D;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.checkBox_IsEstDcbOfRceiver);
            this.groupBox2.Controls.Add(this.checkBox1IsEstimateTropWetZpd);
            this.groupBox2.Controls.Add(this.checkBox_IsFixingCoord);
            this.groupBox2.Controls.Add(this.checkBox_updateStationInfo);
            this.groupBox2.Location = new System.Drawing.Point(5, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(563, 42);
            this.groupBox2.TabIndex = 40;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "通用选项与参数估计";
            // 
            // checkBox_IsEstDcbOfRceiver
            // 
            this.checkBox_IsEstDcbOfRceiver.AutoSize = true;
            this.checkBox_IsEstDcbOfRceiver.Location = new System.Drawing.Point(231, 20);
            this.checkBox_IsEstDcbOfRceiver.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_IsEstDcbOfRceiver.Name = "checkBox_IsEstDcbOfRceiver";
            this.checkBox_IsEstDcbOfRceiver.Size = new System.Drawing.Size(126, 16);
            this.checkBox_IsEstDcbOfRceiver.TabIndex = 30;
            this.checkBox_IsEstDcbOfRceiver.Text = "是否估计接收机DCB";
            this.checkBox_IsEstDcbOfRceiver.UseVisualStyleBackColor = true;
            // 
            // checkBox1IsEstimateTropWetZpd
            // 
            this.checkBox1IsEstimateTropWetZpd.AutoSize = true;
            this.checkBox1IsEstimateTropWetZpd.Location = new System.Drawing.Point(364, 20);
            this.checkBox1IsEstimateTropWetZpd.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox1IsEstimateTropWetZpd.Name = "checkBox1IsEstimateTropWetZpd";
            this.checkBox1IsEstimateTropWetZpd.Size = new System.Drawing.Size(192, 16);
            this.checkBox1IsEstimateTropWetZpd.TabIndex = 30;
            this.checkBox1IsEstimateTropWetZpd.Text = "是否估计测站对流层湿延迟参数";
            this.checkBox1IsEstimateTropWetZpd.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsFixingCoord
            // 
            this.checkBox_IsFixingCoord.AutoSize = true;
            this.checkBox_IsFixingCoord.Location = new System.Drawing.Point(155, 20);
            this.checkBox_IsFixingCoord.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_IsFixingCoord.Name = "checkBox_IsFixingCoord";
            this.checkBox_IsFixingCoord.Size = new System.Drawing.Size(72, 16);
            this.checkBox_IsFixingCoord.TabIndex = 30;
            this.checkBox_IsFixingCoord.Text = "固定坐标";
            this.checkBox_IsFixingCoord.UseVisualStyleBackColor = true;
            // 
            // checkBox_updateStationInfo
            // 
            this.checkBox_updateStationInfo.AutoSize = true;
            this.checkBox_updateStationInfo.Location = new System.Drawing.Point(5, 20);
            this.checkBox_updateStationInfo.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_updateStationInfo.Name = "checkBox_updateStationInfo";
            this.checkBox_updateStationInfo.Size = new System.Drawing.Size(144, 16);
            this.checkBox_updateStationInfo.TabIndex = 30;
            this.checkBox_updateStationInfo.Text = "外部文件更新测站信息";
            this.checkBox_updateStationInfo.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox_IsSmoothRangeWhenPrevPseudorangePosition);
            this.groupBox1.Controls.Add(this.checkBox_IsNeedPseudorangePositionWhenProcess);
            this.groupBox1.Controls.Add(this.namedFloatControl_rmsOfWhitenoiceOfDynamicPos);
            this.groupBox1.Controls.Add(this.checkBox_IsUpdateEstimatePostition);
            this.groupBox1.Location = new System.Drawing.Point(10, 337);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(504, 67);
            this.groupBox1.TabIndex = 39;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "动态定位选项";
            // 
            // checkBox_IsSmoothRangeWhenPrevPseudorangePosition
            // 
            this.checkBox_IsSmoothRangeWhenPrevPseudorangePosition.AutoSize = true;
            this.checkBox_IsSmoothRangeWhenPrevPseudorangePosition.Location = new System.Drawing.Point(297, 17);
            this.checkBox_IsSmoothRangeWhenPrevPseudorangePosition.Name = "checkBox_IsSmoothRangeWhenPrevPseudorangePosition";
            this.checkBox_IsSmoothRangeWhenPrevPseudorangePosition.Size = new System.Drawing.Size(144, 16);
            this.checkBox_IsSmoothRangeWhenPrevPseudorangePosition.TabIndex = 47;
            this.checkBox_IsSmoothRangeWhenPrevPseudorangePosition.Text = "伪距预先定位平滑伪距";
            this.checkBox_IsSmoothRangeWhenPrevPseudorangePosition.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsNeedPseudorangePositionWhenProcess
            // 
            this.checkBox_IsNeedPseudorangePositionWhenProcess.AutoSize = true;
            this.checkBox_IsNeedPseudorangePositionWhenProcess.Location = new System.Drawing.Point(168, 18);
            this.checkBox_IsNeedPseudorangePositionWhenProcess.Name = "checkBox_IsNeedPseudorangePositionWhenProcess";
            this.checkBox_IsNeedPseudorangePositionWhenProcess.Size = new System.Drawing.Size(96, 16);
            this.checkBox_IsNeedPseudorangePositionWhenProcess.TabIndex = 47;
            this.checkBox_IsNeedPseudorangePositionWhenProcess.Text = "伪距预先定位";
            this.checkBox_IsNeedPseudorangePositionWhenProcess.UseVisualStyleBackColor = true;
            // 
            // namedFloatControl_rmsOfWhitenoiceOfDynamicPos
            // 
            this.namedFloatControl_rmsOfWhitenoiceOfDynamicPos.Location = new System.Drawing.Point(21, 44);
            this.namedFloatControl_rmsOfWhitenoiceOfDynamicPos.Margin = new System.Windows.Forms.Padding(4);
            this.namedFloatControl_rmsOfWhitenoiceOfDynamicPos.Name = "namedFloatControl_rmsOfWhitenoiceOfDynamicPos";
            this.namedFloatControl_rmsOfWhitenoiceOfDynamicPos.Size = new System.Drawing.Size(267, 23);
            this.namedFloatControl_rmsOfWhitenoiceOfDynamicPos.TabIndex = 31;
            this.namedFloatControl_rmsOfWhitenoiceOfDynamicPos.Title = "动态定位白噪声模型：";
            this.namedFloatControl_rmsOfWhitenoiceOfDynamicPos.Value = 0.1D;
            // 
            // checkBox_IsUpdateEstimatePostition
            // 
            this.checkBox_IsUpdateEstimatePostition.AutoSize = true;
            this.checkBox_IsUpdateEstimatePostition.Location = new System.Drawing.Point(21, 17);
            this.checkBox_IsUpdateEstimatePostition.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_IsUpdateEstimatePostition.Name = "checkBox_IsUpdateEstimatePostition";
            this.checkBox_IsUpdateEstimatePostition.Size = new System.Drawing.Size(120, 16);
            this.checkBox_IsUpdateEstimatePostition.TabIndex = 30;
            this.checkBox_IsUpdateEstimatePostition.Text = "更新测站估值坐标";
            this.checkBox_IsUpdateEstimatePostition.UseVisualStyleBackColor = true;
            // 
            // enumRadioControl_positionType
            // 
            this.enumRadioControl_positionType.IsReady = false;
            this.enumRadioControl_positionType.Location = new System.Drawing.Point(7, 285);
            this.enumRadioControl_positionType.Margin = new System.Windows.Forms.Padding(4);
            this.enumRadioControl_positionType.Name = "enumRadioControl_positionType";
            this.enumRadioControl_positionType.Size = new System.Drawing.Size(291, 48);
            this.enumRadioControl_positionType.TabIndex = 38;
            this.enumRadioControl_positionType.Title = "定位/定轨类型";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Controls.Add(this.radioButton_loosePrecise);
            this.groupBox6.Controls.Add(this.radioButton_commonPrecise);
            this.groupBox6.Controls.Add(this.namedFloatControl_MinAllowedApproxXyzLen);
            this.groupBox6.Controls.Add(this.radioButton_highPrecise);
            this.groupBox6.Controls.Add(this.label17);
            this.groupBox6.Controls.Add(this.label16);
            this.groupBox6.Controls.Add(this.checkBox_rmsIndicated);
            this.groupBox6.Controls.Add(this.checkBox_approxPos);
            this.groupBox6.Controls.Add(this.textBox_approxPosRms);
            this.groupBox6.Controls.Add(this.textBox_aprioriPos);
            this.groupBox6.Location = new System.Drawing.Point(2, 51);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox6.Size = new System.Drawing.Size(509, 129);
            this.groupBox6.TabIndex = 34;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "接收机初始坐标(网解时为基准站(第一个)坐标)";
            // 
            // radioButton_loosePrecise
            // 
            this.radioButton_loosePrecise.AutoSize = true;
            this.radioButton_loosePrecise.Location = new System.Drawing.Point(175, 78);
            this.radioButton_loosePrecise.Name = "radioButton_loosePrecise";
            this.radioButton_loosePrecise.Size = new System.Drawing.Size(59, 16);
            this.radioButton_loosePrecise.TabIndex = 31;
            this.radioButton_loosePrecise.TabStop = true;
            this.radioButton_loosePrecise.Text = "1000米";
            this.radioButton_loosePrecise.UseVisualStyleBackColor = true;
            this.radioButton_loosePrecise.CheckedChanged += new System.EventHandler(this.radioButton_loosePrecise_CheckedChanged);
            // 
            // radioButton_commonPrecise
            // 
            this.radioButton_commonPrecise.AutoSize = true;
            this.radioButton_commonPrecise.Location = new System.Drawing.Point(122, 78);
            this.radioButton_commonPrecise.Name = "radioButton_commonPrecise";
            this.radioButton_commonPrecise.Size = new System.Drawing.Size(47, 16);
            this.radioButton_commonPrecise.TabIndex = 31;
            this.radioButton_commonPrecise.TabStop = true;
            this.radioButton_commonPrecise.Text = "10米";
            this.radioButton_commonPrecise.UseVisualStyleBackColor = true;
            this.radioButton_commonPrecise.CheckedChanged += new System.EventHandler(this.radioButton_commonPrecise_CheckedChanged);
            // 
            // radioButton_highPrecise
            // 
            this.radioButton_highPrecise.AutoSize = true;
            this.radioButton_highPrecise.Location = new System.Drawing.Point(63, 78);
            this.radioButton_highPrecise.Name = "radioButton_highPrecise";
            this.radioButton_highPrecise.Size = new System.Drawing.Size(53, 16);
            this.radioButton_highPrecise.TabIndex = 31;
            this.radioButton_highPrecise.TabStop = true;
            this.radioButton_highPrecise.Text = "0.1米";
            this.radioButton_highPrecise.UseVisualStyleBackColor = true;
            this.radioButton_highPrecise.CheckedChanged += new System.EventHandler(this.radioButton_highPrecise_CheckedChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(8, 53);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(59, 12);
            this.label17.TabIndex = 5;
            this.label17.Text = "坐标RMS：";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(5, 25);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(65, 12);
            this.label16.TabIndex = 5;
            this.label16.Text = "概略坐标：";
            // 
            // checkBox_rmsIndicated
            // 
            this.checkBox_rmsIndicated.AutoSize = true;
            this.checkBox_rmsIndicated.Location = new System.Drawing.Point(398, 54);
            this.checkBox_rmsIndicated.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_rmsIndicated.Name = "checkBox_rmsIndicated";
            this.checkBox_rmsIndicated.Size = new System.Drawing.Size(96, 16);
            this.checkBox_rmsIndicated.TabIndex = 30;
            this.checkBox_rmsIndicated.Text = "指定精度信息";
            this.checkBox_rmsIndicated.UseVisualStyleBackColor = true;
            // 
            // checkBox_approxPos
            // 
            this.checkBox_approxPos.AutoSize = true;
            this.checkBox_approxPos.Location = new System.Drawing.Point(398, 27);
            this.checkBox_approxPos.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_approxPos.Name = "checkBox_approxPos";
            this.checkBox_approxPos.Size = new System.Drawing.Size(96, 16);
            this.checkBox_approxPos.TabIndex = 30;
            this.checkBox_approxPos.Text = "指定概略坐标";
            this.checkBox_approxPos.UseVisualStyleBackColor = true;
            // 
            // textBox_approxPosRms
            // 
            this.textBox_approxPosRms.Location = new System.Drawing.Point(71, 51);
            this.textBox_approxPosRms.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_approxPosRms.Name = "textBox_approxPosRms";
            this.textBox_approxPosRms.Size = new System.Drawing.Size(300, 21);
            this.textBox_approxPosRms.TabIndex = 23;
            this.textBox_approxPosRms.Text = "10, 10, 10";
            // 
            // textBox_aprioriPos
            // 
            this.textBox_aprioriPos.Location = new System.Drawing.Point(72, 22);
            this.textBox_aprioriPos.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_aprioriPos.Name = "textBox_aprioriPos";
            this.textBox_aprioriPos.Size = new System.Drawing.Size(300, 21);
            this.textBox_aprioriPos.TabIndex = 23;
            this.textBox_aprioriPos.Text = "0, 0, 0";
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
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(574, 381);
            this.tabPage1.TabIndex = 8;
            this.tabPage1.Text = "接收机精度指标";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // namedFloatControl_MinAllowedApproxXyzLen
            // 
            this.namedFloatControl_MinAllowedApproxXyzLen.Location = new System.Drawing.Point(12, 104);
            this.namedFloatControl_MinAllowedApproxXyzLen.Margin = new System.Windows.Forms.Padding(4);
            this.namedFloatControl_MinAllowedApproxXyzLen.Name = "namedFloatControl_MinAllowedApproxXyzLen";
            this.namedFloatControl_MinAllowedApproxXyzLen.Size = new System.Drawing.Size(216, 23);
            this.namedFloatControl_MinAllowedApproxXyzLen.TabIndex = 31;
            this.namedFloatControl_MinAllowedApproxXyzLen.Title = "初始坐标最小地心距离：";
            this.namedFloatControl_MinAllowedApproxXyzLen.Value = 0.1D;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(242, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(221, 12);
            this.label4.TabIndex = 32;
            this.label4.Text = "小于此则采用伪距定位重新计算初始坐标";
            // 
            // SiteReceiverOptionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "SiteReceiverOptionPage";
            this.Size = new System.Drawing.Size(582, 526);
            this.Load += new System.EventHandler(this.GnssOptionForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_receiver.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.RadioButton radioButton_loosePrecise;
        private System.Windows.Forms.RadioButton radioButton_commonPrecise;
        private System.Windows.Forms.RadioButton radioButton_highPrecise;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.CheckBox checkBox_rmsIndicated;
        private System.Windows.Forms.CheckBox checkBox_approxPos;
        private System.Windows.Forms.TextBox textBox_approxPosRms;
        private System.Windows.Forms.TextBox textBox_aprioriPos;
        private System.Windows.Forms.OpenFileDialog openFileDialog_nav;
        private System.Windows.Forms.OpenFileDialog openFileDialog_obs;
        private System.Windows.Forms.OpenFileDialog openFileDialog_clock;
        private System.Windows.Forms.CheckBox checkBox_IsUpdateEstimatePostition;
        private System.Windows.Forms.TabPage tabPage_receiver;
        private System.Windows.Forms.CheckBox checkBox_IsApproxXyzRequired;
        private System.Windows.Forms.CheckBox checkBox_IsSetApproxXyzWithCoordService;
        private System.Windows.Forms.CheckBox checkBox_updateStationInfo;
        private System.Windows.Forms.CheckBox checkBox_IsFixingCoord;
        private Geo.Winform.EnumRadioControl enumRadioControl_positionType;
        private System.Windows.Forms.GroupBox groupBox1;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_rmsOfWhitenoiceOfDynamicPos;
        private System.Windows.Forms.CheckBox checkBox_IsNeedPseudorangePositionWhenProcess;
        private System.Windows.Forms.CheckBox checkBox_IsEstDcbOfRceiver;
        private System.Windows.Forms.CheckBox checkBox_IsSmoothRangeWhenPrevPseudorangePosition;
        private System.Windows.Forms.CheckBox checkBox1IsEstimateTropWetZpd;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_fixedErrorVertical;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_verticalCoefOfProprotion;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_fixedErrorLevel;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_levelCoefOfProprotion;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox checkBox_IsSiteCoordServiceRequired;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_coordPath;
        private System.Windows.Forms.CheckBox checkBox_indicateCoordfile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPage1;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_MinAllowedApproxXyzLen;
        private System.Windows.Forms.Label label4;
    }
}