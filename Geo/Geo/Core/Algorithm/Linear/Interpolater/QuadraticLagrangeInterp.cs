using System;
using System.Collections.Generic;
using System.Text;

namespace Geo.Algorithm
{
    /// <summary>
    /// 根据离散点进行二次的拉格朗日插值。每次自动筛选三个点。
    /// http://www.cnblogs.com/technology
    /// </summary>
    public class QuadraticLagrangeInterp : Geo.Algorithm.IGetY
    {
        /// <summary>
        /// X各点坐标组成的数组
        /// </summary>
        public double [] XArray { get; set; }

        /// <summary>
        /// X各点对应的Y坐标值组成的数组
        /// </summary>
        public double [] YArray { get; set; }

        /// <summary>
        /// x数组或者y数组中元素的个数, 注意两个数组中的元素个数需要一样
        /// </summary>
        public int XCount { get; set; }

        /// <summary>
        /// 初始化拉格朗日插值
        /// </summary>
        /// <param name="xArray">X各点坐标组成的数组</param>
        /// <param name="yArray">X各点对应的Y坐标值组成的数组</param>
        public QuadraticLagrangeInterp(double[] xArray, double[] yArray)
        {
            this.XArray = xArray; 
            this.YArray = yArray;
            this.XCount = xArray.Length;
        }

        /// <summary>
        /// 获得某个横坐标对应的Y坐标值
        /// </summary>
        /// <param name="xValue">x坐标值</param>
        /// <returns></returns>
        public double GetY(double xValue)
        {
            //用于累乘数组始末下标
            int start, end;
            //返回值
            double value = 0.0;
            //如果初始的离散点为空, 返回0
            if (XCount < 1) { return value; }
            //如果初始的离散点只有1个, 返回该点对应的Y值
            if (XCount == 1) { value = YArray[0]; return value; }
            //如果初始的离散点只有2个, 进行线性插值并返回插值
            if (XCount == 2)
            {
                value = (YArray[0] * (xValue - XArray[1]) - YArray[1] * (xValue - XArray[0])) / (XArray[0] - XArray[1]);
                return value;
            }
            //如果插值点小于第一个点X坐标, 取数组前3个点做插值
            if (xValue <= XArray[1]) { start = 0; end = 2; }
            //如果插值点大于等于最后一个点X坐标, 取数组最后3个点做插值
            else if (xValue >= XArray[XCount - 2]) { start = XCount - 3; end = XCount - 1; }
            //除了上述的一些特殊情况, 通常情况如下
            else
            {
                start = 1; end = XCount;
                int temp;
                //使用二分法决定选择哪三个点做插值
                while ((end - start) != 1)
                {
                    temp = (start + end) / 2;
                    if (xValue < XArray[temp - 1])
                        end = temp;
                    else
                        start = temp;
                }
                start--; end--;
                //看插值点跟哪个点比较靠近
                if (Math.Abs(xValue - XArray[start]) < Math.Abs(xValue - XArray[end]))
                    start--;
                else
                    end++;
            }
            //这时已经确定了取哪三个点做插值, 第一个点为x[start]
            double valueTemp;
            //注意是二次的插值公式
            for (int i = start; i <= end; i++)
            {
                valueTemp = 1.0;
                for (int j = start; j <= end; j++)
                    if (j != i)
                        valueTemp *= (double)(xValue - XArray[j]) / (double)(XArray[i] - XArray[j]);
                value += valueTemp * YArray[i];
            }
            return value;
        }

    }
}
