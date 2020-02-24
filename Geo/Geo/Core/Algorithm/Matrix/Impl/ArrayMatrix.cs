//2017.07.18, czs, refactor ib hongqing, ������Ϊ ArrayMatrix

    using System;
    using System.IO;
    using System.IO.Compression;//ѹ��
using Geo.Algorithm;
using Geo.Utils;
using System.Threading.Tasks;
using System.Collections.Generic;



namespace Geo.Algorithm
{ 
    /// <summary>��ά�����ʾ�ľ�����ͨ�õľ�����</summary>
    public class ArrayMatrix : Geo.Algorithm.AbstractMatrix
    { 
        private double[][] data;
        private int rows;
        private int columns;

        private static Random random = new Random();

        /// <summary>Constructs an empty matrix of the given aboutSize.</summary>
        /// <param name="rows">Number of rows.</param>
        /// <param name="columns">Number of columns.</param>
        public ArrayMatrix(int rows, int columns)
            : base(MatrixType.Array)
        {
            this.rows = rows;
            this.columns = columns;
            this.data = new double[rows][];
            for (int i = 0; i < rows; i++)
            {
                this.data[i] = new double[columns];
            } 
        }


        //public Matrix(IMatrix matrix):this(matrix.Array)
        //{

        //}
        /// <summary>
        /// ����������ʼ��һ�� N x 1 �� 1x N �ľ���
        /// </summary>
        /// <param name="vector">����</param>
        /// <param name="isColOrRow">�Ƿ��л�������</param>
        public ArrayMatrix(IVector vector, bool isColOrRow=true)
            : this(MatrixUtil.GetMatrix(vector.OneDimArray, isColOrRow)) {
            if(vector.ParamNames != null)
            {
                if (isColOrRow)
                {
                    this.RowNames = vector.ParamNames;
                    this.ColNames =  new List<string>() { "Value" };
                }
                else
                {
                    this.RowNames = new List<string>() { "Value" };
                    this.ColNames = vector.ParamNames;
                }
            }
        }

        /// <summary>Constructs a matrix of the given aboutSize and assigns a given value to all diagonal elements.</summary>
        /// <param name="rows">Number of rows.</param>
        /// <param name="columns">Number of columns.</param>
        /// <param name="value">Value to assign to the all of the elements.</param>diagnoal
        public ArrayMatrix(int rows, int columns, double value, bool isDiagonal = false)
            : base(MatrixType.Array)
        {
            this.rows = rows;
            this.columns = columns;
            this.data = new double[rows][];
            for (int i = 0; i < rows; i++)
            {
                data[i] = new double[columns];
                for (int j = 0; j < columns; j++)
                {
                    if (isDiagonal)
                    {
                        if (i == j) data[i][j] = value;
                    }
                    else
                    {
                        data[i][j] = value;
                    }
                }
            } 
        }
        /// <summary>
        /// һά����洢�Ķ�ά����
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <param name="value"></param>
        public ArrayMatrix(int rows, int columns, double [] value)
            : base(MatrixType.Array)
        {
            this.rows = rows;
            this.columns = columns;
            this.data = new double[rows][];
            for (int i = 0; i < rows; i++)
            {
                data[i] = new double[columns];
                for (int j = 0; j < columns; j++)
                {
                    data[i][j] = value[i * columns + j]; 
                }
            } 
        }
        /// <summary>Constructs a matrix from the given array.</summary>
        /// <param name="data">The array the matrix gets constructed from.</param>
        public ArrayMatrix(double[][] data)
            : base(MatrixType.Array)
        {
            this.rows = data.Length;
            this.columns = data[0].Length;

            for (int i = 0; i < rows; i++)
            {
                if (data[i].Length != columns)
                {
                    throw new ArgumentException();
                }
            }

            this.data = data; 
        }

