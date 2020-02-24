//2012.09.23, czs, �޸�, 1.���нǶ����� AngelUnit ����ѡ�Ĭ��Ϊ �ȡ�2.���� XyzToGeoCoord2 ϵ�к�����

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Referencing;
using Geo.Utils;
//using Geo.Algorithm;
using Geo.Algorithm;
using Geo.Times;

namespace Geo.Coordinates
{
    /// <summary>
    /// �ṩ��ݵ�����ת�������� 
    /// </summary>
    public static class CoordTransformer
    {
        /// <summary>
        /// �õ�վ�Ǽ����ꡣ�����ŶŶŶ ŶŶŶŶŶŶ
        /// ��λ��Ϊ radians  (clockwise)
        /// Azimuth  radians  (�� neg. below horizon)
        /// </summary>
        /// <param name="satXyz"></param>
        /// <param name="receiverXyz"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        private static Polar XyzToPolar2(XYZ satXyz, XYZ receiverXyz, Geo.Coordinates.AngleUnit unit = AngleUnit.Radian)
        {
            double sla, slo, cla, clo, n, e, u;
            double len = (satXyz - receiverXyz).Length;
            Geo.Coordinates.GeoCoord geo = Geo.Coordinates.CoordTransformer.XyzToGeoCoord(receiverXyz, unit);
            double lon = geo.Lon;
            double lat = geo.Lat;
            if (unit == AngleUnit.Degree)
            {
                lon = geo.Lon * CoordConsts.DegToRadMultiplier;
                lat = geo.Lat * CoordConsts.DegToRadMultiplier;
            }

            sla = Sin(lat);
            cla = Cos(lat);
            slo = Sin(lon);
            clo = Cos(lon);

            //*** topocentric to local horizon system  (coeff.L. Ben notes)

            u = cla * clo * satXyz.X + cla * slo * satXyz.Y + sla * satXyz.Z;
            e = -slo * satXyz.X + clo * satXyz.Y;
            n = -sla * clo * satXyz.X - sla * slo * satXyz.Y + cla * satXyz.Z;

            double Elevation = Math.Asin(u / receiverXyz.Length);
            double Azimuth = Math.Atan2(e, n);

            if (unit == AngleUnit.Degree)
            {
                Azimuth = Azimuth * CoordConsts.RadToDegMultiplier;
                Elevation = Elevation * CoordConsts.RadToDegMultiplier;
            }

            return new Polar(len, Azimuth, Elevation);
        }

        /// <summary>
        /// ժȡ�� Ppos.java.txt
        /// conversion methods
        /// funcKeyToDouble unknowns,y,z into geodetic lat, lon, and ellip. ht
        /// ref: eq matrix.4b, info. 132, appendix matrix, osu #370
        /// ref: geom geod notes gs 658, rapp
        /// </summary>
        public static Geo.Coordinates.GeoCoord XyzToGeoCoord_Rad(double x, double y, double z)
        {
            double A = 6378137.0;              //*** grs-80 ellipsoid
            double E2 = 0.006694380022903416;  //*** (geod.hand.1992)
            double AE2 = A * E2;                 //*** (66(2), pg. 191)

            double lat, lon, height;
            double MAXINT = 10, TOL = 1.0e-14;
            double p, tgla, tglax, sla, w, n;
            int icount = 0;

            //*** compute initial estimate of reduced latitude  (hight=0)

            p = Math.Sqrt(x * x + y * y);
            tgla = z / p / (1.0 - E2);

            //*** iterate to convergence, or to max # iterations

            while (icount < MAXINT)
            {
                tglax = tgla;
                tgla = z / (p - (AE2 / Math.Sqrt(1.0 + (1.0 - E2) * tgla * tgla)));
                icount = icount + 1;

                if (Math.Abs(tgla - tglax) <= TOL)
                {      //*** convergence test
                    lat = Math.Atan(tgla);
                    sla = Sin(lat);
                    lon = Math.Atan2(y, x);
                    w = Math.Sqrt(1.0 - E2 * sla * sla);
                    n = A / w;
                    if (Math.Abs(lat) < 0.7854)
                    {
                        height = p / Cos(lat) - n;
                    }
                    else
                    {
                        height = z / sla - n + E2 * n;
                    }
                    return new Geo.Coordinates.GeoCoord(lon, lat, height);
                }
            }//***endwhile 

            throw new Exception("Fail to converge in xyzgeo(), too many iterations -- probably too close to pole");
        }
        /// <summary>
        /// �ռ�ֱ������ϵ��������������ϵ���˷�û��ѭ�������һЩ������һ������ȸ߳��з������²��
        /// </summary>
        /// <param name="xyz"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static GeoCoord XyzToGeoCoord2(XYZ xyz, AngleUnit unit = AngleUnit.Degree)
        {
            return XyzToGeoCoord2(xyz, Geo.Referencing.Ellipsoid.WGS84, unit);
        }
        /// <summary>
        ///  �ռ�ֱ������ϵ��������������ϵ���˷�û��ѭ�������һЩ������һ������ȸ߳��з������²��
        /// </summary>
        /// <param name="xyz"></param>
        /// <param name="ellipsoid"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static GeoCoord XyzToGeoCoord2(XYZ xyz, Geo.Referencing.Ellipsoid ellipsoid, AngleUnit unit = AngleUnit.Degree)
        {
            return XyzToGeoCoord2(xyz, ellipsoid.SemiMajorAxis, ellipsoid.SemiMinorAxis, unit);
        }
        /// <summary>
        ///  �ռ�ֱ������ϵ��������������ϵ���˷�û��ѭ�������һЩ������һ������ȸ߳��з������²��
        /// </summary>
        /// <param name="xyz"></param>
        /// <param name="a">���뾶</param>
        /// <param name="b">�̰뾶</param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static GeoCoord XyzToGeoCoord2(XYZ xyz, double a, double b, AngleUnit unit = AngleUnit.Degree)
        {
            double x = xyz.X;
            double y = xyz.Y;
            double z = xyz.Z;
            double x2 = x * x;
            double y2 = y * y;
            double z2 = z * z;

            //double a = 6378137.0000;	// earth radius in meters
            //double b = 6356752.3142;	// earth semiminor in meters	
            double e = Math.Sqrt(1 - Math.Pow((b / a), 2));
            double b2 = b * b;
            double e2 = e * e;
            double ep = e * (a / b);
            double r = Math.Sqrt(x2 + y2);
            double r2 = r * r;
            double E2 = a * a - b * b;
            double F = 54 * b2 * z2;
            double G = r2 + (1 - e2) * z2 - e2 * E2;
            double c = (e2 * e2 * F * r2) / (G * G * G);
            double s = Math.Pow(1 + c + Math.Sqrt(c * c + 2 * c), 1 / 3);
            double P = F / (3 * Math.Pow(s + 1 / s + 1, 2) * G * G);
            double Q = Math.Sqrt(1 + 2 * e2 * e2 * P);
            double ro = -(P * e2 * r) / (1 + Q) + Math.Sqrt((a * a / 2) * (1 + 1 / Q) - (P * (1 - e2) * z2) / (Q * (1 + Q)) - P * r2 / 2);
            double tmp = Math.Pow(r - e2 * ro, 2);
            double U = Math.Sqrt(tmp + z2);
            double V = Math.Sqrt(tmp + (1 - e2) * z2);
            double zo = (b2 * z) / (a * V);

            double height = U * (1 - b2 / (a * V));

            double lat = Math.Atan((z + ep * ep * zo) / r);

            double temp = Math.Atan(y / x);
            double lon;
            if (x >= 0)
                lon = temp;
            else if ((x < 0) && (y >= 0))
                lon = CoordConsts.PI + temp;
            else
                lon = temp - CoordConsts.PI;

            if (unit == AngleUnit.Degree)
            {
                lon *= ScreenCoordTransformer.RadToDegMultiplier;
                lat *= ScreenCoordTransformer.RadToDegMultiplier;
            }
            return new GeoCoord(lon, lat, height);
        }


