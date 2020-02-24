//2019.02.20, czs, create in hongqing, 矩阵的写

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo.IO;

namespace Geo.Algorithm
{
    /// <summary>
    /// 矩阵写。
    /// </summary>
    public   class TextMatrixReader : AbstractReader<IMatrix>// BaseBinaryMatrixReader 
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="path">文件路径</param>
        public TextMatrixReader(string path)
            : base(path)
        {
            this.FilePath = path;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="Stream">数据流</param>
        public TextMatrixReader(Stream Stream, Encoding encoding = null)
            : base(Stream,encoding)
        {
        }
        public string FilePath { get; set; } 
         
                 
        public override IMatrix Read()
        {
            var table = ObjectTableReader.Read(this.FilePath);

            return  Matrix.Parse(table);
        }
    }
}
