//2019.01.09, czs, create in hmx, 全站仪结果

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo;
using Geo.Coordinates;

namespace Gnsser.Data
{
    /// <summary>
    /// 全站仪文件
    /// </summary>
    public class TotalStationResultFile
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TotalStationResultFile()
        {
            DirectionResults = new BaseDictionary<GnssBaseLineName, TotalStationDirectionResult>();
            DistanceResults = new BaseDictionary<GnssBaseLineName, TotalStationDistanceResult>();
            AdjustCoordResults = new BaseDictionary<string, TotalStationAdjustCoordResult>();
            CombinedResults = new BaseDictionary<GnssBaseLineName, TotalStationCombinedResult>();
            ApproxCoords = new BaseDictionary<string, XY>();
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 方向结果
        /// </summary>
        public BaseDictionary<GnssBaseLineName, TotalStationDirectionResult> DirectionResults { get; set; }
        /// <summary>
        /// 距离平差结果
        /// </summary>
        public BaseDictionary<GnssBaseLineName, TotalStationDistanceResult> DistanceResults { get; set; }
        /// <summary>
        /// 组合平差结果
        /// </summary>
        public BaseDictionary<GnssBaseLineName, TotalStationCombinedResult> CombinedResults { get; set; }
        /// <summary>
        /// 近似坐标
        /// </summary>
        public BaseDictionary<string, XY> ApproxCoords { get; set; }
        /// <summary>
        /// 平差结果
        /// </summary>
        public BaseDictionary<string, TotalStationAdjustCoordResult> AdjustCoordResults { get; set; }
        /// <summary>
        /// 近似坐标
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetApproxTable()
        {
            ObjectTableStorage table = new ObjectTableStorage("近似坐标");
            foreach (var item in ApproxCoords.KeyValues)
            {
                table.NewRow();
                table.AddItem("Name", item.Key);
                table.AddItem("X", item.Value.X);
                table.AddItem("Y", item.Value.Y);
            }
            return table;
        }
        /// <summary>
        /// 方向平差结果
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetDirectionTable()
        {
            ObjectTableStorage table = new ObjectTableStorage("方向平差结果");
            foreach (var item in this.DirectionResults.KeyValues)
            {
                var obj = item.Value;
                table.NewRow();
                table.AddItem("Name", item.Key);
                table.AddItem("AdjustValue", obj.AdjustValue);
                table.AddItem("AdjustDms", obj.AdjustDms);
                table.AddItem("MeasureValue", obj.MeasureValue);
                table.AddItem("MeasureDms", obj.MeasureDms);
                table.AddItem("MSec", obj.MSec);
                table.AddItem("Ri", obj.Ri);
            }
            return table;
        }
        /// <summary>
        /// 距离平差结果
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetDistanceTable()
        {
            ObjectTableStorage table = new ObjectTableStorage("距离平差结果");
            foreach (var item in this.DistanceResults.KeyValues)
            {
                var obj = item.Value;
                table.NewRow();
                table.AddItem("Name", item.Key);
                table.AddItem("AdjustValue", obj.AdjustValue);
                table.AddItem("MeasureValue", obj.MeasureValue);
                table.AddItem("MSec", obj.MSec);
                table.AddItem("Ri", obj.Ri);
            }
            return table;
        }
        /// <summary>
        /// 平差坐标及其精度
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetCoordTable()
        {
            ObjectTableStorage table = new ObjectTableStorage("平差坐标及其精度");
            foreach (var item in this.AdjustCoordResults.KeyValues)
            {
                var obj = item.Value;
                table.NewRow();
                table.AddItem("Name", item.Key);
                table.AddItem("X", obj.XY.X);
                table.AddItem("Y", obj.XY.Y);
                if (obj.MXY != null)
                {
                    table.AddItem("MX", obj.MXY.X);
                    table.AddItem("MY", obj.MXY.Y);
                    table.AddItem("MP", obj.MP);
                }
            }
            return table;
        }
        /// <summary>
        /// 网点间边长、方位角
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetCombinedTable()
        {
            ObjectTableStorage table = new ObjectTableStorage("网点间边长、方位角");
            foreach (var item in this.CombinedResults.KeyValues)
            {
                var obj = item.Value;
                table.NewRow();
                table.AddItem("Name", item.Key);
                table.AddItem("Distance", obj.Distance);
                table.AddItem("Azimuth", obj.Azimuth);
                table.AddItem("AzimuthDMS", obj.AzimuthDMS);
                table.AddItem("MA", obj.MA); 
            }
            return table;
        }
    }
    /// <summary>
    /// 方向结果
    /// </summary>
    public class TotalStationDirectionResult : TotalStationDistanceResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="LineName"></param>
        public TotalStationDirectionResult(GnssBaseLineName LineName): base(LineName)
        { 
        } 
        /// <summary>
        ///平差结果 度分秒格式
        /// </summary>
        public DMS AdjustDms => new DMS(AdjustValue);
        /// <summary>
        ///测量结果 度分秒格式
        /// </summary>
        public DMS MeasureDms => new DMS(MeasureValue);
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return AdjustDms.ToString();
        } 
    }

    /// <summary>
    /// 距离平差结果
    /// </summary>
    public class TotalStationDistanceResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="LineName"></param>
        public TotalStationDistanceResult(GnssBaseLineName LineName)
        {
            this.LineName = LineName;
        }
        /// <summary>
        /// 基线名称
        /// </summary>
        public GnssBaseLineName LineName { get; set; }
        /// <summary>
        /// L S 
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// RI 
        /// </summary>
        public double Ri { get; set; }
        /// <summary>
        ///  V(sec) 
        /// </summary>
        public double VSec { get; set; }
        /// <summary>
        /// M(sec)
        /// </summary>
        public double MSec { get; set; }
        /// <summary>
        /// 测量结果
        /// </summary>
        public double MeasureValue { get; set; }
        /// <summary>
        /// 平差结果
        /// </summary>
        public double AdjustValue { get; set; } 
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return AdjustValue.ToString();
        }
    }
    /// <summary>
    /// 平差结果
    /// </summary>
    public class TotalStationAdjustCoordResult
    {
        public string Name { get; set; }

        public XY XY { get; set; }
        public XY MXY { get; set; }
        public double MP { get; set; }
        public double E { get; set; }
        public double F { get; set; }
        public double T { get; set; } 
    }

    /// <summary>
    /// 网点间边长、方位角及其相对精度
    /// </summary>
    public class TotalStationCombinedResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="LineName"></param>
        public TotalStationCombinedResult(GnssBaseLineName LineName)
        {
            this.LineName = LineName;
        }
        /// <summary>
        /// 基线名称
        /// </summary>
        public GnssBaseLineName LineName { get; set; }
        /// <summary>
        /// A 
        /// </summary>
        public double Azimuth { get; set; }
        /// <summary>
        /// A 
        /// </summary>
        public DMS AzimuthDMS => new DMS(Azimuth);
        /// <summary>
        ///  V(sec) 
        /// </summary>
        public double MA { get; set; }
        /// <summary>
        /// M(sec)
        /// </summary>
        public double Distance { get; set; }
        /// <summary>
        /// MS
        /// </summary>
        public double MS { get; set; }
        /// <summary>
        /// DisPerMs
        /// </summary>
        public double DisPerMs { get; set; }
        /// <summary>
        /// E
        /// </summary>
        public double E { get; set; }
        /// <summary>
        /// F
        /// </summary>
        public double F { get; set; }
        /// <summary>
        /// T
        /// </summary>
        public double T { get; set; }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return LineName.ToString();
        }
    }
}
