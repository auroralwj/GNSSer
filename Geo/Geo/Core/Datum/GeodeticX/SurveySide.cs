using System;
using Geo;

namespace GeodeticX
{
	/// <summary>
	/// �����е�һ���ߣ��������������һ��Ϊ��վ�㣬һ��Ϊ��׼�㡣
	/// һ���߽�����һЩ�����Ϣ����߳�����λ�ǣ�����߳��ȡ�
	/// </summary>
    public class SurveySide
    {
        //ȱʡ��������Ϊ0.14
        private static readonly double Default_k = 0.14;

        #region ˽�б�������
        /// <summary>
        /// ��վ��
        /// </summary>
        private GeoPoint standPoint;
        /// <summary>
        /// ��վ���������
        /// </summary>
        private double instrumentHeight;
        /// <summary>
        /// ��վ��ƫ�ĵ�ƫ�Ľ�
        /// </summary>
        private double standEccentricAngle;
        /// <summary>
        /// ��վ��ƫ�ĵ�ƫ�ľ�
        /// </summary>
        private double standEccentricDistance;

        /// <summary>
        /// ��׼��
        /// </summary>
        private GeoPoint sightPoint;
        /// <summary>
        /// ��׼���Ŀ���
        /// </summary>
        private double targetHeight;
        /// <summary>
        /// ��׼��ƫ�ĵ�ƫ�Ľ�
        /// </summary>
        private double sightEccentricAngle;
        /// <summary>
        /// ��׼��ƫ�ĵ�ƫ�ľ�
        /// </summary>
        private double sightEccentricDistance;
        #endregion

        #region ���캯��
        public SurveySide() { }

        public SurveySide(GeoPoint StandPoint, GeoPoint SightPoint)
        {
            standPoint = StandPoint;
            sightPoint = SightPoint;
        }

        public SurveySide(GeoPoints points)
        {
            if (points.Count == 2)
                throw new Exception("���������߱���Ϊ������");
            else
            {
                standPoint = points[0];
                sightPoint = points[1];
            }
        }
        #endregion

        #region ���Զ���
        /// <summary>
        /// ��վ��
        /// </summary>
        public GeoPoint StationPoint
        {
            get { return standPoint; }
            set { standPoint = value; }
        }

        /// <summary>
        /// ��վ���������
        /// </summary>
        public double InstrumentHeight
        {
            get { return instrumentHeight; }
            set { instrumentHeight = value; }
        }

        /// <summary>
        /// ��վ���ƫ�Ľ�
        /// </summary>
        public double StandEccentricAngle
        {
            get { return standEccentricAngle; }
            set { standEccentricAngle = value; }
        }

        /// <summary>
        /// ��վ���ƫ�ľ�
        /// </summary>
        public double StandEccentricDistance
        {
            get { return standEccentricDistance; }
            set { standEccentricDistance = value; }
        }


        /// <summary>
        /// ��׼��
        /// </summary>
        public GeoPoint SightPoint
        {
            get { return sightPoint; }
            set { sightPoint = value; }
        }

        /// <summary>
        /// ��׼���Ŀ���
        /// </summary>
        public double TargetHeight
        {
            get { return targetHeight; }
            set { targetHeight = value; }
        }

        /// <summary>
        /// ��׼���ƫ�Ľ�
        /// </summary>
        public double SightEccentricAngle
        {
            get { return sightEccentricAngle; }
            set { sightEccentricAngle = value; }
        }

        /// <summary>
        /// ��׼���ƫ�ľ�
        /// </summary>
        public double SightEccentricDistance
        {
            get { return sightEccentricDistance; }
            set { sightEccentricDistance = value; }
        }
        #endregion

        #region ���������ߵ�����
        /// <summary>
        /// ����������ط�λ��
        /// </summary>
        public double GeodeticAzimuth_Bessel
        {
            get
            {
                if (standPoint.Dimension == 0)
                    throw new Exception("��վ��Ϊ��");
                else if (sightPoint.Dimension == 0)
                    throw new Exception("��׼��Ϊ��");
                else
                {
                    return Geodetic.Bessel_BL_A(standPoint.GeodeticCoordinate.B, standPoint.GeodeticCoordinate.L, sightPoint.GeodeticCoordinate.B,
                                                sightPoint.GeodeticCoordinate.L, standPoint.ReferenceEllipsoid.a, standPoint.ReferenceEllipsoid.f);

                }
            }
        }

