//2017.03.09, czs, create in hongqing, 分时段卫宽项计算器


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
    /// 分时段卫宽项计算器
    /// </summary>
    public class MultiPeriodWideLaneOfBsdSolver
    {
        Log log = new Log(typeof ( MultiPeriodWideLaneOfBsdSolver )); 
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="SmoothedMwValue"></param>
        /// <param name="PeriodPrnManager"></param>
        /// <param name="OutputDirectory"></param>
        public MultiPeriodWideLaneOfBsdSolver(ObjectTableManager SmoothedMwValue, PeriodPrnManager PeriodPrnManager,int minSat = 1, int minEpoch=1, bool IsOutputInEachDirectory =false, string OutputDirectory = null)
        { 
            this.MinSatCount = minSat;
            this.MinEpoch = minEpoch; 
            this.PeriodPrnManager = PeriodPrnManager;
            if (OutputDirectory != null)
            {
                this.OutputDirectory = OutputDirectory;
            }
            else
            {
                this.OutputDirectory = Setting.TempDirectory;
            }
            this.SmoothedMwValue = SmoothedMwValue;
            this.IsOutputInEachDirectory = IsOutputInEachDirectory;
        }

        #region  属性
        /// <summary>
        /// 忽略卫星数量少于的结果
        /// </summary>
        public int MinSatCount { get; set; }
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
        public int TotalObsCount { get; set; }
        /// <summary>
        /// 单独目录输出
        /// </summary>
        public bool IsOutputInEachDirectory { get; set; }
        static object locker = new object();
        #endregion

        /// <summary>
        /// 运行
        /// </summary>
        public void Run()
        {
            TotalObsCount = 0;
            SmoothedMwValue.FillIndexes();//保证可以检索。
            //首先进行分段
            Parallel.ForEach(PeriodPrnManager, item =>
           // foreach (var key in PeriodPrnManager)
            {
                var span = item.TimePeriod;
                var basePrn = item.Value;
                if (basePrn.SatelliteType == SatelliteType.U)
                {
                    log.Error("卫星类未定！");
                    throw new Exception("卫星类未定！");
                }
                var sectionMw = SmoothedMwValue.GetSub(span.Start, span.End);

                string outputDirectory = this.OutputDirectory;
                if (IsOutputInEachDirectory)
                {
                    outputDirectory = System.IO.Path.Combine(this.OutputDirectory, span.ToPathString(false, true, false) + "_" + basePrn);
                    sectionMw.OutputDirectory = outputDirectory;
                }
                var count = ProcessOneBaseSat(sectionMw, basePrn, outputDirectory);
                TotalObsCount += count;
            });
            log.Info(" 总共处理的观测数值数 " + TotalObsCount);
        }
        /// <summary>
        /// 处理一个卫星
        /// </summary>
        /// <param name="sectionMw"></param>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        private int ProcessOneBaseSat(ObjectTableManager sectionMw, SatelliteNumber basePrn, string outputDirectory)
        {
            using (var solver = new WideLaneOfBsdSolver(sectionMw, basePrn, MinSatCount, MinEpoch, outputDirectory))
            {
                solver.Run();

                //输出
                if (this.IsOutputInt && solver.IntValueTables != null) { solver.IntValueTables.WriteAllToFileAndClearBuffer(); }
                if (this.IsOutputFraction && solver.FractionValueTables != null) { solver.FractionValueTables.WriteAllToFileAndClearBuffer(); }
                if (this.IsOutputSummary && solver.SummeryTables != null) { solver.SummeryTables.WriteAllToFileAndClearBuffer(); }
                return solver.TotalObsCount;
            }
        }
    }
}
