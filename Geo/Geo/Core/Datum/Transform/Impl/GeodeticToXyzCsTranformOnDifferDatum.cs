//2014.06.13, czs, create

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Referencing;

namespace Geo.Coordinates
{
    /// <summary>
    /// 空间直角坐标系向大地坐标系统的转换,而椭球基准变，要进行椭球的转换。
    /// 组合模式。具体实现采用责任链。
    /// </summary>
    public class GeodeticToXyzCsTranformOnDifferDatum : AbstractCompositCoordTranform
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="geodeticCrs">待转换坐标系统，类型必须是GeodeticCs</param>
        /// <param name="xyzCrs">目标坐标系统，类型必须是XyzCs</param>
        public GeodeticToXyzCsTranformOnDifferDatum(ICoordinateReferenceSystem geodeticCrs, ICoordinateReferenceSystem xyzCrs)
            : base(geodeticCrs, xyzCrs)
        {
            //第一个节点，统一坐标到XYZ
            ICoordinateReferenceSystem middleCrs = new CrsFactory().Create(CoordinateSystem.XyzCs, this.SourceCrs.Datum);
            TransformChain = new GeodeticToXyzCsTranform(this.SourceCrs, middleCrs);
            //第二个节点
            AbstractCoordTranform datumTrans = new GeodeticDatumTranform(middleCrs, this.TargetCrs);
            //设置链条
            TransformChain.Successor = (datumTrans);
        }
     
        /// <summary>
        /// 逆向转换
        /// </summary>
        /// <returns></returns>
       public override ICrsTranform GetInverse()
       {
           return new XyzToGeodeticCsTranformOnDifferDatum(TargetCrs, SourceCrs);
       }

       //组合中都有判断，在此是否多此一举？或者这里显得更加简便。
       //public override bool IsMatched
       //{
       //    get
       //    {
       //        bool match = SourceCrs.CoordinateSystem.CoordinateType == CoordinateType.LonLatHeight
       //            && TargetCrs.CoordinateSystem.CoordinateType == CoordinateType.XYZ
       //            && SourceCrs.Datum is IGeodeticDatum
       //            && TargetCrs.Datum is IGeodeticDatum;

       //        return match;
       //    }
       //}
       /// <summary>
       /// 转换操作。输入待转坐标，输出目标坐标。
       /// 责任链。
       /// </summary>
       /// <param name="oldCoord">待转坐标，只取其数字部分，参考系取自属性本对象的TargetCrs属性 </param>
       /// <returns></returns>
       public override ICoordinate MatchedTrans(ICoordinate oldCoord)
       {           
           return TransformChain.Trans(oldCoord);
       }
    }
}
