//2014.12.03, czs, create in jinxingliaomao shuangliao, 定位信息写入器
//2014.12.06, czs, edit in jinxingliaomao shuangliao, 历元目录加入日期，利于符合多历元计算
//2015.12.25, czs, edit in hongqing, 改进，简化，面向用户
//2016.10.01, czs, edit in hongqing, 优化代码，修复错误
//2018.11.28, czs, edit in hmx, 修改代码

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Gnsser;
using Gnsser.Domain;
using Gnsser.Service; 
using Gnsser.Data.Rinex;
using Geo.Utils;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Common;
using Geo;
using Geo.Algorithm;
using Geo.IO;

namespace Gnsser
{
    /// <summary>
    /// 定位信息写入器
    /// </summary>
    public class GnssResultWriter
    {
        /// <summary>
        /// 日志
        /// </summary>
        protected ILog log = Log.GetLog(typeof(GnssResultWriter));

        /// <summary>
        /// 构造函数，
        /// </summary> 
        /// <param name="option"></param>
        /// <param name="IsWriteEpochInfo"></param>
        /// <param name="IsWriteSatInfo"></param> 
        public GnssResultWriter( GnssProcessOption option,  bool IsWriteEpochInfo = false, bool IsWriteSatInfo = false)
        {
            this.IsWriteEpochInfo = IsWriteEpochInfo;
            this.IsWriteSatInfo = IsWriteSatInfo;
            this.ProjectOutputDirectory = option.OutputDirectory; 
            this.Encoding = Encoding.Default;
            this.Option = option;

            if (!Directory.Exists(ProjectOutputDirectory)) { Directory.CreateDirectory(ProjectOutputDirectory); }

            writer = new GnsserPointPositionResultWriter(ProjectOutputDirectory, option);
            this.SatBasedResultManager = new ObjectTableStorage();
            this.ResultFileNameBuilder = new ResultFileNameBuilder(ProjectOutputDirectory);
        }

        #region 属性
        GnssProcessOption Option { get; set; }
        ObjectTableStorage SatBasedResultManager { get; set; }
        /// <summary>
        /// 字体编码
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// 是否写入历元数据
        /// </summary>
        public bool IsWriteEpochInfo { get; set; }
         
        /// <summary>
        /// 是否写入卫星数据
        /// </summary>
        public bool IsWriteSatInfo { get; set; }

        /// <summary>
        /// 工程目录
        /// </summary>
        public string ProjectOutputDirectory { get; set; }
        #endregion

        GnsserPointPositionResultWriter writer { get; set; }


        /// <summary>
        /// 批量写入文件，写入批量和最后一个结果。
        /// </summary>
        /// <param name="results">计算结果</param>
        public void Write(List<SingleSiteGnssResult> results)
        {
            if (results == null || results.Count == 0) { return; }
            SingleSiteGnssResult last = results[results.Count - 1];
            Write(results, last);
        }

        /// <summary>
        /// 当批量和最优结果分开时，或有参数有可能为NULL时，采用此方法。
        /// </summary>
        /// <param name="results"></param>
        /// <param name="best"></param>
        public void Write(List<SingleSiteGnssResult> results, SingleSiteGnssResult best)
        {
            WriteFinal(best);

            WriteDetails(results);
        }
        /// <summary>
        /// 写各历元
        /// </summary>
        /// <param name="results"></param>
        public void WriteDetails(List<SingleSiteGnssResult> results)
        { 
            if (IsWriteEpochInfo) { WriteEpochInfo(results, ProjectOutputDirectory); }

            if (Option.IsOutputAdjustMatrix) { WriteAdjustMatrix(results, ProjectOutputDirectory); }
             
            if (IsWriteSatInfo) { WriteSatelliteInfos(results, ProjectOutputDirectory); }
        }


        /// <summary>
        /// 写入最终结果
        /// </summary>
        /// <param name="best"></param>
        public void WriteFinal(BaseGnssResult best)
        {
            if (best == null) { return; }
            WriteFinalResult(best, ProjectOutputDirectory);
            if (best is SingleSiteGnssResult)
            { 
                writer.Write(best as SingleSiteGnssResult);
            }
        }

        /// <summary>
        /// 写 Sinex 文件
        /// </summary>
        /// <param name="results">定位结果</param>
        public void WriteSinexFile(List<SingleSiteGnssResult> results)
        {
            if (results == null || results.Count == 0) { return; }

            int i = 0;
            string dir = Path.Combine(this.ProjectOutputDirectory, "Sinex");
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            foreach (var item in results)
            {
                string path = Path.Combine(dir, item.Name + "_" + (i++) + ".SNX");
                WriteToSinex(path, item);
            }
        }

        /// <summary>
        /// 写到文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="result"></param>
        public static void WriteToSinex(string path, SingleSiteGnssResult result)
        {
            File.WriteAllText(path, ResultSinexBuillder.Build(result).ToString(), Encoding.Default);
        }
        

