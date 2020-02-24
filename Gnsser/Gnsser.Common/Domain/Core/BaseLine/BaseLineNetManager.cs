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
    /// 多网
    /// </summary>
    public class BaseLineNetManager : BaseDictionary<BufferedTimePeriod, BaseLineNet>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseLineNetManager()
        {

        }

        /// <summary>
        /// 数据变动后，应该初始化测站名称
        /// </summary>
        public new void Init()
        {
            var SiteNames = GetSiteNames();

            //检查，确保测站名称对应的否坐标一致
            SiteCoordsManager siteCoords = new SiteCoordsManager();
            foreach (var net in this)
            {
                foreach (var baseLine in net)
                {
                    siteCoords.GetOrCreate(baseLine.BaseLineName.RefName).Add(baseLine.ApproxXyzOfRef);
                    siteCoords.GetOrCreate(baseLine.BaseLineName.RovName).Add(baseLine.ApproxXyzOfRov);
                }
            }
            foreach (var item in siteCoords)
            {
                if (item.Coords.Count > 0 && !item.IsCoordSame())//坐标不一致
                {
                    SetSiteCoord(item.Name, item.Coord);//采用第一个进行赋值
                }
            } 
        }
        public void Add( BaseLineNet  net)
        {
            this.Add(net.TimePeriod, net);
        }
        public void Add(BaseLineNetManager nets)
        {
            foreach (var net in nets)
            {
                this.Add(net.TimePeriod, net);
            }        
        }
        /// <summary>
        /// 是否包含测站名称
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public bool Contains(string siteName)
        {
            foreach (var item in this)
            {
                if (item.Contains(siteName)) return true;
            }
            return false;
        }

        /// <summary>
        /// 所有的测站名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetSiteNames()
        {
            List<string> sites = new List<string>();
            foreach (var item in this)
            {
                sites.AddRange(item.SiteNames);
            }
            return sites.Distinct().ToList();
        }
        /// <summary>
        /// 基线
        /// </summary>
        /// <returns></returns>
        public List<EstimatedBaseline> GetBaseLines()
        {
            List<EstimatedBaseline> lines = new List<EstimatedBaseline>();
            foreach (var item in this)
            {
                lines.AddRange(item);
            }
            return lines;
        }
        /// <summary>
        /// 各时段独立基线集合。
        /// </summary>
        /// <param name="lineSelectType"></param>
        /// <param name="GnssReveiverNominalAccuracy">接收机标称精度</param>
        /// <returns></returns>
        public BaseLineNetManager GetIndependentLines(IndependentLineSelectType lineSelectType)
        {
            var independentLineNet = new BaseLineNetManager();
            foreach (var item in this.KeyValues)
            {
                var lines = item.Value.GetIndependentNet(lineSelectType);
                independentLineNet.Add(item.Key, lines);
            }
            return independentLineNet;
        }

        /// <summary>
        /// 重复基线较差表格
        /// </summary>
        /// <param name="qualities"></param>
        /// <param name="GnssReveiverNominalAccuracy"></param>
        /// <param name="isBadOnly">是否只显示超限的</param>
        /// <returns></returns>
        public ObjectTableStorage BuildRepeatBaselingErrorTable(Dictionary<BufferedTimePeriod, RepeatErrorQualityManager> qualities, GnssReveiverNominalAccuracy GnssReveiverNominalAccuracy, bool isBadOnly = false)
        {
            var asynchClosureError = new ObjectTableManager();
            int netIndex = 1;
            foreach (var pkv in qualities)
            {
                var period = pkv.Key;
                var totalNet = pkv.Value;
                var netName = (netIndex + "-0");
                var data = qualities[period];
                ObjectTableStorage lineTable = BaseLineNet.BuildRepeatBaseLineCheckResultTable(data, netName, isBadOnly);//用于显示，查看

                asynchClosureError.Add(lineTable);
                netIndex++;
            }
            var asyncErrorTable = asynchClosureError.Combine("复测基线较差");
            return asyncErrorTable;
        }

        /// <summary>
        /// 重复基线较差质量检核结果
        /// </summary>
        /// <param name="periodsRepeatErrors"></param>
        /// <param name="GnssReveiverNominalAccuracy"></param>
        /// <returns></returns>
        public Dictionary<BufferedTimePeriod, RepeatErrorQualityManager> BuildRepeatBaselingQulities(MultiPeriodRepeatBaseLineError periodsRepeatErrors, GnssReveiverNominalAccuracy GnssReveiverNominalAccuracy)
        {
            var qualities = new Dictionary<BufferedTimePeriod, RepeatErrorQualityManager>();
            int netIndex = 1;
            BaseLineNet baseNet = null;// phaselNets[0];
            foreach (var pkv in this.KeyValues)
            {
                var period = pkv.Key;
                var totalNet = pkv.Value;


                if (baseNet == null) { baseNet = totalNet; continue; }
                var netName = (netIndex + "-0");

                var asychCloserErrors = periodsRepeatErrors[period];

               var data = baseNet.BuildRepeatBaseLineError(totalNet, asychCloserErrors, GnssReveiverNominalAccuracy, netName);
                qualities[period] = data;
            }
            return qualities;
        }

        /// <summary>
        /// 计算各个时段复测基线较差
        /// </summary>
        /// <returns></returns>
        public MultiPeriodRepeatBaseLineError BuildRepeatBaselineError()
        {
            var periodsRepeatErrors = new MultiPeriodRepeatBaseLineError();

            BaseLineNet baseNet = null;// phaselNets[0];
            foreach (var pkv in this.KeyValues)
            {
                var period = pkv.Key;
                var repeatNet = pkv.Value;
                if (baseNet == null) { baseNet = repeatNet; continue; }

                var asychCloserErrors = baseNet.GetRepeatBaseLineClosureError(repeatNet);
                periodsRepeatErrors[period] = asychCloserErrors;
            }

            return periodsRepeatErrors;
        }


        /// <summary>
        /// 设置坐标
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
        /// 将三角形同步环质量结果生成表格返回，合并到一个表
        /// </summary>
        /// <param name="allQualities"></param>
        /// <returns></returns>
        public ObjectTableStorage BuildSyncTrilateralErrorTable(
            Dictionary<BufferedTimePeriod, TriguilarNetQualitiyManager> allQualities)
        {
            var synchClosureError = new ObjectTableManager();
            int netIndex = 0;
            foreach (var pkv in this.KeyValues)
            {
                var period = pkv.Key;
                var net = pkv.Value;

                var netName = (netIndex++).ToString();

               var qualities = allQualities[period];

                ObjectTableStorage lineTable = BaseLineNet.BuildSynchNetTrilateralCheckTResultable( netName, qualities,period);

                synchClosureError.Add(lineTable);
            }
            var syncErrorTable = synchClosureError.Combine("多时段同步环");
            return syncErrorTable;
        }

        /// <summary>
        /// 提取各个基线质量检核情况
        /// </summary>
        /// <param name="GnssReveiverNominalAccuracy"></param>
        /// <returns></returns>
        public Dictionary<BufferedTimePeriod, TriguilarNetQualitiyManager> BuildTriangularClosureQualies(GnssReveiverNominalAccuracy GnssReveiverNominalAccuracy)
        {
            var result = new Dictionary<BufferedTimePeriod, TriguilarNetQualitiyManager>();
            //同步环闭合差计算 
            int netIndex = 0;
            string name = "Net";
            foreach (var pkv in this.KeyValues)
            {
                var period = pkv.Key;
                var net = pkv.Value;

                var netName = name + (netIndex++).ToString("00");

                var qualities = net.BuildTriangularClosureQualies(GnssReveiverNominalAccuracy);

                result[period] = qualities;
            }
            return result;
        }
         

        /// <summary>
        /// 提取异步GNSS网
        /// </summary>
        /// <param name="pathes"></param>
        /// <param name="periodSpanMinutes">不同时段间隔，间隔内认为是同一时段，单位：分</param>
        /// <returns></returns>
        public static BaseLineNetManager Load(string[] pathes, double periodSpanMinutes)
        {
            var tables = ObjectTableManager.Read(pathes, ".");
            //合并所有的表格
            var table = tables.Combine();
            var phaselNets = BaseLineNetManager.Parse(table, periodSpanMinutes);//时段网
            return phaselNets;
        }

        /// <summary>
        /// 获取近似坐标
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public XYZ GetApproxXyz(string name)
        {
            foreach (var item in this)
            {
                var xyz = item.GetApproxXyz(name);
                if (xyz != null) { return xyz; }
            }
            return null;
        }

        /// <summary>
        /// 提取多时段GNSS网，
        /// </summary>
        /// <param name="table">异步网络基线结果</param>
        /// <param name="periodSpanMinutes">不同时段间隔，间隔内认为是同一时段，单位：分</param>
        /// <returns></returns>
        public static BaseLineNetManager Parse(ObjectTableStorage table, double periodSpanMinutes)
        {
            var list = new List<EstimatedBaseline>();
            var timePeriods = BufferedTimePeriod.GroupToPeriods(table.GetColValues<Time>(ParamNames.Epoch), periodSpanMinutes * 60);
            var data = new BaseLineNetManager();
            foreach (var timePeriod in timePeriods)
            {
                data.Add(timePeriod, new BaseLineNet() { TimePeriod = timePeriod, Name = timePeriod.ToTimeString() });
            }
            foreach (var row in table.BufferedValues)
            {
                EstimatedBaseline obj = EstimatedBaseline.Parse(row);
                foreach (var item in data.KeyValues)
                {
                    if (item.Key.BufferedContains(obj.Epoch))
                    {
                        data[item.Key].Set(obj.BaseLineName, obj);
                    }
                }
            }
            foreach (var item in data.KeyValues)
            {
                item.Value.Init();
            }
            return data;
        }
        /// <summary>
        /// 获取基线或者反基线
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public EstimatedBaseline GetBaseLineOrReversed(GnssBaseLineName line)
        {
            foreach (var item in this.KeyValues)
            {
                var estLine = item.Value.GetOrReversed(line);
                if (estLine != null)
                {
                    return estLine;
                }
            }
            return null;
        }

        /// <summary>
        /// 测站表
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetSiteTable()
        {
            ObjectTableManager tableObjects = new ObjectTableManager();
            int netIndex = 0;
            foreach (var item in this)
            {
                var table = item.GetSiteTable();
                table.Name += netIndex + "_net";
                tableObjects.Add(table);
                netIndex++;
            }
            return tableObjects.Combine();
        }
        /// <summary>
        /// 基线表
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetLineTable()
        {
            int netIndex = 0;
            ObjectTableManager tableObjects = new ObjectTableManager();
            foreach (var item in this)
            {
                var table = item.GetLineTable();
                table.Name += netIndex + "_net";
                tableObjects.Add(table);
                netIndex++; 
            }
            return tableObjects.Combine();

        }
        /// <summary>
        /// 用于显示
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetPhaseTable()
        {
            ObjectTableStorage table = new ObjectTableStorage(this.Name);
            int i = 0;
            foreach (var netKv in this.KeyValues)
            {
                foreach (var item in netKv.Value)
                {
                    table.NewRow();
                    table.AddItem("Number", i);
                    table.AddItem("Name", item.Name);

                    table.AddItem("Phase", netKv.Key);

                }
                i++;
            }

            return table;
        }
        /// <summary>
        /// 获取第一个匹配的结果
        /// </summary>
        /// <param name="lineName"></param>
        /// <returns></returns>
        public EstimatedBaseline GetFirst(GnssBaseLineName lineName)
        {
            foreach (var item in this)
            {
                var line = item.GetOrReversed(lineName);
                if(line != null) { return line; }
            }
            return null;
        }
    }

    /// <summary>
    /// 多时段复测基线较差
    /// </summary>
    public class MultiPeriodRepeatBaseLineError : BaseDictionary<BufferedTimePeriod, Dictionary<GnssBaseLineName, BaseLineRepeatError>>
    {

        public override Dictionary<GnssBaseLineName, BaseLineRepeatError> Create(BufferedTimePeriod key)
        {
            return new Dictionary<GnssBaseLineName, BaseLineRepeatError>();
        }

        /// <summary>
        /// 国标规定，三角形闭合差系数 Math.Sqrt(3) / 5.0;
        /// 按照GB/T 18314-2009，B、C级复测基线长度较差应满足：Wx<= 2 √2 σ 
        /// </summary>
        public double ClousureErrorFactor => 2 * Math.Sqrt(2);

        public bool IsOk(GnssBaseLineName lineName, GnssReveiverNominalAccuracy GnssReveiverNominalAccuracy)
        {
            return true;
        }

        /// <summary>
        /// 最弱边
        /// </summary>
        /// <returns></returns>
        public BaseLineRepeatError GetWorst()
        {
            double error = 0;
            BaseLineRepeatError worst = null;
            foreach (var period in this.KeyValues)
            {
                foreach (var item in period.Value)
                {
                    if (error < item.Value.ErrorLength)
                    {
                        error = item.Value.ErrorLength;
                        worst = item.Value;
                    }
                }
            }
            return worst;
        }
    }
    /// <summary>
    /// 基线复测误差
    /// </summary>
    public class BaseLineRepeatError
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="LineName"></param>
        /// <param name="RmsedXYZ"></param>
        public BaseLineRepeatError(GnssBaseLineName LineName, RmsedXYZ RmsedXYZ)
        {
            this.LineName = LineName;
            this.RmsedXYZ = RmsedXYZ;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public GnssBaseLineName LineName { get; set; }
        /// <summary>
        /// 误差
        /// </summary>
        public RmsedXYZ RmsedXYZ { get; set; }
        /// <summary>
        /// 误差长度,长度闭合差
        /// </summary>
        public double ErrorLength => RmsedXYZ.Value.Length; 


        public override string ToString()
        {
            return LineName + "\t" + ErrorLength;
        }

    }



}