//2016.12.10, czs & double , create in hongqing, 比较多维数组和交错数组的速度。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gnsser.Winform.Testing
{
    /// <summary>
    /// 测试速度
    /// </summary>
   public  class ArraySpeedTest
    {
        /// <summary>
        /// 比较多维数组和交错数组的速度。
        /// </summary>
        static public void CompareArraySpeed()
        {
            int length = 10000;

            double[][] jiaocuo = new double[length][];
            for (int i = 0; i < length; i++)
            {
                jiaocuo[i] = new double[length];
            }
            DateTime start = DateTime.Now;
            for (int i = 0; i < length; i++)
            {
                var aa = jiaocuo[i];
                for (int j = 0; j < length; j++)
                {
                    aa[j] = 1;
                    var a = aa[j] * aa[j];
                    aa[j] = a;
                }
            }

            var span = DateTime.Now - start;
            Console.WriteLine(span.TotalMilliseconds + " ms");

            double[,] duowei = new double[length, length];
            start = DateTime.Now;

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    duowei[i, j] = 1;
                    var a = duowei[i, j] * duowei[i, j];
                    duowei[i, j] = a;
                }
            }
            span = DateTime.Now - start;
            Console.WriteLine(span.TotalMilliseconds + " ms");
        }
    }
}
