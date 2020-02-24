//2017.04.24, czs, create in hongqing, 实时GNSS数据提供器。

using System.Collections.Generic;
using System;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using System.Text;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic.CompilerServices;
using Gnsser.Ntrip.Rtcm;
using Geo;
using Geo.IO;
using Geo.Utils;
using Geo.Times;
using Gnsser.Data.Rinex;

//using System.Timers;

namespace Gnsser.Ntrip
{
    /// <summary>
    /// 实时数据源的用户
    /// </summary>
    public class RealTimeGnssDataUser
    {
        Log log = new Log(typeof(RealTimeGnssDataUser));

        /// <summary>
        /// 实时数据源
        /// </summary>
        public RealTimeGnssDataProvider RealTimeGnssDataProvider { get; set; }
        /// <summary>
        /// 绑定数据源
        /// </summary>
        /// <param name="dataProvider"></param>
        public void BindRealTimeGnssDataProvider(RealTimeGnssDataProvider dataProvider)
        {
            if (this.RealTimeGnssDataProvider == dataProvider) { return; }

            this.RealTimeGnssDataProvider = dataProvider;
            dataProvider.ObsHeaderCreated += dataProvider_ObsHeaderCreated;
            dataProvider.EphemerisInfoReceived += dataProvider_EphemerisInfoReceived;
            dataProvider.EpochObservationReceived += dataProvider_EpochObservationReceived;
            dataProvider.SSRClkRecordReceived += dataProvider_SSRClkRecordReceived;
            dataProvider.GlonassNavRecordReceived += dataProvider_GlonassNavRecordReceived;
            dataProvider.ObsHeaderUpdated += dataProvider_ObsHeaderUpdated;
            dataProvider.SSRSp3RecordReceived += dataProvider_SSRSp3RecordReceived;
        }

        protected virtual void dataProvider_ObsHeaderCreated(RinexObsFileHeader obj)
        {
            //throw new NotImplementedException();
        }

        #region  与数据提供器方法绑定

        protected virtual void dataProvider_SSRSp3RecordReceived(Sp3Section obj)
        { 
        }

        protected virtual void dataProvider_ObsHeaderUpdated(RinexObsFileHeader obj)
        { 
        }

        protected virtual void dataProvider_GlonassNavRecordReceived(GlonassNavRecord obj)
        { 
        }

        protected virtual void dataProvider_SSRClkRecordReceived(AtomicClock obj)
        {  
        }

        protected virtual void dataProvider_EpochObservationReceived(RinexEpochObservation obj)
        { 
        }

        protected virtual void dataProvider_EphemerisInfoReceived(EphemerisParam obj)
        { 
        }
        #endregion


    }

    /// <summary>
    /// 实时GNSS数据写入器,对应一个测站。
    /// </summary>
    public class RealTimeGnssDataWriter : RealTimeGnssDataUser, IDisposable
    {
        Log log = new Log(typeof(RealTimeGnssDataWriter));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="siteName"></param>
        /// <param name="OutpuDirectory"></param>
        public RealTimeGnssDataWriter(String OutpuDirectory, string siteName, Time startTime)
        {
            this.OutpuDirectory = OutpuDirectory;
            this.SiteName = siteName;
            this.OFileNameBuilder = new RinexFileNameBuilder().SetRinexFileType(RinexFileType.O).SetSationName(siteName).SetTimeResolution(TimeUnit.Second);
            var fileName = OFileNameBuilder.Build(startTime);
            var path = Path.Combine(this.OutpuDirectory, fileName);
            this.RinexObsFileWriter = new RinexObsFileWriter(path, 3.02);// RtcmDataParser.ObsHeader.Version);

            //导航文件，一次观测只有一个 
            this.NFileNameBuilder = new RinexFileNameBuilder();
            fileName = this.NFileNameBuilder.SetRinexFileType(RinexFileType.N).SetSationName(siteName).SetTimeResolution(TimeUnit.Minute).SetTime(startTime).Build();
            var path1 = Path.Combine(this.OutpuDirectory, fileName);
            fileName = this.NFileNameBuilder.SetRinexFileType(RinexFileType.R).SetSationName(siteName).SetTimeResolution(TimeUnit.Minute).SetTime(startTime).Build();
           
            var path2 = Path.Combine(this.OutpuDirectory, fileName);
            this.GpsNavFileWriter = new ParamNavFileWriter(path1);
            this.GlonassNavFileWriter = new GlonassNavFileWriter(path2);
                
            //精密星历
            fileName = Path.Combine(this.OutpuDirectory,siteName +startTime.GpsWeek + ""+ (int)startTime.DayOfWeek+ ".sp3");
            Sp3Writer = new Sp3Writer(fileName, null);

            fileName = Path.Combine(this.OutpuDirectory,siteName +startTime.GpsWeek + ""+ (int)startTime.DayOfWeek+ ".clk");
            ClockFileWriter = new ClockFileWriter(fileName);
        }
        #region 属性
        /// <summary>
        /// 观测文件的头部是否已经写入
        /// </summary>
        public bool IsObsHeaderWrited { get; set; }

