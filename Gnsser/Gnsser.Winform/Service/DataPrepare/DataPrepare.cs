using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
//using System.Windows.Forms;
using Geo.Utils;
using Geo;
using Geo.IO;

namespace Gnsser.Winform
{
    /// <summary>
    /// GNSS数据准备，包含从服务器下载、解压、格式化、大小写命名等。
    /// </summary>
    public class DataPrepare
    {
        public event InfoProducedEventHandler InfoProduced;
        
        string exeDir = @".\Data\Exe\";
        string baseDir = Setting.TempDirectory;
        string[] fileUrls;
        int allCount = 0;
        int allCountProcess = 0;

        //路径定义
        const string ORIGIN_DIR = "\\SOURSE";
        const string D_DIR = "\\DFile";
        const string TEMP_DIR = "\\TEMP";
        const string FORMATED_DIR = "\\FORMATED";
        const string RINEX_DIR = "\\RINEX";

        //功能
        bool isRemoteSourse = true;//是否远程
        bool isZSourse = true;//是否压缩

        bool enableTeqcFormat = true; //是否TEQC
        bool enableUpLow = true;
        bool isUp = false;
        bool delOriFile = false;
        bool delMidFile = false;
        bool ignoreError = true;
        Geo.IProgressCounter ProgressBar;
        private bool cancellationPending;
        /// <summary>
        /// 在执行过程中取消
        /// </summary>
        public bool IsCancelProcessing
        {
            get { return cancellationPending; }
            set { cancellationPending = value;
            ShowInfo("操作中断！");
            }
        }
        FileProcess currentProcess;

        /// <summary>
        /// GNSS数据准备。
        /// 从服务器下载压缩文件到本地，并解压。可选是否保留原始文件，中间文件，大小写文件名，格式化文件等。
        /// </summary>
        /// <param name="fileUrls"></param>
        /// <param name="saveDir"></param>
        /// <param name="exeDir"></param>
        /// <param name="enableTeqcFormat"></param>
        /// <param name="enableUpLow"></param>
        /// <param name="isUp"></param>
        /// <param name="delOriFile"></param>
        /// <param name="delMidFile"></param>
        /// <param name="ignoreError"></param>
        /// <param name="ProgressBar"></param>
        public DataPrepare( 
            string[] fileUrls,
            string saveDir,
            string exeDir = @".\Data\Exe\",
            bool isRemoteSourse = true,
            bool isZSourse = true,
            bool enableTeqcFormat = true,
            bool enableOFileFormat = true, //如果是Bern,则应是True
            bool enableUpLow =true,
            bool isUp = true,
            bool delOriFile = true,
            bool delMidFile = true, 
            bool ignoreError = true,
            Geo.IProgressCounter ProgressBar = null)
        {
            this.isRemoteSourse = isRemoteSourse;
            this.isZSourse = isZSourse;
            this.fileUrls = fileUrls;
            this.baseDir = saveDir;
            this.exeDir = exeDir;
            this.enableTeqcFormat = enableTeqcFormat;
            this.enableOFileFormat =  enableOFileFormat;
            this.enableUpLow = enableUpLow;
            this.delMidFile = delMidFile;
            this.delOriFile = delOriFile;
            this.isUp = isUp;
            this.ignoreError = ignoreError;
            this.ProgressBar = ProgressBar;


            allCount++; //下载
            if(isZSourse) allCount++; //解压Z
            if (isZSourse) allCount++; //解压Rinex
            allCount++; //移动和大小写
            if (enableTeqcFormat) allCount++; //格式化 
            if (enableOFileFormat) allCount++; //格式化 
        }

        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
        private bool enableOFileFormat;

