//2017.06.18, czs, funcKeyToDouble from c++ in hongqing, MathUtil
//2017.06.24, czs, edit in hongqing, format and refactor codes


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Geo.Algorithm;


namespace Geo.Utils
{
     
    /// <summary>
    /// 一些数学工具
    /// </summary>
    public static class MathUtil
    {
        /// <summary>
        /// 开根号
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static decimal Sqrt(decimal val)
        {
            return Convert.ToDecimal( Math.Sqrt(Convert.ToDouble(val)));
        }


        /// <summary>
        /// 二分法开根号
        /// </summary>
        /// <param name="n"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        static public double SqrtByBisection(double n, double eps = 1e-15)
        {
            double low, up, mid, last;
            low = 0;
            up = (n < 1 ? 1 : n);
            mid = (low + up) / 2;
            do
            {
                if (mid * mid > n)
                    up = mid;
                else
                    low = mid;
                last = mid;
                mid = (up + low) / 2;
            } while (Math.Abs(mid - last) > eps);

            return mid;
        } 
        /// <summary>
        /// 开方算法，牛顿法
        /// </summary>
        /// <param name="x"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        static public double SqrtByNewton(double x, double eps = 1e-15)
        {
            double val = x;//初始值
            double last;
            do
            {
                last = val;
                val = (val + x / val) / 2;
            } while (Math.Abs(val - last) > eps);
            return val;
        } 
        /// <summary>
        /// 获取正态分布函数值
        /// </summary>
        /// <param name="x"></param>
        /// <param name="u">中心值，期望值，均值</param>
        /// <param name="delta">标准差</param>
        /// <returns></returns>
        static public double GetNormalDistributionValue(double x,  double u, double delta)
        {
            var exponent = -Math.Pow(x - u, 2) / (2.0 * delta * delta);
            double y = 1.0 / Math.Sqrt(2.0 * Math.PI) * Math.Pow(Math.E, exponent);
            return y;
        }


        /// <summary>
        /// 一维正态分布密度函数
        /// </summary>
        /// <param name="x">参数</param>
        /// <param name="u">期望</param>
        /// <param name="d">方差</param>
        /// <returns></returns>
        public static double OneDimNormalDistribution(double x, double u, double d)
        {
            double exp = -1 * Math.Pow(x - u, 2.0) / ( 2.0 * d * d);
            return 1.0 / (Math.Sqrt(2.0 * Math.PI) * d) * Math.Pow(Math.E, exp);
        }
        

        /// <summary>
        /// 计算测站和卫星连线在电离层的穿刺点。实质为计算一条直线和球面的交点。
        /// </summary>
        /// <param name="siteXyz">卫星坐标</param>
        /// <param name="satXyz">测站坐标</param>
        /// <param name="radius">电离层圆半径，地心在地球</param>
        /// <returns></returns>
        public static double[] GetPuncturePoint(double[] siteXyz, double[] satXyz, double radius)
        {
            double x0 = siteXyz[0];
            double y0 = siteXyz[1];
            double z0 = siteXyz[2];

            double x1 = satXyz[0];

            double rx = satXyz[0] - siteXyz[0];
            double ry = satXyz[1] - siteXyz[1];
            double rz = satXyz[2] - siteXyz[2];

            double rx_2 = rx * rx;
            double ry_2 = ry * ry;
            double rz_2 = rz * rz;

            double a0 = rx_2 + ry_2 + rz_2;
            double b0 = 2.0 * (x0 * rx + y0 * ry + z0 * rz);
            double c0 = x0 * x0 + y0 * y0 + z0 * z0  - radius * radius;


            //求解
            double[] ts = SolveQuadraticEquation(a0, b0, c0);
            double t = ts[0];
            double x = x0 + rx * t;
            //取在中间的值
            if (x > Math.Max(x0, x1) || x < Math.Min(x0, x1))
            {
                t = ts[1];
                x = x0 + rx * t;
            }

            //求参数值           
            double y = y0 + ry * t;
            double z = z0 + rz * t;
            //check
            //var len = Math.Sqrt(x * x + y * y + z * z);
            //if (Math.Abs(len - radius) > 0.1)
            //{
            //    //呵呵，计算错误
            //    int i = 0;
            //}
            return new double[] { x, y, z };
        }

        /// <summary>
        /// 平方
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Square(double x) { return x * x; }

        /// <summary>
        /// 计算一元二次方程。ax²+bx+c=0（a≠0） x=[-b±√(b²-4ac)]/2a，quadratic equation of one unknown
        /// </summary>
        /// <param name="quadraticCoefficient">二次项系数 a</param>
        /// <param name="monomialCoefficient">一次项系数 b </param>
        /// <param name="constantTerm">常数项 c </param>
        /// <returns>2 个值前加后减</returns>
        public static double[] SolveQuadraticEquation(double quadraticCoefficient, double monomialCoefficient, double constantTerm)
        {
            double adjustTo = 1.0 / quadraticCoefficient;
            double a = quadraticCoefficient * adjustTo;
            double b = monomialCoefficient * adjustTo;
            double c = constantTerm * adjustTo;


            double delta = b * b - 4 * a * c;
            //if (delta < 0) { throw new ArgumentException("判别式小于 0 ，方程无实数解！"); }

            double latter = Math.Sqrt(delta);
            double[] result = new double[2];
            result[0] = (-b + latter) / (2 * a);
            result[1] = (-b - latter) / (2 * a);

            //double x = result[0];
            ////check
            //double zero = a * x * x + b * x + c;
            //if (zero != 0)
            //{
            //    throw new ArgumentException("二项式计算错误结果不能算回 0！"); 
            //   // int ll = 0;
            //}  

            return result;
        }
        /// <summary>
        /// 取小数部分，始终大于等于0.
        /// Fractional part of a number (y=x-[x])
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Fraction(double x) { return x - Math.Floor(x); }


