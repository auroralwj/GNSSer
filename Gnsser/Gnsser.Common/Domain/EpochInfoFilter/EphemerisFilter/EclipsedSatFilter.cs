//2014.07.02, Cui Yang, created, 质量控制

using System;
using System.Collections.Generic;
using System.Linq;
using Gnsser.Domain;
using System.Text;

using Gnsser.Times;
using Gnsser.Service;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo;
using Geo.Times; 



namespace Gnsser.Filter
{
    /** This class filters out satellites that are eclipsed by Earth shadow.
      *
      * This class is meant to be used with the GNSS satData structures objects
      * found in "DataStructures" class.
     *   * The "EclipsedSatFilter" object will visit every satellite in the GNSS
      * satData structure that is "gRin" and will determine if such satellite is
      * in eclipse, or whether it recently was.
      *
      * This effect may be important when using precise positioning, because
      * satellite orbits tend to degrade when satellites are in eclipse, or
      * when they have been in eclipse recently.
      *
      * There are dayServices adjustable parameters in this class: Shadow cone angle
      * (30 degrees by default), and the period after eclipse that the
      * satellite will still be deemed unreliable (1800 fraction by default).
      *
      */
    /// <summary>
    /// 删除太阳阴影影响的卫星。
    /// </summary>
    public class EclipsedSatFilter:EpochInfoReviser
    {
        /// <summary>
        /// 太阳阴影影响，删除卫星。Default constructor.
        /// </summary>
        public EclipsedSatFilter() : this(30, 1800)
        {
            this.Name = "太阳阴影删除器";
        }
        /** Common constructor
         *
         * @param angle      Aperture angle of shadow cone, in degrees.
         * @param pShTime    Time after exiting shadow that satellite will
         *                   still be filtered out, in fraction.
         */
        
        public EclipsedSatFilter(double angle, double pShTime)
        {
            this.Name = "太阳阴影删除器";
            log.Info("将删除受太阳阴影影响的卫星。");
            ConeAngle = angle;
            PostShadowPeriod = pShTime;
            RemovedPrn = new List<SatelliteNumber>();
        }
        List<SatelliteNumber> RemovedPrn;
        /// <summary>
        /// Aperture angle of shadow cone, in degree
        /// </summary>
        private double ConeAngle { get; set; }

        /// <summary>
        /// Time after exiting shadow that satelltie will still be filtered out, in fraction
        /// </summary>
        private double PostShadowPeriod { get; set; }

        /// <summary>
        /// Dictionary holding the time information about every satellite in eclipse
        /// </summary>
        private Dictionary<SatelliteNumber, Time> ShadowEpoch = new Dictionary<SatelliteNumber, Time>();



        /// <summary>
        /// EclipsedSatFilter核心
        /// </summary>
        /// <param name="gData"></param>
        /// <returns></returns>
        public override bool Revise(ref EpochInformation gData)
        {
           
            //   Time epoch = gData.CorrectedTime;
            Time epoch = gData.ReceiverTime;



            List<SatelliteNumber> satRejectedSet = new List<SatelliteNumber>();
            // Set the threshold to declare that satellites are in eclipse
            // threshold = cos(180 - coneAngle/2)
            double threshold = Math.Cos((CoordConsts.PI - ConeAngle / 2.0 * CoordConsts.DegToRadMultiplier));

            // Compute Sun position at this epoch, and store it in a Triple
            SunPosition sunPosition = new SunPosition();
            // Variables to hold Sun and Moon positions
            XYZ sunPos = (sunPosition.GetPosition(epoch));

            // Loop through all the satellites
            foreach (var sat in gData.EnabledSats)
            { 
                SatelliteNumber prn = sat.Prn;
                //Define a XYZ that will hold satellite position, in ECEF
                XYZ svPos = gData[prn].Ephemeris.XYZ;//.GetSatPostion(satelliteType);
                XYZ rk = svPos.UnitVector();    // Unitary vector from Earth mass center to satellite

                // Unitary vector from Earth mass center to Sun
                XYZ ri = sunPos.UnitVector();

                //Get dot product between unitary vectors = cosine(angle)
                double cosAngle = ri.Dot(rk);

                // Check if satellite is within shadow
                if (cosAngle <= threshold)
                {
                    // If satellite is eclipsed, then schedule it for removal
                    satRejectedSet.Add(prn);

                    if (!ShadowEpoch.ContainsKey(prn)) ShadowEpoch.Add(prn, epoch);
                    // Keep track of last known epoch the satellite was in eclipse
                    ShadowEpoch[prn] = epoch;

                    continue;
                }
                else
                {
                    // Maybe the satellite is out fo shadow, but it was recently
                    // in eclipse. Check also that.
                    if (ShadowEpoch.ContainsKey(prn))
                    {
                        // // If satellite was recently in eclipse, check if elapsed
                        // time is less or equal than postShadowPeriod
                        double temp = (double)Math.Abs(epoch - ShadowEpoch[prn]);

                        if (temp <= PostShadowPeriod)
                        {
                            // Satellite left shadow, but too recently. Delete it
                            satRejectedSet.Add(prn);
                        }
                        else
                        {
                            // If satellite left shadow a long time ago, set it free
                            ShadowEpoch.Remove(prn);
                        }
                    }
                }


            }
            //debug
            if (satRejectedSet.Count > 0 && satRejectedSet.FindAll(m=>!RemovedPrn.Contains(m) ).Count > 0)
            {

                StringBuilder sb = new StringBuilder();
                sb.Append(gData.Name + "," + gData.ReceiverTime + ",删除太阳阴影影响的卫星:");
                sb.Append(String.Format(new EnumerableFormatProvider(), "{0}", satRejectedSet));
                log.Debug(sb.ToString());
                RemovedPrn.AddRange(satRejectedSet.FindAll(m => !RemovedPrn.Contains(m)));
            }

            //Remove satellites with missing satData
            gData.Remove(satRejectedSet, true,"删除太阳阴影影响的卫星");
           

            return true;
        }


    

    }
}
