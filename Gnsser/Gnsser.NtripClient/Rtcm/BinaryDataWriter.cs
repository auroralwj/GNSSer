//2015.11.05, czs, create in hongqing 招待所, 二进制写入器

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gnsser.Ntrip.Rtcm;
using Gnsser.Data.Rinex;
using System.IO;
using Geo.Times; 

namespace Gnsser.Ntrip
{ 
    /// <summary>
    /// 二进制写入器
    /// </summary>
    public class BinaryDataWriter
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        public BinaryDataWriter(string filePath)
            : this(new FileStream(filePath, FileMode.Create))
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stream"></param>
        public BinaryDataWriter(Stream stream)
        {
            this.Writer = new BinaryWriter(stream, Encoding.ASCII);
        }

        /// <summary>
        /// 数据流写入器
        /// </summary>
        public BinaryWriter Writer { get; set; }

        public void Write(byte[] bytes)
        {
            Writer.Write(bytes);
        }

        public void Close()
        {
            Flush();
            this.Writer.Close();
        }

        public void Flush()
        {
            this.Writer.Flush();
        }
    }
}
