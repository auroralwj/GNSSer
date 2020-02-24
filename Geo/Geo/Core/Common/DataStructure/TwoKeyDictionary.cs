//2017.07.22, czs, create in hongqing, 双键字典

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 双键字典,采用双字典快速检索方法。
    /// </summary>
    public class TwoKeyDictionary<TKeyA, TKeyB, TValue>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TwoKeyDictionary()
        {
            DataA = new Dictionary<TKeyA, Dictionary<TKeyB, TValue>>();
            DataB = new Dictionary<TKeyB, Dictionary<TKeyA, TValue>>();
        }

        #region 属性
        #region 核心属性
        /// <summary>
        /// 数据字典A
        /// </summary>
        private Dictionary<TKeyA, Dictionary<TKeyB, TValue>> DataA { get; set; }
        /// <summary>
        /// 数据字典B
        /// </summary>
        private Dictionary<TKeyB, Dictionary<TKeyA, TValue>> DataB { get; set; }
        #endregion

        /// <summary>
        /// B 键集合
        /// </summary>
        public List<TKeyB> KeyBs { get { return new List<TKeyB>(this.DataB.Keys); } }
        /// <summary>
        ///  A 键集合
        /// </summary>
        public List<TKeyA> KeyAs { get { return new List<TKeyA>(this.DataA.Keys); } }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get { return this.DataA.Count; } }
        /// <summary>
        /// 检索直接获取
        /// </summary>
        /// <param name="keyA"></param>
        /// <param name="keyB"></param>
        /// <returns></returns>
        public TValue this[TKeyA keyA, TKeyB keyB]
        {
            get { return DataA[keyA][ keyB]; }
            set { Set(keyA, keyB, value); }
        }
        /// <summary>
        /// 检索器
        /// </summary>
        /// <param name="keyA"></param>
        /// <returns></returns>
        public Dictionary<TKeyB, TValue> this[TKeyA keyA] { get { return GetValuesByKeyA(keyA); } }
        /// <summary>
        /// 检索器
        /// </summary>
        /// <param name="keyB"></param>
        /// <returns></returns>
        public Dictionary<TKeyA, TValue> this[TKeyB keyB] { get { return GetValuesByKeyB(keyB); } }
        #endregion

        #region 方法
        #region  数据管理方法
        /// <summary>
        /// 是否包含键值A，且A集合中也包含B
        /// </summary>
        /// <param name="keyA"></param>
        /// <param name="keyB"></param>
        /// <returns></returns>
        public bool ContainsKeyAB(TKeyA keyA, TKeyB keyB) { return DataA.ContainsKey(keyA) && DataA[keyA].ContainsKey(keyB); }
        /// <summary>
        /// 是否包含键值B，且B集合中也包含A
        /// </summary>
        /// <param name="keyA"></param>
        /// <param name="keyB"></param>
        /// <returns></returns>
        public bool ContainsKeyBA(TKeyB keyB, TKeyA keyA) { return DataB.ContainsKey(keyB) && DataB[keyB].ContainsKey(keyA); ; }
        /// <summary>
        /// 是否包含键值
        /// </summary>
        /// <param name="keyA"></param>
        /// <returns></returns>
        public bool ContainsKeyA(TKeyA keyA) { return this.DataA.ContainsKey(keyA); }
        /// <summary>
        /// 是否包含键值
        /// </summary>
        /// <param name="keyB"></param>
        /// <returns></returns>
        public bool ContainsKeyB(TKeyB keyB) { return this.DataB.ContainsKey(keyB); }
        /// <summary>
        /// 循环，默认从 A 键开始
        /// </summary>
        /// <param name="action"></param>
        public void ForEach(Action<TKeyA, TKeyB, TValue> action) { ForEachKeyAFirst(action); }
        /// <summary>
        /// 循环，从A键开始
        /// </summary>
        /// <param name="action"></param>
        public void ForEachKeyAFirst(Action<TKeyA, TKeyB, TValue> action)
        {
            foreach (var kvA in DataA)
            {
                foreach (var kv in kvA.Value)
                {
                    action(kvA.Key, kv.Key, kv.Value);
                }
            }
        }
        /// <summary>
        /// 循环，从B键开始
        /// </summary>
        /// <param name="action"></param>
        public void ForEachKeyBFirst(Action<TKeyA, TKeyB, TValue> action)
        {
            foreach (var kvB in DataB)
            {
                foreach (var kv in kvB.Value)
                {
                    action(kv.Key, kvB.Key, kv.Value);
                }
            }
        }
        /// <summary>
        /// 数据获取从A键
        /// </summary>
        /// <param name="keyA"></param>
        /// <returns></returns>
        public Dictionary<TKeyB, TValue> GetValuesByKeyA(TKeyA keyA) { return DataA[keyA]; }
        /// <summary>
        /// 数据获取，从B键
        /// </summary>
        /// <param name="keyB"></param>
        /// <returns></returns>
        public Dictionary<TKeyA, TValue> GetValuesByKeyB(TKeyB keyB) { return DataB[keyB]; }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="keyA"></param>
        /// <param name="keyB"></param>
        /// <param name="val"></param>
        public void Set(TKeyA keyA, TKeyB keyB, TValue val)
        {
            if (!this.ContainsKeyA(keyA)) { this.DataA[keyA] = new Dictionary<TKeyB, TValue>(); }
            if (!this.ContainsKeyB(keyB)) { this.DataB[keyB] = new Dictionary<TKeyA, TValue>(); }

            this.DataA[keyA][keyB] = val;
            this.DataB[keyB][keyA] = val;
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="keyA"></param>
        /// <param name="keyB"></param>
        /// <returns></returns>
        public TValue Get(TKeyA keyA, TKeyB keyB)
        {
            if (ContainsKeyAB(keyA, keyB))
            {
                return DataA[keyA][keyB];
            }
            return default(TValue);
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="keyA"></param>
        /// <param name="keyB"></param>
        /// <returns></returns>
        public bool Remove(TKeyA keyA, TKeyB keyB)
        {
            bool result = false;
            if (ContainsKeyAB(keyA, keyB))
            {
                result = true;
                var valA = DataA[keyA];
                valA.Remove(keyB);

                var valB = DataB[keyB];
                valB.Remove(keyA);
            }
            return result;
        }
        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            this.DataA.Clear();
            this.DataB.Clear();
        }
        #endregion

        #region  转换
        /// <summary>
        /// 数据表
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="isKeyAsKey"></param>
        /// <returns></returns>
        public ObjectTableStorage GetTable(string keyName = "Key", bool isKeyAsKey = true)
        {
            ObjectTableStorage table = new ObjectTableStorage();
            if (isKeyAsKey)
                foreach (var kvA in DataA)
                {
                    table.NewRow();
                    table.AddItem(keyName, kvA.Key);
                    var dic = new Dictionary<string, Object>();
                    foreach (var kv in kvA.Value)
                    {
                        dic[kv.Key.ToString()] = kv.Value;
                    }
                    table.AddItem(dic);
                }
            else foreach (var kvB in DataB)
                {
                    table.NewRow();
                    table.AddItem(keyName, kvB.Key);

                    var dic = new Dictionary<string, Object>();
                    foreach (var kv in kvB.Value)
                    {
                        dic[kv.Key.ToString()] = kv.Value;
                    }
                    table.AddItem(dic);
                }
            return table;

        }
        #endregion

        #endregion
    }
}
