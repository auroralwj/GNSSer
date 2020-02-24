//2014.06.09 editing by cuiyang. 增加偏倒项
//2015.04.17, cy, edit , 优化 GetNearstIndexes 方法

using System;
using System.Collections.Generic;
using System.Text;

namespace Geo.Algorithm
{
    /// <summary>
    /// 拉格朗日多项式插值 
    /// </summary>
    public class LagrangeInterplation : IGetY
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="yArray"></param>
        /// <param name="order"></param>
        public LagrangeInterplation(double[] yArray, int order = 10)
        {
            double[] xArray = new double[yArray.Length];
            for (int i = 0; i < yArray.Length; i++)
            {
                xArray[i] = i;
            }

            this.XArray = xArray;
            this.YArray = yArray;
            this.XCount = xArray.Length;
            this.Order = order;
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xArray">应该是递增的值</param>
        /// <param name="yArray">函数值</param>
        /// <param name="order">阶次</param>
        public LagrangeInterplation(double[] xArray, double[] yArray, int order = 10)
        {
            this.XArray = xArray;
            this.YArray = yArray;
            this.XCount = xArray.Length;
            this.Order = order;
        }

        /// <summary>
        /// X各点坐标组成的数组
        /// </summary>
        public double[] XArray { get; set; }

        /// <summary>
        /// X各点对应的Y坐标值组成的数组
        /// </summary>
        public double[] YArray { get; set; }
        /// <summary>
        /// 多项式的阶次
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// x数组或者y数组中元素的个数, 注意两个数组中的元素个数需要一样
        /// </summary>
        public int XCount { get; set; }

        /// <summary>
        /// 插值
        /// </summary>
        /// <param name="xValue"></param>
        /// <returns></returns>
        public double GetY(double xValue)
        {
            List<int> indexes = Geo.Utils.DoubleUtil.GetNearstIndexes(XArray, xValue, Order);

            List<double> xList = new List<double>();
            List<double> yList = new List<double>();
            foreach (var item in indexes)
            {
                xList.Add(XArray[item]);
                yList.Add(YArray[item]);
            }

            double y = Lagrange(xList.ToArray(), yList.ToArray(), xValue);
            return y;
        }

        /// <summary>
        /// 插值，返回 the value of Y(x) and dY(X)/dX
        /// Assumes that xValue is between X[k-1] and X[k] ,where k=N/2
        /// Warning: for use with the precise(sp3) ephemeris only when velocity is not avilable estimates of velocity , and especially clock drift, not as accurate.
        /// Cui Yang, 2014.06.09
        /// </summary>
        /// <param name="xValue"></param>
        /// <param name="Y">位置</param>
        /// <param name="dYdX">速度，单位Km</param>
        public void GetYdY(double xValue, ref double Y, ref double dYdX)
        {
            List<int> indexes = Geo.Utils.DoubleUtil.GetNearstIndexes(XArray, xValue, Order);

            List<double> xList = new List<double>();
            List<double> yList = new List<double>();
            foreach (var item in indexes)
            {
                xList.Add(XArray[item]);
                yList.Add(YArray[item]);
            }
            double y = 0.0;
            double dydx = 0.0;
            Lagrange(xList.ToArray(), yList.ToArray(), xValue, ref y, ref dydx);
            Y = y;
            dYdX = dydx;
        }

        public void GetYdY(double xValue, double xValue1, ref double Y, ref double dYdX)
        {
            List<int> indexes = Geo.Utils.DoubleUtil.GetNearstIndexes(XArray, xValue, Order);

            List<double> xList = new List<double>();
            List<double> yList = new List<double>();
            foreach (var item in indexes)
            {
                xList.Add(XArray[item] - XArray[indexes[0]]);
                yList.Add(YArray[item]);
            }

            xValue = xValue - XArray[indexes[0]] - xValue1;

            double y = 0.0;
            double dydx = 0.0;
            Lagrange(xList.ToArray(), yList.ToArray(), xValue, ref y, ref dydx);
            Y = y;
            dYdX = dydx;
        }


        /// <summary>
        /// Linear interpolation at coeffOfParams single point x.
        /// </summary>
        /// <param name="x">double</param>
        /// <param name="x0">double</param>
        /// <param name="x1">double</param>
        /// <param name="y0">double</param>
        /// <param name="y1">double</param>
        /// <returns>double</returns>
        static public double Linear(double x, double x0, double x1, double y0, double y1)
        {
            if ((x1 - x0) == 0)
            {
                return (y0 + y1) / 2;
            }
            return y0 + (x - x0) * (y1 - y0) / (x1 - x0);
        }

        /// <summary>
        /// Lagrange polynomial interpolation at coeffOfParams single point x. Because Lagrange polynomials tend to be ill behaved, this method should be used with care.
        /// A LagrangeInterpolator object should be used if multiple interpolations are to be performed using the same data
        /// </summary>
        /// <param name="xArray">the x data</param>
        /// <param name="yArray">the y data</param>
        /// <param name="x">double</param>
        /// <returns>double</returns>
        static public double LagrangeInterp(double[] xArray, double[] yArray, double x)
        {
            if (xArray.Length != yArray.Length)
            {
                throw new ArgumentException("Arrays must be of equal length."); //$NON-NLS-1$
            }
            double sum = 0;
            for (int i = 0, n = xArray.Length; i < n; i++)
            {
                if (x - xArray[i] == 0)
                {
                    return yArray[i];
                }
                double product = yArray[i];
                for (int j = 0; j < n; j++)
                {
                    if ((i == j) || (xArray[i] - xArray[j] == 0))
                    {
                        continue;
                    }
                    product *= (x - xArray[j]) / (xArray[i] - xArray[j]);
                }
                sum += product;
            }
            return sum;
        }
        /// <summary>
        /// 拉格朗日插值
        /// </summary>
        /// <param name="xs"></param>
        /// <param name="ys"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Lagrange(double[] xs, double[] ys, double x)
        {
            if (xs.Length != ys.Length) throw new ArgumentException("数组大小必须一致.");

            double val = 0;
            int count = xs.Length;
            for (int i = 0; i < count; i++)
            {
                if (x - xs[i] == 0) return ys[i];

                double y = ys[i];
                double f = 1;
                for (int j = 0; j < count; j++)
                {
                    if (j == i || (xs[i] - xs[j]) == 0) continue;

                    f *= (x - xs[j]) / (xs[i] - xs[j]);
                }

                val += y * f;
            }

            return val;
        }

        /// <summary>
        /// 拉格朗日插值 崔阳
        /// </summary>
        /// <param name="xs"></param>
        /// <param name="ys"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="dydx"></param>
        /// <returns></returns>
        public static void Lagrange(double[] xs, double[] ys, double x, ref double y, ref double dydx)
        {
            if (xs.Length != ys.Length) throw new ArgumentException("数组大小必须一致.");


            int N = xs.Length;
            int M = (N * (N + 1)) / 2;
            double[] P = new double[N];
            double[] Q = new double[M];
            double[] D = new double[N];
            for (int i = 0; i < N; i++) { P[i] = 1.0; D[i] = 1.0; }
            for (int i = 0; i < M; i++) { Q[i] = 1.0; }

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (i != j)
                    {
                        P[i] *= x - xs[j];
                        D[i] *= xs[i] - xs[j];
                        if (i < j)
                        {
                            for (int k = 0; k < N; k++)
                            {
                                if (k == i || k == j) continue;
                                Q[i + (j * (j + 1)) / 2] *= x - xs[k];
                            }
                        }
                    }
                }
            }

            y = 0.0;
            dydx = 0.0;
            for (int i = 0; i < N; i++)
            {
                y += ys[i] * (P[i] / D[i]);
                double S = 0;
                for (int k = 0; k < N; k++) if (i != k)
                    {
                        if (k < i) S += Q[k + (i * (i + 1)) / 2] / D[i];
                        else S += Q[i + (k * (k + 1)) / 2] / D[i];
                    }
                dydx += ys[i] * S;
            }
        }
        public static void Test()
        {
            double[] xs = new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
            double[] ys = new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
            int len = xs.Length;
            double[] result = new double[len];
            double[] result2 = new double[len];

            for (int i = 0; i < len; i++)
            {
                result[i] = LagrangeInterp(xs, ys, i + 0.5);
                result2[i] = Lagrange(xs, ys, i + 0.5);
            }
        }

    }

}
