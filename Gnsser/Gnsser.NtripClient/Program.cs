using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Geo.Common;
using Gnsser.Domain;
using Geo.IO;
using Gnsser.Ntrip.WinForms;

namespace Gnsser.Ntrip
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // System.Reflection.MethodBase.GetCurrentMethod().GetType(
            //   Logger.GetLog(typeof( Program)).Info("程序启动");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            CheckOrCleanTempDirectory();

            Setting.Init();
            Gnsser.Winform.Setting.LoadConfig();

            Setting.LoadNtripSourceManager();
            Application.Run(new MainlyForm());

            Gnsser.Winform.Setting.SaveConfigToFile();
             
            //   Logger.GetLog(typeof(Program)).Info("程序正常退出");
        }
        /// <summary>
        /// 临时目录检查和清空
        /// </summary>
        public static void CheckOrCleanTempDirectory()
        { 
            if (Directory.Exists(Setting.TempDirectory))
            {
                var files = Directory.GetFiles(Setting.TempDirectory);
                if (files.Length > 0)
                {
                    if (Geo.Utils.FormUtil.ShowYesNoMessageBox("检测到临时目录还存在 " + files.Length + " 个文件，是否立即清空？" + Setting.TempDirectory) == DialogResult.OK)
                    {
                        Setting.TryClearTempDirectory();
                    }
                }
            }
        }
    }
}
