//2014.10.28, czs, create in numu, 矩阵的读取

using System;
using System.IO;
using System.Collections.Generic;
using Geo.Utils;

namespace Geo.Algorithm
{
    /// <summary>
    /// 对角阵读取。
    /// </summary>
    public class BinarySymmetricMatrixReader : BaseBinaryMatrixReader
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="path">文件路径</param>
        public BinarySymmetricMatrixReader(string path)
            : base(path)
        {         
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="Stream">文件路径</param>
        public BinarySymmetricMatrixReader(Stream Stream)
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

            double[] vector = new double[header.ContentCount]; 

            for (int i = 0; i < header.ContentCount; i++)
            {
                vector[i] = BinaryReader.ReadDouble(); 
            }

            Char end = this.BinaryReader.ReadChar();
            if (end != BinarySpliter.EndOfContent)
            {
                throw new Exception("末尾数字匹配上啊！！！");
            }
            SymmetricMatrix dm = new SymmetricMatrix(vector);
            return dm;
        } 
    }
}