        //写共享文档锁
        static object locker = new object();
        ResultFileNameBuilder ResultFileNameBuilder { get; set; }
        /// <summary>
        /// 写入总文件
        /// </summary>
        /// <param name="best">最后历元计算结果</param>
        /// <param name="directory">工程目录</param>
        private void WriteFinalResult(BaseGnssResult best, string directory)
        {
            if (best == null) { return; }

            lock (locker)//共享文档可能存在冲突
            {
                Geo.Utils.FileUtil.CheckOrCreateDirectory(directory);

                string allPath = ResultFileNameBuilder.BuildFinalResultFilePath(best.GetType()); 

                string str = AppentShortTabToFile(best, allPath);
                
                //所有汇总文件
                var globalPath = Setting.GnsserConfig.PppResultFile;

                try
                {
                    if (best is PppResult)
                    {
                        AppentShortTabToFile(best, globalPath);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("写入 PPP 总汇总文件出现错误：" + ex.Message +"\r\n" +globalPath);
                }
            }
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="best"></param>
        /// <param name="allPath"></param>
        /// <returns></returns>
        private string AppentShortTabToFile(BaseGnssResult best, string allPath)
        {
            if (!File.Exists(allPath)) {
                Geo.Utils.FileUtil.CheckOrCreateDirectory(Path.GetDirectoryName(allPath));
                var str1 =  best.ToShortTabTitles();
                File.AppendAllText(allPath, str1 + "\r\n", Encoding);
            }

            string str = best.ToShortTabValue();
            File.AppendAllText(allPath, str + "\r\n", Encoding);
            return str;
        }

        /// <summary>
        ///  写平差矩阵文本信息
        /// </summary>
        /// <param name="results">定位计算结果</param>
        /// <param name="dirPath">工程目录</param>
        private void WriteAdjustMatrix(List<SingleSiteGnssResult> results, string dirPath)
        {
            if (results == null || results.Count == 0) { return; }
            Geo.Utils.FileUtil.CheckOrCreateDirectory(dirPath);

            EpochInformation epoch = results[results.Count - 1].MaterialObj;
            string marker = "_" + (int)DateTime.Now.TimeOfDay.TotalSeconds + "";
            string path = Path.Combine(dirPath, epoch.SiteInfo.SiteName + "_" + epoch.ReceiverTime.ToDateString() + "_平差矩阵" + marker + ".adjust.txt");
            
            //常数项，没有按照卫星对应输出。
            StringBuilder sb = new StringBuilder();
            int i = 0; 
            foreach (var result in results)
            {
                sb.AppendLine(result.ResultMatrix.ObsMatrix.ToReadableText());

                i++;
            }

            File.AppendAllText(path, sb.ToString(), Encoding); 
        }
         
        /// <summary>
        /// 写历元信息
        /// </summary>
        /// <param name="results"></param>
        /// <param name="dirPath"></param>
        private void WriteEpochInfo(List<SingleSiteGnssResult> results, string dirPath)
        {
            if (results == null || results.Count == 0) { return; }
            Geo.Utils.FileUtil.CheckOrCreateDirectory(dirPath);

            EpochInformation epoch = results[results.Count - 1].MaterialObj;

            string marker = "_" + (int)DateTime.Now.TimeOfDay.TotalSeconds;
            string pathEpoch = Path.Combine(dirPath, epoch.SiteInfo.SiteName + "_" + epoch.ReceiverTime.ToDateString() + marker + ".xls");

            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

            //写入到文件
            StringBuilder sb = new StringBuilder();
            int i = 0;
            bool isPPP = false;
            foreach (var result in results)
            {
                if (isPPP || result is PppResult)
                {
                    isPPP = true;
                    var pppresult = result as PppResult;
                    EpochInformation EpochInfo = pppresult.MaterialObj;
                    sb.Append(EpochInfo.SiteInfo.SiteName);
                    sb.Append("\t");
                    sb.Append(EpochInfo.ReceiverTime.ToString());
                    sb.Append("\t");


                    var str = SatBasedResultManager.ToSplitedValueString(pppresult.AmbiguityDic);
                    sb.Append(str);
                    sb.AppendLine();

                }
                else
                {
                    if (i == 0) sb.AppendLine(result.GetTabTitles());
                    sb.AppendLine(result.GetTabValues());
                }
                i++;
            }
            if (isPPP)
            {
                var titles = "Name" + "\t" + "Time" + "\t" + SatBasedResultManager.ToSplitedTitleString() + "\r\n";
                sb.Insert(0, titles);
            }
            File.AppendAllText(pathEpoch, sb.ToString(), Encoding);
        }
        /// <summary>
        /// 写历元测站->卫星信息
        /// </summary>
        /// <param name="results">定位计算结果</param>
        /// <param name="dirPath">工程目录</param>
        private void WriteSatelliteInfos(List<SingleSiteGnssResult> results, string dirPath)
        {
            if (results == null || results.Count == 0) { return; }
            Geo.Utils.FileUtil.CheckOrCreateDirectory(dirPath);

            EpochInformation epoch = results[results.Count - 1].MaterialObj;

            StringBuilder sb = new StringBuilder();
            string satePath = Path.Combine(dirPath, epoch.SiteInfo.SiteName + "_" + epoch.ReceiverTime.ToDateString() + "_" + "SatDetail");


            string marker = "_" + (int)DateTime.Now.TimeOfDay.TotalSeconds + "";

            //改正数文件
            Dictionary<SatelliteNumber, string> prnPathes = new Dictionary<SatelliteNumber, string>();
            foreach (var item in results)
            {
                foreach (var sat in item.MaterialObj)
                {
                    if (!prnPathes.ContainsKey(sat.Prn))
                    {
                        string path = Path.Combine(satePath, sat.Prn + marker + ".xls");
                        prnPathes.Add(sat.Prn, path);
                        File.AppendAllText(prnPathes[sat.Prn], sat.GetTabTitles() + "\r\n", Encoding);
                        sb.AppendLine(path);
                    }
                    File.AppendAllText(prnPathes[sat.Prn], sat.GetTabValues() + "\r\n", Encoding);
                }
            }
        }
    }

}
