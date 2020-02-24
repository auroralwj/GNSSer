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
                buttonNext.Text = "���";

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
                        string confirmMessage = "ֻ�������ݿ��⵽��ʱ����Ҫִ�л�ԭ������\n\n���Ƿ�ȷ��Ҫ��ԭ���ݿ⣿";
                        if (MessageBox.Show(confirmMessage, "ȷ�ϻ�ԭ", MessageBoxButtons.YesNo,
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
                saveFileDialog.Title = "�����ݿ����ȫ���ݱ��浽";
                saveFileDialog.Filter = "���ݿ���ȫ�����ļ�.fbk|*.fbk";
            }
            else
            {
                saveFileDialog.Title = "�����ݿ�Ĳ��챸�ݱ��浽";
                saveFileDialog.Filter = "���ݿ���챸���ļ�.dbk|*.dbk";
            }

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.labelMsg.Text = "���ڽ������ݿⱸ�ݲ���������Ҫ���Ѽ�����ʱ�䣬�������ĵȴ�����";
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
                    MessageBox.Show("���ݿ�ɹ����ݣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (SqlException se)
                {
                    MessageBox.Show("���ݿⱸ��ʧ�ܣ�\n\n" + se.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("��ѡ����ȫ�����ļ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (restoreDifference)
            {
                if (backFileD.Length == 0)
                {
                    MessageBox.Show("��ѡ����챸���ļ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                sqlRecovery = "RESTORE DATABASE " + DBName + " FROM DISK = '" + backFileA + "' WITH NORECOVERY; ";
                sqlRecovery += "RESTORE DATABASE " + DBName + " FROM DISK = '" + backFileD + "' WITH RECOVERY";
            }
            else
            {
                sqlRecovery = "RESTORE DATABASE " + DBName + " FROM DISK = '" + backFileA + "' WITH RECOVERY";
            }

            this.labelMsg.Text = "���ڽ������ݿ⻹ԭ����������Ҫ���Ѽ�����ʱ�䣬�������ĵȴ�����";
            this.panel1.Visible = true;

            this.Refresh();

            this.Cursor = Cursors.WaitCursor;
            SqlCommand cmd = new SqlCommand(sqlRecovery, connection);
            cmd.CommandTimeout = 0;
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("���ݿ�ɹ���ԭ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException se)
            {
                MessageBox.Show("���ݿ⻹ԭʧ�ܣ�\n\n" + se.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            openFileDialog.Title = "ѡ�����ݿ���ȫ�����ļ�";
            openFileDialog.Filter = "���ݿ���ȫ�����ļ�.fbk|*.fbk";

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                textBoxRestoreFileA.Text = openFileDialog.FileName;
            else
                textBoxRestoreFileA.Text = String.Empty;
        }

        private void buttonSelectDifference_Click(object sender, EventArgs e)
        {
            openFileDialog.Title = "ѡ�����ݿ���챸���ļ�";
            openFileDialog.Filter = "���ݿ���챸���ļ�.dbk|*.dbk";

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