        /// <summary>
        /// �ɵ��ĵع̿ռ�ֱ��������������ꡣ
        /// </summary>
        /// <param name="pos">�ռ�ֱ������</param>
        /// <param name="date">������</param>
        /// <param name="ellipsoid">�ο�����</param>
        /// <param name="unit">�Ƕȵ�λ</param>
        /// <returns></returns>
        public static GeoCoord XyzToGeoCoord(IXYZ pos, Julian date, Geo.Referencing.Ellipsoid ellipsoid = null, AngleUnit unit = AngleUnit.Degree)
        {
            if (ellipsoid == null)
            {
                ellipsoid = Geo.Referencing.Ellipsoid.WGS84;
            }
            double f = ellipsoid.Flattening;
            double a = ellipsoid.SemiMajorAxis;

            double TwoPi = 2 * CoordConsts.PI;
            double x = pos.X;
            double y = pos.Y;
            double z = pos.Z;

            double theta = (GeoMath.AcTan(pos.Y, pos.X) - date.GetGreenwichMeanSiderealTime()) % TwoPi;
            theta = theta % TwoPi;

            if (theta < 0.0)
            {
                // "wrap" negative modulo
                theta += TwoPi;
            }

            double r = Math.Sqrt(x * x + y * y);
            double e2 = f * (2.0 - f);
            double lat = GeoMath.AcTan(z, r);

            const double DELTA = 1.0e-07;
            double phi;
            double c;

            do
            {
                phi = lat;
                c = 1.0 / Math.Sqrt(1.0 - e2 * GeoMath.Sqr(Sin(phi)));
                lat = GeoMath.AcTan(pos.Z + a * c * e2 * Sin(phi), r);
            }
            while (Math.Abs(lat - phi) > DELTA);

            double Altitude = (r / Cos(lat)) - a * c;

            if (unit == AngleUnit.Degree)
            {
                lat *= AngularConvert.RadToDegMultiplier;
                theta *= AngularConvert.RadToDegMultiplier;
            }

            double Lat = lat;
            double Lon = theta;

            return new GeoCoord(Lon, Lat, Altitude, unit);
        }


        /// <summary>
        ///  �������ǵ�վ�ļ����꣬���ڵ�ǰ��վ�Ĵ�����꣬���߶Ƚǻ���������棬��ǰ����Ϊ0ʱ����ֱ�ӵ��� lon lat
        /// </summary>
        /// <param name="satXyz">���ǵĵ��Ŀռ�ֱ������</param>
        /// <param name="stationPosition">��վ�ĵ��Ŀռ�ֱ������</param>
        /// <param name="unit">�Ƕȵ�λ,��ʾ����ĽǶȵ�λ</param>
        /// <returns></returns>
        public static Polar XyzToGeoPolar(XYZ satXyz, XYZ stationPosition, AngleUnit unit = AngleUnit.Degree)
        {
            double lat, lon, h;
            //����վ������ľ�γ�ȣ��˷�Ϊ��Ϊ��λ,by Zhao Dongqing
            GeodeticX.Geodetic.XYZ_BLH(stationPosition.X, stationPosition.Y, stationPosition.Z, out lat, out lon, out h,
                 Geo.Referencing.Ellipsoid.WGS84.SemiMajorAxis, Geo.Referencing.Ellipsoid.WGS84.InverseFlattening);

            if (unit != AngleUnit.Degree)
            {
                lat = AngularConvert.Convert(lat, AngleUnit.Degree, unit);
                lon = AngularConvert.Convert(lon, AngleUnit.Degree, unit);
            }
            return XyzToPolar(satXyz, stationPosition, lon, lat, unit);
        }
        /// <summary>
        ///  �������ǵ�վ�ļ����꣬���ڵ�ǰ��վ�������꣬���߶Ƚǻ������棬��ǰ����Ϊ0ʱ����ֱ�ӵ��� lon lat
        /// </summary>
        /// <param name="satXyz">���ǵĵ��Ŀռ�ֱ������</param>
        /// <param name="stationPosition">��վ�ĵ��Ŀռ�ֱ������</param>
        /// <param name="unit">�Ƕȵ�λ,��ʾ����ĽǶȵ�λ</param>
        /// <returns></returns>
        public static Polar XyzToSpherePolar(XYZ satXyz, XYZ stationPosition, AngleUnit unit = AngleUnit.Degree)
        { 
           var sphereCoord = XyzToSphere(stationPosition, unit);
             
            return XyzToPolar(satXyz, stationPosition, sphereCoord.Lon, sphereCoord.Lat, unit);
        }


