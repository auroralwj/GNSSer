using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Times; 
namespace Gnsser
{
    public class Xvt
    {
        public Xvt()
        {
            x = new double[3];
            v = new double[3];
        }

        public double relcorr;
        public double clkbias;
        public double clkdrift;
        public double[] x;
        public double[] v;
    }

    /// <summary>
    /// Rtklib 的轨道计算方法。
    /// </summary>
   public class RtkLibOrbitUtil
    {

        public class eph_t
        {
            public eph_t() { }
            public eph_t(EphemerisParam eph)
            {
                this.A = eph.SqrtA * eph.SqrtA;
                this.toe = eph.Toe;
               // this.sat = eph.sat;
                this.M0 = eph.MeanAnomaly;
                this.deln = eph.DeltaN;
                this.e = eph.Eccentricity;
                this.omg = eph.ArgumentOfPerigee;
                this.cuc = eph.Cuc;
                this.crs = eph.Crs;
                this.cis = eph.Cis;
                this.cus = eph.Cus;
                this.cic = eph.Cis;
                this.crc = eph.Crc;
                this.OMG0 = eph.LongOfAscension;
                this.OMGd = eph.OmegaDot;
                this.toes = eph.Toe;
                this.toc = eph.Toe;//??;
                this.f0 = eph.ClockBias;
                this.f1 = eph.ClockDrift;
                this.f2 = eph.DriftRate;
                this.sva = eph.SVAccuracy;
                this.i0 = eph.Inclination;
                this.idot = eph.EyeDot;
            }


            public double A;
            public double toe;
         //   public double sat;
            public double M0;
            public double deln;
            public double e;
            public double omg;
            public double cuc;
            public double crs;
            public double cis;
            public double cus;
            public double cic;
            public double crc;
            public double OMG0;
            public double OMGd;
            public double toes;
            public double toc;
            public double f0;
            public double f1;
            public double f2;
            public double sva;
            public double i0;
            public double idot;
        } 

        const int SYS_GAL = 0, SYS_CMP = 1;


        static double PI = 3.1415926535897932;  /* pi */
        static double D2R = (PI / 180.0);     /* deg to rad */
        static double R2D = (180.0 / PI);     /* rad to deg */
        static double CLIGHT = 299792458;   /* speed of light (m/s) */
        public static double SC2RAD = 3.1415926535898;   /* semi-circle to radian (IS-GPS) */
        public static double AU = 149597870691.0;   /* 1 AU (m) */
        public static double AS2R = (D2R / 3600.0);   /* arc sec to radian */

        public static double OMGE = 7.2921151467E-5; /* earth angular velocity (IS-GPS) (rad/s) */

        public static double RE_WGS84 = 6378137.0;    /* earth semimajor axis (WGS84) (m) */
        public static double FE_WGS84 = (1.0 / 298.257223563); /* earth flattening (WGS84) */

        public static double HION = 350000.0;          /* ionosphere height (m) */


        public static double RE_GLO = 6378136.0;     /* radius of earth (m)            ref [2] */
        public static double MU_GPS = 3.9860050E14;   /* gravitational constant         ref [1] */
        public static double MU_GLO = 3.9860044E14;   /* gravitational constant         ref [2] */
        public static double MU_GAL = 3.986004418E14; /* earth gravitational constant   ref [7] */
        public static double MU_CMP = 3.986004418E14; /* earth gravitational constant   ref [9] */
        public static double J2_GLO = 1.0826257E-3;    /* 2nd zonal harmonic of geopot   ref [2] */

        public static double OMGE_GLO = 7.292115E-5;   /* earth angular velocity (rad/s) ref [2] */
        public static double OMGE_GAL = 7.2921151467E-5; /* earth angular velocity (rad/s) ref [7] */
        public static double OMGE_CMP = 7.292115E-5;  /* earth angular velocity (rad/s) ref [9] */

        public static double SIN_5 = -0.0871557427476582;/* sin(-5.0 deg) */
        public static double COS_5 = 0.9961946980917456; /* cos(-5.0 deg) */

        public static double ERREPH_GLO = 5.0;       /* error of glonass ephemeris (m) */
        public static double TSTEP = 60.0;       /* integration step glonass ephemeris (s) */
        public static double RTOL_KEPLER = 1E-14;       /* relative tolerance for Kepler equation */

        public static double DEFURASSR = 0.15;        /* default accurary of ssr corr (m) */
        public static double MAXECORSSR = 10.0;        /* max orbit correction of ssr (m) */
        public static double MAXCCORSSR = (1E-6 * CLIGHT); /* max clock correction of ssr (m) */
        public static double MAXAGESSR = 70.0;      /* max age of ssr orbit and clock (s) */
        public static double MAXAGESSR_HRCLK = 10.0;   /* max age of ssr high-rate clock (s) */
        public static double STD_BRDCCLK = 30.0;      /* error of broadcast clock (m) */

        public static XYZ GetPos(double secOfWeek, EphemerisParam param)
        {
            double  [] rs = new double [3];
            double dts = 0, var = 0;
            eph2pos(secOfWeek, new eph_t(param), rs, ref dts, ref var, param.Prn);
            return XYZ.Parse(rs);
        }


