using System;
using System.Collections.Generic;
using Geo;

namespace GeodeticX
{
	/// <summary>
	/// Point ��ժҪ˵����
	/// </summary>

    public class GeoPoint
    {
        //ȱʡֵʹ��WGS��84�������
        private static readonly double Default_a = 6378137;
        private static readonly double Default_f = 298.257223563;

        #region ˽�б�������
        /// <summary>
        /// ����ϵ
        /// </summary>
        private ReferenceEllipsoid ellipsoid;
        /// <summary>
        /// ��˹ƽ������
        /// </summary>
        private GaussCoordinate gaussCoord;
        /// <summary>
        /// �������
        /// </summary>
        private GeodeticCoordinate geodeticCoord;
        /// <summary>
        /// �ռ�ֱ������
        /// </summary>
        private SpatialRectCoordinate spatialCoord;
 
        /// <summary>
        /// ����ά������ά���Ƕ�ά��0��ʾδ��ֵ
        /// </summary>
        private int dimension;

        /// <summary>
        /// �߳��쳣
        /// </summary>
        private double dH;
        private bool dHlIsNull;


        #endregion

        #region ���ֹ��캯��
        public GeoPoint()
        {
            dimension = 0;
            dHlIsNull = true;
        }

        public GeoPoint(double B, double L, double a, double f)
        {
            ellipsoid = new ReferenceEllipsoid(a, f);
            geodeticCoord = new GeodeticCoordinate(B, L);

            double x, y;
            Geodetic.BL_xy(B, L, out x, out y, ellipsoid.a, ellipsoid.f);
            gaussCoord = new GaussCoordinate(x, y);

            dimension = 2;
        }

        public GeoPoint(double B, double L, ReferenceEllipsoid e)
            : this(B, L, e.a, e.f) { }

        public GeoPoint(double B, double L)
            : this(B, L, Default_a, Default_f) { }

        public GeoPoint(double B, double L, double H, double a, double f)
            : this(B, L, a, f)
        {
            geodeticCoord = new GeodeticCoordinate(B, L, H);

            double X, Y, Z;
            Geodetic.BLH_XYZ(B, L, H, out X, out Y, out Z, ellipsoid.a, ellipsoid.f);
            spatialCoord = new SpatialRectCoordinate(X, Y, Z);

            dimension = 3;
        }

        public GeoPoint(double B, double L, double H, ReferenceEllipsoid e)
            : this(B, L, H, e.a, e.f) { }

        public GeoPoint(double B, double L, double H)
            : this(B, L, H, Default_a, Default_f) { }

        public GeoPoint(GeodeticCoordinate BLH, double a, double f)
        {

            try
            {
                //��ȡ������꣬�������ֵ�ǿն����������쳣
                double B = BLH.B;
                double L = BLH.L;

                geodeticCoord = BLH;
                ellipsoid = new ReferenceEllipsoid(a, f);

                double x, y;
                Geodetic.BL_xy(B, L, out x, out y, ellipsoid.a, ellipsoid.f);
                gaussCoord = new GaussCoordinate(x, y);

                dimension = 2;

                try
                {
                    //��ȡ��ظߣ����û�д�ظߣ�������쳣��ֻȡ��ά����
                    double H = BLH.H;

                    double X, Y, Z;

                    Geodetic.BLH_XYZ(B, L, H, out X, out Y, out Z, ellipsoid.a, ellipsoid.f);
                    spatialCoord = new SpatialRectCoordinate(X, Y, Z);

                    dimension = 3;
                }
                catch { }
            }
            catch { }
        }

        public GeoPoint(GeodeticCoordinate BLH, ReferenceEllipsoid e)
            : this(BLH, e.a, e.f) { }

        public GeoPoint(GeodeticCoordinate BLH)
            : this(BLH, Default_a, Default_f) { }

