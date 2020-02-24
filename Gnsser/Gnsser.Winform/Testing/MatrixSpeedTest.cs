//2016.12.10, czs & double , create in hongqing, 比较并行和穿行矩阵乘法的速度。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;


namespace Gnsser.Winform.Testing
{
    /// <summary>
    /// 测试速度
    /// </summary>
    public class MatrixSpeedTest
    {
        /// <summary>
        /// 比较并行和穿行矩阵乘法的速度。。
        /// </summary>
        static public void CompareArraySpeed()
        {
            int length = 10;

            var start = DateTime.Now;
            ArrayMatrix Matrix = new Geo.Algorithm.ArrayMatrix(length, length);
            for (int i = 0; i < 100; i++)
            {
                var x = Matrix * Matrix;
                
            }
          
             

            var span = DateTime.Now - start;
            Console.WriteLine(span.TotalMilliseconds + " ms");

           
        }
    }
}
