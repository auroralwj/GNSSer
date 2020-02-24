namespace Gnsser.Winform
{
    partial class BaselineNetClosureErrorForm
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
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_showOnMap = new System.Windows.Forms.Button();
            this.button_showLines = new System.Windows.Forms.Button();
            this.button_exportLgoasc = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.namedFloatControl_periodSpanMinutes = new Geo.Winform.Controls.NamedFloatControl();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.fileOpenControl_input = new Geo.Winform.Controls.FileOpenControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.namedFloatControl_fixedErrorVertical = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_verticalCoefOfProprotion = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_fixedErrorLevel = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_levelCoefOfProprotion = new Geo.Winform.Controls.NamedFloatControl();
            this.button_run = new System.Windows.Forms.Button();
            this.tabControl_res = new System.Windows.Forms.TabControl();
            this.tabPage_rawData = new System.Windows.Forms.TabPage();
            this.objectTableControl_rawData = new Geo.Winform.ObjectTableControl();
            this.tabPage_allSync = new System.Windows.Forms.TabPage();
            this.objectTableControl_syncclosureError = new Geo.Winform.ObjectTableControl();
            this.tabPage_allRepeat = new System.Windows.Forms.TabPage();
            this.objectTableControl_closureErrorOfRepeatBaseline = new Geo.Winform.ObjectTableControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_textResult = new Geo.Winform.Controls.RichTextBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabControl_res.SuspendLayout();
            this.tabPage_rawData.SuspendLayout();
            this.tabPage_allSync.SuspendLayout();
            this.tabPage_allRepeat.SuspendLayout();
            this.tabPage1.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.button_exportLgoasc);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.namedFloatControl_periodSpanMinutes);
            this.splitContainer1.Panel1.Controls.Add(this.tabControl2);
            this.splitContainer1.Panel1.Controls.Add(this.button_run);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl_res);
            this.splitContainer1.Size = new System.Drawing.Size(869, 450);
            this.splitContainer1.SplitterDistance = 186;
            this.splitContainer1.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(550, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(305, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "注意：若有重复基线结果，将自动采用最新的一个结果。";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_showOnMap);
            this.groupBox1.Controls.Add(this.button_showLines);
            this.groupBox1.Location = new System.Drawing.Point(13, 129);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(191, 52);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "地图查看";
            // 
            // button_showOnMap
            // 
            this.button_showOnMap.Location = new System.Drawing.Point(6, 18);
            this.button_showOnMap.Name = "button_showOnMap";
            this.button_showOnMap.Size = new System.Drawing.Size(72, 26);
            this.button_showOnMap.TabIndex = 5;
            this.button_showOnMap.Text = "查看测站";
            this.button_showOnMap.UseVisualStyleBackColor = true;
            this.button_showOnMap.Click += new System.EventHandler(this.button_showOnMap_Click);
            // 
            // button_showLines
            // 
            this.button_showLines.Location = new System.Drawing.Point(84, 18);
            this.button_showLines.Name = "button_showLines";
            this.button_showLines.Size = new System.Drawing.Size(81, 26);
            this.button_showLines.TabIndex = 5;
            this.button_showLines.Text = "所有基线";
            this.button_showLines.UseVisualStyleBackColor = true;
            this.button_showLines.Click += new System.EventHandler(this.button_showLines_Click);
            // 
            // button_exportLgoasc
            // 
            this.button_exportLgoasc.Location = new System.Drawing.Point(331, 103);
            this.button_exportLgoasc.Name = "button_exportLgoasc";
            this.button_exportLgoasc.Size = new System.Drawing.Size(136, 26);
            this.button_exportLgoasc.TabIndex = 5;
            this.button_exportLgoasc.Text = "导出为LGO基线ASC文件";
            this.button_exportLgoasc.UseVisualStyleBackColor = true;
            this.button_exportLgoasc.Click += new System.EventHandler(this.button_exportLgoasc_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(210, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "用于区别不同时段";
            // 
            // namedFloatControl_periodSpanMinutes
            // 
            this.namedFloatControl_periodSpanMinutes.Location = new System.Drawing.Point(13, 106);
            this.namedFloatControl_periodSpanMinutes.Name = "namedFloatControl_periodSpanMinutes";
            this.namedFloatControl_periodSpanMinutes.Size = new System.Drawing.Size(191, 23);
            this.namedFloatControl_periodSpanMinutes.TabIndex = 3;
            this.namedFloatControl_periodSpanMinutes.Title = "同时段最大间隙(分)：";
            this.namedFloatControl_periodSpanMinutes.Value = 20D;
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
            this.tabControl2.Size = new System.Drawing.Size(859, 96);
            this.tabControl2.TabIndex = 2;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.fileOpenControl_input);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(851, 70);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "基线文件";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // fileOpenControl_input
            // 
            this.fileOpenControl_input.AllowDrop = true;
            this.fileOpenControl_input.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileOpenControl_input.FilePath = "";
            this.fileOpenControl_input.FilePathes = new string[0];
            this.fileOpenControl_input.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_input.FirstPath = "";
            this.fileOpenControl_input.IsMultiSelect = true;
            this.fileOpenControl_input.LabelName = "文件：";
            this.fileOpenControl_input.Location = new System.Drawing.Point(3, 3);
            this.fileOpenControl_input.Name = "fileOpenControl_input";
            this.fileOpenControl_input.Size = new System.Drawing.Size(845, 64);
            this.fileOpenControl_input.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.label1);
            this.tabPage4.Controls.Add(this.label3);
            this.tabPage4.Controls.Add(this.namedFloatControl_fixedErrorVertical);
            this.tabPage4.Controls.Add(this.namedFloatControl_verticalCoefOfProprotion);
            this.tabPage4.Controls.Add(this.namedFloatControl_fixedErrorLevel);
            this.tabPage4.Controls.Add(this.namedFloatControl_levelCoefOfProprotion);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(851, 70);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "接收机精度";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(452, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "注意：ppm 为百万分之一";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(653, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "按照 GB/T 18314-2009，三边同步环闭合差应满足：Wx <= √3 / 5 σ，B、C级复测基线长度较差应满足：Wx <= 2 √2 σ";
            // 
            // namedFloatControl_fixedErrorVertical
            // 
            this.namedFloatControl_fixedErrorVertical.Location = new System.Drawing.Point(229, 6);
            this.namedFloatControl_fixedErrorVertical.Name = "namedFloatControl_fixedErrorVertical";
            this.namedFloatControl_fixedErrorVertical.Size = new System.Drawing.Size(203, 23);
            this.namedFloatControl_fixedErrorVertical.TabIndex = 4;
            this.namedFloatControl_fixedErrorVertical.Title = "垂直固定误差(mm)：";
            this.namedFloatControl_fixedErrorVertical.Value = 10D;
            // 
            // namedFloatControl_verticalCoefOfProprotion
            // 
            this.namedFloatControl_verticalCoefOfProprotion.Location = new System.Drawing.Point(229, 30);
            this.namedFloatControl_verticalCoefOfProprotion.Name = "namedFloatControl_verticalCoefOfProprotion";
            this.namedFloatControl_verticalCoefOfProprotion.Size = new System.Drawing.Size(203, 23);
            this.namedFloatControl_verticalCoefOfProprotion.TabIndex = 3;
            this.namedFloatControl_verticalCoefOfProprotion.Title = "垂直比例系数(ppm)：";
            this.namedFloatControl_verticalCoefOfProprotion.Value = 1D;
            // 
            // namedFloatControl_fixedErrorLevel
            // 
            this.namedFloatControl_fixedErrorLevel.Location = new System.Drawing.Point(10, 6);
            this.namedFloatControl_fixedErrorLevel.Name = "namedFloatControl_fixedErrorLevel";
            this.namedFloatControl_fixedErrorLevel.Size = new System.Drawing.Size(203, 23);
            this.namedFloatControl_fixedErrorLevel.TabIndex = 6;
            this.namedFloatControl_fixedErrorLevel.Title = "水平固定误差(mm)：";
            this.namedFloatControl_fixedErrorLevel.Value = 5D;
            // 
            // namedFloatControl_levelCoefOfProprotion
            // 
            this.namedFloatControl_levelCoefOfProprotion.Location = new System.Drawing.Point(10, 30);
            this.namedFloatControl_levelCoefOfProprotion.Name = "namedFloatControl_levelCoefOfProprotion";
            this.namedFloatControl_levelCoefOfProprotion.Size = new System.Drawing.Size(203, 23);
            this.namedFloatControl_levelCoefOfProprotion.TabIndex = 5;
            this.namedFloatControl_levelCoefOfProprotion.Title = "水平比例系数(ppm)：";
            this.namedFloatControl_levelCoefOfProprotion.Value = 1D;
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(771, 129);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(84, 44);
            this.button_run.TabIndex = 1;
            this.button_run.Text = "计算";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // tabControl_res
            // 
            this.tabControl_res.Controls.Add(this.tabPage_rawData);
            this.tabControl_res.Controls.Add(this.tabPage_allSync);
            this.tabControl_res.Controls.Add(this.tabPage_allRepeat);
            this.tabControl_res.Controls.Add(this.tabPage1);
            this.tabControl_res.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_res.Location = new System.Drawing.Point(0, 0);
            this.tabControl_res.Name = "tabControl_res";
            this.tabControl_res.SelectedIndex = 0;
            this.tabControl_res.Size = new System.Drawing.Size(869, 260);
            this.tabControl_res.TabIndex = 0;
            // 
            // tabPage_rawData
            // 
            this.tabPage_rawData.Controls.Add(this.objectTableControl_rawData);
            this.tabPage_rawData.Location = new System.Drawing.Point(4, 22);
            this.tabPage_rawData.Name = "tabPage_rawData";
            this.tabPage_rawData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_rawData.Size = new System.Drawing.Size(861, 234);
            this.tabPage_rawData.TabIndex = 2;
            this.tabPage_rawData.Text = "原始结果";
            this.tabPage_rawData.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_rawData
            // 
            this.objectTableControl_rawData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_rawData.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_rawData.Name = "objectTableControl_rawData";
            this.objectTableControl_rawData.Size = new System.Drawing.Size(855, 228);
            this.objectTableControl_rawData.TabIndex = 1;
            this.objectTableControl_rawData.TableObjectStorage = null;
            // 
            // tabPage_allSync
            // 
            this.tabPage_allSync.Controls.Add(this.objectTableControl_syncclosureError);
            this.tabPage_allSync.Location = new System.Drawing.Point(4, 22);
            this.tabPage_allSync.Name = "tabPage_allSync";
            this.tabPage_allSync.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_allSync.Size = new System.Drawing.Size(861, 234);
            this.tabPage_allSync.TabIndex = 0;
            this.tabPage_allSync.Text = "同步环闭合差";
            this.tabPage_allSync.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_syncclosureError
            // 
            this.objectTableControl_syncclosureError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_syncclosureError.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_syncclosureError.Name = "objectTableControl_syncclosureError";
            this.objectTableControl_syncclosureError.Size = new System.Drawing.Size(855, 228);
            this.objectTableControl_syncclosureError.TabIndex = 1;
            this.objectTableControl_syncclosureError.TableObjectStorage = null;
            // 
            // tabPage_allRepeat
            // 
            this.tabPage_allRepeat.Controls.Add(this.objectTableControl_closureErrorOfRepeatBaseline);
            this.tabPage_allRepeat.Location = new System.Drawing.Point(4, 22);
            this.tabPage_allRepeat.Name = "tabPage_allRepeat";
            this.tabPage_allRepeat.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_allRepeat.Size = new System.Drawing.Size(861, 234);
            this.tabPage_allRepeat.TabIndex = 1;
            this.tabPage_allRepeat.Text = "复测基线较差";
            this.tabPage_allRepeat.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_closureErrorOfRepeatBaseline
            // 
            this.objectTableControl_closureErrorOfRepeatBaseline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_closureErrorOfRepeatBaseline.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_closureErrorOfRepeatBaseline.Name = "objectTableControl_closureErrorOfRepeatBaseline";
            this.objectTableControl_closureErrorOfRepeatBaseline.Size = new System.Drawing.Size(855, 228);
            this.objectTableControl_closureErrorOfRepeatBaseline.TabIndex = 0;
            this.objectTableControl_closureErrorOfRepeatBaseline.TableObjectStorage = null;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextBoxControl_textResult);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(861, 234);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "文本结果";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_textResult
            // 
            this.richTextBoxControl_textResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_textResult.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_textResult.MaxAppendLineCount = 5000;
            this.richTextBoxControl_textResult.Name = "richTextBoxControl_textResult";
            this.richTextBoxControl_textResult.Size = new System.Drawing.Size(855, 228);
            this.richTextBoxControl_textResult.TabIndex = 0;
            this.richTextBoxControl_textResult.Text = "";
            // 
            // BaselineNetClosureErrorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(869, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "BaselineNetClosureErrorForm";
            this.Text = "BaselineNetClosureErrorForm";
            this.Load += new System.EventHandler(this.BaselineNetClosureErrorForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabControl_res.ResumeLayout(false);
            this.tabPage_rawData.ResumeLayout(false);
            this.tabPage_allSync.ResumeLayout(false);
            this.tabPage_allRepeat.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Geo.Winform.Controls.FileOpenControl fileOpenControl_input;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl_res;
        private System.Windows.Forms.TabPage tabPage_allSync;
        private System.Windows.Forms.TabPage tabPage_allRepeat;
        private Geo.Winform.ObjectTableControl objectTableControl_syncclosureError;
        private Geo.Winform.ObjectTableControl objectTableControl_closureErrorOfRepeatBaseline;
        private System.Windows.Forms.Button button_run;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label1;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_fixedErrorVertical;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_verticalCoefOfProprotion;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_fixedErrorLevel;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_levelCoefOfProprotion;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_periodSpanMinutes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPage_rawData;
        private Geo.Winform.ObjectTableControl objectTableControl_rawData;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_showOnMap;
        private System.Windows.Forms.Button button_showLines;
        private System.Windows.Forms.Button button_exportLgoasc;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tabPage1;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl_textResult;
    }
}