using System;
using Geo;

namespace GeodeticX
{
	/// <summary>
	/// 测量中的一条边，包含两个点对象，一个为测站点，一个为照准点。
	/// 一条边将包含一些相对信息，如边长，方位角，大地线长等。
	/// </summary>
    public class SurveySide
    {
        //缺省大气常数为0.14
        private static readonly double Default_k = 0.14;

        #region 私有变量定义
        /// <summary>
        /// 测站点
        /// </summary>
        private GeoPoint standPoint;
        /// <summary>
        /// 测站点的仪器高
        /// </summary>
        private double instrumentHeight;
        /// <summary>
        /// 测站点偏心的偏心角
        /// </summary>
        private double standEccentricAngle;
        /// <summary>
        /// 测站点偏心的偏心距
        /// </summary>
        private double standEccentricDistance;

        /// <summary>
        /// 照准点
        /// </summary>
        private GeoPoint sightPoint;
        /// <summary>
        /// 照准点的目标高
        /// </summary>
        private double targetHeight;
        /// <summary>
        /// 照准点偏心的偏心角
        /// </summary>
        private double sightEccentricAngle;
        /// <summary>
        /// 照准点偏心的偏心距
        /// </summary>
        private double sightEccentricDistance;
        #endregion

        #region 构造函数
        public SurveySide() { }

        public SurveySide(GeoPoint StandPoint, GeoPoint SightPoint)
        {
            standPoint = StandPoint;
            sightPoint = SightPoint;
        }

        public SurveySide(GeoPoints points)
        {
            if (points.Count == 2)
                throw new Exception("构建测量边必须为两个点");
            else
            {
                standPoint = points[0];
                sightPoint = points[1];
            }
        }
        #endregion

        #region 属性定义
        /// <summary>
        /// 测站点
        /// </summary>
        public GeoPoint StationPoint
        {
            get { return standPoint; }
            set { standPoint = value; }
        }

        /// <summary>
        /// 测站点的仪器高
        /// </summary>
        public double InstrumentHeight
        {
            get { return instrumentHeight; }
            set { instrumentHeight = value; }
        }

        /// <summary>
        /// 测站点的偏心角
        /// </summary>
        public double StandEccentricAngle
        {
            get { return standEccentricAngle; }
            set { standEccentricAngle = value; }
        }

        /// <summary>
        /// 测站点的偏心距
        /// </summary>
        public double StandEccentricDistance
        {
            get { return standEccentricDistance; }
            set { standEccentricDistance = value; }
        }


        /// <summary>
        /// 照准点
        /// </summary>
        public GeoPoint SightPoint
        {
            get { return sightPoint; }
            set { sightPoint = value; }
        }

        /// <summary>
        /// 照准点的目标高
        /// </summary>
        public double TargetHeight
        {
            get { return targetHeight; }
            set { targetHeight = value; }
        }

        /// <summary>
        /// 照准点的偏心角
        /// </summary>
        public double SightEccentricAngle
        {
            get { return sightEccentricAngle; }
            set { sightEccentricAngle = value; }
        }

        /// <summary>
        /// 照准点的偏心距
        /// </summary>
        public double SightEccentricDistance
        {
            get { return sightEccentricDistance; }
            set { sightEccentricDistance = value; }
        }
        #endregion

        #region 其他测量边的属性
        /// <summary>
        /// 贝赛尔求解大地方位角
        /// </summary>
        public double GeodeticAzimuth_Bessel
        {
            get
            {
                if (standPoint.Dimension == 0)
                    throw new Exception("测站点为空");
                else if (sightPoint.Dimension == 0)
                    throw new Exception("照准点为空");
                else
                {
                    return Geodetic.Bessel_BL_A(standPoint.GeodeticCoordinate.B, standPoint.GeodeticCoordinate.L, sightPoint.GeodeticCoordinate.B,
                                                sightPoint.GeodeticCoordinate.L, standPoint.ReferenceEllipsoid.a, standPoint.ReferenceEllipsoid.f);

                }
            }
        }

        /// <summary>
        /// 贝赛尔求解大地反方位角
        /// </summary>
        public double GeodeticInverseAzimuth_Bessel
        {
            get
            {
                if (standPoint.Dimension == 0)
                    throw new Exception("测站点为空");
                else if (sightPoint.Dimension == 0)
                    throw new Exception("照准点为空");
                else
                {
                    return Geodetic.Bessel_BL_A(sightPoint.GeodeticCoordinate.B, sightPoint.GeodeticCoordinate.L, standPoint.GeodeticCoordinate.B,
                                                standPoint.GeodeticCoordinate.L, sightPoint.ReferenceEllipsoid.a, sightPoint.ReferenceEllipsoid.f);
                }
            }
        }

        /// <summary>
        /// 由高斯平面坐标通过坐标方位角、曲率改正、子午线收敛角反解大地方位角
        /// </summary>
        public double GeodeticAzimuth_xy
        {
            get
            {
                if (standPoint.Dimension == 0)
                    throw new Exception("测站点为空");
                else if (sightPoint.Dimension == 0)
                    throw new Exception("照准点为空");
                else
                    return FlatAzimuth - CurvatureCorrection / 3600 + standPoint.ConvergenceAngle / 3600;
            }
        }

