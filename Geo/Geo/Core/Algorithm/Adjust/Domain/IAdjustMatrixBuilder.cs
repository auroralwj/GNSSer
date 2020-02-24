//2014.09.04, czs, create, 进一步对平差进行封装
//2018.04.15, czs, 增加自由项，将ObsMiusApprox改回Observation

using System;
using System.Collections.Generic;


namespace Geo.Algorithm.Adjust
{
    /// <summary>
    /// 平差计算中需要转换的矩阵。
    /// </summary>
    public interface IAdjustMatrixBuilder
    {
        /// <summary>
        /// 信息
        /// </summary>
        string Message { get; }
        /// <summary>
        /// 指示是否可以平差。如果数据质量太差，则不推荐计算，以免影响结果。
        /// </summary>
        bool IsAdjustable { get; }

        /// <summary>
        /// 参数名称
        /// </summary>
        List<string> ParamNames { get; }
        /// <summary>
        /// 观测量，即设计矩阵的行数。
        /// </summary>
        int ObsCount { get; }
        /// <summary>
        /// 参数数量
        /// </summary>
        int ParamCount { get; }  
         
        /// <summary>
        /// 设计阵，误差方程系数阵。
        /// </summary>
        Matrix Coefficient { get; }
        /// <summary>
        /// 观测值，一般为残差，即观测值减去近似值
        /// </summary>
        WeightedVector Observation { get; }
        /// <summary>
        /// 自由项D，B0等等。则参数平差中，满足满足 l = L - (AX0 + D)
        /// </summary>
        Vector FreeVector { get; }
        /// <summary>
        /// 是否有自由项
        /// </summary>
        /// <returns></returns>
        bool HasFreeVector { get; }


    }
}
