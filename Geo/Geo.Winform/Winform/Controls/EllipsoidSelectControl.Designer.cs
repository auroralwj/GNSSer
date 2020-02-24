namespace Geo.Winform
{
    partial class EllipsoidSelectControl
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
            this.panel_ellipsiodParam = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_flattonOrAnt = new System.Windows.Forms.TextBox();
            this.textBox_semiMajor = new System.Windows.Forms.TextBox();
            this.enumRadioControl_ellipsoidType = new Geo.Winform.EnumRadioControl();
            this.checkBox_inputParam = new System.Windows.Forms.CheckBox();
            this.panel_ellipsiodParam.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_ellipsiodParam
            // 
            this.panel_ellipsiodParam.Controls.Add(this.label3);
            this.panel_ellipsiodParam.Controls.Add(this.label2);
            this.panel_ellipsiodParam.Controls.Add(this.textBox_flattonOrAnt);
            this.panel_ellipsiodParam.Controls.Add(this.textBox_semiMajor);
            this.panel_ellipsiodParam.Location = new System.Drawing.Point(80, 48);
            this.panel_ellipsiodParam.Margin = new System.Windows.Forms.Padding(2);
            this.panel_ellipsiodParam.Name = "panel_ellipsiodParam";
            this.panel_ellipsiodParam.Size = new System.Drawing.Size(438, 34);
            this.panel_ellipsiodParam.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(194, 11);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "扁率或扁率倒数：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 11);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "椭球长半径(米)：";
            // 
            // textBox_flattonOrAnt
            // 
            this.textBox_flattonOrAnt.Location = new System.Drawing.Point(299, 7);
            this.textBox_flattonOrAnt.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_flattonOrAnt.Name = "textBox_flattonOrAnt";
            this.textBox_flattonOrAnt.Size = new System.Drawing.Size(126, 21);
            this.textBox_flattonOrAnt.TabIndex = 2;
            this.textBox_flattonOrAnt.Text = "298.257223563";
            // 
            // textBox_semiMajor
            // 
            this.textBox_semiMajor.Location = new System.Drawing.Point(106, 7);
            this.textBox_semiMajor.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_semiMajor.Name = "textBox_semiMajor";
            this.textBox_semiMajor.Size = new System.Drawing.Size(72, 21);
            this.textBox_semiMajor.TabIndex = 2;
            this.textBox_semiMajor.Text = "6378137";
            // 
            // enumRadioControl_ellipsoidType
            // 
            this.enumRadioControl_ellipsoidType.Dock = System.Windows.Forms.DockStyle.Top;
            this.enumRadioControl_ellipsoidType.IsReady = false;
            this.enumRadioControl_ellipsoidType.Location = new System.Drawing.Point(0, 0);
            this.enumRadioControl_ellipsoidType.Name = "enumRadioControl_ellipsoidType";
            this.enumRadioControl_ellipsoidType.Size = new System.Drawing.Size(520, 48);
            this.enumRadioControl_ellipsoidType.TabIndex = 4;
            this.enumRadioControl_ellipsoidType.Title = "椭球选择";
            // 
            // checkBox_inputParam
            // 
            this.checkBox_inputParam.AutoSize = true;
            this.checkBox_inputParam.Location = new System.Drawing.Point(6, 54);
            this.checkBox_inputParam.Name = "checkBox_inputParam";
            this.checkBox_inputParam.Size = new System.Drawing.Size(72, 16);
            this.checkBox_inputParam.TabIndex = 6;
            this.checkBox_inputParam.Text = "手动椭球";
            this.checkBox_inputParam.UseVisualStyleBackColor = true;
            this.checkBox_inputParam.CheckedChanged += new System.EventHandler(this.checkBox_inputParam_CheckedChanged);
            // 
            // EllipsoidSelectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox_inputParam);
            this.Controls.Add(this.panel_ellipsiodParam);
            this.Controls.Add(this.enumRadioControl_ellipsoidType);
            this.Name = "EllipsoidSelectControl";
            this.Size = new System.Drawing.Size(520, 83);
            this.Load += new System.EventHandler(this.EllipsoidSelectControl_Load);
            this.panel_ellipsiodParam.ResumeLayout(false);
            this.panel_ellipsiodParam.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel_ellipsiodParam;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_flattonOrAnt;
        private System.Windows.Forms.TextBox textBox_semiMajor;
        private EnumRadioControl enumRadioControl_ellipsoidType;
        private System.Windows.Forms.CheckBox checkBox_inputParam;
    }
}
