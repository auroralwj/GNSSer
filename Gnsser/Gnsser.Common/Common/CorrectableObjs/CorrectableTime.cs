//2014.10.09, czs, create, 具有改正数的时间

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Times;
using Geo.Correction;
using Gnsser.Times;

namespace Gnsser.Correction
{

    /// <summary>
    /// 具有改正数的时间。
    /// </summary> 
    public class CorrectableTime : AbstractCorrectable<Time, Double>, ICorrectable<Time, Double>
    {
        /// <summary>
        /// 观测量构造函数。
        /// </summary>
        /// <param name="val">改正值</param> 
        public CorrectableTime(Time val ):base(val)
        {
        }

        #region 改正数
         

        /// <summary>
        /// 卫星钟的改正数。单位：秒。改正数的总和。需要由Corrector来赋值。后继赋值。
        /// </summary>
        public override double Correction { get; set; }
         
        #endregion
        
        /// <summary>
        /// 改正后的值，方便访问
        /// </summary>
        public override Time CorrectedValue
        {
            get { return Value + Correction; }
        } 
    } 
}