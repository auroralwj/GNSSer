//2014.09.27, czs, created, 具有权值的坐标

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;
using Geo.Common;

namespace Geo.Coordinates
{
    /// <summary>
    /// X,Y,Z 三维坐标。三维空间的 3 个双精度分量。
    /// </summary> 
    public class RmsedXYZ : RmsedValue<XYZ>
    {
        /// <summary>
        /// 加权坐标
        /// </summary>
        public RmsedXYZ()
            : base(new XYZ(), new XYZ())
        {
        }
        /// <summary>
        /// 加权坐标
        /// </summary>
        /// <param name="xyz"></param>
        public RmsedXYZ(XYZ xyz)
            : base(xyz, new XYZ())
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="xyz">坐标</param>
        /// <param name="rms">精度信息</param>
        public RmsedXYZ(XYZ xyz, XYZ rms)
            : base(xyz, rms)
        {

        }

        public static RmsedXYZ operator +(RmsedXYZ left, XYZ right)
        {
            return new RmsedXYZ(left.Value + right, left.Rms);
        }
        public static RmsedXYZ operator +(XYZ left, RmsedXYZ right)
        {
            return new RmsedXYZ(left + right.Value, right.Rms);
        }
        public static RmsedXYZ operator +(RmsedXYZ left, RmsedXYZ right)
        {
            return new RmsedXYZ(left.Value + right.Value,
               new XYZ(
                   Geo.Utils.DoubleUtil.SquareRootOfSumOfSquares(left.Rms.X, right.Rms.X),
                   Geo.Utils.DoubleUtil.SquareRootOfSumOfSquares(left.Rms.Y, right.Rms.Y),
                   Geo.Utils.DoubleUtil.SquareRootOfSumOfSquares(left.Rms.Z, right.Rms.Z)));
        }
        public static RmsedXYZ operator -(RmsedXYZ left, RmsedXYZ right)
        {
            return new RmsedXYZ(left.Value - right.Value,
               new XYZ(
                   Geo.Utils.DoubleUtil.SquareRootOfSumOfSquares(left.Rms.X, right.Rms.X),
                   Geo.Utils.DoubleUtil.SquareRootOfSumOfSquares(left.Rms.Y, right.Rms.Y),
                   Geo.Utils.DoubleUtil.SquareRootOfSumOfSquares(left.Rms.Z, right.Rms.Z)));
        }
        public static RmsedXYZ operator -(RmsedXYZ right)
        {
            return new RmsedXYZ( - right.Value, right.Rms);
        }
        /// <summary>
        /// 0
        /// </summary>
        public static RmsedXYZ Zero => new RmsedXYZ();
        /// <summary>
        /// 最大值
        /// </summary>
        public static RmsedXYZ MaxValue => new RmsedXYZ(XYZ.MaxValue, XYZ.MaxValue);
    }
}