        /// <summary>
        /// 输出目录
        /// </summary>
        public string OutpuDirectory { get; set; }

        /// <summary>
        /// 测站，挂载点
        /// </summary>
        public string SiteName { get; set; }
        /// <summary>
        /// O文件写入器
        /// </summary>
        public RinexObsFileWriter RinexObsFileWriter { get; set; }
        /// <summary>
        /// 名称命名
        /// </summary>
        RinexFileNameBuilder OFileNameBuilder { get; set; }
        #region  导航文件写入器
        ClockFileWriter ClockFileWriter { get; set; }
        Sp3Writer Sp3Writer { get; set; }
        /// <summary>
        /// 导航文件
        /// </summary>
        ParamNavFileWriter GpsNavFileWriter { get; set; }
        ParamNavFileWriter BdsNavFileWriter { get; set; }
        ParamNavFileWriter QzssNavFileWriter { get; set; }
        ParamNavFileWriter GalileoNavFileWriter { get; set; }
        /// <summary>
        /// 导航文件
        /// </summary>
        GlonassNavFileWriter GlonassNavFileWriter { get; set; }
        RinexFileNameBuilder NFileNameBuilder { get; set; }
        #endregion
        #endregion
        #region  与数据提供器方法绑定
        protected override void dataProvider_ObsHeaderCreated(RinexObsFileHeader obj)
        {

        }
        protected override void dataProvider_SSRSp3RecordReceived(Sp3Section obj)
        {
            this.WriteSp3Record(obj);
        }

        protected override void dataProvider_ObsHeaderUpdated(RinexObsFileHeader obj)
        {
            this.ReWriteHeader(obj);
        }

        protected override void dataProvider_GlonassNavRecordReceived(GlonassNavRecord obj)
        {
            this.WriteGlonassNavRecord(obj);
        }

        protected override void dataProvider_SSRClkRecordReceived(AtomicClock obj)
        {
            this.WriteClkRecord(obj);
        }

        protected override void dataProvider_EpochObservationReceived(RinexEpochObservation obj)
        {
            this.WriteEpochObservation(obj);
        }

        protected override void dataProvider_EphemerisInfoReceived(EphemerisParam obj)
        {
            this.WriteEphemerisParam(obj);
        }

        #endregion
        #region 观测文件写入文件
        /// <summary>
        /// 写头部信息
        /// </summary>
        /// <param name="obj"></param>
        public void WriteHeader(RinexObsFileHeader obj)
        {
            RinexObsFileWriter.WriteHeader(obj);
            IsObsHeaderWrited = true;
        }
        public void ReWriteHeader(RinexObsFileHeader obj)
        {
            if (!IsObsHeaderWrited)
            {
                WriteHeader(obj);
            }
            else
            {
                RinexObsFileWriter.ReWriteHeader(obj); 
            }
        }
        /// <summary>
        /// 已接收历元数据
        /// </summary>
        /// <param name="obj"></param>
        public void WriteEpochObservation(RinexEpochObservation obj)
        {
            if (!IsObsHeaderWrited && obj.Header != null) { this.WriteHeader(obj.Header); }
            if (obj.Header == null) { return; }

            RinexObsFileWriter.WriteEpochObservation(obj);
            RinexObsFileWriter.Flush();
        }
        #endregion

        #region 星历写入文件
        public void WriteSp3Record(Sp3Section obj)
        {
            Sp3Writer.Write(obj);
            Sp3Writer.Flush();
        }
        public void WriteClkRecord(AtomicClock obj)
        {
            ClockFileWriter.Write(obj);
            ClockFileWriter.Flush();
        }


        public void WriteGlonassNavRecord(GlonassNavRecord obj)
        {
            GlonassNavFileWriter.CheckAndAppendEphemerisParam(obj);
            GlonassNavFileWriter.SaveToFile();
        }

        public void WriteEphemerisParam(EphemerisParam obj)
        {
            //写入文件
            GpsNavFileWriter.CheckAndAppendEphemerisParam(obj);
            GpsNavFileWriter.SaveToFile();
        }
        #endregion

        public void Dispose()
        {
            RinexObsFileWriter.Dispose();
            GpsNavFileWriter.Dispose();
        }
    }

}