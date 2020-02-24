//2017., czs, create in hongqing, 勒让德函数递推


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gnsser.Service;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Gnsser.Times;
using Geo.Utils;
using Gnsser;
using Geo.Times;
using Geo;
using Geo.IO;

namespace Gnsser.Data
{
    #region  勒让德函数递推
    /// <summary>
    /// 勒让德函数递推
    /// </summary>
    public class FastLegendreCalculater
    {
        Log log = new Log(typeof(SphericalHarmonicsCalculater));
        /// <summary>
        /// 勒让德函数递推
        /// </summary>
        /// <param name="Theta">天顶距？？</param>
        public FastLegendreCalculater(double Theta)
        {
            this.theta = Theta;

            Storage = new TwoKeyDictionary<int, int, double>();

        } 
        /// <summary>
        /// const
        /// </summary>
        public const double GM = 3986004.415E8;
        /// <summary>
        /// 文件
        /// </summary>
        private SphericalHarmonicsFile File { get; set; }
        /// <summary>
        /// m
        /// </summary>
        public const double a = 6378136.3;
        public const double R = 6378136.3;
        /// <summary>
        /// 天顶距
        /// </summary>
        public double theta { get; set; }

        TwoKeyDictionary<int, int, double> Storage { get; set; }


        #region 测试

        private void TestLegendreFunctions(double phi)
        {
            DateTime Start = DateTime.Now;

            double theta = Math.PI / 2.0 - phi;
            int order = 17;
            Start = DateTime.Now;
            var one = CheckLegendre(order, theta, LegendreCosTheta);
            var time1 = DateTime.Now - Start;
            log.Info(one + ", time1 : " + time1);
            Start = DateTime.Now;
            var two = CheckLegendre(order, theta, LegendreStrideOrder);
            var time2 = DateTime.Now - Start;
            log.Info(two + ",time2 : " + time2);
            Start = DateTime.Now;
            var three = CheckLegendre(order, phi, LegendreSinPhi);
            var time3 = DateTime.Now - Start;
            log.Info(three + ",time3 : " + time3);
            Start = DateTime.Now;
            var four = CheckLegendre(order, theta, LegendreBelikov);
            var time4 = DateTime.Now - Start;
            log.Info(four + ",time4 : " + time4);
        }
        /// <summary>
        /// 检核勒让德精度
        /// </summary>
        /// <param name="n"></param>
        /// <param name="theta"></param>
        /// <param name="Legendre"></param>
        /// <returns></returns>
        public static double CheckLegendre(int n, double theta, Func<int, int, double, double> Legendre)
        {
            double sum = 0;

            for (int m = 0; m < n; m++)
            {
                double p = Legendre(n, m, theta);
                double val = p * p;
                sum += val;
            }
            double shouldBe = 2 * n + 1;
            double result = Math.Abs(shouldBe - sum) / shouldBe;
            return result;
        }
        #endregion

        #region  一个老算法
        public double GetValueOld(int n, int m)
        {
            var store = Storage;
            if (store.ContainsKeyAB(n, m))
            {
                return store.Get(n, m);
            }

            double result = 1;
            if (n < m) { return 0; }
            else if (n <= 0 && m <= 0)
            {
                result = 1;
            }
            else if (n == 1 && m <= 0)
            {
                result = Math.Sqrt(3.0) * Math.Cos(theta);
            }
            else if (n == 1 && m == 1)
            {
                result = Math.Sqrt(3.0) * Math.Sin(theta);
            }
            else if (n == m)//已检查
            {
                double cm = 1;
                if (m == 1) { cm = Math.Sqrt(3.0); } else { cm = Math.Sqrt((2.0 * m + 1.0) / (2.0 * m)); }
                result = cm * Math.Sin(theta) * GetValue(m - 1, m - 1);
            }
            else if (n > m)
            {
                double anm = Math.Sqrt((2.0 * n - 1.0) * (2.0 * n + 1.0) / ((n - m) * (n + m)));
                double bnm = Math.Sqrt((2.0 * n + 1.0) * (n + m - 1.0) * (n - m - 1.0) / ((2.0 * n - 3) * (n + m) * (n - m)));
                result = anm * Math.Cos(theta) * GetValue(n - 1, m) - bnm * GetValue(n - 2, m);
            }

            store.Set(n, m, result);
            return result;

            throw new ArgumentException("你不应该看到我的。应该 n >= m, n= " + n + ", m= " + m);
        }
        /// <summary>
        /// 取值
        /// </summary>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public double GetValue(int n, int m)
        {
            var store = Storage;
            if (store.ContainsKeyAB(n, m))
            {
                return store.Get(n, m);
            }

            double[] all = Leg(n, theta);
            for (int mi = 0; mi < all.Length; mi++)
            {
                store.Set(n, mi, all[mi]);
            } 

            return all[m];
        }
        #endregion


