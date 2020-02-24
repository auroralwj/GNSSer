//2015.06.05, czs, create in namu, 文件服务
//2016.11.29, czs, edit in hongqing, 从 Geodesy 中迁移到 Geo 中。

 using System;  
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Common;
using Geo;


namespace Geo.Data
{
    public abstract class FileDataService<TProduct,ItemFile > : Geo.FileBasedService<TProduct>
        where ItemFile : ItemFile<TProduct>, new()
        where TProduct : IStringId
    {
           /// <summary>
        /// 文件数据源构造函数。
        /// </summary>
        /// <param name="Option">文件选项</param>
        /// <param name="name">名称</param>
        public FileDataService(FileOption Option, string name = "文件数据源")
            : base(Option, name)
        {
          
        }

           /// <summary>
        /// 文件数据源构造函数。
        /// </summary>
        /// <param name="path">文件选项</param>
        /// <param name="name">名称</param>
        public FileDataService(string path, string name = "文件数据源")
            : this( new FileOption(path), name)
        { 
        }

        public  ItemReader<TProduct, ItemFile> Reader;

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
         public ItemFile GetItems()
         {
             return  Reader.Read();
         }
    }


}
