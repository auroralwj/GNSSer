//2014.10.29, czs, create in numu, 包含一个Stream属性，用于输入输出。

using System;
using System.IO;
using System.Text;

namespace Geo.IO
{
    /// <summary>
    /// 包含Stream, Encoding属性，用于输入输出。
    /// </summary>
    public abstract class AbstractBinaryWriter<TProduct> : AbstractWriter<TProduct> 
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stream">数据流</param>
        public AbstractBinaryWriter(Stream stream, Encoding encoding)
            : base(stream, encoding)
        {
            this.BinaryWriter = new BinaryWriter(stream, this.Encoding);
        }
        /// <summary>
        /// 二进制写入器
        /// </summary>
        public BinaryWriter BinaryWriter { get; protected set; }
    }
}
