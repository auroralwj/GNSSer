//2017.02.06, czs, create in hongqing, FCB 计算器
//2017.03.21, czs, edit in hognqing,分离提取BsdProductSolver
//2018.09.11, czs, edit in hmx, 重新设计
//2018.09.13, czs, edit in hmx, 重新设计

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Times;
using Geo.IO;

namespace Gnsser.Winform
{
    /// <summary>
    ///  BSD Between Satellite Difference 窄巷， FCB 计算器
    /// </summary>
    public class EpochBsdOfNarrowLaneSolver : BsdProductSolver
    { 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="BasePrn">基准卫星</param>
        /// <param name="FloatAmbiSolution">PPP模糊度浮点解，未做差分,单位为周。</param>
        /// <param name="IntValueOfDifferWL">宽巷MW差分值，整型。</param>
        public EpochBsdOfNarrowLaneSolver(SatelliteNumber BasePrn, MultiSiteEpochValueStorage FloatAmbiSolution, MultiSitePeriodValueStorage IntValueOfDifferWL, string OutputDirectory, bool IsPppAmbiInCycleOrLen = true)
            : base("NL", BasePrn, OutputDirectory)
        {
            this.FloatAmbiSolution = FloatAmbiSolution;
            this.IntValueOfDifferWL = IntValueOfDifferWL;
            this.OutputDirectory = OutputDirectory;
            this.IsPppAmbiInCycleOrLen = IsPppAmbiInCycleOrLen;
            this.MaxRmsTimes = 2.5;
            Init();
        }

        #region 属性 
        /// <summary>
        /// PPP模糊度单位是周（L1频率）还是长度。
        /// </summary>
        public bool IsPppAmbiInCycleOrLen { get; set; }
        /// <summary>
        /// PPP 模糊度浮点解，未做差分,单位为周。
        /// </summary>
        public MultiSiteEpochValueStorage FloatAmbiSolution { get; set; }
        /// <summary>
        /// 宽巷MW差分值，整型。
        /// </summary>
        public MultiSitePeriodValueStorage IntValueOfDifferWL { get; set; } 

        #region 产品

        /// <summary>
        /// 以测站为表的，差分模糊度浮点解，单位为周。列为不同卫星与基准星只差。如 G02-G01，G03-G01
        /// </summary>
        public ObjectTableManager SiteAmbiDiffers { get; set; }
        /// <summary>
        /// 以卫星为表的，差分模糊度浮点解，单位为周。列为各个测站的名称。如 XXXXX.O2O
        /// </summary>
        public ObjectTableManager SatAmbiDiffers { get; set; }

        #endregion

