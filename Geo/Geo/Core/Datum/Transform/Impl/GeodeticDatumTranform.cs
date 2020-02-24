//2014.06.09, czs, created

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Referencing;

namespace Geo.Coordinates
{
    /// <summary>
    /// 不同大地基准，相同坐标系（XYZ）的坐标转换。
    /// </summary>
    public class GeodeticDatumTranform : AbstractCoordTranform
    {
        /// <summary>
        /// 大地基准转换实例
        /// </summary>
        /// <param name="sourceDatum">原大地基准</param>
        /// <param name="targetCrs">目标大地基准</param>
        public GeodeticDatumTranform(ICoordinateReferenceSystem sourceCrs, ICoordinateReferenceSystem targetCrs)
            : base(sourceCrs, targetCrs)
        {
        } 

        /// <summary>
        /// 责任判断。
        /// 1.必须为空间直角坐标；
        /// 2.必须为大地基准
        /// </summary>
        /// <param name="sourceCrs"></param>
        /// <returns></returns>
        public override bool IsMatched
        {
            get
            {
                bool match = SourceCrs.CoordinateSystem.CoordinateType == CoordinateType.XYZ
                    && this.TargetCrs.CoordinateSystem.CoordinateType == CoordinateType.XYZ
                && (SourceCrs.Datum is IGeodeticDatum)
                && (TargetCrs.Datum is IGeodeticDatum);

                return match;
            }
        }
        /// <summary>
        /// 得到逆向转换
        /// </summary>
        /// <returns></returns>
        public override ICrsTranform GetInverse()
        {
            return new GeodeticDatumTranform(TargetCrs, SourceCrs);
        }

        /// <summary>
        /// 只有基准不同的 XYZ 坐标转换,采用布尔沙 7 参数模型。
        /// 以WGS84转换参数作为中间参数，共转换 2 次
        /// </summary>
        /// <param name="oldCoord">待转坐标，只取其数字部分，参考系取自属性本对象的TargetCrs属性 </param>
        /// <returns></returns>
        public override ICoordinate MatchedTrans(ICoordinate oldCoord)
        {
            BursaTransParams BursaTransParams = ((IGeodeticDatum)SourceCrs.Datum).TransParamsToWgs84;
            IXYZ temp = GeodeticUtils.BursaTransform((IXYZ)oldCoord, BursaTransParams);

            //double[] result = BursaTransParams.Transform(new double[] { oldCoord.X, oldCoord.Y, oldCoord.Z });
      
            IXYZ xyz = temp;
            if (!TargetCrs.Datum.Equals(GeodeticDatum.WGS84))
                xyz = GeodeticUtils.BursaTransform(temp, ((IGeodeticDatum)TargetCrs.Datum).TransParamsToWgs84.GetInverse()); 

            return CoordinateFactory.CreateXyzCoord(xyz.X, xyz.Y, xyz.Z);
        }
    }
}
