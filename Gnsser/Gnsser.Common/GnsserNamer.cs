//2019.03.03, czs, create in hongqing, GNSSer 名字计算

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnsser
{
    /// <summary>
    /// GNSSer 名字计算
    /// </summary>
    public class GnsserNamer
    {
        public const string DifferSpliter = "-";
        public const string NameSpliter = "_";

        public static string BuildDiffer(SatelliteNumber prn, SatelliteNumber basePrn)
        {
            return prn + DifferSpliter + basePrn;
        }
        public static string BuildDiffer(string prn, string basePrn)
        {
            return prn + DifferSpliter + basePrn;
        }
        public static string BuildDiffer(object prn, object basePrn)
        {
            return prn + DifferSpliter + basePrn;
        }

        public static List<string> ParseDiffer(string name)
        {
            var list = Parse(name);
            var first = list.First();

            return new List<string>(first.Split(new string[] { DifferSpliter }, StringSplitOptions.RemoveEmptyEntries));
         }
        public static List<string> Parse(string name)
        {
            return new  List<string>( name.Split(new string[] { NameSpliter }, StringSplitOptions.RemoveEmptyEntries));

        }

    }
}
