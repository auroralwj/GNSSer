using System;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using Gnsser.Times;
using Geo.Times;
using Geo.Coordinates;

#region 文档解释
// ////////////////////////////////////////////////////////////////////////
//
// NASA Two-Line Element Data format
//
// [Reference: Dr. TProduct.S. Kelso / www.celestrak.com]
//
// Two-line element satData consists of three lines in the following format:
//
// AAAAAAAAAAAAAAAAAAAAAAAA
// 1 NNNNNU NNNNNAAA NNNNN.NNNNNNNN +.NNNNNNNN +NNNNN-N +NNNNN-N N NNNNN
// 2 NNNNN NNN.NNNN NNN.NNNN NNNNNNN NNN.NNNN NNN.NNNN NN.NNNNNNNNNNNNNN
//  
// Line 0 is a twenty-four-character name.
// 
// Lines 1 and 2 are the standard Two-Line Orbital Element Set Format identical
// to that used by NORAD and NASA.  The format description is:
//      
//     Line 1
//     Column    Description
//     01-01     Line Number of Element Data
//     03-07     Satellite Number
//     10-11     International Designator (Last dayServices digits of launch year)
//     12-14     International Designator (Launch number of the year)
//     15-17     International Designator (Piece of launch)
//     19-20     Epoch Year (Last dayServices digits of year)
//     21-32     Epoch (Julian Day and fractional portion of the secondOfWeek)
//     34-43     First Time Derivative of the Mean Motion
//               or Ballistic Coefficient (Depending on ephemeris type)
//     45-52     Second Time Derivative of Mean Motion (decimal point assumed;
//               blank if N/A)
//     54-61     BSTAR drag term if GP4 general perturbation theory was used.
//               Otherwise, radiation pressure coefficient.  (Decimal point assumed)
//     63-63     Ephemeris type
//     65-68     Element number
//     69-69     Check Sum (Modulo 10)
//               (Letters, blanks, periods, plus signs = 0; minus signs = 1)
//     Line 2
//     Column    Description
//     01-01     Line Number of Element Data
//     03-07     Satellite Number
//     09-16     Inclination [Degrees]
//     18-25     Right Ascension of the Ascending Node [Degrees]
//     27-33     Eccentricity (decimal point assumed)
//     35-42     Argument of Perigee [Degrees]
//     44-51     Mean Anomaly [Degrees]
//     53-63     Mean Motion [Revs per secondOfWeek]
//     64-68     Revolution number at epoch [Revs]
//     69-69     Check Sum (Modulo 10)
//        
//     All other columns are blank or fixed.
//          
// Example:
//      
// NOAA 6
// 1 11416U          86 50.28438588 0.00000140           67960-4 0  5293
// 2 11416  98.5105  69.3305 0012788  63.2828 296.9658 14.24899292346978
#endregion

namespace Gnsser.Orbits
{
   /// <summary>
   /// This class encapsulates a single set of standard NORAD dayServices-line elements.
   /// </summary>
   public class TwoLineElement
   {
      #region Construction

      #region Column Offsets

      // Note: The column offsets are zero-based.

      // Name
      private const int TLE_LEN_LINE_DATA      = 69; private const int TLE_LEN_LINE_NAME      = 24;

      // Line 1
      private const int TLE1_COL_SATNUM        =  2; private const int TLE1_LEN_SATNUM        =  5;
      private const int TLE1_COL_INTLDESC_A    =  9; private const int TLE1_LEN_INTLDESC_A    =  2;
      private const int TLE1_COL_INTLDESC_B    = 11; private const int TLE1_LEN_INTLDESC_B    =  3;
      private const int TLE1_COL_INTLDESC_C    = 14; private const int TLE1_LEN_INTLDESC_C    =  3;
      private const int TLE1_COL_EPOCH_A       = 18; private const int TLE1_LEN_EPOCH_A       =  2;
      private const int TLE1_COL_EPOCH_B       = 20; private const int TLE1_LEN_EPOCH_B       = 12;
      private const int TLE1_COL_MEANMOTIONDT  = 33; private const int TLE1_LEN_MEANMOTIONDT  = 10;
      private const int TLE1_COL_MEANMOTIONDT2 = 44; private const int TLE1_LEN_MEANMOTIONDT2 =  8;
      private const int TLE1_COL_BSTAR         = 53; private const int TLE1_LEN_BSTAR         =  8;
      private const int TLE1_COL_EPHEMTYPE     = 62; private const int TLE1_LEN_EPHEMTYPE     =  1;
      private const int TLE1_COL_ELNUM         = 64; private const int TLE1_LEN_ELNUM         =  4;

