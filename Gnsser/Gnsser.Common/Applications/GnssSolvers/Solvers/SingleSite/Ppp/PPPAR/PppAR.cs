using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Data.Rinex;
using Gnsser.Data;
using Gnsser.Domain;
using Gnsser.Checkers;
using Gnsser;
using Geo.Common;
using Gnsser.Times;
using System.Data;
using Geo.Times;
using Geo.IO;

namespace Gnsser.Service
{
    public class PppAR
    {
        public PppAR(ISingleSiteObsStream dataSource, SatelliteNumber refsat, List<SatelliteType> satellitetype)
        {
            this.dataSource = dataSource;
            this.refsatellite = refsat;
            this.SatelliteTypes = satellitetype;
            this.Interval = dataSource.ObsInfo.Interval;
            this.Checkers = new List<ICycleSlipDetector>();
        }

        #region 属性
        /// <summary>
        /// 支持系统的类型
        /// </summary>
        public List<SatelliteType> SatelliteTypes { get; protected set; }

        /// <summary>
        /// 观测量数据源
        /// </summary>
        public ISingleSiteObsStream dataSource { get; set; }

        /// <summary>
        /// 将采样间隔读进来，以便于删除独立的数据
        /// </summary>
        public double Interval;

        /// <summary>
        /// 参考星
        /// </summary>
        public SatelliteNumber refsatellite;

        /// <summary>
        /// 日志记录。错误信息记录在日志里面。
        /// </summary>
        protected ILog log = Log.GetLog(typeof(PppAR));

        /// <summary>
        /// 干净的EpochInformation
        /// </summary>
        List<EpochInformation> cleanEpochInformation = new List<EpochInformation>();

        /// <summary>
        /// 每个历元的卫星宽巷
        /// </summary>
        Dictionary<Time, Dictionary<SatelliteNumber, double>> WL = new Dictionary<Time, Dictionary<SatelliteNumber, double>>();
        /// <summary>
        /// 相对于参考星的每个历元的单差UPD
        /// </summary>
        Dictionary<Time, Dictionary<SatelliteNumber, double>> SDWL = new Dictionary<Time, Dictionary<SatelliteNumber, double>>();

        /// <summary>
        /// 每颗卫星的宽巷
        /// </summary>
        Dictionary<SatelliteNumber, Dictionary<Time, double>> sat_wl = new Dictionary<SatelliteNumber, Dictionary<Time, double>>();

        /// <summary>
        /// 相对于参考星的每颗卫星的单差UPD
        /// </summary>
        SortedDictionary<SatelliteNumber, Dictionary<Time, double>> sat_swdl = new SortedDictionary<SatelliteNumber, Dictionary<Time, double>>();


        /// <summary>
        /// 相对于参考星的UPD均值
        /// </summary>
        Dictionary<SatelliteNumber, double> SDWLFCB = new Dictionary<SatelliteNumber, double>();
        /// <summary>
        /// 每颗卫星的精度
        /// </summary>
        Dictionary<SatelliteNumber, double> sat_sigma = new Dictionary<SatelliteNumber, double>();

        /// <summary>
        /// 周跳探测
        /// </summary> 
        private List<ICycleSlipDetector> Checkers { get; set; }
        #endregion

        public void calculateMW()
        {
            //CycleSlipRemoveChainProcessor CycleSlipRemoveChainProcessor = new Checkers.CycleSlipRemoveChainProcessor();

            //CycleSlipRemoveChainProcessor.Add(new Gnsser.Checkers.LiCycleSlipRemoveor());
            //CycleSlipRemoveChainProcessor.Add(new Gnsser.Checkers.MwCycleSlipRemoveor());
            List<EpochInformation> epochnInfos;
            //Dictionary<SatelliteNumber, Time> prns = new Dictionary<SatelliteNumber, Time>();

            //Dictionary<SatelliteNumber, int> satepocount = new Dictionary<SatelliteNumber, int>();
            ////Dictionary<Time, List<SatelliteNumber>> cycleprns = new Dictionary<Time, List<SatelliteNumber>>();
            //List<SatelliteNumber> sats = new List<SatelliteNumber>();
            while ((epochnInfos = LoadNextOne()).Count > 0)
            {
                EpochInformation epochInfo = epochnInfos[0];

                #region
                //发生周跳的卫星剔除                
                //foreach (var sat in epochInfo)
                //{
                //    if (epochInfo.ReceiverTime.SecondsOfDay == 4470)
                //    {
                //        int kk = 0;
                //    }
                //    sat.HasCycleSlip = CycleSlipRemoveChainProcessor.Detect(sat);

                //    if (satepocount.Keys.Contains(sat.Prn))
                //    {
                //        satepocount[sat.Prn] = satepocount[sat.Prn] + 1;
                //    }
                //    else
                //    {
                //        satepocount[sat.Prn] = 1;
                //    } 
                //    if (sat.HasCycleSlip == true &&  satepocount[sat.Prn]> 18)
                //    {
                //        //cycleprns[epochInfo.ReceiverTime].Add(sat.Prn);

                //        if(!prns.Keys.Contains(sat.Prn))
                //        {
                //            prns.Add(sat.Prn, epochInfo.ReceiverTime);
                //        }
                //        if(!sats.Contains(sat.Prn))
                //        {
                //            sats.Add(sat.Prn);
                //        }                        
                //    }                                       
                //}

                ////&& (epochInfo.ReceiverTime.SecondsOfDay - key.Value.SecondsOfDay) < 3600 * 3
                //if (prns.Count > 0)
                //{
                //    List<SatelliteNumber> toberemoveprns = new List<SatelliteNumber>();//下一个历元如果没有观测到该卫星,prns就要剔除该卫星
                //    foreach(var key in prns)
                //    {                        
                //        if(epochInfo.TotalPrns.Contains(key.Key))
                //        {
                //            continue;
                //        }
                //        if (!epochInfo.TotalPrns.Contains(key.Key)) //观测数据分成明显的两段
                //        {
                //            toberemoveprns.Add(key.Key);
                //            satepocount[key.Key] = 1;                                                                                                               
                //        }                                           
                //    }
                //    foreach(var sat in toberemoveprns)
                //    {
                //        sats.Remove(sat);
                //    }
                //    epochInfo.RemoveByPrns(sats);   
                //} 
                #endregion
                cleanEpochInformation.Add(epochInfo);
            }
        }

