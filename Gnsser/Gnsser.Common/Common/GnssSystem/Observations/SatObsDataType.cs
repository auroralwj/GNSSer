//2014.12.09, czs, create in jinxinliaomao shuangliao, 数据类型,包含组合类型，观测类型。
//2014.12.13, czs, edit in namu shuangliao, 数据分为观测值，近似值两类

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Coordinates; 
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Times; 
using Geo.Correction;
using Gnsser.Correction;
using Geo.Utils;
using Geo;

namespace Gnsser
{

    //2017.07.24, czs, create in hongqing, 伪距类型
    /// <summary>
    /// 距离类型，包括载波和伪距类型
    /// </summary>
    public enum RangeType
    {
        RangeA,
        RangeB,
        RangeC,
        IonoFreeRangeOfAB,
        IonoFreeRangeOfBC,
        IonoFreeRangeOfAC,
    }

    //2017.07.24, czs, create in hongqing, 频率组合类型
    /// <summary>
    /// 频率组合类型
    /// </summary>
    public enum FreqCombinationType
    {
        AB,
        BC,
        AC, 
    }



    /// <summary>
    /// 近似值类型
    /// </summary>
    public enum SatApproxDataType
    {
        /// <summary>无电离层伪距估值</summary>
        IonoFreeApproxPseudoRange,
        /// <summary>无电离层载波相位距离估值</summary>
        IonoFreeApproxPhaseRange,

        /// <summary>伪距估值</summary>
        ApproxPseudoRangeA,
        /// <summary>载波相位距离估值</summary>
        ApproxPhaseRangeA,
        /// <summary>伪距估值</summary>
        ApproxPseudoRangeB,
        /// <summary>载波相位距离估值</summary>
        ApproxPhaseRangeB,
        /// <summary>伪距估值</summary>
        ApproxPseudoRangeC,
        /// <summary>载波相位距离估值</summary>
        ApproxPhaseRangeC,
        /// <summary>三频伪距估值</summary>
        ApproxPseudoRangeOfTriFreq,
        /// <summary>三频载波相位距离估值</summary>
        ApproxPhaseRangeOfTriFreq,
        
    }
    public enum SatApproxOfTriFreqDataType
    {
        /// <summary>三频伪距估值</summary>
        ApproxPseudoRangeOfTriFreq,
        /// <summary>三频载波相位距离估值</summary>
        ApproxPhaseRangeOfTriFreq,
        
    }
    /// <summary>
    /// 观测方程左边的数据类型,包含组合类型，观测类型。
    /// </summary>
    public enum SatObsDataType
    { 
        IonoFreePhaseRangeOfTriFreq,
        /// <summary>对齐的无电离层载波相位组合，由频率A、B、C组成</summary>
        AlignedIonoFreePhaseRangeOfTriFreq,
        /// <summary>无电离层伪距组合，由频率A、B组成 </summary>
        IonoFreeRange,
        /// <summary>无电离层载波相位组合，由频率A、B组成</summary>
        IonoFreePhaseRange,
        /// <summary>对齐的无电离层载波相位组合，由频率A、B组成</summary>
        AlignedIonoFreePhaseRange,
        /// <summary>无电离层伪距组合，由频率A、B、C组成 </summary>
        IonoFreeRangeOfTriFreq,
        /// <summary>对齐的无电离层载波相位组合，由频率A、B组成</summary>
        //AlignedIonoFreePhaseRangeTriFrequency,
        /// <summary> 频率A的伪距 </summary>      
        PseudoRangeA,
        /// <summary>  频率A的载波距离，频率观测值乘以波长 </summary>
        PhaseA,
        /// <summary> 对齐的频率A的载波距离 </summary>
        PhaseRangeA,
        /// <summary>频率B的伪距</summary>
        PseudoRangeB,
        /// <summary>频率B的载波距离，频率观测值乘以波长 </summary>
        PhaseB,
        /// <summary> 对齐的频率C的载波距离</summary>
        PhaseRangeB,
        /// <summary> 频率C的伪距</summary>
        PseudoRangeC,
        /// <summary> 频率C的载波距离，频率观测值乘以波长</summary>
        PhaseC,
        /// <summary>对齐的频率C的载波距离 </summary>
        PhaseRangeC,
        /// <summary>
        /// MW组合
        /// </summary>
        MwCombination,
        /// <summary>
        /// MW组合
        /// </summary>
        LiCombination
    }

    /// <summary>
    /// 观测类型帮助
    /// </summary>
    public class SatObsDataTypeHelper
    {

        /// <summary>
        /// 从观测类型提取频率类型
        /// </summary>
        /// <param name="ObsDataType"></param>
        /// <returns></returns>
        public static FrequenceType GetFrequenceTypeFromObsDataType(SatObsDataType ObsDataType)
        {
            if (ObsDataType.ToString().EndsWith("A"))
            {
                return FrequenceType.A;
            }
            else if (ObsDataType.ToString().EndsWith("B"))
            {
                return FrequenceType.B;
            }
            else if (ObsDataType.ToString().EndsWith("C"))
            {
                return FrequenceType.C;
            }
            return FrequenceType.A;
        }

    }
}