        /// <summary>
        ///  �������ǵ�վ�ļ����꣬ȫ�� �� Ϊ��λ,�˱�XYZ����ȷ���Ƽ�������2017.10.12
        /// </summary>
        /// <param name="satXyz">���ǵĵ��Ŀռ�ֱ������</param>
        /// <param name="stationGeoCoord">��վ�ĵ��Ĵ������������꣬��ͬ�����Ӧ��ͬ�ĸ߶Ƚ�</param>
        /// <returns></returns>
        public static Polar XyzToPolar(XYZ satXyz, GeoCoord stationGeoCoord)
        {
            var staXyz = CoordTransformer.GeoCoordToXyz(stationGeoCoord); 
            return XyzToPolar(satXyz, staXyz, stationGeoCoord.Lon, stationGeoCoord.Lat, AngleUnit.Degree);
        }

        /// <summary>
        /// �������ǵ�վ�ļ�����,ָ���˾�γ�ȣ����Ӿ�ȷ�������ظ����㡣
        /// </summary>
        /// <param name="satXyz"></param>
        /// <param name="stationPosition"></param>
        /// <param name="unit">����ʾ����Ƕȵĵ�λ��ҲҪ������Ƕȵĵ�λ</param>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public static Polar XyzToPolar(XYZ satXyz, XYZ stationPosition, double lon, double lat, AngleUnit unit = AngleUnit.Degree)
        {
            XYZ deltaXyz = satXyz - stationPosition;//���浽���ǵ��򾶡�

            lon = Math.Abs(lon) < 1E-10 ? 0  : lon;

            NEU neu = XyzToNeu(deltaXyz, lat, lon, unit);
            return NeuToPolar(neu, unit);
        }

        #region  վ������ENU����Ŀռ�ֱ������XYZ��ת������֤���� 2016.09.10
        /// <summary>
        /// վ������ENU����Ŀռ�ֱ������XYZ��ת����
        /// ע��˴��������궼Ϊ���ĵع�����ϵ��
        /// </summary>
        /// <param name="targetXyzEcef">Ŀ���������ECEF</param>
        /// <param name="siteXyzEcef">��վ��������ECEF</param>
        /// <returns></returns>
        static public ENU XyzToEnu(XYZ targetXyzEcef, XYZ siteXyzEcef)
        {
            return LocaXyzToEnu(targetXyzEcef - siteXyzEcef, siteXyzEcef);
        }
   
        /// <summary>
        /// XYZתENU�ľ���ת��
        /// </summary>
        /// <param name="CovaOfLocalXyz">��ֵ�����Э������3*3�ף�</param>
        /// <param name="siteXyzEcef">���ز�վԭ������ECEF</param>
        /// <returns></returns>
        static public ENU XyzToEnuRms(Matrix CovaOfLocalXyz, XYZ siteXyzEcef)
        {
            Matrix CovaOfEnu = XyyToEnuCova(CovaOfLocalXyz, siteXyzEcef);

            double qeG = Math.Sqrt(CovaOfEnu[0, 0]);
            double qnG = Math.Sqrt(CovaOfEnu[1, 1]);
            double quG = Math.Sqrt(CovaOfEnu[2, 2]);

            var CovOfEEN = new ENU(qeG, qnG, quG);
            return CovOfEEN;
        }
        /// <summary>
        /// XYZ��Э����ת��ΪENUЭ���
        /// </summary>
        /// <param name="CovaOfLocalXyz"></param>
        /// <param name="siteXyzEcef"></param>
        /// <returns></returns>
        public static  Matrix XyyToEnuCova(Matrix CovaOfLocalXyz, XYZ siteXyzEcef)
        {
            var geo = XyzToGeoCoord(siteXyzEcef, AngleUnit.Radian);
            Matrix M = BuilTransferMatrixFromXyzToEnu(geo);
            Geo.Algorithm.Matrix CovaOfEnu = M * CovaOfLocalXyz * M.Transpose();
            return CovaOfEnu;
        }
        /// <summary>
        /// XYZת��ΪENU����˾���
        /// </summary>
        /// <param name="geo"></param>
        /// <returns></returns>
        public static Matrix BuilTransferMatrixFromXyzToEnu(GeoCoord geo)
        {
            Geo.Algorithm.Matrix M = new Geo.Algorithm.Matrix(3);
            M[0, 0] = -Math.Sin(geo.Lon);
            M[0, 1] = Math.Cos(geo.Lon);
            M[0, 2] = 0;
            M[1, 0] = -Math.Sin(geo.Lat) * Math.Cos(geo.Lon);
            M[1, 1] = -Math.Sin(geo.Lat) * Math.Sin(geo.Lon);
            M[1, 2] = Math.Cos(geo.Lat);
            M[2, 0] = Math.Cos(geo.Lat) * Math.Cos(geo.Lon);
            M[2, 1] = Math.Cos(geo.Lat) * Math.Sin(geo.Lon);
            M[2, 2] = Math.Sin(geo.Lat);
            return M;
        }

