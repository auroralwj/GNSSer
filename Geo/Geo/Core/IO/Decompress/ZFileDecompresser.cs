//2015.11.14, czs, create in hongqing, 文件转换

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
//
using Geo.Utils;

namespace Geo.IO
{
    /// <summary>
    /// 文件转换
    /// </summary>
    public class ZFileDecompresser : FileDecompresser
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ZFileDecompresser()
        {
            SourceType = "Z";
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="path"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public override List<string> DoDecompress(string path, string dest)
        {            
            try
            {
                List<string> result = new List<string>();
                if (!File.Exists(path))
                {
                    log.Error(path + "文件不存在！");
                    return result;
                }

                if (!IsOverwrite && File.Exists(dest))
                {
                    log.Info("已经存在 " + dest + "，不必解压。");
                    result.Add(dest);
                    return result;
                }

                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
               //   System.IO.Compression.GZipStream input = new System.IO.Compression.GZipStream(fs, CompressionMode.Decompress);
               ICSharpCode.SharpZipLib.LZW.LzwInputStream input = new ICSharpCode.SharpZipLib.LZW.LzwInputStream(fs);

                FileStream output = new FileStream(dest, FileMode.Create);
                int count = 0, size = 1024;
                byte[] buffer = new byte[size];
                while ((count = input.Read(buffer, 0, size)) > 0)
                {
                    output.Write(buffer, 0, count); output.Flush();
                }
                output.Close();
                output.Dispose();
                input.Close();
                input.Dispose();

                result.Add(dest);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new List<string>();
            } 
        }
    }

}