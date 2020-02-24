//2014.10.06，czs, create in hailutu, 单点定位选项
//2015.10.15, czs, edit in 西安五路口袁记肉夹馍店, 增加延迟数量
//2016.05.01, czs, edit in hongqing, 更改为 Gnss 数据处理选项
//2016.08.02, czs, edit in fujian yongan, 增加固定参考站PPP
//2017.09.05, czs, edit in hongqing, 增加扩展计算，动态伪距定位，多系统伪距定位
//2017.09.18, czs, edit in hongqing, 增加单站多历元GNSS计算
//2018.04.18, czs, edit in hmx, 增加递归最小二乘法 
//2018.05.15, czs, edit in hmx, 增加电离层硬件延迟计算
//2018.07.30, czs, edit in hmx, 调整顺序，让PPP默认第一

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Geo.Times;
using Geo.Coordinates;
using Gnsser;
using Gnsser.Core;
using Geo;
using Gnsser.Service;
using Geo.IO;

namespace Gnsser
{
    /// <summary>
    /// 单站单星
    /// </summary>
    public enum SingleSiteSingleSatSolverType
    {



        /// <summary>
        /// 电离层硬件延迟计算
        /// </summary>
        电离层硬件延迟计算,
    }

    /// <summary>
    /// 单站解算器
    /// </summary>
    public enum SingleSiteGnssSolverType
    {
        /// <summary>
        /// 精密单点定位
        /// </summary>
        无电离层组合PPP,
        /// <summary>
        /// 最简伪距定位
        /// </summary>
        最简伪距定位,
        /// <summary>
        /// 动态伪距定位
        /// </summary>
        动态伪距定位,
        /// <summary>
        /// 多系统伪距定位
        /// </summary>
        多系统伪距定位,
        多频伪距定位,
        参数化对流层伪距定位,
        递归无电离层组合PPP,
        对流层增强无电离层PPP,
        /// <summary>
        /// 单站多历元扩展计算
        /// </summary>
        单站多历元扩展计算,
        /// <summary>
        /// 单频PPP
        /// </summary>
        单频PPP,
        单频半和PPP,
        电离层模型化单频PPP,
        单频多历元电离层参数化定位,
        /// <summary>
        /// 非差非组合
        /// </summary>
        非差非组合PPP,
        /// <summary>
        /// 通用单站单频计算
        /// </summary>
        通用单站单频计算,
        /// <summary>
        ///固定参考站PPP 
        /// </summary>
        固定参考站PPP,
        /// <summary>
        /// 单站单历元扩展计算
        /// </summary>
        单站单历元扩展计算,
        /// <summary>
        /// 单频消电离层组合PPP
        /// </summary>
        单频消电离层组合,
        单站多历元消电离层PPP,

        /// <summary>
        /// 电离层硬件延迟计算
        /// </summary>
        电离层硬件延迟计算,
        电离层延迟变化计算,
        双频电离层改正单频PPP
    } 

