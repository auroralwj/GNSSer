namespace Geo.Winform
{
    partial class AddSubProgramForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.browsebutton1 = new System.Windows.Forms.Button();
            this.pathtextBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.addbutton1 = new System.Windows.Forms.Button();
            this.cancelbutton2 = new System.Windows.Forms.Button();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.browsebutton1);
            this.groupBox1.Controls.Add(this.pathtextBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(481, 57);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息录入";
            // 
            // browsebutton1
            // 
            this.browsebutton1.Location = new System.Drawing.Point(388, 18);
            this.browsebutton1.Name = "browsebutton1";
            this.browsebutton1.Size = new System.Drawing.Size(75, 23);
            this.browsebutton1.TabIndex = 2;
            this.browsebutton1.Text = "浏览";
            this.browsebutton1.UseVisualStyleBackColor = true;
            this.browsebutton1.Click += new System.EventHandler(this.browseFilebutton1_Click);
            // 
            // pathtextBox1
            // 
            this.pathtextBox1.Location = new System.Drawing.Point(93, 20);
            this.pathtextBox1.Name = "pathtextBox1";
            this.pathtextBox1.Size = new System.Drawing.Size(284, 21);
            this.pathtextBox1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "程序地址：";
            // 
            // addbutton1
            // 
            this.addbutton1.Location = new System.Drawing.Point(337, 75);
            this.addbutton1.Name = "addbutton1";
            this.addbutton1.Size = new System.Drawing.Size(75, 23);
            this.addbutton1.TabIndex = 1;
            this.addbutton1.Text = "添加";
            this.addbutton1.UseVisualStyleBackColor = true;
            this.addbutton1.Click += new System.EventHandler(this.addbutton1_Click);
            // 
            // cancelbutton2
            // 
            this.cancelbutton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelbutton2.Location = new System.Drawing.Point(418, 75);
            this.cancelbutton2.Name = "cancelbutton2";
            this.cancelbutton2.Size = new System.Drawing.Size(75, 23);
            this.cancelbutton2.TabIndex = 2;
            this.cancelbutton2.Text = "取消";
            this.cancelbutton2.UseVisualStyleBackColor = true;
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.Filter = "图片文件(*.jpg,*.gif,*.bmp,*.png)|*.jpg;*.gif;*.bmp;*.png";
            // 
            // AddSubProgramForm
            // 
            this.AcceptButton = this.addbutton1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelbutton2;
            this.ClientSize = new System.Drawing.Size(505, 115);
            this.Controls.Add(this.cancelbutton2);
            this.Controls.Add(this.addbutton1);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddSubProgramForm";
            this.ShowIcon = false;
            this.Text = "添加子程序";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button browsebutton1;
        private System.Windows.Forms.TextBox pathtextBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button addbutton1;
        private System.Windows.Forms.Button cancelbutton2;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
    }
}