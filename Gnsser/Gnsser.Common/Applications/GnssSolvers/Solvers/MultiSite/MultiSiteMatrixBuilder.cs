//2016.04.06 double create in Zhengzhou 参考BasePositionMatrixBuilder.cs
//2017.09.02, czs, refactor in hongqing, 重构状态转移模型
//2018.12.23, czs, edit in hmx, 单独成文件

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Data.Rinex;
using Gnsser.Checkers;
using Geo.Utils;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Times;

namespace Gnsser
{
    /// <summary>
    /// 基础的定位矩阵生成器
    /// </summary>
    public abstract class MultiSiteMatrixBuilder : BaseGnssMatrixBuilder<BaseGnssResult,MultiSiteEpochInfo>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MultiSiteMatrixBuilder(GnssProcessOption GnssOption)
            : base(GnssOption)
        {

        }
        /// <summary>
        /// 测站数量
        /// </summary>
        public virtual int SiteCount { get => this.CurrentMaterial.Count; }

        public virtual int BaseClockCount { get; set; }

        /// <summary>
        /// 生成多测站单历元原始伪距+载波的观测权逆阵，将基准站放在第一，基准星放在第一。
        /// </summary>
        protected DiagonalMatrix BuildPrmevalObsCovaMatrix()
        {
            this.BaseSiteName = this.CurrentMaterial.BaseSiteName;

            ObsCovaMatrixBuilder builder = new ObsCovaMatrixBuilder(this.SatWeightProvider);
            return builder.BuildPrmevalRangeAndPhaseObsCovaMatrix(this.CurrentMaterial,BaseSiteName, CurrentBasePrn,  Option.PhaseCovaProportionToRange);

          
        }
        //  public MultiSiteMatrixBuilder SetMaterial(MultiSiteEpochInfo EpochInfos) { this.CurrentMaterial = EpochInfos; return this; }

    }

      
}