//2016.10.17, double, create in hongqing, RTCM MSM基础类

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    ///  RTCM MSM基础类
    /// </summary>
    public class BaseMSM
    {
        public BaseMSM()
        {
            this.ExtendedSatInfo = new List<uint>();
            this.HalfCycleAmbiguityIndicator = new List<int>();
            this.NumberOfIntegerMsInSatRoughRange = new List<uint>();
            this.PhaseRangeLockTimeIndicator = new List<uint>();
            this.PhaseRangeLockTimeIndicatorWithExtendedRangeAndResolution = new List<uint>();
            this.SatelliteRoughRangesModulo1ms = new List<uint>();
            this.SatRoughPhaseRangeRate = new List<int>();
            this.SignalCnr = new List<uint>();
            this.SignalCnrWithExtendedResolution = new List<uint>();
            this.SignalFinePhaseRange = new List<int>();
            this.SignalFinePhaseRangeRate = new List<int>();
            this.SignalFinePhaseRangeWithExtendedResolution = new List<int>();
            this.SignalFinePseudorange = new List<int>();
            this.SignalFinePseudorangeWithExtendedResolution = new List<int>();
         
        }
        /// <summary>
        /// uint8
        /// </summary>
        public List<uint> NumberOfIntegerMsInSatRoughRange { get; set; }
        /// <summary>
        /// uint4
        /// </summary>
        public List<uint> ExtendedSatInfo { get; set; }
        /// <summary>
        /// uint10
        /// </summary>
        public List<uint> SatelliteRoughRangesModulo1ms { get; set; }
        /// <summary>
        /// int14
        /// </summary>
        public List<int> SatRoughPhaseRangeRate { get; set; }
        /// <summary>
        /// int20
        /// </summary>
        public List<int> SignalFinePseudorangeWithExtendedResolution { get; set; }
        /// <summary>
        /// int24
        /// </summary>
        public List<int> SignalFinePhaseRangeWithExtendedResolution { get; set; }
        /// <summary>
        /// uint10
        /// </summary>
        public List<uint> PhaseRangeLockTimeIndicatorWithExtendedRangeAndResolution { get; set; }
        /// <summary>
        /// bit(1)
        /// </summary>
        public List<int> HalfCycleAmbiguityIndicator { get; set; }
        /// <summary>
        /// uint10
        /// </summary>
        public List<uint> SignalCnrWithExtendedResolution { get; set; }
        /// <summary>
        /// int15
        /// </summary>
        public List<int> SignalFinePhaseRangeRate { get; set; }
        /// <summary>
        /// int15
        /// </summary>
        public List<int> SignalFinePseudorange { get; set; }
        /// <summary>
        /// int22
        /// </summary>
        public List<int> SignalFinePhaseRange { get; set; }
        /// <summary>
        /// uint4
        /// </summary>
        public List<uint> PhaseRangeLockTimeIndicator { get; set; }
        /// <summary>
        /// uint6
        /// </summary>
        public List<uint> SignalCnr { get; set; }
    }
}