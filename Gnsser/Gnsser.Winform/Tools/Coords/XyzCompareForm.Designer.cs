namespace Gnsser.Winform
{
    partial class XyzCompareForm
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

        #region Windows Form Designer generated obsCodeode

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the obsCodeode editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_readB = new System.Windows.Forms.Button();
            this.button_showOnMap = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.label5 = new System.Windows.Forms.Label();
            this.button_draw = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.checkBox_unitMm = new System.Windows.Forms.CheckBox();
            this.checkBox_isBaseline = new System.Windows.Forms.CheckBox();
            this.fileOpenControlB = new Geo.Winform.Controls.FileOpenControl();
            this.fileOpenControlA = new Geo.Winform.Controls.FileOpenControl();
            this.enabledIntControl_nameLength = new Geo.Winform.Controls.EnabledIntControl();
            this.objectTableControl_tableA = new Geo.Winform.ObjectTableControl();
            this.objectTableControl_tableB = new Geo.Winform.ObjectTableControl();
            this.objectTableControl_result = new Geo.Winform.ObjectTableControl();
            this.objectTableControl_ave = new Geo.Winform.ObjectTableControl();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_readB
            // 
            this.button_readB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_readB.Location = new System.Drawing.Point(741, 104);
            this.button_readB.Name = "button_readB";
            this.button_readB.Size = new System.Drawing.Size(97, 37);
            this.button_readB.TabIndex = 19;
            this.button_readB.Text = "读取并比较";
            this.button_readB.UseVisualStyleBackColor = true;
            this.button_readB.Click += new System.EventHandler(this.button_readB_Click);
            // 
            // button_showOnMap
            // 
            this.button_showOnMap.Location = new System.Drawing.Point(96, 103);
            this.button_showOnMap.Name = "button_showOnMap";
            this.button_showOnMap.Size = new System.Drawing.Size(74, 33);
            this.button_showOnMap.TabIndex = 36;
            this.button_showOnMap.Text = "地图上显示";
            this.button_showOnMap.UseVisualStyleBackColor = true;
            this.button_showOnMap.Click += new System.EventHandler(this.button_showOnMap_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(438, 196);
            this.tabControl1.TabIndex = 40;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.objectTableControl_tableA);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(430, 170);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "数据表格";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl2);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Size = new System.Drawing.Size(854, 208);
            this.splitContainer1.SplitterDistance = 438;
            this.splitContainer1.TabIndex = 41;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(438, 12);
            this.label3.TabIndex = 41;
            this.label3.Text = "文件A";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 12);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(412, 196);
            this.tabControl2.TabIndex = 41;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.objectTableControl_tableB);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(404, 170);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "数据表格";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(412, 12);
            this.label4.TabIndex = 42;
            this.label4.Text = "文件B";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer2.Panel2.Controls.Add(this.label5);
            this.splitContainer2.Size = new System.Drawing.Size(854, 416);
            this.splitContainer2.SplitterDistance = 208;
            this.splitContainer2.TabIndex = 42;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 12);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.objectTableControl_result);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.objectTableControl_ave);
            this.splitContainer4.Size = new System.Drawing.Size(854, 192);
            this.splitContainer4.SplitterDistance = 447;
            this.splitContainer4.TabIndex = 44;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(854, 12);
            this.label5.TabIndex = 42;
            this.label5.Text = "比较结果与统计误差";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_draw
            // 
            this.button_draw.Location = new System.Drawing.Point(13, 104);
            this.button_draw.Margin = new System.Windows.Forms.Padding(2);
            this.button_draw.Name = "button_draw";
            this.button_draw.Size = new System.Drawing.Size(78, 32);
            this.button_draw.TabIndex = 43;
            this.button_draw.Text = "绘偏差图";
            this.button_draw.UseVisualStyleBackColor = true;
            this.button_draw.Click += new System.EventHandler(this.button_draw_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(323, 12);
            this.label6.TabIndex = 44;
            this.label6.Text = "说明：支持文本文件，规则为列头部必须包含Name，X，Y，Z";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.tabControl3);
            this.splitContainer3.Panel1.Controls.Add(this.button_readB);
            this.splitContainer3.Panel1.Controls.Add(this.checkBox_isBaseline);
            this.splitContainer3.Panel1.Controls.Add(this.button_draw);
            this.splitContainer3.Panel1.Controls.Add(this.button_showOnMap);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer3.Size = new System.Drawing.Size(854, 564);
            this.splitContainer3.SplitterDistance = 144;
            this.splitContainer3.TabIndex = 45;
            // 
            // tabControl3
            // 
            this.tabControl3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl3.Controls.Add(this.tabPage1);
            this.tabControl3.Controls.Add(this.tabPage3);
            this.tabControl3.Location = new System.Drawing.Point(7, 3);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(835, 100);
            this.tabControl3.TabIndex = 46;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.fileOpenControlB);
            this.tabPage1.Controls.Add(this.fileOpenControlA);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(827, 74);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "数据输入";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.enabledIntControl_nameLength);
            this.tabPage3.Controls.Add(this.checkBox_unitMm);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(827, 74);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "设置";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // checkBox_unitMm
            // 
            this.checkBox_unitMm.AutoSize = true;
            this.checkBox_unitMm.Location = new System.Drawing.Point(18, 45);
            this.checkBox_unitMm.Name = "checkBox_unitMm";
            this.checkBox_unitMm.Size = new System.Drawing.Size(132, 16);
            this.checkBox_unitMm.TabIndex = 45;
            this.checkBox_unitMm.Text = "毫米(默认米为单位)";
            this.checkBox_unitMm.UseVisualStyleBackColor = true;
            // 
            // checkBox_isBaseline
            // 
            this.checkBox_isBaseline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_isBaseline.AutoSize = true;
            this.checkBox_isBaseline.Location = new System.Drawing.Point(663, 114);
            this.checkBox_isBaseline.Name = "checkBox_isBaseline";
            this.checkBox_isBaseline.Size = new System.Drawing.Size(72, 16);
            this.checkBox_isBaseline.TabIndex = 45;
            this.checkBox_isBaseline.Text = "比较基线";
            this.checkBox_isBaseline.UseVisualStyleBackColor = true;
            // 
            // fileOpenControlB
            // 
            this.fileOpenControlB.AllowDrop = true;
            this.fileOpenControlB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControlB.FilePath = "";
            this.fileOpenControlB.FilePathes = new string[0];
            this.fileOpenControlB.Filter = "坐标文件(SINEX;文本Excel;txt;Org文件;Rep文件)|*.snx;*.xls;*.txt;*.org;*.rep|所有文件(*.*)|*.*";
            this.fileOpenControlB.FirstPath = "";
            this.fileOpenControlB.IsMultiSelect = false;
            this.fileOpenControlB.LabelName = "文件B：";
            this.fileOpenControlB.Location = new System.Drawing.Point(14, 31);
            this.fileOpenControlB.Name = "fileOpenControlB";
            this.fileOpenControlB.Size = new System.Drawing.Size(718, 22);
            this.fileOpenControlB.TabIndex = 20;
            // 
            // fileOpenControlA
            // 
            this.fileOpenControlA.AllowDrop = true;
            this.fileOpenControlA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControlA.FilePath = "";
            this.fileOpenControlA.FilePathes = new string[0];
            this.fileOpenControlA.Filter = "坐标文件(SINEX;文本Excel;txt;Org文件;Rep文件)|*.snx;*.xls;*.txt;*.org;*.rep|所有文件(*.*)|*.*";
            this.fileOpenControlA.FirstPath = "";
            this.fileOpenControlA.IsMultiSelect = false;
            this.fileOpenControlA.LabelName = "文件A：";
            this.fileOpenControlA.Location = new System.Drawing.Point(14, 6);
            this.fileOpenControlA.Name = "fileOpenControlA";
            this.fileOpenControlA.Size = new System.Drawing.Size(718, 22);
            this.fileOpenControlA.TabIndex = 20;
            // 
            // enabledIntControl_nameLength
            // 
            this.enabledIntControl_nameLength.Location = new System.Drawing.Point(18, 16);
            this.enabledIntControl_nameLength.Name = "enabledIntControl_nameLength";
            this.enabledIntControl_nameLength.Size = new System.Drawing.Size(196, 23);
            this.enabledIntControl_nameLength.TabIndex = 0;
            this.enabledIntControl_nameLength.Title = "比较名称长度:";
            this.enabledIntControl_nameLength.Value = 4;
            // 
            // objectTableControl_tableA
            // 
            this.objectTableControl_tableA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_tableA.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_tableA.Name = "objectTableControl_tableA";
            this.objectTableControl_tableA.Size = new System.Drawing.Size(424, 164);
            this.objectTableControl_tableA.TabIndex = 2;
            this.objectTableControl_tableA.TableObjectStorage = null;
            // 
            // objectTableControl_tableB
            // 
            this.objectTableControl_tableB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_tableB.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_tableB.Name = "objectTableControl_tableB";
            this.objectTableControl_tableB.Size = new System.Drawing.Size(398, 164);
            this.objectTableControl_tableB.TabIndex = 2;
            this.objectTableControl_tableB.TableObjectStorage = null;
            // 
            // objectTableControl_result
            // 
            this.objectTableControl_result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_result.Location = new System.Drawing.Point(0, 0);
            this.objectTableControl_result.Name = "objectTableControl_result";
            this.objectTableControl_result.Size = new System.Drawing.Size(447, 192);
            this.objectTableControl_result.TabIndex = 0;
            this.objectTableControl_result.TableObjectStorage = null;
            // 
            // objectTableControl_ave
            // 
            this.objectTableControl_ave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_ave.Location = new System.Drawing.Point(0, 0);
            this.objectTableControl_ave.Name = "objectTableControl_ave";
            this.objectTableControl_ave.Size = new System.Drawing.Size(403, 192);
            this.objectTableControl_ave.TabIndex = 0;
            this.objectTableControl_ave.TableObjectStorage = null;
            // 
            // XyzCompareForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 564);
            this.Controls.Add(this.splitContainer3);
            this.Name = "XyzCompareForm";
            this.Text = "坐标比较";
            this.Load += new System.EventHandler(this.XyzCompareForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_showOnMap;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button_readB;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_draw;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private Geo.Winform.Controls.FileOpenControl fileOpenControlB;
        private Geo.Winform.Controls.FileOpenControl fileOpenControlA;
        private System.Windows.Forms.CheckBox checkBox_isBaseline;
        private Geo.Winform.ObjectTableControl objectTableControl_result;
        private Geo.Winform.ObjectTableControl objectTableControl_ave;
        private Geo.Winform.ObjectTableControl objectTableControl_tableA;
        private Geo.Winform.ObjectTableControl objectTableControl_tableB;
        private System.Windows.Forms.CheckBox checkBox_unitMm;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage3;
        private Geo.Winform.Controls.EnabledIntControl enabledIntControl_nameLength;
    }
}