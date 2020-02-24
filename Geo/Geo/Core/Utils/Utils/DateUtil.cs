
//2017.06.18, czs, funcKeyToDouble from c++ in hongqing, Date
//2017.06.26, czs, edit in hongqing, format and refactor codes
//2017.07.19, czs, edit in hongqing, 重构为MjdData

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Geo.Coordinates;

using Geo.Algorithm; using Geo.Utils; 

namespace Geo.Utils
{ 
    /// <summary>
    /// Time and date computation
    /// Auxiliary class for date and time output
    /// 日期辅助类
    /// </summary>
 
    public class DateUtil
    {

        /// <summary>
        /// 转换为日期时间字符串
        /// </summary>
        /// <param name="mjd"></param>
        /// <returns></returns>
        public static string MjdToDateTimeString(double mjd)
        {
            // Constants

            const double mSecs = 86400.0e3;   // Milliseconds per day
            const double eps = 0.1 / mSecs;    // 0.1 msec

            // Variables
            double MjdRound;
            int Year = 0, Month = 0, Day = 0;
            int H = 0, M = 0;
            double S = 0;

            // Round to 1 msec

            MjdRound = (Math.Floor(mSecs * mjd + 0.5) / mSecs) + eps;

            MjdToDate(MjdRound, out Year, out  Month, out  Day, out  H, out M, out  S);
            DateTime dateTime = new DateTime(Year, Month, Day, H, M, (int)S, (int)(MathUtil.Fraction(S) * 1000 ));

            var info = dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"); 
            //Year.ToString("0000") + "-" + Month.ToString("00") + "-" + Day.ToString("00") + " "
            //    + H.ToString("00") + ":" + M.ToString("00") + ":" + S.ToString("00.000");
            return (info);
        }

        #region  静态工具方法

        /// <summary>
        ///  Modified Julian Date from calendar date and time
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="Month"></param>
        /// <param name="Day"></param>
        /// <param name="Hour"></param>
        /// <param name="Min"></param>
        /// <param name="Sec"></param>
        /// <returns> Modified Julian Date</returns>
        public static double DateToMjd(int Year, int Month, int Day, int Hour = 0, int Min = 0, double Sec = 0.0)
        {
            // Variables
            long MjdMidnight = 0;
            double FracOfDay;
            int b;

            if (Month <= 2) { Month += 12; --Year; }

            if ((10000L * Year + 100L * Month + Day) <= 15821004L)
                b = -2 + ((Year + 4716) / 4) - 1179;     // Julian calendar 
            else
                b = (Year / 400) - (Year / 100) + (Year / 4);  // Gregorian calendar 

            MjdMidnight = 365L * Year - 679004L + b + (int)(30.6001 * (Month + 1)) + Day;
            FracOfDay = (Hour + Min / 60.0 + Sec / 3600.0) / 24.0;

            return MjdMidnight + FracOfDay;
        }


        /// <summary>
        /// Calendar date and time from Modified Julian Date
        /// </summary>
        /// <param name="Mjd"> Modified Julian Date</param>
        /// <param name="Year"></param>
        /// <param name="Month"></param>
        /// <param name="Day"></param>
        /// <param name="Hour"></param>
        /// <param name="Min"></param>
        /// <param name="Sec"></param>
        public static void MjdToDate(double Mjd,
                     out int Year, out  int Month, out  int Day,
                     out  int Hour, out  int Min, out  double Sec)
        {
            // Variables
            long a, b, c, d, e, f;
            double Hours, x;

            // Convert Julian day number to calendar date
            a = (long)(Mjd + 2400001.0);

            if (a < 2299161)
            {  // Julian calendar
                b = 0;
                c = a + 1524;
            }
            else
            {                // Gregorian calendar
                b = (long)((a - 1867216.25) / 36524.25);
                c = a + b - (b / 4) + 1525;
            }

            d = (long)((c - 122.1) / 365.25);
            e = 365 * d + d / 4;
            f = (long)((c - e) / 30.6001);

            Day = (int)c - (int)e - (int)(30.6001 * f);
            Month = (int)f - 1 - (int)(12 * (f / 14));
            Year = (int)d - 4715 - (int)((7 + Month) / 10);

            Hours = 24.0 * (Mjd - Math.Floor(Mjd));

            Hour = (int)(Hours);
            x = (Hours - Hour) * 60.0; Min = (int)(x); Sec = (x - Min) * 60.0;
        }
        #endregion
    }
}