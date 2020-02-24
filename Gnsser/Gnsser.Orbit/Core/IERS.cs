//2017.06.18, czs, funcKeyToDouble from c++ in hongqing, Date
//2017.06.26, czs, edit in hongqing, format and refactor codes
//2018.10.14, czs, edit in hmx, 进行了简单的整理

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;
using Geo.Algorithm;
using Geo.Utils;

namespace Gnsser.Orbits
{
    /// <summary>
    /// 天球到地球参考框架的转换。
    /// Transformations betweeen celestial and terrestrial reference systems
    /// </summary>
    public class IERS
    {
        /// <summary>
        ///   Setting of IERS Earth rotation parameters
        ///   (UT1-UTC [s],UTC-TAI [s], x ["], y ["])
        /// </summary>
        /// <param name="UT1_UTC">UT1与UTC之差</param>
        /// <param name="UTC_TAI">UTC与TAI之差</param>
        /// <param name="x_pole">极移X方向，角秒</param>
        /// <param name="y_pole">极移Y方向，角秒</param>
        public IERS(double UT1_UTC, double UTC_TAI,double x_pole, double y_pole)
        {
            this.UT1_UTC_ = UT1_UTC;
            this.UTC_TAI = UTC_TAI;
            this.PolarMotionX = x_pole / OrbitConsts.ArcSecondsPerRad;
            this.PolarMotionY = y_pole / OrbitConsts.ArcSecondsPerRad;
        }

        /// <summary>
        /// 简化儒略日 2000 历元。
        /// Modif. Julian Date of J2000.0
        /// </summary>
        public const double MJD_J2000 = Geo.Utils.OrbitConsts.MJD_J2000;// 51544.5;             

        // Default values of Earth rotation parameters
        /// <summary>
        /// UT1-UTC time difference [s]
        /// </summary>
        public double UT1_UTC_ { get; set; }
        /// <summary>
        ///  UTC-TAI time difference [s] 
        /// </summary>
        public double UTC_TAI { get; set; }
        /// <summary>
        ///  Pole coordinate [rad]
        /// </summary>
        public double PolarMotionX { get; set; }
        /// <summary>
        ///  Pole coordinate [rad]
        /// </summary>
        public double PolarMotionY { get; set; }

        /// <summary>
        ///  UT1-UTC time difference [s]
        /// </summary>
        /// <param name="Mjd_UTC">UTC 下的简化儒略日。</param>
        /// <returns></returns>
        public double GetUT1_UTC(double Mjd_UTC) { return UT1_UTC_; }


        /// <summary>
        ///  TT-UTC time difference [s]
        /// </summary>
        /// <param name="Mjd_UTC">UTC 下的简化儒略日</param>
        /// <returns></returns>
        public double GetTT_UTC(double Mjd_UTC) { return OrbitConsts.TT_TAI - UTC_TAI; }

        /// <summary>
        ///  GPS-UTC time difference [s]
        /// </summary>
        /// <param name="Mjd_UTC">UTC 下的简化儒略日</param>
        /// <returns></returns>
        public double GetGPS_UTC(double Mjd_UTC) { return OrbitConsts.GPS_TAI - UTC_TAI; }


        /// <summary>
        /// 极移旋转矩阵。
        /// Transformation from pseudo Earth-fixed to Earth-fixed coordinates for a given date
        /// </summary>
        /// <param name="Mjd_UTC">地心力学时下的简化儒略日。 Mjd_UTC   Modified Julian Date UTC</param>
        /// <returns>Pole matrix</returns>
        public Matrix PoleMatrix(double Mjd_UTC)
        {
            return Matrix.RotateY3D(-PolarMotionX) * Matrix.RotateX3D(-PolarMotionY);
        }

        #region 静态工具计算方法
        /// <summary>
        //计算黄道平均倾角Computes the mean obliquity of the ecliptic
        /// </summary>
        /// <param name="Mjd_TT">地心力学时下的简化儒略日。 Modified Julian Date (Terrestrial Time)</param>
        /// <returns>Mean obliquity of the ecliptic</returns>
        static public double MeanObliquity(double Mjd_TT)
        {
            double T = (Mjd_TT - MJD_J2000) / 36525.0;
            return OrbitConsts.RadPerDeg * (23.43929111 - (46.8150 + (0.00059 - 0.001813 * T) * T) * T / 3600.0);
        }


