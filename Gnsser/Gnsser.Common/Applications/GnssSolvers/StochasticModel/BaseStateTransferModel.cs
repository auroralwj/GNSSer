//2016.05.05, czs, refactor in hongqing, 提取状态随机模型的基类
//2017.08.12, czs, edit in hongqing, 提取参数，使得可以设置

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Domain;
using Gnsser.Data.Rinex;
using Geo.Times;

namespace Gnsser.Models
{
    /// <summary>
    /// 状态随机模型的基类
    /// </summary>
    public class BaseStateTransferModel
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaseStateTransferModel() {
            PreviousTime = Time.MinValue;
            CurrentTime = Time.MinValue;
        }
        /// <summary>
        /// 对应的参数名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 上一历元
        /// </summary>
        public Time PreviousTime { get; set; }
        /// <summary>
        ///当前历元
        /// </summary>
        public Time CurrentTime { get; set; }

        /// <summary>
        /// 状态转移系数。  
        /// </summary>
        /// <returns></returns>
        public virtual double GetTrans()
        {
            return 1.0;
        }
        /// <summary>
        /// 噪声模型，随机模型方差，权逆阵。 
        /// </summary>
        /// <returns></returns>
        public virtual double GetNoiceVariance() { return 0.0; }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="epochInfo"></param>
        public virtual void Init(ISiteSatObsInfo epochInfo)
        {
            if (epochInfo is EpochInformation)
            {
                Init((EpochInformation)epochInfo);
            }
            else if (epochInfo is PeriodInformation)
            {
                Init((PeriodInformation)epochInfo);
            }
            else if (epochInfo is MultiSiteEpochInfo)
            {
                Init((MultiSiteEpochInfo)epochInfo);
            }
            else if (epochInfo is MultiSitePeriodInfo)
            {
                Init((MultiSitePeriodInfo)epochInfo);
            } 
        }
        /// <summary>
        /// 初始化
        /// </summary> 
        /// <param name="epochInfo">历元信息</param>
        public virtual void Init(EpochInformation epochInfo) { }
        public virtual void Init(MultiSiteEpochInfo epochInfo) { this.Init(epochInfo.First); }
        public virtual void Init(MultiSitePeriodInfo epochInfo) { this.Init(epochInfo.First); }
        public virtual void Init(PeriodInformation epochInfo) { this.Init(epochInfo.First); }
        

        internal double GetTrans_V()
        {
            return 1.0;
        }
    }

 
   

















}
