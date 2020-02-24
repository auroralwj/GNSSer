//2018.12.27, czs, create in ryd, 基准测站选择

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;
using Gnsser.Domain;

namespace Gnsser
{
    /// <summary>
    /// 基准测站选择类型
    /// </summary>
    public enum BaseSiteSelectType
    {
        /// <summary>
        /// 几何中心
        /// </summary>
        GeoCenter,
        /// <summary>
        /// 第一个
        /// </summary>
        First,
        /// <summary>
        /// 最后一个
        /// </summary>
        Last,
        /// <summary>
        /// 外部指定
        /// </summary>
        Indicated
    }


    /// <summary>
    /// 基准测站选择。
    /// </summary>
    public class BaseSiteSelection
    {
         /// <summary>
         /// 构造函数
         /// </summary>
         public BaseSiteSelection(BaseSiteSelectType BaseSiteSelectType, string IndicatedSiteName)
         {
            this.BaseSiteSelectType = BaseSiteSelectType;
            this.IndicatedSiteName = IndicatedSiteName;
         }

        /// <summary>
        /// 基站选择方法
        /// </summary>
        public BaseSiteSelectType BaseSiteSelectType { get; set; }
        /// <summary>
        /// 指定的测站名称
        /// </summary>
        public string IndicatedSiteName { get; set; }

        /// <summary>
        /// 获取基准测站
        /// </summary>
        /// <param name="obsFiles"></param>
        /// <returns></returns>
        public ObsSiteInfo GetBaseSite(List<ObsSiteInfo> obsFiles)
        {
            switch (BaseSiteSelectType)
            {
                case BaseSiteSelectType.GeoCenter:
                    return GetBaseSiteWithGeoCenter(obsFiles);
                    break;
                case BaseSiteSelectType.First:
                    return obsFiles[0];
                case BaseSiteSelectType.Last:
                    return obsFiles[obsFiles.Count -1];
                    break;
                case BaseSiteSelectType.Indicated:
                    var baseSite = obsFiles.Find(m => m.SiteName == IndicatedSiteName);
                    return baseSite;
                    break;
                default:
                    break;
            }
            return GetBaseSiteWithGeoCenter(obsFiles);
        }

        private static ObsSiteInfo GetBaseSiteWithGeoCenter(List<ObsSiteInfo> obsFiles)
        {
            XYZ coord = null;

            int i = 0;
            foreach (var item in obsFiles)
            {
                var xyz = item.SiteObsInfo.ApproxXyz;
                if (!xyz.IsZero)
                {
                    i++;
                    if (coord == null)
                    {
                        coord = xyz;
                        continue;
                    }
                    coord += xyz;
                }
            }

            var center = coord / i;
            ObsSiteInfo site = null;
            double distance = double.MaxValue;
            foreach (var item in obsFiles)
            {
                var xyz = item.SiteObsInfo.ApproxXyz;
                if (xyz.IsZero) { continue; }
                var dis = (xyz - center).Length;
                if (dis < distance)
                {
                    distance = dis;
                    site = item;
                }
            }
            return site;
        }

        public override string ToString()
         {
             return BaseSiteSelectType.ToString();
         }
    }
}
