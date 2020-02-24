//2017.09.25, czs, create in hongiqng, 基于整体缓存的周跳探测方法

using System;
using Gnsser.Domain;
using System.Collections;
using System.Collections.Generic;
using Geo;
using Geo.Times;
using Gnsser.Core;
using System.Text;

namespace Gnsser
{
     

    /// <summary>
    /// 基于整体缓存的周跳探测方法，周跳探测结果通用接口。CycleSlip 缩写为 CS。
    /// </summary>
    public class BufferedCycleSlipDetector : EpochInfoReviser, IBufferedCycleSlipDetector
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>  
        /// <param name="Option"></param>
        public BufferedCycleSlipDetector(GnssProcessOption Option)
        {
            this.SatObsDataType = Option.ObsDataType;
            this.MaxBreakCount = Option.MaxBreakingEpochCount;
            this.Interval =30;
            this.MaxErrorTimes = Option.MaxErrorTimesOfBufferCs;
            this.PolyFitOrder = Option.PolyFitOrderOfBufferCs;
            this.MinWindowSize = Option.MinWindowSizeOfCs;
            this.DifferTime = Option.DifferTimesOfBufferCs;
            this.IgnoreCsed = Option.IgnoreCsedOfBufferCs;
            DataManager = new WindowData<EpochInformation>(Option.BufferSize - 1);
            this.IsFirstWindow = true;
            this.Name = "缓存事后周跳探测";
        }
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="SatObsDataType"></param>
        /// <param name="maxBreakCount"></param>
        public BufferedCycleSlipDetector(SatObsDataType SatObsDataType, double maxBreakCount = 4)
        {
            this.SatObsDataType = SatObsDataType;
            this.MaxBreakCount = maxBreakCount;
            this.Interval = 30;
            this.MaxErrorTimes = 3;
            this.PolyFitOrder = 2;
            this.MinWindowSize = 5;
            this.DifferTime = 1;
            this.IgnoreCsed = true;
            this.Name = "缓存事后周跳探测";
            DataManager = new WindowData<EpochInformation>(10);
            this.IsFirstWindow = true;
        }

        #region 属性
        /// <summary>
        /// 数据流是否是第一个窗口。第一个窗口将标记首历元周跳，否则，不标记。
        /// </summary>
        public bool IsFirstWindow { get; set; }

