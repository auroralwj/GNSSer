//2014.06.05, czs, create

using System;
using System.Collections.Generic;
using Geo.Referencing;
using System.Collections;

namespace Geo.Coordinates
{
    /// <summary>
    /// 同一参考系下的坐标序列。
    /// </summary>
    public interface ICoordinateSequence : IEnumerable<ICoordinate>
    {
        /// <summary>
        /// 坐标的维数。默认坐标处于同一参考系下，所以坐标的维数相同。
        /// </summary>
        int Dimension { get; }

        /// <summary>
        /// 获取或设置某坐标轴的坐标值。      
        /// </summary>
        /// <param name="index">序列中的坐标编号</param>
        /// <param name="ordinate"> 坐标的坐标轴 </param>
        /// <returns></returns>       
        Double this[Int32 index, Ordinate ordinate] { get; set; }
        /// <summary>
        /// 坐标所使用的参考系统。没有参考系统的坐标是没有意义的。
        /// </summary>
        ICoordinateReferenceSystem ReferenceSystem { get; }
        /// <summary>
        /// 获取或设置序列中指定编号的坐标。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        ICoordinate this[Int32 index] { get; set; }

        /// <summary>
        /// 转换为 <see name="ICoordinate"/> 的数组。
        /// </summary>
        ICoordinate[] ToArray();
        /// <summary>
        /// 添加一个到序列末尾。
        /// </summary>
        /// <param name="coord"></param>
        void Add(ICoordinate coord);
        /// <summary>
        /// 添加到指定位置。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="coord"></param>
        void Insert(int index, ICoordinate coord);
        /// <summary>
        /// 合并坐标序列
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
         void  Merge(ICoordinateSequence other);
        /// <summary>
        /// 克隆坐标序列
        /// </summary>
        /// <returns></returns>
        ICoordinateSequence Clone();
        /// <summary>
        /// 在指定精度下，坐标是否相等。
        /// </summary>
        /// <param name="other">待比较的坐标序列</param>
        /// <param name="tolerance">若为空则值为 0 </param>
        /// <returns></returns>
        Boolean Equals(ICoordinateSequence other, Tolerance tolerance);

        //ICoordinateSequence Freeze();
        /// <summary>
        /// 序列中的第一个
        /// </summary>
        ICoordinate First { get; }
        /// <summary>
        /// 序列中的最后一个
        /// </summary>
        ICoordinate Last { get; }
        //Boolean IsFrozen { get; }
        /// <summary>
        /// 序列大小
        /// </summary>
        Int32 Count { get; }
        /// <summary>
        /// 距离远点最远的坐标。
        /// </summary>
        ICoordinate Maximum { get; }
        /// <summary>
        /// 距离原点最近的坐标。
        /// </summary>
        ICoordinate Minimum { get; }
        /// <summary>
        /// 本身反序
        /// </summary>
        /// <returns></returns>
        void Reverse();
      
        //ICoordinateSequenceFactory CoordinateSequenceFactory { get; }

        //Pair<ICoordinate> SegmentAt(Int32 index);
        /// <summary>
        /// 序列改变了。
        /// </summary>
        event EventHandler SequenceChanged;
    }
}
