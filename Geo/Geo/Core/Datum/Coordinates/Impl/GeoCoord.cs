//2014.11.17, czs, edit in namu, �̳��� Vector �� ���̳� XY��
//2017.07.19, czs, edit in hongqing, �ϲ� orbit GeoCoord

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Referencing;
using Geo.Algorithm;
using Geo.Utils;

namespace Geo.Coordinates
{
    /// <summary>
    /// �������, ��ά��������,�ɾ���Lon��ά��Lat�͸߳�Height��ɡ�
    /// �������������ֵ���ο�ϵ����Ҫ�����������жϡ�
    /// </summary>
    public class GeoCoord : LonLat, IGeodeticCoord
    {
        const double eps_mach = 1.0e-15;//double.Epsilon;// 1.0e-15;
        /// <summary>
        /// Ĭ�Ϲ��캯����
        /// </summary>
        public GeoCoord() { this.SetDimension(3); }
        /// <summary>
        /// ����һ��ʵ����
        /// </summary>
        /// <param name="lon">����</param>
        /// <param name="lat">ά��</param>
        /// <param name="height">�߳�</param>
        public GeoCoord(double lon, double lat, double height, AngleUnit unit = AngleUnit.Degree)
            : base(lon, lat, unit)
        {
            this.SetDimension(3);
            this[2] = height;
        }
        /// <summary>
        /// ��ȡһ��Ψһ��ʶ�������ͬ�ľ��Ȳ�ͬ�ļ���
        /// </summary>
        /// <param name="resolution">Ĭ������Ϊ��λ</param>
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
        /// �̡߳����Ǵ�ظߣ���Ϊ�����ŷ��ߵ�������ľ��롣
        /// </summary>
        public double Height { get { return this[2]; } set { this[2] = value; } }
        /// <summary>
        /// �̣߳�ͬ Height
        /// </summary>
        public double Altitude { get { return Height; } set { Height = value; } }
         
        public static GeoCoord operator +(GeoCoord first, GeoCoord second)        {            return new GeoCoord(first.Lon + second.Lon, first.Lat + second.Lat, first.Height + second.Height);        } 
        public static GeoCoord operator -(GeoCoord first, GeoCoord second)        {            return new GeoCoord(first.Lon - second.Lon, first.Lat - second.Lat, first.Height - second.Height);       } 

        /// <summary>
        /// ����Ϊ�ȷ����ַ���
        /// </summary>
        /// <returns></returns>
        public string ToDmsString(string splitter = ",")
        {
            return new DMS(Lon).ToString() + splitter + new DMS(Lat) + splitter + Height.ToString("0.000000");
        }
        /// <summary>
        /// ��С�����ṩ�߾�����Ա�
        /// </summary>
        /// <returns></returns>
        public string ToSecondsString(string splitter = ",")
        {
            return new DMS(Lon).Seconds + splitter + new DMS(Lat).Seconds + splitter + Height.ToString("0.000000");
        }
        /// <summary>
        /// �ַ����
        /// </summary>
        /// <param name="format"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public override string ToString(string format, string spliter = ", ")
        {
            return Lon.ToString(format) + spliter + Lat.ToString(format) + spliter + Height.ToString(format);
        }
        /// <summary>
        /// �ַ���
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Lon.ToString() + "," + Lat.ToString() + "," + Height.ToString(); 
        }
         
        /// <summary>
        /// �����ַ���Ϊʵ����
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
            if (from == AngleUnit.Degree)//ת��Ϊ��׼�Ķ�С����˵��
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
        /// ��ǰ������갴��ָ������ת��Ϊ�ռ�ֱ�����ꡣ ��λ����
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
        /// ���ؿռ�ֱ�����굽�������ת��. Computes azimuth and elevation from local tangent coordinates
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
        ///  ���ؿռ�ֱ�����굽�������ת�����������΢�֡� AzEl Computes azimuth, elevation and partials from local tangent coordinates
        /// </summary>
        /// <param name="localEnz">����ˮƽ����ENZ�� Topocentric local tangent coordinates (East-North-Zenith frame)</param>
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
        /// �ռ�ֱ������ת���ɴ������
        /// </summary>
        /// <param name="X">X������ ��</param>
        /// <param name="Y">Y������ ��</param>
        /// <param name="Z">Z������ ��</param>
        /// <param name="raduisOfEquator">����뾶</param>
        /// <param name="f">�������</param>
        /// <param name="lon">����-����</param>
        /// <param name="lat">γ��-����</param>
        /// <param name="height">�߳�-��</param>
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
