//2016.05.03, czs, create in hongqing, 新建多历元多测站数据模型
//2016.08.15, czs, edit in hongqing, 支持多测站数据流

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
    /// 多历元多测站数据构建器
    /// </summary>
    public class MultiSitePeriodInfoBuilder : AbstractBuilder<MultiSitePeriodInfo>
    {
        Log log = new Log(typeof(MultiSitePeriodInfoBuilder));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        public MultiSitePeriodInfoBuilder(GnssProcessOption option)
        {
            this.AssignedEpochCount = option.MultiEpochCount;
            if (this.AssignedEpochCount < 2)
            {
                log.Error("错误：指定的历元数量为 " + AssignedEpochCount + " ,已修改为 2");
                this.AssignedEpochCount = 2;
            }
            this.MinSatCount = option.MinSatCount;
            this.MaxTimeSpan = option.MaxEpochSpan;
            this.IsRequireSameSats = option.IsSameSatRequired;
            this.Data = new List<MultiSiteEpochInfo>();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="AssignedEpochCount">历元数量</param>
        /// <param name="MaxTimeSpan"></param>
        /// <param name="MinCommonEnabledSatCount">最小的可用卫星数量</param> 
        public MultiSitePeriodInfoBuilder(int AssignedEpochCount,  double MaxTimeSpan=121, int MinCommonEnabledSatCount=0)
        {
            this.AssignedEpochCount = AssignedEpochCount;
            this.MinSatCount = MinCommonEnabledSatCount;
            this.MaxTimeSpan = MaxTimeSpan; 
            this.Data = new List<MultiSiteEpochInfo>();
        } 

        #region 属性
        /// <summary>
        /// 指定的历元数量，如果超过数量，则剔除第一历元。
        /// </summary>
        public int AssignedEpochCount { get; set; } 
        /// <summary>
        /// 最少卫星数量，如果少于此，则重新生成。
        /// </summary>
        public int MinSatCount { get; set; }
        /// <summary>
        /// 最大时间间隔，超出后，则重新构建
        /// </summary>
        public double MaxTimeSpan { get; set; }
        /// <summary>
        /// 历元是否满足要求
        /// </summary>
        public bool IsFull { get { return Data.Count >= AssignedEpochCount; } }
        /// <summary>
        /// s是否需要相同的卫星
        /// </summary>
        public bool IsRequireSameSats { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public List<MultiSiteEpochInfo> Data { get; set; }
        #endregion
        /// <summary>
        /// 添加一个。
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Add(MultiSiteEpochInfo info)
        {
            if (Data.Contains(info)) { return false; }
             
            Data.Add(info); //add prevObj

            if (Data.Count > AssignedEpochCount)
            {
                Data.Clear();
                Data.Add(info); //add prevObj
              //  Data.RemoveAt(0);
            }
            return true;
        }
        /// <summary>
        /// 生成。如果历元不足，则标记 Enabled 为 false；果失败，则返回 null.
        /// </summary>
        /// <returns></returns>
        public override MultiSitePeriodInfo Build()
        {
            MultiSitePeriodInfo infos = new MultiSitePeriodInfo();
            infos.Add(Data); //首先添加数据	 

            //接下来，进行必要的检核
            if (infos.Count < AssignedEpochCount)
            {
                var msg = "历元数量不足, 只有 " + infos.Count + ",需要 " + AssignedEpochCount;
                infos.Enabled = false;

                log.Debug(msg);
                return infos;
            }
            
            //卫星数量不足
            if (IsRequireSameSats && infos.EnabledSatCount < MinSatCount) //历元数量皆相同
            {
                var msg = "共用卫星数量不足，至少需要 " + MinSatCount + " 颗，但是只有 " + infos.EnabledSatCount + "，跳过此时段" + infos;
                infos.Enabled = false;

                log.Error(msg);
                return infos;
            }

            //是否在时段范围内
            if (infos.Count < 2)
            {
                log.Error("多历元不可少于 2 ，请检查配置是否正确！");
                infos.Enabled = false;
                return infos;
            }

            var lastTimeDiffer = infos.Last.ReceiverTime - infos[infos.Count - 2].ReceiverTime;
            if (lastTimeDiffer > MaxTimeSpan)
            {
                this.Data.Clear();

                var msg = "新历元超过允许的时段范围 " + MaxTimeSpan + "，将清空旧历元，重新来过。上次：" + infos[infos.Count - 2].ReceiverTime + ",这次：" + infos.Last.ReceiverTime + ", 间隔：" + lastTimeDiffer;
                infos.Enabled = false;
                log.Error(msg);
                return infos;
            }

            infos.Name = infos[0].Name;

            return infos;
        }
    }
 
}