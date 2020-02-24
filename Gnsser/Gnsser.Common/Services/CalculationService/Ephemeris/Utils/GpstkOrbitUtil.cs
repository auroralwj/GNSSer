using System;
using Gnsser.Times;
using System.Collections.Generic;
using Geo.Times; 
using Geo;
using System.Text;
using System.IO;
using Geo.Coordinates; 

namespace Gnsser
{
    /// <summary>
    /// 轨道计算方法。
    /// 参考自Gpstk的算法.
    /// </summary>
    public class GpstkOrbitUtil
    {
        public GpstkOrbitUtil(EphemerisParam eph)
        {
            this.af0 = eph.ClockBias;
            this.af1 = eph.ClockDrift;
            this.af2 = eph.DriftRate;
            this.ctToc = eph.Toe;//
            this.Cic = eph.Cic;
            this.Crc = eph.Crc;
            this.Crs = eph.Crs;
            this.OMEGAdot = eph.OmegaDot;
            this.e0 = eph.Eccentricity;//
            this.Cuc = eph.Cuc;
            this.Cis = eph.Cis;
            this.Cus = eph.Cus;
            this.ctToe = eph.Toe;
            this.Adot = 0;//??
            this.A = eph.SqrtA * eph.SqrtA;
            this.dn = eph.DeltaN;
            this.M0 = eph.MeanAnomaly;
            this.ecc = eph.Eccentricity;
            this.REL_CONST = eph.Eccentricity;
            this.PI = CoordConsts.PI;
            this.eye0 = eph.Inclination;
            this.Omega = eph.ArgumentOfPerigee;
            this.OMEGA0 = eph.LongOfAscension;
            this.dndot = 0;//??
            this.idot = eph.EyeDot;

            this.prn = eph.Prn;
        }
        SatelliteNumber prn;

        private double af0;
        private double af1;
        private double af2;
        private double ctToc;
        private double Cic;
        private double Crc;
        private double Crs;
        private double OMEGAdot;
        private double e0;
        private double Cuc;
        private double Cis;
        private double Cus;
        private double ctToe;
        private double Adot;
        private double A;
        private double dn;
        private double M0;
        private double ecc;
        private double REL_CONST;
        private double PI;
        private double eye0;
        private double Omega;
        private double OMEGA0;
        private double dndot;
        private double idot;

        // Compute the satellite clock bias (fraction) at the given time
        double svClockBias(double t)
        {

            double dtc, elaptc;
            elaptc = t - ctToc;
            dtc = af0 + elaptc * (af1 + elaptc * af2);
            return dtc;
        }

        // Compute the satellite clock drift (sec/sec) at the given time
        double svClockDrift(double t)
        {
            double drift, elaptc;
            elaptc = t - ctToc;
            drift = af1 + elaptc * af2;
            return drift;
        }

