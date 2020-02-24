//2018.12.28, czs, create in ryd, 时段观测数据服务

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Geo;
using Geo.Times;  
using System.Linq; 
using System.Threading.Tasks;
using Gnsser.Domain;

namespace Gnsser.Service
{
    /// <summary>
    /// 测站时段观测数据服务
    /// </summary>
    public class SiteSatPeriodDataService : BaseDictionary<string, SatParamPeriodData>
    {
        /// <summary>
        /// 测站卫星时段服务
        /// </summary>
        /// <param name="MaxGapSecond"></param>
        public SiteSatPeriodDataService(double MaxGapSecond)
        {

            this.MaxGapSecond = MaxGapSecond;
        }
        /// <summary>
        /// 允许的最大断裂跨度
        /// </summary>
        public double MaxGapSecond { get; set; } 

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override SatParamPeriodData Create(string key)
        {
            return new SatParamPeriodData(MaxGapSecond);
        }
        /// <summary>
        /// 设置断裂时间单位秒
        /// </summary>
        /// <param name="MaxGapSecond"></param>
        public void SetMaxGapSecond(double MaxGapSecond)
        {
            this.MaxGapSecond = MaxGapSecond;
            foreach (var item in this)
            {
                item.MaxGapSecond = MaxGapSecond;
            }
        }

        /// <summary>
        /// 获取 MW 单差值
        /// </summary> 
        /// <param name="epochInfo"></param>
        /// <param name="basePrn"></param> 
        /// <returns></returns>
        public NameRmsedNumeralVector<SatelliteNumber> GetMwCycleDifferValueBeweenSat(EpochInformation epochInfo, SatelliteNumber basePrn)
        {
            //计算所有测站星间单差
            var time = epochInfo.ReceiverTime; 
            var site = this.Get(epochInfo.SiteName); 
            var result = site.GetDifferMwCycle(time, basePrn); 
            return result;
        }

        /// <summary>
        /// 获取MW双差值，名称与双差值匹配。
        /// </summary> 
        /// <param name="sites"></param>
        /// <param name="basePrn"></param>
        /// <param name="isNetSolver">是否为网解</param>
        /// <returns></returns>
        public NameRmsedNumeralVector GetMwCycleDoubleDifferValue(MultiSiteEpochInfo sites, SatelliteNumber basePrn, bool isNetSolver = true)
        { 
            //计算所有测站星间单差
            var satDiffersOfSites = new Dictionary<string, NameRmsedNumeralVector<SatelliteNumber>>();
            var time = sites.ReceiverTime;
            foreach (var rovSiteObj in sites)
            {
                var rovSite = this.Get(rovSiteObj.SiteName);
                satDiffersOfSites[rovSiteObj.SiteName] = rovSite.GetDifferMwCycle(time, basePrn);
            }

            //计算站间单差，即双差
            NameRmsedNumeralVector result = new NameRmsedNumeralVector();
            var refSite = satDiffersOfSites[sites.BaseSiteName];
            foreach (var kv in satDiffersOfSites)
            {
                if(kv.Key == sites.BaseSiteName) { continue; } 

                var differ = kv.Value - refSite;

                foreach (var diffItem in differ)
                {
                    NetDoubleDifferName differName = new NetDoubleDifferName()
                    {
                        RefPrn = basePrn,
                        RovPrn = diffItem.Key
                    };
                    if (isNetSolver)//网解带坐标
                    {
                        differName.RefName = sites.BaseSiteName;
                        differName.RovName = kv.Key;
                    }
                    var name = differName.ToString(Gnsser.ParamNames.DoubleDifferAmbiguitySuffix);
                    result[name] = diffItem.Value;
                }
            } 
            return result;
        }
        /// <summary>
        /// 获取MW双差值
        /// </summary>
        /// <param name="time"></param>
        /// <param name="rovSiteName"></param>
        /// <param name="refSiteName"></param>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public NameRmsedNumeralVector<SatelliteNumber> GetMwCycleDoubleDifferValue( Time time, string rovSiteName, string refSiteName, SatelliteNumber basePrn)
        {
            var rovSite = this.Get(rovSiteName);
            var refSite = this.Get(refSiteName);
            var rovMw = rovSite.GetDifferMwCycle(time, basePrn);
            var refMw = refSite.GetDifferMwCycle(time, basePrn);

            var differ = rovMw - refMw; 
            return differ;
        }

        /// <summary>
        /// 构建输出表格，主要用于测试。
        /// </summary>
        /// <param name="OutputDirectory"></param>
        /// <returns></returns>
        public ObjectTableManager BuildTableObjectManager(string OutputDirectory)
        {
            ObjectTableManager tables = new ObjectTableManager(OutputDirectory);
            foreach (var site in this.KeyValues)
            {
                foreach (var sat in site.Value.KeyValues)
                {
                    var satParam = sat.Value;

                    foreach (var para in sat.Value.KeyValues)
                    {
                        var paramName = para.Key;
                       var table = tables.GetOrCreate(site.Key + "_" + sat.Key + "_" + para.Key);

                        var windDic =  satParam.GetAllWindowDataDic(paramName);
                         
                        int index = 0;
                        int groupIndex = 0;
                        foreach (var window in windDic)
                        {
                            groupIndex++;
                            foreach (var item in window.Value)
                            {
                                index++;

                                table.NewRow();
                                table.AddItem("Index", index);
                                table.AddItem("PeriodIndex", groupIndex);
                                table.AddItem("Period", window.Key);
                                table.AddItem("Value", item);
                            } 
                        }
                    }
                }

            }
            return tables;
        }

    }


