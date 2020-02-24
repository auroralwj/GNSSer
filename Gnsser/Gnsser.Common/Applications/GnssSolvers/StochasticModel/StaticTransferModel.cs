//2016.10.20, czs & double,  模型方差修改为1e-12，避免求逆出现错误
//2017.08.12, czs, edit in hongqing, 提取参数，使得可以设置。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Domain;
using Gnsser.Data.Rinex;

namespace Gnsser.Models
{ 
    /// <summary>
    /// 静态模型。 
    /// </summary>
    public class StaticTransferModel : BaseStateTransferModel
    {
        /// <summary>
        /// Default constructor
        /// </summary>
     //   public StaticTransferModel(double StdDev = 1e-10)
         
        public StaticTransferModel(double StdDev) {
            this.Variance = StdDev * StdDev;  
        }
        /// <summary>
        /// 静态模型噪声方差
        /// </summary>
        public double Variance { get; set; }
        /// <summary>
        /// 状态转移系数。  Get element of the state transition matrix Phi
        /// </summary>
        /// <returns></returns>
        public override double GetTrans()
        {
            return 1.0;
        }
        /// <summary>
        /// 噪声模型，权逆阵。 Get element of the process noise matrix Q
        /// </summary>
        /// <returns></returns>
        public override double GetNoiceVariance() { return Variance; } 

    }


   

















}
