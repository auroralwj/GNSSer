//2015.11.16, czs, create in hongqing, 文件解压管理器
//2018.04.27, czs, edit in hmx, 增加gzip和crx文件解压的支持

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Utils;
using System.Text.RegularExpressions;

namespace Geo.IO
{
    /// <summary>
    /// 文件解压管理器，关键字为输入类型
    /// </summary>
    public class FileDecompressManager : BaseDictionary<string, FileDecompresser>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FileDecompressManager(string DestDirectory = "C:\\GnsserTemp\\")
            : base("文件解压管理器")
        {
            this.DestDirectory = DestDirectory;
            this.MaxDecompressCount = 5;
        }
        /// <summary>
        /// 目标文件夹
        /// </summary>
        public string DestDirectory { get; set; }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="FileDecompresser"></param>
        public void Regist(FileDecompresser FileDecompresser)
        {
            this[FileDecompresser.SourceType.ToUpper()] = FileDecompresser;
        }

        /// <summary>
        /// 最大解压数量，如果超出，则认为是死循环
        /// </summary>
        public int MaxDecompressCount { get; set; }

        /// <summary>
        /// 解压一次。若已经匹配则直接返回。
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="destType">后缀名</param> 
        /// <returns></returns>
        public List<string> Decompress(string inputPath, string destType)
        {
            return DecompressOnce(inputPath, destType);
        }


        /// <summary>
        /// 解压一次。若已经匹配则直接返回。
        /// </summary>
        /// <param name="inputPath">输入文件路径，必须有后缀名</param>
        /// <param name="destType">后缀名</param>
        /// <param name="loopCount"> 解压次数</param>
        /// <returns></returns>
        protected List<string> DecompressOnce(string inputPath, string destType, int loopCount = 0)
        {
            //只保留点号后的后缀名
            destType = destType.ToUpper();//必须大写

            List<string> mathedPathes = new List<string>();
            //如果已经匹配，则直接返回
            if (PathUtil.IsFileExtensionMatched(inputPath, destType))
            {
                mathedPathes.Add(inputPath);
                return mathedPathes;
            }

            if (loopCount >= MaxDecompressCount) { return mathedPathes; }

            //解压缩一次文件
            List<string> dePathes = new List<string>();
            var sourceType = Path.GetExtension(inputPath).TrimStart('.').ToUpper();
            foreach (var item in this.Keys)
            {
                if (Geo.Utils.PathUtil.IsFileExtensionMatched(inputPath, item))
                {
                    var decompresser = Get(item);

                    decompresser.DestDirectory = this.DestDirectory;
                    var results = decompresser.Decompress(inputPath);
                    dePathes.AddRange(results);
                    continue;
                }
            }

            if (dePathes.Count == 0)
            {
                var msg = "没有该类型的解压器！" + sourceType;
                log.Error(msg); 
                return mathedPathes;
                throw new Exception(msg);
            }


            foreach (var path in dePathes)
            {
                if (PathUtil.IsFileExtensionMatched(path, destType))
                {
                    mathedPathes.Add(path);
                }
                else//迭代，继续解压
                {
                    mathedPathes = DecompressOnce(path, destType, loopCount++);
                }
            }
            return mathedPathes;
        }

        /// <summary>
        /// 提供一个more的管理器。
        /// </summary>
        public static FileDecompressManager GetDefault(string tempFolder = @"C:\GnsserTemp\", string exeDir = @"Data\Exe\")
        {
            FileDecompressManager FileDecompressManager = new FileDecompressManager(tempFolder);
            FileDecompressManager.Regist(new GZipFileDecompresser());
            FileDecompressManager.Regist(new ZFileDecompresser());
            FileDecompressManager.Regist(new ZipFileDecompresser());

            FileDecompressManager.Regist(new RinexCrxFileDecompresser(exeDir));
            FileDecompressManager.Regist(new RinexDFileDecompresser(exeDir));

            return FileDecompressManager;
        }

    }


}