    /// <summary>
    /// 时段观测数据服务
    /// </summary>
    public class SatParamPeriodData : BaseDictionary<SatelliteNumber, ParamPeriodData>
    {
        public SatParamPeriodData(double MaxGapSecond)
        {
            this.MaxGapSecond = MaxGapSecond;
        }
        /// <summary>
        /// 最后注册时间
        /// </summary>
        public Time LastRegistTime { get; set; }
        /// <summary>
        /// 允许的最大断裂跨度
        /// </summary>
        public double MaxGapSecond { get; set; }

        /// <summary>
        /// 注册时段
        /// </summary>
        /// <param name="obj"></param>
        public void Regist(EpochInformation obj)
        {
            foreach (var sat in obj)
            {
              var paramWinData = this.GetOrCreate(sat.Prn);

                paramWinData.Regist(ParamNames.MwCycle, obj.ReceiverTime, sat.MwCycle, sat.IsUnstable);
            }
        }

        /// <summary>
        /// 获取时段MW平均值
        /// </summary>
        /// <param name="time"></param>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public NameRmsedNumeralVector<SatelliteNumber> GetDifferMwCycle(Time time, SatelliteNumber basePrn)
        {
            var result = new NameRmsedNumeralVector<SatelliteNumber>();
            var data = GetAverageMwCycle(time);
            return data.Minus(basePrn); 
        }

        /// <summary>
        /// 获取时段MW平均值
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public NameRmsedNumeralVector<SatelliteNumber> GetAverageMwCycle(Time time)
        { 
           var result = new NameRmsedNumeralVector<SatelliteNumber>();
            foreach (var item in this.KeyValues)
            {
                var prn = item.Key;
                var val = GetAverageMwCycle(prn, time);
                if (val == null) { continue; }

                result[prn] = val;
            } 
            return result;
        }
        /// <summary>
        /// 获取时段MW平均值
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public RmsedNumeral GetAverageMwCycle(SatelliteNumber prn, Time time)
        {
            var window = GetWindowData(prn, ParamNames.MwCycle, time);
            if (window == null || window.Count == 0) { return null; }
            return window.Average;
        }

        /// <summary>
        /// 窗口数据
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="paramName"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public TimeNumeralWindowData GetWindowData(SatelliteNumber prn, string paramName, Time time)
        {
            var satData = Get(prn);
            if(satData == null) { return null; }
            
            var windData = satData.GetWindowData(paramName, time); 
            return windData;
        }
        public override ParamPeriodData Create(SatelliteNumber key)
        {
            return new ParamPeriodData(MaxGapSecond);
        }
    }

    /// <summary>
    /// 时段数据
    /// </summary> 
    public class ParamPeriodData : SuccessiveTimePeriodService<String>
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="MaxGapSecond"></param>
        public ParamPeriodData( double MaxGapSecond) : base(MaxGapSecond)
        {
            MaxDiffer = 3;
        }
        /// <summary>
        /// 允许最大偏差
        /// </summary>
        public double MaxDiffer { get; set; }

        /// <summary>
        /// 注册数据
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="time"></param>
        /// <param name="val"></param>
        /// <param name="isBreakTime">是否重新记录，如发生了周跳</param>
        public void Regist( string paramName, Time time, double val, bool isBreakTime)
        {
            TimePeriod period = null;
            TimeNumeralWindowData winData = null;
            var last = this.GetLastTimePeriod(paramName);
            if(last != null)
            {
                winData = GetWindowData(last);

                if (winData.Count > 0)
                {
                    var differ = Math.Abs(winData.AverageValue - val);
                    if (differ > MaxDiffer)
                    {
                        //重新建立分区
                        period = base.Regist(paramName, time, true);
                        winData = GetWindowData(period);
                    }
                }
            }

            if(period ==null)//新建
            {
                period = base.Regist(paramName, time, isBreakTime);
                winData = GetWindowData(period);
            }

            //最后都要添加
            winData.Add(time, val);
        }

        /// <summary>
        /// 窗口数据
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public TimeNumeralWindowData GetWindowData(string paramName, Time time)
        {
            var period = GetTimePeriod(paramName, time);
            if(period == null) { return null; }

            return GetWindowData(period); 
        }

        /// <summary>
        /// 提取绑定的窗口数据
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public TimeNumeralWindowData GetWindowData(TimePeriod period)
        {
            TimeNumeralWindowData data = null;
            if (period.Tag == null)
            {
                data = new TimeNumeralWindowData(100000, MaxGapSecond * 2);
                period.Tag = data;
            }
            else
            {
                data = period.Tag as TimeNumeralWindowData;
            }
            return data;
        }
        public List<TimeNumeralWindowData> GetAllWindowData(string key)
        {
            var vals = Get(key);

            List<TimeNumeralWindowData> result = new List<TimeNumeralWindowData>();
            foreach (var item in vals)
            {
                var data = item.Tag as TimeNumeralWindowData;
                if (data == null) { continue; }

                result.Add(data);
            }
            return result;
        }
        public Dictionary<TimePeriod,TimeNumeralWindowData> GetAllWindowDataDic(string key)
        {
            var vals = Get(key);

            var result = new Dictionary<TimePeriod, TimeNumeralWindowData>();
            foreach (var item in vals)
            {
                var data = item.Tag as TimeNumeralWindowData;
                if (data == null) { continue; }

                result[item] =(data);
            }
            return result;
        }
        public TimePeriod GetTimePeriod(string key, TimeNumeralWindowData manager)
        {
            var vals = Get(key);
            foreach (var item in vals)
            {
                var data = item.Tag as TimeNumeralWindowData;
                if (data == null) { continue; }
                if(data == manager) { return item; } 
            }
            return null;

        }
    }

}
