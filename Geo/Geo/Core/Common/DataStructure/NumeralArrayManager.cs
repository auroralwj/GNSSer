//2016.08.09, czs, create in fujian yongan, 数组管理器

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Algorithm; 
using Geo.Times;
using Geo.IO;

namespace Geo
{
    //2016.08.19, czs, create in 安徽 黄山 屯溪, 具有编号的数据
    /// <summary>
    /// 具有编号的数据
    /// </summary>
    public class IndexedNumeralArrayManager : IndexedArrayManager<double>
    {
        public override BaseDictionary<int, double> Create(string key)
        {
            return new BaseDictionary<int, double>();
        }
    }

    //2016.08.19, czs, create in 江西上饶火车站, 同时具有平滑数据和原始数据
    /// <summary>
    /// 具有编号的数据
    /// </summary>
    public class IndexedRawSmoothArrayManager : IndexedArrayManager<RawSmoothValue>
    {
    }
    /// <summary>
    /// 具有编号的数据
    /// </summary>
    public class IndexedArrayManager<T> : BaseDictionary<string, BaseDictionary<int, T>>
    {

        public override BaseDictionary<int, T> Create(string key)
        {
            this[key] = new BaseDictionary<int, T>();
            return this[key];
        }

        public List<T> GetList(string key)
        {
            List<T> list = new List<T>();
            var ls = GetOrCreate(key);
            return new List<T>(ls); 
        }
    }

    /// <summary>
    /// 数组管理器
    /// </summary>
    public class NumeralArrayManager : BaseDictionary<string,  List<double>>
    {
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public NumeralArrayManager()
        {

        }
        public override List<double> Create(string key)
        {
            return new List<double>();
        }

        /// <summary>
        /// 增加一个数组
        /// </summary>
        /// <param name="key"></param>
        public void Add(string key) { if (!this.Contains(key)) this.Add(key, new List<double>()); }

        /// <summary>
        /// 返回向量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Vector GetVector(string key)
        {
            return new Vector(this[key]);
        }
    }
}