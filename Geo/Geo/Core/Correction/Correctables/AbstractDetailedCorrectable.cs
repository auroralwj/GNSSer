//2014.10.25, czs, create in numu, 可以改正的对象，对象与改正数不必是同一种类型, 显示改正数的详细情况

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Utils; 
namespace Geo.Correction
{ 
    /// <summary>
    /// 可以改正的对象，对象与改正数不必是同一种类型, 显示改正数的详细情况
    /// </summary>
    public abstract class AbstractDetailedCorrectable<TValue, TCorrection>: 
        AbstractCorrectable<TValue, TCorrection>, 
        IDetailedCorrectable<TValue, TCorrection>
    {
        /// <summary>
        /// 观测量构造函数。
        /// </summary>
        /// <param name="val">改正值</param> 
        public AbstractDetailedCorrectable(TValue val)
        :base (val)
        {
            this.Corrections = new Dictionary<string, TCorrection>(); 
        }

        /// <summary>
        /// 所有改正数之和。
        /// </summary>
        public abstract TCorrection TotalCorrection { get; }
        /// <summary>
        /// 设置改正数，如果之前有，则覆盖之。
        /// </summary>
        /// <param name="type">改正类型</param>
        /// <param name="val">改正值</param>
        public void SetCorrection(string type, TCorrection val) { this.Corrections[type] = val; }
        /// <summary>
        /// 添加改正数列表。
        /// </summary>
        /// <param name="corrections">改正数列表</param>
        public void SetCorrection(Dictionary<string, TCorrection> corrections)
        {
            foreach (var item in corrections)
            {
                SetCorrection(item.Key, item.Value);
            }
        }
        /// <summary>
        /// 添加改正数列表。
        /// </summary>
        /// <param name="corrections">改正数列表</param>
        public void SetCorrection(AbstractDetailedCorrectable<TValue, TCorrection> corrections)
        {
            SetCorrection(corrections.Corrections); 
        }
        /// <summary>
        /// 是否具有指定类型的改正数。
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public bool ContainsCorrection(string type) { return Corrections.ContainsKey(type); }
        /// <summary>
        /// 详细的改正数列表。
        /// </summary>
        public Dictionary<string, TCorrection> Corrections { get; protected set; }
  
        /// <summary>
        /// 清除改正数。
        /// </summary>
        public void ClearCorrections()
        {
            Corrections.Clear();
        }
         

        /// <summary>
        /// 改正数列表数量。
        /// </summary>
        public int Count
        {
            get { return Corrections.Count; }
        }

        /// <summary>
        /// 以Tab分开的项。
        /// </summary>
        /// <returns></returns>
        public override string GetTabValues()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.GetTabValues());
            foreach (var item in this.Corrections)
            {
                sb.Append("\t");
                if (item.Value is IToTabRow)
                {
                    IToTabRow row = item.Value as IToTabRow;
                    sb.Append(row.GetTabValues());
                }
                else
                { 
                    sb.Append(String.Format(FormatProvider, "{0}", item.Value));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Tab 分开项目的标题
        /// </summary>
        /// <returns></returns>
        public override string GetTabTitles()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.GetTabTitles());
            foreach (var item in this.Corrections)
            {
                sb.Append("\t");
                sb.Append(item.Key);
                if (item.Value is IToTabRow)
                {
                    IToTabRow row = item.Value as IToTabRow;
                    sb.Append(row.GetTabTitles());
                } 
            }
            return sb.ToString(); 
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            foreach (var item in Corrections)
            {
                //sb.AppendLine(StringUtil.FillSpaceLeft(key.Key, 10) + ":\t" + key.Value);
                sb.AppendLine(item.Key+ ":\t" + String.Format(FormatProvider, "{0:\t}", item.Value));
            }
            return sb.ToString();
        }
    }  
}
