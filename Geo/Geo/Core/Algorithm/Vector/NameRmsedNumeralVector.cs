//2018.12.28, czs, create in ryd, 具有名称的RMS向量

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Geo.IO;

namespace Geo
{

    /// <summary>
    /// 具有名称的RMS向量
    /// </summary>
    public class NameRmsedNumeralVector : NameRmsedNumeralVector<string>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NameRmsedNumeralVector()
        { 
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="vals"></param>
        /// <param name="rmses"></param>
        /// <param name="names"></param>
        public NameRmsedNumeralVector(double[] vals, double[] rmses, string[] names) :base(vals, rmses, names)
        { 
        }
        /// <summary>
        /// 获取四舍五入值，RMS默认为0
        /// </summary>
        /// <returns></returns>
        public new NameRmsedNumeralVector GetRound()
        {
            var result = new NameRmsedNumeralVector ();
            foreach (var item in this.Data)
            {
                result[item.Key] = new RmsedNumeral(Math.Round(item.Value.Value), 1e-20);
            }
            return result;
        }
        #region 操作数

        public static NameRmsedNumeralVector operator +(NameRmsedNumeralVector left, NameRmsedNumeralVector right)
        {
            var result = new NameRmsedNumeralVector();
            foreach (var item in left.Data)
            {
                if (!right.Data.ContainsKey(item.Key)) { continue; }

                result.Data[item.Key] = item.Value + right.Data[item.Key];
            }
            return result;
        }
        public static NameRmsedNumeralVector operator -(NameRmsedNumeralVector left, NameRmsedNumeralVector right)
        {
            var result = new NameRmsedNumeralVector();
            foreach (var item in left)
            {
                if (!right.Data.ContainsKey(item.Key)) { continue; }

                result[item.Key] = item.Value - right[item.Key];
            }
            return result;
        }
        public static NameRmsedNumeralVector operator -(double left, NameRmsedNumeralVector right)
        {
            var result = new NameRmsedNumeralVector();
            foreach (var item in right)
            {
                result[item.Key] = left - item.Value;
            }
            return result;
        }
        public static NameRmsedNumeralVector operator -(NameRmsedNumeralVector left, double right)
        {
            var result = new NameRmsedNumeralVector();
            foreach (var item in left)
            {
                result[item.Key] = item.Value - right;
            }
            return result;
        }
        public static NameRmsedNumeralVector operator *(double left, NameRmsedNumeralVector right)
        {
            var result = new NameRmsedNumeralVector();
            foreach (var item in right)
            {
                result[item.Key] = left * item.Value;
            }
            return result;
        }
        public static NameRmsedNumeralVector operator *(NameRmsedNumeralVector left, double right)
        {
            return right * left;
        }
        public static NameRmsedNumeralVector operator /(double left, NameRmsedNumeralVector right)
        {
            var result = new NameRmsedNumeralVector();
            foreach (var item in right)
            {
                result[item.Key] = left / item.Value;
            }
            return result;
        }
        public static NameRmsedNumeralVector operator /(NameRmsedNumeralVector left, double right)
        {
            var result = new NameRmsedNumeralVector();
            foreach (var item in left)
            {
                result[item.Key] = item.Value / right;
            }
            return result;
        }
        #endregion
    }

