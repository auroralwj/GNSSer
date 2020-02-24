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
    public class XyzCoord : XyCoord, IXyzCoord
    { 
        /// <summary>
        /// 默认构造函数。初始化为 Coordinate.Empty。
        /// </summary>
        public XyzCoord():this(null) {}
        /// <summary>
        /// 将顶层坐标转换为XYZ坐标。
        /// </summary>
        /// <param name="coord">顶层坐标</param>
        public XyzCoord(ICoordinate coord) : 
            this(coord.ReferenceSystem, coord[Ordinate.X], coord[Ordinate.Y], coord[Ordinate.Z], coord.Weight)
        {

        }
        /// <summary>
        /// 由参考系统实例化坐标。
        /// </summary>
        /// <param name="referenceSystem">参考系统</param>
        public XyzCoord(ICoordinateReferenceSystem referenceSystem, double x = 0, double y = 0, double z = 0, double weight = 0)
            : base(referenceSystem,x,y, weight)
        {
            if (!ReferenceSystem.CoordinateSystem.Contains(Ordinate.Z))
                throw new ArgumentException("参考系中没有 Z 轴", "referenceSystem");
            this.Z = z;
        }

        /// <summary>
        /// Z 轴分量值。
        /// </summary>
        public double Z
        {
            get { return this[Ordinate.Z]; }
            set { this[Ordinate.Z] = value; }
        }

        //public override string ToString()
        //{
        //    return "(" + X + "," + Y + "," + "," + Z + ")";
        //}
    }
}
