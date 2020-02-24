//2014.10.06， czs, create in hailutu, 数据源配置
//2015.05.12, czs, edit in namu, 名称改为 GnssDataSourceOption ， 增加注释，继承自TypedSatFileOptionManager，实现多系统单独配置功能
//2017.05.04, czs, edit in hongqing, 剥离出 IgsProductSourceOption 
//2017.09.04, lly, add in zz, GPT2模型 1度分辨率
//2018.05.01, czs, edit in Hmx, 将时变信息分开,删除 GnssDataSourceOption ，直接采用GsserConfig代替

//2018.05.01, czs, create in Hmx, 提取时变信息

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.Times;
using Gnsser.Times;
using Gnsser.Data;

namespace Gnsser
{
    /// <summary>
    /// 会话信息
    /// </summary>
    public class SessionInfo
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="SatelliteType">系统类型</param>
        /// <param name="timePeriod">时段信息</param>
        public SessionInfo(Time timePeriod, List<SatelliteType> SatelliteType)
        {
            this.TimePeriod = new BufferedTimePeriod( timePeriod, timePeriod + TimeSpan.FromDays(1));
            this.SatelliteTypes = SatelliteType;
        }
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="SatelliteType">系统类型</param>
        /// <param name="timePeriod">时段信息</param>
        public SessionInfo(TimePeriod timePeriod, List<SatelliteType> SatelliteType)
        {
            this.TimePeriod = new BufferedTimePeriod( timePeriod.Start, timePeriod.End);
            this.SatelliteTypes = SatelliteType;
        }
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="SatelliteType">系统类型</param>
        /// <param name="timePeriod">时段信息</param>
        public SessionInfo(BufferedTimePeriod timePeriod, List<SatelliteType> SatelliteType)
        {
            this.TimePeriod = timePeriod;
            this.SatelliteTypes = SatelliteType;
        }
        /// <summary>
        /// 数据源时段信息
        /// </summary>
        public BufferedTimePeriod TimePeriod { get; set; }
        /// <summary>
        /// 系统类型
        /// </summary>
        public List<SatelliteType> SatelliteTypes { get; set; }
        /// <summary>
        /// 是否包含指定系统和时间。
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool Contains(SatelliteNumber prn, Time time)
        {
            return TimePeriod.Contains(time) && SatelliteTypes.Contains(prn.SatelliteType);
        }

        public override string ToString()
        {
            return Geo.Utils.StringUtil.ToString(SatelliteTypes) + ", " +  TimePeriod;
        }
    }
}