    /// <summary>
    /// 单基线（双站）算类型
    /// </summary>
    public enum TwoSiteSolverType
    {
        /// <summary>
        /// 无电离层双差
        /// </summary>
        无电离层双差,
        /// <summary>
        /// 简易单历元载波双差
        /// </summary>
        单历元单频双差,
        /// <summary>
        /// 单历元双频双差
        /// </summary>
        单历元双频双差,
        /// <summary>
        /// 单历元载波双差
        /// </summary>
        单历元载波双差,
        /// <summary>
        /// 单历元双频载波双差
        /// </summary>
        单历元双频载波双差, 
        /// <summary>
        /// 模糊度固定的单历元纯载波双差
        /// </summary>
        模糊度固定的单历元纯载波双差,
        /// <summary>
        /// 多历元载波单差
        /// </summary>
        多历元载波单差,
        /// <summary>
        /// 多历元载波无相关单差
        /// </summary>
        多历元载波无相关单差,
        /// <summary>
        /// 多历元载波双差
        /// </summary>
        多历元载波双差,
        /// <summary>
        /// 多站单历元扩展计算
        /// </summary>
        多站单历元扩展计算,
        /// <summary>
        /// 多站多历元扩展计算
        /// </summary>
        多站多历元扩展计算,
    }
    /// <summary>
    /// 多站网解算法类型
    /// </summary>
    public enum MultiSiteNetSolverType{
        网解双差定位,
        递归网解双差定位,
        网解单差定位,
        钟差网解,
        双差定轨,
        非差定轨,
        简易伪距定轨,
        多站单历元扩展计算,
        多站多历元扩展计算
    }

         
    /// <summary>
    /// 预处理或格式化观测文件处理类型
    /// </summary>
    public enum RinexObsFileFormatType
    {
        /// <summary>
        /// 独立的观测站
        /// </summary>
        单站单历元,
        /// <summary>
        /// 多站单历元
        /// </summary>
        多站单历元,
        /// <summary>
        /// 多站多历元
        /// </summary>
        单站多历元,
        /// <summary>
        /// 多站多历元
        /// </summary>
        多站多历元,

    }

    /// <summary>
    /// 多站网解计算
    /// </summary>
    public enum GnssSolverType
    {
        /// <summary>
        /// 精密单点定位
        /// </summary>
        无电离层组合PPP,
        /// <summary>
        /// 最简伪距定位
        /// </summary>
        最简伪距定位,
        /// <summary>
        /// 动态伪距定位
        /// </summary>
        动态伪距定位,
        /// <summary>
        /// 多系统伪距定位
        /// </summary>
        多系统伪距定位,
        /// <summary>
        /// 多频伪距定位
        /// </summary>
        多频伪距定位,
        参数化对流层伪距定位,
        递归无电离层组合PPP,
        对流层增强无电离层PPP,
        /// <summary>
        /// 非差非组合精密单点定位
        /// </summary>
        非差非组合PPP,
        /// <summary>
        /// 非差非组合精密单点定位
        /// </summary>
        单频PPP,
        单频半和PPP,
        电离层模型化单频PPP,
        单频多历元电离层参数化定位,
        /// <summary>
        /// 通用单站单频计算
        /// </summary>
        通用单站单频计算,
        /// <summary>
        ///固定参考站PPP 
        /// </summary>
        固定参考站PPP,
        /// <summary>
        /// 钟差解算
        /// </summary>
        钟差网解,
        /// <summary>
        /// 双差网解定位
        /// </summary>
        双差网解定位,
        网解双差定位,
        递归网解双差定位,
        /// <summary>
        /// 网解单差定位
        /// </summary>
        网解单差定位,
        /// <summary>
        /// 无电离层双差
        /// </summary>
        无电离层双差,
        /// <summary>
        /// 多历元载波单差
        /// </summary>
        多历元载波单差,
        /// <summary>
        /// 多历元载波无相关单差
        /// </summary>
        多历元载波无相关单差,
        /// <summary>
        /// 多历元载波双差
        /// </summary>
        多历元载波双差,
        /// <summary>
        /// 单历元载波双差
        /// </summary>
        单历元载波双差,
        /// <summary>
        /// 简易单历元载波双差
        /// </summary>
        单历元单频双差,
        /// <summary>
        /// 模糊度固定的单历元纯载波双差
        /// </summary>
        模糊度固定的单历元纯载波双差,

