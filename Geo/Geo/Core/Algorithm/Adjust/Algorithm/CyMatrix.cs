using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;//Added for run code in parallel
using System.Diagnostics;//Added for the stopwatch
using Geo.Algorithm;

namespace Geo.Algorithm.Adjust
{
    
    //cyMatrix类是矩阵分块的类
    public class cyMatrix
    {
        //原矩阵进行分块，得到矩阵块数组，每个块矩阵的大小和首行、首列在整个矩阵中的位置需要标记出来，方便计算。
        //以下为属性
        public IMatrix OriginA;//原始矩阵，为节省内存，可注释
        public int[] arowBlock;//每个块矩阵的行数，为节省内存，可注释
        public int[] columnBlock;//每个块矩阵的列数，为节省内存，可注释
        public int[] firstarowBlock;//每个块矩阵的首行数位置标记，不可省略
        public int[] firstcolumnBlock;//每个块矩阵的首列数位置标记，不可省略
        public IMatrix[][] blockMatrix;//分块后的矩阵块数组，不可省略
        public int arows, columns;//行、列数
        public IMatrix[] blockMatrix1;//分块后的矩阵块数组，不可省略
        public IMatrix[] blockMatrix2;//分块后的矩阵块数组，不可省略
        public cyMatrix()
        {
            int CoreNum = Environment.ProcessorCount;//获取本机处理器个数
            this.arows = CoreNum;
            this.columns = CoreNum;//按核分行、列
            this.arowBlock = new int[arows * columns];
            this.columnBlock = new int[arows * columns];
            this.firstarowBlock = new int[arows * columns];
            this.firstcolumnBlock = new int[arows * columns];
        }
        public cyMatrix(int arows, int columns)
        {
            this.arows = arows;
            this.columns = columns;
            this.arowBlock = new int[arows * columns];
            this.columnBlock = new int[arows * columns];
            this.firstarowBlock = new int[arows * columns];
            this.firstcolumnBlock = new int[arows * columns];
            blockMatrix = new IMatrix[arows][];
            for (int i = 0; i < arows; i++)
            { blockMatrix[i] = new IMatrix[columns]; }
            this.blockMatrix1 = new IMatrix[arows * columns];
        }
        //构造函数
        public cyMatrix(IMatrix OriginA)
        {
            this.OriginA = OriginA;
            int CoreNum = Environment.ProcessorCount;//获取本机处理器个数
            this.arows = CoreNum;
            this.columns = CoreNum;//按核分行、列
            block();
            
        }

        //指定分块大小
        public cyMatrix(IMatrix OriginA, int arows, int columns)
        {
            this.OriginA = OriginA;
            this.arows = arows;
            this.columns = columns;//按核分行、列
            //block(arows,columns);
            block(4);
            
        }
        

