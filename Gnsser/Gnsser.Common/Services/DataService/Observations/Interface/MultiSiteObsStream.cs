//2016.04.20, czs & cuiyang & double, create in hongqing, 多观测数据源
//2016.04.23, czs, edit in hongqing, 遍历 MultiSiteEpochInformation
//2018.11.03, czs, edit in hmx, 整理代码
//2018.12.15, czs, edit in hmx, 增加时段信息
//2019.01.25, czs, edit in hmx, TimePeriod为公共最小时段

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates; 
using Gnsser.Service; 
using Gnsser.Data;
using Gnsser.Times;
using Gnsser.Data.Rinex;
using Geo.Times;
using Geo.IO;
using Geo;
using Gnsser.Domain;

namespace Gnsser
{

    /// <summary>
    /// 多观测数据源
    /// </summary>
    public class MultiSiteObsStream : AbstractEnumer<MultiSiteEpochInfo>, IObservationStream<MultiSiteEpochInfo>,  IService
    {
        Log log = new Log(typeof(MultiSiteObsStream));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pathes"></param>
        /// <param name="IsSameSatRequired"></param>
        /// <param name="BaseSiteSelectType"></param>
        /// <param name="IndicatedBaseSiteName"></param>
        public MultiSiteObsStream(IEnumerable<String> pathes, BaseSiteSelectType BaseSiteSelectType, bool IsSameSatRequired, string IndicatedBaseSiteName=null)
        {
            this.IsSameSatRequired = IsSameSatRequired;
            this.IndicatedBaseSiteName = IndicatedBaseSiteName;
            this.BaseSiteSelectType = BaseSiteSelectType;
            this.Pathes = new List<string>( pathes);
            this.DataSources = new BaseDictionary<string, ISingleSiteObsStream>();
            foreach (var item in Pathes)
            {
                var source = new RinexFileObsDataSource(item);
                this.DataSources.Add(source.Name, source);
            }
            Init();
        }

        /// <summary>
        /// 构造函数，只有一个站
        /// </summary>
        /// <param name="dataSource"></param>
        public MultiSiteObsStream(ISingleSiteObsStream dataSource)
        {
            this.BaseSiteSelectType = BaseSiteSelectType.First;
            this.IndicatedBaseSiteName = dataSource.SiteInfo.SiteName;
            this.DataSources = new BaseDictionary<string, ISingleSiteObsStream>();
            this.DataSources.Add(dataSource.Name, dataSource);
            Init();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            if (String.IsNullOrWhiteSpace(IndicatedBaseSiteName))
            {
                IndicatedBaseSiteName = DataSources.First.SiteInfo.SiteName;
            }
            List<string> otherNames = new List<string>();
            foreach (var item in this.OtherDataSources)
            {
                otherNames.Add(item.SiteInfo.SiteName);
            } 

            string name = ResultFileNameBuilder.BuildMultiSiteEpochInfoName(this.BaseDataSource.SiteInfo.SiteName, otherNames);
            this.Name = name;


            this.TimePeriod = GetSameTimePeriod();
            log.Info(this.Name + " " + this.DataSources.Count + "个数据源的公共时段为：" + TimePeriod);
        }

