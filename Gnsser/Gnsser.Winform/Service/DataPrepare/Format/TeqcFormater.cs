using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Geo.Common;
using System.Diagnostics;
using System.Threading.Tasks;


namespace Gnsser.Winform
{
    public class TeqcFormater
    { 
        const string CLEANED_DIR = "\\CLEANED";
        public TeqcFormater()
        {
            this.teqcPath = "\"" + Application.StartupPath + "\\Data\\Exe\\teqc.exe" + "\"";
        //    this.teqcPath = "\"" + Application.StartupPath + "\\teqc.exe" + "\"";
        }
        string teqcPath;
        public void Formate(string[] sourses, string dest, bool delSourse = false)
        {
            foreach (var item in sourses)
            {
                Formate(item, dest, delSourse);
            }
        }

        public string Formate(string sourse, string destDir, bool delSourse = false)
        {
            string dest = Path.Combine(destDir, Path.GetFileName(sourse));
            string param = " \"" + sourse + "\">\"" + dest + "\"";
            string param3 = " -R -S -J -C -E -O.dec 30 -O.obs L1L2C1P1P2 " + sourse + " > " + dest;
            //ProcessRunner cmd = new ProcessRunner(teqcPath);
            //string result = "";//cmd.Run(param3)[0];
            var cmd = new Command();
            string result = cmd.Run(teqcPath + param3);
         
            // string param3 = " -O.obs " + "\"L1L2C1P1P2\" " + sourse + ">" + dest;//剔除GLONASS的数据，崔阳，2013.12.24
            
            //Process p = new Process();
            //p.StartInfo.FileName = teqcPath;//需要启动的程序名   
            //p.StartInfo.RedirectStandardError = false;
            //p.StartInfo.UseShellExecute = true;
            ////p.ErrorDataReceived += new DataReceivedEventHandler(Output);
            //p.StartInfo.Arguments = param3;//启动参数   
            //p.StartInfo.CreateNoWindow = true;
            //p.Start();//启动
            //p.Close();
            //p.Dispose();//释放资源 



            //System.Diagnostics.Process exep = new System.Diagnostics.Process();
            //exep.StartInfo.FileName = teqcPath;
            //exep.StartInfo.Arguments = param3;
            //exep.StartInfo.CreateNoWindow = true;
            //exep.StartInfo.UseShellExecute = false;
            //exep.Start();
            //exep.WaitForExit();//关键，等待外部程序退出后才能往下执行



           // System.Threading.Thread.Sleep(100);

            if (delSourse && Path.GetDirectoryName(dest).ToUpper() != Path.GetDirectoryName(sourse).ToUpper()) File.Delete(sourse);

            //Cui Yang: 删除观测文件中的断续部分
            string cleanDir = destDir + CLEANED_DIR;
            if (!Directory.Exists(cleanDir)) Directory.CreateDirectory(cleanDir);
            string destfile = Path.Combine(cleanDir, Path.GetFileName(dest));

            if (File.Exists(dest))
            {
                FileStream aFile = new FileStream(destfile, FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(aFile);
                StreamReader r = new StreamReader(dest);
                string line = null;
                while ((line = r.ReadLine()) != null)//读取观测数据的第一句。
                {
                    if (line.Length == 32 && line.Substring(0, 26) == "                          ")
                    {
                        int mark = int.Parse(line.Substring(28, 2));

                        if (mark == 4)
                        {
                            //
                            int count = int.Parse(line.Substring(30, 2));
                            for (int j = 0; j < count; j++)
                            { line = r.ReadLine(); }
                        }
                    }
                    else
                    {
                        sw.WriteLine(line);
                    }
                }
                r.Close();
                sw.Close();
            }
            //Rinex.ObsFile obsFile = Gnss.Rinex.ObsFile.Read(dest);

            return result;

        }

    }
}
