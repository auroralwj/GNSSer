 //2017.07.22, czs, create in hongqing, 双键表
//2017.10.09, czs, edit in hongqing, 提取键解析方法

using System.Runtime.InteropServices;
using System.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Diagnostics;


namespace Geo
{
    //2017.10.15, czs, create in hongqing, 双数字键表
    /// <summary>
    /// 双数字键表
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class NumeralKeyTable<TValue> : TwoKeyTable<double, double, TValue>
    {
        /// <summary>
        /// 双数字键表
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="keyA"></param>
        /// <param name="keyB"></param>
        public NumeralKeyTable(ObjectTableStorage Table, string keyA, string keyB):base(Table, keyA, keyB)
        { 
        } 
    } 

    /// <summary>
    /// 双键表
    /// </summary>
    public abstract class  TwoKeyTable<TKeyA, TKeyB, TValue>{
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="keyA"></param>
        /// <param name="keyB"></param>
        public TwoKeyTable(ObjectTableStorage Table, string keyA, string keyB)
        {
            this.KeyA = keyA;
            this.KeyB = keyB;
            this.Table = Table;
        }
        /// <summary>
        /// 数据表
        /// </summary>
        public ObjectTableStorage Table { get; set; }
        /// <summary>
        /// 键值A
        /// </summary>
        public string KeyA { get; set; }
        /// <summary>
        ///  键值B
        /// </summary>
        public string KeyB { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public TwoKeyDictionary<TKeyA, TKeyB, TValue> Data { get; set; }
        /// <summary>
        /// 在此完成一些初始化工作。主要包括表的解析。
        /// </summary>
        public virtual void Init()
        {
            this.Data = ParseTable();
        }

        /// <summary>
        /// 解析表
        /// </summary>
        /// <returns></returns>
        public virtual TwoKeyDictionary<TKeyA, TKeyB, TValue> ParseTable()
        {
           var Data = new TwoKeyDictionary<TKeyA, TKeyB, TValue>();

           foreach (var item in Table.BufferedValues)
           {
               var keyAData = item[KeyA];
               var keyBData = item[KeyB];
               var keyAObj = ParseKeyA(keyAData);
               var keyBObj = ParseKeyB(keyBData);
               var val  =RowToValue(item);
               Data.Set(keyAObj, keyBObj, val);
           }
           return Data;
        }
        /// <summary>
        /// 解析键值B
        /// </summary>
        /// <param name="keyBData"></param>
        /// <returns></returns>
        public virtual TKeyB ParseKeyB(object keyBData)
        {
            var keyBObj = (TKeyB)Convert<TKeyB>(keyBData);
            return keyBObj;
        }
        /// <summary>
        /// 解析键值A 
        /// </summary>
        /// <param name="keyAData"></param>
        /// <returns></returns>
        public virtual TKeyA ParseKeyA(object keyAData)
        {
            var keyAObj = (TKeyA)Convert<TKeyA>(keyAData);
            return keyAObj;
        }
        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public abstract TValue RowToValue(Dictionary<string, Object> row);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="keyA"></param>
        /// <param name="keyB"></param>
        /// <returns></returns>
        public virtual TValue Get(TKeyA keyA, TKeyB keyB){
            return Data.Get(keyA, keyB);
        }
        /// <summary>
        /// 类型 转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Object Convert<T>(Object obj)
        {
            var type = typeof(T);
            if (type == typeof(String))
            {
                if (obj is String) { return obj; }
                return (obj + "");
            }
            if (type == typeof(Double))
            {
                if (obj is Double) { return obj; }
                return Geo.Utils.StringUtil.ParseDouble(obj + "");
            }
            if (type == typeof(Int32))
            {
                if (obj is Int32) { return obj; }
                return Geo.Utils.StringUtil.ParseInt((object)(obj + ""));
            }
            if (type == typeof(DateTime))
            {
                if (obj is DateTime) { return obj; }
                return DateTime.Parse(obj + "");
            }
            if (type == typeof( Geo.Times.Time))
            {
                if (obj is Geo.Times.Time) { return obj; }
                return Geo.Times.Time.Parse(obj + "");
            }

            return obj;            
        }

    }
}
