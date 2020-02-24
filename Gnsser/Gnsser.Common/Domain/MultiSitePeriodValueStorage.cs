//2017.02.09, czs, create in hongqing, FCB 宽巷计算器
//2017.03.08, czs, edit in hongqing, MwTableBuilder单独提出，便于后续组建产品
//2018.08.30, czs, edit in hmx,去除卫星高度角文件，以星历服务代替，DCB改正适应所有日期
//2018.09.02, czs, create in hmx, 全球测站MW快速提取。
//2018.09.09, czs, create in hmx, 测站时段数据存储。

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
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Gnsser
{
    /// <summary>
    /// 测站时段数据存储
    /// </summary>
    public class MultiSitePeriodValueStorage : BaseConcurrentDictionary<string, MultiSatPeriodRmsNumeralStorage>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="name"></param>
        public MultiSitePeriodValueStorage(string name)
        {
            this.Name = name;
            log.MsgProducer = this.GetType(); 
        }
        public override MultiSatPeriodRmsNumeralStorage Create(string key)
        {
            return new MultiSatPeriodRmsNumeralStorage(key);
        } 
        /// <summary>
        /// 所有卫星的差分产品，计算一次即存储。第二次访问直接获取。
        /// </summary>
        public Dictionary<SatelliteNumber, Dictionary<SatelliteNumber, RmsedNumeral>> ProductsOfAllDiffer { get; set; }
        /// <summary>
        /// 历元时段
        /// </summary>
        public TimePeriod TimePeriod
        {
            get
            {
                TimePeriod timePeriod = null;
                foreach (var item in this)
                {
                    if(item.Count  == 0) { continue; }
                    if(timePeriod == null)
                    {
                        timePeriod = new TimePeriod(item.TimePeriod);
                    }
                    else
                    {
                        timePeriod = timePeriod.Exppand(item.TimePeriod);
                    }
                }
                return timePeriod;
            }
        }

        /// <summary>
        /// 时段小数表。一行为一个测站，所有卫星的平均值的小数部分。
        /// </summary>
        public ObjectTableStorage FractionTable { get; set; }
        /// <summary>
        /// 详细表,完整的信息都包含于此，所有产品皆可由此衍生.
        /// 一行为一个测站，一个时段的值。
        /// </summary>
        public ObjectTableStorage DetailTable { get; set; }
        /// <summary>
        /// 获取数值
        /// </summary>
        /// <param name="site"></param>
        /// <param name="prn"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public RmsedNumeral  GetValue(string site, SatelliteNumber prn, Time time)
        {
            if (!this.Contains(site)) { return null; }
            return this[site].GetValue(prn, time);            
        }

        /// <summary>
        /// 移除RMS大于此的数据
        /// </summary>
        /// <param name="maxRms"></param>
        public void RemoveRmsGreaterThan(double maxRms)
        {
            foreach (var item in this)
            {
                item.RemoveRmsGreaterThan(maxRms);
            }
        }

        #region 宽巷差分整数生成
        /// <summary>
        /// 所有可能的差分整数，所有卫星都做一遍基准星
        /// </summary>
        /// <returns></returns>
        public BaseDictionary<SatelliteNumber, MultiSitePeriodValueStorage> GetAllPossibleDifferInts()
        {    
            var data = new BaseConcurrentDictionary<SatelliteNumber, MultiSitePeriodValueStorage>();
            List<SatelliteNumber> prns = GetAllPrns();
            if (true)
            {
                log.Info("即将并行宽巷模糊度固定，请系好安全带！");
                Parallel.ForEach(prns, new Action<SatelliteNumber>(prn => data[prn] = GetDifferInt(prn)));
            }
            else  {  foreach (var prn in prns)  {  data[prn] = GetDifferInt(prn);  }  }
            return new BaseDictionary<SatelliteNumber, MultiSitePeriodValueStorage>(data.Data);
        }

        /// <summary>
        /// 返回宽巷差分后的整数部分，用于固定模糊度，注意区分时段。
        /// 如果不含基准卫星，返回null
        /// </summary>
        /// <param name="basePrn">基准卫星</param>
        /// <param name="maxDifferForAmbi">允许的最大偏差</param>
        /// <param name="maxRms">允许的最大的RMS</param>
        /// <returns></returns>
        public MultiSitePeriodValueStorage GetDifferInt(SatelliteNumber basePrn, double maxDifferForAmbi=0.35, double maxRms =0.8)
        {
            log.Info("开始生成星间单差模糊度，基准星：" + basePrn);
            //宽巷小数产品
            var fracProducts = GetDifferFractionProduct();    //包括所有基准星的差分产品
            if (!fracProducts.ContainsKey(basePrn)) { log.Debug ("产品不包含基准卫星！" + basePrn); return null; }
            var fractions = fracProducts[basePrn];           //当前基准星，星间单差的小数部分， 
         
            MultiSitePeriodValueStorage floatRawDiffer = GetRawDiffer(basePrn);                             //星间差分宽巷浮点数
            MultiSitePeriodValueStorage result = floatRawDiffer.GetRoundInt(fractions, maxDifferForAmbi, maxRms);  //星间差分宽巷整数

            log.Info("星间单差模糊度生成完毕，基准星：" + basePrn);
            return result;
        }

        /// <summary>
        /// 四舍五入法固定模糊度
        /// </summary>
        /// <param name="maxDifferForAmbi">允许的最大偏差</param>
        /// <param name="maxRms">允许的最大的RMS</param>
        /// <param name="fractions">小数部分产品</param>
        /// <returns></returns>
        public MultiSitePeriodValueStorage GetRoundInt(Dictionary<SatelliteNumber, RmsedNumeral> fractions, double maxDifferForAmbi, double maxRms)
        {
            MultiSitePeriodValueStorage result = new MultiSitePeriodValueStorage("所有站宽巷星间单差整数模糊度");
            foreach (var siteKv in this.Data) //遍历计算所有测站
            {
                string siteName = siteKv.Key;
                var periodVals = siteKv.Value;
                MultiSatPeriodRmsNumeralStorage newSite = periodVals.GetRoundInt(siteName, fractions, maxDifferForAmbi, maxRms);
                if (newSite.Count > 0)
                {
                    result.Add(newSite.Name, newSite);
                }
            }
            return result;
        }
        
        #region  数据原始差分
        BaseDictionary<SatelliteNumber, MultiSitePeriodValueStorage> CasheOfRawDiffers = new BaseDictionary<SatelliteNumber, MultiSitePeriodValueStorage>();
        static object rawDifferLocker = new object();
        /// <summary>
        /// 所有测站与指定参考星的差分值（浮点数）。
        /// 必须与测站共视才可以差分。结果中包括了共视时段。
        /// </summary>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public MultiSitePeriodValueStorage GetRawDiffer(SatelliteNumber basePrn)
        {
            if (CasheOfRawDiffers.Contains(basePrn))
            {
                return CasheOfRawDiffers[basePrn];
            }
            lock (rawDifferLocker)
            {
                if (CasheOfRawDiffers.Contains(basePrn))
                {
                    return CasheOfRawDiffers[basePrn];
                }

                log.Info("开始生成 星间单差浮点数，基准星：" + basePrn);
                MultiSitePeriodValueStorage sitePeriodDifferFloat = new MultiSitePeriodValueStorage("所有站星间单差宽巷浮点数模糊度(MW)");
                foreach (var siteKv in this.Data)
                {
                    if (!siteKv.Value.Contains(basePrn)) { continue; }
                    string siteName = siteKv.Key;
                    var periodVals = siteKv.Value;
                    MultiSatPeriodRmsNumeralStorage newSite = periodVals.GetRawDiffer(basePrn);
                    sitePeriodDifferFloat.Add(siteName, newSite);
                }
                log.Info("星间单差浮点数生成完毕，基准星：" + basePrn);

                CasheOfRawDiffers[basePrn] = sitePeriodDifferFloat;

                return sitePeriodDifferFloat;
            }
        }
        #endregion

        #endregion

        #region 历元产品计算
        /// <summary>
        /// 将所有卫星产品归算到一颗卫星上面。
        /// </summary>
        /// <param name="basePrn"></param>
        /// <param name="intervalSeconds"></param>
        /// <param name="maxRms"></param>
        /// <returns></returns>
        public List<FcbOfUpd> GetWideLaneFcbOfAllSatAverage(SatelliteNumber basePrn, double intervalSeconds = 30, double maxRms = 0.6)
        {
            var timePeriod = this.TimePeriod;
            var list = new List<FcbOfUpd>();
            for (Time epoch = timePeriod.Start; epoch <= timePeriod.End; epoch += intervalSeconds)
            {
                FcbOfUpd fcb = GetWideLaneFcbOfAllSat(basePrn, epoch, maxRms);

                list.Add(fcb);
            }
            return list;
        }

        /// <summary>
        /// 将所有卫星产品归算到一颗卫星上面
        /// </summary>
        /// <param name="basePrn"></param>
        /// <param name="maxRms"></param>
        /// <param name="epoch"></param>
        /// <returns></returns>
        public FcbOfUpd GetWideLaneFcbOfAllSat(SatelliteNumber basePrn, Time epoch, double maxRms)
        {
            //首先，所有的都计算一次
            var prns = this.GetAllPrns();
            var data = new BaseDictionary<SatelliteNumber, List<RmsedNumeral>>("数据集合", (prn) => new List<RmsedNumeral>());
            foreach (var prn in prns)
            {
                //计算以当前卫星为基准的差分数据
                Dictionary<SatelliteNumber, RmsedNumeral> fcbDic = GetWideLaneFcbDic(prn, epoch, maxRms);
                if (!fcbDic.ContainsKey(basePrn)) { continue; } // 没有基准星，则不考虑

                var baseVal = fcbDic[basePrn];
                //其次，归算到指定的卫星
                foreach (var item in fcbDic)
                {
                    var currentPrn = item.Key;
                    var newVal = item.Value - baseVal;
                    newVal.Value = Geo.Utils.DoubleUtil.GetRoundFraction(newVal.Value);
                    data.GetOrCreate(currentPrn).Add(newVal);
                }
            }

            //加权平均
            var ave = Geo.Utils.DoubleUtil.GetRoundFractionAverageValues(data);
            FcbOfUpd fcb = new FcbOfUpd(basePrn, epoch, ave, true);
            return fcb;
        }

        /// <summary>
        /// 返回所有,然后再加权平均，这样可以获得更可靠的结果！
        /// </summary>
        /// <param name="intervalSeconds"></param>
        /// <returns></returns>
        public Dictionary<SatelliteNumber, List<FcbOfUpd>> GetWideLaneFcb(double intervalSeconds = 30, double maxRms = 0.6)
        {
            var data = new Dictionary<SatelliteNumber, List<FcbOfUpd>>();
            var prns = this.GetAllPrns();
            foreach (var basePrn in prns)
            {
                data[basePrn] = GetWideLaneFcb(basePrn, intervalSeconds, maxRms);
            } 
            return data;
        }


        /// <summary>
        /// 按照指定的间隔，返回当前时间范围内的所有计算值。
        /// 仅单个卫星，匹配不上的不参与计算，造成了浪费。
        /// </summary>
        /// <param name="basePrn">基准星</param>
        /// <param name="intervalSeconds"></param>
        /// <returns></returns>
        public List<FcbOfUpd> GetWideLaneFcb(SatelliteNumber basePrn, double intervalSeconds = 30, double maxRms = 0.6)
        {
            var timePeriod = this.TimePeriod;
            var list = new List<FcbOfUpd>();
            for (Time epoch = timePeriod.Start; epoch <= timePeriod.End; epoch+= intervalSeconds)
            {
                var fcb = GetWideLaneFcb(basePrn, epoch, maxRms);
                if (fcb != null)
                {
                    list.Add(fcb);
                }
            }
            return list;
        }

        /// <summary>
        /// 当前历元产品，为所在时段的平均。
        /// </summary>
        /// <param name="basePrn">基准星</param>
        /// <param name="epoch">历元</param>
        /// <returns></returns>
        public FcbOfUpd GetWideLaneFcb(SatelliteNumber basePrn, Time epoch, double maxRms = 0.6)
        {
            //获取各测站内部差值。
            Dictionary<SatelliteNumber, RmsedNumeral> result = GetWideLaneFcbDic(basePrn, epoch, maxRms);

            if (result.Count == 0) { return null; }
            FcbOfUpd fcb = new FcbOfUpd(basePrn, epoch, result);
            return fcb;
        }
        /// <summary>
        /// 当前历元产品，为所在时段的平均。返回对应的各颗卫星数据。
        /// </summary>
        /// <param name="basePrn"></param>
        /// <param name="epoch"></param>
        /// <param name="maxRms"></param>
        /// <returns></returns>
        public Dictionary<SatelliteNumber, RmsedNumeral> GetWideLaneFcbDic(SatelliteNumber basePrn, Time epoch, double maxRms)
        {
            var rawDiffer = GetRawDiffer(basePrn, epoch, maxRms);
            var eachs = new Dictionary<SatelliteNumber, List<RmsedNumeral>>();

            var allPrns = this.GetAllPrns();
            foreach (var prn in allPrns)
            {
                eachs[prn] = new List<RmsedNumeral>(); //将同一差分卫星归为一个列表，然后求平均
            }

            foreach (var site in rawDiffer)
            {
                foreach (var item in site.Data)
                {
                    eachs[item.Key].Add(item.Value );
                }
            }

            //求加权平均
            Dictionary<SatelliteNumber, RmsedNumeral> result = Geo.Utils.DoubleUtil.GetRoundFractionAverageValues(eachs);
            return result;
        }


        /// <summary>
        /// 获取指定历元所有数值与基准的差分值。
        /// 首先进行时段差分，再获取。
        /// </summary>
        /// <param name="basePrn"></param>
        /// <param name="epoch">历元</param>
        /// <param name="maxRms">RMS过滤</param>
        /// <returns></returns>
        public BaseDictionary<String,BaseDictionary<SatelliteNumber, RmsedNumeral>> GetRawDiffer(SatelliteNumber basePrn, Time epoch, double maxRms)
        {
            var result = new BaseDictionary<String, BaseDictionary<SatelliteNumber, RmsedNumeral>>(basePrn + "_" + epoch.ToString());
            foreach (var kv in this.Data)
            {
                var dic = kv.Value.GetRawDiffer(basePrn, epoch, maxRms);
                if(dic.Count == 0) { continue; }
                result[kv.Key] = dic;
            }
         
            return result;
        }
        #endregion

        #region 小数部分的计算
        /// <summary>
        /// 将所有产品归算到一个卫星上面。取平均。
        /// </summary>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public FcbOfUpd GetWideLaneFcb(SatelliteNumber basePrn)
        {
            Dictionary<SatelliteNumber, RmsedNumeral> result = GetFinalFcbOfBsd(basePrn);
            result[basePrn] = new RmsedNumeral(0, 0);
            var prns = result.Keys.ToList();
            prns.Sort();
            //生成产品
            FcbOfUpd fcb = new FcbOfUpd();
            fcb.BasePrn = basePrn;
            fcb.WnMarker = FcbOfUpd.WideLaneMaker;
            fcb.Count = 32;// result.Count;
            fcb.Epoch = this.First.First.FirstKey.Start;
            //fcb.Prns = prns;
            int i = 0;
            foreach (var prn in prns)
            {
                i++;
                if(i > 32) { break; }
                fcb.Add(prn, result[prn]);
            }

            return fcb;
        }

        /// <summary>
        /// 最后，将所有归算到一个基准卫星上面。
        /// </summary>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public Dictionary<SatelliteNumber, RmsedNumeral> GetFinalFcbOfBsd(SatelliteNumber basePrn)
        {
            var dic = GetDifferFractionProduct();

            //首先归算到一个卫星上
            var data = new Dictionary<SatelliteNumber, List<RmsedNumeral>>();
            foreach (var kv in dic)
            {
                if (kv.Key != basePrn && !kv.Value.ContainsKey(basePrn)) { continue; }//忽略没有此基准星的产品
                var baseVal = kv.Value[basePrn];
                foreach (var item in kv.Value)
                {
                    var prn = item.Key;
                    if (!data.ContainsKey(prn)) { data[prn] = new List<RmsedNumeral>(); }

                    var bsd = item.Value - baseVal;
                    if(Double.IsNaN( bsd.Value ))
                    {
                        continue;
                    }
                    data[prn].Add(bsd);
                }
            }
            //归算到一个区间
            //其次求平均
            Dictionary<SatelliteNumber, RmsedNumeral> result = Geo.Utils.DoubleUtil.GetRoundFractionAverageValues(data);
            return result;
        }

        /// <summary>
        /// 以表格形式返回所有基准卫星差分产品
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <returns></returns>
        public ObjectTableStorage GetProductTableOfAllDiffer(string tableName = "所有卫星差分产品")
        {
            var dic = GetDifferFractionProduct();

            var table = new ObjectTableStorage(tableName);
            int index = 0;
            var prns = dic.Keys.ToList();
            prns.Sort();
            foreach (var basePrn in prns)
            {
                index++;
                var vals = dic[basePrn];
                table.NewRow();
                table.AddItem("Num", index);
                table.AddItem("BasePrn", basePrn);
                foreach (var prn in prns)
                {
                    if (!vals.ContainsKey(prn)) { continue; }

                    table.AddItem(prn, vals[prn].Value);
                }
            }

            return table;
        }

        /// <summary>
        /// 返回所有卫星之间的差分产品。
        /// </summary>
        /// <returns>正小数的产品</returns>
        public Dictionary<SatelliteNumber, Dictionary<SatelliteNumber, RmsedNumeral>> GetDifferFractionProduct()
        {
            if (ProductsOfAllDiffer != null) { return ProductsOfAllDiffer; }

            var dic = new Dictionary<SatelliteNumber, Dictionary<SatelliteNumber, RmsedNumeral>>();
            List<SatelliteNumber> prns = GetAllPrns();

            foreach (var prn in prns)
            {
                dic[prn] = GetDifferFractionProduct(prn);
            }
            this.ProductsOfAllDiffer = dic;

            return dic;
        }
        List<SatelliteNumber> AllPrns { get; set; }
        private List<SatelliteNumber> GetAllPrns()
        {
            if(AllPrns!=null) { return AllPrns; }

            var prns = this.First.Keys;

            foreach (var item in this)
            {
                prns = Geo.Utils.ListUtil.GetAll<SatelliteNumber>(item.Keys, prns);
            }

            prns.Sort();
            this.AllPrns = prns;
            return prns;
        }

        /// <summary>
        /// 计算差分产品,小数部分，区间[-0.5, 0.5]
        /// </summary>
        /// <param name="basePrn">基准卫星</param>
        /// <returns>正小数的产品</returns>
        public Dictionary<SatelliteNumber, RmsedNumeral> GetDifferFractionProduct(SatelliteNumber basePrn)
        {
            var rawDiffer = this.GetRawDiffer(basePrn);               //首先获取各站内的卫星差分
            var fractions = rawDiffer.GetAverageRoundFractionTable(); //计算小数部分的平均，以表格返回，列名为卫星，行索引为测站
           //字典法  
           var dic = new Dictionary<SatelliteNumber, RmsedNumeral>();
            var results = fractions.GetAveragesWithRms();           //所有卫星差分值求平均
            foreach (var item in results)
            {
                var prn = SatelliteNumber.Parse(item.Key);
                if (prn == SatelliteNumber.Default) { continue; }

                var frac = Geo.Utils.DoubleUtil.GetRoundFraction(item.Value.Value); //产品为 [-0.5, 0.5] 区间的小数

                dic[prn] = new RmsedNumeral(frac, item.Value.Rms);
            }

            return dic;           
        }


        /// <summary>
        /// 获取各测站小数部分的平均，与时段无关。
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public ObjectTableStorage GetAverageRoundFractionTable(string tableName = "所有站MW计算结果小数")
        {
            if(FractionTable!= null) { log.Info("小数表 FractionTable 已经计算，直接返回！"); return FractionTable; }

            FractionTable = new ObjectTableStorage(tableName);
            foreach (var siteMw in this.Data)
            {
                FractionTable.NewRow();
                FractionTable.AddItem("Site", siteMw.Key);
                var dic = siteMw.Value.GetAverageRoundFraction();
                foreach (var item in dic.Data)
                {
                    FractionTable.AddItem(item.Key, item.Value.Value);//每颗星的数值放在一列
                }
            }
            return FractionTable; 
        }
        #endregion

        #region  完整的信息都包含于此，所有产品皆可由此衍生
        /// <summary>
        /// 获取表格，以Key为列名
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <returns></returns>
        public ObjectTableStorage GetDetailTable(string tableName = "所有站MW时段详情")
        {
            if (DetailTable != null) { log.Info("详细表 DetailTable 已经计算，直接返回！"); return DetailTable; }
            DetailTable = new ObjectTableStorage(tableName);
            int index = 0;
            foreach (var siteMw in this.Data)
            {
                siteMw.Value.AddDetailRowsToTable(DetailTable, ref index, "Site", siteMw.Key); 
            } 
            return DetailTable;
        }

        /// <summary>
        /// 解析为卫星表格集合。
        /// </summary>
        /// <param name="prnPathes"></param>
        /// <returns></returns>
        public static BaseDictionary<SatelliteNumber, MultiSitePeriodValueStorage> ParsePrnTables(string [] prnPathes)
        {
            var mgr = new BaseDictionary<SatelliteNumber, MultiSitePeriodValueStorage>("卫星表格集合");
            foreach (var path in prnPathes)
            {
                var prn = SatelliteNumber.Parse( Path.GetFileName(path));
                mgr[prn] = ParseDetailTable(path);
            }
            return mgr;
        }

        /// <summary>
        /// 解析为对象
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static MultiSitePeriodValueStorage ParseDetailTable(string path)
        {
            var data = new ObjectTableReader(path).Read();
            return MultiSitePeriodValueStorage.ParseDetailTable(data);
        }

        /// <summary>
        /// 解析表格获取对象
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static MultiSitePeriodValueStorage ParseDetailTable(ObjectTableStorage table)
        {
            MultiSitePeriodValueStorage store = new MultiSitePeriodValueStorage(table.Name);
            store.DetailTable = table;
            foreach (var row in table.BufferedValues)
            {
                var site = row["Site"].ToString();
                var prn = SatelliteNumber.Parse( row["Name"].ToString());
                var period = TimePeriod.Parse( row["Group"].ToString());
                var val = (double)row["Value"];
                var rms = (double)row["RMS"];

                store.GetOrCreate(site).GetOrCreate(prn)[period] = new RmsedNumeral(val, rms);
            }
            return  store;
        }
        #endregion

        /// <summary>
        /// 获取表格，只显示分段，与时段无关
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetTable(string tableName = "所有站MW计算结果")
        {
            ObjectTableStorage table = new ObjectTableStorage(tableName);
            foreach (var siteMw in this.Data)
            {
                siteMw.Value.ExtractRowToTable(table);

            }
            return table;
        }

    }

}