        public void block()
        {
            int CoreNum = Environment.ProcessorCount;//获取本机处理器个数
            blockMatrix = new IMatrix[CoreNum][];
           // this.arows = CoreNum; 
           //this.columns = CoreNum;//按核分行、列
            int rows = OriginA.RowCount;//.Rows;
            int columns = OriginA.ColCount;//.Columns;
            arowBlock = averageArowBlock(rows);
            columnBlock = averageClowBlock(columns);
            firstarowBlock = firstArowBlock(rows);
            firstcolumnBlock = firstClowBlock(columns);
            ////////////////串行分块
            //for (int i = 0; i < CoreNum; i++)
            //{
            //    blockMatrix[i] = new Matrix[CoreNum];
            //    for (int j = 0; j < CoreNum; j++)
            //    {
            //        int countarow = arowBlock[i * CoreNum + j];
            //        int countcolumn = columnBlock[i * CoreNum + j];
            //        int firstarow = firstarowBlock[i * CoreNum + j];
            //        int firstcolumn = firstcolumnBlock[i * CoreNum + j];
            //        double[][] data = new double[countarow][];
            //        for (int ii = 0; ii < countarow; ii++)
            //        {
            //            data[ii] = new double[countcolumn];
            //            for (int jj = 0; jj < countcolumn; jj++)
            //            {
            //                data[ii][jj] = OriginA.Datas[firstarow + ii][firstcolumn + jj];
            //            }
            //        }
            //        blockMatrix[i][j] = new Matrix(data);
            //    }
            //}
            ////////////////并行分块
            Parallel.For(0, CoreNum, (int i) =>
            {
                blockMatrix[i] = new IMatrix[CoreNum];
                for (int j = 0; j < CoreNum; j++)
                {
                    int countarow = arowBlock[i * CoreNum + j];
                    int countcolumn = columnBlock[i * CoreNum + j];
                    int firstarow = firstarowBlock[i * CoreNum + j];
                    int firstcolumn = firstcolumnBlock[i * CoreNum + j];
                    double[][] data = new double[countarow][];
                    for (int ii = 0; ii < countarow; ii++)
                    {
                        data[ii] = new double[countcolumn];
                        for (int jj = 0; jj < countcolumn; jj++)
                        {
                            data[ii][jj] = OriginA.Array[firstarow + ii][firstcolumn + jj];
                        }
                    }
                    blockMatrix[i][j] = new ArrayMatrix(data);
                }
            });
        }

        public void block(int arow,int colum)
        {
            //int CoreNum = Environment.ProcessorCount;//获取本机处理器个数
            blockMatrix = new IMatrix[arow][];
            blockMatrix1=new IMatrix[arow*colum];
            // this.arows = CoreNum; 
            //this.columns = CoreNum;//按核分行、列
            int rows = OriginA.RowCount;//.Rows;
            int columns = OriginA.ColCount;//.Columns;
            arowBlock = averageArowBlock(rows);
            columnBlock = averageClowBlock(columns);
            firstarowBlock = firstArowBlock(rows);
            firstcolumnBlock = firstClowBlock(columns);
            DateTime sss = DateTime.Now;

            var span = DateTime.Now - sss;
            //Console.WriteLine(span.TotalMilliseconds + "msfenkuai00");
            ////////////////并行分块
            //Parallel.For(0, arow, (int i) =>
            //{
            //    blockMatrix[i] = new IMatrix[colum];
            //    for (int j = 0; j < colum; j++)
            //    {
            //        int countarow = arowBlock[i * colum + j];
            //        int countcolumn = columnBlock[i * colum + j];
            //        int firstarow = firstarowBlock[i * colum + j];
            //        int firstcolumn = firstcolumnBlock[i * colum + j];
            //        double[][] data = new double[countarow][];
            //        for (int ii = 0; ii < countarow; ii++)
            //        {
            //            data[ii]=GetVector(countcolumn, firstarow, firstcolumn, ii);
            //        }
            //        blockMatrix[i][j] = new Matrix(data);
            //    }
            //});
            
            sss = DateTime.Now;
            for (int i = 0; i < 4; i++)
            {
                if (i == 0 || i == 3)
                {
                    //sss = DateTime.Now;
                    int countOfarowBlock = arowBlock[i];
                    int countOfcolumnBlock = columnBlock[i];
                    int count = countOfarowBlock * (countOfarowBlock + 1) / 2 ;
                    double[] aa = new double[count];
                    int indexOffirstarowBlock=firstarowBlock[i];
                    int indexOffirstcolumnBlock=firstcolumnBlock[i];
                    //for (int j = 0; j < countOfarowBlock; j++)
                    //    GetSymmetricArray(i, aa, j, indexOffirstarowBlock, indexOffirstcolumnBlock);
                    //span = DateTime.Now - sss;
                    //Console.WriteLine(span.TotalMilliseconds + "msfenkuai11---0zhiqian");
                    //blockMatrix1[i] = new SymmetricMatrix(aa);
                    Parallel.For(0, countOfarowBlock, new Action<int>(delegate(int j)
                    {
                        GetSymmetricArray(i, aa, j, indexOffirstarowBlock, indexOffirstcolumnBlock);
                    }));
                    //span = DateTime.Now - sss;
                    //Console.WriteLine(span.TotalMilliseconds + "msfenkuai11---0");
                    ////for (int j = 0; j < arowBlock[i]; j++)
                    ////    GetSymmetricArray(i, aa, j);
                    blockMatrix1[i] = new SymmetricMatrix(aa);
                }
                if (i == 1)
                {
                    //sss = DateTime.Now;
                    double[][] aa = new double[arowBlock[i]][];
                    int indexOffirstarowBlock = firstarowBlock[i];
                    int indexOffirstcolumnBlock = firstcolumnBlock[i];
                    int countOfcolumnBlock = columnBlock[i];
                    Parallel.For(0, arowBlock[i], new Action<int>(delegate(int j)
                    {
                        aa[j] = GetMatrixArrayRow(i, j, countOfcolumnBlock,indexOffirstarowBlock, indexOffirstcolumnBlock);
                    }));
                    //span = DateTime.Now - sss;
                    //Console.WriteLine(span.TotalMilliseconds + "msfenkuai11---1");
                    blockMatrix1[i] = new ArrayMatrix(aa); 
                } 
            }
            //span = DateTime.Now - sss;
            //Console.WriteLine(span.TotalMilliseconds + "msfenkuai11");
        }

