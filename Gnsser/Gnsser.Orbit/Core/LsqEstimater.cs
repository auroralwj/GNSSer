//2017.06.18, czs, funcKeyToDouble from c++ in hongqing, Matrix
//2017.06.24, czs, edit in hongqing, format and refactor codes
//2018.10.14, czs, edit in hmx, 进行了简单的整理


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;
using Geo.Algorithm;
using Geo.Utils;

namespace Gnsser.Orbits
{

    /// <summary>
    /// Least squares estimation class 
    /// </summary>
    public class LsqEstimater
    {
        // Elements Square-root information matrix and satData vector
        private int ParamCount;         // Number of estimation parameters
        Geo.Algorithm.Vector RightHandSide { get; set; }       // Right hand side of transformed equations
        Matrix SquareRoot { get; set; }            // Square-root information matrix 
        // (Upper right triangular matrix)

        /// <summary>
        ///Constructor  LSQ class (implementation)
        /// </summary>
        /// <param name="n">Number of estimation parameters</param>
        public LsqEstimater(int n)
        {
            ParamCount = (n);

            // Allocate storage for R and d and initialize to zero
            RightHandSide = new Geo.Algorithm.Vector(ParamCount);
            SquareRoot = new Matrix(ParamCount, ParamCount);
        }

        /// <summary>
        ///  Initialization without apriori information
        /// </summary>
        public void Init()
        {
            // Reset all elements of R and d
            SquareRoot = new Matrix(ParamCount, ParamCount);// 0.0;
            RightHandSide = new Geo.Algorithm.Vector(ParamCount);// 0.0;
        }

        /// <summary>
        /// Initialization with apriori information
        /// </summary>
        /// <param name="prioriParam"></param>
        /// <param name="prioriCova"></param>

        public void Init(Geo.Algorithm.Vector prioriParam,   // a priori parameters
                         Matrix prioriCova)   // a priori covariance
        {
            // Variables
            int i, j, k;
            double Sum;

            // Start the factorization of matrix P. Compute upper triangular 
            // factor R of P, where P = (R)*(R^T). Proceed backward column 
            // by column.
            for (j = ParamCount - 1; j >= 0; j--)
            {
                // Compute j-th diagonal element.
                Sum = 0.0;
                for (k = j + 1; k <= ParamCount - 1; k++) Sum += SquareRoot[j, k] * SquareRoot[j, k];
                SquareRoot[j, j] = Math.Sqrt(prioriCova[j, j] - Sum);

                // Complete factorization of j-th column.
                for (i = j - 1; i >= 0; i--)
                {
                    Sum = 0.0;
                    for (k = j + 1; k <= ParamCount - 1; k++) Sum += SquareRoot[i, k] * SquareRoot[j, k];
                    SquareRoot[i, j] = (prioriCova[i, j] - Sum) / SquareRoot[j, j];
                }
            }

            // Replace R by its inverse R^(-1)
            SquareRoot = MatrixUtil.InverseUpRight(SquareRoot);

            // Initialize right hand side
            RightHandSide = SquareRoot * prioriParam;
        }


        /// <summary>
        ///   将形式为Ax = b的satData方程添加到最小二乘系统（使用Givens旋转执行行方式QR转换）
        ///   Add a satData equation of form Ax=b to the least squares system
        //  (performs a row-wise QR transformation using Givens rotations)
        /// </summary>
        /// <param name="A"></param>
        /// <param name="b"></param>
        /// <param name="sigma"></param>
        public void Accumulate(Geo.Algorithm.Vector A, double b, double sigma = 1.0)
        {
            // Variables
            int i, j;
            double c, s, h;

            // Weighting
            var a = A / sigma;  // Normalize A 
            b = b / sigma;  // Normalize b

            // Construct and apply Givens plane rotation.
            for (i = 0; i < ParamCount; i++)
            {
                // Construct the rotation and apply it to
                // eliminate the i-th element of a.
                if (SquareRoot[i, i] == 0.0 && a[i] == 0.0)
                {
                    c = 1.0; s = 0.0; SquareRoot[i, i] = 0.0;
                }
                else
                {
                    h = Math.Sqrt(SquareRoot[i, i] * SquareRoot[i, i] + a[i] * a[i]);
                    if (SquareRoot[i, i] < 0.0) h = -h;
                    c = SquareRoot[i, i] / h;
                    s = a[i] / h;
                    SquareRoot[i, i] = h;
                };

                a[i] = 0.0;

                // Apply the rotation to the remaining elements of a
                for (j = i + 1; j < ParamCount; j++)
                {
                    h = +c * SquareRoot[i, j] + s * a[j];
                    a[j] = -s * SquareRoot[i, j] + c * a[j];
                    SquareRoot[i, j] = h;
                }

                // Apply the rotation to the i-th element of d
                h = +c * RightHandSide[i] + s * b;
                b = -s * RightHandSide[i] + c * b;
                RightHandSide[i] = h;
            }

        }


        /// <summary>
        /// 用反替换法求解向量x[]的LSQ问题
        /// 通过反向替换解决向量x []的LSQ问题
        /// Solve the LSQ problem for vector x[] by backsubstitution
        /// </summary>
        /// <param name="x"></param>
        public void Solve(Geo.Algorithm.Vector x)
        {
            // Variables
            int i, j; i = j = 0;
            double Sum = 0.0;

            // Check for singular matrix 
            for (i = 0; i < ParamCount; i++)
                if (SquareRoot[i, i] == 0.0)
                {
                    Console.Write(" ERROR: Singular matrix R in LSQ::Solve()");// 
                };

            //  Solve Rx=d for x_n,...,x_1 by backsubstitution
            x[ParamCount - 1] = RightHandSide[ParamCount - 1] / SquareRoot[ParamCount - 1, ParamCount - 1];
            for (i = ParamCount - 2; i >= 0; i--)
            {
                Sum = 0.0;
                for (j = i + 1; j < ParamCount; j++) Sum += SquareRoot[i, j] * x[j];
                x[i] = (RightHandSide[i] - Sum) / SquareRoot[i, i];
            };
        }



        /// <summary>
        ///  Covariance matrix
        /// </summary>
        /// <returns></returns>
        public Matrix Cova()
        {
            // Variables
            int i, j, k;
            double Sum;
            //new Matrix(N, N);

            // Calculate the inverse T = R^(-1)

            Matrix T = MatrixUtil.InverseUpRight(SquareRoot);

            // Replace T by the covariance matrix C=T*T^t

            for (i = 0; i < ParamCount; i++)
                for (j = i; j < ParamCount; j++)
                {
                    Sum = 0.0; for (k = j; k < ParamCount; k++) Sum += T[i, k] * T[j, k];
                    T[i, j] = Sum;
                    T[j, i] = Sum;
                };

            // Result
            return T;
        }


        /// <summary>
        ///  Standard deviation
        /// </summary>
        /// <returns></returns>
        public Geo.Algorithm.Vector StdDev()
        {
            // Variables
            int i;
            var Sigma = new Geo.Algorithm.Vector(ParamCount);
            Matrix C = Cova(); // Covariance
            // Standard deviation
            for (i = 0; i < ParamCount; i++) Sigma[i] = Math.Sqrt(C[i, i]);
            return Sigma;
        }

        //
        // Square-root information matrix and satData vector
        //

        public Matrix SRIM() { return SquareRoot; }     // Copy of R

        public Geo.Algorithm.Vector Data() { return RightHandSide; }      // Copy of d  
    }
}
