//2014.11.07,czs, create in namu, 盒子接口

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
    public interface IEnvelope : IBox<XY>
    {
        /// <summary>
        /// 交集
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        IEnvelope And(IEnvelope other);
        /// <summary>
        /// 并集
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        IEnvelope Expands(IEnvelope box);
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