        /// <summary>
        /// 由高斯平面坐标通过坐标方位角、曲率改正、子午线收敛角反解大地反方位角
        /// </summary>
        public double GeodeticInverseAzimuth_xy
        {
            get
            {
                if (standPoint.Dimension == 0)
                    throw new Exception("测站点为空");
                else if (sightPoint.Dimension == 0)
                    throw new Exception("照准点为空");
                else
                {
                    double flatA = FlatAzimuth; 
                    if (flatA >= 180) flatA -= 180;
                    return flatA + CurvatureCorrection / 3600 + sightPoint.ConvergenceAngle / 3600;
                }
            }
        }

        /// <summary>
        /// 二炮阵地测量的由空间直角坐标求解方位角
        /// </summary>
        public double GeodeticAzimuth_XYZ
        {
            get
            {
                if (standPoint.Dimension == 0)
                    throw new Exception("测站点为空");
                else if (sightPoint.Dimension == 0)
                    throw new Exception("照准点为空");
                else
                    try
                    {
                        return Geodetic.Missile_XYZ_A(standPoint.SpatialRectCoordinate, sightPoint.SpatialRectCoordinate,
                                                     standPoint.ReferenceEllipsoid.a, standPoint.ReferenceEllipsoid.f);
                    }
                    catch
                    {
                        throw new Exception("由空间直角坐标计算方位角需要三维坐标");
                    }
            }
        }

        /// <summary>
        /// 贝赛尔求解大地线长（球面距离）
        /// </summary>
        public double GeodesicLength_Bessel
        {
            get
            {
                if (standPoint.Dimension == 0)
                    throw new Exception("测站点为空");
                else if (sightPoint.Dimension == 0)
                    throw new Exception("照准点为空");
                else
                    return Geodetic.Bessel_BL_S(standPoint.GeodeticCoordinate.B, standPoint.GeodeticCoordinate.L, sightPoint.GeodeticCoordinate.B,
                                                sightPoint.GeodeticCoordinate.L, standPoint.ReferenceEllipsoid.a, standPoint.ReferenceEllipsoid.f);
            }
        }

        /// <summary>
        /// 由高斯坐标计算坐标方位角
        /// </summary>
        public double FlatAzimuth
        {
            get
            {
                if (standPoint.Dimension == 0)
                    throw new Exception("测站点为空");
                else if (sightPoint.Dimension == 0)
                    throw new Exception("照准点为空");
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
        /// 由高斯坐标计算平面距离
        /// </summary>
        public double FlatDistance
        {
            get
            {
                if (standPoint.Dimension == 0)
                    throw new Exception("测站点为空");
                else if (sightPoint.Dimension == 0)
                    throw new Exception("照准点为空");
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

        /* 其他方法求解平面距离          
        /// <summary>
        /// 以两点的平均大地高为准空间直角坐标解算，然后由空间直角坐标计算平面距离
        /// </summary>
        public double FlatDistance2
        {
            get
            {
                if (standPoint.Dimension == 0)
                    throw new Exception("测站点为空");
                else if (sightPoint.Dimension == 0)
                    throw new Exception("照准点为空");
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
        /// 以两点的中线为中央子午线进行投影，从而用平面坐标计算平面距离
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
        /// 由两点空间直角坐标直接计算平面距离
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
        /// 由平面坐标和大地高求解斜距
        /// </summary>
        public double SlopeDistance
        {
            get
            {
                if (standPoint.Dimension == 0)
                    throw new Exception("测站点为空");
                else if (sightPoint.Dimension == 0)
                    throw new Exception("照准点为空");
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
                        throw new Exception("计算斜距需要大地高信息");
                    }

                    double a = standPoint.ReferenceEllipsoid.a;
                    double sddx = FlatDistance / (1 + Math.Pow((y2 + y1) / 2 / a, 2) / 2);
                    double k1 = 2 * a * Math.Sin(sddx / 2 / a);
                    return Math.Sqrt(k1 * k1 * ((1 + H1 / a) * (1 + H2 / a)) + (H2 - H1) * (H2 - H1)) + 0.0005;
                }
            }
        }

        /// <summary>
        /// 曲率改正，单位：秒
        /// </summary>
        public double CurvatureCorrection
        {
            get
            {
                if (standPoint.Dimension == 0)
                    throw new Exception("测站点为空");
                else if (sightPoint.Dimension == 0)
                    throw new Exception("照准点为空");
                else
                {
                    double x1 = standPoint.GaussCoordinate.x;
                    double y1 = standPoint.GaussCoordinate.y;
                    double x2 = sightPoint.GaussCoordinate.x;
                    double y2 = sightPoint.GaussCoordinate.y;

                    //此处的高斯坐标必须转换成自然坐标进行计算，否则会出现大的错误
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
        /// 距离改正
        /// </summary>
        public double DistanceCoorection
        {
            get
            {
                if (standPoint.Dimension == 0)
                    throw new Exception("测站点为空");
                else if (sightPoint.Dimension == 0)
                    throw new Exception("照准点为空");
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

                    //此处的高斯坐标必须转换成自然坐标进行计算，否则会出现大的错误
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

        #region 测量边的方法
        /// <summary>
        /// 由平面坐标和大地高求解垂直角，单位度
        /// </summary>
        public double VerticalAngle()
        {
            return VerticalAngle(Default_k);
        }

        /// <summary>
        /// 由平面坐标和大地高求解垂直角，单位度
        /// </summary>
        /// <param name="k">大气常数</param>
        /// <returns>垂直角，单位度</returns>
        public double VerticalAngle(double k)
        {
            if (standPoint.Dimension == 0)
                throw new Exception("测站点为空");
            else if (sightPoint.Dimension == 0)
                throw new Exception("照准点为空");
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
                    throw new Exception("计算垂直角需要大地高信息");
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
