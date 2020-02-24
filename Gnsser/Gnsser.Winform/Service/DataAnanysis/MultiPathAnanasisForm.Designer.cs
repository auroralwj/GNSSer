namespace Gnsser.Winform
{
    partial class MultiPathAnanasisForm
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
            this.fileOpenControl1_fiels = new Geo.Winform.Controls.FileOpenControl();
            this.button_run = new System.Windows.Forms.Button();
            this.namedFloatControl_angleCut = new Geo.Winform.Controls.NamedFloatControl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.objectTableControl1 = new Geo.Winform.ObjectTableControl();
            this.gnssSystemSelectControl1 = new Gnsser.Winform.Controls.GnssSystemSelectControl();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileOpenControl1_fiels
            // 
            this.fileOpenControl1_fiels.AllowDrop = true;
            this.fileOpenControl1_fiels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl1_fiels.FilePath = "";
            this.fileOpenControl1_fiels.FilePathes = new string[0];
            this.fileOpenControl1_fiels.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl1_fiels.FirstPath = "";
            this.fileOpenControl1_fiels.IsMultiSelect = true;
            this.fileOpenControl1_fiels.LabelName = "文件：";
            this.fileOpenControl1_fiels.Location = new System.Drawing.Point(12, 12);
            this.fileOpenControl1_fiels.Name = "fileOpenControl1_fiels";
            this.fileOpenControl1_fiels.Size = new System.Drawing.Size(602, 64);
            this.fileOpenControl1_fiels.TabIndex = 0;
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(539, 82);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 31);
            this.button_run.TabIndex = 2;
            this.button_run.Text = "运行";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // namedFloatControl_angleCut
            // 
            this.namedFloatControl_angleCut.Location = new System.Drawing.Point(295, 90);
            this.namedFloatControl_angleCut.Name = "namedFloatControl_angleCut";
            this.namedFloatControl_angleCut.Size = new System.Drawing.Size(160, 23);
            this.namedFloatControl_angleCut.TabIndex = 5;
            this.namedFloatControl_angleCut.Title = "高度截止角(度)：";
            this.namedFloatControl_angleCut.Value = 15D;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(12, 128);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(602, 285);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.objectTableControl1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(594, 259);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "结果";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // objectTableControl1
            // 
            this.objectTableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl1.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl1.Name = "objectTableControl1";
            this.objectTableControl1.Size = new System.Drawing.Size(588, 253);
            this.objectTableControl1.TabIndex = 0;
            this.objectTableControl1.TableObjectStorage = null;
            // 
            // gnssSystemSelectControl1
            // 
            this.gnssSystemSelectControl1.Location = new System.Drawing.Point(11, 81);
            this.gnssSystemSelectControl1.Margin = new System.Windows.Forms.Padding(2);
            this.gnssSystemSelectControl1.Name = "gnssSystemSelectControl1";
            this.gnssSystemSelectControl1.Size = new System.Drawing.Size(279, 40);
            this.gnssSystemSelectControl1.TabIndex = 9;
            // 
            // MultiPathAnanasisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 413);
            this.Controls.Add(this.gnssSystemSelectControl1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.namedFloatControl_angleCut);
            this.Controls.Add(this.button_run);
            this.Controls.Add(this.fileOpenControl1_fiels);
            this.Name = "MultiPathAnanasisForm";
            this.Text = "多路径效应分析";
            this.Load += new System.EventHandler(this.ObsPeriodDividerForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Geo.Winform.Controls.FileOpenControl fileOpenControl1_fiels;
        private System.Windows.Forms.Button button_run;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_angleCut;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Geo.Winform.ObjectTableControl objectTableControl1;
        private Controls.GnssSystemSelectControl gnssSystemSelectControl1;
    }
}