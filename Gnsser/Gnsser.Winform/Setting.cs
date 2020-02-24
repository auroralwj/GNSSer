//2015.01.19, czs, edit in namu, 抽离了Confg出来，配置文件保存到 Gnsser.conf  中。


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Windows.Forms;
using Gnsser.Times;
using Geo.IO;
using System.IO;
using Geo;
using Geo.Times;

namespace Gnsser.Winform
{
    /// <summary>
    /// 程序数据设置
    /// </summary>
    public  class Setting : Gnsser.Setting
    {
       
        public static string configPath = "gnsser.conf";
        static ILog log = Log.GetLog(typeof(Setting));
        public static string HelpDocument = @"Data\Documents\Help.pdf";
        /// <summary>
        /// 表格显示，最大的行数。
        /// </summary>
        internal static int MaxTableRowCount = 30000;
        /// <summary>
        /// 表格显示，最大的列数。
        /// </summary>
        internal static int MaxTableColCount = 500;


        /// <summary>
        /// 加载文件。保存到 GnsserConfig 属性中。
        /// </summary>
        /// <returns></returns>
        public static GnsserConfig LoadConfig()
        { 
             ConfigReader configReader = new ConfigReader(configPath);
             Setting.GnsserConfig = new Gnsser.GnsserConfig(configReader.Read());

            Geo.Utils.ObjectUtil.VisitAllProperties( (Setting.GnsserConfig));


            CurrentOperationManager = GnsserOperationManager.Default;

             log.Info("成功加载配置文件 " + configPath);
             return GnsserConfig;
        }
        /// <summary>
        /// 保存到文件中去
        /// </summary>
        public static void SaveConfigToFile()
        {
            Geo.IO.ConfigWriter writer = new Geo.IO.ConfigWriter(configPath);
            writer.Write(GnsserConfig);
            log.Info("保存了配置到文件 " + configPath);
        } 
        /// <summary>
        /// 操作管理器。通常可以通过控制此内容设置权限。
        /// </summary>
        public static Geo.OperationManager CurrentOperationManager { get; set; }


        /// <summary>
        /// 临时目录检查和清空,需要form支持
        /// </summary>
        public static void CheckOrCleanTempDirectory()
        {
            if (Directory.Exists(Setting.TempDirectory))
            {
                var files = Directory.GetFiles(Setting.TempDirectory);
                var direcotries = Directory.GetDirectories(Setting.TempDirectory);
                if (files.Length > 0 || direcotries.Length > 0)
                {
                    if (Geo.Utils.FormUtil.ShowYesNoMessageBox("临时目录存在 " + files.Length + " 个文件，"+ direcotries.Length + "个目录，是否立即清空？\r\n您也可以稍后手动删除，或使用 文件/清空临时目录 " + Setting.TempDirectory) == DialogResult.Yes)
                    {
                        Setting.TryClearTempDirectory();
                    }
                }
            }
        } 

    }
}
