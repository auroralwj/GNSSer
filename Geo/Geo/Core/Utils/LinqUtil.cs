//2015.06.27, czs, create in namu, Linq 工具

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;  
using System.Text;

namespace Geo.Utils
{
    /// <summary>
    /// Linq 工具
    /// </summary>
    public class LinqUtil
    {
        /// <summary>
        /// 获取属性名称
        /// 2.	Response.Write(GetPropertyName &gt; TestClass (info= &gt;info.ID)) ; //输出的是 "ID" 两字母  
        /// 3.	Response.Write(GetPropertyName &gt; TestClass   &gt;(info= &gt;info. Name)) ; //输出的是 "Name" 四个字母  
        /// 4.	Response.Write(GetPropertyName &gt; TestClass  &gt; (info= &gt;info)) ; //输出的是 "TestClass" 九个字母 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static string GetPropertyName<T>(Expression<Func<T, object>> expr)
        {
            var rtn = "";
            if (expr.Body is UnaryExpression)
            {
                rtn = ((MemberExpression)((UnaryExpression)expr.Body).Operand).Member.Name;
            }
            else if (expr.Body is MemberExpression)
            {
                rtn = ((MemberExpression)expr.Body).Member.Name;
            }
            else if (expr.Body is ParameterExpression)
            {
                rtn = ((ParameterExpression)expr.Body).Type.Name;
            }
            return rtn;
        }

    }
}
