//2014.10.30, czs, create in numu, 矩阵的读取

using System;
using System.IO;
using System.Collections.Generic;
using Geo.Utils;

namespace Geo.Algorithm
{
    /// <summary>
    /// 对角阵读取。
    /// </summary>
    public class BinarySparseMatrixReader : BaseBinaryMatrixReader
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="path">文件路径</param>
        public BinarySparseMatrixReader(string path)
            : base(path)
        {
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="Stream">文件路径</param>
        public BinarySparseMatrixReader(Stream Stream)
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
            
            SparseMatrix matr = new SparseMatrix(row, col);
            int itemCount = header.ContentCount;
            for (int i = 0; i < itemCount; i++)
            {
                //各个节点位置无关，可以考虑并行存取，但是由于内存共享，还得序列化读取。
                 int Row = BinaryReader.ReadInt32();
                 int Col = BinaryReader.ReadInt32();
                 double Value = BinaryReader.ReadDouble();
                 matr.Add(Row, Col, Value);
            }
            
            Char end = this.BinaryReader.ReadChar();
            if (end != BinarySpliter.EndOfContent)
            {
                throw new Exception("末尾数字匹配上啊！！！");
            }

            return matr;
        }
    }
}
