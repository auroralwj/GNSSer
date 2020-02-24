//2015.11.07, czs, create in 达州火车站华莱士, 对象Xml文件写入通用接口

using System;
using System.IO;
using System.Text;
using System.Data;
using Geo.Common;
using Geo.Coordinates;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;

namespace Geo.IO
{

    /// <summary>
    /// 块式读取，适合小文件。对象Xml文件写入通用接口.
    /// </summary>
    public class XmlFileWriter<T> : Geo.IO.EntityWriter<T>, IDisposable
        where T : IOrderedProperty
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public XmlFileWriter() {  }

        #region 构造函数与初始化
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">数据文件路径。</param>
        /// <param name="metaFilePathOrDirectory">元数据文件或者元数据文件存放的目录。若不指定，则自动寻找文件目录下的元数据，若没有则调用默认的元数据，若还没有则报错。</param>
        public XmlFileWriter(string filePath, string metaFilePathOrDirectory = null)
            : base(filePath, metaFilePathOrDirectory)
        { 
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">数据文件路径。</param>
        /// <param name="Gmetadata"></param>
        public XmlFileWriter(string filePath, Gmetadata Gmetadata)
            : base(filePath, Gmetadata)
        { 
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Stream">数据流。</param>
        /// <param name="Gmetadata"></param>
        public XmlFileWriter(Stream Stream, Gmetadata Gmetadata)
            : base(Stream, Gmetadata)
        {  
        } 
         

        ///// <summary>
        ///// 设置数据流
        ///// </summary>
        //protected override void InitStreamWriter(Stream Stream)
        //{
        //    StreamWriter = new StreamWriter(Stream, System.Text.Encoding.UTF8);
        //    this.CurrentIndex = -1;
        //}

        #region 初始化元数据
        private void SetMetaData(Gmetadata Gmetadata)
        {
            if (Gmetadata != null)
            {
                this.Metadata = Gmetadata;
            }
            else
            {
                this.Metadata = GetDefaultMetadata();
            }

        } 
        #endregion
         
       #endregion

        #region 属性     
        /// <summary>
        /// 数据流阅读器
        /// </summary>
      //  public StreamWriter StreamWriter { get; set; } 
        #endregion

        /// <summary>
        /// 写入一行注释
        /// </summary>
        /// <param name="comment"></param>
        public override void WriteCommentLine(string comment)
        {
            StreamWriter.WriteLine(BuildComment(comment));
        }

        /// <summary>
        /// 在当前行追加注释
        /// </summary>
        /// <param name="comment"></param>
        public override void AppendComment(string comment)
        {
            StreamWriter.Write(BuildComment(comment));
        } 
        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="obj"></param>
        public override void Write(T obj)
        {
            StringBuilder sb = new StringBuilder();
            var className = this.EntityType.Name;

            AppendStartTagLine(className, sb);
                 
            foreach (var item in Properties)
	        {
                var xml = BuildElementLine(item, obj); 
               sb.Append(xml);  
	        }

            AppendEndTagLine(className, sb);
            StreamWriter.WriteLine(sb.ToString());

            CurrentIndex++;
        }

        #region 转换工具  
        string former = "<";
        string latter = ">";
        /// <summary>
        /// 建立XML注释字符串
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        protected string BuildElementLine(PropertyInfo propertyInfo, T obj) { return BuildElement(propertyInfo, obj) + "\r\n";  }
        /// <summary>
        /// 建立XML注释字符串
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        protected string BuildElement(PropertyInfo propertyInfo, T obj)
        {
            var name = propertyInfo.Name;
   
            StringBuilder sb = new StringBuilder();

            AppendStartTag(name, sb);

            var val = propertyInfo.GetValue(obj, null);
            sb.Append(val);

            AppendEndTag(name, sb);

            return sb.ToString();
        }
         /// <summary>
         /// 追加结束标签
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sb"></param>
        protected void AppendStartTagLine(string name, StringBuilder sb) { AppendStartTag(name, sb); sb.AppendLine(); }
       
        /// <summary>
        /// 追加起始标签
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sb"></param>
        protected void AppendStartTag(string name, StringBuilder sb)
        {
            sb.Append(former);
            sb.Append(name);
            sb.Append(latter);
        }
        /// <summary>
        /// 追加结束标签
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sb"></param>
        protected void AppendEndTagLine(string name, StringBuilder sb) { AppendEndTag(name, sb); sb.AppendLine(); }
        /// <summary>
        /// 追加结束标签
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sb"></param>
        protected void AppendEndTag(string name, StringBuilder sb)
        {
            sb.Append(former);
            sb.Append("/");
            sb.Append(name);
            sb.Append(latter);
        }


        /// <summary>
        /// 建立XML注释字符串
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        protected string BuildComment(string comment)
        {
            return "<!-- " + comment + "-->";
        }
        #endregion 
    }
}
