//2014.11.07,czs, create in namu, 盒子接口

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using Geo.Coordinates;
using Geo.Utils;
using Geo.Algorithm;

namespace Geo.Coordinates
{
    /// <summary>
    /// 几何对象之间的关系
    /// </summary>
    public enum GeometryRelation
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown,
        /// <summary>
        /// 包含
        /// </summary>
        Contain,
        /// <summary>
        /// 切割而过
        /// </summary>
        Cross,
        /// <summary>
        /// 相切一半
        /// </summary>
        Intersect, 
        /// <summary>
        ///  相离
        /// </summary>
        Sparate
    }

    /// <summary>
    /// 无所不在的盒子、边界、窗口、矩形。
    /// 以 （X， Y）为坐标，笛卡尔右手坐标系，Y为竖轴指向上对应高度，X为横轴指向右对应宽度。
    /// Box, Bound, Box ViewPort.
    /// </summary>
    [Serializable]
    public class Box<TCoord> : IBox<TCoord> 
        where TCoord :INumeralIndexing , ICloneable, new()
    {
        #region constructor
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public Box() { }


        /// <summary>
        /// 以一个点初始化。
        /// </summary>
        /// <param name="xy"></param>
        public Box(TCoord xy)
        {
            double minX = xy[0];
            double minY = xy[1];
            double maxX = xy[0];
            double maxY = xy[1];
            InitParams(minX, minY, maxX, maxY);
        }
        /// <summary>
        /// 以四个参数初始化
        /// </summary>
        /// <param name="minX"></param>
        /// <param name="minY"></param>
        /// <param name="maxX"></param>
        /// <param name="maxY"></param>
        public Box(double minX, double minY, double maxX, double maxY)
        {
            InitParams(minX, minY, maxX, maxY);
        }
        /// <summary>
        /// 以中心点初始化
        /// </summary>
        /// <param name="centerXy"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Box(TCoord centerXy, double width, double height)
        {
            double minX = centerXy[0] - width / 2;
            double maxX = centerXy[0] + width / 2;
            double minY = centerXy[1] - height / 2;
            double maxY = centerXy[1] + height / 2;
            InitParams(minX, minY, maxX, maxY);
        }

        /// <summary>
        /// 自动提取TCoord的最大小X、Y值生成一个Box。
        /// </summary>
        /// <param name="xy1">第一个坐标</param>
        /// <param name="xy2">第二个坐标</param>
        public Box(INumeralIndexing xy1, INumeralIndexing xy2)
        {
            double minX = Math.Min(xy1[0], xy2[0]);
            double minY = Math.Min(xy1[1], xy2[1]);
            double maxX = Math.Max(xy1[0], xy2[0]);
            double maxY = Math.Max(xy2[1], xy1[1]);
            InitParams(minX, minY, maxX, maxY);
        } 
        private void InitParams(double minX, double minY, double maxX, double maxY)
        {
            this.MinHorizontal = (minX);
            this.MinVertical = (minY);
            this.MaxHorizontal = (maxX);
            this.MaxVertical = (maxY);
        }

        #endregion 

        #region property
        /// <summary>
        /// 盒子中心
        /// </summary>
        public virtual TCoord Center
        {
            get
            {
                TCoord t = new TCoord();
                t[0] = (MinHorizontal + MaxHorizontal) / 2;
                t[1] = (MinVertical + MaxVertical) / 2;
                return t;
            }
        }
        /// <summary>
        /// 左上角
        /// </summary>
        public virtual TCoord LeftTop
        {
            get
            {
                TCoord t = new TCoord();
                t[0] = MinHorizontal;
                t[1] = MaxVertical;
                return t;
            }
        }
        /// <summary>
        /// 左下角
        /// </summary>
        public virtual TCoord LeftBottom
        {            
            get
            {
                TCoord t = new TCoord();
                t[0] = MinHorizontal;
                t[1] = MinVertical;
                return t;
            }
        }
        /// <summary>
        /// 右上角
        /// </summary>
        public virtual TCoord RightTop
        {          
            get
            {
                TCoord t = new TCoord();
                t[0] = MaxHorizontal;
                t[1] = MaxVertical;
                return t;
            } 
        }
        /// <summary>
        /// 右下角
        /// </summary>
        public virtual TCoord RightBottom
        {       
            get
            {
                TCoord t = new TCoord();
                t[0] = MaxHorizontal;
                t[1] = MinVertical;
                return t;
            } 
        } 
        /// <summary>
        /// 盒子宽度
        /// </summary>
        public double Width { get { return Math.Abs(MaxHorizontal - MinHorizontal); } }
        /// <summary>
        /// 盒子高度
        /// </summary>
        public double Height { get { return Math.Abs(MaxVertical - MinVertical); } }

        /// <summary>
        /// X 轴最小值
        /// </summary>
        public double MinHorizontal { get; set; }
        /// <summary>
        /// Y 轴最大值
        /// </summary>
        public double MaxVertical { get; set; }
        /// <summary>
        /// X 轴的最大值
        /// </summary>
        public double MaxHorizontal { get; set; }
        /// <summary>
        /// Y 轴最小值
        /// </summary>
        public double MinVertical { get; set; }
        #endregion

        #region toperlogy

        /// <summary>
        /// 是否包含此点
        /// </summary>
        /// <param name="xy"></param>
        /// <returns></returns>
        public bool Contains(TCoord xy)
        {
            if (xy[0] < MinHorizontal || xy[0] > MaxHorizontal || xy[1] < MinVertical || xy[1] > MaxVertical) return false;
            return true;
        }
        /// <summary>
        /// 是否包含这个线段
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool Contains(LineSegment<TCoord> line)
        {
            return this.Contains(line.CoordA) && this.Contains(line.CoordB);
        }

        /// <summary>
        /// 求交集盒子
        /// </summary>
        /// <returns></returns>
        public IBox<TCoord> And(IBox<TCoord> other)
        {
            if (this.IntersectsWith(other))
            {
                double minX = DoubleUtil.Max(this.MinHorizontal, other.MinHorizontal);
                double minY = DoubleUtil.Max(this.MinVertical, other.MinVertical);
                double maxX = DoubleUtil.Min(this.MaxHorizontal, other.MaxHorizontal);
                double maxY = DoubleUtil.Min(this.MaxVertical, other.MaxVertical);
                return new Box<TCoord>(minX, minY, maxX, maxY);
            }
            return null;
        }

        /// <summary>
        /// Returns true if this instance contains the <see cref="Envelope"/>
        /// </summary>
        /// <param name="box"><see cref="BoundingBox"/></param>
        /// <returns>True it contains</returns>
        public bool Contains(IBox<TCoord> box)
        {
            return (this.MinHorizontal <= box.MinHorizontal) && (this.MinVertical <= box.MinVertical) && (this.MaxHorizontal >= box.MaxHorizontal) && (this.MaxVertical >= box.MaxVertical);
        }

        /// <summary>
        /// 长或宽为 0
        /// </summary>
        public bool IsEmpty { get { return this.Width == 0 || this.Height == 0; } }


        /// <summary>
        /// Determines whether the boundingbox intersects another boundingbox
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public bool IntersectsWith(IBox<TCoord> box)
        {
            return !(box.MinHorizontal > MaxHorizontal || box.MaxHorizontal < MinHorizontal || box.MinVertical > MaxVertical || box.MaxVertical < MinVertical);
        }


        /// <summary>
        /// 合并两个盒子。大盒子为两小盒子的最小并集盒子。
        /// </summary>
        /// <param name="bbox"></param>
        /// <returns></returns>
        public IBox<TCoord> Expands(IBox<TCoord> bbox)
        {
            if (bbox == null)
                return (Box<TCoord>)Clone();
            else
                return new Box<TCoord>(Math.Min(MinHorizontal, bbox.MinHorizontal), Math.Min(MinVertical, bbox.MinVertical),
                                       Math.Max(MaxHorizontal, bbox.MaxHorizontal), Math.Max(MaxVertical, bbox.MaxVertical));
        }
        /// <summary>
        /// 获取一个拷贝。
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new Box<TCoord>(this.MinHorizontal, this.MinVertical, this.MaxHorizontal, this.MaxVertical);
        }

        /// <summary>
        ///扩张倍数。
        /// </summary>
        /// <param name="times"></param>
        /// <returns></returns>
        public IBox<TCoord> Expands(double times)
        {
            return new Box<TCoord>(this.Center, this.Width * times, this.Height * times);
        }
         
        #endregion

        #region override
        /// <summary>
        /// 值相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            //  if (base.Equals(obj)) return true;
            if (!(obj is Box<TCoord>)) return false;

            Box<TCoord> en = (Box<TCoord>)obj;
            if (en.MaxHorizontal == MaxHorizontal && en.MaxVertical == MaxVertical && en.MinHorizontal == MinHorizontal && en.MinVertical == MinVertical)
                return true;

            return false;
        }
        /// <summary>
        /// 值相等
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (int)(37 * MinVertical + 37 * MinHorizontal + 37 * MaxVertical + 37 * MaxHorizontal);
        }

        /// <summary>
        /// 自定义的格式化输出。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Box<TCoord> [maxX=" + MaxHorizontal.ToString("0.#####") + ", maxY=" + MaxVertical.ToString("0.#####") + ", minX=" + MinHorizontal.ToString("0.#####")
                    + ", minY=" + MinVertical.ToString("0.#####") + "]";
        } 
        #endregion
         

        /// <summary>
        /// 坐标转换
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool Transform(ICoordTransformer trans)
        {
            XYZ topRight = new XYZ(this.MaxHorizontal, this.MaxVertical, 0);
            XYZ leftBottom = new XYZ(this.MinHorizontal, this.MinVertical, 0);

            topRight = trans.Trans(topRight);
            leftBottom = trans.Trans(leftBottom);

            this.MaxHorizontal = topRight[0];
            this.MaxVertical = topRight[1];
            this.MinHorizontal = leftBottom[0];
            this.MinVertical = leftBottom[1];

            return true;
        } 


        public TwoDLineSegment<TCoord> Top
        {
            get { return new TwoDLineSegment<TCoord>(LeftTop, RightTop); }
        }

        public TwoDLineSegment<TCoord> Bottom
        {
            get { return new TwoDLineSegment<TCoord>(LeftBottom, RightBottom); }
        }

        public TwoDLineSegment<TCoord> Left
        {
            get { return new TwoDLineSegment<TCoord>(LeftBottom, LeftTop); }
        }

        public TwoDLineSegment<TCoord> Right
        {
            get { return new TwoDLineSegment<TCoord>(RightBottom, RightTop); }
        }
        /// <summary>
        /// 获取这个坐标在相对盒子的方向。如果在盒子内则为 Unkown。
        /// </summary>
        /// <param name="coord">待判断的坐标</param>
        /// <returns></returns>
        public Direction GetDirection(TCoord coord)
        {
            if (this.Contains(coord)) return Direction.UnKnown;
            double x = coord[0];
            double y = coord[1];
            if (y >= this.MaxVertical)
            {
                if (x >= this.MaxHorizontal) return Direction.NorthEast;
                if (x >= this.MinHorizontal && x <= this.MaxHorizontal) return Direction.North;
                if (x <= this.MinHorizontal) return Direction.NorthWest;
            }
            if (y >= this.MinVertical && y <= this.MaxVertical)
            {
                if (x >= this.MaxHorizontal) return Direction.East;
                if (x >= this.MinHorizontal && x <= this.MaxHorizontal) return Direction.UnKnown;
                if (x <= this.MinHorizontal) return Direction.West;
            }
            if (y <=this.MinVertical)
            {
                if (x >= this.MaxHorizontal) return Direction.SouthEast;
                if (x >= this.MinHorizontal && x <= this.MaxHorizontal) return Direction.South;
                if (x <= this.MinHorizontal) return Direction.SouthWest;
            }
            return Direction.UnKnown; 
        }

        /// <summary>
        /// 获取相交点，如果没有，则返回null。
        /// </summary>
        /// <param name="line">线段</param>
        /// <returns></returns>
        public TCoord GetIntersectPoint(TwoDLineSegment<TCoord> line)
        {
            TCoord val = default(TCoord);

            if (!this.IntersectsWith(line.Box)) return val;

            TCoord coord = this.Left.GetIntersectionCoord(line);
            if (coord != null) return coord;
            coord = this.Top.GetIntersectionCoord(line);
            if (coord != null) return coord;
            coord = this.Right.GetIntersectionCoord(line);
            if (coord != null) return coord;
            coord = this.Bottom.GetIntersectionCoord(line);

            return coord;
        }
        /// <summary>
        /// 线段与盒子，可能出现1，包含，2，远离，3穿过，4相交一个边
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public List<TCoord> GetIntersectPoints(TwoDLineSegment<TCoord> line)
        { 
            List<TCoord> list = new List<TCoord>();

            //1包含
            if (this.Contains(line.Box)) return list;
            //2远离
            if (!this.IntersectsWith(line.Box)) return list;

            // 4相交一个边
            if (this.Contains(line.CoordA))//包含A
            {
                list.Add(line.CoordA);
                list.AddRange( GetIntersectPoints(line, line.CoordB));
                return list;
            }
            if (this.Contains(line.CoordB))//包含A
            {
                list.Add(line.CoordB);
                list.AddRange(GetIntersectPoints(line, line.CoordA));
                return list; 
             //   return GetIntersectPoints(line, line.CoordA);
            }
            //3穿过，

           // TCoord currentVal = default(TCoord);

            TCoord coordLeft = this.Left.GetIntersectionCoord(line);
            if (coordLeft != null) list.Add(coordLeft);

            TCoord coordTop = this.Top.GetIntersectionCoord(line);
            if (coordTop != null) list.Add(coordTop);

            TCoord coordRight = this.Right.GetIntersectionCoord(line);
            if (coordRight != null) list.Add(coordRight);

            TCoord coordbottom = this.Bottom.GetIntersectionCoord(line);
            if (coordbottom != null) list.Add(coordbottom);

            return list; 
        }

        private TCoord GetTCoord( params TCoord  [] coords)
        {
            foreach (var item in coords)
            {
                if (item != null) return item;
            }
            return default(TCoord);
        }

        private List<TCoord> GetIntersectPoints(TwoDLineSegment<TCoord> line, TCoord outerCoord)
        {
            List<TCoord> list = new List<TCoord>();
            Direction direction = GetDirection(outerCoord);
            switch (direction)
            {
                case Direction.East:
                    list.Add(this.Right.GetIntersectionCoord(line));
                    return list;
                case Direction.South:
                    list.Add(this.Bottom.GetIntersectionCoord(line));
                    return list;
                case Direction.North:
                    list.Add(this.Top.GetIntersectionCoord(line));
                    return list;
                case Direction.West:
                    list.Add(this.Left.GetIntersectionCoord(line));
                    return list;
                case Direction.NorthEast:
                    TCoord coordA = this.Top.GetIntersectionCoord(line);
                    TCoord coordB = this.Right.GetIntersectionCoord(line);
                    if (coordA != null) list.Add(coordA);
                    if (coordB != null) list.Add(coordB);
                    return list;
                case Direction.NorthWest:
                    TCoord coordA1 = this.Top.GetIntersectionCoord(line);
                    TCoord coordB1 = this.Left.GetIntersectionCoord(line);
                    if (coordA1 != null) list.Add(coordA1);
                    if (coordB1 != null) list.Add(coordB1);
                    return list;
                case Direction.SouthEast:
                    TCoord coordA2 = this.Bottom.GetIntersectionCoord(line);
                    TCoord coordB2 = this.Right.GetIntersectionCoord(line);
                    if (coordA2 != null) list.Add(coordA2);
                    if (coordB2 != null) list.Add(coordB2);
                    return list;
                case Direction.SouthWest:
                    TCoord coordA3 = this.Bottom.GetIntersectionCoord(line);
                    TCoord coordB3 = this.Left.GetIntersectionCoord(line);
                    if (coordA3 != null) list.Add(coordA3);
                    if (coordB3 != null) list.Add(coordB3);
                    return list;
                default:
                    return list;
            }
        }
    }

}
