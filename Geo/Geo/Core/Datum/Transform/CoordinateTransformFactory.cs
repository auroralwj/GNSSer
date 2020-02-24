//2014.06.12,czs,creat

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Referencing;

namespace Geo.Coordinates
{
    /// <summary>
    /// 通用坐标转换器。推荐采用此类创建坐标转换器。
    /// 本类根据输入和输出参考系的类型，自动选择合适的转换器，并可以组成责任链。
    /// </summary>
    public class CoordinateTransformFactory
    {
        /// <summary>
        /// 实例化一个通用坐标转换器，自动判断参考系的类型，组成责任链。
        /// </summary>
        /// <param name="sourceCrs">待转参考系</param>
        /// <param name="targetCrs">目标参考系</param>
        public CoordinateTransformFactory(ICoordinateReferenceSystem sourceCrs, ICoordinateReferenceSystem targetCrs)
        {
            this.SourceCrs = sourceCrs;
            this.TargetCrs = targetCrs;                       
        }
        /// <summary>
        /// 由输入输出的参考系类型，自动选择合适的转换器，并可以组成责任链。
        /// </summary>
        /// <returns></returns>
        public ICrsTranform Create(
            ICoordinateReferenceSystem sourceCrs = null,
            ICoordinateReferenceSystem targetCrs = null)
        {
            if (sourceCrs == null) sourceCrs = SourceCrs;
            if (targetCrs == null) targetCrs = TargetCrs;

            //判断并设置转换器
            //现在只支持几种情况：
            //1.椭球基准不变，大地坐标与空间直角坐标的转换；
            //2.在空间直角坐标下，转换椭球基准；
            //3.以上两种转换的组合。
            if (sourceCrs.Datum is IGeodeticDatum && targetCrs.Datum is IGeodeticDatum)//1.大地基准下。
            {
                if (sourceCrs.Datum.Equals(targetCrs.Datum))//1.1 大地基准相同
                {
                    //1.1.1 LonLatHeight  ->  XYZ
                    if (sourceCrs.CoordinateSystem.CoordinateType == CoordinateType.LonLatHeight
                        && targetCrs.CoordinateSystem.CoordinateType == CoordinateType.XYZ)
                    {
                        return CreateGeodeticToXyzCsTranform();
                    }
                    //1.1.2 XYZ  ->  LonLatHeight
                    if (sourceCrs.CoordinateSystem.CoordinateType == CoordinateType.XYZ
                        && targetCrs.CoordinateSystem.CoordinateType == CoordinateType.LonLatHeight)
                    {
                        return CreateXyzToGeodeticCsTranform();
                    }
                }
                else//1.2 大地基准不同
                {
                    //1.2.1 坐标系相同，则基准（椭球）转换 XYZ -> XYZ
                    if (sourceCrs.CoordinateSystem.CoordinateType == CoordinateType.XYZ
                        && targetCrs.CoordinateSystem.CoordinateType == CoordinateType.XYZ)
                    {
                        return CreateGeodeticDatumTranform();
                    }
                    //1.2.2 大地基准不同，坐标系统也不同,源为大地坐标系，目标为空间直角坐标系。
                    //      应该首先统一坐标系，然后统一大地基准
                    if (sourceCrs.CoordinateSystem.CoordinateType == CoordinateType.LonLatHeight
                        && targetCrs.CoordinateSystem.CoordinateType == CoordinateType.XYZ)
                    {
                        return CreateGeodeticToXyzCsTransformOnDifferDatum();
                    }
                    //1.2.3 大地基准不同，坐标系统也不同。源为空间直角坐标系，目标为大地坐标系。
                    if (sourceCrs.CoordinateSystem.CoordinateType == CoordinateType.XYZ
                        && targetCrs.CoordinateSystem.CoordinateType == CoordinateType.LonLatHeight)
                    {
                        return CreateXyzToGeodeticCsTransformOnDifferDatum();
                    }
                }
            } 
            //兜底条款
            throw new ArgumentException("我们很遗憾的告诉你：当前本程序，还不支持您所输入参考系之间的转换。进一步信息请联系我们。");
        }
       

        /// <summary>
        /// 待转参考系
        /// </summary>
        public ICoordinateReferenceSystem SourceCrs { get; set; }
        /// <summary>
        /// 目标参考系
        /// </summary>
        public ICoordinateReferenceSystem TargetCrs { get; set; }

