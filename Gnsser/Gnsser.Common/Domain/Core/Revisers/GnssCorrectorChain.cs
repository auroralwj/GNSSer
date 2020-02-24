//2017.09.05, czs, create in hongqing, 改正器类型

using System;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Gnsser.Domain;
using Geo.Utils;
using System.Collections.Generic;
using System.Collections;
using Geo.Correction;

namespace Gnsser.Correction
{
    //2017.09.05, czs, create in hongqing, 改正器类型
    /// <summary>
    /// 改正器链分类，用于确定改正数最终加在什么量上面。
    /// </summary>
    public enum CorrectChianType
    {
        /// <summary>
        /// 自己直接改正，责任链不必相加。
        /// </summary>
        Self,
        /// <summary>
        /// 通用距离改正
        /// </summary>
        Common,
        /// <summary>
        /// 伪距改正
        /// </summary>
        RangeOnly,
        /// <summary>
        /// 相位距离改正
        /// </summary>
        PhaseRangeOnly,
        /// <summary>
        /// 相位改正
        /// </summary>
        Phase

    }

    /// <summary>
    /// 改正数链表。
    /// </summary>
    /// <typeparam name="TCorrection"></typeparam>
    /// <typeparam name="TInput"></typeparam>
    public class GnssCorrectorChain<TCorrection, TInput> : CorrectorChain<TCorrection, TInput>
    {

        /// <summary>
        /// 改正器类型
        /// </summary>
        public CorrectChianType CorrectChianType { get; set; }

    }
}