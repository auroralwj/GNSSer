namespace Geo.Winform
{
    partial class MatrixViewerForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.fileOpenControl1 = new Geo.Winform.Controls.FileOpenControl();
            this.button_read = new System.Windows.Forms.Button();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.namedIntControl_row = new Geo.Winform.Controls.NamedIntControl();
            this.namedIntControl_col = new Geo.Winform.Controls.NamedIntControl();
            this.button_randomGen = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button_saveAsBinary = new System.Windows.Forms.Button();
            this.button_saveAsText = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl1 = new Geo.Winform.Controls.RichTextBoxControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.objectTableControl1 = new Geo.Winform.ObjectTableControl();
            this.namedStringControl_prefName = new Geo.Winform.Controls.NamedStringControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 112;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 112);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tabControl3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 86);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "输入";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage5);
            this.tabControl3.Controls.Add(this.tabPage6);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(3, 3);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(786, 80);
            this.tabControl3.TabIndex = 1;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.fileOpenControl1);
            this.tabPage5.Controls.Add(this.button_read);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(778, 54);
            this.tabPage5.TabIndex = 0;
            this.tabPage5.Text = "文件读取";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl1.AllowDrop = true;
            this.fileOpenControl1.FilePath = "";
            this.fileOpenControl1.FilePathes = new string[0];
            this.fileOpenControl1.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl1.FirstPath = "";
            this.fileOpenControl1.IsMultiSelect = false;
            this.fileOpenControl1.LabelName = "文件路径：";
            this.fileOpenControl1.Location = new System.Drawing.Point(3, 6);
            this.fileOpenControl1.Name = "fileOpenControl1";
            this.fileOpenControl1.Size = new System.Drawing.Size(685, 22);
            this.fileOpenControl1.TabIndex = 0;
            // 
            // button_read
            // 
            this.button_read.Location = new System.Drawing.Point(700, 6);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(75, 23);
            this.button_read.TabIndex = 1;
            this.button_read.Text = "读取";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_read_Click);
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.namedStringControl_prefName);
            this.tabPage6.Controls.Add(this.namedIntControl_row);
            this.tabPage6.Controls.Add(this.namedIntControl_col);
            this.tabPage6.Controls.Add(this.button_randomGen);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(778, 54);
            this.tabPage6.TabIndex = 1;
            this.tabPage6.Text = "随机生成";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // namedIntControl_row
            // 
            this.namedIntControl_row.Location = new System.Drawing.Point(6, 6);
            this.namedIntControl_row.Name = "namedIntControl_row";
            this.namedIntControl_row.Size = new System.Drawing.Size(119, 23);
            this.namedIntControl_row.TabIndex = 2;
            this.namedIntControl_row.Title = "行数：";
            this.namedIntControl_row.Value = 100;
            // 
            // namedIntControl_col
            // 
            this.namedIntControl_col.Location = new System.Drawing.Point(146, 6);
            this.namedIntControl_col.Name = "namedIntControl_col";
            this.namedIntControl_col.Size = new System.Drawing.Size(119, 23);
            this.namedIntControl_col.TabIndex = 1;
            this.namedIntControl_col.Title = "列数：";
            this.namedIntControl_col.Value = 100;
            // 
            // button_randomGen
            // 
            this.button_randomGen.Location = new System.Drawing.Point(396, 6);
            this.button_randomGen.Name = "button_randomGen";
            this.button_randomGen.Size = new System.Drawing.Size(75, 23);
            this.button_randomGen.TabIndex = 0;
            this.button_randomGen.Text = "生成";
            this.button_randomGen.UseVisualStyleBackColor = true;
            this.button_randomGen.Click += new System.EventHandler(this.button_randomGen_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button_saveAsBinary);
            this.tabPage2.Controls.Add(this.button_saveAsText);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 86);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "输出";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button_saveAsBinary
            // 
            this.button_saveAsBinary.Location = new System.Drawing.Point(92, 15);
            this.button_saveAsBinary.Name = "button_saveAsBinary";
            this.button_saveAsBinary.Size = new System.Drawing.Size(88, 23);
            this.button_saveAsBinary.TabIndex = 2;
            this.button_saveAsBinary.Text = "另存为二进制";
            this.button_saveAsBinary.UseVisualStyleBackColor = true;
            this.button_saveAsBinary.Click += new System.EventHandler(this.button_saveAsBinary_Click);
            // 
            // button_saveAsText
            // 
            this.button_saveAsText.Location = new System.Drawing.Point(11, 14);
            this.button_saveAsText.Name = "button_saveAsText";
            this.button_saveAsText.Size = new System.Drawing.Size(75, 23);
            this.button_saveAsText.TabIndex = 2;
            this.button_saveAsText.Text = "另存为文本";
            this.button_saveAsText.UseVisualStyleBackColor = true;
            this.button_saveAsText.Click += new System.EventHandler(this.button_saveAsText_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControl2);
            this.splitContainer2.Size = new System.Drawing.Size(800, 334);
            this.splitContainer2.SplitterDistance = 55;
            this.splitContainer2.TabIndex = 0;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(800, 275);
            this.tabControl2.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.richTextBoxControl1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(792, 249);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "文本";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl1
            // 
            this.richTextBoxControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl1.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl1.MaxAppendLineCount = 5000;
            this.richTextBoxControl1.Name = "richTextBoxControl1";
            this.richTextBoxControl1.Size = new System.Drawing.Size(786, 243);
            this.richTextBoxControl1.TabIndex = 0;
            this.richTextBoxControl1.Text = "";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.objectTableControl1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(792, 249);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "表格";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // objectTableControl1
            // 
            this.objectTableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl1.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl1.Name = "objectTableControl1";
            this.objectTableControl1.Size = new System.Drawing.Size(786, 243);
            this.objectTableControl1.TabIndex = 0;
            this.objectTableControl1.TableObjectStorage = null;
            // 
            // namedStringControl_prefName
            // 
            this.namedStringControl_prefName.Location = new System.Drawing.Point(7, 30);
            this.namedStringControl_prefName.Name = "namedStringControl_prefName";
            this.namedStringControl_prefName.Size = new System.Drawing.Size(217, 23);
            this.namedStringControl_prefName.TabIndex = 3;
            this.namedStringControl_prefName.Title = "前缀名称：";
            // 
            // MatrixViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MatrixViewerForm";
            this.Text = "MatrixViewerForm";
            this.Load += new System.EventHandler(this.MatrixViewerForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button button_randomGen;
        private Controls.FileOpenControl fileOpenControl1;
        private ObjectTableControl objectTableControl1;
        private Controls.RichTextBoxControl richTextBoxControl1;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private Controls.NamedIntControl namedIntControl_row;
        private Controls.NamedIntControl namedIntControl_col;
        private System.Windows.Forms.Button button_saveAsBinary;
        private System.Windows.Forms.Button button_saveAsText;
        private System.Windows.Forms.Button button_read;
        private Controls.NamedStringControl namedStringControl_prefName;
    }
}