//2014.10.28, czs, create in numu, 矩阵的写

using System;
using System.IO;
using System.Collections.Generic;
using Geo.IO;
using System.Text;

namespace Geo.Algorithm
{
    /// <summary>
    /// 矩阵写。
    /// </summary>
    public abstract class AbstractBinaryMatrixWriter<TProduct> : AbstractBinaryWriter<TProduct>
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="path">文件路径</param>
        public AbstractBinaryMatrixWriter(string path)
            : base( new FileStream(path, FileMode.Create, FileAccess.Write), Encoding.UTF8)
        { 
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="Stream">数据流</param>
        public AbstractBinaryMatrixWriter(Stream Stream)
            : base(Stream, Encoding.UTF8)
   
        { 
        }  

    }
}
