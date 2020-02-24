//2014.10.28, czs, create in numu, 矩阵的写

using System;
using System.IO;
using System.Collections.Generic;

namespace Geo.Algorithm
{
    /// <summary>
    /// 矩阵写。
    /// </summary>
    public abstract class BaseBinaryMatrixWriter : AbstractBinaryMatrixWriter<IMatrix>
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="path">文件路径</param>
        public BaseBinaryMatrixWriter(string path)
            : base(path)
        {
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="Stream">数据流</param>
        public BaseBinaryMatrixWriter(Stream Stream)
            : base(Stream)
        {
        }

        public override void Write(IMatrix matrix)
        {
            MatrixHeader header= MatrixHeader.GetDefaultHeader(matrix); 
            WriteHeader(header);

            WriteMatrix(matrix);
        }
        protected virtual void WriteHeader(MatrixHeader header)
        {
            BinaryMatrixHeaderWriter HeaderWriter = new BinaryMatrixHeaderWriter(this.Stream);
            HeaderWriter.Write(header);
        }
        /// <summary>
        /// 只写矩数据阵内容本身
        /// </summary>
        /// <param name="product">矩阵</param>
        protected abstract void WriteMatrix(IMatrix matrix);
    }
}
