using System;

namespace Gnsser.Ntrip.WinForms
{
    partial class SourceTableForm
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
            this.button_openAndView = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_ok = new System.Windows.Forms.Button();
            this.fileOpenControl1 = new Geo.Winform.Controls.FileOpenControl();
            this.enumRadioControl1 = new Geo.Winform.EnumRadioControl();
            this.simpleTableControl1 = new Geo.Winform.SimpleTableControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.button_ok);
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel1.Controls.Add(this.enumRadioControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.simpleTableControl1);
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(557, 391);
            this.splitContainer1.SplitterDistance = 112;
            this.splitContainer1.TabIndex = 0;
            // 
            // button_openAndView
            // 
            this.button_openAndView.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_openAndView.Location = new System.Drawing.Point(450, 3);
            this.button_openAndView.Name = "button_openAndView";
            this.button_openAndView.Size = new System.Drawing.Size(81, 26);
            this.button_openAndView.TabIndex = 1;
            this.button_openAndView.Text = "读取并查看";
            this.button_openAndView.UseVisualStyleBackColor = true;
            this.button_openAndView.Click += new System.EventHandler(this.button_openAndView_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(557, 275);
            this.dataGridView1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(542, 58);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.comboBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(534, 32);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "选择数据源";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.fileOpenControl1);
            this.tabPage2.Controls.Add(this.button_openAndView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(534, 32);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "手动查看本地数据源";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(66, 6);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(215, 20);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "数据源：";
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ok.Location = new System.Drawing.Point(470, 86);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 4;
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl1.AllowDrop = true;
            this.fileOpenControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileOpenControl1.FilePath = "";
            this.fileOpenControl1.FilePathes = new string[0];
            this.fileOpenControl1.Filter = "数据文件dat|*.dat|文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl1.IsMultiSelect = false;
            this.fileOpenControl1.LabelName = "文件：";
            this.fileOpenControl1.Location = new System.Drawing.Point(3, 3);
            this.fileOpenControl1.Name = "fileOpenControl1";
            this.fileOpenControl1.Size = new System.Drawing.Size(447, 26);
            this.fileOpenControl1.TabIndex = 0;
            // 
            // enumRadioControl1
            // 
            this.enumRadioControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.enumRadioControl1.Location = new System.Drawing.Point(12, 67);
            this.enumRadioControl1.Name = "enumRadioControl1";
            this.enumRadioControl1.Size = new System.Drawing.Size(317, 42);
            this.enumRadioControl1.TabIndex = 2;
            this.enumRadioControl1.Title = "选项";
            this.enumRadioControl1.EnumItemSelected += new Action<string, bool>(this.enumRadioControl1_EnumItemSelected);
            // 
            // simpleTableControl1
            // 
            this.simpleTableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simpleTableControl1.EnableExport = false;
            this.simpleTableControl1.HiddenColumns = null;
            this.simpleTableControl1.IsShowTitle = true;
            this.simpleTableControl1.Location = new System.Drawing.Point(0, 0);
            this.simpleTableControl1.Name = "simpleTableControl1";
            this.simpleTableControl1.ShowToolStrip = true;
            this.simpleTableControl1.Size = new System.Drawing.Size(557, 275);
            this.simpleTableControl1.TabIndex = 1;
            this.simpleTableControl1.Title = "导航";
            // 
            // SourceTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 391);
            this.Controls.Add(this.splitContainer1);
            this.Name = "SourceTableForm";
            this.Text = "数据源表查看器";
            this.Load += new System.EventHandler(this.SourceTableForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button_openAndView;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl1;
        private Geo.Winform.EnumRadioControl enumRadioControl1;
        private Geo.Winform.SimpleTableControl simpleTableControl1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button_ok;
    }
}