        /// <summary>
        /// ����XYZ���꣬ת����ENU���ꡣ
        /// </summary>
        /// <param name="localXyz">XYZ ����ƫ��</param>
        /// <param name="siteXyzEcef">��վ����</param>
        /// <returns></returns>
        static public ENU LocaXyzToEnu(XYZ localXyz, XYZ siteXyzEcef)
        {
            var geo = XyzToGeoCoord(siteXyzEcef, AngleUnit.Radian);
            var v = localXyz;
            var e = -v.X * Sin(geo.Lon) + v.Y * Cos(geo.Lon);
            var n = -v.X * Sin(geo.Lat) * Cos(geo.Lon) - v.Y * Sin(geo.Lat) * Sin(geo.Lon) + v.Z * Cos(geo.Lat);
            var u = v.X * Cos(geo.Lat) * Cos(geo.Lon) + v.Y * Cos(geo.Lat) * Sin(geo.Lon) + v.Z * Sin(geo.Lat);

            return new ENU(e, n, u);
        }
        /// <summary>
        /// վ������ENU����Ŀռ�ֱ������XYZ��ת��
        /// </summary>
        /// <param name="sat"></param>
        /// <param name="siteCoord"></param>
        /// <returns></returns>
        static public XYZ EnuToXyz(ENU sat, XYZ siteCoord)
        {
            XYZ dxyz = LocalEnuToDxyz(sat, siteCoord);

            return dxyz + siteCoord;
        }

        /// <summary>
        /// ����ENU����ת���ɵ�����������ڲ�վ���꣩
        /// </summary>
        /// <param name="sat"></param>
        /// <param name="siteCoord"></param>
        /// <returns></returns>
        public static XYZ LocalEnuToDxyz(ENU sat, XYZ siteCoord)
        {
            var geo = XyzToGeoCoord(siteCoord, AngleUnit.Radian);

            var x = -sat.E * Sin(geo.Lon) - sat.N * Sin(geo.Lat) * Cos(geo.Lon) + sat.U * Cos(geo.Lat) * Cos(geo.Lon);
            var y = sat.E * Cos(geo.Lon) - sat.N * Sin(geo.Lat) * Sin(geo.Lon) + sat.U * Cos(geo.Lat) * Sin(geo.Lon);
            var z = sat.N * Cos(geo.Lat) + sat.U * Sin(geo.Lat);
            var dxyz = new XYZ(x, y, z);
            return dxyz;
        }
        #endregion
        /// <summary>
        /// �ռ�ֱ������ϵ����վ������ϵ��ת����
        /// </summary>
        /// <param name="vector">վ������ϵ��Ŀ������</param>
        /// <param name="lonlat">վ�ĵĴ������</param>
        /// <param name="unit">��ǰ����ĽǶȵ�λ</param>
        /// <returns></returns>
        public static NEU XyzToNeu(XYZ vector, LonLat lonlat, AngleUnit unit = AngleUnit.Degree)
        {
            //double[][] trans = (GetGlobalToLocalCoordMatrix(lonlat));
            //double[] result = MatrixUtil.GetMultiply(trans, vector.Array);
            //XYZ xyz = XYZ.Parse(result);

            NEU neu = XyzToNeu(vector, lonlat.Lat, lonlat.Lon, unit);
            return neu;
        }

        /// <summary>
        ///  �ռ�ֱ������ϵ����վ������ϵ��ת����Ĭ��B L ��λ �ȣ�����Ϊ������ʹ������
        ///  ���Ŀռ�ֱ������ϵ(XYZ)ת��Ϊ�ط����ֵѿ���ֱ������ϵ��NEU,XYZ��
        /// </summary>
        /// <param name="vector1">��վ�����ǵ���</param>
        /// <param name="lat">վ������γ��</param>
        /// <param name="lon">վ�����ھ���</param>
        /// <param name="angleUnit">վ�����ھ��ȵĵ�λ</param>
        /// <returns></returns>
        public static NEU XyzToNeu(XYZ vector1, double lat, double lon, AngleUnit angleUnit = AngleUnit.Degree)
        {
            if (angleUnit != AngleUnit.Radian) //��ǰ����Ϊ 0 ��ʱ��U ��ת������ַ��Ŵ��󣿣���������2017.10.12.
            {
                lat = AngularConvert.ToRad(lat, angleUnit);
                lon = AngularConvert.ToRad(lon, angleUnit);
            }
            
            XYZ v = vector1;

            double n = -v.X * Sin(lat) * Cos(lon) - v.Y * Sin(lat) * Sin(lon) + v.Z * Cos(lat);
            double e = -v.X * Sin(lon) + v.Y * Cos(lon);
            double u = v.X * Cos(lat) * Cos(lon) + v.Y * Cos(lat) * Sin(lon) + v.Z * Sin(lat);

            return new NEU(n, e, u);// { N = n, E = e, U = u };
        }

        /// <summary>
        /// վ������ϵ����������ϵ��
        /// </summary>
        /// <param name="neu">��������ϵ</param>
        /// <param name="siteCoord">��������ԭ���ڵ�������ϵ�е�����</param>
        /// <returns></returns>
        public static XYZ NeuToXyz(NEU neu, XYZ siteCoord)
        {
            GeoCoord coord = XyzToGeoCoord(siteCoord);
            return NeuToXyz(neu, coord);
        }
        /// <summary>
        /// վ������ϵ����������ϵ
        /// </summary>
        /// <param name="sat"> վ������ϵ,�����ǣ�����</param>
        /// <param name="siteCoord">����ԭ���ڵ�������ϵ�е�����</param>
        /// <returns></returns>
        public static XYZ NeuToXyz(NEU sat, GeoCoord siteCoord)
        { 
            XYZ siteXyz = CoordTransformer.GeoCoordToXyz(siteCoord);
            return EnuToXyz(sat.ENU, siteXyz);
        }


