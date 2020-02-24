//2014.10.28, czs, create in numu, 矩阵的写

using System;
using System.IO;
using System.Collections.Generic;

namespace Geo.Algorithm
{
    /// <summary>
    /// 矩阵写。
    /// </summary>
    public abstract class BaseBinaryMatrixReader : AbstractBinaryMatrixReader<IMatrix, MatrixHeader>
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="path">文件路径</param>
        public BaseBinaryMatrixReader(string path)
            : base(path)
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="Stream">数据流</param>
        public BaseBinaryMatrixReader(Stream Stream)
            : base(Stream)
        {
        }

        public virtual MatrixHeader ReadHeader()
        {
            BinaryMatrixHeaderReader reader = new BinaryMatrixHeaderReader(this.Stream);
            return reader.Read();
        }


        /// <summary>
        /// 数据流标识跳到内容起始位置  BinarySpliter.StartOfContent
        /// 下一步直接读取内容。
        /// </summary>
        protected void SkipHeader()
        {
            Char ch;
            while (this.BinaryReader.PeekChar() != -1)
            {
                if ((ch = this.BinaryReader.ReadChar()) == BinarySpliter.StartOfContent) break;
            }
        }
         
        public IMatrix Read()
        {
            var header = ReadHeader();
            var mat = Read(header);
            mat.ColNames = header.ColNames;
            mat.RowNames = header.RowNames;
            return mat;
        }
    }
}
