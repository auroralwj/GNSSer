
//2018.05.17, czs, create, 通用的观测改正

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Correction;
using Geo.Correction;

namespace Gnsser.Domain
{

    /// <summary>
    /// 通用的观测改正
    /// </summary>
    public interface ICommonObservationCorrection
    {
        /// <summary>
        /// 通用模型距离改正，同时适用于伪距和载波，如卫星钟差改正、对流层改正等。
        /// </summary>
        NumerialCorrectionDic CommonCorrection { get; set; }
        /// <summary>
        /// 相位距离改正，如电离层改正-
        /// </summary>
        NumerialCorrectionDic PhaseOnlyCorrection { get; set; }
        /// <summary>
        /// 伪距特有距离改正，如电离层改正+
        /// </summary>
        NumerialCorrectionDic RangeOnlyCorrection { get; set; }

        /// <summary>
        /// 获取站星层次的通用载波距离改正
        /// </summary>
        /// <returns></returns>
        NumerialCorrectionDic GetCommonPhaseCorrection();
        /// <summary>
        /// 获取站星层次的通用伪距距离改正
        /// </summary>
        /// <returns></returns>
        NumerialCorrectionDic GetCommonRangeCorrection();

        /// <summary>
        /// 添加相位改正。对所有的相位起作用。这里转换成相位观测值距离的改正数（Frequence.PhaseRange.Correction）。
        /// </summary>
        /// <param name="corrector">相位改正数，是相位</param>
        /// <param name="cycle">相位改正数，是相位</param>
        void AddPhaseCyleCorrection(string corrector, double cycle);
        /// <summary>
        /// 通用周为单位的距离改正
        /// </summary>
        /// <param name="corrector"></param>
        /// <param name="cycle"></param>
        void AddCommonCyleCorrection(string corrector, double cycle);
        /// <summary>
        /// 添加伪距距离改正
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        void AddRangeCorrection(string key, double val);
        /// <summary>
        /// 添加通用距离改正
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        void AddCommonCorrection(string key, double val);
        /// <summary>
        /// 添加相位距离改正
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        void AddPhaseCorrection(string key, double val);
        /// <summary>
        /// 移除所有的改正数，包括公共的和频率私有的。
        /// 通常是为了重新计算。
        /// </summary>
        void ClearCorrections();


    }
}