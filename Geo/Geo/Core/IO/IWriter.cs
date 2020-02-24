//2014.10.29, czs, create in numu, 包含一个Stream属性，用于输入输出。

using System; 

namespace Geo.IO
{
    /// <summary>
    /// 包含Stream, Encoding属性，用于输入输出。
    /// </summary>
    public interface IWriter<TProduct> : IEncodedStreamer
    {
        /// <summary>
        /// 写入一个到数据流。
        /// </summary>
        /// <returns></returns>
       void Write(TProduct product);
    }
}
