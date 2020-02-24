//2015.11.16, czs, create in hongqing, 封装了RINEX D文件解压

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using Geo.Utils; 
using Geo.IO;
using System.IO;

namespace Geo.IO
{
    /// <summary>
    /// 封装了RINEX D文件解压
    /// </summary>
    public class RinexDFileDecompresser : FileDecompresser
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RinexDFileDecompresser(string exeDir )
        {
            SourceType = "D";
            //此处添加双引号用于支持空格
            string exePath = Path.Combine( exeDir , "crx2rnx.exe"); 
            Init( exePath);
        }

        public void Init(string ExePath)
        {
            this.ExePath = ExePath;
            de = new Geo.Utils.DecompressRinexer(ExePath);
        }
        Geo.Utils.DecompressRinexer de;
        /// <summary>
        /// 外部工具路径
        /// </summary>
        public string ExePath { get; set; }

                /// <summary>
        /// 构建目标路径。
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <returns></returns>
        protected override string BuildDestFilePath(string sourceFilePath)
        {
            var fileName = Path.GetFileName(sourceFilePath);
            fileName = fileName.TrimEnd('d', 'D') + "O";
            string destFilePath = Path.Combine(DestDirectory, fileName);
            return destFilePath;
        }
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public override List<string> DoDecompress(string sourceFile, string dest)
        {
      
            try
            {
              return  de.Decompress(sourceFile, DestDirectory, IsDeleteSource, IsOverwrite);
            }
            catch (Exception ex) { log.Error(ex.Message ); }

            return new List<string>();
        }
    }

}
