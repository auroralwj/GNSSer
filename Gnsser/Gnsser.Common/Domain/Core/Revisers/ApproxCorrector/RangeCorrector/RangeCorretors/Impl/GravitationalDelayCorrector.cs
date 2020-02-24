//2014.05.22, Cui Yang, created
//2014.09.16, cy, 面向对象


using System;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Domain;

namespace Gnsser.Correction
{    
    /// <summary>
    /// 引力延迟效应改正
    /// Default computes the delay in the signal due to changes in the gravity field
    /// </summary>
    public class GravitationalDelayCorrector : AbstractRangeCorrector
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GravitationalDelayCorrector()
        {

            this.Name = "引力延迟距离改正";
            this.CorrectionType = CorrectionType.GravitationalDelay;
        }

        public override void Correct(EpochSatellite epochSatellite)
        {
            IEphemeris sat = epochSatellite.Ephemeris;
            if(sat == null) { return; }
            XYZ svPos = sat.XYZ;
            XYZ nominalPos = epochSatellite.SiteInfo.EstimatedXyz;
            double correction = GetSatGravDelayCorrection(svPos, nominalPos);      

            this.Correction = (correction);
         }

        /// <summary>
        /// 引力延迟效应改正
        /// </summary>
        /// <param name="svPos"></param>
        /// <param name="staPos"></param>
        /// <returns></returns>
        public static double GetSatGravDelayCorrection(XYZ svPos, XYZ staPos)
        {
            /// Constant value needed for computation.This value comes from：
            /// K=(1+gamma) muE/(c*c),where
            /// gamma=1.0 (general relativity)
            /// muE=3.986004418E14 m"3/s"3 (std gravitational parameter, Earth)
            /// c=2.99792458e8 m/s (speed of light)
            /// </summary>
            double K = 0.0088700561;// 0.887005608e-2; 
            double gravDel = 0.0;
            //Get magnitude of satellite position vector
            double r2 = svPos.Length;

            //Get vector from Earth mass center to receiver
            XYZ rxPos = new XYZ(staPos.X, staPos.Y, staPos.Z);
            //Compute magnitude of receiver position vector
            double r1 = rxPos.Length;

            //Compute the difference vector between satellite and receiver positions
            XYZ difPos = svPos - rxPos;  

            //Compute magnitude of the difference between rxPos and svPos
            //Sagnac effect
            double r12 = difPos.Length;// +AstronomicalFunctions.OMGE * (svPos.X * rxPos.Y - svPos.Y * rxPos.X) / GnssConst.LIGHT_SPEED;

            //Compute gravitational delay correction
            gravDel = K * Math.Log((r1 + r2 + r12) / (r1 + r2 - r12));

            return gravDel;           
        }      
    }
}
