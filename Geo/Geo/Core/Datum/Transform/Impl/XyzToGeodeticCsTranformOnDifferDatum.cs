//2014.06.09, czs, create

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Referencing;

namespace Geo.Coordinates
{
    /// <summary>
    /// 在不同椭球下，空间直角坐标系向大地坐标系统的转换（要进行椭球基准的转换）。
    /// </summary>
    public class XyzToGeodeticCsTranformOnDifferDatum : AbstractCompositCoordTranform
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sourceCrs">待转换坐标系统，类型是XyzCs才符合</param>
        /// <param name="targetCrs">目标坐标系统，类型是GeodeticCs才符合</param>
        public XyzToGeodeticCsTranformOnDifferDatum(ICoordinateReferenceSystem sourceCrs, ICoordinateReferenceSystem targetCrs)
            : base(sourceCrs, targetCrs)
        { 
            //第一个节点,统一基准
            ICoordinateReferenceSystem middleCrs = new CrsFactory().Create(CoordinateSystem.XyzCs, this.TargetCrs.Datum);
            this.TransformChain = new GeodeticDatumTranform(this.SourceCrs, middleCrs);
            //第二个节点，转换到大地坐标
            AbstractCoordTranform xyzToGeo = new XyzToGeodeticCsTranform(middleCrs, this.TargetCrs);
            //设置链条
            this.TransformChain.Successor = (xyzToGeo); 
        }

        /// <summary>
        /// 逆向转换
        /// </summary>
        /// <returns></returns>
        public override ICrsTranform GetInverse()
        {
            return new GeodeticToXyzCsTranformOnDifferDatum(this.TargetCrs, this.SourceCrs);
        }

        //组合中都有判断，在此是否多此一举？或者这里显得更加简便。
        ///// <summary>
        ///// 是否是我的菜。
        ///// </summary>
        ///// <param name="sourceCrs">参考系</param>
        ///// <returns></returns>
        //public override bool IsMatched
        //{
        //    get
        //    {
        //        bool match = SourceCrs.CoordinateSystem.CoordinateType == CoordinateType.XYZ
        //            && TargetCrs.CoordinateSystem.CoordinateType == CoordinateType.LonLatHeight
        //            && SourceCrs.Datum is IGeodeticDatum
        //            && TargetCrs.Datum is IGeodeticDatum;
        //        return match;
        //    }
        //}

        /// <summary>
        /// 转换操作。输入待转坐标，输出目标坐标。
        /// </summary>
        /// <param name="oldCoord">待转坐标，只取其数字部分，参考系取自属性本对象的TargetCrs属性 </param>
        /// <returns></returns>
        public override ICoordinate MatchedTrans(ICoordinate oldCoord)
        {
            return TransformChain.Trans(oldCoord);
        }
    }
}