      // Line 2
      private const int TLE2_COL_SATNUM        = 2;  private const int TLE2_LEN_SATNUM        =  5;
      private const int TLE2_COL_INCLINATION   = 8;  private const int TLE2_LEN_INCLINATION   =  8;
      private const int TLE2_COL_RAASCENDNODE  = 17; private const int TLE2_LEN_RAASCENDNODE  =  8;
      private const int TLE2_COL_ECCENTRICITY  = 26; private const int TLE2_LEN_ECCENTRICITY  =  7;
      private const int TLE2_COL_ARGPERIGEE    = 34; private const int TLE2_LEN_ARGPERIGEE    =  8;
      private const int TLE2_COL_MEANANOMALY   = 43; private const int TLE2_LEN_MEANANOMALY   =  8;
      private const int TLE2_COL_MEANMOTION    = 52; private const int TLE2_LEN_MEANMOTION    = 11;
      private const int TLE2_COL_REVATEPOCH    = 63; private const int TLE2_LEN_REVATEPOCH    =  5;

      #endregion
       /// <summary>
       /// 构造函数，以一个三行的两行根数字符串初始化。
       /// </summary>
      /// <param name="str">一个三行的两行根数字符串</param>
      public TwoLineElement(string str)
      {
          string[] lines = str.Split(new char[]{'\r', '\n'},  StringSplitOptions.RemoveEmptyEntries);
          m_Line0 = lines[0];
          m_Line1 = lines[1];
          m_Line2 = lines[2];

          Initialize();
      }


      // //////////////////////////////////////////////////////////////////////////
      public TwoLineElement(string strName, string strLine1, string strLine2)
      {
         m_Line0  = strName;
         m_Line1 = strLine1;
         m_Line2 = strLine2;

         Initialize();
      }

      // //////////////////////////////////////////////////////////////////////////
      public TwoLineElement(TwoLineElement tle) : 
         this(tle.Name, tle.Line1, tle.Line2)
      {
      }

      // //////////////////////////////////////////////////////////////////////////
      private void Initialize()
      {
         m_Field = new Dictionary<Field, string>();
         m_Cache = new Dictionary<int, double>();
   
         m_Field[Field.NoradNum] = m_Line1.Substring(TLE1_COL_SATNUM, TLE1_LEN_SATNUM);
         m_Field[Field.IntlDesc] = m_Line1.Substring(TLE1_COL_INTLDESC_A,
                                                        TLE1_LEN_INTLDESC_A +
                                                        TLE1_LEN_INTLDESC_B +   
                                                        TLE1_LEN_INTLDESC_C);   
         m_Field[Field.EpochYear] = m_Line1.Substring(TLE1_COL_EPOCH_A, TLE1_LEN_EPOCH_A);
         m_Field[Field.EpochDay] =  m_Line1.Substring(TLE1_COL_EPOCH_B, TLE1_LEN_EPOCH_B);
   
         if (m_Line1[TLE1_COL_MEANMOTIONDT] == '-')
         {
            // value is negative
            m_Field[Field.MeanMotionDt] = "-0";
         }
         else
         {
            m_Field[Field.MeanMotionDt] = "0";
         }
   
         m_Field[Field.MeanMotionDt] += m_Line1.Substring(TLE1_COL_MEANMOTIONDT + 1,
                                                             TLE1_LEN_MEANMOTIONDT);  
         // decimal point assumed; exponential notation
         m_Field[Field.MeanMotionDt2] = 
            ExpToDecimal(m_Line1.Substring(TLE1_COL_MEANMOTIONDT2,
                                              TLE1_LEN_MEANMOTIONDT2));

         // decimal point assumed; exponential notation
         m_Field[Field.BStarDrag] = 
            ExpToDecimal(m_Line1.Substring(TLE1_COL_BSTAR, TLE1_LEN_BSTAR));
         //TLE1_COL_EPHEMTYPE      
         //TLE1_LEN_EPHEMTYPE   

         m_Field[Field.SetNumber] = 
            m_Line1.Substring(TLE1_COL_ELNUM, TLE1_LEN_ELNUM).TrimStart();

         // TLE2_COL_SATNUM         
         // TLE2_LEN_SATNUM         
   
         m_Field[Field.Inclination] = 
            m_Line2.Substring(TLE2_COL_INCLINATION, TLE2_LEN_INCLINATION).TrimStart();
   
         m_Field[Field.Raan] = 
            m_Line2.Substring(TLE2_COL_RAASCENDNODE, TLE2_LEN_RAASCENDNODE).TrimStart();

         // Eccentricity: decimal point is assumed
         m_Field[Field.Eccentricity] = "0." + m_Line2.Substring(TLE2_COL_ECCENTRICITY,
            TLE2_LEN_ECCENTRICITY);
   
         m_Field[Field.ArgPerigee] = 
            m_Line2.Substring(TLE2_COL_ARGPERIGEE, TLE2_LEN_ARGPERIGEE).TrimStart();
   
         m_Field[Field.MeanAnomaly] = 
            m_Line2.Substring(TLE2_COL_MEANANOMALY, TLE2_LEN_MEANANOMALY).TrimStart();
   
         m_Field[Field.MeanMotion] = 
            m_Line2.Substring(TLE2_COL_MEANMOTION, TLE2_LEN_MEANMOTION).TrimStart();
   
         m_Field[Field.OrbitAtEpoch] = 
            m_Line2.Substring(TLE2_COL_REVATEPOCH, TLE2_LEN_REVATEPOCH).TrimStart();
      }

