//2014.05.31, czs, created 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Common;

namespace Geo.Referencing
{
   
    /// <summary>
    /// 大地坐标系基准，定义了参考椭球和首子午线。
    /// </summary>
    public class GeodeticDatum : Datum, IGeodeticDatum
    {
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public GeodeticDatum() :
            this(Ellipsoid.WGS84, PrimeMeridian.Greenwich, new BursaTransParams(), "WGS84") { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ellipsoid">椭球体</param>
        /// <param name="primeMeridian">首子午线</param>
        /// <param name="name">名称</param>
        /// <param name="bursaParamsToWGS84">转换参数</param>
        /// <param name="id">ID</param>
        public GeodeticDatum(Ellipsoid ellipsoid, PrimeMeridian primeMeridian, BursaTransParams bursaParamsToWGS84 = null, string name = null, string id = null)
            : base(name, id)
        {
            this.Ellipsoid = ellipsoid;
            this.PrimeMeridian = primeMeridian;
            this.TransParamsToWgs84 = bursaParamsToWGS84;
        }
        /// <summary>
        /// 向WGS84转换的七参数。
        /// </summary>
        public BursaTransParams TransParamsToWgs84 { get; set; }
        /// <summary>
        /// 参考椭球。
        /// </summary>
        public Ellipsoid Ellipsoid { get; set; }
        /// <summary>
        /// 首子午线
        /// </summary>
        public PrimeMeridian PrimeMeridian { get; set; }

        /// <summary>
        /// 若椭球与基准相等则相等。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            GeodeticDatum datum = obj as GeodeticDatum;
            if (datum == null) return false;

            bool val = datum.Ellipsoid.Equals(this.Ellipsoid)
                && datum.PrimeMeridian.Equals(this.PrimeMeridian);
            return val;
        }

        public override int GetHashCode()
        {
            return Ellipsoid.GetHashCode() * 3 + PrimeMeridian.GetHashCode() * 13;
        }
        /// <summary>
        /// 默认以逗号隔开的字符串。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = "";
            if (Name != null) str += Name;
            else str = Ellipsoid + "";
            return str;
        }
        #region 常用大地基准
        /// <summary>
        /// WGS 84
        /// </summary>
        public static GeodeticDatum WGS84
        {
            get
            {
                return new GeodeticDatum(Ellipsoid.WGS84, PrimeMeridian.Greenwich, new BursaTransParams(), "WGS84");
            }
        }
        /// <summary>
        /// WGS72大地基准
        /// </summary>
        public static GeodeticDatum WGS72
        {
            get
            {
                GeodeticDatum datum =
                    new GeodeticDatum(
                         Ellipsoid.WGS72,
                         PrimeMeridian.Greenwich,
                         BursaTransParams.WGS72ToWGS84,//   new BursaTransParams(0, 0, 4.5, 0, 0, 0.554, 0.219),
                        "WGS72");
                return datum;
            }
        }

        #endregion

    }
}
