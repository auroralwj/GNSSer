//2015.01.18, czs, create in namu,  配置文件内容读取器
//2018.03.19, czs, edti in hmx, 与分组配置文件合并，提取抽象类

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Geo.Common;

namespace Geo.IO
{
    /// <summary>
    /// 配置文件内容。
    /// 通常为一行一个变量和值。采用分隔符分开。
    /// </summary>
    public abstract  class ObjectConfigReader<TTable, TItem, TKey> : TypedConfigReader<TTable, TItem, TKey, Object>
        where TTable : TypedConfig<TItem, TKey, Object>// ObjectConfig<TKey>
        where TItem : ObjectConfigItem<TKey>
        //  where TKey : IComparable<TKey>

        // AbstractReader<Config>
    {
        /// <summary>
        ///配置文件内容
        /// </summary> 
        /// <param name="path">路径</param> 
        /// <param name="ValueSplitter">数值分隔符</param> 
        /// <param name="CommentSplitter">注释分隔符</param> 
        public ObjectConfigReader(string path, string ValueSplitter = "=", string CommentSplitter = "#") : this(new FileStream(path, FileMode.Open), Encoding.UTF8, ValueSplitter, CommentSplitter) { }

        /// <summary>
        ///配置文件内容
        /// </summary> 
        /// <param name="stream">数据流</param>
        /// <param name="encoding">编码</param>
        /// <param name="ValueSplitter">数值分隔符</param> 
        /// <param name="CommentSplitter">注释分隔符</param> 
        public ObjectConfigReader(Stream stream, Encoding encoding, string ValueSplitter = "=", string CommentSplitter = "#")
            : base(stream, encoding, ValueSplitter, CommentSplitter)
        { 
        }
          
         
    }
}
