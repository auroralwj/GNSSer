//2018.08.31, czs, create in HMX, GPS 窄巷服务器
//2018.09.12, czs, edit in hmx, 宽巷窄巷格式定义

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Times;
using Geo;
using Geo.IO;
using System.Collections;

namespace Gnsser.Service
{
    /// <summary>
    /// 宽巷窄巷的小数部分，相位未校准延迟的小数部分(Fraction Code Bias of Uncalibrated phase delay)
    /// </summary>
    public class FcbOfUpd: IOrderedProperty
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="basePrn"></param>
        /// <param name="epoch"></param>
        /// <param name="isWideOrNarrow"></param>
        public FcbOfUpd(SatelliteNumber basePrn, Time epoch, bool isWideOrNarrow)
        {
            Data = new BaseDictionary<SatelliteNumber, RmsedNumeral>();
            this.Count = 32;// data.Count;
            this.BasePrn = basePrn;
            this.Epoch = epoch;
            this.WnMarker = isWideOrNarrow ? FcbOfUpd.WideLaneMaker : FcbOfUpd.NarrowLaneMaker;
        }
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FcbOfUpd()
        {
            Data = new BaseDictionary<SatelliteNumber, RmsedNumeral>();
            this.Count = 32;// data.Count;
        }
        /// <summary>
        /// 字典初始化
        /// </summary>
        /// <param name="basePrn"></param>
        /// <param name="epoch"></param>
        /// <param name="dic"></param>
        /// <param name="isWideOrNarrow"></param>
        public FcbOfUpd(SatelliteNumber basePrn, Time epoch, Dictionary<SatelliteNumber, RmsedNumeral> dic, bool isWideOrNarrow = true) : this()
        {
            this.BasePrn = basePrn;
            this.WnMarker = isWideOrNarrow? FcbOfUpd.WideLaneMaker: FcbOfUpd.NarrowLaneMaker;
            this.Epoch = epoch;
             
            foreach (var item in dic)
            {
                if (this.Data.Count >= Count) { break; }
                this.Add(item.Key, item.Value);
            }
        }

        /// <summary>
        /// 以IGS宽巷初始化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="basePrn"></param>
        public FcbOfUpd(SatelliteNumber basePrn, WideLaneBiasItem data):this()
        { 
            this.BasePrn = basePrn;
            this.WnMarker = FcbOfUpd.WideLaneMaker;
            this.Epoch = data.Time;

            var dic = data.GetMwDiffer(basePrn);
            foreach (var item in dic)
            { 
                if(this.Data.Count >= Count) { break; }
                this.Add(item.Key, new RmsedNumeral( item.Value, 0.0001));
            }
        }

        public const string WideLaneMaker = "W";
        public const string NarrowLaneMaker = "N";
        /// <summary>
        /// 若失败，则返回此。
        /// </summary>
        public static RmsedNumeral failedValue = RmsedNumeral.NaN;

        public bool IsWideOrNarrowLane { get => WnMarker == WideLaneMaker; }
        /// <summary>
        /// 历元
        /// </summary>
        public Time Epoch { get; set; }
        /// <summary>
        /// 基准星
        /// </summary>
        public SatelliteNumber BasePrn { get; set; }
        /// <summary>
        /// 标识，W N 宽巷还是窄巷
        /// </summary>
        public string WnMarker { get; set; }
        /// <summary>
        /// 卫星或产品数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 以逗号分隔的卫星序列
        /// </summary>
        public string PrnsString { get; set; }
        /// <summary>
        /// 解析自 PrnsString
        /// </summary>
        public List<SatelliteNumber> Prns { get => Data.Keys; }
        /// <summary>
        /// 获取，失败则以 FailedValue 表示。
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public RmsedNumeral Get(SatelliteNumber prn)
        {
            if (!Data.Contains(prn))
            {
                return FailedValue;
            }
            return Data[prn];
        }
        /// <summary>
        /// 数据数量。
        /// </summary>
        public int DataCount { get => Data.Count; }

        /// <summary>
        /// 数据
        /// </summary>
        public BaseDictionary<SatelliteNumber, RmsedNumeral> Data { get; set; }
        /// <summary>
        /// 获取当前的BSD值
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public RmsedNumeral GetBsdValue(SatelliteNumber prn, SatelliteNumber basePrn)
        {
            var result = Data[prn] - Data[basePrn];
            result = Geo.Utils.DoubleUtil.GetRoundFraction(result);
            return result;
        }


        public List<string> OrderedProperties => orderedProperties ;
        public static List<string> orderedProperties = FcbOfUpdFile.BuildTitles(); 


        public void Add(SatelliteNumber prn, RmsedNumeral value)
        {
            Data[prn] = value;
        }
        /// <summary>
        /// 转换到另一个基准卫星。
        /// </summary>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public FcbOfUpd ConvertTo(SatelliteNumber basePrn)
        {
            if (basePrn == this.BasePrn) { return this; }

            FcbOfUpd other = new FcbOfUpd()
            {
                BasePrn = basePrn,
                Count = FcbOfUpdFile.TotalGpsSatCount,
                Epoch = this.Epoch,
                WnMarker = this.WnMarker
            };
            var oldBaseVal = this.Get(basePrn);
            if (!RmsedNumeral.IsValid(oldBaseVal))
            {
                return null;
            }

            foreach (var item in this.Data.KeyValues)
            {
                var prn = item.Key;
                RmsedNumeral newVal = null;
                if (prn == basePrn)
                {
                    newVal = RmsedNumeral.Zero;
                }
                else
                {
                    newVal = this.Get(prn) - oldBaseVal;
                    newVal = Geo.Utils.DoubleUtil.GetRoundFraction(newVal);
                }
                //只赋值有效数据
                if (RmsedNumeral.IsValid(newVal))
                {
                    other.Data[prn] = newVal;
                }
            }
            return other;
        }

        public List<ValueProperty> Properties => throw new NotImplementedException();

        public static RmsedNumeral FailedValue => failedValue;

        public BaseDictionary<SatelliteNumber, RmsedNumeral> Get(Time time)
        { 
            return Data;
        }
        /// <summary>
        /// 如果获取失败，则返回 FailedValue 。
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public RmsedNumeral Get(SatelliteNumber prn, Time time)
        {
            if (Data.Contains(prn))
            {
                return Data[prn];
            }

            return FailedValue;
        }

    }
}
