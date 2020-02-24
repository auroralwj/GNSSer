namespace Gnsser.Winform
{
    partial class SiteInfoSetingPage
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
            this.fileOpenControl_siteinfo = new Geo.Winform.Controls.FileOpenControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_updateStationInfo = new System.Windows.Forms.CheckBox();
            this.checkBox_IsUpdateSiteCoord = new System.Windows.Forms.CheckBox();
            this.enumRadioControl_positionType = new Geo.Winform.EnumRadioControl();
            this.fileOpenControl_siteCoord = new Geo.Winform.Controls.FileOpenControl();
            this.checkBox_updateEpochSiteCoord = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileOpenControl_siteinfo
            // 
            this.fileOpenControl_siteinfo.AllowDrop = true;
            this.fileOpenControl_siteinfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_siteinfo.FilePath = "";
            this.fileOpenControl_siteinfo.FilePathes = new string[0];
            this.fileOpenControl_siteinfo.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_siteinfo.FirstPath = "";
            this.fileOpenControl_siteinfo.IsMultiSelect = false;
            this.fileOpenControl_siteinfo.LabelName = "测站信息文件：";
            this.fileOpenControl_siteinfo.Location = new System.Drawing.Point(25, 74);
            this.fileOpenControl_siteinfo.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.fileOpenControl_siteinfo.Name = "fileOpenControl_siteinfo";
            this.fileOpenControl_siteinfo.Size = new System.Drawing.Size(432, 28);
            this.fileOpenControl_siteinfo.TabIndex = 0;
            this.fileOpenControl_siteinfo.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox_updateEpochSiteCoord);
            this.groupBox1.Controls.Add(this.enumRadioControl_positionType);
            this.groupBox1.Controls.Add(this.checkBox_updateStationInfo);
            this.groupBox1.Controls.Add(this.checkBox_IsUpdateSiteCoord);
            this.groupBox1.Controls.Add(this.fileOpenControl_siteCoord);
            this.groupBox1.Controls.Add(this.fileOpenControl_siteinfo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(709, 379);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "测站信息";
            // 
            // checkBox_updateStationInfo
            // 
            this.checkBox_updateStationInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_updateStationInfo.AutoSize = true;
            this.checkBox_updateStationInfo.Location = new System.Drawing.Point(489, 83);
            this.checkBox_updateStationInfo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_updateStationInfo.Name = "checkBox_updateStationInfo";
            this.checkBox_updateStationInfo.Size = new System.Drawing.Size(119, 19);
            this.checkBox_updateStationInfo.TabIndex = 31;
            this.checkBox_updateStationInfo.Text = "更新测站信息";
            this.checkBox_updateStationInfo.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsUpdateSiteCoord
            // 
            this.checkBox_IsUpdateSiteCoord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_IsUpdateSiteCoord.AutoSize = true;
            this.checkBox_IsUpdateSiteCoord.Location = new System.Drawing.Point(489, 135);
            this.checkBox_IsUpdateSiteCoord.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_IsUpdateSiteCoord.Name = "checkBox_IsUpdateSiteCoord";
            this.checkBox_IsUpdateSiteCoord.Size = new System.Drawing.Size(149, 19);
            this.checkBox_IsUpdateSiteCoord.TabIndex = 32;
            this.checkBox_IsUpdateSiteCoord.Text = "更新测站初始坐标";
            this.checkBox_IsUpdateSiteCoord.UseVisualStyleBackColor = true;
            this.checkBox_IsUpdateSiteCoord.Visible = false;
            // 
            // enumRadioControl_positionType
            // 
            this.enumRadioControl_positionType.Location = new System.Drawing.Point(25, 231);
            this.enumRadioControl_positionType.Margin = new System.Windows.Forms.Padding(5);
            this.enumRadioControl_positionType.Name = "enumRadioControl_positionType";
            this.enumRadioControl_positionType.Size = new System.Drawing.Size(593, 68);
            this.enumRadioControl_positionType.TabIndex = 39;
            this.enumRadioControl_positionType.Title = "定位/定轨类型";
            // 
            // fileOpenControl_siteCoord
            // 
            this.fileOpenControl_siteCoord.AllowDrop = true;
            this.fileOpenControl_siteCoord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_siteCoord.FilePath = "";
            this.fileOpenControl_siteCoord.FilePathes = new string[0];
            this.fileOpenControl_siteCoord.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_siteCoord.FirstPath = "";
            this.fileOpenControl_siteCoord.IsMultiSelect = false;
            this.fileOpenControl_siteCoord.LabelName = "测站坐标文件：";
            this.fileOpenControl_siteCoord.Location = new System.Drawing.Point(25, 128);
            this.fileOpenControl_siteCoord.Margin = new System.Windows.Forms.Padding(5);
            this.fileOpenControl_siteCoord.Name = "fileOpenControl_siteCoord";
            this.fileOpenControl_siteCoord.Size = new System.Drawing.Size(432, 28);
            this.fileOpenControl_siteCoord.TabIndex = 0;
            this.fileOpenControl_siteCoord.Visible = false;
            // 
            // checkBox_updateEpochSiteCoord
            // 
            this.checkBox_updateEpochSiteCoord.AutoSize = true;
            this.checkBox_updateEpochSiteCoord.Location = new System.Drawing.Point(35, 204);
            this.checkBox_updateEpochSiteCoord.Name = "checkBox_updateEpochSiteCoord";
            this.checkBox_updateEpochSiteCoord.Size = new System.Drawing.Size(149, 19);
            this.checkBox_updateEpochSiteCoord.TabIndex = 40;
            this.checkBox_updateEpochSiteCoord.Text = "更新历元测站坐标";
            this.checkBox_updateEpochSiteCoord.UseVisualStyleBackColor = true;
            // 
            // SiteInfoSetingPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "SiteInfoSetingPage";
            this.Size = new System.Drawing.Size(709, 379);
            this.Load += new System.EventHandler(this.SelectFilePageControl_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Geo.Winform.Controls.FileOpenControl fileOpenControl_siteinfo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox_updateStationInfo;
        private System.Windows.Forms.CheckBox checkBox_IsUpdateSiteCoord;
        private Geo.Winform.EnumRadioControl enumRadioControl_positionType;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_siteCoord;
        private System.Windows.Forms.CheckBox checkBox_updateEpochSiteCoord;
    }
}
