//2018.11.09, czs, create in HMX, 一条基线的平差结果
//2018.11.10, czs, create in hmx, 增加基线组合类
//2018.11.30, czs, create in hmx, 实现IToTabRow接口，用于规范输出,合并定义新的 BaseLineNet

using System;
using System.Collections.Generic;
using Gnsser.Domain;
using System.Text;
using System.Linq;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Filter;
using Gnsser.Checkers;
using Geo.Common;
using Geo;
using AnyInfo.Graphs.Structure;
using Geo.Times;
using AnyInfo.Graphs;

namespace Gnsser
{  
    /// <summary>
    /// 多时段
    /// </summary>
    public class MultiPeriodBaseLineNet : BaseDictionary<TimePeriod ,BaseLineNet>
    {
        protected override void Init()
        {
            base.Init();
            foreach (var item in this)
            {
                item.Init();
            }
        }
        public override BaseLineNet Create(TimePeriod key)
        {
            return new BaseLineNet() ;
        }
        /// <summary>
        /// 用于显示
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetLineTable()
        {
            ObjectTableManager tables = new ObjectTableManager();
            foreach (var item in this.KeyValues)
            {
                var table = item.Value.GetLineTable();
                tables[item.Key.ToDefualtPathString()] = table;
            }
            ObjectTableStorage result = tables.Combine();
            return result;
        }
        public List<EstimatedBaseline> GetAllLines()
        {
            List<EstimatedBaseline> result = new List<EstimatedBaseline>();
            foreach (var item in this)
            {
                result.AddRange(item.Values);
            }
            return result;
        }

        /// <summary>
        /// 多时段单表解析
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static MultiPeriodBaseLineNet Parse(ObjectTableStorage table)
        {
            var data = new MultiPeriodBaseLineNet();
           // var data = new BaseLineNet();
            foreach (var row in table.BufferedValues)
            {
                EstimatedBaseline obj = EstimatedBaseline.Parse(row);
                var period = TimePeriod.GetMaxCommon(data.Keys, obj.ApporxNetPeriod);
                BaseLineNet BaseLineNet = null;
                if (period == null)
                {
                    BaseLineNet = data.GetOrCreate(obj.ApporxNetPeriod);
                }
                else
                {
                    BaseLineNet = data.GetOrCreate(period);
                }
                BaseLineNet.Set(obj.BaseLineName, obj);
            }
            data.Init();
            return data;
        }
        /// <summary>
        /// 测站坐标
        /// </summary>
        /// <returns></returns>
        public List<NamedXyz> GetSiteCoords()
        {
            List<NamedXyz> coords = new List<NamedXyz>();
            foreach (var item in this)
            {
                coords.AddRange(item.GetSiteCoords());
            }
            return coords;
        }
        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="netA"></param>
        /// <param name="netB"></param>
        /// <returns></returns>
        public static Dictionary<TimePeriod, List<NamedXyzEnu>> Compare(MultiPeriodBaseLineNet netA, MultiPeriodBaseLineNet netB)
        {
            var keyValuePairs = new Dictionary<TimePeriod, List<NamedXyzEnu>>();

            foreach (var netKv in netA.KeyValues)
            {
                var netPeriod = netKv.Key;
                var neta = netKv.Value;
                TimePeriod samePeriod = netB.GetInterSectPeriod(netPeriod);
                if(samePeriod == null) { continue; }

                var netb = netB[samePeriod];
                keyValuePairs[netPeriod] = BaseLineNet.Compare(neta, netb);
            }
            return keyValuePairs;
        }

        private TimePeriod GetInterSectPeriod(TimePeriod key)
        {
            return TimePeriod.GetMaxCommon(this.Keys, key); 
        }
    }