        /// <summary>
        /// 计算星间单差FCBs
        /// </summary>
        public void CalculateSDWLFCBs()
        {
            foreach (var item in cleanEpochInformation)
            {
                if (item.Contains(refsatellite))
                {
                    Dictionary<SatelliteNumber, double> sdwl = new Dictionary<SatelliteNumber, double>();
                    Dictionary<SatelliteNumber, double> wl = new Dictionary<SatelliteNumber, double>();


                    foreach (var sat in item.EnabledPrns)
                    {
                        sdwl[sat] = item[sat].Combinations.MwRangeCombination.Value - item[refsatellite].Combinations.MwRangeCombination.Value;
                        wl[sat] = item[sat].Combinations.MwRangeCombination.Value;
                    }

                    SDWL.Add(item.ReceiverTime, sdwl);
                    WL.Add(item.ReceiverTime, wl);
                }
            }
        }

        /// <summary>
        /// 往后加载数据
        /// </summary>
        /// <returns></returns>
        private List<EpochInformation> LoadNextOne()
        {
            List<EpochInformation> epochObs = null;
            #region
            //if (dataSource is IBlockObservationDataSource)
            //{
            //    IBlockObservationDataSource ds = (IBlockObservationDataSource)dataSource;
            //    epochObs = new List<EpochObservation>();
            //    ds.GetItems(index, epochCount);
            //    index = index + epochCount;
            //}
            #endregion

            if (dataSource is ISingleSiteObsStream)
            {
                ISingleSiteObsStream ds = (ISingleSiteObsStream)dataSource;
                epochObs = ds.GetNexts(1);
            }

            // List<EpochInformation> epochnInfos = EpochInformation.Parse(epochObs, SatelliteTypes);
            return epochObs;
        }


