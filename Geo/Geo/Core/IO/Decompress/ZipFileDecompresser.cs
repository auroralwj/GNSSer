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
using Geo.Utils;
using ICSharpCode.SharpZipLib.Zip;

namespace Geo.IO
{

    /// <summary>
    /// 文件转换
    /// </summary>
    public class ZipFileDecompresser : FileDecompresser
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ZipFileDecompresser()
        {
            SourceType = "Zip";
        }
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public override List<string> DoDecompress(string sourceFile, string dest)
        {
            var result =  Decompress(sourceFile, DestDirectory);
            return result;

            if (false)
            {
                System.IO.Compression.ZipFile.ExtractToDirectory(sourceFile, DestDirectory);
            }

            if (false)
            {
                ICSharpCode.SharpZipLib.Zip.FastZip fastZip = new ICSharpCode.SharpZipLib.Zip.FastZip();
                if (!String.IsNullOrEmpty(Password)) fastZip.Password = Password;
                fastZip.ExtractZip(sourceFile, DestDirectory, "");
            }
        }
        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="sourceFile">源文件</param>
        /// <param name="targetDirectory">目标路经</param>
        public List<string> Decompress(string sourceFile, string targetDirectory)
        {
            List<string> pathes = new List<string>();
            if (!File.Exists(sourceFile))
            {
                throw new FileNotFoundException(string.Format("未能找到文件 '{0}' ", sourceFile));
            }
            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }
            using (ZipInputStream s = new ZipInputStream(File.OpenRead(sourceFile)))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directorName = Path.Combine(targetDirectory, Path.GetDirectoryName(theEntry.Name));
                    string fileName = Path.Combine(directorName, Path.GetFileName(theEntry.Name));
                    // 创建目录
                    Geo.Utils.FileUtil.CheckOrCreateDirectory(directorName); 

                    if (! String.IsNullOrEmpty( fileName))
                    {
                        pathes.Add(fileName);
                        using (FileStream streamWriter = File.Create(fileName))
                        {
                            int size = 4096;
                            byte[] data = new byte[4 * 1024];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else break;
                            }
                        }

                    }
                }
            }
            return pathes;
        }

    }
}