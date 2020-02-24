//2018.11.09, czs, create in HMX, 一条基线的平差结果
//2018.11.10, czs, create in hmx, 增加基线组合类
//2018.11.30, czs, create in hmx, 实现IToTabRow接口，用于规范输出,合并定义新的 BaseLineNet
//2018.12.19, czs, edit in hmx, 三角形网质量
//2019.01.13, czs, create in hmx, 多时段测站管理器

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
using System.Collections;

namespace Gnsser
{

    /// <summary>
    /// 时段性三角形闭合差
    /// </summary>
    public class PeriodTriguilarNetQualitiyManager : BaseDictionary<TimePeriod, TriguilarNetQualitiyManager>
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override TriguilarNetQualitiyManager Create(TimePeriod key)
        {
            return new TriguilarNetQualitiyManager();
        }

        /// <summary>
        /// 对象表
        /// </summary>
        /// <param name="IsBadOnly"></param>
        /// <returns></returns>
        public ObjectTableStorage GetObjectTable(bool IsBadOnly = false)
        {
            //B-E网，GPS
            ObjectTableStorage lineTable = new ObjectTableStorage("多时段三角形同步闭合差");

            foreach (var qualities in this.KeyValues)
            {
                var period = qualities.Key;
                foreach (var kv in qualities.Value.KeyValues)
                {
                    var quality = kv.Value;
                    if (IsBadOnly && quality.IsAllOk)
                    {
                        continue;
                    }
                    lineTable.NewRow();
                    lineTable.AddItem("网编号", period.ToDefualtPathString());
                    //lineTable.AddItem("Index", i++);

                    lineTable.AddItem(quality.GetObjectRow()); 
                    lineTable.AddItem("时段", period.ToString());
                    lineTable.AddItem("闭合路线", kv.Key);
                }
            }
            return lineTable;
        }
    }


    /// <summary>
    /// 异步三角形网质量
    /// </summary>
    public class AsyncTriguilarNetQualitiyManager : BaseDictionary<TriangularNetName, List<QualityOfTriAngleClosureError>>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public AsyncTriguilarNetQualitiyManager()
        {
        }


        /// <summary>
        /// 这条基线是否合限，原理：如果所有都超限了，则肯定超限如果有合限的，则合限。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public QualityOfTriAngleClosureError GetBest(GnssBaseLineName line)
        {
            var lines = this.Get(line);
            if (lines.Count == 0)
            {
                return null;
            }
            lines.Sort();
            return lines[0];
        }

        /// <summary>
        /// 获取最差环
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public QualityOfTriAngleClosureError GetWorst(GnssBaseLineName line)
        {
            var lines = this.Get(line);
            if (lines.Count == 0)
            {
                return null;
            }
            lines.Sort();
            return lines[lines.Count - 1];
        }


        /// <summary>
        /// 获取包含的三角网
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public List<QualityOfTriAngleClosureError> Get(GnssBaseLineName line)
        {
            List<QualityOfTriAngleClosureError> nets = new List<QualityOfTriAngleClosureError>();
            foreach (var item in this.KeyValues)
            {
                if (item.Key.Contains(line)) { nets.AddRange(item.Value); }
            }
            return nets;
        }

        /// <summary>
        /// 编号
        /// </summary>
        /// <returns></returns>
        public BaseDictionary<TimePeriod, int> GetAllPeriodWithOrderNumber()
        {
            BaseDictionary<TimePeriod, int> result = new BaseDictionary<TimePeriod, int>();
            List<TimePeriod> list = GetAlTrimNetPeriodInOrder();
            int i = 1;
            foreach (var item in list)
            {
                result[item] = i++;
            }
            return result;
        }

        private List<TimePeriod> GetAlTrimNetPeriodInOrder()
        {
            List<TimePeriod> timePeriods = new List<TimePeriod>();
            foreach (var kv in this.KeyValues)
            {
                var list = kv.Value;
                var lines = kv.Key;
                foreach (var error in list)
                {
                    foreach (var item in error.BaseLineNet)
                    {
                        timePeriods.Add(item.NetPeriod);
                    } 
                }
            }
            var result = timePeriods.Distinct().ToList();
            result.Sort();
            return result;
        }

        /// <summary>
        /// 对象表
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetObjectTable()
        {
            ObjectTableStorage table = new ObjectTableStorage("异步环闭合差");
            int baseLineIndex = 1; 
            BaseDictionary<TimePeriod, int> periodNums = GetAllPeriodWithOrderNumber();
            foreach (var kv in this.KeyValues)
            {
                var list = kv.Value;
                var lines = kv.Key;
                foreach (var error in list)
                {
                    table.NewRow();
                    table.AddItem("基线号", baseLineIndex++);
                    //  table.AddItem("三边", lines);

                    StringBuilder sb = BuildNetPeriodNumStr(periodNums, error);

                    table.AddItem("三边时段号", sb.ToString());


                    Dictionary<string, object> objRow = error.GetObjectRow();
                    table.AddItem(objRow);

                    StringBuilder sb1 = BuildDetailPeriod(error);

                    table.AddItem("三边时段详情", sb1.ToString());
                }
            }

            return table;
        }

        private static StringBuilder BuildNetPeriodNumStr(BaseDictionary<TimePeriod, int> periodNums, QualityOfTriAngleClosureError error)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var line in error.BaseLineNet)
            {
                if (i > 0) { sb.Append(", "); }
                sb.Append(line.Name);
                var periodNum = periodNums.GetOrCreate(line.NetPeriod);
                sb.Append("[" + periodNum + "]");
                i++;
            }

            return sb;
        }

        private static StringBuilder BuildDetailPeriod(QualityOfTriAngleClosureError error)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var line in error.BaseLineNet)
            {
                if (i > 0) { sb.Append(", "); }
                sb.Append(line.Name);
                sb.Append("[" + line.NetPeriod.ToDefualtPathString() + "]");
                i++;
            }

            return sb;
        }
    }



    /// <summary>
    /// 三角形网质量
    /// </summary>
    public class TriguilarNetQualitiyManager : BaseDictionary<TriangularNetName, QualityOfTriAngleClosureError>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TriguilarNetQualitiyManager()
        { 
        }


        /// <summary>
        /// 这条基线是否合限，原理：如果所有都超限了，则肯定超限如果有合限的，则合限。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public QualityOfTriAngleClosureError GetBest(GnssBaseLineName line)
        {
            var lines = this.Get(line);
            if (lines.Count == 0)
            {
                return null;
            }
            lines.Sort();
            return lines[0];
        }

        /// <summary>
        /// 获取最差环
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public QualityOfTriAngleClosureError GetWorst(GnssBaseLineName line)
        {
            var lines = this.Get(line);
            if (lines.Count == 0)
            {
                return null;
            }
            lines.Sort();
            return lines[lines.Count - 1];
        }


        /// <summary>
        /// 获取包含的三角网
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public List<QualityOfTriAngleClosureError> Get(GnssBaseLineName line)
        {
            List<QualityOfTriAngleClosureError> nets = new List<QualityOfTriAngleClosureError>();
            foreach (var item in this.KeyValues)
            {
                if (item.Key.Contains(line)) { nets.Add(item.Value); }
            }
            return nets;
        }
    }

    /// <summary>
    /// 三角形网质量
    /// </summary>
    public class TriguilarNetManager : BaseDictionary<TriangularNetName, BaseLineNet>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TriguilarNetManager()
        {
        }


        /// <summary>
        /// 这条基线是否合限，原理：如果所有都超限了，则肯定超限如果有合限的，则合限。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public BaseLineNet GetBest(GnssBaseLineName line)
        {
            var lines = this.Get(line);
            if (lines.Count == 0)
            {
                return null;
            }
            lines.Sort();
            return lines[0];
        }
        /// <summary>
        /// 获取包含的三角网
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public List<BaseLineNet> Get(GnssBaseLineName line)
        {
            List<BaseLineNet> nets = new List<BaseLineNet>();
            foreach (var item in this.KeyValues)
            {
                if (item.Key.Contains(line)) { nets.Add(item.Value); }
            }
            return nets;
        }
    }

    /// <summary>
    ///三角形名称，点位顺序不重要
    /// </summary>
    public class TriangularNetName : IEnumerable<string>
    {  /// <summary>
       /// 构造函数。
       /// </summary>
       /// <param name="lineName"></param>
       /// <param name="OtherPoint"></param> 
        public TriangularNetName(BaseLineName lineName, string OtherPoint)
        {
            this.PointA = lineName.RefName;
            this.PointB = lineName.RovName;
            this.PointC = OtherPoint;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="PointA"></param>
        /// <param name="PointB"></param>
        /// <param name="PointC"></param>
        public TriangularNetName(string PointA, string PointB, string PointC)
        {
            this.PointA = PointA;
            this.PointB = PointB;
            this.PointC = PointC;
        }

        /// <summary>
        /// 绑定对象
        /// </summary>
        public Object Tag { get; set; }

        /// <summary>
        /// A
        /// </summary>
        public string PointA { get; set; }
        /// <summary>
        /// A
        /// </summary>
        public string PointB { get; set; }
        /// <summary>
        /// A
        /// </summary>
        public string PointC { get; set; }
        /// <summary>
        /// 名称列表
        /// </summary>
        public List<String> Names => new List<string>() { PointA , PointB, PointC};
        /// <summary>
        /// 数量
        /// </summary>
        public int Count => 3;

        /// <summary>
        /// 包含站点否
        /// </summary>
        /// <param name="pointName"></param>
        /// <returns></returns>
        public bool Contains(string pointName)
        {
            return PointA == pointName
                || PointB == pointName
                || PointC == pointName;
        }
        /// <summary>
        /// 基线名称
        /// </summary>
        /// <returns></returns>
        public List<GnssBaseLineName> GetBaseLineNames()
        {           
            return new List<GnssBaseLineName>()
            {
                LineA,
                LineB,
                LineC
            };
        }
        public GnssBaseLineName LineA => new GnssBaseLineName(PointA, PointB);
        public GnssBaseLineName LineB => new GnssBaseLineName(PointB, PointC);
        public GnssBaseLineName LineC => new GnssBaseLineName(PointC, PointA);
        /// <summary>
        /// 包含基线否
        /// </summary>
        /// <param name="baseLineName"></param>
        /// <returns></returns>
        public bool Contains(GnssBaseLineName baseLineName)
        {
            return Contains(baseLineName.RefName) && Contains(baseLineName.RovName);
        }

        public override bool Equals(object obj)
        {
            var o = obj as TriangularNetName;
            if (o == null) { return false; }

            return this.Contains(o.PointA)
                && this.Contains(o.PointB)
                && this.Contains(o.PointC);
        }
        public override int GetHashCode()
        {
            return PointA.GetHashCode() + PointB.GetHashCode() + PointC.GetHashCode();
        }
        public override string ToString()
        {
            return PointA + "-" + PointB + "-" + PointC;
        }
        /// <summary>
        /// 查找所有的三角回路
        /// </summary>
        /// <param name="lineNames"></param>
        /// <param name="siteNames"></param>
        /// <returns></returns>
        public static List<TriangularNetName> BuildTriangularNetNames(List<GnssBaseLineName> lineNames, List<string> siteNames)
        {
            var length = lineNames.Count;
            List<TriangularNetName> list = new List<TriangularNetName>();
            foreach (var line in lineNames)
            {
                foreach (var site in siteNames)
                {
                    if (line.Contains(site)) { continue; }
                    var lineA = new GnssBaseLineName(line.RefName, site);
                    var lineB = new GnssBaseLineName(line.RovName, site);
                    if(GnssBaseLineName.ContainsOrReversedContains(lineNames, lineA)
                        && GnssBaseLineName.ContainsOrReversedContains(lineNames, lineB)
                        )
                    {
                        var net = new TriangularNetName(line, site);
                        list.Add(net);
                    }
                }
            }
            var result = list.Distinct().ToList();
            return result; 
        }

        /// <summary>
        /// 创建所有三角形名称
        /// </summary>
        /// <param name="siteNames"></param>
        /// <returns></returns>
        public static List<TriangularNetName> BuildTriangularNetNames(List<string> siteNames)
        {
            var length = siteNames.Count;
            List<TriangularNetName> list = new List<TriangularNetName>();
            for (int i = 0; i < length; i++)
            {
                var a = siteNames[i];
                for (int j = i + 1; j < length; j++)
                {
                    var b = siteNames[j];
                    for (int k = j + 1; k < length; k++)
                    {
                        var c = siteNames[k];
                        var net = new TriangularNetName(a, b, c);
                        list.Add(net);
                    }
                }
            }
            return list;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return Names.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}