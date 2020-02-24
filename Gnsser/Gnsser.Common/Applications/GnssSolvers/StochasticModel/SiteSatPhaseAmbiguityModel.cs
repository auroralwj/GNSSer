//2016.01.29, cy, 添加时间标记
//2017.08.12, czs, edit in hongqing, 提取参数，使得可以设置
//2018.07.28, czs, edit in HMX, 修改，增加注释
//2018.10.26, czs, create in hmx, 修改名称为字符串，以支持多星

using System;
using Gnsser.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Data.Rinex;
using Geo.Times;


namespace Gnsser.Models
{
    /// <summary>
    /// 相位模糊度模型。
    /// 计算相位参数的状态转移模型，
    /// 如果有周跳则是白噪声模型，如果没有周跳，则是常量模型。
    /// </summary>
    public class SiteSatPhaseAmbiguityModel : BaseStateTransferModel
    {
        /// <summary>
        /// 相位模糊度模型
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="prn"></param>
        /// <param name="stdDev"></param>
        /// <param name="stdDevWhenCycled"></param>
        public SiteSatPhaseAmbiguityModel(string siteName, SatelliteNumber prn, double stdDev, double stdDevWhenCycled)
        {
            VarianceWhenCycled = stdDevWhenCycled * stdDevWhenCycled;
            this.Variance = stdDev * stdDev;
            this.HasCycleSlip = true;
            this.Prn = prn;
            this.SiteName = siteName;
        }

        #region 属性
        /// <summary>
        /// 测站名称
        /// </summary>
        public string SiteName { get; set; }
        /// <summary>
        ///  White noise variance
        /// </summary>
        private double VarianceWhenCycled { get; set; }
        /// <summary>
        /// 方差
        /// </summary>
        public double Variance { get; set; }

        /// <summary>
        /// Boolean stating if there is a cycle slip at current epoch
        /// </summary>
        public bool HasCycleSlip { get; set; }

        /// <summary>
        /// 当前卫星编号
        /// </summary>
        public SatelliteNumber Prn { get; set; }
        #endregion

        /// <summary>
        /// 是否
        /// </summary>
        /// <returns></returns>
        public virtual bool IsBreaked()
        {
            return HasCycleSlip;
        }

        /// <summary>
        ///  Get element of the state transition matrix Phi
        /// </summary>
        /// <returns></returns>
        public override double GetTrans()
        {
            // Check if there is a cycle slip
            if (IsBreaked())
            {
                return 0.0;
            }
            else
            {
                return 1.0;
            }
        }
        /// <summary>
        /// Get element of the state transition matrix Phi
        /// </summary>
        /// <returns></returns>
        public override double GetNoiceVariance()
        {
            // Check if there is a cycle slip
            if (IsBreaked())
            {
                return VarianceWhenCycled;
            }
            else
            {
                return Variance;// 0.0;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="material"></param>
        public override void Init(EpochInformation material)
        {
            if (material.EnabledPrns.Contains(Prn))
            {
                this.HasCycleSlip = material[Prn].IsUnstable;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="material"></param>
        public override void Init(PeriodInformation material)
        {
            foreach (var site in material)
            {
                if (SiteName == site.SiteName)
                {
                    this.HasCycleSlip = site.HasCycleSlip(Prn);
                    break;
                }

                //if (site[Prn].IsUnstable)
                //{
                //    this.HasCycleSlip = true;
                //    return;
                //}
            }
            this.HasCycleSlip = false;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="material"></param>
        public override void Init(MultiSiteEpochInfo material)
        {
            foreach (var site in material)
            {
                if(SiteName == site.SiteName)
                {
                    this.HasCycleSlip = site.HasCycleSlip(Prn);
                    break;
                }
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="material"></param>
        public override void Init(MultiSitePeriodInfo material) 
        {
            if (material.EnabledPrns.Contains(Prn))
            {
                this.HasCycleSlip = material.HasCycleSlip(Prn);
            }
        }
    }
}