        /// <summary>
        /// 赤道到天球赤道坐标的转换矩阵.
        /// Transformation of equatorial to ecliptical coordinates
        /// </summary>
        /// <param name="Mjd_TT">Mjd_TT    Modified Julian Date (Terrestrial Time)</param>
        /// <returns>  Transformation matrix</returns>
        static public Matrix EclMatrix(double Mjd_TT)
        {
            return Matrix.RotateX3D(MeanObliquity(Mjd_TT));
        }


        /// <summary>
        /// 岁差转换矩阵。Precession transformation of equatorial coordinates
        /// </summary>
        /// <param name="Mjd_1"> 转换起始历元。 Epoch given (Modified Julian Date TT)</param>
        /// <param name="Mjd_2"> 转换到历元。 Epoch to precess to (Modified Julian Date TT)</param>
        /// <returns> Precession transformation matrix</returns>
        static public Matrix PrecessionMatrix(double Mjd_1, double Mjd_2)
        {
            // Constants
            double T = (Mjd_1 - MJD_J2000) / 36525.0;
            double dT = (Mjd_2 - Mjd_1) / 36525.0;

            // Variables
            double zeta, z, theta;

            // Precession angles
            zeta = ((2306.2181 + (1.39656 - 0.000139 * T) * T) +
                          ((0.30188 - 0.000344 * T) + 0.017998 * dT) * dT) * dT / OrbitConsts.ArcSecondsPerRad;
            z = zeta + ((0.79280 + 0.000411 * T) + 0.000205 * dT) * dT * dT / OrbitConsts.ArcSecondsPerRad;
            theta = ((2004.3109 - (0.85330 + 0.000217 * T) * T) -
                          ((0.42665 + 0.000217 * T) + 0.041833 * dT) * dT) * dT / OrbitConsts.ArcSecondsPerRad;

            // Precession matrix
            return Matrix.RotateZ3D(-z) * Matrix.RotateY3D(theta) * Matrix.RotateZ3D(-zeta);
        }

