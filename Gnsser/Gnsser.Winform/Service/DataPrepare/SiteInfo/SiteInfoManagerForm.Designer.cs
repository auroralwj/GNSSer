namespace Gnsser.Winform
{
    partial class SiteInfoManagerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SiteInfoManagerForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.StationName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AntU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HeightCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AntN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AntE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReceiverType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReceiverNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AntennaType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AntennaNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.打开OToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.新建NToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.保存SToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton新建 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton编辑 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton删除 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton搜索 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1saveas = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开OToolStripButton,
            this.保存SToolStripButton,
            this.toolStripButton1saveas,
            this.toolStripSeparator1,
            this.新建NToolStripButton,
            this.toolStripButton新建,
            this.toolStripButton编辑,
            this.toolStripButton删除,
            this.toolStripSeparator,
            this.toolStripButton搜索});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(637, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StationName,
            this.TimeFrom,
            this.TimeTo,
            this.AntU,
            this.HeightCode,
            this.AntN,
            this.AntE,
            this.ReceiverType,
            this.ReceiverNumber,
            this.AntennaType,
            this.AntennaNumber});
            this.dataGridView1.DataSource = this.bindingSource1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 25);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(637, 398);
            this.dataGridView1.TabIndex = 1;
            // 
            // StationName
            // 
            this.StationName.DataPropertyName = "StationName";
            this.StationName.HeaderText = "StationName";
            this.StationName.Name = "StationName";
            this.StationName.ReadOnly = true;
            // 
            // TimeFrom
            // 
            this.TimeFrom.DataPropertyName = "TimeFrom";
            this.TimeFrom.HeaderText = "TimeFrom";
            this.TimeFrom.Name = "TimeFrom";
            this.TimeFrom.ReadOnly = true;
            // 
            // TimeTo
            // 
            this.TimeTo.DataPropertyName = "TimeTo";
            this.TimeTo.HeaderText = "TimeTo";
            this.TimeTo.Name = "TimeTo";
            this.TimeTo.ReadOnly = true;
            // 
            // AntU
            // 
            this.AntU.DataPropertyName = "AntU";
            this.AntU.HeaderText = "AntU";
            this.AntU.Name = "AntU";
            this.AntU.ReadOnly = true;
            // 
            // HeightCode
            // 
            this.HeightCode.DataPropertyName = "HeightCode";
            this.HeightCode.HeaderText = "HeightCode";
            this.HeightCode.Name = "HeightCode";
            this.HeightCode.ReadOnly = true;
            // 
            // AntN
            // 
            this.AntN.DataPropertyName = "AntN";
            this.AntN.HeaderText = "AntN";
            this.AntN.Name = "AntN";
            this.AntN.ReadOnly = true;
            // 
            // AntE
            // 
            this.AntE.DataPropertyName = "AntE";
            this.AntE.HeaderText = "AntE";
            this.AntE.Name = "AntE";
            this.AntE.ReadOnly = true;
            // 
            // ReceiverType
            // 
            this.ReceiverType.DataPropertyName = "ReceiverType";
            this.ReceiverType.HeaderText = "ReceiverType";
            this.ReceiverType.Name = "ReceiverType";
            this.ReceiverType.ReadOnly = true;
            // 
            // ReceiverNumber
            // 
            this.ReceiverNumber.DataPropertyName = "ReceiverNumber";
            this.ReceiverNumber.HeaderText = "ReceiverNumber";
            this.ReceiverNumber.Name = "ReceiverNumber";
            this.ReceiverNumber.ReadOnly = true;
            // 
            // AntennaType
            // 
            this.AntennaType.DataPropertyName = "AntennaType";
            this.AntennaType.HeaderText = "AntennaType";
            this.AntennaType.Name = "AntennaType";
            this.AntennaType.ReadOnly = true;
            // 
            // AntennaNumber
            // 
            this.AntennaNumber.DataPropertyName = "AntennaNumber";
            this.AntennaNumber.HeaderText = "AntennaNumber";
            this.AntennaNumber.Name = "AntennaNumber";
            this.AntennaNumber.ReadOnly = true;
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BindingSource = this.bindingSource1;
            this.bindingNavigator1.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 423);
            this.bindingNavigator1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigator1.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigator1.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigator1.Size = new System.Drawing.Size(637, 25);
            this.bindingNavigator1.TabIndex = 2;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(32, 22);
            this.bindingNavigatorCountItem.Text = "/ {0}";
            this.bindingNavigatorCountItem.ToolTipText = "总项数";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "位置";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "当前位置";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "stainfo";
            this.openFileDialog1.FileName = "SationInfoFile";
            this.openFileDialog1.Filter = "测站信息文件|*.stainfo|所有文件(*.*)|*.*";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "SationInfoFile";
            this.saveFileDialog1.Filter = "测站信息文件|*.stainfo|所有文件(*.*)|*.*";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "移到第一条记录";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "移到上一条记录";
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "移到下一条记录";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "移到最后一条记录";
            // 
            // 打开OToolStripButton
            // 
            this.打开OToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.打开OToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("打开OToolStripButton.Image")));
            this.打开OToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.打开OToolStripButton.Name = "打开OToolStripButton";
            this.打开OToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.打开OToolStripButton.Text = "打开(&O)";
            this.打开OToolStripButton.Click += new System.EventHandler(this.打开OToolStripButton_Click);
            // 
            // 新建NToolStripButton
            // 
            this.新建NToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.新建NToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("新建NToolStripButton.Image")));
            this.新建NToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.新建NToolStripButton.Name = "新建NToolStripButton";
            this.新建NToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.新建NToolStripButton.Text = "新建(&N)";
            this.新建NToolStripButton.Click += new System.EventHandler(this.新建NToolStripButton_Click);
            // 
            // 保存SToolStripButton
            // 
            this.保存SToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.保存SToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("保存SToolStripButton.Image")));
            this.保存SToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.保存SToolStripButton.Name = "保存SToolStripButton";
            this.保存SToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.保存SToolStripButton.Text = "保存(&S)";
            this.保存SToolStripButton.Click += new System.EventHandler(this.保存SToolStripButton_Click);
            // 
            // toolStripButton新建
            // 
            this.toolStripButton新建.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton新建.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton新建.Image")));
            this.toolStripButton新建.Name = "toolStripButton新建";
            this.toolStripButton新建.RightToLeftAutoMirrorImage = true;
            this.toolStripButton新建.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton新建.Text = "新添";
            this.toolStripButton新建.Click += new System.EventHandler(this.新建NToolStripButton_Click);
            // 
            // toolStripButton编辑
            // 
            this.toolStripButton编辑.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton编辑.Image = global::Gnsser.Winform.Properties.Resources.edit_16x161;
            this.toolStripButton编辑.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton编辑.Name = "toolStripButton编辑";
            this.toolStripButton编辑.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton编辑.Text = "编辑";
            this.toolStripButton编辑.Click += new System.EventHandler(this.toolStripButton编辑_Click);
            // 
            // toolStripButton删除
            // 
            this.toolStripButton删除.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton删除.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton删除.Image")));
            this.toolStripButton删除.Name = "toolStripButton删除";
            this.toolStripButton删除.RightToLeftAutoMirrorImage = true;
            this.toolStripButton删除.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton删除.Text = "删除";
            this.toolStripButton删除.Click += new System.EventHandler(this.toolStripButton删除_Click);
            // 
            // toolStripButton搜索
            // 
            this.toolStripButton搜索.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton搜索.Image = global::Gnsser.Winform.Properties.Resources.search_16x16;
            this.toolStripButton搜索.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton搜索.Name = "toolStripButton搜索";
            this.toolStripButton搜索.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton搜索.Text = "搜索";
            this.toolStripButton搜索.Click += new System.EventHandler(this.toolStripButton搜索_Click);
            // 
            // toolStripButton1saveas
            // 
            this.toolStripButton1saveas.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1saveas.Image = global::Gnsser.Winform.Properties.Resources.saveas16x16;
            this.toolStripButton1saveas.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1saveas.Name = "toolStripButton1saveas";
            this.toolStripButton1saveas.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1saveas.Text = "另存为";
            this.toolStripButton1saveas.Click += new System.EventHandler(this.toolStripButton1saveas_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // SiteInfoManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 448);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.bindingNavigator1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "SiteInfoManagerForm";
            this.Text = "测站信息维护";
            this.Load += new System.EventHandler(this.SiteInfoManagerForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton编辑;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton新建;
        private System.Windows.Forms.ToolStripButton toolStripButton删除;
        private System.Windows.Forms.ToolStripButton 打开OToolStripButton;
        private System.Windows.Forms.ToolStripButton 新建NToolStripButton;
        private System.Windows.Forms.ToolStripButton 保存SToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton toolStripButton搜索;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.DataGridViewTextBoxColumn StationName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn AntU;
        private System.Windows.Forms.DataGridViewTextBoxColumn HeightCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn AntN;
        private System.Windows.Forms.DataGridViewTextBoxColumn AntE;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReceiverType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReceiverNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn AntennaType;
        private System.Windows.Forms.DataGridViewTextBoxColumn AntennaNumber;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripButton toolStripButton1saveas;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}