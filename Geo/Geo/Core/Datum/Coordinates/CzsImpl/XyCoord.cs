//2014.06.06,czs,created

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;
using Geo.Referencing;


namespace Geo.Coordinates
{
    /// <summary>
    /// 具有 X Y 分量的坐标。
    /// </summary>
    [Serializable]
    public class XyCoord : Coordinate, IXyCoord
    {
        /// <summary>
        /// 默认构造函数。初始化为 Coordinate.Empty。
        /// </summary>
        public XyCoord():this(null) {}
         
        /// <summary>
        /// 将一个隐含有 X, Y 的坐标转换为 XY 坐标。
        /// </summary>
        /// <param name="coord">含有 Lon, Lat的坐标转换</param>
        public XyCoord(ICoordinate coord) 
            :this(coord.ReferenceSystem, coord[Ordinate.X], coord[Ordinate.Y], coord.Weight)
        {
         }
        /// <summary>
        /// 由参考系统实例化坐标。
        /// </summary>
        /// <param name="referenceSystem">参考系统</param>
        public XyCoord(ICoordinateReferenceSystem referenceSystem, double x= 0, double y = 0,double weight=0):base(referenceSystem, weight)
        {
            if (!ReferenceSystem.CoordinateSystem.Contains(Ordinate.X)
                || !ReferenceSystem.CoordinateSystem.Contains(Ordinate.Y))
                throw new ArgumentException("参考系中没有 X Y 轴", "referenceSystem");
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// X 轴分量值。
        /// </summary>
        public double X
        {
            get { return this[Ordinate.X]; }
            set { this[Ordinate.X] = value; }
        }

        /// <summary>
        /// Y 轴分量值。
        /// </summary>
        public double Y
        {
            get { return this[Ordinate.Y]; }
            set { this[Ordinate.Y] = value; }
        }

        /// <summary>
        /// 值是否全为 0.
        /// </summary>
        public bool IsZero { get { return X == 0 && Y == 0; } }
    }
}