        /// <summary>
        /// վ������ϵ��վ�ļ�����ϵ��
        /// </summary>
        /// <param name="neu"></param>
        /// <param name="unit">Ĭ�ϵ�λΪ��</param>
        /// <returns></returns>
        public static Polar NeuToPolar(NEU neu, AngleUnit unit = AngleUnit.Degree)
        {
            double r = neu.Length;
            double a = Math.Atan2(neu.E, neu.N);
            if (a < 0)//�Ա���Ϊ��׼��˳ʱ�룬�޸���
            {
                a += 2.0 * CoordConsts.PI;
            }

            double o = Math.Asin(neu.U / r);
            if (unit != AngleUnit.Radian)
            {
                a = AngularConvert.RadTo(a, unit);
                o = AngularConvert.RadTo(o, unit);
            }
            return new Polar(r, a, o) { Unit = unit };
        }
        /// <summary>
        /// ������ϵ��ͬ�Ŀռ�ֱ������ϵ��
        /// </summary>
        /// <param name="p">������</param>
        /// <returns></returns>
        public static NEU PolarToNeu(Polar p)
        {
            double ele = AngularConvert.ToRad(p.Elevation, p.Unit);
            double azi = AngularConvert.ToRad(p.Azimuth, p.Unit); 

            double n = p.Range * Cos(ele) * Cos(azi);
            double e = p.Range * Cos(ele) * Sin(azi);
            double u = p.Range * Sin(ele);
            return new NEU() { E = e, N = n, U = u };
        }

        #region ��˹�����ת��
        /// <summary>
        /// ��γ��ת������˹����XY
        /// </summary>
        /// <param name="lonlat"></param>
        /// <param name="beltWidth"></param>
        /// <param name="Ellipsoid"></param>
        /// <param name="unit"></param>
        /// <param name="aveGeoHeight">ͶӰ��ƽ����ظ�</param>
        /// <param name="YConst">����Y�ӳ���</param>
        /// <param name="orinalLonDeg">���������ߣ���С��</param>
        /// <param name="isYWithBeltNum"></param>
        /// <returns></returns>
        public static XY LonLatToGaussXy(LonLat lonlat, double aveGeoHeight, int beltWidth, ref double orinalLonDeg, bool IsIndicateOriginLon, Ellipsoid Ellipsoid, double YConst=500000, AngleUnit unit = AngleUnit.Degree, bool isYWithBeltNum = true)
        {
            double B = lonlat.Lat;
            double L = lonlat.Lon;
            double x;
            double y; 
            //��λת��
            L = AngularConvert.Convert(L, unit, AngleUnit.Degree);
            B = AngularConvert.Convert(B, unit, AngleUnit.Degree);
            Geo.Coordinates.GeodeticUtils.LonLatToGaussXy(L, B, aveGeoHeight, out x, out y, ref orinalLonDeg, IsIndicateOriginLon, beltWidth, YConst, isYWithBeltNum,
                Ellipsoid.SemiMajorAxis, Ellipsoid.InverseFlattening);

            return new XY(x, y);
        }

        /// <summary>
        /// ƽ�����꣨��Ȼ�����ٶ����꣩���������ĸ�˹����
        /// </summary>
        /// <param name="xy"></param>
        /// <param name="beltWidth"></param>
        /// <param name="Ellipsoid"></param>
        /// <param name="aveGeoHeight">ͶӰ���ظ�</param>
        /// <param name="YConst">����Y�ӳ���</param>
        /// <param name="unit">����Ƕȵĵ�λ</param>
        /// <param name="originLon_deg"> ���������ߣ���</param>
        /// <returns></returns>
        public static LonLat GaussXyToLonLat(XY xy, double aveGeoHeight, int beltWidth,double originLon_deg, double YConst, Ellipsoid Ellipsoid,  AngleUnit unit = AngleUnit.Degree)
        {
            return GaussXyToLonLat(xy, aveGeoHeight, beltWidth, originLon_deg, YConst, Ellipsoid.SemiMajorAxis, Ellipsoid.InverseFlattening, unit);
        }

        /// <summary>
        /// ƽ�����꣨��Ȼ�����ٶ����꣩���������ĸ�˹����
        /// </summary>
        /// <param name="xy">xΪ����</param>
        /// <param name="a"></param>
        /// <param name="YConst">����Y�ӳ���</param>
        /// <param name="InverseFlat ">�ο�������ʵ���</param>
        /// <param name="beltWidth">6��3</param>
        /// <param name="aveGeoHeight">ͶӰ���ظ�</param>
        /// <param name="unit">����Ƕȵĵ�λ</param>
        /// <param name="originLon_deg">���������ߣ���</param>
        /// <returns></returns>
        public static LonLat GaussXyToLonLat(XY xy, double aveGeoHeight, int beltWidth, double originLon_deg, double YConst,
            double a=Ellipsoid.SemiMajorAxisOfCGCS2000, double InverseFlat = Ellipsoid.InverseFlatOfCGCS2000,  AngleUnit unit = AngleUnit.Degree)
        {
            double B;
            double L;
            double x = xy.X;
            double y = xy.Y;

             var lonlat =  GeodeticUtils.GaussXyToLonLat(xy, aveGeoHeight, originLon_deg, YConst, a, InverseFlat);
            //GeodeticX.Geodetic.xy_BL(x, y, out B, out L, beltWidth, beltNum, a, InverseFlat );
            //��λת��
            L = AngularConvert.Convert(lonlat.Lon, AngleUnit.Degree, unit);
            B = AngularConvert.Convert(lonlat.Lat, AngleUnit.Degree, unit); 

            return new LonLat(L, B);
        }

        #endregion

        #region �ռ�ֱ�����굽������
        /// <summary>
        /// �ռ�ֱ�����굽������
        /// </summary>
        /// <param name="xyz"></param>
        /// <param name="unit">�Ƕȵ�λ</param>
        /// <returns></returns>
        public static Polar XyzToPolar(XYZ xyz, AngleUnit unit = AngleUnit.Degree)
        {
            double x = xyz.X;
            double y = xyz.Y;
            double z = xyz.Z;

            double radius = Math.Sqrt(x * x + y * y + z * z);
            double azimuth = Math.Atan2(y, x);
            if (azimuth < 0)
            {
                azimuth += 2.0 * CoordConsts.PI;
            }
            double elevatAngle = Math.Atan2(z, Math.Sqrt(x * x + y * y));

            azimuth = AngularConvert.RadTo(azimuth, unit);
            elevatAngle = AngularConvert.RadTo(elevatAngle, unit);

            return new Polar(radius, azimuth, elevatAngle, unit);
        }

