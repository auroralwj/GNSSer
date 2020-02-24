//2014.06.03, czs, edit

using System;
using System.Collections.Generic;
using Geo.Referencing;

namespace Geo.Coordinates
{
    /// <summary>
    /// 顶层坐标值的接口。所有的坐标类都应该实现本接口。
    /// </summary>
    public interface ICoordinate :
         ICloneable,
         Geo.Algorithm.IVector,
         IComparable,
         IEnumerable<KeyValuePair<Ordinate, Double>>,//坐标由在各轴上的数字组成。 
         IEquatable<ICoordinate>,
         INumeralIndexing
     {
        /// <summary>
        /// 权值。兼容于GeoAPI对应于其 M 变量。
        /// </summary>
        double Weight { get; set; }
        /// <summary>
        /// 标签，用于存储一个对象。
        /// </summary>
        object Tag { get; set; }
        
        /// <summary>
        /// 坐标分量的维数，通常为1维、2维和3维。
        /// </summary>
        int Dimension { get; }
        /// <summary>
        /// 坐标所使用的参考系统。没有参考系统的坐标是没有意义的。
        /// </summary>
        ICoordinateReferenceSystem ReferenceSystem { get;  }
        /// <summary>
        /// 是否是同类型坐标，如同一参考系、相同的坐标轴、坐标单位等。
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        bool IsHomogenized(ICoordinate other);

        /// <summary>
        /// 为 0.
        /// </summary>
        ICoordinate Zero { get; }
        /// <summary>
        /// 获取和设置坐标值。
        /// </summary>
        /// <param name="ordinate"></param>
        /// <returns></returns>
        double this[Ordinate ordinate] { get; set; }
        /// <summary>
        /// 获取或设置坐标轴数值。
        /// </summary>
        /// <param name="axisIndex">坐标轴序号，从 0 开始</param>
        /// <returns></returns>
        //double this[int axisIndex] { get; set; }

        /// <summary>
        /// 坐标轴
        /// </summary>
        List<IAxis> Axes { get; }

        /// <summary>
        /// 是否包含坐标。
        /// </summary>
        /// <param name="ordinate"></param>
        /// <returns></returns>
        Boolean ContainsOrdinate(Ordinate ordinate);
        /// <summary>
        /// 与另一坐标的距离。
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        Double Distance(ICoordinate other);
        /// <summary>
        /// 到原点的欧式距离，半径。
        /// </summary>
        double Radius { get; }

        /// <summary>
        /// 是否为空
        /// </summary>
        Boolean IsEmpty { get; }
        /// <summary>
        /// 在制定精度范围（含）内是否相等。
        /// </summary>
        /// <param name="other"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        bool Equals(ICoordinate other, Tolerance tolerance);
    }

   
}
