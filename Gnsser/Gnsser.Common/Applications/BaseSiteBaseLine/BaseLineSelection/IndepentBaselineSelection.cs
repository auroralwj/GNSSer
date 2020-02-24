//2015.11.19, cy, 从n个测站中找出n-1条独立基线
//2016.06.09, cy, 优化选取代码
//2018.11.26，czs, edit in hmx, 全基线生成
//2019.05.09, czs, edit in hmx, 增加指定的基线生成


using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.IO;
using Geo;
using Gnsser.Domain;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Filter;
using Gnsser.Checkers;
using Geo.Common;
using Geo.Times;
using AnyInfo.Graphs;
using AnyInfo.Graphs.Structure;

namespace Gnsser
{
    //2018.07.30, czs, create in Hmx, 基线选择类型
    /// <summary>
    /// 基线选择类型
    /// </summary>
    public enum BaseLineSelectionType
    {
        /// <summary>
        /// 全基线法，用于工程测量
        /// </summary>
        全基线,
        /// <summary>
        /// 最短路径法
        /// </summary>
        最短路径,
        /// <summary>
        /// 最多历元观测量法
        /// </summary>
        最多观测量,
        /// <summary>
        /// 中心站法
        /// </summary>
        中心站法,
        /// <summary>
        /// 混合法
        /// </summary>
        混合法,
        /// <summary>
        /// 指定外部文件输入
        /// </summary>
        外部文件
    }
    
    /// <summary>
    /// 调用器。选择独立基线。
    /// </summary>
    public class BaseLineSelector : AbstractBuilder<List<GnssBaseLineName>, List<ObsSiteInfo>>
    {
        /// <summary>
        /// 基线选择
        /// </summary>
        /// <param name="BaseLineSelectionType"></param>
        /// <param name="IndicatedBaseLines"></param>
        /// <param name="stringParam">字符串参数，可以是中心站名称，也可以是基线文件路径，取决于类型</param>
        public BaseLineSelector(BaseLineSelectionType BaseLineSelectionType, string stringParam, List<GnssBaseLineName> IndicatedBaseLines)
        {
            this.BaseLineSelectionType = BaseLineSelectionType;
            this.IndicatedBaseLines = IndicatedBaseLines;
            this.CenterSiteName = stringParam;
        }
        /// <summary>
        /// 基线选择
        /// </summary>
        /// <param name="BaseLineSelectionType"></param>
        /// <param name="outerBaseLineFilePath"></param>
        /// <param name="stringParam">字符串参数，可以是中心站名称，也可以是基线文件路径，取决于类型</param>
        public BaseLineSelector(BaseLineSelectionType BaseLineSelectionType, string stringParam, string outerBaseLineFilePath)
        {
            this.BaseLineSelectionType = BaseLineSelectionType;
            this.CenterSiteName = stringParam;

            //读取基线
            var IndicatedBaseLines = new List<GnssBaseLineName>();
            if (File.Exists(outerBaseLineFilePath))
            {
                var lines = File.ReadAllLines(outerBaseLineFilePath);
                foreach (var line in lines)
                {
                    if (String.IsNullOrEmpty(line.Trim())) { continue; }

                    IndicatedBaseLines.Add(new GnssBaseLineName(line));
                }
            }
            this.IndicatedBaseLines = IndicatedBaseLines;
        }

        #region 属性
        /// <summary>
        /// 指定的GNSS基线名称列表。
        /// </summary>
        public  List<GnssBaseLineName> IndicatedBaseLines{ get; set; }
        /// <summary>
        /// 基线选择类型
        /// </summary>
        public BaseLineSelectionType BaseLineSelectionType { get; set; }
        /// <summary>
        /// 中心站名称
        /// </summary>
        public string CenterSiteName { get; set; }
        #endregion