        /// <summary>
        ///  x mod y 模
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double Modulo(double x, double y) { return y * Fraction(x / y); }

    }

    /// <summary>
    /// 积分工具
    /// </summary>
    public class IntegralUtil
    {
        /// <summary>
        /// 梯形公式
        /// </summary>
        /// <param name="fun">被积函数</param>
        /// <param name="up">积分上限</param>
        /// <param name="down">积分下限</param>
        /// <returns>积分值</returns>

        public static double TiXing(Func<double, double> fun, double up, double down)
        {
            return (up - down) / 2 * (fun(up) + fun(down));
        }
        /// <summary>
        /// 辛普森公式
        /// </summary>
        /// <param name="fun">被积函数</param>
        /// <param name="up">积分上限</param>
        /// <param name="down">积分下限</param>
        /// <returns>积分值</returns>
        public static double Simpson(Func<double, double> fun, double up, double down)
        {
            return (up - down) / 6 * (fun(up) + fun(down) + 4 * fun((up + down) / 2));
        }
        /// <summary>
        /// 科特克斯公式
        /// </summary>
        /// <param name="fun">被积函数</param>
        /// <param name="up">积分上限</param>
        /// <param name="down">积分下限</param>
        /// <returns>积分值</returns>
        public static double Cotes(Func<double, double> fun, double up, double down)
        {
            double C = (up - down) / 90 * (7 * fun(up) + 7 * fun(down) + 32 * fun((up + 3 * down) / 4)
                     + 12 * fun((up + down) / 2) + 32 * fun((3 * up + down) / 4));
            return C;
        }

        /// <summary>
        /// 复化梯形公式
        /// </summary>
        /// <param name="fun">被积函数</param>
        /// <param name="N">区间划分快数</param>
        /// <param name="up">积分上限</param>
        /// <param name="down">积分下限</param>
        /// <returns>积分值</returns>
        public static double FuHuaTiXing(Func<double, double> fun, int N, double up, double down)
        {
            double h = (up - down) / N;
            double result = 0;
            double x = down;
            for (int i = 0; i < N - 1; i++)
            {
                x += h;
                result += fun(x);
            }
            result = (fun(up) + result * 2 + fun(down)) * h / 2;
            return result;
        }

        /// <summary>
        /// 复化辛浦生公式
        /// </summary>
        /// <param name="fun">被积函数</param>
        /// <param name="N">区间划分快数</param>
        /// <param name="up">积分上限</param>
        /// <param name="down">积分下限</param>
        /// <returns>积分值</returns>
        public static double FSimpson(Func<double, double> fun, int N, double up, double down)
        {
            double h = (up - down) / N;
            double result = 0;
            for (int n = 0; n < N; n++)
            {
                result += h / 6 * (fun(down) + 4 * fun(down + h / 2) + fun(down + h));
                down += h;
            }
            return result;
        }
        /// <summary>
        /// 复化科特斯公式
        /// </summary>
        /// <param name="fun">被积函数</param>
        /// <param name="N">区间划分快数</param>
        /// <param name="up">积分上限</param>
        /// <param name="down">积分下限</param>
        /// <returns>积分值</returns>
        public static double FCotes(Func<double, double> fun, int N, double up, double down)
        {
            double h = (up - down) / N;
            double result = 0;
            for (int n = 0; n < N; n++)
            {
                result += h / 90 * (7 * fun(down) + 32 * fun(down + h / 4) + 12 * fun(down + h / 2) +
                    32 * fun(down + 3 * h / 4) + 7 * fun(down + h));
                down += h;
            }
            return result;
        }
        /// <summary>
        /// 龙贝格算法
        /// </summary>
        /// <param name="fun">被积函数</param>
        /// <param name="e">结果精度</param>
        /// <param name="up">积分上限</param>
        /// <param name="down">积分下限</param>
        /// <returns>积分值</returns>
        public static double Romberg(Func<double, double> fun, double e, double up, double down)
        {
            double R1 = 0, R2 = 0;
            int k = 0; //2的k次方即为N（划分的子区间数）
            R1 = (64 * C(fun, 2 * (int)Math.Pow(2, k), up, down) - C(fun, (int)Math.Pow(2, k++), up, down)) / 63;
            R2 = (64 * C(fun, 2 * (int)Math.Pow(2, k), up, down) - C(fun, (int)Math.Pow(2, k++), up, down)) / 63;
            while (Math.Abs(R2 - R1) > e)
            {
                R1 = R2;
                R2 = (64 * C(fun, 2 * (int)Math.Pow(2, k), up, down) - C(fun, (int)Math.Pow(2, k++), up, down)) / 63;
            }
            return R2;
        }
        private static double S(Func<double, double> fun, int N, double up, double down)
        {
            return (4 * FuHuaTiXing(fun, 2 * N, up, down) - FuHuaTiXing(fun, N, up, down)) / 3;
        }
        private static double C(Func<double, double> fun, int N, double up, double down)
        {
            return (16 * S(fun, 2 * N, up, down) - S(fun, N, up, down)) / 15;
        }
    }
}