      #endregion

      #region 核心变量
      // Satellite name and dayServices satData lines
      private string m_Line0;
      private string m_Line1;
      private string m_Line2;

      // Converted fields, in Double.Parse()-able form
      private Dictionary<Field, string> m_Field;

      // Cache of field values in "double" format. 
      // Key   - integer
      // Value - cached value
      private Dictionary<int, double> m_Cache;
      #endregion

      #region Properties

      public string Name
      {
         get { return m_Line0; }
      }

      public string Line1
      {
         get { return m_Line1; }
      }

      public string Line2
      {
         get { return m_Line2; }
      }

      public string NoradNumber
      {
         get { return GetField(Field.NoradNum, false); }
      }

      public string Eccentricity
      {
         get { return GetField(Field.Eccentricity, false); }
      }

      public string Inclination
      {
         get { return GetField(Field.Inclination, true); }
      }

      public string Epoch
      {
         get
         {
            return string.Format(CultureInfo.InvariantCulture, "{0:00}{1:000.00000000}",
                                 GetField(Field.EpochYear), GetField(Field.EpochDay));
         }
      }

      public string IntlDescription
      {
         get { return GetField(Field.IntlDesc, false); }
      }

      public string SetNumber
      {
         get { return GetField(Field.SetNumber, false); }
      }
       /// <summary>
      /// 在轨圈数
       /// </summary>
      public string OrbitAtEpoch
      {
         get { return GetField(Field.OrbitAtEpoch, false); }
      }

      public string RAAscendingNode
      {
         get { return GetField(Field.Raan, true); }
      }

      public string ArgPerigee
      {
         get { return GetField(Field.ArgPerigee, true); }
      }

      public string MeanAnomaly
      {
         get { return GetField(Field.MeanAnomaly, true); }
      }

      public string MeanMotion
      {
         get { return GetField(Field.MeanMotion, true); }
      }

      public string MeanMotionDt
      {
         get { return GetField(Field.MeanMotionDt, false); }
      }

      public string MeanMotionDt2
      {
         get { return GetField(Field.MeanMotionDt2, false); }
      }

      public string BStarDrag
      {
         get { return GetField(Field.BStarDrag, false); }
      }

      public Julian EpochJulian
      {
         get
         {
            int epochYear = (int)GetField(TwoLineElement.Field.EpochYear);
            double epochDay = GetField(TwoLineElement.Field.EpochDay);

            if (epochYear < 57)
            {
               epochYear += 2000;
            }
            else
            {
               epochYear += 1900;
            }

            return new Julian(epochYear, epochDay);
         }
      }

      #endregion

      #region GetField

      /// <summary>
      /// Returns the requested TLE satData field.
      /// </summary>
      /// <param name="fld">The field to return.</param>
      /// <returns>The requested field, in native form.</returns>
      public double GetField(Field fld)
      {
         return GetField(fld, AngleUnit.Degree );
      }

