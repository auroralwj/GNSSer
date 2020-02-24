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
    /// 参数先言值。
    /// </summary>
    public class SolutionAprioriGroup
    {
        public SolutionAprioriGroup() { }
        public SolutionAprioriGroup(List<SolutionValue> Items)
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
        /// <param name="colName"></param>
        /// <returns></returns>
        public static List<SolutionAprioriGroup> GetGroups(List<SolutionValue> list)
        {
            List<SolutionAprioriGroup> group = new List<SolutionAprioriGroup>();
            //首先进行排序
            // IOrderedEnumerable<SolutionValue> items = colName.OrderBy(m => m.SiteCode);

            for (int i = 0; i < list.Count / 3; i = i + 3)
            {
                List<SolutionValue> ls = new List<SolutionValue>();
                ls.Add(list[i]);
                ls.Add(list[i + 1]);
                ls.Add(list[i + 2]);

                SolutionAprioriGroup g = new SolutionAprioriGroup(ls);
                group.Add(g);
            }
            return group;
        }
    }  

}
