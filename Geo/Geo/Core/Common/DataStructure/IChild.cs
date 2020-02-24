//2016.02.20,czs, create in hongqing, 建立孩子接口

    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations; 
    using System.Data.Entity;
    using System.Globalization;
    using Geo;
    using Geo.Utils; 
namespace Geo
{
    
 
    /// <summary>
    /// 这是一个孩子，可能有父亲
    /// </summary>
    public interface IChild<TParentKey>
    {
        /// <summary>
        /// 父亲节点的
        /// </summary>
         TParentKey ParentId { get; set; }
    }
    /// <summary>
    /// 具有 ParentId 属性的一个节点，ParentId 属性可为Null。
    /// </summary>
    public interface IChildOfNullableInt : IChild<int?>
    {

    }

}