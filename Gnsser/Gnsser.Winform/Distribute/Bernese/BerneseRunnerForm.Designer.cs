namespace Gnsser.Winform
{
    partial class BerneseRunnerForm
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

        #region Windows Form Designer generated obsCodeode

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the obsCodeode editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_solveType = new System.Windows.Forms.ComboBox();
            this.dateTimePicker_date = new System.Windows.Forms.DateTimePicker();
            this.textBox_campaign = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_run = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.textBox_result = new System.Windows.Forms.TextBox();
            this.button_viewState = new System.Windows.Forms.Button();
            this.checkBox_asyn = new System.Windows.Forms.CheckBox();
            this.checkBox_skip = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkBox_skip);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.comboBox_solveType);
            this.groupBox1.Controls.Add(this.dateTimePicker_date);
            this.groupBox1.Controls.Add(this.textBox_campaign);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(570, 107);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "算法：";
            // 
            // comboBox_solveType
            // 
            this.comboBox_solveType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_solveType.FormattingEnabled = true;
            this.comboBox_solveType.Location = new System.Drawing.Point(88, 74);
            this.comboBox_solveType.Name = "comboBox_solveType";
            this.comboBox_solveType.Size = new System.Drawing.Size(139, 20);
            this.comboBox_solveType.TabIndex = 4;
            // 
            // dateTimePicker_date
            // 
            this.dateTimePicker_date.Location = new System.Drawing.Point(88, 45);
            this.dateTimePicker_date.Name = "dateTimePicker_date";
            this.dateTimePicker_date.Size = new System.Drawing.Size(194, 21);
            this.dateTimePicker_date.TabIndex = 3;
            this.dateTimePicker_date.Value = new System.DateTime(2002, 5, 23, 0, 0, 0, 0);
            // 
            // textBox_campaign
            // 
            this.textBox_campaign.Location = new System.Drawing.Point(88, 19);
            this.textBox_campaign.Name = "textBox_campaign";
            this.textBox_campaign.Size = new System.Drawing.Size(194, 21);
            this.textBox_campaign.TabIndex = 2;
            this.textBox_campaign.Text = "EXAMPLE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "历元：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "工程：";
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(418, 126);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 33);
            this.button_run.TabIndex = 1;
            this.button_run.Text = "运行";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(508, 126);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 33);
            this.button_cancel.TabIndex = 2;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // textBox_result
            // 
            this.textBox_result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_result.Location = new System.Drawing.Point(13, 165);
            this.textBox_result.Multiline = true;
            this.textBox_result.Name = "textBox_result";
            this.textBox_result.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_result.Size = new System.Drawing.Size(569, 230);
            this.textBox_result.TabIndex = 3;
            // 
            // button_viewState
            // 
            this.button_viewState.Location = new System.Drawing.Point(13, 126);
            this.button_viewState.Name = "button_viewState";
            this.button_viewState.Size = new System.Drawing.Size(96, 33);
            this.button_viewState.TabIndex = 4;
            this.button_viewState.Text = "查看执行状态";
            this.button_viewState.UseVisualStyleBackColor = true;
            this.button_viewState.Click += new System.EventHandler(this.button_viewState_Click);
            // 
            // checkBox_asyn
            // 
            this.checkBox_asyn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_asyn.AutoSize = true;
            this.checkBox_asyn.Checked = true;
            this.checkBox_asyn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_asyn.Location = new System.Drawing.Point(349, 135);
            this.checkBox_asyn.Name = "checkBox_asyn";
            this.checkBox_asyn.Size = new System.Drawing.Size(48, 16);
            this.checkBox_asyn.TabIndex = 5;
            this.checkBox_asyn.Text = "异步";
            this.checkBox_asyn.UseVisualStyleBackColor = true;
            // 
            // checkBox_skip
            // 
            this.checkBox_skip.AutoSize = true;
            this.checkBox_skip.Location = new System.Drawing.Point(271, 78);
            this.checkBox_skip.Name = "checkBox_skip";
            this.checkBox_skip.Size = new System.Drawing.Size(72, 16);
            this.checkBox_skip.TabIndex = 10;
            this.checkBox_skip.Text = "跳过步骤";
            this.checkBox_skip.UseVisualStyleBackColor = true;
            // 
            // BerneseRunnerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 407);
            this.Controls.Add(this.checkBox_asyn);
            this.Controls.Add(this.button_viewState);
            this.Controls.Add(this.textBox_result);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_run);
            this.Controls.Add(this.groupBox1);
            this.Name = "BerneseRunnerForm";
            this.Text = "Bernese 执行器";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dateTimePicker_date;
        private System.Windows.Forms.TextBox textBox_campaign;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_run;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_solveType;
        private System.Windows.Forms.TextBox textBox_result;
        private System.Windows.Forms.Button button_viewState;
        private System.Windows.Forms.CheckBox checkBox_asyn;
        private System.Windows.Forms.CheckBox checkBox_skip;
    }
}