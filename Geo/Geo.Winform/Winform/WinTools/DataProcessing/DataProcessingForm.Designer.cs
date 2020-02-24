using Geo.Winform.Controls;
using Geo.Winform;

namespace Geo.WinTools
{
    partial class DataProcessingForm
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.richTextBox_input = new Geo.Winform.Controls.RichTextBoxControl();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel_settingPanel = new System.Windows.Forms.Panel();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.button_run = new System.Windows.Forms.Button();
            this.checkBox_hasRowId = new System.Windows.Forms.CheckBox();
            this.button_draw = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_result = new System.Windows.Forms.TabPage();
            this.richTextBox_output = new Geo.Winform.Controls.RichTextBoxControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_info = new Geo.Winform.Controls.RichTextBoxControl();
            this.button_setInputToFraction = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel_settingPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage_result.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(642, 406);
            this.splitContainer1.SplitterDistance = 185;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_setInputToFraction);
            this.groupBox2.Controls.Add(this.richTextBox_input);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(185, 406);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "数据录入";
            // 
            // richTextBox_input
            // 
            this.richTextBox_input.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox_input.Location = new System.Drawing.Point(2, 44);
            this.richTextBox_input.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBox_input.Name = "richTextBox_input";
            this.richTextBox_input.Size = new System.Drawing.Size(181, 360);
            this.richTextBox_input.TabIndex = 0;
            this.richTextBox_input.Text = "";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.panel_settingPanel);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer2.Size = new System.Drawing.Size(454, 406);
            this.splitContainer2.SplitterDistance = 198;
            this.splitContainer2.SplitterWidth = 3;
            this.splitContainer2.TabIndex = 0;
            // 
            // panel_settingPanel
            // 
            this.panel_settingPanel.Controls.Add(this.splitContainer3);
            this.panel_settingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_settingPanel.Location = new System.Drawing.Point(0, 0);
            this.panel_settingPanel.Margin = new System.Windows.Forms.Padding(2);
            this.panel_settingPanel.Name = "panel_settingPanel";
            this.panel_settingPanel.Size = new System.Drawing.Size(198, 406);
            this.panel_settingPanel.TabIndex = 1;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.button_run);
            this.splitContainer3.Panel1.Controls.Add(this.checkBox_hasRowId);
            this.splitContainer3.Panel1.Controls.Add(this.button_draw);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.panel1);
            this.splitContainer3.Size = new System.Drawing.Size(198, 406);
            this.splitContainer3.SplitterDistance = 84;
            this.splitContainer3.TabIndex = 8;
            // 
            // button_run
            // 
            this.button_run.Location = new System.Drawing.Point(16, 16);
            this.button_run.Margin = new System.Windows.Forms.Padding(2);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(81, 31);
            this.button_run.TabIndex = 0;
            this.button_run.Text = "执行";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // checkBox_hasRowId
            // 
            this.checkBox_hasRowId.AutoSize = true;
            this.checkBox_hasRowId.Location = new System.Drawing.Point(16, 58);
            this.checkBox_hasRowId.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_hasRowId.Name = "checkBox_hasRowId";
            this.checkBox_hasRowId.Size = new System.Drawing.Size(96, 16);
            this.checkBox_hasRowId.TabIndex = 6;
            this.checkBox_hasRowId.Text = "是否含编号列";
            this.checkBox_hasRowId.UseVisualStyleBackColor = true;
            // 
            // button_draw
            // 
            this.button_draw.Location = new System.Drawing.Point(102, 17);
            this.button_draw.Name = "button_draw";
            this.button_draw.Size = new System.Drawing.Size(81, 29);
            this.button_draw.TabIndex = 1;
            this.button_draw.Text = "绘图";
            this.button_draw.UseVisualStyleBackColor = true;
            this.button_draw.Click += new System.EventHandler(this.button_draw_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(198, 318);
            this.panel1.TabIndex = 7;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_result);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(253, 406);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage_result
            // 
            this.tabPage_result.Controls.Add(this.richTextBox_output);
            this.tabPage_result.Location = new System.Drawing.Point(4, 22);
            this.tabPage_result.Name = "tabPage_result";
            this.tabPage_result.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_result.Size = new System.Drawing.Size(245, 380);
            this.tabPage_result.TabIndex = 0;
            this.tabPage_result.Text = "结果输出";
            this.tabPage_result.UseVisualStyleBackColor = true;
            // 
            // richTextBox_output
            // 
            this.richTextBox_output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_output.Location = new System.Drawing.Point(3, 3);
            this.richTextBox_output.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBox_output.Name = "richTextBox_output";
            this.richTextBox_output.Size = new System.Drawing.Size(239, 374);
            this.richTextBox_output.TabIndex = 1;
            this.richTextBox_output.Text = "";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.richTextBoxControl_info);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(245, 380);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "处理信息";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_info
            // 
            this.richTextBoxControl_info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_info.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_info.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBoxControl_info.Name = "richTextBoxControl_info";
            this.richTextBoxControl_info.Size = new System.Drawing.Size(239, 374);
            this.richTextBoxControl_info.TabIndex = 2;
            this.richTextBoxControl_info.Text = "";
            // 
            // button_setInputToFraction
            // 
            this.button_setInputToFraction.Location = new System.Drawing.Point(6, 16);
            this.button_setInputToFraction.Name = "button_setInputToFraction";
            this.button_setInputToFraction.Size = new System.Drawing.Size(75, 23);
            this.button_setInputToFraction.TabIndex = 1;
            this.button_setInputToFraction.Text = "截取小数";
            this.button_setInputToFraction.UseVisualStyleBackColor = true;
            this.button_setInputToFraction.Click += new System.EventHandler(this.button_setInputToFraction_Click);
            // 
            // DataProcessingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 406);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DataProcessingForm";
            this.Text = "数据处理";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LogListeningForm_FormClosing);
            this.Load += new System.EventHandler(this.DataProcessingForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel_settingPanel.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_result.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_run;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private RichTextBoxControl richTextBox_input;
        private RichTextBoxControl richTextBox_output;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_result;
        private System.Windows.Forms.TabPage tabPage2;
        private RichTextBoxControl richTextBoxControl_info;
        private System.Windows.Forms.Button button_draw;
        private System.Windows.Forms.CheckBox checkBox_hasRowId;
        protected System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel_settingPanel;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Button button_setInputToFraction;
    }
}