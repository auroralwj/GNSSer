//2016.12.10, czs & double ,create in hongqing,  RTCM文件命名和解析

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Times;

namespace Gnsser.Ntrip
{
   
    /// <summary>
    /// RTCM文件命名和解析
    /// </summary>
    public class RtcmFileNamer
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public RtcmFileNamer()
        {

        }
        public RtcmFileNamer(string NtripMountPoint)
        {
            this.NtripMountPoint = NtripMountPoint;
        }

        /// <summary>
        /// 挂载点
        /// </summary>
        public string NtripMountPoint { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public Time Time { get; set; }

        /// <summary>
        /// 建立名称
        /// </summary>
        /// <param name="utcTime"></param>
        /// <returns></returns>
        public string BuildRtcm3FileName(DateTime utcTime)
        {
            var name = NtripMountPoint + "+" + utcTime.ToString("yyyy-MM-dd_HH_mm_ss") + ".rtcm3";
            return name;
        }
        /// <summary>
        /// 建立名称
        /// </summary>
        /// <returns></returns>
        public string BuildRtcm3FileName()
        {
            return BuildRtcm3FileName(Setting.ReceivingTimeOfNtripData.DateTime);
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static RtcmFileNamer Parse(string name)
        {
            RtcmFileNamer namer = new RtcmFileNamer();
            var nameendIndex = name.IndexOf("+");
            string siteName = name.Substring(0, nameendIndex);
            namer.NtripMountPoint = siteName;
            string timeString = name.Substring(nameendIndex + 1, name.IndexOf(".") - nameendIndex - 1);


            namer.Time = Geo.Times.Time.Parse(timeString, new char[] { '_', '-' });
            Setting.ReceivingTimeOfNtripData = namer.Time;
            // var name = NtripMountPoint + "+" + DateTime.UtcNow.ToString("yyyy-MM-dd_HH_mm_ss") + ".rtcm3";
            return namer;
        }

    }
}
