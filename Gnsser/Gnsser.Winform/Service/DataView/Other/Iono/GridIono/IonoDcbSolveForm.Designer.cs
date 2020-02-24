namespace Gnsser.Winform
{
    partial class IonoDcbSolveForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.bindingSource_obsInfo = new System.Windows.Forms.BindingSource(this.components);
            this.button_view = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox_sat = new System.Windows.Forms.ComboBox();
            this.bindingSource_sat = new System.Windows.Forms.BindingSource(this.components);
            this.textBox_show = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_read = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.fileOpenControl1 = new Geo.Winform.Controls.FileOpenControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.attributeBox1 = new Geo.Winform.AttributeBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.objectTableControl1 = new Geo.Winform.ObjectTableControl();
            this.object_56fe9e41_2cbf_4ac3_8650_fc556669d280 = new System.Windows.Forms.BindingNavigator(this.components);
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_result = new Geo.Winform.Controls.RichTextBoxControl();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.radioButton_isUserRawValue = new System.Windows.Forms.RadioButton();
            this.radioButton_isUserPolyFitVal = new System.Windows.Forms.RadioButton();
            this.radioButton_isUsePhaaseSmoothP = new System.Windows.Forms.RadioButton();
            this.namedFloatControl_satCutoff = new Geo.Winform.Controls.NamedFloatControl();
            this.namedIntControl_smoothWindow = new Geo.Winform.Controls.NamedIntControl();
            this.button_viewObs = new System.Windows.Forms.Button();
            this.button_calculate = new System.Windows.Forms.Button();
            this.splitContainer_main = new System.Windows.Forms.SplitContainer();
            this.splitContainer_top = new System.Windows.Forms.SplitContainer();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.button_multiSolve = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_obsInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_sat)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.objectTableControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.object_56fe9e41_2cbf_4ac3_8650_fc556669d280)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).BeginInit();
            this.splitContainer_main.Panel1.SuspendLayout();
            this.splitContainer_main.Panel2.SuspendLayout();
            this.splitContainer_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_top)).BeginInit();
            this.splitContainer_top.Panel1.SuspendLayout();
            this.splitContainer_top.Panel2.SuspendLayout();
            this.splitContainer_top.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_view
            // 
            this.button_view.Location = new System.Drawing.Point(334, 46);
            this.button_view.Name = "button_view";
            this.button_view.Size = new System.Drawing.Size(75, 25);
            this.button_view.TabIndex = 9;
            this.button_view.Text = "查看数据";
            this.button_view.UseVisualStyleBackColor = true;
            this.button_view.Click += new System.EventHandler(this.button_view_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(62, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "卫星：";
            // 
            // comboBox_sat
            // 
            this.comboBox_sat.DataSource = this.bindingSource_sat;
            this.comboBox_sat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_sat.FormattingEnabled = true;
            this.comboBox_sat.Location = new System.Drawing.Point(108, 45);
            this.comboBox_sat.Name = "comboBox_sat";
            this.comboBox_sat.Size = new System.Drawing.Size(201, 20);
            this.comboBox_sat.TabIndex = 13;
            // 
            // textBox_show
            // 
            this.textBox_show.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_show.Location = new System.Drawing.Point(0, 20);
            this.textBox_show.Multiline = true;
            this.textBox_show.Name = "textBox_show";
            this.textBox_show.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_show.Size = new System.Drawing.Size(166, 160);
            this.textBox_show.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "信息";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_read
            // 
            this.button_read.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_read.Location = new System.Drawing.Point(707, 16);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(61, 22);
            this.button_read.TabIndex = 15;
            this.button_read.Text = "读入";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_read_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.fileOpenControl1);
            this.groupBox1.Controls.Add(this.button_read);
            this.groupBox1.Controls.Add(this.button_view);
            this.groupBox1.Controls.Add(this.comboBox_sat);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(774, 82);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "读取查看选项";
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl1.AllowDrop = true;
            this.fileOpenControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl1.FilePath = "";
            this.fileOpenControl1.FilePathes = new string[0];
            this.fileOpenControl1.Filter = "Rinex观测文件(*.*O;*.rnx)|;*.rnx;*.*O|表格文件o.txt.xls|*.??o.txt.xls|RINEX压缩文件(*.crx;*.c" +
    "rx.gz;*.*D;*.*D.Z)|*.crx;*.crx.gz;*.*D.Z;*.*D|所有文件|*.*";
            this.fileOpenControl1.FirstPath = "";
            this.fileOpenControl1.IsMultiSelect = false;
            this.fileOpenControl1.LabelName = "输入文件：";
            this.fileOpenControl1.Location = new System.Drawing.Point(8, 16);
            this.fileOpenControl1.Margin = new System.Windows.Forms.Padding(4);
            this.fileOpenControl1.Name = "fileOpenControl1";
            this.fileOpenControl1.Size = new System.Drawing.Size(677, 23);
            this.fileOpenControl1.TabIndex = 27;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(799, 365);
            this.splitContainer1.SplitterDistance = 166;
            this.splitContainer1.TabIndex = 19;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.textBox_show);
            this.splitContainer2.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.attributeBox1);
            this.splitContainer2.Size = new System.Drawing.Size(166, 365);
            this.splitContainer2.SplitterDistance = 180;
            this.splitContainer2.TabIndex = 15;
            // 
            // attributeBox1
            // 
            this.attributeBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.attributeBox1.Location = new System.Drawing.Point(0, 0);
            this.attributeBox1.Margin = new System.Windows.Forms.Padding(4);
            this.attributeBox1.Name = "attributeBox1";
            this.attributeBox1.Size = new System.Drawing.Size(166, 181);
            this.attributeBox1.TabIndex = 0;
            this.attributeBox1.Tilte = "属性";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(629, 365);
            this.tabControl1.TabIndex = 17;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.objectTableControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(621, 339);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "数据内容";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // objectTableControl1
            // 
            this.objectTableControl1.Controls.Add(this.object_56fe9e41_2cbf_4ac3_8650_fc556669d280);
            this.objectTableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl1.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl1.Name = "objectTableControl1";
            this.objectTableControl1.Size = new System.Drawing.Size(615, 333);
            this.objectTableControl1.TabIndex = 1;
            this.objectTableControl1.TableObjectStorage = null;
            // 
            // object_56fe9e41_2cbf_4ac3_8650_fc556669d280
            // 
            this.object_56fe9e41_2cbf_4ac3_8650_fc556669d280.AddNewItem = null;
            this.object_56fe9e41_2cbf_4ac3_8650_fc556669d280.CountItem = null;
            this.object_56fe9e41_2cbf_4ac3_8650_fc556669d280.DeleteItem = null;
            this.object_56fe9e41_2cbf_4ac3_8650_fc556669d280.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.object_56fe9e41_2cbf_4ac3_8650_fc556669d280.Location = new System.Drawing.Point(0, 308);
            this.object_56fe9e41_2cbf_4ac3_8650_fc556669d280.MoveFirstItem = null;
            this.object_56fe9e41_2cbf_4ac3_8650_fc556669d280.MoveLastItem = null;
            this.object_56fe9e41_2cbf_4ac3_8650_fc556669d280.MoveNextItem = null;
            this.object_56fe9e41_2cbf_4ac3_8650_fc556669d280.MovePreviousItem = null;
            this.object_56fe9e41_2cbf_4ac3_8650_fc556669d280.Name = "object_56fe9e41_2cbf_4ac3_8650_fc556669d280";
            this.object_56fe9e41_2cbf_4ac3_8650_fc556669d280.PositionItem = null;
            this.object_56fe9e41_2cbf_4ac3_8650_fc556669d280.Size = new System.Drawing.Size(615, 25);
            this.object_56fe9e41_2cbf_4ac3_8650_fc556669d280.TabIndex = 1;
            this.object_56fe9e41_2cbf_4ac3_8650_fc556669d280.Text = "bindingNavigator1";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.richTextBoxControl_result);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(621, 339);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "结果显示";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_result
            // 
            this.richTextBoxControl_result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_result.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_result.MaxAppendLineCount = 5000;
            this.richTextBoxControl_result.Name = "richTextBoxControl_result";
            this.richTextBoxControl_result.Size = new System.Drawing.Size(615, 333);
            this.richTextBoxControl_result.TabIndex = 0;
            this.richTextBoxControl_result.Text = "";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(799, 159);
            this.tabControl2.TabIndex = 22;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(791, 133);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "观测文件";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.radioButton_isUserRawValue);
            this.tabPage5.Controls.Add(this.radioButton_isUserPolyFitVal);
            this.tabPage5.Controls.Add(this.radioButton_isUsePhaaseSmoothP);
            this.tabPage5.Controls.Add(this.namedFloatControl_satCutoff);
            this.tabPage5.Controls.Add(this.namedIntControl_smoothWindow);
            this.tabPage5.Controls.Add(this.button_viewObs);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(791, 133);
            this.tabPage5.TabIndex = 3;
            this.tabPage5.Text = "计算选项";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // radioButton_isUserRawValue
            // 
            this.radioButton_isUserRawValue.AutoSize = true;
            this.radioButton_isUserRawValue.Checked = true;
            this.radioButton_isUserRawValue.Location = new System.Drawing.Point(27, 93);
            this.radioButton_isUserRawValue.Name = "radioButton_isUserRawValue";
            this.radioButton_isUserRawValue.Size = new System.Drawing.Size(155, 16);
            this.radioButton_isUserRawValue.TabIndex = 27;
            this.radioButton_isUserRawValue.TabStop = true;
            this.radioButton_isUserRawValue.Text = "批量计算采用原始观测值";
            this.radioButton_isUserRawValue.UseVisualStyleBackColor = true;
            // 
            // radioButton_isUserPolyFitVal
            // 
            this.radioButton_isUserPolyFitVal.AutoSize = true;
            this.radioButton_isUserPolyFitVal.Location = new System.Drawing.Point(27, 71);
            this.radioButton_isUserPolyFitVal.Name = "radioButton_isUserPolyFitVal";
            this.radioButton_isUserPolyFitVal.Size = new System.Drawing.Size(155, 16);
            this.radioButton_isUserPolyFitVal.TabIndex = 27;
            this.radioButton_isUserPolyFitVal.TabStop = true;
            this.radioButton_isUserPolyFitVal.Text = "批量计算采用多项式平滑";
            this.radioButton_isUserPolyFitVal.UseVisualStyleBackColor = true;
            // 
            // radioButton_isUsePhaaseSmoothP
            // 
            this.radioButton_isUsePhaaseSmoothP.AutoSize = true;
            this.radioButton_isUsePhaaseSmoothP.Location = new System.Drawing.Point(27, 49);
            this.radioButton_isUsePhaaseSmoothP.Name = "radioButton_isUsePhaaseSmoothP";
            this.radioButton_isUsePhaaseSmoothP.Size = new System.Drawing.Size(179, 16);
            this.radioButton_isUsePhaaseSmoothP.TabIndex = 27;
            this.radioButton_isUsePhaaseSmoothP.Text = "批量计算采用载波相位平滑值";
            this.radioButton_isUsePhaaseSmoothP.UseVisualStyleBackColor = true;
            // 
            // namedFloatControl_satCutoff
            // 
            this.namedFloatControl_satCutoff.Location = new System.Drawing.Point(6, 6);
            this.namedFloatControl_satCutoff.Name = "namedFloatControl_satCutoff";
            this.namedFloatControl_satCutoff.Size = new System.Drawing.Size(184, 23);
            this.namedFloatControl_satCutoff.TabIndex = 24;
            this.namedFloatControl_satCutoff.Title = "卫星高度截止角(°)：";
            this.namedFloatControl_satCutoff.Value = 15D;
            // 
            // namedIntControl_smoothWindow
            // 
            this.namedIntControl_smoothWindow.Location = new System.Drawing.Point(286, 22);
            this.namedIntControl_smoothWindow.Name = "namedIntControl_smoothWindow";
            this.namedIntControl_smoothWindow.Size = new System.Drawing.Size(131, 23);
            this.namedIntControl_smoothWindow.TabIndex = 23;
            this.namedIntControl_smoothWindow.Title = "平滑窗口：";
            this.namedIntControl_smoothWindow.Value = 10;
            // 
            // button_viewObs
            // 
            this.button_viewObs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_viewObs.Location = new System.Drawing.Point(713, 22);
            this.button_viewObs.Name = "button_viewObs";
            this.button_viewObs.Size = new System.Drawing.Size(75, 23);
            this.button_viewObs.TabIndex = 1;
            this.button_viewObs.Text = "平滑伪距";
            this.button_viewObs.UseVisualStyleBackColor = true;
            this.button_viewObs.Click += new System.EventHandler(this.button_viewObs_Click);
            // 
            // button_calculate
            // 
            this.button_calculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_calculate.Location = new System.Drawing.Point(605, 15);
            this.button_calculate.Name = "button_calculate";
            this.button_calculate.Size = new System.Drawing.Size(75, 34);
            this.button_calculate.TabIndex = 9;
            this.button_calculate.Text = "计算";
            this.button_calculate.UseVisualStyleBackColor = true;
            this.button_calculate.Click += new System.EventHandler(this.button_calculate_Click);
            // 
            // splitContainer_main
            // 
            this.splitContainer_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_main.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer_main.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_main.Name = "splitContainer_main";
            this.splitContainer_main.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_main.Panel1
            // 
            this.splitContainer_main.Panel1.Controls.Add(this.splitContainer_top);
            // 
            // splitContainer_main.Panel2
            // 
            this.splitContainer_main.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer_main.Size = new System.Drawing.Size(799, 593);
            this.splitContainer_main.SplitterDistance = 224;
            this.splitContainer_main.TabIndex = 23;
            // 
            // splitContainer_top
            // 
            this.splitContainer_top.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_top.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_top.Name = "splitContainer_top";
            this.splitContainer_top.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_top.Panel1
            // 
            this.splitContainer_top.Panel1.Controls.Add(this.tabControl2);
            // 
            // splitContainer_top.Panel2
            // 
            this.splitContainer_top.Panel2.Controls.Add(this.progressBarComponent1);
            this.splitContainer_top.Panel2.Controls.Add(this.button_multiSolve);
            this.splitContainer_top.Panel2.Controls.Add(this.button_calculate);
            this.splitContainer_top.Size = new System.Drawing.Size(799, 224);
            this.splitContainer_top.SplitterDistance = 159;
            this.splitContainer_top.TabIndex = 23;
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(46, 15);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(527, 34);
            this.progressBarComponent1.TabIndex = 10;
            // 
            // button_multiSolve
            // 
            this.button_multiSolve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_multiSolve.Location = new System.Drawing.Point(699, 15);
            this.button_multiSolve.Name = "button_multiSolve";
            this.button_multiSolve.Size = new System.Drawing.Size(75, 34);
            this.button_multiSolve.TabIndex = 9;
            this.button_multiSolve.Text = "批量计算";
            this.button_multiSolve.UseVisualStyleBackColor = true;
            this.button_multiSolve.Click += new System.EventHandler(this.button_multiSolve_Click);
            // 
            // IonoDcbSolveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 593);
            this.Controls.Add(this.splitContainer_main);
            this.Name = "IonoDcbSolveForm";
            this.Text = "DCB计算";
            this.Load += new System.EventHandler(this.ObsFileViewerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_obsInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_sat)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.objectTableControl1.ResumeLayout(false);
            this.objectTableControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.object_56fe9e41_2cbf_4ac3_8650_fc556669d280)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.splitContainer_main.Panel1.ResumeLayout(false);
            this.splitContainer_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).EndInit();
            this.splitContainer_main.ResumeLayout(false);
            this.splitContainer_top.Panel1.ResumeLayout(false);
            this.splitContainer_top.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_top)).EndInit();
            this.splitContainer_top.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource bindingSource_obsInfo;
        private System.Windows.Forms.Button button_view;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox_sat;
        private System.Windows.Forms.BindingSource bindingSource_sat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_show;
        private System.Windows.Forms.Button button_read;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Geo.Winform.AttributeBox attributeBox1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button button_viewObs;
        private System.Windows.Forms.TabPage tabPage3;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl_result;
        private System.Windows.Forms.Button button_calculate;
        private System.Windows.Forms.SplitContainer splitContainer_main;
        private Geo.Winform.ObjectTableControl objectTableControl1;
        private System.Windows.Forms.BindingNavigator object_56fe9e41_2cbf_4ac3_8650_fc556669d280;
        protected Geo.Winform.Controls.FileOpenControl fileOpenControl1;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_smoothWindow;
        private System.Windows.Forms.Button button_multiSolve;
        private System.Windows.Forms.SplitContainer splitContainer_top;
        private Geo.Winform.Controls.ProgressBarComponent progressBarComponent1;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_satCutoff;
        private System.Windows.Forms.RadioButton radioButton_isUserRawValue;
        private System.Windows.Forms.RadioButton radioButton_isUserPolyFitVal;
        private System.Windows.Forms.RadioButton radioButton_isUsePhaaseSmoothP;
    }
}

