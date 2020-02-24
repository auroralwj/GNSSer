//2014.10.25, czs, create, 详情改正数列表

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Utils;

namespace Geo.Correction
{
    /// <summary>
    /// 基础的具有改正数的数值类。具有详情改正数列表
    /// </summary> 
    public class DetailedCorrectableNumeral :
        AbstractDetailedCorrectable<Double, Double>,
        IDetailedCorrectable<Double, Double>,
        ICorrectableNumeral,
        IToTabRow
    {
        /// <summary>
        /// 观测量构造函数。
        /// </summary>
        /// <param name="val">改正值</param> 
        public DetailedCorrectableNumeral(double val = 0)
            : base(val)
        {
            this.Corrections = new Dictionary<string, double>();
        }
        /// <summary>
        /// 所有详细的改正数之和。
        /// </summary>
        public override double TotalCorrection
        {
            get
            {
                double total = 0;
                foreach (var item in Corrections.Values)
                {
                    if (Geo.Utils.DoubleUtil.IsValid(item)) { total += item; } 
                }
                return total;
            }
        }
        /// <summary>
        /// 改正数
        /// </summary>
        public override double Correction
        {
            get
            {
                return TotalCorrection;
            }
            set
            {
                throw new NotImplementedException("不用设置了！这里采用 TotalCorrection 属性！");
            }
        }


        /// <summary>
        /// 改正后的数值
        /// </summary>
        public override double CorrectedValue
        {
            get { return Value + Correction; }
        }
        /// <summary>
        /// 值为 0
        /// </summary>
        public static DetailedCorrectableNumeral Zero { get { return new DetailedCorrectableNumeral(0); } }
    }
}