      /// <summary>
      /// Returns the requested TLE satData field as a type double.
      /// </summary>
      /// <remarks>
      /// The numeric return values are cached; requesting the same field 
      /// repeatedly incurs minimal overhead.
      /// </remarks>
      /// <param name="fld">The TLE field to retrieve.</param>
      /// <param name="units">Specifies the units desired.</param>
      /// <returns>
      /// The requested field's value, converted to the correct units if necessary.
      /// </returns>
      public double GetField(Field fld, AngleUnit units)
      {
         // Return cache contents if it exists, else populate cache.
         int key = Key(units, fld);

         if (m_Cache.ContainsKey(key))
         {
            // return cached value
            return (double)m_Cache[key];
         }
         else
         {
            // Value not in cache; add it
            double valNative = Double.Parse(m_Field[fld].ToString(), CultureInfo.InvariantCulture);
            double valConv   = ConvertUnits(valNative, fld, units); 
            m_Cache[key]     = valConv;

            return valConv;
         }
      }

      /// <summary>
      /// Returns the requested TLE satData field in native form as a text string.
      /// </summary>
      /// <param name="fld">The TLE field to retrieve.</param>
      /// <param name="appendUnits">If true, the native units are appended to 
      /// the end of the returned string.</param>
      /// <returns>The requested field as a string.</returns>
      public string GetField(Field fld, bool appendUnits)
      {
         string str = m_Field[fld].ToString();
   
         if (appendUnits)
         {
            str += GetUnits(fld);
         }
      
         return str.Trim();
      }

      #endregion

      #region Utility

      // ///////////////////////////////////////////////////////////////////////////
      // Generates a keyPrev for the TLE field cache
      private static int Key(AngleUnit u, Field f)
      {
         return ((int)u * 100) + (int)f;
      }

      /// <summary>
      /// Converts the given TLE field to the requested units.
      /// </summary>
      /// <param name="valNative">Value to funcKeyToDouble (native units).</param>
      /// <param name="fld">Field ID of the value being converted.</param>
      /// <param name="units">Units to funcKeyToDouble to.</param>
      /// <returns>The converted value.</returns>
      protected static double ConvertUnits(double valNative,
                                           Field fld,
                                           AngleUnit  units)
      {
         if (fld == Field.Inclination  ||
             fld == Field.Raan         ||
             fld == Field.ArgPerigee   ||
             fld == Field.MeanAnomaly)
         {
            // The native TLE format is degrees
            if (units == AngleUnit.Radian)
            {
                return valNative * AngularConvert.DegToRadMultiplier;// Globals.ToRadians(valNative);
            }
         }

         // unconverted native format
         return valNative;
      }

      // ///////////////////////////////////////////////////////////////////////////
      protected static string GetUnits(Field fld) 
      {
         const string strDegrees    = " degrees";
         const string strRevsPerDay = " revs / day";

         switch (fld)
         {
            case Field.Inclination:
            case Field.Raan:
            case Field.ArgPerigee:
            case Field.MeanAnomaly:
               return strDegrees;

            case Field.MeanMotion:
               return strRevsPerDay;

            default:
               return string.Empty;   
         }
      }

      // //////////////////////////////////////////////////////////////////////////
      // Converts TLE-style exponential notation of the form [ |+|-]00000[ |+|-]0
      // to decimal notation. Assumes implied decimal point to the left of the prevObj
      // number in the string, time.e., 
      //       " 12345-3" =  0.00012345
      //       "-23429-5" = -0.0000023429   
      //       " 40436+1" =  4.0436
      // Also assumes that lack of a sign character implies a positive value, time.e.,
      //       " 00000 0" =  0.00000
      //       " 31415 1" =  3.1415
      protected static string ExpToDecimal(string str)
      {
         const int COL_SIGN     = 0;
         const int LEN_SIGN     = 1;

         const int COL_MANTISSA = 1;
         const int LEN_MANTISSA = 5;

         const int COL_EXPONENT = 6;
         const int LEN_EXPONENT = 2;

         string sign     = str.Substring(COL_SIGN,     LEN_SIGN);
         string mantissa = str.Substring(COL_MANTISSA, LEN_MANTISSA);
         string exponent = str.Substring(COL_EXPONENT, LEN_EXPONENT).TrimStart();

         double val = Double.Parse(sign +"0." + mantissa + "e" + exponent, CultureInfo.InvariantCulture);

         int sigDigits = mantissa.Length + Math.Abs(int.Parse(exponent, CultureInfo.InvariantCulture));

         return val.ToString("F" + sigDigits, CultureInfo.InvariantCulture);
      }

