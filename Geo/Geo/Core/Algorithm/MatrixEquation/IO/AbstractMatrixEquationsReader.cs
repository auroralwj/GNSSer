//2019.02.23, czs, create in hongqing, 矩阵方程读取

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.IO;
using System.IO;
using System.Text;

namespace Geo.Algorithm
{
    /// <summary>
    /// 矩阵方程读取
    /// </summary>
    public abstract class AbstractMatrixEquationsReader : AbstractReader<MatrixEquationManager>
    { 
        /// <summary>
      /// 本地文件读取
      /// </summary>
      /// <param name="path"></param>
      /// <param name="encoding"></param>
        public AbstractMatrixEquationsReader(string path, Encoding encoding = null)
            : this(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), encoding)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stream">数据流</param>
        /// <param name="encoding">编码</param>
        public AbstractMatrixEquationsReader(Stream stream, Encoding encoding) :
        base(stream, encoding)
        {

        }
    }
}
