//2014.10.25, czs, create in numu shuangliao, 坐标工具

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Referencing;

namespace Geo.Coordinates
{
    /// <summary>
    /// 坐标工具
    /// </summary>
    public static class CoordUtil
    {

        /// <summary>
        /// 计算 测站-卫星 方向的距离。
        /// </summary>
        /// <param name="neuCorrection">测站改正 NEU</param>
        /// <param name="receiverXyz">测站坐标</param>
        /// <param name="satXyz">卫星坐标</param>
        /// <returns></returns>
        public static double GetDirectionLength(NEU neuCorrection, XYZ receiverXyz, XYZ satXyz)
        {
            XYZ ray = (satXyz - receiverXyz);
            GeoCoord geoCoord = CoordTransformer.XyzToGeoCoord(receiverXyz);

            //此处修改为 NEU 坐标系。Rotate vector ray to UEN reference frame
            NEU rayNeu = CoordTransformer.XyzToNeu(ray, geoCoord, AngleUnit.Degree);
            NEU directionUnit = rayNeu.UnitNeuVector();//方向向量

            //计算沿着射线方向的改正数。Compute corrections = displacement vectors components along ray direction.
            double rangeCorretion = neuCorrection.Dot(directionUnit);
            return rangeCorretion;
        }


        /// <summary>
        /// 将NEU偏差根据站星径向换算为等效距离偏差
        /// </summary>
        /// <param name="neuCorrection"></param>
        /// <param name="polar"></param>
        /// <returns></returns>
        public static double GetDirectionLength(NEU neuCorrection, Polar polar)
        {
            double azimuth = polar.Azimuth * CoordConsts.DegToRadMultiplier;
            double elevation = polar.Elevation * CoordConsts.DegToRadMultiplier;
            double cosel = Math.Cos(elevation);
            double e0 = Math.Sin(azimuth) * cosel;
            double e1 = Math.Cos(azimuth) * cosel;
            double e2 = Math.Sin(elevation);

            double rangeCorretion = neuCorrection.E * e0 + neuCorrection.N * e1 + neuCorrection.U * e2;
            return rangeCorretion;
        }













    }
}