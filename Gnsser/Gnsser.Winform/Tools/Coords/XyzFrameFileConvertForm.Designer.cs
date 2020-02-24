namespace Gnsser.Winform
{
    partial class XyzFrameFileConvertForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.checkBox1_est7params = new System.Windows.Forms.CheckBox();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.fileOpenControl_xyz = new Geo.Winform.Controls.FileOpenControl();
            this.label2 = new System.Windows.Forms.Label();
            this.fileOpenControl_knownCoord = new Geo.Winform.Controls.FileOpenControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.xyzTransParamControl1 = new Geo.Winform.Controls.XyzFrameTrans7ParamControl();
            this.label1 = new System.Windows.Forms.Label();
            this.button_run = new System.Windows.Forms.Button();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.objectTableControl_inputXyz = new Geo.Winform.ObjectTableControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.objectTableControl_knownCoord = new Geo.Winform.ObjectTableControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.objectTableControl_commonCoordToBeConvert = new Geo.Winform.ObjectTableControl();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.objectTableControl_knownCommonCoord = new Geo.Winform.ObjectTableControl();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.tabControl4 = new System.Windows.Forms.TabControl();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.objectTableControl_resultXy = new Geo.Winform.ObjectTableControl();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.objectTableControl1_adjustCompare = new Geo.Winform.ObjectTableControl();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.objectTableControl_compareBeforce = new Geo.Winform.ObjectTableControl();
            this.tabPage12 = new System.Windows.Forms.TabPage();
            this.objectTableControl_residuals = new Geo.Winform.ObjectTableControl();
            this.button_showOnMap = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage10.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.tabControl4.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.tabPage11.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage12.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl3);
            this.splitContainer1.Size = new System.Drawing.Size(924, 522);
            this.splitContainer1.SplitterDistance = 184;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.button_showOnMap);
            this.splitContainer2.Panel2.Controls.Add(this.label1);
            this.splitContainer2.Panel2.Controls.Add(this.button_run);
            this.splitContainer2.Size = new System.Drawing.Size(924, 184);
            this.splitContainer2.SplitterDistance = 139;
            this.splitContainer2.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(924, 139);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.checkBox1_est7params);
            this.tabPage5.Controls.Add(this.directorySelectionControl1);
            this.tabPage5.Controls.Add(this.fileOpenControl_xyz);
            this.tabPage5.Controls.Add(this.label2);
            this.tabPage5.Controls.Add(this.fileOpenControl_knownCoord);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(916, 113);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "输入文件";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // checkBox1_est7params
            // 
            this.checkBox1_est7params.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1_est7params.AutoSize = true;
            this.checkBox1_est7params.Checked = true;
            this.checkBox1_est7params.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1_est7params.Location = new System.Drawing.Point(770, 40);
            this.checkBox1_est7params.Name = "checkBox1_est7params";
            this.checkBox1_est7params.Size = new System.Drawing.Size(132, 16);
            this.checkBox1_est7params.TabIndex = 9;
            this.checkBox1_est7params.Text = "启用，且估计七参数";
            this.checkBox1_est7params.UseVisualStyleBackColor = true;
            this.checkBox1_est7params.CheckedChanged += new System.EventHandler(this.checkBox1_est7params_CheckedChanged);
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl1.IsAddOrReplase = false;
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "输出文件夹：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(26, 85);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "D:\\ProgramFiles\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\Temp";
            this.directorySelectionControl1.Pathes = new string[] {
        "D:\\ProgramFiles\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\Temp"};
            this.directorySelectionControl1.Size = new System.Drawing.Size(876, 22);
            this.directorySelectionControl1.TabIndex = 10;
            // 
            // fileOpenControl_xyz
            // 
            this.fileOpenControl_xyz.AllowDrop = true;
            this.fileOpenControl_xyz.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_xyz.FilePath = "";
            this.fileOpenControl_xyz.FilePathes = new string[0];
            this.fileOpenControl_xyz.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_xyz.FirstPath = "";
            this.fileOpenControl_xyz.IsMultiSelect = false;
            this.fileOpenControl_xyz.LabelName = "待转XYZ坐标文件：";
            this.fileOpenControl_xyz.Location = new System.Drawing.Point(6, 6);
            this.fileOpenControl_xyz.Name = "fileOpenControl_xyz";
            this.fileOpenControl_xyz.Size = new System.Drawing.Size(900, 22);
            this.fileOpenControl_xyz.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(85, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(233, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "坐标一行一个，必须列名包含X、Y、Z 字符";
            // 
            // fileOpenControl_knownCoord
            // 
            this.fileOpenControl_knownCoord.AllowDrop = true;
            this.fileOpenControl_knownCoord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_knownCoord.FilePath = "";
            this.fileOpenControl_knownCoord.FilePathes = new string[0];
            this.fileOpenControl_knownCoord.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_knownCoord.FirstPath = "";
            this.fileOpenControl_knownCoord.IsMultiSelect = false;
            this.fileOpenControl_knownCoord.LabelName = "已知目标框架XYZ坐标：";
            this.fileOpenControl_knownCoord.Location = new System.Drawing.Point(6, 34);
            this.fileOpenControl_knownCoord.Name = "fileOpenControl_knownCoord";
            this.fileOpenControl_knownCoord.Size = new System.Drawing.Size(745, 22);
            this.fileOpenControl_knownCoord.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.xyzTransParamControl1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(916, 113);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "设置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // xyzTransParamControl1
            // 
            this.xyzTransParamControl1.Location = new System.Drawing.Point(8, 6);
            this.xyzTransParamControl1.Name = "xyzTransParamControl1";
            this.xyzTransParamControl1.Size = new System.Drawing.Size(519, 96);
            this.xyzTransParamControl1.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(119, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "注意：参数估计必须 3 对坐标以上。";
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(837, 3);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 35);
            this.button_run.TabIndex = 2;
            this.button_run.Text = "计算";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage7);
            this.tabControl3.Controls.Add(this.tabPage8);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(0, 0);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(924, 334);
            this.tabControl3.TabIndex = 1;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.tabControl2);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(916, 308);
            this.tabPage7.TabIndex = 0;
            this.tabPage7.Text = "输入数据";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Controls.Add(this.tabPage10);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(3, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(910, 302);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.objectTableControl_inputXyz);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(902, 276);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "待转三维XYZ坐标";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_inputXyz
            // 
            this.objectTableControl_inputXyz.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_inputXyz.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_inputXyz.Name = "objectTableControl_inputXyz";
            this.objectTableControl_inputXyz.Size = new System.Drawing.Size(896, 270);
            this.objectTableControl_inputXyz.TabIndex = 0;
            this.objectTableControl_inputXyz.TableObjectStorage = null;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.objectTableControl_knownCoord);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(902, 276);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "已知框架坐标";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_knownCoord
            // 
            this.objectTableControl_knownCoord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_knownCoord.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_knownCoord.Name = "objectTableControl_knownCoord";
            this.objectTableControl_knownCoord.Size = new System.Drawing.Size(896, 270);
            this.objectTableControl_knownCoord.TabIndex = 1;
            this.objectTableControl_knownCoord.TableObjectStorage = null;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.objectTableControl_commonCoordToBeConvert);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(902, 276);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "待转公共坐标";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_commonCoordToBeConvert
            // 
            this.objectTableControl_commonCoordToBeConvert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_commonCoordToBeConvert.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_commonCoordToBeConvert.Name = "objectTableControl_commonCoordToBeConvert";
            this.objectTableControl_commonCoordToBeConvert.Size = new System.Drawing.Size(896, 270);
            this.objectTableControl_commonCoordToBeConvert.TabIndex = 2;
            this.objectTableControl_commonCoordToBeConvert.TableObjectStorage = null;
            // 
            // tabPage10
            // 
            this.tabPage10.Controls.Add(this.objectTableControl_knownCommonCoord);
            this.tabPage10.Location = new System.Drawing.Point(4, 22);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage10.Size = new System.Drawing.Size(902, 276);
            this.tabPage10.TabIndex = 3;
            this.tabPage10.Text = "已知公共坐标";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_knownCommonCoord
            // 
            this.objectTableControl_knownCommonCoord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_knownCommonCoord.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_knownCommonCoord.Name = "objectTableControl_knownCommonCoord";
            this.objectTableControl_knownCommonCoord.Size = new System.Drawing.Size(896, 270);
            this.objectTableControl_knownCommonCoord.TabIndex = 2;
            this.objectTableControl_knownCommonCoord.TableObjectStorage = null;
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.tabControl4);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(916, 308);
            this.tabPage8.TabIndex = 1;
            this.tabPage8.Text = "计算结果";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // tabControl4
            // 
            this.tabControl4.Controls.Add(this.tabPage9);
            this.tabControl4.Controls.Add(this.tabPage11);
            this.tabControl4.Controls.Add(this.tabPage6);
            this.tabControl4.Controls.Add(this.tabPage12);
            this.tabControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl4.Location = new System.Drawing.Point(3, 3);
            this.tabControl4.Name = "tabControl4";
            this.tabControl4.SelectedIndex = 0;
            this.tabControl4.Size = new System.Drawing.Size(910, 302);
            this.tabControl4.TabIndex = 0;
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.objectTableControl_resultXy);
            this.tabPage9.Location = new System.Drawing.Point(4, 22);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(902, 276);
            this.tabPage9.TabIndex = 0;
            this.tabPage9.Text = "平差结果-目标参考框架";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_resultXy
            // 
            this.objectTableControl_resultXy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_resultXy.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_resultXy.Name = "objectTableControl_resultXy";
            this.objectTableControl_resultXy.Size = new System.Drawing.Size(896, 270);
            this.objectTableControl_resultXy.TabIndex = 2;
            this.objectTableControl_resultXy.TableObjectStorage = null;
            // 
            // tabPage11
            // 
            this.tabPage11.Controls.Add(this.objectTableControl1_adjustCompare);
            this.tabPage11.Location = new System.Drawing.Point(4, 22);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage11.Size = new System.Drawing.Size(902, 276);
            this.tabPage11.TabIndex = 2;
            this.tabPage11.Text = "平差后与已知坐标比较";
            this.tabPage11.UseVisualStyleBackColor = true;
            // 
            // objectTableControl1_adjustCompare
            // 
            this.objectTableControl1_adjustCompare.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl1_adjustCompare.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl1_adjustCompare.Name = "objectTableControl1_adjustCompare";
            this.objectTableControl1_adjustCompare.Size = new System.Drawing.Size(896, 270);
            this.objectTableControl1_adjustCompare.TabIndex = 3;
            this.objectTableControl1_adjustCompare.TableObjectStorage = null;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.objectTableControl_compareBeforce);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(902, 276);
            this.tabPage6.TabIndex = 3;
            this.tabPage6.Text = "平差前与已知比较";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_compareBeforce
            // 
            this.objectTableControl_compareBeforce.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_compareBeforce.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_compareBeforce.Name = "objectTableControl_compareBeforce";
            this.objectTableControl_compareBeforce.Size = new System.Drawing.Size(896, 270);
            this.objectTableControl_compareBeforce.TabIndex = 4;
            this.objectTableControl_compareBeforce.TableObjectStorage = null;
            // 
            // tabPage12
            // 
            this.tabPage12.Controls.Add(this.objectTableControl_residuals);
            this.tabPage12.Location = new System.Drawing.Point(4, 22);
            this.tabPage12.Name = "tabPage12";
            this.tabPage12.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage12.Size = new System.Drawing.Size(902, 276);
            this.tabPage12.TabIndex = 4;
            this.tabPage12.Text = "输入数据平差前后比较";
            this.tabPage12.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_residuals
            // 
            this.objectTableControl_residuals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_residuals.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_residuals.Name = "objectTableControl_residuals";
            this.objectTableControl_residuals.Size = new System.Drawing.Size(896, 270);
            this.objectTableControl_residuals.TabIndex = 5;
            this.objectTableControl_residuals.TableObjectStorage = null;
            // 
            // button_showOnMap
            // 
            this.button_showOnMap.Location = new System.Drawing.Point(3, 9);
            this.button_showOnMap.Name = "button_showOnMap";
            this.button_showOnMap.Size = new System.Drawing.Size(75, 23);
            this.button_showOnMap.TabIndex = 8;
            this.button_showOnMap.Text = "地图显示";
            this.button_showOnMap.UseVisualStyleBackColor = true;
            this.button_showOnMap.Click += new System.EventHandler(this.button_showOnMap_Click);
            // 
            // XyzFrameFileConvertForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 522);
            this.Controls.Add(this.splitContainer1);
            this.Name = "XyzFrameFileConvertForm";
            this.Text = "XyzToGausssXyForm";
            this.Load += new System.EventHandler(this.XyzToGausssXyForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage10.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.tabControl4.ResumeLayout(false);
            this.tabPage9.ResumeLayout(false);
            this.tabPage11.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage12.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_run;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private System.Windows.Forms.Label label2;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_xyz;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_knownCoord;
        private Geo.Winform.ObjectTableControl objectTableControl_inputXyz;
        private Geo.Winform.ObjectTableControl objectTableControl_knownCoord;
        private Geo.Winform.Controls.XyzFrameTrans7ParamControl xyzTransParamControl1;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.TabControl tabControl4;
        private System.Windows.Forms.TabPage tabPage9;
        private Geo.Winform.ObjectTableControl objectTableControl_resultXy;
        private System.Windows.Forms.TabPage tabPage11;
        private Geo.Winform.ObjectTableControl objectTableControl1_adjustCompare;
        private System.Windows.Forms.TabPage tabPage6;
        private Geo.Winform.ObjectTableControl objectTableControl_compareBeforce;
        private System.Windows.Forms.CheckBox checkBox1_est7params;
        private System.Windows.Forms.TabPage tabPage2;
        private Geo.Winform.ObjectTableControl objectTableControl_commonCoordToBeConvert;
        private System.Windows.Forms.TabPage tabPage10;
        private Geo.Winform.ObjectTableControl objectTableControl_knownCommonCoord;
        private System.Windows.Forms.TabPage tabPage12;
        private Geo.Winform.ObjectTableControl objectTableControl_residuals;
        private System.Windows.Forms.Button button_showOnMap;
    }
}