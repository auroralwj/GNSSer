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
    public abstract class MultiSitePeroidMatrixBuilder : BaseGnssMatrixBuilder<BaseGnssResult, MultiSitePeriodInfo>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MultiSitePeroidMatrixBuilder(GnssProcessOption GnssOption)
            : base(GnssOption)
        {

        }

        #region 全局基础属性

        /// <summary>
        /// 参与计算的卫星的数量
        /// </summary>
        public int SatCount { get { return CurrentMaterial.EnabledSatCount; } }


        /// <summary>
        /// 测站数量
        /// </summary>
        public virtual int SiteCount { get => this.CurrentMaterial.Count; }
        /// <summary>
        /// 当前历元列表
        /// </summary>
        public List<Time> Epoches { get { return CurrentMaterial.Epoches; } }

        /// <summary>
        /// 历元数量,743333
        /// </summary>
        public int EpochCount { get { return CurrentMaterial.EpochCount; } }
        //  public MultiSiteMatrixBuilder SetMaterial(MultiSiteEpochInfo EpochInfos) { this.CurrentMaterial = EpochInfos; return this; }

        #endregion

    
        //  public MultiSiteMatrixBuilder SetMaterial(MultiSiteEpochInfo EpochInfos) { this.CurrentMaterial = EpochInfos; return this; }

    }
}