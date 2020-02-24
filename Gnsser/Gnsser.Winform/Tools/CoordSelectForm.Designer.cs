namespace Gnsser.Winform
{
    partial class CoordSelectForm
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
            this.namedStringControl_coord = new Geo.Winform.Controls.NamedStringControl();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.fileOpenControl_extractFromOFile = new Geo.Winform.Controls.FileOpenControl();
            this.comboBox_coordSource = new System.Windows.Forms.ComboBox();
            this.bindingSource_coord = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.button_setCoordFromFile = new System.Windows.Forms.Button();
            this.label_siteInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_coord)).BeginInit();
            this.SuspendLayout();
            // 
            // namedStringControl_coord
            // 
            this.namedStringControl_coord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.namedStringControl_coord.Location = new System.Drawing.Point(12, 152);
            this.namedStringControl_coord.Name = "namedStringControl_coord";
            this.namedStringControl_coord.Size = new System.Drawing.Size(357, 23);
            this.namedStringControl_coord.TabIndex = 0;
            this.namedStringControl_coord.Title = "已选坐标：";
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(468, 145);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 30);
            this.button_cancel.TabIndex = 1;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ok.Location = new System.Drawing.Point(387, 145);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 30);
            this.button_ok.TabIndex = 1;
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // fileOpenControl_extractFromOFile
            // 
            this.fileOpenControl_extractFromOFile.AllowDrop = true;
            this.fileOpenControl_extractFromOFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_extractFromOFile.FilePath = "";
            this.fileOpenControl_extractFromOFile.FilePathes = new string[0];
            this.fileOpenControl_extractFromOFile.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_extractFromOFile.FirstPath = "";
            this.fileOpenControl_extractFromOFile.IsMultiSelect = false;
            this.fileOpenControl_extractFromOFile.LabelName = "从观测文件提取：";
            this.fileOpenControl_extractFromOFile.Location = new System.Drawing.Point(12, 12);
            this.fileOpenControl_extractFromOFile.Name = "fileOpenControl_extractFromOFile";
            this.fileOpenControl_extractFromOFile.Size = new System.Drawing.Size(438, 22);
            this.fileOpenControl_extractFromOFile.TabIndex = 2;
            this.fileOpenControl_extractFromOFile.FilePathSetted += new System.EventHandler(this.fileOpenControl_extractFromOFile_FilePathSetted);
            // 
            // comboBox_coordSource
            // 
            this.comboBox_coordSource.DataSource = this.bindingSource_coord;
            this.comboBox_coordSource.FormattingEnabled = true;
            this.comboBox_coordSource.Location = new System.Drawing.Point(116, 60);
            this.comboBox_coordSource.Name = "comboBox_coordSource";
            this.comboBox_coordSource.Size = new System.Drawing.Size(232, 20);
            this.comboBox_coordSource.TabIndex = 3;
            this.comboBox_coordSource.SelectedIndexChanged += new System.EventHandler(this.comboBox_coordSource_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "系统坐标服务：";
            // 
            // button_setCoordFromFile
            // 
            this.button_setCoordFromFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setCoordFromFile.Location = new System.Drawing.Point(468, 12);
            this.button_setCoordFromFile.Name = "button_setCoordFromFile";
            this.button_setCoordFromFile.Size = new System.Drawing.Size(75, 32);
            this.button_setCoordFromFile.TabIndex = 1;
            this.button_setCoordFromFile.Text = "提取并设置";
            this.button_setCoordFromFile.UseVisualStyleBackColor = true;
            this.button_setCoordFromFile.Click += new System.EventHandler(this.button_setCoordFromFile_Click);
            // 
            // label_siteInfo
            // 
            this.label_siteInfo.AutoSize = true;
            this.label_siteInfo.Location = new System.Drawing.Point(114, 95);
            this.label_siteInfo.Name = "label_siteInfo";
            this.label_siteInfo.Size = new System.Drawing.Size(53, 12);
            this.label_siteInfo.TabIndex = 4;
            this.label_siteInfo.Text = "已选择：";
            // 
            // CoordSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 191);
            this.Controls.Add(this.label_siteInfo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_coordSource);
            this.Controls.Add(this.fileOpenControl_extractFromOFile);
            this.Controls.Add(this.button_setCoordFromFile);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.namedStringControl_coord);
            this.Name = "CoordSelectForm";
            this.Text = "坐标选择输入器";
            this.Load += new System.EventHandler(this.CoordSelectForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_coord)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Geo.Winform.Controls.NamedStringControl namedStringControl_coord;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_extractFromOFile;
        private System.Windows.Forms.ComboBox comboBox_coordSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource bindingSource_coord;
        private System.Windows.Forms.Button button_setCoordFromFile;
        private System.Windows.Forms.Label label_siteInfo;
    }
}