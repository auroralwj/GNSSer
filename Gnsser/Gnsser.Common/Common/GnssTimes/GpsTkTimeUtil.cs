using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Utils;
using Geo.Times;


namespace Gnsser.Times
{
    /// <summary>
    /// GpsTk 时间算法。核心由 儒略日+日秒+毫秒 组成。
    /// 采用浮点数计算。
    /// </summary>
    public static class GpsTkTimeUtil
    {

        public static void JulianDayToCalendar(long jd, ref int iyear, ref int imonth, ref int iday)
        {
            long L, M, N, P, Q;
            if (jd > 2299160)    // after Oct 4, 1582
            {
                L = jd + 68569;
                M = (4 * L) / 146097;
                L = L - ((146097 * M + 3) / 4);
                N = (4000 * (L + 1)) / 1461001;
                L = L - ((1461 * N) / 4) + 31;
                P = (80 * L) / 2447;
                iday = Convert.ToInt32(L - (2447 * P) / 80);
                L = P / 11;
                imonth = Convert.ToInt32(P + 2 - 12 * L);
                iyear = Convert.ToInt32(100 * (M - 49) + N + L);
            }
            else
            {
                P = jd + 1402;
                Q = (P - 1) / 1461;
                L = P - 1461 * Q;
                M = (L - 1) / 365 - L / 1461;
                N = L - 365 * M + 30;
                P = (80 * N) / 2447;
                iday = Convert.ToInt32(N - (2447 * P) / 80);
                N = P / 11;
                imonth = Convert.ToInt32(P + 2 - 12 * N);
                iyear = Convert.ToInt32(4 * Q + M + N - 4716);
                if (iyear <= 0)
                {
                    --iyear;
                }
            }
            // catch century/non-400 non-leap years
            if (iyear > 1599 &&
               (iyear % 100) != 0 &&
               (iyear % 400) == 0 &&
               imonth == 2 &&
               iday == 29)
            {
                imonth = 3;
                iday = 1;
            }
        }

        public static long CalendarToJulianDay(int yy, int mm, int dd)
        {
            if (yy == 0)
            { --yy; } //there is no year 0
            if (yy < 0)
            { ++yy; }

            long jd;
            double y = yy, m = mm, d = dd;

            // In the conversion from the Julian Calendar to the Gregorian
            // Calendar the secondOfWeek after October 4, 1582 was October 15, 1582.
            //
            // if the date is before October 15, 1582
            if (yy < 1582 || (yy == 1582 && (mm < 10 || (mm == 10 && dd < 15))))
            {
                jd = 1729777 + dd + 367 * yy
                   - Convert.ToInt64(Math.Floor(7 * (y + 5001 + Math.Floor((m - 9) / 7)) / 4))
                   + Convert.ToInt64(Math.Floor(275 * m / 9));
            }
            else   // after Oct 4, 1582
            {
                jd = 1721029 + dd + 367 * yy
                   - Convert.ToInt64(Math.Floor((7 * (y + Math.Floor((m + 9) / 12)) / 4)))
                   - Convert.ToInt64(Math.Floor((3 * (1 + Math.Floor((y + (m - 9) / 7) / 100)) / 4)))
                   + Convert.ToInt64(Math.Floor((275 * m / 9)));

                // catch century/non-400 non-leap years
                if (((yy % 100) != 0 &&
                     (yy % 400) == 0 &&
                     mm > 2 &&
                     mm < 9) ||
                    (((yy - 1) % 100) != 0 &&
                     ((yy - 1) % 400) == 0 &&
                     mm == 1))
                {
                    --jd;
                }
            }
            return jd;
        }
 
        /// <summary>
        /// Time - fraction
        /// </summary>
        /// <param name="left">Time</param>
        /// <param name="fraction"></param>
        /// <returns></returns>
        public static Time GpsTimeMinusSecond(Time left, double seconds)
        {
            Time result = new Time(left.DateTime - TimeSpan.FromSeconds(seconds));

            //result.JulianDay = left.JulianDay;
            //result.MillisecondsOfDay = left.MillisecondsOfDay;
            //result.FractionalMilliseconds = left.FractionalMilliseconds;
          //  result.Tolerance = left.Tolerance;
           // result.timeFrame = left.timeFrame;


            long ldd = 0, lds = 0;
            double ds = -seconds * TimeConsts.FACTOR;
            ldd += (long)(ds / TimeConsts.MS_PER_DAY);
            ds -= (long)(ds / TimeConsts.MS_PER_DAY) * (double)(TimeConsts.MS_PER_DAY);

            // Use temp variables so that we don't modify our
            // satData members until we know these values are good.
            long workingJday = (result.JulianDay), workingMsod = (result.MillisecondOfDay);
            double workingMsec = (result.FractionalMilliseconds), temp = (0);

            workingMsec += ds;
            // check that workingMsod is not out of bounds
            if (workingMsec < 0.0)
            {
                // split workingMsec into integer and fraction parts
                // workingMsec gets the fraction and temp gets the integer
                string strD = System.Math.Abs(workingMsec).ToString();
                string[] strS = strD.Split('.');

                temp = Convert.ToDouble(strS[0]);
                if (workingMsec < 0) temp = -temp;

                //   temp = Math.Floor(workingMsec);
                workingMsec = 1 + (workingMsec - temp);
                // add the (negative) integer milliseconds to lds
                if (workingMsec == 1)
                {
                    workingMsec = 0;
                    lds += (long)(temp);
                }
                else
                    lds += (long)(temp) - 1;
            }
            else if (workingMsec >= 1.0)
            {
                // same as above
                temp = Math.Floor(workingMsec);
                workingMsec = workingMsec - temp;
                // add the integer milliseconds to lds
                lds += (long)(temp);
            }

            workingMsod += lds;
            // add any full days to ldd
            ldd += workingMsod / (TimeConsts.SEC_DAY * TimeConsts.FACTOR);

            // this will get us here:
            // -(SEC_DAY * FACTOR) < workingMsod < (SEC_DAY * FACTOR)
            workingMsod %= (TimeConsts.SEC_DAY * TimeConsts.FACTOR);

            // this will get us here: 0 <= workingMsod < (SEC_DAY * FACTOR)
            if (workingMsod < 0)
            {
                workingMsod += (TimeConsts.SEC_DAY * TimeConsts.FACTOR);
                --ldd;
            }

            workingJday += ldd;
            // check that workingJday is not out of bounds
            if (workingJday < TimeConsts.BEGIN_LIMIT_JDAY)
            {
                throw new Exception("DayTime underflow");

            }
            if (workingJday > TimeConsts.END_LIMIT_JDAY)
            {
                throw new Exception("DayTime overflow");
            }

            // everything's OK, so set the satData members
            //result.JulianDay = workingJday;
            //result.MillisecondsOfDay = workingMsod;
            //result.FractionalMilliseconds = workingMsec;

            //if (Math.Abs(result.FractionalMilliseconds - 1) / TimeConsts.FACTOR < result.Tolerance)
            //{
            //    // decrement mSec, except mSec must not be negative
            //    // alternately, set mSec = 0, but perhaps this contributes numerical noise?
            //    result.FractionalMilliseconds = (result.FractionalMilliseconds - 1 < 0 ? 0 : result.FractionalMilliseconds - 1);
            //    result.MillisecondsOfDay += 1;
            //}
            //if (result.MillisecondsOfDay >= TimeConsts.SEC_DAY * TimeConsts.FACTOR)
            //{
            //    result.MillisecondsOfDay -= TimeConsts.SEC_DAY * TimeConsts.FACTOR;
            //    result.JulianDay += 1;
            //}

            return result;
        }
    }
}