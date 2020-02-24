//2014.10.28, czs, create in numu, 矩阵的读取

using System;
using System.IO;
using System.Collections.Generic;
using Geo.Utils;

namespace Geo.Algorithm
{
    /// <summary>
    /// 二维矩阵原生态（无压缩）读取。
    /// </summary>
    public class BinaryArrayMatrixReader : BaseBinaryMatrixReader
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="path">文件路径</param>
        public BinaryArrayMatrixReader(string path)
            : base(path)
        {         
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="path">文件路径</param>
        public BinaryArrayMatrixReader(Stream Stream)
            : base(Stream)
        {
        }
        /// <summary>
        /// 写方法。Version 1.
        /// </summary>
        public override IMatrix Read(MatrixHeader header)
        {
            SkipHeader();

            int row = header.RowCount;
            int col = header.ColCount; 

            Geo.Algorithm.ArrayMatrix matrix = new Geo.Algorithm.ArrayMatrix(row, col); 
              
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (header.MatrixType == MatrixType.FloatWing && i != j)
                        matrix[i, j] = BinaryReader.ReadSingle();
                    else matrix[i, j] = BinaryReader.ReadDouble();
                }
            }
            Char end = this.BinaryReader.ReadChar();
            if (end != BinarySpliter.EndOfContent)
            {
                throw new Exception("末尾数字匹配上啊！！！");
            }
            return matrix;
        } 
    }
}
