using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Geo.Common;
using Gnsser.Domain;
using Geo.IO;
using System.IO;
using System.Net;
using System.Text;
using Gnsser.Winform;

namespace Gnsser.Winform
{
    static class Program
    {
        static ILog log = Log.GetLog(typeof(Program));
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //try  if(false)//
            {
                if (Setting.VersionType != Geo.VersionType.Development)
                {
                    //Setting.Version = 0;
                    //throw new NotSupportedException("不要慌张，只是个测试！");
                 var now = DateTime.Now;

                    if (now < Setting.StartUsage || now > Setting.EndUsage)
                    {
                        Geo.Utils.FormUtil.ShowWarningMessageBox("对不起，软件试用期已过，为了保证质量，请获取新版本。gnsser@163.com, www.gnsser.com");
                        System.Diagnostics.Process.Start(Setting.WebSiteUrlForNewVersion);
                        System.Environment.Exit(0); //无论在主线程和其它线程，只要执行了这句，都可以把程序结束干净; 
                        return;
                    }


                    var coreFileName = Path.Combine(Setting.ApplicationDirectory, "Gnsser.Core.dll");

                    var geoFileName = Path.Combine(Setting.ApplicationDirectory, "Geo.dll");
                    var geoMD5 = "5eacdd7a18db582da0da6760dff4b3eb";
                    var should = Geo.Utils.FileUtil.GetMD5(geoFileName);
                    if ( false && should != geoMD5)
                    {
                        //Geo.Utils.FormUtil.ShowWarningMessageBox("对不起，软件被擅自更改了，为了保证质量，请获取原始版本。");
                        System.Diagnostics.Process.Start(Setting.WebSiteUrlForNewVersion);
                        System.Environment.Exit(0); //无论在主线程和其它线程，只要执行了这句，都可以把程序结束干净; 
                        return;

                    }                    
                }


                //检查是否有新版本     
                System.Threading.Tasks.Task t0 = new System.Threading.Tasks.Task(() =>
                {
                    if (CheckNewVersionNoticeGoVisit())
                    {
                        System.Environment.Exit(0); //无论在主线程和其它线程，只要执行了这句，都可以把程序结束干净; 
                    }
                }
                 );  //新建一个Task,异步执行
                //启动Task
                t0.Start();
                //if (CheckNewVersionNoticeGoVisit()) { return; }


                // System.Reflection.MethodBase.GetCurrentMethod().GetType(
                Log.GetLog(typeof(Program)).Info("程序启动");

                Setting.IsShowDebug = false;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Geo.Utils.FormUtil.ShowSlpash("");
                //AnyInfo
                AnyInfo.Setting.TryReadConfigFromXml();

                Setting.Init();
                var config = Setting.LoadConfig();
                GlobalDataSourceService.Instance.Init(config); //初始化

                //Gnsser.Winform.Setting.CheckOrCleanTempDirectory();
                Action action = new Action(Gnsser.Winform.Setting.CheckOrCleanTempDirectory);             
                System.Threading.Tasks.Task t1 = new System.Threading.Tasks.Task(action);   //新建一个Task,异步执行
                //启动Task
                t1.Start();
                //t1.Wait();//同步执行呢

                StartAsAdmin(new MainForm());

                Setting.SaveConfigToFile();

                //AnyInfo 
                AnyInfo.Setting.TryWriteConfigToXml();

                Log.GetLog(typeof(Program)).Info("程序正常退出");
            }
            //catch (Exception ex)
            //{
            //    DealWithException(ex);
            //}
        }
        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="ex"></param>
        private static void DealWithException(Exception ex)
        {
            try
            {
                var msg = "非差抱歉！发生了未知错误 ";
                if (Geo.Utils.NetUtil.IsConnectedToInternet())
                {
                    msg += "是否愿意将以下错误信息反馈到我们网站？以便我们及时修复。\r\n " + ex.Message;
                    if (Geo.Utils.FormUtil.ShowYesNoMessageBox(msg) == DialogResult.Yes)
                    {              
                        string postData = "Version=" + Setting.Version
                            + "&VersionType=" + Setting.VersionType
                            + "&Content=" + ex.Message;
                        byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                        HttpWebRequest objWebRequest = (HttpWebRequest)WebRequest.Create(Setting.BugReportUrl);
                        objWebRequest.Method = "POST";
                        objWebRequest.ContentType = "application/x-www-form-urlencoded";
                        objWebRequest.ContentLength = byteArray.Length;
                        Stream newStream = objWebRequest.GetRequestStream();
                        // Send the satData. 
                        newStream.Write(byteArray, 0, byteArray.Length); //写入参数 
                        newStream.Close();

                        HttpWebResponse response = (HttpWebResponse)objWebRequest.GetResponse();
                        StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                        string textResponse = sr.ReadToEnd(); // 返回的数据 
                        MessageBox.Show("已经成功反馈，非差感谢您的支持！\r\n" + textResponse);
                    }
                }
                else
                {
                    msg += ex.Message;
                    log.Error(msg);
                    MessageBox.Show(msg);
                }
            }
            catch (Exception ex2)
            {
            }
        }
        /// <summary>
        /// 检查网络新版本
        /// </summary>
        public static bool CheckNewVersionNoticeGoVisit()
        {
            if (Geo.Utils.NetUtil.IsConnectedToInternet())
            {
                Log.GetLog(typeof(Program)).Info("检查新版本");
                var version = Setting.Version;
                var latestVersion = Geo.Utils.NetUtil.GetNetDouble(Setting.CurrentVersionUrl);
                if (version < latestVersion)
                {
                    Log.GetLog(typeof(Program)).Info("新版本：" + latestVersion );
                    
                    var character = Geo.Utils.NetUtil.GetNetString(Setting.CurrentVersionCharacterUrl);
                    if (Geo.Utils.FormUtil.ShowYesNoMessageBox("发现新版本： " + latestVersion + "， 是否前往免费下载？\r\n 特性：" + character)
                        == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(Setting.WebSiteUrlForNewVersion);
                        return true;
                    }
                }

            }
            return false;
        }


        static public void StartAsAdmin(Form form)
        {
            /**
              * 当前用户是管理员的时候，直接启动应用程序
              * 如果不是管理员，则使用启动对象启动程序，以确保使用管理员身份运行
              */
            //获得当前登录的Windows用户标示
            System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);

           // MessageBox.Show(identity.AuthenticationType + "");
            //判断当前登录用户是否为管理员
            //if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
            {
                //如果是管理员，则直接运行
                Application.Run(form);
            }
            //else
            {
                //Start();
                //退出

                 // System.Threading.Thread.Sleep(1000);
                 // Application.Exit();
            }
        }

        private static void Start()
        {
            var log = Log.GetLog(typeof(Setting));
            log.Debug("以非管理员账户启动！");
            MessageBox.Show("以非管理员账户启动！");
            //创建启动对象
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = Environment.CurrentDirectory;
            startInfo.FileName = Application.ExecutablePath;
            //设置启动动作,确保以管理员身份运行
            startInfo.Verb = "runas";
            //try
            //{
            System.Diagnostics.Process.Start(startInfo);
            //}
            //catch
            //{
            //    return;
            //}
        }

    }
}

