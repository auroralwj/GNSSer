//2017.02.09, czs, create in hongqing, FCB 宽巷计算器
//2017.03.08, czs, edit in hongqing, 差分计算器，剥离了MW提取和平滑出去。
//2017.03.21, czs, edit in hognqing,分离提取BsdProductSolver
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Times;
using Geo.IO;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Gnsser.Winform
{
    /// <summary>
    /// 星间单差计算器
    /// </summary>
    public class BsdProductSolver : IDisposable
    {
        protected ILog log = new Log(typeof(BsdProductSolver));

        public BsdProductSolver(string ProductTypeMarker, SatelliteNumber BasePrn, string OutputDirectory = null)
        {
            this.ProductTypeMarker = ProductTypeMarker;
            this.BasePrn = BasePrn;
            this.OutputDirectory = OutputDirectory ?? Setting.TempDirectory;
            this.SummeryTables = new ObjectTableManager(OutputDirectory);
            this.IngoreRowCountLessThan = 1;//原来为5
            this.IsRemoveCycleSlipedInt = true;
            this.MaxAllowedDiffer = 0.25;
            this.MinSiteCount = 6;
            this.MinEpoch = 20;

            this.NameOfOriginalProduct = BasePrn + "_OriginalProductOf" + ProductTypeMarker;
            this.NameOfOriginalProductRms = BasePrn + "_OriginalProductRmsOf" + ProductTypeMarker;
            this.NameOfCountOfOriginalProduct = BasePrn + "_OriginalProductCountOf" + ProductTypeMarker;
            this.NameOfOriginalSDAmb = BasePrn + "_OriginalSDAmb" + ProductTypeMarker;
        }
        #region 属性
        /// <summary>
        /// 忽略测站数量少于的结果
        /// </summary>
        public int MinSiteCount { get; set; }
        /// <summary>
        /// 忽略历元数量少于的结果
        /// </summary>
        public int MinEpoch { get; set; }
        /// <summary>
        /// 产品类型标记，WL 或 NL。
        /// </summary>
        public string ProductTypeMarker { get; set; }
        /// <summary>
        /// 原始产品的名称
        /// </summary>
        public string NameOfOriginalProduct { get; set; }
        /// <summary>
        /// 参与计算的数量统计，可以分析与RMS的关系。
        /// </summary>
        public string NameOfCountOfOriginalProduct { get; set; }
        /// <summary>
        /// 原始产品RMS的名称
        /// </summary>
        public string NameOfOriginalProductRms { get; set; }
        /// <summary>
        /// 原始星间单差模糊度
        /// </summary>
        public string NameOfOriginalSDAmb { get; set; }
        /// <summary>
        /// 是否移除整型跳变的数据。默认真。
        /// </summary>
        public bool IsRemoveCycleSlipedInt { get; set; }
        /// <summary>
        /// 计算最终产品是，忽略行数量少于指定数的值。
        /// </summary>
        public int IngoreRowCountLessThan { get; set; }
        /// <summary>
        /// 输出目录
        /// </summary>
        public string OutputDirectory { get; set; }

        /// <summary>
        /// 基准卫星
        /// </summary>
        public SatelliteNumber BasePrn { get; set; }
        /// <summary>
        /// 所有的参与计算的观测数据总量。不包含被剔除的数据。
        /// </summary>
        public int TotalObsCount { get; set; }
        #endregion

        #region  产品
        /// <summary>
        /// 一些汇总表格，如结果，末尾数据等。
        /// </summary>
        public ObjectTableManager SummeryTables { get; set; }
        /// <summary>
        /// 网解浮点数部分
        /// </summary>
        public ObjectTableManager FloatValueTables { get; set; }
        /// <summary>
        /// 整数部分数据表
        /// </summary>
        public ObjectTableManager IntValueTables { get; set; }
        /// <summary>
        /// 网解小数部分数据表
        /// </summary>
        public ObjectTableManager FractionValueTables { get; set; }
        /// <summary>
        /// 最大允许的偏差
        /// </summary>
        public double MaxAllowedDiffer { get; set; }

        #endregion

        /// <summary>
        /// 运行
        /// </summary>
        public virtual void Run()
        {

        }

        /// <summary>
        /// 构建产品，前提是星间单差浮点解已经算出。
        /// </summary>
        public virtual void BuildProducts()
        {
            //统计参与计算情况
            this.TotalObsCount = FloatValueTables.GetCountOfNumeralCell();
            log.Info(BasePrn + " 可用计算数为：" + TotalObsCount);

            #region 浮点数、小数、整数 三剑客
            //修理浮点解
            this.FloatValueTables.RemoveEmptyRows();                                    //删除空行，减少计算量           
            this.FloatValueTables.RemoveTableDataCountLessThan(MinEpoch, MinSiteCount);  //清理卫星数量过少和测站数量过少的数据


            //修理小数部分
            this.FractionValueTables = FloatValueTables.GetPeriodPipeFilterTable(1, 0.5, true, "FractionOf" + ProductTypeMarker);//统一小数部分区间
            this.FractionValueTables.PipeFilterWithLastValues(MaxAllowedDiffer);
            this.FractionValueTables.RemoveColWithLastValOffCenter(MaxAllowedDiffer);
            this.FractionValueTables.RemoveTableDataCountLessThan(MinEpoch, MinSiteCount);
            this.FractionValueTables.OutputDirectory = this.OutputDirectory;//更新输出文件，否则将跑到PPP目录中去

            //同步浮点数表与小数表，删除多余
            this.FloatValueTables.SynchronizeTable(this.FractionValueTables);

            //获取整数部分，此处有待考虑部分跳跃情况的处理方式。
            this.IntValueTables = FloatValueTables.GetIntByMinusAndRound(FractionValueTables, "IntOf" + ProductTypeMarker, true);
            this.IntValueTables.RemoveMinorityValueOfEachCol();
            #endregion

            #region 统计与成果输出或更新
            //每个测站最后的数值结果，并进行平均保存到表
            var LastFraction = this.FractionValueTables.GetFractionOfLastValueOfAllCols(BasePrn + "", IngoreRowCountLessThan).Transpose("LastFractionOf" + ProductTypeMarker);
            LastFraction.GetAveragesWithStdDevAndAppendToTable();
            if (LastFraction.IsEmpty) { log.Error(BasePrn + " LastFractionOf" + ProductTypeMarker + " 为空, 退出计算"); return; }
            SummeryTables.Add(LastFraction);

            //统计每个测站每个卫星参与计算的数量，便于与RMS对比分析
            var ValueCountTable = FloatValueTables.GetValidDataCount(m => m > 0, BasePrn + "_EpochCountOf" + ProductTypeMarker).Transpose("", false);
            var sumDic = ValueCountTable.GetAndAppendSumToTable();
            Dictionary<string, double[]> lastAveValues = FractionValueTables.GetAverageValsOfLastRow(IngoreRowCountLessThan);
            var vals = new Dictionary<string, double>();
            var rms = new Dictionary<string, double>();
            foreach (var item in lastAveValues)
            {
                vals[item.Key] = item.Value[0];
                rms[item.Key] = item.Value[1];
            }
            //表格数据的产品输出
            var epoch = (Time)FractionValueTables.GetOne().LastRow["Epoch"];
            AppendToProductTable(NameOfCountOfOriginalProduct, epoch, sumDic); //参与计算的数量
            //AppendToProductTable(NameOfOriginalSDAmb, epoch, sumDic); //参与计算的数量      
            AppendToProductTable(NameOfOriginalProductRms, epoch, rms);                        //产品输出RMS           
            var OriginalProduct = AppendToProductTable(NameOfOriginalProduct, epoch, vals);    //产品输出    

            OriginalProduct.IndexColName = "BasePrn";
            var basesicPrnChain = ObjectTableStorage.GetBasicFractionCascadeTransTable(OriginalProduct, "BasePrn", ProductTypeMarker);

            if (!basesicPrnChain.IsEmpty)
            {
                var nodes = Geo.Utils.ListUtil.GetNoRepeatList<string>(OriginalProduct.GetIndexStrings());
                var basePrnChain = ObjectTableStorage.GetCascadePlusTransferTable(basesicPrnChain, nodes, "CascadePlusOf" + ProductTypeMarker);
                this.SummeryTables.Add(basesicPrnChain);
                this.SummeryTables.Add(basePrnChain);
                //一行式输出,追加到表格
                BuildAndOutputProduct(basePrnChain, BasePrn + "", vals, epoch);
            }
            #endregion
        }

        static object productLocker = new object();
        static object productLocker2 = new object();
        /// <summary>
        /// 更新原始产品。适用于本区模糊度固定。
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="values"></param>
        /// <param name="indexVal"></param>
        protected ObjectTableStorage AppendToProductTable(string tableName, Time indexVal, Dictionary<string, double> values)
        {
            lock (productLocker)
            {
                var product = SummeryTables.ReadOrCreateTable(tableName);

                product.NewRow();
                product.AddItem("Epoch", indexVal);
                product.AddItem("BasePrn", this.BasePrn);
                foreach (var item in values)
                {
                    var prn = item.Key.Substring(0, 3);
                    product.AddItem(prn, item.Value);
                }
                //写入文件，下一次即将读取之
                SummeryTables.WriteTable(product);
                return product;
            }
        }

        /// <summary>
        /// 构建并输出表格式产品，基于同一个基准站的宽项单差产品。其它星间单差可以递推获取。
        /// </summary>
        /// <param name="basePrnChain"></param>
        /// <param name="basePrn"></param>
        /// <param name="mwFractionVals"></param>
        /// <param name="epoch"></param>
        protected void BuildAndOutputProduct(ObjectTableStorage basePrnChain, string basePrn, Dictionary<string, double> mwFractionVals, Time epoch)
        {
            lock (productLocker2)
            {
                //一行决定
                var productName = "ProductOf" + ProductTypeMarker;
                var product = SummeryTables.ReadOrCreateTable(productName);

                //上一个BasePrn,如果没有，采用当前基准星 
                string lastBasePrn = basePrn;
                var avaiableBasePrn = basePrn; //可以使用的最终基准星
                double baseVal = 0;
                if (product.RowCount > 0)//若之前有数据，
                {
                    var lastRow = product.LastRow;//获取上次结果最后一行
                    lastBasePrn = lastRow["BasePrn"] + "";//上一个基准星  
                }

                //获取当前采用的基准星对应最早基准星的差分结果
                var baseDsbValRow = basePrnChain.GetRow(basePrn);
                if (baseDsbValRow != null && baseDsbValRow.ContainsKey(lastBasePrn))
                {
                    var objVal = baseDsbValRow[lastBasePrn];
                    if (Geo.Utils.ObjectUtil.IsNumerial(objVal))
                    {
                        baseVal = Geo.Utils.ObjectUtil.GetNumeral(objVal) % 1;
                        avaiableBasePrn = lastBasePrn;
                    }
                }

                product.NewRow();
                product.AddItem("Epoch", epoch);
                product.AddItem("BasePrn", avaiableBasePrn);

                //归算到一个区间内
                var newMgr = new PeriodPipeFilterManager(1, 0.5);
                newMgr.Init(product);
                foreach (var item in mwFractionVals)
                {
                    var prn = item.Key.Substring(0, 3);
                    var val = newMgr.GetOrCreate(prn).Filter((item.Value + baseVal) % 1);
                    product.AddItem(prn, val);
                }

                //当前行是否包含基准星与上一基准星的差值，如果没有，则采用上一行填充
                var currentRow = product.CurrentRow;
                if (!currentRow.ContainsKey(basePrn))//是否包含当前基准星和上一基准星之差
                {
                    var val = newMgr.GetOrCreate(basePrn).Filter(baseVal);
                    product.AddItem(basePrn, val);
                }
                //写入文件，下一次即将读取之
                SummeryTables.WriteTable(product);
            }
        }

        public virtual void Dispose()
        {
            if (this.SummeryTables != null) { SummeryTables.Dispose(); }
            if (FloatValueTables != null) { FloatValueTables.Dispose(); }
            if (IntValueTables != null) { IntValueTables.Dispose(); }
            if (FractionValueTables != null) { FractionValueTables.Dispose(); }
        }
    }
}