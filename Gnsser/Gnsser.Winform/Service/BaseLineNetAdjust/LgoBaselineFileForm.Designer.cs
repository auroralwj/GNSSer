namespace Gnsser.Winform
{
    partial class LgoBaselineFileForm
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
            this.button_read = new System.Windows.Forms.Button();
            this.button_showOnMap = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.fileOpenControl_baseline = new Geo.Winform.Controls.FileOpenControl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.objectTableControl_rawData = new Geo.Winform.ObjectTableControl();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.objectTableControl_sites = new Geo.Winform.ObjectTableControl();
            this.label2 = new System.Windows.Forms.Label();
            this.namedFloatControl_periodSpanMinutes = new Geo.Winform.Controls.NamedFloatControl();
            this.button_write = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage5.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.button_write);
            this.splitContainer1.Panel1.Controls.Add(this.button_read);
            this.splitContainer1.Panel1.Controls.Add(this.button_showOnMap);
            this.splitContainer1.Panel1.Controls.Add(this.tabControl2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(756, 509);
            this.splitContainer1.SplitterDistance = 181;
            this.splitContainer1.TabIndex = 1;
            // 
            // button_read
            // 
            this.button_read.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_read.Location = new System.Drawing.Point(565, 149);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(75, 23);
            this.button_read.TabIndex = 4;
            this.button_read.Text = "读取";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_read_Click);
            // 
            // button_showOnMap
            // 
            this.button_showOnMap.Location = new System.Drawing.Point(5, 149);
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
            this.tabControl2.Location = new System.Drawing.Point(3, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(746, 137);
            this.tabControl2.TabIndex = 2;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.namedFloatControl_periodSpanMinutes);
            this.tabPage3.Controls.Add(this.fileOpenControl_baseline);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(738, 111);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "基线文件";
            this.tabPage3.UseVisualStyleBackColor = true;
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
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(756, 324);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.objectTableControl_rawData);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(748, 298);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "原始数据";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_rawData
            // 
            this.objectTableControl_rawData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_rawData.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_rawData.Name = "objectTableControl_rawData";
            this.objectTableControl_rawData.Size = new System.Drawing.Size(742, 292);
            this.objectTableControl_rawData.TabIndex = 1;
            this.objectTableControl_rawData.TableObjectStorage = null;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.objectTableControl_sites);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(748, 254);
            this.tabPage6.TabIndex = 3;
            this.tabPage6.Text = "测站坐标";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_sites
            // 
            this.objectTableControl_sites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_sites.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_sites.Name = "objectTableControl_sites";
            this.objectTableControl_sites.Size = new System.Drawing.Size(742, 248);
            this.objectTableControl_sites.TabIndex = 2;
            this.objectTableControl_sites.TableObjectStorage = null;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(203, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "用于区别不同时段";
            // 
            // namedFloatControl_periodSpanMinutes
            // 
            this.namedFloatControl_periodSpanMinutes.Location = new System.Drawing.Point(6, 68);
            this.namedFloatControl_periodSpanMinutes.Name = "namedFloatControl_periodSpanMinutes";
            this.namedFloatControl_periodSpanMinutes.Size = new System.Drawing.Size(191, 23);
            this.namedFloatControl_periodSpanMinutes.TabIndex = 7;
            this.namedFloatControl_periodSpanMinutes.Title = "同时段最大间隙(分)：";
            this.namedFloatControl_periodSpanMinutes.Value = 20D;
            // 
            // button_write
            // 
            this.button_write.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_write.Location = new System.Drawing.Point(658, 149);
            this.button_write.Name = "button_write";
            this.button_write.Size = new System.Drawing.Size(75, 23);
            this.button_write.TabIndex = 4;
            this.button_write.Text = "写出";
            this.button_write.UseVisualStyleBackColor = true;
            this.button_write.Click += new System.EventHandler(this.button_write_Click);
            // 
            // LgoBaselineFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 509);
            this.Controls.Add(this.splitContainer1);
            this.Name = "LgoBaselineFileForm";
            this.Text = "BaselineNetAdjustForm";
            this.Load += new System.EventHandler(this.BaselineNetClosureErrorForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Geo.Winform.Controls.FileOpenControl fileOpenControl_baseline;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage5;
        private Geo.Winform.ObjectTableControl objectTableControl_rawData;
        private System.Windows.Forms.Button button_showOnMap;
        private System.Windows.Forms.Button button_read;
        private System.Windows.Forms.TabPage tabPage6;
        private Geo.Winform.ObjectTableControl objectTableControl_sites;
        private System.Windows.Forms.Label label2;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_periodSpanMinutes;
        private System.Windows.Forms.Button button_write;
    }
}