      /// <summary>
      /// Determines if a given string has the expected format of a single 
      /// line of TLE satData.
      /// </summary>
      /// <param name="str">The input string.</param>
      /// <param name="line">The line ID of the input string.</param>
      /// <returns>True if the input string has the format of 
      /// the given line ID.</returns>
      /// <remarks>
      /// A valid satellite name is less than or equal to TLE_LEN_LINE_NAME
      ///      characters;
      /// A valid satData line must:
      ///      Have as the prevObj character the line number
      ///      Have as the second character a blank
      ///      Be TLE_LEN_LINE_DATA characters long
      /// </remarks>
      public static bool IsValidFormat(string str, Line line)
      {
         str = str.Trim();

         int nLen = str.Length;

         if (line == Line.Zero)
         {
            // Satellite name
            return str.Length <= TLE_LEN_LINE_NAME;
         }
         else
         {
            // Data line
            return (nLen == TLE_LEN_LINE_DATA) &&
                   ((str[0] - '0') == (int)line) &&
                   (str[1] == ' ');
         }
      }

      // //////////////////////////////////////////////////////////////////////////
      // Calculate the check sum for a given line of TLE satData, the last character
      // of which is the current checksum. (Although there is no check here,
      // the current checksum should match the one calculated.)
      // The checksum algorithm: 
      //    Each number in the satData line is summed, modulo 10.
      //    Non-numeric characters are zero, except minus signs, which are 1.
      //
      static int CheckSum(string str)
      {
         // The length is "- 1" because the current (existing) checksum 
         // character is not included.
         int len  = str.Length - 1;
         int xsum = 0;
   
         for (int i = 0; i < len; i++)
         {
            char ch = str[i];

            if (Char.IsDigit(ch))
            {
               xsum += (ch - '0');
            }
            else if (ch == '-')
            {
               xsum++;
            }
         }
   
         return (xsum % 10);
      }

      #endregion

      public enum Line
      {
          Zero,
          One,
          Two
      };

      /// <summary>
      /// 两行根数的域。
      /// </summary>
      public enum Field
      {
          NoradNum,
          IntlDesc,
          SetNumber,     // TLE set number
          EpochYear,     // Epoch: Last dayServices digits of year
          EpochDay,      // Epoch: Fractional Julian Day of year
          OrbitAtEpoch,  // Orbit at epoch
          Inclination,   // Inclination
          Raan,          // R.A. ascending node
          Eccentricity,  // Eccentricity
          ArgPerigee,    // Argument of perigee
          MeanAnomaly,   // Mean anomaly
          MeanMotion,    // Mean motion
          MeanMotionDt,  // First time derivative of mean motion
          MeanMotionDt2, // Second time derivative of mean motion
          BStarDrag      // BSTAR Drag
      }
      public override string ToString()
      {
          StringBuilder sb = new StringBuilder();
          sb.AppendLine("                                          名称 Name : " + this.Name);
          sb.AppendLine("                                  卫星编号 NoradNum : " + this.NoradNumber);
          sb.AppendLine("国际标志符 两位年+年发射数+次发射数 IntlDescription : " + this.IntlDescription);
          sb.AppendLine("                    历元 年份后两位数.年积日  Epoch : " + this.Epoch);
          sb.AppendLine("                       历元 年份后两位数  EpochYear : " + this.GetField(Field.EpochYear));
          sb.AppendLine("                               历元 年积日 EpochDay : " + this.GetField(Field.EpochDay));
          sb.AppendLine("                平均运动的一阶时间导数 MeanMotionDt : " + this.MeanMotionDt);
          sb.AppendLine("               平均运动的二阶时间导数 MeanMotionDt2 : " + this.MeanMotionDt2);
          sb.AppendLine("                            BSTAR阻力系数 BStarDrag : " + this.BStarDrag);
          sb.AppendLine("                                 星历编号 SetNumber : " + this.SetNumber);
          sb.AppendLine("                                  卫星编号 NoradNum : " + this.NoradNumber);
          sb.AppendLine("                             轨道的交角 Inclination : " + this.Inclination);
          sb.AppendLine("                         升交点赤经 RAAscendingNode : " + this.RAAscendingNode);
          sb.AppendLine("                                离心率 Eccentricity : " + this.Eccentricity);
          sb.AppendLine("                              近地点角距 ArgPerigee : " + this.ArgPerigee);
          sb.AppendLine("                               平近点角 MeanAnomaly : " + this.MeanAnomaly);
          sb.AppendLine("                            每日绕行圈数 MeanMotion : " + this.MeanMotion);
          sb.AppendLine("                              在轨圈数 OrbitAtEpoch : " + this.OrbitAtEpoch);

          return sb.ToString();
      } 
   }
}
