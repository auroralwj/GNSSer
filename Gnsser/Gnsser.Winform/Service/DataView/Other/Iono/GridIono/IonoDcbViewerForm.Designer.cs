namespace Gnsser.Winform
{
    partial class IonoDcbViewerForm
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
            this.namedStringControl1 = new Geo.Winform.Controls.NamedStringControl();
            this.objectTableControl1 = new Geo.Winform.ObjectTableControl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button_view = new System.Windows.Forms.Button();
            this.timePeriodControl1 = new Geo.Winform.Controls.TimePeriodControl();
            this.checkBox_valOrRMS = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // namedStringControl1
            // 
            this.namedStringControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.namedStringControl1.Location = new System.Drawing.Point(12, 14);
            this.namedStringControl1.Name = "namedStringControl1";
            this.namedStringControl1.Size = new System.Drawing.Size(647, 23);
            this.namedStringControl1.TabIndex = 0;
            this.namedStringControl1.Title = "测站或卫星名称：";
            // 
            // objectTableControl1
            // 
            this.objectTableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl1.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl1.Name = "objectTableControl1";
            this.objectTableControl1.Size = new System.Drawing.Size(788, 318);
            this.objectTableControl1.TabIndex = 1;
            this.objectTableControl1.TableObjectStorage = null;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(-3, 96);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(802, 350);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.objectTableControl1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(794, 324);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "显示";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button_view
            // 
            this.button_view.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_view.Location = new System.Drawing.Point(687, 25);
            this.button_view.Name = "button_view";
            this.button_view.Size = new System.Drawing.Size(101, 48);
            this.button_view.TabIndex = 3;
            this.button_view.Text = "查看";
            this.button_view.UseVisualStyleBackColor = true;
            this.button_view.Click += new System.EventHandler(this.button_view_Click);
            // 
            // timePeriodControl1
            // 
            this.timePeriodControl1.Location = new System.Drawing.Point(63, 60);
            this.timePeriodControl1.Margin = new System.Windows.Forms.Padding(2);
            this.timePeriodControl1.Name = "timePeriodControl1";
            this.timePeriodControl1.Size = new System.Drawing.Size(407, 24);
            this.timePeriodControl1.TabIndex = 4;
            this.timePeriodControl1.TimeFrom = new System.DateTime(2018, 5, 15, 17, 22, 38, 468);
            this.timePeriodControl1.TimeStringFormat = "yyyy-MM-dd HH:mm";
            this.timePeriodControl1.TimeTo = new System.DateTime(2018, 5, 15, 17, 22, 38, 475);
            this.timePeriodControl1.Title = "时段：";
            // 
            // checkBox_valOrRMS
            // 
            this.checkBox_valOrRMS.AutoSize = true;
            this.checkBox_valOrRMS.Checked = true;
            this.checkBox_valOrRMS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_valOrRMS.Location = new System.Drawing.Point(503, 68);
            this.checkBox_valOrRMS.Name = "checkBox_valOrRMS";
            this.checkBox_valOrRMS.Size = new System.Drawing.Size(78, 16);
            this.checkBox_valOrRMS.TabIndex = 5;
            this.checkBox_valOrRMS.Text = "数值或RMS";
            this.checkBox_valOrRMS.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(113, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "注意：测站名称为四个字的小写字母";
            // 
            // IonoDcbViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox_valOrRMS);
            this.Controls.Add(this.timePeriodControl1);
            this.Controls.Add(this.button_view);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.namedStringControl1);
            this.Name = "IonoDcbViewerForm";
            this.Text = "IonoDcbViewerForm";
            this.Load += new System.EventHandler(this.IonoDcbViewerForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Geo.Winform.Controls.NamedStringControl namedStringControl1;
        private Geo.Winform.ObjectTableControl objectTableControl1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button button_view;
        private Geo.Winform.Controls.TimePeriodControl timePeriodControl1;
        private System.Windows.Forms.CheckBox checkBox_valOrRMS;
        private System.Windows.Forms.Label label1;
    }
}