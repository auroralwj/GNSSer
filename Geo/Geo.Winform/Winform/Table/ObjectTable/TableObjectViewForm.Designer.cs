namespace Geo.Winform
{
    partial class TableObjectViewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableObjectViewForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button_selectDraw = new System.Windows.Forms.Button();
            this.button_draw = new System.Windows.Forms.Button();
            this.paramVectorRenderControl1 = new Geo.Winform.Controls.ParamVectorRenderControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.打开OToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel_reload = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton_toWord = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_toExcel = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1新窗口打开 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel_multiDraw = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel1另存为 = new System.Windows.Forms.ToolStripLabel();
            this.objectTableControl1 = new Geo.Winform.ObjectTableControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button_selectDraw);
            this.splitContainer1.Panel1.Controls.Add(this.button_draw);
            this.splitContainer1.Panel1.Controls.Add(this.paramVectorRenderControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.objectTableControl1);
            this.splitContainer1.Size = new System.Drawing.Size(805, 417);
            this.splitContainer1.SplitterDistance = 114;
            this.splitContainer1.TabIndex = 5;
            // 
            // button_selectDraw
            // 
            this.button_selectDraw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_selectDraw.Location = new System.Drawing.Point(703, 76);
            this.button_selectDraw.Name = "button_selectDraw";
            this.button_selectDraw.Size = new System.Drawing.Size(99, 33);
            this.button_selectDraw.TabIndex = 3;
            this.button_selectDraw.Text = "选择绘制";
            this.button_selectDraw.UseVisualStyleBackColor = true;
            this.button_selectDraw.Click += new System.EventHandler(this.button_selectDraw_Click);
            // 
            // button_draw
            // 
            this.button_draw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_draw.Location = new System.Drawing.Point(703, 13);
            this.button_draw.Name = "button_draw";
            this.button_draw.Size = new System.Drawing.Size(99, 57);
            this.button_draw.TabIndex = 2;
            this.button_draw.Text = "绘图";
            this.button_draw.UseVisualStyleBackColor = true;
            this.button_draw.Click += new System.EventHandler(this.button_draw_Click);
            // 
            // paramVectorRenderControl1
            // 
            this.paramVectorRenderControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.paramVectorRenderControl1.Location = new System.Drawing.Point(2, 0);
            this.paramVectorRenderControl1.Margin = new System.Windows.Forms.Padding(2);
            this.paramVectorRenderControl1.Name = "paramVectorRenderControl1";
            this.paramVectorRenderControl1.Size = new System.Drawing.Size(696, 112);
            this.paramVectorRenderControl1.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开OToolStripButton,
            this.toolStripLabel_reload,
            this.toolStripButton_toWord,
            this.toolStripButton_toExcel,
            this.toolStripLabel1新窗口打开,
            this.toolStripLabel_multiDraw,
            this.toolStripLabel1另存为});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(805, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // 打开OToolStripButton
            // 
            this.打开OToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.打开OToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("打开OToolStripButton.Image")));
            this.打开OToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.打开OToolStripButton.Name = "打开OToolStripButton";
            this.打开OToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.打开OToolStripButton.Text = "打开(&O)";
            this.打开OToolStripButton.Click += new System.EventHandler(this.打开OToolStripButton_Click);
            // 
            // toolStripLabel_reload
            // 
            this.toolStripLabel_reload.Name = "toolStripLabel_reload";
            this.toolStripLabel_reload.Size = new System.Drawing.Size(56, 22);
            this.toolStripLabel_reload.Text = "重新加载";
            this.toolStripLabel_reload.Click += new System.EventHandler(this.toolStripLabel_reload_Click);
            // 
            // toolStripButton_toWord
            // 
            this.toolStripButton_toWord.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_toWord.Image = global::Geo.Properties.Resources.doc;
            this.toolStripButton_toWord.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_toWord.Name = "toolStripButton_toWord";
            this.toolStripButton_toWord.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_toWord.Text = "Word 导出";
            this.toolStripButton_toWord.Click += new System.EventHandler(this.toolStripButton_toWord_Click);
            // 
            // toolStripButton_toExcel
            // 
            this.toolStripButton_toExcel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_toExcel.Image = global::Geo.Properties.Resources.excel_16x16;
            this.toolStripButton_toExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_toExcel.Name = "toolStripButton_toExcel";
            this.toolStripButton_toExcel.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_toExcel.Text = "Excel 导出";
            this.toolStripButton_toExcel.Click += new System.EventHandler(this.toolStripButton_toExcel_Click);
            // 
            // toolStripLabel1新窗口打开
            // 
            this.toolStripLabel1新窗口打开.Name = "toolStripLabel1新窗口打开";
            this.toolStripLabel1新窗口打开.Size = new System.Drawing.Size(68, 22);
            this.toolStripLabel1新窗口打开.Text = "新窗口打开";
            this.toolStripLabel1新窗口打开.Click += new System.EventHandler(this.toolStripLabel1新窗口打开_Click);
            // 
            // toolStripLabel_multiDraw
            // 
            this.toolStripLabel_multiDraw.Name = "toolStripLabel_multiDraw";
            this.toolStripLabel_multiDraw.Size = new System.Drawing.Size(56, 22);
            this.toolStripLabel_multiDraw.Text = "批量绘图";
            this.toolStripLabel_multiDraw.Click += new System.EventHandler(this.toolStripLabel_multiDraw_Click);
            // 
            // toolStripLabel1另存为
            // 
            this.toolStripLabel1另存为.Name = "toolStripLabel1另存为";
            this.toolStripLabel1另存为.Size = new System.Drawing.Size(44, 22);
            this.toolStripLabel1另存为.Text = "另存为";
            this.toolStripLabel1另存为.Click += new System.EventHandler(this.toolStripLabel1另存为_Click);
            // 
            // objectTableControl1
            // 
            this.objectTableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl1.Location = new System.Drawing.Point(0, 0);
            this.objectTableControl1.Name = "objectTableControl1";
            this.objectTableControl1.Size = new System.Drawing.Size(805, 299);
            this.objectTableControl1.TabIndex = 0;
            this.objectTableControl1.TableObjectStorage = null;
            // 
            // DataTableViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 442);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "DataTableViewForm";
            this.Text = "DataTable表显示";
            this.Load += new System.EventHandler(this.DataTableViewForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        protected Controls.ParamVectorRenderControl paramVectorRenderControl1;
        private System.Windows.Forms.Button button_draw;
        private System.Windows.Forms.ToolStripButton toolStripButton_toWord;
        private System.Windows.Forms.ToolStripButton toolStripButton_toExcel;
        private System.Windows.Forms.ToolStripButton 打开OToolStripButton;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1新窗口打开;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_multiDraw;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_reload;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1另存为;
        private System.Windows.Forms.Button button_selectDraw;
        private ObjectTableControl objectTableControl1;
    }
}