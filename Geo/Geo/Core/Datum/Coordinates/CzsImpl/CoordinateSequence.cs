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
    public class CoordinateSequence : ICoordinateSequence
    {      
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public CoordinateSequence() : this(new List<ICoordinate>()) { }
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public CoordinateSequence(IEnumerable<ICoordinate> coords)
        {
            Coords = new List<ICoordinate>( coords);
            if (Coords.Count > 0)
                this.ReferenceSystem = Coords[0].ReferenceSystem;
        } 
        /// <summary>
        /// 序列改变了。
        /// </summary>
        public event EventHandler SequenceChanged;

        /// <summary>
        /// 存储实体。
        /// </summary>
        protected List<ICoordinate> Coords{get;set;}

        /// <summary>
        /// 坐标的维数。默认坐标处于同一参考系下，所以坐标的维数相同。
        /// </summary>
        public int Dimension { get { return ReferenceSystem.CoordinateSystem.Dimension; } }

        /// <summary>
        /// 获取或设置某坐标轴的坐标值。      
        /// </summary>
        /// <param name="index">序列中的坐标编号</param>
        /// <param name="ordinate"> 坐标的坐标轴 </param>
        /// <returns></returns>       
        public Double this[Int32 index, Ordinate ordinate]
        {
            get { return Coords[index][ordinate]; }
            set { Coords[index][ordinate] = value; }
        }
        /// <summary>
        /// 坐标所使用的参考系统。没有参考系统的坐标是没有意义的。
        /// </summary>
        public ICoordinateReferenceSystem ReferenceSystem { get; set; }
        /// <summary>
        /// 获取或设置序列中指定编号的坐标。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ICoordinate this[Int32 index]
        {
            get { return Coords[index]; }
            set { Coords[index] = value; }
        }

        /// <summary>
        /// 转换为 <see name="ICoordinate"/> 的数组。
        /// </summary>
        public ICoordinate[] ToArray()
        {
            return Coords.ToArray();
        }

        /// <summary>
        /// 添加一个到序列末尾。
        /// </summary>
        /// <param name="coord"></param>
        public void Add(ICoordinate coord)
        {
            Coords.Add(coord);
            if (SequenceChanged != null) SequenceChanged(this, new EventArgs());
        }
        /// <summary>
        /// 添加到指定位置。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="coord"></param>
        public void Insert(int index, ICoordinate coord)
        {
            Coords.Insert(index, coord);
            if (SequenceChanged != null) SequenceChanged(this, new EventArgs());
        }

        /// <summary>
        /// 合并坐标序列
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public void Merge(ICoordinateSequence other)
        {
            List<ICoordinate> list = new List<ICoordinate>(this.Coords);
            list.AddRange(((CoordinateSequence)other).Coords);
            if (SequenceChanged != null) SequenceChanged(this, new EventArgs());
        }
        /// <summary>
        /// 克隆坐标序列
        /// </summary>
        /// <returns></returns>
        public ICoordinateSequence Clone()
        {
            return new CoordinateSequence(Coords);
        }
        /// <summary>
        /// 在指定精度下，坐标是否相等。
        /// </summary>
        /// <param name="other">待比较的坐标序列</param>
        /// <param name="tolerance">若为空则值为 0 </param>
        /// <returns></returns>
        public Boolean Equals(ICoordinateSequence other, Tolerance tolerance)
        {
            if (!this.ReferenceSystem.Equals(other.ReferenceSystem)) return false;
            if (this.Count != other.Count) return false;
            for (int i = 0; i < Count; i++)
            {
                if (!this[i].Equals(other[i], tolerance)) return false;
            }

            return true;
        }

        //ICoordinateSequence Freeze();
        /// <summary>
        /// 序列中的第一个
        /// </summary>
        public ICoordinate First { get { return  Coords[0]; } }
        /// <summary>
        /// 序列中的最后一个
        /// </summary>
        public ICoordinate Last { get { return Coords[Count - 1]; } }
        //Boolean IsFrozen { get; }
        /// <summary>
        /// 序列大小
        /// </summary>
        public Int32 Count { get { return Coords.Count; } }
        /// <summary>
        /// 距离远点最远的坐标。
        /// </summary>
        public ICoordinate Maximum
        {
            get
            {
                ICoordinate max = First;
                foreach (var item in Coords)
                {
                    if (item.Radius - max.Radius > 0) max = item;
                }
                return max;
            }
        }
        /// <summary>
        /// 距离原点最近的坐标。
        /// </summary>
        public ICoordinate Minimum
        {
            get
            {
                ICoordinate min = First;
                foreach (var item in Coords)
                {
                    if (item.Radius - min.Radius < 0) min = item;
                }
                return min;
            }
        }
        /// <summary>
        /// 本身反序
        /// </summary>
        /// <returns></returns>
        public void Reverse()
        {
            Coords.Reverse();
            if (SequenceChanged != null) SequenceChanged(this, new EventArgs());
        }
        //ICoordinateSequenceFactory CoordinateSequenceFactory { get; }

        //Pair<ICoordinate> SegmentAt(Int32 index);      

        #region IEnumerable
        public IEnumerator<ICoordinate> GetEnumerator()
        {
            return Coords.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Coords.GetEnumerator();
        }
#endregion
    }
}
