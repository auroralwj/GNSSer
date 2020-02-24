//2014.10.28, czs, create in numu, 矩阵头文件

using System;
using System.IO;
using System.Collections.Generic;
using Geo.Utils;
using Geo.IO;

namespace Geo.Algorithm
{
    /// <summary>
    /// 矩阵头文件。
    /// </summary>
    public class BinaryMatrixHeaderWriter : AbstractBinaryMatrixWriter<MatrixHeader>
    {
        /// <summary>
        /// 构造函数。
        /// </summary> 
        public BinaryMatrixHeaderWriter(string path)
            : base(path)
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary> 
        public BinaryMatrixHeaderWriter(Stream Stream)
            : base(Stream)
        {
        }

        public override void Write(MatrixHeader header)
        {
            //写一个开始字符
            BinaryWriter.Write(BinarySpliter.StartOfHeader);  //起始符号
            BinaryWriter.Write((byte)1);                      //版本
            BinaryWriter.Write((byte)header.MatrixType);      //矩阵类型  
            BinaryWriter.Write(header.RowCount);              //行数
            BinaryWriter.Write(header.ColCount);              //列数
            BinaryWriter.Write(header.ContentCount);          //内容元素数量
            Char[] char8 = StringUtil.GetFixedLength(header.Name, 8).ToCharArray();
            BinaryWriter.Write(char8,0,8); //矩阵名称 
            BinaryWriter.Write(header.CreationTime.Ticks);    //创建时间 
            BinaryWriter.Write(StringUtil.GetFixedLength(header.Creator, 8).ToCharArray(), 0, 8);      //创建者 

            BinaryWriter.Write(BinarySpliter.StartOfRowParams);  //起始符号
            int i = 0;
            foreach (var item in header.RowNames)             //行名称
            {
                if (i != 0) { BinaryWriter.Write(','); }

                Char[] array =  item.ToCharArray();
                BinaryWriter.Write(array, 0, array.Length);
                i++;
            }
            BinaryWriter.Write(BinarySpliter.EndOfRowParams);

            i = 0;
            BinaryWriter.Write(BinarySpliter.StartOfColParams);  //起始符号
            foreach (var item in header.ColNames)             //列名称
            {
                if (i != 0) { BinaryWriter.Write(','); }

                Char[] array = item.ToCharArray();
                BinaryWriter.Write(array, 0, array.Length);
                i++;
            }

            BinaryWriter.Write(BinarySpliter.EndOfColParams);


            foreach (var item in header.Comments)             //注释
            {
                Char[] array = item.ToCharArray();
                BinaryWriter.Write(array,0, array.Length);
            }
            BinaryWriter.Write(BinarySpliter.EndOfHeader);
        }
    }
}
