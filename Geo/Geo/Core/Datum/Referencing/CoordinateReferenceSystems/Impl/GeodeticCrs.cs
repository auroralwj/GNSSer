//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Referencing
{
    /// <summary>
    /// 地固系，Terrestrial Reference System， Earth-fiexd
    /// 基于大地基准的坐标系统参照系，通常有地理坐标参照系和地心坐标参照系。
    /// </summary>
    public class GeodeticCrs : SingleCrs
    {
        /// <summary>
        /// 初始化一个实例。
        /// </summary>
        public GeodeticCrs() { }

        /// <summary>
        /// 初始化大地参考系。
        /// </summary>
        /// <param name="coordSys">坐标系统</param>
        /// <param name="datum">基准</param>
        /// <param name="name">名称</param>
        public GeodeticCrs(ICoordinateSystem coordSys, IGeodeticDatum datum, string name = null)
        :base(coordSys,datum,name){ }

        /// <summary>
        /// 大地基准。
        /// </summary>
        public new IGeodeticDatum Datum { get; set; }

        #region 常用

        /// <summary>
        /// WGS84 坐标参考系
        /// </summary>
        public static GeodeticCrs WGS84
        {
            get
            {
                return new GeodeticCrs(Referencing.CoordinateSystem.GeodeticCs, GeodeticDatum.WGS84, "WGS84_CRS");
            }
        }

        #endregion
    }
}