        /// <summary>
        /// ����������ط���λ��
        /// </summary>
        public double GeodeticInverseAzimuth_Bessel
        {
            get
            {
                if (standPoint.Dimension == 0)
                    throw new Exception("��վ��Ϊ��");
                else if (sightPoint.Dimension == 0)
                    throw new Exception("��׼��Ϊ��");
                else
                {
                    return Geodetic.Bessel_BL_A(sightPoint.GeodeticCoordinate.B, sightPoint.GeodeticCoordinate.L, standPoint.GeodeticCoordinate.B,
                                                standPoint.GeodeticCoordinate.L, sightPoint.ReferenceEllipsoid.a, sightPoint.ReferenceEllipsoid.f);
                }
            }
        }

        /// <summary>
        /// �ɸ�˹ƽ������ͨ�����귽λ�ǡ����ʸ����������������Ƿ����ط�λ��
        /// </summary>
        public double GeodeticAzimuth_xy
        {
            get
            {
                if (standPoint.Dimension == 0)
                    throw new Exception("��վ��Ϊ��");
                else if (sightPoint.Dimension == 0)
                    throw new Exception("��׼��Ϊ��");
                else
                    return FlatAzimuth - CurvatureCorrection / 3600 + standPoint.ConvergenceAngle / 3600;
            }
        }

        /// <summary>
        /// �ɸ�˹ƽ������ͨ�����귽λ�ǡ����ʸ����������������Ƿ����ط���λ��
        /// </summary>
        public double GeodeticInverseAzimuth_xy
        {
            get
            {
                if (standPoint.Dimension == 0)
                    throw new Exception("��վ��Ϊ��");
                else if (sightPoint.Dimension == 0)
                    throw new Exception("��׼��Ϊ��");
                else
                {
                    double flatA = FlatAzimuth; 
                    if (flatA >= 180) flatA -= 180;
                    return flatA + CurvatureCorrection / 3600 + sightPoint.ConvergenceAngle / 3600;
                }
            }
        }

        /// <summary>
        /// ������ز������ɿռ�ֱ��������ⷽλ��
        /// </summary>
        public double GeodeticAzimuth_XYZ
        {
            get
            {
                if (standPoint.Dimension == 0)
                    throw new Exception("��վ��Ϊ��");
                else if (sightPoint.Dimension == 0)
                    throw new Exception("��׼��Ϊ��");
                else
                    try
                    {
                        return Geodetic.Missile_XYZ_A(standPoint.SpatialRectCoordinate, sightPoint.SpatialRectCoordinate,
                                                     standPoint.ReferenceEllipsoid.a, standPoint.ReferenceEllipsoid.f);
                    }
                    catch
                    {
                        throw new Exception("�ɿռ�ֱ��������㷽λ����Ҫ��ά����");
                    }
            }
        }

        /// <summary>
        /// ������������߳���������룩
        /// </summary>
        public double GeodesicLength_Bessel
        {
            get
            {
                if (standPoint.Dimension == 0)
                    throw new Exception("��վ��Ϊ��");
                else if (sightPoint.Dimension == 0)
                    throw new Exception("��׼��Ϊ��");
                else
                    return Geodetic.Bessel_BL_S(standPoint.GeodeticCoordinate.B, standPoint.GeodeticCoordinate.L, sightPoint.GeodeticCoordinate.B,
                                                sightPoint.GeodeticCoordinate.L, standPoint.ReferenceEllipsoid.a, standPoint.ReferenceEllipsoid.f);
            }
        }

        /// <summary>
        /// �ɸ�˹����������귽λ��
        /// </summary>
        public double FlatAzimuth
        {
            get
            {
                if (standPoint.Dimension == 0)
                    throw new Exception("��վ��Ϊ��");
                else if (sightPoint.Dimension == 0)
                    throw new Exception("��׼��Ϊ��");
                else
                {
                    double x1 = standPoint.GaussCoordinate.x;
                    double y1 = standPoint.GaussCoordinate.y;
                    double x2 = sightPoint.GaussCoordinate.x;
                    double y2 = sightPoint.GaussCoordinate.y;

                    int belt1 = 0, belt2 = 0;
                    if (y1 > 1000000)
                    {
                        belt1 = (int)Math.Floor(y1 / 1000000);
                    }
                    if (y2 > 1000000)
                    {
                        belt2 = (int)Math.Floor(y2 / 1000000);
                    }

                    if (belt1 > belt2)
                    {
                        double L1 = standPoint.GeodeticCoordinate.L - (belt2 - 0.5) * sightPoint.GaussCoordinate.BeltWidth;
                        Geodetic.Bl_xy(standPoint.GeodeticCoordinate.B, L1, out x1, out y1,
                                        sightPoint.ReferenceEllipsoid.a, sightPoint.ReferenceEllipsoid.f,
                                        sightPoint.GaussCoordinate.BeltWidth);
                        y2 -= belt2 * 1000000 + 500000;
                    }

                    if (belt1 < belt2)
                    {
                        double L2 = sightPoint.GeodeticCoordinate.L - (belt1 - 0.5) * standPoint.GaussCoordinate.BeltWidth;
                        Geodetic.Bl_xy(sightPoint.GeodeticCoordinate.B, L2, out x2, out y2,
                                        standPoint.ReferenceEllipsoid.a, standPoint.ReferenceEllipsoid.f,
                                        standPoint.GaussCoordinate.BeltWidth);
                        y1 -= belt1 * 1000000 + 500000;
                    }

                    double A = Math.Atan((y2 - y1) / (x2 - x1));

                    if (y2 < y1 && x2 > x1)
                    {
                        A += 2 * CoordConsts.PI;
                    }
                    if (x2 < x1)
                    {
                        A += CoordConsts.PI;
                    }

                    return A * 180 / CoordConsts.PI;
                }
            }
        }

