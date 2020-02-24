//2015.05.12, czs, create in namu, 字典接口
//2016.11.17, czs, create in hongqing, 线程安全的字典类
//2016.11.18, czs, refactor in hongqing, 提取抽象字典。

using System;
using System.Collections.Generic;
using Geo.Common;
using Geo.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace Geo
{
    /// <summary>
    /// 线程安全字典。具有关键字的数据存储结构。核心存储为字典。属于管理者模式应用。
    /// </summary>
    /// <typeparam name="TKey">关键字</typeparam>
    /// <typeparam name="TValue">值</typeparam>
    public class BaseConcurrentDictionary<TKey, TValue> : AbstractBaseDictionary<TKey, TValue>//, IDisposable
       //  where TKey : IComparable<TKey>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="CreateFunc">创建函数</param>
        public BaseConcurrentDictionary(string name = "", Func<TKey, TValue> CreateFunc=null):base(CreateFunc)
        {
            this.Name = name;
            data = new ConcurrentDictionary<TKey, TValue>();
        }
        /// <summary>
        /// 采用字典数据直接初始化
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="name"></param>
        public BaseConcurrentDictionary(Dictionary<TKey, TValue> dic, string name = "")
        {
            this.Name = name;
            data = new ConcurrentDictionary<TKey, TValue>(dic);
            this.OrderedKeys.AddRange(dic.Keys);
        }
        /// <summary>
        /// 核心数据
        /// </summary>
        protected ConcurrentDictionary<TKey, TValue> data { get; set; }

        /// <summary>
        /// 核心数据返回。
        /// </summary>

        //[Obsolete("请不要直接在此属性上执行添加、删除等操作，直接使用对象方法操作")]
        public override IDictionary<TKey, TValue> Data { get { return this.data; } }

        /// <summary>
        /// 添加，若有保存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public override void Add(TKey key, TValue val)
        {
            if (!this.OrderedKeys.Contains(key)) { this.OrderedKeys.Add(key); }
            data.TryAdd(key, val);
        }

        /// <summary>
        /// 移除一个
        /// </summary>
        /// <param name="key"></param>
        public override void Remove(TKey key)
        {
            TValue val;
            if (data.ContainsKey(key)) data.TryRemove(key, out val);
        }
    }
}
