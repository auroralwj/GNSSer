namespace Gnsser.Winform
{
    partial class BaselineNetAdjustForm
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
            this.button_showIndependentLine = new System.Windows.Forms.Button();
            this.button_showOnMap = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.label1 = new System.Windows.Forms.Label();
            this.fileOpenControl_knownCoord = new Geo.Winform.Controls.FileOpenControl();
            this.fileOpenControl_baseline = new Geo.Winform.Controls.FileOpenControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.checkBox_isAllLinIndependentOre = new System.Windows.Forms.CheckBox();
            this.panel_fixedSites = new System.Windows.Forms.Panel();
            this.arrayCheckBoxControl_fixedSites = new Geo.Winform.ArrayCheckBoxControl();
            this.button_loadSItes = new System.Windows.Forms.Button();
            this.enumRadioControl_adjustType = new Geo.Winform.EnumRadioControl();
            this.enumRadioControl_selectLineType = new Geo.Winform.EnumRadioControl();
            this.label2 = new System.Windows.Forms.Label();
            this.namedFloatControl_periodSpanMinutes = new Geo.Winform.Controls.NamedFloatControl();
            this.button_read = new System.Windows.Forms.Button();
            this.button_run = new System.Windows.Forms.Button();
            this.tabControl_result = new System.Windows.Forms.TabControl();
            this.tabPage_rawData = new System.Windows.Forms.TabPage();
            this.objectTableControl_rawData = new Geo.Winform.ObjectTableControl();
            this.tabPage_netAdjustResult = new System.Windows.Forms.TabPage();
            this.objectTableControl_netAdjustResult = new Geo.Winform.ObjectTableControl();
            this.tabPage_independentLines = new System.Windows.Forms.TabPage();
            this.objectTableControl_independentLine = new Geo.Winform.ObjectTableControl();
            this.tabPage_vectorCorrection = new System.Windows.Forms.TabPage();
            this.objectTableControl_vectorCorrection = new Geo.Winform.ObjectTableControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.panel_fixedSites.SuspendLayout();
            this.tabControl_result.SuspendLayout();
            this.tabPage_rawData.SuspendLayout();
            this.tabPage_netAdjustResult.SuspendLayout();
            this.tabPage_independentLines.SuspendLayout();
            this.tabPage_vectorCorrection.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.button_showIndependentLine);
            this.splitContainer1.Panel1.Controls.Add(this.button_showOnMap);
            this.splitContainer1.Panel1.Controls.Add(this.tabControl2);
            this.splitContainer1.Panel1.Controls.Add(this.button_read);
            this.splitContainer1.Panel1.Controls.Add(this.button_run);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl_result);
            this.splitContainer1.Size = new System.Drawing.Size(756, 509);
            this.splitContainer1.SplitterDistance = 241;
            this.splitContainer1.TabIndex = 1;
            // 
            // button_showIndependentLine
            // 
            this.button_showIndependentLine.Location = new System.Drawing.Point(110, 210);
            this.button_showIndependentLine.Name = "button_showIndependentLine";
            this.button_showIndependentLine.Size = new System.Drawing.Size(97, 23);
            this.button_showIndependentLine.TabIndex = 3;
            this.button_showIndependentLine.Text = "地图看独立基线";
            this.button_showIndependentLine.UseVisualStyleBackColor = true;
            this.button_showIndependentLine.Click += new System.EventHandler(this.button_showIndependentLine_Click);
            // 
            // button_showOnMap
            // 
            this.button_showOnMap.Location = new System.Drawing.Point(7, 210);
            this.button_showOnMap.Name = "button_showOnMap";
            this.button_showOnMap.Size = new System.Drawing.Size(97, 23);
            this.button_showOnMap.TabIndex = 3;
            this.button_showOnMap.Text = "地图查看站点";
            this.button_showOnMap.UseVisualStyleBackColor = true;
            this.button_showOnMap.Click += new System.EventHandler(this.button_showOnMap_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Location = new System.Drawing.Point(3, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(746, 192);
            this.tabControl2.TabIndex = 2;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.directorySelectionControl1);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.fileOpenControl_knownCoord);
            this.tabPage3.Controls.Add(this.fileOpenControl_baseline);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(738, 166);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "基线文件";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl1.IsAddOrReplase = false;
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "输出文件夹：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(6, 122);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "D:\\ProgramFiles\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\Temp";
            this.directorySelectionControl1.Pathes = new string[] {
        "D:\\ProgramFiles\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\Temp"};
            this.directorySelectionControl1.Size = new System.Drawing.Size(726, 22);
            this.directorySelectionControl1.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(505, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(227, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "坐标一行一个，必须列名包含X、Y、Z字母";
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
            this.fileOpenControl_knownCoord.LabelName = "已知坐标文件：";
            this.fileOpenControl_knownCoord.Location = new System.Drawing.Point(5, 66);
            this.fileOpenControl_knownCoord.Name = "fileOpenControl_knownCoord";
            this.fileOpenControl_knownCoord.Size = new System.Drawing.Size(732, 22);
            this.fileOpenControl_knownCoord.TabIndex = 1;
            this.fileOpenControl_knownCoord.FilePathSetted += new System.EventHandler(this.fileOpenControl_baseline_FilePathSetted);
            // 
            // fileOpenControl_baseline
            // 
            this.fileOpenControl_baseline.AllowDrop = true;
            this.fileOpenControl_baseline.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_baseline.FilePath = "";
            this.fileOpenControl_baseline.FilePathes = new string[0];
            this.fileOpenControl_baseline.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_baseline.FirstPath = "";
            this.fileOpenControl_baseline.IsMultiSelect = true;
            this.fileOpenControl_baseline.LabelName = "基线解算文件：";
            this.fileOpenControl_baseline.Location = new System.Drawing.Point(3, 3);
            this.fileOpenControl_baseline.Name = "fileOpenControl_baseline";
            this.fileOpenControl_baseline.Size = new System.Drawing.Size(732, 59);
            this.fileOpenControl_baseline.TabIndex = 0;
            this.fileOpenControl_baseline.FilePathSetted += new System.EventHandler(this.fileOpenControl_baseline_FilePathSetted);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.checkBox_isAllLinIndependentOre);
            this.tabPage4.Controls.Add(this.panel_fixedSites);
            this.tabPage4.Controls.Add(this.enumRadioControl_adjustType);
            this.tabPage4.Controls.Add(this.enumRadioControl_selectLineType);
            this.tabPage4.Controls.Add(this.label2);
            this.tabPage4.Controls.Add(this.namedFloatControl_periodSpanMinutes);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(738, 166);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "平差方法";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // checkBox_isAllLinIndependentOre
            // 
            this.checkBox_isAllLinIndependentOre.AutoSize = true;
            this.checkBox_isAllLinIndependentOre.Location = new System.Drawing.Point(413, 53);
            this.checkBox_isAllLinIndependentOre.Name = "checkBox_isAllLinIndependentOre";
            this.checkBox_isAllLinIndependentOre.Size = new System.Drawing.Size(168, 16);
            this.checkBox_isAllLinIndependentOre.TabIndex = 7;
            this.checkBox_isAllLinIndependentOre.Text = "独立基线解算，否则全基线";
            this.checkBox_isAllLinIndependentOre.UseVisualStyleBackColor = true;
            this.checkBox_isAllLinIndependentOre.CheckedChanged += new System.EventHandler(this.checkBox_isAllLinIndependentOre_CheckedChanged);
            // 
            // panel_fixedSites
            // 
            this.panel_fixedSites.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_fixedSites.Controls.Add(this.arrayCheckBoxControl_fixedSites);
            this.panel_fixedSites.Controls.Add(this.button_loadSItes);
            this.panel_fixedSites.Location = new System.Drawing.Point(0, 90);
            this.panel_fixedSites.Name = "panel_fixedSites";
            this.panel_fixedSites.Size = new System.Drawing.Size(732, 70);
            this.panel_fixedSites.TabIndex = 3;
            // 
            // arrayCheckBoxControl_fixedSites
            // 
            this.arrayCheckBoxControl_fixedSites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.arrayCheckBoxControl_fixedSites.Location = new System.Drawing.Point(56, 0);
            this.arrayCheckBoxControl_fixedSites.Name = "arrayCheckBoxControl_fixedSites";
            this.arrayCheckBoxControl_fixedSites.Size = new System.Drawing.Size(676, 70);
            this.arrayCheckBoxControl_fixedSites.TabIndex = 4;
            this.arrayCheckBoxControl_fixedSites.Title = "固定站点";
            // 
            // button_loadSItes
            // 
            this.button_loadSItes.Dock = System.Windows.Forms.DockStyle.Left;
            this.button_loadSItes.Location = new System.Drawing.Point(0, 0);
            this.button_loadSItes.Name = "button_loadSItes";
            this.button_loadSItes.Size = new System.Drawing.Size(56, 70);
            this.button_loadSItes.TabIndex = 5;
            this.button_loadSItes.Text = "载入测站名";
            this.button_loadSItes.UseVisualStyleBackColor = true;
            this.button_loadSItes.Click += new System.EventHandler(this.button_loadSites_Click);
            // 
            // enumRadioControl_adjustType
            // 
            this.enumRadioControl_adjustType.IsReady = false;
            this.enumRadioControl_adjustType.Location = new System.Drawing.Point(0, 6);
            this.enumRadioControl_adjustType.Name = "enumRadioControl_adjustType";
            this.enumRadioControl_adjustType.Size = new System.Drawing.Size(404, 63);
            this.enumRadioControl_adjustType.TabIndex = 3;
            this.enumRadioControl_adjustType.Title = "平差方法";
            this.enumRadioControl_adjustType.EnumItemSelected += new System.Action<string, bool>(this.enumRadioControl_adjustType_EnumItemSelected);
            // 
            // enumRadioControl_selectLineType
            // 
            this.enumRadioControl_selectLineType.IsReady = false;
            this.enumRadioControl_selectLineType.Location = new System.Drawing.Point(587, 43);
            this.enumRadioControl_selectLineType.Name = "enumRadioControl_selectLineType";
            this.enumRadioControl_selectLineType.Size = new System.Drawing.Size(411, 42);
            this.enumRadioControl_selectLineType.TabIndex = 3;
            this.enumRadioControl_selectLineType.Title = "独立基线选择算法";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(607, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "用于区别不同时段";
            // 
            // namedFloatControl_periodSpanMinutes
            // 
            this.namedFloatControl_periodSpanMinutes.Location = new System.Drawing.Point(410, 14);
            this.namedFloatControl_periodSpanMinutes.Name = "namedFloatControl_periodSpanMinutes";
            this.namedFloatControl_periodSpanMinutes.Size = new System.Drawing.Size(191, 23);
            this.namedFloatControl_periodSpanMinutes.TabIndex = 5;
            this.namedFloatControl_periodSpanMinutes.Title = "同时段最大间隙(分)：";
            this.namedFloatControl_periodSpanMinutes.Value = 20D;
            // 
            // button_read
            // 
            this.button_read.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_read.Location = new System.Drawing.Point(553, 201);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(84, 32);
            this.button_read.TabIndex = 1;
            this.button_read.Text = "读取";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_read_Click);
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(660, 201);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(84, 32);
            this.button_run.TabIndex = 1;
            this.button_run.Text = "计算";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // tabControl_result
            // 
            this.tabControl_result.Controls.Add(this.tabPage_rawData);
            this.tabControl_result.Controls.Add(this.tabPage_netAdjustResult);
            this.tabControl_result.Controls.Add(this.tabPage_independentLines);
            this.tabControl_result.Controls.Add(this.tabPage_vectorCorrection);
            this.tabControl_result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_result.Location = new System.Drawing.Point(0, 0);
            this.tabControl_result.Name = "tabControl_result";
            this.tabControl_result.SelectedIndex = 0;
            this.tabControl_result.Size = new System.Drawing.Size(756, 264);
            this.tabControl_result.TabIndex = 0;
            // 
            // tabPage_rawData
            // 
            this.tabPage_rawData.Controls.Add(this.objectTableControl_rawData);
            this.tabPage_rawData.Location = new System.Drawing.Point(4, 22);
            this.tabPage_rawData.Name = "tabPage_rawData";
            this.tabPage_rawData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_rawData.Size = new System.Drawing.Size(748, 238);
            this.tabPage_rawData.TabIndex = 2;
            this.tabPage_rawData.Text = "原始数据";
            this.tabPage_rawData.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_rawData
            // 
            this.objectTableControl_rawData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_rawData.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_rawData.Name = "objectTableControl_rawData";
            this.objectTableControl_rawData.Size = new System.Drawing.Size(742, 232);
            this.objectTableControl_rawData.TabIndex = 1;
            this.objectTableControl_rawData.TableObjectStorage = null;
            // 
            // tabPage_netAdjustResult
            // 
            this.tabPage_netAdjustResult.Controls.Add(this.objectTableControl_netAdjustResult);
            this.tabPage_netAdjustResult.Location = new System.Drawing.Point(4, 22);
            this.tabPage_netAdjustResult.Name = "tabPage_netAdjustResult";
            this.tabPage_netAdjustResult.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_netAdjustResult.Size = new System.Drawing.Size(748, 238);
            this.tabPage_netAdjustResult.TabIndex = 0;
            this.tabPage_netAdjustResult.Text = "网平差结果";
            this.tabPage_netAdjustResult.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_netAdjustResult
            // 
            this.objectTableControl_netAdjustResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_netAdjustResult.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_netAdjustResult.Name = "objectTableControl_netAdjustResult";
            this.objectTableControl_netAdjustResult.Size = new System.Drawing.Size(742, 232);
            this.objectTableControl_netAdjustResult.TabIndex = 1;
            this.objectTableControl_netAdjustResult.TableObjectStorage = null;
            // 
            // tabPage_independentLines
            // 
            this.tabPage_independentLines.Controls.Add(this.objectTableControl_independentLine);
            this.tabPage_independentLines.Location = new System.Drawing.Point(4, 22);
            this.tabPage_independentLines.Name = "tabPage_independentLines";
            this.tabPage_independentLines.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_independentLines.Size = new System.Drawing.Size(748, 238);
            this.tabPage_independentLines.TabIndex = 1;
            this.tabPage_independentLines.Text = "独立基线";
            this.tabPage_independentLines.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_independentLine
            // 
            this.objectTableControl_independentLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_independentLine.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_independentLine.Name = "objectTableControl_independentLine";
            this.objectTableControl_independentLine.Size = new System.Drawing.Size(742, 232);
            this.objectTableControl_independentLine.TabIndex = 0;
            this.objectTableControl_independentLine.TableObjectStorage = null;
            // 
            // tabPage_vectorCorrection
            // 
            this.tabPage_vectorCorrection.Controls.Add(this.objectTableControl_vectorCorrection);
            this.tabPage_vectorCorrection.Location = new System.Drawing.Point(4, 22);
            this.tabPage_vectorCorrection.Name = "tabPage_vectorCorrection";
            this.tabPage_vectorCorrection.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_vectorCorrection.Size = new System.Drawing.Size(748, 238);
            this.tabPage_vectorCorrection.TabIndex = 3;
            this.tabPage_vectorCorrection.Text = "向量改正数";
            this.tabPage_vectorCorrection.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_vectorCorrection
            // 
            this.objectTableControl_vectorCorrection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_vectorCorrection.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_vectorCorrection.Name = "objectTableControl_vectorCorrection";
            this.objectTableControl_vectorCorrection.Size = new System.Drawing.Size(742, 232);
            this.objectTableControl_vectorCorrection.TabIndex = 1;
            this.objectTableControl_vectorCorrection.TableObjectStorage = null;
            // 
            // BaselineNetAdjustForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 509);
            this.Controls.Add(this.splitContainer1);
            this.Name = "BaselineNetAdjustForm";
            this.Text = "BaselineNetAdjustForm";
            this.Load += new System.EventHandler(this.BaselineNetClosureErrorForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.panel_fixedSites.ResumeLayout(false);
            this.tabControl_result.ResumeLayout(false);
            this.tabPage_rawData.ResumeLayout(false);
            this.tabPage_netAdjustResult.ResumeLayout(false);
            this.tabPage_independentLines.ResumeLayout(false);
            this.tabPage_vectorCorrection.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Geo.Winform.Controls.FileOpenControl fileOpenControl_baseline;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl_result;
        private System.Windows.Forms.TabPage tabPage_netAdjustResult;
        private System.Windows.Forms.TabPage tabPage_independentLines;
        private Geo.Winform.ObjectTableControl objectTableControl_netAdjustResult;
        private Geo.Winform.ObjectTableControl objectTableControl_independentLine;
        private System.Windows.Forms.Button button_run;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage_rawData;
        private Geo.Winform.ObjectTableControl objectTableControl_rawData;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_knownCoord;
        private Geo.Winform.EnumRadioControl enumRadioControl_adjustType;
        private Geo.Winform.ArrayCheckBoxControl arrayCheckBoxControl_fixedSites;
        private System.Windows.Forms.Button button_loadSItes;
        private System.Windows.Forms.Panel panel_fixedSites;
        private Geo.Winform.EnumRadioControl enumRadioControl_selectLineType;
        private System.Windows.Forms.Label label2;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_periodSpanMinutes;
        private System.Windows.Forms.Button button_showOnMap;
        private System.Windows.Forms.Button button_showIndependentLine;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_read;
        private System.Windows.Forms.CheckBox checkBox_isAllLinIndependentOre;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private System.Windows.Forms.TabPage tabPage_vectorCorrection;
        private Geo.Winform.ObjectTableControl objectTableControl_vectorCorrection;
    }
}