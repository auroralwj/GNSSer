//2014.10.24, czs, create in namu shuangliao, 通用数据源接口

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Geo.Common;
using Geo.IO;

namespace Geo
{
    /// <summary>
    /// 通用文件数据源，初始化时，采用文件选项进行初始化.
    /// </summary>
    public class FileBasedService<TProduct> : IFileBasedService<TProduct>
    {
        protected Log log = new Log(typeof(FileBasedService<TProduct>));

        /// <summary>
        /// 文件数据源构造函数。
        /// </summary>
        /// <param name="fileName">文件选项</param>
        public FileBasedService(string fileName)
            :this(new FileOption(fileName), Path.GetFileName(fileName) )
        { 
        }
        /// <summary>
        /// 文件数据源构造函数。
        /// </summary>
        /// <param name="Option">文件选项</param>
        /// <param name="name">名称</param>
        public FileBasedService(FileOption Option, string name = "文件数据源")
        {
            this.Option = Option;
            this.Name = name; 
            if(String.IsNullOrWhiteSpace(this.Option.FilePath)){
                log.Error( this.Name + "小心！服务路径为空！");
            }else  if ( !this.Option.FilePath.Contains("{")){
                if( Geo.Utils.FileUtil.IsDirectory(this.Option.FilePath)){
                    if (!Directory.Exists(this.Option.FilePath))
                    {
                        log.Error(this.Name + "小心！服务目录不存在 " + this.Option.FilePath);
                    }
                }else if(!File.Exists(this.Option.FilePath)) {
                    log.Error(this.Name + "小心！服务文件不存在 " + this.Option.FilePath);
                }
            }           
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///  数据源选项。
        /// </summary>
        public FileOption Option { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }



}
