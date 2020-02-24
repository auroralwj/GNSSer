//2014.09.14, czs, create, 观测量，Gnsser核心模型！

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Correction
{

    /// <summary>
    /// 基础的具有改正数的数值类。
    /// </summary>
    public class CorrectableNumeral : ICorrectableNumeral
    {
        /// <summary>
        /// 观测量构造函数。
        /// </summary>
        /// <param name="val">改正值</param> 
        public CorrectableNumeral(double val = 0)
        {
            this.Value = val; 
        }

        #region 改正数 
        /// <summary>
        /// 改正数的总和。需要由Corrector来赋值。后继赋值。
        /// </summary>
        public virtual double Correction { get; set; }
         
        #endregion
        
        /// <summary>
        /// 改正后的数值，方便访问
        /// </summary>
        public virtual double CorrectedValue
        {
            get { return Correction + Value; }
        }
        /// <summary>
        /// 数值。
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// 是否具有改正数
        /// </summary>
        public virtual bool HasCorrection { get => Correction != 0; }
        public override string ToString()
        {
            return CorrectedValue.ToString();
        }
    } 
}