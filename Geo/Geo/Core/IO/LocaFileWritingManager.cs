//2016.11.23, czs & cuiyang, create in hongqing, 写入文件管理控制器

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.IO;
using Geo;
using System.Text.RegularExpressions;
using Geo.Utils;

namespace Geo.IO
{
     
    /// <summary>
    /// 文件写状态
    /// </summary>
    public class FileWritingState
    {
        /// <summary>
        /// 构造函数，默认正在写。
        /// </summary>
        /// <param name="localPath"></param>
        public FileWritingState(string localPath)
        {
            this.Sources = new Dictionary<string, bool>();
            //IsWriting = true;
            LocalPath = localPath;
        }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string LocalPath { get; set; }
        /// <summary>
        /// 是否正在写
        /// </summary>
        public bool IsWriting { get; set; }
        /// <summary>
        /// 来源,并标记是否下过。
        /// </summary>
        public Dictionary<string, bool> Sources { get; set; }
        /// <summary>
        /// 等待写结束
        /// </summary>
        public void WaitWrting()
        {
            //while (IsWriting)
            //{
            //    System.Threading.Thread.Sleep(500);
            //}
        }
        /// <summary>
        /// 标记是否下载过。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="isDownloaded"></param>
        public void Update(string path, bool isDownloaded = false)
        {
            Sources[path] = isDownloaded;
        }
        /// <summary>
        /// 注册一个，若重复注册，则报错
        /// </summary>
        /// <param name="url"></param>
        internal void Regist(string url)
        {
            Sources.Add(url, false);
        }
        /// <summary>
        /// 指定来源是否已经写过。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public bool IsWrited(string source) { return Sources[source]; }

        /// <summary>
        /// 文件是否可用
        /// </summary>
        /// <returns></returns>
        public bool IsAvailable()
        {
            if (File.Exists(LocalPath))
            {
                FileInfo info = new FileInfo(LocalPath);
                if (info.Length == 0) {

                    Geo.Utils.FileUtil.TryDeleteFileOrDirectory(LocalPath);
                    
                    return false;
                }

                return true;
            }
            return false;
        }
        /// <summary>
        /// 是否已经注册
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        internal bool IsRegisted(string url)
        {
            return Sources.ContainsKey(url);
        }
    }
    /// <summary>
    /// 文件写管理器
    /// </summary>
    public class LocaFileWritingManager : BaseConcurrentDictionary<string, FileWritingState>
    {
        private static LocaFileWritingManager instance = new LocaFileWritingManager();
        /// <summary>
        /// 单例模式
        /// </summary>
        static public LocaFileWritingManager Instance { get { return instance; } } 
    }
}