        /// <summary>
        /// 运行。
        /// </summary>
        public void Run()
        {
            watch.Stop();
            watch.Start();
            Geo.Utils.FileUtil.CheckOrCreateDirectory(baseDir);

            string sourseDir = baseDir + ORIGIN_DIR;
            string dDir = baseDir + D_DIR;
            string rinexDir = baseDir + RINEX_DIR;
            string formateDir = baseDir + FORMATED_DIR;
            string currentDir;
            currentDir = sourseDir;

            allCountProcess++;

            //清空以往数据
            try
            {
                if (Directory.Exists(baseDir))
                {
                    Directory.Delete(baseDir, true);
                    ShowInfo("删除文件夹：" + baseDir);
                }
            }
            catch (Exception ex) { ShowInfo("删除文件夹出错，" + baseDir + "," + ex.Message); }

            //数据下载
            if (isRemoteSourse)
            {
                String msg = Download(currentDir);
                currentDir = sourseDir;
            }
            else
            {
                CopyToCurrentDir(currentDir);
            }


            if (isZSourse)
            {
                allCountProcess++;
                DecomressZFile(currentDir, dDir, this.delOriFile);
                if (this.delOriFile) { Geo.Utils.FileUtil.TryDeleteFileOrDirectory(currentDir); }
                currentDir = dDir;

                allCountProcess++;
                DecomposeRinex(currentDir, rinexDir, this.delMidFile);
                if (this.delMidFile) { Geo.Utils.FileUtil.TryDeleteFileOrDirectory(currentDir); }
                currentDir = rinexDir;
            }

            if (enableTeqcFormat)
            {
                allCountProcess++;
                FormatRinex(currentDir, formateDir, this.delMidFile);
                if (this.delMidFile) { Geo.Utils.FileUtil.TryDeleteFileOrDirectory(currentDir); }
                currentDir = formateDir;
            }

            if (enableOFileFormat)
            {
                allCountProcess++;
                FormatOFile(currentDir, formateDir, this.delMidFile);
                if (this.delMidFile) { Geo.Utils.FileUtil.TryDeleteFileOrDirectory(currentDir); }
                currentDir = formateDir;
            }

            allCountProcess++;
            MoveAndUpperLowFileName(currentDir, baseDir);
            if (this.delMidFile) { Geo.Utils.FileUtil.TryDeleteFileOrDirectory(currentDir); }

            watch.Stop();
            string info = "操作结束！" + "耗时：" + watch.Elapsed;

            ShowInfo(info);
        }

        /// <summary>
        /// 运行。
        /// 返回下载后文件保存地址
        /// </summary>
        public void strRun()
        {
            watch.Stop();
            watch.Start();

            if (!Directory.Exists(baseDir)) Directory.CreateDirectory(baseDir);
            string sourseDir = baseDir + ORIGIN_DIR;
            string dDir = baseDir + D_DIR;
            string rinexDir = baseDir + RINEX_DIR;
            string formateDir = baseDir + FORMATED_DIR;
            string currentDir;
            currentDir = sourseDir;

            allCountProcess++;

            //清空以往数据,是否需要？？，不需要，则下面都得改动，不能按照文件夹判断了。
            try
            {
                if (Directory.Exists(baseDir))
                {
                    Directory.Delete(baseDir, true);
                    ShowInfo("删除文件夹：" + baseDir);
                }
            }
            catch (Exception ex) { ShowInfo("删除文件夹出错，" + baseDir + "," + ex.Message); }

            //数据下载
            if (isRemoteSourse)
            {
                String msg = Download(currentDir);
                currentDir = sourseDir;
            }
            else
            {
                CopyToCurrentDir(currentDir);
            }


            if (isZSourse)
            {
                allCountProcess++;
                DecomressZFile(currentDir, dDir, this.delOriFile);
                if (this.delOriFile && Directory.GetFiles(currentDir).Length == 0)
                {
                    Directory.Delete(currentDir);
                }
                currentDir = dDir;

                allCountProcess++;
                DecomposeRinex(currentDir, rinexDir, this.delMidFile);
                if (this.delMidFile && Directory.GetFiles(currentDir).Length == 0)
                {
                    Directory.Delete(currentDir);
                }
                currentDir = rinexDir;
            }

            if (enableTeqcFormat)
            {
                allCountProcess++;
                FormatRinex(currentDir, formateDir, this.delMidFile);
                if (this.delMidFile && Directory.GetFiles(currentDir).Length == 0)
                {
                    Directory.Delete(currentDir);
                }
                currentDir = formateDir;
            }

            if (enableOFileFormat)//Bernese调用，标准化文件
            {
                allCountProcess++;
                FormatOFile(currentDir, formateDir, this.delMidFile);
                if (this.delMidFile && Directory.GetFiles(currentDir).Length == 0)
                {
                    Directory.Delete(currentDir);
                }
                currentDir = formateDir;
            }

            allCountProcess++;
            MoveAndUpperLowFileName(currentDir, baseDir);
            if (this.delMidFile && Directory.GetFiles(currentDir).Length == 0)
            {
                Directory.Delete(currentDir);
            }

            watch.Stop();
            string info = "操作结束！" + "耗时：" + watch.Elapsed;

            ShowInfo(info);
        }
        /// <summary>
        /// 复制到指定目录
        /// </summary>
        /// <param name="currentDir"></param>
        private void CopyToCurrentDir(string currentDir)
        {
            if (this.ProgressBar != null) this.ProgressBar.InitProcess(fileUrls.Length);
            if (!Directory.Exists(currentDir)) Directory.CreateDirectory(currentDir);
            foreach (var item in this.fileUrls)
            {
                string dest = Path.Combine(currentDir, Path.GetFileName(item));

                if (File.Exists(dest)) { File.Delete(dest); }

                File.Copy(item, dest, true);

                ShowInfo("总进度 " + (allCountProcess) + "/" + allCount + " 复制到指定目录 " + item);
                if (this.ProgressBar != null) this.ProgressBar.PerformProcessStep();
            }
        }

