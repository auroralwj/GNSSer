//2014.09, cy, 误差改正类型
//2014.10.25, czs, 命名为 CorrectionType

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gnsser.Correction
{
    /// <summary>
    /// 计算类型
    /// </summary>
    public enum CorrectionType
    {
        /// <summary>
        /// 未知，默认
        /// </summary>
        Unkown,
        /// <summary>
        /// 接收机天线ARP改正，单位NEU
        /// </summary>
        RecAntennaArp,
        /// <summary>
        /// 接收机天线PCO误差改正，单位NEU
        /// </summary>
        RecAntennaPco,
        /// <summary>
        /// 接收机天线对频率观测值的改正
        /// </summary>
        RecAntennaPcv,
        /// <summary>
        ///卫星天线对频率观测值的改正
        /// </summary>
        SatAntennaPcv,
        /// <summary>
        /// 海洋潮汐改正，单位NEU
        /// </summary>
        OceanTides,
        /// <summary>
        /// 极潮改正，单位NEU
        /// </summary>
        PoleTides,
        /// <summary>
        ///  固体潮改正，单位NEU
        /// </summary>
        SolidTides,
        /// <summary>
        /// 卫星钟改正， 距离改正数
        /// </summary>
        ClockBiasCorrector,
        /// <summary>
        /// 距离差分改正
        /// </summary>
        RangeDifferCorrector,
        /// <summary>
        /// DCB改正
        /// </summary>
        Dcb,
        /// <summary>
        /// 距离改正
        /// </summary>
        RangeOnlyCorrector,
        /// <summary>
        /// 相位改正
        /// </summary>
        PhaseOnlyCorrector,

        /// <summary>
        /// 引力延迟效应改正
        /// </summary>
        GravitationalDelay,
        /// <summary>
        /// 计算相位缠绕改正
        /// </summary>
        PhaseWindUp,
        /// <summary>
        ///  对流程改正
        /// </summary>
        RangeTropo,
       
        /// <summary>
        /// 钟差的相对论改正，与光速相差转换为伪距改正。
        /// </summary>
        ClockRelative,
        /// <summary>
        /// 计算卫星天线相位中心改正数。
        /// </summary>
        SatAntennaPhaseCenter,
        /// <summary>
        /// 对流程改正。默认采用Neill模型。这里只是干分量slant改正，湿延迟作为未知参数估计。
        /// </summary>
        TroposphericModel,
        /// <summary>
        /// 相位改正组合值
        /// </summary>
        PhaseChain,
        /// <summary>
        /// 电离层参数改正模型
        /// </summary>
        IonoParam,
        /// <summary>
        /// 电离层格网改正模型
        /// </summary>
        IonoGridModel,
        /// <summary>
        /// 通用距离改正组合
        /// </summary>
        RangeChain,
        /// <summary>
        /// 通用测站坐标改正
        /// </summary>
        NeuChain,
        /// <summary>
        /// 依据频率的卫星天线Rang改正
        /// </summary>
        FrequenceBasedRangChain,
        /// <summary>
        /// 依据频率的测站 NEU 改正
        /// </summary>
        FrequenceBasedNeuChain

    }
}
