//2014.10.26，czs, create in numu, 具体的改正数表

using System;
using System.Text;
using System.Collections.Generic;
using Geo.Utils;

namespace Geo.Correction
{
    /// <summary>
    /// 详细的改正数表抽象类。
    /// </summary>
    public abstract class AbstractCorrectionDic<TCorrection> : ICorrectionDic<TCorrection>, IToTabRow
    {
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public AbstractCorrectionDic()
        {
            this.Corrections = new Dictionary<string, TCorrection>();
            this.FormatProvider = new NumeralFormatProvider();
        }
        /// <summary>
        /// 格式器
        /// </summary>
        public IFormatProvider FormatProvider { get; set; }
        /// <summary>
        /// 详细的改正数表。
        /// </summary>
        public Dictionary<string, TCorrection> Corrections { get; protected set; }
        /// <summary>
        /// 改正数的数量。
        /// </summary>
        public int Count { get { return this.Corrections.Count; } }

        /// <summary>
        /// 是否包含改正数。
        /// </summary>
        /// <param name="type">改正数类型</param>
        /// <returns></returns>
        public bool ContainsCorrection(string type)
        {
            return this.Corrections.ContainsKey(type);
        }
        /// <summary>
        /// 获取改正数。
        /// </summary>
        /// <param name="type">改正数类型</param> 
        public TCorrection GetCorrection(string type)
        {
            if (ContainsCorrection(type))
            {
                return this.Corrections[type];
            }
            return default(TCorrection);
        }
        /// <summary>
        /// 添加一个改正数。
        /// </summary>
        /// <param name="type">改正数类型</param>
        /// <param name="correction">改正数</param>
        public void SetCorrection(string type, TCorrection correction)
        {
            this.Corrections[type] = (correction);
        }
        /// <summary>
        /// 添加一个改正数。
        /// </summary>
        /// <param name="type">改正数类型</param>
        /// <param name="correction">改正数</param>
        public void AddCorrection(string type, TCorrection correction)
        {
            this.Corrections.Add(type, correction);
        }
        /// <summary>
        /// 添加改正数
        /// </summary>
        /// <param name="corrections"></param>
        public void AddCorrection(Dictionary<string, TCorrection> corrections)
        { 
            foreach (var item in corrections)
            {
                this.AddCorrection(item.Key, item.Value);
            }
        }
        /// <summary>
        /// 添加一个改正数。
        /// </summary>
        /// <param name="corrections">改正数集合</param> 
        public void SetCorrection(Dictionary<string, TCorrection> corrections)
        {
            foreach (var item in corrections)
            {
                this.SetCorrection(item.Key, item.Value);
            }
        }
        /// <summary>
        /// 改正数的集合。
        /// </summary>
        public abstract TCorrection TotalCorrection{get;}

        /// <summary>
        /// 清理完所有的改正数。
        /// </summary>
        public virtual void ClearCorrections()
        {
            this.Corrections.Clear();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var item in this.Corrections)
            {
                sb.Append(item.Key + ":" + item.Value );
                if( i != this.Count - 1)  sb.Append(",");
                i++;
            } 

            return sb.ToString();
        }

        /// <summary>
        /// 以Tab分开的项。
        /// </summary>
        /// <returns></returns>
        public virtual string GetTabValues()
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var item in this.Corrections)
            {
                sb.Append(String.Format( this.FormatProvider, "{0}", item.Value ));
               
                if( i != this.Count - 1)  sb.Append("\t");
                i++;
            } 
            return sb.ToString();
        }

        /// <summary>
        /// Tab 分开项目的标题
        /// </summary>
        /// <returns></returns>
        public string GetTabTitles()
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var item in this.Corrections)
            {
                sb.Append(item.Key);
                if (i != this.Count - 1) sb.Append("\t");
                i++;
            } 
            return sb.ToString();
        }
    }    
}
