namespace Geo.Winform
{
    partial class PlanXyLineViewerForm
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
            this.button_run = new System.Windows.Forms.Button();
            this.fileOpenControl1 = new Geo.Winform.Controls.FileOpenControl();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.objectTableControl_site = new Geo.Winform.ObjectTableControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.objectTableControl_vector = new Geo.Winform.ObjectTableControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_result = new Geo.Winform.Controls.RichTextBoxControl();
            this.button_showOnMap = new System.Windows.Forms.Button();
            this.namedFloatControl_aveGeoHeight = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_orinalLonDeg = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControlYConst = new Geo.Winform.Controls.NamedFloatControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.button_solveAzimuth = new System.Windows.Forms.Button();
            this.comboBox_targetPtB = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_targetPt = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_startPt = new System.Windows.Forms.ComboBox();
            this.bindingSource_targetPtB = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource_targetPtA = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource_startPt = new System.Windows.Forms.BindingSource(this.components);
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.objectTableControl_point = new Geo.Winform.ObjectTableControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_targetPtB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_targetPtA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_startPt)).BeginInit();
            this.tabPage6.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.button_run);
            this.splitContainer1.Panel1.Controls.Add(this.button_showOnMap);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl2);
            this.splitContainer1.Size = new System.Drawing.Size(661, 389);
            this.splitContainer1.SplitterDistance = 130;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(661, 104);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.namedFloatControl_aveGeoHeight);
            this.tabPage1.Controls.Add(this.namedFloatControl_orinalLonDeg);
            this.tabPage1.Controls.Add(this.namedFloatControlYConst);
            this.tabPage1.Controls.Add(this.fileOpenControl1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(653, 78);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "输入";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(579, 104);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 23);
            this.button_run.TabIndex = 1;
            this.button_run.Text = "运行";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl1.AllowDrop = true;
            this.fileOpenControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl1.FilePath = "";
            this.fileOpenControl1.FilePathes = new string[0];
            this.fileOpenControl1.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl1.FirstPath = "";
            this.fileOpenControl1.IsMultiSelect = false;
            this.fileOpenControl1.LabelName = "文件：";
            this.fileOpenControl1.Location = new System.Drawing.Point(8, 6);
            this.fileOpenControl1.Name = "fileOpenControl1";
            this.fileOpenControl1.Size = new System.Drawing.Size(624, 22);
            this.fileOpenControl1.TabIndex = 0;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage6);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(661, 255);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.objectTableControl_site);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(653, 229);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "原始数据";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_site
            // 
            this.objectTableControl_site.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_site.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_site.Name = "objectTableControl_site";
            this.objectTableControl_site.Size = new System.Drawing.Size(647, 223);
            this.objectTableControl_site.TabIndex = 0;
            this.objectTableControl_site.TableObjectStorage = null;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.objectTableControl_vector);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(653, 229);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "向量表格";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_vector
            // 
            this.objectTableControl_vector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_vector.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_vector.Name = "objectTableControl_vector";
            this.objectTableControl_vector.Size = new System.Drawing.Size(647, 223);
            this.objectTableControl_vector.TabIndex = 1;
            this.objectTableControl_vector.TableObjectStorage = null;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.richTextBoxControl_result);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(653, 229);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "文本";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_result
            // 
            this.richTextBoxControl_result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_result.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_result.MaxAppendLineCount = 5000;
            this.richTextBoxControl_result.Name = "richTextBoxControl_result";
            this.richTextBoxControl_result.Size = new System.Drawing.Size(647, 223);
            this.richTextBoxControl_result.TabIndex = 1;
            this.richTextBoxControl_result.Text = "";
            // 
            // button_showOnMap
            // 
            this.button_showOnMap.Location = new System.Drawing.Point(7, 106);
            this.button_showOnMap.Name = "button_showOnMap";
            this.button_showOnMap.Size = new System.Drawing.Size(75, 23);
            this.button_showOnMap.TabIndex = 4;
            this.button_showOnMap.Text = "地图查看";
            this.button_showOnMap.UseVisualStyleBackColor = true;
            this.button_showOnMap.Click += new System.EventHandler(this.button_showOnMap_Click);
            // 
            // namedFloatControl_aveGeoHeight
            // 
            this.namedFloatControl_aveGeoHeight.Location = new System.Drawing.Point(236, 29);
            this.namedFloatControl_aveGeoHeight.Name = "namedFloatControl_aveGeoHeight";
            this.namedFloatControl_aveGeoHeight.Size = new System.Drawing.Size(171, 23);
            this.namedFloatControl_aveGeoHeight.TabIndex = 10;
            this.namedFloatControl_aveGeoHeight.Title = "投影面大地高(m)：";
            this.namedFloatControl_aveGeoHeight.Value = 1500D;
            // 
            // namedFloatControl_orinalLonDeg
            // 
            this.namedFloatControl_orinalLonDeg.Location = new System.Drawing.Point(6, 54);
            this.namedFloatControl_orinalLonDeg.Name = "namedFloatControl_orinalLonDeg";
            this.namedFloatControl_orinalLonDeg.Size = new System.Drawing.Size(214, 23);
            this.namedFloatControl_orinalLonDeg.TabIndex = 8;
            this.namedFloatControl_orinalLonDeg.Title = "中央子午线(度小数)：";
            this.namedFloatControl_orinalLonDeg.Value = 99.5D;
            // 
            // namedFloatControlYConst
            // 
            this.namedFloatControlYConst.Location = new System.Drawing.Point(49, 29);
            this.namedFloatControlYConst.Name = "namedFloatControlYConst";
            this.namedFloatControlYConst.Size = new System.Drawing.Size(171, 23);
            this.namedFloatControlYConst.TabIndex = 9;
            this.namedFloatControlYConst.Title = "横轴Y加常数：";
            this.namedFloatControlYConst.Value = 500000D;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.button_solveAzimuth);
            this.tabPage5.Controls.Add(this.comboBox_targetPtB);
            this.tabPage5.Controls.Add(this.label3);
            this.tabPage5.Controls.Add(this.comboBox_targetPt);
            this.tabPage5.Controls.Add(this.label2);
            this.tabPage5.Controls.Add(this.label1);
            this.tabPage5.Controls.Add(this.comboBox_startPt);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(653, 78);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "方位角";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // button_solveAzimuth
            // 
            this.button_solveAzimuth.Location = new System.Drawing.Point(450, 14);
            this.button_solveAzimuth.Name = "button_solveAzimuth";
            this.button_solveAzimuth.Size = new System.Drawing.Size(75, 23);
            this.button_solveAzimuth.TabIndex = 13;
            this.button_solveAzimuth.Text = "求方位角";
            this.button_solveAzimuth.UseVisualStyleBackColor = true;
            this.button_solveAzimuth.Click += new System.EventHandler(this.button_solveAzimuth_Click);
            // 
            // comboBox_targetPtB
            // 
            this.comboBox_targetPtB.DataSource = this.bindingSource_targetPtB;
            this.comboBox_targetPtB.FormattingEnabled = true;
            this.comboBox_targetPtB.Location = new System.Drawing.Point(290, 34);
            this.comboBox_targetPtB.Name = "comboBox_targetPtB";
            this.comboBox_targetPtB.Size = new System.Drawing.Size(121, 20);
            this.comboBox_targetPtB.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(224, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "目标点2：";
            // 
            // comboBox_targetPt
            // 
            this.comboBox_targetPt.DataSource = this.bindingSource_targetPtA;
            this.comboBox_targetPt.FormattingEnabled = true;
            this.comboBox_targetPt.Location = new System.Drawing.Point(287, 6);
            this.comboBox_targetPt.Name = "comboBox_targetPt";
            this.comboBox_targetPt.Size = new System.Drawing.Size(121, 20);
            this.comboBox_targetPt.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(222, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "目标点A：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "起始点：";
            // 
            // comboBox_startPt
            // 
            this.comboBox_startPt.DataSource = this.bindingSource_startPt;
            this.comboBox_startPt.FormattingEnabled = true;
            this.comboBox_startPt.Location = new System.Drawing.Point(61, 6);
            this.comboBox_startPt.Name = "comboBox_startPt";
            this.comboBox_startPt.Size = new System.Drawing.Size(121, 20);
            this.comboBox_startPt.TabIndex = 7;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.objectTableControl_point);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(653, 229);
            this.tabPage6.TabIndex = 3;
            this.tabPage6.Text = "点位信息";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_point
            // 
            this.objectTableControl_point.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_point.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_point.Name = "objectTableControl_point";
            this.objectTableControl_point.Size = new System.Drawing.Size(647, 223);
            this.objectTableControl_point.TabIndex = 2;
            this.objectTableControl_point.TableObjectStorage = null;
            // 
            // PlanXyLineViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 389);
            this.Controls.Add(this.splitContainer1);
            this.Name = "PlanXyLineViewerForm";
            this.Text = "PlanXyViewerForm";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_targetPtB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_targetPtA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_startPt)).EndInit();
            this.tabPage6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private Controls.FileOpenControl fileOpenControl1;
        private System.Windows.Forms.Button button_run;
        private ObjectTableControl objectTableControl_site;
        private ObjectTableControl objectTableControl_vector;
        private System.Windows.Forms.TabPage tabPage2;
        private Controls.RichTextBoxControl richTextBoxControl_result;
        private System.Windows.Forms.Button button_showOnMap;
        private Controls.NamedFloatControl namedFloatControl_aveGeoHeight;
        private Controls.NamedFloatControl namedFloatControl_orinalLonDeg;
        private Controls.NamedFloatControl namedFloatControlYConst;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button button_solveAzimuth;
        private System.Windows.Forms.ComboBox comboBox_targetPtB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_targetPt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_startPt;
        private System.Windows.Forms.BindingSource bindingSource_targetPtB;
        private System.Windows.Forms.BindingSource bindingSource_targetPtA;
        private System.Windows.Forms.BindingSource bindingSource_startPt;
        private System.Windows.Forms.TabPage tabPage6;
        private ObjectTableControl objectTableControl_point;
    }
}