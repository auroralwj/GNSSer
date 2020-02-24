//2014.11.07, czs, create in numu, 二维平面坐标接口，以X、Y分量表示。

using System;

namespace Geo.Coordinates
{
    /// <summary>
    /// 二维平面坐标接口，以X、Y分量表示。
    /// </summary>
    public class LineSegment<TCoord> where  TCoord :INumeralIndexing , ICloneable, new()
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="CoordA">第一个坐标</param>
        /// <param name="CoordB">第二个坐标</param>
        public LineSegment(TCoord CoordA, TCoord CoordB)
        {
            this.CoordA = CoordA;
            this.CoordB = CoordB;
        }

        public TCoord this[int i] { get { if (i == 0) return CoordA; return CoordB; } }
        /// <summary>
        /// 盒子。
        /// </summary>
        public Box<TCoord> Box{ get { return new Box<TCoord>(CoordA, CoordB); } }
        /// <summary>
        /// CoordA 坐标分量
        /// </summary>
        public TCoord CoordA { get; set; }
        /// <summary>
        /// CoordB 坐标分量
        /// </summary>
        public TCoord CoordB { get; set; }

        public Direction Direction
        {
            get
            {
                if (CoordA.Equals(CoordB)) return Coordinates.Direction.UnKnown;
                if (CoordA[0] == CoordB[0])
                {
                    if (CoordA[1] == CoordB[1]) return Coordinates.Direction.UnKnown;
                    if (CoordA[1] > CoordB[1]) return Coordinates.Direction.South;
                    if (CoordA[1] < CoordB[1]) return Coordinates.Direction.North;
                } 
                if (CoordA[0] < CoordB[0])
                {
                    if (CoordA[1] == CoordB[1]) return Coordinates.Direction.East;
                    if (CoordA[1] > CoordB[1]) return Coordinates.Direction.SouthEast;
                    if (CoordA[1] < CoordB[1]) return Coordinates.Direction.NorthEast;
                }
                if (CoordA[0] > CoordB[0])
                {
                    if (CoordA[1] == CoordB[1]) return Coordinates.Direction.West;
                    if (CoordA[1] > CoordB[1]) return Coordinates.Direction.SouthWest;
                    if (CoordA[1] < CoordB[1]) return Coordinates.Direction.NorthWest;
                }
                return Coordinates.Direction.UnKnown;
            }
        }
        public override string ToString()
        {
            return CoordA.ToString() + "->" + CoordB.ToString();
        }
    }
    /// <summary>
    /// 2 维直线。
    /// </summary>
    public class TwoDLineSegment<TCoord> : 
        LineSegment<TCoord> 
        where TCoord : INumeralIndexing, ICloneable, new()
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="xyA">第1个坐标</param>
        /// <param name="xyB">第2个坐标</param>
        public TwoDLineSegment(TCoord xyA, TCoord xyB)
            : base(xyA, xyB)
        {

        }
        /// <summary>
        /// 计算两线段的交点。如果不相交，则返回null。
        /// </summary>
        /// <param name="other">另一条线段</param>
        /// <returns></returns>
        public virtual TCoord GetIntersectionCoord(TwoDLineSegment<TCoord> other) {

            return (TCoord)XyUtil.GetIntersectionPtOfTwoLineSegment(this.CoordA, this.CoordB, other.CoordA, other.CoordB);
           // return default(TCoord); 
        }
    }

    /// <summary>
    /// 2 维直线。
    /// </summary>
    public class XyLineSegment : TwoDLineSegment<XY>
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="xyA">第1个坐标</param>
        /// <param name="xyB">第2个坐标</param>
        public XyLineSegment(XY xyA, XY xyB)
            : base(xyA, xyB)
        {

        }
        /// <summary>
        /// 计算两线段的交点。如果不相交，则返回null。
        /// </summary>
        /// <param name="other">另一条线段</param>
        /// <returns></returns>
        public override XY GetIntersectionCoord(TwoDLineSegment<XY> other)
        {
           return  XyUtil.GetIntersectionPtOfTwoLineSegment(this.CoordA, this.CoordB, other.CoordA, other.CoordB);
        }
         
    }




}
