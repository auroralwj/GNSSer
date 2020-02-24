//2016.02.23, czs, create in hongqing, 具有一个整型标识属性，可以转换为Enum枚举。

using System;



namespace Geo
{
     
    /// <summary>
    /// 具有一个标识属性，可以转换为Enum枚举，主要用于数据库
    /// </summary>
    public interface ITypeIndex 
    {
        int TypeIndex { get; set; }
    }
    /// <summary>
    /// 具有一个整型标识属性，可以转换为Enum枚举，主要用于数据库
    /// </summary>
    public interface INullableTypeIndex
    {
        int ? TypeIndex { get; set; }
    }
}
