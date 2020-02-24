//2014.10.25, czs, create in namu shuangliao, 可以改正的对象，对象与改正数不必是同一种类型
//2014.11.19, czs, edit in numu, 名称 CorrectableObject 改为 AbstractCorrectable
//2018.03.27, czs, edit in hmx, 增加改正数构造函数


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Correction
{
    /// <summary>
    /// 可以改正的对象，对象与改正数不必是同一种类型
    /// </summary>
    public abstract class AbstractCorrectable<TValue, TCorrection> : ICorrectable<TValue, TCorrection>, IToTabRow
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="Value">值</param>
        public AbstractCorrectable(TValue Value)
        {
            this.Value = Value;
            this.FormatProvider = new NumeralFormatProvider();
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="Value">值</param>
        /// <param name="correction">改正数</param>
        public AbstractCorrectable(TValue Value, TCorrection correction)
        {
            this.Value = Value;
            this.Correction = correction;
            this.FormatProvider = new NumeralFormatProvider();
        }

        /// <summary>
        /// 格式化提供器
        /// </summary>
        public IFormatProvider FormatProvider { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public TValue Value { get; set; }

        /// <summary>
        /// 改正数
        /// </summary>
        public virtual TCorrection Correction { get; set; }

        /// <summary>
        /// 改正后的值
        /// </summary>
        public abstract TValue CorrectedValue { get; }
        /// <summary>
        /// 是否具有改正数
        /// </summary>
        public virtual bool HasCorrection { get => Correction != null || !Correction .Equals( default(TCorrection)); }

        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return CorrectedValue.ToString();
        }

        /// <summary>
        /// 以 Tab 字符分开的元素数值。
        /// </summary>
        /// <returns></returns>
        public virtual string GetTabValues()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(GetTabValues<TValue>(Value));
            sb.Append("\t");
            sb.Append(GetTabValues<TCorrection>(Correction));
            sb.Append("\t");
            sb.Append(GetTabValues<TValue>(CorrectedValue)); 
            return sb.ToString();
        }

        /// <summary>
        /// 以 Tab 字符分开的元素标题。
        /// </summary>
        /// <returns></returns>
        public virtual string GetTabTitles()
        { 
            StringBuilder sb = new StringBuilder();
            sb.Append(GetTabTitles(Value, "数值"));
            sb.Append("\t");
            sb.Append(GetTabTitles(Correction, "改正数")); 
            sb.Append("\t");
            sb.Append(GetTabTitles(CorrectedValue, "改正值")); 
            return sb.ToString();
        }

        /// <summary>
        /// 获取表格分开的标题
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="t">对象</param>
        /// <param name="defalutTitle">默认题目</param>
        /// <returns></returns>
        public string GetTabTitles<T>(T t, string defalutTitle = "Name")
        {
            if (t is IToTabRow)
            {
                IToTabRow val = t as IToTabRow;
                return defalutTitle + (val.GetTabTitles());
            }
            else
            {
                return defalutTitle;
            }
        }

        /// <summary>
        /// 获取表格分开的项目
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public string GetTabValues<T>(T t)
        {
            if (t is IToTabRow)
            {
                IToTabRow val = t as IToTabRow;
                return  (val.GetTabValues());
            }
            else
            {
                return  (String.Format(FormatProvider, "{0}", t));
            }
        }

    }  
}
