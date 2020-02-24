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
using Gnsser.Api;

namespace Gnsser.Cmd
{
    /// <summary>
    /// 为了满足控制台程序需求，特建立此项目
    /// </summary>
    public  class Program
    {
        /// <summary>
        /// 命令行参数是以空格符分开的。
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //try
            //{
                //首先欢迎
                ShowInfo("              GNSSer——GNSS数据处理软件   ");
                ShowInfo("-------------   Version 0.1, Email:gnsser@163.com    --------------------");

                OperationManager OperationManager = new Geo.OperationManager();
                OperationManager.Regist(new FtpDownload());


                var path = @"Data\Gpe\test.gpe";// args[0];

                OperationProcessEngine ProcessEngine = new OperationProcessEngine(OperationManager, path);
                ProcessEngine.Process();

            //}
            //catch (Exception ex)
            //{
            //    ShowInfo(ex.Message);
            //}
            //暂停屏幕
            System.Console.ReadLine();
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
        /// 输出信息到控制台。
        /// </summary>
        /// <param name="msg"></param>
        static void ShowInfo(string msg)
        {
            System.Console.WriteLine(msg);
        } 

    }
}
