//2014.06.09, czs, create

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Referencing;

namespace Geo.Coordinates
{
    /// <summary>
    /// 在同一椭球下，空间直角坐标系向大地坐标系统的转换（不进行椭球基准的转换）。
    /// </summary>
    public class XyzToGeodeticCsTranform : AbstractCoordTranform
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sourceCrs">待转换坐标系统，类型是XyzCs才符合</param>
        /// <param name="targetCrs">目标坐标系统，类型是GeodeticCs才符合</param>
        public XyzToGeodeticCsTranform(ICoordinateReferenceSystem sourceCrs, ICoordinateReferenceSystem targetCrs)
            : base(sourceCrs, targetCrs)
        {
        }

        /// <summary>
        /// 逆向转换
        /// </summary>
        /// <returns></returns>
        public override ICrsTranform GetInverse()
        {
            return new GeodeticToXyzCsTranform(this.TargetCrs, this.SourceCrs);
        }

        /// <summary>
        /// 是否是我的菜。
        /// </summary> 
        /// <returns></returns>
        public override bool IsMatched
        {
            get
            {
                bool match = SourceCrs.CoordinateSystem.CoordinateType == CoordinateType.XYZ
                    && TargetCrs.CoordinateSystem.CoordinateType == CoordinateType.LonLatHeight
                    && SourceCrs.Datum is IGeodeticDatum
                    && TargetCrs.Datum is IGeodeticDatum
                    && TargetCrs.Datum.Equals(SourceCrs.Datum);


                return match;
            }
        }

        /// <summary>
        /// 转换操作。输入待转坐标，输出目标坐标。
        /// </summary>
        /// <param name="oldCoord">待转坐标，只取其数字部分，参考系取自属性本对象的TargetCrs属性 </param>
        /// <returns></returns>
        public override ICoordinate MatchedTrans(ICoordinate oldCoord)
        {
            throw new Exception();

            //IEllipsoid e = (TargetCrs.Datum as GeodeticDatum).Ellipsoid;
            //IXYZ xyz = (IXYZ)oldCoord;
            //double lon, lat, height;
            //GeodeticUtils.XyzToGeodeticCoord(xyz.X, xyz.Y, xyz.Z, out lon, out lat, out height, e.SemiMajorAxis, e.InverseFlattening);

           // return CoordinateFactory.CreateGeodeticCoord(lon, lat, height, oldCoord.Weight);
        }
    }
}
