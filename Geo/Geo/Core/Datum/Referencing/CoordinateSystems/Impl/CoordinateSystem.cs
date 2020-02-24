//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Common;
using System.Globalization;

namespace Geo.Referencing
{
    /// <summary>
    /// 坐标系统是描述物质存在的空间位置（坐标）的参照系，通过定义特定基准及其参数形式来实现。
    /// 为了方便书写，其子类简称为CS。
    /// </summary>
    [Serializable]
    public class CoordinateSystem : IdentifiedObject, ICoordinateSystem
    {
        /// <summary>
        /// 坐标系统。默认构造函数
        /// </summary>
        public CoordinateSystem()
            : this(new List<IAxis>())
        { }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="axes">坐标轴</param>
        /// <param name="name">坐标系统名称</param>
        /// <param name="id">坐标系统编号</param>
        public CoordinateSystem(IEnumerable<IAxis> axes, string name = null, string id = null)
            : base(id, name)
        {
            this.Axes = new List<IAxis>(axes);

            //组合类型赋值
            this.CoordinateType = Referencing.CoordinateType.Other;
            if (this.Contains(Ordinate.X, Ordinate.Y))
            {
                this.CoordinateType = CoordinateType.XY;
                if (Contains(Ordinate.Z))
                    this.CoordinateType = CoordinateType.XYZ;
            }
            else if (this.Contains(Ordinate.Lon, Ordinate.Lat))
            {
                this.CoordinateType = CoordinateType.LonLat;
                if (Contains(Ordinate.Height))
                    this.CoordinateType = CoordinateType.LonLatHeight;
            }
        }

        /// <summary>
        /// 坐标分量的组合类型。
        /// </summary>
        public CoordinateType CoordinateType { get; protected set; }

        /// <summary>
        /// 坐标轴检索。
        /// </summary>
        /// <param name="axisIndex">编号，从0开始</param>
        /// <returns></returns>
        public IAxis this[int axisIndex] { get { return Axes[axisIndex]; } set { Axes[axisIndex] = value; } }

        /// <summary>
        /// 坐标系统维数。
        /// </summary>
        public int Dimension { get { return this.Axes.Count; } }

        /// <summary>
        /// 坐标轴集合。
        /// </summary>
        public List<IAxis> Axes { get; set; }

        /// <summary>
        /// 是否包含指定坐标轴。
        /// </summary>
        /// <param name="ordinate">坐标轴</param>
        /// <returns>返回真相</returns>
        public bool Contains(Ordinate ordinate)
        {
            return Axes.Exists(m => m.Ordinate == ordinate);
        }
        /// <summary>
        /// 是否包含指定坐标轴。
        /// </summary>
        /// <param name="ordinates">坐标轴</param>
        /// <returns>返回真相</returns>
        public bool Contains(params Ordinate[] ordinates)
        {
            foreach (var item in ordinates)
            {
                if (!this.Contains(item)) return false;
            }
            return true;
        }
        /// <summary>
        /// 坐标系统是否相等，主要比较坐标轴。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            CoordinateSystem cs = obj as CoordinateSystem;
            if (cs == null) return false;

            if (Dimension != cs.Dimension) return false;
            for (int i = 0; i < Dimension; i++)
            {
                if (!Axes[i].Equals(cs.Axes[i])) return false;
            }

            return true;
        }
        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int code = 0;
            for (int i = 0; i < Dimension; i++)
            {
                code += Axes[i].GetHashCode() * (i + 1) * 3;
            }
            return code;
        }
        /// <summary>
        /// 默认以逗号隔开的字符串。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = "";
            if (Name != null) str += Name;
            else str = CoordinateType + "";
            return str;
        }

        #region 迭代器接口实现
        /// <summary>
        /// 返回循环访问的枚举器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IAxis> GetEnumerator()
        {
            return Axes.GetEnumerator();
        }
        /// <summary>
        /// 返回循环访问的枚举器
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Axes.GetEnumerator();
        }
        #endregion

        #region 常用坐标系统
        /// <summary>
        /// 坐标轴分别为 X Y 的 2 维坐标系统。 尺度为米。
        /// </summary>
        public static CoordinateSystem XyCs = new CoordinateSystem(
                    new List<IAxis>() { Axis.X, Axis.Y },
                    "XyCs");
        /// <summary>
        /// 坐标轴分别为 X Y Z 的三维坐标系统。 尺度为米。
        /// </summary>
        public static CoordinateSystem XyzCs = new CoordinateSystem(
                    new List<IAxis>() { Axis.X, Axis.Y, Axis.Z },
                    "XyzCs");
        /// <summary>
        /// 坐标轴分别为 Lon Lat 的 2 维坐标系统。 尺度为度。
        /// </summary>
        public static CoordinateSystem LonLatCs = new CoordinateSystem(
                    new List<IAxis>() { Axis.Lon, Axis.Lat },
                    "LonLatCS");
        /// <summary>
        /// 坐标轴分别为 Lon Lat Heigt 的 3 维坐标系统。 Lon Lat 尺度为度, Height 为米。
        /// </summary>
        public static CoordinateSystem GeodeticCs = new CoordinateSystem(
                    new List<IAxis>() { Axis.Lon, Axis.Lat, Axis.Height },
                    "GeodeticCS");
        /// <summary>
        /// 球面坐标
        /// </summary>
        public static CoordinateSystem SphereCs = new CoordinateSystem(
                    new List<IAxis>() { Axis.Lon, Axis.Lat, Axis.Radius },
                    "SphereCS");
        /// <summary>
        /// 站心坐标 NEU
        /// </summary>
        public static CoordinateSystem NeuCs = new CoordinateSystem(
                    new List<IAxis>() { Axis.North, Axis.East, Axis.Up },
                    "NeuCS");
        public static CoordinateSystem EnuCs = new CoordinateSystem(
                    new List<IAxis>() { Axis.East, Axis.North, Axis.Up },
                    "EnuCs");
        /// <summary>
        /// 测站坐标 HEN
        /// </summary>
        public static CoordinateSystem HenCs = new CoordinateSystem(
                    new List<IAxis>() { Axis.Height, Axis.East, Axis.North },
                    "HenCS");
        /// <summary>
        /// RadiusAzimuthZenithAngle
        /// </summary>
        public static CoordinateSystem PolorCs = new CoordinateSystem(
                    new List<IAxis>() { Axis.Radius, Axis.Azimuth, Axis.ElevatAngle },
                    "PolorCs");
        /// <summary>
        /// RadiusAzimuth
        /// </summary>
        public static CoordinateSystem PlanePolorCs = new CoordinateSystem(
                    new List<IAxis>() { Axis.Radius, Axis.Azimuth },
                    "PlanePolorCs");
        #endregion


    }
}