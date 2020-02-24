//2017.07.21, czs, create in hongiqng, DOP计算
//2017.10.09, czs, edit in hongqing, 改进，采用实际数据计算

using System;
using System.Collections.Generic;
using System.Text;
using Gnsser.Domain;
using Geo.Coordinates;
using Gnsser.Data.Rinex; 
using System.Linq;
using Gnsser.Data.Sinex;
using Geo.Algorithm.Adjust;
using Geo;
using Geo.IO;
using Geo.Algorithm;  
using Geo.Data; 
using Gnsser;
using Gnsser.Service;
using Gnsser.Data;
using Geo.Times; 
using Geo.Utils;

namespace Gnsser.Service
{ 
    /// <summary>
    /// 计算DOP值
    /// </summary>
    public class DopSolver
    {
        Log log = new Log(typeof(DopSolver));

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="EphemerisService"></param>
        /// <param name="CutOffAngle"></param>
        /// <param name="EnabledPrns"></param>
        /// <param name="SatWeights"></param>
        /// <param name="outDirectory"></param>
        /// <param name="TimeLooper"></param>
        /// <param name="GeoGridLooper"></param>
        public DopSolver(IEphemerisService EphemerisService, 
            double CutOffAngle, 
            string outDirectory, 
            List<SatelliteNumber> EnabledPrns, 
            SatWeightTable SatWeights, 
            TimeLooper TimeLooper, 
            GeoGridLooper GeoGridLooper)
        {
            this.OutputDirectory = outDirectory;
            this.EphemerisService = EphemerisService;
            this.EnabledPrns = EnabledPrns;
            this.SatWeights = SatWeights;
            this.CutOffAngle = CutOffAngle;
            this.GeoGridLooper = GeoGridLooper;

            var satTypes = SatelliteNumber.GetSatTypes(EnabledPrns);
            this.FileNamePrefix = Geo.Utils.EnumerableUtil.ToString<SatelliteType>(satTypes, "-");

            this.TimeLooper = TimeLooper;
            this.TimeLooper.Looping += TimeLooper_Looping;
            this.MaxDopThreshold = double.MaxValue;
        }

        #region 属性
        /// <summary>
        /// 最大阈值限制
        /// </summary>
        public double MaxDopThreshold { get; set; }
        /// <summary>
        /// 星历服务
        /// </summary>
        public IEphemerisService EphemerisService { get; set; }
        /// <summary>
        /// 卫星编号
        /// </summary>
        public List<SatelliteNumber> EnabledPrns { get; set; }
        /// <summary>
        /// 卫星权值
        /// </summary>
        public SatWeightTable SatWeights { get; set; }

        /// <summary>
        /// 是否采用简略模式，即不输出经纬度，空数据采用N替代。
        /// </summary>
        public bool IsSimpleModel { get; set; }
        /// <summary>
        /// 名称前缀。
        /// </summary>
        public string FileNamePrefix { get; set; } 
        /// <summary>
        /// 输出目录
        /// </summary>
        public string OutputDirectory { get; set; }
        /// <summary>
        /// 高度截止角
        /// </summary>
        public double CutOffAngle { get; set; }
        /// <summary>
        /// 地理网格遍历器
        /// </summary>
        public GeoGridLooper GeoGridLooper { get; set; }
        /// <summary>
        /// 时间遍历器
        /// </summary>
        public TimeLooper TimeLooper { get; set; }
        #endregion


        void TimeLooper_Looping(Time Time)
        {
            log.Info("正在计算 " + Time);
            ObjectTableManager TableManager = new Geo.ObjectTableManager(OutputDirectory);


            var Table = TableManager.AddTable(FileNamePrefix + "_DOPS_at_" + Geo.Utils.DateTimeUtil.GetDateTimePathString(Time.DateTime));
            GeoGridLooper NewGeoGridLooper = GeoGridLooper.Clone();
            var DopCaculator = new DopCaculator(EphemerisService, EnabledPrns, CutOffAngle, SatWeights);
            NewGeoGridLooper.Looping += new Action<LonLat>(delegate(LonLat geo)
            {
                NewGeoGridLooper.IsCancel = GeoGridLooper.IsCancel;
                var geoCoord = new GeoCoord(geo.Lon, geo.Lat, 10);
                Build(DopCaculator, Table, Time, geoCoord, IsSimpleModel);
            });
            NewGeoGridLooper.Init();
            NewGeoGridLooper.Run();
            TableManager.WriteAllToFileAndCloseStream();
            log.Info("计算完毕 " + Time);
        }

        /// <summary>
        /// 顺序计算
        /// </summary>
        public void Solve() { TimeLooper.Run(); }
        /// <summary>
        /// 并行计算
        /// </summary>
        public void SolveAsync() { TimeLooper.LoopAsync(Environment.ProcessorCount); }

