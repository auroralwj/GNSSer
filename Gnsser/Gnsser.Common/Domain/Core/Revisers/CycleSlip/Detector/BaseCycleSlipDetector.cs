//2014.09.10, czs, create, 周跳探测初步
//2017.08.13, czs, edit in hongqing, 面向对象重构
//2017.09.23, czs, edit in hongqing, 增加长缓存

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Geo;
using Geo.Times;
using Geo.IO;

namespace Gnsser
{
    /// <summary>
    /// 探测周跳基本类。
    /// </summary>
    public abstract class BaseCycleSlipDetector<T> : BaseDictionary<SatelliteNumber, T>, ICycleSlipDetector
    {
        /// <summary>
        /// 日志记录
        /// </summary>
        protected Log log = new Log(typeof(BaseCycleSlipDetector));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="IsUsingRecordedCycleSlipInfo"></param>
        public BaseCycleSlipDetector(bool IsUsingRecordedCycleSlipInfo, int bufferSize = 10)
        {
            this.BufferSize = bufferSize;
            CycleSlipStorage = new InstantValueStorage();
            this.SatValueManager = new TimeNumeralWindowDataManager<SatelliteNumber>(bufferSize);
            this.EpochSats = new BaseDictionary<SatelliteNumber, WindowData<EpochSatellite>>();
            this.IsUsingRecordedCsInfo = IsUsingRecordedCycleSlipInfo;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Option"></param>
        public BaseCycleSlipDetector(GnssProcessOption Option)
        {
            this.BufferSize = Option.BufferSize;
            CycleSlipStorage = new InstantValueStorage();
            this.SatValueManager = new TimeNumeralWindowDataManager<SatelliteNumber>(Option.BufferSize);
            this.EpochSats = new BaseDictionary<SatelliteNumber, WindowData<EpochSatellite>>();
            this.IsUsingRecordedCsInfo =Option.IsUsingRecordedCycleSlipInfo;
        }

        public int BufferSize { get; set; }
        /// <summary>
        /// 周跳探测结果
        /// </summary>
        public bool CurrentResult { get; set; }
        /// <summary>
        /// 数据查看器
        /// </summary>
        public ObjectTableManager TableObjectManager { get; set; }
        /// <summary>
        /// 是否保存结果到表中。
        /// </summary>
        public bool IsSaveResultToTable { get; set; }
        /// <summary>
        /// 当前卫星
        /// </summary>
        public EpochSatellite EpochSat { get; set; } 

        /// <summary>
        /// 历元卫星缓存。
        /// </summary>
        public TimeNumeralWindowDataManager<SatelliteNumber> SatValueManager { get; set; }
        /// <summary>
        /// 缓存
        /// </summary>
        public BaseDictionary<SatelliteNumber, WindowData<EpochSatellite>> EpochSats { get; set; }
        /// <summary>
        /// 当前卫星观测值，实时更新
        /// </summary>
        public WindowData<EpochSatellite> CurentEpochSats { get; set; }
        /// <summary>
        /// 是否使用已经记录的周跳信息，否则只从数据本身探测。
        /// </summary>
        public bool IsUsingRecordedCsInfo { get; set; }

        /// <summary>
        /// 周跳探测类型。
        /// </summary>
        public abstract CycleSlipDetectorType DetectorType { get; }
        /// <summary>
        /// 周跳探测结果存储器
        /// </summary>
        public InstantValueStorage CycleSlipStorage { get; set; }
        /// 探测
        /// </summary>
        public abstract bool Detect(EpochSatellite epochSat);
        /// <summary>
        /// 设置站星信息。
        /// </summary>
        /// <param name="epochSat"></param>
        protected virtual void SetEpochSatellite(EpochSatellite epochSat)
        {
            this.EpochSat = epochSat;
        }

        /// <summary>
        /// 注册当前为有周跳。
        /// </summary>
        protected void RegistCurrentCsed()
        {
            CycleSlipStorage.Regist(EpochSat.Prn.ToString(), EpochSat.Time.Value);
        }
        /// <summary>
        /// 获取输出表。
        /// </summary>
        /// <returns></returns>
        protected ObjectTableStorage GetOutTable()
        {
            var tableName = EpochSat.Prn + "_" + this.DetectorType;
            var table = TableObjectManager.GetOrCreate(tableName);
            return table;
        }
    }

    /// <summary>
    /// 探测周跳基本类。
    /// </summary>
    public abstract class BaseCycleSlipDetector :  BaseCycleSlipDetector<bool>{
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="IsUsingRecordedCycleSlipInfo"></param>
        public BaseCycleSlipDetector(bool IsUsingRecordedCycleSlipInfo)
            : base(IsUsingRecordedCycleSlipInfo)
        { 
        }
    }
}