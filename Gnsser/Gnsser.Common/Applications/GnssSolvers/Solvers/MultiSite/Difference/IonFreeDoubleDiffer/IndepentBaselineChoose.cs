//2015.11.19, cy, 从n个测站中找出n-1条独立基线
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.IO;
using Geo;


using Gnsser.Domain;
using System.Text;
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

namespace Gnsser.Service
{
    
    public class IndepentBaselineChoose 
    {
       /// <summary>
        /// 选择独立基线
        /// </summary>
        /// <param name="obsPaths"></param>
        public IndepentBaselineChoose( string[] obsPaths)
        {
            this.files = obsPaths;
            Init();
        }

        /// <summary>
        /// 观测文件路径
        /// </summary>
        public string[] files { get; set; }


       

        ///// <summary>
        ///// 初始以Time、GPS等设计
        ///// </summary>
        //public void InitDetect()
        //{
        //    if (files.Length <= 0) { throw new Exception("选择独立基线前，请配置好观测数据！"); }

        //    DataOfAllSites = new Dictionary<string, SiteEpochSatData>();
        //    int m = 0;
        //    foreach (var file in files)
        //    {
        //        Gnsser.Data.Rinex.FileStreamObsDataSource reader = new FileStreamObsDataSource(file, false);

        //        SiteEpochSatData siteEpochSatData = new SiteEpochSatData();
        //        siteEpochSatData.SiteName = reader.SiteInfo.MarkerName.ToUpper();
        //        siteEpochSatData.SiteNumber = m;

        //        m++;
        //        foreach (var key in reader)
        //        {
        //            siteEpochSatData.Data.Add(key.Time, key.Prns);

        //            string[] prns = new string[key.Prns.Count];
        //            int kk = 0;
        //            foreach (var prn in key.Prns)
        //            {
        //                prns[kk] = prn.ToString();
        //                kk++;
        //            }

        //            siteEpochSatData.EpochSatData.Add(key.Time, prns);

        //        }

        //        DataOfAllSites.Add(reader.SiteInfo.MarkerName.ToUpper(), siteEpochSatData);
        //    }
        //}

        /// <summary>
        /// 更改简洁版
        /// </summary>
        public void Init()
        {
            if (files.Length <= 0) { throw new Exception("选择独立基线前，请配置好观测数据！"); }

            DataOfAllSites = new Dictionary<string, SiteEpochSatData>();
            int m = 0;
            foreach (var file in files)
            {
                var reader = new RinexFileObsDataSource(file, false);

                SiteEpochSatData siteEpochSatData = new SiteEpochSatData();
                siteEpochSatData.SiteName = reader.SiteInfo.SiteName.ToUpper();
                siteEpochSatData.SiteNumber = m;

                m++;
                foreach (var item in reader)
                {
                    double seconds = item.ReceiverTime.Hour * 60 * 60 + item.ReceiverTime.Minute * 60 + item.ReceiverTime.Seconds;

                    double seconds1 = item.ReceiverTime.SecondsOfDay;
                    if (seconds != seconds1)
                    {
                        throw new Exception("单基线选取中时间统计出错！");
                    }
                    int[] prn = new int[item.EnabledPrns.Count];
                    for (int j = 0; j < item.EnabledPrns.Count; j++) prn[j] = item.EnabledPrns[j].PRN;


                    siteEpochSatData.Data.Add(seconds, prn);

                    //string[] prns = new string[key.Prns.Count];
                    //int kk = 0;
                    //foreach (var prn in key.Prns)
                    //{
                    //    prns[kk] = prn.ToString();
                    //    kk++;
                    //}

                    siteEpochSatData.EpochSatData.Add(seconds, prn);

                }

                DataOfAllSites.Add(reader.SiteInfo.SiteName.ToUpper(), siteEpochSatData);
            }
        }



