namespace Geo.Winform.Tools
{
    partial class XyzNeuConvertForm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button_xyzToNeu = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.splitContainer_up = new System.Windows.Forms.SplitContainer();
            this.richTextBox_originEcefXyz = new Geo.Winform.Controls.RichTextBoxControl();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_exchange = new System.Windows.Forms.Button();
            this.richTextBox_targetEcefXyz = new Geo.Winform.Controls.RichTextBoxControl();
            this.splitContainer_main = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.objectTableControl1 = new Geo.Winform.ObjectTableControl();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_up)).BeginInit();
            this.splitContainer_up.Panel1.SuspendLayout();
            this.splitContainer_up.Panel2.SuspendLayout();
            this.splitContainer_up.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).BeginInit();
            this.splitContainer_main.Panel1.SuspendLayout();
            this.splitContainer_main.Panel2.SuspendLayout();
            this.splitContainer_main.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.objectTableControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.SuspendLayout();
            // 
            // button_xyzToNeu
            // 
            this.button_xyzToNeu.Location = new System.Drawing.Point(23, 144);
            this.button_xyzToNeu.Name = "button_xyzToNeu";
            this.button_xyzToNeu.Size = new System.Drawing.Size(75, 31);
            this.button_xyzToNeu.TabIndex = 6;
            this.button_xyzToNeu.Text = "计算";
            this.button_xyzToNeu.UseVisualStyleBackColor = true;
            this.button_xyzToNeu.Click += new System.EventHandler(this.button_xyzToNeu_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "地心原坐标";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "地心目标坐标";
            // 
            // splitContainer_up
            // 
            this.splitContainer_up.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_up.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_up.Name = "splitContainer_up";
            // 
            // splitContainer_up.Panel1
            // 
            this.splitContainer_up.Panel1.Controls.Add(this.richTextBox_originEcefXyz);
            this.splitContainer_up.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer_up.Panel2
            // 
            this.splitContainer_up.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer_up.Size = new System.Drawing.Size(678, 244);
            this.splitContainer_up.SplitterDistance = 254;
            this.splitContainer_up.TabIndex = 8;
            // 
            // richTextBox_originEcefXyz
            // 
            this.richTextBox_originEcefXyz.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_originEcefXyz.Location = new System.Drawing.Point(0, 12);
            this.richTextBox_originEcefXyz.MaxAppendLineCount = 5000;
            this.richTextBox_originEcefXyz.Name = "richTextBox_originEcefXyz";
            this.richTextBox_originEcefXyz.Size = new System.Drawing.Size(254, 232);
            this.richTextBox_originEcefXyz.TabIndex = 8;
            this.richTextBox_originEcefXyz.Text = "10000000,1000000,1000000";
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
            this.splitContainer2.Panel1.Controls.Add(this.label5);
            this.splitContainer2.Panel1.Controls.Add(this.label4);
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            this.splitContainer2.Panel1.Controls.Add(this.button_exchange);
            this.splitContainer2.Panel1.Controls.Add(this.button_xyzToNeu);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.richTextBox_targetEcefXyz);
            this.splitContainer2.Panel2.Controls.Add(this.label3);
            this.splitContainer2.Size = new System.Drawing.Size(420, 244);
            this.splitContainer2.SplitterDistance = 127;
            this.splitContainer2.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(53, 204);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(11, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "V";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(53, 195);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "V";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 185);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "|";
            // 
            // button_exchange
            // 
            this.button_exchange.Location = new System.Drawing.Point(23, 21);
            this.button_exchange.Name = "button_exchange";
            this.button_exchange.Size = new System.Drawing.Size(75, 31);
            this.button_exchange.TabIndex = 3;
            this.button_exchange.Text = "<- 交换 ->";
            this.button_exchange.UseVisualStyleBackColor = true;
            this.button_exchange.Click += new System.EventHandler(this.button_exchangeXyz_Click);
            // 
            // richTextBox_targetEcefXyz
            // 
            this.richTextBox_targetEcefXyz.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_targetEcefXyz.Location = new System.Drawing.Point(0, 12);
            this.richTextBox_targetEcefXyz.MaxAppendLineCount = 5000;
            this.richTextBox_targetEcefXyz.Name = "richTextBox_targetEcefXyz";
            this.richTextBox_targetEcefXyz.Size = new System.Drawing.Size(289, 232);
            this.richTextBox_targetEcefXyz.TabIndex = 8;
            this.richTextBox_targetEcefXyz.Text = "10000001,1000001,1000001";
            // 
            // splitContainer_main
            // 
            this.splitContainer_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_main.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_main.Name = "splitContainer_main";
            this.splitContainer_main.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_main.Panel1
            // 
            this.splitContainer_main.Panel1.Controls.Add(this.splitContainer_up);
            // 
            // splitContainer_main.Panel2
            // 
            this.splitContainer_main.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer_main.Size = new System.Drawing.Size(678, 500);
            this.splitContainer_main.SplitterDistance = 244;
            this.splitContainer_main.TabIndex = 9;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.objectTableControl1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(678, 252);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "结果";
            // 
            // objectTableControl1
            // 
            this.objectTableControl1.Controls.Add(this.bindingNavigator1);
            this.objectTableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl1.Location = new System.Drawing.Point(3, 17);
            this.objectTableControl1.Name = "objectTableControl1";
            this.objectTableControl1.Size = new System.Drawing.Size(672, 232);
            this.objectTableControl1.TabIndex = 1;
            this.objectTableControl1.TableObjectStorage = null;
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.CountItem = null;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 207);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.Size = new System.Drawing.Size(672, 25);
            this.bindingNavigator1.TabIndex = 1;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // XyzNeuConvertForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 500);
            this.Controls.Add(this.splitContainer_main);
            this.Name = "XyzNeuConvertForm";
            this.Text = "地心XYZ与站星坐标的计算";
            this.splitContainer_up.Panel1.ResumeLayout(false);
            this.splitContainer_up.Panel1.PerformLayout();
            this.splitContainer_up.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_up)).EndInit();
            this.splitContainer_up.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer_main.Panel1.ResumeLayout(false);
            this.splitContainer_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).EndInit();
            this.splitContainer_main.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.objectTableControl1.ResumeLayout(false);
            this.objectTableControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_xyzToNeu;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SplitContainer splitContainer_up;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer_main;
        private System.Windows.Forms.GroupBox groupBox2;
        private ObjectTableControl objectTableControl1;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.Button button_exchange;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private Controls.RichTextBoxControl richTextBox_originEcefXyz;
        private Controls.RichTextBoxControl richTextBox_targetEcefXyz;
    }
}

