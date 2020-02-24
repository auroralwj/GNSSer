using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Utils;

namespace Gnsser.Data.Sinex
{
    /// <summary>
    /// 参数估计值。
    /// </summary>
    public class SolutionEstimateGroup
    {
        public SolutionEstimateGroup() { }
        public SolutionEstimateGroup(List<SolutionValue> Items)
        {
            this.Items = Items;
            if (Items.Count != 3) throw new ArgumentException("必须是3个。");
            if (Items.Count(m => (m.SiteCode == Items[0].SiteCode)) != Items.Count) throw new ArgumentException("测站名称不一致。");
            // this.Items.Sort(delegate(SolutionValue coeffOfParams, SolutionValue b) { return coeffOfParams.ParameterType.CompareTo(b.ParameterType); });
            this.Xyz = new XYZ(
                   Items[0].ParameterValue,
                   Items[1].ParameterValue,
                   Items[2].ParameterValue);
            this.GeoCoord = Geo.Coordinates.CoordTransformer.XyzToGeoCoord(Xyz);
        }


        public List<SolutionValue> Items { get; set; }
        public XYZ Xyz { get; set; }
        public GeoCoord GeoCoord { get; set; }

        /// <summary>
        /// 假设list的XYZ为相邻。分别为XYZ。
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<SolutionEstimateGroup> GetGroups(List<SolutionValue> list)
        {
            List<SolutionEstimateGroup> group = new List<SolutionEstimateGroup>();
            //首先进行排序
            // IOrderedEnumerable<SolutionValue> items = list.OrderBy(m => m.SiteCode);

            for (int i = 0; i < list.Count; i ++)
            {
                if (list[i].ParameterType == ParameterType.STAX)//说明开始了
                {
                    List<SolutionValue> ls = new List<SolutionValue>();
                    if (list.Count > i && list[i].ParameterType == ParameterType.STAX) ls.Add(list[i]);
                    if (list.Count > i + 1 && list[i + 1].ParameterType == ParameterType.STAY) ls.Add(list[i + 1]);
                    if (list.Count > i + 2 && list[i + 2].ParameterType == ParameterType.STAZ) ls.Add(list[i + 2]);

                    if (ls.Count == 3)
                    {
                        SolutionEstimateGroup g = new SolutionEstimateGroup(ls);
                        group.Add(g);
                    }
                }
            }
            return group;
        }
    }

}
