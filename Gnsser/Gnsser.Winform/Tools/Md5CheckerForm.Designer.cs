namespace Gnsser.Winform
{
    partial class Md5CheckerForm
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
            this.button_check = new System.Windows.Forms.Button();
            this.tabControl_input = new System.Windows.Forms.TabControl();
            this.tabPage_inputMd5Text = new System.Windows.Forms.TabPage();
            this.tabPage_inputMd5File = new System.Windows.Forms.TabPage();
            this.fileOpenControl1_filePath = new Geo.Winform.Controls.FileOpenControl();
            this.richTextBoxControl_inputText = new Geo.Winform.Controls.RichTextBoxControl();
            this.fileOpenControl1_md5File = new Geo.Winform.Controls.FileOpenControl();
            this.richTextBoxControl_md5Result = new Geo.Winform.Controls.RichTextBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl_input.SuspendLayout();
            this.tabPage_inputMd5Text.SuspendLayout();
            this.tabPage_inputMd5File.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.fileOpenControl1_filePath);
            this.splitContainer1.Panel1.Controls.Add(this.tabControl_input);
            this.splitContainer1.Panel1.Controls.Add(this.button_check);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(760, 460);
            this.splitContainer1.SplitterDistance = 199;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(760, 257);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextBoxControl_md5Result);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(752, 231);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "MD5结果";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button_check
            // 
            this.button_check.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_check.Location = new System.Drawing.Point(659, 162);
            this.button_check.Name = "button_check";
            this.button_check.Size = new System.Drawing.Size(89, 34);
            this.button_check.TabIndex = 0;
            this.button_check.Text = "校验";
            this.button_check.UseVisualStyleBackColor = true;
            this.button_check.Click += new System.EventHandler(this.button_check_Click);
            // 
            // tabControl_input
            // 
            this.tabControl_input.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_input.Controls.Add(this.tabPage_inputMd5Text);
            this.tabControl_input.Controls.Add(this.tabPage_inputMd5File);
            this.tabControl_input.Location = new System.Drawing.Point(9, 41);
            this.tabControl_input.Name = "tabControl_input";
            this.tabControl_input.SelectedIndex = 0;
            this.tabControl_input.Size = new System.Drawing.Size(747, 115);
            this.tabControl_input.TabIndex = 1;
            // 
            // tabPage_inputMd5Text
            // 
            this.tabPage_inputMd5Text.Controls.Add(this.richTextBoxControl_inputText);
            this.tabPage_inputMd5Text.Location = new System.Drawing.Point(4, 22);
            this.tabPage_inputMd5Text.Name = "tabPage_inputMd5Text";
            this.tabPage_inputMd5Text.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_inputMd5Text.Size = new System.Drawing.Size(739, 89);
            this.tabPage_inputMd5Text.TabIndex = 0;
            this.tabPage_inputMd5Text.Text = "校验文本";
            this.tabPage_inputMd5Text.UseVisualStyleBackColor = true;
            // 
            // tabPage_inputMd5File
            // 
            this.tabPage_inputMd5File.Controls.Add(this.fileOpenControl1_md5File);
            this.tabPage_inputMd5File.Location = new System.Drawing.Point(4, 22);
            this.tabPage_inputMd5File.Name = "tabPage_inputMd5File";
            this.tabPage_inputMd5File.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_inputMd5File.Size = new System.Drawing.Size(739, 89);
            this.tabPage_inputMd5File.TabIndex = 1;
            this.tabPage_inputMd5File.Text = "载入文件";
            this.tabPage_inputMd5File.UseVisualStyleBackColor = true;
            // 
            // fileOpenControl1_filePath
            // 
            this.fileOpenControl1_filePath.AllowDrop = true;
            this.fileOpenControl1_filePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl1_filePath.FilePath = "";
            this.fileOpenControl1_filePath.FilePathes = new string[0];
            this.fileOpenControl1_filePath.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl1_filePath.FirstPath = "";
            this.fileOpenControl1_filePath.IsMultiSelect = false;
            this.fileOpenControl1_filePath.LabelName = "文件：";
            this.fileOpenControl1_filePath.Location = new System.Drawing.Point(9, 13);
            this.fileOpenControl1_filePath.Name = "fileOpenControl1_filePath";
            this.fileOpenControl1_filePath.Size = new System.Drawing.Size(739, 22);
            this.fileOpenControl1_filePath.TabIndex = 2;
            // 
            // richTextBoxControl_inputText
            // 
            this.richTextBoxControl_inputText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_inputText.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_inputText.MaxAppendLineCount = 5000;
            this.richTextBoxControl_inputText.Name = "richTextBoxControl_inputText";
            this.richTextBoxControl_inputText.Size = new System.Drawing.Size(733, 83);
            this.richTextBoxControl_inputText.TabIndex = 0;
            this.richTextBoxControl_inputText.Text = "";
            // 
            // fileOpenControl1_md5File
            // 
            this.fileOpenControl1_md5File.AllowDrop = true;
            this.fileOpenControl1_md5File.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl1_md5File.FilePath = "";
            this.fileOpenControl1_md5File.FilePathes = new string[0];
            this.fileOpenControl1_md5File.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl1_md5File.FirstPath = "";
            this.fileOpenControl1_md5File.IsMultiSelect = false;
            this.fileOpenControl1_md5File.LabelName = "文件：";
            this.fileOpenControl1_md5File.Location = new System.Drawing.Point(6, 6);
            this.fileOpenControl1_md5File.Name = "fileOpenControl1_md5File";
            this.fileOpenControl1_md5File.Size = new System.Drawing.Size(718, 22);
            this.fileOpenControl1_md5File.TabIndex = 2;
            // 
            // richTextBoxControl_md5Result
            // 
            this.richTextBoxControl_md5Result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_md5Result.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_md5Result.MaxAppendLineCount = 5000;
            this.richTextBoxControl_md5Result.Name = "richTextBoxControl_md5Result";
            this.richTextBoxControl_md5Result.Size = new System.Drawing.Size(746, 225);
            this.richTextBoxControl_md5Result.TabIndex = 0;
            this.richTextBoxControl_md5Result.Text = "";
            // 
            // Md5CheckerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 460);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Md5CheckerForm";
            this.Text = "MD5校验";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabControl_input.ResumeLayout(false);
            this.tabPage_inputMd5Text.ResumeLayout(false);
            this.tabPage_inputMd5File.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button button_check;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControl_input;
        private System.Windows.Forms.TabPage tabPage_inputMd5Text;
        private System.Windows.Forms.TabPage tabPage_inputMd5File;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl1_filePath;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl_inputText;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl1_md5File;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl_md5Result;
    }
}