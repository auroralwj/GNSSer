//2014.10.30, czs, create in numu, 矩阵的读取

using System;
using System.IO;
using System.Collections.Generic;
using Geo.Utils;

namespace Geo.Algorithm
{
    /// <summary>
    /// 对角阵写入。
    /// </summary>
    public class BinarySparseMatrixWriter : BaseBinaryMatrixWriter
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="path">文件路径</param>
        public BinarySparseMatrixWriter(string path)
            : base(path)
        {         
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="path">文件路径</param>
        public BinarySparseMatrixWriter(Stream Stream)
            : base(Stream)
        {
        }
        protected override void WriteMatrix(IMatrix matrix)
        {
            SparseMatrix sm = matrix as SparseMatrix;
            if (sm == null)//其它表示法
            {
                sm = new SparseMatrix(matrix);
            }

            BinaryWriter.Write(BinarySpliter.StartOfContent);
             
            foreach (var item in sm.MatrixItems)
            {
                BinaryWriter.Write(item.Row);
                BinaryWriter.Write(item.Col);
                BinaryWriter.Write(item.Value);
            }  
            BinaryWriter.Write(BinarySpliter.EndOfContent);
        } 
    }
}
