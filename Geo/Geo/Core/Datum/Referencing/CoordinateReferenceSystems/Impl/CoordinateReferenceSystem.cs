//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Referencing
{
    /// <summary>
    /// 坐标参照系统，通常由坐标系统和坐标基准构成。
    /// 由于坐标参照系的实现虽然严密，但是非常抽象，不易于使用，因而采用参考框架进行实现。
    /// 坐标参照系的实现——参考框架，即一组具有相应参照系下坐标及其时间演变的点。
    /// 只有提供了坐标参照系的坐标系统的坐标采用意义。
    /// 提供原点、尺度、定向及其时间演变的一组协议、算法和常数（IERS）。
    /// 相比基准（Datum），参考系的内涵和外延更广。
    /// </summary>
    public class CoordinateReferenceSystem : IdentifiedObject, ICoordinateReferenceSystem
    {
        #region 构造函数
        /// <summary>
        ///  创建一个实例。需要随后指定坐标系统和基准。
        /// </summary>
        public CoordinateReferenceSystem() { }
        /// <summary>
        /// 创建一个实例。
        /// </summary>
        /// <param name="coordinateSystem"></param>
        /// <param name="datum"></param>
        public CoordinateReferenceSystem(ICoordinateSystem coordinateSystem, IDatum datum, string name = null, string id = null)
            : base(id, name)
        {
            this.CoordinateSystem = coordinateSystem;
            this.Datum = datum;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 坐标系统
        /// </summary>
        public ICoordinateSystem CoordinateSystem { get; set; }
        /// <summary>
        /// 基准，计量参照。
        /// </summary>
        public IDatum Datum { get; set; }
        #endregion

        #region 覆盖方法
        /// <summary>
        /// 内容是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            CoordinateReferenceSystem other = obj as CoordinateReferenceSystem;
            if (other == null) return false;

            bool val = CoordinateSystem.Equals(other.CoordinateSystem)
                && Datum.Equals(other.Datum);
            return val;
        }
        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return CoordinateSystem.GetHashCode() * 3 + Datum.GetHashCode() * 13;
        }
        /// <summary>
        /// 可读描述
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Datum: "+ Datum.ToString()+", CoordSystem:"+ CoordinateSystem.ToString();
        }

        #endregion

        #region 常用的坐标参考系统
        #region 椭球，大地
        /// <summary>
        /// 以WGS72基准的空间直角坐标参考系。
        /// </summary>
        public static CoordinateReferenceSystem Wgs72XyzCs { get { return new CoordinateReferenceSystem(Referencing.CoordinateSystem.XyzCs, GeodeticDatum.WGS72); } }
        /// <summary>
        /// 以WGS72基准的大地坐标参考系。
        /// </summary>
        public static CoordinateReferenceSystem Wgs72GeodeticCs { get { return new CoordinateReferenceSystem(Referencing.CoordinateSystem.GeodeticCs, GeodeticDatum.WGS72); } }
        /// <summary>
        /// 以WGS84基准的空间直角坐标参考系。
        /// </summary>
        public static CoordinateReferenceSystem Wgs84XyzCs { get { return new CoordinateReferenceSystem(Referencing.CoordinateSystem.XyzCs, GeodeticDatum.WGS84); } }
        /// <summary>
        /// 以WGS84基准的大地坐标参考系。
        /// </summary>
        public static CoordinateReferenceSystem Wgs84GeodeticCs { get { return new CoordinateReferenceSystem(Referencing.CoordinateSystem.GeodeticCs, GeodeticDatum.WGS84); } }
        #endregion   
 
        /// <summary>
        /// 站心坐标的参考系
        /// </summary>
        public static CoordinateReferenceSystem NeuCrs { get { return new CoordinateReferenceSystem(Referencing.CoordinateSystem.NeuCs, new Datum()); } }
     
        /// <summary>
        /// 站心坐标的参考系
        /// </summary>
        public static CoordinateReferenceSystem HenCrs { get { return new CoordinateReferenceSystem(Referencing.CoordinateSystem.HenCs, new Datum()); } }
        /// <summary>
        /// 站心坐标Enu的参考系
        /// </summary>
        public static CoordinateReferenceSystem EnuCrs { get { return new CoordinateReferenceSystem(Referencing.CoordinateSystem.EnuCs, new Datum()); } }
    

         /// <summary>
        /// 站心坐标的参考系
        /// </summary>
        public static CoordinateReferenceSystem SphereCrs { get { return new CoordinateReferenceSystem(Referencing.CoordinateSystem.SphereCs, new Datum()); } }
   

         /// <summary>
        /// 站心坐标的参考系
        /// </summary>
        public static CoordinateReferenceSystem PolorCrs { get { return new CoordinateReferenceSystem(Referencing.CoordinateSystem.PolorCs, new Datum()); } }
         /// <summary>
        /// 站心坐标的参考系
        /// </summary>
        public static CoordinateReferenceSystem PlanePolorCrs { get { return new CoordinateReferenceSystem(Referencing.CoordinateSystem.PlanePolorCs, new Datum()); } }
   

        #endregion
    }
}