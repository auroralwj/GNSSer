namespace Gnsser.Winform
{
    partial class NarrowLineSolverForm
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
            this.baseSatSelectingControl1 = new Gnsser.Winform.Controls.BaseSatSelectingControl();
            this.fileOpenControl_wideLane = new Geo.Winform.Controls.FileOpenControl();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_skipCount = new System.Windows.Forms.TextBox();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage0
            // 
            this.tabPage0.Size = new System.Drawing.Size(655, 137);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.textBox_skipCount);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.fileOpenControl_wideLane);
            this.panel4.Controls.Add(this.baseSatSelectingControl1);
            this.panel4.Size = new System.Drawing.Size(649, 120);
            this.panel4.Controls.SetChildIndex(this.baseSatSelectingControl1, 0);
            this.panel4.Controls.SetChildIndex(this.fileOpenControl_wideLane, 0);
            this.panel4.Controls.SetChildIndex(this.label1, 0);
            this.panel4.Controls.SetChildIndex(this.textBox_skipCount, 0);
            // 
            // baseSatSelectingControl1
            // 
            this.baseSatSelectingControl1.EnableBaseSat = true;
            this.baseSatSelectingControl1.Location = new System.Drawing.Point(116, 0);
            this.baseSatSelectingControl1.Name = "baseSatSelectingControl1";
            this.baseSatSelectingControl1.Size = new System.Drawing.Size(279, 85);
            this.baseSatSelectingControl1.TabIndex = 17;
            // 
            // fileOpenControl_wideLane
            // 
            this.fileOpenControl_wideLane.AllowDrop = true;
            this.fileOpenControl_wideLane.FilePath = "Data\\GNSS\\DcbProducts.sdfcb.xls";
            this.fileOpenControl_wideLane.FilePathes = new string[] {
        "Data\\GNSS\\DcbProducts.sdfcb.xls"};
            this.fileOpenControl_wideLane.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_wideLane.IsMultiSelect = false;
            this.fileOpenControl_wideLane.LabelName = "宽项星间单差文件：";
            this.fileOpenControl_wideLane.Location = new System.Drawing.Point(28, 76);
            this.fileOpenControl_wideLane.Name = "fileOpenControl_wideLane";
            this.fileOpenControl_wideLane.Size = new System.Drawing.Size(429, 22);
            this.fileOpenControl_wideLane.TabIndex = 57;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(400, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 58;
            this.label1.Text = "跳过起始历元数：";
            // 
            // textBox_skipCount
            // 
            this.textBox_skipCount.Location = new System.Drawing.Point(499, 13);
            this.textBox_skipCount.Name = "textBox_skipCount";
            this.textBox_skipCount.Size = new System.Drawing.Size(49, 21);
            this.textBox_skipCount.TabIndex = 59;
            this.textBox_skipCount.Text = "250";
            // 
            // NarrowLineSolverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 405);
            this.IsShowProgressBar = true;
            this.Name = "NarrowLineSolverForm";
            this.Text = "窄项模糊度计算器";
            this.Load += new System.EventHandler(this.WideLaneSolverForm_Load);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.BaseSatSelectingControl baseSatSelectingControl1;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_wideLane;
        private System.Windows.Forms.TextBox textBox_skipCount;
        private System.Windows.Forms.Label label1;


    }
}