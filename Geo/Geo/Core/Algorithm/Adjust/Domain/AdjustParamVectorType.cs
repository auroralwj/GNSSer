//2016.10.04, czs, create in hongqing, 平差向量类型

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using Geo.Common; 

using System.Threading.Tasks;
using Geo.IO;


namespace Geo.Algorithm.Adjust
{
    
    /// <summary>
    /// 平差向量类型
    /// </summary>
    public enum AdjustParamVectorType
    {
        /// <summary>
        /// Observation
        /// </summary>
        //观测向量,
        /// <summary>
        /// Apriori
        /// </summary>
        参数先验值,
        /// <summary>
        /// Predicted
        /// </summary>
        参数预测值,
        /// <summary>
        /// Estimated
        /// </summary>
        参数估计值,
        /// <summary>
        /// Corrected
        /// </summary>
        参数改正后向量,
        /// <summary>
        /// PostfitResidual
        /// </summary>
        //观测向量验后残差
    }       
}