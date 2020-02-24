//2015.11.16, czs, create in hongqing, 文件转换

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
    /// 文件转换
    /// </summary>
    public abstract class FileConverter
    {
        public FileConverter()
        {
            DestDirectory = @"C:\GnsserTemp\";
        }

        #region 属性
        /// <summary>
        /// 是否取消计算过程。只有在批量计算中有用。
        /// </summary>
        public bool IsCancel { get; set; }

        /// <summary>
        /// 输入类型
        /// </summary>
        public string SourceType { get; protected set; }
        /// <summary>
        /// 目标类型
        /// </summary>
        public string DestType { get; protected set; }
        /// <summary>
        /// 压缩、解压缩密码，可选
        /// </summary>
        public string Password { get; protected set; }
        /// <summary>
        /// 目标文件夹
        /// </summary>
        public string DestDirectory { get; protected set; }
        /// <summary>
        /// 是否覆盖
        /// </summary>
        public bool IsOverwrite { get; protected set; }
        /// <summary>
        /// 是否删除源数据
        /// </summary>
        public bool IsDeleteSource { get; protected set; }
        #endregion

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="path"></param>
        /// <param name="destFolderOrFile"></param>
        /// <returns></returns>
        public virtual void Convert(string sourceFolderOrFile)
        {
            Geo.Utils.FileUtil.CheckOrCreateDirectory(DestDirectory);

            if (Geo.Utils.FileUtil.IsDirectory(sourceFolderOrFile))
            {
                var files = Directory.GetFiles(sourceFolderOrFile, "*." + SourceType);
                foreach (var item in files)
                {
                    if (IsCancel) break;

                    DoConvertOrDeleteSource(item);
                }
            }
            else
            {
                DoConvertOrDeleteSource(sourceFolderOrFile);
            }
        }

        /// <summary>
        /// 执行转换，
        /// </summary>
        /// <param name="sourceFilePath"></param>
        protected virtual void DoConvertOrDeleteSource(string sourceFilePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(sourceFilePath);
            if (!String.IsNullOrEmpty(DestType))
            {
                var ext = Path.GetExtension(fileName);
                if (!String.IsNullOrEmpty(ext))
                {
                    if (ext.TrimStart('.').ToLower() != DestType)
                    {
                        fileName += "." + DestType;
                    }
                }
                else
                {
                    fileName += "." + DestType;
                }
            }

            string dest = Path.Combine(DestDirectory, fileName);


            if (File.Exists(dest))
            {
                if (IsOverwrite) { File.Delete(dest); }
                else return;
            }

            DoConvert(sourceFilePath, dest);

            if (IsDeleteSource)
            {
                File.Delete(sourceFilePath);
            }
        }

        /// <summary>
        /// 执行单文件解压
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="destFilePath"></param>
        public abstract void DoConvert(string sourceFilePath, string destFilePath);
    }

}