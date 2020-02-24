namespace Gnsser.Winform
{
    partial class AmbiguityCombineerForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.fileOpenControlB = new Geo.Winform.Controls.FileOpenControl();
            this.fileOpenControlA = new Geo.Winform.Controls.FileOpenControl();
            this.button_combine = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.objectTableControlA = new Geo.Winform.ObjectTableControl();
            this.objectTableControlB = new Geo.Winform.ObjectTableControl();
            this.objectTableControlC = new Geo.Winform.ObjectTableControl();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.fileOpenControlB);
            this.groupBox1.Controls.Add(this.fileOpenControlA);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(791, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息";
            // 
            // fileOpenControlB
            // 
            this.fileOpenControlB.AllowDrop = true;
            this.fileOpenControlB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControlB.FilePath = "";
            this.fileOpenControlB.FilePathes = new string[0];
            this.fileOpenControlB.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControlB.FirstPath = "";
            this.fileOpenControlB.IsMultiSelect = false;
            this.fileOpenControlB.LabelName = "模糊度文件B：";
            this.fileOpenControlB.Location = new System.Drawing.Point(7, 61);
            this.fileOpenControlB.Name = "fileOpenControlB";
            this.fileOpenControlB.Size = new System.Drawing.Size(747, 22);
            this.fileOpenControlB.TabIndex = 0;
            // 
            // fileOpenControlA
            // 
            this.fileOpenControlA.AllowDrop = true;
            this.fileOpenControlA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControlA.FilePath = "";
            this.fileOpenControlA.FilePathes = new string[0];
            this.fileOpenControlA.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControlA.FirstPath = "";
            this.fileOpenControlA.IsMultiSelect = false;
            this.fileOpenControlA.LabelName = "模糊度文件A：";
            this.fileOpenControlA.Location = new System.Drawing.Point(7, 21);
            this.fileOpenControlA.Name = "fileOpenControlA";
            this.fileOpenControlA.Size = new System.Drawing.Size(747, 22);
            this.fileOpenControlA.TabIndex = 0;
            // 
            // button_combine
            // 
            this.button_combine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_combine.Location = new System.Drawing.Point(704, 123);
            this.button_combine.Name = "button_combine";
            this.button_combine.Size = new System.Drawing.Size(75, 23);
            this.button_combine.TabIndex = 1;
            this.button_combine.Text = "合成";
            this.button_combine.UseVisualStyleBackColor = true;
            this.button_combine.Click += new System.EventHandler(this.button_combine_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(13, 150);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(802, 404);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(794, 378);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "结果";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.objectTableControlC);
            this.splitContainer1.Size = new System.Drawing.Size(788, 372);
            this.splitContainer1.SplitterDistance = 188;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.objectTableControlA);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.objectTableControlB);
            this.splitContainer2.Size = new System.Drawing.Size(788, 188);
            this.splitContainer2.SplitterDistance = 376;
            this.splitContainer2.TabIndex = 0;
            // 
            // objectTableControlA
            // 
            this.objectTableControlA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControlA.Location = new System.Drawing.Point(0, 0);
            this.objectTableControlA.Name = "objectTableControlA";
            this.objectTableControlA.Size = new System.Drawing.Size(376, 188);
            this.objectTableControlA.TabIndex = 0;
            this.objectTableControlA.TableObjectStorage = null;
            // 
            // objectTableControlB
            // 
            this.objectTableControlB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControlB.Location = new System.Drawing.Point(0, 0);
            this.objectTableControlB.Name = "objectTableControlB";
            this.objectTableControlB.Size = new System.Drawing.Size(408, 188);
            this.objectTableControlB.TabIndex = 1;
            this.objectTableControlB.TableObjectStorage = null;
            // 
            // objectTableControlC
            // 
            this.objectTableControlC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControlC.Location = new System.Drawing.Point(0, 0);
            this.objectTableControlC.Name = "objectTableControlC";
            this.objectTableControlC.Size = new System.Drawing.Size(788, 180);
            this.objectTableControlC.TabIndex = 1;
            this.objectTableControlC.TableObjectStorage = null;
            // 
            // AmbiguityCombineerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 554);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button_combine);
            this.Controls.Add(this.groupBox1);
            this.Name = "AmbiguityCombineerForm";
            this.Text = "AmbiguityCombineerForm";
            this.Load += new System.EventHandler(this.AmbiguityCombineerForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Geo.Winform.Controls.FileOpenControl fileOpenControlA;
        private Geo.Winform.Controls.FileOpenControl fileOpenControlB;
        private System.Windows.Forms.Button button_combine;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Geo.Winform.ObjectTableControl objectTableControlA;
        private Geo.Winform.ObjectTableControl objectTableControlB;
        private Geo.Winform.ObjectTableControl objectTableControlC;
    }
}