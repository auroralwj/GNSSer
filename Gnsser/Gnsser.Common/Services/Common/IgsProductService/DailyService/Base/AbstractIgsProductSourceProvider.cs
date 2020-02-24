//2018.03.16, czs, create in hmx, 根据输入设置，提供连续的多系统的星历服务
//2018.03.17, czs, edit in hmx, 提取为接口同时为钟差和星历服务
//2018.05.09, czs, edit in hmx, IGS产品接口增加时段，避免空时段数据加入服务
//2019.01.06, czs, edit in hmx, 增加时间分辨率，修复IGS超快星历访问

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
    /// IGS产品接口
    /// </summary>
    public interface IIgsProductFile
    {
        /// <summary>
        /// 代码
        /// </summary>
        BufferedTimePeriod TimePeriod { get; }
        /// <summary>
        /// 代码
        /// </summary>
        string SourceCode { get; }
    } 

    /// <summary>
    /// 根据输入设置，提供连续的多系统的产品服务。
    /// 如果本地没有，则自动从网络下载。
    /// </summary> 
    public abstract class AbstractIgsProductSourceProvider<TIgsProductFile, TService> : BaseIgsProductSourceProvider<TService> where TIgsProductFile : IIgsProductFile
    {
        /// <summary>
        /// 日志记录。错误信息记录在日志里面。
        /// </summary> 
       new  protected Log log = new Log(typeof(AbstractIgsProductSourceProvider<TIgsProductFile, TService>));

        /// <summary>
        /// 多系统数据源服务
        /// </summary>
       public AbstractIgsProductSourceProvider(IgsProductSourceOption opt, IgsProductType IgsProductSourceType, int TimeIntervalSeconds = 86400)
            :base(opt, IgsProductSourceType, TimeIntervalSeconds)
       {
            this.IsConnectIgsProduct = opt.IsConnectIgsProduct;
       } 
        #region 核心属性
        /// <summary>
        /// IGS产品缓存，避免重复读取，减少内存压力
        /// </summary>
        public static BaseConcurrentDictionary<string, TIgsProductFile> LoadedIgsProductFiles = new BaseConcurrentDictionary<string, TIgsProductFile>();
         

        #endregion

        #region abstract methods 

        #endregion

        #region 预制方法
        /// <summary>
        /// 按照不同系统类型建立按服务类字典。
        /// </summary>
        /// <returns></returns>
        protected Dictionary<SatelliteType, Dictionary<string, List<TIgsProductFile>>> BuildMultiSysServices()
        {
           var data = new Dictionary<SatelliteType, Dictionary<string, List<TIgsProductFile>>>();
            foreach (var satType in this.SatelliteTypes)//
            {
                var dailyServices = CreateSatTypeBasedServices(satType);

                if (dailyServices != null && dailyServices.Count > 0)
                {
                    data.Add(satType, dailyServices);
                }
            }
            return data;
        }
        /// <summary>
        /// 是否拼接IGS产品，如星历可以隔日计算。
        /// </summary>
        public bool IsConnectIgsProduct { get; set; }

        /// <summary>
        /// 建立按天组织的服务。一个系统类型一个服务。
        /// 如果失败，则网络获取一次。
        /// </summary>
        /// <param name="satType"></param>
        /// <returns></returns>
        protected Dictionary<string, List<TIgsProductFile>> CreateSatTypeBasedServices(SatelliteType satType)
        {
            Dictionary<string, List<TIgsProductFile>> dic = new Dictionary<string, List<TIgsProductFile>>();

            //按天生产星历服务
            for (var time = this.Option.TimePeriod.Start;  time <= this.Option.TimePeriod.End; time = time + TimeSpan.FromSeconds(TimeIntervalSeconds))
            {
                var files = GetFiles(satType, time);

                if (files != null && files.Count != 0)
                {
                    AddToDic(dic, files);
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
                        AddToDic(dic, files);
                    }
                    if (dic.Count > 0 && !Option.IsDownloadingSurplurseIgsProducts)
                    {
                        log.Info("已经具有 " + dic.Count + " 个服务，且设置为不下载多余产品，取消继续下载。");
                        return dic;
                    }
                }

                if (!IsConnectIgsProduct) { log.Info("计算选项设置为不拼接 IGS 产品，因此只采用起始历元的产品。"); break; }
            }
            return dic;
        } 
        /// <summary>
        /// 将产品分类存储
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="files"></param>
        private  void AddToDic(Dictionary<string, List<TIgsProductFile>> dic, List<TIgsProductFile> files)
        {
            foreach (var file in files)
            {
                if (!dic.ContainsKey(file.SourceCode)) { dic[file.SourceCode] = new List<TIgsProductFile>(); }
                dic[file.SourceCode].Add(file);
            }
        }

        /// <summary>
        /// 根据类型和历元，按照给定的文件命名规则，读取本地文件
        /// </summary>
        /// <param name="satType">卫星类型</param>
        /// <param name="gpsTime">时间</param>
        /// <returns></returns>
        protected virtual List<TIgsProductFile> GetFiles(SatelliteType satType, Time gpsTime)
        {
            List<string> pathes = GetIgsProductLocalPathes(satType, gpsTime);

            List<TIgsProductFile> services = new List<TIgsProductFile>(); 
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

                    string fileBufferKey = BuildFileBufferKey(filePath);

                    TIgsProductFile service = default(TIgsProductFile);
                    if (LoadedIgsProductFiles.Contains(fileBufferKey))
                    {
                        log.Info("缓存已经加载同名文件，直接从缓存返回： " + filePath);
                        service = LoadedIgsProductFiles[fileBufferKey];
                    }
                    else
                    {
                        service = LoadFile(filePath);
                        if (service == null || service.TimePeriod.Span == 0)
                        {
                            log.Info("数据源" + service + "不可用 " + this.IgsProductSourceType + "," + filePath);
                            continue;
                        }
                        log.Info("成功载入 " + this.IgsProductSourceType + "," + filePath);


                        if (LoadedIgsProductFiles.Count > Option.MaxIgsProductCacheCount)
                        {
                            LoadedIgsProductFiles.RemoveFirst();
                            //LoadedIgsProductFiles.Clear();
                            //log.Info("IGS 缓存产品超过最大数量 " + Option.MaxIgsProductCacheCount + ", 清空缓存重新来过。");
                        }

                        LoadedIgsProductFiles[fileBufferKey] = service;
                    }


                    //包含则添加
                    services.Add(service);

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
        /// 构建文件缓存关键字
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        protected virtual string BuildFileBufferKey(string filePath)
        {
            return Path.GetFileName(filePath).ToLower();
        }

        /// <summary>
        /// 在本地读取IGS产品
        /// </summary>
        /// <param name="localPath"></param>
        /// <returns></returns>
        protected abstract TIgsProductFile LoadFile(string localPath);

        #endregion
    }
}