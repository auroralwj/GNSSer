//2019.02.23, czs create in hongqing, 矩阵方程读取

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
    /// 矩阵方程读取
    /// </summary>
    public class TextMatrixEquationsReader : AbstractMatrixEquationsReader
    { 
        /// <summary>
      /// 本地文件读取
      /// </summary>
      /// <param name="path"></param>
      /// <param name="encoding"></param>
        public TextMatrixEquationsReader(string path, Encoding encoding = null)
            : this(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), encoding)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stream">数据流</param>
        /// <param name="encoding">编码</param>
        public TextMatrixEquationsReader(Stream stream, Encoding encoding) :
        base(stream, encoding)
        {

        }
        public override MatrixEquationManager Read()
        {
            ObjectTableManagerReader reader = new ObjectTableManagerReader(Stream);

            var tables = reader.Read();
            var keys = tables.Keys;
            List<string> names = new List<string>();
            foreach (var item in keys)
            {
                var name = MatrixEquationNameBuiler.GetName(item);
                if (!names.Contains(name))
                {
                    names.Add(name);
                }
            }
            MatrixEquationManager manager = new MatrixEquationManager();
            foreach (var name in names)
            {
                var leftName = MatrixEquationNameBuiler.GetLeftSideName(name);
                var rightName = MatrixEquationNameBuiler.GetRightSideName(name);
                var QOfrightName = MatrixEquationNameBuiler.GetInverseWeightNameOfRightSide(name);
                var left = tables.Get(leftName);
                var right = tables.Get(rightName);                 
                var QofRightSide = tables.Get(QOfrightName);
                 
                 
                var n = Matrix.Parse(left);
                var u = Matrix.Parse(right);

                Matrix q = null;
                if (QofRightSide != null)
                {
                    q = Matrix.Parse(QofRightSide);
                }
                var eq = new MatrixEquation(n, u, name) { QofU = q };
                manager[name] = eq;
            }
            return manager;
        }
    }
}
