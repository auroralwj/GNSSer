
//2017.06.19, czs, edit in hongqing, 采用牛顿法改进开普勒方程计算方法

using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Times; 

namespace Gnsser
{
    /// <summary>
    /// 卫星轨道计算方法。
    /// 是本程序的主要的轨道计算方法。
    /// </summary>
    public  class OrbitUtil
    {
        static double SIN_5 = -0.0871557427476582;/* sin(-5.0 deg) */
        static double COS_5 = 0.9961946980917456; /* cos(-5.0 deg) */
        static double  RTOL_KEPLER = 1E-14 ;        /* relative tolerance for Kepler equation */

        public static Geo.Coordinates.XYZ GetSatXyz(EphemerisParam eph, double secOfWeek)
        {
             bool isGeosynchronous = eph.Prn.PRN <= 5 && eph.Prn.SatelliteType == SatelliteType.C;
             return GetSatXyz(eph, secOfWeek, eph.Prn.SatelliteType, isGeosynchronous);
        }
        
        /// <summary>
        /// 根据轨道根数计算卫星位置。
        /// 核心计算程序。
        /// </summary>
        /// <param name="secOfWeek">GPS周秒</param>
        /// <param name="isGeosynchronous">是否是地球同步轨道卫星</param>
        /// <returns></returns>
        public static Geo.Coordinates.XYZ GetSatXyz(EphemerisParam eph, double gpsSecOfWeek, SatelliteType satelliteType = SatelliteType.G, bool isGeosynchronous = false)
        {
            double e = eph.Eccentricity;
            double A, en0, tk, en, M, E, sinE, cosE, f, u, cos2u, sin2u, duk, drk, dik;
            double uk, rk, eyek, cosEyek, xpk, ypk, omgk, cosOmgk, sinOmgk, xk, yk, zk;
            double secOfWeek = gpsSecOfWeek;
            if (satelliteType == SatelliteType.C)//如果是北斗，需要转换为北斗时间来计算。
            {
                secOfWeek = gpsSecOfWeek - 14;
            }

            //*** time since orbit reference epoch
            tk = Time.GetDifferSecondOfWeek(secOfWeek, eph.Toe);
            Geo.Referencing.IEllipsoid ellipsoid = GnssSystem.GetGnssSystem(satelliteType).Ellipsoid;
            
            double GM = ellipsoid.GM;// MU_GPS;
            double omge = ellipsoid.AngleVelocity;//  OMGE;

            //平均角速度
            A = eph.SqrtA * eph.SqrtA;
            en0 = Math.Sqrt(GM) / (A * eph.SqrtA);

            //改正平均角度
            en = en0 + eph.DeltaN;

            //*** mean anomaly, M
            M = eph.MeanAnomaly + en * tk;

            //*** solve kepler's equation for eccentric anomaly, E
            E = KeplerEqForEccAnomaly(M, e);
            //var E0 = KeplerEqForEccAnomalyOld(M, e);
            //var E1 = KeplerEqForEccAnomaly1(M, e);
         //  double E1 = KeplerEqForEccAnomaly1(eph, M); 

            sinE = Math.Sin(E);
            cosE = Math.Cos(E);

            //*** true anomaly, L
            f = Math.Atan2(Math.Sqrt(1.0 - e * e) * sinE, cosE - e);

            //*** argument of latitude, rightHandSide
            u = f + eph.ArgumentOfPerigee;

            cos2u = Math.Cos(u + u);
            sin2u = Math.Sin(u + u);

            //*** corrections to the arg. of lat., radius, and inclination

            duk = eph.Cuc * cos2u + eph.Cus * sin2u;
            drk = eph.Crc * cos2u + eph.Crs * sin2u;
            dik = eph.Cic * cos2u + eph.Cis * sin2u;

            //*** correct the arg. of lat., radius, and inclination

            uk = u + duk;
            rk = A * (1.0 - e * cosE) + drk;
            eyek = eph.Inclination + eph.EyeDot * tk + dik;

            //*** position in the orbital plane

            xpk = rk * Math.Cos(uk);
            ypk = rk * Math.Sin(uk);

            cosEyek = Math.Cos(eyek);
            //*** correct the longitude of the ascending node
            XYZ xyz = new XYZ();
            //同步轨道卫星，如北斗
            if (isGeosynchronous)
            {
                omgk = eph.LongOfAscension + eph.OmegaDot * tk - omge * eph.Toe;
                cosOmgk = Math.Cos(omgk);
                sinOmgk = Math.Sin(omgk);
                //*** compute ecbf coordinates
                double xg = xpk * cosOmgk - ypk * sinOmgk * cosEyek;
                double yg = xpk * sinOmgk + ypk * cosOmgk * cosEyek;
                double zg = ypk * Math.Sin(eyek);

                double sino = Math.Sin(omge * tk);
                double coso = Math.Cos(omge * tk);

                double x = xg * coso + yg * sino * COS_5 + zg * sino * SIN_5;
                double y = -xg * sino + yg * coso * COS_5 + zg * coso * SIN_5;
                double z = -yg * SIN_5 + zg * COS_5;
                xyz = new XYZ(x, y, z);
            }
            else
            { 
                omgk = eph.LongOfAscension + (eph.OmegaDot - omge) * tk - omge * eph.Toe;
                cosOmgk = Math.Cos(omgk);
                sinOmgk = Math.Sin(omgk);
                //*** compute ecbf coordinates
                xk = xpk * cosOmgk - ypk * sinOmgk * cosEyek;
                yk = xpk * sinOmgk + ypk * cosOmgk * cosEyek;
                zk = ypk * Math.Sin(eyek);
                xyz = new XYZ(xk, yk, zk);
            }

            return xyz;
            //以下为测试比较
            XYZ xyz2 =  RtkLibOrbitUtil.GetPos(secOfWeek, eph);
            //XYZ xyzGpstk = GetPosXYZ(eph, secOfWeek);
           // return xyzGpstk;
        }

