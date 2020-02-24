using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gnsser
{

    /// <summary>
    /// GNSS数据在服务器上的地址
    /// </summary>
    public class GnssUrl
    {
        public const string YEAR = "YEAR";
        public const string DAY_OF_YEAR_3CHAR = "DAY_OF_YEAR_3";
        public const string NAME = "NAME";
        public const string YEAR_2CHAR = "YEAR_2";

        string urlRule = "ftp://25.20.220.196/{YEAR}/{DAY_OF_YEAR_3}/{NAME}{DAY_OF_YEAR_3}0.{YEAR_2}d.Z";
        string year, year_2;

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="year">Year 为四位数</param>
        /// <param name="urlRule">ftp://25.20.220.196/{YEAR}/{DAY_OF_YEAR_3}/{NAME}{DAY_OF_YEAR_3}0.{YEAR_2}d.Z</param>
        public GnssUrl(int year, string urlRule)
        {
            this.urlRule = urlRule;
            this.year = year.ToString();
            this.year_2 = year.ToString().Substring(2);
        }

        public List<string> GetUrls(int fromDay, int toDay, string[] siteNames)
        {
            List<string> urls = new List<string>();
            for (int i = fromDay; i <= toDay; i++)
            {
                foreach (string siteName in siteNames)
                {
                    urls.Add(GetUrl(siteName.ToLower(), i));
                }
            }
            return urls;
        }

        public string GetUrl(string siteName, int dayOfYear)
        {
            string url = urlRule
                           .Replace(DAY_OF_YEAR_3CHAR, GetDayOfYearString(dayOfYear))
                           .Replace(YEAR_2CHAR, year_2)
                           .Replace(NAME, siteName)
                           .Replace(YEAR, year)
                           .Replace("{", "")
                           .Replace("}", "");
            return url;
        }
        /// <summary>
        /// 获取3位数字的年积日
        /// </summary>
        /// <param name="dayOfYear"></param>
        /// <returns></returns>
        public static string GetDayOfYearString(int dayOfYear)
        {
            string str = dayOfYear.ToString();
            while (str.Length < 3) str = "0" + str;
            return str;
        }
    }
}
