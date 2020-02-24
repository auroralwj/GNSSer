//2014.10.29, czs, create in numu, 包含一个Stream属性，用于输入输出。

using System; 

namespace Geo.IO
{
    /// <summary>
    /// 包含Stream, Encoding属性，用于输入输出。
    /// </summary>
    public interface IEncodedStreamer: IStreamer
    {
        /// <summary>
        /// 数据流。
        /// </summary>
        global::System.Text.Encoding Encoding { get; set; }
    }
}
