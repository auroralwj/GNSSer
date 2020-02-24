using System;
using System.Collections.Generic;
using System.Text;
using Geo.Coordinates;
using Geo.Times;
using Gnsser.Data;


namespace Gnsser
{

    /// <summary>
    /// 天文函数，待重构。
    /// </summary>
    public class SunMoonPosition
    {

        //Time of the prevObj valid time
        private static Time initialTime = new Time(1900, 3, 1, 0, 0, 0.0);

        // Time of the last valid time
        private static Time finalTime = new Time(2100, 2, 28, 0, 0, 0.0);

        /// <summary>
        /// Default constructor
        /// </summary>
        public SunMoonPosition()
        { }

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
        public void GetPosition(Time time, ErpItem erpv)
        {
            //gpst2utc
            Time tutc = time.GpstToUtc();


            // Test if the time interval is correct
            if ((tutc < initialTime) || (tutc > finalTime))
            { throw new Exception("Provided epoch is out of bounds!"); }

            //utc -> ut1
            Time tut = tutc + erpv.Ut12Utc;

            //sun and moon position in eci
            XYZ rs = new XYZ(0, 0, 0);
            XYZ rm = new XYZ(0, 0, 0);
            sunmoonpos_eci(tut, ref rs, ref rm);


            //eci to ecef transformation matrix
            double[] U = new double[9];//3*3 transformation matrix
            double gmst_ = 0.0;
            eci2ecef(tutc, erpv, ref U, ref gmst_);


            //sun and moon postion in ecef
            char[] tr = new char[2]; tr[0] = 'N'; tr[1] = 'N';
            double[] rsn = new double[3]; rsn[0] = rs.X; rsn[1] = rs.Y; rsn[2] = rs.Z;
            double[] rmn = new double[3]; rmn[0] = rm.X; rmn[1] = rm.Y; rmn[2] = rm.Z;

            double[] rsun = new double[3]; double[] rmoon = new double[3];

            matmul(tr, 3, 1, 3, 1.0, U, rsn, 0.0, ref rsun);

            matmul(tr, 3, 1, 3, 1.0, U, rmn, 0.0, ref rmoon);



            this.rSun = new XYZ(rsun);
            this.rMoon = new XYZ(rmoon);
            this.gmst = gmst_;

        }

        public XYZ rSun { get; set; }

        public XYZ rMoon { get; set; }

        public double gmst { get; set; }


        public void sunmoonpos_eci(Time tut, ref XYZ rsun, ref XYZ rmoon)
        {

            Time gpstime0 = new Time(2000, 1, 1, 12);



            double t = (double)(tut - gpstime0) / 86400.0 / 36525.0;

            double[] f = new double[5];

            //astronomical arguments
            ast_args(t, ref f);


            //obliquity of the ecliptic 黄道倾斜度
            double eps = 23.439291 - 0.0130042 * t;

            double sine = Math.Sin(eps * SunMoonPosition.DegToRad);
            double cose = Math.Cos(eps * SunMoonPosition.DegToRad);

            /* sun position in eci */
            double Ms = 357.5277233 + 35999.05034 * t;
            double ls = 280.460 + 36000.770 * t + 1.914666471 * Math.Sin(Ms * SunMoonPosition.DegToRad) + 0.019994643 * Math.Sin(2.0 * Ms * SunMoonPosition.DegToRad);
            double rs = SunMoonPosition.AU_CONST * (1.000140612 - 0.016708617 * Math.Cos(Ms * SunMoonPosition.DegToRad) - 0.000139589 * Math.Cos(2.0 * Ms * SunMoonPosition.DegToRad));
            double sinl = Math.Sin(ls * SunMoonPosition.DegToRad); double cosl = Math.Cos(ls * SunMoonPosition.DegToRad);
            rsun[0] = rs * cosl;
            rsun[1] = rs * cose * sinl;
            rsun[2] = rs * sine * sinl;

            /* moon position in eci */
            double lm = 218.32 + 481267.883 * t + 6.29 * Math.Sin(f[0]) - 1.27 * Math.Sin(f[0] - 2.0 * f[3]) +
               0.66 * Math.Sin(2.0 * f[3]) + 0.21 * Math.Sin(2.0 * f[0]) - 0.19 * Math.Sin(f[1]) - 0.11 * Math.Sin(2.0 * f[2]);
            double pm = 5.13 * Math.Sin(f[2]) + 0.28 * Math.Sin(f[0] + f[2]) - 0.28 * Math.Sin(f[2] - f[0]) -
               0.17 * Math.Sin(f[2] - 2.0 * f[3]);
            double rm = SunMoonPosition.RE_WGS84 / Math.Sin((0.9508 + 0.0518 * Math.Cos(f[0]) + 0.0095 * Math.Cos(f[0] - 2.0 * f[3]) +
                       0.0078 * Math.Cos(2.0 * f[3]) + 0.0028 * Math.Cos(2.0 * f[0])) * SunMoonPosition.DegToRad);

            sinl = Math.Sin(lm * SunMoonPosition.DegToRad); cosl = Math.Cos(lm * SunMoonPosition.DegToRad);
            double sinp = Math.Sin(pm * SunMoonPosition.DegToRad); double cosp = Math.Cos(pm * SunMoonPosition.DegToRad);
            rmoon[0] = rm * cosp * cosl;
            rmoon[1] = rm * (cose * cosp * sinl - sine * sinp);
            rmoon[2] = rm * (sine * cosp * sinl + cose * sinp);

        }


