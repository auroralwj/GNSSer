//2014.06.04,czs,created

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;
using Geo.Referencing;


namespace Geo.Coordinates
{
    /// <summary>
    /// 通用坐标。 
    /// </summary>
    [Serializable]
    public class Coordinate : Geo.Algorithm.AbstractVector, ICoordinate
    {
        /// <summary>
        /// 默认构造函数。初始化为 Empty。
        /// </summary>
        //public Coordinate():this(null) {}
        /// <summary>
        /// 由参考系统实例化坐标。
        /// </summary>
        /// <param name="referenceSystem">参考系统</param>
        public Coordinate(ICoordinateReferenceSystem referenceSystem, double weight = 0, CoordinateType CoordinateType = CoordinateType.Other)
        {
            this.ReferenceSystem = referenceSystem;
            this.Weight = weight;
            this.CoordDic = new Dictionary<Ordinate, double>();

            foreach (var item in ReferenceSystem.CoordinateSystem.Axes)
            {
                this.CoordDic.Add(item.Ordinate, 0);
            }
        }

        #region 属性
        /// <summary>
        /// 存储坐标的字典。
        /// </summary>
        protected Dictionary<Ordinate, Double> CoordDic { get; set; }
        /// <summary>
        /// 权值。兼容于GeoAPI对应于其 M 变量。
        /// </summary>
        //public double Weight { get; set; }
        /// <summary>
        /// 标签，用于存储一个对象。
        /// </summary>
        //public object Tag { get; set; }
        /// <summary>
        /// 获取或设置坐标轴数值。
        /// </summary>
        /// <param name="axisIndex">坐标轴序号，从 0 开始</param>
        /// <returns></returns>
        public override double this[int axisIndex]
        { 
            get { return  this[ReferenceSystem.CoordinateSystem[axisIndex].Ordinate]; }
            set { this[ReferenceSystem.CoordinateSystem[axisIndex].Ordinate] = value; } 
        }
        /// <summary>
        /// 坐标轴列表。
        /// </summary>
        public List<IAxis> Axes { get { return ReferenceSystem.CoordinateSystem.Axes; } }
        /// <summary>
        /// 坐标的维数。
        /// </summary>
        public override int Dimension { get { return CoordDic.Count; } }
        /// <summary>
        /// 坐标参考系。
        /// </summary>
        public ICoordinateReferenceSystem ReferenceSystem { get; set; }

         

        /// <summary>
        /// 是否为空。若没有指定参考系统，则为空。
        /// </summary>
        public bool IsEmpty
        {
            get { return ReferenceSystem == null; }
        }

        /// <summary>
        /// 获取或设置指定坐标值。
        /// </summary>
        /// <param name="ordinate"></param>
        /// <returns></returns>
        public double this[Ordinate ordinate] { get { return CoordDic[ordinate]; } set { CoordDic[ordinate] = value; } }
        #endregion

        #region 方法
        /// <summary>
        /// 克隆。
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            Coordinate clone = new Coordinate(ReferenceSystem);
            foreach (var item in this.CoordDic.Keys)
            {
                clone[item] = this[item];
            }
            return clone;
        }
        /// <summary>
        /// 比较。与原点的距离。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj is ICoordinate)
            {
                ICoordinate other = obj as ICoordinate;
                return (int)((Distance(Zero) - other.Distance(Zero)) * 1000000);
            }
            return 0;
        } 
        /// <summary>
        /// 是否包含指定坐标
        /// </summary>
        /// <param name="ordinate"></param>
        /// <returns></returns>
        public bool ContainsOrdinate(Ordinate ordinate)
        {
            return CoordDic.ContainsKey(ordinate);
        }
        /// <summary>
        /// 到原点的欧式距离，半径。
        /// </summary>
        public double Radius { get { return Distance(Zero); } }
        /// <summary>
        /// 欧式距离。
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public double Distance(ICoordinate other)
        {
            if (!IsHomogenized(other)) throw new ArgumentException("坐标类型不同，没有可比性", "other");

            double dis = 0;
            foreach (var item in CoordDic)
            {
                dis += Math.Pow(this[item.Key] - other[item.Key], 2.0);
            }

            return Math.Sqrt(dis);
        }


        /// <summary>
        /// 是否为同类坐标，主要是通过参考系判断。
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsHomogenized(ICoordinate other)
        {
            //同为野坐标也为同质坐标
            if (IsEmpty != other.IsEmpty) return false;

            if (!ReferenceSystem.Equals(other.ReferenceSystem)) return false;

            return true;
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            bool isFirst = true;
            foreach (var item in this)
            {
                if (!isFirst)  sb.Append(", ");
                sb.Append(item);
            }
            sb.Append(")");
            return sb.ToString();
        }

        /// <summary>
        /// 数值是否相等。
        /// </summary>
        /// <param name="other">待比较对象</param>
        /// <returns></returns>
        public bool Equals(ICoordinate other)
        {
            return Equals(other, Geo.Coordinates.Tolerance.Default);
        }
         
        /// <summary>
        /// 相同参考系下，数值上是否相等。
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ICoordinate other, Tolerance tolerance)
        {
           //throw  new NullReferenceException("other");

            if (other == null) throw new NullReferenceException("other");

            if (!this.ReferenceSystem.Equals(other.ReferenceSystem)) return false;
            if (this.Dimension != other.Dimension) return false;
            if (this.Weight != other.Weight) return false;

            foreach (var item in this.CoordDic.Keys)
            {
                if (tolerance == null)
                {
                    if (this[item] != other[item]) 
                        return false;
                }
                else
                {
                    if (Math.Abs(this[item] - other[item]) > tolerance.Value)
                        return false;                    
                }
            }
            return true;
        }

        /// <summary>
        /// 相同参考系下，数值上是否相等。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Coordinate other = obj as Coordinate;
            if (other == null) return false;

            bool val = Equals(other, Geo.Coordinates.Tolerance.Default);
            return val;
        }

        /// <summary>
        /// 哈希数。
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int code = ReferenceSystem.GetHashCode() * 13;
            foreach (var item in this.CoordDic.Values)
            {
                code += item.GetHashCode();
            }
            return code;
        }
        #region 枚举器接口
        /// <summary>
        /// 枚举器接口
        /// </summary>
        /// <returns></returns>
        public   IEnumerator<KeyValuePair<Ordinate, double>> GetEnumerator()
        {
            return CoordDic.GetEnumerator();
        }
        /// <summary>
        /// 枚举器接口
        /// </summary>
        /// <returns></returns>
      System.Collections.IEnumerator  System.Collections.IEnumerable.GetEnumerator()
        {
            return CoordDic.GetEnumerator();
        }
        #endregion
        #endregion

        #region 常用坐标
        /// <summary>
        /// 原点。
        /// </summary>
        public ICoordinate Zero
        {
            get { return new Coordinate(ReferenceSystem); }
        }
        #endregion


        public override Algorithm.IVector Create(int count)
        {
            return   new  Coordinate(CoordinateReferenceSystem.Wgs84GeodeticCs);
        }

        public override Algorithm.IVector Create(double[] array)
        {
            return new Coordinate(CoordinateReferenceSystem.Wgs84GeodeticCs);
        }
       
    }
}