    /// <summary>
    /// 基线网，估值基线集合，如星形网解基线。此处不一定为同步网。
    /// </summary>
    public class BaseLineNet : BaseDictionary<GnssBaseLineName, EstimatedBaseline>, IReadable, IComparable<BaseLineNet>
    {
        /// <summary>
        /// /构造函数
        /// </summary> 
        public BaseLineNet()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="baseLines"></param>
        public BaseLineNet(IEnumerable<SiteObsBaseline> baseLines)
        {
            foreach (var line in baseLines)
            {
                if(line == null || line.EstimatedResult == null) { continue; }
                var estline = (EstimatedBaseline)line.EstimatedResult;
                estline.NetPeriod = line.NetPeriod;
                this.Add(estline);
            }
            Init();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="baseLines"></param>
        public BaseLineNet(IEnumerable<EstimatedBaseline> baseLines)
        {
            foreach (var line in baseLines)
            {
                if(line == null) { continue; }
                this.Add(line);
            }
            Init();
        }
        /// <summary>
        /// 数据变动后，应该初始化测站名称
        /// </summary>
        public new void Init()
        { 
            //检查，确保测站名称对应的否坐标一致
            SiteCoordsManager siteCoords = new SiteCoordsManager();
            foreach (var baseLine in this)
            {
                siteCoords.GetOrCreate(baseLine.BaseLineName.RefName).Add(baseLine.ApproxXyzOfRef);
                siteCoords.GetOrCreate(baseLine.BaseLineName.RovName).Add(baseLine.ApproxXyzOfRov);
            }
            foreach (var item in siteCoords)
            {
                if (item.Coords.Count > 0 && !item.IsCoordSame())//坐标不一致
                {
                    SetSiteCoord(item.Name, item.Coord);//采用第一个进行赋值
                }
            }
            //默认时段
            if (TimePeriod == null && this.Count > 0) { TimePeriod = new BufferedTimePeriod(First.ApporxNetPeriod); }
        }
        
        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="netA"></param>
        /// <param name="netB"></param>
        /// <returns></returns>
        public static List<NamedXyzEnu> Compare(BaseLineNet netA, BaseLineNet netB)
        {//提取和转换，有的方向不一致
            List<NamedXyz> netBlines = new List<NamedXyz>();
            foreach (var item in netB)
            {
                var line = netA.GetOrReversed(item.BaseLineName);
                if (line == null) { continue; }
                var namedXyz = new NamedXyz(line.BaseLineName.Name, line.EstimatedVector);
                netBlines.Add(namedXyz);
            }

            var CoordsB = netB.GetNamedXyzs();
            var Compared = NamedXyz.Compare(netBlines, CoordsB);

            var CompareResults = new List<NamedXyzEnu>();
            foreach (var localXyz in Compared)
            {
                GnssBaseLineName name = new GnssBaseLineName(localXyz.Name);
                var staXyz = netA.GetSiteCoord(name.RefName);
                if (staXyz == null) { continue; }
                var item = NamedXyzEnu.Get(localXyz.Name, localXyz.Value, staXyz);
                CompareResults.Add(item);
            }
            return CompareResults;
        }
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        public List<NamedXyz> GetNamedXyzs()
        {
            List<NamedXyz> namedXyzs = new List<NamedXyz>();
            foreach (var item in this)
            {
                namedXyzs.Add(new NamedXyz(item.Name, item.EstimatedVector));
            } 
            return namedXyzs;
        }

        /// <summary>
        /// 是否包含测站
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public bool Contains(string siteName)
        {
            foreach (var baseLine in BaseLines)
            {
                if (baseLine.Contains(siteName)) { return true; }
            }
            return false;
        }
        /// <summary>
        /// 此测站设置为此坐标
        /// </summary>
        /// <param name="name"></param>
        /// <param name="xyz"></param>
        public void SetSiteCoord(string name, XYZ xyz)
        {
            foreach (var item in this)
            {
                item.SetSiteCoord(name, xyz);
            }
        }
        /// <summary>
        /// 获取第一个坐标值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public XYZ GetSiteCoord(string name)
        {
            foreach (var item in this)
            {
                var site = item.GetApproxXyz(name);
                if (site != null) { return site; }
            }
            return null;
        }

        /// <summary>
        /// 是否包含基线
        /// </summary>
        /// <param name="name"></param>
        /// <param name="reverseAble"></param>
        /// <returns></returns>
        public bool Contains(GnssBaseLineName name, bool reverseAble = true)
        {
            foreach (var baseLine in BaseLines)
            {
                if (baseLine.IsEqualOrReverseEqual(name)) { return true; }
            }
            return false;
        }
        /// <summary>
        /// 是否包含基线
        /// </summary>
        /// <param name="refName"></param>
        /// <param name="rovName"></param>
        /// <param name="reverseAble"></param>
        /// <returns></returns>
        public bool Contains(string refName, string rovName, bool reverseAble = true)
        {
            foreach (var baseLine in BaseLines)
            {
                if (baseLine.Contains(refName, rovName, reverseAble)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// 增加一条,如果重复，将覆盖上一条
        /// </summary>
        /// <param name="estimatedBaseline"></param>
        public void Add(EstimatedBaseline estimatedBaseline) { if (estimatedBaseline == null) { return; } this[estimatedBaseline.BaseLineName] = estimatedBaseline; }

        /// <summary>
        /// 闭合差，当网作为闭合回路时。
        /// </summary>
        public RmsedXYZ ClosureError { get; set; }
        /// <summary>
        /// 测量时段，通常以最后历元为中心
        /// </summary>
        public BufferedTimePeriod TimePeriod { get; set; }

        /// <summary>
        /// 所有基线名称
        /// </summary>
        public List<GnssBaseLineName> BaseLines { get => Keys; }
        /// <summary>
        /// 所有测站名称
        /// </summary>
        public List<string> SiteNames
        {
            get
            {
                List<string> siteNames = new List<string>();
                foreach (var item in this)
                {
                    siteNames.Add(item.BaseLineName.RefName);
                    siteNames.Add(item.BaseLineName.RovName);
                }
                return siteNames.Distinct().ToList();
            }
        }
        /// <summary>
        /// 所有基线平均长度
        /// </summary>
        public double AverageLength { get => TotalLength / this.Count; }
        /// <summary>
        /// 所有基线长度
        /// </summary>
        public double TotalLength
        {
            get
            {
                double len = 0;
                foreach (var line in this)
                {
                    len += line.EstimatedVector.Length;
                }
                return len;
            }
        }
        /// <summary>
        /// 获取坐标
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public XYZ GetApproxXyz(string siteName)
        {
            foreach (var item in this.KeyValues)
            {
                if (item.Key.Contains(siteName))
                {
                    return item.Value.GetApproxXyz(siteName);
                }
            }
            return null;
        }

        /// <summary>
        /// 根据名称提取基线，如果是反向的，则提取反向的
        /// </summary>
        /// <param name="baseLineName"></param>
        /// <returns></returns>
        public EstimatedBaseline GetOrReversed(GnssBaseLineName baseLineName)
        {
            var result = Get(baseLineName);
            if (result == null)
            {
                result = Get(baseLineName.ReverseBaseLine);
                if (result != null)
                {
                    result = (EstimatedBaseline)result.ReversedBaseline;
                }
                else
                {
                    // log.Error("网中没有 " + baseLineName);
                }
            }
            return result;
        }
        /// <summary>
        /// 测站坐标
        /// </summary>
        /// <returns></returns>
        public List<NamedXyz> GetSiteCoords()
        {
            List<NamedXyz> coords = new List<NamedXyz>();
            foreach (var item in this.SiteNames)
            {
                var xyz = this.GetApproxXyz(item);
                var namedXyz = new NamedXyz(item, xyz);
                coords.Add(namedXyz);
            }
            return coords;
        }

        #region 复测较差

        /// <summary>
        /// 提取共同基线，复测基线较差。
        /// </summary>
        /// <param name="otherNet"></param>
        /// <returns></returns>
        public Dictionary<GnssBaseLineName, BaseLineRepeatError> GetRepeatBaseLineClosureError(BaseLineNet otherNet)
        {
            var names = new Dictionary<GnssBaseLineName, BaseLineRepeatError>();
            foreach (var line in this)
            {
                var name = line.BaseLineName;
                var vec = otherNet.GetOrReversed(name);
                if (vec == null) { continue; }
                var error = line.EstimatedVectorRmsedXYZ - vec.EstimatedVectorRmsedXYZ;

                BaseLineRepeatError baseLineRepeatError = new BaseLineRepeatError(vec.BaseLineName, error);
                names.Add(vec.BaseLineName, baseLineRepeatError);
            }
            return names;
        }

        /// <summary>
        /// 提取共同基线，异步环检验。
        /// </summary>
        /// <param name="otherNet"></param>
        /// <returns></returns>
        public List<GnssBaseLineName> GetSameBaseLineName(BaseLineNet otherNet)
        {
            List<GnssBaseLineName> names = new List<GnssBaseLineName>();
            foreach (var item in this.BaseLines)
            {
                var vec = otherNet.GetOrReversed(item);
                names.Add(vec.BaseLineName);
            }
            return names;
        }
        #endregion

        #region  闭合环提取或计算
        /// <summary>
        /// 检查闭合差，没有则设置，最后返回。
        /// </summary>
        /// <returns></returns>
        public RmsedXYZ CheckOrSetAndGetClosureError()
        {
            if (this.ClosureError == null) { this.CalculateSetAndGetClosureError(); }
            return this.ClosureError;
        }

        /// <summary>
        /// 提取并计算闭合差
        /// </summary>
        /// <returns></returns>
        public RmsedXYZ CalculateSetAndGetClosureError()
        {
            RmsedXYZ result = new RmsedXYZ();
            var lines = GnssBaseLineName.GetClosuredPath(this.BaseLines);
             
            foreach (var line in lines)
            {
                var vec = this.GetOrReversed(line);
                result += vec.EstimatedVectorRmsedXYZ;
            }
            if (result.Equals(RmsedXYZ.Zero))
            {
                result = RmsedXYZ.MaxValue;
            }
            this.ClosureError = result; 

            return result; 
        }
               

        /// <summary>
        /// 构造对应的图
        /// </summary>
        /// <returns></returns>
        private Graph BuildGraph()
        {
            List<Edge> edges = new List<Edge>();
            foreach (var baseLine in BaseLines) { edges.Add(new Edge(new Node(baseLine.RefName), new Node(baseLine.RovName))); }
            Graph graph = new Graph(edges);//每次查询，都重置所有访问情况 
            return graph;
        }
        #endregion

        #region  提取独立基线，用于网平差
        /// <summary>
        /// 获取独立基线网
        /// </summary>
        /// <param name="IndependentLineSelectType"></param>
        /// <returns></returns>
        public BaseLineNet GetIndependentNet(IndependentLineSelectType IndependentLineSelectType)
        {
            var lienWeighter = new BaseLineWeighter(this, IndependentLineSelectType);
            Graph graph = BuildGraph();
            if(graph.Edges.Count  == 0) { return new BaseLineNet(); }

            MinCostSpanTreeBuilder builder = new MinCostSpanTreeBuilder(graph, lienWeighter);
            var g = builder.Build();
            BaseLineNet result = new BaseLineNet();
            foreach (var item in g.Edges)
            {
                result.Add(this.GetOrReversed(new GnssBaseLineName(item.NodeA.Id, item.NodeB.Id)));
            }
            result.Init();

            return result;
        }

        #endregion

        /// <summary>
        /// 返回基线顺序的路径。
        /// </summary>
        /// <returns></returns>
        public string GetPath()
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var item in this)
            {
                if (i > 0) { sb.Append(", "); }
                sb.Append(item.Name);
                i++;
            }
            return sb.ToString();
        }
       

        /// <summary>
        /// 添加基线网
        /// </summary>
        /// <param name="lines"></param>
        public void Add(BaseLineNet lines)
        {
            foreach (var item in lines)
            {
                this.Add(item);
            }
        }

        /// <summary>
        /// 包含基线
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public List<EstimatedBaseline> GetEstimatedBaselines(String siteName)
        {
            List<EstimatedBaseline> result = new List<EstimatedBaseline>();
            foreach (var item in this)
            {
                if (item.BaseLineName.Contains(siteName)) result.Add(item);
            }
            return result;
        }

        /// <summary>
        /// 包含基线
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<GnssBaseLineName> GetBaselines(String name)
        {
            List<GnssBaseLineName> result = new List<GnssBaseLineName>();
            foreach (var item in this)
            {
                if (item.BaseLineName.Contains(name)) result.Add(item.BaseLineName);
            }
            return result;
        }


        /// <summary>
        /// 点对像表
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetSiteTable()
        {
            ObjectTableStorage result = new ObjectTableStorage("Site_" + Name);
            foreach (var name in this.SiteNames)
            {
                result.NewRow();
                result.AddItem("Name", name);
                var xyz = GetApproxXyz(name);
                result.AddItem(xyz.GetObjectRow());

                var lines = GetBaselines(name);
                result.AddItem("BaseLines", Geo.Utils.StringUtil.ToString(lines));

            }
            return result;
        }

        /// <summary>
        /// 线对像表
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetLineTable()
        {
            ObjectTableStorage result = new ObjectTableStorage(Name);
            foreach (var item in this)
            {
                result.NewRow();
                result.AddItem(item.GetObjectRow());
            }
            return result;
        }
        /// <summary>
        /// 字符串显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GetPath();
        }
        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(BaseLineNet other)
        {
            var thisError = this.CheckOrSetAndGetClosureError();
            var thatError = other.CheckOrSetAndGetClosureError();
            return thisError.Value.CompareTo(thatError.Value);
        }
        /// <summary>
        /// 可读
        /// </summary>
        /// <param name="splitter"></param>
        /// <returns></returns>
        public string ToReadableText(string splitter = ",")
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("测站数：" + (this.Count + 1));
            // sb.AppendLine("基准站：" + SiteInfoOfRef.SiteName);
            sb.AppendLine("基线(" + (this.Count) + "条)：");
            sb.Append("基线：");
            int i = 0;
            foreach (var kv in this.KeyValues)
            {
                if (i > 0) { sb.Append(", "); }
                sb.Append(kv.Value.BaseLineName.RovName);
                i++;
            }
            //   sb.AppendLine("->"+ SiteInfoOfRef.SiteName);
            i = 1;
            foreach (var kv in this.KeyValues)
            {
                sb.Append(i.ToString() + "  ");
                sb.AppendLine(kv.Value.ToReadableText(splitter));
                i++;
            }
            return sb.ToString();
        }