        /// <summary>Constructs a matrix from the given array.</summary>
        /// <param name="data">The array the matrix gets constructed from.</param>
        public ArrayMatrix(decimal[][] data)
            : base(MatrixType.Array)
        {
            this.rows = data.Length;
            this.columns = data[0].Length;
             
            double[][] dat = new double[this.rows][];
            for (int i = 0; i < this.rows; ++i)
            {
                var newRow = new double[this.columns];
                dat[i] = newRow;
                var oldRow = data[i]; 
                for (int j = 0; j < this.columns; ++j)
                {
                    newRow[j] = Convert.ToDouble( oldRow[j]);
                }
            }
            this.data = dat;
        }
        public ArrayMatrix(double[,] value)
            : base(MatrixType.Array)
        {
            this.rows = value.GetLength(0);
            this.columns = value.GetLength(1);
            double[][] data = new double[this.rows][];
            for (int i = 0; i < this.rows; ++i)
            {
                data[i] = new double[this.columns];
                for (int j = 0; j < this.columns; ++j)
                {
                    data[i][j] = value[i, j];
                }
            }
            this.data = data; 
        }
        /// <summary>
        /// ��ά����
        /// </summary>
        public override double[][] Array
        {
            get
            {
                return this.data;
            }
        }
        /// <summary>
        /// �߾��ȵĶ�ά����
        /// </summary>
        public  decimal[][] ArrayDecimal
        {
            get
            {
                var dat = Geo.Utils.MatrixUtil.CreateT<decimal>(this.RowCount, this.ColCount);
                for (int i = 0; i < this.RowCount; i++)
                {
                    var row = dat[i];
                    var oldRow = data[i];
                    for (int j = 0; j < this.ColCount; j++)
                    {
                        row[j] =Convert.ToDecimal( oldRow[j]);
                    }
                } 
                return dat;
            }
        }



        /// <summary>Returns the number of columns.</summary>
        public int Rows
        {
            get
            {
                return this.rows;
            }
        }

        /// <summary>Returns the number of columns.</summary>
        public int Columns
        {
            get
            {
                return this.columns;
            }
        }
        /// <summary>
        /// �ܹ�Ԫ��������
        /// </summary>
        public override int ItemCount
        {
            get { return ColCount * RowCount; }
        }

      

        /// <summary>Returns a sub matrix extracted from the current matrix.</summary>
        /// <param name="i0">Start row index</param>
        /// <param name="i1">End row index</param>
        /// <param name="j0">Start column index</param>
        /// <param name="j1">End column index</param>
        public ArrayMatrix Submatrix(int i0, int i1, int j0, int j1)
        {
            if ((i0 > i1) || (j0 > j1) || (i0 < 0) || (i0 >= this.rows) || (i1 < 0) || (i1 >= this.rows) || (j0 < 0) || (j0 >= this.columns) || (j1 < 0) || (j1 >= this.columns))
            {
                throw new ArgumentException();
            }

            ArrayMatrix X = new ArrayMatrix(i1 - i0 + 1, j1 - j0 + 1);
            double[][] x = X.Array;
            for (int i = i0; i <= i1; i++)
            {
                for (int j = j0; j <= j1; j++)
                {
                    x[i - i0][j - j0] = data[i][j];
                }
            }
            if (this.ColNames != null && this.ColNames.Count > 0) { X.ColNames = this.ColNames.GetRange(i0, i1 - i0+1); }
            if (this.RowNames != null && this.RowNames.Count > 0) { X.RowNames = this.RowNames.GetRange(j0, j1 - j0+1); }

            return X;
        }

        /// <summary>Returns a sub matrix extracted from the current matrix.</summary>
        /// <param name="r">Array of row indices</param>
        /// <param name="c">Array of row indices</param>
        public ArrayMatrix Submatrix(int[] r, int[] c)
        {
            ArrayMatrix X = new ArrayMatrix(r.Length, c.Length);
            double[][] x = X.Array;
            for (int i = 0; i < r.Length; i++)
            {
                for (int j = 0; j < c.Length; j++)
                {
                    if ((r[i] < 0) || (r[i] >= rows) || (c[j] < 0) || (c[j] >= columns))
                    {
                        throw new ArgumentException();
                    }

                    x[i][j] = data[r[i]][c[j]];
                }
            }

            return X;
        }

        /// <summary>Returns a sub matrix extracted from the current matrix.</summary>
        /// <param name="i0">Starttial row index</param>
        /// <param name="i1">End row index</param>
        /// <param name="c">Array of row indices</param>
        public ArrayMatrix Submatrix(int i0, int i1, int[] c)
        {
            if ((i0 > i1) || (i0 < 0) || (i0 >= this.rows) || (i1 < 0) || (i1 >= this.rows))
            {
                throw new ArgumentException();
            }

            ArrayMatrix X = new ArrayMatrix(i1 - i0 + 1, c.Length);
            double[][] x = X.Array;
            for (int i = i0; i <= i1; i++)
            {
                for (int j = 0; j < c.Length; j++)
                {
                    if ((c[j] < 0) || (c[j] >= columns))
                    {
                        throw new ArgumentException();
                    }

                    x[i - i0][j] = data[i][c[j]];
                }
            }

            return X;
        }