        /// <summary>
        /// 获取基线
        /// </summary>
        /// <param name="obsFilePaths"></param>
        /// <returns></returns>
        public List<Baseline> GetFileBaselines(string[] obsFilePaths)
        {
            var array = GetBaselines(obsFilePaths);
            List<Baseline> baselines = new List<Baseline>();
            foreach (var item in array)
            {
                var pathes = GnssBaseLineName.GetRefRovName( item.Name);
                baselines.Add(new Baseline() { StartName = pathes[0], EndName = pathes[1] });
            }
            return baselines;
        }
        public override List<GnssBaseLineName> Build(List<ObsSiteInfo> material)
        {
            var fileNames =new List<string>();
            foreach (var item in material)
            {
                fileNames.Add(item.FilePath);
            }
            return GetBaselines(fileNames.ToArray());
        }
        /// <summary>
        /// 生成基线名称
        /// </summary>
        /// <param name="obsFilePaths"></param>
        /// <returns></returns>
        public List<GnssBaseLineName> GetBaselines(string[] obsFilePaths)
        {
            if (obsFilePaths.Length < 2)
            {
                return new List<GnssBaseLineName>() { new GnssBaseLineName(obsFilePaths[0], obsFilePaths[0], true) };
            }
            //唯一基线
            if (obsFilePaths.Length == 2)
            {
                return new List<GnssBaseLineName>() { new GnssBaseLineName(obsFilePaths[1], obsFilePaths[0], true) };
            }

            switch (BaseLineSelectionType)
            {
                case BaseLineSelectionType.全基线:
                    return new TotalBaseLineBuilder(obsFilePaths).Build();
                case BaseLineSelectionType.最短路径:
                    return new ShortestPathBaseLineBuilder(obsFilePaths).Build();
                case BaseLineSelectionType.最多观测量:
                    MaxEpochIndepentBaselineSelection sect = new MaxEpochIndepentBaselineSelection(obsFilePaths);
                    sect.IndepentBaselineProcess();
                    return sect.IndepentBaselinesInfo;
                    break;
                case BaseLineSelectionType.中心站法:
                    if (String.IsNullOrEmpty(CenterSiteName)) {
                        log.Warn("中心站必须手动设置！此处先默认为第一个。");
                    }
                    return new CenterBaseLineBuilder(obsFilePaths, CenterSiteName).Build();
                case BaseLineSelectionType.混合法:
                    log.Warn("尚未实现 混合法 。。。。");
                    break;
                case BaseLineSelectionType.外部文件:
                    if (IndicatedBaseLines == null || IndicatedBaseLines.Count == 0) {
                        log.Error(BaseLineSelectionType  + " 类型，必须外部输入基线序列！");
                    }
                    return new IndicatedBaseLineBuilder(obsFilePaths, IndicatedBaseLines).Build();

                default:
                    break;
            }
            return new ShortestPathBaseLineBuilder(obsFilePaths).Build();
        }
    }


