using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Geo.Winform
{
    public partial class DbBackupForm : Form
    {
        private SqlConnection connection;
        private int step;

        private static string DBName = "Geo2015";

        public DbBackupForm(SqlConnection connection)
        {
            InitializeComponent();
            this.connection = connection;
        }

        private void BackForm_Load(object sender, EventArgs e)
        {
            step = 0;

            radioButtonBackup.Checked = true;
            radioButtonBackupAll.Checked = true;
            checkBoxRestoreAll.Checked = true;
            buttonRestoreFileD.Enabled = false;

            panelBackup.Visible = false;
            panelRestore.Visible = false;
        }

        private void radioButtonBackup_CheckedChanged(object sender, EventArgs e)
        {
            pictureBoxBackup.Visible = radioButtonBackup.Checked;
            pictureBoxRestore.Visible = !radioButtonBackup.Checked;
        }

        private void radioButtonRestore_CheckedChanged(object sender, EventArgs e)
        {
            pictureBoxBackup.Visible = !radioButtonRestore.Checked;
            pictureBoxRestore.Visible = radioButtonRestore.Checked;
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            step++;

            if (step > 0)
                buttonBack.Enabled = true;
            if (step == 2)
                buttonNext.Text = "完成";

            switch (step)
            {
                case 1:
                    panelSelectOperation.Visible = false;

                    if (radioButtonBackup.Checked)
                    {
                        panelBackup.Visible = true;
                    }
                    else
                    {
                        string confirmMessage = "只有在数据库遭到损坏时才需要执行还原操作。\n\n你是否确认要还原数据库？";
                        if (MessageBox.Show(confirmMessage, "确认还原", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            panelRestore.Visible = true;
                        }
                    }

                    break;

                case 2:
                    if (panelBackup.Visible)
                    {
                        if (Backup(radioButtonBackupAll.Checked) == true) this.Close();
                    }
                    else
                    {
                        if (Restore(checkBoxRestoreDifference.Checked) == true) this.Close();
                    }
                    break;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool Backup(bool backupAll)
        {
            if (backupAll)
            {
                saveFileDialog.Title = "将数据库的完全备份保存到";
                saveFileDialog.Filter = "数据库完全备份文件.fbk|*.fbk";
            }
            else
            {
                saveFileDialog.Title = "将数据库的差异备份保存到";
                saveFileDialog.Filter = "数据库差异备份文件.dbk|*.dbk";
            }

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.labelMsg.Text = "正在进行数据库备份操作，可能要花费几分钟时间，请您耐心等待……";
                this.panel1.Visible = true;

                string sqlBackup, fileBackup;
                fileBackup = saveFileDialog.FileName;

                if (backupAll)
                    sqlBackup = "BACKUP DATABASE " + DBName + " TO DISK = '" + fileBackup + "' WITH INIT";
                else
                    sqlBackup = "BACKUP DATABASE " + DBName + " TO DISK = '" + fileBackup + "' WITH DIFFERENTIAL, INIT";

                SqlCommand cmd = new SqlCommand(sqlBackup, connection);
                cmd.CommandTimeout = 0;
                this.Refresh();
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("数据库成功备份！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (SqlException se)
                {
                    MessageBox.Show("数据库备份失败！\n\n" + se.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Cursor = Cursors.Default;
                    this.panel1.Visible = false;
                    return false;
                }

                this.Cursor = Cursors.Default;
                this.panel1.Visible = false;
                return true;
            }

            return false;
        }

        private bool Restore(bool restoreDifference)
        {
            string backFileA = textBoxRestoreFileA.Text;
            string backFileD = textBoxRestoreFileD.Text;
            string sqlRecovery = String.Empty;

            if (backFileA.Length == 0)
            {
                MessageBox.Show("请选择完全备份文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (restoreDifference)
            {
                if (backFileD.Length == 0)
                {
                    MessageBox.Show("请选择差异备份文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                sqlRecovery = "RESTORE DATABASE " + DBName + " FROM DISK = '" + backFileA + "' WITH NORECOVERY; ";
                sqlRecovery += "RESTORE DATABASE " + DBName + " FROM DISK = '" + backFileD + "' WITH RECOVERY";
            }
            else
            {
                sqlRecovery = "RESTORE DATABASE " + DBName + " FROM DISK = '" + backFileA + "' WITH RECOVERY";
            }

            this.labelMsg.Text = "正在进行数据库还原操作，可能要花费几分钟时间，请您耐心等待……";
            this.panel1.Visible = true;

            this.Refresh();

            this.Cursor = Cursors.WaitCursor;
            SqlCommand cmd = new SqlCommand(sqlRecovery, connection);
            cmd.CommandTimeout = 0;
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("数据库成功还原！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException se)
            {
                MessageBox.Show("数据库还原失败！\n\n" + se.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                this.panel1.Visible = false;
                return false;
            }

            this.Cursor = Cursors.Default;
            this.panel1.Visible = false;
            return true;
        }

        private void buttonSelectAllFile_Click(object sender, EventArgs e)
        {
            openFileDialog.Title = "选择数据库完全备份文件";
            openFileDialog.Filter = "数据库完全备份文件.fbk|*.fbk";

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                textBoxRestoreFileA.Text = openFileDialog.FileName;
            else
                textBoxRestoreFileA.Text = String.Empty;
        }

        private void buttonSelectDifference_Click(object sender, EventArgs e)
        {
            openFileDialog.Title = "选择数据库差异备份文件";
            openFileDialog.Filter = "数据库差异备份文件.dbk|*.dbk";

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                textBoxRestoreFileD.Text = openFileDialog.FileName;
            else
                textBoxRestoreFileD.Text = String.Empty;
        }

        private void checkBoxRestoreAll_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxRestoreAll.Checked = true;
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            step--;

            if (step == 0)
            {
                panelSelectOperation.Visible = true;
                panelBackup.Visible = false;
                panelRestore.Visible = false;
            }            
        }

        private void checkBoxRestoreDifference_CheckedChanged(object sender, EventArgs e)
        {
            buttonRestoreFileD.Enabled = checkBoxRestoreDifference.Checked;
        }
    }
}