        MGraph mgraph;
        Road[] road;
        int[] v;
        public void IndepentBaselineProcess()
        {
            //比较计算
           // var name = DateTime.Now.ToString("yyyy-MM-dd_HH");
            string outPath = Path.GetDirectoryName(files[0]) + "\\" + "baseline.param";
            StreamWriter writer = new StreamWriter(outPath);


            //存储基线
            Dictionary<string, Edge> graphRound = new Dictionary<string, Edge>();

            int n = DataOfAllSites.Count;

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
                SiteEpochSatData dataA = DataOfAllSites.ElementAt(i).Value;

                mgraph.vex[i] = new VertexType();
                mgraph.vex[i].no = i;
                mgraph.vex[i].name = dataA.SiteName;
                v[i] = i; //顶点ver[i]初始时表示各在不同的连通分支v[i]中，父结点依次为v[i]

                for (int j = i + 1; j < n; j++)
                {
                    mgraph.edges[i] = new int[n];

                    SiteEpochSatData dataB = DataOfAllSites.ElementAt(j).Value;

                    double toleranceSeccond = 0.5; //限差 单位：秒
                    int count = 0;

                    for (int k = 0, s = 0; k < dataA.Data.Count && s < dataB.Data.Count; k++, s++)
                    {
                        double diff = Math.Abs(dataA.Data.ElementAt(k).Key - dataB.Data.ElementAt(s).Key);
                        
                        if (diff <= toleranceSeccond)
                        {

                            IEnumerable<int> strA = dataA.EpochSatData.ElementAt(k).Value;
                            IEnumerable<int> strB = dataB.EpochSatData.ElementAt(s).Value;

                            IEnumerable<int> comPrn = strA.Intersect(strB);                         
   
                            //bool isIntersected = strA.Intersect(strB).Count() > 0;
    
                            count += comPrn.Count();


                            int[] iA = dataA.EpochSatData.ElementAt(k).Value;
                            int[] iB = dataB.EpochSatData.ElementAt(s).Value;
                            int lengthA = iA.Length;
                            int lengthB = iB.Length;

                            int maxLength = Math.Max(lengthA, lengthB);

                            int kk = 0, nn = 0, mm = 0;

                            for (int ss = 0; ss < maxLength; ss++, kk++, nn++)
                            {

                                if (kk >= lengthA) break;
                                if (nn >= lengthB) break;

                                while (iA[kk] != iB[nn])
                                {
                                    if (iA[kk] > iB[nn]) { nn++; if (nn >= lengthB) break; }
                                    if (iA[kk] < iB[nn]) { kk++; if (kk >= lengthA) break; }

                                }

                                if (kk >= lengthA) break;
                                if (nn >= lengthB) break;

                                if (iA[kk] == iB[nn]) { mm++; }

                            }

                            if (mm != comPrn.Count())
                            {
                                //
                            }


                            //foreach (var key in dataA.Data.ElementAt(k).Value)
                            //{
                            //    if (dataB.Data.ElementAt(s).Value.Contains(key))
                            //    { count += 1; }
                            //}

                          
                        }
                        else
                        {
                            if (dataB.Data.ElementAt(s).Key < dataA.Data.ElementAt(k).Key)
                            {
                                while (diff > toleranceSeccond)
                                {
                                    s++;
                                    if (s >= dataB.Data.Count) break;

                                    diff = Math.Abs(dataA.Data.ElementAt(k).Key - dataB.Data.ElementAt(s).Key);
                                    if (diff <= toleranceSeccond)
                                    {
                                        IEnumerable<int> strA = dataA.EpochSatData.ElementAt(k).Value;
                                        IEnumerable<int> strB = dataB.EpochSatData.ElementAt(s).Value;

                                        IEnumerable<int> comPrn = strA.Intersect(strB);
                                        count += comPrn.Count();


                                        //foreach (var key in dataA.Data.ElementAt(k).Value)
                                        //{
                                        //    if (dataB.Data.ElementAt(s).Value.Contains(key))
                                        //    { count += 1; }
                                        //}
                                        break;
                                    }

                                }
                            }
                            else if (dataB.Data.ElementAt(s).Key > dataA.Data.ElementAt(k).Key)
                            {
                                while (diff > toleranceSeccond)
                                {
                                    k++;
                                    if (k >= dataA.Data.Count) break;
                                    diff = Math.Abs(dataA.Data.ElementAt(k).Key - dataB.Data.ElementAt(s).Key);
                                    if (diff <= toleranceSeccond)
                                    {
                                        IEnumerable<int> strA = dataA.EpochSatData.ElementAt(k).Value;
                                        IEnumerable<int> strB = dataB.EpochSatData.ElementAt(s).Value;

                                        IEnumerable<int> comPrn = strA.Intersect(strB);
                                        count += comPrn.Count();

                                        //foreach (var key in dataA.Data.ElementAt(k).Value)
                                        //{
                                        //    if (dataB.Data.ElementAt(s).Value.Contains(key))
                                        //    { count += 1; }
                                        //}
                                        break;
                                    }

                                }
                            }
                        }

                    } //完成一条基线的统计

                    mgraph.edges[i][j] = count;
                    road[ii] = new Road();
                    road[ii].a = i;
                    road[ii].b = j;
                    road[ii].w = count;
                    ii++;

                    //写入
                    string sb = dataA.SiteName + "-" + dataB.SiteName + " " + count.ToString();
                    writer.WriteLine(sb);

                 
                    Node NodeA = new Node(); NodeA.Name = dataA.SiteName; NodeA.Visited = false;
                    Node NodeB = new Node(); NodeB.Name = dataB.SiteName; NodeB.Visited = false;

                    Edge edge = new Edge();
                    edge.NodeA = NodeA;
                    edge.NodeB = NodeB;
                    edge.Weight = count;
                    string baselineName = NodeA.Name + NodeB.Name;

                    graphRound.Add(baselineName, edge);

                }
            }