        public static  RmsedXYZ GetPosXYZ(EphemerisParam eph, double secOfWeek)
        {
            GpstkOrbitUtil o = new GpstkOrbitUtil(eph);
            Xvt xvt = o.svXvt(secOfWeek);

            XYZ xyzGpstk = XYZ.Parse(xvt.x);
            XYZ velercity = XYZ.Parse(xvt.v);
            return new RmsedXYZ(xyzGpstk, velercity);
        }

        /// <summary>
        /// 牛顿法
        /// 开普勒方程 由平近点角 M 和 离心率 e 计算 偏近点角E
        /// solve for eccentric anomaly given mean anomaly and orbital eccentricity
        /// use simple fixed point iteration of kepler's equation
        /// </summary>
        /// <param name="em">rad</param>
        /// <returns></returns>
        public static double KeplerEqForEccAnomalyOld(double M, double e)
        {
            int n;
            int maxN = 15;
            double Ek, E1;
            for (n = 0, E1 = M, Ek = 0.0; Math.Abs(E1 - Ek) > RTOL_KEPLER && n < maxN; n++)
            {
                Ek = E1;
                E1 -= (E1 - e  * Math.Sin(E1) - M) / (1.0 - e  * Math.Cos(E1));
            }
            return E1;
        }
        const double eps_mach = double.Epsilon;// 1.0e-15;
        const double pi = Math.PI;
        /// <summary>
        /// 牛顿法
        /// 开普勒方程 for 偏心改正。
        /// solve for eccentric anomaly given mean anomaly and orbital eccentricity
        /// use simple fixed point iteration of kepler's equation
        /// </summary>
        /// <param name="em">M 平近点角</param>
        /// <param name="e">椭圆轨道的偏心率</param>
        /// <returns></returns>
        public static double KeplerEqForEccAnomaly(double M, double e)
        {
            // Constants
            const int maxit = 15;
            const double eps = 100.0 * eps_mach;
            // Variables
            int i = 0;
            double E, f;

            // Starting value
            M = Modulo(M, 2.0 * pi);
            if (e < 0.8) E = M; else E = pi;

            // Iteration
            do
            {
                f = E - e * Math.Sin(E) - M;
                E = E - f / (1.0 - e * Math.Cos(E));
                ++i;
                if (i == maxit)
                {
                    Console.Error.WriteLine(" convergence problems in EccAnom");
                    break;
                }
            }
            while (Math.Abs(f) > eps);

            return E;

        }
        /// <summary>
        /// 求小数部分，始终为正。
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        static double Frac(double x) { return x - Math.Floor(x); }

