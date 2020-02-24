//2016.08.29, czs, create in hongqing, 是否包含了指定的卫星

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
using Gnsser.Excepts;
using Geo;

namespace Gnsser.Checkers
{
    //2016.10.19, czs, create in hongqing, 多站指定卫星检核

    /// <summary>
    /// 多站指定卫星检核
    /// </summary>
    public class MultiSiteIndicatedSatContainedChecker : BaseDictionary<string, IndicatedSatContainedChecker>, IChecker<MultiSiteEpochInfo>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="BasePrn"></param>
        public MultiSiteIndicatedSatContainedChecker(SatelliteNumber BasePrn)
        {
            this.Name = "指定卫星检核器 " + BasePrn;
            this.BasePrn = BasePrn;
        }
        #region 属性
        /// <summary>
        /// 基准卫星
        /// </summary>
        public SatelliteNumber BasePrn { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        #endregion

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override IndicatedSatContainedChecker Create(string key)
        {
            return new IndicatedSatContainedChecker(BasePrn) { Name = key };
        }
        /// <summary>
        /// 检核
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Check(MultiSiteEpochInfo t)
        {
            foreach (var item in t)
            {
                return this.GetOrCreate(item.Name).Check(item);
            }

            return true;
        }

        public Exception Exception
        {
            get { return  new NotImplementedException(); }
        }

    }






    /// <summary>
    /// 是否包含了指定的卫星
    /// </summary>
    public class IndicatedSatContainedChecker : EpochInfoChecker
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="prn">必须包含的卫星</param>
        public IndicatedSatContainedChecker(SatelliteNumber prn)
        {
            this.Name = "指定的卫星检核";
            this.Prns = new List<SatelliteNumber>(){ prn};
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="shouldContainPrns">必须包含的卫星</param>
        public IndicatedSatContainedChecker(List<SatelliteNumber> shouldContainPrns)
        {
            this.Name = "指定的卫星检核";
            this.Prns = shouldContainPrns;
        }
        /// <summary>
        /// 编号
        /// </summary>
        public List<SatelliteNumber> Prns { get; set; }

        /// <summary>
        /// 检核是否满足要求
        /// </summary>
        /// <param name="epochInfo"></param>
        public override bool Check(EpochInformation epochInfo)
        {
            foreach (var prn in Prns)
            {
                if (!epochInfo.TotalPrns.Contains(prn))
                {
                    log.Debug(epochInfo.Name + " " + epochInfo.ReceiverTime.ToShortTimeString() + " 不包含指定的卫星： " + prn + "，检核不通过！");
                    return false;
                }
            }

            return true;
        }
    }
}