        /// <summary>
        /// �ռ伫����ϵ���ռ�ֱ������ϵ
        /// </summary>
        /// <param name="polar"></param>
        /// <param name="unit">�Ƕȵ�λ</param>
        /// <returns></returns>
        public static XYZ PolarToXyz(Polar polar, AngleUnit unit = AngleUnit.Degree)
        {
            double radius = polar.Range;
            double elevatAngle = polar.Zenith;
            double azimuth = polar.Azimuth;

            elevatAngle = AngularConvert.ToRad(elevatAngle, polar.Unit);
            azimuth = AngularConvert.ToRad(azimuth, polar.Unit);

            double x = radius * (Cos(elevatAngle) + Cos(azimuth));
            double y = radius * (Cos(elevatAngle) + Sin(azimuth));
            double z = radius * Sin(elevatAngle);

            return new XYZ(x, y, z);
        }
        #endregion

        /// <summary>
        /// ���ļ�����ץ��Ϊ���Ĵ������
        /// </summary>
        /// <param name="polarInGeoCenter">������</param>
        /// <param name="unit">�Ƕȵ�λ</param>
        /// <returns></returns>
        public static GeoCoord PolarToGeoCoord(Polar polarInGeoCenter, AngleUnit unit = AngleUnit.Degree)
        {
            XYZ xyz = PolarToXyz(polarInGeoCenter, unit);
            return XyzToGeoCoord(xyz, unit);
        }
        /// <summary>
        /// վ�ļ�����ת��Ϊ���Ĵ������
        /// </summary>
        /// <param name="localPolar">������</param>
        /// <param name="sitePosInGeoCenter">��վ�ڵ�������ϵ������</param>
        /// <param name="unit">�Ƕȵ�λ</param>
        /// <returns></returns>
        public static GeoCoord LocalPolarToGeoCoord(Polar localPolar, XYZ sitePosInGeoCenter, AngleUnit unit = AngleUnit.Degree)
        {
            XYZ xyz = PolarToXyz(localPolar, unit); //ת��Ϊվ�Ŀռ�ֱ������
            XYZ geoCenterXyz = sitePosInGeoCenter + xyz;
            return XyzToGeoCoord(geoCenterXyz, unit);
        }
        /// <summary>
        /// վ�ļ�����ת��Ϊ���Ĵ������
        /// </summary>
        /// <param name="localPolar">Ŀ���ڵ��ļ������λ��</param>
        /// <param name="sitePosInGeoCenter">��վ�������</param>
        /// <param name="el">�ο�����</param>
        /// <param name="unit">�Ƕȵ�λ</param>
        /// <returns></returns>
        public static GeoCoord LocalPolarToGeoCoord(Polar localPolar, GeoCoord sitePosInGeoCenter, Geo.Referencing.Ellipsoid el = null, AngleUnit unit = AngleUnit.Degree)
        {
            if (el == null) el = Geo.Referencing.Ellipsoid.WGS84;

            NEU neu = PolarToNeu(localPolar);
            XYZ xyz = NeuToXyz(neu, sitePosInGeoCenter);
            return XyzToGeoCoord(xyz, unit);
        }


        #region �������Ϳռ�ֱ������֮���ת��

        /// <summary>
        /// �������תΪ�ռ�ֱ�����ꡣ
        /// </summary>
        /// <param name="ellipsoidCoord"></param>
        /// <returns></returns>
        public static XYZ GeoCoordToXyz(IGeodeticCoord ellipsoidCoord, Geo.Referencing.Ellipsoid el = null)
        {
            if (el == null) el = Geo.Referencing.Ellipsoid.WGS84;

            double lon = ellipsoidCoord.Lon;
            double lat = ellipsoidCoord.Lat;
            double height = ellipsoidCoord.Height;
            double a = el.SemiMajorAxis;
            double e = el.FirstEccentricity;

            return GeoCoordToXyz(lon, lat, height, a, e, ellipsoidCoord.Unit);
        }
        /// <summary>
        /// ���ĵع�����ϵת�����չ�����ϵ.X����̫���ڳ���ϵ�ͶӰ�غϡ�
        /// </summary>
        /// <param name="xyz"></param>
        /// <param name="time">�͸�����������Ƚ�</param>
        /// <returns></returns>
        public static XYZ EarthXyzToSunFixedXyz(XYZ xyz, DateTime time)
        { 
            return EarthXyzToSunFixedXyz(xyz, time.TimeOfDay);
        }

        /// <summary>
        /// ���ĵع�����ϵת�����չ�����ϵ.X����̫���ڳ���ϵ�ͶӰ�غϡ�
        /// </summary>
        /// <param name="xyz"></param>
        /// <param name="greenWidthTimeSpan">�͸�����������Ƚ�</param>
        /// <returns></returns>
        public static XYZ EarthXyzToSunFixedXyz(XYZ xyz, TimeSpan greenWidthTimeSpan)
        {
            var hours = (greenWidthTimeSpan.TotalHours  + 12.0) % 24.0;//����ҹת��������
            var rad = 1.0 * hours * (2.0 * Math.PI / 24.0); //�ӱ����Ͽ�������Ϊ˳ʱ����ת��ʱ�����ź�Ӧ����ʱ��ת������

            var newXyz = xyz.RotateZ(rad);
                 
            return newXyz;
        }

