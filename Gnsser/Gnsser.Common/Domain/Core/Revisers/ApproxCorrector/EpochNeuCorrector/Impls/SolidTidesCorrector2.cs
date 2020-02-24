//2015.01.20，崔阳，借鉴gLAB、RTKLIB等的固体潮模型和IERS2010规范，重新编程固体潮改正模型，可能存在的问题是太阳、月亮的位置计算。
//2015.04.12， cy, 修正了ERP文件的读取错误

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;
using Gnsser;
using Gnsser.Times;
using Gnsser.Domain;
using Geo.Times;
using Geo.Algorithm;
using Gnsser.Data;

namespace Gnsser.Correction
{
    public class SolidTidesCorrector2 : AbstractEpochNeuReviser
    {

        /// <summary>
        /// 固体潮改正，单位NEU
        /// 采用IERS2010的规范。需要ERP文件支持。
        /// 存在的问题是太阳、月亮的位置计算，不知道如何检验。
        /// </summary>
        public SolidTidesCorrector2(DataSourceContext DataSouceProvider)
        {
            this.Name = "固体潮改正";
            this.CorrectionType = CorrectionType.SolidTides;
            this.DataSouceProvider = DataSouceProvider;
        }
        DataSourceContext DataSouceProvider;
        //Love numbers
        // private  const double H_LOVE = 0.609, L_LOVE = 0.0852;
        private const double H_LOVE = 0.6078, L_LOVE = 0.0847; //IERS 2010 

        // Phase lag. Assumed as zero here because no phase lag has been 
        // detected so far.
        private const double PH_LAG = 0.0;

        public override void Correct(EpochInformation epochInformation)
        {
            if (XYZ.IsZeroOrEmpty(epochInformation.SiteInfo.EstimatedXyz) || epochInformation.EnabledSatCount ==0 || epochInformation.ReceiverTime.IsZero) { this.Correction = new NEU(); return; }

            //   Time gpsTime = epochInformation.CorrectedTime;
            Time gpsTime = epochInformation.ReceiverTime;

            XYZ reciverPosition = epochInformation.SiteInfo.EstimatedXyz;

            GeoCoord geoPosition = epochInformation.SiteInfo.ApproxGeoCoord;

            XYZ rsun = new XYZ(); XYZ rmoon = new XYZ(); double gmst = 0;

            //gpst2utc
            Time tutc = gpsTime.GpstToUtc();

            //查找地球自转信息
            Gnsser.Data.ErpItem erpv = null;
            if (DataSouceProvider.ErpDataService != null)
            {
                erpv = DataSouceProvider.ErpDataService.Get(tutc);
            }
            if (erpv == null) erpv = ErpItem.Zero;

            Time tut = tutc + erpv.Ut12Utc;

            //采用RTKLIB的太阳月亮位置计算方法（历书计算方法不同，相关文献表明对最终的定位精度影响差别不大）
            DataSouceProvider.UniverseObjectProvider.GetSunPosition(gpsTime, erpv, ref rsun, ref rmoon, ref gmst);

            NEU Correction = GetSolidTidesCorrectValue(geoPosition, reciverPosition, rsun, rmoon, gmst);

            this.Correction = (Correction);
        }

