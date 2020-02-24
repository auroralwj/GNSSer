//2019.02.20, czs, create in hongqing, 矩阵的写

using System;
using System.IO;
using System.Collections.Generic;

namespace Geo.Algorithm
{
    /// <summary>
    /// 矩阵写。
    /// </summary>
    public   class BinaryMatrixReader : BaseBinaryMatrixReader 
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="path">文件路径</param>
        public BinaryMatrixReader(string path)
            : base(path)
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="Stream">数据流</param>
        public BinaryMatrixReader(Stream Stream)
            : base(Stream)
        {
        }
         BaseBinaryMatrixReader matrixReader;

        public override IMatrix Read(MatrixHeader option)
        {
            this.matrixReader = InitWriter(option);

           return   matrixReader.Read(option); 
        }

        private BaseBinaryMatrixReader InitWriter(MatrixHeader option)
        {
            BaseBinaryMatrixReader matrixReader = this.matrixReader;
            switch (option.MatrixType)
            {
                case MatrixType.Array:
                    matrixReader = new BinaryArrayMatrixReader(Stream);
                    break;
                case MatrixType.Vector:
                    break;
                case MatrixType.Sparse:
                    matrixReader = new BinaryArrayMatrixReader(Stream);
                    break;
                case MatrixType.Float:
                    break;
                case MatrixType.FloatWing:
                    break;
                case MatrixType.Diagonal:
                    matrixReader = new BinaryArrayMatrixReader(Stream);
                    break;
                case MatrixType.Symmetric:
                    matrixReader = new BinaryArrayMatrixReader(Stream);
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
                    matrixReader = new BinaryArrayMatrixReader(Stream);
                    break;
            }
            return matrixReader;
        }
    }
}
