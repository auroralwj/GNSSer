//2018.07.28, czs, create in HMX, 双星相位模糊度模型

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
    /// 双星相位模糊度模型。
    /// 计算相位参数的状态转移模型，
    /// 如果有周跳则是白噪声模型，如果没有周跳，则是常量模型。
    /// </summary>
    public class DueSatPhaseAmbiguityMode: SingleSatPhaseAmbiguityModel
    { 
        /// <summary>
        /// 相位模糊度模型
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="stdDev"></param>
        /// <param name="stdDevWhenCycled"></param>
        public DueSatPhaseAmbiguityMode(SatelliteNumber prn, double stdDev, double stdDevWhenCycled):base( prn, stdDev, stdDevWhenCycled)
        {
            this.BasePrn = SatelliteNumber.Default;

            VarianceWhenCycled = stdDevWhenCycled * stdDevWhenCycled;
            this.Variance = stdDev * stdDev;
            this.HasCycleSlip = true;
            this.Prn = prn;
            this.BasePrnHasCycleSlip = true;
        }   

        /// <summary>
        /// 当前基准卫星编号
        /// </summary>
        public SatelliteNumber BasePrn { get; set; }
        /// <summary>
        /// 基准星是否改变
        /// </summary>
        public bool IsBasePrnChanged { get; set; }

        /// <summary>
        /// 基准星是否发生周跳
        /// </summary>
        public bool BasePrnHasCycleSlip { get; set; }

        /// <summary>
        /// 当前基准卫星编号
        /// </summary>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public DueSatPhaseAmbiguityMode SetBasePrn(SatelliteNumber basePrn)
        {
            this.IsBasePrnChanged = (this.BasePrn != basePrn);
            this.BasePrn = basePrn; 

            return this;
        }

        /// <summary>
        /// 是否断裂
        /// </summary>
        /// <returns></returns>
        public override bool IsBreaked()
        {
            return base.IsBreaked() || IsBasePrnChanged || BasePrnHasCycleSlip;
        }



        /// <summary>
        ///  White noise variance
        /// </summary>
        private double VarianceWhenCycled { get; set; }


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

            if (material.EnabledPrns.Contains(BasePrn))
            {
                this.BasePrnHasCycleSlip = material[BasePrn].IsUnstable;
            }
        }



        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="material"></param>
        public override void Init(MultiSiteEpochInfo material)
        {
            this.HasCycleSlip = material.HasCycleSlip(Prn);

            this.BasePrnHasCycleSlip = material.HasCycleSlip(BasePrn);
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
            this.BasePrnHasCycleSlip = material.HasCycleSlip(BasePrn);
        }
    }

}

