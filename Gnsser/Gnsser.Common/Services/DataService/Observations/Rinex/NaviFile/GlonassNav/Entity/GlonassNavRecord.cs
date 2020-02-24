//2014.12.19, czs, edit in namu, 添加注释

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Times;

namespace Gnsser.Data.Rinex
{

    /// <summary>
    /// Glonass navigation message Data Record.
    /// </summary>
    public class GlonassNavRecord :SatClockBias
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GlonassNavRecord()
        {

        }

        public GlonassNavRecord(SatClockBias record)
        {
            this.Time = record.Time;
            this.Prn = record.Prn;
            this.ClockBias = record.ClockBias;
            this.ClockDrift = record.ClockDrift;
            this.DriftRate = record.DriftRate;
        }  
        /// <summary>
        /// 卫星相对频率偏差
        /// </summary>
        public double RelativeFrequencyBias { get; set; }
        /// <summary>
        /// 电文帧时间
        /// </summary>
        public double MessageTime { get; set; }
        /// <summary>
        /// 位置坐标
        /// </summary>
        public XYZ XYZ { get; set; }
        /// <summary>
        /// 速度
        /// </summary>
        public XYZ XyzVelocity { get; set; }
        /// <summary>
        /// 加速度
        /// </summary>
        public XYZ XyzAcceleration { get; set; }
        /// <summary>
        /// 0 OK
        /// </summary>
        public double Health { get; set; }
        /// <summary>
        /// 频率编号
        /// </summary>
        public double FrequencyNumber { get; set; }
        /// <summary>
        /// Days
        /// </summary>
        public double AgeOfOper { get; set; }


    }

}
