//2014.11.17, czs, edit in namu, 继承于 Vector ， 不继承 XY。
//2017.07.19, czs, edit in hongqing, 合并 orbit GeoCoord

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Referencing;
using Geo.Algorithm;
using Geo.Utils;

namespace Geo.Coordinates
{
    /// <summary>
    /// 大地坐标, 三维地理坐标,由经度Lon、维度Lat和高程Height组成。
    /// 具体是所属何种地球参考系，需要依据上下文判断。
    /// </summary>
    public class GeoCoord : LonLat, IGeodeticCoord
    {
        const double eps_mach = 1.0e-15;//double.Epsilon;// 1.0e-15;
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public GeoCoord() { this.SetDimension(3); }
        /// <summary>
        /// 创建一个实例。
        /// </summary>
        /// <param name="lon">经度</param>
        /// <param name="lat">维度</param>
        /// <param name="height">高程</param>
        public GeoCoord(double lon, double lat, double height, AngleUnit unit = AngleUnit.Degree)
            : base(lon, lat, unit)
        {
            this.SetDimension(3);
            this[2] = height;
        }
        /// <summary>
        /// 获取一个唯一的识别键。不同的精度不同的键。
        /// </summary>
        /// <param name="resolution">默认以米为单位</param>
        /// <returns></returns>
        public override string GetUniqueKey(double resolution = 1e-3)
        {  
            double meter =  resolution;
            StringBuilder sb = new StringBuilder();
            sb.Append(base.GetUniqueKey(resolution));
            sb.Append(",");
            sb.Append( Geo.Utils.StringUtil.GetUniqueKey(Height, resolution));

           return sb.ToString();
        }


        /// <summary>
        /// 高程。若是大地高，则为点沿着法线到椭球面的距离。
        /// </summary>
        public double Height { get { return this[2]; } set { this[2] = value; } }
        /// <summary>
        /// 高程，同 Height
        /// </summary>
        public double Altitude { get { return Height; } set { Height = value; } }
         
        public static GeoCoord operator +(GeoCoord first, GeoCoord second)        {            return new GeoCoord(first.Lon + second.Lon, first.Lat + second.Lat, first.Height + second.Height);        } 
        public static GeoCoord operator -(GeoCoord first, GeoCoord second)        {            return new GeoCoord(first.Lon - second.Lon, first.Lat - second.Lat, first.Height - second.Height);       } 

        /// <summary>
        /// 保存为度分秒字符串
        /// </summary>
        /// <returns></returns>
        public string ToDmsString(string splitter = ",")
        {
            return new DMS(Lon).ToString() + splitter + new DMS(Lat) + splitter + Height.ToString("0.000000");
        }
        /// <summary>
        /// 秒小数，提供高精度秒对比
        /// </summary>
        /// <returns></returns>
        public string ToSecondsString(string splitter = ",")
        {
            return new DMS(Lon).Seconds + splitter + new DMS(Lat).Seconds + splitter + Height.ToString("0.000000");
        }
        /// <summary>
        /// 字符输出
        /// </summary>
        /// <param name="format"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public override string ToString(string format, string spliter = ", ")
        {
            return Lon.ToString(format) + spliter + Lat.ToString(format) + spliter + Height.ToString(format);
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Lon.ToString() + "," + Lat.ToString() + "," + Height.ToString(); 
        }
         
        /// <summary>
        /// 解析字符串为实例。
        /// </summary>
        /// <param name="toString"></param>
        /// <returns></returns>
        public static new GeoCoord Parse(string toString,
            AngleUnit from = AngleUnit.Degree,
            AngleUnit to = AngleUnit.Degree, 
            AngleUnit fromDegFormat = AngleUnit.Degree)
        {
            toString = toString.Replace("(", "").Replace(")", "");
            string[] spliters = new string[] { ",", "\t", " " };
            string[] strs = toString.Split(spliters, StringSplitOptions.RemoveEmptyEntries);

            double lon = double.Parse(strs[0]);
            double lat = double.Parse(strs[1]);
            if (from == AngleUnit.Degree)//转换为标准的度小数再说。
            {
                lon = AngularConvert.ConvertDegree(lon, fromDegFormat);
                lat = AngularConvert.ConvertDegree(lat, fromDegFormat);      
            }

           lon = AngularConvert.Convert(lon, from, to);
           lat = AngularConvert.Convert(lat, from, to);           

            double h = 0;
            if (strs.Length > 2)
                h = double.Parse(strs[2]);
            return new GeoCoord(lon, lat, h);
        }

        #region from orbit



        /// <summary>
        /// 当前大地坐标按照指定椭球转换为空间直角坐标。 单位：米
        /// Position vector [m] from geodetic coordinates
        /// </summary>
        /// <param name="raduisOfEquator">Equator radius [m]</param>
        /// <param name="flattening"> Flattening</param>
        /// <returns></returns>
        public Geo.Algorithm.Vector ToXyzVector(double raduisOfEquator, double flattening)
        {
            double e2 = flattening * (2.0 - flattening);        // Square of eccentricity
            double CosLat = Math.Cos(Lat);         // (Co)sine of geodetic latitude
            double SinLat = Math.Sin(Lat);

            Geo.Algorithm.Vector r = new Geo.Algorithm.Vector(3);

            // Position vector 
            double N = raduisOfEquator / Math.Sqrt(1.0 - e2 * SinLat * SinLat);

            r[0] = (N + Height) * CosLat * Math.Cos(Lon);
            r[1] = (N + Height) * CosLat * Math.Sin(Lon);
            r[2] = ((1.0 - e2) * N + Height) * SinLat;

            return r;
        }

