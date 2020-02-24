//2016.04.23, czs, create in hongqing, 产品计算器接口

using System;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo;

namespace Gnsser
{ 

    /// <summary>
    /// GNSS 计算器
    /// </summary>
    public interface IGnssSolver
    { 
        /// <summary>
        /// 平差矩阵
        /// </summary>
        AdjustResultMatrix Adjustment { get; set; }
        /// <summary>
        /// 数据源上下文
        /// </summary>
        DataSourceContext DataSourceContext { get; set; }
        /// <summary>
        /// 定位选项
        /// </summary>
        GnssProcessOption Option { get; set; }
        /// <summary>
        /// 基础的，不变的参数数量，如伪距定位通常为 4.如果PPP则为5，其它为可变的卫星模糊度参数。 
        /// </summary>
        int BaseParamCount { get; set; }
        /// <summary>
        /// 矩阵生成器
        /// </summary>
        BaseAdjustMatrixBuilder MatrixBuilder { get; }
        /// <summary>
        /// 计算完后激活
        /// </summary>
        void Complete();
    }

    /// <summary>
    /// GNSS产品计算器接口
    /// </summary>
    public interface IGnssSolver<TProduct, TMaterial> : IService<TProduct, TMaterial>, IGnssSolver
        where TMaterial : ISiteSatObsInfo
        where TProduct : BaseGnssResult
    {
       
        /// <summary>
        /// 矩阵生成器
        /// </summary>
        new BaseGnssMatrixBuilder<TProduct, TMaterial> MatrixBuilder { get; set; }

    }
}
