namespace Gnsser.Winform
{
    partial class WhuFcbServiceForm
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
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.comboBox_basePrn = new System.Windows.Forms.ComboBox();
            this.bindingSource_basePrn = new System.Windows.Forms.BindingSource(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl1 = new Geo.Winform.Controls.RichTextBoxControl();
            this.namedTimeControl1 = new Geo.Winform.Controls.NamedTimeControl();
            this.button_vewSingleEpoch = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button_multiShow = new System.Windows.Forms.Button();
            this.timePeriodControl1 = new Geo.Winform.Controls.TimePeriodControl();
            this.objectTableControl1 = new Geo.Winform.ObjectTableControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button_saveToGnsserFcb = new System.Windows.Forms.Button();
            this.button_viewSingleWL = new System.Windows.Forms.Button();
            this.button_multiWLView = new System.Windows.Forms.Button();
            this.button_saveWLToGNSSer = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_basePrn)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.directorySelectionControl1);
            this.splitContainer1.Panel1.Controls.Add(this.comboBox_basePrn);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1094, 631);
            this.splitContainer1.SplitterDistance = 78;
            this.splitContainer1.TabIndex = 0;
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "文件夹：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(17, 12);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "";
            this.directorySelectionControl1.Pathes = new string[0];
            this.directorySelectionControl1.Size = new System.Drawing.Size(598, 22);
            this.directorySelectionControl1.TabIndex = 8;
            // 
            // comboBox_basePrn
            // 
            this.comboBox_basePrn.DataSource = this.bindingSource_basePrn;
            this.comboBox_basePrn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_basePrn.FormattingEnabled = true;
            this.comboBox_basePrn.Location = new System.Drawing.Point(77, 40);
            this.comboBox_basePrn.Name = "comboBox_basePrn";
            this.comboBox_basePrn.Size = new System.Drawing.Size(87, 20);
            this.comboBox_basePrn.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "基准卫星：";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1094, 549);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextBoxControl1);
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(983, 489);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "单历元服务";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl1
            // 
            this.richTextBoxControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl1.Location = new System.Drawing.Point(3, 52);
            this.richTextBoxControl1.MaxAppendLineCount = 5000;
            this.richTextBoxControl1.Name = "richTextBoxControl1";
            this.richTextBoxControl1.Size = new System.Drawing.Size(977, 434);
            this.richTextBoxControl1.TabIndex = 5;
            this.richTextBoxControl1.Text = "";
            // 
            // namedTimeControl1
            // 
            this.namedTimeControl1.Location = new System.Drawing.Point(3, 13);
            this.namedTimeControl1.Name = "namedTimeControl1";
            this.namedTimeControl1.Size = new System.Drawing.Size(217, 23);
            this.namedTimeControl1.TabIndex = 4;
            this.namedTimeControl1.Title = "历元：";
            // 
            // button_vewSingleEpoch
            // 
            this.button_vewSingleEpoch.Location = new System.Drawing.Point(233, 13);
            this.button_vewSingleEpoch.Name = "button_vewSingleEpoch";
            this.button_vewSingleEpoch.Size = new System.Drawing.Size(127, 23);
            this.button_vewSingleEpoch.TabIndex = 1;
            this.button_vewSingleEpoch.Text = "查看窄巷";
            this.button_vewSingleEpoch.UseVisualStyleBackColor = true;
            this.button_vewSingleEpoch.Click += new System.EventHandler(this.button_vewSingleEpoch_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.objectTableControl1);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1086, 523);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "多历元服务";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button_multiShow
            // 
            this.button_multiShow.Location = new System.Drawing.Point(421, 12);
            this.button_multiShow.Name = "button_multiShow";
            this.button_multiShow.Size = new System.Drawing.Size(107, 23);
            this.button_multiShow.TabIndex = 5;
            this.button_multiShow.Text = "多历元查看窄巷";
            this.button_multiShow.UseVisualStyleBackColor = true;
            this.button_multiShow.Click += new System.EventHandler(this.button_multiShow_Click);
            // 
            // timePeriodControl1
            // 
            this.timePeriodControl1.Location = new System.Drawing.Point(2, 12);
            this.timePeriodControl1.Margin = new System.Windows.Forms.Padding(2);
            this.timePeriodControl1.Name = "timePeriodControl1";
            this.timePeriodControl1.Size = new System.Drawing.Size(414, 24);
            this.timePeriodControl1.TabIndex = 4;
            this.timePeriodControl1.TimeFrom = new System.DateTime(2018, 9, 17, 17, 31, 58, 314);
            this.timePeriodControl1.TimeStringFormat = "yyyy-MM-dd HH:mm:ss";
            this.timePeriodControl1.TimeTo = new System.DateTime(2018, 9, 17, 17, 31, 58, 324);
            this.timePeriodControl1.Title = "时段：";
            // 
            // objectTableControl1
            // 
            this.objectTableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl1.Location = new System.Drawing.Point(3, 50);
            this.objectTableControl1.Name = "objectTableControl1";
            this.objectTableControl1.Size = new System.Drawing.Size(1080, 470);
            this.objectTableControl1.TabIndex = 3;
            this.objectTableControl1.TableObjectStorage = null;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_saveWLToGNSSer);
            this.panel1.Controls.Add(this.button_multiWLView);
            this.panel1.Controls.Add(this.button_saveToGnsserFcb);
            this.panel1.Controls.Add(this.timePeriodControl1);
            this.panel1.Controls.Add(this.button_multiShow);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1080, 47);
            this.panel1.TabIndex = 6;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button_viewSingleWL);
            this.panel2.Controls.Add(this.namedTimeControl1);
            this.panel2.Controls.Add(this.button_vewSingleEpoch);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(977, 49);
            this.panel2.TabIndex = 6;
            // 
            // button_saveToGnsserFcb
            // 
            this.button_saveToGnsserFcb.Location = new System.Drawing.Point(681, 13);
            this.button_saveToGnsserFcb.Name = "button_saveToGnsserFcb";
            this.button_saveToGnsserFcb.Size = new System.Drawing.Size(166, 23);
            this.button_saveToGnsserFcb.TabIndex = 6;
            this.button_saveToGnsserFcb.Text = "保存窄巷为GNSSerFCB文件";
            this.button_saveToGnsserFcb.UseVisualStyleBackColor = true;
            this.button_saveToGnsserFcb.Click += new System.EventHandler(this.button_saveToGnsserFcb_Click);
            // 
            // button_viewSingleWL
            // 
            this.button_viewSingleWL.Location = new System.Drawing.Point(392, 12);
            this.button_viewSingleWL.Name = "button_viewSingleWL";
            this.button_viewSingleWL.Size = new System.Drawing.Size(129, 23);
            this.button_viewSingleWL.TabIndex = 5;
            this.button_viewSingleWL.Text = "查看宽巷";
            this.button_viewSingleWL.UseVisualStyleBackColor = true;
            this.button_viewSingleWL.Click += new System.EventHandler(this.button_viewSingleWL_Click);
            // 
            // button_multiWLView
            // 
            this.button_multiWLView.Location = new System.Drawing.Point(535, 12);
            this.button_multiWLView.Name = "button_multiWLView";
            this.button_multiWLView.Size = new System.Drawing.Size(131, 23);
            this.button_multiWLView.TabIndex = 7;
            this.button_multiWLView.Text = "多历元查看宽巷";
            this.button_multiWLView.UseVisualStyleBackColor = true;
            this.button_multiWLView.Click += new System.EventHandler(this.button_multiWLView_Click);
            // 
            // button_saveWLToGNSSer
            // 
            this.button_saveWLToGNSSer.Location = new System.Drawing.Point(854, 13);
            this.button_saveWLToGNSSer.Name = "button_saveWLToGNSSer";
            this.button_saveWLToGNSSer.Size = new System.Drawing.Size(171, 23);
            this.button_saveWLToGNSSer.TabIndex = 8;
            this.button_saveWLToGNSSer.Text = "保存宽巷为GNSSerFCB文件";
            this.button_saveWLToGNSSer.UseVisualStyleBackColor = true;
            this.button_saveWLToGNSSer.Click += new System.EventHandler(this.button_saveWLToGNSSer_Click);
            // 
            // WhuFcbServiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1094, 631);
            this.Controls.Add(this.splitContainer1);
            this.Name = "WhuFcbServiceForm";
            this.Text = "武大FCB服务";
            this.Load += new System.EventHandler(this.IgsFcbViewerForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_basePrn)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Geo.Winform.ObjectTableControl objectTableControl1;
        private System.Windows.Forms.Button button_vewSingleEpoch;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ComboBox comboBox_basePrn;
        private System.Windows.Forms.Label label2;
        private Geo.Winform.Controls.NamedTimeControl namedTimeControl1;
        private System.Windows.Forms.BindingSource bindingSource_basePrn;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl1;
        private System.Windows.Forms.Button button_multiShow;
        private Geo.Winform.Controls.TimePeriodControl timePeriodControl1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_saveToGnsserFcb;
        private System.Windows.Forms.Button button_viewSingleWL;
        private System.Windows.Forms.Button button_multiWLView;
        private System.Windows.Forms.Button button_saveWLToGNSSer;
    }
}