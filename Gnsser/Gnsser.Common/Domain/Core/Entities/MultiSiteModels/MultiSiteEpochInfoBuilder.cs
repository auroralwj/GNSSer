//2016.04.24, czs, edit in hongqing, 多测站历元信息
//2016.05.03, czs, create in hongqing, 多测站历元信息构建器

using System;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Times;
using Geo.IO;

namespace Gnsser.Domain
{
    /// <summary>
    /// 多测站历元信息构建器
    /// </summary>
    public class MultiSiteEpochInfoBuilder : AbstractBuilder<MultiSiteEpochInfo>
    {
        Log log = new Log(typeof(MultiSiteEpochInfoBuilder));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="IsAllSiteRequired">是否需要所有测站</param>
        /// <param name="IsSameSatRequired">是否需要相同卫星</param>
        /// <param name="MaxTimeDifferSecond">站间最大时间偏差</param>
        public MultiSiteEpochInfoBuilder(bool IsSameSatRequired, bool IsAllSiteRequired = true, double MaxTimeDifferSecond = 0.5)
        {
            this.IsAllSiteRequired = IsAllSiteRequired;
            this.MaxTimeDiffer = MaxTimeDifferSecond;
            Epoch = Time.Default;
            this.IsSameSatRequired = IsSameSatRequired;
            ListExceptBase = new List<EpochInformation>();
        }
        /// <summary>
        /// 最大时间偏差
        /// </summary>
        public double MaxTimeDiffer { get; set; }             
        /// <summary>
        /// 当前历元
        /// </summary>
        public Time Epoch { get; set; }
        /// <summary>
        /// 基准测站的观测信息
        /// </summary>
        public EpochInformation BaseEpochInfo { get; private set; }
        /// <summary>
        /// 是否需要卫星共视
        /// </summary>
        public bool IsSameSatRequired { get; set; }
        /// <summary>
        /// 是否需要所有加入的测站
        /// </summary>
        public bool IsAllSiteRequired { get; set; }

        /// <summary>
        /// 其它历元信息，除了基准信息
        /// </summary>
        public List<EpochInformation> ListExceptBase { get; private set; }

        /// <summary>
        /// 添加。
        /// </summary>
        /// <param name="EpochInformation"></param>
        /// <param name="isBase"></param>
        public void Add(EpochInformation EpochInformation, bool isBase = false)
        {
            if(Epoch == Time.Default)
            {
                Epoch = EpochInformation.ReceiverTime;
            }else if (Epoch != EpochInformation.ReceiverTime)
            {
                throw new ArgumentException("时间不匹配！要求：" + Epoch + ", 但输入历元是：" + EpochInformation.ReceiverTime);
            }

            if (isBase)
            {
                BaseEpochInfo = EpochInformation;
            }
            else
            {
                if (!ListExceptBase.Contains(EpochInformation))
                {
                    ListExceptBase.Add(EpochInformation);
                }
            }
        }
        /// <summary>
        /// 如果不指定基准，则默认以第一个为基准。
        /// </summary>
        /// <returns></returns>
        public override MultiSiteEpochInfo Build()
        {
            if (BaseEpochInfo == null && ListExceptBase.Count == 0)
            {
                throw new ArgumentException("至少需要2个测站的历元观测数据！");
            }

            MultiSiteEpochInfo info = new MultiSiteEpochInfo(IsSameSatRequired);
            if (BaseEpochInfo != null)
            {
                info.BaseEpochInfo = BaseEpochInfo;
            }
            else
            {
                info.BaseEpochInfo = ListExceptBase[0];
                ListExceptBase.RemoveAt(0);
            }
            //添加基准星到字典
            info.Add(BaseEpochInfo.SiteName, info.BaseEpochInfo);

            foreach (var item in ListExceptBase)
            {
                var timeDiffer = Math.Abs(BaseEpochInfo.ReceiverTime - item.ReceiverTime);
                if (timeDiffer >= MaxTimeDiffer)
                {
                    log.Debug("历元时间超限！生成失败" + timeDiffer + ">" + MaxTimeDiffer);
                    if (IsAllSiteRequired) { return null; }
                }

                info.Add(item.SiteName, item);
            }

            //生成名字,若生成不一致，将导致产生不同的结果文件。//2018.10.26，czs，hmx  
            //第一个为基准站！！，//2018.11.30，czs，hmx 
            string name = ResultFileNameBuilder.BuildMultiSiteEpochInfoName(this.BaseEpochInfo.SiteName, info.RovSiteNames);
            info.Name = name;
            return info;
        }
      
    }

}