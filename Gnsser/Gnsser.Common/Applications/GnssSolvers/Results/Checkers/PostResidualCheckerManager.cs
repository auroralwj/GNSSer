 //2017.10.30, czs, create in hongqing, 验后残差分析

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Checkers;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo.Times;
using Geo.IO;

namespace Gnsser.Service
{

    /// <summary>
    /// 参数检查
    /// </summary>
    public class PostResidualCheckerManager
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PostResidualCheckerManager(double MaxErrorTimes = 5)
        {
            ElementResidualCheckers = new TimeNumeralWindowDataManager<string>(5, 5);
            this.MaxErrorTimes = MaxErrorTimes;
        }
        /// <summary>
        /// 最大误差倍数
        /// </summary>
        public double MaxErrorTimes { get; set; }
        /// <summary>
        /// 残差分量检查
        /// </summary>
        TimeNumeralWindowDataManager<string> ElementResidualCheckers { get; set; }


        /// <summary>
        /// 获取失败的卫星,具有周跳的，已经进行了降权，因此不返回。
        /// </summary>
        /// <returns></returns>
        public List<SatelliteNumber> GetBadPrns(BaseGnssResult product)
        {
            List<SatelliteNumber> badPrns = new List<SatelliteNumber>();
            var csedPrns = product.Material.UnstablePrns;//忽略周跳卫星的检查


            var postResiduals = product.ResultMatrix.PostfitResidual;

            if (product is NetDoubleDifferPositionResult || product is DualSiteEpochDoubleDifferResult)
            {
                postResiduals = postResiduals.FilterContains(ParamNames.DoubleDiffer + ParamNames.PhaseL);

                var collection = GetBadElementResidual(postResiduals);

                foreach (var item in collection)
                {
                    var name = NetDoubleDifferName.Parse(item);
                    AddToBadPrns(badPrns, csedPrns, name.RovPrn);
                }                
            }else if(product.Material is EpochInformation)
            {
                var collection = GetBadElementResidual(postResiduals);
                foreach (var item in collection)
                {
                    SatelliteNumber prn = SatelliteNumber.Parse(item);
                    if(prn.PRN == 0) { continue; }

                    AddToBadPrns(badPrns, csedPrns, prn);
                } 
            } 


            return badPrns;
        }

        private static void AddToBadPrns(List<SatelliteNumber> badPrns, List<SatelliteNumber> csedPrns, SatelliteNumber prn)
        {
            if (!csedPrns.Contains(prn) && !badPrns.Contains(prn))
            {
                badPrns.Add(prn);
            }
        }

        /// <summary>
        /// 残差分量检查
        /// </summary>
        /// <param name="postResiduals"></param>
        /// <returns></returns>
        public List<string> GetBadElementResidual(WeightedVector postResiduals)
        {
            List<string> list = new List<string>();
            var count = postResiduals.Count;
            //for (int i = 0; i < count; i++)
            //    //纵向比较，统计误差超过5倍的
            //{
            //    var name = postResiduals.ParamNames[i];
            //    if (String.IsNullOrWhiteSpace(name)) { return list; }//
            //    var window = ElementResidualCheckers.GetOrCreate(name);
            //    var val = postResiduals[i];
            //    var isOk = (window.AverageCheckAddOrClear(product.ReceiverTime, val, MaxErrorTimes));
            //    if (!isOk )
            //    {
            //        list.Add(name);
            //    }
            //}

            //横向比较，统计误差，统计误差超过5倍的
            var errored = Geo.Utils.DoubleUtil.GetIndexesOfGrossError(postResiduals.GetRmsVector(), MaxErrorTimes, false);
            foreach (var index in errored)
            {
                var name = postResiduals.ParamNames[index];
                if (!list.Contains(name))
                {
                    list.Add(name);
                }
            }

            //直接比较残差
            errored = Geo.Utils.DoubleUtil.GetIndexesOfGrossError(postResiduals, MaxErrorTimes, false);
            foreach (var index in errored)
            {
                var name = postResiduals.ParamNames[index];
                if (!list.Contains(name))
                {
                    list.Add(name);
                }
            }



            return list;
        }


    }
}