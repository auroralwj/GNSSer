//2015.11.11, czs, create in  xi'an hongqing, 数据源管理器
//2018.04.27, czs, edit in hmx, 增加下载失败列表，让失败的暂时不再下载

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
    /// 数据源类型。
    /// </summary>
    public enum DataSourceType
    {
        /// <summary>
        /// 未知数据源，默认为本地数据源
        /// </summary>
        Unkown,
        /// <summary>
        /// http 网络
        /// </summary>
        Http,
        /// <summary>
        /// ftp 服务器
        /// </summary>
        Ftp,
        /// <summary>
        /// 本地目录
        /// </summary>
        LocalFolder,
        /// <summary>
        /// 本地文件
        /// </summary>
        LocalFile
    }

    public delegate void FileDownloadedEventHandler(string localFileName, string url);

    /// <summary>
    /// 数据文件管理器。
    /// </summary>
    public class InputFileManager 
    {
        //负责输入数据的准备，包括网络数据

        Geo.IO.Log log = new Log(typeof(InputFileManager));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="LocalTempDirectory"></param>
        /// <param name="FileDecompressManager"></param>
        public InputFileManager(FileDecompressManager FileDecompressManager, string LocalTempDirectory = @"Data\Temp\InputFiles\")
        {
            this.IsOverwrite = false;
            this.LocalTempDirectory = LocalTempDirectory;
            this.FileDecompressManager = FileDecompressManager;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="LocalTempDirectory"></param> 
        public InputFileManager(string LocalTempDirectory = null)
        {
            this.IsOverwrite = false;
            if (LocalTempDirectory == null)
            {
                LocalTempDirectory = Path.Combine(Setting.TempDirectory, "InputFiles");
            }
            this.LocalTempDirectory = LocalTempDirectory;
            var dDecompressExeDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Exe\");
            this.FileDecompressManager = FileDecompressManager.GetDefault(this.LocalTempDirectory, dDecompressExeDir);
        }

        public event FileDownloadedEventHandler FileDownloaded;
         
        /// <summary>
        /// 用于应对可能出现的解压缩情况。
        /// </summary>
        public FileDecompressManager FileDecompressManager { get; set; }

        /// <summary>
        /// 本地临时目录，将网络下载的数据保存于此。
        /// </summary>
        public string LocalTempDirectory { get; set; }
        /// <summary>
        /// 是否覆盖，若false，判断目标存在，且大小非0，或大于10bytes，则不覆盖。
        /// </summary>
        public bool IsOverwrite { get; set; }
        /// <summary>
        /// 是否立刻取消下载。
        /// </summary>
        public bool IsCancelDownloading { get; set; }

        /// <summary>
        /// 返回一个，匹配的第一个。如果没有，则返回null。
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public string GetLocalFilePath(string inputPath, string searchPattern = "*.*")
        {
            List<string> list = GetLocalFilePathes(inputPath, searchPattern);
            if (list.Count == 0) return null;
            return list[0];
        }

        /// <summary>
        /// 返回一个，匹配的第一个。如果没有，则返回null。
        /// 输入可为网络路径，如ftp地址。
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="destFileExtension"></param>
        /// <param name="sourceFileExtension"></param>
        /// <returns></returns>
        public string GetLocalFilePath(string inputPath, string destFileExtension = "*.*o", string sourceFileExtension = "*.*")
        {
            List<string> list = GetLocalFilePathes(inputPath, destFileExtension, sourceFileExtension);
            if (list.Count == 0) return null;
            return list[0];
        } 

        /// <summary>
        /// 批量获或下载文件，并返回其本地路径。
        /// </summary>
        /// <param name="inputPathes"></param>
        /// <param name="destFileExtension"></param>
        /// <param name="sourceFileExtension"></param>
        /// <returns></returns>
        public Dictionary<string, List<string>> GetLocalFilePathesInDic(string[] inputPathes, string destFileExtension = "*.*o", string sourceFileExtension = "*.*", bool isDownAll = true)
        {
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
             
            foreach (var inputPath in inputPathes)
            {
                if(dic.ContainsKey(inputPath))
                {
                    continue;
                }
                if (IsCancelDownloading) { break; }

                var list  = GetLocalFilePathes(inputPath, destFileExtension, sourceFileExtension);
                dic[inputPath] = list;
              
                //如果只需下载其一
                if (list.Count > 0 && !isDownAll) { log.Info("已经成功了下载一个，指定不再下载其它文件了。 "); return dic; }
            }
            return dic;
        }

        /// <summary>
        /// 批量获或下载文件，并返回其本地路径。
        /// </summary>
        /// <param name="inputPathes"></param>
        /// <param name="destFileExtension"></param>
        /// <param name="sourceFileExtension"></param>
        /// <param name="isDownAll"></param>
        /// <returns></returns>
        public List<string> GetLocalFilePathes(string[] inputPathes, string destFileExtension = "*.*o", string sourceFileExtension = "*.*", bool isDownAll = true)
        {
            List<string> pathes = new List<string>();
            foreach (var inputPath in inputPathes)
            {
                if (IsCancelDownloading) { break; }

                var ps = GetLocalFilePathes(inputPath, destFileExtension, sourceFileExtension);
                pathes.AddRange(ps);
                //如果只需下载其一
                if (pathes.Count > 0 && !isDownAll) { log.Info("已经成功了下载一个，指定不再下载其它文件了。 "); return pathes; }
            }
            return pathes;
        }
        /// <summary>
        /// 失败地址禁用器
        /// </summary>
        static TempObjectHoulder<string> FailedUrlTempHolder = new TempObjectHoulder<string>(TimeSpan.FromHours(2));

        /// <summary>
        /// 主要API，获取文件。如果是网络数据，则下载到本地后返回本地路径。
        /// 如果是本地目录，则返回匹配的所有文件路径，注意：目录只搜索本层。
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="searchPattern">匹配文件类型，仅对于目录(本地目录或ftp目录)有效。，可以以分号分隔多个匹配类型</param>
        /// <returns></returns>
        public List<string> GetLocalFilePathes(string inputPath, string searchPattern = "*.*")
        {
            List<string> pathes = new List<string>();
            if (!FailedUrlTempHolder.IsAvailable(inputPath))
            {
                var remaindTime = FailedUrlTempHolder.GetRemainTime(inputPath);
                log.Warn("指定网址获取失败过，剩余解禁时间 " + remaindTime + ", " + inputPath);
                return pathes;
            }



            bool isLoopSubFolderLocalOnly = true;


            var sourceType = Geo.Utils.PathUtil.GetDataSourceType(inputPath);
            if (sourceType == DataSourceType.LocalFile)
            {
                pathes.Add(inputPath);
                return pathes;
            }
            else if (sourceType == DataSourceType.LocalFolder)
            {
                pathes.AddRange(Geo.Utils.FileUtil.GetFiles(inputPath, searchPattern, isLoopSubFolderLocalOnly));
                return pathes;
            }
            else if (sourceType == DataSourceType.Http)
            {
                try
                {
                    if (!Setting.IsNetEnabled) { log.Warn("网络不可用或没有启用，无法下载！"); return pathes; }

                    var locaPath = Path.Combine(LocalTempDirectory, Path.GetFileName(inputPath));
                    if(!Geo.Utils.NetUtil.Download(inputPath, locaPath))
                    {
                        FailedUrlTempHolder.Regist(inputPath);
                    }
                    pathes.Add(locaPath);
                    return pathes;
                }
                catch (Exception ex)
                {
                    FailedUrlTempHolder.Regist(inputPath);
                    log.Error(inputPath + " " + ex.Message);
                }
            }
            else if (sourceType == DataSourceType.Ftp)
            {
                if (!Setting.IsNetEnabled) { log.Warn("网络不可用或没有启用，无法下载！"); return pathes; }

                try
                {
                    var localPathes = DownloadFtpDirecotryOrFile(inputPath, searchPattern, LocalTempDirectory);

                    if (localPathes == null || localPathes.Count == 0)
                    {
                        FailedUrlTempHolder.Regist(inputPath);
                    }
                    pathes.AddRange(localPathes);
                    return pathes;
                }
                catch (Exception ex)
                {
                    FailedUrlTempHolder.Regist(inputPath);
                    log.Error(inputPath + " "+ex.Message);
                }
            }

            return pathes;
        }

        static object locker = new object();
        /// <summary>
        /// 当前正在下载的文件。避免重复下载。
        /// </summary>
        static BaseConcurrentDictionary<string, List<string>> CurrentDownloadings = new BaseConcurrentDictionary<string, List<string>>();

        FileDownloadingManager FileDownloadingManager = FileDownloadingManager.Instance;
        /// <summary>
        /// 下载，返回本地路径。如果重复下载，则直接返回。
        /// </summary>
        /// <param name="ftpFolderOrFilePath">路径，含IP地址和端口，如果需要账号请写成ftp://user:pass@Url的形式，若是目录，请以"/"结尾</param>
        /// <param name="extension">若是目录，则设置，可以以分号分隔多个匹配类型</param>
        /// <param name="localFolder"></param> 
        /// <returns></returns>
        public List<string> DownloadFtpDirecotryOrFile(string ftpFolderOrFilePath, string extension = "*.*", string localFolder = @"C:\GnsserTemp\")
        {
            return FileDownloadingManager.DownloadFileOrDirectory(ftpFolderOrFilePath, extension, localFolder); 
        }

        /// <summary>
        /// 主要API，获取文件。如果是网络数据，则下载到本地后返回本地路径。
        /// 如果是本地目录，则返回匹配的所有文件路径，注意：目录只搜索本层。
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="sourceFileExtension">如[.*z]，输入匹配文件类型，仅对于目录(本地目录或ftp目录)有效。，可以以分号分隔多个匹配类型</param>
        /// <param name="destFileExtension">如[.*o]，输出匹配文件类型，仅对于目录(本地目录或ftp目录)有效。，可以以分号分隔多个匹配类型</param>
        /// <returns></returns>
        public List<string> GetLocalFilePathes(string inputPath, string destFileExtension = "*.*o", string sourceFileExtension = "*.*")
        {
            //首先下载原始数据
            List<string> list = GetLocalFilePathes(inputPath, sourceFileExtension);

            //如果匹配目标类型，则直接返回。
            if (destFileExtension.EndsWith("*", StringComparison.CurrentCultureIgnoreCase)) { return list; }

            List<string> pathes = new List<string>();

            //判断并执行数据解压 
            foreach (var fileName in list)
            {
                if (!File.Exists(fileName)) { continue; }
               // if (IsCancel) { break; }
                var ps = FileDecompressManager.Decompress(fileName, destFileExtension);
                pathes.AddRange(ps);
            }

            return pathes;
        }
    }




}
