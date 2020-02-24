//2015.10.02, czs, create in 彭州, URL生成器
//2015.10.06, czs, edit in 彭州到成都动车C6186, 时间段字符串生成器

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.IO;
using Geo;
using Geo.Times;

namespace Gnsser.Api
{
    /// <summary>
    /// 复制文件
    /// </summary>
    public class TimeScopeStringGenerator : ParamBasedOperation<TimeScopeStringGeneratorParam>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TimeScopeStringGenerator()
        {
        }

      
        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public override bool Do()
        {
            var reader = new TimeScopeStringGeneratorReader(this.OperationInfo.ParamFilePath);
            foreach (var item in reader)
            {
                this.CurrentParam = item;
                // Geo.Utils.FileUtil.UrlGeneratorFileOrDirectory(key.FormFileOrDirectory, key.ToFileOrDirectory, key.Overwrite);   

                List<string> pathes = new List<string>();
                
                var from =item.StartTime;
                var to =item.EndTime;
                var interval = TimeSpan.FromSeconds( item.Interval);
                for (var ifrom = from; ifrom <= to; ifrom = ifrom + interval)
                {
                    var pattern = item.Pattern;
                    string path = BuildTimedString(pattern, ifrom);

                    pathes.Add(path);
                } 
            }
            return true;
        }

        /// <summary>
        /// 建立时间属性的字符串
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string BuildTimedString( string pattern, DateTime dateTime)
        {
            Time time = Time.Parse(dateTime);
            //{SourceName}{Year}{Month}{Day}{Week}{DayOfWeek}{DayOfYear}
            string path = pattern
                .Replace("{Year}", time.Year.ToString("0000"))
                .Replace(ELMarker.Month, time.Month.ToString("00"))
                .Replace(ELMarker.Day, time.Day.ToString("00"))
                .Replace("{Week}", time.GpsWeek + "")
                .Replace("{Minute}", time.Minute + "")
                .Replace("{Second}", time.Second + "")
                .Replace("{SubYear}", time.SubYear + "")
                .Replace("{Week}", time.GpsWeek + "")
                .Replace("{DayOfWeek}", ((int)time.DayOfWeek) + "")
                .Replace("{DayOfYear}", time.DayOfYear.ToString("000"));
            // .Replace(ELMarker.SourceName, sourceName);
            return path;
        }
    }
}
