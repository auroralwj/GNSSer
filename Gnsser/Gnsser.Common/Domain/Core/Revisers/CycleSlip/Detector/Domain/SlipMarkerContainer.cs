// 2014.09.10, czs, create, 周跳探测初步

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Times;
using Geo.Times; 

namespace Gnsser.Checkers
{
    /// <summary>
    /// 周跳容器，负责存储一个数据源的周跳信息。
    /// </summary>
    public class SlipMarkerContainer
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public SlipMarkerContainer()
        {
            this.data = new Dictionary<FrequenceType, Dictionary<SatelliteNumber, List<SlipMarker>>>();
        }
        #region 核心存储模型
        private Dictionary<FrequenceType, Dictionary<SatelliteNumber, List<SlipMarker>>> data { get; set; }
        #endregion
        #region 属性

        public int PhaseCount { get { return data.Count; } }

        public int PhaseACount { get { return GetPhaseCount(FrequenceType.A); } }
        public int PhaseBCount { get { return GetPhaseCount(FrequenceType.B); } }
        public int PhaseCCount { get { return GetPhaseCount(FrequenceType.C); } }

        public List<SatelliteNumber> PhaseAPrns { get { return GetPrns(FrequenceType.A); } }

        public List<SatelliteNumber> PhaseBPrns { get { return GetPrns(FrequenceType.B); } }


        #endregion
        public List<SatelliteNumber> GetPrns(FrequenceType phaseName) { return Contians(phaseName)? new List<SatelliteNumber>(data[phaseName].Keys):new List<SatelliteNumber>();}
        public int GetPhaseCount(FrequenceType phaseName) { return Contians(phaseName) ? data[phaseName].Count : 0; }
        #region 标记方法
        public bool Contians(FrequenceType phaseName) { return data.ContainsKey(phaseName); }
        public bool Contians(FrequenceType phaseName, SatelliteNumber prn) { return Contians(phaseName) && data[phaseName].ContainsKey(prn); }
        public bool Contians(FrequenceType phaseName, SatelliteNumber prn, Time time) { return Contians(phaseName, prn) && data[phaseName][prn].Exists(m => m.GpsTime.Equals(time)); }

        public void CheckAndSet(FrequenceType phaseName) { if (!Contians(phaseName)) this.data[phaseName] = new Dictionary<SatelliteNumber, List<SlipMarker>>(); }
        public void CheckAndSet(FrequenceType phaseName, SatelliteNumber prn)
        {
            if (!Contians(phaseName, prn)) this.data[phaseName][prn] = new List<SlipMarker>();
        }
        /// <summary>
        /// 设置一个Maker值
        /// </summary>
        /// <param name="marker"></param>
        public void SetMarker(SlipMarker marker)
        {
            CheckAndSet(marker.PhaseName, marker.Prn);
            this.data[marker.PhaseName][marker.Prn].Add(marker);
        }
        public bool HasSlipe(FrequenceType phaseName, SatelliteNumber prn, Time time)
        {
            return Contians(phaseName, prn, time);
        }
        /// <summary>
        /// 任意一个频率跳变产生都视为跳变了。
        /// </summary>
        /// <param name="satelliteType"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool HasSlipe(SatelliteNumber prn, Time time)
        {
            return Contians(FrequenceType.A, prn, time) || Contians(FrequenceType.B, prn, time);
        }

        public List<SlipMarker> GetClipeMarkers(FrequenceType phaseA, SatelliteNumber prn)
        {
            if (Contians(phaseA, prn)) return data[phaseA][prn];
            return new List<SlipMarker>();
        }
        #endregion

    }
}
