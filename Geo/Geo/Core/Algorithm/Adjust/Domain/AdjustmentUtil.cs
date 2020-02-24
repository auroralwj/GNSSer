//2014.08.20,czs,edit,提取平差常用公共向量和矩阵
//2014.09.01,czs,edit,封装具有权值的向量
//2015.05.07, lly， 微信修改，  IMatrix dL = A.Multiply(X).Minus(L);
//2016.10.21, double & czs, 加入快速计算的实用方法，如ATPA

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using Geo.Common; 
using Geo.Algorithm;

using System.Threading.Tasks;
using Geo.IO;


namespace Geo.Algorithm.Adjust
{ 
     
    /// <summary>
    /// 平差通用接口。包含观测值，先验值，预测值和估计值。
    /// </summary>
    public static  class AdjustmentUtil 
    {
        static  ILog log = Log.GetLog(typeof(AdjustResultMatrix));
         
  

        #region 实用快速计算方法
        #region BQBT 计算
        /// <summary>
        /// 实用方法，Q为对称阵,速度较慢？摘抄自 宋力杰测量平差程序设计 P11
        /// </summary>
        /// <param name="B"></param>
        /// <param name="Q">可能非对称</param>
        /// <returns></returns>
        public static IMatrix BQBT(IMatrix B, IMatrix Q)
        { 
            if (B is DiagonalMatrix)
            {
                var B1 = B as DiagonalMatrix;
                return BQBT(B1, Q);
            }
            int rowCount = B.RowCount;
            int colCountB = B.ColCount;
            int count = (rowCount + 1) * rowCount / 2;
            double[] resultArray = new double[count];
            double[][] dataB = B.Array;
            double[][] dataQ = Q.Array;
            

            if (rowCount < 100)
            {
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    BqbtOnce(resultArray, dataB, dataQ, rowIndex);
                }
            }
            else // 并行
            {
                Parallel.For(0, rowCount, new Action<int>(delegate(int rowIndex)
                {
                    BqbtOnce(resultArray, dataB, dataQ, rowIndex);
                }));
            }

            return new SymmetricMatrix(resultArray);
        }

        /// <summary>
        /// 计算一次
        /// </summary>
        /// <param name="resultArray"></param>
        /// <param name="dataB"></param>
        /// <param name="dataQ"></param>
        /// <param name="rowIndex"></param>
        private static void BqbtOnce(double[] resultArray, double[][] dataB, double[][] dataQ, int rowIndex)
        {
            var dataBVector = dataB[rowIndex];
            for (int colIndex = 0; colIndex <= rowIndex; colIndex++)
            {
                var dataBVector1 = dataB[colIndex];
                double val = 0.0;
                int col = dataBVector1.Length;
                for (int k = 0; k < col; k++)
                {
                    var dataQVector = dataQ[k];
                    int len = dataQVector.Length;
                    for (int s = 0; s < len; s++)
                    {
                        // if( isDialoge )
                        val += dataBVector[k] * dataQVector[s] * dataBVector1[s];
                    }
                }

                int index = GetIndexOfLowerTriangleMatrix(rowIndex, colIndex);
                resultArray[index] += val;
            }
        }

