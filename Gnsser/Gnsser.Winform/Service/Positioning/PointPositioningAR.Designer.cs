namespace Gnsser.Winform
{
    partial class PointPositioningAR
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_obsPath = new System.Windows.Forms.TextBox();
            this.button_calWLFCBs = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.openFileDialog_obs = new System.Windows.Forms.OpenFileDialog();
            this.button_getObsPath = new System.Windows.Forms.Button();
            this.button_readObs = new System.Windows.Forms.Button();
            this.bindingSource_SDWL = new System.Windows.Forms.BindingSource(this.components);
            this.comboBox_sat = new System.Windows.Forms.ComboBox();
            this.checkBox_BD = new System.Windows.Forms.CheckBox();
            this.checkBox_GPS = new System.Windows.Forms.CheckBox();
            this.dataGridView_SDWL = new System.Windows.Forms.DataGridView();
            this.button_view = new System.Windows.Forms.Button();
            this.dataGridView_UPD = new System.Windows.Forms.DataGridView();
            this.bindingSource_UPD = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_SDWL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_SDWL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_UPD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_UPD)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "观测文件：";
            // 
            // textBox_obsPath
            // 
            this.textBox_obsPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_obsPath.Location = new System.Drawing.Point(106, 22);
            this.textBox_obsPath.Name = "textBox_obsPath";
            this.textBox_obsPath.Size = new System.Drawing.Size(665, 21);
            this.textBox_obsPath.TabIndex = 1;
            this.textBox_obsPath.Text = "E:\\TestData\\2013001sinex中有\\dgar0010.13o";
            // 
            // button_calWLFCBs
            // 
            this.button_calWLFCBs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_calWLFCBs.Location = new System.Drawing.Point(669, 68);
            this.button_calWLFCBs.Name = "button_calWLFCBs";
            this.button_calWLFCBs.Size = new System.Drawing.Size(103, 22);
            this.button_calWLFCBs.TabIndex = 2;
            this.button_calWLFCBs.Text = "计算宽巷FCBs";
            this.button_calWLFCBs.UseVisualStyleBackColor = true;
            this.button_calWLFCBs.Click += new System.EventHandler(this.button_calWLFCBs_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "参考星：";
            // 
            // openFileDialog_obs
            // 
            this.openFileDialog_obs.FileName = "openFileDialog1";
            // 
            // button_getObsPath
            // 
            this.button_getObsPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getObsPath.Location = new System.Drawing.Point(790, 20);
            this.button_getObsPath.Name = "button_getObsPath";
            this.button_getObsPath.Size = new System.Drawing.Size(75, 23);
            this.button_getObsPath.TabIndex = 5;
            this.button_getObsPath.Text = "…";
            this.button_getObsPath.UseVisualStyleBackColor = true;
            this.button_getObsPath.Click += new System.EventHandler(this.button_getObsPath_Click);
            // 
            // button_readObs
            // 
            this.button_readObs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_readObs.Location = new System.Drawing.Point(567, 68);
            this.button_readObs.Name = "button_readObs";
            this.button_readObs.Size = new System.Drawing.Size(75, 23);
            this.button_readObs.TabIndex = 6;
            this.button_readObs.Text = "read";
            this.button_readObs.UseVisualStyleBackColor = true;
            this.button_readObs.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox_sat
            // 
            this.comboBox_sat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_sat.DataSource = this.bindingSource_SDWL;
            this.comboBox_sat.FormattingEnabled = true;
            this.comboBox_sat.Location = new System.Drawing.Point(106, 69);
            this.comboBox_sat.Name = "comboBox_sat";
            this.comboBox_sat.Size = new System.Drawing.Size(250, 20);
            this.comboBox_sat.TabIndex = 7;
            // 
            // checkBox_BD
            // 
            this.checkBox_BD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_BD.AutoSize = true;
            this.checkBox_BD.Location = new System.Drawing.Point(426, 73);
            this.checkBox_BD.Name = "checkBox_BD";
            this.checkBox_BD.Size = new System.Drawing.Size(48, 16);
            this.checkBox_BD.TabIndex = 8;
            this.checkBox_BD.Text = "北斗";
            this.checkBox_BD.UseVisualStyleBackColor = true;
            // 
            // checkBox_GPS
            // 
            this.checkBox_GPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_GPS.AutoSize = true;
            this.checkBox_GPS.Checked = true;
            this.checkBox_GPS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_GPS.Location = new System.Drawing.Point(501, 72);
            this.checkBox_GPS.Name = "checkBox_GPS";
            this.checkBox_GPS.Size = new System.Drawing.Size(42, 16);
            this.checkBox_GPS.TabIndex = 9;
            this.checkBox_GPS.Text = "GPS";
            this.checkBox_GPS.UseVisualStyleBackColor = true;
            // 
            // dataGridView_SDWL
            // 
            this.dataGridView_SDWL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_SDWL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_SDWL.Location = new System.Drawing.Point(37, 109);
            this.dataGridView_SDWL.Name = "dataGridView_SDWL";
            this.dataGridView_SDWL.RowTemplate.Height = 23;
            this.dataGridView_SDWL.Size = new System.Drawing.Size(570, 425);
            this.dataGridView_SDWL.TabIndex = 10;
            // 
            // button_view
            // 
            this.button_view.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_view.Location = new System.Drawing.Point(790, 68);
            this.button_view.Name = "button_view";
            this.button_view.Size = new System.Drawing.Size(75, 23);
            this.button_view.TabIndex = 11;
            this.button_view.Text = "查看";
            this.button_view.UseVisualStyleBackColor = true;
            this.button_view.Click += new System.EventHandler(this.button_view_Click);
            // 
            // dataGridView_UPD
            // 
            this.dataGridView_UPD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_UPD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_UPD.Location = new System.Drawing.Point(625, 109);
            this.dataGridView_UPD.Name = "dataGridView_UPD";
            this.dataGridView_UPD.RowTemplate.Height = 23;
            this.dataGridView_UPD.Size = new System.Drawing.Size(240, 425);
            this.dataGridView_UPD.TabIndex = 12;
            // 
            // PointPositioningAR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 546);
            this.Controls.Add(this.dataGridView_UPD);
            this.Controls.Add(this.button_view);
            this.Controls.Add(this.dataGridView_SDWL);
            this.Controls.Add(this.checkBox_GPS);
            this.Controls.Add(this.checkBox_BD);
            this.Controls.Add(this.comboBox_sat);
            this.Controls.Add(this.button_readObs);
            this.Controls.Add(this.button_getObsPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_calWLFCBs);
            this.Controls.Add(this.textBox_obsPath);
            this.Controls.Add(this.label1);
            this.Name = "PointPositioningAR";
            this.Text = "PointPositioningAR";
            this.Load += new System.EventHandler(this.PointPositioningAR_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_SDWL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_SDWL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_UPD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_UPD)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_obsPath;
        private System.Windows.Forms.Button button_calWLFCBs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog_obs;
        private System.Windows.Forms.Button button_getObsPath;
        private System.Windows.Forms.Button button_readObs;
        private System.Windows.Forms.BindingSource bindingSource_SDWL;
        private System.Windows.Forms.ComboBox comboBox_sat;
        private System.Windows.Forms.CheckBox checkBox_BD;
        private System.Windows.Forms.CheckBox checkBox_GPS;
        private System.Windows.Forms.DataGridView dataGridView_SDWL;
        private System.Windows.Forms.Button button_view;
        private System.Windows.Forms.DataGridView dataGridView_UPD;
        private System.Windows.Forms.BindingSource bindingSource_UPD;
    }
}