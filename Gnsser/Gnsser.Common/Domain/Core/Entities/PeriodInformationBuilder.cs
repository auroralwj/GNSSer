//2016.05.03, czs, create in hongqing, 新建多历元测站数据模型

using System;
using System.Text;
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
using Gnsser.Filter;
using Gnsser.Checkers; 
using Geo.Times;
using Geo.IO;

namespace Gnsser.Domain
{
    /// <summary>
    /// 多历元测站数据模型
    /// </summary>
    public class PeriodInformationBuilder : AbstractBuilder<PeriodInformation>
    {
        Log log = new Log(typeof(PeriodInformationBuilder));
        public PeriodInformationBuilder(bool IsRequireSameSats, int AssignedEpochCount = 3)
        {
            this.AssignedEpochCount = AssignedEpochCount;
            this.MinSatCount = 4;
            this.MaxTimeSpan = 121;
            this.IsNeedSameSat = IsRequireSameSats; 
            this.Data = new List<EpochInformation>(); 
        } 
        #region 属性，检索器 
        /// <summary>
        /// 是否已经满了。
        /// </summary>
        public bool IsFull { get { return this.AssignedEpochCount >= this.Data.Count; } }

        /// <summary>
        /// 指定的历元数量，如果超过数量，则剔除第一历元。
        /// </summary>
        public int AssignedEpochCount { get; set; } 
        /// <summary>
        /// 最少卫星数量，如果少于此，则重新生成。
        /// </summary>
        public int MinSatCount { get; set; }
        /// <summary>
        /// 是否需要相同的卫星。
        /// </summary>
        public bool IsNeedSameSat { get; set; }
        /// <summary>
        /// 最大时间间隔，超出后，则重新构建
        /// </summary>
        public double MaxTimeSpan { get; set; }

        /// <summary>
        /// 差分定位选项
        /// </summary>
        public GnssProcessOption Option { get; set; }
        #endregion

        public List<EpochInformation> Data { get; set; }

        public bool Add(EpochInformation info)
        {
            if (Data.Contains(info)) return false;

            Data.Add(info); //add prevObj

            if (Data.Count > AssignedEpochCount)
            {
                Data.RemoveAt(0);
            }
            return true;
        }

        /// <summary>
        /// 生成。如果历元不足，则标记 Enabled 为 false；果失败，则返回 null.
        /// </summary>
        /// <returns></returns>
        public override PeriodInformation Build()
        {
            PeriodInformation infos = new PeriodInformation(IsNeedSameSat, AssignedEpochCount);
            infos.Add(Data); //首先添加数据	 

            //接下来，进行必要的检核
            if (infos.Count < AssignedEpochCount)
            {
              //  var msg = "历元数量不足, 只有 " + infos.Count + ",需要 " + AssignedEpochCount;
                infos.Enabled = false;

              //  log.Error(msg);
                return infos;
            }


            //if (IsNeedSameSat)//使观测的卫星相同，禁用不同的卫星
            //{
            //    List<SatelliteNumber> prns = new List<SatelliteNumber>();
            //    var fist = infos.First;
            //    foreach (var info in infos)
            //    {
            //        if (info == fist) continue;

            //        prns = SatelliteNumberUtils.GetDiffers(fist.EnabledPrns, info.EnabledPrns);
            //    }
            //    infos.DisableOthers(prns);
            //}
            //卫星数量不足
            if (infos.EnabledSatCount < MinSatCount) //历元数量皆相同
            {
                var msg = "共用卫星数量不足，至少需要 " + MinSatCount + " 颗，但是只有 " + infos.EnabledSatCount + "，跳过此时段" + infos;
                infos.Enabled = false;

                log.Error(msg);
                return infos;
            }

            //是否在时段范围内

            var lastTimeDiffer = infos.Last.ReceiverTime - infos[infos.Count - 2].ReceiverTime;
            if (lastTimeDiffer > MaxTimeSpan)
            {
                this.Data.Clear();

                var msg = "新历元超过允许的时段范围 " + MaxTimeSpan + "，将清空旧历元，重新来过。上次：" + infos[infos.Count - 2].Time + ",这次：" + infos.Last.Time + ", 间隔：" + lastTimeDiffer;
                infos.Enabled = false;
                log.Error(msg);
                return infos;
            }

            return infos;
        }
    }
}