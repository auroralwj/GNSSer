namespace Geo.Winform.Controls
{
    partial class FileOpenControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label_fileName = new System.Windows.Forms.Label();
            this.textBox_filepath = new System.Windows.Forms.TextBox();
            this.button_setPath = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button_open = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_fileName
            // 
            this.label_fileName.AutoSize = true;
            this.label_fileName.Dock = System.Windows.Forms.DockStyle.Left;
            this.label_fileName.Location = new System.Drawing.Point(0, 0);
            this.label_fileName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_fileName.Name = "label_fileName";
            this.label_fileName.Size = new System.Drawing.Size(52, 15);
            this.label_fileName.TabIndex = 0;
            this.label_fileName.Text = "文件：";
            // 
            // textBox_filepath
            // 
            this.textBox_filepath.AllowDrop = true;
            this.textBox_filepath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_filepath.Location = new System.Drawing.Point(52, 0);
            this.textBox_filepath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_filepath.Name = "textBox_filepath";
            this.textBox_filepath.Size = new System.Drawing.Size(410, 25);
            this.textBox_filepath.TabIndex = 1;
            this.textBox_filepath.TextChanged += new System.EventHandler(this.textBox_filepath_TextChanged);
            this.textBox_filepath.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form_DragDrop);
            this.textBox_filepath.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form_DragEnter);
            // 
            // button_setPath
            // 
            this.button_setPath.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_setPath.Location = new System.Drawing.Point(462, 0);
            this.button_setPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_setPath.Name = "button_setPath";
            this.button_setPath.Size = new System.Drawing.Size(47, 28);
            this.button_setPath.TabIndex = 2;
            this.button_setPath.Text = "...";
            this.button_setPath.UseVisualStyleBackColor = true;
            this.button_setPath.Click += new System.EventHandler(this.button_setPath_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "文本文件|*.txt|所有文件|*.*";
            // 
            // button_open
            // 
            this.button_open.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_open.Location = new System.Drawing.Point(509, 0);
            this.button_open.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_open.Name = "button_open";
            this.button_open.Size = new System.Drawing.Size(63, 28);
            this.button_open.TabIndex = 3;
            this.button_open.Text = "打开";
            this.button_open.UseVisualStyleBackColor = true;
            this.button_open.Click += new System.EventHandler(this.button_open_Click);
            // 
            // FileOpenControl
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox_filepath);
            this.Controls.Add(this.button_setPath);
            this.Controls.Add(this.label_fileName);
            this.Controls.Add(this.button_open);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FileOpenControl";
            this.Size = new System.Drawing.Size(572, 28);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FileOpenControl_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FileOpenControl_DragEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_fileName;
        private System.Windows.Forms.TextBox textBox_filepath;
        private System.Windows.Forms.Button button_setPath;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_open;
    }
}