            //sort
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



            //Kruskal GetMinCostSpanTree

            List<Edge> finded = new List<Edge>();
            List<List<Node>> findedNodes = new List<List<Node>>();

            findedNodes.Add(new List<Node>() { graph.ElementAt(0).Value.NodeA, graph.ElementAt(0).Value.NodeB });

            finded.Add(graph.ElementAt(0).Value);

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
                    finded.Add(item.Value);
                }
                else if (!item.Value.NodeA.Visited && !item.Value.NodeB.Visited)
                {
                    //都不包含，直接添加新列表
                    findedNodes.Add(new List<Node>() { nodeA, nodeB });
                    finded.Add(item.Value);
                }
                else if (item.Value.NodeA.Visited && !item.Value.NodeB.Visited)
                {
                    //包含A，则将B添加到A的集合中去
                    findedNodes[indexA].Add(nodeB);
                    finded.Add(item.Value);
                }
                else if (!item.Value.NodeA.Visited && item.Value.NodeB.Visited)
                {
                    //包含B，则将A添加到B的集合中去
                    findedNodes[indexB].Add(nodeA);
                    finded.Add(item.Value);
                }

                item.Value.NodeA.Visited = false;
                item.Value.NodeB.Visited = false;

            }


            writer.WriteLine("\n");
            writer.WriteLine("根据最大观测量选择的独立基线：  ");

            foreach (var item in finded)
            {
                string tmp = item.NodeA.Name + "-" + item.NodeB.Name + " " + item.Weight;
                //写入
                writer.WriteLine(tmp);
            }

            writer.Close(); //关闭


        }



        /// <summary>
        /// 每个测站文件的观测数据集合
        /// </summary>
        public class SiteEpochSatData
        {

            public int SiteNumber { get; set; }

            public string SiteName { get; set; }

           
            // public Dictionary<Time, List<SatelliteNumber>> Data = new Dictionary<Time, List<SatelliteNumber>>();
            public Dictionary<double, int[]> Data = new Dictionary<double, int[]>();




           // public Dictionary<Time, string[]> EpochSatData = new Dictionary<Time, string[]>();
            public Dictionary<double, int[]> EpochSatData = new Dictionary<double, int[]>();

 
        }

        public Dictionary<string, SiteEpochSatData> DataOfAllSites = new Dictionary<string, SiteEpochSatData>();


        /// <summary>
        /// 取得顶点的根节点，从而得到连通分支
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public int GetRoot(int a)
        {
            while (a != v[a])
            {
                a = v[a];
            }
            return a;
        }


        public class Node
        {
            public bool Visited { get; set; }

            public string Name { get; set; }
        }

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

        public class Road
        {
            public int a; //边的起点
            public int b; //边的终点 
            public int w; //边的权值
        }

    }
}