        #region 勒让德 行推法  //完全规格化缔合勒让德函数及其导数的递推算法与适用范围比较_雷伟伟

        /// <summary>
        /// 新方法，返回数组，下标对应m
        /// </summary>
        /// <param name="n"></param>
        /// <param name="theta"></param>
        /// <returns></returns>
        static public double[] Leg(int n, double theta)
        {
            double[] vals = new double[n + 1];
            double top = legDiagonal(n, theta);
            vals[n] = top;
            double prev1 = top;
            double prev2 = 0;
            for (int m = n - 1; m >= 0; m--)
            {
                vals[m] = legMsmallThanN(n, m, theta, prev1, prev2);
                prev1 = vals[m];
                prev2 = vals[m + 1];
            }
            return vals;
        }
        /// <summary>
        /// 如果M小于N时
        /// </summary>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <param name="theta"></param>
        /// <param name="next1"></param>
        /// <param name="next2"></param>
        /// <returns></returns>
        static double legMsmallThanN(int n, int m, double theta, double next1, double next2)
        {
            if (m < 0) return 0;

            if (m < n)
            {
                if (n == 1 && m == 0)
                {
                    return sqrt(3.0) * cos(theta);
                }
                else if (n >= 2)
                {
                    return (1 / sqrt(j_m(m))) * (g(n, m) * (cos(theta) / sin(theta))
                        * next1 - h(n, m)//*Leg(n, m + 1, theta) - h(n, m)
                        * next2); //	*Leg(n, m + 2, theta));
                }
            }
            else if (m == n)
            {
                return legDiagonal(m, theta);
            }
            if (m > n)
            {
                return 0;
            }
            return 0.0;
        }

        /// <summary>
        /// 刚好在对角线上时
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="theta"></param>
        /// <returns></returns>
        static double legDiagonal(int dimension, double theta)
        {
            double prevDiagonal = 0;
            for (int i = 0; i <= dimension; i++)
            {
                prevDiagonal = legDiagonal(i, theta, prevDiagonal);
            }
            return prevDiagonal;
        }
 
        /// <summary>
        /// 刚好在对角线上时dimension==n
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="theta"></param>
        /// <param name="prevDiagonal"></param>
        /// <returns></returns>
        static double legDiagonal(int dimension, double theta, double prevDiagonal)
        {
            if (dimension == 1)
            {
                return sqrt(3.0) * sin(theta);
            }
            else if (dimension == 0)
            {
                return 1.0;
            }
            else if (dimension >= 2)
            {
                return c(dimension) * sin(theta) * prevDiagonal;
                //	Leg(dimension - 1, dimension - 1, theta);
            }
            return 0.0;
        }

        //系数公式
        static double g(int n, int m)
        {
            double upper = 2.0 * (m + 1.0);
            double lower = sqrt((n - m) * (n + m + 1.0));
            return upper / lower;
        }

        static double h(int n, int m)
        {
            double upper = (n + m + 2.0) * (n - m - 1.0);
            double lower = (n - m) * (n + m + 1.0);
            return sqrt(upper / lower);
        }

        static double c(int m)
        {
            double temp_c = sqrt((2.0 * m + 1.0) / (2.0 * m));
            return temp_c;
        }

        static double j_m(int m)
        {
            if (m == 0)
                return 2.0;
            return 1.0;
        }

        #endregion
        

        #region Belikov        
        /// <summary>
        /// Belikov 列推法
        /// </summary>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <param name="theta"></param>
        /// <returns></returns>
        public static double LegendreBelikov(int n, int m, double theta)
        {
            double p = Math.Sqrt(2.0 * n + 1) * GetN(n, m) * BelikovNotMormal(n, m, theta);
            return p;
        }

