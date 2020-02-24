namespace Geo.Winform
{
    partial class DifferOfObjectTableForm
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
            this.button_convert = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_openBNew = new System.Windows.Forms.Button();
            this.button_openANew = new System.Windows.Forms.Button();
            this.fileOpenControlB = new Geo.Winform.Controls.FileOpenControl();
            this.fileOpenControlA = new Geo.Winform.Controls.FileOpenControl();
            this.objectTableControl1 = new Geo.Winform.ObjectTableControl();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_convert
            // 
            this.button_convert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_convert.Location = new System.Drawing.Point(654, 103);
            this.button_convert.Name = "button_convert";
            this.button_convert.Size = new System.Drawing.Size(94, 45);
            this.button_convert.TabIndex = 1;
            this.button_convert.Text = "作差";
            this.button_convert.UseVisualStyleBackColor = true;
            this.button_convert.Click += new System.EventHandler(this.button_convert_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.button_openBNew);
            this.groupBox2.Controls.Add(this.button_openANew);
            this.groupBox2.Controls.Add(this.fileOpenControlB);
            this.groupBox2.Controls.Add(this.fileOpenControlA);
            this.groupBox2.Location = new System.Drawing.Point(15, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(733, 84);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "设置";
            // 
            // button_openBNew
            // 
            this.button_openBNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_openBNew.Location = new System.Drawing.Point(644, 49);
            this.button_openBNew.Name = "button_openBNew";
            this.button_openBNew.Size = new System.Drawing.Size(75, 23);
            this.button_openBNew.TabIndex = 2;
            this.button_openBNew.Text = "新窗口打开";
            this.button_openBNew.UseVisualStyleBackColor = true;
            this.button_openBNew.Click += new System.EventHandler(this.button_openBNew_Click);
            // 
            // button_openANew
            // 
            this.button_openANew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_openANew.Location = new System.Drawing.Point(642, 19);
            this.button_openANew.Name = "button_openANew";
            this.button_openANew.Size = new System.Drawing.Size(75, 23);
            this.button_openANew.TabIndex = 2;
            this.button_openANew.Text = "新窗口打开";
            this.button_openANew.UseVisualStyleBackColor = true;
            this.button_openANew.Click += new System.EventHandler(this.button_openANew_Click);
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
            this.fileOpenControlB.LabelName = "文件B：";
            this.fileOpenControlB.Location = new System.Drawing.Point(6, 49);
            this.fileOpenControlB.Name = "fileOpenControlB";
            this.fileOpenControlB.Size = new System.Drawing.Size(629, 22);
            this.fileOpenControlB.TabIndex = 1;
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
            this.fileOpenControlA.LabelName = "文件A：";
            this.fileOpenControlA.Location = new System.Drawing.Point(6, 21);
            this.fileOpenControlA.Name = "fileOpenControlA";
            this.fileOpenControlA.Size = new System.Drawing.Size(629, 22);
            this.fileOpenControlA.TabIndex = 0;
            // 
            // objectTableControl1
            // 
            this.objectTableControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectTableControl1.Location = new System.Drawing.Point(-2, 154);
            this.objectTableControl1.Name = "objectTableControl1";
            this.objectTableControl1.Size = new System.Drawing.Size(753, 284);
            this.objectTableControl1.TabIndex = 3;
            this.objectTableControl1.TableObjectStorage = null;
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.IsBackwardProcess = false;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(21, 103);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(503, 34);
            this.progressBarComponent1.TabIndex = 4;
            // 
            // DifferOfObjectTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 440);
            this.Controls.Add(this.progressBarComponent1);
            this.Controls.Add(this.objectTableControl1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button_convert);
            this.Name = "DifferOfObjectTableForm";
            this.Text = "文本转换为表对象";
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_convert;
        private System.Windows.Forms.GroupBox groupBox2;
        private Controls.FileOpenControl fileOpenControlB;
        private Controls.FileOpenControl fileOpenControlA;
        private ObjectTableControl objectTableControl1;
        private System.Windows.Forms.Button button_openBNew;
        private System.Windows.Forms.Button button_openANew;
        private Controls.ProgressBarComponent progressBarComponent1;
    }
}