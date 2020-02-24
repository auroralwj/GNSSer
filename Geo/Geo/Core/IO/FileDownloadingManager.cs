//2016.11.23, czs & cuiyang, create in hongqing, 下载，写入文件管理控制器

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
    /// 文件下载状态
    /// </summary>
    public class FileDownloadingState
    {
        public FileDownloadingState(string url)
        {
            IsDownloading = false;
            TriedCount = 0;
            Url = url;
        }
        /// <summary>
        /// 更新状态信息
        /// </summary>
        /// <param name="LocalFiles"></param>
        public void UpdateState(List<string> LocalFiles)
        {
            this.LocalFiles = LocalFiles;
            TriedCount++;
        }
        /// <summary>
        /// 网址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 是否正在下载
        /// </summary>
        public bool IsDownloading { get; set; }
        /// <summary>
        /// 是否下载失败
        /// </summary>
        public bool IsFailed { get { return LocalFiles == null || LocalFiles.Count == 0; } }
        /// <summary>
        /// 尝试重下次数
        /// </summary>
        public int TriedCount { get; set; }
        /// <summary>
        /// 下载后的本地路径
        /// </summary>
        public List<string> LocalFiles { get; set; }
        /// <summary>
        /// 等待下载完毕。
        /// </summary>
        public void WaitDownloading()
        {
            while (IsDownloading)
            {
                System.Threading.Thread.Sleep(500); 
            }
        }
        /// <summary>
        /// 是否需要重下
        /// </summary>
        /// <param name="maxTryCount"></param>
        /// <returns></returns>
        public bool IsNeedToReDownload(int maxTryCount = 5)
        {
            if (IsFailed && TriedCount < maxTryCount)
            {
                return true;
            }
            return false;
        }
    }
    /// <summary>
    /// 文件下载管理器。
    /// </summary>
    public class FileDownloadingManager : BaseConcurrentDictionary<string, FileDownloadingState>
    {
        private static FileDownloadingManager instance = new FileDownloadingManager();
        /// <summary>
        /// 单例模式
        /// </summary>
        static public FileDownloadingManager Instance { get { return instance; } }
        /// <summary>
        /// 文件写入管理器
        /// </summary>
        LocaFileWritingManager LocaFileWritingManager = LocaFileWritingManager.Instance;
        /// <summary>
        /// 最大尝试次数
        /// </summary>
        public int MaxTryCount = 5;
        static object locker = new object();
        /// <summary>
        /// 下载文件或目录支持 ftp 目录。如果返回为0个，则表示失败。
        /// </summary>
        /// <param name="url"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public List<string> DownloadFileOrDirectory(string url, string extension = "*.*", string localFolder = @"D:\Temp")
        {
            if (!this.Contains(url))
            {
                var DownloadState = new FileDownloadingState(url);
                this.Add(url, DownloadState);
            }

            var state = this[url];
            state.WaitDownloading();

            if (state.TriedCount == 0)
            {
                return DoDownload(state, extension, localFolder); 
            }

            if (state.IsNeedToReDownload(MaxTryCount))
            {
                return DoDownload(state, extension, localFolder);
            }
            return state.LocalFiles;
        }
        #region 实现细节
        /// <summary>
        /// 直接下载,并更新状态。
        /// </summary>
        /// <param name="DownloadState"></param>
        /// <param name="extension"></param>
        /// <param name="localFolder"></param>
        /// <returns></returns>
        private List<string> DoDownload(FileDownloadingState DownloadState, string extension = "*.*", string localFolder = @"D:\Temp")
        {
            DownloadState.WaitDownloading();
            lock (locker)
            {
                DownloadState.IsDownloading = true; 
            }
           // DownloadState.WaitDownloading();

            var oks = DownloadFtpDirecotryOrFile(DownloadState.Url, extension, localFolder);
            DownloadState.IsDownloading = false;

            DownloadState.UpdateState(oks);
            return oks;
        }

        /// <summary>
        /// 下载目录或文件，返回本地路径。如果重复下载，则直接返回。
        /// </summary>
        /// <param name="ftpFolderOrFilePath">路径，含IP地址和端口，若是目录，请以"/"结尾</param>
        /// <param name="extension">若是目录，则设置，可以以分号分隔多个匹配类型</param>
        /// <param name="isDownAll">是否下载所有的文件，如果不是，则成成功一个后就停止下载。</param>
        /// <param name="localFolder"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="throwException"></param>
        /// <returns></returns>
        protected List<string> DownloadFtpDirecotryOrFile(string ftpFolderOrFilePath, string extension = "*.*", string localFolder = @"C:\GnsserTemp\",  string userName = "Anonymous", string password = "User@", bool IsOverwrite = false, bool throwException = false)
        {
            List<string> localFilePathes = new List<string>();
            List<string> fileUrlPathes = NetUtil.GetFtpFileUrls(ftpFolderOrFilePath, extension, userName, password);

            foreach (var url in fileUrlPathes)
            {
                var localPath = Path.Combine(localFolder, Path.GetFileName(url));
                log.Info("正在尝试下载 " + url);

                TryDownloadOne(localFilePathes, url, localPath, userName, password, IsOverwrite, throwException);
            }
            return localFilePathes;
        }

        /// <summary>
        /// 尝试下载一个。
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="IsOverwrite"></param>
        /// <param name="throwException"></param>
        /// <param name="localFilePathes"></param>
        /// <param name="url"></param>
        /// <param name="localPath"></param>
        private bool TryDownloadOne(List<string> localFilePathes, string url, string localPath, string userName = "Anonymous", string password = "User@", bool IsOverwrite = false, bool throwException = false)
        {
            bool result = false;
            if (LocaFileWritingManager.Contains(localPath))
            {
                var state = LocaFileWritingManager.Get(localPath);
                state.WaitWrting(); 

                if (state.IsAvailable())
                {
                    log.Info( "文件 " + localPath+  " 可用" );
                    localFilePathes.Add(localPath);
                }
                else
                {
                    if (!state.IsRegisted(url))
                    {
                        log.Info(localPath + " 获得新来源 " + url);
                        state.Regist(url);
                    }

                    if (!state.IsWrited(url))
                    {
                        log.Info("尝试从 " + url + " 下载 " + localPath + "");
                        result = DoDownload(localFilePathes, url, state, userName, password, IsOverwrite, throwException);
                    }
                }
            }
            else
            {
                var state = new FileWritingState(localPath);
                state.Regist(url);

                log.Info("第一次下载  " + url + " 到 " + localPath + "");
                LocaFileWritingManager.Add(localPath, state);
                result = DoDownload(localFilePathes, url, state, userName, password, IsOverwrite, throwException);
            }
            return result;
        }
        static object downLocker = new object();

        /// <summary>
        /// 执行下载，并标记状态
        /// </summary>
        /// <param name="localFilePathes"></param>
        /// <param name="url"></param>
        /// <param name="state"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="IsOverwrite"></param>
        /// <param name="throwException"></param>
        private  bool DoDownload(List<string> localFilePathes, string url, FileWritingState state, string userName = "Anonymous", string password = "User@", bool IsOverwrite = false, bool throwException = false)
        {
           // state.IsWriting = true;
            lock (downLocker)
            {
                //内有log记录
                if (NetUtil.FtpDownload(url, state.LocalPath, userName, password, IsOverwrite, throwException))
                {
                    state.Update(url, true);
                    if (state.IsAvailable())
                    {
                        localFilePathes.Add(state.LocalPath);
                        return true;
                    }
                }
            }
           // state.IsWriting = false;
            return false;
        }
        #endregion
    }
}