//2014.10.29, czs, create in numu, 包含一个Stream属性，用于输入输出。

using System;
using System.IO;
using System.Text;

namespace Geo.IO
{
    /// <summary>
    /// 包含Stream, Encoding属性，用于输入输出。
    /// </summary>
    public abstract class AbstractBinaryReader<TProduct>: AbstractReader<TProduct> 
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stream">数据流</param>
        public AbstractBinaryReader(Stream stream, Encoding encoding)
            : base(stream, encoding)
        {
            this.BinaryReader = new BinaryReader(stream, this.Encoding);
        }
        /// <summary>
        /// 读入器
        /// </summary>
        public BinaryReader BinaryReader { get; set; }
    }

    /// <summary>
    /// 包含Stream, Encoding属性，用于输入输出。
    /// </summary>
    public abstract class AbstractBinaryReader<TProduct, TOption> : AbstractReader<TProduct, TOption>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path">数据流</param>
        public AbstractBinaryReader(string path, Encoding encoding)
            : base(path, encoding)
        {
            this.BinaryReader = new BinaryReader( new FileStream(path, FileMode.Open), this.Encoding);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stream">数据流</param>
        public AbstractBinaryReader(Stream stream, Encoding encoding, string name="Stream")
            : base(stream, encoding, name)
        {
            this.BinaryReader = new BinaryReader(stream, this.Encoding);
        }
        /// <summary>
        /// 读入器
        /// </summary>
        public BinaryReader BinaryReader { get; set; }
    }
}
