//2018.10.27, czs, refactor in hmx, 修改为 Decimal

using System;

namespace Geo.Algorithm
{

	/// <summary>
	///	 QR 分解。  QR decomposition for a rectangular matrix.
	/// </summary>
	/// <remarks>
	///   For an m-by-n matrix <c>A</c> with <c>m &gt;= n</c>, the QR decomposition is an m-by-n
	///   orthogonal matrix <c>Q</c> and an n-by-n upper triangular 
	///   matrix <c>R</c> so that <c>A = Q * R</c>.
	///   The QR decompostion always exists, even if the matrix does not have
	///   full rank, so the constructor will never fail.  The primary use of the
	///   QR decomposition is in the least squares solution of nonsquare systems
	///   of simultaneous linear equations.
	///   This will fail if <see cref="IsFullRank"/> returns <see langword="false"/>.
	/// </remarks>
	public class QrDecompositionDecimal
    {
		private ArrayMatrix QR;
		private decimal [] Rdiag;

		/// <summary>Construct a QR decomposition.</summary>	
		public QrDecompositionDecimal(ArrayMatrix A)
		{
			QR = (ArrayMatrix) A.Clone();
            decimal[][] qr = QR.ArrayDecimal;
			int m = A.RowCount;
			int n = A.Columns;
			Rdiag = new decimal[n];
	
			for (int k = 0; k < n; k++) 
			{
                // Compute 2-norm of k-th column without under/overflow.
                decimal nrm = 0m;
                for (int i = k; i < m; i++)
                {
                    nrm = this.Hypotenuse(nrm, Convert.ToDecimal(qr[i][k]));
                }

				if (nrm != 0.0m) 
				{
					// Form k-th Householder vector.
					if (qr[k][k] < 0)
						nrm = -nrm;
                    for (int i = k; i < m; i++)
                    {
                        var val = Convert.ToDecimal( qr[i][k]);
                        qr[i][k] = val / nrm;
                    }
					qr[k][k] += 1.0m;
	
					// Apply transformation to remaining columns.
					for (int j = k+1; j < n; j++) 
					{
                        decimal s = 0.0m;
                        for (int i = k; i < m; i++)
                        {
                            s += qr[i][k] * qr[i][j];
                        }
						s = -s/qr[k][k];
                        for (int i = k; i < m; i++)
                        {
                            qr[i][j] += s * qr[i][k];
                        }
					}
				}
				Rdiag[k] = -nrm;
			}
		}

		/// <summary>Least squares solution of <c>A * X = B</c></summary>
		/// <param name="rhs">Right-hand-side matrix with as many rows as <c>A</c> and any number of columns.</param>
		/// <returns>A matrix that minimized the two norm of <c>Q * R * X - B</c>.</returns>
		/// <exception cref="T:System.ArgumentException">Matrix row dimensions must be the same.</exception>
		/// <exception cref="T:System.InvalidOperationException">Matrix is rank deficient.</exception>
		public ArrayMatrix Solve(ArrayMatrix rhs)
		{
			if (rhs.RowCount != QR.RowCount) throw new ArgumentException("Matrix row dimensions must agree.");
			if (!IsFullRank) throw new InvalidOperationException("Matrix is rank deficient.");
				
			// Copy right hand side
			int count = rhs.Columns;
            decimal[][] X = rhs.ArrayDecimal;
			int m = QR.RowCount;
			int n = QR.Columns;
            decimal[][] qr = QR.ArrayDecimal;
			
			// Compute Y = transpose(Q)*B
			for (int k = 0; k < n; k++) 
			{
				for (int j = 0; j < count; j++) 
				{
                    decimal s = 0.0m; 
					for (int i = k; i < m; i++)
                    {
                        var qrItem = qr[i][k];
                        var XItem = X[i][j];

                        s += qrItem * XItem;
                        //s += qr[i][k] * X[i][j];
                    }
					s = -s / qr[k][k];
					for (int i = k; i < m; i++)
						X[i][j] += s * qr[i][k];
				}
			}
				
			// Solve R*X = Y;
			for (int k = n-1; k >= 0; k--) 
			{
				for (int j = 0; j < count; j++) 
					X[k][j] /= Rdiag[k];
	
				for (int i = 0; i < k; i++) 
					for (int j = 0; j < count; j++) 
						X[i][j] -= X[k][j] * qr[i][k];
			}
            var res = Geo.Utils.MatrixUtil.GetSubMatrixDecimal(X, n, count, 0, 0);
            return new ArrayMatrix(res);
            //ArrayMatrix X2;
            //return X2.Submatrix(0, n-1, 0, count-1);
		}

		/// <summary>Shows if the matrix <c>A</c> is of full rank.</summary>
		/// <value>The value is <see langword="true"/> if <c>R</c>, and hence <c>A</c>, has full rank.</value>
		public bool IsFullRank
		{
			get
			{
				int columns = QR.Columns;
				for (int j = 0; j < columns; j++)
                {
                    var Val = Rdiag[j];
                    if (Val == 0)
						return false;
                }
                return true;
			}			
		}
	
		/// <summary>Returns the upper triangular factor <c>R</c>.</summary>
		public ArrayMatrix UpperTriangularFactor
		{
			get
			{
				int n = QR.Columns;
				ArrayMatrix X = new ArrayMatrix(n, n);
                Decimal[][] x = X.ArrayDecimal;
                Decimal[][] qr = QR.ArrayDecimal;
				for (int i = 0; i < n; i++) 
					for (int j = 0; j < n; j++) 
						if (i < j)
							x[i][j] = qr[i][j];
						else if (i == j) 
							x[i][j] = Rdiag[i];
						else
							x[i][j] = 0.0m;
	
				return X;
			}
		}

        /// <summary> Returns the orthogonal factor <c>Q</c>.</summary>
        public ArrayMatrix OrthogonalFactor
		{
			get
			{
				ArrayMatrix X = new ArrayMatrix(QR.RowCount, QR.Columns);
				double[][] x = X.Array;
				double[][] qr = QR.Array;
				for (int k = QR.Columns - 1; k >= 0; k--) 
				{
					for (int i = 0; i < QR.RowCount; i++)
					{
						x[i][k] = 0.0;
					}

					x[k][k] = 1.0;
					for (int j = k; j < QR.Columns; j++) 
					{
						if (qr[k][k] != 0) 
						{
							double s = 0.0;
							for (int i = k; i < QR.RowCount; i++)
							{
								s += qr[i][k] * x[i][j];
							}

							s = -s / qr[k][k];
							for (int i = k; i < QR.RowCount; i++)
							{
								x[i][j] += s * qr[i][k];
							}
						}
					}
				}

				return X;
			}
		}
        /// <summary>
        /// 直角三角形的斜边
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
		private decimal Hypotenuse(decimal a, decimal b) 
		{
			if (Math.Abs(a) > Math.Abs(b))
			{
                decimal r = b / a;
				return Math.Abs(a) * Geo.Utils.MathUtil.Sqrt(1m + r * r);
			}

			if (b != 0)
			{
                decimal r = a / b;
				return Math.Abs(b) * Geo.Utils.MathUtil.Sqrt(1m + r * r);
			}

			return 0.0m;
		}
	}
}
