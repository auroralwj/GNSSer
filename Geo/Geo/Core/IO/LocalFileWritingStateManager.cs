//2015.12.26, czs, create in 洪庆, 写文件管理器

using System;
using System.Collections.Generic;

namespace Geo.IO
{
    /// <summary>
    /// 写文件管理器
    /// </summary>
    public class LocalFileWritingStateManager
    {
        private LocalFileWritingStateManager()
        {
            this.data = new List<string>();
        }
        Log log = new Log(typeof( LocalFileWritingStateManager ));

        static LocalFileWritingStateManager instance = new LocalFileWritingStateManager();

        public static LocalFileWritingStateManager Instance { get { return instance; } }


        private List<string> data { get; set; }

        public void Add(string filePath)
        {
            this.data.Add(filePath.ToUpper());
        }

        public void Remove(string filePath)
        {
            this.data.Remove(filePath.ToUpper());
        }

        public bool Contains(string filePath)
        {
            return this.data.Contains(filePath.ToUpper());
        }

        /// <summary>
        /// 查看是
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool Regist(string filePath)
        {
            if (this.Contains(filePath))
            {
                log.Info("文件正在被处理，不可操作，" + filePath);
                return false;
            }
            else
            {
                this.Add(filePath);
                return true;
            }
        }

        public void Unregist(string filePath)
        {
           this.Remove(filePath);         
        }
    }

}
