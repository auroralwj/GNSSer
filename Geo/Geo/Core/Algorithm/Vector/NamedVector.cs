// 2014.09.10, czs, create in 海鲁吐， 具有名称的矩阵
//2019.02.15, czs, edit in hongqing, 增加操作数等

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Common;
using System.Collections; 
using System.Linq; 
using Geo.IO;

namespace Geo.Algorithm
{
    /// <summary>
    /// 元素具有名称的向量。每一个元素都有一个名词。
    /// 矩阵行列转换类。通过名称转换。
    /// 代表一个名称的行。
    /// </summary>
    public class NamedVector :AbstractVector,  IEnumerable<KeyValuePair<string, double>>
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="DefaultValue">默认非对角线数值</param>
        /// <param name="DefaultDiagonalValue">默认对角线数值的平方根</param>
        public NamedVector(string name="NoName", double DefaultValue = 0, double DefaultDiagonalValue = 1E10)
        {
            this.Name = name;
            this.Data = new Dictionary<string, double>();
            this.DefaultDiagonalValue = Math.Pow( DefaultDiagonalValue, 2);
            this.DefaultValue = DefaultValue;
        }

        /// <summary>
        /// 默认对角线数字
        /// </summary>
        public double DefaultDiagonalValue { get; protected set; }
        /// <summary>
        /// 默认数
        /// </summary>
        public double DefaultValue { get; protected set; }

        /// <summary>
        /// 名称。行列名称。
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 内部存储核心数据结构。
        /// </summary>
        private Dictionary<string, double> Data { get; set; }
         
        /// <summary>
        /// 键集合
        /// </summary>
        public List<string> Keys => Data.Keys.ToList();

        public override List<string> ParamNames { get => Keys; set =>throw new Exception("不可直接设置"); }


        /// <summary>
        ///索引器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override double this[string key]
        {
            get { return Data[key]; }
            set { Data[key] = value; }
        }
        /// <summary>
        /// 移除一个
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            Data.Remove(key);
        }
        /// <summary>
        /// 批量移除
        /// </summary>
        /// <param name="keys"></param>
        public void Remove(IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {
                Data.Remove(key);
            }
        }
        /// <summary>
        /// 设置数值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetValue(string name, double value)
        {
            Data[name] = value;
        }
        /// <summary>
        /// 获取数值，如果没有这返回默认。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public double GetValue(string name)
        {
            if (Data.ContainsKey(name)) return Data[name];
            if (this.Name.Equals(name)) return DefaultDiagonalValue;
            return DefaultValue;
        }

        public IEnumerator<KeyValuePair<string, double>> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Count: " + Count.ToString() + " ");
            int i = 0;
            foreach (var item in this)
            {
                if (i > 0) { sb.Append(", "); }
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

        public override IVector Create(int count)
        {
            return new NamedVector();
        }

        public override IVector Create(double[] array)
        {
            var vec = new NamedVector();
            int i = 0;
            foreach (var item in array)
            {
                vec[i++] = item;
            }
            return vec;
        }


        public override object Clone()
        {
            NamedVector result = new NamedVector();
            foreach (var item in this.Data)
            {
                result.Add(item.Key, item.Value);
            }
            return result;
        }

        public void Add(string key, double value)
        {
            this.Data.Add(key, value);
        }



        #region 操作数

        public static NamedVector operator +(NamedVector left, NamedVector right)
        {
            var result = new NamedVector();
            foreach (var item in left.Data)
            {
                if (!right.Data.ContainsKey(item.Key)) { continue; }

                result.Data[item.Key] = item.Value + right.Data[item.Key];
            }
            return result;
        }
        public static NamedVector operator -(NamedVector left, NamedVector right)
        {
            var result = new NamedVector();
            foreach (var item in left)
            {
                if (!right.Data.ContainsKey(item.Key)) { continue; }

                result[item.Key] = item.Value - right[item.Key];
            }
            return result;
        }
        public static NamedVector operator -(double left, NamedVector right)
        {
            var result = new NamedVector();
            foreach (var item in right)
            {
                result[item.Key] = left - item.Value;
            }
            return result;
        }
        public static NamedVector operator -(NamedVector left, double right)
        {
            var result = new NamedVector();
            foreach (var item in left)
            {
                result[item.Key] = item.Value - right;
            }
            return result;
        }
        public static NamedVector operator *(double left, NamedVector right)
        {
            var result = new NamedVector();
            foreach (var item in right)
            {
                result[item.Key] = left * item.Value;
            }
            return result;
        }
        public static NamedVector operator *(NamedVector left, double right)
        {
            return right * left;
        }
        public static NamedVector operator /(double left, NamedVector right)
        {
            var result = new NamedVector();
            foreach (var item in right)
            {
                result[item.Key] = left / item.Value;
            }
            return result;
        }
        public static NamedVector operator /(NamedVector left, double right)
        {
            var result = new NamedVector();
            foreach (var item in left)
            {
                result[item.Key] = item.Value / right;
            }
            return result;
        }
        #endregion
    }

}