        /// <summary>
        /// 重复基线检核结果表格
        /// </summary>
        /// <param name="data"></param>
        /// <param name="netName"></param>
        /// <param name="isBadOnly"></param>
        /// <returns></returns>
        public static ObjectTableStorage BuildRepeatBaseLineCheckResultTable(RepeatErrorQualityManager data, string netName, bool isBadOnly)
        {
            ObjectTableStorage lineTable = new ObjectTableStorage(netName + "重复基线检核结果");
            foreach (var kv in data.KeyValues)
            {
                var baseLine = kv.Key;
                var qualityOfBaseLine = kv.Value;

                if (isBadOnly && kv.Value.LengthIsOk)
                {
                    continue;
                }

                lineTable.NewRow();
                lineTable.AddItem("网编号", netName);
                lineTable.AddItem("基线", baseLine.Name);
                lineTable.AddItem(qualityOfBaseLine.GetObjectRow()); 
            }

            return lineTable;
        }

        /// <summary>
        /// 求重复基线较差
        /// </summary>
        /// <param name="checkNet"></param>
        /// <param name="asychCloserErrors"></param>
        /// <param name="gnssReveiverNominalAccuracy"></param>
        /// <param name="netName"></param>
        /// <returns></returns>
        public RepeatErrorQualityManager BuildRepeatBaseLineError(BaseLineNet checkNet, Dictionary<GnssBaseLineName, BaseLineRepeatError> asychCloserErrors, GnssReveiverNominalAccuracy gnssReveiverNominalAccuracy, string netName)
        {
            List<string> badBaseLines = new List<string>();
            //按照GB/T 18314-2009，B、C级复测基线长度较差应满足：Wx<= 2 √2 σ 
            RepeatErrorQualityManager data = new RepeatErrorQualityManager(); 
            foreach (var kv in asychCloserErrors)
            {
                var baseLine = checkNet.GetOrReversed( kv.Key);
                var ClosureError = kv.Value.RmsedXYZ;
                
                QualityOfRepeatError qualityOfBaseLine = new QualityOfRepeatError(baseLine, ClosureError, gnssReveiverNominalAccuracy);
                data[kv.Key] = qualityOfBaseLine;
                if (!qualityOfBaseLine.LengthIsOk)
                {
                    badBaseLines.Add(baseLine.Name);
                }
            }
            if (badBaseLines.Count > 0)
            {
                log.Warn("复测观测网 " + netName + " 中，" + badBaseLines.Count + " 条基线长度较差不合格： " + Geo.Utils.StringUtil.ToString(badBaseLines, ", "));
            }

            return data;
        }
        /// <summary>
        /// 生成所有可能的三角形，然后提取网络，计算闭合差 
        /// </summary>
        /// <returns></returns>
        public TriguilarNetQualitiyManager BuildTriangularClosureQualies(GnssReveiverNominalAccuracy GnssReveiverNominalAccuracy)
        { 
            var triNetNames = TriangularNetName.BuildTriangularNetNames(this.SiteNames);
            
            //保存结果
           var CurrentQualityManager = new TriguilarNetQualitiyManager();
            List<BaseLineNet> nets = new List<BaseLineNet>();
            foreach (var triNetName in triNetNames)
            {
                var subNet = this.GetTriguilarNet(triNetName);
                if (subNet == null) { continue; }
                var qulity = new QualityOfTriAngleClosureError(subNet,  GnssReveiverNominalAccuracy);
                CurrentQualityManager.Add(triNetName, qulity);
            }
            return CurrentQualityManager;
        }


