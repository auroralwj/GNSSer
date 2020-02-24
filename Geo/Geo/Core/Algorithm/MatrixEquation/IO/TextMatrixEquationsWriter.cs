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
    public class TextMatrixEquationsWriter : AbstractMatrixEquationsWriter
    {
        /// <summary>
        /// 本地文件读取
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        public TextMatrixEquationsWriter(string path, Encoding encoding = null)
            : this(new FileStream(path, FileMode.Create), Encoding.Default)
        {
            this.FilePath = path;
        }

        string FilePath { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stream">数据流</param>
        /// <param name="encoding">编码</param>
        public TextMatrixEquationsWriter(Stream stream, Encoding encoding)
            :  base(stream, encoding)
        {

        }

        public override void Write(MatrixEquationManager product)
        {
            ObjectTableManager objects = product.GetObjectTables();
            objects.WriteAsOneFile(Stream); 
        }
    }
}
