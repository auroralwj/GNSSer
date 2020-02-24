//2014.06.24，czs, create, for RINEX 3.0
//2015.05.10, czs, edit in namu, 分离数据与服务
//2016.03.05, czs, edint in hongqing, 重构为字典
//2018.05.16, czs, edit in HMX, 修复数据读取为空的错误

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Gnsser.Times;
using Gnsser.Service;
using Geo.Times; 
using Geo;

namespace Gnsser.Data.Rinex
{
    /// <summary>
    /// RINEX GnssNavFile 导航文件。
    /// 以导航星历参数表达的导航文件，包括北斗、GPS、伽利略。
    /// 一个类的实例代表一个文件。
    /// </summary>
    public class ParamNavFile : BaseDictionary<SatelliteNumber, List<EphemerisParam>>, IDisposable, IIgsProductFile
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ParamNavFile()
        { 
            StartTime = Time.MaxValue;
            EndTime = Time.MinValue; 
        }
        #region 属性
        /// <summary>
        /// 导航头部信息
        /// </summary>
        public NavFileHeader Header { get; set; } 
        /// <summary>
        /// 卫星编号集合
        /// </summary>
        public List<SatelliteNumber> Prns { get { return new List<SatelliteNumber>(this.Keys); } }
        /// <summary>
        /// 文件名称
        /// </summary>
        public override string Name
        {
            get { return Header.Name; }
            set { if(Header !=null)  Header.Name = value; }
        }
         
        /// <summary>
        /// 开始时间，不代表所有为卫星。
        /// </summary>
        public Time StartTime { get; set; }
        /// <summary>
        /// 结束时间，不代表所有为卫星。
        /// </summary>
        public Time EndTime { get; set; }

        /// <summary>
        /// 时段
        /// </summary>
        public BufferedTimePeriod TimePeriod { get { return new BufferedTimePeriod(StartTime, EndTime, 24 * 3600); } }
        /// <summary>
        /// 代码
        /// </summary>
        public string SourceCode { get => "igs"; }
        #endregion

        #region 方法

        /// <summary>
        /// 添加星历参数,重复星历将失败。
        /// </summary>
        /// <param name="EphemerisParam"></param>
        public void Add(EphemerisParam EphemerisParam)
        {
            var prn = EphemerisParam.Prn; 
            var list =   GetOrCreate(prn);
            SetTime(EphemerisParam.Time);
             
            if (!list.Contains(EphemerisParam)) { list.Add(EphemerisParam); }
        }

        private void SetTime(Time time)
        { 
            if (StartTime > time)
            {
                StartTime = time;
            }
            if (EndTime < time)
            {
                EndTime = time;
            }
        }
        /// <summary>
        /// 添加一个星历参数
        /// </summary>
        /// <param name="EphemerisParam"></param>
        public void Add(List<EphemerisParam> EphemerisParam)
        {
            foreach (var item in EphemerisParam)
            {
                Add(item);
            }
        }
         
         
        /// <summary>
        /// 获取星历参数集合
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public List<EphemerisParam> GetEphemerisParams(SatelliteNumber prn)
        { 
            return GetOrCreate(prn);
        } 
        /// <summary>
        /// 获取指定范围星历集合
        /// </summary>
        /// <param name="PRN"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public List<EphemerisParam> GetEphemerisParams(SatelliteNumber PRN, Time from, Time to)
        {
            return GetEphemerisParams(PRN).FindAll(b => b.Time >= from && b.Time <= to);
        }

        //public override List<EphemerisParam> GetOrCreate(SatelliteNumber key)
        //{
        //    return  new List<EphemerisParam>();
        //}
        /// <summary>
        /// 创建一个
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override List<EphemerisParam> Create(SatelliteNumber key)
        {
            return new List<EphemerisParam>();
        }
        #endregion  
    } 
}
