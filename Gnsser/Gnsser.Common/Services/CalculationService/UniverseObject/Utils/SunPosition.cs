//2014.05.22, Cui Yang, created

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
    /** This class computes the approximate position of the Sun at the 
       * given epoch in the ECEF system. It yields best results between 
       * March 1st 1900 and February 28th 2100.
       *
       * This is a C++ implementation version based on the FORTRAN version 
       * originally written by P.TProduct. Wallace, Starlink Project. The FORTRAN
       * version of Starlink project was available under the GPL license.
       *
       * Errors in position in the period 1950-2050 are:
       *
       * \ltime Maximum: 13*10^-5 AU (19200 km).
       * \ltime RMS: 5*10^-5 AU (7600 km).
       *
       * More information may be found in http://starlink.jach.hawaii.edu/
       */
    public class SunPosition:SunMoonPosition
    {   /// GPS value of PI*2
        public const double TWO_PI = 6.2831853071796;
        //Time of the prevObj valid time
        private static  Time initialTime = new Time(1900, 3, 1, 0, 0, 0.0);

        // Time of the last valid time
        private static  Time finalTime = new Time(2100, 2, 28, 0, 0, 0.0);

        /// <summary>
        /// Default constructor
        /// </summary>
        public SunPosition()
        {}

        /// <summary>
        /// Returns the position of Sun ECEF coordinates (meters) at the indicated time.
        /// </summary>
        /// <param name="t">the time to look up</param>
        /// <returns>the position of the Sun at time (as a Triple)</returns>
        /// @throw InvalidRequest If the request can not be completed for any
        /// reason, this is thrown. The text may have additional
        /// information as to why the request failed.
        /// @warning This method yields an approximate result, given that pole movement 
        /// is not taken into account, neither precession nor nutation.
        public XYZ GetPosition(Time t)
        {
            // Test if the time interval is correct
            if ((t < initialTime) || (t > finalTime))
            { throw new Exception("Provided epoch is out of bounds!"); }

            XYZ res = new XYZ();//Store the results
            res = getPositionCIS(t);
            res = CIS2CTS(res, t);

            return res;
        }

        /// <summary>
        /// Function to compute Sun position in CIS system (coordinates in meters)
        /// </summary>
        /// <param name="t">Epoch</param>
        /// <returns></returns>
        public XYZ getPositionCIS(Time t)
        {
            // Test if the time interval is correct
            if ((t < initialTime) || (t > finalTime))
            { throw new Exception("Provided epoch is out of bounds!"); }

            //Compute the years, and fraction of year, pased since J1900.0
            int y = t.Year;//Current year
            int doy = t.DayOfYear; //Day of current year

            double fd = t.SecondsOfDay / 86400.0;//Fraction of secondOfWeek
            int years = y - 1900;//Integer number of years since J1900.0
            int iy4 = ((y % 4) + 4) % 4; //Is it a leap year?

            //Compute fraction of year
            double yearfrac = ((4 * (doy - 1 / (iy4 + 1)) - iy4 - 2) + 4.0 * fd) / 1461.0;

            double time = years + yearfrac;

            //Compute the geometric mean longitude of the Sun
            double elm = Math.IEEERemainder((4.881628 + TWO_PI * yearfrac + 0.0001342 * time), TWO_PI);
            if (elm < 0)
            { elm += TWO_PI; }


            //Mean longitude of perihelion
            double gamma = (4.90823 + 0.00030005 * time);

            // Mean anomaly
            double em = (elm - gamma);

            // Mean obliquity
            double eps0 = (0.40931975 - 2.27e-6 * time);

            // Eccentricity
            double e = (0.016751 - 4.2e-7 * time);
            double esq = (e * e);

            // True anomaly
            double v = (em + 2.0 * e * Math.Sin(em) + 1.25 * esq * Math.Sin(2.0 * em));

            // True ecliptic longitude
            double elt = (v + gamma);

            // True distance
            double r = ((1.0 - esq) / (1.0 + e * Math.Cos(v)));

            // Moon's mean longitude
            double elmm = (Math.IEEERemainder((4.72 + 83.9971 * time), TWO_PI));
            if (elmm < 0)
            { elmm += TWO_PI; }

            // Useful definitions
            double coselt = Math.Cos(elt);
            double sineps = Math.Sin(eps0);
            double coseps = Math.Cos(eps0);
            double w1 = -r * Math.Sin(elt);
            double selmm = Math.Sin(elmm);
            double celmm = Math.Cos(elmm);

            // Sun position is the opposite of Earth position
            double sx = (r * coselt + MeanEarthMoonBary * celmm) * AU_CONST;
            double sy = (MeanEarthMoonBary * selmm - w1) * coseps * AU_CONST;
            double sz = (-w1 * sineps) * AU_CONST;

            XYZ result = new XYZ(sx, sy, sz);

            return result;
        }

        /// <summary>
        /// Determine the earliest time for which this object can successfully determine the position for the Sun.
        /// </summary>
        /// <returns>The initial time</returns>
        public Time getInitialTime()
        { return initialTime; }

        /// <summary>
        /// etermine the latest time for which this object can  successfully determine the position for the Sun.
        /// </summary>
        /// <returns>The final time</returns>
        public Time getFinalTime()
        { return finalTime; }
    }
}
