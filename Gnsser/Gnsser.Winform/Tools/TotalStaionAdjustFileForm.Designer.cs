namespace Gnsser.Winform
{
    partial class TotalStaionAdjustFileForm
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
            this.button_read = new System.Windows.Forms.Button();
            this.button_showOnMap = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.namedFloatControl_aveGeoHeight = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_orinalLonDeg = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControlYConst = new Geo.Winform.Controls.NamedFloatControl();
            this.fileOpenControl_file = new Geo.Winform.Controls.FileOpenControl();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.button_solveAzimuth = new System.Windows.Forms.Button();
            this.comboBox_targetPtB = new System.Windows.Forms.ComboBox();
            this.bindingSource_targetPtB = new System.Windows.Forms.BindingSource(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_targetPt = new System.Windows.Forms.ComboBox();
            this.bindingSource_targetPtA = new System.Windows.Forms.BindingSource(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_startPt = new System.Windows.Forms.ComboBox();
            this.bindingSource_startPt = new System.Windows.Forms.BindingSource(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.objectTableControl_approxCoord = new Geo.Winform.ObjectTableControl();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.objectTableControl_directionResult = new Geo.Winform.ObjectTableControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.objectTableControl_distanceResult = new Geo.Winform.ObjectTableControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.objectTableControl_adjustCoord = new Geo.Winform.ObjectTableControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.objectTableControl_combined = new Geo.Winform.ObjectTableControl();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_result = new Geo.Winform.Controls.RichTextBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_targetPtB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_targetPtA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_startPt)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage8.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.button_read);
            this.splitContainer1.Panel1.Controls.Add(this.button_showOnMap);
            this.splitContainer1.Panel1.Controls.Add(this.tabControl2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(756, 509);
            this.splitContainer1.SplitterDistance = 159;
            this.splitContainer1.TabIndex = 1;
            // 
            // button_read
            // 
            this.button_read.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_read.Location = new System.Drawing.Point(658, 129);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(75, 23);
            this.button_read.TabIndex = 4;
            this.button_read.Text = "读取";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_read_Click);
            // 
            // button_showOnMap
            // 
            this.button_showOnMap.Location = new System.Drawing.Point(7, 129);
            this.button_showOnMap.Name = "button_showOnMap";
            this.button_showOnMap.Size = new System.Drawing.Size(75, 23);
            this.button_showOnMap.TabIndex = 3;
            this.button_showOnMap.Text = "地图查看";
            this.button_showOnMap.UseVisualStyleBackColor = true;
            this.button_showOnMap.Click += new System.EventHandler(this.button_showOnMap_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage7);
            this.tabControl2.Location = new System.Drawing.Point(3, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(746, 120);
            this.tabControl2.TabIndex = 2;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.namedFloatControl_aveGeoHeight);
            this.tabPage3.Controls.Add(this.namedFloatControl_orinalLonDeg);
            this.tabPage3.Controls.Add(this.namedFloatControlYConst);
            this.tabPage3.Controls.Add(this.fileOpenControl_file);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(738, 94);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "输入";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // namedFloatControl_aveGeoHeight
            // 
            this.namedFloatControl_aveGeoHeight.Location = new System.Drawing.Point(234, 36);
            this.namedFloatControl_aveGeoHeight.Name = "namedFloatControl_aveGeoHeight";
            this.namedFloatControl_aveGeoHeight.Size = new System.Drawing.Size(171, 23);
            this.namedFloatControl_aveGeoHeight.TabIndex = 7;
            this.namedFloatControl_aveGeoHeight.Title = "投影面大地高(m)：";
            this.namedFloatControl_aveGeoHeight.Value = 1500D;
            // 
            // namedFloatControl_orinalLonDeg
            // 
            this.namedFloatControl_orinalLonDeg.Location = new System.Drawing.Point(4, 61);
            this.namedFloatControl_orinalLonDeg.Name = "namedFloatControl_orinalLonDeg";
            this.namedFloatControl_orinalLonDeg.Size = new System.Drawing.Size(214, 23);
            this.namedFloatControl_orinalLonDeg.TabIndex = 5;
            this.namedFloatControl_orinalLonDeg.Title = "中央子午线(度小数)：";
            this.namedFloatControl_orinalLonDeg.Value = 99.5D;
            // 
            // namedFloatControlYConst
            // 
            this.namedFloatControlYConst.Location = new System.Drawing.Point(47, 36);
            this.namedFloatControlYConst.Name = "namedFloatControlYConst";
            this.namedFloatControlYConst.Size = new System.Drawing.Size(171, 23);
            this.namedFloatControlYConst.TabIndex = 6;
            this.namedFloatControlYConst.Title = "横轴Y加常数：";
            this.namedFloatControlYConst.Value = 500000D;
            // 
            // fileOpenControl_file
            // 
            this.fileOpenControl_file.AllowDrop = true;
            this.fileOpenControl_file.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_file.FilePath = "";
            this.fileOpenControl_file.FilePathes = new string[0];
            this.fileOpenControl_file.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_file.FirstPath = "";
            this.fileOpenControl_file.IsMultiSelect = false;
            this.fileOpenControl_file.LabelName = "全站仪结果文件：";
            this.fileOpenControl_file.Location = new System.Drawing.Point(3, 3);
            this.fileOpenControl_file.Name = "fileOpenControl_file";
            this.fileOpenControl_file.Size = new System.Drawing.Size(732, 27);
            this.fileOpenControl_file.TabIndex = 0;
            this.fileOpenControl_file.FilePathSetted += new System.EventHandler(this.fileOpenControl_baseline_FilePathSetted);
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.button_solveAzimuth);
            this.tabPage7.Controls.Add(this.comboBox_targetPtB);
            this.tabPage7.Controls.Add(this.label3);
            this.tabPage7.Controls.Add(this.comboBox_targetPt);
            this.tabPage7.Controls.Add(this.label2);
            this.tabPage7.Controls.Add(this.label1);
            this.tabPage7.Controls.Add(this.comboBox_startPt);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(738, 94);
            this.tabPage7.TabIndex = 1;
            this.tabPage7.Text = "求方位角";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // button_solveAzimuth
            // 
            this.button_solveAzimuth.Location = new System.Drawing.Point(448, 14);
            this.button_solveAzimuth.Name = "button_solveAzimuth";
            this.button_solveAzimuth.Size = new System.Drawing.Size(75, 23);
            this.button_solveAzimuth.TabIndex = 6;
            this.button_solveAzimuth.Text = "求方位角";
            this.button_solveAzimuth.UseVisualStyleBackColor = true;
            this.button_solveAzimuth.Click += new System.EventHandler(this.button_solveAzimuth_Click);
            // 
            // comboBox_targetPtB
            // 
            this.comboBox_targetPtB.DataSource = this.bindingSource_targetPtB;
            this.comboBox_targetPtB.FormattingEnabled = true;
            this.comboBox_targetPtB.Location = new System.Drawing.Point(288, 34);
            this.comboBox_targetPtB.Name = "comboBox_targetPtB";
            this.comboBox_targetPtB.Size = new System.Drawing.Size(121, 20);
            this.comboBox_targetPtB.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(222, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "目标点2：";
            // 
            // comboBox_targetPt
            // 
            this.comboBox_targetPt.DataSource = this.bindingSource_targetPtA;
            this.comboBox_targetPt.FormattingEnabled = true;
            this.comboBox_targetPt.Location = new System.Drawing.Point(285, 6);
            this.comboBox_targetPt.Name = "comboBox_targetPt";
            this.comboBox_targetPt.Size = new System.Drawing.Size(121, 20);
            this.comboBox_targetPt.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(220, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "目标点A：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "起始点：";
            // 
            // comboBox_startPt
            // 
            this.comboBox_startPt.DataSource = this.bindingSource_startPt;
            this.comboBox_startPt.FormattingEnabled = true;
            this.comboBox_startPt.Location = new System.Drawing.Point(59, 6);
            this.comboBox_startPt.Name = "comboBox_startPt";
            this.comboBox_startPt.Size = new System.Drawing.Size(121, 20);
            this.comboBox_startPt.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage8);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(756, 346);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.objectTableControl_approxCoord);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(748, 320);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "近似坐标";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_approxCoord
            // 
            this.objectTableControl_approxCoord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_approxCoord.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_approxCoord.Name = "objectTableControl_approxCoord";
            this.objectTableControl_approxCoord.Size = new System.Drawing.Size(742, 314);
            this.objectTableControl_approxCoord.TabIndex = 1;
            this.objectTableControl_approxCoord.TableObjectStorage = null;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.objectTableControl_directionResult);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(748, 320);
            this.tabPage6.TabIndex = 3;
            this.tabPage6.Text = "方向平差结果";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_directionResult
            // 
            this.objectTableControl_directionResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_directionResult.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_directionResult.Name = "objectTableControl_directionResult";
            this.objectTableControl_directionResult.Size = new System.Drawing.Size(742, 314);
            this.objectTableControl_directionResult.TabIndex = 2;
            this.objectTableControl_directionResult.TableObjectStorage = null;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.objectTableControl_distanceResult);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(748, 320);
            this.tabPage1.TabIndex = 4;
            this.tabPage1.Text = "距离平差结果";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_distanceResult
            // 
            this.objectTableControl_distanceResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_distanceResult.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_distanceResult.Name = "objectTableControl_distanceResult";
            this.objectTableControl_distanceResult.Size = new System.Drawing.Size(742, 314);
            this.objectTableControl_distanceResult.TabIndex = 2;
            this.objectTableControl_distanceResult.TableObjectStorage = null;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.objectTableControl_adjustCoord);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(748, 320);
            this.tabPage2.TabIndex = 5;
            this.tabPage2.Text = "平差坐标及其精度";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_adjustCoord
            // 
            this.objectTableControl_adjustCoord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_adjustCoord.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_adjustCoord.Name = "objectTableControl_adjustCoord";
            this.objectTableControl_adjustCoord.Size = new System.Drawing.Size(742, 314);
            this.objectTableControl_adjustCoord.TabIndex = 2;
            this.objectTableControl_adjustCoord.TableObjectStorage = null;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.objectTableControl_combined);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(748, 320);
            this.tabPage4.TabIndex = 6;
            this.tabPage4.Text = "网点间边长、方位角及其相对精度";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_combined
            // 
            this.objectTableControl_combined.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_combined.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_combined.Name = "objectTableControl_combined";
            this.objectTableControl_combined.Size = new System.Drawing.Size(742, 314);
            this.objectTableControl_combined.TabIndex = 2;
            this.objectTableControl_combined.TableObjectStorage = null;
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.richTextBoxControl_result);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(748, 320);
            this.tabPage8.TabIndex = 7;
            this.tabPage8.Text = "方位角文本";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_result
            // 
            this.richTextBoxControl_result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_result.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_result.MaxAppendLineCount = 5000;
            this.richTextBoxControl_result.Name = "richTextBoxControl_result";
            this.richTextBoxControl_result.Size = new System.Drawing.Size(742, 314);
            this.richTextBoxControl_result.TabIndex = 0;
            this.richTextBoxControl_result.Text = "";
            // 
            // TotalStaionAdjustFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 509);
            this.Controls.Add(this.splitContainer1);
            this.Name = "TotalStaionAdjustFileForm";
            this.Text = "查看全站仪平差文件ou2";
            this.Load += new System.EventHandler(this.BaselineNetClosureErrorForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_targetPtB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_targetPtA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_startPt)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Geo.Winform.Controls.FileOpenControl fileOpenControl_file;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage5;
        private Geo.Winform.ObjectTableControl objectTableControl_approxCoord;
        private System.Windows.Forms.Button button_showOnMap;
        private System.Windows.Forms.Button button_read;
        private System.Windows.Forms.TabPage tabPage6;
        private Geo.Winform.ObjectTableControl objectTableControl_directionResult;
        private System.Windows.Forms.TabPage tabPage1;
        private Geo.Winform.ObjectTableControl objectTableControl_distanceResult;
        private System.Windows.Forms.TabPage tabPage2;
        private Geo.Winform.ObjectTableControl objectTableControl_adjustCoord;
        private System.Windows.Forms.TabPage tabPage4;
        private Geo.Winform.ObjectTableControl objectTableControl_combined;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.Button button_solveAzimuth;
        private System.Windows.Forms.ComboBox comboBox_targetPtB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_targetPt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_startPt;
        private System.Windows.Forms.TabPage tabPage8;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl_result;
        private System.Windows.Forms.BindingSource bindingSource_startPt;
        private System.Windows.Forms.BindingSource bindingSource_targetPtA;
        private System.Windows.Forms.BindingSource bindingSource_targetPtB;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_orinalLonDeg;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControlYConst;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_aveGeoHeight;
    }
}