        /* broadcast ephemeris to satellite position and clock bias --------------------
        * compute satellite position and clock bias with broadcast ephemeris (gps,
        * galileo, qzss)
        * args   : gtime_t time     I   time (gpst)
        *          eph_t *eph       I   broadcast ephemeris
        *          double *rs       O   satellite position (ecef) {x,y,z} (m)
        *          double *dts      O   satellite clock bias (s)
        *          double *var      O   satellite position and clock variance (m^2)
        * return : none
        * notes  : see ref [1],[7],[8]
        *          satellite clock includes relativity correction without obsCode bias
        *          (tgd or bgd)
        *-----------------------------------------------------------------------------*/
     public static void eph2pos(double secOfWeek, eph_t eph, double[] rs, ref double dts,
                        ref   double var, SatelliteNumber PRN )
        {
            SatelliteType satelliteType = PRN.SatelliteType;
            double tk, M, E, Ek, sinE, cosE, u, r, i, O, sin2u, cos2u, x, y, sinO, cosO, cosi, mu, omge;
            double xg, yg, zg, sino, coso;
            int n, sys, prn = PRN.PRN;

            // trace(4,"eph2pos : time=%s sat=%2d\n",time_str(time,3),eph.sat);

            if (eph.A <= 0.0)
            {
                rs[0] = rs[1] = rs[2] = dts = var = 0.0;
                return;
            }
            //tk = timediff(time, eph.toe);

            tk = Time.GetDifferSecondOfWeek(secOfWeek, eph.toe);
            // switch ((sys = satsys(eph.sat, satelliteType)))
            //  {
            //   case SYS_GAL: mu = MU_GAL; omge = OMGE_GAL; break;
            //    case SYS_CMP: mu = MU_CMP; omge = OMGE_CMP; break;
            //  default: mu = MU_GPS; omge = OMGE; break;
            // }
            switch (satelliteType)//不同系统对应不同的基准
            {
                case SatelliteType.E: mu = MU_GAL; omge = OMGE_GAL; break;
                case SatelliteType.C: mu = MU_CMP; omge = OMGE_CMP; break;
                default: mu = MU_GPS; omge = OMGE; break;
            }

            M = eph.M0 + (sqrt(mu / (eph.A * eph.A * eph.A)) + eph.deln) * tk;

            for (n = 0, E = M, Ek = 0.0; fabs(E - Ek) > RTOL_KEPLER; n++)
            {
                Ek = E;
                E -= (E - eph.e * sin(E) - M) / (1.0 - eph.e * cos(E));
            }

            sinE = sin(E);
            cosE = cos(E);

            // trace(4,"kepler: sat=%2d e=%8.5f n=%2d del=%10.3e\n",eph.sat,eph.e,n,E-Ek);
            u = atan2(sqrt(1.0 - eph.e * eph.e) * sinE, cosE - eph.e) + eph.omg;

            sin2u = sin(2.0 * u);
            cos2u = cos(2.0 * u);

            r = eph.A * (1.0 - eph.e * cosE);
            i = eph.i0 + eph.idot * tk;

            u += eph.cus * sin2u + eph.cuc * cos2u;
            r += eph.crs * sin2u + eph.crc * cos2u;
            i += eph.cis * sin2u + eph.cic * cos2u;
            x = r * cos(u);
            y = r * sin(u);
            cosi = cos(i);

            /* beidou geo satellite (ref [9]) */
            if (satelliteType == SatelliteType.C && prn <= 5)
            {
                O = eph.OMG0 + eph.OMGd * tk - omge * eph.toes;
                sinO = sin(O);
                cosO = cos(O);

                xg = x * cosO - y * cosi * sinO;
                yg = x * sinO + y * cosi * cosO;
                zg = y * sin(i);

                sino = sin(omge * tk); coso = cos(omge * tk);
                rs[0] = xg * coso + yg * sino * COS_5 + zg * sino * SIN_5;
                rs[1] = -xg * sino + yg * coso * COS_5 + zg * coso * SIN_5;
                rs[2] = -yg * SIN_5 + zg * COS_5;
            }
            else
            {


                O = eph.OMG0 + (eph.OMGd - omge) * tk - omge * eph.toes;
                sinO = sin(O);
                cosO = cos(O);
                rs[0] = x * cosO - y * cosi * sinO;
                rs[1] = x * sinO + y * cosi * cosO;
                rs[2] = y * sin(i);
            }
            //  tk = timediff(time, eph.toc);
            tk = Time.GetDifferSecondOfWeek(secOfWeek, eph.toc);
            dts = eph.f0 + eph.f1 * tk + eph.f2 * tk * tk;

            /* relativity correction */
            dts -= 2.0 * sqrt(mu * eph.A) * eph.e * sinE / SQR(CLIGHT);

            /* position and clock error variance */
            var = var_uraeph(eph.sva);
        }

     public static double SQR(double CLIGHT)
        {
            return CLIGHT * CLIGHT;
        }

        public static double var_uraeph(double p)
        {
          //  throw new NotImplementedException();
            return p;
        }

        public static double atan2(double p1, double p2)
        {
            return Math.Atan2(p1, p2);
        }

        public static double cos(double E)
        {
            return Math.Cos(E);
        }

        public static double sin(double E)
        {
            return Math.Sin(E);
        }

        public static double sqrt(double p)
        {
            return Math.Sqrt(p);
        }

        public static double fabs(double p)
        {
            return Math.Abs(p);
        }
    }
}
