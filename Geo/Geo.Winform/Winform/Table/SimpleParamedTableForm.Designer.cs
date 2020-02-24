namespace Geo.Winform
{
    partial class SimpleParamedTableForm<T>
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
            this.simpleTableControl1 = new Geo.Winform.SimpleTableControl();
            this.SuspendLayout();
            // 
            // simpleTableControl1
            // 
            this.simpleTableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simpleTableControl1.EnableExport = false;
            this.simpleTableControl1.HiddenColumns = null;
            this.simpleTableControl1.IsShowTitle = false;
            this.simpleTableControl1.Location = new System.Drawing.Point(0, 0);
            this.simpleTableControl1.Name = "simpleTableControl1";
            this.simpleTableControl1.Size = new System.Drawing.Size(669, 449);
            this.simpleTableControl1.TabIndex = 0;
            this.simpleTableControl1.Title = "导航";
            // 
            // SimpleTableViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 449);
            this.Controls.Add(this.simpleTableControl1);
            this.Name = "SimpleTableViewForm";
            this.Text = "数据表查看器";
            this.ResumeLayout(false);

        }

        #endregion

        private SimpleTableControl simpleTableControl1;
    }
}