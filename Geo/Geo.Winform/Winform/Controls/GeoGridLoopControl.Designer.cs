namespace Geo.Winform.Controls
{
    partial class GeoGridLoopControl
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBox_lonOrder = new System.Windows.Forms.CheckBox();
            this.checkBox_latOrder = new System.Windows.Forms.CheckBox();
            this.checkBox_latFirst = new System.Windows.Forms.CheckBox();
            this.floatSpanControl_lon = new Geo.Winform.Controls.FloatSpanControl();
            this.namedFloatControl_latStep = new Geo.Winform.Controls.NamedFloatControl();
            this.floatSpanControl_lat = new Geo.Winform.Controls.FloatSpanControl();
            this.namedFloatControl_lonStep = new Geo.Winform.Controls.NamedFloatControl();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.96866F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.03134F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 93F));
            this.tableLayoutPanel1.Controls.Add(this.floatSpanControl_lon, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.namedFloatControl_latStep, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.floatSpanControl_lat, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.namedFloatControl_lonStep, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.checkBox_lonOrder, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.checkBox_latOrder, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.checkBox_latFirst, 3, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(540, 57);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // checkBox_lonOrder
            // 
            this.checkBox_lonOrder.AutoSize = true;
            this.checkBox_lonOrder.Location = new System.Drawing.Point(389, 3);
            this.checkBox_lonOrder.Name = "checkBox_lonOrder";
            this.checkBox_lonOrder.Size = new System.Drawing.Size(48, 16);
            this.checkBox_lonOrder.TabIndex = 12;
            this.checkBox_lonOrder.Text = "逆序";
            this.checkBox_lonOrder.UseVisualStyleBackColor = true;
            // 
            // checkBox_latOrder
            // 
            this.checkBox_latOrder.AutoSize = true;
            this.checkBox_latOrder.Checked = true;
            this.checkBox_latOrder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_latOrder.Location = new System.Drawing.Point(389, 31);
            this.checkBox_latOrder.Name = "checkBox_latOrder";
            this.checkBox_latOrder.Size = new System.Drawing.Size(48, 16);
            this.checkBox_latOrder.TabIndex = 13;
            this.checkBox_latOrder.Text = "逆序";
            this.checkBox_latOrder.UseVisualStyleBackColor = true;
            // 
            // checkBox_latFirst
            // 
            this.checkBox_latFirst.AutoSize = true;
            this.checkBox_latFirst.Checked = true;
            this.checkBox_latFirst.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_latFirst.Location = new System.Drawing.Point(449, 31);
            this.checkBox_latFirst.Name = "checkBox_latFirst";
            this.checkBox_latFirst.Size = new System.Drawing.Size(72, 16);
            this.checkBox_latFirst.TabIndex = 14;
            this.checkBox_latFirst.Text = "纬度优先";
            this.checkBox_latFirst.UseVisualStyleBackColor = true;
            // 
            // floatSpanControl_lon
            // 
            this.floatSpanControl_lon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.floatSpanControl_lon.From = -180D;
            this.floatSpanControl_lon.Location = new System.Drawing.Point(3, 3);
            this.floatSpanControl_lon.Name = "floatSpanControl_lon";
            this.floatSpanControl_lon.Size = new System.Drawing.Size(229, 22);
            this.floatSpanControl_lon.TabIndex = 11;
            this.floatSpanControl_lon.Title = "经度范围(度)：";
            this.floatSpanControl_lon.To = 180D;
            // 
            // namedFloatControl_latStep
            // 
            this.namedFloatControl_latStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.namedFloatControl_latStep.Location = new System.Drawing.Point(238, 31);
            this.namedFloatControl_latStep.Name = "namedFloatControl_latStep";
            this.namedFloatControl_latStep.Size = new System.Drawing.Size(145, 23);
            this.namedFloatControl_latStep.TabIndex = 8;
            this.namedFloatControl_latStep.Title = "间隔（分）：";
            this.namedFloatControl_latStep.Value = 120D;
            // 
            // floatSpanControl_lat
            // 
            this.floatSpanControl_lat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.floatSpanControl_lat.From = -90D;
            this.floatSpanControl_lat.Location = new System.Drawing.Point(3, 31);
            this.floatSpanControl_lat.Name = "floatSpanControl_lat";
            this.floatSpanControl_lat.Size = new System.Drawing.Size(229, 23);
            this.floatSpanControl_lat.TabIndex = 10;
            this.floatSpanControl_lat.Title = "纬度范围(度)：";
            this.floatSpanControl_lat.To = 90D;
            // 
            // namedFloatControl_lonStep
            // 
            this.namedFloatControl_lonStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.namedFloatControl_lonStep.Location = new System.Drawing.Point(238, 3);
            this.namedFloatControl_lonStep.Name = "namedFloatControl_lonStep";
            this.namedFloatControl_lonStep.Size = new System.Drawing.Size(145, 22);
            this.namedFloatControl_lonStep.TabIndex = 9;
            this.namedFloatControl_lonStep.Title = "间隔（分）：";
            this.namedFloatControl_lonStep.Value = 120D;
            // 
            // GeoGridLoopControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "GeoGridLoopControl";
            this.Size = new System.Drawing.Size(540, 57);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FloatSpanControl floatSpanControl_lat;
        private NamedFloatControl namedFloatControl_latStep;
        private FloatSpanControl floatSpanControl_lon;
        private NamedFloatControl namedFloatControl_lonStep;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox checkBox_lonOrder;
        private System.Windows.Forms.CheckBox checkBox_latOrder;
        private System.Windows.Forms.CheckBox checkBox_latFirst;

    }
}
