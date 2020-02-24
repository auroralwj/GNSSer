//2015.11.02, cy, 统计两个同步观测文件的观测值数量
using System;
using System.Collections.Generic;
using Gnsser.Domain;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Filter;
using Gnsser.Checkers;
using Geo.Common;

namespace Gnsser.Service
{
   
    /// <summary>
    ///  统计两个测站同步观测时段内的共视卫星数目
    /// </summary>
    public class EnableObsCount //: IonoFreePpp
    {
        #region 构造函数
        /// <summary>
        /// 统计两个测站同步观测时段内的共视卫星数
        /// </summary>
        /// <param name="referObsDataSource">观测站数据源</param>
        /// <param name="rovpath">流动站测站信息</param>

        public EnableObsCount(RinexFileObsDataSource ObsDataSourceA, RinexFileObsDataSource ObsDataSourceB)
        {
            
            this.ObsDataSourceA = ObsDataSourceA;
            this.ObsDataSourceB = ObsDataSourceB;
            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="lines">文件路径</param>
        public EnableObsCount(string[] lines)
        {
            //
            for (int i = 0; i < lines.Length; i++)
            {
                string itemA = lines[i];
                RinexFileObsDataSource fileA = new RinexFileObsDataSource(itemA);

            }
        }


        #endregion


        #region 参考站属性
        /// <summary>
        /// 测站数据源
        /// </summary>
        RinexFileObsDataSource ObsDataSourceA { get; set; }
        /// <summary>
        /// 测站数据源
        /// </summary>
        RinexFileObsDataSource ObsDataSourceB { get; set; }



        #endregion

        #region 核心计算方法

       

        private Dictionary<Geo.Times.Time, List<SatelliteNumber>> DataA = new Dictionary<Geo.Times.Time, List<SatelliteNumber>>();


        private Dictionary<Geo.Times.Time, List<SatelliteNumber>> DataB = new Dictionary<Geo.Times.Time, List<SatelliteNumber>>();


        /// <summary>
        /// 逐历元计算共视卫星的数目
        /// 思路：找到同步历元，累加共视卫星数目
        /// </summary>
        /// <param name="obsSection"></param>
        /// <returns></returns>
        public int Process(double toleranceSeccond = 1e-15)
        {
            int count = 0;


            #region 初级判断共视卫星数目


            ObsDataSourceA.Reset();
            ObsDataSourceB.Reset();

            //if (ObsDataSourceA.Current == null)
            //    ObsDataSourceA.MoveNext();

            while (ObsDataSourceA.MoveNext() && ObsDataSourceB.MoveNext())
            {
                EpochInformation obsSectionA = ObsDataSourceA.Current;
                EpochInformation obsSectionB = ObsDataSourceB.Current;
                double diff = Math.Abs(obsSectionA.ReceiverTime - obsSectionB.ReceiverTime);
                if (diff <= toleranceSeccond)
                {
                    //DataA.Add(obsSectionA.ReceiverTime, obsSectionA.TotalPrns);
                    //DataB.Add(obsSectionB.ReceiverTime, obsSectionB.TotalPrns);


                    foreach (var item in obsSectionA)
                    {
                        if (obsSectionB.Contains(item.Prn))
                        { count += 1; }
                    }

                }
                else
                {
                    if (obsSectionB.ReceiverTime < obsSectionA.ReceiverTime && diff > toleranceSeccond)
                    {
                        while (ObsDataSourceB.MoveNext())
                        {
                            obsSectionB = ObsDataSourceB.Current;
                            diff = Math.Abs(obsSectionA.ReceiverTime - obsSectionB.ReceiverTime);

                            if (diff <= toleranceSeccond)
                            {
                                //DataA.Add(obsSectionA.ReceiverTime, obsSectionA.TotalPrns);
                                //DataB.Add(obsSectionB.ReceiverTime, obsSectionB.TotalPrns);
                                
                                foreach (var item in obsSectionA)
                                {
                                    if (obsSectionB.Contains(item.Prn))
                                    { count += 1; }
                                }

                                break;
                            }
                        }
                    }
                    else if (obsSectionB.ReceiverTime > obsSectionA.ReceiverTime && diff > toleranceSeccond)
                    {
                        while (ObsDataSourceA.MoveNext())
                        {
                            obsSectionA = ObsDataSourceA.Current;
                            diff = Math.Abs(obsSectionA.ReceiverTime - obsSectionB.ReceiverTime);

                            if (diff < toleranceSeccond)
                            {
                                //DataA.Add(obsSectionA.ReceiverTime, obsSectionA.TotalPrns);
                                //DataB.Add(obsSectionB.ReceiverTime, obsSectionB.TotalPrns);

                                foreach (var item in obsSectionA)
                                {
                                    if (obsSectionB.Contains(item.Prn))
                                    { count += 1; }
                                }

                                break;
                            }
                        }
                    }
                }

            }
            
            





       
            #endregion

            #region 通过各类检核判断共视卫星数目
            //CurrentRefEpochInfo = refEpochInfoBuilder.Build(refObsSection);
            //if (CurrentRefEpochInfo == null || CurrentRefEpochInfo.EnabledSatCount <= 1) return 0;

            ////历元算前准备 
            //EpochInformation rovEpochInfo = rovEpochInfoBuilder.Build(obsSection);


            //if (rovEpochInfo == null || rovEpochInfo.EnabledSatCount <= 1) return 0;

            ////观测同步,让观测的卫星同步，卫星顺序也必须一致
            //new EpochInfoSynchronous(CurrentRefEpochInfo).Revise(ref rovEpochInfo);

            //if (rovEpochInfo.EnabledPrns.Count < 1)
            //{
            //    return 0;
            //}
            //count = rovEpochInfo.EnabledPrns.Count;
            #endregion

            return count;

        }


       




        #endregion


    }
}
