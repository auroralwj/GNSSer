using System;
using System.Text;
using Gnsser.Times;
using Geo.Coordinates;
using Geo.Utils;
using Geo.Times;

namespace Gnsser.Orbits
{
   /// <summary>
   /// This class accepts a single satellite's NORAD dayServices-line element
   /// set and provides information regarding the satellite's orbit 
   /// such as period, axis length, ECI coordinates, velocity, etc.
   /// </summary>
   public class Orbit
   {
      #region Construction

      /// <summary>
      /// Standard constructor.
      /// </summary>
      /// <param name="tle">Two-line element orbital parameters.</param>
      public Orbit(TwoLineElement tle)
      {
         TwoLineElement     = tle;
         Epoch = TwoLineElement.EpochJulian;

         m_Inclination   = GetRad(TwoLineElement.Field.Inclination);
         m_Eccentricity  = TwoLineElement.GetField(TwoLineElement.Field.Eccentricity);
         m_RAAN          = GetRad(TwoLineElement.Field.Raan);              
         m_ArgPerigee    = GetRad(TwoLineElement.Field.ArgPerigee);        
         m_BStar         = TwoLineElement.GetField(TwoLineElement.Field.BStarDrag);   
         m_Drag          = TwoLineElement.GetField(TwoLineElement.Field.MeanMotionDt);
         m_MeanAnomaly   = GetRad(TwoLineElement.Field.MeanAnomaly);
         m_TleMeanMotion = TwoLineElement.GetField(TwoLineElement.Field.MeanMotion);  

         // Recover the original mean motion and semimajor axis from the
         // input elements.
         double mm     = TleMeanMotion;
         double rpmin  = mm * OrbitConsts.TwoPi / OrbitConsts.MinPerDay;   // rads per second

         double a1     = Math.Pow(OrbitConsts.Xke / rpmin, 2.0 / 3.0);
         double e      = Eccentricity;
         double i      = Inclination;
         double temp = (1.5 * OrbitConsts.Ck2 * (3.0 * GeoMath.Sqr(Math.Cos(i)) - 1.0) / 
                         Math.Pow(1.0 - e * e, 1.5));   
         double delta1 = temp / (a1 * a1);
         double a0     = a1 * 
                        (1.0 - delta1 * 
                        ((1.0 / 3.0) + delta1 * 
                        (1.0 + 134.0 / 81.0 * delta1)));

         double delta0 = temp / (a0 * a0);

         m_rmMeanMotionRec    = rpmin / (1.0 + delta0);
         m_aeAxisSemiMajorRec = a0 / (1.0 - delta0);
         m_aeAxisSemiMinorRec = m_aeAxisSemiMajorRec * Math.Sqrt(1.0 - (e * e));
         m_kmPerigeeRec       = OrbitConsts.RadiusOfEquator * (m_aeAxisSemiMajorRec * (1.0 - e) - OrbitConsts.Ae);
         m_kmApogeeRec        = OrbitConsts.RadiusOfEquator * (m_aeAxisSemiMajorRec * (1.0 + e) - OrbitConsts.Ae);

         //针对周期的不同，选择不同的模型。
         if (Period.TotalMinutes >= 225.0)
         {
            // SDP4 - period >= 225 minutes.
            NoradModel = new NoradSDP4(this);
         }
         else
         {
            // SGP4 - period < 225 minutes
            NoradModel = new NoradSGP4(this);
         }
      }

      #endregion

      #region 参数
      // Caching variables
      private TimeSpan m_Period = new TimeSpan(0, 0, 0, -1);

      // TLE caching variables
      private double m_Inclination;
      private double m_Eccentricity;
      private double m_RAAN;
      private double m_ArgPerigee;
      private double m_BStar;
      private double m_Drag;
      private double m_MeanAnomaly;
      private double m_TleMeanMotion;

      // Caching variables recovered from the input TLE elements
      private double m_aeAxisSemiMajorRec;  // semimajor axis, in AE units
      private double m_aeAxisSemiMinorRec;  // semiminor axis, in AE units
      private double m_rmMeanMotionRec;     // radians per second
      private double m_kmPerigeeRec;        // perigee, in km
      private double m_kmApogeeRec;         // apogee, in km
      #endregion

      #region Properties
      /// <summary>
       /// 两行根数
       /// </summary>
      private TwoLineElement   TwoLineElement      { get; set; }
       /// <summary>
       /// 第一行根数
       /// </summary>
      public string Line1OfElement { get { return TwoLineElement.Line1; }}
       /// <summary>
       /// 第二行根数
       /// </summary>
      public string Line2OfElement { get { return TwoLineElement.Line2; }}

      public Julian   Epoch     { get; private set; }
      public DateTime EpochTime { get { return Epoch.ToTime(); }}

      private NoradBase NoradModel { get; set; }

      // "Recovered" from the input elements
      public double SemiMajor    { get { return m_aeAxisSemiMajorRec; }}
      public double SemiMinor    { get { return m_aeAxisSemiMinorRec; }}
      public double MeanMotion   { get { return m_rmMeanMotionRec;    }}
      public double Major        { get { return 2.0 * SemiMajor;      }}
      public double Minor        { get { return 2.0 * SemiMinor;      }}
      public double Perigee      { get { return m_kmPerigeeRec;       }}
      public double Apogee       { get { return m_kmApogeeRec;        }}

