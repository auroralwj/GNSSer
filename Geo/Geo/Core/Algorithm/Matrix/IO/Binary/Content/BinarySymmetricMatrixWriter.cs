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
    public class BinarySymmetricMatrixWriter : BaseBinaryMatrixWriter
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="path">文件路径</param>
        public BinarySymmetricMatrixWriter(string path)
            : base(path)
        {
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="Stream">文件路径</param>
        public BinarySymmetricMatrixWriter(Stream Stream)
            : base(Stream)
        {
        }
        /// <summary>
        /// 写矩阵
        /// </summary>
        /// <param name="matrix"></param>
        protected override void WriteMatrix(IMatrix matrix)
        {
            BinaryWriter.Write(BinarySpliter.StartOfContent);

            SymmetricMatrix sm = matrix as SymmetricMatrix;
            if (sm == null)
                throw new Exception("请使用对称矩阵。");

            foreach (var item in sm.Vector)
            {
                BinaryWriter.Write(item);
            }

            BinaryWriter.Write(BinarySpliter.EndOfContent);
        }
    }
}