        // Compute satellite position at the given time.
       public  Xvt svXvt(double t)
        {
            //  if(!dataLoadedFlag)
            //  GPSTK_THROW(InvalidRequest("Data not loaded"));

            Xvt sv = new Xvt();
            double E;              // eccentric anomaly
           // double delea;           // delta eccentric anomaly during iteration
            double tk;          // elapsed time since Toe
            //double elaptc;          // elapsed time since Toc
            //double dtc,dtr;
            double q, sinE, cosE;
            double GSTA, GCTA;
            double en;
            double M;           // mean anomaly
           // double F;
            double G;             // temporary real variables
            double u, u_u, cos2u, sin2u, du, dr, di, U, R, f, eyek;
            double omgk, cosu, sinu, xip, yip, cosOmgk, sinOmgk, cosEyek, sinEyek;
            double xef, yef, zef, dek, dlk, div, domk, duv, drv;
            double dxp, dyp, vxef, vyef, vzef;


            Geo.Referencing.IEllipsoid ell = GnssSystem.GetGnssSystem(prn.SatelliteType).Ellipsoid;
            double sqrtGM = SQRT(ell.GM);
            double twoPI = 2.0e0 * PI;
            double e;            // eccentricity
            double eyeDot;          // dt inclination
            double sqrtA = SQRT(A); // A is semi-major axis of orbit
            double ToeSOW = this.ctToe;// GPSWeekSecond(ctToe).sow;    // SOW is time-system-independent

            e = ecc;
            eyeDot = idot;

            // Compute time since ephemeris & clock epochs
            tk = Time.GetDifferSecondOfWeek(t, ctToe);// t - ctToe;

            // Compute A at time of interest (LNAV: Adot==0)
            double Ak = A + Adot * tk;

            // Compute mean motion (LNAV: dndot==0)
            double dnA = dn + 0.5 * dndot * tk;
            en = (sqrtGM / (A * sqrtA)) + dnA;     // Eqn specifies A0, not Ak

            // In-plane angles
            //     meana - Mean anomaly
            //     ea    - Eccentric anomaly
            //     truea - True anomaly
            M = M0 + tk * en;
            M = fmod(M, twoPI);

            E =OrbitUtil.KeplerEqForEccAnomaly(M, e);
            //E = M + e * sin(M);

            //int loop_cnt = 1;
            //do
            //{
            //    F = M - (E - e * sin(E));
            //    G = 1.0 - e * cos(E);
            //    delea = F / G;
            //    E = E + delea;
            //    loop_cnt++;
            //} while ((fabs(delea) > 1.0e-11) && (loop_cnt <= 20));

            // Compute clock corrections
            sv.relcorr = svRelativity(t);
            sv.clkbias = svClockBias(t);
            sv.clkdrift = svClockDrift(t);
            // sv.frame = ReferenceFrame.WGS84;

            // Compute true anomaly
            sinE = sin(E);
            cosE = cos(E);

            q = SQRT(1.0 - e * e);

            G = 1.0 - e * cosE;

            //  G*SIN(TA) AND G*COS(TA)
            GSTA = q * sinE;
            GCTA = cosE - e;

            //  True anomaly
            f = atan2(GSTA, GCTA);

            // Argument of lat and correction terms (2nd harmonic)
            u = f + Omega;
            u_u = 2.0 * u;
            cos2u = cos(u_u);
            sin2u = sin(u_u);

            du = cos2u * Cuc + sin2u * Cus;
            dr = cos2u * Crc + sin2u * Crs;
            di = cos2u * Cic + sin2u * Cis;

            // U = updated argument of lat, R = radius, AINC = inclination
            U = u + du;
            R = Ak * G + dr;
            eyek = eye0 + eyeDot * tk + di;

            //  Longitude of ascending node (ANLON)
            omgk = OMEGA0 + (OMEGAdot - ell.AngleVelocity) * tk 
                - ell.AngleVelocity * ToeSOW;

            // In plane location
            cosu = cos(U);
            sinu = sin(U);
            xip = R * cosu;
            yip = R * sinu;

            //  Angles for rotation to earth fixed
            cosOmgk = cos(omgk);
            sinOmgk = sin(omgk);
            cosEyek = cos(eyek);
            sinEyek = sin(eyek);

            // Earth fixed coordinates in meters
            xef = xip * cosOmgk - yip * cosEyek * sinOmgk;
            yef = xip * sinOmgk + yip * cosEyek * cosOmgk;
            zef = yip * sinEyek;
            sv.x[0] = xef;
            sv.x[1] = yef;
            sv.x[2] = zef;

            // Compute velocity of rotation coordinates
            dek = en * Ak / R;
            dlk = sqrtA * q * sqrtGM / (R * R);
            div = eyeDot - 2.0e0 * dlk * (Cic * sin2u - Cis * cos2u);
            domk = OMEGAdot - ell.AngleVelocity;
            duv = dlk * (1.0 + 2.0 * (Cus * cos2u - Cuc * sin2u));
            drv = Ak * e * dek * sinE - 2.0 * dlk * (Crc * sin2u - Crs * cos2u);
            dxp = drv * cosu - R * sinu * duv;
            dyp = drv * sinu + R * cosu * duv;

            // Calculate velocities
            vxef = dxp * cosOmgk - xip * sinOmgk * domk - dyp * cosEyek * sinOmgk
                     + yip * (sinEyek * sinOmgk * div - cosEyek * cosOmgk * domk);
            vyef = dxp * sinOmgk + xip * cosOmgk * domk + dyp * cosEyek * cosOmgk
                     - yip * (sinEyek * cosOmgk * div + cosEyek * sinOmgk * domk);
            vzef = dyp * sinEyek + yip * cosEyek * div;

            // Move results into output variables
            sv.v[0] = vxef;
            sv.v[1] = vyef;
            sv.v[2] = vzef;

            return sv;
        }


        // Compute satellite relativity correction (sec) at the given time
        // throw Invalid Request if the required satData has not been stored.
        double svRelativity(double t)
        {

            Geo.Referencing.IEllipsoid ell = GnssSystem.GetGnssSystem(prn.SatelliteType).Ellipsoid;
            double twoPI = 2.0 * PI;
            double sqrtgm = SQRT(ell.GM);
            double elapte = t - ctToe;

            // Compute A at time of interest
            double Ak = A + Adot * elapte;                 // LNAV: Adot==0
            //double dnA = dn + 0.5*dndot*elapte;          // LNAV: dndot==0
            double Ahalf = SQRT(A);
            double amm = (sqrtgm / (A * Ahalf)) + dn;      // Eqn specifies A0 not Ak
            double meana, F, G, delea;

            meana = M0 + elapte * amm;
            meana = fmod(meana, twoPI);
            double ea = meana + ecc * sin(meana);

            int loop_cnt = 1;
            do
            {
                F = meana - (ea - ecc * sin(ea));
                G = 1.0 - ecc * cos(ea);
                delea = F / G;
                ea = ea + delea;
                loop_cnt++;
            } while ((ABS(delea) > 1.0e-11) && (loop_cnt <= 20));

            return (REL_CONST * ecc * SQRT(Ak) * sin(ea));
        }

        private double ABS(double delea)
        {
            return Math.Abs(delea);
        }

        private double fmod(double meana, double twoPI)
        {
            return meana % twoPI;
        }

        private double SQRT(double A)
        {
            return Math.Sqrt(A);
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