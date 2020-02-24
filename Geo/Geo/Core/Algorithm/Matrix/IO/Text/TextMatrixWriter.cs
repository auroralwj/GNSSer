//2019.02.20, czs, create in hongqing, 矩阵的写

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
    public class TextMatrixWriter : AbstractWriter<IMatrix>
    {

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="encoding">编码</param>
        public TextMatrixWriter(string path, Encoding encoding = null)
            : base(path, encoding??Encoding.Default)
        {
        } 
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="Stream">数据流</param>
        /// <param name="encoding">编码</param>
        public TextMatrixWriter(Stream Stream, Encoding encoding = null)
            : base(Stream, encoding ?? Encoding.Default)
        {
        }

        public override void Write(IMatrix matrix)
        {
            Matrix mat = new Matrix(matrix);
            ObjectTableWriter.Write(mat.GetObectTable(), Stream, this.Encoding);
        }
          
    }
}
