//2016.10.17.13, czs, create in hongqing, 注册统计器

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
   
    /// <summary>
    /// 注册统计器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Register<T> : IEnumerable<T>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Register()
        {
            this.data = new HashSet<T>();
        }
        HashSet<T> data { get; set; }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Regist(T t)
        {
            return data.Add(t);
        }
        /// <summary>
        /// 批量注册
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public void Regist(IEnumerable< T> list)
        {
            foreach (var item in list)
            {
                 data.Add(item);
            }
        }
        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Contains(T t) { return data.Contains(t); }

        public IEnumerator<T> GetEnumerator()
        {
            var list = data.ToList();
            list.Sort();
            return list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
