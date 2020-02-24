// 2014.10.11, czs, create, 多普勒频率计算器，采用载波相位进行计算。

using System;
using System.Collections.Generic;
using Gnsser.Domain;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Checkers;
using Geo.Common;
using Gnsser.Times;
using Geo.Times;


namespace Gnsser
{
    /// <summary>
    /// 多普勒频率计算器，采用载波相位进行计算。
    /// </summary>
    public class DopplerShiftSetter : EpochInfoReviser
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DopplerShiftSetter()
        {
        }

        /// <summary>
        /// 上一个历元的时间。
        /// </summary>
        Time PreviousTime { get { if (Previous == null) return Time.MinValue; return Previous.ReceiverTime; } }
        EpochInformation Previous { get; set; }
        /// <summary>
        /// 矫正
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override bool Revise(ref EpochInformation info)
        {
            //第一个历元不计算。
            if (PreviousTime.Equals(Time.MinValue))
            {
                Previous = info;
                return true;
            }
            //为了防止历元跳跃，还是老老实实一步一步的计算
            double interVal = (Double)(info.ReceiverTime - PreviousTime);

            foreach (var sat in info)
            {
                SatelliteNumber prn = sat.Prn;
                if (!Previous.Contains(prn)) continue;//新卫星第一个历元不计算

                foreach (var item in sat.FrequenceTypes)
                {
                    if (!Previous[prn].Contains(item))
                        continue;
                    //这是一种近似算法，取得的是这段时间的平均值，但是误差不大，一般在一个频率以下，实际上应该积微分
                    //这里是前减后，定义如果为负，则远离，如果为正数，则靠近 approach
                    double doppler = (Previous[prn][item].PhaseRange.RawPhaseValue - sat[item].PhaseRange.RawPhaseValue) / interVal;

                    sat[item].Set(ObservationType.D, new Domain.Observation(doppler, new ObservationCode(ObservationType.D,(int)item)));
                }
            }
            return true;
        }
    }
}