        /// <summary>
        /// 是否忽略已经标记为周跳的历元卫星。
        /// </summary>
        public bool IgnoreCsed { get; set; }
        /// <summary>
        /// 差分次数
        /// </summary>
        public int DifferTime { get; set; }
        /// <summary>
        /// 最小窗口大小，小于此，都认为有周跳。
        /// </summary>
        public int MinWindowSize { get; set; }
        /// <summary>
        /// 观测数据类型
        /// </summary>
        public SatObsDataType SatObsDataType { get; set; }
        /// <summary>
        /// 最大断裂次数。
        /// </summary>
        public double MaxBreakCount { get; set; }
        /// <summary>
        /// 最大误差次数
        /// </summary>
        public double MaxErrorTimes { get; set; }
        /// <summary>
        /// 采样间隔
        /// </summary>
        public double Interval { get; set; }
        /// <summary>
        /// 拟合阶次
        /// </summary>
        public int PolyFitOrder { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 结果存储器
        /// </summary>
        public InstantValueStorage InstantValueStorage { get; set; }
        #endregion

        WindowData<EpochInformation> DataManager { get; set; }

        public override bool Revise(ref EpochInformation obj)
        {
            DataManager.Add(obj);
            if (DataManager.IsFull)
            {
                Detect(DataManager);

                //移除前1/3
                DataManager.RemoveAt(0, DataManager.Count / 3);
            }
            return true;
        }

        /// <summary>
        /// 执行探测,直接标记。
        /// </summary>
        public void Detect(IBuffer<EpochInformation> epochInfos)
        {
            Dictionary<SatelliteNumber, Dictionary<Time, EpochSatellite>> satData = BuildSatsData(epochInfos, IgnoreCsed);
            var WindowDataManager = BuildWindowData(satData);

            var interval = Math.Abs(epochInfos.Second.ReceiverTime - epochInfos.First.ReceiverTime);
            this.Interval = interval != 0 ? interval : 30;
            var maxTimeSpan = Interval * MaxBreakCount;

            Dictionary<SatelliteNumber, List<Time>> grossDataKeys = DetectGrossError(WindowDataManager, maxTimeSpan);

            //mark
            MarkSatUnstable(satData, grossDataKeys);

            BuildInstantValueStorage(grossDataKeys);
                if (IsFirstWindow) { IsFirstWindow = false; }
        }
        #region 内部方法
        /// <summary>
        /// 保存结果
        /// </summary>
        /// <param name="grossDataKeys"></param>
        private void BuildInstantValueStorage(Dictionary<SatelliteNumber, List<Time>> grossDataKeys)
        {
            InstantValueStorage = new InstantValueStorage();
            foreach (var kv in grossDataKeys)
            {
                InstantValueStorage.Regist(kv.Key.ToString(), kv.Value);
            }
        }

        /// <summary>
        /// 探测粗差或周跳
        /// </summary>
        /// <param name="WindowDataManager"></param>
        /// <param name="maxTimeSpan"></param>
        /// <returns></returns>
        private Dictionary<SatelliteNumber, List<Time>> DetectGrossError(TimeNumeralWindowDataManager<SatelliteNumber> WindowDataManager, double maxTimeSpan)
        {
            var grossDataKeys = new Dictionary<SatelliteNumber, List<Time>>();
            foreach (var kv in WindowDataManager.Data)
            {
                var prn = kv.Key;
                var window = kv.Value;
                //if (prn == SatelliteNumber.Parse("G29"))
                //{
                //    int i=0;
                //}
                List<Time> grossTimes = window.SplitAndGetKeysOfGrossError(m => m.SecondsOfWeek, maxTimeSpan, PolyFitOrder, MaxErrorTimes, MinWindowSize, DifferTime,IsFirstWindow);
               
                grossDataKeys[prn] = grossTimes;
            }
            return grossDataKeys;
        } 
        /// <summary>
        /// 将探测出的标记到卫星上。
        /// </summary>
        /// <param name="satData"></param>
        /// <param name="grossDataKeys"></param>
        private static void MarkSatUnstable(Dictionary<SatelliteNumber, Dictionary<Time, EpochSatellite>> satData, Dictionary<SatelliteNumber, List<Time>> grossDataKeys)
        {
            foreach (var kv in grossDataKeys)
            {
                var satDic = satData[kv.Key];
                foreach (var item in kv.Value)
                {
                    if (satDic.ContainsKey(item)) { satDic[item].IsUnstable = true; }
                }
            }
        }

        /// <summary>
        /// 构建窗口数据。
        /// </summary>
        /// <param name="satData"></param>
        /// <returns></returns>
        private TimeNumeralWindowDataManager<SatelliteNumber> BuildWindowData(Dictionary<SatelliteNumber, Dictionary<Time, EpochSatellite>> satData)
        {
            var WindowDataManager = new TimeNumeralWindowDataManager<SatelliteNumber>(Int16.MaxValue);
            foreach (var satDic in satData)
            {
                var window = WindowDataManager.GetOrCreate(satDic.Key);
                foreach (var sat in satDic.Value)
                {
                    var val = sat.Value[SatObsDataType].CorrectedValue;
                    window.Add(sat.Key, val);
                }
            }
            return WindowDataManager;
        }

        /// <summary>
        /// 构建卫星数据字典。
        /// </summary>
        /// <param name="epochInfos"></param>
        /// <returns></returns>
        private static Dictionary<SatelliteNumber, Dictionary<Time, EpochSatellite>> BuildSatsData(IBuffer<EpochInformation> epochInfos, bool ignoreCsed = true)
        {
            var satData = new Dictionary<SatelliteNumber, Dictionary<Time, EpochSatellite>>();
            foreach (var epochInfo in epochInfos)
            {
                foreach (var sat in epochInfo)
                {
                    if (sat.IsUnstable && ignoreCsed) { continue; }

                    if (!satData.ContainsKey(sat.Prn)) { satData[sat.Prn] = new Dictionary<Time, EpochSatellite>(); }
                    satData[sat.Prn][sat.ReceiverTime] = sat; 
                }
            }
            return satData;
        }
        #endregion

    }
}