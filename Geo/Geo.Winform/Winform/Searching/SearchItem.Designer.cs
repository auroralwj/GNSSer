namespace Geo.Winform
{
    partial class SearchItem
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.comboBox_property = new System.Windows.Forms.ComboBox();
            this.bindingSource_property = new System.Windows.Forms.BindingSource(this.components);
            this.comboBox_match = new System.Windows.Forms.ComboBox();
            this.bindingSource_condition = new System.Windows.Forms.BindingSource(this.components);
            this.textBox_value1 = new System.Windows.Forms.TextBox();
            this.textBox_value2 = new System.Windows.Forms.TextBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.comboBox_connect = new System.Windows.Forms.ComboBox();
            this.comboBox_List = new System.Windows.Forms.ComboBox();
            this.bindingSource_list = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_property)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_condition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_list)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox_property
            // 
            this.comboBox_property.DataSource = this.bindingSource_property;
            this.comboBox_property.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_property.FormattingEnabled = true;
            this.comboBox_property.Location = new System.Drawing.Point(52, 1);
            this.comboBox_property.Name = "comboBox_property";
            this.comboBox_property.Size = new System.Drawing.Size(85, 20);
            this.comboBox_property.TabIndex = 0;
            // 
            // bindingSource_property
            // 
            this.bindingSource_property.CurrentChanged += new System.EventHandler(this.bindingSource_property_CurrentChanged);
            // 
            // comboBox_match
            // 
            this.comboBox_match.DataSource = this.bindingSource_condition;
            this.comboBox_match.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_match.FormattingEnabled = true;
            this.comboBox_match.Location = new System.Drawing.Point(143, 1);
            this.comboBox_match.Name = "comboBox_match";
            this.comboBox_match.Size = new System.Drawing.Size(48, 20);
            this.comboBox_match.TabIndex = 1;
            // 
            // bindingSource_condition
            // 
            this.bindingSource_condition.CurrentChanged += new System.EventHandler(this.bindingSource_condition_CurrentChanged);
            // 
            // textBox_value1
            // 
            this.textBox_value1.Location = new System.Drawing.Point(197, 1);
            this.textBox_value1.Name = "textBox_value1";
            this.textBox_value1.Size = new System.Drawing.Size(143, 21);
            this.textBox_value1.TabIndex = 2;
            // 
            // textBox_value2
            // 
            this.textBox_value2.Location = new System.Drawing.Point(346, 1);
            this.textBox_value2.Name = "textBox_value2";
            this.textBox_value2.Size = new System.Drawing.Size(147, 21);
            this.textBox_value2.TabIndex = 2;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(197, 1);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker1.Size = new System.Drawing.Size(143, 21);
            this.dateTimePicker1.TabIndex = 3;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(346, 1);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker2.Size = new System.Drawing.Size(150, 21);
            this.dateTimePicker2.TabIndex = 4;
            // 
            // comboBox_connect
            // 
            this.comboBox_connect.FormattingEnabled = true;
            this.comboBox_connect.Items.AddRange(new object[] {
            "且",
            "或",
            "非"});
            this.comboBox_connect.Location = new System.Drawing.Point(4, 1);
            this.comboBox_connect.Name = "comboBox_connect";
            this.comboBox_connect.Size = new System.Drawing.Size(42, 20);
            this.comboBox_connect.TabIndex = 5;
            this.comboBox_connect.TabStop = false;
            this.comboBox_connect.Text = "且";
            // 
            // comboBox_List
            // 
            this.comboBox_List.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_List.FormattingEnabled = true;
            this.comboBox_List.Location = new System.Drawing.Point(197, 1);
            this.comboBox_List.Name = "comboBox_List";
            this.comboBox_List.Size = new System.Drawing.Size(298, 20);
            this.comboBox_List.TabIndex = 6;
            // 
            // SearchItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBox_List);
            this.Controls.Add(this.comboBox_connect);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.textBox_value2);
            this.Controls.Add(this.textBox_value1);
            this.Controls.Add(this.comboBox_match);
            this.Controls.Add(this.comboBox_property);
            this.Name = "SearchItem";
            this.Size = new System.Drawing.Size(498, 23);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_property)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_condition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_list)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_property;
        private System.Windows.Forms.ComboBox comboBox_match;
        private System.Windows.Forms.TextBox textBox_value1;
        private System.Windows.Forms.BindingSource bindingSource_property;
        private System.Windows.Forms.BindingSource bindingSource_condition;
        private System.Windows.Forms.TextBox textBox_value2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.ComboBox comboBox_connect;
        private System.Windows.Forms.ComboBox comboBox_List;
        private System.Windows.Forms.BindingSource bindingSource_list;
    }
}
