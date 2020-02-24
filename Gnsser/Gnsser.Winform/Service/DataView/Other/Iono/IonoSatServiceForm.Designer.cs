namespace Gnsser.Winform
{
    partial class IonoSatServiceForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.objectTableControl1 = new Geo.Winform.ObjectTableControl();
            this.button_view = new System.Windows.Forms.Button();
            this.checkBox_isRangeOrTec = new System.Windows.Forms.CheckBox();
            this.timeLoopControl1 = new Geo.Winform.Controls.TimeLoopControl();
            this.namedStringControl_coord = new Geo.Winform.Controls.NamedStringControl();
            this.namedStringControl_prn = new Geo.Winform.Controls.NamedStringControl();
            this.checkBox_freq = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_coordSet = new System.Windows.Forms.Button();
            this.namedFloatControl_satCutOff = new Geo.Winform.Controls.NamedFloatControl();
            this.enumRadioControl_IonoSerivceType = new Geo.Winform.EnumRadioControl();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(-3, 209);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(811, 252);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.objectTableControl1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(803, 226);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "显示";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // objectTableControl1
            // 
            this.objectTableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl1.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl1.Name = "objectTableControl1";
            this.objectTableControl1.Size = new System.Drawing.Size(797, 220);
            this.objectTableControl1.TabIndex = 1;
            this.objectTableControl1.TableObjectStorage = null;
            // 
            // button_view
            // 
            this.button_view.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_view.Location = new System.Drawing.Point(696, 25);
            this.button_view.Name = "button_view";
            this.button_view.Size = new System.Drawing.Size(101, 48);
            this.button_view.TabIndex = 3;
            this.button_view.Text = "查看";
            this.button_view.UseVisualStyleBackColor = true;
            this.button_view.Click += new System.EventHandler(this.button_view_Click);
            // 
            // checkBox_isRangeOrTec
            // 
            this.checkBox_isRangeOrTec.AutoSize = true;
            this.checkBox_isRangeOrTec.Checked = true;
            this.checkBox_isRangeOrTec.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_isRangeOrTec.Location = new System.Drawing.Point(103, 140);
            this.checkBox_isRangeOrTec.Name = "checkBox_isRangeOrTec";
            this.checkBox_isRangeOrTec.Size = new System.Drawing.Size(120, 16);
            this.checkBox_isRangeOrTec.TabIndex = 5;
            this.checkBox_isRangeOrTec.Text = "延迟斜距或电子数";
            this.checkBox_isRangeOrTec.UseVisualStyleBackColor = true;
            // 
            // timeLoopControl1
            // 
            this.timeLoopControl1.Location = new System.Drawing.Point(56, 70);
            this.timeLoopControl1.Margin = new System.Windows.Forms.Padding(2);
            this.timeLoopControl1.Name = "timeLoopControl1";
            this.timeLoopControl1.Size = new System.Drawing.Size(578, 30);
            this.timeLoopControl1.TabIndex = 6;
            // 
            // namedStringControl_coord
            // 
            this.namedStringControl_coord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.namedStringControl_coord.Location = new System.Drawing.Point(12, 43);
            this.namedStringControl_coord.Name = "namedStringControl_coord";
            this.namedStringControl_coord.Size = new System.Drawing.Size(591, 23);
            this.namedStringControl_coord.TabIndex = 0;
            this.namedStringControl_coord.Title = "测站坐标(XYZ)：";
            // 
            // namedStringControl_prn
            // 
            this.namedStringControl_prn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.namedStringControl_prn.Location = new System.Drawing.Point(12, 14);
            this.namedStringControl_prn.Name = "namedStringControl_prn";
            this.namedStringControl_prn.Size = new System.Drawing.Size(656, 23);
            this.namedStringControl_prn.TabIndex = 0;
            this.namedStringControl_prn.Title = "卫星编号：";
            // 
            // checkBox_freq
            // 
            this.checkBox_freq.AutoSize = true;
            this.checkBox_freq.Checked = true;
            this.checkBox_freq.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_freq.Location = new System.Drawing.Point(262, 140);
            this.checkBox_freq.Name = "checkBox_freq";
            this.checkBox_freq.Size = new System.Drawing.Size(120, 16);
            this.checkBox_freq.TabIndex = 5;
            this.checkBox_freq.Text = "GPS L1或 L2 频率";
            this.checkBox_freq.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(101, 169);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "电子数量，单位 1e16";
            // 
            // button_coordSet
            // 
            this.button_coordSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_coordSet.Location = new System.Drawing.Point(610, 44);
            this.button_coordSet.Name = "button_coordSet";
            this.button_coordSet.Size = new System.Drawing.Size(75, 23);
            this.button_coordSet.TabIndex = 8;
            this.button_coordSet.Text = "设置坐标";
            this.button_coordSet.UseVisualStyleBackColor = true;
            this.button_coordSet.Click += new System.EventHandler(this.button_coordSet_Click);
            // 
            // namedFloatControl_satCutOff
            // 
            this.namedFloatControl_satCutOff.Location = new System.Drawing.Point(12, 105);
            this.namedFloatControl_satCutOff.Name = "namedFloatControl_satCutOff";
            this.namedFloatControl_satCutOff.Size = new System.Drawing.Size(223, 23);
            this.namedFloatControl_satCutOff.TabIndex = 9;
            this.namedFloatControl_satCutOff.Title = "卫星高度截止角(度)：";
            this.namedFloatControl_satCutOff.Value = 0.1D;
            // 
            // enumRadioControl_IonoSerivceType
            // 
            this.enumRadioControl_IonoSerivceType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.enumRadioControl_IonoSerivceType.Location = new System.Drawing.Point(406, 105);
            this.enumRadioControl_IonoSerivceType.Name = "enumRadioControl_IonoSerivceType";
            this.enumRadioControl_IonoSerivceType.Size = new System.Drawing.Size(395, 79);
            this.enumRadioControl_IonoSerivceType.TabIndex = 10;
            this.enumRadioControl_IonoSerivceType.Title = "选项";
            // 
            // IonoSatServiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 465);
            this.Controls.Add(this.enumRadioControl_IonoSerivceType);
            this.Controls.Add(this.namedFloatControl_satCutOff);
            this.Controls.Add(this.button_coordSet);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.timeLoopControl1);
            this.Controls.Add(this.checkBox_freq);
            this.Controls.Add(this.checkBox_isRangeOrTec);
            this.Controls.Add(this.button_view);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.namedStringControl_coord);
            this.Controls.Add(this.namedStringControl_prn);
            this.Name = "IonoSatServiceForm";
            this.Text = "电离层卫星服务";
            this.Load += new System.EventHandler(this.IonoDcbViewerForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Geo.Winform.Controls.NamedStringControl namedStringControl_prn;
        private Geo.Winform.ObjectTableControl objectTableControl1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button button_view;
        private System.Windows.Forms.CheckBox checkBox_isRangeOrTec;
        private Geo.Winform.Controls.NamedStringControl namedStringControl_coord;
        private Geo.Winform.Controls.TimeLoopControl timeLoopControl1;
        private System.Windows.Forms.CheckBox checkBox_freq;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_coordSet;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_satCutOff;
        private Geo.Winform.EnumRadioControl enumRadioControl_IonoSerivceType;
    }
}