        private void MoveAndUpperLowFileName(string sourseDir, string destDir)
        {
            if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);

            string[] files = Directory.GetFiles(sourseDir);
            string msg = " 移动 " ;
            if(this.enableUpLow) msg += isUp ? "并 大写文件名 " : "并 小写文件名 ";
            string infoHeader = "总进度 " + (allCountProcess) + "/" + allCount + msg;

            currentProcess = new FileProcess(files, infoHeader, this.ProgressBar);
            currentProcess.HandleFileEvent += new FileProcess.HandleFileEventHandler(UpperLowFileName_HandleFileEvent);
            currentProcess.Tag = destDir; 
            currentProcess.Run();
        }

        void UpperLowFileName_HandleFileEvent(FileProcess FileProcess, string filePath)
        {
            string destDir = currentProcess.Tag as String;
            string fileName = Path.GetFileName(filePath);
            if (this.enableUpLow)
                fileName = isUp ? fileName.ToUpper() : fileName.ToLower();
            string newPath = Path.Combine(destDir, fileName);

            if (File.Exists(newPath)) File.Delete(newPath);

            File.Move(filePath, newPath);
            ShowInfo(FileProcess.Info);
        }

        private void FormatRinex(string sourseDir, string destDir, bool delSourse)
        {
            if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);

            string[] files = Directory.GetFiles(sourseDir);

            if (this.ProgressBar != null) this.ProgressBar.InitProcess(files.Length);
             
