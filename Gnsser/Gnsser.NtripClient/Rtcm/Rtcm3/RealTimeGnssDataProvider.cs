//2017.04.24, czs, create in hongqing, 实时GNSS数据提供器。


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;
using System.IO;
using Geo.Times;
using Geo.IO;
using Gnsser.Data.Rinex;

namespace Gnsser.Ntrip.Rtcm
{



    /// <summary>
    /// 实时GNSS数据提供器,对应一个测站。
    /// </summary>
    public class RealTimeGnssDataProvider
    {
        Log log = new Log(typeof(RealTimeGnssDataWriter));

        public RealTimeGnssDataProvider()
        {
        }

        /// <summary>
        /// 测站，挂载点
        /// </summary>
        public string SiteName { get; set; }

        #region 事件，与外界交互数据
        /// <summary>
        /// 更新了观测文件的头部信息
        /// </summary>
        public event Action<RinexObsFileHeader> ObsHeaderCreated;
        /// <summary>
        /// 更新了观测文件的头部信息
        /// </summary>
        public event Action<RinexObsFileHeader> ObsHeaderUpdated;
        /// <summary>
        /// 传来了新的历元观测信息
        /// </summary>
        public event Action<RinexEpochObservation> EpochObservationReceived;
        /// <summary>
        /// 传来了新的星历信息
        /// </summary>
        public event Action<EphemerisParam> EphemerisInfoReceived;
        /// <summary>
        /// 传来了新的星历信息
        /// </summary>
        public event Action<GlonassNavRecord> GlonassNavRecordReceived;
        /// <summary>
        /// 传来了Sp3Record
        /// </summary>
        public event Action<Sp3Section> SSRSp3RecordReceived;
        public event Action<AtomicClock > SSRClkRecordReceived;

        protected void OnObsHeaderCreated(RinexObsFileHeader obj)
        {
            if (obj == null) { return; }
           if (ObsHeaderCreated != null) { ObsHeaderCreated(obj); } 
        }
        protected void OnObsHeaderUpdated(RinexObsFileHeader obj) {
            if (obj == null) { return; }
            if (ObsHeaderUpdated != null) { ObsHeaderUpdated(obj); } 
        }
        protected void OnEpochObservationReceived(RinexEpochObservation obj) {
            if (obj == null || obj.Count == 0) { return; }
            if (EpochObservationReceived != null) { EpochObservationReceived(obj); }       
        }
        protected void OnEphemerisInfoReceived(EphemerisParam obj)
        {
            if (obj == null ) { return; }
            if (EphemerisInfoReceived != null) { EphemerisInfoReceived(obj); } 
        }
        protected void OnSSRSp3RecordReceived(Sp3Section obj)
        {
            if (obj == null) { return; }
            if (SSRSp3RecordReceived != null) { SSRSp3RecordReceived(obj); } 
        }
        protected void OnSSRClkRecordReceived(AtomicClock obj)
        {
            if (obj == null ) { return; }
            if (SSRClkRecordReceived != null) { SSRClkRecordReceived(obj); } 
        }
        protected void OnGlonassNavRecordReceived(GlonassNavRecord obj)
        {
            if (obj == null ) { return; }
            if (GlonassNavRecordReceived != null) { GlonassNavRecordReceived(obj); }       
        }
        #endregion


        /// <summary>
        /// 创建头部信息 默认 3.02 版本
        /// </summary>
        /// <returns></returns>
        protected RinexObsFileHeader CreateOFileHeader(double version = 3.02)
        {
            RinexObsFileHeader header = new RinexObsFileHeader();
            header.Version = version;
            header.MarkerName = SiteName;
            header.SetTypeOfObserves(SatelliteType.G, new List<string>() { "C1X", "L1X", "P2X", "L2X" });
            header.ApproxXyz = new Geo.Coordinates.XYZ();
            header.Hen = new Geo.Coordinates.HEN();

            return header;
        }
    }

}