        /// <summary>Returns a sub matrix extracted from the current matrix.</summary>
        /// <param name="r">Array of row indices</param>
        /// <param name="j0">Start column index</param>
        /// <param name="j1">End column index</param>
        public ArrayMatrix Submatrix(int[] r, int j0, int j1)
        {
            if ((j0 > j1) || (j0 < 0) || (j0 >= columns) || (j1 < 0) || (j1 >= columns))
            {
                throw new ArgumentException();
            }

            ArrayMatrix X = new ArrayMatrix(r.Length, j1 - j0 + 1);
            double[][] x = X.Array;
            for (int i = 0; i < r.Length; i++)
            {
                for (int j = j0; j <= j1; j++)
                {
                    if ((r[i] < 0) || (r[i] >= this.rows))
                    {
                        throw new ArgumentException();
                    }

                    x[i][j - j0] = data[r[i]][j];
                }
            }

            return X;
        }
        /// <summary>
        /// ���þ������ֵ
        /// </summary>
        /// <param name="sub"></param>
        /// <param name="fromMainRow"></param>
        /// <param name="fromMainCol"></param>
        public void Set(IMatrix sub, int fromMainRow=0, int fromMainCol=0)
        {
            //double[][] x = Array;
            for (int i = fromMainRow; i < RowCount && i - fromMainRow < sub.RowCount; i++)
            {
                for (int j = fromMainCol; j < ColCount && j - fromMainCol < sub.ColCount; j++)
                {
                    this[i, j] = sub[i - fromMainRow, j - fromMainCol];
                }
            }
        }

        /// <summary>Creates a copy of the matrix.</summary>
        public ArrayMatrix Clone()
        {
            ArrayMatrix X = new ArrayMatrix(rows, columns);
            double[][] x = X.Array;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    x[i][j] = data[i][j];
                }
            }

