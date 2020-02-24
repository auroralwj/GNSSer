using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Geo.Utils;
using Geo;

namespace Gnsser.Winform
{
    /// <summary>
    /// 处理文件过程。
    /// </summary>
    public class FileProcess
    {
        public delegate void HandleFileEventHandler(FileProcess FileProcess, string filePath);

        public FileProcess(string[] pathes, string infoHeader, IProgressCounter ProgressBar=null)
        {
            this.Pathes = pathes;
            this.InfoHeader = infoHeader;
            this.ProgressBar = ProgressBar;
        }

        public event HandleFileEventHandler HandleFileEvent;

        public Object Tag { get; set; }

        public bool CancellationPending { get; set; }

        public Geo.IProgressCounter ProgressBar { get; set; }
        //  public int ProcessCount { get; set; }

        public string[] Pathes { get; set; }

        public string InfoHeader { get; set; }

        public string Info { get; set; }

        public void Run()
        {
           if( this.ProgressBar!=null) this.ProgressBar.InitProcess(Pathes.Length);

            foreach (string path in Pathes)
            {
                if (CancellationPending) break;
                this.Info = InfoHeader + path;
                if (this.ProgressBar != null) this.ProgressBar.ShowInfo(this.Info);

                if (HandleFileEvent != null) HandleFileEvent(this, path);

                if (this.ProgressBar != null) this.ProgressBar.PerformProcessStep();
            }
        }
    }

}
