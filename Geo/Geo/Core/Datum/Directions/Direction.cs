using System;
using System.Collections.Generic;
using System.Text;

namespace Geo.Coordinates
{
    /// <summary>
    /// 方向，8个方向。
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// 未知
        /// </summary>
        UnKnown,
        /// <summary>
        /// 东
        /// </summary>
        East,
        /// <summary>
        /// 南
        /// </summary>
        South,
        /// <summary>
        /// 西
        /// </summary>
        West,
        /// <summary>
        /// 北
        /// </summary>
        North,
        /// <summary>
        /// 东南
        /// </summary>
        SouthEast,
        /// <summary>
        /// 西南
        /// </summary>
        SouthWest,
        /// <summary>
        /// 东北
        /// </summary>
        NorthEast,
        /// <summary>
        /// 西北
        /// </summary>
        NorthWest
    }
    /// <summary>
    /// 行走方向，趋势。
    /// </summary>
    public enum DrivenDirection
    {
        /// <summary>
        /// 向前
        /// </summary>
        Ahead,
        /// <summary>
        /// 向后
        /// </summary>
        Back,
        /// <summary>
        /// 向右
        /// </summary>
        Right,
        /// <summary>
        /// 向左
        /// </summary>
        Left
    }
}
