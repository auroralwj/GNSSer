//2015.10.20, czs, create in xi'an hongqing, Gnsser 行文件写入通用接口
//2015.11.07, czs, create in 达州火车站华莱士, 提取对象读写通用接口


using System;

namespace Geo.IO
{ 
    /// <summary>
    /// 写入器。
    /// </summary>
    public interface IEntityWriter
    {
        /// <summary>
        /// 初始化，等同于构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        //void Init(string filePath, Gmetadata Gmetadata);

        /// <summary>
        /// 初始化，等同于构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePathOrDirectory"></param>
        /// <returns></returns>
        void Init(string filePath, string metaFilePathOrDirectory = null);

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="obj"></param>
        void Write(Object obj);

        /// <summary>
        /// 关闭数据流
        /// </summary>
        void Close();
    }

    /// <summary>
    /// 行对象文件读取器接口。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntityWriter<T> : IEntityWriter, IBaseDirecory
         where T : IOrderedProperty
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        string FilePath { get; set; }
        /// <summary>
        /// 文件元数据
        /// </summary>
        global::Geo.IO.Gmetadata Metadata { get; set; }
        /// <summary>
        /// 对象类型
        /// </summary>
        Type EntityType { get; }
        /// <summary>
        /// 读取方法
        /// </summary>
        /// <param name="obj"></param>
        void Write(T obj);
  
    }
    /// <summary>
    /// 写入器。
    /// </summary>
    public interface ILineFileWriter : IEntityWriter
    { 
    }

    /// <summary>
    /// 行对象文件读取器接口。
    /// </summary>
    /// <typeparam name="TLineClass"></typeparam>
    public interface ILineFileWriter<TLineClass> : ILineFileWriter, IBaseDirecory
         where TLineClass : IOrderedProperty
    {
        
  
    }
}