        /// <summary>
        /// eci to ecef transformation matrix
        /// compute eci to ecef transformation matrix
        /// </summary>
        /// <param name="tutc">time in UTC</param>
        /// <param name="erpv">erp value (xp, yp, ut1_utc, lod) (rad, rad, s, s/d)</param>
        /// <param name="U">eci to ecef transformation matrix (3*3)</param>
        /// <param name="gmst">greenwich mean sidereal time (rad)</param>
        private void eci2ecef(Time tutc, ErpItem erpv, ref double[] U, ref double gmst)
        {
            Time tutc_ = tutc;

            Time gpstime0 = new Time(2000, 1, 1, 12);

            //terrestrial time
            Time tgps = tutc_.UtcToGpsT();

            double t = ((double)(tgps - gpstime0) + 19.0 + 32.184) / 86400.0 / 36525.0;

            double t2 = t * t; double t3 = t2 * t;


            double[] f = new double[5];

            //astronomical arguments
            ast_args(t, ref f);

            /* iau 1976 precession */
            double ze = (2306.2181 * t + 0.30188 * t2 + 0.017998 * t3) * SunMoonPosition.DAS2R;
            double th = (2004.3109 * t - 0.42665 * t2 - 0.041833 * t3) * SunMoonPosition.DAS2R;
            double z = (2306.2181 * t + 1.09468 * t2 + 0.018203 * t3) * SunMoonPosition.DAS2R;
            double eps = (84381.448 - 46.8150 * t - 0.00059 * t2 + 0.001813 * t3) * SunMoonPosition.DAS2R;
            double[] R1 = Rz(-z); double[] R2 = Ry(th); double[] R3 = Rz(-ze);
            char[] tr = new char[2]; tr[0] = 'N'; tr[1] = 'N';
            double[] R = new double[9];
            matmul(tr, 3, 3, 3, 1.0, R1, R2, 0.0, ref R);
            double[] P = new double[9];
            matmul(tr, 3, 3, 3, 1.0, R, R3, 0.0, ref P);/* P=Rz(-z)*Ry(th)*Rz(-ze) */

            // iau 1980 nutation
            double dpsi = 0.0, deps = 0.0;
            nut_iau1980(t, f, ref dpsi, ref deps);
            R1 = Rx(-eps - deps); R2 = Rz(-dpsi); R3 = Rx(eps);

            R = new double[9];
            matmul(tr, 3, 3, 3, 1.0, R1, R2, 0.0, ref R);
            double[] N = new double[9];
            matmul(tr, 3, 3, 3, 1.0, R, R3, 0.0, ref N); /* N=Rx(-eps)*Rz(-dspi)*Rx(eps) */

            //greenwich aparent sidereal time (rad)
            double gmst_ = Time.UtcToGmst(tutc_, erpv.Ut12Utc);

            double gast = gmst_ + dpsi * Math.Cos(eps);
            gast += (0.00264 * Math.Sin(f[4]) + 0.000063 * Math.Sin(2.0 * f[4])) * SunMoonPosition.DAS2R;

            /* eci to ecef transformation matrix */
            R1 = Ry(-erpv.Xpole * SunMoonPosition.DAS2R); R2 = Rx(-erpv.Ypole * SunMoonPosition.DAS2R); R3 = Rz(gast);
            double[] W = new double[9];
            matmul(tr, 3, 3, 3, 1.0, R1, R2, 0.0, ref W);


            R = new double[9];
            matmul(tr, 3, 3, 3, 1.0, W, R3, 0.0, ref R); /* W=Ry(-xp)*Rx(-yp) */
            double[] NP = new double[9];
            matmul(tr, 3, 3, 3, 1.0, N, P, 0.0, ref NP);
            double[] U_ = new double[9];
            matmul(tr, 3, 3, 3, 1.0, R, NP, 0.0, ref U_); /* U=W*Rz(gast)*N*P */

            for (int i = 0; i < 9; i++) U[i] = U_[i];
            gmst = gmst_;
        }