        private static NEU GetSolidTidesCorrectValue(GeoCoord geoPosition, XYZ reciverPosition, XYZ rsun, XYZ rmoon, double gmst)
        {
            double[] pos = new double[2];
            pos[0] = Math.Asin(reciverPosition.Z / reciverPosition.Length);
            pos[1] = Math.Atan2(reciverPosition.Y, reciverPosition.X);

            double sinp = Math.Sin(pos[0]); double cosp = Math.Cos(pos[0]);
            double sinl = Math.Sin(pos[1]); double cosl = Math.Cos(pos[1]);

            double[] E = new double[9];

            E[0] = -sinl; E[3] = cosl; E[6] = 0.0;
            E[1] = -sinp * cosl; E[4] = -sinp * sinl; E[7] = cosp;
            E[2] = cosp * cosl; E[5] = cosp * sinl; E[8] = sinp;


            double[] eu = new double[3];
            /* step1: time domain */
            eu[0] = E[2]; eu[1] = E[5]; eu[2] = E[8];


            double[] dr1 = tide_pl(eu, rsun, SunMoonPosition.GMS, pos);

            double[] dr2 = tide_pl(eu, rmoon, SunMoonPosition.GMM, pos);

            /* step2: frequency domain, only K1 radial */
            double sin2l = Math.Sin(2.0 * pos[0]);
            double du = -0.012 * sin2l * Math.Sin(gmst + pos[1]);

            double[] dr = new double[3];
            dr[0] = dr1[0] + dr2[0] + du * E[2]; //x
            dr[1] = dr1[1] + dr2[1] + du * E[5]; //y
            dr[2] = dr1[2] + dr2[2] + du * E[8]; //z


            double lat = geoPosition.Lat;
            double lon = geoPosition.Lon;

            double refLat = 0, refLon = 0;
            if (lat > 90.0 || lat < -90.0)
            {
                //如果在此，说明有大错误！
                refLat = 0.0;
            }
            else
            {
                refLat = lat * SunMoonPosition.DegToRad;
            }
            refLon = lon * SunMoonPosition.DegToRad;




            double sinlat = Math.Sin(refLat); double coslat = Math.Cos(refLat);
            double sinlon = Math.Sin(refLon); double coslon = Math.Cos(refLon);

            double[][] rotate ={
                                  new double[]{-coslon*sinlat,-sinlon,coslat*coslon},
                                   new double[]{-sinlon*sinlat,coslon,coslat*sinlon},
                                    new double[]{coslat,0.0,sinlat},
                              };


            double detN = rotate[0][0] * dr[0] + rotate[1][0] * dr[1] + rotate[2][0] * dr[2];
            double detE = rotate[0][1] * dr[0] + rotate[1][1] * dr[1] + rotate[2][1] * dr[2];
            double detH = rotate[0][2] * dr[0] + rotate[1][2] * dr[1] + rotate[2][2] * dr[2];


            double e11 = -sinlon; double e12 = coslon; double e13 = 0.0;
            double e21 = -sinlat * coslon; double e22 = -sinlat * sinlon; double e23 = coslat;
            double e31 = coslat * coslon; double e32 = coslat * sinlon; double e33 = sinlat;

            double e = e11 * dr[0] + e12 * dr[1] + e13 * dr[2];
            double n = e21 * dr[0] + e22 * dr[1] + e23 * dr[2];
            double u = e31 * dr[0] + e32 * dr[1] + e33 * dr[2];



            NEU newCorrection = new NEU(n, e, u);
            return newCorrection;
        }

        private static double[] tide_pl(double[] eu, XYZ rp, double GMp, double[] pos)
        {
            double[] dr = new double[3];
            double H3 = 0.292, L3 = 0.015;
            double r, latp, lonp, p, K2, K3, a, H2, L2, dp, du, cosp, sinl, cosl;


            r = rp.Length;
            double[] ep = new double[3];
            ep[0] = rp.X / r; ep[1] = rp.Y / r; ep[2] = rp.Z / r;

            K2 = GMp / SunMoonPosition.GME * (SunMoonPosition.RE_WGS84 * SunMoonPosition.RE_WGS84 * SunMoonPosition.RE_WGS84 * SunMoonPosition.RE_WGS84) / (r * r * r);
            K3 = K2 * SunMoonPosition.RE_WGS84 / r;
            latp = Math.Asin(ep[2]);
            lonp = Math.Atan2(ep[1], ep[0]);

            cosp = Math.Cos(latp); sinl = Math.Sin(pos[0]); cosl = Math.Cos(pos[0]);

            /* step1 in phase (degree 2) */
            p = (3.0 * sinl * sinl - 1.0) / 2.0;
            H2 = 0.6078 - 0.0006 * p;
            L2 = 0.0847 + 0.0002 * p;
            a = dot(ep, eu, 3);
            dp = K2 * 3.0 * L2 * a;
            du = K2 * (H2 * (1.5 * a * a - 0.5) - 3.0 * L2 * a * a);

            /* step1 in phase (degree 3) */
            dp += K3 * L3 * (7.5 * a * a - 1.5);
            du += K3 * (H3 * (2.5 * a * a * a - 1.5 * a) - L3 * (7.5 * a * a - 1.5) * a);

            /* step1 out-of-phase (only radial) */
            du += 3.0 / 4.0 * 0.0025 * K2 * Math.Sin(2.0 * latp) * Math.Sin(2.0 * pos[0]) * Math.Sin(pos[1] - lonp);
            du += 3.0 / 4.0 * 0.0022 * K2 * cosp * cosp * cosl * cosl * Math.Sin(2.0 * (pos[1] - lonp));

            dr[0] = dp * ep[0] + du * eu[0];
            dr[1] = dp * ep[1] + du * eu[1];
            dr[2] = dp * ep[2] + du * eu[2];

            return dr;
        }

        private static double dot(double[] a, double[] b, int n)
        {
            double c = 0.0;
            while (--n >= 0) c += a[n] * b[n];
            return c;
        }



    }
}
