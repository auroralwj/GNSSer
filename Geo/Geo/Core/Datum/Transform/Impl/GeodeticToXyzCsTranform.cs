using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Referencing;

namespace Geo.Coordinates
{
    /// <summary>
    /// 空间直角坐标系向大地坐标系统的转换,而椭球基准不变，不进行椭球的转换。
    /// </summary>
    public class GeodeticToXyzCsTranform : AbstractCoordTranform
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="geodeticCrs">待转换坐标系统，类型必须是GeodeticCs</param>
        /// <param name="xyzCrs">目标坐标系统，类型必须是XyzCs</param>
        public GeodeticToXyzCsTranform(ICoordinateReferenceSystem geodeticCrs, ICoordinateReferenceSystem xyzCrs)
            : base(geodeticCrs, xyzCrs)
        {
        }

        /// <summary>
        /// 逆向转换
        /// </summary>
        /// <returns></returns>
       public override ICrsTranform GetInverse()
       {
           return new XyzToGeodeticCsTranform(TargetCrs, SourceCrs);
       }

       public override bool IsMatched
       {
           get
           {
               bool match = SourceCrs.CoordinateSystem.CoordinateType == CoordinateType.LonLatHeight
                   && TargetCrs.CoordinateSystem.CoordinateType == CoordinateType.XYZ
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
           IEllipsoid e = (TargetCrs.Datum as GeodeticDatum).Ellipsoid;

           IGeodeticCoord geo = (IGeodeticCoord)oldCoord;
           double x, y, z;
           GeodeticUtils.GeodeticToXyzCoord(geo.Lon, geo.Lat, geo.Height, out x, out y, out z, e.SemiMajorAxis, e.InverseFlattening);

           return CoordinateFactory.CreateXyzCoord(x, y, z, oldCoord.Weight);
       }
    }
}
