//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Referencing
{  
    /// <summary>
    /// 坐标系统是描述物质存在的空间位置（坐标）的参照系，
    /// 通过定义特定基准及其参数形式来实现。
    /// 空间位置用坐标实现，坐标系统是由一个或多个坐标轴组成的集合。
    /// </summary>
    public interface ICoordinateSystem : IEnumerable<IAxis>
    {
        /// <summary>
        /// 坐标系统维数。
        /// </summary>
        int Dimension { get; }

        /// <summary>
        /// 坐标分量的组合类型。
        /// </summary>
        CoordinateType CoordinateType { get; }

        /// <summary>
        /// 是否包含指定坐标轴。
        /// </summary>
        /// <param name="ordinate">坐标轴</param>
        /// <returns></returns>
        bool Contains(Ordinate ordinate);
        /// <summary>
        /// 是否包含指定坐标轴。
        /// </summary>
        /// <param name="ordinates">坐标轴</param>
        /// <returns></returns>
        bool Contains(params Ordinate [] ordinates);

        /// <summary>
        /// 坐标轴集合。
        /// </summary>
        List<IAxis> Axes { get; set; }        

        /// <summary>
        /// 坐标轴检索。
        /// </summary>
        /// <param name="axisIndex">编号，从0开始</param>
        /// <returns></returns>
        IAxis this[int axisIndex] { get; set; }
    }
}