        #endregion
        /// <summary>
        /// 之于浮模糊度点解的无电离层乘法因子。
        /// 对于特定系统这是一个常量。应该提前赋值。2016.10.20, czs, 
        /// 这是浮点解除以L1频率波长后，单位为周的乘法因子。
        /// </summary>
        public double FloatAmbiguityL1CycleMultiFactor { get; private set; }
        /// <summary>
        /// 如果PPP模糊度采用的是长度单位，则采用此因子。可以与周单位相互校验。
        /// </summary>
        public double FloatAmbiguityLenMultiFactor { get; private set; }
        /// <summary>
        /// 之于宽项模糊度整数解的乘法因子。
        /// 对于特定系统这是一个常量。应该提前赋值。
        /// 2016.10.20, czs 
        /// </summary>
        public double WideLaneMultiFactor { get; private set; }
        /// <summary>
        /// 最大允许RMS倍数
        /// </summary>
        public double MaxRmsTimes { get;  set; }
        /// <summary>
        /// FCB 历元产品
        /// </summary>
        public List<FcbOfUpd> FcbOfUpds { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        public MultiSatEpochRmsNumeralStorage FcbProducts { get; set; }
        public void Init()
        {
            /// 第一频率
            Frequence f1 = Frequence.GetFrequence(BasePrn.SatelliteType, 1);
            /// 第二频率
            Frequence f2 = Frequence.GetFrequence(BasePrn.SatelliteType, 2);
            this.FloatAmbiguityL1CycleMultiFactor = (f1.Value + f2.Value) / f1.Value;
            this.FloatAmbiguityLenMultiFactor = (f1.Value + f2.Value) * 1e6 / GnssConst.LIGHT_SPEED;
            this.WideLaneMultiFactor = f2.Value / (f1.Value - f2.Value);
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <returns></returns>
        public override void Run()
        {
            //各个测站做星间单差 //获取星间单差值
            MultiSiteEpochValueStorage rawDiffer = FloatAmbiSolution.GetRawDiffer(BasePrn);

            //与宽巷模糊度整数产品一起，计算窄巷模糊度的浮点解
            MultiSiteEpochValueStorage floatNlVals = rawDiffer.GetNarrowLaneFcbs(IntValueOfDifferWL, GetNarrowLaneValue);

            //浮点数小数部分
            MultiSiteEpochValueStorage fractionOfNL = floatNlVals.GetRoundFraction();

            //合并相同卫星的浮点数小数部分
            EpochSatSiteValueList epochSatValues = fractionOfNL.GetSameSatValues();

            //求加权平均，每一个历元，每颗卫星只有一个产品,忽略数量太少的历元
            this.FcbProducts = epochSatValues.GetAverage(this.MinSiteCount, MaxRmsTimes);

            //生成窄巷FCB 产品，并写入文件
            this.FcbOfUpds = FcbProducts.GetFcbProduct(BasePrn);

            //写入文件
            FcbOfUpdWriter.WriteEpochProducts(FcbOfUpds, BasePrn + "_EpochNLFcbOfDcb");


            //显示逐历元结果,表名为卫星，各表列为测站名称
            this.FloatValueTables = epochSatValues.GetTable(this.OutputDirectory, BasePrn);

            //生成逐历元表格文件
            base.BuildProducts();
        }


        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            if (this.SiteAmbiDiffers != null) { SiteAmbiDiffers.Dispose(); }
            if (this.SatAmbiDiffers != null) { SatAmbiDiffers.Dispose(); }
            base.Dispose();
        }


        #region 窄巷计算相关参数

        /// <summary>
        /// 计算窄巷模糊度浮点解
        /// </summary>
        /// <param name="FloatAmbiguityDiffer"></param>
        /// <param name="wideLaneInteger"></param>
        /// <returns></returns>
        public double GetNarrowLaneValue(double FloatAmbiguityDiffer, int wideLaneInteger)
        {
            //求乘数因子
            double narrowLane = 0;
            if (IsPppAmbiInCycleOrLen)
            {
                narrowLane = (FloatAmbiguityL1CycleMultiFactor * FloatAmbiguityDiffer
                    - WideLaneMultiFactor * wideLaneInteger);
            }
            else
            {
                narrowLane =
                    (FloatAmbiguityLenMultiFactor * FloatAmbiguityDiffer
                    - WideLaneMultiFactor * wideLaneInteger);
            }
            return narrowLane;
        }

        /// <summary>
        /// 计算窄巷模糊度浮点解
        /// </summary>
        /// <param name="FloatAmbiguityDiffer"></param>
        /// <param name="wideLaneInteger"></param>
        /// <returns></returns>
        public RmsedNumeral GetNarrowLaneValue(RmsedNumeral FloatAmbiguityDiffer, RmsedNumeral wideLaneInteger)
        {
            //求乘数因子
            RmsedNumeral narrowLane = RmsedNumeral.Zero;
            if (IsPppAmbiInCycleOrLen)
            {
                narrowLane = (FloatAmbiguityL1CycleMultiFactor * FloatAmbiguityDiffer
                    - WideLaneMultiFactor * wideLaneInteger);
            }
            else
            {
                narrowLane =
                    (FloatAmbiguityLenMultiFactor * FloatAmbiguityDiffer
                    - WideLaneMultiFactor * wideLaneInteger);
            }
            return narrowLane;
        }
        #endregion
    }
}