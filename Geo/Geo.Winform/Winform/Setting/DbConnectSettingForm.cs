using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Xml;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.Configuration; 
using System.Collections.Specialized;


namespace Geo.Winform
{
    public partial class DbConnectSettingForm : Form
    {
        public DbConnectSettingForm()
        {
            InitializeComponent();

            this.LoginInfo = Setting.LoginInfo;
            InitLoginInfo = Setting.LoginInfo.Clone();
        }
        private Geo.Utils.DbLoginInfo loginInfo { get; set; }
        private Geo.Utils.DbLoginInfo InitLoginInfo { get; set; }

        /// <summary>
        /// 信息是否改变
        /// </summary>
        public bool IsLoginInfoChanged { get; set; }
        /// <summary>
        /// 界面信息绑定
        /// </summary>
        public Geo.Utils.DbLoginInfo LoginInfo
        {
            get {

                if (loginInfo == null)
                {
                    loginInfo = new Geo.Utils.DbLoginInfo();
                }

                loginInfo.ServerAddress = this.textBox_serverIp.Text.Trim();
                loginInfo.DababaseName = this.textBox_dbName.Text.Trim();
                loginInfo.LoginUser = this.textBox_loginUser.Text.Trim();
                loginInfo.LoginPass = this.textBox_pass.Text.Trim();
                return loginInfo; }
            set
            {
                loginInfo = value;
                this.textBox_serverIp.Text = value.ServerAddress;
                this.textBox_dbName.Text = value.DababaseName;
                this.textBox_loginUser.Text = value.LoginUser;
                this.textBox_pass.Text = value.LoginPass;
            }
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                string cnStr = SqlUtil.GetConnString(LoginInfo);

                SqlConnection cn = new SqlConnection(cnStr);
                try
                {
                    cn.Open();

                    if (LoginInfo.Equals(InitLoginInfo))
                    {
                        IsLoginInfoChanged = false;
                        MessageBox.Show("数据库连接成功！本窗口将关闭。");
                    }
                    else
                    {
                        IsLoginInfoChanged = true;
                        Setting.SaveLoginInfoToConfigFile(LoginInfo);
                        Geo.Winform.Setting.ConectionStringChanged = true;

                        if (Geo.Utils.FormUtil.ShowYesNoMessageBox("登录信息已更改，需重启系统。\r\n立即重启？") == System.Windows.Forms.DialogResult.Yes)
                        { Application.Restart(); }
                    }

                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("数据库连接失败！\r\n" + ex.Message);
                }
                finally
                {
                    cn.Close();
                }

            }
        }


        private bool ValidateInput()
        {
            if (LoginInfo.ServerAddress == string.Empty
                || LoginInfo.ServerAddress == string.Empty
                || LoginInfo.ServerAddress == string.Empty
                || LoginInfo.ServerAddress == string.Empty)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("信息输入不完整！");
                return false;
            }
            return true;
        } 

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
