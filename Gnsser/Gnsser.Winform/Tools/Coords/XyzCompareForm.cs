//2016.11.29, czs, edit in 洪庆, 增加 Gamit ORG 文件
//2016.12.26, cuiyang, edit in 洪庆, 增加基线生成和比较
//2018.12.24, czs, edit in ryd, 增加新基线比较支持
//2018.12.27, czs, edit in ryd, 支持基线反向比较
//2019.01.19, czs, edit in hmx, 基线比较增加时间比较

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms; 
using Gnsser.Data.Sinex; 
using Geo.Utils;
using Geo.Coordinates;
using AnyInfo; 
using System.Drawing.Drawing2D;
using System.Windows.Forms.DataVisualization.Charting;
using Gnsser;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service; 
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo;
using Geo.IO;


namespace Gnsser.Winform
{
    public partial class XyzCompareForm : Form, IShowLayer
    {
        Log log = new Log(typeof(XyzCompareForm));
        public XyzCompareForm()
        {
            InitializeComponent();
         //   Geo.Coordinates.NamedXyz
        }

        public event ShowLayerHandler ShowLayer;
        bool IsInUnitMm => checkBox_unitMm.Checked;
        List<NamedXyz> CoordsA { get; set; }
        List<NamedXyz> CoordsB { get; set; }
        /// <summary>
        /// 显示坐标
        /// </summary>
        List<NamedXyz> ShowCoordsA { get; set; }
        /// <summary>
        /// 显示坐标
        /// </summary>
        List<NamedXyz> ShowCoordsB { get; set; } 
        List<NamedXyzEnu> CompareResults { get; set; }
        Geo.EnableInteger NameLength => enabledIntControl_nameLength.GetEnabledValue();
        private void button_readB_Click(object sender, EventArgs e) { Read(); }

