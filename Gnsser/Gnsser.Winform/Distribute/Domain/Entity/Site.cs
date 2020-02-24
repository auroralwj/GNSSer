//2013.04, czs, create, 基于Bernese的分布式计算
//2015.11.02, czs, edit in K998  成都到西安南列车, 基于Gnsser操作任务的分布式计算   

using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.ComponentModel.DataAnnotations;
using Geo.Times;
using System.IO;
using Geo;
using Gnsser.Api;

namespace Gnsser.Winform {

    /// <summary>
    /// 测站 观测数据
    /// </summary>
    public class Site : RowClass
     {
         /// <summary>
         /// 默认构造函数
         /// </summary>
         public Site() {

             this.OrderedProperties = new List<string>()
            {
                Geo.Utils.ObjectUtil.GetPropertyName<Site>( m=>m.Name ),
                Geo.Utils.ObjectUtil.GetPropertyName<Site>( m=>m.Url ), 
            };
         }

         public Site(string url)
         {
             this.Url = url;
             this.Name = Path.GetFileName(this.Url);
         }

         /// <summary>
         /// 测站名称
         /// </summary>
         [Display(Name = "测站名称")]
         public string Name { get; set; }

         /// <summary>
         /// 测站地址。
         /// </summary>
         [Display(Name = "测站地址")]
         public string Url { get; set; }
              

         #region override
         /// <summary>
         /// 字符串
         /// </summary>
         /// <returns></returns>
         public override string ToString()
         {
             return Name;
         }
         /// <summary>
         /// 如果只是一个同名字符串，也会相等。
         /// </summary>
         /// <param name="obj"></param>
         /// <returns></returns>
         public override bool Equals(object obj)
         {
             //if (obj is Named) 
             //    return Name.Equals(((Named)obj).Name);
             //if (obj is String)
             //    return Name.Equals((obj.ToString()));

             return Name.Equals(obj.ToString());
         }
         /// <summary>
         /// 哈希数
         /// </summary>
         /// <returns></returns>
         public override int GetHashCode()
         {
             return Name.GetHashCode();
         }
         #endregion

     }
}