        /// <summary>
        /// ��������תΪ�ռ�ֱ�����ꡣĬ�ϵ�λΪ�ȡ�
        /// </summary>
        /// <param name="lon">���ȣ��ȣ�</param>
        /// <param name="lat">γ�ȣ��ȣ�</param>
        /// <param name="height">��ظ�</param>
        /// <param name="a">����뾶</param>
        /// <param name="flatOrInverse">���ʻ��䵹��</param>
        /// <param name="unit">��λ</param>
        /// <returns></returns>
        public static XYZ GeoCoordToXyz(double lon, double lat, double height, double a, double flatOrInverse, AngleUnit unit = AngleUnit.Degree)
        { 
            lon = AngularConvert.ToRad(lon, unit);
            lat = AngularConvert.ToRad(lat, unit);

            //�����ж�
            double e = flatOrInverse;
            if (flatOrInverse > 1)
            {
                e = 1.0 / e;
            } 

            double n = a / Math.Sqrt(1 - Math.Pow(e * Sin(lat), 2));

            double x = (n + height) * Cos(lat) * Cos(lon);
            double y = (n + height) * Cos(lat) * Sin(lon);
            double z = (n * (1 - e * e) + height) * Sin(lat);
            return new XYZ(x, y, z);
        }
        /// <summary>
        /// �ɿռ�ֱ������ת��Ϊ�������ꡣĬ�ϽǶȵ�λΪ�ȡ�
        /// </summary>
        /// <param name="xyz"></param>
        /// <param name="ellipsoid"></param>
        /// <param name="angeUnit"></param>
        /// <returns></returns>
        public static GeoCoord XyzToGeoCoord(IXYZ xyz, Geo.Referencing.Ellipsoid ellipsoid, AngleUnit angeUnit = AngleUnit.Degree)
        {
            double x = xyz.X;
            double y = xyz.Y;
            double z = xyz.Z;

            double a = ellipsoid.SemiMajorAxis;
            double e = ellipsoid.FirstEccentricity;
            return XyzToGeoCoord(x, y, z, a, e, angeUnit);
        }
        /// <summary>
        /// �ɿռ�ֱ������ת��Ϊ������꣬�ο�����ΪWGS84��
        /// Ĭ�ϵ�λΪ�ȡ�
        /// </summary>
        /// <param name="xyz">�ռ�ֱ������</param>
        /// <param name="angeUnit">�Ƕȵ�λ��Ĭ�ϵ�λΪ�ȡ�</param>
        /// <returns></returns>

        public static GeoCoord XyzToGeoCoord(IXYZ xyz, AngleUnit angeUnit = AngleUnit.Degree) { return XyzToGeoCoord(xyz, Geo.Referencing.Ellipsoid.WGS84, angeUnit); }

        /// <summary>
        /// �ɿռ�ֱ������ת��Ϊ�������ꡣĬ�ϽǶȵ�λΪ�ȡ�
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="a"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static GeoCoord XyzToGeoCoord(double x, double y, double z, double a, double e, AngleUnit angeUnit = AngleUnit.Degree)
        {
            double ee = e * e;
            double lon = Math.Atan2(y, x);
            double lat;
            double height;

            //iteration
            double deltaZ = 0;
            double sqrtXy = Math.Sqrt(x * x + y * y);
            double tempLat = Math.Atan2(z, sqrtXy);//��ʼȡֵ
            lat = tempLat;
            //if (Math.Abs(lat) > 80.0 * CoordConverter.DegToRadMultiplier)//lat=+-90,��height�ش�ʱ�����ô��㷨���ȶ���ʵ���д�ʵ�飬����both���룩
            //{
            int count = 0;//������ѭ��
            do
            {
                var sinLat = Sin(lat);
                deltaZ = a * ee * sinLat / Math.Sqrt(1 - Math.Pow(e * sinLat, 2));
                tempLat = lat;
                lat = Math.Atan2(z + deltaZ, sqrtXy);
                count++;
            } while (Math.Abs(lat - tempLat) > 1E-12 || count < 20);
            //}
            //else//�����㷨
            //{
            //    do
            //    {
            //        double tanB = Math.Tan(lat);
            //        tempLat = lat;
            //        lat = Math.Atan2(z + a * ee * tanB / Math.Sqrt(1 + tanB * tanB * (1 - ee)), sqrtXy);
            //    } while (Math.Abs(lat - tempLat) > 1E-12);
            //}

            double n = a / Math.Sqrt(1 - Math.Pow(e * Sin(tempLat), 2));
            //double   height = Math.Sqrt(x * x + y * y + Math.Pow((z + deltaZ), 2)) - n;

            height = sqrtXy / Cos(lat) - n;

            //����γ��
            //double dixinLat = (1 - ee * n / (n + height)) * Math.Tan(lat);
            //double dixinLatDeg = dixinLat * CoordConverter.RadToDegdMultiplier;


            lon = AngularConvert.RadTo(lon, angeUnit);
            lat = AngularConvert.RadTo(lat, angeUnit);

            return new GeoCoord(lon, lat, height);
        }
        #endregion

        #region ������ ��ת�� �ѿ������ֿռ�ֱ������ϵ
        /// <summary>
        /// ��������ת�����ѿ������ֿռ�ֱ������ϵ
        /// </summary>
        /// <param name="sphere"></param>
        /// <returns></returns>
        public static XYZ SphereToXyz(SphereCoord sphere)
        {
            double r = sphere.Radius;
            double lat = sphere.Lat;
            double lon = sphere.Lon;

            return SphereToXyz(lon, lat, r, sphere.Unit);
        }
        /// <summary>
        /// �ѿ������ֿռ�ֱ������ϵ ת���� ��������
        /// </summary>
        /// <param name="xyz"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static SphereCoord XyzToSphere(XYZ xyz, AngleUnit unit = AngleUnit.Degree)
        {
            double x = xyz.X;
            double y = xyz.Y;
            double z = xyz.Z;
            SphereCoord sphere = XyzToSphere(x, y, z, unit);
            return sphere;
        }


        #region ͨ�ú����㷨

