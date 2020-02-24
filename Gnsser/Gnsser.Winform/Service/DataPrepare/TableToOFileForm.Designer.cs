namespace Gnsser.Winform
{
    partial class TableToOFileForm
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
            this.namedFloatControl1Vertion = new Geo.Winform.Controls.NamedFloatControl();
            this.panel4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage0
            // 
            this.tabPage0.Size = new System.Drawing.Size(567, 240);
            // 
            // tabPage2 
            // 
            // panel3
            // 
            this.panel3.Size = new System.Drawing.Size(593, 120);
            // 
            // panel2
            // 
            this.panel2.Size = new System.Drawing.Size(593, 120);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.namedFloatControl1Vertion);
            this.panel4.Size = new System.Drawing.Size(593, 120);
            this.panel4.Controls.SetChildIndex(this.namedFloatControl1Vertion, 0);
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl_inputPathes.FilePathes = new string[0];
            this.fileOpenControl_inputPathes.Size = new System.Drawing.Size(561, 98);
            // 
            // tabPage1
            // 
            this.tabPage1.Size = new System.Drawing.Size(567, 126);
            // 
            // namedFloatControl1Vertion
            // 
            this.namedFloatControl1Vertion.Location = new System.Drawing.Point(17, 67);
            this.namedFloatControl1Vertion.Name = "namedFloatControl1Vertion";
            this.namedFloatControl1Vertion.Size = new System.Drawing.Size(140, 23);
            this.namedFloatControl1Vertion.TabIndex = 58;
            this.namedFloatControl1Vertion.Title = "输出版本：";
            this.namedFloatControl1Vertion.Value = 3.02D;
            // 
            // TableToOFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 508);
            this.IsShowProgressBar = true;
            this.Name = "TableToOFileForm";
            this.Text = "表文件转换为Rinex观测文件";
            this.Load += new System.EventHandler(this.TableToOFileForm_Load);
            this.panel4.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Geo.Winform.Controls.NamedFloatControl namedFloatControl1Vertion;



    }
}

