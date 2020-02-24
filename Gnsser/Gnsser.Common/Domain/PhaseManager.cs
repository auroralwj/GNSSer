//2014.09.17, cy, ceate,  GnssAsistantInfo 必要的辅助信息
//2014.09.20, czs, refactor, PhaseManager 负责 接收机和卫星相位维护
//2016.03.23, czs, refactor in hongqing, 提取，采用字典实现

using System;
using System.Collections.Generic;
using Gnsser.Data.Rinex;
using Geo.Coordinates;
using Gnsser;
using System.Text;
using Geo;

namespace Gnsser
{ 

    /// <summary>
    /// 接收机和卫星相位维护。
    /// </summary>
    public class PhaseManager : BaseDictionary<SatelliteNumber, SatVectorPhase>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PhaseManager()
        { }
    }
    
    /// <summary>
    /// 测站卫星相位
    /// </summary>
    public class SatVectorPhase
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        public SatVectorPhase()
        {

        }
        /// <summary>
        /// 接收机相位
        /// </summary>
        public double PhaseOfReceiver { get; set; }
        /// <summary>
        /// 卫星相位
        /// </summary>
        public double PhaseOfSatellite { get; set; }
        /// <summary>
        /// 改正数
        /// </summary>
        public double CorrectionValue { get; set; }
    }

}
