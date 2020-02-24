//2016.11.28， czs & cy & double, 是否缺失测站检核器

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Gnsser.Data;
using Gnsser.Domain;


namespace Gnsser.Checkers
{
    /// <summary>
    /// 是否缺失测站检核器
    /// </summary>
    public class EpochSitesMissingChecker : Checker<MultiSiteEpochInfo>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="siteCount"></param>
        public EpochSitesMissingChecker(int siteCount)
        {
            this.siteCount = siteCount;
            Name = "测站数量检核器";
        }
        int siteCount;

        public override bool Check(MultiSiteEpochInfo t)
        {
            if (t.Count != siteCount)
            {
                log.Error("测站数量错误，应该是 " + this.siteCount + ", 实际上为 " +t.Count);
                return false;
            }
            return true;
        }

         
    }

}
