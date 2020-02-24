namespace Gnsser.Winform
{
    partial class SingleCombineOFileForm
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
            this.fileOpenControl_inputs = new Geo.Winform.Controls.FileOpenControl();
            this.button_run = new System.Windows.Forms.Button();
            this.button_sort = new System.Windows.Forms.Button();
            this.directorySelectionControl_outputDir = new Geo.Winform.Controls.DirectorySelectionControl();
            this.namedFloatControl_outVersion = new Geo.Winform.Controls.NamedFloatControl();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.SuspendLayout();
            // 
            // fileOpenControl_inputs
            // 
            this.fileOpenControl_inputs.AllowDrop = true;
            this.fileOpenControl_inputs.FilePath = "";
            this.fileOpenControl_inputs.FilePathes = new string[0];
            this.fileOpenControl_inputs.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_inputs.FirstPath = "";
            this.fileOpenControl_inputs.IsMultiSelect = true;
            this.fileOpenControl_inputs.LabelName = "文件：";
            this.fileOpenControl_inputs.Location = new System.Drawing.Point(12, 12);
            this.fileOpenControl_inputs.Name = "fileOpenControl_inputs";
            this.fileOpenControl_inputs.Size = new System.Drawing.Size(597, 157);
            this.fileOpenControl_inputs.TabIndex = 0;
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(571, 287);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 43);
            this.button_run.TabIndex = 1;
            this.button_run.Text = "拼接";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // button_sort
            // 
            this.button_sort.Location = new System.Drawing.Point(534, 175);
            this.button_sort.Name = "button_sort";
            this.button_sort.Size = new System.Drawing.Size(75, 43);
            this.button_sort.TabIndex = 1;
            this.button_sort.Text = "排序";
            this.button_sort.UseVisualStyleBackColor = true;
            this.button_sort.Click += new System.EventHandler(this.button_sort_Click);
            // 
            // directorySelectionControl_outputDir
            // 
            this.directorySelectionControl_outputDir.AllowDrop = true;
            this.directorySelectionControl_outputDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl_outputDir.IsMultiPathes = false;
            this.directorySelectionControl_outputDir.LabelName = "输出目录：";
            this.directorySelectionControl_outputDir.Location = new System.Drawing.Point(12, 229);
            this.directorySelectionControl_outputDir.Name = "directorySelectionControl_outputDir";
            this.directorySelectionControl_outputDir.Path = "";
            this.directorySelectionControl_outputDir.Pathes = new string[0];
            this.directorySelectionControl_outputDir.Size = new System.Drawing.Size(592, 22);
            this.directorySelectionControl_outputDir.TabIndex = 2;
            // 
            // namedFloatControl_outVersion
            // 
            this.namedFloatControl_outVersion.Location = new System.Drawing.Point(12, 175);
            this.namedFloatControl_outVersion.Name = "namedFloatControl_outVersion";
            this.namedFloatControl_outVersion.Size = new System.Drawing.Size(140, 23);
            this.namedFloatControl_outVersion.TabIndex = 3;
            this.namedFloatControl_outVersion.Title = "输出版本：";
            this.namedFloatControl_outVersion.Value = 3.02D;
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.IsBackwardProcess = false;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(54, 287);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(468, 34);
            this.progressBarComponent1.TabIndex = 4;
            // 
            // SingleCombineOFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 361);
            this.Controls.Add(this.progressBarComponent1);
            this.Controls.Add(this.namedFloatControl_outVersion);
            this.Controls.Add(this.directorySelectionControl_outputDir);
            this.Controls.Add(this.button_sort);
            this.Controls.Add(this.button_run);
            this.Controls.Add(this.fileOpenControl_inputs);
            this.Name = "SingleCombineOFileForm";
            this.Text = "单站拼接";
            this.Load += new System.EventHandler(this.CombineOFileForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Geo.Winform.Controls.FileOpenControl fileOpenControl_inputs;
        private System.Windows.Forms.Button button_run;
        private System.Windows.Forms.Button button_sort;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl_outputDir;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_outVersion;
        private Geo.Winform.Controls.ProgressBarComponent progressBarComponent1;
    }
}