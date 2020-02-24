
//2017.06.18, czs, funcKeyToDouble from c++ in hongqing, Matrix
//2017.06.24, czs, edit in hongqing, format and refactor codes


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;

 using Geo.Algorithm; namespace Gnsser.Orbits
{
    /// <summary>
    /// 矩阵计算工具
    /// </summary>
    public class MatrixUtil
    {
 
        /// <summary>
        /// Dyadic product。两个n,m维矩阵向量，点乘为 n × m 矩阵。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>

        public static Matrix Dyadic(Geo.Algorithm.Vector left, Geo.Algorithm.Vector right)
        {
            Matrix Mat = new Matrix(left.Dimension, right.Dimension);
            for (int i = 0; i < left.Dimension; i++)
                for (int j = 0; j < right.Dimension; j++)
                    Mat.Data[i][j] = left.Data[i] * right.Data[j];
            return Mat;
        } 
          
        /// <summary>
        /// 上三角求逆 Inversion of an upper right triangular matrix
        ///  This function may be called with the same actual parameter for R and T
        /// </summary>
        /// <param name="R"> R    Upper triangular square matrix</param>
        /// <returns></returns>
        public static Matrix InverseUpRight(Matrix R)
        {
            int N = R.RowCount;   // Dimension of R and T
            Matrix T = new Matrix(N, N);
            int i, j, k;
            double Sum;

            if (R.ColCount != N)
            {
                throw new ArgumentException(" ERROR: Incompatible shapes in InvUpper");
            }

            // Check diagonal elements
            for (i = 0; i < N; i++)
                if (R[i, i] == 0.0)
                {
                    throw new ArgumentException(" ERROR: Singular matrix in InvUpper");
                }
                else
                {
                    // Compute the inverse of i-th diagonal element.
                    T[i, i] = 1.0 / R[i, i];
                };

            // Calculate the inverse T = R^(-1)

            for (i = 0; i < N - 1; i++)
                for (j = i + 1; j < N; j++)
                {
                    Sum = 0.0; for (k = i; k <= j - 1; k++) Sum += T[i, k] * R[k, j];
                    T[i, j] = -T[j, j] * Sum;
                };

            return T;
        }
    }
}