        /// <summary>
        /// 当B为对角阵时
        /// </summary>
        /// <param name="B"></param>
        /// <param name="Q"></param>
        /// <returns></returns>
        public static IMatrix BQBT(DiagonalMatrix B, IMatrix Q)
        {
            int rowCount = B.RowCount;
            int colCount = B.ColCount;
            int count = (rowCount + 1) * rowCount / 2;
            double[] resultArray = new double[count];
            double[] diagnoalB = B.Vector;
            double[][] qArray = Q.Array;
            if (rowCount < 100)
            {
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    if (diagnoalB[rowIndex] == 0) { continue; }

                    BqbtOnce(ref resultArray, diagnoalB, qArray, rowIndex);

                }
            }
            else // 并行
            {
                Parallel.For(0, rowCount, new Action<int>(delegate(int rowIndex)
                {
                    if (diagnoalB[rowIndex] == 0) { return; }

                    BqbtOnce(ref resultArray, diagnoalB, qArray, rowIndex);
                }));
            }
            return new SymmetricMatrix(resultArray);
        }

        /// <summary>
        /// 计算一次
        /// </summary>
        /// <param name="resultArray"></param>
        /// <param name="diagnoalB"></param>
        /// <param name="qArray"></param>
        /// <param name="rowIndex"></param>
        private static void BqbtOnce(ref double[] resultArray, double[] diagnoalB, double[][] qArray, int rowIndex)
        {
            var dataQVector = qArray[rowIndex];
            for (int colIndex = 0; colIndex <= rowIndex; colIndex++)
            {
                int index = GetIndexOfLowerTriangleMatrix(rowIndex, colIndex);
                resultArray[index] = diagnoalB[rowIndex] * diagnoalB[colIndex] * dataQVector[colIndex];
            }
        }
        #endregion BQBT

        #region ATPA 计算
        /// <summary>
        /// 快捷计算方法，返回 SymmetricMatrix
        /// </summary>
        /// <param name="A">系数阵</param>
        /// <param name="P">对角阵</param>
        /// <returns></returns>
        public static IMatrix ATPA(IMatrix A, IMatrix P)
        {
            if (!(P is DiagonalMatrix))
            {
                return A.Transposition.Multiply(P).Multiply(A);
            }
            
            var pM = P as DiagonalMatrix;
            IMatrix AT = A.Transposition;
            int row = AT.RowCount;
            int count = (row + 1) * row / 2;
            double[] resultArray = new double[count];
            double[] resultArray1 = new double[count];
            double[] resultArray2 = new double[count];
            double[] resultArray3 = new double[count];
            double[] diagonalP = pM.Vector;
            
            double[][] aArray = AT.Array;
            DateTime start = DateTime.Now;
            if (A is SparseMatrix)
            {
                var A11 = AT as SparseMatrix;
                if (row < 10000)
                {
                    for (int i = 0; i < row; i++)
                    {

                        if (A11.Data.Contains(i))
                            AtpaOnce2(resultArray2, aArray, diagonalP, i, A11.Data[i]);
                    }
                }
                else // 并行
                {
                    Parallel.For(0, row, new Action<int>(delegate(int i)
                    {
                        if (A11.Data.Contains(i))
                            AtpaOnce2(resultArray2, aArray, diagonalP, i, A11.Data[i]);
                    }));
                }
                return new SymmetricMatrix(resultArray2);

            }
            //var span = DateTime.Now - start;
            //Console.WriteLine(span.TotalMilliseconds+"ms");
            //start = DateTime.Now;
            //if (A is SparseMatrix)
            //{
            //    var A11 = AT as SparseMatrix;
            //    if (row < 100)
            //    {
            //        for (int i = 0; i < row; i++)
            //        {
            //            if (A11.Data.Contains(i))
            //                AtpaOnce3(resultArray3, diagonalP, i, A11.Data);
            //        }
            //    }
            //    else // 并行
            //    {
            //        Parallel.For(0, row, new Action<int>(delegate(int i)
            //        {
            //            if (A11.Data.Contains(i))
            //                AtpaOnce3(resultArray3, diagonalP, i, A11.Data);
            //        }));
            //    }
            //    //return new SymmetricMatrix(resultArray);

            //}
            //span = DateTime.Now - start;
            //Console.WriteLine(span.TotalMilliseconds+"ms");
            //start = DateTime.Now;
            //if (row < 100)
            //{
            //    for (int i = 0; i < row; i++)
            //    {
            //        AtpaOnce2(resultArray1, aArray, diagonalP, i); 
            //    }
            //}
            //else // 并行
            //{
            //    Parallel.For(0, row, new Action<int>(delegate(int i)
            //    {
            //        AtpaOnce2(resultArray1, aArray, diagonalP, i); 
            //    }));
            //}
            //span = DateTime.Now - start;
            //Console.WriteLine(span.TotalMilliseconds + "ms");
            //start = DateTime.Now;
            if (row < 10000)
            { 
                for (int i = 0; i < row; i++)
                {
                    //AtpaOnce2(resultArray, aArray, diagonalP, i); 
                    AtpaOnce(resultArray, aArray, diagonalP, i);
                }
            }
            else // 并行
            {
                Parallel.For(0, row, new Action<int>(delegate(int i)
                {
                    //AtpaOnce2(resultArray, aArray, diagonalP, i); 
                    AtpaOnce(resultArray, aArray, diagonalP, i);
                }));
            }
            //span = DateTime.Now - start;
            //Console.WriteLine(span.TotalMilliseconds + "ms");
            return new SymmetricMatrix(resultArray);

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
        }
        /// <summary>
        /// 计算一次
        /// </summary>
        /// <param name="resultArray"></param>
        /// <param name="aArray"></param>
        /// <param name="diagonalP"></param>
        /// <param name="rowIndex"></param>
        private static void AtpaOnce(double[] resultArray, double[][] aArray, double[] diagonalP, int rowIndex)
        {
            double[] rowA = aArray[rowIndex];
            for (int colIndex = 0; colIndex <= rowIndex; colIndex++)
            {
                double[] rowA2 = aArray[colIndex];
                double cellVal = 0;
                int col = rowA2.Length;
                for (int k = 0; k < col; k++)
                {
                    cellVal += rowA[k] * rowA2[k] * diagonalP[k];
                }
                int index = GetIndexOfLowerTriangleMatrix(rowIndex, colIndex);
                resultArray[index] = cellVal;
            }
        }

        private static void AtpaOnce2(double[] resultArray,double[][] rightArray, double[] diagonalP, int rowIndex)
        {
            double[] leftRow = rightArray[rowIndex];
            for (int colIndex = 0; colIndex <= rowIndex; colIndex++)
            {
                double[] rightCol = rightArray[colIndex];
                double sum = MultiplyAndSum(leftRow, rightCol, diagonalP);
                int index = GetIndexOfLowerTriangleMatrix(rowIndex, colIndex);
                resultArray[index] = sum;
            }
        }
        private static double MultiplyAndSum(double[] vetorA, double[] vetorB, double[] diagonalP)
        {
            double sum = 0;
            int colsA = vetorA.Length;
            for (int k = 0; k < colsA; k++)
            {
                sum += vetorA[k] * vetorB[k] * diagonalP[k];
            }
            return sum;
        }
        private static void AtpaOnce3(double[] resultArray, double[] diagonalP, int rowIndex, DoubleKeyDictionary<int,  double> ab)
        {
            var ab1 = ab[rowIndex];
            for (int colIndex = 0; colIndex <= rowIndex; colIndex++)
            {
                if (!ab.Contains(colIndex)) continue;
                var right = ab[colIndex];
                double cellVal = 0;
                foreach (var qqqq in ab1)
                {
                    int q = qqqq.Key;
                    if(right.ContainsKey(q))
                        cellVal += qqqq.Value *right[q] * diagonalP[q];
                }
                int index = GetIndexOfLowerTriangleMatrix(rowIndex, colIndex);
                resultArray[index] = cellVal;
            }
        }
        private static void AtpaOnce2(double[] resultArray, double[][] rightArray, double[] diagonalP, int rowIndex,   Dictionary<int, double> ab)
        {
            for (int colIndex = 0; colIndex <= rowIndex; colIndex++)
            {
                double[] rightCol = rightArray[colIndex];
                double cellVal = 0;
                foreach (var qqqq in ab)
                {
                    int q=qqqq.Key;
                    cellVal += qqqq.Value * rightCol[q] * diagonalP[q];
                }
                int index = GetIndexOfLowerTriangleMatrix(rowIndex, colIndex);
                resultArray[index] = cellVal;
            }
        }


        #endregion

        /// <summary>
        /// 获取以一维数组存储的下三角矩阵的编号
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        private static int GetIndexOfLowerTriangleMatrix(int rowIndex, int colIndex)
        {
            int index = rowIndex * (rowIndex + 1) / 2 + colIndex;
            return index;
        }
        /// <summary>
        /// 快捷方法
        /// </summary>
        /// <param name="A"></param>
        /// <param name="Q"></param>
        /// <param name="L"></param>
        /// <returns></returns>
        public static IMatrix ATPL(IMatrix A, IMatrix Q, IMatrix L)
        {
            int row = A.RowCount;
            int column = A.ColCount;
            if (A.RowCount != Q.RowCount || Q.ColCount != L.RowCount)
                throw new DimentionException("维数相同才可以计算！");
            ArrayMatrix ATPL = new ArrayMatrix(column, 1); 
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    ATPL[j, 0] += A[i, j] * L[i, 0] / Q[i, i];
                }
            }
            return ATPL;
        }
        #endregion
         
    }
}