        /// <summary>
        /// XYZ to Sphere Coordinate
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static SphereCoord XyzToSphere(double x, double y, double z, AngleUnit unit = AngleUnit.Degree)
        {
            double radius = Math.Sqrt(x * x + y * y + z * z);
            double lon = Math.Atan2(y, x);
            double lat = Math.Atan2(z, Math.Sqrt(x * x + y * y));
             
            lon = AngularConvert.RadTo(lon, unit);
            lat = AngularConvert.RadTo(lat, unit);

            SphereCoord sphere = new SphereCoord(lon, lat, radius);
            return sphere;
        }
        /// <summary>
        /// Sphere to XYZ Coordinate
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static XYZ SphereToXyz(double lon, double lat, double r, AngleUnit unit = AngleUnit.Degree)
        { 
            lon = AngularConvert.ToRad(lon, unit);
            lat = AngularConvert.ToRad(lat, unit);

            double x = r * Cos(lat) * Cos(lon);
            double y = r * Cos(lat) * Sin(lon);
            double z = r * Sin(lat);
            return new XYZ(x, y, z);
        }

        #endregion
        #endregion
        /// <summary>
        /// �����������漸�α���λ�� 0-360
        /// </summary>
        /// <param name="satellitePos"></param>
        /// <param name="staPos"></param>
        /// <returns></returns>
        public static double GetAzimuthAngle(XYZ staPos, XYZ satellitePos)
        {
            double xy = staPos.X * staPos.X + staPos.Y * staPos.Y;
            double xyz = xy + staPos.Z * staPos.Z;
            xy = Math.Sqrt(xy);
            xyz = Math.Sqrt(xyz);

            if (xy <= Math.Pow(10, -14) || xyz <= Math.Pow(10, -14))
            {
                throw new Exception("Divide by Zero Error");
            }

            double cos1 = staPos.X / xy;
            double sin1 = staPos.Y / xy;
            double sint = staPos.Z / xyz;

            double xn1 = -sint * cos1;
            double xn2 = -sint * sin1;
            double xn3 = xy / xyz;


            double xe1 = -sin1;
            double xe2 = cos1;


            double z1 = satellitePos.X - staPos.X;
            double z2 = satellitePos.Y - staPos.Y;
            double z3 = satellitePos.Z - staPos.Z;

            double y = xn1 * z1 + xn2 * z2 + xn3 * z3;
            double x = xe1 * z1 + xe2 * z2;


            double test = Math.Abs(y) + Math.Abs(x);
            if (test < Math.Pow(10, -16))
            {
                throw new Exception("AzAngle(), failed p1+p2 test.");
            }

            return GetAzimuthAngleInRightHandXyCoordSystem( x, y);
        }
        /// <summary>
        /// ƽ�淽λ�ǣ�Y����Ϊ���򣬲��� ��˹����, 0-360
        /// ���ֿռ�ֱ������ϵ��
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double GetAzimuthAngleInRightHandXyCoordSystem(double x, double y)
        {
            double az = 90 - Math.Atan2(y, x) * CoordConsts.RadToDegMultiplier;
            if (az < 0)
            {
                return az + 360;
            }
            else
            {
                return az;
            }
        }
        /// <summary>
        /// �������㷽λ�ǣ�X ����Ϊ������ ��˹����, 0-360�ȣ����ֿռ�ֱ������ϵ
        /// </summary>
        /// <param name="xyVector"></param>
        /// <returns></returns>
        public static double GetAzimuthAngleInLeftHandXyCoordSystem(XY xyVector){ return GetAzimuthAngleInLeftHandXyCoordSystem(xyVector.X, xyVector.Y); }
        /// <summary>
        /// ƽ�淽λ�ǣ�X ����Ϊ������ ��˹����, 0-360��
        /// ���ֿռ�ֱ������ϵ��
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double GetAzimuthAngleInLeftHandXyCoordSystem(double x, double y)
        {
            double az = Math.Atan2(y, x) * CoordConsts.RadToDegMultiplier;
            if (az < 0)
            {
                return az + 360;
            }
            else
            {
                return az;
            }
        }


        /// <summary>
        /// ƽ�淽λ�ǣ�X ����Ϊ���򣬸�˹����, 0-360
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double GetAzimuthAngleOfLeftHandXy(XY start, XY end)
        {
            var vec = end - start;
            return GetAzimuthAngleInLeftHandXyCoordSystem(vec.X, vec.Y); 
        }

        /// <summary>
        /// Finds the elevation angle of the second point with respect to the prevObj point
        /// Cui Yang, Add.
        /// </summary>
        /// <param name="staPos"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static double EleAngle(XYZ staPos, XYZ right)
        {
            XYZ z = right - staPos;
            double c = CosVector(staPos, staPos);

            double res = 90.0 - Math.Acos(c) * CoordConsts.RadToDegMultiplier;
            return res;
        }
        /// <summary>
        /// Function that returns the cosine of angle between this and right
        /// Cui Yang, Added, 2014.06.2
        /// </summary>
        /// <param name="staPos"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static double CosVector(XYZ staPos, XYZ right)
        {
            double rx = right.Dot(staPos);
            double ry = right.Dot(right);
            if (rx <= Math.Pow(10, -15) || ry <= Math.Pow(10, -15))
            {
                throw new Exception("Divide by Zero Error");
            }

            double cosvects = right.Dot(right) / Math.Sqrt(rx * ry);

            //this if checks for and corrects round off error
            if (Math.Abs(cosvects) > 1.0e0)
            {
                cosvects = Math.Abs(cosvects) / cosvects;
            }
            return cosvects;
        }
        /// <summary>
        /// �ֲ������ת����
        /// </summary>
        /// <param name="hen"></param>
        /// <returns></returns>
        public static NEU HenToNeu(HEN hen)
        {
            return new NEU(hen.N, hen.E, hen.H);
        }

        #region ����
        /// <summary>
        ///  Math.Cos(currentVal)
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private static double Cos(double val) { return Math.Cos(val); }
        /// <summary>
        ///  Math.Sin(currentVal)
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private static double Sin(double val) { return Math.Sin(val); }
        /// <summary>
        /// Math.Tan(currentVal)
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private static double Tan(double val) { return Math.Tan(val); }
        #endregion
    }
}