        /// <summary>
        /// 生成所有可能的三角形，然后提取网络，计算闭合差 
        /// </summary>
        /// <returns></returns>
        public TriguilarNetManager BuildTriangularClosureNets()
        { 
            var triNetNames = TriangularNetName.BuildTriangularNetNames(this.SiteNames);
            
            //保存结果
           var CurrentQualityManager = new TriguilarNetManager();
            List<BaseLineNet> nets = new List<BaseLineNet>();
            foreach (var triNetName in triNetNames)
            {
                var subNet = this.GetTriguilarNet(triNetName);
                if (subNet == null) { continue; }
                subNet.CalculateSetAndGetClosureError(); //计算闭合差
                CurrentQualityManager.Add(triNetName, subNet);
            }
            return CurrentQualityManager;
        }
        /// <summary>
        /// 三边闭合环检核，构建环路环检核结果
        /// </summary>
        public static ObjectTableStorage BuildSynchNetTrilateralCheckTResultable(
            string netName,
            TriguilarNetQualitiyManager qualities,
            BufferedTimePeriod timeperiod = null, bool IsBadOnly = false)
        {
            //B-E网，GPS
            ObjectTableStorage lineTable = new ObjectTableStorage(timeperiod+"");
            foreach (var kv in qualities.KeyValues)
            {
                var quality = kv.Value;
                if (IsBadOnly && quality.IsAllOk)
                {
                    continue;
                }
                lineTable.NewRow();
                lineTable.AddItem("网编号", netName);
                //lineTable.AddItem("Index", i++);

                lineTable.AddItem(quality.GetObjectRow());
                if (timeperiod != null)
                {
                    lineTable.AddItem("时段", timeperiod.ToString());
                }
                lineTable.AddItem("闭合路线", kv.Key);
            }

            return lineTable;
        }

