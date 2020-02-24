//2015.10.15, czs, create in 西安五路口袁记肉夹馍店, 观测信息连续性探测
//2015.10.16, czs, edit in 达州火车站，调试，调整

using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Utils;
using Geo.Common;
using Gnsser.Checkers;
using Gnsser.Filter;
using Geo.Times;


namespace Gnsser
{
    

    /// <summary>
    /// 卫星连续性分析器。
    /// </summary>
    public class SatConsecutionAnalyst : SatAnalyst
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="MinSequenceCount">历元数量</param>
        /// <param name="MaxAllowedGap">历元最大间隔数量</param>
        /// <param name="interval">采样间隔</param>
        public SatConsecutionAnalyst(int MinSequenceCount, int MaxAllowedGap = 0, double interval = 30.0)
            :base(interval)
        { 
            this.MinSequenceCount = MinSequenceCount;
            this.MaxAllowedGap = MaxAllowedGap;
            this.SatCounters = new Dictionary<SatelliteNumber, SatMarker>();
            this.EpochInfoBuffer = new EpochInfoBuffer(MinSequenceCount);
            this.EpochInfoBuffer.Dequeuing += EpochInfoBuffer_Dequeuing; 
        }


        /// <summary>
        /// 最小历元数量，连续数量少于此数量则表示为不连续。
       /// </summary>
       public int MinSequenceCount { get; set; }
        /// <summary>
        /// 允许最大的间隔(含)。即缺少了 GapCount 个历元仍然认为是连续的。超过了，则认为是断开的。
        /// </summary>
       public int MaxAllowedGap { get; set; }
        /// <summary>
        /// 卫星历元记录器
        /// </summary>
       Dictionary<SatelliteNumber, SatMarker> SatCounters { get; set; }
       int epochCount = 0;

       EpochInfoBuffer EpochInfoBuffer { get; set; }


        void EpochInfoBuffer_Dequeuing(List<EpochInformation> queue)
        {
            //throw new NotImplementedException();
        } 

        /// <summary>
        /// 处理过程
        /// </summary>
        /// <param name="obs">观测数据</param>
        /// <returns></returns>
        public override bool Revise(ref EpochInformation obs)
       {
           epochCount++;
           //入队后再说
           EpochInfoBuffer.Enqueue(obs); 

           if (obs == null)
           { return false; }
           var prns = obs.EnabledPrns;
           //首先，初始化，标记第一次出现的卫星为连续。
           foreach (var prn in prns)
           {
               if (!SatCounters.ContainsKey(prn))
               {
                   var marker = new SatMarker(prn, obs.ReceiverTime);
                   SatCounters[prn] = marker;
               }
           }

           //其次，标记，遍历所有记录的卫星，检查其是否连续，并进行标记，这一步只是简单的记录。
           foreach (var prn in SatCounters.Keys)
           {
               var maker = SatCounters[prn];
               //如果本历元有，则标记之为有，连续数增加，断开数量清零
               //可能：首次出现；继续出现
               if (prns.Contains(prn))
               {
                   if (maker.GapCount > 0)//首次出现,之前为断开
                   {
                       //记录此断开时段
                       SatSequentialPeriod.AddTimePeriod(prn, new BufferedTimePeriod(maker.StartRecordTime, obs.ReceiverTime));
                       maker.StartRecordTime = obs.ReceiverTime;
                   }
                   if (maker.SequenceCount == 0)
                   {
                       maker.StartRecordTime = obs.ReceiverTime;
                   }

                   maker.SequenceCount++;
                   maker.GapCount = 0;
                   maker.Mark = true;
               }
               else//如果本历元无，则标记为没有，断开数增加
               {
                   //可能：首次出现，继续出现
                  // maker.SequenceCount = 0;

                   if (maker.SequenceCount < MinSequenceCount)//首次断开,之前为连续,且连续数量太少，认为可以移除
                   {
                       //记录此断开时段
                       SatSequentialPeriod.AddTimePeriod(prn, new BufferedTimePeriod(maker.StartRecordTime, obs.ReceiverTime));
                        maker.StartBreakingTime = obs.ReceiverTime;//.Time.Value;
                   }

                   if (maker.GapCount == 0)
                   {
                        maker.StartBreakingTime = obs.ReceiverTime;
                   }
                   maker.SequenceCount = 0;
                   maker.GapCount++;
                   maker.Mark = false;
               }
           }



           ////记录本历元没有标记的，即断开的，要求断开数量大于指定最大
           //foreach (var maker in SatCounters.Values)
           //{
           //    if (!maker.Mark &&( maker.SequenceCount <= MinSequenceCount || maker.GapCount >= this.MaxAllowedGap ))
           //    {
           //        SatPeriodInfoManager.AddTimePeriod(maker.Prn, new BufferedTimePeriod(maker.StartRecordTime, obs.Time.Value));
           //    }
           //    maker.SequenceCount = 0;
           //}

           return true;
       }
       /// <summary>
       /// 卫星标记器
       /// </summary>
       class SatMarker
       {
           /// <summary>
           /// 构造
           /// </summary>
           /// <param name="Prn"></param>
           /// <param name="StartRecordTime"></param>
           public SatMarker(SatelliteNumber Prn, Geo.Times.Time StartRecordTime)
           {
               this.Prn = Prn;
               this.SequenceCount = 1;
               this.StartRecordTime = StartRecordTime;
           }
           /// <summary>
           /// 卫星编号
           /// </summary>
           public SatelliteNumber Prn { get; set; }

           /// <summary>
           ///断开的起始时间。
           /// </summary>
           public Geo.Times.Time StartBreakingTime { get; set; }

           /// <summary>
           ///记录起始时间。
           /// </summary>
           public Geo.Times.Time StartRecordTime { get; set; }
           /// <summary>
           /// 标记器
           /// </summary>
           public bool Mark { get; set; }
           /// <summary>
           /// 连续的数量
           /// </summary>
           public int SequenceCount { get; set; }
           /// <summary>
           /// 已经断开的数量
           /// </summary>
           public int GapCount { get; set; }

           public override string ToString()
           {
               return Prn + ":" + Mark + "," + SequenceCount + "," + GapCount;
           }
       }
    }

    /// <summary>
    /// 历元数据缓存器。
    /// </summary>
    public class EpochInfoBuffer :  BaseQueue<EpochInformation>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EpochInfoBuffer(int BufferSize = 40):base(BufferSize, true)
        {
            this.BufferSize = BufferSize; 
        }

        /// <summary>
        /// 缓存大小,单位为历元。
        /// </summary>
        public int BufferSize { get; set; }

        public void CheckOrInit(SatelliteNumber prn)
        {


            //if (!this.Contains(satelliteType))
            //{
            //    var queue =  new BaseQueue<EpochInformation>(BufferSize, true);
            //    queue.Dequeuing += queue_Dequeuing;
            //    this[satelliteType] = queue;
            //}
        }



    }
}