        #region 属性
        /// <summary>
        /// 产生了产品
        /// </summary>
        public event Action<MultiSiteEpochInfo> ProductProduced;
        /// <summary>
        /// 基准站选择方法
        /// </summary>
        BaseSiteSelectType BaseSiteSelectType { get; set; }
        /// <summary>
        /// 外部指定的基准站名称
        /// </summary>
        string IndicatedBaseSiteName { get; set; }
        /// <summary>
        /// 是否需要卫星共视
        /// </summary>
        public bool IsSameSatRequired { get; set; }
        /// <summary>
        ///是否允许 丢失某测站的某历元。
        /// </summary>
        public bool IsAllowMissingEpochSite { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public List<String> Pathes { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public BaseDictionary<string, ISingleSiteObsStream> DataSources { get; set; }
        /// <summary>
        /// 星历数据源
        /// </summary>
        public BaseDictionary<string, IEphemerisService> EphemerisServices { get; set; }

        private ISingleSiteObsStream baseDataSource{ get; set; }
        /// <summary>
        /// 产生产品
        /// </summary>
        /// <param name="obj"></param>
        protected void OnProductProduced(MultiSiteEpochInfo obj) { ProductProduced?.Invoke(obj); }
        /// <summary>
        /// 基准数据流。如果只有一个，则只需以此遍历。
        /// </summary>
        public ISingleSiteObsStream BaseDataSource { get
            {
                if (baseDataSource == null)
                {
                    baseDataSource = DataSources.First; 

                    if (DataSources.Count > 2)
                    {
                        List<ObsSiteInfo> list = new List<ObsSiteInfo>();
                        foreach (var item in DataSources)
                        {
                            ObsSiteInfo fileInfo = new ObsSiteInfo(item.Path);
                            list.Add(fileInfo);
                        }

                        BaseSiteSelection baseSiteSelection = new BaseSiteSelection( BaseSiteSelectType, this.IndicatedBaseSiteName);
                        var baseSite = baseSiteSelection.GetBaseSite(list);
                        foreach (var item in DataSources)
                        {
                            if (item.Path == baseSite.OriginalPath)
                            {
                                baseDataSource = item;
                                break;
                            }
                        }
                        log.Info("基准测站设置为邻近几何中心 " + baseSite.OriginalPath);
                    }
                }
                return baseDataSource;
            } set { baseDataSource = value; } }
        /// <summary>
        /// 其它数据源
        /// </summary>
        public ISingleSiteObsStream OtherDataSource { get { return OtherDataSources[0]; } }
        /// <summary>
        /// 其它的数据源
        /// </summary>
        public List<ISingleSiteObsStream> OtherDataSources { get { return DataSources.Values.FindAll(m=>!m.Equals(BaseDataSource)); } }
        /// <summary>
        /// 测站信息
        /// </summary>
        public ISiteInfo SiteInfo => BaseDataSource.SiteInfo;
        /// <summary>
        /// 观测信息
        /// </summary>
        public IObsInfo ObsInfo => BaseDataSource.ObsInfo;
        /// <summary>
        /// 当前产品
        /// </summary>
        public MultiSiteEpochInfo CurrentProduct { get; set; }
        /// <summary>
        /// 有效时段
        /// </summary>
        public TimePeriod TimePeriod { get; set; }
        #endregion
        /// <summary>
        /// 获取最大时段信息
        /// </summary>
        /// <returns></returns>
        public TimePeriod GetMaxCommonTimePeriod()
        {
            TimePeriod timePeriod = BaseDataSource.ObsInfo.TimePeriod;
            foreach (var item in DataSources)
            {
                if(item == BaseDataSource) { continue; }
                timePeriod.Exppand(item.ObsInfo.TimePeriod);
            }
            return timePeriod;
        }

        /// <summary>
        /// 计算所有观测的共有时段，忽略单独观测时段
        /// </summary>
        /// <returns></returns>
        public TimePeriod GetSameTimePeriod()
        {
            TimePeriod timePeriod = BaseDataSource.ObsInfo.TimePeriod;
            foreach (var item in DataSources)
            {
                if (item == BaseDataSource) { continue; }

                var Segment = timePeriod.GetIntersect(item.ObsInfo.TimePeriod);
                if (Segment == null)
                {
                    return TimePeriod.Zero;
                }
                timePeriod = new TimePeriod(Segment.Start, Segment.End);
            }
            return timePeriod;  
        }

        /// <summary>
        /// 尝试加载自带的星历文件。
        /// </summary>
        public void TryLoadEphemerisServices()
        {
            EphemerisServices = new BaseDictionary<string, IEphemerisService>();
            foreach (var item in DataSources)
            {
                if (System.IO.File.Exists(item.NavPath))
                {
                    var eph = EphemerisDataSourceFactory.Create(item.NavPath);
                    EphemerisServices[item.Name] = eph;
                } 
            }
        }
        /// <summary>
        /// 移动到下一个
        /// </summary>
        /// <returns></returns>
        public override bool MoveNext()
        {
            if (!base.MoveNext()) { return false; } //采用基类的流程控制。

            //主获取下一个
            var isBaseResultOk = BaseDataSource.MoveNext();
            //必须从公共时段开始读取
            var baseEpochInfo = BaseDataSource.Current;
            if (baseEpochInfo == null) { return false; }

           //时段控制
            while (isBaseResultOk)
            {
                if (!TimePeriod.Contains(baseEpochInfo.ReceiverTime))
                {
                    log.Debug(BaseDataSource.Name  + ", " + baseEpochInfo.ReceiverTime+ ", 在共同时段外，跳过！共同时段：" + TimePeriod);
                    isBaseResultOk = BaseDataSource.MoveNext();
                    //update
                    baseEpochInfo = BaseDataSource.Current;
                    if (baseEpochInfo == null) { return false; }
                }
                else { break; }
            }

            if (isBaseResultOk)
            {
                var builder = new MultiSiteEpochInfoBuilder(IsSameSatRequired); 
                builder.Add(baseEpochInfo, true);

                foreach (var obsDataSource in DataSources)
                {
                    if (BaseDataSource == obsDataSource)
                    {
                        continue;
                    }
                    var other = obsDataSource.Get(baseEpochInfo.ReceiverTime, 0.1);//最大分辨率 10HZ。
                    if (other == null)
                    {
                        //是否忽略
                        if (IsAllowMissingEpochSite)
                        {
                            log.Debug(obsDataSource.Name + ", 获取同步观测历元失败！" + baseEpochInfo.ReceiverTime + ", 本历元少此站。");
                            continue;
                        }

                        log.Warn(obsDataSource.Name + ", 获取同步观测历元失败！" + baseEpochInfo.ReceiverTime + ", 此历元无数据，继续尝试下一历元。");
                        return MoveNext();
                    }
                    builder.Add(other);
                }
                var result = builder.Build();

                this.Current = result;
                OnProductProduced(this.Current);
            }

            return isBaseResultOk;
        }



        /// <summary>
        /// 重置
        /// </summary>
        public override void Reset()
        {
            foreach (var item in DataSources)
            {
                item.Reset();
            } 
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            foreach (var item in DataSources)
            {
                item.Dispose();
            }
            DataSources.Clear();
        }
        public override string ToString()
        {
            return Name + "[" + DataSources.Count + "]";// base.ToString();
        }

        public List<MultiSiteEpochInfo> Gets(int startIndex = 0, int maxCount = int.MaxValue)
        {
            throw new NotImplementedException();
        }

        public List<MultiSiteEpochInfo> GetNexts(int count)
        {
            throw new NotImplementedException();
        }
    }
}