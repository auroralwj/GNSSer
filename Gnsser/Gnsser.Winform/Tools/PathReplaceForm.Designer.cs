namespace Gnsser.Winform
{
    partial class PathReplaceForm
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
            this.fileOpenControl_pathes = new Geo.Winform.Controls.FileOpenControl();
            this.namedStringControl_key = new Geo.Winform.Controls.NamedStringControl();
            this.button_ok = new System.Windows.Forms.Button();
            this.namedStringControl_extension = new Geo.Winform.Controls.NamedStringControl();
            this.enumRadioControl_type = new Geo.Winform.EnumRadioControl();
            this.SuspendLayout();
            // 
            // fileOpenControl_pathes
            // 
            this.fileOpenControl_pathes.AllowDrop = true;
            this.fileOpenControl_pathes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_pathes.FilePath = "";
            this.fileOpenControl_pathes.FilePathes = new string[0];
            this.fileOpenControl_pathes.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_pathes.FirstPath = "";
            this.fileOpenControl_pathes.IsMultiSelect = true;
            this.fileOpenControl_pathes.LabelName = "文件：";
            this.fileOpenControl_pathes.Location = new System.Drawing.Point(12, 12);
            this.fileOpenControl_pathes.Name = "fileOpenControl_pathes";
            this.fileOpenControl_pathes.Size = new System.Drawing.Size(850, 116);
            this.fileOpenControl_pathes.TabIndex = 0;
            // 
            // namedStringControl_key
            // 
            this.namedStringControl_key.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.namedStringControl_key.Location = new System.Drawing.Point(12, 151);
            this.namedStringControl_key.Name = "namedStringControl_key";
            this.namedStringControl_key.Size = new System.Drawing.Size(217, 23);
            this.namedStringControl_key.TabIndex = 1;
            this.namedStringControl_key.Title = "关键字：";
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ok.Location = new System.Drawing.Point(754, 189);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(93, 34);
            this.button_ok.TabIndex = 2;
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // namedStringControl_extension
            // 
            this.namedStringControl_extension.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.namedStringControl_extension.Location = new System.Drawing.Point(12, 180);
            this.namedStringControl_extension.Name = "namedStringControl_extension";
            this.namedStringControl_extension.Size = new System.Drawing.Size(204, 23);
            this.namedStringControl_extension.TabIndex = 1;
            this.namedStringControl_extension.Title = "后缀名称：";
            // 
            // enumRadioControl_type
            // 
            this.enumRadioControl_type.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.enumRadioControl_type.Location = new System.Drawing.Point(300, 151);
            this.enumRadioControl_type.Name = "enumRadioControl_type";
            this.enumRadioControl_type.Size = new System.Drawing.Size(377, 59);
            this.enumRadioControl_type.TabIndex = 3;
            this.enumRadioControl_type.Title = "选项";
            // 
            // PathReplaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 236);
            this.Controls.Add(this.enumRadioControl_type);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.namedStringControl_extension);
            this.Controls.Add(this.namedStringControl_key);
            this.Controls.Add(this.fileOpenControl_pathes);
            this.Name = "PathReplaceForm";
            this.Text = "PathReplaceForm";
            this.ResumeLayout(false);

        }

        #endregion

        private Geo.Winform.Controls.FileOpenControl fileOpenControl_pathes;
        private Geo.Winform.Controls.NamedStringControl namedStringControl_key;
        private System.Windows.Forms.Button button_ok;
        private Geo.Winform.Controls.NamedStringControl namedStringControl_extension;
        private Geo.Winform.EnumRadioControl enumRadioControl_type;
    }
}