namespace Gnsser.Winform.Tools
{
    partial class TropProductsCompForm
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
            this.fileOpenControl1 = new Geo.Winform.Controls.FileOpenControl();
            this.fileOpenControl2 = new Geo.Winform.Controls.FileOpenControl();
            this.readandcompare = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl1.AllowDrop = true;
            this.fileOpenControl1.FilePath = "";
            this.fileOpenControl1.FilePathes = new string[0];
            this.fileOpenControl1.Filter = "对流层产品|*.17zpd|所有文件|*.*";
            this.fileOpenControl1.FirstPath = "";
            this.fileOpenControl1.IsMultiSelect = false;
            this.fileOpenControl1.LabelName = "IGS：";
            this.fileOpenControl1.Location = new System.Drawing.Point(71, 37);
            this.fileOpenControl1.Name = "fileOpenControl1";
            this.fileOpenControl1.Size = new System.Drawing.Size(429, 22);
            this.fileOpenControl1.TabIndex = 0;
            // 
            // fileOpenControl2
            // 
            this.fileOpenControl2.AllowDrop = true;
            this.fileOpenControl2.FilePath = "";
            this.fileOpenControl2.FilePathes = new string[0];
            this.fileOpenControl2.Filter = "文本文件|*.txt.xls|所有文件|*.*";
            this.fileOpenControl2.FirstPath = "";
            this.fileOpenControl2.IsMultiSelect = false;
            this.fileOpenControl2.LabelName = "GNSSer：";
            this.fileOpenControl2.Location = new System.Drawing.Point(71, 67);
            this.fileOpenControl2.Name = "fileOpenControl2";
            this.fileOpenControl2.Size = new System.Drawing.Size(429, 22);
            this.fileOpenControl2.TabIndex = 1;
            // 
            // readandcompare
            // 
            this.readandcompare.Location = new System.Drawing.Point(532, 53);
            this.readandcompare.Name = "readandcompare";
            this.readandcompare.Size = new System.Drawing.Size(75, 36);
            this.readandcompare.TabIndex = 2;
            this.readandcompare.Text = "读取并比较";
            this.readandcompare.UseVisualStyleBackColor = true;
            this.readandcompare.Click += new System.EventHandler(this.readandcompare_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.time,
            this.C2});
            this.dataGridView1.DataSource = this.bindingSource1;
            this.dataGridView1.Location = new System.Drawing.Point(71, 95);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(712, 464);
            this.dataGridView1.TabIndex = 3;
            // 
            // time
            // 
            this.time.DataPropertyName = "time";
            this.time.HeaderText = "Time";
            this.time.Name = "time";
            // 
            // C2
            // 
            this.C2.DataPropertyName = "ZTD";
            this.C2.HeaderText = "Difference";
            this.C2.Name = "C2";
            // 
            // TropProductsCompForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(981, 579);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.readandcompare);
            this.Controls.Add(this.fileOpenControl2);
            this.Controls.Add(this.fileOpenControl1);
            this.Name = "TropProductsCompForm";
            this.Text = "TropProductsComp";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Geo.Winform.Controls.FileOpenControl fileOpenControl1;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl2;
        private System.Windows.Forms.Button readandcompare;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.DataGridViewTextBoxColumn time;
        private System.Windows.Forms.DataGridViewTextBoxColumn C2;
    }
}