        private void Read()
        {
            string pathA = fileOpenControlA.FilePath;

            if (!File.Exists(pathA))
            {
                FormUtil.ShowFileNotExistBox(pathA);
                return;
            }
            string pathB = this.fileOpenControlB.FilePath;
            if (!File.Exists(pathB))
            {
                FormUtil.ShowFileNotExistBox(pathB);
                return;
            }
            bool isBaseLine = checkBox_isBaseline.Checked;

            if (!isBaseLine)//坐标的比较
            {
                this.CoordsA = NamedXyzParser.GetCoords(pathA);
                this.CoordsB = NamedXyzParser.GetCoords(pathB);
                //此用于地图显示
                this.ShowCoordsA = this.CoordsA;
                this.ShowCoordsB = this.CoordsB;

                ObjectTableStorage tableA = BuildObjectTable(CoordsA);
                ObjectTableStorage tableB = BuildObjectTable(CoordsB);
                objectTableControl_tableA.DataBind(tableA);
                objectTableControl_tableB.DataBind(tableB);

                this.Compared = NamedXyz.Compare(CoordsA, CoordsB, NameLength.Enabled, NameLength.Value);

                CompareResults = new List<NamedXyzEnu>();
                var nameLen = NameLength;
                foreach (var localXyz in Compared)
                {
                   // var name = GetCuttedName(localXyz.Name, nameLen);

                    var staXyz = CoordsA.Find(m => String.Equals(GetCuttedName(m.Name, nameLen), GetCuttedName(localXyz.Name, nameLen), StringComparison.CurrentCultureIgnoreCase));
                    if (staXyz == null) { continue; }
                    var item = NamedXyzEnu.Get(localXyz.Name, localXyz.Value, new XYZ(staXyz.X, staXyz.Y, staXyz.Z));
                    CompareResults.Add(item);
                }
            }
            else//基线选择与输出
            {
                try
                {
                    var path = pathA;
                    ObjectTableStorage tableA = ParseLineTable(pathA);
                    ObjectTableStorage tableB = ParseLineTable(pathB);
                    if (tableA == null || tableB == null)
                    {
                        Geo.Utils.FormUtil.ShowWarningMessageBox("不支持的基线格式！");
                        return;
                    }
                    this.CoordsA = PareToNamedXyz(tableA);
                    this.CoordsB = PareToNamedXyz(tableB);

                    objectTableControl_tableA.DataBind(tableA);
                    objectTableControl_tableB.DataBind(tableB);

                    var netA = MultiPeriodBaseLineNet.Parse(tableA);
                    var netB = MultiPeriodBaseLineNet.Parse(tableB);

                    //此用于地图显示
                    this.ShowCoordsA = netA.GetSiteCoords();
                    this.ShowCoordsB = netB.GetSiteCoords();

                    var compared = MultiPeriodBaseLineNet.Compare(netA, netB);
                    CompareResults = new List<NamedXyzEnu>();
                    foreach (var item in compared)
                    {
                        CompareResults.AddRange(item.Value);
                    }

                }
                catch (Exception ex)
                {
                    Geo.Utils.FormUtil.ShowErrorMessageBox(ex.Message + ",发生了错误\r\n注意：文件内只能有一条同名基线！"); return;
                }
            }
            ObjectTableStorage table = BuildObjectTable(CompareResults);
            //转换为毫米单位
            if (IsInUnitMm) { table.UpdateAllBy(1000, NumeralOperationType.乘); };
            this.objectTableControl_result.DataBind(table);


            //更进一步，计算偏差RMS
            var residualRms = table.GetResidualRmse();
            var meanError = table.GetAbsMean();
            var vector = table.GetAveragesWithStdDev();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("参数\t平均误差\t互差中误差\t系统偏差\t系统差中误差");
            var format = "G4";
            foreach (var item in vector)
            {
                sb.AppendLine(item.Key +"\t" + meanError[item.Key][0].ToString(format) + "\t" + residualRms[item.Key][0].ToString(format) + "\t" + item.Value[0].ToString(format) + "\t" + item.Value[1].ToString(format));
            }
            var info = sb.ToString();
            MessageBox.Show(info);
            log.Info(info);

            ObjectTableStorage summeryTable = new ObjectTableStorage("结果汇总");
            summeryTable.NewRow();
            summeryTable.AddItem("Name", "MeanError");
            foreach (var item in meanError)
            {
                summeryTable.AddItem(item.Key, item.Value[0].ToString(format));
            }
            summeryTable.NewRow();
            summeryTable.AddItem("Name", "MutualDevRms");
            foreach (var item in residualRms)
            {
                summeryTable.AddItem(item.Key, item.Value[0].ToString(format));
            }
            summeryTable.NewRow();
            summeryTable.AddItem("Name", "AveOrSysDev");
            foreach (var item in vector)
            {
                summeryTable.AddItem(item.Key, item.Value[0].ToString(format));
            }
            summeryTable.NewRow();
            summeryTable.AddItem("Name", "AveDevRms");
            foreach (var item in vector)
            {
                summeryTable.AddItem(item.Key, item.Value[1].ToString(format));
            }
            summeryTable.EndRow();


            objectTableControl_ave.DataBind(summeryTable);
        }

        private static string GetCuttedName(string name, EnableInteger nameLen)
        {
            if (nameLen.Enabled)
            {
                name = Geo.Utils.StringUtil.SubString(name, 0, nameLen.Value);
            }

            return name;
        }

        private static ObjectTableStorage ParseLineTable(string path)
        {
            ObjectTableStorage tableA = null;
            if (path.ToUpper().EndsWith("ASC"))
            {
                tableA = ParseAscLine(path); 
            }
            else if(path.EndsWith(Setting.TextTableFileExtension))
            {
                tableA = ObjectTableReader.Read(path);
            }

            return tableA;
        }

        private static ObjectTableStorage ParseAscLine(string path)
        {
            ObjectTableStorage Table = null;
            LgoAscBaseLineFileReader reader = new LgoAscBaseLineFileReader(path);
            var BaseLineFile = reader.Read();
            var lines = BaseLineFile.GetBaseLineNetManager();

            Table = lines.GetLineTable();
            return Table;
        }