      public double Inclination    { get { return m_Inclination;   }}
      public double Eccentricity   { get { return m_Eccentricity;  }}
      public double RAAN           { get { return m_RAAN;          }}
      public double ArgPerigee     { get { return m_ArgPerigee;    }}
      public double BStar          { get { return m_BStar;         }}
      public double Drag           { get { return m_Drag;          }}
      public double MeanAnomaly    { get { return m_MeanAnomaly;   }}
      private double TleMeanMotion { get { return m_TleMeanMotion; }}

      public string SatNoradId    { get { return TwoLineElement.NoradNumber; }}
      public string SatName       { get { return TwoLineElement.Name;        }}
      public string SatNameLong   { get { return SatName + " #" + SatNoradId; }}

      public override string ToString()
      {
          StringBuilder sb = new StringBuilder();
          sb.AppendLine("             SatName : " + SatName);
          sb.AppendLine("          SatNoradId : " + SatNoradId);
          sb.AppendLine("               Epoch : " + Epoch);
          sb.AppendLine("           EpochTime : " + EpochTime);
          sb.AppendLine("           SemiMajor : " + SemiMajor);
          sb.AppendLine("           SemiMinor : " + SemiMinor);
          sb.AppendLine("       TleMeanMotion : " + TleMeanMotion);
          sb.AppendLine("          MeanMotion : " + MeanMotion);
          sb.AppendLine("             Perigee : " + Perigee);
          sb.AppendLine("              Apogee : " + Apogee);
          sb.AppendLine("         Inclination : " + Inclination);
          sb.AppendLine("        Eccentricity : " + Eccentricity);
          sb.AppendLine("                RAAN : " + RAAN);
          sb.AppendLine("          ArgPerigee : " + ArgPerigee);
          sb.AppendLine("               BStar : " + BStar);
          sb.AppendLine("                Drag : " + Drag);
          sb.AppendLine("         MeanAnomaly : " + MeanAnomaly);

          return sb.ToString();
      }


      public TimeSpan Period 
      {
         get 
         { 
            if (m_Period.TotalSeconds < 0.0)
            {
               // Calculate the period using the recovered mean motion.
               if (MeanMotion == 0)
               {
                  m_Period = new TimeSpan(0, 0, 0);
               }
               else
               {
                  double secs  = (OrbitConsts.TwoPi / MeanMotion) * 60.0;
                  int    msecs = (int)((secs - (int)secs) * 1000);

                  m_Period = new TimeSpan(0, 0, 0, (int)secs, msecs);
               }
            }

            return m_Period;
         }
      }

      #endregion

      #region Get Position

      /// <summary>
      /// Calculate satellite ECI position/velocity for a given time.
      /// </summary>
      /// <param name="mpe">Target time, in minutes past the TLE epoch.</param>
      /// <returns>Kilometer-based position/velocity ECI coordinates.</returns>
      public TimedMotionState PositionEci(double mpe)
      {
         TimedMotionState eci = NoradModel.GetPosition(mpe);

         // Convert ECI vector units from AU to kilometers
         double radiusAe = OrbitConsts.RadiusOfEquator / OrbitConsts.Ae;

         eci.ScalePosVector(radiusAe);                               // km
         eci.ScaleVelVector(radiusAe * (OrbitConsts.MinPerDay / 86400.0)); // km/sec

         return eci;
      }

      /// <summary>
      /// Calculate ECI position/velocity for a given time.
      /// </summary>
      /// <param name="utc">Target time (UTC).</param>
      /// <returns>Kilometer-based position/velocity ECI coordinates.</returns>
      public TimedMotionState PositionEci(DateTime utc)
      {
         return PositionEci(TPlusEpoch(utc).TotalMinutes);
      }

      /// <summary>
      /// Calculate satellite ECI position/velocity for a given time.
      /// </summary>
      /// <param name="mpe">Target time, in minutes past the TLE epoch.</param>
      /// <returns>Kilometer-based position/velocity ECI coordinates.</returns>
      [Obsolete("Use PositionEci()")]
      public TimedMotionState GetPosition(double mpe)
      {
         return PositionEci(mpe);
      }

      /// <summary>
      /// Calculate ECI position/velocity for a given time.
      /// </summary>
      /// <param name="utc">Target time (UTC).</param>
      /// <returns>Kilometer-based position/velocity ECI coordinates.</returns>
      [Obsolete("Use PositionEci()")]
      public TimedMotionState GetPosition(DateTime utc)
      {
         return PositionEci(TPlusEpoch(utc).TotalMinutes);
      }

      #endregion

      // ///////////////////////////////////////////////////////////////////////////
      // Returns elapsed time from epoch to given time.
      // Note: "Predicted" TLEs can have epochs in the future.
      public TimeSpan TPlusEpoch(DateTime utc) 
      {
         return (utc - EpochTime);
      } 
      // Returns elapsed time from epoch to current time.
      // Note: "Predicted" TLEs can have epochs in the future.
      public TimeSpan TPlusEpoch()
      {
         return TPlusEpoch(DateTime.UtcNow);
      }

      #region Utility
        
      protected double GetRad(TwoLineElement.Field fld) 
      { 
         return TwoLineElement.GetField(fld, AngleUnit.Radian); 
      } 
      protected double GetDeg(TwoLineElement.Field fld) 
      { 
         return TwoLineElement.GetField(fld, AngleUnit.Degree); 
      }

      #endregion
   }
}