        #region  satData
        /// <summary>
        /// 日月章动数据??
        /// </summary>
        static long[][] C = new long[][]  // long C[106][9] = new long[][]
      {
       //
       // l  l' F  D Om    dpsi    *T     deps     *T       #
       //
          new long[] {  0, 0, 0, 0, 1,-1719960,-1742,  920250,   89 },   //   1
          new long[] {  0, 0, 0, 0, 2,   20620,    2,   -8950,    5 },   //   2
          new long[] { -2, 0, 2, 0, 1,     460,    0,    -240,    0 },   //   3
          new long[] {  2, 0,-2, 0, 0,     110,    0,       0,    0 },   //   4
          new long[] { -2, 0, 2, 0, 2,     -30,    0,      10,    0 },   //   5
          new long[] {  1,-1, 0,-1, 0,     -30,    0,       0,    0 },   //   6
          new long[] {  0,-2, 2,-2, 1,     -20,    0,      10,    0 },   //   7
          new long[] {  2, 0,-2, 0, 1,      10,    0,       0,    0 },   //   8
          new long[] {  0, 0, 2,-2, 2, -131870,  -16,   57360,  -31 },   //   9
          new long[] {  0, 1, 0, 0, 0,   14260,  -34,     540,   -1 },   //  10
          new long[] {  0, 1, 2,-2, 2,   -5170,   12,    2240,   -6 },   //  11
          new long[] {  0,-1, 2,-2, 2,    2170,   -5,    -950,    3 },   //  12
          new long[] {  0, 0, 2,-2, 1,    1290,    1,    -700,    0 },   //  13
          new long[] {  2, 0, 0,-2, 0,     480,    0,      10,    0 },   //  14
          new long[] {  0, 0, 2,-2, 0,    -220,    0,       0,    0 },   //  15
          new long[] {  0, 2, 0, 0, 0,     170,   -1,       0,    0 },   //  16
          new long[] {  0, 1, 0, 0, 1,    -150,    0,      90,    0 },   //  17
          new long[] {  0, 2, 2,-2, 2,    -160,    1,      70,    0 },   //  18
          new long[] {  0,-1, 0, 0, 1,    -120,    0,      60,    0 },   //  19
          new long[] { -2, 0, 0, 2, 1,     -60,    0,      30,    0 },   //  20
          new long[] {  0,-1, 2,-2, 1,     -50,    0,      30,    0 },   //  21
          new long[] {  2, 0, 0,-2, 1,      40,    0,     -20,    0 },   //  22
          new long[] {  0, 1, 2,-2, 1,      40,    0,     -20,    0 },   //  23
          new long[] {  1, 0, 0,-1, 0,     -40,    0,       0,    0 },   //  24
          new long[] {  2, 1, 0,-2, 0,      10,    0,       0,    0 },   //  25
          new long[] {  0, 0,-2, 2, 1,      10,    0,       0,    0 },   //  26
          new long[] {  0, 1,-2, 2, 0,     -10,    0,       0,    0 },   //  27
          new long[] {  0, 1, 0, 0, 2,      10,    0,       0,    0 },   //  28
          new long[] { -1, 0, 0, 1, 1,      10,    0,       0,    0 },   //  29
          new long[] {  0, 1, 2,-2, 0,     -10,    0,       0,    0 },   //  30
          new long[] {  0, 0, 2, 0, 2,  -22740,   -2,    9770,   -5 },   //  31
          new long[] {  1, 0, 0, 0, 0,    7120,    1,     -70,    0 },   //  32
          new long[] {  0, 0, 2, 0, 1,   -3860,   -4,    2000,    0 },   //  33
          new long[] {  1, 0, 2, 0, 2,   -3010,    0,    1290,   -1 },   //  34
          new long[] {  1, 0, 0,-2, 0,   -1580,    0,     -10,    0 },   //  35
          new long[] { -1, 0, 2, 0, 2,    1230,    0,    -530,    0 },   //  36
          new long[] {  0, 0, 0, 2, 0,     630,    0,     -20,    0 },   //  37
          new long[] {  1, 0, 0, 0, 1,     630,    1,    -330,    0 },   //  38
          new long[] { -1, 0, 0, 0, 1,    -580,   -1,     320,    0 },   //  39
          new long[] { -1, 0, 2, 2, 2,    -590,    0,     260,    0 },   //  40
          new long[] {  1, 0, 2, 0, 1,    -510,    0,     270,    0 },   //  41
          new long[] {  0, 0, 2, 2, 2,    -380,    0,     160,    0 },   //  42
          new long[] {  2, 0, 0, 0, 0,     290,    0,     -10,    0 },   //  43
          new long[] {  1, 0, 2,-2, 2,     290,    0,    -120,    0 },   //  44
          new long[] {  2, 0, 2, 0, 2,    -310,    0,     130,    0 },   //  45
          new long[] {  0, 0, 2, 0, 0,     260,    0,     -10,    0 },   //  46
          new long[] { -1, 0, 2, 0, 1,     210,    0,    -100,    0 },   //  47
          new long[] { -1, 0, 0, 2, 1,     160,    0,     -80,    0 },   //  48
          new long[] {  1, 0, 0,-2, 1,    -130,    0,      70,    0 },   //  49
          new long[] { -1, 0, 2, 2, 1,    -100,    0,      50,    0 },   //  50
          new long[] {  1, 1, 0,-2, 0,     -70,    0,       0,    0 },   //  51
          new long[] {  0, 1, 2, 0, 2,      70,    0,     -30,    0 },   //  52
          new long[] {  0,-1, 2, 0, 2,     -70,    0,      30,    0 },   //  53
          new long[] {  1, 0, 2, 2, 2,     -80,    0,      30,    0 },   //  54
          new long[] {  1, 0, 0, 2, 0,      60,    0,       0,    0 },   //  55
          new long[] {  2, 0, 2,-2, 2,      60,    0,     -30,    0 },   //  56
          new long[] {  0, 0, 0, 2, 1,     -60,    0,      30,    0 },   //  57
          new long[] {  0, 0, 2, 2, 1,     -70,    0,      30,    0 },   //  58
          new long[] {  1, 0, 2,-2, 1,      60,    0,     -30,    0 },   //  59
          new long[] {  0, 0, 0,-2, 1,     -50,    0,      30,    0 },   //  60
          new long[] {  1,-1, 0, 0, 0,      50,    0,       0,    0 },   //  61
          new long[] {  2, 0, 2, 0, 1,     -50,    0,      30,    0 },   //  62
          new long[] {  0, 1, 0,-2, 0,     -40,    0,       0,    0 },   //  63
          new long[] {  1, 0,-2, 0, 0,      40,    0,       0,    0 },   //  64
          new long[] {  0, 0, 0, 1, 0,     -40,    0,       0,    0 },   //  65
          new long[] {  1, 1, 0, 0, 0,     -30,    0,       0,    0 },   //  66
          new long[] {  1, 0, 2, 0, 0,      30,    0,       0,    0 },   //  67
          new long[] {  1,-1, 2, 0, 2,     -30,    0,      10,    0 },   //  68
          new long[] { -1,-1, 2, 2, 2,     -30,    0,      10,    0 },   //  69
          new long[] { -2, 0, 0, 0, 1,     -20,    0,      10,    0 },   //  70
          new long[] {  3, 0, 2, 0, 2,     -30,    0,      10,    0 },   //  71
          new long[] {  0,-1, 2, 2, 2,     -30,    0,      10,    0 },   //  72
          new long[] {  1, 1, 2, 0, 2,      20,    0,     -10,    0 },   //  73
          new long[] { -1, 0, 2,-2, 1,     -20,    0,      10,    0 },   //  74
          new long[] {  2, 0, 0, 0, 1,      20,    0,     -10,    0 },   //  75
          new long[] {  1, 0, 0, 0, 2,     -20,    0,      10,    0 },   //  76
          new long[] {  3, 0, 0, 0, 0,      20,    0,       0,    0 },   //  77
          new long[] {  0, 0, 2, 1, 2,      20,    0,     -10,    0 },   //  78
          new long[] { -1, 0, 0, 0, 2,      10,    0,     -10,    0 },   //  79
          new long[] {  1, 0, 0,-4, 0,     -10,    0,       0,    0 },   //  80
          new long[] { -2, 0, 2, 2, 2,      10,    0,     -10,    0 },   //  81
          new long[] { -1, 0, 2, 4, 2,     -20,    0,      10,    0 },   //  82
          new long[] {  2, 0, 0,-4, 0,     -10,    0,       0,    0 },   //  83
          new long[] {  1, 1, 2,-2, 2,      10,    0,     -10,    0 },   //  84
          new long[] {  1, 0, 2, 2, 1,     -10,    0,      10,    0 },   //  85
          new long[] { -2, 0, 2, 4, 2,     -10,    0,      10,    0 },   //  86
          new long[] { -1, 0, 4, 0, 2,      10,    0,       0,    0 },   //  87
          new long[] {  1,-1, 0,-2, 0,      10,    0,       0,    0 },   //  88
          new long[] {  2, 0, 2,-2, 1,      10,    0,     -10,    0 },   //  89
          new long[] {  2, 0, 2, 2, 2,     -10,    0,       0,    0 },   //  90
          new long[] {  1, 0, 0, 2, 1,     -10,    0,       0,    0 },   //  91
          new long[] {  0, 0, 4,-2, 2,      10,    0,       0,    0 },   //  92
          new long[] {  3, 0, 2,-2, 2,      10,    0,       0,    0 },   //  93
          new long[] {  1, 0, 2,-2, 0,     -10,    0,       0,    0 },   //  94
          new long[] {  0, 1, 2, 0, 1,      10,    0,       0,    0 },   //  95
          new long[] { -1,-1, 0, 2, 1,      10,    0,       0,    0 },   //  96
          new long[] {  0, 0,-2, 0, 1,     -10,    0,       0,    0 },   //  97
          new long[] {  0, 0, 2,-1, 2,     -10,    0,       0,    0 },   //  98
          new long[] {  0, 1, 0, 2, 0,     -10,    0,       0,    0 },   //  99
          new long[] {  1, 0,-2,-2, 0,     -10,    0,       0,    0 },   // 100
          new long[] {  0,-1, 2, 0, 1,     -10,    0,       0,    0 },   // 101
          new long[] {  1, 1, 0,-2, 1,     -10,    0,       0,    0 },   // 102
          new long[] {  1, 0,-2, 2, 0,     -10,    0,       0,    0 },   // 103
          new long[] {  2, 0, 0, 2, 0,      10,    0,       0,    0 },   // 104
          new long[] {  0, 0, 2, 4, 2,     -10,    0,       0,    0 },   // 105
          new long[] {  0, 1, 0, 1, 0,      10,    0,       0,    0 }    // 106
      };
        #endregion


