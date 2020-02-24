//2017.03.09, czs, create in hongqing, 分时段卫宽项计算器
//2017.03.24, czs, edit in hongqing, 分时段宽巷和窄巷自动计算器

using System;
using System.Collections.Generic;
using System.Linq;
using Geo;
using Geo.Common;
using Geo.IO;
using Geo.Coordinates;

using Geo.Times;
using Gnsser.Core;
using Gnsser.Domain;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Gnsser.Winform
{
    /// <summary>
    /// 分时段宽巷和窄巷计算器
    /// </summary>
    public class MultiPeriodBsdProductSolver
    {
        Log log = new Log(typeof ( MultiPeriodWideLaneOfBsdSolver ));

        /// <summary>
        /// 构造函数。基于文件路径。
        /// </summary>
        /// <param name="pppResult"></param>
        /// <param name="obsPathes"></param>
        /// <param name="periodPrnPath"></param>
        public MultiPeriodBsdProductSolver(string[] pppResult, string[] obsPathes, string periodPrnPath)
            : this(PppTableResultFileReader.ReadPppAmbiResultInCycle(pppResult), ObjectTableManager.Read(obsPathes), PeriodPrnManager.ReadFromFile(periodPrnPath))
        { }

        /// <summary>
        /// 构造函数。基于表对象。
        /// </summary>
        /// <param name="SmoothedMwValue"></param>
        /// <param name="PeriodPrnManager"></param>
        /// <param name="OutputDirectory"></param>
        public MultiPeriodBsdProductSolver(ObjectTableManager FloatAmbiguitiesOfPpp, ObjectTableManager SmoothedMwValue, PeriodPrnManager PeriodPrnManager, int minSite = 3, int minEpoch = 10, string OutputDirectory = null)
        {
            MaxAllowedDiffer = 0.25;
            this.MinSiteCount  = minSite;
            this.MinEpoch = minEpoch;
            this.FloatAmbiguitiesOfPpp = FloatAmbiguitiesOfPpp;
            this.PeriodPrnManager = PeriodPrnManager; 
            this.OutputDirectory =  (OutputDirectory != null)? OutputDirectory : Setting.TempDirectory; 
            this.SmoothedMwValue = SmoothedMwValue; 
        }

        #region 属性
        /// <summary>
        /// PPP模糊度浮点解，由PPP结果得出。
        /// </summary>
        public ObjectTableManager FloatAmbiguitiesOfPpp { get; set; }
        /// <summary>
        /// 忽略测站数量少于的结果
        /// </summary>
        public int MinSiteCount  { get; set; }
        /// <summary>
        /// 忽略历元数量少于的结果
        /// </summary>
        public int MinEpoch { get; set; } 
        /// <summary>
        /// 输出目录
        /// </summary>
        public string OutputDirectory { get; set; }
        /// <summary>
        /// 核心输入数据，平滑后的MW数值。
        /// </summary>
        public ObjectTableManager SmoothedMwValue { get; set; }
        /// 分时段卫星编号记录器。
        /// </summary>
        public PeriodPrnManager PeriodPrnManager { get; set; }
        /// <summary>
        /// 是否输出整数部分
        /// </summary>
        public bool IsOutputInt { get; set; }
        /// <summary>
        /// 是否输出小数部分
        /// </summary>
        public bool IsOutputFraction { get; set; }
        /// <summary>
        /// 是否输出汇总文件
        /// </summary>
        public bool IsOutputSummary { get; set; }
        /// <summary>
        /// 所有观测数的统计
        /// </summary>
        public int TotalObsCountOfWL { get; set; }
        /// <summary>
        /// 所有观测数的统计
        /// </summary>
        public int TotalObsCountOfNL { get; set; }
        #endregion

        /// <summary>
        /// 运行
        /// </summary>
        public void Run()
        {
            TotalObsCountOfWL = 0;
            TotalObsCountOfNL = 0;
            SmoothedMwValue.FillIndexes();//保证可以检索。
            //首先进行分段
            Parallel.ForEach(PeriodPrnManager, item =>
            //foreach (var key in PeriodPrnManager)
            {
                var span = item.TimePeriod;
                var basePrn = item.Value;
                if (basePrn.SatelliteType == SatelliteType.U)
                {
                    log.Error("卫星类未定！");
                    throw new Exception("卫星类未定！");
                }
                var sectionMw = SmoothedMwValue.GetSub(span.StartDateTime, span.EndDateTime);
               
                ProcessOneBaseSat(sectionMw, basePrn);
               
            });
            log.Info(" 总共处理的观测数值数 " + TotalObsCountOfWL);
        }
        /// <summary>
        /// 处理一个卫星
        /// </summary>
        /// <param name="sectionMw"></param>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        private void ProcessOneBaseSat(ObjectTableManager sectionMw, SatelliteNumber basePrn)
        {
            using (var wLSolver = new WideLaneOfBsdSolver(sectionMw, basePrn, MinSiteCount , MinEpoch, this.OutputDirectory))
            {
                wLSolver.MaxAllowedDiffer = MaxAllowedDiffer;
                wLSolver.Run();

                using (var nLSolver = new NarrowLaneOfBsdSolver(basePrn, FloatAmbiguitiesOfPpp, wLSolver.IntValueTables, this.OutputDirectory))
                {
                    nLSolver.MinEpoch = MinEpoch;
                    nLSolver.MinSiteCount = MinSiteCount ;
                    nLSolver.MaxAllowedDiffer = MaxAllowedDiffer;
                    nLSolver.Run();

                    TotalObsCountOfNL += nLSolver.TotalObsCount;

                    if (this.IsOutputInt && nLSolver.IntValueTables != null) { nLSolver.IntValueTables.WriteAllToFileAndClearBuffer(); }
                    if (this.IsOutputFraction && nLSolver.FractionValueTables != null) { nLSolver.FractionValueTables.WriteAllToFileAndClearBuffer(); }
                    if (this.IsOutputSummary && nLSolver.SummeryTables != null) { nLSolver.SummeryTables.WriteAllToFileAndClearBuffer(); }
                }
                
                //输出
                if (this.IsOutputInt && wLSolver.IntValueTables != null) { wLSolver.IntValueTables.WriteAllToFileAndClearBuffer(); }
                if (this.IsOutputFraction && wLSolver.FractionValueTables != null) { wLSolver.FractionValueTables.WriteAllToFileAndClearBuffer(); }
                if (this.IsOutputSummary && wLSolver.SummeryTables != null) { wLSolver.SummeryTables.WriteAllToFileAndClearBuffer(); }

                TotalObsCountOfWL += wLSolver.TotalObsCount;
            }
        }

        public double MaxAllowedDiffer { get; set; }
    }
}
