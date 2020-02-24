namespace Geo.Winform
{
    partial class NumeralConditionControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.comboBox_match = new System.Windows.Forms.ComboBox();
            this.bindingSource_condition = new System.Windows.Forms.BindingSource(this.components);
            this.textBox_value2 = new System.Windows.Forms.TextBox();
            this.textBox_value1 = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_condition)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox_match
            // 
            this.comboBox_match.DataSource = this.bindingSource_condition;
            this.comboBox_match.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_match.FormattingEnabled = true;
            this.comboBox_match.Location = new System.Drawing.Point(3, 3);
            this.comboBox_match.Name = "comboBox_match";
            this.comboBox_match.Size = new System.Drawing.Size(139, 20);
            this.comboBox_match.TabIndex = 8;
            // 
            // bindingSource_condition
            // 
            this.bindingSource_condition.CurrentChanged += new System.EventHandler(this.bindingSource_condition_CurrentChanged);
            // 
            // textBox_value2
            // 
            this.textBox_value2.Location = new System.Drawing.Point(297, 3);
            this.textBox_value2.Name = "textBox_value2";
            this.textBox_value2.Size = new System.Drawing.Size(173, 21);
            this.textBox_value2.TabIndex = 9;
            this.textBox_value2.Text = "0";
            // 
            // textBox_value1
            // 
            this.textBox_value1.Location = new System.Drawing.Point(148, 3);
            this.textBox_value1.Name = "textBox_value1";
            this.textBox_value1.Size = new System.Drawing.Size(143, 21);
            this.textBox_value1.TabIndex = 10;
            this.textBox_value1.Text = "0";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.comboBox_match);
            this.flowLayoutPanel1.Controls.Add(this.textBox_value1);
            this.flowLayoutPanel1.Controls.Add(this.textBox_value2);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(473, 27);
            this.flowLayoutPanel1.TabIndex = 11;
            // 
            // NumeralConditionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "NumeralConditionControl";
            this.Size = new System.Drawing.Size(473, 27);
            this.Load += new System.EventHandler(this.NumeralConditionControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_condition)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox comboBox_match;
        private System.Windows.Forms.BindingSource bindingSource_condition;
        private System.Windows.Forms.TextBox textBox_value2;
        private System.Windows.Forms.TextBox textBox_value1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
