//2015.07.01,czs, create in TianJing, 盒子接口

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using Geo.Coordinates;
using Geo.Utils;

namespace Geo.Coordinates
{
    /// <summary>
    /// XY 二维接口。
    /// </summary>
    public interface ILonLatEnvelope : IBox<LonLat>
    {
        /// <summary>
        /// 交集
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        ILonLatEnvelope And(ILonLatEnvelope other);
        /// <summary>
        /// 并集
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        ILonLatEnvelope Expands(ILonLatEnvelope box);
        /// <summary>
        /// 最大X
        /// </summary>
        double MaxX { get; }
        /// <summary>
        /// 最大Y
        /// </summary>
        double MaxY { get; }
        /// <summary>
        /// 最小X
        /// </summary>
        double MinX { get; }
        /// <summary>
        /// 最小Y
        /// </summary>
        double MinY { get; }
    }
}