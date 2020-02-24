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
    /// Xml读取通用接口。本类采用流式读取，实现了枚举接口 IEnumerable 和 IEnumerator。
    /// </summary>
    public class XmlStreamReader<T> : Geo.IO.EntityFileReader<T> where T : IOrderedProperty
    {
        Log log = new Log(typeof( XmlStreamReader<T>));

        #region 构造函数与初始化
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public XmlStreamReader()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">数据文件路径。</param>
        /// <param name="metaFilePathOrDirectory">元数据文件或者元数据文件存放的目录。若不指定，则自动寻找文件目录下的元数据，若没有则调用默认的元数据，若还没有则报错。</param>
        public XmlStreamReader(string filePath, string metaFilePathOrDirectory = null)
            : base(filePath, metaFilePathOrDirectory)
        {  
        } 

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">数据文件路径。</param>
        /// <param name="Gmetadata"></param>
        public XmlStreamReader(string filePath, Gmetadata Gmetadata)
            : base(filePath, Gmetadata)
        { 
        }
           
        /// <summary>
        /// 设置数据流
        /// </summary>
        protected override void InitStreamReader()
        {                       
            StreamReader = new XmlTextReader(InputPath);
            this.CurrentIndex = -1;
        }
       #endregion

        #region 属性
         
        /// <summary>
        /// 数据流阅读器
        /// </summary>
        private XmlTextReader StreamReader { get; set; } 
          
        #endregion
         
        #region 核心转换  
           


        /// <summary>
        /// 移动到下一个。
        /// </summary>
        /// <returns></returns>
        public override bool MoveNext()
        {
            #region 流程控制
            CurrentIndex++;
            if (CurrentIndex == StartIndex) { log.Debug("数据流 " + this.Name + " 开始读取数据。"); }
            if (this.IsCancel) { log.Info("数据流 " + this.Name + " 已被手动取消。"); return false; }
            if (CurrentIndex > this.MaxEnumIndex) { log.Info("数据流 " + this.Name + " 已达指定的最大编号 " + this.MaxEnumIndex); return false; }
            while (CurrentIndex < this.StartIndex) { this.MoveNext(); }
            #endregion

            this.StreamReader.Read();

            var model = Activator.CreateInstance<T>();
            var reader = StreamReader;
            while (reader.Read())
            {
                int loopCount = 0;
                if (reader.NodeType == XmlNodeType.Element)
                {
                    var tagName = reader.Name;
                    if (reader.Name == ClassName)
                    {
                        loopCount++;
                        //model.BookType = reader.GetAttribute(0);  
                        //model.BookISBN = reader.GetAttribute(1);  
                    }

                    if (this.PropertyIndexes.ContainsKey(tagName))
                    {
                        var valString = reader.ReadElementString().Trim();

                        var propertyInfo = EntityType.GetProperty(tagName);
                        var initVal = propertyInfo.GetValue(model, null);
                        object newValue = GetOrParseValueString(valString, tagName);

                        //赋值
                        if (newValue != null && newValue != initVal)
                        {
                            propertyInfo.SetValue(model, newValue, null);
                        }
                    }
                }
                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.Name == ClassName || (loopCount > 1))
                    {
                        break;
                    }
                }
            }
            //更新上一个
            this.PreviousObject = model;

            return true;
        }




        /// <summary>
        /// 重置。先释放资源，再初始化。
        /// </summary>
        public override  void Reset()
        {
            CurrentIndex = -1;
            Dispose();
            InitStreamReader();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override  void Dispose()
        {
            if (StreamReader != null )
            {
                StreamReader.Close();
            }
        } 
        #endregion
         
         
    }
}