        public void block(int a)
        {
            int firstRowCount = OriginA.RowCount / 2;
            int lastRowCount = OriginA.RowCount - firstRowCount;
            int[] RowCount = { firstRowCount, lastRowCount, lastRowCount };
            int[] ColCount = { firstRowCount, firstRowCount, lastRowCount };
            int[] firstIndexOfRow = { 0, firstRowCount, firstRowCount };
            int[] firstIndexOfCol = { 0, 0, firstRowCount };
            double[][] array = OriginA.Array;
            blockMatrix1 = new IMatrix[3];
            DateTime start = DateTime.Now;
            //for (int i = 0; i < 3; i++)
            //{
            //    int row = RowCount[i];
            //    int col = ColCount[i];
            //    int firstRow = firstIndexOfRow[i];
            //    int firstCol = firstIndexOfCol[i];
            //    if (i == 0 || i == 2)
            //    {
            //        blockMatrix1[i] = GetDiagData(array, row, firstRow);
            //    }
            //    if (i == 1)
            //    {
            //        blockMatrix1[i] = GetOrdinaryMatrixData(array, row, col, firstRow, firstCol);
            //    }
            //}
            //for (int i = 0; i < 3; i++)
            //{
            //    int row = RowCount[i];
            //    int col = ColCount[i];
            //    int firstRow = firstIndexOfRow[i];
            //    int firstCol = firstIndexOfCol[i];
            //    if (i == 0 || i == 2)
            //    {
            //        blockMatrix1[i] = GetDiagData(array, row, firstRow);
            //    }
            //    if (i == 1)
            //    {
            //        blockMatrix1[i] = GetOrdinaryMatrixData(array, row, col, firstRow, firstCol);
            //    }
            //}
            //var span = DateTime.Now - start;
            //Console.WriteLine(span.TotalMilliseconds+"ms更改之后串行");
            start = DateTime.Now;
            Parallel.For(0, 3, new Action<int>(delegate(int i)
            {
                int row = RowCount[i];
                int col = ColCount[i];
                int firstRow = firstIndexOfRow[i];
                int firstCol = firstIndexOfCol[i];
                if (i == 0 || i == 2)
                {
                    
                    blockMatrix1[i]=GetDiagData(array, row, firstRow);
                }
                if (i == 1)
                {
                    blockMatrix1[i] = GetOrdinaryMatrixData(array, row, col, firstRow, firstCol);
                }
            }));
            //var span = DateTime.Now - start;
            //Console.WriteLine(span.TotalMilliseconds + "ms更改之后串行1");
        }
        #region 获取非对角阵矩阵数据
        private static IMatrix GetOrdinaryMatrixData(double[][] array, int row, int col, int firstRow, int firstCol)
        {
            int totalCount = row * col;
            double[][] x = new double[row][];
            //for (int j = 0; j < row; j++)
            //    x[j] = GetDataOnceRow(array, j, col, firstRow, firstCol);

            Parallel.For(0, row, new Action<int>(delegate(int j)
            {
                int RowIndex = j + firstRow;
                double[] rowA = array[RowIndex];
                x[j] = GetDataOnceRow(rowA, j, col, firstRow, firstCol); 
            }));
            return new ArrayMatrix(x);
        }
        private static double[] GetDataOnceRow(double[] rowA, int rowIndex, int colCount, int firstRow, int firstCol)
        {
            double[] rowX = new double[colCount];
            for (int col = 0; col < colCount; col++)
            {
                rowX[col] = rowA[col];
            }
            return rowX;
 
        }
        #endregion
        #region 返回非对角阵矩阵数据
        private static void GetOriginalOrdinaryMatrixData(double[][] array,double[][] x, int row, int col, int firstRow, int firstCol)
        {
            //for (int j = 0; j < row; j++)
            //{
            //    int RowIndex = j + firstRow;
            //    GetOriginalDataOnceRow(array[RowIndex], x[j], col);
            //}


            Parallel.For(0, row, new Action<int>(delegate(int j)
            {
                int RowIndex = j + firstRow;
               GetOriginalDataOnceRow(array[RowIndex],x[j],  col);
            }));
        }
        private static void GetOriginalDataOnceRow(double[] rowX,double[] rowA,  int colCount)
        {
            for (int col = 0; col < colCount; col++)
            {
                rowX[col] = rowA[col];
            }

        }
        #endregion
        #region 获取对角线矩阵数据
        private static IMatrix GetDiagData(double[][] array, int row, int firstRow)
        {
            int totalCount = row * (row + 1) / 2;
            double[] symetric = new double[totalCount];

            Parallel.For(0, row, new Action<int>(delegate(int j)
            {
                int RowIndex = j + firstRow;
                double[] rowA = array[RowIndex];
                GetDataOnce(symetric, rowA, j, firstRow);
            }));

            //for (int j = 0; j < row; j++)
            //{
            //    int RowIndex = j + firstRow;
            //    double[] rowA = array[RowIndex];
            //    GetDataOnce(symetric, rowA, j, firstRow);
            //}
            return new SymmetricMatrix(symetric);
        }