        /// <summary>
        /// 查看SDWL和UPD
        /// </summary>
        /// <returns></returns>
        public List<DataTable> viewSDWL()
        {
            int i = 0;
            ////求UPD参数
            Dictionary<Time, double> sum = new Dictionary<Time, double>();
            Dictionary<SatelliteNumber, Dictionary<Time, double>> sum_order = new Dictionary<SatelliteNumber, Dictionary<Time, double>>();   //只保留某颗卫星一个弧段最后一个历元的观测时间及单差宽巷的和 
            Dictionary<SatelliteNumber, Dictionary<Time, double>> active_sdwl = new Dictionary<SatelliteNumber, Dictionary<Time, double>>();

            Dictionary<Time, int> num = new Dictionary<Time, int>();
            Dictionary<SatelliteNumber, Dictionary<Time, int>> num_order = new Dictionary<SatelliteNumber, Dictionary<Time, int>>(); //只保留某颗卫星一个弧段的历元总和  

            List<SatelliteNumber> totalprns = new List<SatelliteNumber>();
            totalprns = gettotalprns();
            foreach (var sat in totalprns)
            {
                Dictionary<Time, double> sat_time = new Dictionary<Time, double>();
                foreach (var item in SDWL)
                {
                    if (item.Value.Keys.Contains(sat))
                    {
                        sat_time.Add(item.Key, item.Value[sat]);
                    }
                }
                sat_swdl.Add(sat, sat_time);
            }

            clean();
            int order = 0;

            foreach (var item in sat_swdl)
            {
                int ii = 0, jj = 0, kk = 0, mm = 0, nn = 0, pp = 0, qq = 0, rr = 0;//暂时最多将数据分为8段
                List<Time> totalEpoch = new List<Time>(item.Value.Keys);
                for (i = 0; i < totalEpoch.Count; i++)
                {
                    sum[totalEpoch[i]] = 0;
                    num[totalEpoch[i]] = 0;
                }

                for (ii = 0; ii < totalEpoch.Count; ii++)
                {
                    if (ii == 0) //第一个历元进行初始化
                    {
                        sum[totalEpoch[ii]] = item.Value[totalEpoch[ii]];
                        num[totalEpoch[ii]] = 1;
                        continue;
                    }
                    if (getmax(new double[7] { jj, kk, mm, nn, pp, qq, rr }) == totalEpoch.Count - 1) break;

                    if (Math.Abs(item.Value[totalEpoch[ii]] - item.Value[totalEpoch[ii - 1]]) < 8 && (double)(totalEpoch[ii].SecondsOfDay - totalEpoch[ii - 1].SecondsOfDay) == Interval)
                    {
                        sum[totalEpoch[ii]] = sum[totalEpoch[ii - 1]] + item.Value[totalEpoch[ii]];
                        num[totalEpoch[ii]] = num[totalEpoch[ii - 1]] + 1;

                        if (ii == totalEpoch.Count - 1)
                        {
                            Dictionary<Time, double> tmp_sum = new Dictionary<Time, double>();
                            tmp_sum.Add(totalEpoch[ii], sum[totalEpoch[ii]]);
                            Dictionary<Time, int> tmp_num = new Dictionary<Time, int>();
                            tmp_num.Add(totalEpoch[ii], num[totalEpoch[ii]]);
                            sum_order.Add(item.Key, tmp_sum);
                            num_order.Add(item.Key, tmp_num);
                            break;
                        }
                    }
                    else
                    {
                        Dictionary<Time, double> tmp_sum = new Dictionary<Time, double>();
                        tmp_sum.Add(totalEpoch[ii - 1], sum[totalEpoch[ii - 1]]);
                        Dictionary<Time, int> tmp_num = new Dictionary<Time, int>();
                        tmp_num.Add(totalEpoch[ii - 1], num[totalEpoch[ii - 1]]);
                        order = order + 1; jj = ii + 1;
                        sum[totalEpoch[ii]] = item.Value[totalEpoch[ii]]; num[totalEpoch[ii]] = 1;
                        if (ii == totalEpoch.Count - 1)
                        {
                            jj = jj - 1;
                            tmp_sum.Add(totalEpoch[jj], sum[totalEpoch[jj]]);
                            tmp_num.Add(totalEpoch[jj], num[totalEpoch[jj]]);
                            sum_order.Add(item.Key, tmp_sum);
                            num_order.Add(item.Key, tmp_num);
                            break;

                        }
                        for (jj = ii + 1; jj < totalEpoch.Count; jj++)
                        {
                            if (getmax(new double[7] { ii, kk, mm, nn, pp, qq, rr }) == totalEpoch.Count - 1) break;
                            if (Math.Abs(item.Value[totalEpoch[jj]] - item.Value[totalEpoch[jj - 1]]) < 8 && (double)(totalEpoch[jj].SecondsOfDay - totalEpoch[jj - 1].SecondsOfDay) == Interval)//不知道这个阈值是否合适
                            {
                                sum[totalEpoch[jj]] = sum[totalEpoch[jj - 1]] + item.Value[totalEpoch[jj]];
                                num[totalEpoch[jj]] = num[totalEpoch[jj - 1]] + 1;
                                if (jj == totalEpoch.Count - 1)
                                {
                                    tmp_sum.Add(totalEpoch[jj], sum[totalEpoch[jj]]);
                                    tmp_num.Add(totalEpoch[jj], num[totalEpoch[jj]]);
                                    sum_order.Add(item.Key, tmp_sum);
                                    num_order.Add(item.Key, tmp_num);
                                    break;
                                }
                            }
                            else
                            {
                                tmp_sum.Add(totalEpoch[jj - 1], sum[totalEpoch[jj - 1]]);
                                tmp_num.Add(totalEpoch[jj - 1], num[totalEpoch[jj - 1]]);
                                order = order + 1; kk = jj + 1;
                                sum[totalEpoch[jj]] = item.Value[totalEpoch[jj]]; num[totalEpoch[jj]] = 1;
                                if (jj == totalEpoch.Count - 1)
                                {
                                    kk = kk - 1;
                                    tmp_sum.Add(totalEpoch[kk], sum[totalEpoch[kk]]);
                                    tmp_num.Add(totalEpoch[kk], num[totalEpoch[kk]]);
                                    sum_order.Add(item.Key, tmp_sum);
                                    num_order.Add(item.Key, tmp_num);
                                    break;
                                }
                                for (kk = jj + 1; kk < totalEpoch.Count; kk++)
                                {
                                    if (getmax(new double[7] { ii, jj, mm, nn, pp, qq, rr }) == totalEpoch.Count - 1) break;
                                    if (Math.Abs(item.Value[totalEpoch[kk]] - item.Value[totalEpoch[kk - 1]]) < 8 && (double)(totalEpoch[kk].SecondsOfDay - totalEpoch[kk - 1].SecondsOfDay) == Interval)//不知道这个阈值是否合适
                                    {
                                        sum[totalEpoch[kk]] = sum[totalEpoch[kk - 1]] + item.Value[totalEpoch[kk]];
                                        num[totalEpoch[kk]] = num[totalEpoch[kk - 1]] + 1;
                                        if (kk == totalEpoch.Count - 1)
                                        {
                                            tmp_sum.Add(totalEpoch[kk], sum[totalEpoch[kk]]);
                                            tmp_num.Add(totalEpoch[kk], num[totalEpoch[kk]]);
                                            sum_order.Add(item.Key, tmp_sum);
                                            num_order.Add(item.Key, tmp_num);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        //tmp_sum = new Dictionary<Time, double>();
                                        tmp_sum.Add(totalEpoch[kk - 1], sum[totalEpoch[kk - 1]]);
                                        //tmp_num = new Dictionary<Time, int>();
                                        tmp_num.Add(totalEpoch[kk - 1], num[totalEpoch[kk - 1]]);
                                        //active_sdwl[key.Key].Add(totalEpoch[kk], key.Value[totalEpoch[kk]]); 
                                        order = order + 1; mm = kk + 1;
                                        sum[totalEpoch[kk]] = item.Value[totalEpoch[kk]]; num[totalEpoch[kk]] = 1;
                                        if (kk == totalEpoch.Count - 1)
                                        {
                                            mm = mm - 1;
                                            tmp_sum.Add(totalEpoch[mm], sum[totalEpoch[mm]]);
                                            tmp_num.Add(totalEpoch[mm], num[totalEpoch[mm]]);
                                            sum_order.Add(item.Key, tmp_sum);
                                            num_order.Add(item.Key, tmp_num);
                                            break;

                                        }
                                        for (mm = kk + 1; mm < totalEpoch.Count; mm++)
                                        {
                                            if (getmax(new double[7] { ii, jj, kk, nn, pp, qq, rr }) == totalEpoch.Count - 1) break;
                                            if (Math.Abs(item.Value[totalEpoch[mm]] - item.Value[totalEpoch[mm - 1]]) < 8 && (double)(totalEpoch[mm].SecondsOfDay - totalEpoch[mm - 1].SecondsOfDay) == Interval)//不知道这个阈值是否合适
                                            {
                                                sum[totalEpoch[mm]] = sum[totalEpoch[mm - 1]] + item.Value[totalEpoch[mm]];
                                                num[totalEpoch[mm]] = num[totalEpoch[mm - 1]] + 1;
                                                if (mm == totalEpoch.Count - 1)
                                                {
                                                    tmp_sum.Add(totalEpoch[mm], sum[totalEpoch[mm]]);
                                                    tmp_num.Add(totalEpoch[mm], num[totalEpoch[mm]]);
                                                    sum_order.Add(item.Key, tmp_sum);
                                                    num_order.Add(item.Key, tmp_num);
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                //tmp_sum = new Dictionary<Time, double>();
                                                tmp_sum.Add(totalEpoch[mm - 1], sum[totalEpoch[mm - 1]]);
                                                //tmp_num = new Dictionary<Time, int>();
                                                tmp_num.Add(totalEpoch[mm - 1], num[totalEpoch[mm - 1]]);
                                                order = order + 1; nn = mm + 1;
                                                sum[totalEpoch[mm]] = item.Value[totalEpoch[mm]]; num[totalEpoch[mm]] = 1;
                                                if (mm == totalEpoch.Count - 1)
                                                {
                                                    nn = nn - 1;
                                                    tmp_sum.Add(totalEpoch[nn], sum[totalEpoch[nn]]);
                                                    tmp_num.Add(totalEpoch[nn], num[totalEpoch[nn]]);
                                                    sum_order.Add(item.Key, tmp_sum);
                                                    num_order.Add(item.Key, tmp_num);
                                                    break;
                                                }
                                                for (nn = mm + 1; nn < totalEpoch.Count; nn++)
                                                {
                                                    if (getmax(new double[7] { ii, jj, kk, mm, pp, qq, rr }) == totalEpoch.Count - 1) break;
                                                    if (Math.Abs(item.Value[totalEpoch[nn]] - item.Value[totalEpoch[nn - 1]]) < 8 && (double)(totalEpoch[nn].SecondsOfDay - totalEpoch[nn - 1].SecondsOfDay) == Interval)//不知道这个阈值是否合适
                                                    {
                                                        sum[totalEpoch[nn]] = sum[totalEpoch[nn - 1]] + item.Value[totalEpoch[nn]];
                                                        num[totalEpoch[nn]] = num[totalEpoch[nn - 1]] + 1;
                                                        if (nn == totalEpoch.Count - 1)
                                                        {
                                                            tmp_sum.Add(totalEpoch[nn], sum[totalEpoch[nn]]);
                                                            tmp_num.Add(totalEpoch[nn], num[totalEpoch[nn]]);
                                                            sum_order.Add(item.Key, tmp_sum);
                                                            num_order.Add(item.Key, tmp_num);
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //tmp_sum = new Dictionary<Time, double>();
                                                        tmp_sum.Add(totalEpoch[nn - 1], sum[totalEpoch[nn - 1]]);
                                                        //tmp_num = new Dictionary<Time, int>();
                                                        tmp_num.Add(totalEpoch[nn - 1], num[totalEpoch[nn - 1]]);
                                                        order = order + 1; pp = nn + 1;
                                                        sum[totalEpoch[nn]] = item.Value[totalEpoch[nn]]; num[totalEpoch[nn]] = 1;
                                                        if (nn == totalEpoch.Count - 1)
                                                        {
                                                            pp = pp - 1;
                                                            tmp_sum.Add(totalEpoch[pp], sum[totalEpoch[pp]]);
                                                            tmp_num.Add(totalEpoch[pp], num[totalEpoch[pp]]);
                                                            sum_order.Add(item.Key, tmp_sum);
                                                            num_order.Add(item.Key, tmp_num);
                                                            break;
                                                        }
                                                        for (pp = nn + 1; pp < totalEpoch.Count; pp++)
                                                        {
                                                            if (getmax(new double[7] { ii, jj, kk, mm, nn, qq, rr }) == totalEpoch.Count - 1) break;
                                                            if (Math.Abs(item.Value[totalEpoch[pp]] - item.Value[totalEpoch[pp - 1]]) < 8 && (double)(totalEpoch[pp].SecondsOfDay - totalEpoch[pp - 1].SecondsOfDay) == Interval)//不知道这个阈值是否合适
                                                            {
                                                                sum[totalEpoch[pp]] = sum[totalEpoch[pp - 1]] + item.Value[totalEpoch[pp]];
                                                                num[totalEpoch[pp]] = num[totalEpoch[pp - 1]] + 1;

                                                                if (pp == totalEpoch.Count - 1)
                                                                {
                                                                    tmp_sum.Add(totalEpoch[pp], sum[totalEpoch[pp]]);
                                                                    tmp_num.Add(totalEpoch[pp], num[totalEpoch[pp]]);
                                                                    sum_order.Add(item.Key, tmp_sum);
                                                                    num_order.Add(item.Key, tmp_num);
                                                                    break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //tmp_sum = new Dictionary<Time, double>();
                                                                tmp_sum.Add(totalEpoch[pp - 1], sum[totalEpoch[pp - 1]]);
                                                                //tmp_num = new Dictionary<Time, int>();
                                                                tmp_num.Add(totalEpoch[pp - 1], num[totalEpoch[pp - 1]]);
                                                                order = order + 1; qq = pp + 1;
                                                                sum[totalEpoch[pp]] = item.Value[totalEpoch[pp]]; num[totalEpoch[pp]] = 1;
                                                                if (pp == totalEpoch.Count - 1)
                                                                {
                                                                    qq = qq - 1;
                                                                    tmp_sum.Add(totalEpoch[qq], sum[totalEpoch[qq]]);
                                                                    tmp_num.Add(totalEpoch[qq], num[totalEpoch[qq]]);
                                                                    sum_order.Add(item.Key, tmp_sum);
                                                                    num_order.Add(item.Key, tmp_num);
                                                                    break;
                                                                }
                                                                for (qq = pp + 1; qq < totalEpoch.Count; qq++)
                                                                {
                                                                    if (getmax(new double[7] { ii, jj, kk, mm, nn, pp, rr }) == totalEpoch.Count - 1) break;
                                                                    if (Math.Abs(item.Value[totalEpoch[qq]] - item.Value[totalEpoch[qq - 1]]) < 8 && (double)(totalEpoch[qq].SecondsOfDay - totalEpoch[qq - 1].SecondsOfDay) == Interval)//不知道这个阈值是否合适
                                                                    {
                                                                        sum[totalEpoch[qq]] = sum[totalEpoch[qq - 1]] + item.Value[totalEpoch[qq]];
                                                                        num[totalEpoch[qq]] = num[totalEpoch[qq - 1]] + 1;

                                                                        if (qq == totalEpoch.Count - 1)
                                                                        {
                                                                            tmp_sum.Add(totalEpoch[qq], sum[totalEpoch[qq]]);
                                                                            tmp_num.Add(totalEpoch[qq], num[totalEpoch[qq]]);
                                                                            sum_order.Add(item.Key, tmp_sum);
                                                                            num_order.Add(item.Key, tmp_num);
                                                                            break;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        //tmp_sum = new Dictionary<Time, double>();
                                                                        tmp_sum.Add(totalEpoch[qq - 1], sum[totalEpoch[qq - 1]]);
                                                                        //tmp_num = new Dictionary<Time, int>();
                                                                        tmp_num.Add(totalEpoch[qq - 1], num[totalEpoch[qq - 1]]);
                                                                        order = order + 1; rr = qq + 1;
                                                                        sum[totalEpoch[qq]] = item.Value[totalEpoch[qq]]; num[totalEpoch[qq]] = 1;
                                                                        if (qq == totalEpoch.Count - 1)
                                                                        {
                                                                            rr = rr - 1;
                                                                            tmp_sum.Add(totalEpoch[rr], sum[totalEpoch[rr]]);
                                                                            tmp_num.Add(totalEpoch[rr], num[totalEpoch[rr]]);
                                                                            sum_order.Add(item.Key, tmp_sum);
                                                                            num_order.Add(item.Key, tmp_num);
                                                                            break;
                                                                        }
                                                                        for (rr = qq + 1; rr < totalEpoch.Count; rr++)
                                                                        {
                                                                            if (getmax(new double[7] { ii, jj, kk, mm, nn, pp, qq }) == totalEpoch.Count - 1) break;
                                                                            if (Math.Abs(item.Value[totalEpoch[rr]] - item.Value[totalEpoch[rr - 1]]) < 8 && (double)(totalEpoch[rr].SecondsOfDay - totalEpoch[rr - 1].SecondsOfDay) == Interval)//不知道这个阈值是否合适
                                                                            {
                                                                                sum[totalEpoch[rr]] = sum[totalEpoch[rr - 1]] + item.Value[totalEpoch[rr]];
                                                                                num[totalEpoch[rr]] = num[totalEpoch[rr - 1]] + 1;

                                                                                if (rr == totalEpoch.Count - 1)
                                                                                {
                                                                                    tmp_sum.Add(totalEpoch[rr], sum[totalEpoch[rr]]);
                                                                                    tmp_num.Add(totalEpoch[rr], num[totalEpoch[rr]]);
                                                                                    sum_order.Add(item.Key, tmp_sum);
                                                                                    num_order.Add(item.Key, tmp_num);
                                                                                    break;
                                                                                }
                                                                            }
                                                                            else
                                                                            {

                                                                            }
                                                                        }

                                                                    }
                                                                }

                                                            }
                                                        }

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //可能原因是某个弧段最后发生周跳，例如DGAR站03:37:00时刻G02卫星就发生了周跳，将其后的数据删除
            //为了求均方根误差，将发生了周跳的sat_sdwl的数据也删除，这样就可以利用sat_sdwl求均方根误差了            
            foreach (var item in sum_order)
            {
                Time startgpstime;
                List<Time> gpstime = new List<Time>(item.Value.Keys);
                for (i = 0; i < gpstime.Count; i++)
                {
                    if (num_order[item.Key][gpstime[i]] < 60)//通过分析发现，周跳一般发生在观测结束历元，且维持时间较短
                    {
                        //开始删除了，三个变量
                        item.Value.Remove(gpstime[i]);
                        num_order[item.Key].Remove(gpstime[i]);
                        if (i == 0)
                        {
                            startgpstime = sat_swdl[refsatellite].First().Key;
                        }
                        else
                        {
                            startgpstime = gpstime[i - 1];
                        }
                        List<Time> time = new List<Time>(sat_swdl[item.Key].Keys);
                        for (int j = 0; j < time.Count; j++)
                        {
                            if (time[j].SecondsOfDay > startgpstime.SecondsOfDay && time[j].SecondsOfDay <= gpstime[i].SecondsOfDay)
                            {
                                sat_swdl[item.Key].Remove(time[j]);
                            }
                        }
                    }
                }
            }


            SortedDictionary<SatelliteNumber, Dictionary<Time, double>> UPD_order = new SortedDictionary<SatelliteNumber, Dictionary<Time, double>>();

            foreach (var item in sum_order)
            {
                Dictionary<Time, double> tmp_UPD = new Dictionary<Time, double>();
                foreach (var gpstime in item.Value.Keys)
                {
                    tmp_UPD[gpstime] = item.Value[gpstime] / num_order[item.Key][gpstime];
                }
                UPD_order.Add(item.Key, tmp_UPD);
            }
            getSDWLFCB(UPD_order);

            //求均方根误差
            //calsigma();

            //输出星间单差UPD
            #region
            List<string> stringUPDSat = new List<string>();
            stringUPDSat.Add("Epoch");
            stringUPDSat.Add("UPD");

            string[][] stringUPD = new string[32][];
            for (i = 0; i < 32; i++)
            {
                stringUPD[i] = new string[2];
            }


            List<SatelliteNumber> totalsats = new List<SatelliteNumber>();
            for (i = 1; i <= 32; i++)
            {
                totalsats.Add(SatelliteNumber.Parse(i.ToString()));
            }
            i = 0;
            foreach (var sat in totalsats)
            {
                stringUPD[i][0] = sat.ToString();
                if (sat.Equals(refsatellite))
                {
                    stringUPD[i][1] = "参考星";
                    i += 1;
                    continue;
                }

                if (SDWLFCB.Keys.Contains(sat))
                {
                    stringUPD[i][1] = SDWLFCB[sat].ToString();
                }
                else
                {
                    stringUPD[i][1] = 0.ToString();
                }
                i += 1;
            }

            DataTable table_UPD = Geo.Utils.DataTableUtil.Create("UPD", stringUPDSat.ToArray(), stringUPD);
            List<DataTable> table = new List<DataTable>();
            #endregion

            #region 输出星间单差宽巷值
            List<SatelliteNumber> sats = new List<SatelliteNumber>(sat_swdl.Keys);
            List<Time> totaltime = new List<Time>(sat_swdl[refsatellite].Keys);
            sats.Sort();
            //int row = SDWL.Count;

            int row = sat_swdl[refsatellite].Keys.Count;
            int col = sats.Count;
            string[][] cells = new string[row][];
            i = 0;
            foreach (var time in totaltime)
            {
                cells[i] = new string[col + 1];
                cells[i][0] = time.ToString(); ;
                int j = 1;
                foreach (var sat in sats)
                {
                    if (sat_swdl[sat].Keys.Contains(time))
                    {
                        cells[i][j] = sat_swdl[sat][time].ToString();
                    }
                    else
                    {
                        cells[i][j] = 0.ToString();
                    }
                    j += 1;
                }
                i += 1;
            }

            List<string> stringSats = new List<string>();
            stringSats.Add("Epoch");
            foreach (var sat in sats)
            {
                stringSats.Add(sat.ToString());
            }
            DataTable table_SDWL = Geo.Utils.DataTableUtil.Create("SDWL", stringSats.ToArray(), cells);
            #endregion
            table.Add(table_SDWL);
            table.Add(table_UPD);
            return table;
        }

        /// <summary>
        /// 获取每颗卫星的观测历元总数
        /// </summary>
        /// <param name="sat_swdl"></param>
        /// <returns></returns>
        public List<Time> getEpochNumber(Dictionary<Time, double> s_swdl)
        {
            return new List<Time>(s_swdl.Keys);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private double getmax(double[] a)
        {
            double max = 0;
            max = Math.Max(a[0], a[1]);
            if (a.Length > 2)
            {
                for (int i = 2; i < a.Length; i++)
                {
                    max = Math.Max(max, a[i]);
                }
            }
            return max;
        }

        /// <summary>
        /// 返回一个数组的最大值及其序号
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private double[] getmax_order(double[] a)
        {
            double[] order_max = new double[2];
            double max = 0;
            if (a[0] >= a[1])
            {
                order_max[0] = 0;
                order_max[1] = a[0];
            }
            else
            {
                order_max[0] = 1;
                order_max[1] = a[1];
            }
            max = Math.Max(a[0], a[1]);
            if (a.Length > 2)
            {
                for (int i = 2; i < a.Length; i++)
                {
                    if (max >= a[i])
                    {
                        continue;
                    }
                    else
                    {
                        order_max[0] = i;
                        order_max[1] = a[i];
                    }
                }
            }
            return order_max;
        }

        /// <summary>
        /// 返回一个数组的最小值及其序号
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private double[] getmin_order(double[] a)
        {
            double[] order_min = new double[2];
            double min = 0;
            if (a[0] <= a[1])
            {
                order_min[0] = 0;
                order_min[1] = a[0];
            }
            else
            {
                order_min[0] = 1;
                order_min[1] = a[1];
            }
            min = Math.Min(a[0], a[1]);
            if (a.Length > 2)
            {
                for (int i = 2; i < a.Length; i++)
                {
                    if (min <= a[i])
                    {
                        continue;
                    }
                    else
                    {
                        order_min[0] = i;
                        order_min[1] = a[i];
                    }
                }
            }
            return order_min;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns> 
        private double getmin(double[] a)
        {
            double min = 0;
            min = Math.Min(a[0], a[1]);
            if (a.Length > 2)
            {
                for (int i = 2; i < a.Length; i++)
                {
                    min = Math.Min(min, a[i]);
                }
            }
            return min;
        }

        public List<SatelliteNumber> gettotalprns()
        {
            List<SatelliteNumber> totalprns = new List<SatelliteNumber>();
            totalprns.Add(refsatellite);
            foreach (var item in SDWL)
            {
                foreach (var sat in item.Value.Keys)
                {
                    if (!totalprns.Contains(sat))
                        totalprns.Add(sat);
                }
            }
            return totalprns;
        }

        /// <summary>
        /// 清除不连续的观测值，小于30min的观测弧度删除
        /// </summary>
        public void clean()
        {
            int step = 60;//小于30min的观测弧度删除
            foreach (var item in sat_swdl)//卫星编号
            {
                List<Time> tmp_sat = new List<Time>();
                int num = 0;
                List<Time> time = new List<Time>(item.Value.Keys);

                for (int j = 1; j < time.Count; j++)
                {
                    if ((time[j].SecondsOfDay - time[j - 1].SecondsOfDay) == Interval)
                    {
                        num += 1;
                        tmp_sat.Add(time[j - 1]);
                    }
                    else
                    {
                        tmp_sat.Add(time[j - 1]);
                        num += 1;
                        if (num < step)
                        {
                            for (int k = 0; k < num; k++)
                            {
                                sat_swdl[item.Key].Remove(tmp_sat[k]);
                            }
                        }
                        num = 0;
                        tmp_sat = new List<Time>();
                    }
                    if (j == time.Count - 1)//结尾部分
                    {
                        tmp_sat.Add(time[j]);
                        num = num + 1;
                        if (num < step)
                        {
                            for (int m = 0; m < num; m++)
                            {
                                sat_swdl[item.Key].Remove(tmp_sat[m]);
                            }
                        }
                    }
                }
            }
            //有的卫星删除观测值之后不剩观测值了
            List<SatelliteNumber> sats = new List<SatelliteNumber>(sat_swdl.Keys);
            foreach (var sat in sats)
            {
                if (sat_swdl[sat].Values.Count == 0)
                {
                    sat_swdl.Remove(sat);
                }
            }
        }

        public void getSDWLFCB(SortedDictionary<SatelliteNumber, Dictionary<Time, double>> UPD_order)
        {
            Dictionary<SatelliteNumber, double> tmp_SDWLFCB = new Dictionary<SatelliteNumber, double>();
            foreach (var item in UPD_order)
            {
                List<Time> gpstime = new List<Time>(item.Value.Keys);
                Dictionary<Time, double> UPD_Floor = new Dictionary<Time, double>();
                Dictionary<Time, double> tmp_UPD = new Dictionary<Time, double>();
                List<double> tmp = new List<double>();
                switch (gpstime.Count)
                {
                    case 1:
                        tmp_SDWLFCB.Add(item.Key, item.Value[gpstime[0]] - Math.Round(item.Value[gpstime[0]]));
                        break;

                    case 2:
                        tmp_UPD[gpstime[0]] = item.Value[gpstime[0]] - Math.Floor(item.Value[gpstime[0]]); //0
                        tmp_UPD[gpstime[1]] = item.Value[gpstime[1]] - Math.Floor(item.Value[gpstime[1]]);//1
                        if (tmp_UPD[gpstime[0]] > 0.5 && tmp_UPD[gpstime[1]] > 0.5)//两大
                        {
                            tmp_UPD[gpstime[0]] = tmp_UPD[gpstime[0]] - 1;
                            tmp_UPD[gpstime[1]] = tmp_UPD[gpstime[1]] - 1;
                        }
                        tmp = new List<double>(tmp_UPD.Values);
                        tmp.Sort();
                        if ((tmp[1] - tmp[0]) > 0.4)
                        {
                            tmp[1] = tmp[1] - 1;
                        }
                        tmp_SDWLFCB.Add(item.Key, (tmp[0] + tmp[1]) / 2);
                        break;

                    case 3:
                        tmp_UPD[gpstime[0]] = item.Value[gpstime[0]] - Math.Floor(item.Value[gpstime[0]]); //0
                        tmp_UPD[gpstime[1]] = item.Value[gpstime[1]] - Math.Floor(item.Value[gpstime[1]]);//1
                        tmp_UPD[gpstime[2]] = item.Value[gpstime[2]] - Math.Floor(item.Value[gpstime[2]]); //2
                        if (tmp_UPD[gpstime[0]] > 0.5 && tmp_UPD[gpstime[1]] > 0.5 && tmp_UPD[gpstime[2]] > 0.5)//三大
                        {
                            tmp_UPD[gpstime[0]] = tmp_UPD[gpstime[0]] - 1;
                            tmp_UPD[gpstime[1]] = tmp_UPD[gpstime[1]] - 1;
                            tmp_UPD[gpstime[2]] = tmp_UPD[gpstime[2]] - 1;
                        }
                        tmp = new List<double>(tmp_UPD.Values);
                        tmp.Sort();
                        if ((tmp[2] - tmp[0]) > 0.4 && (tmp[1] - tmp[0]) > 0.4) //一小两大
                        {
                            tmp[2] = tmp[2] - 1;
                            tmp[1] = tmp[1] - 1;
                        }
                        if ((tmp[2] - tmp[0]) > 0.4 && (tmp[2] - tmp[1]) > 0.4)//两小一大
                        {
                            tmp[2] = tmp[2] - 1;
                        }
                        tmp_SDWLFCB.Add(item.Key, (tmp[0] + tmp[1] + tmp[2]) / 3);
                        break;

                    case 4:
                        tmp_UPD[gpstime[0]] = item.Value[gpstime[0]] - Math.Floor(item.Value[gpstime[0]]); //0
                        tmp_UPD[gpstime[1]] = item.Value[gpstime[1]] - Math.Floor(item.Value[gpstime[1]]);//1
                        tmp_UPD[gpstime[2]] = item.Value[gpstime[2]] - Math.Floor(item.Value[gpstime[2]]); //2
                        tmp_UPD[gpstime[3]] = item.Value[gpstime[3]] - Math.Floor(item.Value[gpstime[3]]);//3
                        if (tmp_UPD[gpstime[0]] > 0.5 && tmp_UPD[gpstime[1]] > 0.5 && tmp_UPD[gpstime[2]] > 0.5 && tmp_UPD[gpstime[3]] > 0.5)
                        {
                            tmp_UPD[gpstime[0]] = tmp_UPD[gpstime[0]] - 1;
                            tmp_UPD[gpstime[1]] = tmp_UPD[gpstime[1]] - 1;
                            tmp_UPD[gpstime[2]] = tmp_UPD[gpstime[2]] - 1;
                            tmp_UPD[gpstime[2]] = tmp_UPD[gpstime[3]] - 1;
                        }
                        tmp = new List<double>(tmp_UPD.Values);
                        tmp.Sort();
                        if ((tmp[2] - tmp[0]) > 0.4 && (tmp[1] - tmp[0]) > 0.4 && (tmp[3] - tmp[0]) > 0.4) //一小三大
                        {
                            tmp[3] = tmp[3] - 1;
                            tmp[2] = tmp[2] - 1;
                            tmp[1] = tmp[1] - 1;
                        }
                        if ((tmp[2] - tmp[0]) > 0.4 && (tmp[2] - tmp[1]) > 0.4 && (tmp[3] - tmp[0]) > 0.4 && (tmp[3] - tmp[1]) > 0.4)//两小两大
                        {
                            tmp[2] = tmp[2] - 1;
                            tmp[3] = tmp[3] - 1;
                        }
                        if ((tmp[3] - tmp[0]) > 0.4 && (tmp[3] - tmp[1]) > 0.4 && (tmp[3] - tmp[2]) > 0.4)//三小一大
                        {
                            tmp[3] = tmp[3] - 1;
                        }
                        tmp_SDWLFCB.Add(item.Key, (tmp[0] + tmp[1] + tmp[2] + tmp[3]) / 4);
                        break;
                }
            }
            foreach (var item in tmp_SDWLFCB)
            {
                if (item.Value > 0.5)
                {
                    SDWLFCB[item.Key] = item.Value - 1;
                }
                else
                {
                    SDWLFCB[item.Key] = item.Value;
                }
            }
        }

        /// <summary>
        /// 余误差函数
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double erfc(double x)
        {
            double result;
            double a1 = 0.0705230784;
            double a2 = 0.0422820123;
            double a3 = 0.0092705272;
            double a4 = 0.0001520143;
            double a5 = 0.0002765672;
            double a6 = 0.0000430638;
            result = 1 / (Math.Pow(1 + a1 * x + a2 * Math.Pow(x, 2) + a3 * Math.Pow(x, 3) + a4 * Math.Pow(x, 4) + a5 * Math.Pow(x, 5) + a6 * Math.Pow(x, 6), 16));
            return result;
        }

        public void calsigma()
        {
            foreach (var item in sat_swdl)
            {
                List<Time> time1 = new List<Time>(item.Value.Keys);
                double tmp = 0;
                for (int i = 0; i < time1.Count; i++)
                {
                    tmp += Math.Pow(item.Value[time1[i]] - Math.Round(item.Value[time1[i]]) - SDWLFCB[item.Key], 2);
                }
                sat_sigma[item.Key] = Math.Sqrt(tmp / time1.Count);
            }

        }
        /// <summary>
        /// 求解宽巷固定率
        /// </summary>
        public void fixrate()
        {
            foreach (var item in sat_swdl)
            {
                List<Time> time1 = new List<Time>(item.Value.Keys);

                for (int i = 1; i < 10000; i++)
                {

                }
            }
        }
    }
}
