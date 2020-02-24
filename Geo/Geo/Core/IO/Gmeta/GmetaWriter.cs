//2015.09.26, czs, create in xi'an hongqing, API开始了! 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Geo.IO
{
    /// <summary>
    /// 写文件
    /// </summary>
    public class GmetaWriter : AbstractWriter<Gmetadata>
    { /// <summary>
        ///配置文件内容
        /// </summary> 
        /// <param name="path">路径</param>  
        public GmetaWriter(string path) : this(new FileStream(path, FileMode.Create), Encoding.UTF8) { }

        /// <summary>
        ///配置文件内容
        /// </summary> 
        /// <param name="stream">数据流</param>
        /// <param name="encoding">编码</param> 
        public GmetaWriter(Stream stream, Encoding encoding)
            : base(stream, encoding)
        {
        }

        /// <summary>
        /// 还没有实现
        /// </summary>
        /// <param name="data"></param>
        public override  void Write(Gmetadata data)
        {
            throw new NotImplementedException();

        }


    }
}
