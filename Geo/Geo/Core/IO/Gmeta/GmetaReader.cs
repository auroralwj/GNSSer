//2015.09.26, czs, create in xi'an hongqing, API开始了! 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using  Geo.Common;

using Geo.IO;

namespace Geo.IO
{ 

    /// <summary>
    /// 配置文件内容。
    /// 通常为一行一个变量和值。采用分隔符分开。
    /// </summary>
    public class GmetaReader : AbstractReader<Gmetadata>
    {
        /// <summary>
        ///配置文件内容
        /// </summary> 
        /// <param name="path">路径</param> 
        /// <param name="ValueSplitter">数值分隔符</param> 
        /// <param name="CommentSplitter">注释分隔符</param> 
        public GmetaReader(string path, string ValueSplitter = "=", string CommentSplitter = "#") : this(new FileStream(path, FileMode.Open), Encoding.Default, ValueSplitter, CommentSplitter) { }

        /// <summary>
        ///配置文件内容
        /// </summary> 
        /// <param name="stream">数据流</param>
        /// <param name="encoding">编码</param>
        /// <param name="ValueSplitter">数值分隔符</param> 
        /// <param name="CommentSplitter">注释分隔符</param> 
        public GmetaReader(FileStream stream, Encoding encoding, string ValueSplitter = "=", string CommentSplitter = "#")
            : base(stream, encoding)
        {
            reader = new ConfigReader(stream, encoding, ValueSplitter, CommentSplitter);
        }
        ConfigReader reader;
        public override Gmetadata Read()
        {
            var data = reader.Read();
            return new Gmetadata(data);
        }
    }
}
