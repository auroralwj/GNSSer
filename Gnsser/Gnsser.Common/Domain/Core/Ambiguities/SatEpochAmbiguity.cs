//2015.01.12, czs,  create in namu, 双差定位结果的模糊度信息

using System;
using System.Collections.Generic;
using System.Text;
using Gnsser.Domain;
using Gnsser.Data.Sinex;
using Gnsser.Data.Rinex;
using Gnsser.Times;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Times; 

namespace Gnsser.Service
{   
    /// <summary>
    /// 历元模糊度参数
    /// </summary>
    public class SatEpochAmbiguity
    {
        /// <summary>
        /// 模糊度参数 的构造函数。
        /// </summary>
        /// <param name="ParamName">参数名称</param>
        /// <param name="Ambiguity">模糊度</param>
        /// <param name="Time">时刻</param>
        public SatEpochAmbiguity(string ParamName,int Ambiguity, Time Time)
        {
            this.ParamName = ParamName;
            this.Ambiguity = Ambiguity;
            this.Time = Time;
        }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParamName { get; set; }

        /// <summary>
        /// 时刻
        /// </summary>
        public Time Time { get; set; }

        /// <summary>
        /// 模糊度
        /// </summary>
        public int Ambiguity { get; set; }

        public override string ToString()
        {
            return ParamName + "(" +Time.ToShortTimeString() + ")_" + Ambiguity;
        }
    }
}