        /// <summary>
        /// �ɸ�˹�������ƽ�����
        /// </summary>
        public double FlatDistance
        {
            get
            {
                if (standPoint.Dimension == 0)
                    throw new Exception("��վ��Ϊ��");
                else if (sightPoint.Dimension == 0)
                    throw new Exception("��׼��Ϊ��");
                else
                {
                    double x1 = standPoint.GaussCoordinate.x;
                    double y1 = standPoint.GaussCoordinate.y;
                    double x2 = sightPoint.GaussCoordinate.x;
                    double y2 = sightPoint.GaussCoordinate.y;

                    int belt1 = 0, belt2 = 0;
                    if (y1 > 1000000)
                    {
                        belt1 = (int)Math.Floor(y1 / 1000000);
                    }
                    if (y2 > 1000000)
                    {
                        belt2 = (int)Math.Floor(y2 / 1000000);
                    }

                    if (belt1 > belt2)
                    {
                        double L1 = standPoint.GeodeticCoordinate.L - (belt2 - 0.5) * sightPoint.GaussCoordinate.BeltWidth;
                        Geodetic.Bl_xy(standPoint.GeodeticCoordinate.B, L1, out x1, out y1,
                                        sightPoint.ReferenceEllipsoid.a, sightPoint.ReferenceEllipsoid.f,
                                        sightPoint.GaussCoordinate.BeltWidth);
                        y2 -= belt2 * 1000000 + 500000;
                    }

                    if (belt1 < belt2)
                    {
                        double L2 = sightPoint.GeodeticCoordinate.L - (belt1 - 0.5) * standPoint.GaussCoordinate.BeltWidth;
                        Geodetic.Bl_xy(sightPoint.GeodeticCoordinate.B, L2, out x2, out y2,
                                        standPoint.ReferenceEllipsoid.a, standPoint.ReferenceEllipsoid.f,
                                        standPoint.GaussCoordinate.BeltWidth);
                        y1 -= belt1 * 1000000 + 500000;
                    }

                    return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
                }
            }
        }

