//2015.06.05, czs, craete in namu, 文本文件
//2016.02.10, czs, edit in hongqing, 重构，将NameNumber重构为INamed，只需要一个标识就可以了

 using System;  
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo;
using Geo.Common;
using Geo.IO;

namespace Geo.Data
{
    /// <summary>
    /// Gnsser ID 文件管理器。
    /// </summary>
    public class GidManager : BaseDictionary<string, string>
    {
        public GidManager() { }
        public GidManager(string path)
        {
            LineFileReader<GidItem> r = new LineFileReader<GidItem>(path);
            var list = r.ReadAll();
            foreach (var item in list)
            {
                this[item.FileId] = item.DbId;
            }
        }
        /// <summary>
        /// 读取文件ID和数据库ID。比如平差之星的Pname文件。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static GidManager ReadFileIdDbId(string path)
        {
            Geo.IO.Gmetadata nameData = Geo.IO.Gmetadata.NewInstance;
            nameData.PropertyNames = new string[] { "FileId", "DbId" };
            nameData.ItemSplliter = new string[] { " ", "\t" };

            GidManager GidManager = new Data.GidManager();
            LineFileReader<GidItem> r = new LineFileReader<GidItem>(path, nameData);
            var list = r.ReadAll();
            foreach (var item in list)
            {
                GidManager[ item.FileId ] = item.DbId;
            }
            return GidManager;
        }
    }


    /// <summary>
    /// 一行代表一个对象，两个标识。对应数据库ID和文件ID。
    /// </summary>
    public class GidItem : IOrderedProperty
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public GidItem()
        {
            OrderedProperties = new List<string>() { "DbId", "FileId" };
        }

        /// <summary>
        /// 点名
        /// </summary>
        public String DbId { get; set; }
        /// <summary>
        /// 点号
        /// </summary>
        public String FileId { get; set; }

        public List<string> OrderedProperties { get; set; }

        public List<ValueProperty> Properties { get; set; }
    }

}