        public GeoPoint(SpatialRectCoordinate XYZ, double a, double f)
        {
            try
            {
                //��ȡ�ռ�ֱ������ֵ�����δ��ֵ������쳣
                double X = XYZ.X;
                double Y = XYZ.Y;
                double Z = XYZ.Z;

                ellipsoid = new ReferenceEllipsoid(a, f);
                spatialCoord = XYZ;

                double B, L, H;
                Geodetic.XYZ_BLH(X, Y, Z, out B, out L, out H, ellipsoid.a, ellipsoid.f);
                geodeticCoord = new GeodeticCoordinate(B, L, H);

                double x, y;
                Geodetic.BL_xy(geodeticCoord.B, geodeticCoord.L, out x, out y, ellipsoid.a, ellipsoid.f);
                gaussCoord = new GaussCoordinate(x, y);

                if (!dHlIsNull)
                    gaussCoord.h = H - dH;

                dimension = 3;
            }
            catch { }
        }

        public GeoPoint(SpatialRectCoordinate XYZ, ReferenceEllipsoid e)
            : this(XYZ, e.a, e.f) { }

        public GeoPoint(SpatialRectCoordinate XYZ)
            : this(XYZ, Default_a, Default_f) { }

        public GeoPoint(GaussCoordinate xyh, double a, double f)
        {
            try
            {
                //��ȡ��˹ƽ������ֵ�������δ��ֵ����������쳣
                double x = xyh.x;
                double y = xyh.y;

                ellipsoid = new ReferenceEllipsoid(a, f);
                gaussCoord = xyh;

                double B, L;
                Geodetic.xy_BL(x, y, out B, out L, xyh.BeltWidth,0, ellipsoid.a, ellipsoid.f);
                geodeticCoord = new GeodeticCoordinate(B, L);

                dimension = 2;

                try
                {
                    //������ڸ߳��쳣�������ߣ��򰴴�ʽ�����ظ�
                    double h = xyh.h;
                    if (!dHlIsNull)
                    {
                        geodeticCoord.H = h + dH;
                        double X, Y, Z;
                        Geodetic.BLH_XYZ(B, L, h + dH, out X, out Y, out Z, ellipsoid.a, ellipsoid.f);
                        spatialCoord = new SpatialRectCoordinate(X, Y, Z);
                        dimension = 3;
                    }
                }
                catch { }
            }
            catch { }
        }

        public GeoPoint(GaussCoordinate xyh, ReferenceEllipsoid e)
            : this(xyh, e.a, e.f) { }

        public GeoPoint(GaussCoordinate xyh)
            : this(xyh, Default_a, Default_f) { }
        #endregion