        /// <summary>
        /// 构建。
        /// </summary>
        /// <param name="DopCaculator"></param>
        /// <param name="table"></param>
        /// <param name="Time"></param>
        /// <param name="geoCoord"></param>
        private void Build(DopCaculator DopCaculator, Geo.ObjectTableStorage table, Time Time, Geo.Coordinates.GeoCoord geoCoord, bool isSimpleModel)
        {
            //var xyz = Geo.Coordinates.CoordTransformer.GeoCoordToXyz(geoCoord);
            //if (geoCoord.Lon == 0)
            //{

            //    var geoCoord2 = Geo.Coordinates.CoordTransformer.XyzToGeoCoord(xyz);
            //    int i = 0; 
            //}
            var dop = DopCaculator.Calculate(geoCoord, Time);
            if (dop != null)
            {
                table.NewRow();
                //table.AddItem("Epoch", Time);
                //table.AddItem("X", xyz.X);
                //table.AddItem("Y", xyz.Y);
                //table.AddItem("Z", xyz.Z);
                if (!isSimpleModel)
                {
                    table.AddItem("Lon", geoCoord.Lon);
                    table.AddItem("Lat", geoCoord.Lat);
                }
                //table.AddItem("Heigh", geoCoord.Height);
                table.AddItem("SatCount", dop.SatCount);
                if (Geo.Utils.DoubleUtil.IsValid(dop.GDop))
                {
                    table.AddItem("GDOP", GetValNotExceed(dop.GDop));
                    table.AddItem("PDOP", GetValNotExceed(dop.PDop));
                    table.AddItem("HDOP", GetValNotExceed(dop.HDop));
                    table.AddItem("VDOP", GetValNotExceed(dop.VDop));
                    table.AddItem("TDOP", GetValNotExceed(dop.TDop));
                }
                table.EndRow();
            }
            else if (isSimpleModel) //如果没有经纬度，则采用N替代，否书数据空缺很危险
            {
                table.NewRow();
                //table.AddItem("Epoch", Time);
                //table.AddItem("X", xyz.X);
                //table.AddItem("Y", xyz.Y);
                //table.AddItem("Z", xyz.Z);
                //table.AddItem("Lon", geoCoord.Lon);
                //table.AddItem("Lat", geoCoord.Lat);
                //table.AddItem("Heigh", geoCoord.Height);
                table.AddItem("SatCount", "N");
                table.AddItem("GDOP", "N");
                table.AddItem("PDOP", "N");
                table.AddItem("HDOP", "N");
                table.AddItem("VDOP", "N");
                table.AddItem("TDOP", "N");
                table.EndRow();
            }
        }

        public double GetValNotExceed(double val)
        {
            if (val > MaxDopThreshold) return MaxDopThreshold;
            return val;
        }
    }

    /// <summary>
    /// 计算具体的某次DOP。
    /// </summary>
    public class DopCaculator
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="EphemerisService"></param>
        /// <param name="Prns"></param>
        /// <param name="CutOffAngle"></param>
        /// <param name="SatWeights"></param>
        public DopCaculator(IEphemerisService EphemerisService, List<SatelliteNumber> Prns, double CutOffAngle, SatWeightTable SatWeights)
        {
            this.EphemerisService = EphemerisService;
            this.Prns = Prns;
            this.SatWeights = SatWeights;
            this.CutOffAngle = CutOffAngle;
        }