        /// <summary>
        /// ｐｎｍ （ｃｏｓθ）是非正常化的勒让德函数
        /// </summary>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <param name="theta"></param>
        /// <returns></returns>
        public static double BelikovNotMormal(int n, int m, double theta)
        {
            if (n < m) { return 0; }

            if (n == 0 && m == 0) { return 1; }
            double t = Math.Sin(theta);
            double u = Math.Cos(theta);

            if (m == 0)
            {
                double p = t * BelikovNotMormal(n - 1, 0, theta)
                    - 0.5 * u * BelikovNotMormal(n - 1, 1, theta);
                return p;
            }

            if (m > 0)
            {
                double p = t * BelikovNotMormal(n - 1, m, theta)
                    - u * (
                            0.25 * BelikovNotMormal(n - 1, m + 1, theta)
                            - BelikovNotMormal(n - 1, m - 1, theta)
                         );
                return p;
            }

            throw new ArgumentException("你不应该看到我的。应该 n >= m, n= " + n + ", m= " + m);
        }


        private static double GetN(int n, int m)
        {
            if (n <= 1 && m <= 1)
            {
                return 1.0;
            }

            if (n >= 2 && n == m)
            {
                double q = Math.Sqrt(1.0 - 1.0 / (2.0 * n)) * GetN(n - 1, m - 1);
                return q;
            }

            if (n >= 2 && m >= 0 && m <= n - 1)
            {
                double p = Math.Sqrt(1.0 - 1.0 * m * m / (n * n)) * GetN(n - 1, m);
                return p;
            }

            throw new ArgumentException("你不应该看到我的。应该 n >= m, n= " + n + ", m= " + m);
        }
        #endregion


        /// <summary>
        /// 跨阶次递推法
        /// </summary>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <param name="theta"></param>
        /// <returns></returns>
        public double LegendreStrideOrder(int n, int m, double theta)
        {
            if (n < m) { return 0; }
            if (m < 2)//当m= 0、1时,采用基本递推公式(4)计算
            {
                return LegendreCosTheta(n, m, theta);
            }

            //m>2
            double mplusn = m + n;
            double nminusm = n - n;
            double tmp = Math.Sqrt(1.0 + GetDeltaValue(m - 2));
            double tmp1 = mplusn * (mplusn - 1.0);
            double tmp2 = ((2.0 * n - 3.0) * tmp1);
            double anm = Math.Sqrt(
                (2.0 * n + 1.0) * nminusm * (nminusm - 1.0) / tmp2
                );
            double bnm = tmp * Math.Sqrt(
                (2.0 * n + 1.0) * (mplusn - 2.0) * (mplusn - 3.0) /
                tmp2
                );
            double cnm = tmp * Math.Sqrt(
                (nminusm + 1.0) * (nminusm + 2.0) /
                tmp1
                );

            double result = anm * LegendreStrideOrder(n - 2, m, theta)
               + bnm * LegendreStrideOrder(n - 2, m - 2, theta)
               - cnm * LegendreStrideOrder(n, m - 2, theta);
            return result;

            throw new ArgumentException("你不应该看到我的。应该 n >= m, n= " + n + ", m= " + m);
        }

        /// <summary>
        /// 计算 σｍ
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private static double GetDeltaValue(int m)
        {
            if (m == 0) return 1.0;
            return 0.0;
        }


        //[1]佚名. 多种超高阶次缔合勒让德函数计算方法的比较_吴星[J]. 刊名缺失, 出版年缺失, 卷缺失(期缺失): 页码范围缺失.

        /// <summary>
        /// 大地测量中经常使用的是标准前向列推法.
        /// 可以看出,当m接近于n时,有anm≈ n。因此,当cosθ接近1,n取值较大时,m取值从1到n,相应的递推系数(anmcosθ)较大,
        /// 同样,不得不考虑递推过程中计算舍入误差的传递问题。
        /// 故此,就必须考虑改进上述递推公式或改变递推路径
        /// </summary>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <param name="theta"></param>
        /// <returns></returns>
        public double LegendreCosTheta(int n, int m, double theta)
        {

            double result = 1;
            if (n < m) { return 0; }
            else if (n <= 0 && m <= 0)
            {
                result = 1;
            }
            else if (n == 1 && m <= 0)
            {
                result = Math.Sqrt(3.0) * Math.Cos(theta);
            }
            else if (n == 1 && m == 1)
            {
                result = Math.Sqrt(3.0) * Math.Sin(theta);
            }
            else if (n == m)//已检查
            {
                double cm = 1;
                if (m == 1) { cm = Math.Sqrt(3.0); } else { cm = Math.Sqrt((2.0 * m + 1.0) / (2.0 * m)); }
                result = cm * Math.Sin(theta) * LegendreCosTheta(m - 1, m - 1, theta);
            }
            else if (n > m)
            {
                double anm = Math.Sqrt((2.0 * n - 1.0) * (2.0 * n + 1.0) / ((n - m) * (n + m)));
                double bnm = Math.Sqrt((2.0 * n + 1.0) * (n + m - 1.0) * (n - m - 1.0) / ((2.0 * n - 3) * (n + m) * (n - m)));
                result = anm * Math.Cos(theta) * LegendreCosTheta(n - 1, m, theta) - bnm * LegendreCosTheta(n - 2, m, theta);
            }

            throw new ArgumentException("你不应该看到我的。应该 n >= m, n= " + n + ", m= " + m);
        }