        /// <summary>
        /// 计算章动经度倾角。
        /// Nutation in longitude and obliquity Nutation matrix
        /// </summary>
        /// <param name="Mjd_TT">地心力学时下的简化儒略日。 Mjd_TT    Modified Julian Date (Terrestrial Time)</param>
        /// <param name="dpsi"></param>
        /// <param name="deps"></param>
        static void NutAngles(double Mjd_TT, out double dpsi, out double deps)
        {
            // Constants
            double T = (Mjd_TT - MJD_J2000) / 36525.0;
            double T2 = T * T;
            double T3 = T2 * T;
            double rev = 360.0 * 3600.0;  // arcsec/revolution

            int N_coeff = 106;

            // Variables
            double l, lp, F, D, Om;
            double arg;

            // Mean arguments of luni-solar motion
            //
            //   l   mean anomaly of the Moon
            //   l'  mean anomaly of the Sun
            //   F   mean argument of latitude
            //   D   mean longitude elongation of the Moon from the Sun 
            //   Om  mean longitude of the ascending node

            l = MathUtil.Modulo(485866.733 + (1325.0 * rev + 715922.633) * T + 31.310 * T2 + 0.064 * T3, rev);
            lp = MathUtil.Modulo(1287099.804 + (99.0 * rev + 1292581.224) * T - 0.577 * T2 - 0.012 * T3, rev);
            F = MathUtil.Modulo(335778.877 + (1342.0 * rev + 295263.137) * T - 13.257 * T2 + 0.011 * T3, rev);
            D = MathUtil.Modulo(1072261.307 + (1236.0 * rev + 1105601.328) * T - 6.891 * T2 + 0.019 * T3, rev);
            Om = MathUtil.Modulo(450160.280 - (5.0 * rev + 482890.539) * T + 7.455 * T2 + 0.008 * T3, rev);

            // Nutation in longitude and obliquity [rad]
            deps = dpsi = 0.0;
            for (int i = 0; i < N_coeff; i++)
            {
                arg = (C[i][0] * l + C[i][1] * lp + C[i][2] * F + C[i][3] * D + C[i][4] * Om) / OrbitConsts.ArcSecondsPerRad;
                dpsi += (C[i][5] + C[i][6] * T) * Math.Sin(arg);
                deps += (C[i][7] + C[i][8] * T) * Math.Cos(arg);
            };

            dpsi = 1.0E-5 * dpsi / OrbitConsts.ArcSecondsPerRad;
            deps = 1.0E-5 * deps / OrbitConsts.ArcSecondsPerRad;
        }

