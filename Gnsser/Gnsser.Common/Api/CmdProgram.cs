//2015.01.18, czs, create in namu, 为了满足控制台程序需求，特建立此项目

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.IO; 
using Gnsser;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Times;
using Gnsser.Service;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.IO;
using Geo;
using Geo.Times;

namespace Gnsser.Cmd
{
    /// <summary>
    /// 为了满足控制台程序需求，特建立此项目
    /// </summary>
    public class CmdProgram
    {
        #region 变量、属性
        static GnsserConfig GnsserConfig;

        static List<BaseGnssResult> _results = new List<BaseGnssResult>();
        static DateTime startTime = DateTime.Now;
        static ISingleSiteObsStream observationDataSource;
        static FileEphemerisService ephemerisDataSource;
        static Data.ClockService clockFile;

        static CaculateType CaculateType = CaculateType.Filter;

        static int ProcessCount = 0;
        static int StartEphoch = 0;
        static int MaxProcessCount = 100000000;
        static ISiteInfo SiteInfo;

        #endregion

        /// <summary>
        /// 命令行参数是以空格符分开的。
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                //首先欢迎
                ShowInfo("              GNSSer——GNSS数据处理软件   ");
                ShowInfo("-------------   Version 0.1, Email:gnsser@163.com    --------------------");

                ProcessCount = 0;
                startTime = DateTime.Now;

                string configPath = "gnsser.conf";

                if (args.Length > 0)
                {
                    ShowInfo("请在配置文件“" + configPath + "”中修改后运行本程序。此版本不需要命令行参数。");
                    return;
                }

                IStreamGnssService pp = null;
                //读取配置文件
                ConfigReader configReader = new ConfigReader(configPath);
                GnsserConfig = new Gnsser.GnsserConfig(configReader.Read());

                ShowInfo("配置文件：" + configPath);
                ShowInfo("观测文件：" + GnsserConfig.ObsPath);
                if (File.Exists(GnsserConfig.NavPath)) ShowInfo("导航文件：" + GnsserConfig.NavPath);
                if (File.Exists(GnsserConfig.ClkPath)) ShowInfo("钟差文件：" + GnsserConfig.ClkPath);
                //读取观测数据
                ReadFiles(GnsserConfig);

                ProcessCount = 0;

                ShowInfo("请稍后，GNSSer 正在努力计算中.....");
                GnssProcessOption option = GnssProcessOption.GetDefault(GnsserConfig, observationDataSource.ObsInfo);
                //pp = new AdaptablePointPosition(observationDataSource, option, ephemerisDataSource, clockFile); 
                pp.Produced += pp_Produced;
                var results = pp.Gets( StartEphoch, MaxProcessCount);
                
                ShowInfo("");
                TimeSpan span = DateTime.Now - startTime;
                ShowInfo("计算完毕, 呵呵 :-D,耗时（秒）：" + span.TotalSeconds); 

                if (results.Count > 0)
                {
                    var last = results[results.Count - 1] as SingleSiteGnssResult;
                    //ShowInfo("以下为最后历元结果：");
                    //ShowInfo(last.GetTabTitles());
                    //ShowInfo(last.GetTabValues());
                    ShowInfo("最后历元:" + last .ReceiverTime.ToString());
                    ShowInfo("估值坐标: " + last.EstimatedXyz);
                    string projectName = new ProjectNameBuilder(SiteInfo.SiteName, results[0].ReceiverTime).Build();

                    string ProjectOutputDirectory = Path.Combine(GnsserConfig.OutputDirectory, projectName);
                    if (GnsserConfig.IsOutputResult)
                    {
                        ShowInfo("请稍后！正在写入计算结果。。。");
                        GnssResultWriter writer = new GnssResultWriter(option,  GnsserConfig.IsOutputAdjust);
                        writer.Write(results);
                    }

                    if (GnsserConfig.IsOutputResult)
                    {
                        ShowInfo("详细计算结果在：" + ProjectOutputDirectory);
                    }
                }
                else
                {
                    ShowInfo("程序执行完毕，%>_<% 但是没有计算结果！");
                } 
            }
            catch (Exception ex)
            {
                ShowInfo(ex.Message);
            }
            //暂停屏幕
            System.Console.ReadLine();
        }

        static void pp_Produced(BaseGnssResult product, EpochInformation material)
        {
            ProcessCount++;
            if (GnsserConfig.IsShowResultOnTime)
            {
                ShowInfo(product.GetTabValues());
            }
            else
            {
                //简易进度条
                if (ProcessCount % 100 == 0)
                {
                    System.Console.Write(".");
                } 
            }
        }

        /// <summary>
        /// 根据后缀名称，自动提取配置文件
        /// </summary> 
        /// <param name="files">待寻找的路径</param>
        private static string GetConfigFilePath(string[] files)
        {
            string configPath = null;
            foreach (var item in files)
            { 
                if (item.Trim().EndsWith("conf", StringComparison.CurrentCultureIgnoreCase))
                {
                    configPath = item;
                    break;
                }
            }
            return configPath;
        }
        /// <summary>
        /// 数据源读取
        /// </summary>
        private static void ReadFiles(GnsserConfig config)
        { 
            //加载文件数据
            observationDataSource = new RinexFileObsDataSource(config.ObsPath);
            SiteInfo = observationDataSource.SiteInfo;
            //是否读取星历文件，如果设置了，且存在则读取只
            if (File.Exists(config.NavPath))
            {
                ephemerisDataSource = EphemerisDataSourceFactory.Create(config.NavPath);
            }
            //是否有精密星历
            if (File.Exists(config.ClkPath))
            {
                clockFile = new ClockService(config.ClkPath);
            }
        }

        /// <summary>
        /// 输出信息到控制台。
        /// </summary>
        /// <param name="msg"></param>
        static void ShowInfo(string msg)
        {
            System.Console.WriteLine(msg);
        } 

    }
}
