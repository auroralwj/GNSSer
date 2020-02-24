//2015.01.18, czs, create in namu,  配置文件内容
//2018.03.19, czs, edit in hmx,  与分组配置文件合并，抽象为通用设置数据 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Common;
using System.ComponentModel.DataAnnotations;
//using EF::System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;

namespace Geo.IO
{  
    /// <summary>
    /// 具有指定类型的项目
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class TypedConfigItem<TKey, TValue> : IComparable<TypedConfigItem<TKey, TValue>>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="group"></param>
        /// <param name="Comment"></param>
        public TypedConfigItem(TKey Name, TValue Value, string group = "Default", string Comment = "")
        {
            this.Name = Name;
            this.Value = Value;
            this.Group = group;
            this.Comment = Comment;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="group"></param>
        /// <param name="Comment"></param>
        public TypedConfigItem(TKey Name, string Value, string group = "Default", string Comment = "")
        {
            this.Name = Name;
            this.Value = ParseString( Value, Name);
            this.Group = group;
            this.Comment = Comment;
        }


        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "名称")]
        public TKey Name { get; set; }
        /// <summary>
        /// 分组。或者Tag标签，以逗号","分隔,表示支持多种分组。
        /// </summary>
        [Display(Name = "分组")]
        public string Group { get; set; }

        /// <summary>
        /// 值
        /// </summary>

        [Display(Name = "值")]
        public TValue Value { get; set; }

        /// <summary>
        /// 注释
        /// </summary>
        [Display(Name = "注释")]
        public string Comment { get; set; }

        /// <summary>
        /// 标准参数名称，去掉了敏感字符
        /// </summary>
        public string StandardName { get { return Geo.Utils.StringUtil.GetStandardName(Name.ToString()); } }

        /// <summary>
        /// 相等与否
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            TypedConfigItem<TKey,TValue> item = obj as TypedConfigItem<TKey, TValue>;
            if (item == null) return false;

            return item.Name.Equals(this.Name) && item.Value .Equals( this.Value);
        }
        /// <summary>
        /// 哈希码
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode() * 5 + this.Value.GetHashCode() * 13;
        }
        /// <summary>
        /// 字符串显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + "=" + Value + " #" + Comment;
        }
        /// <summary>
        /// 解析字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public abstract  TValue ParseString(string str, TKey Name);

        /// <summary>
        /// 比较,默认以分组进行比较。
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(TypedConfigItem<TKey, TValue> other)
        {
            return  this.Name.ToString().CompareTo(other.Name.ToString());
        }

        #region  数据源，暂时不推荐使用后续采用更好的办法。 2018.03.19, czs, 
        /// <summary>
        /// 懒加载
        /// </summary>
        Dictionary<int, string> sources { get; set; }

        /// <summary>
        /// 获取数据字典
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetKeyValueSources()
        {
            if (sources == null)
                sources = ParseKeyValueSources(Comment);
            return sources;
        }

        /// <summary>
        /// 是否自带选项
        /// </summary>
        public bool HasSources { get { return GetKeyValueSources().Count > 0; } }

        /// <summary>
        /// 解析字符串选项，eg.
        /// (0:single,1:dgps,2:kinematic,3:static,4:movingbase,5:fixed,6:ppp-kine,7:ppp-static)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public Dictionary<int, string> ParseKeyValueSources(string source)
        {
            Dictionary<int, string> sourceItems = new Dictionary<int, string>();

            if (source.Contains(","))
            {
                source = source.Replace("(", "").Replace(")", "");
                string[] pairs = source.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var pair in pairs)
                {
                    var names = pair.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (names.Length < 2) continue;

                    sourceItems.Add(int.Parse(names[0]), names[1].Trim());
                }
            }
            return sourceItems;
        }
        #endregion
    }
     
}