        /// <summary>
        /// 提取三角网
        /// </summary>
        /// <param name="triNetNames"></param>
        /// <returns></returns>
        public List<BaseLineNet> GetTriguilarNet(List<TriangularNetName> triNetNames)
        {
            List<BaseLineNet> nets = new List<BaseLineNet>();
            foreach (var triNetName in triNetNames)
            {
                var subNet = GetTriguilarNet(triNetName);
                if(subNet == null) { continue; }
                nets.Add(subNet);
            }
            return nets;
        }

        /// <summary>
        /// 提取三角网
        /// </summary>
        /// <param name="triNetName"></param>
        /// <returns></returns>
        public BaseLineNet GetTriguilarNet(TriangularNetName triNetName)
        {
            List<EstimatedBaseline> lines = new List<EstimatedBaseline>();
            var lineNames = triNetName.GetBaseLineNames();
            foreach (var item in lineNames)
            {
                var line =this.GetOrReversed(item);
                if(line == null) { return null; }
                lines.Add(line);
            }
            BaseLineNet net = new BaseLineNet(lines);
            return net;
        }
         

        #region 静态工具方法                
        /// <summary>
        /// 单时段单表解析
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static BaseLineNet Parse(ObjectTableStorage table)
        {
            var data = new BaseLineNet();
            foreach (var row in table.BufferedValues)
            {
                EstimatedBaseline obj = EstimatedBaseline.Parse(row);
                data.Set(obj.BaseLineName, obj);
            }
            data.Init();
            return data;
        }

        #endregion
    }
     
}