        private static List<NamedXyz> PareToNamedXyz(ObjectTableStorage tableA)
        {
            List<NamedXyz> list = new List<NamedXyz>();
            foreach (var row in tableA.BufferedValues)
            {
                EstimatedBaseline obj = EstimatedBaseline.Parse(row);
                list.Add(new NamedXyz(obj.Name, obj.EstimatedVector));
            }
            return list;
        } 

        List<NamedXyz> Compared { get; set; }
    
        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            List<NamedXyz> coordsB = this.ShowCoordsA;
             
            if (ShowLayer != null && coordsB != null)
            {
                List<AnyInfo.Geometries.Point> lonlats = new List<AnyInfo.Geometries.Point>(); 
                int i = 1;
                foreach (var g in coordsB)
                {
                    var find = this.ShowCoordsB.Find(m => String.Equals(m.Name, g.Name, StringComparison.CurrentCultureIgnoreCase));
                    if (find == null) continue;

                    var geoCoord = Geo.Coordinates.CoordTransformer.XyzToGeoCoord(g.Value);
                    var name = find.Name;// + ","  + find.Value.ToString();
                    lonlats.Add(new AnyInfo.Geometries.Point(geoCoord,  (i++) + "", name));
                } 
                Layer layer = LayerFactory.CreatePointLayer(lonlats);
                ShowLayer(layer);
            }
            else
            {
                MessageBox.Show("请先读取！");
            }
        }

        private void button_draw_Click(object sender, EventArgs e)
        {        
            if (CompareResults != null)
            {
                DrawDifferLine(CompareResults);
            }
            else
            {
                MessageBox.Show("请先读取！");
            }
        }

        public void DrawDifferLine(List<NamedXyzEnu> differs)
        { 

            if (differs == null || differs.Count == 0) return;
            int index = 0; 
            //E
            Series seriesE = new Series("E");
            seriesE.ChartType = SeriesChartType.Column;

            foreach (var item in differs.ToArray())
            {
                index++;
                seriesE.Points.Add(new DataPoint(index, item.E));

            }
            //N
            index = 0;
            Series seriesN = new Series("N");
            seriesN.ChartType = SeriesChartType.Column;
            foreach (var item in differs.ToArray())
            {
                index++;
                seriesN.Points.Add(new DataPoint(index, item.N ));

            }
            //U
            index = 0;
            Series seriesU = new Series("U");
            seriesU.ChartType = SeriesChartType.Column;
            foreach (var item in differs.ToArray())
            {
                index++;
                seriesU.Points.Add(new DataPoint(index, item.U));

            }
            //Len
            index = 0;
            Series seriesLen = new Series("Len");
            seriesLen.ChartType = SeriesChartType.Line;
            foreach (var item in differs.ToArray())
            {
                index++;
                seriesLen.Points.Add(new DataPoint(index, item.Len));

            }

            Geo.Winform.CommonChartForm form = new Geo.Winform.CommonChartForm(seriesE, seriesN, seriesU, seriesLen);
            form.Text = "Residual Bias Count " + differs.Count;
            form.Show();
        }



        #region 独立基线

