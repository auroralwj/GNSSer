namespace Gnsser.Winform
{
    partial class WideLaneGpsBiasViewerForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_wsbUrl = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_down = new System.Windows.Forms.Button();
            this.fileOpenControlOpath = new Geo.Winform.Controls.FileOpenControl();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.richTextBoxControl1 = new Geo.Winform.Controls.RichTextBoxControl();
            this.button_view = new System.Windows.Forms.Button();
            this.button_differ = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.timePeriodControl1 = new Geo.Winform.Controls.TimePeriodControl();
            this.button_periodView = new System.Windows.Forms.Button();
            this.objectTableControl1 = new Geo.Winform.ObjectTableControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button_convertToGnsserFcb = new System.Windows.Forms.Button();
            this.baseSatSelectingControl1 = new Gnsser.Winform.Controls.BaseSatSelectingControl();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_wsbUrl);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.button_down);
            this.groupBox1.Controls.Add(this.fileOpenControlOpath);
            this.groupBox1.Controls.Add(this.baseSatSelectingControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(684, 142);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "文件/公共设置";
            // 
            // textBox_wsbUrl
            // 
            this.textBox_wsbUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_wsbUrl.Location = new System.Drawing.Point(97, 20);
            this.textBox_wsbUrl.Name = "textBox_wsbUrl";
            this.textBox_wsbUrl.Size = new System.Drawing.Size(455, 21);
            this.textBox_wsbUrl.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "WSB URL：";
            // 
            // button_down
            // 
            this.button_down.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_down.Location = new System.Drawing.Point(558, 20);
            this.button_down.Name = "button_down";
            this.button_down.Size = new System.Drawing.Size(120, 23);
            this.button_down.TabIndex = 4;
            this.button_down.Text = "下载最新wsd文件";
            this.button_down.UseVisualStyleBackColor = true;
            this.button_down.Click += new System.EventHandler(this.button_down_Click);
            // 
            // fileOpenControlOpath
            // 
            this.fileOpenControlOpath.AllowDrop = true;
            this.fileOpenControlOpath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControlOpath.FilePath = "";
            this.fileOpenControlOpath.FilePathes = new string[0];
            this.fileOpenControlOpath.Filter = "wsb文件|*.wsb|文本文件|*.txt|所有文件|*.*";
            this.fileOpenControlOpath.FirstPath = "";
            this.fileOpenControlOpath.IsMultiSelect = false;
            this.fileOpenControlOpath.LabelName = "本地WSB文件：";
            this.fileOpenControlOpath.Location = new System.Drawing.Point(6, 53);
            this.fileOpenControlOpath.Name = "fileOpenControlOpath";
            this.fileOpenControlOpath.Size = new System.Drawing.Size(518, 21);
            this.fileOpenControlOpath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "历元选择：";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(92, 15);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 21);
            this.dateTimePicker1.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.richTextBoxControl1);
            this.groupBox2.Location = new System.Drawing.Point(2, 56);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(675, 296);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "输出";
            // 
            // richTextBoxControl1
            // 
            this.richTextBoxControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl1.Location = new System.Drawing.Point(3, 17);
            this.richTextBoxControl1.MaxAppendLineCount = 5000;
            this.richTextBoxControl1.Name = "richTextBoxControl1";
            this.richTextBoxControl1.Size = new System.Drawing.Size(669, 276);
            this.richTextBoxControl1.TabIndex = 0;
            this.richTextBoxControl1.Text = "";
            // 
            // button_view
            // 
            this.button_view.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_view.Location = new System.Drawing.Point(427, 9);
            this.button_view.Name = "button_view";
            this.button_view.Size = new System.Drawing.Size(75, 27);
            this.button_view.TabIndex = 4;
            this.button_view.Text = "查看";
            this.button_view.UseVisualStyleBackColor = true;
            this.button_view.Click += new System.EventHandler(this.button_view_Click);
            // 
            // button_differ
            // 
            this.button_differ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_differ.Location = new System.Drawing.Point(522, 9);
            this.button_differ.Name = "button_differ";
            this.button_differ.Size = new System.Drawing.Size(146, 27);
            this.button_differ.TabIndex = 19;
            this.button_differ.Text = "做差归算到MW正小数";
            this.button_differ.UseVisualStyleBackColor = true;
            this.button_differ.Click += new System.EventHandler(this.button_differ_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(684, 381);
            this.tabControl1.TabIndex = 20;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dateTimePicker1);
            this.tabPage1.Controls.Add(this.button_view);
            this.tabPage1.Controls.Add(this.button_differ);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(676, 355);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "单日查看";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button_convertToGnsserFcb);
            this.tabPage2.Controls.Add(this.timePeriodControl1);
            this.tabPage2.Controls.Add(this.button_periodView);
            this.tabPage2.Controls.Add(this.objectTableControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(676, 355);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "表格批量查看";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // timePeriodControl1
            // 
            this.timePeriodControl1.Location = new System.Drawing.Point(2, 13);
            this.timePeriodControl1.Margin = new System.Windows.Forms.Padding(2);
            this.timePeriodControl1.Name = "timePeriodControl1";
            this.timePeriodControl1.Size = new System.Drawing.Size(414, 24);
            this.timePeriodControl1.TabIndex = 3;
            this.timePeriodControl1.TimeFrom = new System.DateTime(2018, 9, 6, 21, 34, 7, 750);
            this.timePeriodControl1.TimeStringFormat = "yyyy-MM-dd HH:mm:ss";
            this.timePeriodControl1.TimeTo = new System.DateTime(2018, 9, 6, 21, 34, 7, 758);
            this.timePeriodControl1.Title = "时段：";
            // 
            // button_periodView
            // 
            this.button_periodView.Location = new System.Drawing.Point(421, 6);
            this.button_periodView.Name = "button_periodView";
            this.button_periodView.Size = new System.Drawing.Size(91, 31);
            this.button_periodView.TabIndex = 2;
            this.button_periodView.Text = "批量查看";
            this.button_periodView.UseVisualStyleBackColor = true;
            this.button_periodView.Click += new System.EventHandler(this.button_periodView_Click);
            // 
            // objectTableControl1
            // 
            this.objectTableControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectTableControl1.Location = new System.Drawing.Point(6, 56);
            this.objectTableControl1.Name = "objectTableControl1";
            this.objectTableControl1.Size = new System.Drawing.Size(667, 299);
            this.objectTableControl1.TabIndex = 1;
            this.objectTableControl1.TableObjectStorage = null;
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
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(684, 527);
            this.splitContainer1.SplitterDistance = 142;
            this.splitContainer1.TabIndex = 21;
            // 
            // button_convertToGnsserFcb
            // 
            this.button_convertToGnsserFcb.Location = new System.Drawing.Point(531, 6);
            this.button_convertToGnsserFcb.Name = "button_convertToGnsserFcb";
            this.button_convertToGnsserFcb.Size = new System.Drawing.Size(137, 31);
            this.button_convertToGnsserFcb.TabIndex = 7;
            this.button_convertToGnsserFcb.Text = "转换为GNSSer FCB文件";
            this.button_convertToGnsserFcb.UseVisualStyleBackColor = true;
            this.button_convertToGnsserFcb.Click += new System.EventHandler(this.button_convertToGnsserFcb_Click);
            // 
            // baseSatSelectingControl1
            // 
            this.baseSatSelectingControl1.EnableBaseSat = true;
            this.baseSatSelectingControl1.Location = new System.Drawing.Point(9, 93);
            this.baseSatSelectingControl1.Name = "baseSatSelectingControl1";
            this.baseSatSelectingControl1.Size = new System.Drawing.Size(602, 43);
            this.baseSatSelectingControl1.TabIndex = 18;
            // 
            // WideLaneGpsBiasViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 527);
            this.Controls.Add(this.splitContainer1);
            this.Name = "WideLaneGpsBiasViewerForm";
            this.Text = "法国IGS模糊度宽项硬件延迟";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WideLaneGpsBiasViewerForm_FormClosing);
            this.Load += new System.EventHandler(this.WideLaneGpsBiasViewerForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private Geo.Winform.Controls.FileOpenControl fileOpenControlOpath;
        private System.Windows.Forms.Button button_view;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl1;
        private Gnsser.Winform.Controls.BaseSatSelectingControl baseSatSelectingControl1;
        private System.Windows.Forms.Button button_differ;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_down;
        private System.Windows.Forms.TextBox textBox_wsbUrl;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Geo.Winform.ObjectTableControl objectTableControl1;
        private System.Windows.Forms.Button button_periodView;
        private Geo.Winform.Controls.TimePeriodControl timePeriodControl1;
        private System.Windows.Forms.Button button_convertToGnsserFcb;
    }
}