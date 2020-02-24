//2014.10.27, czs, create in numu, DCB数据服务,一月一个文件，从CODE读取
//2017.11.04, czs, edit in honginqg, DCB 自动下载，读取和推估
//2019.05.15, czs, edit in hongqing, 切换目录支持其它IGS目录存放

using System;
using System.Collections.Generic;
using System.Linq;
using Gnsser.Times;
using System.Text;
using System.IO;
using Gnsser.Service;
using Geo.Algorithm;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Utils;
using Gnsser;
using Geo;
using Geo.Times; 
using Geo.Common;

namespace Gnsser.Data
{ 
    /// <summary>
    /// 卫星DCB服务。
    /// </summary>
    public class DcbDataService : FileBasedService<RmsedNumeral>, Geo.IService<RmsedNumeral>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dcbFileDir">地址目录</param>
        /// <param name="Option">设置</param>
        public DcbDataService(string dcbFileDir, GnssProcessOption Option = null) : base(dcbFileDir)
        {
            this.GnssOption = Option;
            data = new Dictionary<string, DcbFile>();
            InputFileManager = new Geo.IO.InputFileManager(dcbFileDir);

            TotalLocalDirectories = new List<string>();
            TotalLocalDirectories.Add(dcbFileDir);
            TotalLocalDirectories.Add(DownloadDirectory);
            TotalLocalDirectories.AddRange(IgsProductDirectories);
        }

        /// <summary>
        /// 总共
        /// </summary>
        public List<string> TotalLocalDirectories { get; set; }
        /// <summary>
        /// 配置
        /// </summary>
        public GnssProcessOption GnssOption { get; set; }
        /// <summary>
        /// 本地下载目录
        /// </summary>
        public string DownloadDirectory => Setting.GnsserConfig.IgsProductLocalDirectory;
        /// <summary>
        /// IGS 产品目录
        /// </summary>
        public List<string> IgsProductDirectories => Setting.GnsserConfig.IgsProductLocalDirectories;
        
        /// <summary>
        /// 文件路径管理器
        /// </summary>
        protected Geo.IO.InputFileManager InputFileManager { get; set; }

        Dictionary<string, DcbFile> data;

        static object locker = new object();
        public RmsedNumeral GetP1C1(SatelliteNumber prn, Time time)
        {
           return Get(prn, time, "P1C1");  
        }
        public RmsedNumeral GetP1P2(SatelliteNumber prn, Time time)
        {
            return Get(prn, time, "P1P2");
        }
        public RmsedNumeral GetP2C2(SatelliteNumber prn, Time time)
        {
            if (GnssOption == null ||  !GnssOption.IsP2C2Enabled) { return RmsedNumeral.Zero; }
            return GetP2C2_rinex(prn, time, "P2C2");
        }
        
        
       // string urlModel = "ftp://ftp.aiub.unibe.ch/CODE/2017/P1P21709.DCB.Z"
        string urlModel = "ftp://ftp.aiub.unibe.ch/CODE/{Year}/"+ ELMarker.ProductType+"{SubYear}{Month}.DCB.Z";

        /// <summary>
        /// 所有可能的
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public List<string> GetPosibleFilePathes(string fileName)
        {
            var allPathes = new List<string>();
            foreach (var dir in TotalLocalDirectories)
            {
                var path = Path.Combine( dir, fileName);
                allPathes.Add(path);
            }
            return allPathes;
        }


        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="time"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public RmsedNumeral Get(SatelliteNumber prn, Time time, string fileType = "P1C1")
        {
            lock (locker)
            {
                string key = TimeTokey(time, fileType);
                DcbFile dcbFile = null;
                if (!data.ContainsKey(key))
                {
                    string fileName = key + ".DCB"; 

                    var allPathes = GetPosibleFilePathes(fileName);
                    foreach (var path in allPathes)
                    {
                        if (File.Exists(path))
                        {
                            dcbFile = new DcbFileReader(path).ReadP2C2();
                            log.Info("DCB 载入 " + path);
                            //是 null 也存储
                            data[key] = dcbFile;
                            break;//匹配成功一个即中断
                        }
                    }


                    //首先尝试下载
                    if (dcbFile == null && Setting.IsInternetEnabled) 
                    {
                        var url = BuildUrl(time, fileType);
                        var localPath = InputFileManager.GetLocalFilePath(url, "*.DCB", "*.DCB.Z");
                        if (File.Exists(localPath))
                        {
                            dcbFile = new DcbFileReader(localPath).Read();
                            log.Info("DCB 载入 " + localPath);
                        }
                        else
                        {
                            log.Info("没有找到 DCB   " + localPath); 
                        }
                    }

                    //是 null 也存储
                     data[key] = dcbFile;

                    #region 其它情况
                    //如果网络不通，或下载失败，则查找附近几个月的结果，先前2月，后考虑后两月。
                    //if (dcbFile == null)
                    //{
                    //    var nearTime = time - TimeSpan.FromDays(30.5); ;
                    //    dcbFile = GetNearDcbFile(nearTime, fileType);
                    //}
                    //if (dcbFile == null)
                    //{
                    //    var nearTime = time + TimeSpan.FromDays(30.5);
                    //    dcbFile = GetNearDcbFile(nearTime, fileType);
                    //} if (dcbFile == null)
                    //{
                    //    var nearTime = time - TimeSpan.FromDays(61);
                    //    dcbFile = GetNearDcbFile(nearTime, fileType);
                    //} if (dcbFile == null)
                    //{
                    //    var nearTime = time + TimeSpan.FromDays(61);
                    //    dcbFile = GetNearDcbFile(nearTime, fileType);
                    //}
                    //if (dcbFile != null)
                    //{
                    //    data[key] = dcbFile;
                    //    log.Info("本次处理 DCB 采用相邻时间文件 " + dcbFile);
                    //}
                    #endregion 
                }
                else
                {
                    dcbFile = data[key];
                }

                if (dcbFile == null) { return RmsedNumeral.Zero; }

                return dcbFile.GetSatInfo(prn);
            }
        }

