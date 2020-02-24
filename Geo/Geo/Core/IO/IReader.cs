//2014.10.29, czs, create in numu, 包含一个Stream属性，用于输入输出。

using System; 

namespace Geo.IO
{
    /// <summary>
    /// 包含Stream, Encoding属性，用于输入输出。
    /// 适用于整存争取的小文件。
    /// </summary>
    public interface IReader<TProduct> : IEncodedStreamer
    {
        /// <summary>
        /// 读取一个。
        /// </summary>
        /// <returns></returns>
        TProduct Read();
    }
    /// <summary>
    /// 包含Stream, Encoding属性，用于输入输出。
    /// 适用于整存争取的小文件。
    /// </summary>
    public interface IReader<TProduct, TOption> : IEncodedStreamer
    {
        /// <summary>
        /// 读取一个。
        /// </summary>
        /// <returns></returns>
        TProduct Read(TOption option );
    }

}
