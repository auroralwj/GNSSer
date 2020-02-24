using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;

namespace Gnsser.Data.Sinex
{
    /// <summary>
    /// 测站，及其属性。
    /// </summary>
    public class SinexSiteDetail: SinexSite
    {
        public double AprioriX { get { if (AprioriXYZ != null) return AprioriXYZ.X; return 0; } }
        public double AprioriY { get { if (AprioriXYZ != null) return AprioriXYZ.Y; return 0; } }
        public double AprioriZ { get { if (AprioriXYZ != null) return AprioriXYZ.Z; return 0; } }

        public double AprioriXyzStdDevX { get { if (AprioriXyzStdDev != null) return AprioriXyzStdDev.X; return 0; } }
        public double AprioriXyzStdDevY { get { if (AprioriXyzStdDev != null)return AprioriXyzStdDev.Y; return 0; } }
        public double AprioriXyzStdDevZ { get { if (AprioriXyzStdDev != null)return AprioriXyzStdDev.Z; return 0; } }


        public double EstimateX { get { if (EstimateXYZ != null)return EstimateXYZ.X; return 0; } }
        public double EstimateY { get { if (EstimateXYZ != null)return EstimateXYZ.Y; return 0; } }
        public double EstimateZ { get { if (EstimateXYZ != null)return EstimateXYZ.Z; return 0; } }

        public double EstimateXyzStdDevX { get { if (EstimateXyzStdDev != null)return EstimateXyzStdDev.X; return 0; } }
        public double EstimateXyzStdDevY { get { if (EstimateXyzStdDev != null) return EstimateXyzStdDev.Y; return 0; } }
        public double EstimateXyzStdDevZ { get { if (EstimateXyzStdDev != null)return EstimateXyzStdDev.Z; return 0; } }


        /// <summary>
        /// 与另一个测站进行比较
        /// </summary>
        /// <param name="another"></param>
        /// <returns></returns>
        public SinexSiteDetail Compare(SinexSiteDetail another) { return Compare(this, another); }

        /// <summary>
        /// 比较列表。以A为基准，比较两个列表同时有的测站。
        /// </summary>
        /// <param name="sitesA"></param>
        /// <param name="sitesB"></param>
        /// <returns></returns>
        public static List<SinexSiteDetail> Compare(List<SinexSiteDetail> sitesA, List<SinexSiteDetail> sitesB)
        {
            List<SinexSiteDetail> list = new List<SinexSiteDetail>();
            foreach (var item in sitesA)
            {
                SinexSiteDetail site = sitesB.Find(m => m.Name == item.Name);
                if (site != null)
                {
                    list.Add(item.Compare(site));
                }
            }
            return list;
        }

        /// <summary>
        /// 两个测站信息之差。
        /// </summary>
        /// <param name="one"></param>
        /// <param name="another"></param>
        /// <returns></returns>
        public static SinexSiteDetail Compare(SinexSiteDetail one, SinexSiteDetail another)
        {
            if (one.Name != another.Name)
            {
                throw new ArgumentException("必须是同一测站才能比较。" + one.Name + " " + another.Name);
            }

            SinexSiteDetail site = new SinexSiteDetail()
            {
                Name = one.Name,
                Antenna = one.Antenna,
                DateEnd = one.DateEnd,
                ApproxGeoCoord = one.ApproxGeoCoord - another.ApproxGeoCoord,
                DateStart = one.DateStart,
                EstimateGeoCoord = one.EstimateGeoCoord - another.EstimateGeoCoord,
                EstimateXYZ = one.EstimateXYZ - another.EstimateXYZ,
                EstimateXyzStdDev = one.EstimateXyzStdDev - another.EstimateXyzStdDev,
                Receiver = one.Receiver
            };
            if( one.Eccentricity  != null && null != another.Eccentricity)
            {
                site.Eccentricity = one.Eccentricity - another.Eccentricity;
            }
            if (one.AprioriGeoCoord != null && null != another.AprioriGeoCoord)
            {
                site.AprioriGeoCoord = one.AprioriGeoCoord - another.AprioriGeoCoord;
                site.AprioriXYZ = one.AprioriXYZ - another.AprioriXYZ;
                site.AprioriXyzStdDev = one.AprioriXyzStdDev - another.AprioriXyzStdDev;
            }
            return site;
        }

    }
}