        private double JD2GAST(Time gpstime)
        {

            //find the Julian Date of the previous midnight, JD0
            double JD = (double)gpstime.JulianDays;
            double JDmin = Math.Floor(JD) - 0.5;
            double JDmax = Math.Floor(JD) + 0.5;
            double JD0 = 0;
            if (JD > JDmin) JD0 = JDmin;
            if (JD > JDmax) JD0 = JDmax;
            double Hour = (JD - JD0) * 24; //time in hours past previous midnight
            double Days = JD - 2451545.0; //Compute the number of days since J2000
            double Days0 = JD0 - 2451545.0;//Compute the number of days since J2000
            double Tcent = Days / 36525; //Compute the number of centuries since J2000
            //Calculate GMST(Greenwhich Mean Sidereal Time) in hours (Oh to 24h) ... then funcKeyToDouble to degrees
            double TMP = 6.697374558 + 0.06570982441908 * Days0 + 1.00273790935 * Hour + 0.000026 * (Tcent * Tcent);
            double GMST = ((6.697374558 + 0.06570982441908 * Days0 + 1.00273790935 * Hour + 0.000026 * (Tcent * Tcent)) % 24) * 15;


            //mean siderial time in degrees
            double THETAm = GMST;

            //Mean obliquity of the ecliptic
            double EPSILONm = 23.439291 - 0.0130111 * Tcent - 1.64e-07 * (Tcent * Tcent) + 5.04e-07 * (Tcent * Tcent * Tcent);

            //Nutations in obliquity and longitude(degrees)
            double L = 280.4665 + 36000.7698 * Tcent;
            double dL = 218.3165 + 481267.8813 * Tcent;
            double OMEGA = 125.04452 - 1934.136261 * Tcent;

            //Calculate nutations using the following dayServices equation
            double dPSI = -17.20 * sind(OMEGA) - 1.32 * sind(2 * L) - 0.23 * sind(2 * dL) + 0.21 * sind(2 * OMEGA);
            double dEPSILON = 9.20 * cosd(OMEGA) + 0.57 * cosd(2 * L) + 0.10 * cosd(2 * dL) - 0.09 * cosd(2 * OMEGA);


            //Convert the units from arc-fraction to degrees
            dPSI = dPSI * (1 / 3600);
            dEPSILON = dEPSILON * (1 / 3600);

            //(GAST) Greenwhich apparent sidereal time expression in degrees
            double GAST = modulo((THETAm + dPSI * cosd(EPSILONm + dEPSILON)), 360);

            double GAST1 = ((THETAm + dPSI * cosd(EPSILONm + dEPSILON)) % 360);

            if (GAST != GAST1)
            {

            }
            return GAST;
        }

        /// <summary>
        /// Sine of argument in degrees
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private double sind(double x)
        {
            double res = 0;
            double n = Math.Round(x / 90);
            x = x - n * 90;
            int m = (int)n % 4; //mod
            if (m == 0) res = Math.Sin(SunMoonPosition.PI / 180 * x);
            if (m == 1) res = Math.Cos(SunMoonPosition.PI / 180 * x);
            if (m == 2) res = -Math.Sin(SunMoonPosition.PI / 180 * x);
            if (m == 3) res = -Math.Cos(SunMoonPosition.PI / 180 * x);
            return res;
        }
        /// <summary>
        /// Cosine of argument in degrees
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private double cosd(double x)
        {
            double res = 0;
            double n = Math.Round(x / 90);
            x = x - n * 90;
            int m = (int)n % 4; //mod
            if (m == 0) res = Math.Cos(SunMoonPosition.PI / 180 * x);
            if (m == 1) res = -Math.Sin(SunMoonPosition.PI / 180 * x);
            if (m == 2) res = -Math.Cos(SunMoonPosition.PI / 180 * x);
            if (m == 3) res = Math.Sin(SunMoonPosition.PI / 180 * x);
            return res;
        }


