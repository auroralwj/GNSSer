//2014.10.28, czs, create in numu, 矩阵的写

using System;
using System.IO;
using System.Collections.Generic;
using Geo.Utils;

namespace Geo.Algorithm
{
    /// <summary>
    /// 二维矩阵原生态写。
    /// 可选是否采用单精度翅膀压缩方法。
    /// </summary>
    public class BinaryArrayMatrixWriter : BaseBinaryMatrixWriter
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="path">文件路径</param>
        public BinaryArrayMatrixWriter(string path, bool IsFloatWingCompress = false)
            : base(path)
        {
            this.IsFloatWingCompress = IsFloatWingCompress;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="Stream">文件路径</param>
        /// <param name="IsFloatWingCompress">文件路径</param>
        public BinaryArrayMatrixWriter(Stream Stream, bool IsFloatWingCompress = false)
            : base(Stream)
        {
            this.IsFloatWingCompress = IsFloatWingCompress;
        }
        /// <summary>
        /// 是否采用两翼Float压缩法。
        /// </summary>
        public bool IsFloatWingCompress { get; set; }
        /// <summary>
        /// 写方法。Version 1.
        /// 内容区：
        /// 第1个数为byte，表示矩阵文件的版本，对应数字 1-255
        /// 第2个数为byte，表示矩阵的类型，对应C#的枚举类型
        /// 第3、4个数为int32，分别表示 矩阵的行和列。
        /// 第5个开始为浮点数（双精度、单精度或整型）记录
        /// </summary>
        /// <param name="matrix">待写成文件的矩阵。</param>
        public override void Write(IMatrix matrix)
        {
            MatrixHeader header ;
            if (this.IsFloatWingCompress)
            {
                header = MatrixHeader.GetDefaultHeader(matrix);
                header.MatrixType = MatrixType.FloatWing;
            }
            else header = MatrixHeader.GetDefaultHeader(matrix);
           
            WriteHeader(header);

            WriteMatrix(matrix);
        }

        protected override void WriteMatrix(IMatrix matrix)
        {
            BinaryWriter.Write(BinarySpliter.StartOfContent);          

            int row = matrix.RowCount;
            int col = matrix.ColCount; 
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if ( i != j && IsFloatWingCompress) BinaryWriter.Write((float)matrix[i, j]);
                    else  BinaryWriter.Write(matrix[i,j]);
                }
            }

            BinaryWriter.Write(BinarySpliter.EndOfContent); 
        }
    }
}