        /// <summary>
        /// !!!计算错误！！！！！2017.11.07， czs
        /// 标准前向行推法,勒让德函数递推,天顶距，单位rad.
        /// 依据(1)～ (3)式便可求出任意阶次的 Pnm(cosθ)。但当θ接近两极极点,并且n、m取值较大时,递推系数(gnm cotθ)较大,
        /// 这就不得不考虑递推过程中计算舍入误差的传递问题,亦即能否满足精度要求。
        /// (1)式的递推是极其不稳定的,因此该递推方法在大地测量中运用较少。
        /// </summary>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <param name="theta">天顶距，单位rad</param>
        /// <returns></returns>
        static public double LegendreCosThetaFormRow(int n, int m, double theta)
        {
            if (n < m) { return 0; }
            if (n == 0 && m == 0)
            {
                return 1;
            }
            if (n == 1 && m == 0)
            {
                return Math.Sqrt(3.0) * Math.Cos(theta);
            }
            if (n == 1 && m == 1)
            {
                return Math.Sqrt(3.0) * Math.Sin(theta);
            }
            if (n > m)//已检查
            {
                double del = m == 0 ? 1 : 0;
                double gnm = 2.0 * (m + 1.0) / Math.Sqrt((n - m) * (n - m + 1.0));
                double hnm = Math.Sqrt(((n + m + 2.0) * (n - m - 1.0)) / ((n + m + 1.0) * (n - m)));

                return Math.Sqrt(1.0 / (1.0 + del)) * (
                    gnm * CTan(theta) * LegendreCosThetaFormRow(n, m + 1, theta)
                    - hnm * LegendreCosThetaFormRow(n, m + 2, theta)
                    );
            }
            if (n == m)//已检查
            {
                double cm = 1;
                if (m == 1) { cm = Math.Sqrt(3.0); } else { cm = Math.Sqrt((2.0 * m + 1.0) / (2.0 * m)); }
                return cm * Math.Sin(theta) * LegendreCosThetaFormRow(m - 1, m - 1, theta);
            }
            throw new ArgumentException("你不应该看到我的。应该 n >= m, n= " + n + ", m= " + m);
        }

        /// <summary>
        /// 勒让德多项式函数，SIN 高度角
        /// </summary>
        /// <param name="n">N</param>
        /// <param name="m">M</param>
        /// <param name="phi">此处为SIN 则是高度角，非天顶距，单位： rad</param>
        /// <returns></returns>
        static public double LegendreSinPhi(int n, int m, double phi)
        {
            double result = 1;
            if (n == 0 && m == 0)
            {
                result = 1.0;
            }
            else if (n == 1 && m == 0)
            {
                result = Math.Sqrt(3.0) * Math.Sin(phi);
            }
            else if (n == 1 && m == 1)
            {
                result = Math.Sqrt(3.0) * Math.Cos(phi);
            }
            else if (n == m)
            {
                var temp1 = (2.0 * n + 1) / (2.0 * n);
                result = Math.Sqrt(temp1) * Math.Cos(phi) * LegendreSinPhi(n - 1, n - 1, phi);
            }
            else if (n < m)
            {
                result = 0.0;
            }
            else
            {
                double denominator = (n * n - m * m);
                double temp1 = (4.0 * n * n - 1.0) / denominator;
                double temp2 = (2.0 * n + 1.0) / (2.0 * n - 3.0);
                double temp3 = ((n - 1.0) * (n - 1.0) - m * m) / denominator;

                result = Math.Sqrt(temp1) * Math.Sin(phi) * LegendreSinPhi(n - 1, m, phi)
                    - Math.Sqrt(temp2 * temp3) * LegendreSinPhi(n - 2, m, phi);
            }
            return result;
        }

