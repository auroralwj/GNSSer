namespace Geo.Winform
{
    partial class MatrixEquationForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.tabControl4 = new System.Windows.Forms.TabControl();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.checkBox_enbaleFixParam = new System.Windows.Forms.CheckBox();
            this.checkBox_isMulti = new System.Windows.Forms.CheckBox();
            this.button_read = new System.Windows.Forms.Button();
            this.fileOpenControl_fixedParam = new Geo.Winform.Controls.FileOpenControl();
            this.fileOpenControl_eqPath = new Geo.Winform.Controls.FileOpenControl();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.namedStringControl_name = new Geo.Winform.Controls.NamedStringControl();
            this.namedStringControl_prefName = new Geo.Winform.Controls.NamedStringControl();
            this.namedIntControl_row = new Geo.Winform.Controls.NamedIntControl();
            this.namedIntControl_col = new Geo.Winform.Controls.NamedIntControl();
            this.button_randomGen = new System.Windows.Forms.Button();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_left = new Geo.Winform.Controls.RichTextBoxControl();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_right = new Geo.Winform.Controls.RichTextBoxControl();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_weightOfU = new Geo.Winform.Controls.RichTextBoxControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_saveAsBinary = new System.Windows.Forms.Button();
            this.button_saveAsText = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.namedIntControl_notShowOrder = new Geo.Winform.Controls.NamedIntControl();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_result = new Geo.Winform.Controls.RichTextBoxControl();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_normalEq = new Geo.Winform.Controls.RichTextBoxControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.objectTableControl_result = new Geo.Winform.ObjectTableControl();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.objectTableControl_residual = new Geo.Winform.ObjectTableControl();
            this.tabPage12 = new System.Windows.Forms.TabPage();
            this.objectTableControl_obs = new Geo.Winform.ObjectTableControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tabControl4.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage11.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage10.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(800, 507);
            this.splitContainer1.SplitterDistance = 216;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 216);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 190);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "输入";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Location = new System.Drawing.Point(3, 3);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.tabControl4);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.tabControl3);
            this.splitContainer3.Size = new System.Drawing.Size(786, 184);
            this.splitContainer3.SplitterDistance = 105;
            this.splitContainer3.TabIndex = 1;
            // 
            // tabControl4
            // 
            this.tabControl4.Controls.Add(this.tabPage7);
            this.tabControl4.Controls.Add(this.tabPage8);
            this.tabControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl4.Location = new System.Drawing.Point(0, 0);
            this.tabControl4.Name = "tabControl4";
            this.tabControl4.SelectedIndex = 0;
            this.tabControl4.Size = new System.Drawing.Size(786, 105);
            this.tabControl4.TabIndex = 2;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.checkBox_enbaleFixParam);
            this.tabPage7.Controls.Add(this.checkBox_isMulti);
            this.tabPage7.Controls.Add(this.button_read);
            this.tabPage7.Controls.Add(this.fileOpenControl_fixedParam);
            this.tabPage7.Controls.Add(this.fileOpenControl_eqPath);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(778, 79);
            this.tabPage7.TabIndex = 0;
            this.tabPage7.Text = "文件读取";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // checkBox_enbaleFixParam
            // 
            this.checkBox_enbaleFixParam.AutoSize = true;
            this.checkBox_enbaleFixParam.Location = new System.Drawing.Point(611, 52);
            this.checkBox_enbaleFixParam.Name = "checkBox_enbaleFixParam";
            this.checkBox_enbaleFixParam.Size = new System.Drawing.Size(48, 16);
            this.checkBox_enbaleFixParam.TabIndex = 2;
            this.checkBox_enbaleFixParam.Text = "启用";
            this.checkBox_enbaleFixParam.UseVisualStyleBackColor = true;
            this.checkBox_enbaleFixParam.CheckedChanged += new System.EventHandler(this.checkBox_enbaleFixParam_CheckedChanged);
            // 
            // checkBox_isMulti
            // 
            this.checkBox_isMulti.AutoSize = true;
            this.checkBox_isMulti.Location = new System.Drawing.Point(611, 11);
            this.checkBox_isMulti.Name = "checkBox_isMulti";
            this.checkBox_isMulti.Size = new System.Drawing.Size(48, 16);
            this.checkBox_isMulti.TabIndex = 2;
            this.checkBox_isMulti.Text = "批量";
            this.checkBox_isMulti.UseVisualStyleBackColor = true;
            // 
            // button_read
            // 
            this.button_read.Location = new System.Drawing.Point(678, 3);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(94, 40);
            this.button_read.TabIndex = 1;
            this.button_read.Text = "读取并计算";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_read_Click);
            // 
            // fileOpenControl_fixedParam
            // 
            this.fileOpenControl_fixedParam.AllowDrop = true;
            this.fileOpenControl_fixedParam.FilePath = "";
            this.fileOpenControl_fixedParam.FilePathes = new string[0];
            this.fileOpenControl_fixedParam.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_fixedParam.FirstPath = "";
            this.fileOpenControl_fixedParam.IsMultiSelect = false;
            this.fileOpenControl_fixedParam.LabelName = "固定参数/模糊度文件路径：";
            this.fileOpenControl_fixedParam.Location = new System.Drawing.Point(3, 46);
            this.fileOpenControl_fixedParam.Name = "fileOpenControl_fixedParam";
            this.fileOpenControl_fixedParam.Size = new System.Drawing.Size(592, 22);
            this.fileOpenControl_fixedParam.TabIndex = 0;
            // 
            // fileOpenControl_eqPath
            // 
            this.fileOpenControl_eqPath.AllowDrop = true;
            this.fileOpenControl_eqPath.FilePath = "";
            this.fileOpenControl_eqPath.FilePathes = new string[0];
            this.fileOpenControl_eqPath.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_eqPath.FirstPath = "";
            this.fileOpenControl_eqPath.IsMultiSelect = false;
            this.fileOpenControl_eqPath.LabelName = "矩阵方程文件路径：";
            this.fileOpenControl_eqPath.Location = new System.Drawing.Point(3, 6);
            this.fileOpenControl_eqPath.Name = "fileOpenControl_eqPath";
            this.fileOpenControl_eqPath.Size = new System.Drawing.Size(592, 22);
            this.fileOpenControl_eqPath.TabIndex = 0;
            this.fileOpenControl_eqPath.FilePathSetted += new System.EventHandler(this.fileOpenControl_eqPath_FilePathSetted);
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.namedStringControl_name);
            this.tabPage8.Controls.Add(this.namedStringControl_prefName);
            this.tabPage8.Controls.Add(this.namedIntControl_row);
            this.tabPage8.Controls.Add(this.namedIntControl_col);
            this.tabPage8.Controls.Add(this.button_randomGen);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(778, 79);
            this.tabPage8.TabIndex = 1;
            this.tabPage8.Text = "随机生成";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // namedStringControl_name
            // 
            this.namedStringControl_name.Location = new System.Drawing.Point(444, 6);
            this.namedStringControl_name.Name = "namedStringControl_name";
            this.namedStringControl_name.Size = new System.Drawing.Size(183, 23);
            this.namedStringControl_name.TabIndex = 4;
            this.namedStringControl_name.Title = "方程名称/标识符：";
            // 
            // namedStringControl_prefName
            // 
            this.namedStringControl_prefName.Location = new System.Drawing.Point(271, 6);
            this.namedStringControl_prefName.Name = "namedStringControl_prefName";
            this.namedStringControl_prefName.Size = new System.Drawing.Size(167, 23);
            this.namedStringControl_prefName.TabIndex = 4;
            this.namedStringControl_prefName.Title = "参数前缀名称：";
            // 
            // namedIntControl_row
            // 
            this.namedIntControl_row.Location = new System.Drawing.Point(6, 6);
            this.namedIntControl_row.Name = "namedIntControl_row";
            this.namedIntControl_row.Size = new System.Drawing.Size(119, 23);
            this.namedIntControl_row.TabIndex = 2;
            this.namedIntControl_row.Title = "行数：";
            this.namedIntControl_row.Value = 100;
            // 
            // namedIntControl_col
            // 
            this.namedIntControl_col.Location = new System.Drawing.Point(146, 6);
            this.namedIntControl_col.Name = "namedIntControl_col";
            this.namedIntControl_col.Size = new System.Drawing.Size(119, 23);
            this.namedIntControl_col.TabIndex = 1;
            this.namedIntControl_col.Title = "列数：";
            this.namedIntControl_col.Value = 10;
            // 
            // button_randomGen
            // 
            this.button_randomGen.Location = new System.Drawing.Point(650, 6);
            this.button_randomGen.Name = "button_randomGen";
            this.button_randomGen.Size = new System.Drawing.Size(75, 23);
            this.button_randomGen.TabIndex = 0;
            this.button_randomGen.Text = "生成";
            this.button_randomGen.UseVisualStyleBackColor = true;
            this.button_randomGen.Click += new System.EventHandler(this.button_randomGen_Click);
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage5);
            this.tabControl3.Controls.Add(this.tabPage6);
            this.tabControl3.Controls.Add(this.tabPage11);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(0, 0);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(786, 75);
            this.tabControl3.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.richTextBoxControl_left);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(778, 49);
            this.tabPage5.TabIndex = 0;
            this.tabPage5.Text = "方程左边";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_left
            // 
            this.richTextBoxControl_left.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_left.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_left.MaxAppendLineCount = 5000;
            this.richTextBoxControl_left.Name = "richTextBoxControl_left";
            this.richTextBoxControl_left.Size = new System.Drawing.Size(772, 43);
            this.richTextBoxControl_left.TabIndex = 1;
            this.richTextBoxControl_left.Text = "";
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.richTextBoxControl_right);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(778, 49);
            this.tabPage6.TabIndex = 1;
            this.tabPage6.Text = "方程右边";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_right
            // 
            this.richTextBoxControl_right.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_right.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_right.MaxAppendLineCount = 5000;
            this.richTextBoxControl_right.Name = "richTextBoxControl_right";
            this.richTextBoxControl_right.Size = new System.Drawing.Size(772, 43);
            this.richTextBoxControl_right.TabIndex = 1;
            this.richTextBoxControl_right.Text = "";
            // 
            // tabPage11
            // 
            this.tabPage11.Controls.Add(this.richTextBoxControl_weightOfU);
            this.tabPage11.Location = new System.Drawing.Point(4, 22);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage11.Size = new System.Drawing.Size(778, 49);
            this.tabPage11.TabIndex = 2;
            this.tabPage11.Text = "观测权逆阵";
            this.tabPage11.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_weightOfU
            // 
            this.richTextBoxControl_weightOfU.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_weightOfU.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_weightOfU.MaxAppendLineCount = 5000;
            this.richTextBoxControl_weightOfU.Name = "richTextBoxControl_weightOfU";
            this.richTextBoxControl_weightOfU.Size = new System.Drawing.Size(772, 43);
            this.richTextBoxControl_weightOfU.TabIndex = 2;
            this.richTextBoxControl_weightOfU.Text = "";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.directorySelectionControl1);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 190);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "输出";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.IsAddOrReplase = false;
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "输出文件夹：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(8, 17);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Enterprise\\Common7\\IDE\\Temp";
            this.directorySelectionControl1.Pathes = new string[] {
        "C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Enterprise\\Common7\\IDE\\Temp"};
            this.directorySelectionControl1.Size = new System.Drawing.Size(531, 22);
            this.directorySelectionControl1.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_saveAsBinary);
            this.groupBox1.Controls.Add(this.button_saveAsText);
            this.groupBox1.Location = new System.Drawing.Point(35, 109);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 53);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "当前矩阵方程存储";
            // 
            // button_saveAsBinary
            // 
            this.button_saveAsBinary.Location = new System.Drawing.Point(88, 20);
            this.button_saveAsBinary.Name = "button_saveAsBinary";
            this.button_saveAsBinary.Size = new System.Drawing.Size(88, 23);
            this.button_saveAsBinary.TabIndex = 2;
            this.button_saveAsBinary.Text = "另存为二进制";
            this.button_saveAsBinary.UseVisualStyleBackColor = true;
            this.button_saveAsBinary.Click += new System.EventHandler(this.button_saveAsBinary_Click);
            // 
            // button_saveAsText
            // 
            this.button_saveAsText.Location = new System.Drawing.Point(7, 19);
            this.button_saveAsText.Name = "button_saveAsText";
            this.button_saveAsText.Size = new System.Drawing.Size(75, 23);
            this.button_saveAsText.TabIndex = 2;
            this.button_saveAsText.Text = "另存为文本";
            this.button_saveAsText.UseVisualStyleBackColor = true;
            this.button_saveAsText.Click += new System.EventHandler(this.button_saveAsText_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.namedIntControl_notShowOrder);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControl2);
            this.splitContainer2.Size = new System.Drawing.Size(800, 287);
            this.splitContainer2.SplitterDistance = 36;
            this.splitContainer2.TabIndex = 0;
            // 
            // namedIntControl_notShowOrder
            // 
            this.namedIntControl_notShowOrder.Location = new System.Drawing.Point(13, 9);
            this.namedIntControl_notShowOrder.Name = "namedIntControl_notShowOrder";
            this.namedIntControl_notShowOrder.Size = new System.Drawing.Size(163, 23);
            this.namedIntControl_notShowOrder.TabIndex = 1;
            this.namedIntControl_notShowOrder.Title = "最大显示阶数：";
            this.namedIntControl_notShowOrder.Value = 1000;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage9);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage10);
            this.tabControl2.Controls.Add(this.tabPage12);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(800, 247);
            this.tabControl2.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.richTextBoxControl_result);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(792, 221);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "文本";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_result
            // 
            this.richTextBoxControl_result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_result.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_result.MaxAppendLineCount = 5000;
            this.richTextBoxControl_result.Name = "richTextBoxControl_result";
            this.richTextBoxControl_result.Size = new System.Drawing.Size(786, 215);
            this.richTextBoxControl_result.TabIndex = 0;
            this.richTextBoxControl_result.Text = "";
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.richTextBoxControl_normalEq);
            this.tabPage9.Location = new System.Drawing.Point(4, 22);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(792, 221);
            this.tabPage9.TabIndex = 2;
            this.tabPage9.Text = "法方程";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_normalEq
            // 
            this.richTextBoxControl_normalEq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_normalEq.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_normalEq.MaxAppendLineCount = 5000;
            this.richTextBoxControl_normalEq.Name = "richTextBoxControl_normalEq";
            this.richTextBoxControl_normalEq.Size = new System.Drawing.Size(786, 215);
            this.richTextBoxControl_normalEq.TabIndex = 1;
            this.richTextBoxControl_normalEq.Text = "";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.objectTableControl_result);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(792, 221);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "参数";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_result
            // 
            this.objectTableControl_result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_result.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_result.Name = "objectTableControl_result";
            this.objectTableControl_result.Size = new System.Drawing.Size(786, 215);
            this.objectTableControl_result.TabIndex = 0;
            this.objectTableControl_result.TableObjectStorage = null;
            // 
            // tabPage10
            // 
            this.tabPage10.Controls.Add(this.objectTableControl_residual);
            this.tabPage10.Location = new System.Drawing.Point(4, 22);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage10.Size = new System.Drawing.Size(792, 221);
            this.tabPage10.TabIndex = 3;
            this.tabPage10.Text = "残差";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_residual
            // 
            this.objectTableControl_residual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_residual.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_residual.Name = "objectTableControl_residual";
            this.objectTableControl_residual.Size = new System.Drawing.Size(786, 215);
            this.objectTableControl_residual.TabIndex = 1;
            this.objectTableControl_residual.TableObjectStorage = null;
            // 
            // tabPage12
            // 
            this.tabPage12.Controls.Add(this.objectTableControl_obs);
            this.tabPage12.Location = new System.Drawing.Point(4, 22);
            this.tabPage12.Name = "tabPage12";
            this.tabPage12.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage12.Size = new System.Drawing.Size(792, 221);
            this.tabPage12.TabIndex = 4;
            this.tabPage12.Text = "观测值";
            this.tabPage12.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_obs
            // 
            this.objectTableControl_obs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_obs.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_obs.Name = "objectTableControl_obs";
            this.objectTableControl_obs.Size = new System.Drawing.Size(786, 215);
            this.objectTableControl_obs.TabIndex = 2;
            this.objectTableControl_obs.TableObjectStorage = null;
            // 
            // MatrixEquationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 507);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MatrixEquationForm";
            this.Text = "矩阵方程";
            this.Load += new System.EventHandler(this.MatrixEquationForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tabControl4.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.tabPage8.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage11.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage9.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage10.ResumeLayout(false);
            this.tabPage12.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private ObjectTableControl objectTableControl_result;
        private Controls.RichTextBoxControl richTextBoxControl_result;
        private System.Windows.Forms.Button button_saveAsBinary;
        private System.Windows.Forms.Button button_saveAsText;
        private System.Windows.Forms.Button button_read;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private Controls.RichTextBoxControl richTextBoxControl_left;
        private Controls.RichTextBoxControl richTextBoxControl_right;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TabControl tabControl4;
        private System.Windows.Forms.TabPage tabPage7;
        private Controls.FileOpenControl fileOpenControl_eqPath;
        private System.Windows.Forms.TabPage tabPage8;
        private Controls.NamedIntControl namedIntControl_row;
        private Controls.NamedIntControl namedIntControl_col;
        private System.Windows.Forms.Button button_randomGen;
        private Controls.NamedStringControl namedStringControl_prefName;
        private Controls.NamedStringControl namedStringControl_name;
        private System.Windows.Forms.CheckBox checkBox_isMulti;
        private System.Windows.Forms.TabPage tabPage9;
        private Controls.RichTextBoxControl richTextBoxControl_normalEq;
        private System.Windows.Forms.TabPage tabPage10;
        private ObjectTableControl objectTableControl_residual;
        private System.Windows.Forms.TabPage tabPage11;
        private Controls.RichTextBoxControl richTextBoxControl_weightOfU;
        private Controls.FileOpenControl fileOpenControl_fixedParam;
        private System.Windows.Forms.CheckBox checkBox_enbaleFixParam;
        private Controls.DirectorySelectionControl directorySelectionControl1;
        private System.Windows.Forms.GroupBox groupBox1;
        private Controls.NamedIntControl namedIntControl_notShowOrder;
        private System.Windows.Forms.TabPage tabPage12;
        private ObjectTableControl objectTableControl_obs;
    }
}