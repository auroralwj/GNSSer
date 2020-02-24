//2018.07.31, czs, create in hmx, 提取双站定位结果接口
//2018.11.12，czs, edit in hmx, 由 ITwoSiteResult 更名为 IEstimatedBaseline


using Geo.Coordinates;
using Geo;
using Geo.Algorithm;
using System.Collections.Generic;
using Geo.Algorithm.Adjust;

namespace Gnsser
{
    /// <summary>
    /// 具有基线的结果
    /// </summary>
    public interface IWithEstimatedBaseline
    {
        /// <summary>
        /// 双站基线定位结果
        /// </summary>
        IEstimatedBaseline GetEstimatedBaseline();
    }
    /// <summary>
    /// 具有基线的结果
    /// </summary>
    public interface IWithEstimatedBaselines
    {
        /// <summary>
        /// 双站基线定位结果
        /// </summary>
        BaseLineNet  GetEstimatedBaselines();
    }
    /// <summary>
    /// 双站基线定位结果
    /// </summary>
    public interface IEstimatedBaseline : Geo.IReadable, Namable, IToTabRow
    {
        /// <summary>
        /// 反向
        /// </summary>
        IEstimatedBaseline ReversedBaseline { get; }
        /// <summary>
        /// 计算算法类型
        /// </summary>
        GnssSolverType GnssSolverType { get; }

            /// <summary>
            /// 计算结果类型
            /// </summary>
            ResultType ResultType { get; set; }
        /// <summary>
        /// 基线名称
        /// </summary>
        GnssBaseLineName BaseLineName { get; set; }
        /// <summary>
        /// 流动站坐标改正数。
        /// </summary>
        RmsedXYZ CorrectionOfRov { get; }
        /// <summary>
        /// XYZ 协方差阵
        /// </summary>
        Matrix CovaMatrix { get; } 
        /// <summary>
        /// 近似向量
        /// </summary>
        XYZ ApproxVector { get; }
        /// <summary>
        /// 参考站坐标
        /// </summary>
        XYZ ApproxXyzOfRef { get; }
        /// <summary>
        /// 流动站近似坐标
        /// </summary>
        XYZ ApproxXyzOfRov { get; }
        /// <summary>
        /// 估值向量
        /// </summary>
        XYZ EstimatedVector { get; }
        /// <summary>
        /// 估值向量
        /// </summary>
        RmsedXYZ EstimatedVectorRmsedXYZ { get; set; }
        /// <summary>
        /// 估值向量
        /// </summary>
        ENU EstimatedVectorEnu { get; }
        /// <summary>
        /// 向量改正数ENU
        /// </summary>
        ENU CorrectionOfRovEnu { get; }

        /// <summary>
        /// 流动站估值坐标
        /// </summary>
        XYZ EstimatedXyzOfRov { get; }
        /// <summary>
        /// 流动站估值大地坐标
        /// </summary>
        GeoCoord EstimatedGeoCoordOfRov { get; }
        /// <summary>
        /// 流动站估值坐标
        /// </summary>
        RmsedXYZ EstimatedRmsXyzOfRov { get; }
        /// <summary>
        /// 参考站信息
        /// </summary>
        ISiteInfo SiteInfoOfRef { get; }
        /// <summary>
        /// 流动站信息
        /// </summary>
        ISiteInfo SiteInfoOfRov { get; }
        /// <summary>
        /// 闭合差
        /// </summary>
        double ClosureError { get; set; }
        /// <summary>
        /// 大地方位角,单位度小数
        /// </summary>
        double GeodeticAzimuth { get; }
        /// <summary>
        /// 以行输出
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> GetObjectRow();
    }
}