
//2018.10.12, czs, create in hmx, IGS产品提取器 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.Common;
using Gnsser.Service;
using Gnsser.Times;
using Geo.IO;
using System.IO;
using Gnsser.Data.Rinex;
using Gnsser.Data;
using Geo.Times;

namespace Gnsser.Data
{
    /// <summary>
    /// IGS 文件产品提取器
    /// </summary>
    public class IgsProductFileExtractor
    {
        protected Log log = new Log(typeof(IgsProductFileExtractor));

        /// <summary>
        /// 多系统数据源服务
        /// </summary>
        public IgsProductFileExtractor(IgsProductSourceOption opt, IgsProductType IgsProductSourceType, int TimeIntervalSeconds = 86400)
        {
            this.Option = opt;
            this.TimeIntervalSeconds = TimeIntervalSeconds;
            this.IgsProductSourceType = IgsProductSourceType;
            this.IgsProductUrlPathBuilder = new IgsProductUrlPathBuilder(Option.IgsProductUrlDirectories, Option.IgsProductSourceDic, Option.IgsProductUrlModels, IgsProductSourceType, TimeIntervalSeconds);
            this.IgsProductLocalPathBuilder = new IgsProductLocalPathBuilder(Option.IgsProductLocalDirectories.ToArray(), Option.IgsProductSourceDic, IgsProductSourceType, TimeIntervalSeconds == 604800);
            this.InputFileManager = new Geo.IO.InputFileManager(Option.IgsProductLocalDirectory);
            this.InputFileManager.FileDownloaded += InputFileManager_FileDownloaded;
        }
        void InputFileManager_FileDownloaded(string localFileName, string url)
        {
            if (!this.Option.IsDownloadingSurplurseIgsProducts)
            {
                log.Info("取消多余文件的下载。");
                this.InputFileManager.IsCancelDownloading = true;
            }
        }
        /// <summary>
        /// 时间间隔，决定是时(3600)、天(86400)、还是周
        /// </summary>
        public int TimeIntervalSeconds { get; set; }
        /// <summary>
        /// IGS URL地址生成器
        /// </summary>
        public IgsProductUrlPathBuilder IgsProductUrlPathBuilder { get; set; }
        /// <summary>
        /// IGS 产品类型
        /// </summary>
        protected IgsProductType IgsProductSourceType { get; set; }

        /// <summary>
        /// 数据源选项
        /// </summary>
        protected IgsProductSourceOption Option { get; set; }
        /// <summary>
        /// 文件路径管理器
        /// </summary>
        protected Geo.IO.InputFileManager InputFileManager { get; set; }
        /// <summary>
        /// 路径生成器
        /// </summary>
        protected IgsProductLocalPathBuilder IgsProductLocalPathBuilder { get; set; }
        #region 方法 
        /// <summary>
        /// 获取本地文件路径
        /// </summary>
        /// <returns></returns>
        public List<string> GetLocalFilePathes()
        {
            List<string> list = new List<string>();
            foreach (var satType in this.Option.SatelliteTypes)
            {
                list.AddRange(CreateSatTypeBasedServices(satType));
            }
            list = list.Distinct().ToList();//去除重复
            return list;
        }


