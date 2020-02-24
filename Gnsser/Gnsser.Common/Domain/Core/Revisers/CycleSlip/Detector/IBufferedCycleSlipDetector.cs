//2017.09.25, czs, create in hongiqng, 基于整体缓存的周跳探测方法

using System;
using Gnsser.Domain;
using System.Collections;
using System.Collections.Generic;
using Geo;
using Geo.Times;
using Gnsser.Core;
using System.Text;

namespace Gnsser
{

    /// <summary>
    /// 基于整体缓存的周跳探测方法，周跳探测结果通用接口。CycleSlip 缩写为 CS。
    /// </summary>
    public interface IBufferedCycleSlipDetector : Geo.Namable
    {
        /// <summary>
        ///  执行探测,直接标记。
        /// </summary>
        /// <param name="epochInfos"></param>
        void Detect(IBuffer<EpochInformation> epochInfos);
    }
}