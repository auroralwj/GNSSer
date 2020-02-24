namespace Gnsser.Winform
{
    partial class MwWideLaneSolverForm
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
            this.baseSatSelectingControl1 = new Gnsser.Winform.Controls.BaseSatSelectingControl();
            this.checkBox_setWeightWithSat = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_MaxError = new System.Windows.Forms.TextBox();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage0
            // 
            this.tabPage0.Size = new System.Drawing.Size(655, 137);
            // 
            // panel3
            // 
            this.panel3.Size = new System.Drawing.Size(649, 120);
            // 
            // panel2
            // 
            this.panel2.Size = new System.Drawing.Size(649, 120);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.textBox_MaxError);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.checkBox_setWeightWithSat);
            this.panel4.Controls.Add(this.baseSatSelectingControl1);
            this.panel4.Size = new System.Drawing.Size(649, 120);
            this.panel4.Controls.SetChildIndex(this.baseSatSelectingControl1, 0);
            this.panel4.Controls.SetChildIndex(this.checkBox_setWeightWithSat, 0);
            this.panel4.Controls.SetChildIndex(this.label1, 0);
            this.panel4.Controls.SetChildIndex(this.textBox_MaxError, 0);
            // 
            // baseSatSelectingControl1
            // 
            this.baseSatSelectingControl1.EnableBaseSat = false;
            this.baseSatSelectingControl1.Location = new System.Drawing.Point(116, 4);
            this.baseSatSelectingControl1.Name = "baseSatSelectingControl1";
            this.baseSatSelectingControl1.Size = new System.Drawing.Size(279, 85);
            this.baseSatSelectingControl1.TabIndex = 17;
            // 
            // checkBox_setWeightWithSat
            // 
            this.checkBox_setWeightWithSat.AutoSize = true;
            this.checkBox_setWeightWithSat.Location = new System.Drawing.Point(407, 59);
            this.checkBox_setWeightWithSat.Name = "checkBox_setWeightWithSat";
            this.checkBox_setWeightWithSat.Size = new System.Drawing.Size(144, 16);
            this.checkBox_setWeightWithSat.TabIndex = 57;
            this.checkBox_setWeightWithSat.Text = "卫星高度角和距离定权";
            this.checkBox_setWeightWithSat.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(394, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 58;
            this.label1.Text = "最大误差：";
            // 
            // textBox_MaxError
            // 
            this.textBox_MaxError.Location = new System.Drawing.Point(465, 82);
            this.textBox_MaxError.Name = "textBox_MaxError";
            this.textBox_MaxError.Size = new System.Drawing.Size(100, 21);
            this.textBox_MaxError.TabIndex = 59;
            this.textBox_MaxError.Text = "1";
            // 
            // MwWideLaneSolverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 405);
            this.IsShowProgressBar = true;
            this.Name = "MwWideLaneSolverForm";
            this.Text = "宽项模糊度计算器";
            this.Load += new System.EventHandler(this.WideLaneSolverForm_Load);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.BaseSatSelectingControl baseSatSelectingControl1;
        private System.Windows.Forms.CheckBox checkBox_setWeightWithSat;
        private System.Windows.Forms.TextBox textBox_MaxError;
        private System.Windows.Forms.Label label1;


    }
}