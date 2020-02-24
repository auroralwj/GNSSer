//2018.03.16, czs, create in hmx, 单颗卫星的星历列表
//2018.03.16, czs, edit in hmx, 数据源标识，如igs,igr,com等，即使同一时刻同一颗卫星，不同的数据源也是不同的。
//2018.05.09, czs, edit in hmx, 增加最简钟差表达

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;//
using Gnsser.Times;
using Gnsser.Service;
using Geo.Times;
using Geo;
using Gnsser.Data.Rinex;

namespace Gnsser.Data
{
    /// <summary>
    /// 组合多文件简易钟差
    /// </summary>
    public class SatSimpleClockCollection : SatClockCollection<SimpleClockBias, SimpleClockFile>
    {        /// <summary>
             /// 默认构造函数
             /// </summary>
             /// <param name="isUniqueSource">是否唯一数据源，高精度的需求</param>
             /// <param name="SourceCode">数据源代码，默认ig</param>
        public SatSimpleClockCollection(bool isUniqueSource = true, string SourceCode = "ig") : base(isUniqueSource, SourceCode)
        {
            this.Name = "组合多文件简易钟差";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data">按照类型读入的原始数据结构</param>
        /// <param name="isUniqueSource"></param>
        /// <param name="indicatedSource"></param>
        public SatSimpleClockCollection(Dictionary<SatelliteType, Dictionary<string, List<SimpleClockFile>>> data, bool isUniqueSource = true, string indicatedSource = "ig")
            : base(data, isUniqueSource, indicatedSource)
        {
        }
         
    }
    /// <summary>
    /// 组合多文件钟差
    /// </summary>
    public class SatClockCollection : SatClockCollection<AtomicClock, ClockFile>
    {        /// <summary>
             /// 默认构造函数
             /// </summary>
             /// <param name="isUniqueSource">是否唯一数据源，高精度的需求</param>
             /// <param name="SourceCode">数据源代码，默认ig</param>
        public SatClockCollection(bool isUniqueSource = true, string SourceCode = "ig") : base(isUniqueSource, SourceCode)
        {
            this.Name = "组合多文件钟差";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data">按照类型读入的原始数据结构</param>
        /// <param name="isUniqueSource"></param>
        /// <param name="indicatedSource"></param>
        public SatClockCollection(Dictionary<SatelliteType, Dictionary<string, List<ClockFile>>> data, bool isUniqueSource = true, string indicatedSource = "ig")
            : base(data, isUniqueSource, indicatedSource)
        {
        }

         
    }
    /// <summary>
    /// 单颗卫星的星历列表
    /// </summary>
    public   class SatClockCollection<TAtomicClock, TClockFile> : TimedSatObjectCollection<TAtomicClock, TimedSatObject<TAtomicClock>>
        where TAtomicClock : ISimpleClockBias
        where TClockFile : ClockFile<TAtomicClock>

    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="isUniqueSource">是否唯一数据源，高精度的需求</param>
        /// <param name="SourceCode">数据源代码，默认ig</param>
        public SatClockCollection(bool isUniqueSource = true, string SourceCode = "ig") : base(isUniqueSource, SourceCode)
        {
            this.Name = "组合多文件钟差";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data">按照类型读入的原始数据结构</param>
        /// <param name="isUniqueSource"></param>
        /// <param name="indicatedSource"></param>
        public SatClockCollection(Dictionary<SatelliteType, Dictionary<string, List<TClockFile>>> data, bool isUniqueSource = true, string indicatedSource = "ig")
            : base(isUniqueSource, indicatedSource)
        {
            this.Name = "组合多文件钟差";
            //把这些组合成一个 SatEphemerisCollection 
            foreach (var typedSource in data)
            {
                if (typedSource.Value.Count > 0)
                {
                    this.Name += "," + typedSource.Key + ":";
                }
                var currentType = typedSource.Key;
                foreach (var sourcedSp3File in typedSource.Value)
                {
                    var source = sourcedSp3File.Key;
                    if (isUniqueSource && !source.Contains(indicatedSource))
                    {
                        log.Warn("由于指定了数据源，且设置为唯一，忽略 " + source);
                        continue;
                    }
                    int i = 0;
                    foreach (var file in sourcedSp3File.Value)
                    {
                        if (i != 0) { this.Name += ", "; }
                        this.Name += file.Name;
                        log.Debug("合并文件 " + file.Name);
                        foreach (var sat in file.GetSatClockCollection())
                        {
                            //只处理选定的类型
                            if (currentType != sat.Prn.SatelliteType) { continue; }
                            this.GetOrCreate(sat.Prn).Add(sat);
                        }
                        i++;
                    }
                }
            }
        }
         
        public override TimedSatObject<TAtomicClock> Create(SatelliteNumber key)
        {
            return new TimedSatObject<TAtomicClock>(key) { SourceCode = SourceCode, IsSameSourceRequired = IsSameSourceRequired };
        }


    }
}