using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Utils;

namespace Gnsser.Data.Sinex
{
    /// <summary>
    /// Sinex 文件的矩阵与数组之间的转换。
    /// </summary>
    public class SinexMatrixConvertor
    {
        /// <summary>
        /// 获取其内的标准差
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        public static double[] GetStdDevs(List<MatrixLine> list)
        {
            double[][] matr = GetMatrix(list,true);
            double [] stdDevs = new double[matr.Length];
            for (int i = 0; i < matr.Length; i++)
            {
                stdDevs[i] = Math.Sqrt( matr[i][i]);                
            }
            return stdDevs;
        }

        /// <summary>
        /// 合并，产生新的。
        /// </summary>
        /// <param name="linesA"></param>
        /// <param name="linesB"></param>
        /// <returns></returns>
        public static List<MatrixLine> Merge(List<MatrixLine> linesA, List<MatrixLine> linesB)
        {
            double[][] matrixA = SinexMatrixConvertor.GetMatrixArrayJagged(linesA);
            double[][] matrixB = SinexMatrixConvertor.GetMatrixArrayJagged(linesB);
            double[][] matrixC = MatrixUtil.BuildMatrix(matrixA, matrixB);
            List<MatrixLine> lines = SinexMatrixConvertor.GetMatrixLines(matrixC);
            return lines;
        }
        /// <summary>
        /// 合并，产生新的。
        /// 乘以协方差系数。 varFactor = NowFactor / OldFactor
        /// </summary>
        /// <param name="list1"></param>
        /// <param name="varFactorA"></param>
        /// <param name="list2"></param>
        /// <param name="varFactorB"></param>
        /// <returns></returns>
        public static List<MatrixLine> Merge(List<MatrixLine> linesA, double varFactorA, List<MatrixLine> linesB, double varFactorB)
        {
            double[][] matrixA = SinexMatrixConvertor.GetMatrixArrayJagged(linesA);
            double[][] matrixB = SinexMatrixConvertor.GetMatrixArrayJagged(linesB);
            //乘以协方差系数。
            MatrixUtil.Multiply(matrixA, varFactorA);
            MatrixUtil.Multiply(matrixB, varFactorB);

            double[][] matrixC = MatrixUtil.BuildMatrix(matrixA, matrixB);
            List<MatrixLine> lines = SinexMatrixConvertor.GetMatrixLines(matrixC);
            return lines;
        }


        /// <summary>
        /// 将数组转换为Sinex行。
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static List<MatrixLine> GetMatrixLines(double[][] matrix)
        {
            List<MatrixLine> list = new List<MatrixLine>();

            List<MatrixUnit> lineUnits = new List<MatrixUnit>();
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j <= i; j++)//只保存对角阵
                { 
                    lineUnits.Add(new MatrixUnit() { Row = i + 1, Col = j + 1, Val = matrix[i][j] });

                    if (lineUnits.Count == 3 || j==i)
                    {
                        list.Add(new MatrixLine() { Row = lineUnits[0].Row, Col = lineUnits[0].Col, MatrixUnits = lineUnits });
                        lineUnits = new List<MatrixUnit>();
                    }
                }            
            }
            return list;
        }
        /// <summary>
        /// 获取指定大小的交错矩阵。
        /// 从左上角开始截取。
        /// </summary>
        /// <param name="aboutSize"></param>
        /// <param name="isTriangular">是否为三角阵，如果是在只读一半就行了，加快读取时间。</param>
        /// <returns></returns>
        public static double[][] GetMatrix(List<MatrixLine> Items, int size, bool isTriangular = true)
        {
            double[][] result = new double[size][];
            for (int i = 0; i < result.Length; i++) result[i] = new double[size];

            double[][] all = GetMatrixArrayJagged(Items, isTriangular);

            for (int i = 0; i < all.Length; i++)
            {
                for (int j = 0; j < all[0].Length; j++)
                {
                    result[i][j] = all[i][j];
                }
            }
            return result;
        }
        /// <summary>
        /// 交错数组.是否为三角阵，如果是在只读一半就行了，加快读取时间。
        /// </summary>
        /// <param name="Items"></param>
        /// <param name="isTriangular"></param>
        /// <returns></returns>
        public static double[][] GetMatrix(List<MatrixLine> Items, bool isTriangular = true)
        {
            return GetMatrixArrayJagged(Items, isTriangular);
        }
        /// <summary>
        /// 交错数组.是否为三角阵，如果是在只读一半就行了，加快读取时间。
        /// </summary>
        /// <returns></returns>
        public static double[][] GetMatrixArrayJagged(List<MatrixLine> Items, bool isTriangular = true)
        {
            if (Items == null || Items.Count == 0) return null;
            MatrixLine last = Items[Items.Count - 1];
            int rowCount = last.Row;
            int colCount = last.MatrixUnits[last.MatrixUnits.Count - 1].Col;

            double[][] matrixArray = new double[rowCount][];
            for (int i = 0; i < rowCount; i++) matrixArray[i] = new double[colCount];

            foreach (MatrixLine line in Items)
                foreach (var unit in line.MatrixUnits)
                {
                    matrixArray[unit.Row - 1][unit.Col - 1] = unit.Val;
                    if (isTriangular && unit.Row != unit.Col)
                    {
                        matrixArray[unit.Col - 1][unit.Row - 1] = unit.Val;
                    }
                }

            return matrixArray;
        }

        /// <summary>
        /// 获取指定大小的二位数组矩阵。
        /// 从左上角开始截取。
        /// </summary>
        /// <param name="aboutSize"></param>
        /// <param name="isTriangular">是否为三角阵，如果是在只读一半就行了，加快读取时间。</param>
        /// <returns></returns>
        public static double[,] GetMatrixArray2D(List<MatrixLine> Items, int size, bool isTriangular = true)
        {
            double[,] result = new double[size, size];

            double[][] all = GetMatrixArrayJagged(Items, isTriangular);

            for (int i = 0; i < all.Length; i++)
            {
                for (int j = 0; j < all[0].Length; j++)
                {
                    result[i, j] = all[i][j];
                }
            }
            return result;
        }
        /// <summary>
        /// 二位数组
        /// </summary>
        /// <param name="isTriangular">是否为三角阵，如果是在只读一半就行了，加快读取时间。</param>
        /// <returns></returns>
        public static double[,] GetMatrixArray2D(List<MatrixLine> Items, bool isTriangular = true)
        {
            MatrixLine last = Items[Items.Count - 1];
            int rowCount = last.Row;
            int colCount = last.MatrixUnits[last.MatrixUnits.Count - 1].Col;

            double[,] matrixArray = new double[rowCount, colCount];

            foreach (MatrixLine line in Items)
                foreach (var unit in line.MatrixUnits)
                {
                    matrixArray[unit.Row - 1, unit.Col - 1] = unit.Val;
                    if (isTriangular && unit.Row != unit.Col)
                    {
                        matrixArray[unit.Col - 1, unit.Row - 1] = unit.Val;
                    }
                }
            return matrixArray;
        }

    }



}