        /// <summary>
        /// Transformation to local tangent coordinates
        /// </summary>
        /// <returns></returns>
        public Matrix ToLocalNez_Matrix()
        {
            return ToLocalNez_Matrix(Lon, Lat);
        }

        /// <summary>
        /// Transformation from Greenwich meridian system to local tangent coordinates
        /// </summary>
        /// <param name="lon">Geodetic East longitude [rad]</param>
        /// <param name="lat"> Geodetic latitude [rad]</param>
        /// <returns>Rotation matrix from the Earth equator and Greenwich meridian
        /// to the local tangent (East-North-Zenith) coordinate system
        /// </returns>
        public Matrix ToLocalNez_Matrix(double lon, double lat)
        {
            Matrix M = new Matrix(3, 3);
            double Aux;

            // Transformation to Zenith-East-North System
            M = Matrix.RotateY3D(-lat) * Matrix.RotateZ3D(lon);

            // Cyclic shift of rows 0,1,2 to 1,2,0 to obtain East-North-Zenith system
            for (int j = 0; j < 3; j++)
            {
                Aux = M[0, j]; M[0, j] = M[1, j]; M[1, j] = M[2, j]; M[2, j] = Aux;
            }
            return M;
        }

        /// <summary>
        /// 本地空间直角坐标到极坐标的转换. Computes azimuth and elevation from local tangent coordinates
        /// </summary>
        /// <param name="localEnz"> s   Topocentric local tangent coordinates (East-North-Zenith frame)</param>
        /// <param name="azimuthRad">  A   Azimuth [rad]</param>
        /// <param name="elevationRad">  E   Elevation [rad]</param>
        /// <param name="Range">   Range [m]</param>
        public static void LocalEnzToPolar(Geo.Algorithm.Vector localEnz, out double azimuthRad, out double elevationRad, out double range)
        {
            range = localEnz.Norm();
            azimuthRad = Math.Atan2(localEnz[0], localEnz[1]);
            azimuthRad = ((azimuthRad < 0.0) ? azimuthRad + OrbitConsts.TwoPI : azimuthRad);
            elevationRad = Math.Atan(localEnz[2] / Math.Sqrt(localEnz[0] * localEnz[0] + localEnz[1] * localEnz[1]));
        }

        /// <summary>
        ///  本地空间直角坐标到极坐标的转换，结果包括微分。 AzEl Computes azimuth, elevation and partials from local tangent coordinates
        /// </summary>
        /// <param name="localEnz">本地水平坐标ENZ， Topocentric local tangent coordinates (East-North-Zenith frame)</param>
        /// <param name="azimuthRad"> A Azimuth [rad] </param>
        /// <param name="elevationRad"> E Elevation [rad]</param>
        /// <param name="dAds">  dAds   Partials of azimuth w.r.t. s</param>
        /// <param name="dEds"> dEds   Partials of elevation w.r.t. s</param>
        static public void LocalEnzToPolar(Geo.Algorithm.Vector localEnz, out double azimuthRad, out double elevationRad, out Geo.Algorithm.Vector dAds, out Geo.Algorithm.Vector dEds)
        {
            double rho = Math.Sqrt(localEnz[0] * localEnz[0] + localEnz[1] * localEnz[1]);
            // Angles
            azimuthRad = Math.Atan2(localEnz[0], localEnz[1]);
            azimuthRad = ((azimuthRad < 0.0) ? azimuthRad + OrbitConsts.TwoPI : azimuthRad);
            elevationRad = Math.Atan(localEnz[2] / rho);
            // Partials
            dAds = new Geo.Algorithm.Vector(localEnz[1] / (rho * rho), -localEnz[0] / (rho * rho), 0.0);
            dEds = new Geo.Algorithm.Vector(-localEnz[0] * localEnz[2] / rho, -localEnz[1] * localEnz[2] / rho, rho) / localEnz.Dot(localEnz);
        }


        /// <summary>
        /// 空间直角坐标转换成大地坐标
        /// </summary>
        /// <param name="X">X分量， 米</param>
        /// <param name="Y">Y分量， 米</param>
        /// <param name="Z">Z分量， 米</param>
        /// <param name="raduisOfEquator">赤道半径</param>
        /// <param name="f">地球扁率</param>
        /// <param name="lon">经度-弧度</param>
        /// <param name="lat">纬度-弧度</param>
        /// <param name="height">高程-米</param>
        public static void XyzToGeoCoord(double X, double Y, double Z, double raduisOfEquator, double f, out double lon, out double lat, out double height)
        {
            double eps = 1.0e3 * eps_mach;   // Convergence criterion 
            double epsRequ = eps * raduisOfEquator;
            double e2 = f * (2.0 - f);        // Square of eccentricity 
            double rho2 = X * X + Y * Y;           // Square of distance from z-axis

            // Iteration 
            double dZ, dZ_new, SinPhi;
            double ZdZ = 0, Nh = 0, N = 0;

            int maxLoop = 20;
            dZ = e2 * Z;
            for (int i = 0; i < maxLoop; i++)
            {
                ZdZ = Z + dZ;
                Nh = Math.Sqrt(rho2 + ZdZ * ZdZ);
                SinPhi = ZdZ / Nh;                    // Sine of geodetic latitude
                N = raduisOfEquator / Math.Sqrt(1.0 - e2 * SinPhi * SinPhi);
                dZ_new = N * e2 * SinPhi;
                if (Math.Abs(dZ - dZ_new) < epsRequ) break;
                dZ = dZ_new;
            }

            // Longitude, latitude, altitude
            lon = Math.Atan2(Y, X);
            lat = Math.Atan2(ZdZ, Math.Sqrt(rho2));
            height = Nh - N;
        }  
        #endregion
    }
}
