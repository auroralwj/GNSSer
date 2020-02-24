//2015.04.14, czs, edit in namu, 尝试去掉原zip包

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Geo.Utils
{

    /// <summary>
    /// 压缩与解压缩。
    /// </summary>
    public class CompressUtil
    {


        public static void DecompressZ(string fileOrDirectory, string toDirectory, bool deleSourse = false, bool overwrite = true)
        {
            bool isDirectory = Directory.Exists(fileOrDirectory);
            if (isDirectory)
            {
                var files = Directory.GetFiles(fileOrDirectory, "*.Z");

                foreach (var item in files)
                {
                    DecompressLzw(item, toDirectory, deleSourse, overwrite);
                }
            }
            else if(File.Exists(fileOrDirectory))
            {
                DecompressLzw(fileOrDirectory, toDirectory, deleSourse, overwrite);
            }
            else
            {
                throw new FileNotFoundException("不存在文件或者目录" + fileOrDirectory);
            }
        }

        /// <summary>
        ///  解压缩 *.Z 文件,可指定是否删除原文件。
        /// </summary>
        /// <param name="compressedFilePath"></param>
        /// <param name="destDir"></param>
        public static void DecompressLzw(string compressedFilePath, string destDir, bool deleSourse = false, bool overwrite = true)
        {
            string dest = destDir + "\\" + Path.GetFileNameWithoutExtension(compressedFilePath);

             if (File.Exists(dest)){
                 if (overwrite) { File.Delete(dest); }
                 else return;
             }

             Utils.FileUtil.CheckOrCreateDirectory(destDir);  

            FileStream fs = new FileStream(compressedFilePath, FileMode.Open, FileAccess.Read);
            System.IO.Compression.GZipStream input = new  System.IO.Compression.GZipStream(fs,  System.IO.Compression.CompressionMode.Decompress);
            //ICSharpCode.SharpZipLib.LZW.LzwInputStream input = new ICSharpCode.SharpZipLib.LZW.LzwInputStream(fs);

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


        //    ZipUtil.DecompressFile(compressedFilePath, dest); 

            if (deleSourse) File.Delete(compressedFilePath); 
        }
        /// <summary>
        /// ZIP or Z
        /// </summary>
        /// <param name="compressedFilePath"></param>
        /// <param name="destDir"></param>
        /// <param name="pass"></param>
        public static void Decompres(string compressedFilePath, string destDir, string pass = null, bool delSourse = false)
        {
            if (Path.GetExtension(compressedFilePath).ToUpper() == ".Z")
            {
                CompressUtil.DecompressLzw(compressedFilePath, destDir, delSourse);
                if (delSourse) File.Delete(compressedFilePath);
            }
            else if (Path.GetExtension(compressedFilePath).ToUpper() == ".ZIP")
            {
                //var dest = Path.Combine(destDir, Path.GetFileName(compressedFilePath).ToLower().Replace(".zip", ""));
                //ZipUtil.DecompressFile(compressedFilePath, dest); 

                //ICSharpCode.SharpZipLib.Zip.FastZip fastZip = new ICSharpCode.SharpZipLib.Zip.FastZip();
                //if (pass != null) fastZip.Password = pass;
                //fastZip.ExtractZip(compressedFilePath, destDir, "");


                System.IO.Compression.ZipFile.ExtractToDirectory(compressedFilePath, destDir);

                if (delSourse) File.Delete(compressedFilePath);

            }
        }


    }
}
