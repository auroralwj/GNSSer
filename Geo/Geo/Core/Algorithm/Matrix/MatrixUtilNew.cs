//2017.07.18, czs, create in hongqing, 创建一个通用、方便的矩阵类。
//2017.07.19, czs, edit in hongqing, 合并了 Orbit matrix 部分函数

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Algorithm
{ 
    /// <summary>
    /// 矩阵计算工具
    /// </summary>
    public class MatrixUtilNew
    {

        /// <summary>
        /// 矩阵转置
        /// </summary>
        /// <param name="Mat"></param>
        /// <returns></returns>
        static public Matrix Transpose(Matrix Mat)
        {
            Matrix T = new Matrix(Mat.ColCount, Mat.RowCount);
            for (int i = 0; i < T.RowCount; i++)
                for (int j = 0; j < T.ColCount; j++)
                    T.Data[i][j] = Mat.Data[j][i];
            return T;
        }

        /// <summary>
        /// Dyadic product。两个n,m维矩阵向量，点乘为 n × m 矩阵。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>

        public static Matrix Dyadic(Vector left, Vector right)
        {
            Matrix Mat = new Matrix(left.Dimension, right.Dimension);
            for (int i = 0; i < left.Dimension; i++)
                for (int j = 0; j < right.Dimension; j++)
                    Mat.Data[i][j] = left.Data[i] * right.Data[j];
            return Mat;
        }

        /// <summary>
        ///  LU-Decomposition.
        ///
        ///   Given an nxn matrix A, this routine replaces it by the LU decomposition
        ///   of a rowwise permutation of itself. A is output, arranged as in 
        ///   equation (2.3.14) of Press et al. (1986); Indx is an ouput vector which
        ///   records the row permutation effected by partial pivoting. This routine is 
        ///   used in combination with LU_BackSub to solve linear equations or invert 
        ///   a matrix.
        /// Adapted from LUDCMP of Press et al. (1986).
        /// </summary>
        /// <param name="A">  Square matrix; replaced by LU decomposition of permutation of A on output</param>
        /// <param name="Indx"> Permutation index vector</param>
        public static void LU_Decomp(Matrix A, Vector Indx)
        {

            // Constants

            int n = A.RowCount;
            const double tiny = 1.0e-20;       // A small number

            // Variables

            int i, j, imax = 0, k;
            double aAmax, Sum, Dum;
            Vector V = new Vector(n);

            // Loop over rows to get scaling information

            for (i = 0; i < n; i++)
            {
                aAmax = 0.0;
                for (j = 0; j < n; j++) if (Math.Abs(A[i, j]) > aAmax) aAmax = Math.Abs(A[i, j]);
                if (aAmax == 0.0)
                {
                    // No nonzero largest element
                    throw new ArgumentException("维数不符合要求！");
                };
                V[i] = 1.0 / aAmax;           // V stores the implicit scaling of each row
            };

            // Loop over columns of Crout's method

            for (j = 0; j < n; j++)
            {

                if (j > 0)
                {
                    for (i = 0; i < j; i++)
                    {   // This is equation 2.3.12 except for i=j
                        Sum = A[i, j];
                        if (i > 0)
                        {
                            for (k = 0; k < i; k++) Sum -= A[i, k] * A[k, j];
                            A[i, j] = Sum;
                        };
                    };
                };

                aAmax = 0.0;                  // Initialize for the search of the largest
                // pivot element

                for (i = j; i < n; i++)
                {     // This is i=j of equation 2.3.12 and 
                    Sum = A[i, j];             // i=j+1..N of equation 2.3.13
                    if (j > 0)
                    {
                        for (k = 0; k < j; k++) Sum -= A[i, k] * A[k, j];
                        A[i, j] = Sum;
                    };
                    Dum = V[i] * Math.Abs(Sum);     // Figure of merit for the pivot
                    if (Dum >= aAmax)
                    {       // Is it better than the best so far ?
                        imax = i;
                        aAmax = Dum;
                    };
                };

                if (j != imax)
                {            // Do we need to interchange rows?
                    for (k = 0; k < n; k++)
                    {    // Yes, do so ...
                        Dum = A[imax, k];
                        A[imax, k] = A[j, k];
                        A[j, k] = Dum;
                    }
                    V[imax] = V[j];           // Also interchange the scale factor 
                };

                Indx[j] = imax;

                if (j != n - 1)
                {             // Now finally devide by the pivot element
                    if (A[j, j] == 0.0)
                    {      // If the pivot element is zero the matrix 
                        A[j, j] = tiny;          // is singular (at least to the precision of
                    };                        // the algorithm). For some applications on
                    Dum = 1.0 / A[j, j];           // singular matrices, it is desirable to 
                    for (i = j + 1; i < n; i++)
                    {     // substitude tiny for zero. 
                        A[i, j] = A[i, j] * Dum;
                    };
                };

            };   // Go back for the next column in the reduction

            if (A[n - 1, n - 1] == 0.0) A[n - 1, n - 1] = tiny;

        }


        /// <summary>
        /// LU 回代。 LU_BackSub   LU Backsubstitution
        /// Solves the set of n linear equations Ax=b. Here A is input, not as the 
        ///   matrix A but rather as its LU decomposition, determined by the function
        ///   LU_Decomp. b is input as the right-hand side vector b, and returns with
        ///   the solution vector x. A and Indx are not modified by this function and 
        ///   can be left in place for successive calls with different right-hand 
        ///   sides b. This routine takes into account the posssibility that B will  
        ///   begin with many zero elements, so it is efficient for use in matrix
        ///   inversions.
        /// 
        /// </summary>
        /// <param name="A">A       LU decomposition of permutation of A</param>
        /// <param name="Indx">Indx    Permutation index vector</param>
        /// <param name="b"> b       Right-hand side vector b; replaced by solution x of Ax=b on output</param>
        public static void LU_BackSub(Matrix A, Vector Indx, Vector b)
        {

            // Constants

            int n = A.RowCount;

            // Local variables

            int ii, i, ll, j;
            double Sum;

            //
            // Start
            //

            ii = -1;                      // When ii is set to a nonegative value, it will
            // become the prevObj nonvanishing element of B. 
            for (i = 0; i < n; i++)
            {         // We now do the forward substitution.
                ll = (int)Indx[i];         // The only wrinkle is to unscramble the 
                Sum = b[ll];                // permutation as we go.
                b[ll] = b[i];
                if (ii != -1)
                {
                    for (j = ii; j < i; j++) Sum -= A[i, j] * b[j];
                }
                else
                {
                    if (Sum != 0.0) ii = i;   // A nonzero element was encountered, so from 
                };                          // now on we will have to do the sums in the
                b[i] = Sum;                 // loop above.
            };

            for (i = n - 1; i >= 0; i--)
            {     // Now we do the backsubstitution, eqn 2.3.7.
                Sum = b[i];
                if (i < n - 1)
                {
                    for (j = i + 1; j < n; j++)
                    {
                        Sum = Sum - A[i, j] * b[j];
                    };
                };
                b[i] = Sum / A[i, i];         // Store a component of the solution vector X.
            };

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


        public static string ToString(double[][] Data)
        {
            StringBuilder sb = new StringBuilder();
            int RowCount = Data.Length, ColCount = Data[0].Length;
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColCount; j++)
                {
                    sb.Append(String.Format("{0, 12:F5}  ", Data[i][j]));
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }


    }
}
