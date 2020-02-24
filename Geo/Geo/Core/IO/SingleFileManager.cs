//2015.11.02, czs, create in hongqing,  基于单文件的文件管理   

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 基于单文件的文件管理。
    /// </summary>
    public abstract class SingleFileManager
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="FilePath"></param>
        public SingleFileManager(string FilePath)
        {
            this.FilePath = FilePath;
            Load();
        }

        #region 属性
        /// <summary>
        /// 文件路径。
        /// </summary>
        public string FilePath { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 从文件加载。
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// 保存到文件
        /// </summary>
        public abstract void Save();
        #endregion
    }
}
