//2019.02.20, czs create in hongqing, 矩阵方程读取

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
    public class TextMatrixEquationReader : AbstractMatrixEquationReader
    { 
        /// <summary>
      /// 本地文件读取
      /// </summary>
      /// <param name="path"></param>
      /// <param name="encoding"></param>
        public TextMatrixEquationReader(string path, Encoding encoding = null)
            : base(path, encoding)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stream">数据流</param>
        /// <param name="encoding">编码</param>
        public TextMatrixEquationReader(Stream stream, Encoding encoding) :
        base(stream, encoding)
        {

        }

        public override MatrixEquation Read()
        {
            ObjectTableManagerReader reader = new ObjectTableManagerReader(Stream);

            var tables = reader.Read(); 
            
            var leftSide = tables.Get(MatrixEquationNameBuiler.L);  
            var rightSide = tables.Get(MatrixEquationNameBuiler.R);
            var QofRightSide = tables.Get(MatrixEquationNameBuiler.Q);

            var n = Matrix.Parse(leftSide);
            var u = Matrix.Parse(rightSide);
            Matrix q = null;
            if (QofRightSide != null)
            {
                q = Matrix.Parse(QofRightSide);
            }

            return new MatrixEquation(n, u) { QofU = q, Name = this.Name  };

        }
    }
}
