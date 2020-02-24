using System;
namespace Geo.Referencing
{
    /// <summary>
    /// 坐标轴。
    /// </summary>
    public interface IAxis
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 坐标轴
        /// </summary>
        Ordinate Ordinate { get; set; }
        /// <summary>
        /// 坐标轴指向
        /// </summary>
        Orientation Orientation { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        Unit Unit { get; set; }
    }
}
