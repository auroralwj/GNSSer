namespace Gnsser.Winform
{
    partial class OFileFormaterForm
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
            this.tabPage1.SuspendLayout();
            this.panel_buttons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage0
            // 
            this.tabPage0.Size = new System.Drawing.Size(647, 248);
            // 
            // panel3
            // 
            this.panel3.Size = new System.Drawing.Size(641, 123);
            // 
            // panel2
            // 
            this.panel2.Size = new System.Drawing.Size(641, 123);
            // 
            // panel4
            // 
            this.panel4.Size = new System.Drawing.Size(641, 123);
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl_inputPathes.FilePathes = new string[0];
            this.fileOpenControl_inputPathes.Size = new System.Drawing.Size(641, 101);
            // 
            // tabPage1
            // 
            this.tabPage1.Size = new System.Drawing.Size(647, 129);
            // 
            // panel_buttons
            // 
            this.panel_buttons.Location = new System.Drawing.Point(388, 0);
            // 
            // OFileFormaterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 505);
            this.IsShowProgressBar = true;
            this.Name = "OFileFormaterForm";
            this.Text = "格式化转换Rinex观测文件";
            this.tabPage1.ResumeLayout(false);
            this.panel_buttons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion


    }
}

