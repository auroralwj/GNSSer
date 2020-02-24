//2014.10.28, czs, create in numu, 矩阵头文件

using System;
using System.IO;
using System.Collections.Generic;
using Geo.Utils;
using Geo.IO;
using System.Text;

namespace Geo.Algorithm
{
    /// <summary>
    /// 矩阵头文件。
    /// </summary>
    public class BinaryMatrixHeaderReader : AbstractBinaryMatrixReader<MatrixHeader>
    {
        /// <summary>
        /// 构造函数。
        /// </summary> 
        public BinaryMatrixHeaderReader(string path)
            : base(path)
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary> 
        public BinaryMatrixHeaderReader(Stream Stream)
            : base(Stream)
        {
        }

        public override MatrixHeader Read()
        {
            //读取第一个字符 
            Char ch = BinaryReader.ReadChar();
            if (ch != BinarySpliter.StartOfHeader)
            {
                throw new Exception("第一个字符不正确。");
            }
            byte version = BinaryReader.ReadByte();
            MatrixType type = (MatrixType)BinaryReader.ReadByte();
            
            MatrixHeader header = new MatrixHeader(type)
            {
                Version = version
            };
            header.RowCount = BinaryReader.ReadInt32(); //行数
            header.ColCount = BinaryReader.ReadInt32(); //列数
            header.ContentCount = BinaryReader.ReadInt32(); //内容元素数量
            header.Name = new String(BinaryReader.ReadChars(8)).Trim();

            DateTime time = new DateTime(BinaryReader.ReadInt64());
            string creator = new String(BinaryReader.ReadChars(8)).Trim();
            header.Creator = creator;

            //
            while ((ch = BinaryReader.ReadChar()) == BinarySpliter.StartOfRowParams)
            {
                break;
            }
            StringBuilder sb = new StringBuilder();
            while ((ch = BinaryReader.ReadChar()) != BinarySpliter.EndOfRowParams)
            {
                if (!Char.IsControl(ch))
                    sb.Append(ch);
            }
            header.RowNames = new List<string>(sb.ToString().Split(','));

            sb = new StringBuilder();
            while ((ch = BinaryReader.ReadChar()) == BinarySpliter.StartOfColParams)
            {
                break;
            }
            while ((ch = BinaryReader.ReadChar()) != BinarySpliter.EndOfColParams)
            {
                if (!Char.IsControl(ch))
                    sb.Append(ch);
            }
            header.ColNames = new List<string>(sb.ToString().Split(','));

            sb = new StringBuilder();
            while ((ch = BinaryReader.ReadChar()) != BinarySpliter.EndOfHeader)
            {
                if(!Char.IsControl(ch))
                sb.Append(ch);
            } 
            header.Comments.Add(sb.ToString());
            return header;
        }
    }
}
