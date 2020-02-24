using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

using System.Diagnostics;

namespace Geo.Utils
{

    public class DecompressRinexer
    {
        public event EventHandler ProcessExited;

        string exePath =Setting.PathOfCrx2rnx;

        public DecompressRinexer(string exePath = null)
        {
            if(exePath != null)
               this.exePath = exePath;
        }


        /// <summary>
        /// 解压文件或目录。
        /// </summary>
        /// <param name="fileOrDirectory"></param>
        /// <param name="toDirectory"></param>
        /// <param name="deleSource"></param>
        /// <param name="overwrite"></param>
        public void DecompressFileOrDirectory(string fileOrDirectory, string toDirectory, bool deleSource, bool overwrite)
        {
            bool isDirectory = Directory.Exists(fileOrDirectory);
            if (isDirectory)
            {

                string[] files = Directory.GetFiles(fileOrDirectory, "*.**d");
                foreach (var item in files)
                {
                    Decompress(item, toDirectory, deleSource, overwrite);
                }
            }
            else if(File.Exists(fileOrDirectory))
            {
                Decompress(fileOrDirectory, toDirectory, deleSource, overwrite);
            }
            else
            {
                throw new FileNotFoundException(" 不存在目录或文件 " + fileOrDirectory);
            }
        }
        

        public void Decompress(string[] files, string destDir, bool delSource = false)
        {
            foreach (var item in files)
            {
                Decompress(item, destDir, delSource);
            }
        }
        public List<string> Decompress(string file, string destDir, bool delSource = false, bool overwrite = true)
        { 
            Geo.Common.ProcessRunner cmd = new Geo.Common.ProcessRunner(exePath);
            cmd.ExitedOrDisposed += cmd_ProcessExited;
           // string str = exePath + " \"" + file + "\" " + "-f -s";
            string str =  file + " -f -s";
            cmd.Run(str);

            if (delSource) File.Delete(file);
            List<string> result = new List<string>();
            //目标路径非相同
            if (!PathUtil.IsSamePath( destDir, Path.GetDirectoryName(file)))
            {
                //暂时没找到更能好的方法2013.03.01
                string[] files = Directory.GetFiles(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "*");
                foreach (var item in files)
                {
                    if (String.Equals(  file,  item, StringComparison.CurrentCultureIgnoreCase)){ continue;}
                    string dest = Path.Combine(destDir, Path.GetFileName(item));

                    result.Add(dest);
                    if (File.Exists(dest) )
                    {
                        if (overwrite)
                        {
                            File.Delete(dest);
                            File.Move(item, dest);
                        }
                    }
                    else
                    {
                        File.Move(item, dest); 
                    } 
                }
            }
            return result;
        }

        void cmd_ProcessExited(object sender, EventArgs e)
        {
            if (ProcessExited != null) ProcessExited(sender, e);
        }


        static public bool DecompressD(string formFileOrDirectory, string toDirectory, bool isDeleSource, bool overwrite){
            DecompressRinexer de = new DecompressRinexer();
            de.DecompressFileOrDirectory(formFileOrDirectory, toDirectory, isDeleSource, overwrite);
            return true;
        }
    }


}