        //勒让德多项式函数
        double Leg(int n, int m, double phi)
        {
            if (n < m)
            {
                return 0.0;
            }
            if (n == 0 && m == 0)
            {
                return 1.0;
            }

            if (n == 1 && m == 0)
            {
                return sqrt(3.0) * sin(phi);
            }

            if (n == 1 && m == 1)
            {
                return sqrt(3.0) * cos(phi);
            }

            if (n == m)
            {
                return (sqrt((2.0 * n + 1) / (2 * n))) * cos(phi) * Leg(n - 1, n - 1, phi);
            }
            if (n > m)
            {
                return ((sqrt(((4.0 * n * n) - 1) / (n * n - m * m))) * sin(phi) * Leg(n - 1, m, phi)) - (sqrt(((2.0 * n + 1) / (2.0 * n - 3)) * (((n - 1) * (n - 1) - m * m) / (n * n - m * m))) * Leg(n - 2, m, phi));
            }

            throw new Exception("错误！");
        }
        /// <summary>
        /// 计算勒让德导数函数
        /// </summary>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <param name="phi"></param>
        /// <returns></returns>
        double D_leg(int n, int m, double phi)
        {
            double temp2;
            temp2 = (sqrt(((2.0 * n + 1) / (2.0 * n - 1)) * (n * n - m * m))) * (Leg(n - 1, m, phi) / cos(phi)) - n * tan(phi) * Leg(n, m, phi);
            return temp2;
        }



        //计算向径rou方向重力分量
        double sigma_g_rou(double r, double phi, double lammbda, double N_degree)
        {
            double temp_rou0 = 0.0, temp_rou1 = 0.0, temp_rou2 = 0.0, temp_rou3 = 0.0, temp_3;
            for (int n = 2; n <= N_degree; n++)
            {
                var nStep = File[n];
                for (int m = 0; m <= n; m++)
                {
                    double Cnm = nStep.C[m].Value;
                    double Snm = nStep.S[m].Value;

                    temp_rou0 = (Cnm * cos(m * lammbda) + Snm * sin(m * lammbda)) * Leg(n, m, phi);
                    temp_rou1 = temp_rou0 + temp_rou1;
                }
                temp_rou2 = temp_rou1 * (n + 1) * pow((R / r), n);
                temp_rou3 = temp_rou2 + temp_rou3;
            }
            temp_3 = (-GM / (r * r)) * temp_rou3;
            return temp_3;
        }
        //计算phi方向的重力分量
        double sigma_g_phi(double r, double phi, double lammbda, int N_degree)
        {
            double temp_phi0 = 0.0, temp_phi1 = 0.0, temp_phi2 = 0.0, temp_phi3 = 0.0, temp_4;
            for (int n = 2; n <= N_degree; n++)
            {
                var nStep = File[n];
                for (int m = 0; m <= n; m++)
                {
                    double Cnm = nStep.C[m].Value;
                    double Snm = nStep.S[m].Value;
                    temp_phi0 = (Cnm * cos(m * lammbda) + Snm * sin(m * lammbda)) * D_leg(n, m, phi);
                    temp_phi1 = temp_phi0 + temp_phi1;
                }
                temp_phi2 = temp_phi1 * pow((R / r), n);
                temp_phi3 = temp_phi2 + temp_phi3;
            }
            temp_4 = (GM / (r * r)) * temp_phi3;
            return temp_4;
        }
        //计算lammbda方向的重力分量
        double sigma_g_lammbda(double r, double phi, double lammbda, int N_degree)
        {
            double temp_lammbda0 = 0.0, temp_lammbda1 = 0.0, temp_lammbda2 = 0.0, temp_lammbda3 = 0.0, temp_5;
            for (int n = 2; n <= N_degree; n++)
            {
                var nStep = File[n];
                for (int m = 0; m <= n; m++)
                {
                    double Cnm = nStep.C[m].Value;
                    double Snm = nStep.S[m].Value;

                    temp_lammbda0 = m * (Snm * cos(m * lammbda) - Cnm * sin(m * lammbda)) * Leg(n, m, phi);
                    temp_lammbda1 = temp_lammbda0 + temp_lammbda1;
                }
                temp_lammbda2 = temp_lammbda1 * pow((R / r), n);
                temp_lammbda3 = temp_lammbda2 + temp_lammbda3;
            }
            temp_5 = (GM / ((r * r) * cos(phi))) * temp_lammbda3;
            return temp_5;
        }

        //余切函数的补充
        private static double CTan(double x)
        {
            return 1.0 / Math.Tan(x);
        }
        static double pow(double val, double order) { return Math.Pow(val, order); }
        static double cos(double val) { return Math.Cos(val); }
        static double sin(double val) { return Math.Sin(val); }
        static double tan(double val) { return Math.Tan(val); }
        static double sqrt(double val) { return Math.Sqrt(val); }
        #endregion


    }
}