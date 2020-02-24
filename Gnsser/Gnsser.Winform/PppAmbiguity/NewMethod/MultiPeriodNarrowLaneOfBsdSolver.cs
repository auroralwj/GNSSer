//2017.03.09, czs, create in hongqing, 多时段窄巷计算器

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Geo;
using Geo.Common;
using Geo.IO;
using Geo.Coordinates;
using Gnsser.Service;
using Geo.Times;
using Gnsser.Core;
using Gnsser.Domain;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Gnsser.Winform
{
    /// <summary>
    /// 多时段窄巷计算器。基于路径。
    /// </summary>
    public class MultiPeriodNarrowLaneOfBsdSolver
    {

        protected ILog log = new Log(typeof(MultiPeriodNarrowLaneOfBsdSolver));

        /// <summary>
        /// 传统构造函数。
        /// </summary>
        /// <param name="pathesOfWLInt"></param>
        /// <param name="pppPathes"></param>
        /// <param name="IsOutputInEachDirectory"></param>
        /// <param name="OutputDirectory"></param>
        public MultiPeriodNarrowLaneOfBsdSolver(string[] pathesOfWLInt, string[] pppPathes, int removeCountOfEachSegment, bool IsOutputInEachDirectory, string OutputDirectory)
        {
            //读取宽巷的整数解 
            //分基准星计算
            this.IsOutputInEachDirectory = IsOutputInEachDirectory;
            this.OutputDirectory = OutputDirectory;
            this.IntOfWLPathes = new Dictionary<SatelliteNumber, List<string>>();
            SatelliteNumber basePrn = new SatelliteNumber();
            foreach (var path in pathesOfWLInt)
            {
                basePrn = SatelliteNumber.Parse(Path.GetFileName(path).Substring(4, 3));
                if (IntOfWLPathes.ContainsKey(basePrn))
                {
                    IntOfWLPathes[basePrn].Add(path);
                }
                else
                {
                    IntOfWLPathes[basePrn] = new List<string>();
                    IntOfWLPathes[basePrn].Add(path);
                }
            }
            this.FloatAmbiguitiesOfPpp = PppTableResultFileReader.ReadPppAmbiResultInCycle(pppPathes);
            //修理浮点解
            FloatAmbiguitiesOfPpp.RemoveStartRowOfEachSegment(removeCountOfEachSegment);
            MaxAllowedDiffer = 0.25;
        }
        /// <summary>
        /// 新方法构造函数
        /// </summary>
        /// <param name="bsdWidelaneInts">不同基准星对应的星间单差宽巷模糊度</param>
        /// <param name="pppPathes"></param>
        /// <param name="IsOutputInEachDirectory"></param>
        /// <param name="OutputDirectory"></param>
        public MultiPeriodNarrowLaneOfBsdSolver(
            BaseDictionary<SatelliteNumber, MultiSitePeriodValueStorage>bsdWidelaneInts, 
            string[] pppPathes, int removeCountOfEachSegment, bool IsOutputInEachDirectory, string OutputDirectory)
        {
            //读取宽巷的整数解 
            //分基准星计算
            this.IsOutputInEachDirectory = IsOutputInEachDirectory;
            this.OutputDirectory = OutputDirectory; 
            this.BsdWidelaneInts = bsdWidelaneInts;
            this.FloatAmbiguitiesOfPpp = PppTableResultFileReader.ReadPppAmbiResultInCycle(pppPathes);


            //修理浮点解
            FloatAmbiguitiesOfPpp.RemoveStartRowOfEachSegment(removeCountOfEachSegment);
            //this.FloatAmbiguitiesOfPpp.RemoveEmptyRows();                                    //删除空行，减少计算量           
            //this.FloatAmbiguitiesOfPpp.RemoveTableDataCountLessThan(MinEpoch, MinSiteCount);  //清理卫星数量过少和测站数量过少的数据


            MultiSitePppEpochValue =  PppTableResultFileReader.ReadToEpochStorage(FloatAmbiguitiesOfPpp);
            MaxAllowedDiffer = 0.25;
            MaxRmsTimes = 2.5;
        }
        MultiSiteEpochValueStorage MultiSitePppEpochValue { get; set; }
        /// <summary>
        /// 不同基准星对应的星间单差宽巷模糊度
        /// </summary>
        BaseDictionary<SatelliteNumber, MultiSitePeriodValueStorage> BsdWidelaneInts { get; set; }
        #region 属性
        /// <summary>
        /// 各基准星单独输出。
        /// </summary>
        public bool IsOutputInEachDirectory { get; set; }
        /// <summary>
        /// 输出文件夹
        /// </summary>
        public string OutputDirectory { get; set; }
        /// <summary>
        /// 最大偏差。
        /// </summary>
        public double MaxAllowedDiffer { get; set; }
        /// <summary>
        /// 宽项的整数分组。
        /// </summary>
        Dictionary<SatelliteNumber, List<string>> IntOfWLPathes { get; set; } 
        /// <summary>
        /// PPP模糊度浮点解，由PPP结果得出。
        /// </summary>
        public ObjectTableManager FloatAmbiguitiesOfPpp { get; set; }

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
        /// 是否循环计算所有卫星
        /// </summary>
        public bool IsLoopAllSats { get; internal set; }
        public int MinSiteCount { get; internal set; }
        public double MaxRmsTimes { get; internal set; }
        public double MaxRms { get; internal set; }
        #endregion

        /// <summary>
        /// 运行。
        /// </summary>
        public void Run()
        {
            var TotalObsCount = 0; 
            if (BsdWidelaneInts != null)//对象差分整数
            {
                log.Info("采用新方法");
                if (MultiSitePppEpochValue != null)
                {
                    Dictionary<SatelliteNumber, MultiSatEpochRmsNumeralStorage> products = new Dictionary<SatelliteNumber, MultiSatEpochRmsNumeralStorage>();
                    foreach (var basePrn in BsdWidelaneInts.Keys)
                    {
                        log.Info("正在计算基准星为 " + basePrn + " 的窄巷模糊度");
                        var intWideLaneObj = BsdWidelaneInts[basePrn];
                        
                        var solver = new EpochBsdOfNarrowLaneSolver(basePrn, MultiSitePppEpochValue, intWideLaneObj, this.OutputDirectory);
                        solver.MaxAllowedDiffer = MaxAllowedDiffer; 
                        solver.MinSiteCount = MinSiteCount; 
                        solver.MaxRmsTimes = MaxRmsTimes; 
                        solver.Run();

                        if (IsOutputFraction && solver.FractionValueTables != null) { solver.FractionValueTables.WriteAllToFileAndClearBuffer(); }
                        if (IsOutputSummary && solver.SummeryTables != null) { solver.SummeryTables.WriteAllToFileAndClearBuffer(); }
                        //if (IsOutputInt && solver.IntValueTables != null) { solver.IntValueTables.WriteAllToFileAndClearBuffer(); }
                        //需要计算所有历元的数值！！！！宽巷也可以，用于查看结果。

                        if (!IsLoopAllSats)
                        {
                            break;
                        }
                        products[basePrn] = solver.FcbProducts;
                    }

                    if (IsLoopAllSats)//汇集产品到一起
                    {
                        //归算到基准星
                        var samePrnProducts = new MultiSiteEpochValueStorage("归算到同一颗卫星");
                         var basePrn = BsdWidelaneInts.Keys.First();
                        foreach (var kv in products)
                        {
                            var name = kv.Key.ToString();
                            if(kv.Key == basePrn)
                            {
                                samePrnProducts[kv.Key.ToString()] = kv.Value;
                            }
                            else
                            {
                                samePrnProducts[name] = kv.Value.GetRawDiffer(basePrn);
                            }  
                        }

                        //汇集各卫星产品到一起
                        var together = samePrnProducts.GetSameSatValues();
                        //计算不准确，算法有待改进！！

                        //求加权平均，每一个历元，每颗卫星只有一个产品,忽略数量太少的历元
                        var FcbProducts = together.GetAverage(1, 3);

                        //生成窄巷FCB 产品，并写入文件
                        var FcbOfUpds = FcbProducts.GetFcbProduct(basePrn);

                        //写入文件  //写入文件
                        FcbOfUpdWriter.WriteEpochProducts(FcbOfUpds, basePrn + "_FinalEpochNLFcbOfDcb"); 
                    }

                }
                else // 次新方法
                { 
                    foreach (var basePrn in BsdWidelaneInts.Keys)
                    {
                        var intWideLaneObj = BsdWidelaneInts[basePrn];
                        var solver = new BsdOfNarrowLaneSolver(basePrn, FloatAmbiguitiesOfPpp, intWideLaneObj, this.OutputDirectory);
                        solver.MaxAllowedDiffer = MaxAllowedDiffer;
                        solver.Run();

                        if (IsOutputFraction) { solver.FractionValueTables.WriteAllToFileAndClearBuffer(); }
                        if (IsOutputSummary) { solver.SummeryTables.WriteAllToFileAndClearBuffer(); }
                        if (IsOutputInt) { solver.IntValueTables.WriteAllToFileAndClearBuffer(); }
                        //需要计算所有历元的数值！！！！宽巷也可以，用于查看结果。

                    }
                }
            }
            else
            {
                log.Info("采用老表格方法");

                foreach (var kv in IntOfWLPathes)
                {
                    ObjectTableManager intWideLane = ObjectTableManager.Read(kv.Value.ToArray());// ReadWideLaneTable(PppResultTableManager);    
                    var basePrn = kv.Key;
                    string outputDirectory = this.OutputDirectory;
                    if (IsOutputInEachDirectory)
                    {
                        var start = intWideLane.GetFirstIndexValue<Time>();
                        var end = intWideLane.GetLastIndexValue<Time>();
                        var span = new TimePeriod(start, end);

                        outputDirectory = System.IO.Path.Combine(this.OutputDirectory, span.ToPathString(false, true, false) + "_" + basePrn);
                        intWideLane.OutputDirectory = outputDirectory;
                        FloatAmbiguitiesOfPpp.OutputDirectory = outputDirectory;
                    }
                    BsdProductSolver solver;


                    //PPP模糊度浮点解 
                    solver = new NarrowLaneOfBsdSolver(basePrn, FloatAmbiguitiesOfPpp, intWideLane, outputDirectory);
                    solver.MaxAllowedDiffer = MaxAllowedDiffer;
                    solver.Run();
                    if (IsOutputFraction) { solver.FractionValueTables.WriteAllToFileAndClearBuffer(); }
                    if (IsOutputSummary) { solver.SummeryTables.WriteAllToFileAndClearBuffer(); }
                    if (IsOutputInt) { solver.IntValueTables.WriteAllToFileAndClearBuffer(); }

                    TotalObsCount += solver.TotalObsCount;
                }

                log.Info(" 总共处理的观测数值数 " + TotalObsCount);
            }
        }

    }
}