        /// <summary>
        /// x mod y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static double Modulo(double x, double y) { return y * Frac(x / y); }
        /// <summary>
        /// 定点迭代法
        /// 开普勒方程 由平近点角 M 和 离心率 e 计算 偏近点角E
        /// solve for eccentric anomaly given mean anomaly and orbital eccentricity
        /// use simple fixed point iteration of kepler's equation
        /// </summary>
        /// <param name="em">rad</param>
        /// <returns></returns>
        public static double KeplerEqForEccAnomaly1(double em, double e)
        {
            double ecca, ecca0;           //*** iterates of eccentric anomaly
            //*** initialize eccentric anomaly
            ecca = em + e * Math.Sin(em);

            //*** exit only on convergence
            int counter = 0;
            do
            {
                ecca0 = ecca;
                ecca = em + e * Math.Sin(ecca0);
                counter++;
            } while (Math.Abs((ecca - ecca0) / ecca) > 1.0e-14 && counter < 20);
            return ecca;
        }
        /// <summary>
        /// 计算位置。
        /// </summary>
        /// <param name="record"></param>
        /// <param name="gpstime">周秒</param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        public static void GetSatPos(EphemerisParam record, double gpstime, out double X, out double Y, out double Z)
        {
            double mu = 3.986005e14;
            double n0, n;
            n0 = Math.Sqrt(mu) / (record.SqrtA * record.SqrtA * record.SqrtA);
            n = n0 + record.DeltaN;

            //double t = GetGPSTime(clk.Year + 2000, 
            //    clk.Month, clk.Day, clk.Hour, clk.Minute, clk.Second);
            double t = gpstime;// -0.075;

            double tk = t - record.Toe;
            if (tk > 302400) tk -= 604800;
            else if (tk < -302400) tk += 604800;

            double Mk = record.MeanAnomaly + n * tk;

            double Ek0, Ek;
            Ek0 = Mk;
            while (true)
            {
                Ek = Mk + record.Eccentricity * Math.Sin(Ek0);
                if (Math.Abs(Ek - Ek0) < 1e-8)
                    break;
                Ek0 = Ek;
            }

            //double fk = Math.Atan(Math.Sqrt(1 - clk.east * clk.east) * Math.Sin(Ek) / (Math.Cos(Ek) - clk.east));
            double fk = 2 * Math.Atan(Math.Sqrt(1 + record.Eccentricity) / Math.Sqrt(1 - record.Eccentricity) * Math.Tan(Ek / 2));

            double fik = fk + record.ArgumentOfPerigee;

            double deltau = record.Cuc * Math.Cos(2 * fik) + record.Cus * Math.Sin(2 * fik);
            double deltar = record.Crc * Math.Cos(2 * fik) + record.Crs * Math.Sin(2 * fik);
            double deltai = record.Cic * Math.Cos(2 * fik) + record.Cis * Math.Sin(2 * fik);

            double uk = fik + deltau;
            double rk = record.SqrtA * record.SqrtA * (1 - record.Eccentricity * Math.Cos(Ek)) + deltar;
            double ik = record.Inclination + deltai + record.EyeDot * tk;

            double xk = rk * Math.Cos(uk);
            double yk = rk * Math.Sin(uk);

            double omegae = 7.2921151467e-5;
            double omegak = record.LongOfAscension + (record.OmegaDot - omegae) * tk - omegae * record.Toe;
            //卫星在地心地固坐标系中的位置。
            X = xk * Math.Cos(omegak) - yk * Math.Cos(ik) * Math.Sin(omegak);
            Y = xk * Math.Sin(omegak) + yk * Math.Cos(ik) * Math.Cos(omegak);
            Z = yk * Math.Sin(ik);
        }

 
    }
}
