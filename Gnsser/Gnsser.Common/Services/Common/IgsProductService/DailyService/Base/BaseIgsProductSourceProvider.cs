//2015.12.09, czs, create in 达州到成都列车D5181, IGS 产品数据源提供

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
using Gnsser.Data;
using Geo.Times;

namespace Gnsser.Data
{

    /// <summary>
    /// 基础的IGS产品提供器
    /// </summary>
    public abstract class BaseIgsProductSourceProvider<TService>
    {
        protected Log log = new Log(typeof(BaseIgsProductSourceProvider<TService>));


        /// <summary>
        /// 多系统数据源服务
        /// </summary>
        public BaseIgsProductSourceProvider(IgsProductSourceOption opt, IgsProductType IgsProductSourceType, int TimeIntervalSeconds = 86400)
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

        #region 核心属性
        /// <summary>
        /// IGS产品缓存，避免重复读取，减少内存压力
        /// </summary>
        //public static BaseConcurrentDictionary<string, TIgsProductFile> LoadedIgsProductFiles = new BaseConcurrentDictionary<string, TIgsProductFile>();


        #endregion
        #region 核心属性 
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

        /// <summary>
        /// 卫星类型
        /// </summary>
        public List<SatelliteType> SatelliteTypes { get { return Option.SatelliteTypes; } }
        #endregion

        /// <summary>
        /// 返回服务
        /// </summary>
        /// <returns></returns>
        public abstract TService GetDataSourceService();

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

    }
}