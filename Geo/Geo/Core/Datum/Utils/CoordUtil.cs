//2014.10.25, czs, create in numu shuangliao, ���깤��

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Referencing;

namespace Geo.Coordinates
{
    /// <summary>
    /// ���깤��
    /// </summary>
    public static class CoordUtil
    {

        /// <summary>
        /// ���� ��վ-���� ����ľ��롣
        /// </summary>
        /// <param name="neuCorrection">��վ���� NEU</param>
        /// <param name="receiverXyz">��վ����</param>
        /// <param name="satXyz">��������</param>
        /// <returns></returns>
        public static double GetDirectionLength(NEU neuCorrection, XYZ receiverXyz, XYZ satXyz)
        {
            XYZ ray = (satXyz - receiverXyz);
            GeoCoord geoCoord = CoordTransformer.XyzToGeoCoord(receiverXyz);

            //�˴��޸�Ϊ NEU ����ϵ��Rotate vector ray to UEN reference frame
            NEU rayNeu = CoordTransformer.XyzToNeu(ray, geoCoord, AngleUnit.Degree);
            NEU directionUnit = rayNeu.UnitNeuVector();//��������

            //�����������߷���ĸ�������Compute corrections = displacement vectors components along ray direction.
            double rangeCorretion = neuCorrection.Dot(directionUnit);
            return rangeCorretion;
        }


        /// <summary>
        /// ��NEUƫ�����վ�Ǿ�����Ϊ��Ч����ƫ��
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