    /// <summary>
    /// 中心站法
    /// </summary>
    public class CenterBaseLineBuilder : AbstractBuilder<List<GnssBaseLineName>>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="obsFilePaths"></param>
        /// <param name="refSiteName"></param>
        public CenterBaseLineBuilder(string[] obsFilePaths, string refSiteName)
        {
            this.obsFilePaths = obsFilePaths;
            this.refSiteName = refSiteName;
        }
        /// <summary>
        /// 观测路径
        /// </summary>
        public string[] obsFilePaths { get; set; }
        /// <summary>
        /// 参考站
        /// </summary>
        public string refSiteName { get; set; }
        /// <summary>
        /// 构建
        /// </summary>
        /// <returns></returns>
        public override List<GnssBaseLineName> Build()
        {
            List<GnssBaseLineName> lines = new List<GnssBaseLineName>();
            ObsSiteFileManager siteObsBaselines = new ObsSiteFileManager(obsFilePaths, TimeSpan.FromMinutes(10));
            foreach (var item in siteObsBaselines)
            {
                if (item.SiteName.Equals(refSiteName)) { continue; }

                var line = new GnssBaseLineName(item.SiteName, refSiteName);
                lines.Add(line);
            }
            return lines;
        }
    }
    /// <summary>
    /// 最短路径
    /// </summary>
    public class ShortestPathBaseLineBuilder : AbstractBuilder<List<GnssBaseLineName>>
    {
        /// <summary>
         /// 默认构造函数
         /// </summary>
         /// <param name="obsFilePaths"></param>
        public ShortestPathBaseLineBuilder(string[] obsFilePaths)
        {
            this.obsFilePaths = obsFilePaths;
        }
        /// <summary>
        /// 观测路径
        /// </summary>
        public string[] obsFilePaths { get; set; }
        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override List<GnssBaseLineName> Build()
        {
            var total = new TotalObsBaseLineBuilder(obsFilePaths).Build();
            SiteObsBaselineManager siteObsBaselines = new SiteObsBaselineManager(total);

            var graph = this.BuildGraph(siteObsBaselines.Values);
            MinCostSpanTreeBuilder builder = new MinCostSpanTreeBuilder(graph, new SiteObsBaselineWeighter(siteObsBaselines));

            var g = builder.Build();
            List<GnssBaseLineName> result = new List<GnssBaseLineName>();
            foreach (var item in g.Edges)
            {
                var line = siteObsBaselines.Get(item.NodeA.Id, item.NodeB.Id);
                if(line != null)
                result.Add(line.LineName);
            }
            return result;
        }

        /// <summary>
        /// 构造对应的图
        /// </summary>
        /// <returns></returns>
        private Graph BuildGraph(List<SiteObsBaseline> BaseLines)
        {
            List<Edge> edges = new List<Edge>();
            foreach (var baseLine in BaseLines) { edges.Add(new Edge(new Node(baseLine.RefName), new Node(baseLine.RovName))); }
            Graph graph = new Graph(edges);//每次查询，都重置所有访问情况 
            return graph;
        }
    }

    /// <summary>
    /// 测站基线加权
    /// </summary>
    public class SiteObsBaselineWeighter : AnyInfo.Graphs.AbstractSimpleEdgeWeighter
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ObsBaseLineManager"></param>
        public SiteObsBaselineWeighter(SiteObsBaselineManager ObsBaseLineManager)
        {
            this.ObsBaseLineManager = ObsBaseLineManager;
        }
        /// <summary>
        /// 观测基线
        /// </summary>
        public SiteObsBaselineManager ObsBaseLineManager { get; set; }
        /// <summary>
        /// 获取权
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public override double GetWeight(INode start, INode end)
        {
            var line = ObsBaseLineManager.GetOrReversed(start.Id, end.Id);
            if(line == null)
            {
                return double.MaxValue;
            }
            return  line.GetLength();
        }
    }
     

    /// <summary>
    /// 全基线生成器
    /// </summary>
    public class TotalObsBaseLineBuilder : AbstractBuilder<List<SiteObsBaseline>>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="obsFilePaths"></param>
        public TotalObsBaseLineBuilder(string[] obsFilePaths)
        {
            this.obsFilePaths = obsFilePaths;
        }
        /// <summary>
        /// 观测路径
        /// </summary>
        public string[] obsFilePaths { get; set; }

        public override List<SiteObsBaseline> Build()
        {
            return BuildTotalBaseLines(obsFilePaths);
        }

        /// <summary>
        /// 生成全基线
        /// </summary>
        /// <param name="obsFilePaths"></param>
        /// <returns></returns>
        public static List<SiteObsBaseline> BuildTotalBaseLines(string[] obsFilePaths)
        {
            List<SiteObsBaseline> result = new List<SiteObsBaseline>();
            //n(n-1) 
            int len = obsFilePaths.Length;
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    var refSite = new ObsSiteInfo(obsFilePaths[i]);
                    var rovSite = new ObsSiteInfo(obsFilePaths[j]);
                    var name = new SiteObsBaseline(rovSite, refSite);
                    //BaseLineName.GetBaseLineName(obsFilePaths[i], obsFilePaths[j]);
                    result.Add(name);
                }
            }
            return result;
        }
    }

    /// <summary>
    /// 全基线生成器
    /// </summary>
    public class TotalBaseLineBuilder : AbstractBuilder<List<GnssBaseLineName>>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="obsFilePaths"></param>
        public TotalBaseLineBuilder(string[] obsFilePaths)
        {
            this.obsFilePaths = obsFilePaths;
        }
        /// <summary>
        /// 观测路径
        /// </summary>
        public string[] obsFilePaths { get; set; }

        public override List<GnssBaseLineName> Build()
        {
            return BuildTotalBaseLines(obsFilePaths);
        }

        /// <summary>
        /// 生成全基线
        /// </summary>
        /// <param name="obsFilePaths"></param>
        /// <returns></returns>
        public static List<GnssBaseLineName> BuildTotalBaseLines(string[] obsFilePaths)
        {
            List<GnssBaseLineName> result = new List<GnssBaseLineName>();
            //n(n-1) 
            int len = obsFilePaths.Length;
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    var name = new GnssBaseLineName(obsFilePaths[j], obsFilePaths[i], true);
                    //BaseLineName.GetBaseLineName(obsFilePaths[i], obsFilePaths[j]);
                    result.Add(name);
                }
            }
            return result;
        }
    }


    /// <summary>
    /// 指定的基线生成器
    /// </summary>
    public class IndicatedBaseLineBuilder : AbstractBuilder<List<GnssBaseLineName>>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="obsFilePaths"></param>
        /// <param name="indicatedLines"></param>
        public IndicatedBaseLineBuilder(string[] obsFilePaths, List<GnssBaseLineName> indicatedLines)
        {
            this.obsFilePaths = obsFilePaths;
            this.IndicatedLines = indicatedLines;
        }
        /// <summary>
        /// 观测路径
        /// </summary>
        public string[] obsFilePaths { get; set; }
        /// <summary>
        /// 指定的线路
        /// </summary>
        public List<GnssBaseLineName> IndicatedLines { get; set; }

        public override List<GnssBaseLineName> Build()
        {
            ObsSiteFileManager totalSite = new ObsSiteFileManager(obsFilePaths, TimeSpan.FromMinutes(1)); 

            List<GnssBaseLineName> result = new List<GnssBaseLineName>();
            foreach (var line in IndicatedLines)
            { 
                if (totalSite.Contains(line.RefName) && totalSite.Contains(line.RovName))
                {
                    result.Add(new GnssBaseLineName(totalSite[line.RefName], totalSite[line.RovName]));
                } 
            }
            return result;
        } 
    }
    /// <summary>
    /// 从同步观测文件中选取观测量最多的独立基线
    /// </summary>
    public class MaxEpochIndepentBaselineSelection
    {
        /// <summary>
        /// 选择独立基线
        /// </summary>
        /// <param name="obsFilePaths">同步网中观测文件</param>
        public MaxEpochIndepentBaselineSelection(string[] obsFilePaths)
        {
            this.ObsFilesPaths = obsFilePaths;
            Init();
        }

        #region 属性
        /// <summary>
        /// 观测文件的路径
        /// </summary>
        public string[] ObsFilesPaths { get; set; }
        public const string BaseLineSplitter = ParamNames.BaseLinePointer;// "->";

        /// <summary>
        /// 返回的选择的独立基线信息，独立基线数=观测文件数-1
        /// </summary>
        public List<GnssBaseLineName> IndepentBaselinesInfo { get; set; }

        /// <summary>
        /// 所有测站数据的观测卫星信息
        /// </summary>
        public SiteEpochSatData[] DataOfAllSites { get; set; }

        //  public Dictionary<string, SiteEpochSatData> DataOfAllSites = new Dictionary<string, SiteEpochSatData>();
        #endregion

        public void IndepentBaselineProcess()
        {
            
            //存储基线
            Dictionary<string, Edge> graphRound = new Dictionary<string, Edge>();
            int n = DataOfAllSites.Length;

            mgraph = new MGraph();
            mgraph.n = n;
            mgraph.e = n * (n - 1) / 2;
            mgraph.vex = new VertexType[n];
            mgraph.edges = new int[n][];

            road = new Road[n * (n - 1) / 2];
            v = new int[n * (n - 1) / 2];

            int ii = 0;
            for (int i = 0; i < n; i++)
            {
                // SiteEpochSatData dataA = DataOfAllSites.ElementAt(i).Value;
                SiteEpochSatData dataA = DataOfAllSites[i];

                mgraph.vex[i] = new VertexType();
                mgraph.vex[i].no = i;
                mgraph.vex[i].name = dataA.SiteName;
                v[i] = i; //顶点ver[i]初始时表示各在不同的连通分支v[i]中，父结点依次为v[i]

                for (int j = i + 1; j < n; j++)
                {
                    mgraph.edges[i] = new int[n];
                    SiteEpochSatData dataB = DataOfAllSites[j];//.ElementAt (j).Value;
                    double toleranceSeccond = 3.5; //限差 单位：秒
                    int count = 0;
                    //for (int k = 0, s = 0; k < dataA.TimeInfo.Length.Data.Count && s < dataB.Data.Count; k++, s++)
                    for (int k = 0, s = 0; k < dataA.TimeInfo.Length && s < dataB.TimeInfo.Length; k++, s++)
                    {
                        //double diff = Math.Abs(dataA.Data.ElementAt(k).Key - dataB.Data.ElementAt(s).Key);
                        double diff = Math.Abs(dataA.TimeInfo[k] - dataB.TimeInfo[s]);
                        if (diff <= toleranceSeccond)
                        {
                            //IEnumerable<int> strB = dataB.EpochSatData.ElementAt(s).Value;
                            string strA = dataA.PrnInfo[k];
                            string strB = dataB.PrnInfo[s];
                            if (strA == null || strB == null) break;
                            for (int ss = 0; ss < strA.Length / 3; ss++)
                            {
                                string prn = strA.Substring(ss * 3, 3);
                                if (strB.Contains(prn))
                                // if (strB.IndexOf(prn)!=-1) //即含有
                                { count += 1; }
                            }
                        }
                        else
                        {
                            // if (dataB.Data.ElementAt(s).Key < dataA.Data.ElementAt(k).Key)
                            if (dataB.TimeInfo[s] < dataA.TimeInfo[k])
                            {
                                while (diff > toleranceSeccond)
                                {
                                    s++;
                                    if (s >= dataB.TimeInfo.Length) break;
                                    //diff = Math.Abs(dataA.Data.ElementAt(k).Key - dataB.Data.ElementAt(s).Key);
                                    diff = Math.Abs(dataA.TimeInfo[k] - dataB.TimeInfo[s]);
                                    if (diff <= toleranceSeccond)
                                    {
                                        string strA = dataA.PrnInfo[k];
                                        string strB = dataB.PrnInfo[s];
                                        if (strA == null || strB == null) break;
                                        for (int ss = 0; ss < strA.Length / 3; ss++)
                                        {
                                            string prn = strA.Substring(ss * 3, 3);
                                            if (strB.Contains(prn))
                                            { count += 1; }
                                        }
                                        break;
                                    }

                                }
                            }
                            else if (dataB.TimeInfo[s] > dataA.TimeInfo[k])
                            {
                                while (diff > toleranceSeccond)
                                {
                                    k++;
                                    if (k >= dataA.TimeInfo.Length) break;
                                    diff = Math.Abs(dataA.TimeInfo[k] - dataB.TimeInfo[s]);
                                    if (diff <= toleranceSeccond)
                                    {
                                        string strA = dataA.PrnInfo[k];
                                        string strB = dataB.PrnInfo[s];
                                        if (strA == null || strB == null) break;
                                        for (int ss = 0; ss < strA.Length / 3; ss++)
                                        {
                                            string prn = strA.Substring(ss * 3, 3);
                                            if (strB.Contains(prn))
                                            { count += 1; }
                                        }
                                        break;
                                    }
                                }
                            }
                        }

                    } //End 完成一条基线的统计

                    mgraph.edges[i][j] = count;
                    road[ii] = new Road();
                    road[ii].a = i;
                    road[ii].b = j;
                    road[ii].w = count;
                    ii++;
                    ////写入
                    //string sb = dataA.SiteName + "-" + dataB.SiteName + " " + count.ToString();
                    //writer.WriteLine(sb);
                    Node NodeA = new Node(); NodeA.Name = dataA.SiteNumber; NodeA.strName = dataA.SiteName; NodeA.Visited = false;NodeA.Tag = dataA.Path ;
                    Node NodeB = new Node(); NodeB.Name = dataB.SiteNumber; NodeB.strName = dataB.SiteName; NodeB.Visited = false; NodeB.Tag = dataB.Path;
                    Edge edge = new Edge();
                    edge.NodeA = NodeA;
                    edge.NodeB = NodeB;
                    edge.Weight = count;
                    string baselineName = dataA.SiteName + dataB.SiteName;
                    graphRound.Add(baselineName, edge);
                }
            }
            //sort 排序 由大到小
            List<KeyValuePair<string, Edge>> graphPair = new List<KeyValuePair<string, Edge>>(graphRound);
            graphPair.Sort(delegate(KeyValuePair<string, Edge> s1, KeyValuePair<string, Edge> s2)
            {
                return s1.Value.Weight.CompareTo(s2.Value.Weight);
            });

            //存储排序基线
            Dictionary<string, Edge> graph = new Dictionary<string, Edge>();
            for (int index = graphPair.Count - 1; index >= 0; index--)
            {
                var item = graphPair.ElementAt(index);
                Edge edge = new Edge(); edge.NodeA = item.Value.NodeA; edge.NodeB = item.Value.NodeB; edge.Weight = item.Value.Weight;
                graph.Add(item.Key, edge);
                //重新排序
                road[mgraph.e - 1 - index].a = DataOfAllSites[item.Value.NodeA.Name].SiteNumber;
                road[mgraph.e - 1 - index].b = DataOfAllSites[item.Value.NodeB.Name].SiteNumber;
                road[mgraph.e - 1 - index].w = item.Value.Weight;
            }

            //根据Kruskal算法生成最小生成树 GetMinCostSpanTree
            List<Edge> findedMinCostSpanTree = new List<Edge>();
            List<List<Node>> findedNodes = new List<List<Node>>();
            findedNodes.Add(new List<Node>() { graph.ElementAt(0).Value.NodeA, graph.ElementAt(0).Value.NodeB });
            findedMinCostSpanTree.Add(graph.ElementAt(0).Value);
            for (int index = 1; index < graph.Count; index++)
            {
                var item = graph.ElementAt(index);
                int i = 0, indexA = -1, indexB = -1;
                Node nodeA = item.Value.NodeA;
                Node nodeB = item.Value.NodeB;
                foreach (var nodes in findedNodes)
                {
                    foreach (var node in nodes)
                    {
                        if (!item.Value.NodeB.Visited && node.Name == nodeB.Name && node.Visited == nodeB.Visited)
                        {
                            item.Value.NodeB.Visited = true;
                            indexB = i;
                        }

                        if (!item.Value.NodeA.Visited && node.Name == nodeA.Name && node.Visited == nodeA.Visited)
                        {
                            item.Value.NodeA.Visited = true;
                            indexA = i;
                        }
                    }
                    i++;
                }
                //
                if (item.Value.NodeA.Visited && item.Value.NodeB.Visited && (indexA != indexB))
                {
                    //连接不同的联通分量，则这两个连通分量可以合并成一个了。
                    int minId = Math.Min(indexA, indexB);
                    int maxId = Math.Max(indexA, indexB);

                    findedNodes[minId].AddRange(findedNodes[maxId]);
                    findedNodes.RemoveAt(maxId);
                    findedMinCostSpanTree.Add(item.Value);
                }
                else if (!item.Value.NodeA.Visited && !item.Value.NodeB.Visited)
                {
                    //都不包含，直接添加新列表
                    findedNodes.Add(new List<Node>() { nodeA, nodeB });
                    findedMinCostSpanTree.Add(item.Value);
                }
                else if (item.Value.NodeA.Visited && !item.Value.NodeB.Visited)
                {
                    //包含A，则将B添加到A的集合中去
                    findedNodes[indexA].Add(nodeB);
                    findedMinCostSpanTree.Add(item.Value);
                }
                else if (!item.Value.NodeA.Visited && item.Value.NodeB.Visited)
                {
                    //包含B，则将A添加到B的集合中去
                    findedNodes[indexB].Add(nodeA);
                    findedMinCostSpanTree.Add(item.Value);
                }
                item.Value.NodeA.Visited = false;
                item.Value.NodeB.Visited = false;
            }
            //writer.WriteLine("\n");
            //writer.WriteLine("根据最大观测量选择的独立基线：  ");
            int jj = 0;
            IndepentBaselinesInfo = new List<GnssBaseLineName>();
            foreach (var item in findedMinCostSpanTree)
            {
                var tmp = new GnssBaseLineName(item.NodeA.strName, item.NodeB.strName)
                {
                    RefFilePath = item.NodeA.Tag + "",
                    RovFilePath = item.NodeB.Tag + "",
                }
                    
                    ;// + BaseLineSplitter + item.NodeB.strName;// + " " + key.Weight;
                IndepentBaselinesInfo.Add( tmp);
                //写入
                //writer.WriteLine(tmp);
                jj++;
            }
            //writer.Close(); //关闭
        }

        /// <summary>
        /// 每个测站文件的观测数据集合
        /// </summary>
        public class SiteEpochSatData
        {
            public SiteEpochSatData()
            {

            }
            /// <summary>
            /// 测站编号
            /// </summary>
            public int SiteNumber { get; set; }
            /// <summary>
            /// 测站名
            /// </summary>
            public string SiteName { get; set; }
            public string Path { get; set; }

            /// <summary>
            /// 观测历元集合
            /// </summary>
            public double[] TimeInfo { get; set; } //时间的数组
            /// <summary>
            /// 观测历元的卫星集合
            /// </summary>
            public string[] PrnInfo { get; set; }  //卫星编号的数组

        }


        #region 最小生成树
        /// <summary>
        /// 初始化，读取赋值
        /// </summary>
        public void Init()
        {
            if (ObsFilesPaths.Length <= 0) { throw new Exception("选择独立基线前，请配置好观测数据！"); }


            //初始化
            DataOfAllSites = new SiteEpochSatData[ObsFilesPaths.Length];
            RinexFileObsDataSource RinexFileObsDataSource = new Data.Rinex.RinexFileObsDataSource(ObsFilesPaths[0], false);
            //加载读取全部文件
            List<ISingleSiteObsStream> obsDataSources = RinexFileObsDataSource.LoadObsData(ObsFilesPaths);

            int siteNumberCount = 0;
            foreach (var obsData in obsDataSources)
            {
                SiteEpochSatData siteEpochSatData = new SiteEpochSatData();
                siteEpochSatData.Path = obsData.Path;
                siteEpochSatData.SiteName = GnssBaseLineName.GetSiteName(obsData.Path);// obsData.SiteInfo.MarkerName.ToUpper();
                siteEpochSatData.SiteNumber = siteNumberCount;
                siteEpochSatData.PrnInfo = new string[2880];
                siteEpochSatData.TimeInfo = new double[2880];
                int jj = 0;
                foreach (var item in obsData)
                {
                    double seconds = item.ReceiverTime.Hour * 60 * 60 + item.ReceiverTime.Minute * 60 + item.ReceiverTime.Seconds;
                    double seconds1 = item.ReceiverTime.SecondsOfDay;
                    if (seconds != seconds1)
                    {
                        throw new Exception("单基线选取中时间统计出错！");
                    }
                    string prnStr = null;
                    foreach (var prn in item.TotalPrns) prnStr += prn.ToString();
                    if (prnStr != null)
                    {
                        siteEpochSatData.TimeInfo[jj] = seconds;
                        siteEpochSatData.PrnInfo[jj] = prnStr;
                        jj++;
                    }
                }
                DataOfAllSites[siteNumberCount] = siteEpochSatData;
                siteNumberCount++;
            }
        }


        /// <summary>
        /// 图/树的存储结构
        /// </summary>
        private MGraph mgraph;
        /// <summary>
        /// 边的存储结构
        /// </summary>
        private Road[] road;
        /// <summary>
        /// 点的存储
        /// </summary>
        private int[] v;



        /// <summary>
        /// 节点
        /// </summary>
        public class Node
        {
            public bool Visited { get; set; }

            public int Name { get; set; }

            public string strName { get; set; }
            /// <summary>
            /// 绑定的对象
            /// </summary>
            public object Tag { get; set; }

        }

        /// <summary>
        /// 边
        /// </summary>
        public class Edge
        {
            public Node NodeA { get; set; }
            public Node NodeB { get; set; }

            public int Weight { get; set; }
        }

        /// <summary>
        /// 结点类型定义
        /// </summary>
        public class VertexType
        {
            public int no;
            public string name;
        }

        /// <summary>
        /// 图的存储结构
        /// </summary>
        public class MGraph
        {
            public int n; //顶点数
            public int e; //所有边数
            public VertexType[] vex; //顶点信息
            public int[][] edges; //各边的权值
        }
        /// <summary>
        /// 边的存储结构
        /// </summary>
        public class Road
        {
            public int a; //边的起点
            public int b; //边的终点 
            public int w; //边的权值
        }

        #endregion



    }
}
