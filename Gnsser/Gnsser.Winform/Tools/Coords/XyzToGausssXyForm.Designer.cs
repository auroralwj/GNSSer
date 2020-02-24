namespace Gnsser.Winform
{
    partial class XyzToGausssXyForm
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
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.fileOpenControl_xyz = new Geo.Winform.Controls.FileOpenControl();
            this.label2 = new System.Windows.Forms.Label();
            this.fileOpenControl_gaussPlainXy = new Geo.Winform.Controls.FileOpenControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.namedFloatControl_orinalLonDeg = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_aveGeoHeight = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControlYConst = new Geo.Winform.Controls.NamedFloatControl();
            this.checkBox_indicated = new System.Windows.Forms.CheckBox();
            this.checkBox_isWithBeltNum = new System.Windows.Forms.CheckBox();
            this.checkBox_is3Belt = new System.Windows.Forms.CheckBox();
            this.ellipsoidSelectControl1 = new Geo.Winform.EllipsoidSelectControl();
            this.enumRadioControl_angleUnit = new Geo.Winform.EnumRadioControl();
            this.plainXyTransParamControl1 = new Geo.Winform.Controls.PlainXyTransParamControl();
            this.label1 = new System.Windows.Forms.Label();
            this.button_run = new System.Windows.Forms.Button();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.objectTableControl_inputXyz = new Geo.Winform.ObjectTableControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.objectTableControl_knownGaussXy = new Geo.Winform.ObjectTableControl();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.tabControl4 = new System.Windows.Forms.TabControl();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.objectTableControl_resultXy = new Geo.Winform.ObjectTableControl();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.objectTableControl1_convertGsussXy = new Geo.Winform.ObjectTableControl();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.objectTableControl1_adjustCompare = new Geo.Winform.ObjectTableControl();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.objectTableControl_compareBeforce = new Geo.Winform.ObjectTableControl();
            this.tabPage12 = new System.Windows.Forms.TabPage();
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
            this.tabPage8.SuspendLayout();
            this.tabControl4.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.tabPage10.SuspendLayout();
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
            this.tabPage5.Controls.Add(this.directorySelectionControl1);
            this.tabPage5.Controls.Add(this.fileOpenControl_xyz);
            this.tabPage5.Controls.Add(this.label2);
            this.tabPage5.Controls.Add(this.fileOpenControl_gaussPlainXy);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(916, 113);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "输入文件";
            this.tabPage5.UseVisualStyleBackColor = true;
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
            this.fileOpenControl_xyz.LabelName = "三维XYZ坐标文件：";
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
            this.label2.Size = new System.Drawing.Size(323, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "坐标一行一个，必须列名包含X、Y、Z(高斯坐标不需要)字符";
            // 
            // fileOpenControl_gaussPlainXy
            // 
            this.fileOpenControl_gaussPlainXy.AllowDrop = true;
            this.fileOpenControl_gaussPlainXy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_gaussPlainXy.FilePath = "";
            this.fileOpenControl_gaussPlainXy.FilePathes = new string[0];
            this.fileOpenControl_gaussPlainXy.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_gaussPlainXy.FirstPath = "";
            this.fileOpenControl_gaussPlainXy.IsMultiSelect = false;
            this.fileOpenControl_gaussPlainXy.LabelName = "已知高斯平面坐标：";
            this.fileOpenControl_gaussPlainXy.Location = new System.Drawing.Point(6, 34);
            this.fileOpenControl_gaussPlainXy.Name = "fileOpenControl_gaussPlainXy";
            this.fileOpenControl_gaussPlainXy.Size = new System.Drawing.Size(900, 22);
            this.fileOpenControl_gaussPlainXy.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.namedFloatControl_orinalLonDeg);
            this.tabPage1.Controls.Add(this.namedFloatControl_aveGeoHeight);
            this.tabPage1.Controls.Add(this.namedFloatControlYConst);
            this.tabPage1.Controls.Add(this.checkBox_indicated);
            this.tabPage1.Controls.Add(this.checkBox_isWithBeltNum);
            this.tabPage1.Controls.Add(this.checkBox_is3Belt);
            this.tabPage1.Controls.Add(this.ellipsoidSelectControl1);
            this.tabPage1.Controls.Add(this.enumRadioControl_angleUnit);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(916, 113);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "设置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // namedFloatControl_orinalLonDeg
            // 
            this.namedFloatControl_orinalLonDeg.Location = new System.Drawing.Point(119, 86);
            this.namedFloatControl_orinalLonDeg.Name = "namedFloatControl_orinalLonDeg";
            this.namedFloatControl_orinalLonDeg.Size = new System.Drawing.Size(214, 23);
            this.namedFloatControl_orinalLonDeg.TabIndex = 3;
            this.namedFloatControl_orinalLonDeg.Title = "中央子午线(度小数)：";
            this.namedFloatControl_orinalLonDeg.Value = 99.5D;
            // 
            // namedFloatControl_aveGeoHeight
            // 
            this.namedFloatControl_aveGeoHeight.Location = new System.Drawing.Point(338, 61);
            this.namedFloatControl_aveGeoHeight.Name = "namedFloatControl_aveGeoHeight";
            this.namedFloatControl_aveGeoHeight.Size = new System.Drawing.Size(171, 23);
            this.namedFloatControl_aveGeoHeight.TabIndex = 3;
            this.namedFloatControl_aveGeoHeight.Title = "投影面大地高(m)：";
            this.namedFloatControl_aveGeoHeight.Value = 1500D;
            // 
            // namedFloatControlYConst
            // 
            this.namedFloatControlYConst.Location = new System.Drawing.Point(162, 61);
            this.namedFloatControlYConst.Name = "namedFloatControlYConst";
            this.namedFloatControlYConst.Size = new System.Drawing.Size(171, 23);
            this.namedFloatControlYConst.TabIndex = 3;
            this.namedFloatControlYConst.Title = "横轴Y加常数：";
            this.namedFloatControlYConst.Value = 500000D;
            // 
            // checkBox_indicated
            // 
            this.checkBox_indicated.AutoSize = true;
            this.checkBox_indicated.Location = new System.Drawing.Point(339, 86);
            this.checkBox_indicated.Name = "checkBox_indicated";
            this.checkBox_indicated.Size = new System.Drawing.Size(108, 16);
            this.checkBox_indicated.TabIndex = 2;
            this.checkBox_indicated.Text = "指定中央子午线";
            this.checkBox_indicated.UseVisualStyleBackColor = true;
            // 
            // checkBox_isWithBeltNum
            // 
            this.checkBox_isWithBeltNum.AutoSize = true;
            this.checkBox_isWithBeltNum.Location = new System.Drawing.Point(8, 88);
            this.checkBox_isWithBeltNum.Name = "checkBox_isWithBeltNum";
            this.checkBox_isWithBeltNum.Size = new System.Drawing.Size(78, 16);
            this.checkBox_isWithBeltNum.TabIndex = 2;
            this.checkBox_isWithBeltNum.Text = "Y轴加带号";
            this.checkBox_isWithBeltNum.UseVisualStyleBackColor = true;
            // 
            // checkBox_is3Belt
            // 
            this.checkBox_is3Belt.AutoSize = true;
            this.checkBox_is3Belt.Checked = true;
            this.checkBox_is3Belt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_is3Belt.Location = new System.Drawing.Point(8, 61);
            this.checkBox_is3Belt.Name = "checkBox_is3Belt";
            this.checkBox_is3Belt.Size = new System.Drawing.Size(132, 16);
            this.checkBox_is3Belt.TabIndex = 2;
            this.checkBox_is3Belt.Text = "三度带，否则六度带";
            this.checkBox_is3Belt.UseVisualStyleBackColor = true;
            // 
            // ellipsoidSelectControl1
            // 
            this.ellipsoidSelectControl1.Location = new System.Drawing.Point(515, 11);
            this.ellipsoidSelectControl1.Name = "ellipsoidSelectControl1";
            this.ellipsoidSelectControl1.Size = new System.Drawing.Size(459, 93);
            this.ellipsoidSelectControl1.TabIndex = 1;
            // 
            // enumRadioControl_angleUnit
            // 
            this.enumRadioControl_angleUnit.IsReady = false;
            this.enumRadioControl_angleUnit.Location = new System.Drawing.Point(8, 11);
            this.enumRadioControl_angleUnit.Name = "enumRadioControl_angleUnit";
            this.enumRadioControl_angleUnit.Size = new System.Drawing.Size(501, 44);
            this.enumRadioControl_angleUnit.TabIndex = 0;
            this.enumRadioControl_angleUnit.Title = "角度单位";
            // 
            // plainXyTransParamControl1
            // 
            this.plainXyTransParamControl1.Location = new System.Drawing.Point(6, 6);
            this.plainXyTransParamControl1.Name = "plainXyTransParamControl1";
            this.plainXyTransParamControl1.Size = new System.Drawing.Size(519, 58);
            this.plainXyTransParamControl1.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "注意：参数估计必须 2 对坐标以上。";
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
            this.tabPage3.Text = "三维XYZ坐标";
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
            this.tabPage4.Controls.Add(this.objectTableControl_knownGaussXy);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(902, 276);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "已知高斯平面坐标";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_knownGaussXy
            // 
            this.objectTableControl_knownGaussXy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_knownGaussXy.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_knownGaussXy.Name = "objectTableControl_knownGaussXy";
            this.objectTableControl_knownGaussXy.Size = new System.Drawing.Size(896, 270);
            this.objectTableControl_knownGaussXy.TabIndex = 1;
            this.objectTableControl_knownGaussXy.TableObjectStorage = null;
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
            this.tabControl4.Controls.Add(this.tabPage10);
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
            this.tabPage9.Text = "平差结果-高斯坐标";
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
            // tabPage10
            // 
            this.tabPage10.Controls.Add(this.objectTableControl1_convertGsussXy);
            this.tabPage10.Location = new System.Drawing.Point(4, 22);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage10.Size = new System.Drawing.Size(902, 276);
            this.tabPage10.TabIndex = 1;
            this.tabPage10.Text = "平差前高斯平面坐标";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // objectTableControl1_convertGsussXy
            // 
            this.objectTableControl1_convertGsussXy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl1_convertGsussXy.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl1_convertGsussXy.Name = "objectTableControl1_convertGsussXy";
            this.objectTableControl1_convertGsussXy.Size = new System.Drawing.Size(896, 270);
            this.objectTableControl1_convertGsussXy.TabIndex = 3;
            this.objectTableControl1_convertGsussXy.TableObjectStorage = null;
            // 
            // tabPage11
            // 
            this.tabPage11.Controls.Add(this.objectTableControl1_adjustCompare);
            this.tabPage11.Location = new System.Drawing.Point(4, 22);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage11.Size = new System.Drawing.Size(902, 276);
            this.tabPage11.TabIndex = 2;
            this.tabPage11.Text = "平差后比较结果";
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
            this.tabPage6.Text = "平差前比较结果";
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
            this.tabPage12.Controls.Add(this.plainXyTransParamControl1);
            this.tabPage12.Location = new System.Drawing.Point(4, 22);
            this.tabPage12.Name = "tabPage12";
            this.tabPage12.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage12.Size = new System.Drawing.Size(902, 276);
            this.tabPage12.TabIndex = 4;
            this.tabPage12.Text = "转换参数";
            this.tabPage12.UseVisualStyleBackColor = true;
            // 
            // XyzToGausssXyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 522);
            this.Controls.Add(this.splitContainer1);
            this.Name = "XyzToGausssXyForm";
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
            this.tabPage1.PerformLayout();
            this.tabControl3.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.tabControl4.ResumeLayout(false);
            this.tabPage9.ResumeLayout(false);
            this.tabPage10.ResumeLayout(false);
            this.tabPage11.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage12.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_orinalLonDeg;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_aveGeoHeight;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControlYConst;
        private System.Windows.Forms.CheckBox checkBox_indicated;
        private System.Windows.Forms.CheckBox checkBox_isWithBeltNum;
        private System.Windows.Forms.CheckBox checkBox_is3Belt;
        private Geo.Winform.EllipsoidSelectControl ellipsoidSelectControl1;
        private Geo.Winform.EnumRadioControl enumRadioControl_angleUnit;
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
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_gaussPlainXy;
        private Geo.Winform.ObjectTableControl objectTableControl_inputXyz;
        private Geo.Winform.ObjectTableControl objectTableControl_knownGaussXy;
        private Geo.Winform.Controls.PlainXyTransParamControl plainXyTransParamControl1;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.TabControl tabControl4;
        private System.Windows.Forms.TabPage tabPage9;
        private Geo.Winform.ObjectTableControl objectTableControl_resultXy;
        private System.Windows.Forms.TabPage tabPage10;
        private Geo.Winform.ObjectTableControl objectTableControl1_convertGsussXy;
        private System.Windows.Forms.TabPage tabPage11;
        private Geo.Winform.ObjectTableControl objectTableControl1_adjustCompare;
        private System.Windows.Forms.TabPage tabPage6;
        private Geo.Winform.ObjectTableControl objectTableControl_compareBeforce;
        private System.Windows.Forms.TabPage tabPage12;
    }
}