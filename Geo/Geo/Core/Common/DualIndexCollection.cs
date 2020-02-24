//2017.06.14, czs, create in hongqing, FCB 数据服务

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Algorithm;
using Geo.Utils;

namespace Geo
{
    //2017.06.14, czs, create in hongqing, 双检索数据结构
    /// <summary>
    /// 双检索数据结构。提供快速查询服务。
    /// </summary>
    /// <typeparam name="TKeyA"></typeparam>
    /// <typeparam name="TKeyB"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class DualIndexCollection<TKeyA, TKeyB, TValue>
    {
        /// <summary>
        /// 双检索数据结构
        /// </summary>
        public DualIndexCollection()
        {
            this.DataA = new Dictionary<TKeyA, Dictionary<TKeyB, TValue>>();
            this.DataB = new Dictionary<TKeyB, Dictionary<TKeyA, TValue>>();
        }

        /// <summary>
        /// 数据 A
        /// </summary>
        protected Dictionary<TKeyA, Dictionary<TKeyB, TValue>> DataA { get; set; }
        /// <summary>
        /// 数据 B
        /// </summary>
        protected Dictionary<TKeyB, Dictionary<TKeyA, TValue>> DataB { get; set; }
        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            this.DataA.Clear();
            this.DataB.Clear();
        }

        /// <summary>
        /// 添加，如果已有则覆盖。
        /// </summary>
        /// <param name="keyA"></param>
        /// <param name="keyB"></param>
        /// <param name="val"></param>
        public void Add(TKeyA keyA, TKeyB keyB, TValue val)
        {
            if (!DataA.ContainsKey(keyA)) { DataA[keyA] = new Dictionary<TKeyB, TValue>(); }
            if (!DataB.ContainsKey(keyB)) { DataB[keyB] = new Dictionary<TKeyA, TValue>(); }

            DataA[keyA][keyB] = val;
            DataB[keyB][keyA] = val;
        }
        /// <summary>
        /// 包括否
        /// </summary>
        /// <param name="keyA"></param>
        /// <param name="keyB"></param>
        /// <returns></returns>
        public bool Contains(TKeyA keyA, TKeyB keyB)
        {
            return DataA.ContainsKey(keyA) && DataB.ContainsKey(keyB);
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="keyA"></param>
        /// <param name="keyB"></param>
        /// <returns></returns>
        public TValue Get(TKeyA keyA, TKeyB keyB)
        {
            if (!Contains(keyA, keyB)) { return default(TValue); }
            return DataA[keyA][keyB];
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="keyA"></param>
        /// <returns></returns>
        public Dictionary<TKeyB, TValue> Get(TKeyA keyA)
        {
            if (DataA.ContainsKey(keyA)) return DataA[keyA];
            return new Dictionary<TKeyB, TValue>();
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="keyB"></param>
        /// <returns></returns>
        public Dictionary<TKeyA, TValue> Get(TKeyB keyB)
        {
            if (DataB.ContainsKey(keyB)) return DataB[keyB];
            return new Dictionary<TKeyA, TValue>();
        }
    }




}