        #region 创建
        /// <summary>
        ///  大地基准不同，坐标系统也不同。源为空间直角坐标系，目标为大地坐标系。
        /// </summary>
        /// <returns></returns>
        public ICrsTranform CreateXyzToGeodeticCsTransformOnDifferDatum(
            ICoordinateReferenceSystem sourceCrs = null,
            ICoordinateReferenceSystem targetCrs = null)
        {
            if (sourceCrs == null) sourceCrs = SourceCrs;
            if (targetCrs == null) targetCrs = TargetCrs;
            return new XyzToGeodeticCsTranformOnDifferDatum(sourceCrs, targetCrs);

            //ICrsFactory crsFac = new CrsFactory();
            ////第一个节点,统一基准
            //ICoordinateReferenceSystem middleCrs = crsFac.Create(CoordinateSystem.XyzCs, this.TargetCrs.Datum);
            //AbstractCoordTranform datumTrans = CreateGeodeticDatumTranform(this.SourceCrs, middleCrs);
            ////第二个节点，转换到大地坐标
            //AbstractCoordTranform xyzToGeo = CreateXyzToGeodeticCsTranform(middleCrs, this.TargetCrs);
            ////设置链条
            //datumTrans.Successor = (xyzToGeo);
            //return datumTrans;
        }
        /// <summary>
        ///  大地基准不同，坐标系统也不同,源为大地坐标系，目标为空间直角坐标系。
        //      应该首先统一坐标系，然后统一大地基准
        /// </summary>
        /// <returns></returns>
        public ICrsTranform CreateGeodeticToXyzCsTransformOnDifferDatum(
            ICoordinateReferenceSystem sourceCrs = null,
            ICoordinateReferenceSystem targetCrs = null)
        {
            if (sourceCrs == null) sourceCrs = SourceCrs;
            if (targetCrs == null) targetCrs = TargetCrs;
            return new GeodeticToXyzCsTranformOnDifferDatum(sourceCrs, targetCrs);

            //ICrsFactory crsFac = new CrsFactory();
            ////第一个节点，统一坐标到XYZ
            //ICoordinateReferenceSystem middleCrs = crsFac.Create(CoordinateSystem.XyzCs, this.SourceCrs.Datum);
            //AbstractCoordTranform geoToXyz = CreateGeodeticToXyzCsTranform(this.SourceCrs, middleCrs);
            ////第二个节点
            //AbstractCoordTranform datumTrans = CreateGeodeticDatumTranform(middleCrs, this.TargetCrs);
            ////设置链条
            //geoToXyz.Successor = (datumTrans);
            //return geoToXyz;
        }

        #region 简单创建
        /// <summary>
        /// 大地坐标系向空间直角坐标系转换
        /// </summary>
        /// <param name="sourceCrs">待转参考系</param>
        /// <param name="targetCrs">目标参考系</param>
        /// <returns></returns>
        public AbstractCoordTranform CreateGeodeticToXyzCsTranform(
            ICoordinateReferenceSystem sourceCrs = null,
            ICoordinateReferenceSystem targetCrs = null)
        {
            if (sourceCrs == null) sourceCrs = SourceCrs;
            if (targetCrs == null) targetCrs = TargetCrs;
            return new GeodeticToXyzCsTranform(sourceCrs, targetCrs);
        }

        /// <summary>
        /// 空间直角坐标系转换为大地坐标系
        /// </summary>
        /// <param name="sourceCrs">待转参考系</param>
        /// <param name="targetCrs">目标参考系</param>
        /// <returns></returns>
        public AbstractCoordTranform CreateXyzToGeodeticCsTranform(
            ICoordinateReferenceSystem sourceCrs = null,
            ICoordinateReferenceSystem targetCrs = null)
        {
            if (sourceCrs == null) sourceCrs = SourceCrs;
            if (targetCrs == null) targetCrs = TargetCrs;
            return new XyzToGeodeticCsTranform(sourceCrs, targetCrs);
        }

        /// <summary>
        /// 椭球基准的变换.
        /// </summary>
        /// <param name="sourceCrs">待转参考系</param>
        /// <param name="targetCrs">目标参考系</param>
        /// <returns></returns>
        public AbstractCoordTranform CreateGeodeticDatumTranform(
            ICoordinateReferenceSystem sourceCrs = null,
            ICoordinateReferenceSystem targetCrs = null)
        {
            if (sourceCrs == null) sourceCrs = SourceCrs;
            if (targetCrs == null) targetCrs = TargetCrs;
            return new GeodeticDatumTranform(sourceCrs, targetCrs);
        }
        #endregion

        #endregion


    }
}