        private static void GetDataOnce(double[] resultArray, double[] rowA, int rowIndex, int firstRow)
        {
            for (int col = 0; col <= rowIndex; col++)
            {
                int ColIndex = col + firstRow;
                resultArray[rowIndex * (rowIndex + 1) / 2 + col] = rowA[ColIndex];
            }
        }
        #endregion
        #region 返回对角线矩阵数据
        private static void GetOriginalDiagData(double[][] array,double[][] symetric, int row, int firstRow)
        {
            //for (int j = 0; j < row; j++)
            //{
            //    int RowIndex = j + firstRow;
            //    double[] rowA = array[RowIndex];
            //    double[] rowSym = symetric[j];
            //    GetOriginalDataOnce(rowSym, rowA, j, firstRow);
            //}

            Parallel.For(0, row, new Action<int>(delegate(int j)
            {
                int RowIndex = j + firstRow;
                double[] rowA = array[RowIndex];
                double[] rowSym = symetric[j];
                GetOriginalDataOnce(rowSym, rowA, j, firstRow);
            }));
        }

        private static void GetOriginalDataOnce(double[] rowSymmetric, double[] rowA, int rowIndex, int firstRow)
        {
            for (int col = 0; col <= rowIndex; col++)
            {
                int ColIndex = col + firstRow;
                rowA[ColIndex] = rowSymmetric[col];
            }
        }
        #endregion