        #region ���Զ���
        /// <summary>
        /// ��ȡ/��������ϵ����
        /// </summary>
        public ReferenceEllipsoid ReferenceEllipsoid
        {
            get
            {
                if (dimension > 0)
                    return ellipsoid;
                else
                    throw new Exception("δ��ֵ�ĵ�");
            }
            set
            {
                ellipsoid = value;

                //���ο��������仯ʱ���ٶ�������겻�䣬��ȥ���ռ�ֱ������͸�˹ƽ������
                if (dimension > 0)
                {
                    double B = geodeticCoord.B;
                    double L = geodeticCoord.L;

                    double x, y;
                    Geodetic.BL_xy(B, L, out x, out y, ellipsoid.a, ellipsoid.f);
                    gaussCoord = new GaussCoordinate(x, y);

                    try
                    {
                        double X, Y, Z;
                        double H = geodeticCoord.H;

                        Geodetic.BLH_XYZ(B, L, H, out X, out Y, out Z, ellipsoid.a, ellipsoid.f);
                        spatialCoord = new SpatialRectCoordinate(X, Y, Z);
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// ��ȡ/���ø�˹ƽ����������
        /// </summary>
        public GaussCoordinate GaussCoordinate
        {
            get
            {
                if (dimension > 0)
                    return gaussCoord;
                else
                    throw new Exception("δ��ֵ�ĵ�");
            }
            set
            {
                double a, f;
                try
                {
                    a = ellipsoid.a;
                    f = ellipsoid.f;
                }
                catch
                {
                    a = Default_a;
                    f = Default_f;
                    ellipsoid = new ReferenceEllipsoid(a, f, "WGS-84");
                }

                double x, y;
                try
                {
                    x = value.x;
                    y = value.y;

                    double B, L;
                    gaussCoord = value;

                    Geodetic.xy_BL(x, y, out B, out L,0, a, f);
                    geodeticCoord = new GeodeticCoordinate(B, L);

                    dimension = 2;

                    try
                    {
                        //������ڸ߳��쳣�������ߣ��򰴴�ʽ�����ظ�
                        double h = value.h;
                        if (!dHlIsNull)
                        {
                            geodeticCoord.H = h + dH;
                            double X, Y, Z;
                            Geodetic.BLH_XYZ(B, L, h + dH, out X, out Y, out Z, a, f);
                            spatialCoord = new SpatialRectCoordinate(X, Y, Z);
                            dimension = 3;
                        }
                    }
                    catch { }
                }
                catch { }
            }
        }

        /// <summary>
        /// ��ȡ/���ô����������
        /// </summary>
        public GeodeticCoordinate GeodeticCoordinate
        {
            get
            {
                if (dimension > 0)
                    return geodeticCoord;
                else
                    throw new Exception("δ��ֵ�ĵ�");
            }
            set
            {
                double a, f;
                try
                {
                    a = ellipsoid.a;
                    f = ellipsoid.f;
                }
                catch
                {
                    a = Default_a;
                    f = Default_f;
                    ellipsoid = new ReferenceEllipsoid(a, f, "WGS-84");
                }

                double B, L;
                try
                {
                    B = value.B;
                    L = value.L;
                    geodeticCoord = value;

                    double x, y;
                    Geodetic.BL_xy(B, L, out x, out y, a, f);
                    gaussCoord = new GaussCoordinate(x, y);

                    dimension = 2;                                        

                    try
                    {
                        double H = value.H;
                        double X, Y, Z;

                        Geodetic.BLH_XYZ(B, L, H, out X, out Y, out Z, a, f);
                        spatialCoord = new SpatialRectCoordinate(X, Y, Z);

                        if (!dHlIsNull)
                            gaussCoord.h = H - dH;

                        dimension = 3;
                    }
                    catch { }
                }
                catch { }
            }
        }

        /// <summary>
        /// ��ȡ/���ÿռ�ֱ����������
        /// </summary>
        public SpatialRectCoordinate SpatialRectCoordinate
        {
            get
            {
                if (dimension == 3)
                    return spatialCoord;
                else
                    throw new Exception("�޷���ȡ��ά����");
            }
            set
            {
                double a, f;
                try
                {
                    a = ellipsoid.a;
                    f = ellipsoid.f;
                }
                catch
                {
                    a = Default_a;
                    f = Default_f;
                    ellipsoid = new ReferenceEllipsoid(a, f, "WGS-84");
                }

                try
                {
                    //��ȡ�ռ�ֱ������ֵ�����δ��ֵ������쳣
                    double X = value.X;
                    double Y = value.Y;
                    double Z = value.Z;

                    spatialCoord = value;

                    double B, L, H;
                    Geodetic.XYZ_BLH(X, Y, Z, out B, out L, out H, a, f);
                    geodeticCoord = new GeodeticCoordinate(B, L, H);

                    double x, y;
                    Geodetic.BL_xy(geodeticCoord.B, geodeticCoord.L, out x, out y, a, f);
                    gaussCoord = new GaussCoordinate(x, y);

                    if (!dHlIsNull)
                        gaussCoord.h = H - dH;

                    dimension = 3;
                }
                catch { }
            }
        }

        /// <summary>
        /// ��ȡ/���ø߳��쳣
        /// </summary>
        public double AbnormalHeight
        {
            get
            {
                if (!dHlIsNull)
                    return dH;
                else
                    throw new Exception("�߳��쳣δ֪");
            }
            set
            {
                dH = value;
                dHlIsNull = false;
            }
        }

        /// <summary>
        /// ��ȡ�����ά��
        /// </summary>
        public int Dimension { get { return dimension; } }
        #endregion

        #region ��λ������ֻ������
        /// <summary>
        /// ƽ�����ʰ뾶
        /// </summary>
        public double CurvatureRadiusR
        {
            get
            {
                if (dimension > 1)
                    return ellipsoid.a * Math.Sqrt(1 - Math.Pow(ellipsoid.e, 2)) / (1 - Math.Pow(ellipsoid.e * Math.Sin(geodeticCoord.B * CoordConsts.PI / 180), 2));
                else
                    throw new Exception("δ��ֵ�ĵ�");
            }
        }

        /// <summary>
        /// ����Ȧ���ʰ뾶
        /// </summary>
        public double CurvatureRadiusM
        {
            get
            {
                if (dimension > 1)
                    return ellipsoid.a * (1 - Math.Pow(ellipsoid.e, 2)) / Math.Pow(1 - Math.Pow(ellipsoid.e * Math.Sin(geodeticCoord.B * CoordConsts.PI / 180), 2), 1.5);
                else
                    throw new Exception("δ��ֵ�ĵ�");
            }
        }

        /// <summary>
        /// î��Ȧ���ʰ뾶
        /// </summary>
        public double CurvatureRadiusN
        {
            get
            {
                if (dimension > 1)
                    return ellipsoid.a / Math.Sqrt(1 - Math.Pow(ellipsoid.e * Math.Sin(geodeticCoord.B * CoordConsts.PI / 180), 2));
                else
                    throw new Exception("δ��ֵ�ĵ�");
            }
        }

        /// <summary>
        /// ���ȱȹ�ʽ
        /// </summary>        
        public double LengthScale
        {
            get
            {
                if (dimension > 1)
                {
                    double a = ellipsoid.a;
                    double f = ellipsoid.f;
                    double ee = (2 * f - 1) / f / f;

                    double R2 = a * a * (1 - ee) / (1 - ee * Math.Pow(Math.Sin(geodeticCoord.B * CoordConsts.PI / 180), 2));
                    double yy = gaussCoord.y * gaussCoord.y;

                    return 1 + yy / R2 / 2 + yy * yy / R2 / R2 / 24;
                }
                else
                    throw new Exception("δ��ֵ�ĵ�");

            }
        }

        /// <summary>
        /// ������������
        /// </summary>        
        public double ConvergenceAngle
        {
            get
            {
                if (dimension > 1)
                {
                    double rB = geodeticCoord.B * CoordConsts.PI / 180;
                    int beltNum = (int)Math.Ceiling(geodeticCoord.L / gaussCoord.BeltWidth);
                    double lSeconds = (geodeticCoord.L - beltNum * gaussCoord.BeltWidth + (gaussCoord.BeltWidth == 6 ? 3 : 0)) * 3600;

                    double yita = ellipsoid.e2 * Math.Cos(rB);
                    double tB = Math.Tan(rB);
                    double cB = Math.Cos(rB);
                    double rou = 3600 * 180 / CoordConsts.PI;

                    return lSeconds * Math.Sin(rB) * (1 + Math.Pow(lSeconds * cB / rou, 2) / 3 * (1 + 3 * yita * yita + 2 * Math.Pow(yita, 4)) + Math.Pow(lSeconds * cB / rou, 4) / 15 / (2 - tB * tB));
                }
                else
                    throw new Exception("δ��ֵ�ĵ�");

            }
        }
        #endregion

        #region ����ת��
        /// <summary>
        /// �����������߲�������ת��
        /// </summary>
        /// <param name="info">ת������</param>
        /// <returns>ת����Ŀռ�ֱ������</returns>
        public SpatialRectCoordinate CoordinateConvert(TransformParameters p)
        {
            if (dimension > 2)
                return Geodetic.CoordinateTransform(spatialCoord, p);
            else
                throw new Exception("��������ת����Ҫ��ά����");
        }
        #endregion
    }
}