    /// <summary>
    /// 具有名称的RMS向量
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class NameRmsedNumeralVector<TKey> : IEnumerable<KeyValuePair<TKey, RmsedNumeral>>
    {
        Log log = new Log(typeof(NameRmsedNumeralVector<TKey>));

        /// <summary>
        /// 构造函数
        /// </summary>
        public NameRmsedNumeralVector()
        {
            this.Data = new Dictionary<TKey, RmsedNumeral>();
        } 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data"></param>
        public NameRmsedNumeralVector(IDictionary<TKey, RmsedNumeral> data)
        {
            this.Data = new Dictionary<TKey, RmsedNumeral>( data);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data"></param>
        public NameRmsedNumeralVector(Dictionary<TKey, double> data)
        {
            this.Data = new Dictionary<TKey, RmsedNumeral>();
            foreach (var item in data)
            {
                this[item.Key] = new RmsedNumeral(item.Value, 1e-20);
            }
        } 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="vals"></param>
        /// <param name="rmses"></param>
        /// <param name="names"></param>
        public NameRmsedNumeralVector(double[] vals, double[] rmses, TKey[] names)
        {
            this.Data = new Dictionary<TKey, RmsedNumeral>();
            for (int i = 0; i < vals.Length; i++)
            {
                Data[names[i]] = new RmsedNumeral(vals[i], rmses[i]);
            }
        }
        /// <summary>
        /// 核心数据
        /// </summary>
        public Dictionary<TKey, RmsedNumeral> Data { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int Count => Data.Count;
        /// <summary>
        /// 键集合
        /// </summary>
        public List<TKey> Keys => Data.Keys.ToList();

        /// <summary>
        ///索引器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public RmsedNumeral this[TKey key]
        {
            get { return Data[key]; }
            set { Data[key] = value; }
        }
        /// <summary>
        /// 移除一个
        /// </summary>
        /// <param name="key"></param>
        public void Remove(TKey key)
        {
            Data.Remove(key);
        }
        /// <summary>
        /// 批量移除
        /// </summary>
        /// <param name="keys"></param>
        public void Remove(IEnumerable<TKey> keys)
        {
            foreach (var key in keys)
            {
                Data.Remove(key); 
            }
        }
        /// <summary>
        /// 获取绝对值大于某数
        /// </summary>
        /// <param name="maxCycleDifferOfIntFloat"></param>
        /// <returns></returns>
        public List<TKey> GetAbsLargerThan(double maxCycleDifferOfIntFloat)
        {
            List<TKey> toRemoves = new List<TKey>();
            foreach (var item in this)
            {
                var paramName = item.Key;
                var intDiffer = Math.Abs(item.Value.Value);
                if (intDiffer > maxCycleDifferOfIntFloat)
                {
                    toRemoves.Add(item.Key);
                    log.Debug("整数与浮点数差 " + intDiffer + " > " + maxCycleDifferOfIntFloat + ",登记移除 " + paramName);
                }
            }
            return toRemoves;
        }
        /// <summary>
        /// RMS 大于
        /// </summary>
        /// <param name="maxRms"></param>
        /// <returns></returns>
        public List<TKey> GetRmsLargerThan(double maxRms)
        {
            List<TKey> toRemoves = new List<TKey>();
            foreach (var item in this)
            {
                var paramName = item.Key;
                var rms = item.Value.Rms;
                if (rms > maxRms)
                {
                    toRemoves.Add(item.Key);
                    log.Debug("RMS  " + rms + " > " + maxRms + ",登记移除 " + paramName);
                }
            }
            return toRemoves;
        }
         

        public WeightedVector GetWeightedVector()
        {
            List<RmsedNumeral> vector = new List<RmsedNumeral>(); 
            List<string> names = new List<string>();
            foreach (var item in this)
            {
                vector.Add(item.Value);
                names.Add(item.Key.ToString());
            }

            WeightedVector vs =   WeightedVector.Parse(vector, names);
            return vs;
        }
        /// <summary>
        /// 获取四舍五入值，RMS默认为0
        /// </summary>
        /// <returns></returns>
        public NameRmsedNumeralVector<TKey> GetRound()
        {
            var result = new NameRmsedNumeralVector<TKey>();
            foreach (var item in this.Data)
            {
                result[item.Key] = new RmsedNumeral(Math.Round(item.Value.Value), 1e-20);
            }
            return result;
        }
        /// <summary>
        /// 内部差分
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public NameRmsedNumeralVector<TKey> Minus(TKey key)
        {
            var baseVal = this[key];
            var result = new NameRmsedNumeralVector<TKey>();
            foreach (var item in this)
            {
                if (item.Key.Equals(key)) { continue; }

                result[item.Key] = item.Value - baseVal;
            }
            return result;
        }

        public IEnumerator<KeyValuePair<TKey, RmsedNumeral>> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        public override string ToString()
        { 
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Count: " + Count.ToString() + " ");
            int i = 0;
            foreach (var item in this)
            {
                if(i > 0) {  sb.Append(", "); } 
                sb.Append(item.Value.ToString("0.0000"));
                i++;
            }
            i = 0;
            sb.AppendLine();
            foreach (var item in this)
            {
                sb.Append(item.Key);
                sb.Append(", ");
            }
            return sb.ToString();
        }

        #region 操作数

        public static NameRmsedNumeralVector<TKey> operator +(NameRmsedNumeralVector<TKey> left, NameRmsedNumeralVector<TKey> right)
        {
            var result = new NameRmsedNumeralVector<TKey>();
            foreach (var item in left.Data)
            {
                if (!right.Data.ContainsKey(item.Key)) { continue; }

                result.Data[item.Key] = item.Value + right.Data[item.Key];
            }
            return result;
        }
        public static NameRmsedNumeralVector<TKey> operator -(NameRmsedNumeralVector<TKey> left, NameRmsedNumeralVector<TKey> right)
        {
            var result = new NameRmsedNumeralVector<TKey>();
            foreach (var item in left)
            {
                if (!right.Data.ContainsKey(item.Key)) { continue; }

                result[item.Key] = item.Value - right[item.Key];
            }
            return result;
        }
        public static NameRmsedNumeralVector<TKey> operator -(double left, NameRmsedNumeralVector<TKey> right)
        {
            var result = new NameRmsedNumeralVector<TKey>();
            foreach (var item in right)
            {
                result[item.Key] = left - item.Value;
            }
            return result;
        }
        public static NameRmsedNumeralVector<TKey> operator -(NameRmsedNumeralVector<TKey> left, double right)
        {
            var result = new NameRmsedNumeralVector<TKey>();
            foreach (var item in left)
            {
                result[item.Key] = item.Value - right;
            }
            return result;
        }
        public static NameRmsedNumeralVector<TKey> operator *(double left, NameRmsedNumeralVector<TKey> right)
        {
            var result = new NameRmsedNumeralVector<TKey>();
            foreach (var item in right)
            {
                result[item.Key] = left * item.Value;
            }
            return result;
        }
        public static NameRmsedNumeralVector<TKey> operator *(NameRmsedNumeralVector<TKey> left, double right)
        {
            return right * left;
        }
        public static NameRmsedNumeralVector<TKey> operator /(double left, NameRmsedNumeralVector<TKey> right)
        {
            var result = new NameRmsedNumeralVector<TKey>();
            foreach (var item in right)
            {
                result[item.Key] = left / item.Value;
            }
            return result;
        }
        public static NameRmsedNumeralVector<TKey> operator /(NameRmsedNumeralVector<TKey> left, double right)
        {
            var result = new NameRmsedNumeralVector<TKey>();
            foreach (var item in left)
            {
                result[item.Key] =  item.Value / right;
            }
            return result;
        }
        #endregion
    }
}