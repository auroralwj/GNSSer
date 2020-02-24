namespace Gnsser.Winform
{
    partial class AmbiguityFileBuilderForm
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
            this.fileOpenControl_EpochParam = new Geo.Winform.Controls.FileOpenControl();
            this.button_combine = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.objectTableControlB = new Geo.Winform.ObjectTableControl();
            this.namedIntControl_allowBreakCount = new Geo.Winform.Controls.NamedIntControl();
            this.fileOpenControl_EpochParamRms = new Geo.Winform.Controls.FileOpenControl();
            this.namedFloatControl_defaultRms = new Geo.Winform.Controls.NamedFloatControl();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.fileOpenControl_EpochParamRms);
            this.groupBox1.Controls.Add(this.fileOpenControl_EpochParam);
            this.groupBox1.Location = new System.Drawing.Point(0, 1);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(798, 90);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息";
            // 
            // fileOpenControl_EpochParam
            // 
            this.fileOpenControl_EpochParam.AllowDrop = true;
            this.fileOpenControl_EpochParam.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_EpochParam.FilePath = "";
            this.fileOpenControl_EpochParam.FilePathes = new string[0];
            this.fileOpenControl_EpochParam.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_EpochParam.FirstPath = "";
            this.fileOpenControl_EpochParam.IsMultiSelect = false;
            this.fileOpenControl_EpochParam.LabelName = "历元参数文件：";
            this.fileOpenControl_EpochParam.Location = new System.Drawing.Point(21, 16);
            this.fileOpenControl_EpochParam.Margin = new System.Windows.Forms.Padding(5);
            this.fileOpenControl_EpochParam.Name = "fileOpenControl_EpochParam";
            this.fileOpenControl_EpochParam.Size = new System.Drawing.Size(755, 28);
            this.fileOpenControl_EpochParam.TabIndex = 0;
            // 
            // button_combine
            // 
            this.button_combine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_combine.Location = new System.Drawing.Point(684, 99);
            this.button_combine.Margin = new System.Windows.Forms.Padding(4);
            this.button_combine.Name = "button_combine";
            this.button_combine.Size = new System.Drawing.Size(100, 43);
            this.button_combine.TabIndex = 1;
            this.button_combine.Text = "提取";
            this.button_combine.UseVisualStyleBackColor = true;
            this.button_combine.Click += new System.EventHandler(this.button_combine_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(0, 145);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(798, 364);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.objectTableControlB);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(790, 335);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "结果";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // objectTableControlB
            // 
            this.objectTableControlB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControlB.Location = new System.Drawing.Point(4, 4);
            this.objectTableControlB.Margin = new System.Windows.Forms.Padding(5);
            this.objectTableControlB.Name = "objectTableControlB";
            this.objectTableControlB.Size = new System.Drawing.Size(782, 327);
            this.objectTableControlB.TabIndex = 1;
            this.objectTableControlB.TableObjectStorage = null;
            // 
            // namedIntControl_allowBreakCount
            // 
            this.namedIntControl_allowBreakCount.Location = new System.Drawing.Point(21, 99);
            this.namedIntControl_allowBreakCount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.namedIntControl_allowBreakCount.Name = "namedIntControl_allowBreakCount";
            this.namedIntControl_allowBreakCount.Size = new System.Drawing.Size(244, 29);
            this.namedIntControl_allowBreakCount.TabIndex = 3;
            this.namedIntControl_allowBreakCount.Title = "最大允许历元断裂数：";
            this.namedIntControl_allowBreakCount.Value = 0;
            // 
            // fileOpenControl_EpochParamRms
            // 
            this.fileOpenControl_EpochParamRms.AllowDrop = true;
            this.fileOpenControl_EpochParamRms.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_EpochParamRms.FilePath = "";
            this.fileOpenControl_EpochParamRms.FilePathes = new string[0];
            this.fileOpenControl_EpochParamRms.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_EpochParamRms.FirstPath = "";
            this.fileOpenControl_EpochParamRms.IsMultiSelect = false;
            this.fileOpenControl_EpochParamRms.LabelName = "参数RMS文件：";
            this.fileOpenControl_EpochParamRms.Location = new System.Drawing.Point(21, 51);
            this.fileOpenControl_EpochParamRms.Margin = new System.Windows.Forms.Padding(5);
            this.fileOpenControl_EpochParamRms.Name = "fileOpenControl_EpochParamRms";
            this.fileOpenControl_EpochParamRms.Size = new System.Drawing.Size(755, 28);
            this.fileOpenControl_EpochParamRms.TabIndex = 0;
            // 
            // namedFloatControl_defaultRms
            // 
            this.namedFloatControl_defaultRms.Location = new System.Drawing.Point(286, 99);
            this.namedFloatControl_defaultRms.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.namedFloatControl_defaultRms.Name = "namedFloatControl_defaultRms";
            this.namedFloatControl_defaultRms.Size = new System.Drawing.Size(187, 29);
            this.namedFloatControl_defaultRms.TabIndex = 4;
            this.namedFloatControl_defaultRms.Title = "默认RMS：";
            this.namedFloatControl_defaultRms.Value = 1E-08D;
            // 
            // AmbiguityFileBuilderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 508);
            this.Controls.Add(this.namedFloatControl_defaultRms);
            this.Controls.Add(this.namedIntControl_allowBreakCount);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button_combine);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AmbiguityFileBuilderForm";
            this.Text = "模糊度提取器";
            this.Load += new System.EventHandler(this.AmbiguityCombineerForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_EpochParam;
        private System.Windows.Forms.Button button_combine;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Geo.Winform.ObjectTableControl objectTableControlB;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_allowBreakCount;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_EpochParamRms;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_defaultRms;
    }
}