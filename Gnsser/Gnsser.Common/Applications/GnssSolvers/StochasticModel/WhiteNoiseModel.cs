//2017.08.12, czs, edit in hongqing, 提取参数，使得可以设置

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Data.Rinex;

namespace Gnsser.Models
{
    /// <summary>
    ///  This class compute the elements of Phi and Q matrices corresponding  to a white noise stochastic model.
    /// </summary>
    public class WhiteNoiseModel : BaseStateTransferModel
    {
        /// <summary>
        /// White noise variance
        /// </summary>
        private double variance;

        /// <summary>
        /// Common constructor
        /// </summary>
        /// <param name="stdDev"> Standard deviation (sigma) of white noise process</param>
     //   public WhiteNoiseModel(double stdDev = 2e7)
        public WhiteNoiseModel(double stdDev)
        {
            variance = stdDev * stdDev;
        }
         
        /// <summary>
        /// Get element of the state transition matrix Phi
        /// </summary>
        /// <returns></returns>
        public override  double GetTrans()
        {
            return 0.0;
        }
        /// <summary>
        /// Get element of the state transition matrix Phi
        /// </summary>
        /// <returns></returns>
        public override  double GetNoiceVariance()
        {
            return variance;
        }
     
    }
}