        /// <summary>
        /// the modulo mod of a number
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private double modulo(double a, double b)
        {
            return a - ((int)(a / b)) * b;
        }

        /// <summary>
        /// Rotate a 3D vector an specified angel in an axis
        /// </summary>
        /// <param name="v"></param>
        /// <param name="angle">rad</param>
        /// <param name="axis"></param>
        /// <returns></returns>
        private XYZ rotate(XYZ v, double angle, int axis)
        {
            XYZ vRes = new XYZ();
            if (axis == 1)
            {
                vRes[0] = v[0];
                vRes[1] = v[1] * Math.Cos(angle) + v[2] * Math.Sin(angle);
                vRes[2] = -v[1] * Math.Sin(angle) + v[2] * Math.Cos(angle);
            }
            else if (axis == 2)
            {
                vRes[0] = v[0] * Math.Cos(angle) - v[2] * Math.Sin(angle);
                vRes[1] = v[1];
                vRes[2] = v[0] * Math.Sin(angle) + v[2] * Math.Cos(angle);
            }
            else if (axis == 3)
            {
                vRes[0] = v[0] * Math.Cos(angle) + v[1] * Math.Sin(angle);
                vRes[1] = -v[0] * Math.Sin(angle) + v[1] * Math.Cos(angle);
                vRes[2] = v[2];
            }
            return vRes;
        }
         


        /// <summary>
        /// Coordinate rotation matrix
        /// </summary>
        /// <param name="t">旋转角度，单位是弧度</param>
        /// <returns></returns>
        private double[] Rz(double t)
        {
            double[] X = new double[9];
            X[8] = 1.0; X[2] = X[5] = X[6] = X[7] = 0.0;
            X[0] = X[4] = Math.Cos(t); X[3] = Math.Sin(t); X[1] = -X[3];
            return X;
        }
        /// <summary>
        /// Coordinate rotation matrix
        /// </summary>
        /// <param name="t">旋转角度，单位是弧度</param>
        /// <returns></returns>
        private double[] Ry(double t)
        {
            double[] X = new double[9];
            X[4] = 1.0; X[1] = X[3] = X[5] = X[7] = 0.0;
            X[0] = X[8] = Math.Cos(t); X[2] = Math.Sin(t); X[6] = -X[2];
            return X;
        }        /// <summary>
        /// Coordinate rotation matrix
        /// </summary>
        /// <param name="t">旋转角度，单位是弧度</param>
        /// <returns></returns>
        private double[] Rx(double t)
        {
            double[] X = new double[9];
            X[0] = 1.0; X[1] = X[2] = X[3] = X[6] = 0.0;
            X[4] = X[8] = Math.Cos(t); X[7] = Math.Sin(t); X[5] = -X[7];
            return X;
        }

        /// <summary>
        /// multiply matrix
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <param name="m"></param>
        /// <param name="alpha"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="beta"></param>
        /// <returns></returns>
        private double[] matmul(char[] tr, int n, int k, int m, double alpha, double[] A, double[] B, double beta, ref double[] C)
        {
            double d = 0.0;
            int f = tr[0] == 'N' ? (tr[1] == 'N' ? 1 : 2) : (tr[1] == 'N' ? 3 : 4);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    d = 0.0;
                    switch (f)
                    {
                        case 1: for (int x = 0; x < m; x++) d += A[i + x * n] * B[x + j * m]; break;
                        case 2: for (int x = 0; x < m; x++) d += A[i + x * n] * B[j + x * k]; break;
                        case 3: for (int x = 0; x < m; x++) d += A[x + i * m] * B[x + j * m]; break;
                        case 4: for (int x = 0; x < m; x++) d += A[x + i * m] * B[j + x * k]; break;

                    }
                    if (beta == 0.0) C[i + j * n] = alpha * d;
                    else C[i + j * n] = alpha * d + beta * C[i + j * n];
                }
            }

