//2015.09.28, czs, create in xi'an hongqing, Gnsser文件读取通用接口
//2015.11.07, czs, create in 达州火车站华莱士, 提取对象读写通用接口
//2016.02.10, czs, edit in hongqing, 继承自 ITableFileReader

using System;
using Geo.Common;
using System.Data;
using Geo.Coordinates;
using System.Collections;
using System.Collections.Generic;

namespace Geo.IO
{

    /// <summary>
    /// 非泛型读取接口，主要用于初始化
    /// </summary>
    public interface IEntityFileReader :  IEnumerable, ITableFileReader
    {
        /// <summary>
        /// 初始化，等同于构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Gmetadata"></param>
        void Init(string filePath, Gmetadata Gmetadata);
        /// <summary>
        /// 初始化，等同于构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="metaFilePathOrDirectory"></param>
        /// <returns></returns>
        void Init(string filePath, string metaFilePathOrDirectory = null);
    }

    /// <summary>
    /// Gnsser对象读取通用接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntityFileReader<T> : IEntityFileReader, IEnumer<T>, IBaseDirecory
        where T : IOrderedProperty
    { 
        /// <summary>
        /// 当前对象
        /// </summary>
        T Current { get; set; }
        /// <summary>
        /// 索引
        /// </summary>
        int CurrentIndex { get; }
        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<T> Read(int startIndex, int count = 1);
        /// <summary>
        /// 读取所有
        /// </summary>
        /// <returns></returns>
        List<T> ReadAll();
        /// <summary>
        /// 读取指定索引
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        T ReadAt(int index);
    }
     
}
