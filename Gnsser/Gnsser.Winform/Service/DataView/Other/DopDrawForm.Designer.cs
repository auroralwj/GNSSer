namespace Gnsser.Winform
{
    partial class DopDrawForm
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
            this.button_tdop = new System.Windows.Forms.Button();
            this.buttonvdop = new System.Windows.Forms.Button();
            this.button_hdop = new System.Windows.Forms.Button();
            this.button_pod = new System.Windows.Forms.Button();
            this.button_drawRms = new System.Windows.Forms.Button();
            this.button_draw = new System.Windows.Forms.Button();
            this.fileOpenControl1 = new Geo.Winform.Controls.FileOpenControl();
            this.button_read = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.objectTableControl1 = new Geo.Winform.ObjectTableControl();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.enabledFloatControl_min = new Geo.Winform.Controls.EnabledFloatControl();
            this.enabledFloatControl_max = new Geo.Winform.Controls.EnabledFloatControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.tabControl2);
            this.splitContainer1.Panel1.Controls.Add(this.button_tdop);
            this.splitContainer1.Panel1.Controls.Add(this.buttonvdop);
            this.splitContainer1.Panel1.Controls.Add(this.button_hdop);
            this.splitContainer1.Panel1.Controls.Add(this.button_pod);
            this.splitContainer1.Panel1.Controls.Add(this.button_drawRms);
            this.splitContainer1.Panel1.Controls.Add(this.button_draw);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(660, 405);
            this.splitContainer1.SplitterDistance = 138;
            this.splitContainer1.TabIndex = 0;
            // 
            // button_tdop
            // 
            this.button_tdop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_tdop.Location = new System.Drawing.Point(586, 109);
            this.button_tdop.Name = "button_tdop";
            this.button_tdop.Size = new System.Drawing.Size(63, 23);
            this.button_tdop.TabIndex = 15;
            this.button_tdop.Text = "绘TDOP";
            this.button_tdop.UseVisualStyleBackColor = true;
            this.button_tdop.Click += new System.EventHandler(this.button_tdop_Click);
            // 
            // buttonvdop
            // 
            this.buttonvdop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonvdop.Location = new System.Drawing.Point(519, 109);
            this.buttonvdop.Name = "buttonvdop";
            this.buttonvdop.Size = new System.Drawing.Size(63, 23);
            this.buttonvdop.TabIndex = 15;
            this.buttonvdop.Text = "绘VDOP";
            this.buttonvdop.UseVisualStyleBackColor = true;
            this.buttonvdop.Click += new System.EventHandler(this.buttonvdop_Click);
            // 
            // button_hdop
            // 
            this.button_hdop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_hdop.Location = new System.Drawing.Point(450, 109);
            this.button_hdop.Name = "button_hdop";
            this.button_hdop.Size = new System.Drawing.Size(63, 23);
            this.button_hdop.TabIndex = 15;
            this.button_hdop.Text = "绘HDOP";
            this.button_hdop.UseVisualStyleBackColor = true;
            this.button_hdop.Click += new System.EventHandler(this.button_hdop_Click);
            // 
            // button_pod
            // 
            this.button_pod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_pod.Location = new System.Drawing.Point(381, 109);
            this.button_pod.Name = "button_pod";
            this.button_pod.Size = new System.Drawing.Size(63, 23);
            this.button_pod.TabIndex = 15;
            this.button_pod.Text = "绘PDOP";
            this.button_pod.UseVisualStyleBackColor = true;
            this.button_pod.Click += new System.EventHandler(this.button_pod_Click);
            // 
            // button_drawRms
            // 
            this.button_drawRms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_drawRms.Location = new System.Drawing.Point(312, 109);
            this.button_drawRms.Name = "button_drawRms";
            this.button_drawRms.Size = new System.Drawing.Size(63, 23);
            this.button_drawRms.TabIndex = 15;
            this.button_drawRms.Text = "绘GDOP";
            this.button_drawRms.UseVisualStyleBackColor = true;
            this.button_drawRms.Click += new System.EventHandler(this.button_drawRms_Click);
            // 
            // button_draw
            // 
            this.button_draw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_draw.Location = new System.Drawing.Point(194, 109);
            this.button_draw.Name = "button_draw";
            this.button_draw.Size = new System.Drawing.Size(99, 23);
            this.button_draw.TabIndex = 14;
            this.button_draw.Text = "绘卫星可见数";
            this.button_draw.UseVisualStyleBackColor = true;
            this.button_draw.Click += new System.EventHandler(this.button_draw_Click);
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl1.AllowDrop = true;
            this.fileOpenControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileOpenControl1.FilePath = "";
            this.fileOpenControl1.FilePathes = new string[0];
            this.fileOpenControl1.Filter = "DOP文件|*.txt.xls|文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl1.FirstPath = "";
            this.fileOpenControl1.IsMultiSelect = true;
            this.fileOpenControl1.LabelName = "文件：";
            this.fileOpenControl1.Location = new System.Drawing.Point(3, 3);
            this.fileOpenControl1.Name = "fileOpenControl1";
            this.fileOpenControl1.Size = new System.Drawing.Size(554, 73);
            this.fileOpenControl1.TabIndex = 6;
            // 
            // button_read
            // 
            this.button_read.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_read.Location = new System.Drawing.Point(557, 3);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(85, 73);
            this.button_read.TabIndex = 7;
            this.button_read.Text = "读取并显示\r\n\r\n(显示第一个)";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_read_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(660, 263);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.objectTableControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(652, 237);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "详细信息";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // objectTableControl1
            // 
            this.objectTableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl1.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl1.Name = "objectTableControl1";
            this.objectTableControl1.Size = new System.Drawing.Size(646, 231);
            this.objectTableControl1.TabIndex = 0;
            this.objectTableControl1.TableObjectStorage = null;
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Location = new System.Drawing.Point(3, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(653, 105);
            this.tabControl2.TabIndex = 17;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.fileOpenControl1);
            this.tabPage1.Controls.Add(this.button_read);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(645, 79);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "输入";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.enabledFloatControl_max);
            this.tabPage3.Controls.Add(this.enabledFloatControl_min);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(645, 79);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "设置";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // enabledFloatControl_min
            // 
            this.enabledFloatControl_min.Location = new System.Drawing.Point(14, 6);
            this.enabledFloatControl_min.Name = "enabledFloatControl_min";
            this.enabledFloatControl_min.Size = new System.Drawing.Size(151, 23);
            this.enabledFloatControl_min.TabIndex = 17;
            this.enabledFloatControl_min.Title = "最小值：";
            this.enabledFloatControl_min.Value = 4D;
            // 
            // enabledFloatControl_max
            // 
            this.enabledFloatControl_max.Location = new System.Drawing.Point(14, 35);
            this.enabledFloatControl_max.Name = "enabledFloatControl_max";
            this.enabledFloatControl_max.Size = new System.Drawing.Size(151, 23);
            this.enabledFloatControl_max.TabIndex = 17;
            this.enabledFloatControl_max.Title = "最大值：";
            this.enabledFloatControl_max.Value = 15D;
            // 
            // DopDrawForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 405);
            this.Controls.Add(this.splitContainer1);
            this.Name = "DopDrawForm";
            this.Text = "DOP绘图";
            this.Load += new System.EventHandler(this.IgsFcbViewerForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl1;
        private System.Windows.Forms.Button button_read;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private Geo.Winform.ObjectTableControl objectTableControl1;
        private System.Windows.Forms.Button button_draw;
        private System.Windows.Forms.Button button_drawRms;
        private System.Windows.Forms.Button button_tdop;
        private System.Windows.Forms.Button buttonvdop;
        private System.Windows.Forms.Button button_hdop;
        private System.Windows.Forms.Button button_pod;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage3;
        private Geo.Winform.Controls.EnabledFloatControl enabledFloatControl_max;
        private Geo.Winform.Controls.EnabledFloatControl enabledFloatControl_min;
    }
}