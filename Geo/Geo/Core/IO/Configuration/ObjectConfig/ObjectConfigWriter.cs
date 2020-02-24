//2015.01.19, czs, create in numu, 配置文件的写
//2018.03.19, czs, edti in hmx, 与分组配置文件合并

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Geo.IO
{
    /// <summary>
    /// 包含Stream, Encoding属性，用于输入输出。
    /// </summary>
    public class ObjectConfigWriter<TItem, TKey> : TypedConfigWriter<ObjectConfig<TItem, TKey>, TItem,  TKey, Object>//AbstractWriter<Config>
      where TItem :ObjectConfigItem<TKey>
       //   where TKey : IComparable<TKey>
    {
        public ObjectConfigWriter(string ValueSplitter = "=", string CommentSplitter = "#"):base(ValueSplitter, CommentSplitter)
        {

        }
        /// <summary>
        ///配置文件内容
        /// </summary> 
        /// <param name="path">路径</param> 
        /// <param name="ValueSplitter">数值分隔符</param> 
        /// <param name="CommentSplitter">注释分隔符</param> 
        public ObjectConfigWriter(string path, string ValueSplitter = "=", string CommentSplitter = "#") : this(new FileStream(path, FileMode.Create), Encoding.UTF8, ValueSplitter, CommentSplitter) { }

        /// <summary>
        ///配置文件内容
        /// </summary> 
        /// <param name="stream">数据流</param>
        /// <param name="encoding">编码</param>
        /// <param name="ValueSplitter">数值分隔符</param> 
        /// <param name="CommentSplitter">注释分隔符</param> 
        public ObjectConfigWriter(Stream stream, Encoding encoding, string ValueSplitter = "=", string CommentSplitter = "#")
            : base(stream, encoding, ValueSplitter, CommentSplitter)
        { 
        } 
    }
}