        /// <summary>
        /// 根据路径选择独立基线
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        private string[] IndepentBaselineProcess(List<NamedXyz> coords)
        {
            string[] IndepentBaselinesInfo = new string[coords.Count - 1];
            //存储基线
            Dictionary<string, Edge> graphRound = new Dictionary<string, Edge>();
            int n = coords.Count;

            mgraph = new MGraph();
            mgraph.n = n;
            mgraph.e = n * (n - 1) / 2;
            mgraph.vex = new VertexType[n];
            mgraph.edges = new double[n][];

            road = new Road[n * (n - 1) / 2];
            v = new int[n * (n - 1) / 2];

            int ii = 0;
            for (int i = 0; i < n; i++)
            {
                // SiteEpochSatData dataA = DataOfAllSites.ElementAt(i).Value;
                //SiteEpochSatData dataA = DataOfAllSites[i];
                NamedXyz dataA = coords[i];

                mgraph.vex[i] = new VertexType();
                mgraph.vex[i].no = i;
                mgraph.vex[i].name = dataA.Name;//.SiteName;
                v[i] = i; //顶点ver[i]初始时表示各在不同的连通分支v[i]中，父结点依次为v[i]

                for (int j = i + 1; j < n; j++)
                {
                    mgraph.edges[i] = new double[n];
                    // SiteEpochSatData dataB = DataOfAllSites[j];//.ElementAt (j).Value;
                    NamedXyz dataB = coords[j];
                    //double toleranceSeccond = 3.5; //限差 单位：秒
                    double count = (dataA.Value - dataB.Value).Length;

                    mgraph.edges[i][j] = count;
                    road[ii] = new Road();
                    road[ii].a = i;
                    road[ii].b = j;
                    road[ii].w = count;
                    ii++;
                    ////写入
                    //string sb = dataA.SiteName + "-" + dataB.SiteName + " " + count.ToString();
                    //writer.WriteLine(sb);
                    Node NodeA = new Node(); NodeA.Name = i; NodeA.strName = dataA.Name; NodeA.Visited = false;
                    Node NodeB = new Node(); NodeB.Name = j; NodeB.strName = dataB.Name; NodeB.Visited = false;
                    Edge edge = new Edge();
                    edge.NodeA = NodeA;
                    edge.NodeB = NodeB;
                    edge.Weight = count;
                    string baselineName = dataA.Name + dataB.Name;
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
            for (int index = 0; index < graphPair.Count; index++)
            {
                var item = graphPair.ElementAt(index);
                Edge edge = new Edge(); edge.NodeA = item.Value.NodeA; edge.NodeB = item.Value.NodeB; edge.Weight = item.Value.Weight;
                graph.Add(item.Key, edge);
                //重新排序
                road[mgraph.e - 1 - index].a = item.Value.NodeA.Name;
                road[mgraph.e - 1 - index].b = item.Value.NodeB.Name;
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
            foreach (var item in findedMinCostSpanTree)
            {
                string tmp = item.NodeA.strName + "-" + item.NodeB.strName;// + " " + key.Weight;
                IndepentBaselinesInfo[jj] = tmp;
                //写入
                //writer.WriteLine(tmp);
                jj++;
            }
            //writer.Close(); //关闭

            return IndepentBaselinesInfo;
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
        private class Node
        {
            public bool Visited { get; set; }

            public int Name { get; set; }

            public string strName { get; set; }

        }

        /// <summary>
        /// 边
        /// </summary>
        private class Edge
        {
            public Node NodeA { get; set; }
            public Node NodeB { get; set; }

            public double Weight { get; set; }
        }

        /// <summary>
        /// 结点类型定义
        /// </summary>
        private class VertexType
        {
            public int no;
            public string name;
        }

        /// <summary>
        /// 图的存储结构
        /// </summary>
        private class MGraph
        {
            public int n; //顶点数
            public int e; //所有边数
            public VertexType[] vex; //顶点信息
            public double[][] edges; //各边的权值
        }
        /// <summary>
        /// 边的存储结构
        /// </summary>
        private class Road
        {
            public int a; //边的起点
            public int b; //边的终点 
            public double w; //边的权值
        }
        #endregion

        private void XyzCompareForm_Load(object sender, EventArgs e)
        {
            fileOpenControlA.Filter = Setting.CoordAndBaseLineFileFilter;
            fileOpenControlB.Filter = Setting.CoordAndBaseLineFileFilter;

            fileOpenControlA.FilePath = Setting.GnsserConfig.SampleSinexFile;
            log.Info("默认坐标基准文件：" + Setting.GnsserConfig.SampleSinexFile);

            var files = Directory.GetFiles(Setting.TempDirectory, "*"+Setting.SiteCoordFileExtension);
            if(files.Length > 0)
            {
                fileOpenControlB.FilePath = files[0];
                log.Info("采用了临时目录的测站坐标文件：" + fileOpenControlB.FilePath);
            }else if (File.Exists(Setting.GnsserConfig.TempPppResultPath))
            {
                fileOpenControlB.FilePath = Setting.GnsserConfig.TempPppResultPath;//.SampleSinexFile;
                log.Info("采用了计算的坐标文件：" + Setting.GnsserConfig.TempPppResultPath);
            }
            else
            {
                fileOpenControlB.FilePath = Setting.GnsserConfig.PppResultFile;//.SampleSinexFile;
                log.Info("采用了默认的坐标文件：" + Setting.GnsserConfig.PppResultFile);
            }
           // fileOpenControlB.FilePath = Setting.GnsserConfig.PppResultFile;//.SampleSinexFile;
            
        }

        private static ObjectTableStorage BuildObjectTable(List<NamedXyzEnu> NamedXyzEnus, string name = "比较结果")
        {
            ObjectTableStorage table = new ObjectTableStorage(name);
            foreach (var item in NamedXyzEnus)
            {
                table.NewRow();
                table.AddItem(Geo.Utils.ObjectUtil.GetPropertyName<NamedXyzEnu>(m => m.Name), item.Name);
                table.AddItem(Geo.Utils.ObjectUtil.GetPropertyName<NamedXyzEnu>(m => m.X), item.X);
                table.AddItem(Geo.Utils.ObjectUtil.GetPropertyName<NamedXyzEnu>(m => m.Y), item.Y);
                table.AddItem(Geo.Utils.ObjectUtil.GetPropertyName<NamedXyzEnu>(m => m.Z), item.Z);
                table.AddItem(Geo.Utils.ObjectUtil.GetPropertyName<NamedXyzEnu>(m => m.E), item.E);
                table.AddItem(Geo.Utils.ObjectUtil.GetPropertyName<NamedXyzEnu>(m => m.N), item.N);
                table.AddItem(Geo.Utils.ObjectUtil.GetPropertyName<NamedXyzEnu>(m => m.U), item.U);
                table.AddItem(Geo.Utils.ObjectUtil.GetPropertyName<NamedXyzEnu>(m => m.Len), item.Len);
            }

            return table;
        }
        private static ObjectTableStorage BuildObjectTable(List<NamedXyz> NamedXyzEnus, string name = "计算结果")
        {
            ObjectTableStorage table = new ObjectTableStorage(name);
            
            foreach (var item in NamedXyzEnus)
            {
                var geo = Geo.Coordinates.CoordTransformer.XyzToGeoCoord(item.Value);

                table.NewRow();
                table.AddItem(Geo.Utils.ObjectUtil.GetPropertyName<NamedXyzEnu>(m => m.Name), item.Name);
                table.AddItem(Geo.Utils.ObjectUtil.GetPropertyName<NamedXyzEnu>(m => m.X), item.X);
                table.AddItem(Geo.Utils.ObjectUtil.GetPropertyName<NamedXyzEnu>(m => m.Y), item.Y);
                table.AddItem(Geo.Utils.ObjectUtil.GetPropertyName<NamedXyzEnu>(m => m.Z), item.Z);
                table.AddItem(Geo.Utils.ObjectUtil.GetPropertyName<NamedXyzEnu>(m => m.Len), item.Len);
                table.AddItem(Geo.Utils.ObjectUtil.GetPropertyName<GeoCoord>(m => m.Lon), geo.Lon);
                table.AddItem(Geo.Utils.ObjectUtil.GetPropertyName<GeoCoord>(m => m.Lat), geo.Lat);
                table.AddItem(Geo.Utils.ObjectUtil.GetPropertyName<GeoCoord>(m => m.Height), geo.Height);
            }

            return table;
        }
    }
}