        /// <summary>
        /// 建立按天组织的服务。一个系统类型一个服务。
        /// 如果失败，则网络获取一次。
        /// </summary>
        /// <param name="satType"></param>
        /// <returns></returns>
        protected List<string> CreateSatTypeBasedServices(SatelliteType satType)
        {
            List<string> dic = new List<string>();

            //按天生产星历服务
            for (var time = this.Option.TimePeriod.Start; time <= this.Option.TimePeriod.End; time = time + TimeSpan.FromSeconds(TimeIntervalSeconds))
            {
                var files = GetFiles(satType, time);
                if (files != null && files.Count > 0)
                {
                    dic.AddRange(files);
                }
                else
                {
                    log.Warn("创建服务失败！" + IgsProductSourceType + ", " + time);
                    log.Info("即将尝试获取远程数据源....");
                    if (!Setting.IsNetEnabled) { log.Warn("网络不可用或未启用，无法下载！"); return dic; }

                    //如果为空，则下载文件继续，一次下载机会 
                    DownloadProduct(time, satType);

                    files = GetFiles(satType, time);
                    if (files != null)
                    {
                        dic.AddRange(files);
                    }

                    if (files.Count > 0 && !Option.IsDownloadingSurplurseIgsProducts)
                    {
                        log.Info("已经具有 " + files.Count + " 个服务，且设置为不下载多余产品，取消继续下载。");
                        return dic;
                    }
                }
            }
            return dic;
        }
        /// <summary>
        /// 根据类型和历元，按照给定的文件命名规则，读取本地文件
        /// </summary>
        /// <param name="satType">卫星类型</param>
        /// <param name="gpsTime">时间</param>
        /// <returns></returns>
        protected virtual List<string> GetFiles(SatelliteType satType, Time gpsTime)
        {
            List<string> pathes = GetIgsProductLocalPathes(satType, gpsTime);

            List<string> services = new List<string>();
            foreach (var path in pathes)//便利所有生产的路径，选择一个可用的
            {
                if (!System.IO.File.Exists(path))
                {
                    log.Debug("不存在预期产品 " + this.IgsProductSourceType + ", " + path);
                    continue;
                }
                try
                {
                    var TargetExtention = "*" + IgsProductNameBuilder.GetFileExtension(IgsProductSourceType, gpsTime).TrimEnd('Z', '.');
                    var filePath = InputFileManager.GetLocalFilePath(path, TargetExtention, "*.*");
                    if (filePath == null)
                        continue;

                    FileInfo info = new FileInfo(path);
                    if (info.Length == 0)
                    {
                        info.Delete();
                        continue;
                    }

                    //包含则添加
                    services.Add(path);

                }
                catch (Exception ex)
                {
                    log.Error("文件解析错误，" + path + ", " + ex.Message);
                }
                break;
            }

            return services;
        }

        /// <summary>
        /// 获取本地库的IGS产品路径,如果指定了唯一数据源，则会只返回该数据源，否则，返回全部。
        /// 自动过滤并删除大小为0的文件。
        /// </summary>
        /// <param name="satType"></param>
        /// <param name="gpsTime"></param>
        /// <returns></returns>
        protected List<string> GetIgsProductLocalPathes(SatelliteType satType, Time gpsTime)
        {
            List<string> pathes = IgsProductLocalPathBuilder.Get(satType, gpsTime);

            pathes = Geo.Utils.FileUtil.RemoveZeroFiles(pathes);

            if (Option.IsUniqueSource)
            {
                pathes = pathes.FindAll(m => Path.GetFileName(m).StartsWith(Option.IndicatedSourceCode));
            }

            return pathes;
        }

        /// <summary>
        /// 下载IGS产品。并返回成功后的本地路径。
        /// </summary>
        /// <param name="gpsTime"></param>
        /// <param name="satType"></param>
        /// <returns></returns>
        protected List<string> DownloadProduct(Time gpsTime, SatelliteType satType = SatelliteType.G)
        {
            var urls = IgsProductUrlPathBuilder.SetSatelliteType(satType).Build(gpsTime).ToList();

            if (Option.IsUniqueSource)
            {
                urls = urls.FindAll(m => Path.GetFileName(m).StartsWith(Option.IndicatedSourceCode));
            }


            var TargetExtention = "*" + IgsProductNameBuilder.GetFileExtension(IgsProductSourceType, gpsTime).TrimEnd('Z', '.');
            return InputFileManager.GetLocalFilePathes(urls.ToArray(), TargetExtention, TargetExtention + ";" + TargetExtention + ".Z", this.Option.IsDownloadingSurplurseIgsProducts);
        }
        #endregion
    }

}