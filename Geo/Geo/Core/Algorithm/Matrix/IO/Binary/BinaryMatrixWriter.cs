//2019.02.20, czs, create in hongqing, 矩阵的写

using System;
using System.IO;
using System.Collections.Generic;

namespace Geo.Algorithm
{
    /// <summary>
    /// 矩阵写。
    /// </summary>
    public  class BinaryMatrixWriter : BaseBinaryMatrixWriter
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="path">文件路径</param>
        public BinaryMatrixWriter(string path)
            : base(path)
        {
        }
        BaseBinaryMatrixWriter matrixWriter;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="Stream">数据流</param>
        public BinaryMatrixWriter(Stream Stream)
            : base(Stream)
        {
        }

        public override void Write(IMatrix matrix)
        {
            matrix = InitWriter(matrix);
            matrixWriter.Write(matrix);
        }

        private IMatrix InitWriter(IMatrix matrix)
        {
            if (matrix is Matrix)
            {
                matrix = ((Matrix)matrix)._matrix;
            }
            switch (matrix.MatrixType)
            {
                case MatrixType.Array:
                    matrixWriter = new BinaryArrayMatrixWriter(Stream);
                    break;
                case MatrixType.Vector:
                    break;
                case MatrixType.Sparse:
                    matrixWriter = new BinarySparseMatrixWriter(Stream);
                    break;
                case MatrixType.Float:
                    break;
                case MatrixType.FloatWing:
                    break;
                case MatrixType.Diagonal:
                    matrixWriter = new BinaryDiagonalMatrixWriter(Stream);
                    break;
                case MatrixType.Symmetric:
                    matrixWriter = new BinarySymmetricMatrixWriter(Stream);
                    break;
                case MatrixType.VectorMatrix:
                    break;
                case MatrixType.ZeroMatrix:
                    break;
                case MatrixType.ConstMatrix:
                    break;
                case MatrixType.ResizeableMatrix:
                    break;
                case MatrixType.Unknow:
                default:
                    matrixWriter = new BinaryArrayMatrixWriter(Stream);
                    break;
            }

            return matrix;
        }

        /// <summary>
        /// 只写矩数据阵内容本身
        /// </summary>
        /// <param name="matrix">矩阵</param>
        protected override void WriteMatrix(IMatrix matrix)
        {
            matrixWriter.Write(matrix);
        }
    }
}
