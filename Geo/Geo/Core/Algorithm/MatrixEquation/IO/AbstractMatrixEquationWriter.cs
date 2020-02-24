//2019.02.20, czs, create in hongqing, 矩阵方程读写

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.IO;
using System.IO;

namespace Geo.Algorithm
{
    /// <summary>
    /// 矩阵方程读写
    /// </summary>
    public abstract class AbstractMatrixEquationWriter : AbstractWriter<MatrixEquation>
    {
        /// <summary>
        /// 本地文件读取
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        public AbstractMatrixEquationWriter(string path, Encoding encoding = null)
            : this(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), Encoding.Default)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stream">数据流</param>
        /// <param name="encoding">编码</param>
        public AbstractMatrixEquationWriter(Stream stream, Encoding encoding)
            :  base(stream, encoding)
        {

        }
         
    }
}