        /// <summary>
        /// 从平赤道到真赤道春分点转换。
        ///  Transformation from mean to true equator and equinox
        /// </summary>
        /// <param name="Mjd_TT">地心力学时下的简化儒略日。 Mjd_TT    Modified Julian Date (Terrestrial Time)</param>
        /// <returns> Nutation matrix</returns>
        static public Matrix NutMatrix(double Mjd_TT)
        {
            double dpsi = 0, deps = 0, eps;
            // Mean obliquity of the ecliptic

            eps = MeanObliquity(Mjd_TT);
            // Nutation in longitude and obliquity

            NutAngles(Mjd_TT, out dpsi, out deps);

            // Transformation from mean to true equator and equinox
            return Matrix.RotateX3D(-eps - deps) * Matrix.RotateZ3D(-dpsi) * Matrix.RotateX3D(+eps);
        }

        /// <summary>
        /// 获取低精度的平向真赤道或春分点的转换。 Transformation from mean to true equator and equinox (low precision)
        /// </summary>
        /// <param name="Mjd_TT"> 地心力学时下的简化儒略日。 Modified Julian Date (Terrestrial Time)</param>
        /// <returns> Nutation matrix</returns>
        static public Matrix NutMatrixSimple(double Mjd_TT)
        {
            // Constants
            double T = (Mjd_TT - MJD_J2000) / 36525.0;

            // Variables
            double ls, D, F, N;
            double eps, dpsi, deps;

            // Mean arguments of luni-solar motion
            ls = OrbitConsts.TwoPI * MathUtil.Fraction(0.993133 + 99.997306 * T);   // mean anomaly Sun          
            D = OrbitConsts.TwoPI * MathUtil.Fraction(0.827362 + 1236.853087 * T);   // diff. longitude Moon-Sun  
            F = OrbitConsts.TwoPI * MathUtil.Fraction(0.259089 + 1342.227826 * T);   // mean argument of latitude 
            N = OrbitConsts.TwoPI * MathUtil.Fraction(0.347346 - 5.372447 * T);   // longit. ascending node    

            // Nutation angles
            dpsi = (-17.200 * Math.Sin(N) - 1.319 * Math.Sin(2 * (F - D + N)) - 0.227 * Math.Sin(2 * (F + N))
                     + 0.206 * Math.Sin(2 * N) + 0.143 * Math.Sin(ls)) / OrbitConsts.ArcSecondsPerRad;
            deps = (+9.203 * Math.Cos(N) + 0.574 * Math.Cos(2 * (F - D + N)) + 0.098 * Math.Cos(2 * (F + N))
                     - 0.090 * Math.Cos(2 * N)) / OrbitConsts.ArcSecondsPerRad;

            // Mean obliquity of the ecliptic
            eps = 0.4090928 - 2.2696E-4 * T;

            return Matrix.RotateX3D(-eps - deps) * Matrix.RotateZ3D(-dpsi) * Matrix.RotateX3D(+eps);
        }


