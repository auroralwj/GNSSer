
using System;

namespace Geo.Algorithm
{

	/// <summary>
	/// 	这里采用陈国良书中的算法，前提是A矩阵m*n，且m>=n!!!!
	/// </summary>
	/// <remarks>
	///	  
	/// </remarks>
	public class SVD
	{
        //A矩阵是m*n阶=U*D*VT
        private ArrayMatrix U;//m*n
        private ArrayMatrix V;//n*n
        private double[] D = null;//n*n
		private int m;
		private int n;
        

	
		/// <summary>Construct singular value decomposition.</summary>
		public SVD(ArrayMatrix A)
		{
            
            m = A.RowCount;
			n = A.Columns;
            U = new ArrayMatrix(m, n);
            D = new double[n];
            V = new ArrayMatrix(n, n);
            ArrayMatrix E = new ArrayMatrix(n, n);
            //E初始化为单位阵
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    E[i, j] = 0;
                }
                E[i, i] = 1;
            }
            double p = 1;//
            double kci = 0.000001;//足够小的正数
            while(p>kci)
            {
                p = 0;
                for (int i = 0; i < n; i++)
                {
                    double[] sum = new double[3];
                    for (int j = i + 1; j < n; j++)
                    {
                        sum[0] = 0; sum[1] = 0; sum[2] = 0;
                        for (int k = 0; k < m; k++)
                        {
                            sum[0] += A[k, i] * A[k, j];
                            sum[1] += A[k, i] * A[k, i];
                            sum[2] += A[k, j] * A[k, j];
                        }
                        double aa = 0; double bb = 0; double rr = 0;
                        double s = 0; double c = 0;
                        double[] temp = new double[m];
                        double[] temp1 = new double[n];
                        if (Math.Abs(sum[0]) > kci)
                        {
                            aa = 2 * sum[0];
                            bb = sum[1] - sum[2];
                            rr = Math.Sqrt(aa * aa + bb * bb);
                            if (bb >= 0)
                            {
                                c = Math.Sqrt((bb + rr) / (2 * rr));
                                s = aa / (2 * rr * c);
                            }
                            else
                            {
                                s = Math.Sqrt((rr - bb) / (2 * rr));
                                c = aa / (2 * rr * s);
                            }
                            for (int k = 0; k < m; k++)
                            {
                                temp[k] = c * A[k, i] + s * A[k, j];
                                A[k, j] = (-s) * A[k, i] + c * A[k, j];
                            }
                            for (int k = 0; k < n; k++)
                            {
                                temp1[k] = c * E[k, i] + s * E[k, j];
                                E[k, j] = (-s) * E[k, i] + c * E[k, j];
                            }
                            for (int k = 0; k < m; k++)
                            { A[k, i] = temp[k]; }
                            for (int k = 0; k < n; k++)
                            { E[k, i] = temp1[k]; }
                            if (Math.Abs(sum[0]) > p)
                            { p = Math.Abs(sum[0]); }
                        }
                    }
                }
            }
            double sums = 0;
            for (int i = 0; i < n; i++)
            {
                sums = 0;
                for (int j = 0; j < m; j++)
                { sums += A[j, i] * A[j, i]; }
                D[i] = Math.Sqrt(sums);
            }
            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < m; i++)
                {
                    U[i, j] = A[i, j] / D[j];
                }
            }
            V = E.Transpose();

		}

	}

    /// <summary>
    /// 	重新编写矩阵的奇异值分解，A=U*S*V'，主要是U应该为m阶酉矩阵，V为n阶酉矩阵
    /// </summary>
    /// <remarks>
    ///	  For an m-by-n matrix <c>A</c> with <c>m >= n</c>, the singular value decomposition is
    ///   an m-by-m orthogonal matrix <c>U</c>, an m-by-n diagonal matrix <c>S</c>, and
    ///   an n-by-n orthogonal matrix <c>V</c> so that <c>A = U * S * V'</c>.
    ///   The singular values, <c>sigma[k] = S[k,k]</c>, are ordered so that
    ///   <c>sigma[0] >= sigma[1] >= ... >= sigma[n-1]</c>.
    ///   The singular value decompostion always exists, so the constructor will
    ///   never fail. The matrix condition number and the effective numerical
    ///   rank can be computed from this decomposition.
    /// </remarks>
}
