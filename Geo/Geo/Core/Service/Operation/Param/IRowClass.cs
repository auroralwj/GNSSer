//2015.10.09, czs, create in xi'an hongqing, 具有输出参数的接口
//2016.02.10, czs, edit in hongqing, 命名为行类，即有属性顺序的类，如行文件，文件参数等

using System;
using System.Collections.Generic;
using Geo.IO;
using Geo;

namespace Geo
{
    /// <summary>
    /// 顶层行类，即有属性顺序的类，如行文件，文件参数等
    /// </summary>
    public abstract class RowClass : OrderedProperty, IRowClass
    {
         
    } 

    /// <summary>
    /// 顶层行类接口，即有属性顺序的类，如行文件，文件参数等
    /// </summary>
    public interface IRowClass : IOrderedProperty
    {
        
    }

    public interface ITwoStringRow : IRowClass
    {

    }

}
