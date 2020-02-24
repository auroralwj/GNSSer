using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Times;
using System.IO;

namespace Gnsser.Ntrip
{
    public class Setting : Geo.Setting
    {
        /// <summary>
        /// 数据正在接收的时刻，主要用于获取当前时间对应的GPS周。 
        /// </summary>
        public static Time ReceivingTimeOfNtripData { get; set; }
        /// <summary>
        /// 数据源
        /// </summary>
        public static NtripSourceManager NtripSourceManager { get; set; }
        /// <summary>
        /// 加载数据设置
        /// </summary>
        static public void LoadNtripSourceManager()
        {
            var path = Path.Combine(Setting.DataDirectory, "Ntrip","NtripSetting.xml");
            NtripSourceManager = new NtripSourceManager(path);
        }

    }
}