        /// <summary>
        /// 单历元双频载波双差
        /// </summary>
        单历元双频载波双差,
        /// <summary>
        /// 电离层硬件延迟计算
        /// </summary>
        电离层硬件延迟计算,
        电离层延迟变化计算,
        /// <summary>
        /// 单站单历元扩展计算
        /// </summary>
        单站单历元扩展计算,
        /// <summary>
        /// 单站多历元扩展计算
        /// </summary>
        单站多历元扩展计算,
        多站单历元扩展计算,
        多站多历元扩展计算,
        /// <summary>
        /// 单频消电离层组合PPP
        /// </summary>
        单频消电离层组合,
        单站多历元消电离层PPP,
        双频电离层改正单频PPP,
        非差定轨,
        双差定轨,
        简易伪距定轨,
        单历元双频双差,
    }

    /// <summary>
    /// 数据处理选项管理器。
    /// </summary>
    public class GnssProcessOptionManager : BaseDictionary<GnssSolverType, GnssProcessOption>
    {
        private static GnssProcessOptionManager instance = new GnssProcessOptionManager();
        /// <summary>
        ///实例
        /// </summary>
        public static GnssProcessOptionManager Instance => instance;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        private GnssProcessOptionManager()
        {
            this.SetData(CreateDefault());            
        }

