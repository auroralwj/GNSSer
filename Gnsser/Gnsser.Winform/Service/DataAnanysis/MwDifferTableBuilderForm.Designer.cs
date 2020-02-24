namespace Gnsser.Winform
{
    partial class MwDifferTableBuilderForm
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
            this.fileOpenControl1 = new Geo.Winform.Controls.FileOpenControl();
            this.button_read = new System.Windows.Forms.Button();
            this.objectTableControl1 = new Geo.Winform.ObjectTableControl();
            this.bindingSource_prns = new System.Windows.Forms.BindingSource(this.components);
            this.arrayCheckBoxControl_prn = new Geo.Winform.ArrayCheckBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_prns)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.arrayCheckBoxControl_prn);
            this.splitContainer1.Panel1.Controls.Add(this.button_read);
            this.splitContainer1.Panel1.Controls.Add(this.fileOpenControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.objectTableControl1);
            this.splitContainer1.Size = new System.Drawing.Size(804, 457);
            this.splitContainer1.SplitterDistance = 160;
            this.splitContainer1.TabIndex = 0;
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl1.AllowDrop = true;
            this.fileOpenControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl1.FilePath = "";
            this.fileOpenControl1.FilePathes = new string[0];
            this.fileOpenControl1.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl1.FirstPath = "";
            this.fileOpenControl1.IsMultiSelect = true;
            this.fileOpenControl1.LabelName = "观测文件：";
            this.fileOpenControl1.Location = new System.Drawing.Point(13, 13);
            this.fileOpenControl1.Margin = new System.Windows.Forms.Padding(4);
            this.fileOpenControl1.Name = "fileOpenControl1";
            this.fileOpenControl1.Size = new System.Drawing.Size(778, 69);
            this.fileOpenControl1.TabIndex = 0;
            this.fileOpenControl1.FilePathSetted += new System.EventHandler(this.fileOpenControl1_FilePathSetted);
            // 
            // button_read
            // 
            this.button_read.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_read.Location = new System.Drawing.Point(683, 108);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(108, 35);
            this.button_read.TabIndex = 1;
            this.button_read.Text = "读取";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_read_Click);
            // 
            // objectTableControl1
            // 
            this.objectTableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl1.Location = new System.Drawing.Point(0, 0);
            this.objectTableControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.objectTableControl1.Name = "objectTableControl1";
            this.objectTableControl1.Size = new System.Drawing.Size(804, 293);
            this.objectTableControl1.TabIndex = 0;
            this.objectTableControl1.TableObjectStorage = null;
            // 
            // arrayCheckBoxControl_prn
            // 
            this.arrayCheckBoxControl_prn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.arrayCheckBoxControl_prn.Location = new System.Drawing.Point(13, 90);
            this.arrayCheckBoxControl_prn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.arrayCheckBoxControl_prn.Name = "arrayCheckBoxControl_prn";
            this.arrayCheckBoxControl_prn.Size = new System.Drawing.Size(663, 66);
            this.arrayCheckBoxControl_prn.TabIndex = 2;
            this.arrayCheckBoxControl_prn.Title = "选项";
            // 
            // MwDifferTableBuilderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 457);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MwDifferTableBuilderForm";
            this.Text = "MwDifferTableBuilderForm";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_prns)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl1;
        private System.Windows.Forms.Button button_read;
        private Geo.Winform.ObjectTableControl objectTableControl1;
        private System.Windows.Forms.BindingSource bindingSource_prns;
        private Geo.Winform.ArrayCheckBoxControl arrayCheckBoxControl_prn;
    }
}