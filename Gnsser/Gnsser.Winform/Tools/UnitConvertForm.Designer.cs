namespace Gnsser.Winform
{
    partial class UnitConvertForm
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
            this.button_nsTom = new System.Windows.Forms.Button();
            this.richTextBoxControl_left = new Geo.Winform.Controls.RichTextBoxControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.richTextBoxControl_right = new Geo.Winform.Controls.RichTextBoxControl();
            this.button_exchange = new System.Windows.Forms.Button();
            this.checkBox_inverse = new System.Windows.Forms.CheckBox();
            this.button_mToNs = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_nsTom
            // 
            this.button_nsTom.Location = new System.Drawing.Point(28, 110);
            this.button_nsTom.Name = "button_nsTom";
            this.button_nsTom.Size = new System.Drawing.Size(75, 23);
            this.button_nsTom.TabIndex = 0;
            this.button_nsTom.Text = "纳秒 -> 米";
            this.button_nsTom.UseVisualStyleBackColor = true;
            this.button_nsTom.Click += new System.EventHandler(this.button_nsTom_Click);
            // 
            // richTextBoxControl_left
            // 
            this.richTextBoxControl_left.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_left.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxControl_left.MaxAppendLineCount = 5000;
            this.richTextBoxControl_left.Name = "richTextBoxControl_left";
            this.richTextBoxControl_left.Size = new System.Drawing.Size(177, 414);
            this.richTextBoxControl_left.TabIndex = 1;
            this.richTextBoxControl_left.Text = "";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.richTextBoxControl_left);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(532, 414);
            this.splitContainer1.SplitterDistance = 177;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            this.splitContainer2.Panel1.Controls.Add(this.checkBox_inverse);
            this.splitContainer2.Panel1.Controls.Add(this.button_exchange);
            this.splitContainer2.Panel1.Controls.Add(this.button_mToNs);
            this.splitContainer2.Panel1.Controls.Add(this.button_nsTom);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.richTextBoxControl_right);
            this.splitContainer2.Size = new System.Drawing.Size(351, 414);
            this.splitContainer2.SplitterDistance = 164;
            this.splitContainer2.TabIndex = 0;
            // 
            // richTextBoxControl_right
            // 
            this.richTextBoxControl_right.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_right.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxControl_right.MaxAppendLineCount = 5000;
            this.richTextBoxControl_right.Name = "richTextBoxControl_right";
            this.richTextBoxControl_right.Size = new System.Drawing.Size(183, 414);
            this.richTextBoxControl_right.TabIndex = 2;
            this.richTextBoxControl_right.Text = "";
            // 
            // button_exchange
            // 
            this.button_exchange.Location = new System.Drawing.Point(31, 13);
            this.button_exchange.Name = "button_exchange";
            this.button_exchange.Size = new System.Drawing.Size(75, 23);
            this.button_exchange.TabIndex = 1;
            this.button_exchange.Text = "左右交换";
            this.button_exchange.UseVisualStyleBackColor = true;
            this.button_exchange.Click += new System.EventHandler(this.button_exchange_Click);
            // 
            // checkBox_inverse
            // 
            this.checkBox_inverse.AutoSize = true;
            this.checkBox_inverse.Location = new System.Drawing.Point(31, 43);
            this.checkBox_inverse.Name = "checkBox_inverse";
            this.checkBox_inverse.Size = new System.Drawing.Size(72, 16);
            this.checkBox_inverse.TabIndex = 2;
            this.checkBox_inverse.Text = "反向计算";
            this.checkBox_inverse.UseVisualStyleBackColor = true;
            // 
            // button_mToNs
            // 
            this.button_mToNs.Location = new System.Drawing.Point(28, 142);
            this.button_mToNs.Name = "button_mToNs";
            this.button_mToNs.Size = new System.Drawing.Size(75, 23);
            this.button_mToNs.TabIndex = 0;
            this.button_mToNs.Text = "米 -> 纳秒";
            this.button_mToNs.UseVisualStyleBackColor = true;
            this.button_mToNs.Click += new System.EventHandler(this.button_mToNs_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "提示：一行一结果";
            // 
            // UnitConvertForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 414);
            this.Controls.Add(this.splitContainer1);
            this.Name = "UnitConvertForm";
            this.Text = "单位转换";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_nsTom;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl_left;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl_right;
        private System.Windows.Forms.Button button_exchange;
        private System.Windows.Forms.CheckBox checkBox_inverse;
        private System.Windows.Forms.Button button_mToNs;
        private System.Windows.Forms.Label label1;
    }
}