        /// <summary>
        /// 从真赤道春分点系统到地球赤道和格林威治子午系统的转变
        ///  Transformation from true equator and equinox to Earth equator and   Greenwich meridian system 
        /// </summary>
        /// <param name="Mjd_UT1">地心力学时下的简化儒略日。 Modified Julian Date UT1</param>
        /// <returns>Greenwich Hour Angle matrix</returns>
        static public Matrix GreenwichHourAngleMatrix(double Mjd_UT1)
        {
            return Matrix.RotateZ3D(GetGastRad(Mjd_UT1));
        }
        /// <summary>
        /// 格林尼治视恒星时。 Greenwich Apparent Sidereal Time
        /// </summary>
        /// <param name="Mjd_UT1"> 地心力学时下的简化儒略日。 Modified Julian Date UT1</param>
        /// <returns> GMST in [rad]</returns>
        static public double GetGastRad(double Mjd_UT1)
        {
            return MathUtil.Modulo(GetGmstRad(Mjd_UT1) + EquationOfEquinox(Mjd_UT1), OrbitConsts.TwoPI);
        }


        /// <summary>
        /// 通过UT1计算格林尼治平太阳时。 Greenwich Mean Sidereal Time
        /// </summary>
        /// <param name="Mjd_UT1"> Mjd_UT1   Modified Julian Date UT1</param>
        /// <returns>  GMST in [rad]</returns>
        public static double GetGmstRad(double Mjd_UT1)
        {
            // Variables
            double gmst = GetGmstSeconds(Mjd_UT1);

            return OrbitConsts.TwoPI * MathUtil.Fraction(gmst / OrbitConsts.SecondsPerDay);       // [rad], 0..2pi
        }
        /// <summary>
        /// 通过UT1计算格林尼治平太阳时
        /// </summary>
        /// <param name="Mjd_UT1"></param>
        /// <returns> GMST in seconds</returns>
        public static double GetGmstSeconds(double Mjd_UT1)
        {
            double Mjd_0, UT1, T_0, T, gmst;

            // Mean Sidereal Time
            Mjd_0 = Math.Floor(Mjd_UT1);
            UT1 = OrbitConsts.SecondsPerDay * (Mjd_UT1 - Mjd_0);          // [s]
            T_0 = (Mjd_0 - MJD_J2000) / 36525.0;
            T = (Mjd_UT1 - MJD_J2000) / 36525.0;

            gmst = 24110.54841 + 8640184.812866 * T_0 + 1.002737909350795 * UT1
                    + (0.093104 - 6.2e-6 * T) * T * T; // [s]
            return gmst;
        }

        // Notes:
        //
        //   The equation of the equinoxes dpsi*cos(eps) is the right ascension of the 
        //   mean equinox referred to the true equator and equinox and is equal to the 
        //   difference between apparent and mean sidereal time.
        //
        //------------------------------------------------------------------------------
        /// <summary>
        /// 计算春分点Computation of the equation of the equinoxes
        /// 分点方程dpsi*cos(eps)是参考真实赤道和平春分点的右升角点，等于视时和平均恒星时差。
        /// </summary>
        /// <param name="Mjd_TT">地心力学时下的简化儒略日。 Mjd_TT    Modified Julian Date (Terrestrial Time)</param>
        /// <returns>Equation of the equinoxes</returns>
        static public double EquationOfEquinox(double Mjd_TT)
        {
            double dpsi = 0, deps = 0;              // Nutation angles

            // Nutation in longitude and obliquity 
            NutAngles(Mjd_TT, out dpsi, out deps);

            // Equation of the equinoxes
            return dpsi * Math.Cos(MeanObliquity(Mjd_TT));
        }
        #endregion
    }
}
