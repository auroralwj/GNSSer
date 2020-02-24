//2016.04.23, czs, create in hongqing, 产品计算器接口
//2017.09.18, czs, edit in hongqing, 增加单站多历元GNSS计算
//2018.05.15, czs, create in hmx, 电离层硬件延迟计算
//2018.06.04, czs, edit in hmx, 增加单站单星多历元GNSS计算

using System;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo;

namespace Gnsser
{

    /// <summary>
    /// GNSS 计算器生成工厂
    /// </summary>
    public class GnssSolverFactory : AbstractBuilder<IGnssSolver>{

        /// <summary>
        /// 数据上下文
        /// </summary>
        public DataSourceContext DataSourceContext { get; set; }
        /// <summary>
        /// 选项
        /// </summary>
        public GnssProcessOption GnssOption { get; set; }
        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        public override IGnssSolver Build()
        {
            return Create(DataSourceContext, GnssOption);
        }

        /// <summary>
        /// 生成GNSS计算器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="DataSourceContext"></param>
        /// <param name="GnssOption"></param>
        /// <returns></returns>
        public static IGnssSolver Create(DataSourceContext DataSourceContext, GnssProcessOption GnssOption)
        {
            IGnssSolver Solver = null;
            SimpleBaseGnssMatrixBuilder matrix = null;
            switch (GnssOption.GnssSolverType)
            {
                case GnssSolverType.双差网解定位:
                    Solver = new NetDoubleDifferPositioner(DataSourceContext, GnssOption); 
                    break;
                case GnssSolverType.网解双差定位:
                    Solver = new NetDoubleDifferPositionSolver(DataSourceContext, GnssOption); 
                    break;
                case GnssSolverType.递归网解双差定位:
                    Solver = new RecursiveNetDoubleDifferPositionSolver(DataSourceContext, GnssOption); 
                    break;
                case GnssSolverType.网解单差定位:
                    Solver = new NetSingleDifferPositionSolver(DataSourceContext, GnssOption); 
                    break;
                case GnssSolverType.钟差网解:
                    Solver = new IonoFreeClockEstimationer(DataSourceContext, GnssOption);
                    GnssOption.IsBaseSatelliteRequried = false;
                    GnssOption.IsRequireSameSats = false;
                    break;
                case GnssSolverType.无电离层双差:
                    GnssOption.IsBaseSatelliteRequried = true;
                    Solver = new IonFreeDoubleDifferPositioner(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.多历元载波单差:
                    GnssOption.IsBaseSatelliteRequried = true;
                    Solver = new SingleDifferPositioner(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.单历元载波双差:
                    GnssOption.IsBaseSatelliteRequried = true;
                    Solver = new EpochDoublePhaseDifferPositioner(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.单历元双频双差:
                    GnssOption.IsBaseSatelliteRequried = true;
                    Solver = new EpochDualFreqDoubleDifferPositioner(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.单历元单频双差:
                    GnssOption.IsBaseSatelliteRequried = true;
                    Solver = new EpochDoubleDifferPositioner(DataSourceContext, GnssOption);                  
                    break;
                //case GnssSolverType.单历元载波双差:
                //    GnssOption.IsBaseSatelliteRequried = true;
                //    Solver = new EpochDoublePhaseDifferPositioner(DataSourceContext, GnssOption);                  
                //    break;
                case GnssSolverType.模糊度固定的单历元纯载波双差:
                    GnssOption.IsBaseSatelliteRequried = true;
                    Solver = new AmbiFixedEpochDoublePhaseOnlytDifferPositioner(DataSourceContext, GnssOption);                  
                    break;
                case GnssSolverType.单历元双频载波双差:
                    GnssOption.IsBaseSatelliteRequried = true;
                    Solver = new EpochDouFreDoubleDifferPositioner(DataSourceContext, GnssOption);                  
                    break;
                case GnssSolverType.多历元载波无相关单差:
                    GnssOption.IsBaseSatelliteRequried = true;
                    Solver = new Gnsser.Service.SingleDifferNoRelevantPositioner(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.多历元载波双差:
                    GnssOption.IsBaseSatelliteRequried = true;
                    Solver = new Gnsser.Service.PeriodDoublePhaseDifferPositioner(DataSourceContext, GnssOption);
                    break; 
                case GnssSolverType.最简伪距定位:
                    Solver = new SimpleRangePositioner(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.多频伪距定位:
                    Solver = new MultiFreqRangePositioner(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.递归无电离层组合PPP:
                    Solver = new RecursiveIonoFreePpp(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.动态伪距定位:
                    Solver = new DynamicRangePositioner(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.多系统伪距定位:
                    Solver = new MultiSysRangePositioner(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.参数化对流层伪距定位:
                    Solver = new RangePositionWithTropParamSolver(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.无电离层组合PPP:
                    Solver = new IonoFreePpp(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.对流层增强无电离层PPP:
                    Solver = new TropAugIonoFreePpp(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.电离层模型化单频PPP:
                    Solver = new IonoModeledSingleFreqPpp(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.固定参考站PPP:
                    Solver = new SiteFixedIonoFreePpp(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.非差非组合PPP:
                    Solver = new UncombinedPpp(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.单频PPP:
                    Solver = new SingleFreqPpp(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.单频半和PPP:
                    Solver = new HalfSumSingeFreqSolver(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.通用单站单频计算:
                    Solver = new CommonSingeFreqGnssSolver(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.双频电离层改正单频PPP:
                    Solver = new DoubleFreqIonoSingleFreqPppSolver(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.单频消电离层组合:
                    Solver = new SingleFreqIonoFreePppSolver(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.单站多历元消电离层PPP:
                    Solver = new SingleSitePeriodIonoFreePppSolver(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.电离层硬件延迟计算:
                    matrix = new IonoHardwareDelaySolveMatrixBuilder(GnssOption);
                    Solver = new CommonSingeSatGnssSolver(DataSourceContext, GnssOption, (SingleSiteSingleSatGnssMatrixBuilder)matrix);
                    break;
                case GnssSolverType.电离层延迟变化计算:
                    matrix = new IonoDeltaSolveMatrixBuilder(GnssOption);
                    Solver = new CommonSingePeriodSatGnssSolver(DataSourceContext, GnssOption, (SingleSiteSinglePeriodSatGnssMatrixBuilder)matrix);
                    break;
                case GnssSolverType.单站单历元扩展计算:
                    Solver = new SingeSiteGnssExtentSolver(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.单站多历元扩展计算:
                    Solver = new SingleSitePeriodGnssExtentSolver(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.单频多历元电离层参数化定位:
                    Solver = new SingleSitePeriodParamedIonoSolver(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.非差定轨:
                    Solver = new ZeroDifferOrbitSolver(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.简易伪距定轨:
                    Solver = new RangeOrbitSolver(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.双差定轨:
                    Solver = new DoubleDifferOrbitSolver(DataSourceContext, GnssOption);
                    break;
                case GnssSolverType.多站单历元扩展计算:
                    Solver = new MultiSiteGnssExtentPositioner(DataSourceContext, GnssOption);
                    break;

                case GnssSolverType.多站多历元扩展计算:
                    Solver = new PeriodMultiSiteGnssExtentSolver(DataSourceContext, GnssOption);
                    break;

                default:
                    Solver = new IonoFreePpp(DataSourceContext, GnssOption);
                    break;
            }
            return Solver;
        }
         


    } 
     
}
