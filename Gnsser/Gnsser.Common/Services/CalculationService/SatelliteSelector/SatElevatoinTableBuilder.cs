//2017.02.12, czs, create in hongqing, 卫星高度角计算器。
//2017.02.16, czs, edit in hongqing, 重构优化
//2018.06.22. czs, edit in hmx,重构，支持指定时间范围
//2018.09.07, czs, edit in hmx, 封装静态方法，提供调用
//2019.01.14, czs, edit in hmx, 默认读取自带导航文件。

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using System.Text; 
using Gnsser;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Domain;
using Gnsser.Data.Rinex;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Times;
using Gnsser.Service;
using Geo.IO;
using Geo;
using Gnsser.Checkers;
using System.Collections.Concurrent;

namespace Gnsser
{

    /// <summary>
    /// 卫星高度角表格生成器。
    /// </summary>
    public class SatElevatoinTableBuilder : AbstractBuilder<ObjectTableManager>
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="obsPathes"></param>
        /// <param name="satTypes"></param>
        /// <param name="outInterval"></param> 
        public SatElevatoinTableBuilder(string[] obsPathes, List<SatelliteType> satTypes, double outInterval = 30, double AngleCut = 0)
        {
            this.SatelliteTypes = satTypes;
            this.OutInterval = outInterval;
            this.ObsPathes = obsPathes;
            this.NamePostfix = "SatEle";
            this.AngleCut = AngleCut;
            this.EphemerisService = GlobalNavEphemerisService.Instance;// GlobalIgsEphemerisService.Instance;
            this.OutputDirectory = Setting.TempDirectory;
            this.SiteCoords = new Dictionary<string, XYZ>();
            Time StartTime = new Time();
            Time endTime = new Time();
            foreach (var filePath in ObsPathes)
            {
                var header = new RinexObsFileReader(filePath, false).GetHeader();
                var siteXyz = header.SiteInfo.ApproxXyz;
                var fileName = Path.GetFileName(filePath);
                var tableName = fileName + "_" + NamePostfix;
                SiteCoords[tableName] = siteXyz;
                if (endTime != header.EndTime)
                {
                    endTime = header.EndTime;
                }
                if (StartTime != header.StartTime)
                {
                    StartTime = header.StartTime;
                }
            }
            TimeLooper = new TimeLooper(new TimePeriod(StartTime, endTime), outInterval);
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="obsPathes"></param>
        /// <param name="satTypes"></param>
        /// <param name="outInterval"></param>
        /// <param name="TimeLooper">若指定星历文件，则只可以处理具体星历日期的数据。</param>
        public SatElevatoinTableBuilder(string[] obsPathes, List<SatelliteType> satTypes, TimeLooper TimeLooper, double outInterval = 30, double AngleCut = 0)
        {
            this.SatelliteTypes = satTypes;
            this.OutInterval = outInterval;
            this.ObsPathes = obsPathes;
            this.NamePostfix = "SatEle";
            this.AngleCut = AngleCut;
            this.EphemerisService = GlobalNavEphemerisService.Instance;// GlobalIgsEphemerisService.Instance;
            this.OutputDirectory = Setting.TempDirectory;
            this.SiteCoords = new Dictionary<string, XYZ>();
            foreach (var filePath in ObsPathes)
            {
                var header = new RinexObsFileReader(filePath, false).GetHeader();
                var siteXyz = header.SiteInfo.ApproxXyz;
                var fileName = Path.GetFileName(filePath);
                var tableName = fileName + "_" + NamePostfix;
                SiteCoords[tableName] = siteXyz;
            }
            this.TimeLooper = TimeLooper;
        }

        /// <summary>
        /// 卫星高度角生成器
        /// </summary>
        /// <param name="SiteCoords"></param>
        /// <param name="TimeLooper"></param>
        /// <param name="satTypes"></param>
        /// <param name="AngleCut"></param>
        public SatElevatoinTableBuilder(Dictionary<string, XYZ> SiteCoords, TimeLooper TimeLooper, List<SatelliteType> satTypes = null, double AngleCut = 0)
        {
            if (satTypes == null) { satTypes = new List<SatelliteType>() { SatelliteType.G }; }
            this.TimeLooper = TimeLooper;
            this.AngleCut = AngleCut;
            this.EphemerisService = GlobalIgsEphemerisService.Instance;
            this.OutputDirectory = Setting.TempDirectory;
            this.SiteCoords = SiteCoords;
        }

        #region 属性
        public Dictionary<string, XYZ> SiteCoords { get; set; }
        /// <summary>
        /// 输出目录
        /// </summary>
        public string OutputDirectory { get; set; }
        /// <summary>
        /// 星历服务
        /// </summary>
        public IEphemerisService EphemerisService { get; set; }
        /// <summary>
        /// 输出的采样间隔
        /// </summary>
        public double OutInterval { get; set; }
        /// <summary>
        /// 卫星类型
        /// </summary>
        public List<SatelliteType> SatelliteTypes { get; set; }
        /// <summary>
        /// 时间迭代器
        /// </summary>
        public TimeLooper TimeLooper { get; set; }
        /// <summary>
        /// 输入路径
        /// </summary>
        public string[] ObsPathes { get; set; }
        /// <summary>
        /// 表的后缀名，以区别其他表格。
        /// </summary>
        public string NamePostfix { get; set; }
        /// <summary>
        /// 高度截止角
        /// </summary>
        public double AngleCut { get; set; }
        public ObjectTableManager TableObjectManager { get; set; }

        /// <summary>
        /// 进度通知接口
        /// </summary>
        public IProgressViewer ProgressViewer { get; set; }
        #endregion


        /// <summary>
        /// 生成表
        /// </summary>
        /// <returns></returns>
        public override ObjectTableManager Build()
        {
            if (ProgressViewer != null)
            {
                ProgressViewer.InitProcess(SiteCoords.Count);
            }
            ConcurrentDictionary<string, ObjectTableStorage> dic = new ConcurrentDictionary<string, ObjectTableStorage>();
            //并行提速
            Parallel.ForEach<KeyValuePair<string, XYZ>>(SiteCoords, kv =>
            {

                if (ProgressViewer != null)
                {
                    ProgressViewer.PerformProcessStep();
                }
                var table = BuildTable(kv.Value, kv.Key);
                if (table != null)
                {
                    dic.TryAdd(table.Name, table);
                }
            });
            TableObjectManager = new ObjectTableManager(dic, Int16.MaxValue / 2, OutputDirectory, "卫星高度角");
            if (ProgressViewer != null)
            {
                ProgressViewer.Full();
            }

            return TableObjectManager;
        }

        /// <summary>
        /// 一个测站，一个文件。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private ObjectTableStorage BuildTable(string filePath)
        {
            var header = new RinexObsFileReader(filePath, false).GetHeader();
            var siteXyz = header.SiteInfo.ApproxXyz;


            var fileName = Path.GetFileName(filePath);
            var tableName = fileName + "_" + NamePostfix;
            //var ObsDataSource = new RinexFileObsDataSource(filePath); 
            return BuildTable(siteXyz, tableName);
        }
        private ObjectTableStorage BuildTable(XYZ siteXyz, string tableName)
        {
            return BuildTable(TimeLooper, siteXyz, SatelliteTypes, AngleCut, EphemerisService, tableName);
        }
        /// <summary>
        /// 生成一个卫星高度角表格
        /// </summary>
        /// <param name="TimeLooper"></param>
        /// <param name="siteXyz"></param>
        /// <param name="SatelliteTypes"></param>
        /// <param name="AngleCut"></param>
        /// <param name="EphemerisService"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static ObjectTableStorage BuildTable(
        TimeLooper TimeLooper,
        XYZ siteXyz,
        List<SatelliteType> SatelliteTypes,
        double AngleCut = 10,
        IEphemerisService EphemerisService = null,
        string tableName = "卫星高度角")
        {
            return BuildTable(TimeLooper.TimePeriod.Start, TimeLooper.TimePeriod.End, TimeLooper.StepInSeconds, siteXyz, SatelliteTypes, AngleCut, EphemerisService, tableName);
        }
        /// <summary>
        /// 生成一个表格
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="interval"></param>
        /// <param name="siteXyz"></param>
        /// <param name="SatelliteTypes"></param>
        /// <param name="AngleCut"></param>
        /// <param name="EphemerisService"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static ObjectTableStorage BuildTable(
            Time startTime, Time endTime, double interval, 
            XYZ siteXyz,
            List<SatelliteType> SatelliteTypes,
            double AngleCut = 10,
            IEphemerisService EphemerisService = null,
            string tableName="卫星高度角")
        {
            var table = new ObjectTableStorage(tableName);

            if (siteXyz == null) { return null; }
            for (Time time = startTime; time <= endTime; time += interval)
            {
                table.NewRow();
                table.AddItem("Epoch", time);

                foreach (var sat in EphemerisService.Prns)
                {
                    if (!SatelliteTypes.Contains(sat.SatelliteType)) { continue; }

                    var eph = EphemerisService.Get(sat, time);
                    if (eph == null) { continue; }

                    var p = CoordTransformer.XyzToGeoPolar(eph.XYZ, siteXyz);
                    var ele = p.Elevation;
                    if (ele >= AngleCut)
                    {
                        table.AddItem(sat, ele);
                    }
                }
                table.EndRow();
            }
            return table;
        }

        /// <summary>
        /// 通过文件构建,按照文件坐标计算，按照其采样率输出。
        /// </summary>
        /// <param name="oFilePath"></param>
        /// <param name="EphemerisService"></param>
        /// <param name="tableName"></param>
        /// <param name="AngleCut"></param>
        /// <returns></returns>
        public static ObjectTableStorage BuildTable(string oFilePath, double AngleCut = 12, IEphemerisService EphemerisService = null, string tableName = null)
        {
            if (String.IsNullOrWhiteSpace(tableName))
            {
                tableName = Path.GetFileName(oFilePath)+ "-卫星高度角";
            }
            RinexObsFileReader reader = new RinexObsFileReader(oFilePath);
            var header = reader.GetHeader();
            var siteXyz = header.ApproxXyz;
            if(EphemerisService == null)
            {
                if (File.Exists(header.NavFilePath))//优先考虑自带导航文件
                {
                    EphemerisService = EphemerisDataSourceFactory.Create(header.NavFilePath);
                }
                else
                { 
                    EphemerisService = GlobalNavEphemerisService.Instance;
                }
            }

           var ephObj =  EphemerisService.Get(SatelliteNumber.G01, reader.GetHeader().StartTime);

            if(ephObj == null)
            {
                EphemerisService = GlobalIgsEphemerisService.Instance;
            }


            var table = new ObjectTableStorage(tableName);

            while (reader.MoveNext())
            {
                var time = reader.Current.ReceiverTime;
                AddRow(AngleCut, EphemerisService, siteXyz, table, time);
            }
            return table;
        }
        
        /// <summary>
         /// 通过文件构建,按照文件坐标计算，按照其采样率输出。
         /// </summary>
         /// <param name="obsFile"></param>
         /// <param name="EphemerisService"></param>
         /// <param name="tableName"></param>
         /// <param name="AngleCut"></param>
         /// <returns></returns>
        public static ObjectTableStorage BuildTable(RinexObsFile obsFile, double AngleCut = 12, IEphemerisService EphemerisService = null, string tableName = null)
        {
            if (String.IsNullOrWhiteSpace(tableName))
            {
                tableName = obsFile.Header.MarkerName + "-卫星高度角";
            }
            if (EphemerisService == null)
            {
                EphemerisService = GlobalNavEphemerisService.Instance;
            } 
            var siteXyz = obsFile.Header.ApproxXyz;

            var ephObj = EphemerisService.Get(SatelliteNumber.G01, obsFile.Header.StartTime);

            if (ephObj == null)
            {
                EphemerisService = GlobalIgsEphemerisService.Instance;
            }

            var table = new ObjectTableStorage(tableName);
            foreach (var sec in obsFile)
            {
                var time = sec.ReceiverTime;
                AddRow(AngleCut, EphemerisService, siteXyz, table, time);
            }
            return table;
        }

        private static void AddRow(double AngleCut, IEphemerisService EphemerisService, XYZ siteXyz, ObjectTableStorage table, Time time)
        {
            table.NewRow();
            table.AddItem("Epoch", time);

            foreach (var sat in EphemerisService.Prns)
            {
                var eph = EphemerisService.Get(sat, time);
                if (eph == null) { continue; }

                var p = CoordTransformer.XyzToGeoPolar(eph.XYZ, siteXyz);
                var ele = p.Elevation;
                if (ele >= AngleCut)
                {
                    table.AddItem(sat, ele);
                }
            }
            table.EndRow();
        }
    }

}
