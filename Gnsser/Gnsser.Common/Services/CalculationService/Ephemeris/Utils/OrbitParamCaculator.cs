
//2017.06.19, czs, edit in hongqing, 采用牛顿法改进开普勒方程计算方法

using System;
using Geo.Coordinates; 
using Gnsser.Times;
using System.Collections.Generic;
using System.Text;
using System.IO; 

namespace Gnsser
{
    /// <summary>
    /// 轨道参数计算器。
    /// </summary>
    public class OrbitParamCaculator
    {

        public OrbitParam CaculateOrbitParam(XYZ pos, XYZ dot, double time)
        {
            double u = 3.986005e14;
            //double sigma = 1e-6;

            XYZ cross = pos.Cross(dot);
            double A = cross.X;
            double B = cross.Y;
            double C = cross.Z;

            double sqrtAB = Math.Sqrt(A * A + B * B);
            //
            //角动量公式,可推出轨道倾角i
            double i = Math.Atan2(sqrtAB, C);//轨道倾角

            double bigOmiga = Math.Atan2(-A, B);//升点赤经 的值

            double r0 = pos.Length;
            double v0_2 = dot.Length * dot.Length;
            //由能量方程,可推出半长轴a的值
            double a = 1.0 / (2 / r0 - v0_2 / u);//轨道长半径
            double n = Math.Sqrt(u / a / a / a);//周期

            double sqrta = Math.Sqrt(a);
            double sqrtu = Math.Sqrt(u);


            double eSinE = pos.Dot(dot) / sqrta / sqrtu;
            double eCosE = 1 - r0 / a;


            double e = Math.Sqrt(Math.Pow(eSinE, 2.0) + Math.Pow(eCosE, 2.0));//偏心率

            double E = Math.Atan2(eSinE, eCosE);

            double t = time;

            //平近点角M与偏近点角E
            //迭代
            double M = E - eSinE;//平近点角
            double tao = t - M / n;

            //求出 E 么，偏近点角。

            double tan2f = Math.Sqrt((1 + e) / (1 - e)) * Math.Tan(E / 2.0);
            double f = Math.Atan(tan2f) * 2;
            double smallOmiga = Math.Atan2(pos.Z / Math.Sin(i), pos.X * Math.Cos(bigOmiga) + pos.Y * Math.Sin(bigOmiga)) - f;

            return new OrbitParam()
            {
                Eccentricity = e,
                LongOfAscension = bigOmiga,
                SemiMajor = a,
                EpochOfPerigee = tao,
                Inclination = i,
                MeanAnomaly = M,
                ArgumentOfPerigee = smallOmiga
            };
        }

        public XYZ GetSatPos(OrbitParam param, double time, Geo.Referencing.Ellipsoid ellipsoid = null)
        {
            if (ellipsoid == null)
            {
                ellipsoid = Geo.Referencing.Ellipsoid.WGS84;
            }

            double u =  ellipsoid.GM;
            double a = param.SemiMajor;
            double tao = param.EpochOfPerigee;
            double e = param.Eccentricity;
            double omega = param.ArgumentOfPerigee;
            double bigOmega = param.LongOfAscension;
            double i = param.Inclination;
            //1.由 a 计算平均角速度
            double n = Math.Sqrt(u / a / a / a);
            //2.计算平近点角 M
            double M = n * (time - tao);
            //3.计算偏近点角，解开普勒方程。
            double E = KeplerEqForEccAnomaly(M, e);

            double sinE = Math.Sin(E);
            double cosE = Math.Cos(E);

            //4.计算矢径 r 和真近点角 f。则已经在极坐标中确定了卫星位置
            double r = a * (1.0 - e * Math.Cos(E));
            double f = 2.0 * Math.Atan(Math.Sqrt((1 + e) / (1 - e)) * Math.Tan(E / 2));
            //5.计算轨道平面坐标（x’,y’） 
            double theta = f + omega;
            double x1 = r * Math.Cos(theta);
            double y1 = r * Math.Sin(theta);
            XYZ xyz1 = new XYZ(x1, y1, 0);
            //6.计算天球空间直角坐标（x，y，z） 
            XYZ rotation = new XYZ(-i, 0, -bigOmega);
            XYZ xyz = xyz1.Rotate(rotation);
            //7.计算瞬时地球坐标系
            //Z轴旋转对应时刻的平格林尼治子午面的春分点时角 

            //8.计算到协议地球坐标系里,如WGS84


            return xyz;
        }

 

        private static double KeplerEqForEccAnomaly1(EphemerisParam eph, double M)
        {
            int n;
            double Ek, E1;
            for (n = 0, E1 = M, Ek = 0.0; Math.Abs(E1 - Ek) > 1e-14; n++)
            {
                Ek = E1;
                E1 -= (E1 - eph.Eccentricity * Math.Sin(E1) - M) / (1.0 - eph.Eccentricity * Math.Cos(E1));
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
        /// 开普勒方程 for 偏心改正。
        /// solve for eccentric anomaly given mean anomaly and orbital eccentricity
        /// use simple fixed point iteration of kepler's equation
        /// </summary>
        /// <param name="em">M 平近点角</param>
        /// <param name="e">椭圆轨道的偏心率</param>
        /// <returns></returns>
        public static double KeplerEqForEccAnomalyFixtPoint(double em, double e)
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
            } while (Math.Abs((ecca - ecca0) / ecca) > 1.0e-14 && counter < 25);
            return ecca;
        }

    }
}