            TeqcFormater de = new TeqcFormater();
            foreach (var item in files)
            {
                if (IsCancelProcessing) break; ;

                try
                {
                    de.Formate(item, destDir, delSourse);
                    ShowInfo("总进度 " + (allCountProcess) + "/" + allCount + " TEQC格式化 " + item);
                    PerformStep();
                }
                catch (Exception ex)
                {
                    string msg = "格式化Rinex出错了:" + ex.Message;
                    ShowInfo(msg);
                    if (!ignoreError)
                        if (FormUtil.ShowYesNoMessageBox(msg + "是否继续?")
                            == System.Windows.Forms.DialogResult.No)
                            break;
                }
            }
        }


        private void FormatOFile(string sourseDir, string destDir, bool delSourse)
        {
            if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);

            string[] files = Directory.GetFiles(sourseDir, "*.*O");

            if (this.ProgressBar != null) this.ProgressBar.InitProcess(files.Length);
             
            foreach (var item in files)
            {
                if (IsCancelProcessing) break; ;

                try
                {
                    string okOrNot = "";

                    if (Gnsser.Data.Rinex.ObsFileFormater.Format(item, destDir))
                    {
                        if (delSourse) File.Delete(item);
                        okOrNot = "已更改！";
                    }
                    else
                    {
                        if (sourseDir != destDir) 
                        File.Move(item, Path.Combine(destDir, Path.GetFileName(item)));
                    }

                    ShowInfo("总进度 " + (allCountProcess) + "/" + allCount + " 观测文件格式化 " + item + " " + okOrNot);
                    PerformStep();
                }
                catch (Exception ex)
                {
                    string msg = "格式化观测文件出错了:" + ex.Message;
                    ShowInfo(msg);
                    if (!ignoreError)
                        if (FormUtil.ShowYesNoMessageBox(msg + "是否继续?")
                            == System.Windows.Forms.DialogResult.No)
                            break;
                }
            }


        }

        private void DecomposeRinex(string sourseDir, string rinexDir, bool delSourse)
        {
            if (!Directory.Exists(rinexDir)) Directory.CreateDirectory(rinexDir);

            string[] files = Directory.GetFiles(sourseDir, "*.**d");
            if (this.ProgressBar != null) this.ProgressBar.InitProcess(files.Length);
             
            Geo.Utils.DecompressRinexer de = new Geo.Utils.DecompressRinexer(Setting.PathOfCrx2rnx);
            foreach (var item in files)
            {
                if (IsCancelProcessing) break;

                try
                {
                    de.Decompress(item, rinexDir);
                    if (delSourse) File.Delete(item);

                    ShowInfo("总进度 " + (allCountProcess) + "/" + allCount + " 解压Rinex " + item);

                    PerformStep();
                }
                catch (Exception ex)
                {
                    string msg = "解压Rinex出错了:" + ex.Message + "\r\n";
                    ShowInfo(msg);
                    if (!ignoreError)
                        if (FormUtil.ShowYesNoMessageBox(msg + "是否继续?")
                            == System.Windows.Forms.DialogResult.No)
                            break;
                }

            }
        }
     

        /// <summary>
        /// 解压Z文件
        /// </summary>
        /// <param name="ObsDataSource"></param>
        /// <param name="destDir"></param>
        /// <param name="delSourse"></param>
        private void DecomressZFile(string source, string destDir, bool delSourse)
        {
            if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);

            string[] files = Directory.GetFiles(source);
            if (this.ProgressBar != null) this.ProgressBar.InitProcess(files.Length);

            foreach (var item in files)
            {
                if (IsCancelProcessing) break; 

                try
                {
                    CompressUtil.Decompres(item, destDir);
                } catch (Exception ex)
                {
                    string msg = "解压 Z 文件出错了:" + ex.Message + "\r\n";
                    ShowInfo(msg);
                    if (!ignoreError)
                        if (FormUtil.ShowYesNoMessageBox(msg + "是否继续?")
                            == System.Windows.Forms.DialogResult.No)
                            break;
                }

                //try{
                //    if (delSourse) { System.Threading.Thread.Sleep(100); File.Delete(key); }
                //}
                //catch (Exception ex)
                //{
                //    string msg = "删除 Z文件出错了:" + ex.Message + key+ "\r\n";
                //    ShowInfo(msg);
                //    if (!ignoreError)
                //        if (FormUtil.ShowYesNoMessageBox(msg + "是否继续?")
                //            == System.Windows.Forms.DialogResult.No)
                //            break;
                //}

                string info = "解压Z文件 " + item;
                ShowInfo("总进度 " + (allCountProcess) + "/" + allCount + " " + info);

                PerformStep();

            }
        }
        /// <summary>
        /// 下载文件。
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private String Download(string dir)
        {
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            if (this.ProgressBar != null) this.ProgressBar.InitProcess(fileUrls.Length);

            List<string> failed = new List<string>();
            foreach (string url in fileUrls)
            {
                if (IsCancelProcessing) break; 

                string info = "下载成功 ";

                if (!Geo.Utils.NetUtil.FtpDownload(url, Path.Combine(dir, Path.GetFileName(url))))
                {
                    failed.Add(url);
                    info = "下载失败！ ";
                }

                ShowInfo("总进度 " + (allCountProcess) + "/" + allCount + " " + info + url);

                PerformStep();
            }
            String msg = "共下载 " + (fileUrls.Length - failed.Count) + " 个文件。\r\n下载失败 " + failed.Count + " 个。\r\n";
            StringBuilder sb = new StringBuilder();
            foreach (string fail in failed)
            {
                sb.AppendLine(fail);
            }
            msg += sb.ToString();
            return msg;
        }

        private void ShowInfo(string info)
        {
            if (this.ProgressBar != null) this.ProgressBar.ShowInfo(info);
            if (InfoProduced != null) InfoProduced(info);
        }
         
        private void PerformStep()
        {
            if (this.ProgressBar != null) this.ProgressBar.PerformProcessStep();
        }



    }


}
