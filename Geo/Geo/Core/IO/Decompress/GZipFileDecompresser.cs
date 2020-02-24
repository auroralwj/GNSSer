//2018.04.26, czs, create in hmx, GZIP文件转换

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using Geo.Utils;
using ICSharpCode.SharpZipLib.GZip;

namespace Geo.IO
{

    /// <summary>
    /// GZIP 文件转换
    /// </summary>
    public class GZipFileDecompresser : FileDecompresser
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GZipFileDecompresser()
        {
            SourceType = "gz";
        }
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="unzipfilename"></param>
        /// <returns></returns>
        public override List<string> DoDecompress(string sourceFile, string unzipfilename)
        {
            //System.IO.Compression.ZipFile.ExtractToDirectory(sourceFile, DestDirectory);

            //自解压具有后缀 
             unzipfilename = Path.GetFileNameWithoutExtension(sourceFile);
             
            string destPath = Path.Combine(DestDirectory, unzipfilename);
            //创建压缩文件的输入流实例
            using (GZipInputStream zipFile = new GZipInputStream(File.OpenRead(sourceFile)))
            {
                //创建目标文件的流
                using (FileStream destFile = File.Open(destPath, FileMode.Create))
                {
                    int buffersize = 2048;//缓冲区的尺寸，一般是2048的倍数
                    byte[] FileData = new byte[buffersize];//创建缓冲数据
                    while (buffersize > 0)//一直读取到文件末尾
                    {
                        buffersize = zipFile.Read(FileData, 0, buffersize);//读取压缩文件数据
                        destFile.Write(FileData, 0, buffersize);//写入目标文件
                    }
                }
            }
            return new List<string>() { unzipfilename };

            //ICSharpCode.SharpZipLib.Zip.FastZip fastZip = new ICSharpCode.SharpZipLib.Zip.FastZip();
            //if (!String.IsNullOrEmpty(Password)) fastZip.Password = Password;
            //fastZip.ExtractZip(sourceFile, DestDirectory, "");
        }
         


    }
}