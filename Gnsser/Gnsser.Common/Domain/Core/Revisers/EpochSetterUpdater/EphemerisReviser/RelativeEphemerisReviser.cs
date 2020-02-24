//2014.09.15, czs, create, 设置卫星星历。
//2014.10.12, czs, edit in hailutu, 对星历赋值进行了重新设计，分解为几个不同的子算法
//2015.02.08, 崔阳, 卫星钟差和精密星历若同时存在，则不可分割
//2017.08.06, czs, refact in hongqing, RelativeEphemerisCorrector 单独成类

using System;
using System.IO;
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
using Gnsser.Data;
using Geo.Times;
using Gnsser.Correction;

namespace Gnsser
{ 
    //2017.08.06, czs, refact in hongqing, RelativeEphemerisCorrector 单独成类
    /// <summary>
    /// 相对论改正钟差
    /// </summary>
    public class RelativeEphemerisReviser : EphemerisReviser
    {
        /// <summary>
        /// 相对论改正钟差
        /// </summary>
        public RelativeEphemerisReviser ()
        {
            this.Name = "相对论改正钟差"; 
        } 

        /// <summary>
        /// 相对论改正钟差
        /// </summary>
        /// <param name="eph"></param>
        /// <returns></returns>
        public override bool Revise(ref IEphemeris eph)
        {
            //地球自转改正之后，计算相对论改正
            XYZ SatXyz = eph.XYZ;
            XYZ SatSpeed = eph.XyzDot;
            double relativeTime = GetRelativeCorrection(SatXyz, SatSpeed);

            eph.ClockBias += relativeTime;
            eph.RelativeCorrection = relativeTime; 
            
            return true;
        }
        /// <summary>
        /// 相对论改正。
        /// </summary>
        /// <param name="SatXyz">卫星位置</param>
        /// <param name="SatSpeed">卫星速度</param>
        /// <returns></returns>
        public static double GetRelativeCorrection(XYZ SatXyz, XYZ SatSpeed)
        {
            double relativity = -2 * (SatXyz.X / GnssConst.LIGHT_SPEED) * (SatSpeed.X / GnssConst.LIGHT_SPEED)
                - 2 * (SatXyz.Y / GnssConst.LIGHT_SPEED) * (SatSpeed.Y / GnssConst.LIGHT_SPEED)
                - 2 * (SatXyz.Z / GnssConst.LIGHT_SPEED) * (SatSpeed.Z / GnssConst.LIGHT_SPEED);

            return relativity;
        }
    } 
     
}
