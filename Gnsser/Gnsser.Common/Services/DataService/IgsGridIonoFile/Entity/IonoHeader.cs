//2017.08.16, czs, create in hongqing, 电离层文件的读取

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Geo;
using Geo.Common;
using Gnsser.Times;
using Geo.Times;
using Gnsser.Data.Rinex;

namespace Gnsser.Data
{
    /// <summary>
    /// 增量数据。
    /// </summary>
    public class IncreaseValue
    {
        /// <summary>
        /// 起始数据
        /// </summary>
        public double Start { get; set; }
        /// <summary>
        /// 结束数据
        /// </summary>
        public double End { get; set; }
        /// <summary>
        /// 增量
        /// </summary>
        public double Increament { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public double Count { get { return (End - Start) / Increament; } }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Start + "-" + End + "," + Increament;
        }

        public override bool Equals(object obj)
        {
            IncreaseValue o = obj as IncreaseValue;
            if (o == null) return false;

            return this.Start == o.Start && this.End == o.End && Increament == o.Increament;
        }

        public override int GetHashCode()
        {
            return Start.GetHashCode() + End.GetHashCode() * 13 + Increament.GetHashCode() * 3;
        }
    }

    /// <summary>
    /// 电离层头文件
    /// </summary>
    public class IonoHeader
    {
        /// <summary>
        /// 电离层头文件
        /// </summary>
        public IonoHeader()
        {
            this.FileInfo = new FileInfomation();
            this.SatellitesWithBiasRms = new Dictionary<SatelliteNumber, RmsedNumeral>();
            this.StationsWithBiasRms = new Dictionary<string, RmsedNumeral>();
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public double Version { get; set; }
        /// <summary>
        /// 采样间隔
        /// </summary>
        public double Interval { get; set; }

        public string FileType { get; set; }
        public string SatSysOrTheoModel { get; set; }

        public double ElevatonCutOff { get; set; }
        /// <summary>
        /// 文件信息，通常是创建信息。
        /// </summary>
        public FileInfomation FileInfo { get; set; }
         
        public string Description { get; set; }
        public List<string> Comments { get; set; }
        public Time EpochOfFirstMap { get; set; }
        public Time EpochOfLastMap { get; set; }

        public int NumOfTotalMaps { get; set; }
        public string MappingFunction { get; set; }
        public string ObservablesUsed { get; set; }
        public int NumOfStations { get; set; }
        public int NumOfSatellites { get; set; }


        public double MeanEarthRadius { get; set; }

        public IncreaseValue HeightRange { get; set; }
        public IncreaseValue LatRange { get; set; }
        public IncreaseValue LonRange { get; set; }
         
        public double Exponent { get; set; }

        public string StartOfAuxData { get; set; }

        public string EndOfAuxData { get; set; }

        public string StartOfTecMap { get; set; }
        public string EndOfTecMap { get; set; }

        public Time EpochOfCurrentMap { get; set; } 


        public string StartOfRmsMap { get; set; }
        public string EndOfRmsMap { get; set; }

        public string StartOfHeightMap { get; set; }
        public string EndOfHeightMap { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        public int MapDimension { get; set; }
        /// <summary>
        /// 测站 DIFFERENTIAL CODE BIASES
        /// </summary>
        public Dictionary<string, RmsedNumeral> StationsWithBiasRms { get; set; }
        /// <summary>
        /// 卫星 DIFFERENTIAL CODE BIASES
        /// </summary>
        public Dictionary<SatelliteNumber, RmsedNumeral> SatellitesWithBiasRms { get; set; }

        public int LineNumber { get; set; }
    }
   
}
