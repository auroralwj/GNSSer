//2015.11.14, czs, create in hongqing, 文件转换

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
    public abstract class FileDecompresser
    {
        protected Geo.IO.Log log = new Log(typeof(FileDecompresser));
        public FileDecompresser()
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
        /// 解压缩密码，可选
        /// </summary>
        public string Password { get; protected set; }
        /// <summary>
        /// 目标文件夹
        /// </summary>
        public string DestDirectory { get;  set; }
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
        /// <param name="sourceFolderOrFile"></param> 
        /// <returns></returns>
        public virtual List<string> Decompress(string sourceFolderOrFile)
        {
            List<string> pathes = new List<string>();
            Geo.Utils.FileUtil.CheckOrCreateDirectory(DestDirectory);

            if (Geo.Utils.FileUtil.IsDirectory(sourceFolderOrFile))
            {
                var files = Directory.GetFiles(sourceFolderOrFile, "*." + SourceType);
                foreach (var item in files)
                {
                    if (IsCancel) break;
                    var dest = DoDecompressOrDeleteSource(item);
                    if (String.IsNullOrWhiteSpace(dest)) continue;
                    pathes.Add( dest );
                }
            }
            else
            {
                var dest = DoDecompressOrDeleteSource(sourceFolderOrFile);
                if (String.IsNullOrWhiteSpace(dest)) return pathes;
                
                pathes.Add(dest);
            }
            return pathes;
        }
        LocalFileWritingStateManager LocalFileWritingStateManager { get { return LocalFileWritingStateManager.Instance; } }
        /// <summary>
        /// 返回解压后的路径
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <returns></returns>
        protected virtual string DoDecompressOrDeleteSource(string sourceFilePath)
        {
            string destFilePath = BuildDestFilePath(sourceFilePath);
            
            if (File.Exists(destFilePath))
            {
                if (IsOverwrite) { File.Delete(destFilePath); log.Info("删除了 " + destFilePath); }
                else return destFilePath;
            }

            log.Info("即将解压 " + sourceFilePath + " 到 " + destFilePath);

            if (LocalFileWritingStateManager.Contains(sourceFilePath))
            {
                log.Error("文件正在下载或写入中，不可解压！");
                return null;
            }

            if (!LocalFileWritingStateManager.Regist(destFilePath))
            {
                log.Error("解压取消，" + sourceFilePath);
                return null;
            }
            try
            {
                var destFilePathes = DoDecompress(sourceFilePath, destFilePath);
                if (destFilePathes.Count > 0)
                {
                    destFilePath = destFilePathes[0];
                }
            }
            catch (Exception ex)
            {
                log.Error("解压出错, " + ex.Message + ", " + sourceFilePath);
            }
            finally
            {
                LocalFileWritingStateManager.Unregist(destFilePath);
            }

            if (IsDeleteSource)
            {
                File.Delete(sourceFilePath); log.Info("删除了 " + sourceFilePath);
            }
            return destFilePath;
        }

        /// <summary>
        /// 构建目标路径。
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <returns></returns>
        protected virtual string BuildDestFilePath(string sourceFilePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(sourceFilePath);
            if (!String.IsNullOrEmpty(DestType))//如果指名了目标类型。
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

            string destFilePath = Path.Combine(DestDirectory, fileName);
            return destFilePath;
        }
        /// <summary>
        /// 执行单文件解压
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="dest">目标路径</param>
        public abstract List<string> DoDecompress(string sourceFilePath, string dest);
    }
}