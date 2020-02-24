//2018.08.31, czs, create in hmx, MW 平滑器，为了PPP模糊度固定
//2018.11.06，czs, create in hmx, WM 平滑管理器，为了无电离层双差网解

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using Geo.Algorithm;
using Geo;
using Geo.Utils;
using Geo.Common;
using Gnsser.Domain;
using Geo.Algorithm.Adjust;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Gnsser.Service;
using Gnsser.Checkers;
using Geo.Times;
using Gnsser.Filter;
using System.IO;
using Gnsser.Data;


namespace Gnsser
{
    /// <summary>
    /// WM 平滑管理器，为了无电离层双差网解
    /// </summary>
    public class SatValueStorageManager : BaseDictionary<string, SatValueStorage>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="epochCount"></param>
        /// <param name="breakSpanSeconds"></param>
        public SatValueStorageManager(int epochCount = 120, double breakSpanSeconds = 121)
            :base("MW平滑管理器", new Func<string, SatValueStorage>(str=>new SatValueStorage(epochCount, breakSpanSeconds)))
        {
        }

        public void Add(MultiSiteEpochInfo mEpochInfo)
        {
            foreach (var item in mEpochInfo)
            {
                this.GetOrCreate(item.SiteName).Add(item);
            }
        }         
    }

    /// <summary>
    /// MW 平滑数据生成器，为了PPP模糊度固定
    /// </summary>
    public class SatValueStorage
    {
    //    EpochSatSiteValueList
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="epochCount"></param>
        /// <param name="breakSpanSeconds"></param>
        public SatValueStorage(int epochCount = 120, double breakSpanSeconds = 121)
        {
            MwDataSmoothManager = new TimeNumeralWindowDataManager<SatelliteNumber>(epochCount, breakSpanSeconds);
        }
        /// <summary>
        /// 平滑数据管理器
        /// </summary>
        TimeNumeralWindowDataManager<SatelliteNumber> MwDataSmoothManager { get; set; }
        /// <summary>
        /// 平滑后的MW值
        /// </summary>
        public Dictionary<SatelliteNumber, RmsedNumeral> SmoothedMwValue { get; set; }
        /// <summary>
        /// 添加一个历元，并返回当前历元的平滑结果。
        /// </summary>
        /// <param name="epochInfo"></param>
        /// <returns></returns>
        public Dictionary<SatelliteNumber, RmsedNumeral> Add(EpochInformation epochInfo)
        {
            //平滑MW
            var time = epochInfo.ReceiverTime;
            var Smooth_MWs = new Dictionary<SatelliteNumber, RmsedNumeral>();
            foreach (var sat in epochInfo)
            {
                var wmWindow = MwDataSmoothManager.GetOrCreate(sat.Prn);

                if (sat.IsUnstable)//周跳
                {
                    wmWindow.Clear();
                }

                var mw = sat.Combinations.MwPhaseCombinationValue;
                wmWindow.Add(time, mw);
                //当前
                Smooth_MWs[sat.Prn] = wmWindow.Average;
            }
            SmoothedMwValue = Smooth_MWs;

            return Smooth_MWs;
        }

        /// <summary>
        /// 获取当前星间单差
        /// </summary>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public Dictionary<SatelliteNumber, RmsedNumeral> GetDifferMwValue(SatelliteNumber basePrn, double maxRmsValue = 0.6)
        {
            var differMws = new Dictionary<SatelliteNumber, RmsedNumeral>();
            var baseMw = SmoothedMwValue[basePrn];
            foreach (var item in SmoothedMwValue)
            {
                if (item.Key == basePrn) { continue; }

                var differValue = item.Value - baseMw;

                if(differValue .Rms > maxRmsValue) { continue; }

                differMws.Add(item.Key, differValue);
            }
            return differMws;
        }

    }
}