        /// <summary>
        /// 新建默认
        /// </summary>
        /// <returns></returns>
        public Dictionary<GnssSolverType, GnssProcessOption> CreateDefault()
        {
            Dictionary<GnssSolverType, GnssProcessOption> result = new Dictionary<GnssSolverType, GnssProcessOption>();
            result.Add(GnssSolverType.模糊度固定的单历元纯载波双差, GnssProcessOption.GetDefaultAmbiFixedEpochDoubleDifferPositioningOption());

            result.Add(GnssSolverType.单历元单频双差, GnssProcessOption.GetDefaultSimpleEpochDoubleDifferPositioningOption());
            result.Add(GnssSolverType.多历元载波单差, GnssProcessOption.GetDefaultDifferPositioningOption());
            result.Add(GnssSolverType.多历元载波双差, GnssProcessOption.GetDefaultPeriodDoubleDifferPositioningOption());
            result.Add(GnssSolverType.多历元载波无相关单差, GnssProcessOption.GetDefaultNoRelevantDifferPositioningOption());
            result.Add(GnssSolverType.单历元载波双差, GnssProcessOption.GetDefaultEpochDoubleDifferPositioningOption());
            result.Add(GnssSolverType.单站多历元扩展计算, GnssProcessOption.GetDefaultSingleFreqPppOption());
            result.Add(GnssSolverType.单频多历元电离层参数化定位, GnssProcessOption.GetDefaultSingleFreqPppOption());
            result.Add(GnssSolverType.单历元双频载波双差, GnssProcessOption.GetDefaultEpochDoubleDueDifferPositioningOption());
            result.Add(GnssSolverType.单历元双频双差, GnssProcessOption.GetDefaultEpochDueFreqDifferPositioningOption());

            result.Add(GnssSolverType.无电离层组合PPP, GnssProcessOption.GetDefaultIonoFreePppOption());
            result.Add(GnssSolverType.递归无电离层组合PPP, GnssProcessOption.GetDefaultRecursiveIonoFreePppOption());
            result.Add(GnssSolverType.对流层增强无电离层PPP, GnssProcessOption.GetDefaultIonoFreePppOption());

            result.Add(GnssSolverType.固定参考站PPP, GnssProcessOption.GetDefaultFixedIonoFreePppOption());
            result.Add(GnssSolverType.最简伪距定位, GnssProcessOption.GetDefaultSimplePseudoRangePositioningOption());
            result.Add(GnssSolverType.多系统伪距定位, GnssProcessOption.GetDefaultPseudoRangePositioningOption());
            result.Add(GnssSolverType.参数化对流层伪距定位, GnssProcessOption.GetDefaultPseudoRangePositioningWithTropOption());


            result.Add(GnssSolverType.多频伪距定位, GnssProcessOption.GetDefaultPseudoRangePositioningOption());
            result.Add(GnssSolverType.动态伪距定位, GnssProcessOption.GetDefaultPseudoRangePositioningOption());
            result.Add(GnssSolverType.无电离层双差, GnssProcessOption.GetDefaultIonoFreeDoubleDifferOption());
            result.Add(GnssSolverType.钟差网解, GnssProcessOption.GetDefaultClockOption());
            result.Add(GnssSolverType.双差网解定位, GnssProcessOption.GetDefaultNetDoubleDifferOption());
            result.Add(GnssSolverType.非差非组合PPP, GnssProcessOption.GetDefaultUncombinedPppOption());
            result.Add(GnssSolverType.单频PPP, GnssProcessOption.GetDefaultSingleFreqPppOption());
            result.Add(GnssSolverType.单频半和PPP, GnssProcessOption.GetDefaultSingleFreqPppOption());
            result.Add(GnssSolverType.电离层模型化单频PPP, GnssProcessOption.GetDefaultIonoModeledSingleFreqPppOption());
            result.Add(GnssSolverType.双频电离层改正单频PPP, GnssProcessOption.GetDefaultIonoModeledSingleFreqPppOption());
            result.Add(GnssSolverType.通用单站单频计算, GnssProcessOption.GetDefaultSingleFreqPppOption());
            result.Add(GnssSolverType.单频消电离层组合, GnssProcessOption.GetDefaultSingleFreqPppOption());
            result.Add(GnssSolverType.单站多历元消电离层PPP, GnssProcessOption.GetDefaultSingleFreqPppOption());
            result.Add(GnssSolverType.单站单历元扩展计算, GnssProcessOption.GetDefaultSingleSiteOption());
            result.Add(GnssSolverType.多站单历元扩展计算, GnssProcessOption.GetDefaultMultiSiteEpochOption());
            result.Add(GnssSolverType.多站多历元扩展计算, GnssProcessOption.GetDefaultPeriodMultiSiteOption());
            result.Add(GnssSolverType.电离层硬件延迟计算, GnssProcessOption.GetDefaultSingleSatOption());
            result.Add(GnssSolverType.电离层延迟变化计算, GnssProcessOption.GetDefaultSinglePeriodSatOption());
            result.Add(GnssSolverType.非差定轨, GnssProcessOption.GetDefaultZeroDifferOrbitOption());
            result.Add(GnssSolverType.简易伪距定轨, GnssProcessOption.GetDefaultSimpleRangeOrbitOption());
            result.Add(GnssSolverType.双差定轨, GnssProcessOption.GetDefaultDoubleDifferOrbitOption());
            result.Add(GnssSolverType.网解双差定位, GnssProcessOption.GetDefaultNetDoublePostionDifferOption());
            result.Add(GnssSolverType.递归网解双差定位, GnssProcessOption.GetDefaultRecursiveNetDoublePostionDifferOption());
            result.Add(GnssSolverType.网解单差定位, GnssProcessOption.GetDefaultNetSinglePostionDifferOption());
            return result;
        }

        public override GnssProcessOption Get(GnssSolverType key)
        {
            if (!this.Contains(key))
            {
                log.Error("当前没有 " + key + ", 的默认设置,采用了默认伪距设置，请前往 " + typeof(GnssProcessOptionManager).Name  + " 提供！");

                return GnssProcessOption.GetDefaultPseudoRangePositioningOption();

            }
            return base.Get(key);
        }

        public GnssProcessOption Get(RinexObsFileFormatType type)
        {
            switch (type)
            {
                case RinexObsFileFormatType.单站单历元: return GnssProcessOption.GetDefaultFormatOption();
                case RinexObsFileFormatType.单站多历元: return GnssProcessOption.GetDefaultFormatOption();
                case RinexObsFileFormatType.多站单历元: return GnssProcessOption.GetDefaultFormatOption();
                case RinexObsFileFormatType.多站多历元: return GnssProcessOption.GetDefaultFormatOption();
                default: return GnssProcessOption.GetDefaultFormatOption();
            }
        }

      
    }
}