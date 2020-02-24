//2015.11.07, czs, create in 达州火车站华莱士, 对象写入通用接口

using System;
using System.IO;
using System.Text;
using System.Data;
using Geo.Common;
using Geo.Coordinates;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.Reflection;

namespace Geo.IO
{

    /// <summary>
    /// 块式读取，适合小文件。对象Xml文件写入通用接口.
    /// </summary>
    public abstract class EntityWriter<T> : Geo.IO.IEntityWriter<T>, IDisposable
        where T : IOrderedProperty
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public EntityWriter() {  }

        #region 构造函数与初始化
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">数据文件路径。</param>
        /// <param name="metaFilePathOrDirectory">元数据文件或者元数据文件存放的目录。若不指定，则自动寻找文件目录下的元数据，若没有则调用默认的元数据，若还没有则报错。</param>
        public EntityWriter(string filePath, string metaFilePathOrDirectory = null, Encoding Encoding = null, FileMode FileMode = FileMode.Create)
            : this()
        {
            this.FilePath = filePath;
            Init(new FileStream(this.FilePath, FileMode, FileAccess.Write), metaFilePathOrDirectory, Encoding);
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">数据文件路径。</param>
        /// <param name="Gmetadata"></param>
        public EntityWriter(string filePath, Gmetadata Gmetadata, Encoding Encoding = null, FileMode FileMode = FileMode.Create)
            : this()
        {
            this.FilePath = filePath;
            Init(new FileStream(this.FilePath, FileMode, FileAccess.Write), Gmetadata, Encoding);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Stream">数据流。</param>
        /// <param name="Gmetadata"></param>
        public EntityWriter(Stream Stream, Gmetadata Gmetadata, Encoding Encoding = null)
            : this()
        {
            Init(Stream, Gmetadata, Encoding);
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="Stream"></param>
        /// <param name="Gmetadata"></param>
        public virtual void Init(Stream Stream, Gmetadata Gmetadata, Encoding Encoding = null)
        {
            if (Encoding == null) { Encoding = Encoding.ASCII; }

            SetMetaData(Gmetadata);
            SetProperties();
            InitStreamWriter(Stream, Encoding);
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="Stream"></param>
        /// <param name="metaFilePathOrDirectory"></param>
        /// <returns></returns>
        public virtual void Init(string filePath, string metaFilePathOrDirectory = null, FileMode FileMode = FileMode.Create)
        {
            this.FilePath = filePath;
            Init(new FileStream(this.FilePath, FileMode, FileAccess.Write), metaFilePathOrDirectory);
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="Stream"></param>
        /// <param name="metaFilePathOrDirectory"></param>
        /// <returns></returns>
        public virtual void Init(string filePath, string metaFilePathOrDirectory = null)
        {
            this.FilePath = filePath;
            Init(new FileStream(this.FilePath, FileMode.Create, FileAccess.Write), metaFilePathOrDirectory);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="Stream"></param>
        /// <param name="metaFilePathOrDirectory"></param>
        /// <returns></returns>
        public virtual void Init(Stream Stream, string metaFilePathOrDirectory = null, Encoding Encoding = null)
        {
            if (Encoding == null) { Encoding = Encoding.ASCII; }

            SetMetaData(metaFilePathOrDirectory);  
            SetProperties();
            InitStreamWriter(Stream, Encoding); 
        }

        /// <summary>
        /// 设置数据流
        /// </summary>
        protected virtual void InitStreamWriter(Stream Stream, Encoding Encoding)
        {
            Encoding = System.Text.Encoding.ASCII;
            StreamWriter = new StreamWriter(Stream, Encoding);
            this.CurrentIndex = -1;
            FinalInit();
        }
        /// <summary>
        /// 最后的初始化，在所有初始化执行之后
        /// </summary>
        public virtual void FinalInit()
        {

        }

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
        private void SetMetaData(string metaFilePathOrDirectory)
        {
            if (FilePath != null)
            {
                this.BaseDirectory = Path.GetDirectoryName(FilePath);
                var extention = Path.GetExtension(FilePath);

                if (String.IsNullOrWhiteSpace(metaFilePathOrDirectory))
                {
                    var dir = Path.GetDirectoryName(FilePath);
                    var metaName = extention.Replace(".", "") + ".gmeta";
                    metaFilePathOrDirectory = Path.Combine(dir, metaName);
                }
            }

            if (File.Exists(metaFilePathOrDirectory))
            {
                this.Metadata = new GmetaReader(metaFilePathOrDirectory).Read();
            }
            else
            {
                this.Metadata = GetDefaultMetadata();
            } ;
        }
        #endregion

        protected virtual void SetProperties()
        {
            this.EntityType = typeof(T);
            Properties = new List<System.Reflection.PropertyInfo>();

            var PropertyNames = Metadata.PropertyNames;


            //采用默认序列
            if (PropertyNames.Length == 0)
            {
                if (Activator.CreateInstance<T>().OrderedProperties != null && Activator.CreateInstance<T>().OrderedProperties.Count > 0)
                {
                    PropertyNames = Activator.CreateInstance<T>().OrderedProperties.ToArray();
                }
                else
                {
                    PropertyNames = Geo.Utils.ObjectUtil.GetPropertyNames(EntityType).ToArray();
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (var item in PropertyNames)
            {
                Properties.Add(EntityType.GetProperty(item.Trim()));
            }
        }

       #endregion

        #region 属性 
        /// <summary>
        /// 字符编码
        /// </summary>
        public Encoding Encoding { get; set; }
        /// <summary>
        /// 当前编号
        /// </summary>
        public int CurrentIndex { get; set;  }
        /// <summary>
        /// 数据流阅读器
        /// </summary>
        public StreamWriter StreamWriter { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 基础目录，当前文件的目录。
        /// </summary>
        public string BaseDirectory { get; set; }

        /// <summary>
        /// 元数据，描述数据的数据。
        /// 应该包含 Name X Y Z
        /// </summary>
        public Gmetadata Metadata { get; set; }
        /// <summary>
        /// 对象类型
        /// </summary>
        public Type EntityType { get; protected set; }
        /// <summary>
        /// 要写入XML的属性。
        /// </summary>
        protected List<System.Reflection.PropertyInfo> Properties { get; set; }
        /// <summary>
        /// 属性名称，用于写字头部
        /// </summary>
        protected List<string> PropertieNames { get; set; }
        #endregion

        /// <summary>
        /// 写入一行注释
        /// </summary>
        /// <param name="comment"></param>
        public abstract void WriteCommentLine(string comment);

        /// <summary>
        /// 在当前行追加注释
        /// </summary>
        /// <param name="comment"></param>
        public abstract  void AppendComment(string comment);
        /// <summary>
        /// 写入对象
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Write(Object obj)
        {
            if (obj is IEnumerable<T>)
            {
                var list = obj as IEnumerable<T>;
                Write(list);
            }
            else
            {
                Write((T)obj);
            }
        }
        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Write(IEnumerable< T> objs)
        {
            foreach (var item in objs)
            {
                this.Write(item);
            }
        }
        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="obj"></param>
        public abstract  void Write(T obj);
         

        #region 核心转换


        /// <summary>
        /// 获取默认的元数据。没有排序。
        /// </summary>
        /// <returns></returns>
        public virtual Gmetadata GetDefaultMetadata()
        {
            Gmetadata data = Gmetadata.NewInstance;

            //var names = Utils.ObjectUtil.GetPropertyNames(typeof(TLineClass));

            //foreach (var key in names)
            //{
            //    data.PropertyNames = names.ToArray();
            //}
            return data;
     
        }
       
        #endregion

        public void Flush()
        {
            if (this.StreamWriter != null)
            {
                this.StreamWriter.Flush(); 
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            Close();
        }
        /// <summary>
        /// 关闭
        /// </summary>
        public virtual void Close()
        {
            if (this.StreamWriter != null)
            {
                this.StreamWriter.Close();
                this.StreamWriter.Dispose();
            }
        }
    }
}