        private double[] GetMatrixArrayRow(int i, int j,int countOfcolumnBlock , int indexOffirstarowBlock, int indexOffirstcolumnBlock)
        {
            double[] aa1 = new double[countOfcolumnBlock];
            int jj = j + indexOffirstarowBlock;
            double[] xx = OriginA.Array[jj];
            for (int k = 0; k < aa1.Length; k++)
            {
                int kk = k + indexOffirstcolumnBlock;
                aa1[k] = xx[kk];
            }
            return aa1;
        }

        private void GetSymmetricArray(int i, double[] aa, int j, int indexOffirstarowBlock, int indexOffirstcolumnBlock)
        {
            int jj = j + indexOffirstarowBlock;
            double[] xx = OriginA.Array[jj];
            for (int k = 0; k <= j; k++)
            {
                int kk = k + indexOffirstcolumnBlock;
                aa[j * (j + 1) / 2 + k] = xx[kk];
            }
        }

        private double[] GetVector(int countcolumn, int firstarow, int firstcolumn, int ii)
        {
            double[] data = new double[countcolumn];
            double[] xx = OriginA.Array[firstarow + ii];
            for (int jj = 0; jj < countcolumn; jj++)
            {
                data[jj] = xx[firstcolumn + jj];
            }
            return data;
        }


        public IMatrix uniteBlockOfSymmetric(cyMatrix LA, int a)
        {
            int firstRowCount = OriginA.RowCount / 2;
            int lastRowCount = OriginA.RowCount - firstRowCount;
            int[] RowCount = { firstRowCount, lastRowCount, lastRowCount };
            int[] ColCount = { firstRowCount, firstRowCount, lastRowCount };
            int[] firstIndexOfRow = { 0, firstRowCount, firstRowCount };
            int[] firstIndexOfCol = { 0, 0, firstRowCount };
            
            IMatrix Array = new ArrayMatrix(OriginA.RowCount, OriginA.RowCount);
            double[][] array = Array.Array;
            //for (int i = 0; i < 3; i++)
            //{
            //    int row = RowCount[i];
            //    int col = ColCount[i];
            //    int firstRow = firstIndexOfRow[i];
            //    int firstCol = firstIndexOfCol[i];
            //    double[][] array1 = LA.blockMatrix1[i].Array;
            //    if (i == 0 || i == 2)
            //    {
            //        GetOriginalDiagData(array, array1, row, firstRow);
            //    }
            //    if (i == 1)
            //    {
            //        GetOriginalOrdinaryMatrixData(array, array1, row, col, firstRow, firstCol);
            //    }
            //}

            Parallel.For(0, 3, new Action<int>(delegate(int i)
            {
                int row = RowCount[i];
                int col = ColCount[i];
                int firstRow = firstIndexOfRow[i];
                int firstCol = firstIndexOfCol[i];
                double[][] array1 = LA.blockMatrix1[i].Array;
                if (i == 0 || i == 2)
                {
                    GetOriginalDiagData(array, array1, row, firstRow);
                }
                if (i == 1)
                {
                    GetOriginalOrdinaryMatrixData(array, array1, row, col, firstRow, firstCol);
                }
            }));
            return new SymmetricMatrix(array);
        }
        
        

        //辅助，计算每块矩阵分到的列数标记
        private int[] averageClowBlock(int clow)
        {
           // int coreNum = Environment.ProcessorCount;
            if (this.arows < 0) { throw new Exception(); }
            if (this.columns < 0) { throw new Exception(); }
            int[] block_mark = new int[this.arows * this.columns];//核数
            int[] block_aro = new int[this.columns];//列数
            int average = (int)(clow / this.columns);
            int leave = clow - average * this.columns;
            int tmp = 0;
            for (int i = 0; i < this.columns; i++)
            {
                if (i < leave)
                {
                    block_aro[i] = average + 1;
                }
                else
                {
                    block_aro[i] = average;
                }
                tmp += block_aro[i];
            }

            if (tmp != clow)
            {
                throw new Exception("出错！");
            }
            for (int i = 0; i < this.arows; i++)
            {
                for (int j = 0; j < this.columns; j++)
                {
                    block_mark[i * this.columns + j] = block_aro[j];
                }
            }
            return block_mark;
        }
        