            return C;
        }
        #region  常数
        // nut[106][10]
          static   double[][] nut ={
         new double[] {   0,   0,   0,   0,   1, -6798.4, -171996, -174.2, 92025,   8.9},
         new double[] {   0,   0,   2,  -2,   2,   182.6,  -13187,   -1.6,  5736,  -3.1},
         new double[] {   0,   0,   2,   0,   2,    13.7,   -2274,   -0.2,   977,  -0.5},
         new double[] {   0,   0,   0,   0,   2, -3399.2,    2062,    0.2,  -895,   0.5},
         new double[] {   0,  -1,   0,   0,   0,  -365.3,   -1426,    3.4,    54,  -0.1},
         new double[] {   1,   0,   0,   0,   0,    27.6,     712,    0.1,    -7,   0.0},
         new double[] {   0,   1,   2,  -2,   2,   121.7,    -517,    1.2,   224,  -0.6},
         new double[] {   0,   0,   2,   0,   1,    13.6,    -386,   -0.4,   200,   0.0},
         new double[] {   1,   0,   2,   0,   2,     9.1,    -301,    0.0,   129,  -0.1},
         new double[] {   0,  -1,   2,  -2,   2,   365.2,     217,   -0.5,   -95,   0.3},
         new double[] {  -1,   0,   0,   2,   0,    31.8,     158,    0.0,    -1,   0.0},
         new double[] {   0,   0,   2,  -2,   1,   177.8,     129,    0.1,   -70,   0.0},
         new double[] {  -1,   0,   2,   0,   2,    27.1,     123,    0.0,   -53,   0.0},
         new double[] {   1,   0,   0,   0,   1,    27.7,      63,    0.1,   -33,   0.0},
         new double[] {   0,   0,   0,   2,   0,    14.8,      63,    0.0,    -2,   0.0},
         new double[] {  -1,   0,   2,   2,   2,     9.6,     -59,    0.0,    26,   0.0},
         new double[] {  -1,   0,   0,   0,   1,   -27.4,     -58,   -0.1,    32,   0.0},
         new double[] {   1,   0,   2,   0,   1,     9.1,     -51,    0.0,    27,   0.0},
         new double[] {  -2,   0,   0,   2,   0,  -205.9,     -48,    0.0,     1,   0.0},
         new double[] {  -2,   0,   2,   0,   1,  1305.5,      46,    0.0,   -24,   0.0},
         new double[] {   0,   0,   2,   2,   2,     7.1,     -38,    0.0,    16,   0.0},
         new double[] {   2,   0,   2,   0,   2,     6.9,     -31,    0.0,    13,   0.0},
         new double[] {   2,   0,   0,   0,   0,    13.8,      29,    0.0,    -1,   0.0},
         new double[] {   1,   0,   2,  -2,   2,    23.9,      29,    0.0,   -12,   0.0},
         new double[] {   0,   0,   2,   0,   0,    13.6,      26,    0.0,    -1,   0.0},
         new double[] {   0,   0,   2,  -2,   0,   173.3,     -22,    0.0,     0,   0.0},
         new double[] {  -1,   0,   2,   0,   1,    27.0,      21,    0.0,   -10,   0.0},
         new double[] {   0,   2,   0,   0,   0,   182.6,      17,   -0.1,     0,   0.0},
         new double[] {   0,   2,   2,  -2,   2,    91.3,     -16,    0.1,     7,   0.0},
         new double[] {  -1,   0,   0,   2,   1,    32.0,      16,    0.0,    -8,   0.0},
         new double[] {   0,   1,   0,   0,   1,   386.0,     -15,    0.0,     9,   0.0},
         new double[] {   1,   0,   0,  -2,   1,   -31.7,     -13,    0.0,     7,   0.0},
         new double[] {   0,  -1,   0,   0,   1,  -346.6,     -12,    0.0,     6,   0.0},
         new double[] {   2,   0,  -2,   0,   0, -1095.2,      11,    0.0,     0,   0.0},
         new double[] {  -1,   0,   2,   2,   1,     9.5,     -10,    0.0,     5,   0.0},
         new double[] {   1,   0,   2,   2,   2,     5.6,      -8,    0.0,     3,   0.0},
         new double[] {   0,  -1,   2,   0,   2,    14.2,      -7,    0.0,     3,   0.0},
         new double[] {   0,   0,   2,   2,   1,     7.1,      -7,    0.0,     3,   0.0},
         new double[] {   1,   1,   0,  -2,   0,   -34.8,      -7,    0.0,     0,   0.0},
         new double[] {   0,   1,   2,   0,   2,    13.2,       7,    0.0,    -3,   0.0},
         new double[] {  -2,   0,   0,   2,   1,  -199.8,      -6,    0.0,     3,   0.0},
         new double[] {   0,   0,   0,   2,   1,    14.8,      -6,    0.0,     3,   0.0},
         new double[] {   2,   0,   2,  -2,   2,    12.8,       6,    0.0,    -3,   0.0},
         new double[] {   1,   0,   0,   2,   0,     9.6,       6,    0.0,     0,   0.0},
         new double[] {   1,   0,   2,  -2,   1,    23.9,       6,    0.0,    -3,   0.0},
         new double[] {   0,   0,   0,  -2,   1,   -14.7,      -5,    0.0,     3,   0.0},
         new double[] {   0,  -1,   2,  -2,   1,   346.6,      -5,    0.0,     3,   0.0},
         new double[] {   2,   0,   2,   0,   1,     6.9,      -5,    0.0,     3,   0.0},
         new double[] {   1,  -1,   0,   0,   0,    29.8,       5,    0.0,     0,   0.0},
         new double[] {   1,   0,   0,  -1,   0,   411.8,      -4,    0.0,     0,   0.0},
         new double[] {   0,   0,   0,   1,   0,    29.5,      -4,    0.0,     0,   0.0},
         new double[] {   0,   1,   0,  -2,   0,   -15.4,      -4,    0.0,     0,   0.0},
         new double[] {   1,   0,  -2,   0,   0,   -26.9,       4,    0.0,     0,   0.0},
         new double[] {   2,   0,   0,  -2,   1,   212.3,       4,    0.0,    -2,   0.0},
         new double[] {   0,   1,   2,  -2,   1,   119.6,       4,    0.0,    -2,   0.0},
         new double[] {   1,   1,   0,   0,   0,    25.6,      -3,    0.0,     0,   0.0},
         new double[] {   1,  -1,   0,  -1,   0, -3232.9,      -3,    0.0,     0,   0.0},
         new double[] {  -1,  -1,   2,   2,   2,     9.8,      -3,    0.0,     1,   0.0},
         new double[] {   0,  -1,   2,   2,   2,     7.2,      -3,    0.0,     1,   0.0},
         new double[] {   1,  -1,   2,   0,   2,     9.4,      -3,    0.0,     1,   0.0},
         new double[] {   3,   0,   2,   0,   2,     5.5,      -3,    0.0,     1,   0.0},
         new double[] {  -2,   0,   2,   0,   2,  1615.7,      -3,    0.0,     1,   0.0},
         new double[] {   1,   0,   2,   0,   0,     9.1,       3,    0.0,     0,   0.0},
         new double[] {  -1,   0,   2,   4,   2,     5.8,      -2,    0.0,     1,   0.0},
         new double[] {   1,   0,   0,   0,   2,    27.8,      -2,    0.0,     1,   0.0},
         new double[] {  -1,   0,   2,  -2,   1,   -32.6,      -2,    0.0,     1,   0.0},
         new double[] {   0,  -2,   2,  -2,   1,  6786.3,      -2,    0.0,     1,   0.0},
         new double[] {  -2,   0,   0,   0,   1,   -13.7,      -2,    0.0,     1,   0.0},
         new double[] {   2,   0,   0,   0,   1,    13.8,       2,    0.0,    -1,   0.0},
         new double[] {   3,   0,   0,   0,   0,     9.2,       2,    0.0,     0,   0.0},
         new double[] {   1,   1,   2,   0,   2,     8.9,       2,    0.0,    -1,   0.0},
         new double[] {   0,   0,   2,   1,   2,     9.3,       2,    0.0,    -1,   0.0},
         new double[] {   1,   0,   0,   2,   1,     9.6,      -1,    0.0,     0,   0.0},
         new double[] {   1,   0,   2,   2,   1,     5.6,      -1,    0.0,     1,   0.0},
         new double[] {   1,   1,   0,  -2,   1,   -34.7,      -1,    0.0,     0,   0.0},
         new double[] {   0,   1,   0,   2,   0,    14.2,      -1,    0.0,     0,   0.0},
         new double[] {   0,   1,   2,  -2,   0,   117.5,      -1,    0.0,     0,   0.0},
         new double[] {   0,   1,  -2,   2,   0,  -329.8,      -1,    0.0,     0,   0.0},
         new double[] {   1,   0,  -2,   2,   0,    23.8,      -1,    0.0,     0,   0.0},
         new double[] {   1,   0,  -2,  -2,   0,    -9.5,      -1,    0.0,     0,   0.0},
         new double[] {   1,   0,   2,  -2,   0,    32.8,      -1,    0.0,     0,   0.0},
         new double[] {   1,   0,   0,  -4,   0,   -10.1,      -1,    0.0,     0,   0.0},
         new double[] {   2,   0,   0,  -4,   0,   -15.9,      -1,    0.0,     0,   0.0},
         new double[] {   0,   0,   2,   4,   2,     4.8,      -1,    0.0,     0,   0.0},
         new double[] {   0,   0,   2,  -1,   2,    25.4,      -1,    0.0,     0,   0.0},
         new double[] {  -2,   0,   2,   4,   2,     7.3,      -1,    0.0,     1,   0.0},
         new double[] {   2,   0,   2,   2,   2,     4.7,      -1,    0.0,     0,   0.0},
         new double[] {   0,  -1,   2,   0,   1,    14.2,      -1,    0.0,     0,   0.0},
         new double[] {   0,   0,  -2,   0,   1,   -13.6,      -1,    0.0,     0,   0.0},
         new double[] {   0,   0,   4,  -2,   2,    12.7,       1,    0.0,     0,   0.0},
         new double[] {   0,   1,   0,   0,   2,   409.2,       1,    0.0,     0,   0.0},
         new double[] {   1,   1,   2,  -2,   2,    22.5,       1,    0.0,    -1,   0.0},
         new double[] {   3,   0,   2,  -2,   2,     8.7,       1,    0.0,     0,   0.0},
         new double[] {  -2,   0,   2,   2,   2,    14.6,       1,    0.0,    -1,   0.0},
         new double[] {  -1,   0,   0,   0,   2,   -27.3,       1,    0.0,    -1,   0.0},
         new double[] {   0,   0,  -2,   2,   1,  -169.0,       1,    0.0,     0,   0.0},
         new double[] {   0,   1,   2,   0,   1,    13.1,       1,    0.0,     0,   0.0},
         new double[] {  -1,   0,   4,   0,   2,     9.1,       1,    0.0,     0,   0.0},
         new double[] {   2,   1,   0,  -2,   0,   131.7,       1,    0.0,     0,   0.0},
         new double[] {   2,   0,   0,   2,   0,     7.1,       1,    0.0,     0,   0.0},
         new double[] {   2,   0,   2,  -2,   1,    12.8,       1,    0.0,    -1,   0.0},
         new double[] {   2,   0,  -2,   0,   1,  -943.2,       1,    0.0,     0,   0.0},
         new double[] {   1,  -1,   0,  -2,   0,   -29.3,       1,    0.0,     0,   0.0},
         new double[] {  -1,   0,   0,   1,   1,  -388.3,       1,    0.0,     0,   0.0},
         new double[] {  -1,  -1,   0,   2,   1,    35.0,       1,    0.0,     0,   0.0},
         new double[] {   0,   1,   0,   1,   0,    27.3,       1,    0.0,     0,   0.0}
    };
        #endregion

          /// <summary>
        /// iau 1980 nutation
        /// </summary>
        /// <param name="t"></param>
        /// <param name="f"></param>
        /// <param name="dpsi"></param>
        /// <param name="deps"></param>
        private void nut_iau1980(double t, double[] f, ref double dpsi, ref double deps)
        {
          
            double ang;
            int i, j;
            dpsi = deps = 0.0;

            for (i = 0; i < 106; i++)
            {
                ang = 0.0;
                for (j = 0; j < 5; j++) { ang += nut[i][j] * f[j]; }
                dpsi += (nut[i][6] + nut[i][7] * t) * Math.Sin(ang);
                deps += (nut[i][8] + nut[i][9] * t) * Math.Cos(ang);
            }
            dpsi = dpsi * 1E-4 * SunMoonPosition.DAS2R; /* 0.1 mas -> rad */
            deps = deps * 1E-4 * SunMoonPosition.DAS2R;
        }





        static double[][] fc ={
                               new double[] { 134.96340251, 1717915923.2178,  31.8792,  0.051635, -0.00024470},
                               new double[]   { 357.52910918,  129596581.0481,  -0.5532,  0.000136, -0.00001149},
                               new double[]   {  93.27209062, 1739527262.8478, -12.7512, -0.001037,  0.00000417},
                               new double[]   { 297.85019547, 1602961601.2090,  -6.3706,  0.006593, -0.00003169},
                               new double[]  { 125.04455501,   -6962890.2665,   7.4722,  0.007702,  -0.00005939}
                          };



        /// <summary>
        /// astronomical arguments : f={1, 1', F, D, OMG} （rad)
        /// 天文 数据
        /// </summary>
        /// <param name="t"></param>
        /// <param name="f"></param>
        private static void ast_args(double t, ref double[] f)
        {
            //astronomical arguments



            double[] tt = new double[4];
            int i, j;

            for (tt[0] = t, i = 1; i < 4; i++) tt[i] = tt[i - 1] * t;

            for (i = 0; i < 5; i++)
            {
                f[i] = fc[i][0] * 3600.0;
                for (j = 0; j < 4; j++) f[i] += fc[i][j + 1] * tt[j];
                f[i] = (f[i] * SunMoonPosition.DAS2R) % (2.0 * SunMoonPosition.PI);
            }
        }






        public const double PI = 3.1415926535897932;
        /// GPS value of PI*2
        /// 
        public const double TWO_PI = 6.2831853071796;
        //
        /// Astronomical Unit value (AU), in meters
        public const double AU_CONST = (1.49597870691e11);

        /// Mean Earth-Moon barycenter (EMB) distance (AU)
        public const double MeanEarthMoonBary = (3.12e-5);

        /// Ratio of mass Sun to Earth
        public const double MU_SUN = (332946.0);

        /// Ratio of mass Moon to Earth
        public const double MU_MOON = (0.01230002);

        /// Earth gravity acceleration on surface (m/s^2)
        public const double EarthGrav = (9.80665);

        /// Degrees to radians
        public const double DegToRad = 0.0174532925199432957692369; //  (PI/180)
        /// <summary>
        /// 地球自转常数 (rad/s)
        /// </summary>
        public const double OMGE = 7.2921151467e-5;    /* earth angular velocity (IS-GPS) (rad/s) */

        public const double GME = 3.986004415e+14; /* earth gravitational constant */

        public const double GMS = 1.327124e+20;    /* sun gravitational constant */

        public const double GMM = 4.902801e+12;   /* moon gravitational constant */

        public const double RE_WGS84 = 6378137.0;          /* earth semimajor axis (WGS84) (m) */

        public const double FE_WGS84 = (1.0 / 298.257223563); /* earth flattening (WGS84) */

        public const double AS2R = (DegToRad / 3600.0); /*arc sec to radian */


        /// <summary>
        /// radians to degrees
        /// </summary>
        public const double R2D = 57.2957795130824; // (180/PI)


        /// Arc fraction to radians
        public const double DAS2R = (4.848136811095359935899141e-6);  //(D2R/3600)

        /// Fraction of time to radians
        public const double DS2R = (7.272205216643039903848712e-5);

        /// Julian epoch of B1950
        public const double B1950 = (1949.9997904423);

        /// Earth equatorial radius in AU ( 6378.137 km / 149597870 km)
        public const double ERADAU = (4.2635212653763e-5);

        /// <summary>
        /// Function to change from CIS to CTS(ECEF) coordinate system
        /// (coordinates in meters)
        /// </summary>
        /// <param name="posCIS">Coordinates in CIS system (in meters).</param>
        /// <param name="t">Epoch</param>
        /// <returns></returns>
        public XYZ CIS2CTS(XYZ posCIS, Time t)
        {
            //Angle of Earth rotation, in radians
            double ts = UTC2SID(t) * TWO_PI / 24.0;
            double x = Math.Cos(ts) * posCIS.X + Math.Sin(ts) * posCIS.Y;
            double y = -Math.Sin(ts) * posCIS.X + Math.Cos(ts) * posCIS.Y;
            double z = posCIS.Z;
            XYZ res = new XYZ(x, y, z);

            return res;

        }

        /// <summary>
        /// Function to funcKeyToDouble from UTC to sidereal time
        /// UTC 到恒心时。
        /// </summary>
        /// <param name="t">Epoch</param>
        /// <returns> sidereal time in hours.</returns>
        public double UTC2SID(Time t)
        {
            double y = t.Year - 1.0;
            double m = 13.0;
            double d = t.DayOfYear;//Get secondOfWeek of year

            //Hours of secondOfWeek(decimal)
            double h = t.SecondsOfDay / 3600.0;

            //Fraction of secondOfWeek
            double frofday = t.SecondsOfDay / 86400.0;

            //Compute Julian Day, including decimal
            double jd = (double)t.JulianDays;
            //Temporal value, in centuries
            double tt = (jd - 2451545.0) / 36525.0;

            double sid = 24110.54841 + tt * ((8640184.812866) +
                  tt * ((0.093104) - (6.2e-6 * tt)));

            sid = sid / 3600.0 + h;
            sid = Math.IEEERemainder(sid, 24.0);
            if (sid < 0.0)
            { sid += 24.0; }
            return sid;
        }

    }
}