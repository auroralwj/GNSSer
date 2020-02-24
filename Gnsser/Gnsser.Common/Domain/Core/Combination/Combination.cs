//2014.09.14, czs, create, 观测量，Gnsser核心模型！

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Correction;
using Gnsser.Correction;

namespace Gnsser.Domain
{
     
    /// <summary>
    /// 组合值.与观测值雷同。
    /// </summary>
    public class Combination : DetailedCorrectableNumeral, ICombinateValue
    {
        /// <summary>
        /// 观测量构造函数。
        /// </summary>
        /// <param name="val">改正值</param> 
        public Combination(double val = 0)
            :base(val)
        { 
        }
         
    } 
}