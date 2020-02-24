namespace Geo.Winform
{
    partial class TextToObjectTableForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_convert = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_insertIndex = new System.Windows.Forms.Button();
            this.richTextBox_splitter = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBoxControl_text = new Geo.Winform.Controls.RichTextBoxControl();
            this.checkBox_removeVacant = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.richTextBoxControl_text);
            this.groupBox1.Location = new System.Drawing.Point(12, 136);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(747, 285);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "文本表格";
            // 
            // button_convert
            // 
            this.button_convert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_convert.Location = new System.Drawing.Point(665, 85);
            this.button_convert.Name = "button_convert";
            this.button_convert.Size = new System.Drawing.Size(94, 45);
            this.button_convert.TabIndex = 1;
            this.button_convert.Text = "转换";
            this.button_convert.UseVisualStyleBackColor = true;
            this.button_convert.Click += new System.EventHandler(this.button_convert_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.checkBox_removeVacant);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.richTextBox_splitter);
            this.groupBox2.Location = new System.Drawing.Point(15, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(741, 66);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "设置";
            // 
            // button_insertIndex
            // 
            this.button_insertIndex.Location = new System.Drawing.Point(15, 85);
            this.button_insertIndex.Name = "button_insertIndex";
            this.button_insertIndex.Size = new System.Drawing.Size(91, 31);
            this.button_insertIndex.TabIndex = 3;
            this.button_insertIndex.Text = "插入编号";
            this.button_insertIndex.UseVisualStyleBackColor = true;
            this.button_insertIndex.Click += new System.EventHandler(this.button_insertIndex_Click);
            // 
            // richTextBox_splitter
            // 
            this.richTextBox_splitter.Location = new System.Drawing.Point(67, 9);
            this.richTextBox_splitter.Name = "richTextBox_splitter";
            this.richTextBox_splitter.Size = new System.Drawing.Size(100, 51);
            this.richTextBox_splitter.TabIndex = 1;
            this.richTextBox_splitter.Text = " \n,\n;\n\t";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "分隔符：\r\n按行输入";
            // 
            // richTextBoxControl_text
            // 
            this.richTextBoxControl_text.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_text.Location = new System.Drawing.Point(3, 17);
            this.richTextBoxControl_text.MaxAppendLineCount = 5000;
            this.richTextBoxControl_text.Name = "richTextBoxControl_text";
            this.richTextBoxControl_text.Size = new System.Drawing.Size(741, 265);
            this.richTextBoxControl_text.TabIndex = 1;
            this.richTextBoxControl_text.Text = "";
            // 
            // checkBox_removeVacant
            // 
            this.checkBox_removeVacant.AutoSize = true;
            this.checkBox_removeVacant.Checked = true;
            this.checkBox_removeVacant.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_removeVacant.Location = new System.Drawing.Point(185, 10);
            this.checkBox_removeVacant.Name = "checkBox_removeVacant";
            this.checkBox_removeVacant.Size = new System.Drawing.Size(72, 16);
            this.checkBox_removeVacant.TabIndex = 3;
            this.checkBox_removeVacant.Text = "移除空值";
            this.checkBox_removeVacant.UseVisualStyleBackColor = true;
            // 
            // TextToObjectTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 433);
            this.Controls.Add(this.button_insertIndex);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button_convert);
            this.Controls.Add(this.groupBox1);
            this.Name = "TextToObjectTableForm";
            this.Text = "文本转换为表对象";
            this.Load += new System.EventHandler(this.TextToObjectTableForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Controls.RichTextBoxControl richTextBoxControl_text;
        private System.Windows.Forms.Button button_convert;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_insertIndex;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox_splitter;
        private System.Windows.Forms.CheckBox checkBox_removeVacant;
    }
}