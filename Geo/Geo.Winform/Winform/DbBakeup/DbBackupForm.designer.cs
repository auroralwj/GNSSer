namespace Geo.Winform
{
    partial class DbBackupForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DbBackupForm));
            this.pictureBoxRestore = new System.Windows.Forms.PictureBox();
            this.panelSelectOperation = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButtonRestore = new System.Windows.Forms.RadioButton();
            this.radioButtonBackup = new System.Windows.Forms.RadioButton();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.pictureBoxBackup = new System.Windows.Forms.PictureBox();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.panelBackup = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.radioButtonBackupDifference = new System.Windows.Forms.RadioButton();
            this.radioButtonBackupAll = new System.Windows.Forms.RadioButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.panelRestore = new System.Windows.Forms.Panel();
            this.textBoxRestoreFileD = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonRestoreFileD = new System.Windows.Forms.Button();
            this.textBoxRestoreFileA = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonRestoreFileA = new System.Windows.Forms.Button();
            this.checkBoxRestoreDifference = new System.Windows.Forms.CheckBox();
            this.checkBoxRestoreAll = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonBack = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelMsg = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRestore)).BeginInit();
            this.panelSelectOperation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBackup)).BeginInit();
            this.panelBackup.SuspendLayout();
            this.panelRestore.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxRestore
            // 
            this.pictureBoxRestore.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxRestore.Image")));
            this.pictureBoxRestore.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxRestore.Name = "pictureBoxRestore";
            this.pictureBoxRestore.Size = new System.Drawing.Size(167, 314);
            this.pictureBoxRestore.TabIndex = 0;
            this.pictureBoxRestore.TabStop = false;
            // 
            // panelSelectOperation
            // 
            this.panelSelectOperation.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelSelectOperation.Controls.Add(this.label2);
            this.panelSelectOperation.Controls.Add(this.label3);
            this.panelSelectOperation.Controls.Add(this.radioButtonRestore);
            this.panelSelectOperation.Controls.Add(this.radioButtonBackup);
            this.panelSelectOperation.Location = new System.Drawing.Point(181, 9);
            this.panelSelectOperation.Name = "panelSelectOperation";
            this.panelSelectOperation.Size = new System.Drawing.Size(220, 262);
            this.panelSelectOperation.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(11, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(200, 141);
            this.label2.TabIndex = 11;
            this.label2.Text = "    数据备份包括完整备份和完整差异备份。数据备份易于使用并且适用于所有数据库，与恢复模式无关。完整备份包含数据库中的所有数据，并且可以用作完整差异备份所基于的" +
                "“基准备份”。完整差异备份仅记录自前一完整备份后发生更改的数据扩展盘区数。因此，与完整备份相比，完整差异备份较小且速度较快，便于进行较频繁的备份，同时降低丢失数" +
                "据的风险。";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(11, 207);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(200, 37);
            this.label3.TabIndex = 10;
            this.label3.Text = "    还原数据库是一个从备份还原数据并在还原所有必要的备份后恢复数据库的过程。";
            // 
            // radioButtonRestore
            // 
            this.radioButtonRestore.AutoSize = true;
            this.radioButtonRestore.Location = new System.Drawing.Point(21, 186);
            this.radioButtonRestore.Name = "radioButtonRestore";
            this.radioButtonRestore.Size = new System.Drawing.Size(89, 16);
            this.radioButtonRestore.TabIndex = 9;
            this.radioButtonRestore.TabStop = true;
            this.radioButtonRestore.Text = "数据还原(&R)";
            this.radioButtonRestore.UseVisualStyleBackColor = true;
            this.radioButtonRestore.CheckedChanged += new System.EventHandler(this.radioButtonRestore_CheckedChanged);
            // 
            // radioButtonBackup
            // 
            this.radioButtonBackup.AutoSize = true;
            this.radioButtonBackup.Location = new System.Drawing.Point(25, 14);
            this.radioButtonBackup.Name = "radioButtonBackup";
            this.radioButtonBackup.Size = new System.Drawing.Size(89, 16);
            this.radioButtonBackup.TabIndex = 5;
            this.radioButtonBackup.TabStop = true;
            this.radioButtonBackup.Text = "数据备份(&B)";
            this.radioButtonBackup.UseVisualStyleBackColor = true;
            this.radioButtonBackup.CheckedChanged += new System.EventHandler(this.radioButtonBackup_CheckedChanged);
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(261, 285);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(60, 23);
            this.buttonNext.TabIndex = 2;
            this.buttonNext.Text = "下一步";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(333, 285);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(60, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // pictureBoxBackup
            // 
            this.pictureBoxBackup.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxBackup.Image")));
            this.pictureBoxBackup.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxBackup.Name = "pictureBoxBackup";
            this.pictureBoxBackup.Size = new System.Drawing.Size(167, 314);
            this.pictureBoxBackup.TabIndex = 4;
            this.pictureBoxBackup.TabStop = false;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.RestoreDirectory = true;
            // 
            // panelBackup
            // 
            this.panelBackup.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelBackup.Controls.Add(this.label1);
            this.panelBackup.Controls.Add(this.label4);
            this.panelBackup.Controls.Add(this.radioButtonBackupDifference);
            this.panelBackup.Controls.Add(this.radioButtonBackupAll);
            this.panelBackup.Location = new System.Drawing.Point(181, 9);
            this.panelBackup.Name = "panelBackup";
            this.panelBackup.Size = new System.Drawing.Size(220, 262);
            this.panelBackup.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(11, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 54);
            this.label1.TabIndex = 11;
            this.label1.Text = "对数据库的完全备份包括所有的数据以及数据库对象。首先将事务日志写到磁盘上，然后根据事务创建相同的数据库和数据库对象以及拷贝数据。";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(11, 177);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(200, 41);
            this.label4.TabIndex = 10;
            this.label4.Text = "差异备份也称为增量数据库备份，是指将最近一次数据库完全备份以来发生的数据变化备份起来。";
            // 
            // radioButtonBackupDifference
            // 
            this.radioButtonBackupDifference.AutoSize = true;
            this.radioButtonBackupDifference.Location = new System.Drawing.Point(21, 148);
            this.radioButtonBackupDifference.Name = "radioButtonBackupDifference";
            this.radioButtonBackupDifference.Size = new System.Drawing.Size(89, 16);
            this.radioButtonBackupDifference.TabIndex = 9;
            this.radioButtonBackupDifference.TabStop = true;
            this.radioButtonBackupDifference.Text = "差异备份(&D)";
            this.radioButtonBackupDifference.UseVisualStyleBackColor = true;
            // 
            // radioButtonBackupAll
            // 
            this.radioButtonBackupAll.AutoSize = true;
            this.radioButtonBackupAll.Location = new System.Drawing.Point(25, 20);
            this.radioButtonBackupAll.Name = "radioButtonBackupAll";
            this.radioButtonBackupAll.Size = new System.Drawing.Size(89, 16);
            this.radioButtonBackupAll.TabIndex = 5;
            this.radioButtonBackupAll.TabStop = true;
            this.radioButtonBackupAll.Text = "完全备份(&A)";
            this.radioButtonBackupAll.UseVisualStyleBackColor = true;
            // 
            // panelRestore
            // 
            this.panelRestore.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelRestore.Controls.Add(this.textBoxRestoreFileD);
            this.panelRestore.Controls.Add(this.label8);
            this.panelRestore.Controls.Add(this.buttonRestoreFileD);
            this.panelRestore.Controls.Add(this.textBoxRestoreFileA);
            this.panelRestore.Controls.Add(this.label7);
            this.panelRestore.Controls.Add(this.buttonRestoreFileA);
            this.panelRestore.Controls.Add(this.checkBoxRestoreDifference);
            this.panelRestore.Controls.Add(this.checkBoxRestoreAll);
            this.panelRestore.Controls.Add(this.label5);
            this.panelRestore.Controls.Add(this.label6);
            this.panelRestore.Location = new System.Drawing.Point(181, 9);
            this.panelRestore.Name = "panelRestore";
            this.panelRestore.Size = new System.Drawing.Size(220, 262);
            this.panelRestore.TabIndex = 6;
            // 
            // textBoxRestoreFileD
            // 
            this.textBoxRestoreFileD.Location = new System.Drawing.Point(13, 228);
            this.textBoxRestoreFileD.Name = "textBoxRestoreFileD";
            this.textBoxRestoreFileD.ReadOnly = true;
            this.textBoxRestoreFileD.Size = new System.Drawing.Size(189, 21);
            this.textBoxRestoreFileD.TabIndex = 19;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 213);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(101, 12);
            this.label8.TabIndex = 18;
            this.label8.Text = "差异备份数据文件";
            // 
            // buttonRestoreFileD
            // 
            this.buttonRestoreFileD.Location = new System.Drawing.Point(172, 209);
            this.buttonRestoreFileD.Name = "buttonRestoreFileD";
            this.buttonRestoreFileD.Size = new System.Drawing.Size(29, 20);
            this.buttonRestoreFileD.TabIndex = 17;
            this.buttonRestoreFileD.Text = "…";
            this.buttonRestoreFileD.UseVisualStyleBackColor = true;
            this.buttonRestoreFileD.Click += new System.EventHandler(this.buttonSelectDifference_Click);
            // 
            // textBoxRestoreFileA
            // 
            this.textBoxRestoreFileA.Location = new System.Drawing.Point(13, 102);
            this.textBoxRestoreFileA.Name = "textBoxRestoreFileA";
            this.textBoxRestoreFileA.ReadOnly = true;
            this.textBoxRestoreFileA.Size = new System.Drawing.Size(189, 21);
            this.textBoxRestoreFileA.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 87);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 12);
            this.label7.TabIndex = 15;
            this.label7.Text = "完全备份数据文件";
            // 
            // buttonRestoreFileA
            // 
            this.buttonRestoreFileA.Location = new System.Drawing.Point(172, 83);
            this.buttonRestoreFileA.Name = "buttonRestoreFileA";
            this.buttonRestoreFileA.Size = new System.Drawing.Size(29, 20);
            this.buttonRestoreFileA.TabIndex = 14;
            this.buttonRestoreFileA.Text = "…";
            this.buttonRestoreFileA.UseVisualStyleBackColor = true;
            this.buttonRestoreFileA.Click += new System.EventHandler(this.buttonSelectAllFile_Click);
            // 
            // checkBoxRestoreDifference
            // 
            this.checkBoxRestoreDifference.AutoSize = true;
            this.checkBoxRestoreDifference.Location = new System.Drawing.Point(25, 137);
            this.checkBoxRestoreDifference.Name = "checkBoxRestoreDifference";
            this.checkBoxRestoreDifference.Size = new System.Drawing.Size(150, 16);
            this.checkBoxRestoreDifference.TabIndex = 13;
            this.checkBoxRestoreDifference.Text = "基于差异备份的还原(&D)";
            this.checkBoxRestoreDifference.UseVisualStyleBackColor = true;
            this.checkBoxRestoreDifference.CheckedChanged += new System.EventHandler(this.checkBoxRestoreDifference_CheckedChanged);
            // 
            // checkBoxRestoreAll
            // 
            this.checkBoxRestoreAll.AutoSize = true;
            this.checkBoxRestoreAll.Checked = true;
            this.checkBoxRestoreAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRestoreAll.Location = new System.Drawing.Point(25, 12);
            this.checkBoxRestoreAll.Name = "checkBoxRestoreAll";
            this.checkBoxRestoreAll.Size = new System.Drawing.Size(150, 16);
            this.checkBoxRestoreAll.TabIndex = 12;
            this.checkBoxRestoreAll.Text = "基于完全备份的还原(&A)";
            this.checkBoxRestoreAll.UseVisualStyleBackColor = true;
            this.checkBoxRestoreAll.CheckedChanged += new System.EventHandler(this.checkBoxRestoreAll_CheckedChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(11, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(200, 40);
            this.label5.TabIndex = 11;
            this.label5.Text = "基于完全备份的数据库还原是指将数据库中的数据按照完全备份文件中信息，还原到备份事件那一刻的状态。";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(11, 156);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(200, 50);
            this.label6.TabIndex = 10;
            this.label6.Text = "差异还原是根据差异备份文件中的信息，将数据库在完全还原的基础上，恢复出与完全备份不同的那部分，使数据保持一定的完整性。";
            // 
            // buttonBack
            // 
            this.buttonBack.Enabled = false;
            this.buttonBack.Location = new System.Drawing.Point(189, 285);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(60, 23);
            this.buttonBack.TabIndex = 7;
            this.buttonBack.Text = "上一步";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelMsg);
            this.panel1.Location = new System.Drawing.Point(12, 140);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(391, 60);
            this.panel1.TabIndex = 8;
            this.panel1.Visible = false;
            // 
            // labelMsg
            // 
            this.labelMsg.AutoSize = true;
            this.labelMsg.Location = new System.Drawing.Point(7, 23);
            this.labelMsg.Name = "labelMsg";
            this.labelMsg.Size = new System.Drawing.Size(377, 12);
            this.labelMsg.TabIndex = 0;
            this.labelMsg.Text = "正在进行数据库备份操作，可能要花费几分钟时间，请您耐心等待……";
            // 
            // DbBackupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 315);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.pictureBoxBackup);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.pictureBoxRestore);
            this.Controls.Add(this.panelRestore);
            this.Controls.Add(this.panelBackup);
            this.Controls.Add(this.panelSelectOperation);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DbBackupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "备份与还原";
            this.Load += new System.EventHandler(this.BackForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRestore)).EndInit();
            this.panelSelectOperation.ResumeLayout(false);
            this.panelSelectOperation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBackup)).EndInit();
            this.panelBackup.ResumeLayout(false);
            this.panelBackup.PerformLayout();
            this.panelRestore.ResumeLayout(false);
            this.panelRestore.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxRestore;
        private System.Windows.Forms.Panel panelSelectOperation;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.PictureBox pictureBoxBackup;
        private System.Windows.Forms.RadioButton radioButtonBackup;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButtonRestore;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelBackup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radioButtonBackupDifference;
        private System.Windows.Forms.RadioButton radioButtonBackupAll;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Panel panelRestore;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBoxRestoreDifference;
        private System.Windows.Forms.CheckBox checkBoxRestoreAll;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonRestoreFileA;
        private System.Windows.Forms.TextBox textBoxRestoreFileD;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button buttonRestoreFileD;
        private System.Windows.Forms.TextBox textBoxRestoreFileA;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelMsg;
    }
}