        /* �����������ƽ�����          
        /// <summary>
        /// �������ƽ����ظ�Ϊ׼�ռ�ֱ��������㣬Ȼ���ɿռ�ֱ���������ƽ�����
        /// </summary>
        public double FlatDistance2
        {
            get
            {
                if (standPoint.Dimension == 0)
                    throw new Exception("��վ��Ϊ��");
                else if (sightPoint.Dimension == 0)
                    throw new Exception("��׼��Ϊ��");
                else
                {
                    double X1, Y1, Z1;
                    double X2, Y2, Z2;
                    double B1 = standPoint.GeodeticCoordinate.B;
                    double L1 = standPoint.GeodeticCoordinate.L;
                    double H1 = standPoint.GeodeticCoordinate.H;
                    double B2 = sightPoint.GeodeticCoordinate.B;
                    double L2 = sightPoint.GeodeticCoordinate.L;
                    double H2 = sightPoint.GeodeticCoordinate.H;

                    double Hm = (H1 + H2) / 2;

                    Geodetic.BLH_XYZ(B1, L1, Hm, out X1, out Y1, out Z1, standPoint.ReferenceEllipsoid.a, standPoint.ReferenceEllipsoid.f);
                    Geodetic.BLH_XYZ(B2, L2, Hm, out X2, out Y2, out Z2, standPoint.ReferenceEllipsoid.a, standPoint.ReferenceEllipsoid.f);

                    return Math.Sqrt((X2 - X1) * (X2 - X1) + (Y2 - Y1) * (Y2 - Y1) + (Z1 - Z2) * (Z1 - Z2));
                }
            }
        }

        /// <summary>
        /// �����������Ϊ���������߽���ͶӰ���Ӷ���ƽ���������ƽ�����
        /// </summary>
        public double FlatDistance3
        {
            get
            {
                double x1, y1, x2, y2;
                double B1, L1, B2, L2, Lm;

                B1 = standPoint.GeodeticCoordinate.B;
                L1 = standPoint.GeodeticCoordinate.L;
                B2 = sightPoint.GeodeticCoordinate.B;
                L2 = sightPoint.GeodeticCoordinate.L;
                Lm = (L1 + L2) / 2;

                Geodetic.Bl_xy(B1, L1 - Lm, out x1, out y1, standPoint.ReferenceEllipsoid.a, standPoint.ReferenceEllipsoid.f, 3);
                Geodetic.Bl_xy(B2, L2 - Lm, out x2, out y2, standPoint.ReferenceEllipsoid.a, standPoint.ReferenceEllipsoid.f, 3);

                return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
            }
        }

        /// <summary>
        /// ������ռ�ֱ������ֱ�Ӽ���ƽ�����
        /// </summary>
        public double FlatDistance4
        {
            get
            {
                double X1 = standPoint.SpatialRectCoordinate.X;
                double Y1 = standPoint.SpatialRectCoordinate.Y;
                double Z1 = standPoint.SpatialRectCoordinate.Z;
                double X2 = sightPoint.SpatialRectCoordinate.X;
                double Y2 = sightPoint.SpatialRectCoordinate.Y;
                double Z2 = sightPoint.SpatialRectCoordinate.Z;

                return Math.Sqrt((X2 - X1) * (X2 - X1) + (Y2 - Y1) * (Y2 - Y1) + (Z1 - Z2) * (Z1 - Z2));
            }
        }
        */
          
        /// <summary>
        /// ��ƽ������ʹ�ظ����б��
        /// </summary>
        public double SlopeDistance
        {
            get
            {
                if (standPoint.Dimension == 0)
                    throw new Exception("��վ��Ϊ��");
                else if (sightPoint.Dimension == 0)
                    throw new Exception("��׼��Ϊ��");
                else
                {
                    double x1 = standPoint.GaussCoordinate.x;
                    double y1 = standPoint.GaussCoordinate.y;
                    if (standPoint.GaussCoordinate.AssumedCoord)
                        y1 -= standPoint.GaussCoordinate.BeltNumber * 1000000 + 500000;

                    double x2 = sightPoint.GaussCoordinate.x;
                    double y2 = sightPoint.GaussCoordinate.y;
                    if (standPoint.GaussCoordinate.AssumedCoord)
                        y2 -= sightPoint.GaussCoordinate.BeltNumber * 1000000 + 500000;

                    double H1, H2;
                    try
                    {
                        H1 = standPoint.GeodeticCoordinate.H + instrumentHeight;
                        H2 = sightPoint.GeodeticCoordinate.H + targetHeight;
                    }
                    catch
                    {
                        throw new Exception("����б����Ҫ��ظ���Ϣ");
                    }

                    double a = standPoint.ReferenceEllipsoid.a;
                    double sddx = FlatDistance / (1 + Math.Pow((y2 + y1) / 2 / a, 2) / 2);
                    double k1 = 2 * a * Math.Sin(sddx / 2 / a);
                    return Math.Sqrt(k1 * k1 * ((1 + H1 / a) * (1 + H2 / a)) + (H2 - H1) * (H2 - H1)) + 0.0005;
                }
            }
        }

        /// <summary>
        /// ���ʸ�������λ����
        /// </summary>
        public double CurvatureCorrection
        {
            get
            {
                if (standPoint.Dimension == 0)
                    throw new Exception("��վ��Ϊ��");
                else if (sightPoint.Dimension == 0)
                    throw new Exception("��׼��Ϊ��");
                else
                {
                    double x1 = standPoint.GaussCoordinate.x;
                    double y1 = standPoint.GaussCoordinate.y;
                    double x2 = sightPoint.GaussCoordinate.x;
                    double y2 = sightPoint.GaussCoordinate.y;

                    //�˴��ĸ�˹�������ת������Ȼ������м��㣬�������ִ�Ĵ���
                    if (standPoint.GaussCoordinate.AssumedCoord)
                        y1 -= standPoint.GaussCoordinate.BeltNumber * 1000000 + 500000;
                    if (standPoint.GaussCoordinate.AssumedCoord)
                        y2 -= sightPoint.GaussCoordinate.BeltNumber * 1000000 + 500000;

                    double Bm = (standPoint.GeodeticCoordinate.B + sightPoint.GeodeticCoordinate.B) / 2 * CoordConsts.PI / 180;

                    double it2 = Math.Pow(standPoint.ReferenceEllipsoid.e2 * Math.Cos(Bm), 2);
                    double Rm = standPoint.ReferenceEllipsoid.c / (1 + it2);
                    double ym = (y1 + y2) / 2;
                    double rou = 3600 * 180 / CoordConsts.PI;

                    return rou * (x1 - x2) * (2 * y1 + y2 - ym * Math.Pow(ym / Rm, 2)) / Rm / Rm / 6 +
                           rou * it2 * Math.Tan(Bm) * (y1 - y2) * Math.Pow(ym / Rm, 2) / Rm;
                }
            }
        }

