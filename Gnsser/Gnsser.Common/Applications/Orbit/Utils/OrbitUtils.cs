//2014.10.03, czs, create, ��װ���õĹ���㷨

using System;
using System.Globalization;
using Gnsser.Times;
using Geo.Coordinates;
using Geo.Utils;
using Geo.Times;
using Geo;

namespace Gnsser.Orbits
{
    /// <summary> 
    /// ��װ���õĹ���㷨
    /// </summary>
    public  class OrbitUtils
    {
        /// <summary>
        /// ��������λ�úͲ�վλ�ü�������վ������ϵ�е�λ�á�
        /// Returns the topo-centric (azimuth, elevation, etc.) coordinates for
        /// a target object described by the given ECI coordinates.
        /// </summary>
        /// <param name="satPos">The ECI coordinates of the target object.</param>
        /// <param name="siteCoord">The ECI coordinates of the �۲�վ</param>
        /// <returns>The look angle to the target object.</returns>
        public static TopoCoord GetSatTopoCoord(TimedMotionState satPos, GeoCoord siteCoord)
        {
            // Calculate the ECI coordinates for this Site object at the time of interest.
            Julian date = satPos.Date;
            MotionState sitePos = OrbitUtils.GetMovingState(siteCoord, date);
            XYZ vecV = satPos.Velocity - sitePos.Velocity;
            XYZ vecP = satPos.Position - sitePos.Position;

            // The site's Local Mean Sidereal Time at the time of interest.
            double thetaRad = date.GetLocalMeanSiderealTime(siteCoord.Lon, false);

            double sinLat = Math.Sin(siteCoord.Lat);
            double cosLat = Math.Cos(siteCoord.Lat);
            double sinTheta = Math.Sin(thetaRad);
            double cosTheta = Math.Cos(thetaRad);

            double top_s = sinLat * cosTheta * vecP.X +
                           sinLat * sinTheta * vecP.Y -
                           cosLat * vecP.Z;
            double top_e = -sinTheta * vecP.X +
                            cosTheta * vecP.Y;
            double top_z = cosLat * cosTheta * vecP.X +
                           cosLat * sinTheta * vecP.Y +
                           sinLat * vecP.Z;
            double az = Math.Atan(-top_e / top_s);

            if (top_s > 0.0)
            {
                az += OrbitConsts.PI;
            }

            if (az < 0.0)
            {
                az += 2.0 * OrbitConsts.PI;
            }

            double el = Math.Asin(top_z / vecP.Radius());
            double rate = vecP.Dot(vecV) / vecP.Radius();

            TopoCoord topo = new TopoCoord(
                                         az,         // azimuth, radians
                                         el,         // elevation, radians
                                         vecP.Radius(), // range
                                         rate      // rate, per sec
                                         );

            #if WANT_ATMOSPHERIC_CORRECTION
            // Elevation correction for atmospheric refraction.
            // Reference:  Astronomical Algorithms by Jean Meeus, pp. 101-104
            // Note:  Correction is meaningless when apparent elevation is below horizon
            topo.Elevation += AngularConvert.DegToRad((1.02 /
                                          Math.Tan(AngularConvert.DegToRad(AngularConvert.RadToDeg(el) + 10.3 /
                                          (AngularConvert.RadToDeg(el) + 5.11)))) / 60.0);
            if (topo.Elevation < 0.0)
            {
                topo.Elevation = el;    // Reset to true elevation
            }

            if (topo.Elevation > (Consts.PI / 2.0))
            {
                topo.Elevation = (Consts.PI / 2.0);
            }
            #endif
            return topo;
        }

        /// <summary>
        /// �ɲ�վ���ĵع̴���������������ռ�ֱ�������λ�ú��ٶȡ�
        /// </summary>
        /// <param name="geo">�������</param>
        /// <param name="date">ʱ��</param>
        /// <param name="ellipsoid">�ο�����</param> 
      /// <param name="isKm">�Ƿ���ǧ��</param>
      /// <returns></returns>
        public static MotionState GetMovingState(
            GeoCoord geo, 
            Julian date, 
            Geo.Referencing.Ellipsoid ellipsoid = null,  
            bool isKm = true
            )
        {
            double lat = geo.Lat;
            double lon = geo.Lon;
            double alt = geo.Altitude;

            if (geo.Unit == AngleUnit.Degree)
            {
                lat = AngularConvert.DegToRad(lat);
                lon = AngularConvert.DegToRad(lon);
            } 

            if (ellipsoid == null) ellipsoid = Geo.Referencing.Ellipsoid.WGS84;

            double f = ellipsoid.Flattening;
            double a = ellipsoid.SemiMajorAxis;

            if (isKm)
            {
               a = ellipsoid.SemiMajorAxis / 1000.0;
            }

            // Calculate Local Mean Sidereal Time (theta)
            double theta = date.GetLocalMeanSiderealTime(lon);

            double c = 1.0 / Math.Sqrt(1.0 + f * (f - 2.0) * GeoMath.Sqr(Math.Sin(lat)));
            double s = GeoMath.Sqr(1.0 - f) * c;
            double achcp = (a * c + alt) * Math.Cos(lat);

            XYZ pos = new XYZ();

            pos.X = achcp * Math.Cos(theta);      
            pos.Y = achcp * Math.Sin(theta);         
            pos.Z = (a * s + alt) * Math.Sin(lat);

            XYZ vel = GetVelocityInCelestialSphere(pos);

            return new MotionState(pos, vel);
        }

        /// <summary>
        /// ͨ���̶��ڵ����ϵĲ�վλ�ü�������������ϵ�е��ٶȣ���������ת���ٶȡ�
        /// </summary>
        /// <param name="pos">�̶��ڵ����ϵĲ�վλ��</param>
        /// <returns>��������ϵ�е��ٶ�</returns>
        private static XYZ GetVelocityInCelestialSphere(XYZ pos)
        { 
            double mfactor = OrbitConsts.TwoPi * (OrbitConsts.EarthRotationPerSiderealDay / TimeConsts.SECOND_PER_DAY);
            double X = - mfactor * pos.Y;
            double Y = mfactor * pos.X;
            double Z = 0.0;
            return new XYZ(X, Y, Z);
        }
    }
}
