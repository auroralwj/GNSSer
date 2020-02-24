//2015.09.28, czs, create in xi'an hongqing, Gnsser文件读取通用接口
//2015.11.07, czs, create in D5181 达州到成都动车，提取对象读写通用接口


using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Data;
using Geo.Common;
using Geo.Coordinates;
using System.Collections;
using System.Collections.Generic;

namespace Geo.IO
{
    /// <summary>
    /// 行文件读取通用接口。本类采用流式读取，实现了枚举接口 IEnumerable 和 IEnumerator。
    /// </summary>
    public  class XmlFileReader<T> : Geo.IO.EntityFileReader<T> where T : IOrderedProperty
    {
        XmlDocument doc = new XmlDocument();  

        #region 构造函数与初始化
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public XmlFileReader()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">数据文件路径。</param>
        /// <param name="metaFilePathOrDirectory">元数据文件或者元数据文件存放的目录。若不指定，则自动寻找文件目录下的元数据，若没有则调用默认的元数据，若还没有则报错。</param>
        public XmlFileReader(string filePath, string metaFilePathOrDirectory = null)
            : base(filePath, metaFilePathOrDirectory)
        {  
        } 

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">数据文件路径。</param>
        /// <param name="Gmetadata"></param>
        public XmlFileReader(string filePath, Gmetadata Gmetadata)
            :base(filePath, Gmetadata)
        { 
        } 
         
        /// <summary>
        /// 设置数据流
        /// </summary>
        protected override void InitStreamReader()
        {
            doc.Load(InputPath);

            Entities = new List<T>();
            var className = this.EntityType.Name;
            XmlNodeList list = doc.GetElementsByTagName(className);
           // XmlNodeList colName = e.SelectNodes("./" + className);

            var obj = Activator.CreateInstance<T>();
            foreach (XmlNode item in list)
            {
                foreach (var property in obj.OrderedProperties)
                {
                    var valString = item.SelectSingleNode("./" + property).InnerText;
                    var propertyInfo = EntityType.GetProperty(property);
                    var initVal = propertyInfo.GetValue(obj, null);
                    object newValue = GetOrParseValueString(valString, property);

                    //赋值
                    if (newValue != null && newValue != initVal)
                    {
                        propertyInfo.SetValue(obj, newValue, null);
                    }

                } 
            }
            Entities.Add(obj);

            this.CurrentIndex = -1;  
        }
       #endregion
        
        #region 属性
        /// <summary>
        /// 实体对象集合
        /// </summary>
        public List<T> Entities { get; set; }
        
        #endregion
         
        /// <summary>
        /// 移动到下一个。
        /// </summary>
        /// <returns></returns>
        public override  bool MoveNext()
        {
            CurrentIndex++;
            if (Entities != null && CurrentIndex < Entities.Count)
            {
               Current = Entities[CurrentIndex];  
            }
             
            return false;
        } 
        /// <summary>
        /// 释放资源
        /// </summary>
        public override  void Dispose()
        {
            doc.Clone(); 
        } 
    }
}
