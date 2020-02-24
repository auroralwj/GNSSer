using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using Geo.Algorithm.Adjust;

namespace Geo.Algorithm
{
    /// <summary>
    /// 第一类切比雪夫多项式拟合
    /// </summary>
    public class ChebyshevPolyFit : IGetY
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="yArray"></param>
        /// <param name="order"></param>
        public ChebyshevPolyFit(double[] yArray, int order = 2)
        {
            double[] xArray = new double[yArray.Length];
            for (int i = 0; i < yArray.Length; i++)
            {
                xArray[i] = i;
            }

            this.order = order + 1;//参数多一个。 
            this.arrayX = xArray;
            this.arrayY = yArray;         }

        /// <summary>
        /// 多项式拟合。
        /// 阶次排序，从低到高。
        /// </summary>
        /// <param name="arrayX"></param>
        /// <param name="arrayY"></param>
        /// <param name="order"></param>
        public ChebyshevPolyFit(double[] arrayX, double[] arrayY, int order = 2)
        {
            if (arrayY.Length != arrayX.Length) throw new ArgumentException("输入的数组X,Y的长度应该相等！");
            if (arrayY.Length < order) throw new ArgumentException("拟合阶次应该小于数组的长度。");
            if (order < 1) throw new ArgumentException("最高此次幂必须大于0。");


            this.order = order +1;//阶次，对应参数多一个。 
            this.arrayX = arrayX;
            this.arrayY = arrayY;                        
        }
        int order = 15;
         

        private double[] arrayX;
        private double[] arrayY;
         
        /// <summary>
        /// 多项式求值。
        /// </summary>
        /// <param name="XArray"></param>
        /// <returns></returns>
        public double GetY(double x)
        {
            List<int> indexes = Geo.Utils.DoubleUtil.GetNearstIndexes(arrayX, x, order);

            List<double> xList = new List<double>();
            List<double> yList = new List<double>();
            foreach (var item in indexes)
            {
                xList.Add(arrayX[item]);
                yList.Add(arrayY[item]);
            }

            double[] xs = xList.ToArray();
            double[] ys = yList.ToArray();

            return ChebyshevFit(x, xs, ys, order);
        } 

        /// <summary>
        /// 切比雪夫多项式拟合
        /// </summary>
        /// <param name="x"></param>
        /// <param name="xs"></param>
        /// <param name="ys"></param>
        /// <param name="order">参数数量=最高阶次+1</param>
        /// <returns></returns>
        public static double ChebyshevFit(double x, double[] xs, double[] ys, int order = 2)
        {

            int xLen = xs.Length;
            double min = Math.Min(xs[0], xs[xLen - 1]);
            double max = Math.Max(xs[0], xs[xLen - 1]);

            double[][] A = MatrixUtil.Create(xLen, order);
            for (int i = 0; i < xLen; i++)
            {
                double scopeX = ToScope(xs[i], min, max);
                for (int j = 0; j < order; j++)
                {
                    A[i][j] = GetT(scopeX, j);
                }
            }

            double[] L = new double[xLen];
            for (int i = 0; i < xLen; i++)
            {
                L[i] = ys[i];
            }

            Adjust.ParamAdjuster pa = new Adjust.ParamAdjuster();
            var ad = pa.Run(new Adjust.AdjustObsMatrix(A, L));
            double[] param = ad.Estimated.OneDimArray; 

            double result = 0;
            double smallX = ToScope(x, min, max);
            for (int i = 0; i < param.Length; i++)
            {
                result += param[i] * GetT(smallX, i);
            }
            return result;
        }
         

        /// <summary>
        /// 第一类
        /// </summary>
        /// <param name="x"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public  static  double GetT(double x, int n)
        {
            if (n < 0) throw new ArgumentException("n 不可小于 0 。");
            if (n == 0) return 1;
            if (n == 1) return x;
            return 2 * x * GetT(x, n - 1) - GetT(x, n - 2);
        }
        /// <summary>
        /// 第二类 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static double GetU(double x, int n)
        {
            if (n < 0) throw new ArgumentException("n 不可小于 0 。");
            if (n == 0) return 1;
            if (n == 1) return 2 * x;
            return 2 * x * GetT(x, n - 1) - GetT(x, n - 2);
        }
        /// <summary>
        /// 归一化到 [-1 1]区间
        /// </summary>
        /// <param name="x"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double ToScope(double x, double min, double max)
        {
            return  2.0 * (x- min) / (max - min) - 1;
        }
        /// <summary>
        /// 节点取值。
        /// </summary>
        /// <param name="order"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double Chazhijiedian(int order, double min, double max)
        {
            return 0.5 * ((max - min) * Math.Cos((2 * order + 1) / (2 * order + 2) * CoordConsts.PI) + max + min);

        }


        public static void Test()
        {
            double[] xs = new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
            double[] ys = new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
            int len = xs.Length;
            double[] result = new double[len];
            double[] result2 = new double[len];

            ChebyshevPolyFit fit1 = new ChebyshevPolyFit(xs, ys, 5);
            ChebyshevPolyFit fit2 = new ChebyshevPolyFit(xs, ys, 10);

            for (int i = 0; i < len; i++)
            {
                result[i] = fit1.GetY( i + 0.5);
                result2[i] = fit2.GetY(i + 0.5);
            }
        }
    }
}
