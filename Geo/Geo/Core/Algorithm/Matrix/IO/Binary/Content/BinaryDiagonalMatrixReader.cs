//2014.10.28, czs, create in numu, 矩阵的读取

using System;
using System.IO;
using System.Collections.Generic;
using Geo.Utils;

namespace Geo.Algorithm
{
    /// <summary>
    /// 对角阵读取。
    /// </summary>
    public class BinaryDiagonalMatrixReader : BaseBinaryMatrixReader
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="path">文件路径</param>
        public BinaryDiagonalMatrixReader(string path)
            : base(path)
        {         
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="Stream">文件路径</param>
        public BinaryDiagonalMatrixReader(Stream Stream)
            : base(Stream)
        {
        }
        /// <summary>
        /// 写方法。Version 1.
        /// 内容区：
        /// 第1个数为byte，表示矩阵文件的版本，对应数字 1-255
        /// 第2个数为byte，表示矩阵的类型，对应C#的枚举类型
        /// 第3、4个数为int32，分别表示 矩阵的行和列。
        /// 第5个开始为浮点数（双精度、单精度或整型）记录
        /// </summary>
        public override IMatrix Read(MatrixHeader header)
        {
            SkipHeader();

            int row = header.RowCount;
            int col = header.ColCount;
          
            double [] vector= new double[row]; 

            for (int i = 0; i < row; i++)
            {
                vector[i] = BinaryReader.ReadDouble(); 
            }
            Char end = this.BinaryReader.ReadChar();
            if (end != BinarySpliter.EndOfContent)
            {
                throw new Exception("末尾数字匹配上啊！！！");
            }
            DiagonalMatrix dm = new DiagonalMatrix(vector);
            return dm;
        } 
    }
}