        /// <summary>
        /// P2C2与P1C1和P1P2不同
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="time"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public RmsedNumeral GetP2C2_rinex(SatelliteNumber prn, Time time, string fileType = "P2C2")
        {
            lock (locker)
            {
                string key = TimeTokey(time, fileType);
                DcbFile dcbFile = null;
                if (!data.ContainsKey(key))
                {
                    string fileName = key + "_RINEX.DCB";

                    var allPathes = GetPosibleFilePathes(fileName);
                    foreach (var path in allPathes)
                    {
                        if (File.Exists(path))
                        {
                            dcbFile = new DcbFileReader(path).ReadP2C2();
                            log.Info("DCB 载入 " + path);
                            //是 null 也存储
                            data[key] = dcbFile;
                            break;//匹配成功一个即中断
                        } 
                    }

                    //首先尝试下载
                    if (dcbFile == null && Setting.IsInternetEnabled)
                    {
                        var url = BuildUrl(time, fileType);
                        var localPath = InputFileManager.GetLocalFilePath(url, "*_RINEX.DCB", "*_RINEX.DCB.Z");
                        if (File.Exists(localPath))
                        {
                            dcbFile = new DcbFileReader(localPath).Read();
                            log.Info("DCB 载入 " + localPath);
                            //是 null 也存储
                            data[key] = dcbFile;
                        }
                    }
                    #region 其它情况
                    //如果网络不通，或下载失败，则查找附近几个月的结果，先前2月，后考虑后两月。
                    //if (dcbFile == null)
                    //{
                    //    var nearTime = time - TimeSpan.FromDays(30.5); ;
                    //    dcbFile = GetNearDcbFile(nearTime, fileType);
                    //}
                    //if (dcbFile == null)
                    //{
                    //    var nearTime = time + TimeSpan.FromDays(30.5);
                    //    dcbFile = GetNearDcbFile(nearTime, fileType);
                    //} if (dcbFile == null)
                    //{
                    //    var nearTime = time - TimeSpan.FromDays(61);
                    //    dcbFile = GetNearDcbFile(nearTime, fileType);
                    //} if (dcbFile == null)
                    //{
                    //    var nearTime = time + TimeSpan.FromDays(61);
                    //    dcbFile = GetNearDcbFile(nearTime, fileType);
                    //}
                    #endregion 

                    if (dcbFile != null)
                    {
                        data[key] = dcbFile;
                        log.Info("本次处理 DCB 采用相邻时间文件 " + dcbFile);
                    }
                }
                else
                {
                    dcbFile = data[key];
                }

                if (dcbFile == null) { return RmsedNumeral.Zero; }

                return dcbFile.GetSatInfo(prn);
            }
        }
        private DcbFile GetNearDcbFile(Time nearTime, string fileType)
        {
            DcbFile nearDcbFile = null;
            var newName = TimeTokey(nearTime, fileType) + ".DCB";
            string newpath = Path.Combine(this.Option.FilePath, newName);
            if (File.Exists(newpath))
            {
                nearDcbFile = new DcbFileReader(newpath).Read();
                log.Info("DCB 载入临近时间 "  + newpath);
            }
            return nearDcbFile;
        }
        /// <summary>
        /// 构建远程路径
        /// </summary>
        /// <param name="time"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        private string BuildUrl(Time time, string fileType)
        {
            Dictionary<string, string> dic = ELMarkerReplaceService.GetTimeKeyWordDictionary(time);
            dic.Add(ELMarker.ProductType, fileType.ToString());
            ELMarkerReplaceService elService = new Geo.ELMarkerReplaceService(dic);
            var url = elService.Get(urlModel);
            return url;
        }
        /// <summary>
        /// P1P21709
        /// </summary>
        /// <param name="time"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        private static string TimeTokey(Time time,string fileType)
        {
            string key = fileType + time.Year.ToString().Substring(2) + time.Month.ToString("00");
            return key;
        }

    }
  
}