        /// <summary>
        /// �������
        /// </summary>
        public double DistanceCoorection
        {
            get
            {
                if (standPoint.Dimension == 0)
                    throw new Exception("��վ��Ϊ��");
                else if (sightPoint.Dimension == 0)
                    throw new Exception("��׼��Ϊ��");
                else
                {
                    double a = standPoint.ReferenceEllipsoid.a;
                    double f = standPoint.ReferenceEllipsoid.f;
                    double ee = (2 * f - 1) / f / f;

                    double B1 = standPoint.GeodeticCoordinate.B;
                    double L1 = standPoint.GeodeticCoordinate.L;
                    double B2 = sightPoint.GeodeticCoordinate.B;
                    double L2 = sightPoint.GeodeticCoordinate.L;
                    double y1 = standPoint.GaussCoordinate.y;
                    double y2 = sightPoint.GaussCoordinate.y;

                    //�˴��ĸ�˹�������ת������Ȼ������м��㣬�������ִ�Ĵ���
                    if (standPoint.GaussCoordinate.AssumedCoord)
                        y1 -= standPoint.GaussCoordinate.BeltNumber * 1000000 + 500000;
                    if (standPoint.GaussCoordinate.AssumedCoord)
                        y2 -= sightPoint.GaussCoordinate.BeltNumber * 1000000 + 500000;

                    double sBm = Math.Sin((B1 + B2) / 2 * CoordConsts.PI / 180);
                    double Rm2 = a * a * (1 - ee) / Math.Pow(1 - ee * sBm * sBm, 2);
                    double ym2 = Math.Pow((y1 + y2) / 2, 2);
                    double S = Geodetic.Bessel_BL_S(B1, L1, B2, L2, a, f);

                    return S * (ym2 / Rm2 / 2 + (y2 - y1) * (y2 - y1) / Rm2 / 24 + Math.Pow(ym2 / Rm2, 2) / 24);
                }
            }
        }
        #endregion

        #region �����ߵķ���
        /// <summary>
        /// ��ƽ������ʹ�ظ���ⴹֱ�ǣ���λ��
        /// </summary>
        public double VerticalAngle()
        {
            return VerticalAngle(Default_k);
        }

        /// <summary>
        /// ��ƽ������ʹ�ظ���ⴹֱ�ǣ���λ��
        /// </summary>
        /// <param name="k">��������</param>
        /// <returns>��ֱ�ǣ���λ��</returns>
        public double VerticalAngle(double k)
        {
            if (standPoint.Dimension == 0)
                throw new Exception("��վ��Ϊ��");
            else if (sightPoint.Dimension == 0)
                throw new Exception("��׼��Ϊ��");
            else
            {
                double x1 = standPoint.GaussCoordinate.x;
                double y1 = standPoint.GaussCoordinate.y;
                if (standPoint.GaussCoordinate.AssumedCoord)
                    y1 -= standPoint.GaussCoordinate.BeltNumber * 1000000 + 500000;

                double x2 = sightPoint.GaussCoordinate.x;
                double y2 = sightPoint.GaussCoordinate.y;
                if (standPoint.GaussCoordinate.AssumedCoord)
                    y2 -= sightPoint.GaussCoordinate.BeltNumber * 1000000 + 500000;

                double H1, H2;
                try
                {
                    H1 = standPoint.GeodeticCoordinate.H;
                    H2 = sightPoint.GeodeticCoordinate.H;
                }
                catch
                {
                    throw new Exception("���㴹ֱ����Ҫ��ظ���Ϣ");
                }

                double a = standPoint.ReferenceEllipsoid.a;
                double S = GeodesicLength_Bessel;
                double c = (1 - k) / 2 / a;
                double dh = S * ((H1  + H2 ) / 2 / a);
                return Math.Atan((H2 - instrumentHeight - H1 + targetHeight - c * S * S) / (S + dh)) * 180 / CoordConsts.PI;
            }
        }
        #endregion
    }
}
