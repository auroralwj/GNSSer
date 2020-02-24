using System;
using System.Collections.Generic;
using Geo;

namespace GeodeticX
{
	/// <summary>
	/// Point 的摘要说明。
	/// </summary>

    public class GeoPoint
    {
        //缺省值使用WGS—84椭球参数
        private static readonly double Default_a = 6378137;
        private static readonly double Default_f = 298.257223563;

        #region 私有变量定义
        /// <summary>
        /// 坐标系
        /// </summary>
        private ReferenceEllipsoid ellipsoid;
        /// <summary>
        /// 高斯平面坐标
        /// </summary>
        private GaussCoordinate gaussCoord;
        /// <summary>
        /// 大地坐标
        /// </summary>
        private GeodeticCoordinate geodeticCoord;
        /// <summary>
        /// 空间直角坐标
        /// </summary>
        private SpatialRectCoordinate spatialCoord;
 
        /// <summary>
        /// 坐标维数，三维还是二维，0表示未赋值
        /// </summary>
        private int dimension;

        /// <summary>
        /// 高程异常
        /// </summary>
        private double dH;
        private bool dHlIsNull;


        #endregion

        #region 多种构造函数
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
                //获取大地坐标，如果赋的值是空对象则会产生异常
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
                    //获取大地高，如果没有大地高，则产生异常，只取二维坐标
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
                //读取空间直角坐标值，如果未赋值则产生异常
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
                //读取高斯平面坐标值，如果是未赋值对象则产生异常
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
                    //如果存在高程异常和正常高，则按此式计算大地高
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

        #region 属性定义
        /// <summary>
        /// 获取/设置坐标系属性
        /// </summary>
        public ReferenceEllipsoid ReferenceEllipsoid
        {
            get
            {
                if (dimension > 0)
                    return ellipsoid;
                else
                    throw new Exception("未赋值的点");
            }
            set
            {
                ellipsoid = value;

                //当参考椭球发生变化时，假定大地坐标不变，而去求解空间直角坐标和高斯平面坐标
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
        /// 获取/设置高斯平面坐标属性
        /// </summary>
        public GaussCoordinate GaussCoordinate
        {
            get
            {
                if (dimension > 0)
                    return gaussCoord;
                else
                    throw new Exception("未赋值的点");
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
                        //如果存在高程异常和正常高，则按此式计算大地高
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
        /// 获取/设置大地坐标属性
        /// </summary>
        public GeodeticCoordinate GeodeticCoordinate
        {
            get
            {
                if (dimension > 0)
                    return geodeticCoord;
                else
                    throw new Exception("未赋值的点");
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
        /// 获取/设置空间直角坐标属性
        /// </summary>
        public SpatialRectCoordinate SpatialRectCoordinate
        {
            get
            {
                if (dimension == 3)
                    return spatialCoord;
                else
                    throw new Exception("无法获取三维坐标");
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
                    //读取空间直角坐标值，如果未赋值则产生异常
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
        /// 获取/设置高程异常
        /// </summary>
        public double AbnormalHeight
        {
            get
            {
                if (!dHlIsNull)
                    return dH;
                else
                    throw new Exception("高程异常未知");
            }
            set
            {
                dH = value;
                dHlIsNull = false;
            }
        }

        /// <summary>
        /// 获取坐标的维数
        /// </summary>
        public int Dimension { get { return dimension; } }
        #endregion

        #region 点位的其他只读属性
        /// <summary>
        /// 平均曲率半径
        /// </summary>
        public double CurvatureRadiusR
        {
            get
            {
                if (dimension > 1)
                    return ellipsoid.a * Math.Sqrt(1 - Math.Pow(ellipsoid.e, 2)) / (1 - Math.Pow(ellipsoid.e * Math.Sin(geodeticCoord.B * CoordConsts.PI / 180), 2));
                else
                    throw new Exception("未赋值的点");
            }
        }

        /// <summary>
        /// 子午圈曲率半径
        /// </summary>
        public double CurvatureRadiusM
        {
            get
            {
                if (dimension > 1)
                    return ellipsoid.a * (1 - Math.Pow(ellipsoid.e, 2)) / Math.Pow(1 - Math.Pow(ellipsoid.e * Math.Sin(geodeticCoord.B * CoordConsts.PI / 180), 2), 1.5);
                else
                    throw new Exception("未赋值的点");
            }
        }

        /// <summary>
        /// 卯酉圈曲率半径
        /// </summary>
        public double CurvatureRadiusN
        {
            get
            {
                if (dimension > 1)
                    return ellipsoid.a / Math.Sqrt(1 - Math.Pow(ellipsoid.e * Math.Sin(geodeticCoord.B * CoordConsts.PI / 180), 2));
                else
                    throw new Exception("未赋值的点");
            }
        }

        /// <summary>
        /// 长度比公式
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
                    throw new Exception("未赋值的点");

            }
        }

        /// <summary>
        /// 子午线收敛角
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
                    throw new Exception("未赋值的点");

            }
        }
        #endregion

        #region 坐标转换
        /// <summary>
        /// 给定参数的七参数坐标转换
        /// </summary>
        /// <param name="info">转换参数</param>
        /// <returns>转换后的空间直角坐标</returns>
        public SpatialRectCoordinate CoordinateConvert(TransformParameters p)
        {
            if (dimension > 2)
                return Geodetic.CoordinateTransform(spatialCoord, p);
            else
                throw new Exception("进行坐标转换需要三维坐标");
        }
        #endregion
    }
}