            return X;
        }

        /// <summary>Returns the transposed matrix.</summary>
        public ArrayMatrix Transpose()
        {
            ArrayMatrix X = new ArrayMatrix(columns, rows);
            double[][] x = X.Array;  

            for (int i = 0; i < rows; i++)
            { 
                var row = data[i];
                for (int j = 0; j < columns; j++)
                { 
                    x[j][i] = row[j];
                }
            }

            return X;
        }

        /// <summary>Returns the One Norm for the matrix.</summary>
        /// <value>The maximum column sum.</value>
        public double Norm1
        {
            get
            {
                double f = 0;
                for (int j = 0; j < columns; j++)
                {
                    double s = 0;
                    for (int i = 0; i < rows; i++)
                    {
                        s += Math.Abs(data[i][j]);
                    }

                    f = Math.Max(f, s);
                }
                return f;
            }
        }

        /// <summary>Returns the Infinity Norm for the matrix.</summary>
        /// <value>The maximum row sum.</value>
        public double InfinityNorm
        {
            get
            {
                double f = 0;
                for (int i = 0; i < rows; i++)
                {
                    double s = 0;
                    for (int j = 0; j < columns; j++)
                        s += Math.Abs(data[i][j]);
                    f = Math.Max(f, s);
                }
                return f;
            }
        }

        /// <summary>Returns the Frobenius Norm for the matrix.</summary>
        /// <value>The square root of sum of squares of all elements.</value>
        public double FrobeniusNorm
        {
            get
            {
                double f = 0;
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        f = this.Hypotenuse(f, data[i][j]);
                    }
                }

                return f;
            }
        }

        /// <summary>Unary minus.</summary>
        public static ArrayMatrix operator -(ArrayMatrix a)
        {
            int rows = a.Rows;
            int columns = a.Columns;
            double[][] data = a.Array;

            ArrayMatrix X = new ArrayMatrix(rows, columns);
            double[][] x = X.Array;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    x[i][j] = -data[i][j];
                }
            }

            return X;
        }
        /// <summary>Matrix addition.</summary>
        public static ArrayMatrix operator +(ArrayMatrix a, IMatrix b)
        {
            int rows = a.Rows;
            int columns = a.Columns;
            double[][] data = a.Array;

            if ((rows != b.RowCount) || (columns != b.ColCount))
            {
                throw new ArgumentException("Matrix dimension do not match.");
            }

            ArrayMatrix X = new ArrayMatrix(rows, columns);
            double[][] x = X.Array;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    x[i][j] = data[i][j] + b[i, j];
                }
            }
            return X;
        }

        /// <summary>Matrix subtraction.</summary>
        public static ArrayMatrix operator -(ArrayMatrix a, IMatrix b)
        {
            int rows = a.Rows;
            int columns = a.Columns;
            double[][] data = a.Array;

            if ((rows != b.RowCount) || (columns != b.ColCount))
            {
                throw new ArgumentException("Matrix dimension do not match.");
            }

            ArrayMatrix X = new ArrayMatrix(rows, columns);
            double[][] x = X.Array;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    x[i][j] = data[i][j] - b[i, j];
                }
            }
            return X;
        }

        /// <summary>Matrix-scalar multiplication.</summary>
        public static ArrayMatrix operator *(ArrayMatrix a, double s)
        {
            int rows = a.Rows;
            int columns = a.Columns;
            double[][] data = a.Array;

            ArrayMatrix X = new ArrayMatrix(rows, columns);

            double[][] x = X.Array;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    x[i][j] = data[i][j] * s;
                }
            }

            return X;
        }
        /// <summary>
        /// ��ݼ��㷽��
        /// </summary>
        /// <param name="A">ϵ����</param>
        /// <param name="Q">�Խ���</param>
        /// <returns></returns>
        public static  IMatrix ATA(IMatrix A)
        {
            
            IMatrix AT = A.Transposition;
            int row = AT.RowCount;
            int column = AT.ColCount;
            int count = (row + 1) * row / 2;
            double[] array = new double[count];
            double[][] dataA = AT.Array;
            double[] dataAVector = new double[AT.ColCount];
            double[] dataAVector1 = new double[AT.ColCount];
            //for (int i = 0; i < row; i++)
            //{
            //    dataAVector = dataA[i];
            //    for (int j = 0; j < column; j++)
            //    {
            //        for (int k = 0; k <= j; k++)
            //        {
            //            array[j * (j + 1) / 2 + k] += dataAVector[j] * dataAVector[k] / dataQ1[i];
            //        }
            //    }
            //}
            for (int i = 0; i < row; i++)
            {
                dataAVector = dataA[i];
                for (int j = 0; j <= i; j++)
                {
                    dataAVector1 = dataA[j];
                    double aa = 0;
                    for (int k = 0; k < column; k++)
                    {
                        aa += dataAVector[k] * dataAVector1[k];
                    }
                    array[i * (i + 1) / 2 + j] = aa;
                }
            }
            return new SymmetricMatrix(array);
        }
        /// <summary>Matrix-matrix multiplication.</summary>
        public static ArrayMatrix operator *(ArrayMatrix left, IMatrix right)
        {
            if (right.RowCount != left.columns)
            {
                throw new ArgumentException("Matrix dimensions are not valid.");
            }

            int rowCount = left.RowCount; 
            double[][] x = new double[rowCount][];// X.Array;
            double[][] aArray = left.Array;           
            double[][] bArray = right.Transposition.Array;
            if (rowCount <= 500)
            { 
                for (int i = 0; i < rowCount; i++)
                { 
                    x[i] = MultiplyRow(aArray[i], bArray);
                }
            }
            else
            {
                Parallel.For(0,  rowCount, new Action<int>(delegate(int i){
                      x[i] = MultiplyRow(aArray[i], bArray);
                }));
            }
            return  new ArrayMatrix(x); 
        }



        /*
        /// <summary>Adds a matrix to the current matrix.</summary>
        public void Add(Matrix A)
        {
        if ((rows != A.Rows) || (columns != A.Columns)) throw new ArgumentException();
        double[][] a = A.Array;
        for (int i = 0; i < rows; i++)
        for (int j = 0; j < columns; j++)
        data[i][j] += a[i][j];
        }

        /// <summary>Subtracts a matrix from the current matrix.</summary>
        public void Sub(Matrix A)
        {
        if ((rows != A.Rows) || (this.columns != A.Columns)) throw new ArgumentException();
        double[][] a = A.Array;
        for (int i = 0; i < rows; i++)
        for (int j = 0; j < columns; j++)
        data[i][j] -= a[i][j];
        }

        /// <summary>Multiplies the current matrix with a scalar factor.</summary>
        public void Times(double s)
        {
        for (int i = 0; i < rows; i++)
        for (int j = 0; j < columns; j++)
        data[i][j] *= s;
        }
        */

        /// <summary>Returns the LHS solution vetor if the matrix is square or the least squares solution otherwise.</summary>
        public ArrayMatrix Solve(ArrayMatrix rhs)
        {
            int maxDoubleDim = 10000000;
            ArrayMatrix inv = null;
            if (rows == columns && this.IsSymmetric)
            {
                try
                {
                    if (rhs.RowCount > maxDoubleDim)
                    {
                        inv = new SljMatrixInverseDecimal(this).Solve(rhs);
                    }
                    else
                    {
                        inv = new SljMatrixInverse(this).Solve(rhs);
                    }

                    if (false)
                    {
                        var I = this * inv;
                        int i = 0;
                    }

                    return inv;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message + "SLJ_Inverse �������������������������! �����������������㡣"); ;
                  //  throw new Exception("Warning:Matrix is singular to working precision!");
                }
            }
            if (rhs.RowCount > maxDoubleDim)
            {
                inv = new QrDecompositionDecimal(this).Solve(rhs);
            }
            else
            {
                inv = new QrDecomposition(this).Solve(rhs);
            } 

            if (false)
            {
                var I = this * inv;
                int i = 0;
            }
            return inv;

           // return (rows == columns) ? new LuDecomposition(this).Solve(rhs) : new QrDecomposition(this).Solve(rhs);
        }

        /// <summary>Inverse of the matrix if matrix is square, pseudoinverse otherwise.</summary>
        public ArrayMatrix Inverse
        {
            get
            {
                return this.Solve(Diagonal(rows, rows, 1.0));
            }
        }

        /// <summary>Determinant if matrix is square.</summary>
        public double Determinant
        {
            get
            {
                return new LuDecomposition(this).Determinant;
            }
        }

        /// <summary>Returns the trace of the matrix.</summary>
        /// <returns>Sum of the diagonal elements.</returns>
        public double Trace
        {
            get
            {
                double trace = 0;
                for (int i = 0; i < Math.Min(rows, columns); i++)
                {
                    trace += data[i][i];
                }
                return trace;
            }
        }
        
        /// <summary>Returns a matrix filled with random values.</summary>
        public static ArrayMatrix Random(int rows, int columns)
        {
            ArrayMatrix X = new ArrayMatrix(rows, columns);
            double[][] x = X.Array;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    x[i][j] = random.NextDouble();
                }
            }
            return X;
        }
        /// <summary>
        /// ����һ����ͬ���ĶԽ���
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static ArrayMatrix EyeMatrix(int dimension, double number)
        {
            ArrayMatrix X = new ArrayMatrix(dimension, dimension);
            double[][] x = X.Array;
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    if (i == j)
                        x[i][j] = number;
                }
            }
            return X;
        }
        /// <summary>
        /// ���������ֵ
        /// </summary>
        public double MaxValue
        {
            get
            {
                double max = data[0][0]; ;

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        if (data[i][j] > max)
                            max = data[i][j];
                    }
                }
                return max;
            }
        }
        /// <summary>
        /// ��������Сֵ
        /// </summary>
        public double MinValue
        {
            get
            {
                double min = data[0][0];

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        if (data[i][j] < min)
                            min = data[i][j];
                    }
                }
                return min;
            }
        }
        /// <summary>
        /// ���е�ƽ��ֵ
        /// </summary>
        public double MeanValue
        {
            get
            {
                double mean = 0.0;
                double sum = 0.0;

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        if (data[i][j] < mean)
                            sum += data[i][j];
                    }
                }
                mean = sum / (rows * columns);
                return mean;
            }
        }

        /// <summary>Returns a diagonal matrix of the given aboutSize.</summary>
        public static ArrayMatrix Diagonal(int rows, int columns, double value)
        {
            ArrayMatrix X = new ArrayMatrix(rows, columns);
            double[][] x = X.Array;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    x[i][j] = ((i == j) ? value : 0.0);
                }
            }
            return X;
        }

        /// <summary>Returns the matrix in a textual form.</summary>
        public override string ToString()
        {
            using (StringWriter writer = new StringWriter())
            {
                writer.Write("Matrix "); 

               writer.Write( MatrixUtil.GetFormatedText(this.Array)); 

                return writer.ToString();
            }
        }
        //������ϸ��⣡
        private double Hypotenuse(double a, double b)
        {
            if (Math.Abs(a) > Math.Abs(b))
            {
                double r = b / a;
                return Math.Abs(a) * Math.Sqrt(1 + r * r);
            }

            if (b != 0)
            {
                double r = a / b;
                return Math.Abs(b) * Math.Sqrt(1 + r * r);
            }

            return 0.0;
        }


        /// <summary>
        /// ��matrixת��Ϊstring,��string���л�char[]��Ȼ������ѹ��Ϊ�������ڴ���
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public byte[] CompressToByte(ArrayMatrix m)
        {
            string str = null;
            str += m.Rows.ToString();
            str += " ";
            str += m.Columns.ToString();
            str += " ";
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    if (m[i, j] != 0)
                    {
                        str += i.ToString();
                        str += " ";
                        str += j.ToString();
                        str += " ";
                        str += m[i, j].ToString();
                        str += " ";
                    }
                }
            }
            //string to byte
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(str);
            //�����ڴ�����GZip����ѹ��
            using (MemoryStream ms = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(ms, CompressionMode.Compress))
                {
                    gzip.Write(bytes, 0, bytes.Length);
                    gzip.Close();
                }
                bytes = ms.ToArray();
                ms.Close();
            }

            return bytes;
        }

        //�Ƚ�����ѹ��Ϊchar[]���ٷ����л�Ϊstring����תΪmatrix
        public ArrayMatrix EecompressToMatrix(byte[] inBytes)
        {
            //��ѹ��
            byte[] writeData = new byte[2048];//[4096]
            MemoryStream inStream = new MemoryStream(inBytes);
            GZipStream zipStream = new GZipStream(inStream, CompressionMode.Decompress);
            MemoryStream outStream = new MemoryStream();
            while (true)
            {
                int size = zipStream.Read(writeData, 0, writeData.Length);
                if (size > 0)
                {
                    outStream.Write(writeData, 0, size);
                }
                else
                {
                    break;
                }
            }
            inStream.Close();
            byte[] outData = outStream.ToArray();
            zipStream.Close();
            outStream.Close();

            //byte to string
            string str = System.Text.Encoding.ASCII.GetString(outData, 0, outData.Length);
            string[] words = str.Split(' ');

            //���ݹ��򣬸�����ֵ
            int arows = Convert.ToInt32(words[0]);
            int columns = Convert.ToInt32(words[1]);
            ArrayMatrix outMatrix = new ArrayMatrix(arows, columns);
            int lengths = words.Length;
            if ((lengths - 3) >= 3 && (Math.IEEERemainder((lengths - 3), 3) != 0))
            { throw new Exception("Ӧ����3�ı�����"); }
            int tu = (int)((lengths - 3) / 3);
            for (int i = 0; i < tu; i++)
            {
                int ii = Convert.ToInt32(words[1 + i * 3 + 1]);
                int jj = Convert.ToInt32(words[1 + i * 3 + 2]);
                double s_value = Convert.ToDouble(words[1 + i * 3 + 3]);
                outMatrix[ii, jj] = s_value;
            }
            return outMatrix;
            //
        }

        /// <summary>
        /// ��matrixת��Ϊstring,��string���л�char[]��Ȼ����
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public byte[] ToByte(ArrayMatrix m)
        {
            string str = null;
            str += m.Rows.ToString();
            str += " ";
            str += m.Columns.ToString();
            str += " ";
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    if (m[i, j] != 0)
                    {
                        str += i.ToString();
                        str += " ";
                        str += j.ToString();
                        str += " ";
                        str += m[i, j].ToString();
                        str += " ";
                    }
                }
            }
            //string to byte
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(str);
            ////�����ڴ�����GZip����ѹ��
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    using (GZipStream gzip = new GZipStream(ms, CompressionMode.Compress))
            //    {
            //        gzip.Write(bytes, 0, bytes.Length);
            //        gzip.Close();
            //    }
            //    bytes = ms.ToArray();
            //    ms.Close();
            //}

            return bytes;
        }

        //char[]�����л�Ϊstring����תΪmatrix
        public ArrayMatrix ToMatrix(byte[] inBytes)
        {
            ////��ѹ��
            //byte[] writeData = new byte[2048];//[4096]
            //MemoryStream inStream = new MemoryStream(inBytes);
            //GZipStream zipStream = new GZipStream(inStream, CompressionMode.Decompress);
            //MemoryStream outStream = new MemoryStream();
            //while (true)
            //{
            //    int aboutSize = zipStream.Read(writeData, 0, writeData.Length);
            //    if (aboutSize > 0)
            //    {
            //        outStream.Write(writeData, 0, aboutSize);
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}
            //inStream.Close();
            //byte[] outData = outStream.ToArray();
            //zipStream.Close();
            //outStream.Close();

            //byte to string
            string str = System.Text.Encoding.ASCII.GetString(inBytes, 0, inBytes.Length);
            string[] words = str.Split(' ');

            //���ݹ��򣬸�����ֵ
            int arows = Convert.ToInt32(words[0]);
            int columns = Convert.ToInt32(words[1]);
            ArrayMatrix outMatrix = new ArrayMatrix(arows, columns);
            int lengths = words.Length;
            if ((lengths - 3) >= 3 && (Math.IEEERemainder((lengths - 3), 3) != 0))
            { throw new Exception("Ӧ����3�ı�����"); }
            int tu = (int)((lengths - 3) / 3);
            for (int i = 0; i < tu; i++)
            {
                int ii = Convert.ToInt32(words[1 + i * 3 + 1]);
                int jj = Convert.ToInt32(words[1 + i * 3 + 2]);
                double s_value = Convert.ToDouble(words[1 + i * 3 + 3]);
                outMatrix[ii, jj] = s_value;
            }
            return outMatrix;
            //
        }


        //public Matrix XSS(Matrix L, Matrix A)
        //{
        //    //
        //    int dimension = L.Rows;
        //    int count = A.Columns;

        //    //Matrix Y = new Matrix(L.columns,A.columns);
        //    double[][] l = L.Array;
        //    double[][] a = A.Array;
        //    // Solve L*Y = B;����L��������m*m�׾���B��m*n�׾���Y��m*n�׾���

        //    Matrix Y = new Matrix(L.Columns, A.Columns); 
        //    for (int i = 0; i < Y.Rows; i++)
        //    {
        //        for (int j = 0; j < Y.Columns; j++)
        //        {
        //            Y[i, j] = A[i, j];
        //            for (int k = 0; k < i; k++)
        //            {
        //                Y[i, j] -= L[i, k] * Y[k, j];
        //            }
        //            Y[i, j] = Y[i, j] / L[i, i];
        //        }
        //    }
        //    Y = Y.Transpose();
        //    return Y;         
        //}

        ////���л�
        //public string toByte(Matrix A)
        //{

        //}

        #region override
        /// <summary>
        /// ����
        /// </summary>
        public override int ColCount { get { return Columns; } }
        /// <summary>
        /// ����
        /// </summary>
        public override int RowCount { get { return Rows; } }
        public override Geo.Algorithm.IMatrix GetInverse()
        {
            return this.Inverse;
        }
        /// <summary>
        /// ת�á�
        /// </summary>
        public override Geo.Algorithm.IMatrix Transposition { get { return this.Transpose(); } }

        #endregion
        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="fromIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override IMatrix SubMatrix(int fromIndex, int count)
        {
            int minRowOrCol = Math.Min(this.RowCount, this.ColCount);
            int maxEndIndexIncluded = Math.Min(fromIndex + count - 1, minRowOrCol - 1);
            //Matrix matrix = new Matrix(minRowOrCol, minRowOrCol);
            //for (int i =fromIndex; i < minEndIndexNotInclude; i++)
            //{
            //    for (int j = 0; j < minEndIndexNotInclude; j++)
            //    {
            //        matrix[i,j] = 
            //    }
            //}

            return this.Submatrix(fromIndex, maxEndIndexIncluded, fromIndex, maxEndIndexIncluded);
        }
    }
}
