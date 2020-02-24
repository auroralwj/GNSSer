namespace Geo.Winform
{
    partial class RunningStateForm
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
            this.richTextBoxControl_output = new Geo.Winform.Controls.RichTextBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.richTextBoxControl_output);
            this.splitContainer1.Size = new System.Drawing.Size(568, 489);
            this.splitContainer1.SplitterDistance = 119;
            this.splitContainer1.TabIndex = 0;
            // 
            // richTextBoxControl_output
            // 
            this.richTextBoxControl_output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_output.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxControl_output.Name = "richTextBoxControl_output";
            this.richTextBoxControl_output.Size = new System.Drawing.Size(568, 366);
            this.richTextBoxControl_output.TabIndex = 0;
            this.richTextBoxControl_output.Text = "";
            // 
            // RunningStateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 489);
            this.Controls.Add(this.splitContainer1);
            this.Name = "RunningStateForm";
            this.Text = "RunningStateForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RunningStateForm_FormClosing);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Controls.RichTextBoxControl richTextBoxControl_output;
    }
}