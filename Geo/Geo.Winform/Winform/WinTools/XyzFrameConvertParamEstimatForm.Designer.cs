namespace Geo.WinTools
{
    partial class XyzFrameConvertParamEstimatForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBox_olds = new Geo.Winform.Controls.RichTextBoxControl();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.button_exchange = new System.Windows.Forms.Button();
            this.button_solveParams = new System.Windows.Forms.Button();
            this.button_oldToNew = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBox_news = new Geo.Winform.Controls.RichTextBoxControl();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.xyzFrameTrans7ParamControl1 = new Geo.Winform.Controls.XyzFrameTrans7ParamControl();
            this.checkBox_IsOutSplitByTab = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage3.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(791, 384);
            this.splitContainer1.SplitterDistance = 330;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(330, 384);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBox_olds);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(322, 358);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "待转系统坐标";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBox_olds
            // 
            this.textBox_olds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_olds.Location = new System.Drawing.Point(3, 3);
            this.textBox_olds.MaxAppendLineCount = 5000;
            this.textBox_olds.Name = "textBox_olds";
            this.textBox_olds.Size = new System.Drawing.Size(316, 352);
            this.textBox_olds.TabIndex = 0;
            this.textBox_olds.Text = "0  0  0\n1   1   1\n2  2    2";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.button_exchange);
            this.splitContainer2.Panel1.Controls.Add(this.button_solveParams);
            this.splitContainer2.Panel1.Controls.Add(this.button_oldToNew);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControl2);
            this.splitContainer2.Size = new System.Drawing.Size(458, 384);
            this.splitContainer2.SplitterDistance = 109;
            this.splitContainer2.SplitterWidth = 3;
            this.splitContainer2.TabIndex = 0;
            // 
            // button_exchange
            // 
            this.button_exchange.Location = new System.Drawing.Point(19, 244);
            this.button_exchange.Margin = new System.Windows.Forms.Padding(2);
            this.button_exchange.Name = "button_exchange";
            this.button_exchange.Size = new System.Drawing.Size(67, 34);
            this.button_exchange.TabIndex = 3;
            this.button_exchange.Text = "<-->";
            this.button_exchange.UseVisualStyleBackColor = true;
            this.button_exchange.Click += new System.EventHandler(this.button_exchange_Click);
            // 
            // button_solveParams
            // 
            this.button_solveParams.Location = new System.Drawing.Point(19, 37);
            this.button_solveParams.Margin = new System.Windows.Forms.Padding(2);
            this.button_solveParams.Name = "button_solveParams";
            this.button_solveParams.Size = new System.Drawing.Size(67, 34);
            this.button_solveParams.TabIndex = 2;
            this.button_solveParams.Text = "参数估计";
            this.button_solveParams.UseVisualStyleBackColor = true;
            this.button_solveParams.Click += new System.EventHandler(this.button_solveParams_Click);
            // 
            // button_oldToNew
            // 
            this.button_oldToNew.Location = new System.Drawing.Point(19, 111);
            this.button_oldToNew.Margin = new System.Windows.Forms.Padding(2);
            this.button_oldToNew.Name = "button_oldToNew";
            this.button_oldToNew.Size = new System.Drawing.Size(67, 34);
            this.button_oldToNew.TabIndex = 2;
            this.button_oldToNew.Text = ">>";
            this.button_oldToNew.UseVisualStyleBackColor = true;
            this.button_oldToNew.Click += new System.EventHandler(this.button_oldToNew_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(346, 384);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textBox_news);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(338, 358);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "目标系统坐标";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBox_news
            // 
            this.textBox_news.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_news.Location = new System.Drawing.Point(3, 3);
            this.textBox_news.MaxAppendLineCount = 5000;
            this.textBox_news.Name = "textBox_news";
            this.textBox_news.Size = new System.Drawing.Size(332, 352);
            this.textBox_news.TabIndex = 0;
            this.textBox_news.Text = "0   0   0\n1   1   1\n2   2    2";
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.tabControl3);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainerMain.Size = new System.Drawing.Size(791, 520);
            this.splitContainerMain.SplitterDistance = 132;
            this.splitContainerMain.TabIndex = 8;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage3);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(0, 0);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(791, 132);
            this.tabControl3.TabIndex = 2;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.xyzFrameTrans7ParamControl1);
            this.tabPage3.Controls.Add(this.checkBox_IsOutSplitByTab);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(783, 106);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "转换参数";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // xyzFrameTrans7ParamControl1
            // 
            this.xyzFrameTrans7ParamControl1.Location = new System.Drawing.Point(3, 6);
            this.xyzFrameTrans7ParamControl1.Name = "xyzFrameTrans7ParamControl1";
            this.xyzFrameTrans7ParamControl1.Size = new System.Drawing.Size(574, 82);
            this.xyzFrameTrans7ParamControl1.TabIndex = 5;
            // 
            // checkBox_IsOutSplitByTab
            // 
            this.checkBox_IsOutSplitByTab.AutoSize = true;
            this.checkBox_IsOutSplitByTab.Checked = true;
            this.checkBox_IsOutSplitByTab.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_IsOutSplitByTab.Location = new System.Drawing.Point(583, 6);
            this.checkBox_IsOutSplitByTab.Name = "checkBox_IsOutSplitByTab";
            this.checkBox_IsOutSplitByTab.Size = new System.Drawing.Size(192, 16);
            this.checkBox_IsOutSplitByTab.TabIndex = 4;
            this.checkBox_IsOutSplitByTab.Text = "输出间隔为制表符，否则为逗号";
            this.checkBox_IsOutSplitByTab.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "注意：参数估计必须 3 对坐标以上。";
            // 
            // XyzFrameConvertParamEstimatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 520);
            this.Controls.Add(this.splitContainerMain);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "XyzFrameConvertParamEstimatForm";
            this.Text = "XYZ坐标转换";
            this.Load += new System.EventHandler(this.GeoXyzConvertForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button button_oldToNew;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Winform.Controls.RichTextBoxControl textBox_olds;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage2;
        private Winform.Controls.RichTextBoxControl textBox_news;
        private System.Windows.Forms.Button button_solveParams;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button button_exchange;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_IsOutSplitByTab;
        private Winform.Controls.XyzFrameTrans7ParamControl xyzFrameTrans7ParamControl1;
    }
}