        #region 属性
        /// <summary>
        ///星历服务器
        /// </summary>
        IEphemerisService EphemerisService { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        Geo.Times.Time Time { get; set; }
        /// <summary>
        /// 高度截止角
        /// </summary>
        public double CutOffAngle { get; set; }
        /// <summary>
        /// 卫星加权
        /// </summary>
        public SatWeightTable SatWeights { get; set; }
        /// <summary>
        /// 卫星编号
        /// </summary>
        List<SatelliteNumber> Prns { get; set; }
        #endregion

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="xyz"></param>
        /// <param name="Time"></param>
        /// <returns></returns>
        public DopValue Calculate(XYZ xyz, Time Time)
        {
           // var prns = this.EphemerisService.Prns.FindAll(m=>this.SatelliteTypes.Contains(m.SatelliteType));
            this.Time = Time;
             
            List<Ephemeris> ephs = new List<Ephemeris>();
            foreach (var prn in Prns)
            {
                if (SatWeights != null && !SatWeights.Contains(prn))
                {
                    continue;
                }
                 ephs.Add(this.EphemerisService.Get(prn, Time)); 
            }
            var sats = EphemerisUtil.GetSatsInVisible(xyz, ephs, CutOffAngle);
            if (sats.Count < 4) { return null; }

            return GetDopValue(xyz, sats);
        }

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="geoCoord"></param>
        /// <param name="Time"></param>
        /// <returns></returns>
        public DopValue Calculate(GeoCoord geoCoord, Time Time)
        {
           // var prns = this.EphemerisService.Prns.FindAll(m=>this.SatelliteTypes.Contains(m.SatelliteType));
            this.Time = Time;
             
            List<Ephemeris> ephs = new List<Ephemeris>();
            foreach (var prn in Prns)
            {
                if (SatWeights != null && !SatWeights.Contains(prn))
                {
                    continue;
                }
                 ephs.Add(this.EphemerisService.Get(prn, Time)); 
            }
            var sats = EphemerisUtil.GetSatsInVisible(geoCoord, ephs, CutOffAngle);
            var xyz = CoordTransformer.GeoCoordToXyz(geoCoord);
            if(sats.Count == 0) { return null; }
            if (sats.Count < 4) { return new DopValue() { StationXYZ = xyz, Time = sats[0].Time, SatCount= sats.Count}; }
            return GetDopValue(xyz, sats);
        }
        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="xyz"></param>
        /// <param name="sats"></param>
        /// <returns></returns>
        public DopValue GetDopValue(XYZ xyz, List<Ephemeris> sats)
        {

            Matrix A = BiuldA(xyz, sats);
            Matrix P = BiuldWeight(sats);

            var N = A.Trans * P * A;
            var InverN = N.Inversion;

            var dialoge = InverN.GetDiagonal();
            DopValue DopValue = new DopValue() { StationXYZ = xyz, Time = sats[0].Time };
            DopValue.GDop = Math.Sqrt(DoubleUtil.Sum(dialoge));
            DopValue.PDop = Math.Sqrt(DoubleUtil.Sum(dialoge.GetSubVector(0, 3)));
            DopValue.HDop = Math.Sqrt(DoubleUtil.Sum(dialoge.GetSubVector(0, 2)));
            DopValue.VDop = Math.Sqrt(DoubleUtil.Sum(dialoge.GetSubVector(0, 1)));
            DopValue.TDop = Math.Sqrt(DoubleUtil.Sum(dialoge.GetSubVector(3, 1)));
            DopValue.SatCount = sats.Count;

            return DopValue;
        }
        /// <summary>
        /// 构建A矩阵
        /// </summary>
        /// <param name="stationPos"></param>
        /// <param name="sats"></param>
        /// <returns></returns>
        private  Matrix BiuldA(XYZ stationPos, List<Ephemeris> sats)
        {
            Matrix A = new Matrix(sats.Count, 4);
            int i = 0;
            foreach (var eph in sats)
            {
                var vector = (eph.XYZ - stationPos);
                A[i, 0] = vector.CosX;
                A[i, 1] = vector.CosY;
                A[i, 2] = vector.CosZ;
                A[i, 3] = -1;

                i++;
            }
            return A;
        }
        /// <summary>
        /// 权阵获取，如果无提供器，则采用默认 1.
        /// </summary>
        /// <param name="sats"></param>
        /// <returns></returns>
        private  Matrix BiuldWeight(List<Ephemeris> sats)
        {
             Matrix P = new Matrix(sats.Count);
            int i = 0;
            foreach (var eph in sats)
            {
                double p = 1;
                if (SatWeights != null )
                {
                    var val  = SatWeights.Get(eph.Time,eph.Prn);
                    if (Geo.Utils.DoubleUtil.IsValid(val))
                    {
                        p = val;
                    }
                }
                P[i, i] = p;

                i++;
            }
            return P;            
        }
    }


    /// <summary>
    /// DOP 数值
    /// </summary>
    public class DopValue
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public DopValue()
        {
            GDop = Double.NaN;
            PDop = Double.NaN;
            VDop = Double.NaN;
            HDop = Double.NaN; 
            TDop = Double.NaN;    
    }
        /// <summary>
        /// 时间
        /// </summary>
        public Time Time { get; set; }
        /// <summary>
        /// 测站坐标
        /// </summary>
        public XYZ StationXYZ { get; set; }
        /// <summary>
        /// 测站大地坐标
        /// </summary>
        public GeoCoord StationGeoCoord { get; set; }
        /// <summary>
        /// 卫星数量
        /// </summary>
        public int SatCount { get; set; }
        /// <summary>
        /// 空间精度衰减因子
        /// </summary>
        public double GDop { get; set; }
        /// <summary>
        /// 空间精度衰减因子
        /// </summary>
        public double PDop { get; set; }
        /// <summary>
        /// 水平经度因子
        /// </summary>
        public double HDop { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double VDop { get; set; }
        /// <summary>
        /// 时间精度因子
        /// </summary>
        public double TDop { get; set; }

    }

}
