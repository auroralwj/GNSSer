//2014.10.28, czs, create in numu, 矩阵的读写

using System;
using System.IO;
using System.Collections.Generic;
using Geo.IO;
using System.Text;

namespace Geo.Algorithm
{
    /// <summary>
    /// 矩阵读。
    /// </summary>
    public abstract class AbstractBinaryMatrixReader<TProduct> : AbstractBinaryReader<TProduct>
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="path">文件路径</param>
        public AbstractBinaryMatrixReader(string path)
            : base(new FileStream(path, FileMode.Open, FileAccess.Read), Encoding.UTF8)
        { 
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="Stream">数据流</param>
        public AbstractBinaryMatrixReader(Stream Stream)
            : base(Stream, Encoding.UTF8)   
        { 
        }
    }
    /// <summary>
    /// 矩阵读。
    /// </summary>
    public abstract class AbstractBinaryMatrixReader<TProduct, TOption> : AbstractBinaryReader<TProduct, TOption>
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="path">文件路径</param>
        public AbstractBinaryMatrixReader(string path)
            : base(new FileStream(path, FileMode.Open, FileAccess.Read), Encoding.UTF8)
        { 
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="Stream">数据流</param>
        public AbstractBinaryMatrixReader(Stream Stream)
            : base(Stream, Encoding.UTF8)   
        { 
        }
    }

}
