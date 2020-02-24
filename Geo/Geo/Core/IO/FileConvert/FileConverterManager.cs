//2015.11.16, czs, create in hongqing, 文件解压管理器

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO; 
using Geo.Utils;

namespace Geo.IO
{
    /// <summary>
    /// 文件解压管理器，关键字为输入类型
    /// </summary>
    public class FileConverterManager : BaseDictionary<string, FileConverter>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FileConverterManager()
            : base("文件解压管理器")
        {
            this.FromTos = new Dictionary<string, List<string>>();
        }

        /// <summary>
        /// 具有的转换映射。一种树形的转换关系，全部存放与此。
        /// </summary>
        protected Dictionary<string, List<string>> FromTos { get; set; }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="FileConverter"></param>
        public void Regist(FileConverter FileConverter)
        {
            if (!FromTos.ContainsKey(FileConverter.SourceType)) { FromTos[FileConverter.SourceType] = new List<string>(); }
            if (!FromTos[FileConverter.SourceType].Contains(FileConverter.DestType))
            {
                FromTos[FileConverter.SourceType].Add(FileConverter.DestType);
            }
            this[FileConverter.SourceType] = FileConverter;
        }

        /// <summary>
        /// 提供一个more的管理器。
        /// </summary>
        public static FileDecompressManager Default
        {
            get
            {
                FileDecompressManager FileDecompressManager = new FileDecompressManager();
                //FileDecompressManager.Regist(new ZFileConverter());
                //FileDecompressManager.Regist(new ZipFileConverter());

                return FileDecompressManager;
            }
        }

    }

 
}