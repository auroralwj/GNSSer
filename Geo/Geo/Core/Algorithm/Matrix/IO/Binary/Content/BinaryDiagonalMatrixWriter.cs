//2014.10.28, czs, create in numu, 矩阵的读取

using System;
using System.IO;
using System.Collections.Generic;
using Geo.Utils;

namespace Geo.Algorithm
{
    /// <summary>
    /// 对角阵写入。
    /// </summary>
    public class BinaryDiagonalMatrixWriter : BaseBinaryMatrixWriter
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="path">文件路径</param>
        public BinaryDiagonalMatrixWriter(string path)
            : base(path)
        {         
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="path">文件路径</param>
        public BinaryDiagonalMatrixWriter(Stream Stream)
            : base(Stream)
        {
        }
        protected override void WriteMatrix(IMatrix matrix)
        { 
            BinaryWriter.Write(BinarySpliter.StartOfContent);

            int row = matrix.RowCount;
            int col = matrix.ColCount; 
            for (int i = 0; i < row; i++)
            { 
                BinaryWriter.Write(matrix[i, i]); 
            }

            BinaryWriter.Write(BinarySpliter.EndOfContent);
        } 
    }
}
