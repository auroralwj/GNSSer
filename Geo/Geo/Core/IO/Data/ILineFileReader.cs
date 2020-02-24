//2015.09.28, czs, create in xi'an hongqing, Gnsser文件读取通用接口
//2015.11.07, czs, edit in 达州火车站华莱士, 提取对象读写通用接口

using System;
using Geo.Common;
using Geo.Coordinates;
using System.Collections;
using System.Collections.Generic;


namespace Geo.IO
{ 

    /// <summary>
    /// 非泛型行读取接口，主要用于初始化
    /// </summary>
    public interface ILineFileReader : IEntityFileReader
    { 
    }

    /// <summary>
    /// Gnsser文件读取通用接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILineFileReader<T> : ILineFileReader, IEnumer<T>, IBaseDirecory
        where T : IOrderedProperty
    { 
         
    }
}
