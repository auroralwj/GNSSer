//2015.04.14, czs, edit in namu ， 补齐注释

using System;
using System.Collections.Generic;
using System.Text;

namespace Geo.Utils
{
    /// <summary>
    /// 界面工具
    /// </summary>
    public class UiUtil
    {
        #region 欢迎界面
        /// <summary>
        /// 系统名称
        /// </summary>
        public static string SystemName = "信息系统"; 
        /// <summary>
        /// 欢迎界面
        /// </summary>
        public static void ShowSplash(string systemName = "信息系统")
        {
            UiUtil.SystemName = systemName;
            //splash
            System.Threading.Thread myThread = new System.Threading.Thread(new System.Threading.ThreadStart(MyStartingMethod));
            myThread.Start();
        }
        /// <summary>
        /// 在当前线程启动启动界面
        /// </summary>
        public static void MyStartingMethod()
        {
            SplashForm splashForm = new SplashForm();
            splashForm.Title = SystemName;
            splashForm.ShowDialog();
        }
        /// <summary>
        /// 显示等待数据库连接
        /// </summary>
        public static void ShowWaittingForConnectionDbForm()
        {
            //splash
            System.Threading.Thread myThread = new System.Threading.Thread(new System.Threading.ThreadStart(ShowWaittingForConnectionDb));
            myThread.Start();
        }
        /// <summary>
        /// 显示正在连接数据库，请等待
        /// </summary>
        public static void ShowWaittingForConnectionDb()
        {
           Geo.Utils. WaitingForm splashForm = new WaitingForm("请稍后，正在连接数据库.....");
            splashForm.ShowDialog();
        } 

        #endregion 
    }
}