        //辅助，计算每块矩阵分到的行数标记
        private int[] averageArowBlock(int arow)
        {
            //int coreNum = Environment.ProcessorCount;
            if (this.arows < 0) { throw new Exception(); }
            if (this.columns < 0) { throw new Exception(); }
            int[] block_mark = new int[this.arows * this.columns];
            int[] block_aro = new int[this.arows];
            int average = (int)(arow / this.arows);
            int leave = arow - average * this.arows;
            int tmp = 0;
            for (int i = 0; i < this.arows; i++)
            {
                // int markarow = 0;
                if (i < leave)
                {
                    block_aro[i] = average + 1;
                    //   markarow += average + 1;
                }
                else
                {
                    block_aro[i] = average;
                    //  markarow += average;
                }
                tmp += block_aro[i];
            }
            if (tmp != arow)
            {
                throw new Exception("出错！");
            }

            for (int j = 0; j < this.arows; j++)
            {
                for (int i = 0; i < this.columns; i++)
                {
                    block_mark[j * this.columns + i] = block_aro[j];
                }
            }
            return block_mark;
        }

        //辅助，计算每块矩阵分到的列数的首列在整个矩阵中的标记
        private int[] firstClowBlock(int clow)
        {
           // int coreNum = Environment.ProcessorCount;
            if (this.arows < 0) { throw new Exception(); }
            if (this.columns < 0) { throw new Exception(); }
            int[] block_mark = new int[this.arows * this.columns];//核数
            int[] block_aro = new int[this.columns];//列数
            int average = (int)(clow / this.columns);
            int leave = clow - average * this.columns;
            int tmp = 0;
            for (int i = 0; i < this.columns; i++)
            {
                if (i < leave)
                {
                    block_aro[i] = average + 1;
                }
                else
                {
                    block_aro[i] = average;
                }
                tmp += block_aro[i];
            }

            if (tmp != clow)
            {
                throw new Exception("出错！");
            }

            for (int i = 0; i < this.arows; i++)
            {
                for (int j = 0; j < this.columns; j++)
                {
                    if (j == 0)
                    { block_mark[i * this.columns + j] = 0; }
                    else
                    {
                        block_mark[i * this.columns + j] = block_mark[i * this.columns + j - 1] + block_aro[j - 1];
                    }
                }
            }
            return block_mark;
        }

        //辅助，计算每块矩阵分到的行数的首行在整个矩阵中的标记
        private int[] firstArowBlock(int arow)
        {
           // int coreNum = Environment.ProcessorCount;
            if (this.arows < 0) { throw new Exception(); }
            if (this.columns < 0) { throw new Exception(); }
            int[] block_mark = new int[this.arows * this.columns];
            int[] block_aro = new int[this.arows];
            int average = (int)(arow / this.arows);
            int leave = arow - average * this.arows;
            int tmp = 0;
            for (int i = 0; i < this.arows; i++)
            {
                // int markarow = 0;
                if (i < leave)
                {
                    block_aro[i] = average + 1;
                    //   markarow += average + 1;
                }
                else
                {
                    block_aro[i] = average;
                    //  markarow += average;
                }
                tmp += block_aro[i];
            }
            if (tmp != arow)
            {
                throw new Exception("出错！");
            }

            for (int j = 0; j < this.arows; j++)
            {
                for (int i = 0; i < this.columns; i++)
                {
                    if (j == 0)
                    { block_mark[j * this.columns + i] = 0; }
                    else
                    {
                        block_mark[j * this.columns + i] = block_mark[(j - 1) * this.columns + i] + block_aro[j - 1];
                    }
                }
            }
            return block_mark;
        }

    }
}