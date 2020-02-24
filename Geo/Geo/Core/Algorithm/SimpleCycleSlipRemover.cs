//2017.02.08, czs, create in hongqing, 简单的周跳移除器。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 简单的周跳移除器
    /// </summary>
    public class SimpleCycleSlipRemover
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Lambda"></param>
        public SimpleCycleSlipRemover(double Lambda)
        {
            this.Lambda = Lambda;
            this.Index = -1;
        }
        /// <summary>
        /// 数据编号
        /// </summary>
        public int Index { get; set; } 
        /// <summary>
        /// 波长，若值是频率则为1.
        /// </summary>
        public double Lambda { get; set; }
        /// <summary>
        /// 最后的数值
        /// </summary>
        public double LastVal { get; set; }
        /// <summary>
        /// 最后一个移除了周跳的数值
        /// </summary>
        public double LastCylceRemovedVal { get { return LastVal - CycledLambda; } } 
        /// <summary>
        /// 已跳的整周数
        /// </summary>
        public double CycledLambda { get; set; } 
        /// <summary>
        /// 计算值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public double GetVal(double val)
        {
            Index++;
            if (Index == 0)
            {
                this.CycledLambda = 0;
            }else if (IsCycleSliped(val)) //有周跳
            {
                //计算新的周跳
                this.CycledLambda = GetNewCycledLambda(val);
            }

            this.LastVal = val;
            return LastCylceRemovedVal;
        }

        /// <summary>
        /// 计算新的偏差值。
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public double GetNewCycledLambda(double val)
        {
            var differVal  = (val - LastCylceRemovedVal) ;
            var differInCycle = (int)(differVal / Lambda);
            return differInCycle * Lambda;
        }

        /// <summary>
        /// 是否产生了周跳
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool IsCycleSliped(double val)
        {
            return Math.Abs(val - LastVal) > Lambda;
        }
    }
 }
