//2015.06.28, czs, edit in namu, 程序全局文件，整理，增加部分注释

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Geo.Winform;
using System.Configuration;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace Geo.Winform
{
    /// <summary>
    /// 设置。
    /// </summary>
    public class Setting : Geo.Setting
    {
        #region 路径
        /// <summary>
        /// 当前默认打开目录，即打开文件选择框时定位的目录。
        /// </summary>
        public static string CurrentDirecotry = System.IO.Directory.GetCurrentDirectory();

        /// <summary>
        /// 启动图片路径
        /// </summary>
        public static string SplashPath = Path.Combine(DataDirectory, @"\Images\Splash.jpg");
        #endregion

        /// <summary>
        /// 程序名称，存储在配置文件中，更新后需要保存才生效。
        /// </summary>
        public static string Title { get { return GetAppSettingValue("Title"); } set { SetAndSaveAppSettingValue("Title", value); } }

        /// <summary>
        /// 最后一次的登录用户名称。
        /// </summary>
        public static string LastLoginUserName = "gdser";
        /// <summary>
        /// 是否第一次运行程序。若是，则展开相关配置。
        /// </summary>
        public static bool IsFirstRun { get; set; }
        /// <summary>
        /// 指示是否为测试版本。
        /// </summary>
        //public static bool IsDebug { get { return Boolean.Parse(GetAppSettingValue("IsDebug")); } set { SetAndSaveAppSettingValue("IsDebug", value.ToString()); } }

        #region 配置文件工具
        /// <summary>
        /// 从 AppConfig 文件获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static public string GetAppSettingValue(string key) { return System.Configuration.ConfigurationManager.AppSettings[key]; }
        /// <summary>
        /// 设置，并保存到文件。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        static public void SetAndSaveAppSettingValue(string key, string val)
        {
            System.Configuration.ConfigurationManager.AppSettings[key] = val;

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //Save to 
            config.AppSettings.Settings[key].Value = val;
            config.Save(ConfigurationSaveMode.Modified);

            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }

        #endregion
        #region 数据库登录信息
        private static Geo.Utils.DbLoginInfo loginInfo = null;

        /// <summary>
        /// 登录
        /// </summary>
        public static Geo.Utils.DbLoginInfo LoginInfo
        {
            get
            {
                if (loginInfo == null || ConectionStringChanged)
                { loginInfo = GetLoginInfoFromConfigFile();
                    ConectionStringChanged = false;
                }
                return loginInfo;
            }
            set { Setting.loginInfo = value; }
        }

        /// <summary>
        /// 数据库连接字符串是否已经改变。
        /// </summary>
        public static bool ConectionStringChanged = true;

        /// <summary>
        /// 数据库连接字符串。
        /// </summary>
        public static string ConnectionString { get { return Geo.Winform.SqlUtil.GetConnString(LoginInfo); } }


        /// <summary>
        /// 从配置文件读取数据库连接设置.从自定义的 databaseSettings 中，非系统字符串中。
        /// </summary>
        /// <returns></returns>
        public static Geo.Utils.DbLoginInfo GetLoginInfoFromConfigFile()
        {
            Geo.Utils.DbLoginInfo loginInfo = new Geo.Utils.DbLoginInfo();
            NameValueCollection nc = (NameValueCollection)ConfigurationManager.GetSection("databaseSettings");

            loginInfo.ServerAddress = nc["db.datasource"].Trim();
            loginInfo.DababaseName = nc["db.database"].Trim();
            loginInfo.LoginUser = nc["db.user"].Trim();
            loginInfo.LoginPass = Geo.Utils.EncryptUtil.Decrypt(nc["db.password"].Trim());//nc["db.password"].Trim();// 

            return loginInfo; 
        }

        /// <summary>
        /// 将登陆信息保存到数据库。
        /// </summary>
        /// <param name="loginInfo"></param>
        public static void SaveLoginInfoToConfigFile(Geo.Utils.DbLoginInfo loginInfo)
        {
            Setting.LoginInfo = loginInfo;

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //Save to
            string AppName = config.AppSettings.Settings["AppName"].Value;

            Geo.Utils.AppConfigSetter setter = new Geo.Utils.AppConfigSetter();
            setter.AppName = AppName;
            setter.Open();
            setter.SingleNodeName = "//databaseSettings";
            setter.SetValue("db.datasource", loginInfo.ServerAddress);
            setter.SetValue("db.database", loginInfo.DababaseName);
            setter.SetValue("db.user", loginInfo.LoginUser);
            string pass = Geo.Utils.EncryptUtil.Encrypt(loginInfo.LoginPass.Trim());//LoginInfo.LoginPass.Trim();//

            setter.SetValue("db.password", pass);

            setter.Save();


          //  var dbConsection = config.GetSection("databaseSettings");
          //config.SectionGroups["config"].Sections[""].
            
            //  dbConsection["bd"]
           // var conn = config.GetSectionGroup("databaseSettings");

           

         //   dbConsection.

            //记录登录用户名，下次自动填充。
            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //Save to 
            //config.AppSettings.Settings["LastUserName"].Value = username;
           // config.Save(ConfigurationSaveMode.Modified);
            //log.Debug("写登录信息到磁盘。");
            //ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.RefreshSection("databaseSettings");
        }
        #endregion

        #region 系统维护信息
        /// <summary>
        /// 设置系统是否为第一次运行。这样方便维护系统。
        /// </summary>
        /// <param name="tOrF"></param>
        public static void SetFirstRun(bool tOrF)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //Save to 
            config.AppSettings.Settings["IsFirstRun"].Value = tOrF.ToString();
            config.Save(ConfigurationSaveMode.Modified);
        }

        /// <summary>
        /// 必须先初始化PDF阅读器才能使用之。
        /// </summary>
        public static void InitPDFReader()
        {
            //string path = AppDir + "\\Docs\\ForPdfInit.pdf";
            //string text = PDFReader.Instance.ParsePdf(path);
            ////再获取
            //PDFReader reader = PDFReader.Instance;
        }
        #endregion
    }
}
