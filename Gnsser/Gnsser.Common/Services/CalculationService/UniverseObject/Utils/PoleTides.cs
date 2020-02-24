//2014.05.22, Cui Yang, created
//2014.06.30, Cui Yang, 极潮改正的参数更新为IERS2010
//2014.08.18, czs, 将结果采用NEU坐标表示，分别对应北东天方向。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;
using Gnsser;
using Gnsser.Times;
using Geo.Times; 

namespace Gnsser
{
    /** This class computes the effect of pole tides, or more properly
       *  called "rotational deformations due to polar motion", at a given
       *  position and epoch.
       *
       *  The model used is the one proposed by the "International Earth
       *  Rotation and Reference Systems Service" (IERS) in its upcomming
       *  "IERS Conventions" document (Chapter 7), available at:
       *
       *  http://tai.bipm.org/iers/convupdt/convupdt.html
       *
       *  The pole movement parameters x, y for a given epoch may be
       *  found at:
       *
       *  ftp://hpiers.obspm.fr/iers/eop/eop.others
       *
       *  Maximum displacements because of this effect are:
       *
       *  \ltime Vertical:    2.5 cm
       *  \ltime Horizontal:  0.7 cm
       *
       *  For additional information you may consult: Wahr, J.M., 1985,
       *  "Deformation Induced by Polar Motion", Journal of Geophysical
       *  Research, Vol. 90, No B11, info. 9363-9368.
       *
       *  \warning Please take into account that pole tide equations in
       *  IERS document use co-latitude instead of latitude.
       */
    public class PoleTides
    {
     
        
        // Default constructor. Sets zero pole displacement
        public PoleTides()
        {
            this.XDisplacement = 0.0;
            this.YDisplacement = 0.0;
        }

        /** Common constructor
         *
         * @param x     Pole displacement x, in arcseconds
         * @param y     Pole displacement y, in arcseconds
         */
        public PoleTides(double x, double y)
        {
            this.XDisplacement = x;
            this.YDisplacement = y;
        }  
        
        /// <summary>
        ///  Pole displacement x, in arcseconds
        /// </summary>
        public double XDisplacement { get; set; }
        /// <summary>
        /// Pole displacement y, in arcseconds
        /// </summary>
        public double YDisplacement { get; set; }


        /** Returns the effect of pole tides (meters) at the given
          *  position and epoch, in the Up-East-North (UEN) reference frame.
          *
          * @param[in]  t Epoch to look up
          * @param[in]  info Position of interest
          *
          * @return a Triple with the pole tide effect, in meters and in
          *    the UEN reference frame.
          *
          * @throw InvalidRequest If the request can not be completed for any
          *    reason, this is thrown. The text may have additional
          *    information about the reason the request failed.
          *
          * @warning In order to use this method, you must have previously
          *    set the current pole displacement parameters.
          *
          */
        public NEU GetPoleTide(Time t, XYZ p)
        {
            //store the results
            NEU res = new NEU(0.0, 0.0, 0.0);

            //Declare J2000 reference time: January 1st, 2000, at noon
            Time j2000 = new Time(2000, 1, 1, 12, 0, 0.0);

            //Get current position's latitude and longitude, in radians
            GeoCoord geoCoord = Geo.Coordinates.CoordTransformer.XyzToGeoCoord_Rad(p.X, p.Y, p.Z);
            double latitude = geoCoord.Lat;
            double longitude = geoCoord.Lon;

            // Compute appropriate running averages
            // Get time difference between current epoch and
            // J2000.0, in years
            double timedif = (double)((t.MJulianDays - j2000.MJulianDays) / 365.25M);

            //IERS mean pole model
            //double xpbar = (0.054 + timedif * 0.00083);
            //double ypbar = (0.357 + timedif * 0.00395);
            double xpbar = 0.0;
            double ypbar = 0.0;

            #region IERS Conventions 2010 p115

            if (t.Year < 2010)
            {
                xpbar = 55.974 * 1.0e-3 + timedif * 1.8243 * 1.0e-3 + timedif * timedif * 0.18413 * 1.0e-3 + timedif * timedif * timedif * 0.007024 * 1.0e-3;
                ypbar = 346.346 * 1.0e-3 + timedif * 1.7896 * 1.0e-3 + timedif * timedif * (-0.10729) * 1.0e-3 + timedif * timedif * timedif * (-0.000908) * 1.0e-3;
            }
            else
            {
                xpbar = 23.513 * 1.0e-3 + timedif * 7.6141 * 1.0e-3;
                ypbar = 358.891 * 1.0e-3 + timedif * (-0.6287) * 1.0e-3;
            }

            #endregion
            // Now, compute m1 and m2 parameters
            double m1 = (XDisplacement - xpbar);
            double m2 = (ypbar - YDisplacement);

            // Now, compute some useful values
            double sin2lat = (Math.Sin(2.0 * latitude));
            double cos2lat = (Math.Cos(2.0 * latitude));
            double sinlat = (Math.Sin(latitude));
            double sinlon = (Math.Sin(longitude));
            double coslon = (Math.Cos(longitude));

            // Finally, get the pole tide values, in UEN reference
            // frame and meters
            double u = -0.033 * sin2lat * (m1 * coslon + m2 * sinlon);
            double e = +0.009 * sinlat * (m1 * sinlon - m2 * coslon);
            double n = -0.009 * cos2lat * (m1 * coslon + m2 * sinlon);

            res = new NEU(n, e, u);
            
            // Please be aware that the former equations take into account
            // that the IERS pole tide equations use CO-LATITUDE instead
            // of LATITUDE. See Wahr, 1985.
            return res;
        }

        /** Returns the effect of pole tides (meters) on the given
         *  position, in the Up-East-North (UEN) reference frame.
         *
         * @param[in] info Position of interest
         * @param[in] x Pole displacement x, in arcseconds
         * @param[in] y Pole displacement y, in arcseconds
         *
         * @return a Triple with the pole tide effect, in meters and in
         *    the UEN reference frame.
         *
         * @throw InvalidRequest If the request can not be completed for
         *    any reason, this is thrown. The text may have additional
         *    information about the reason the request failed.
         */
        public NEU getPoleTide(Time t, XYZ p, double x, double y)
        {
            SetXY(x, y);
            return GetPoleTide(t, p);
        }

        /** Method to set the pole displacement parameters
         *
         * @param x     Pole displacement x, in arcseconds
         * @param y     Pole displacement y, in arcseconds
         *
         * @return This same object
         */
        public PoleTides SetXY(double x, double y)
        {
            XDisplacement = x;
            YDisplacement = y;
            return this;
        }


    }
}
