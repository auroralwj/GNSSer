//2015.10.02, czs, create in 彭州, URL生成器
//2015.10.06, czs, edit in 彭州到成都动车C6186, 时间段字符串生成器
//2015.10.07, czs, edit in 安康到西安临客K8182, 星历地址生成器

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
    public class Sp3UrlGenerator : ParamBasedOperation<Sp3UrlGeneratorParam>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Sp3UrlGenerator()
        {
        }

      
        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public override bool Do()
        {
            var reader = new Sp3UrlGeneratorReader(this.OperationInfo.ParamFilePath);
    
            int i = 0;
            foreach (var item in reader)
            {
                this.CurrentParam = item;
                List<string> pathes = new List<string>();
                
                var from =item.StartTime;
                var to =item.EndTime;
                var interval = TimeSpan.FromSeconds( item.Interval);
                for (var ifrom = from; ifrom <= to; ifrom = ifrom + interval)
                {
                    var pattern = item.Pattern;
                    string path = TimeScopeStringGenerator.BuildTimedString(pattern, ifrom);
                    path = ReplaceSourceName(path, item.SourceName);
                    //自动生成一个地址
                    if (pathes.Count == 0)
                    {
                        path += ", " + Path.GetDirectoryName(item.LocalDirectory);
                    }
                    pathes.Add(path);
                }
                if ( i ==0 && File.Exists(item.OutputPath))
                {
                    File.Delete(item.OutputPath);

                    this.StatedMessage = StatedMessage.Processing;
                    this.StatedMessage.Message = "删除了原参数文件 " + item.OutputPath;
                    this.OnStatedMessageProduced();
                }
                //写入文件，追加
                File.AppendAllLines(item.OutputPath, pathes.ToArray());

                this.StatedMessage = StatedMessage.Processing;
                this.StatedMessage.Message = "生成并追加 " + pathes.Count + " 条地址到 " + item.OutputPath;
                this.OnStatedMessageProduced();

                i++;
            }
            return true;
        }

        /// <summary>
        /// 建立时间属性的字符串
        /// </summary>
        /// <param name="sourceName">数据源名称</param>
        /// <param name="pattern">模板</param>
        /// <returns></returns>
        public static string ReplaceSourceName(string pattern, string sourceName)
        { 
            //{SourceName}{Year}{Month}{Day}{Week}{DayOfWeek}{DayOfYear}
            string path = pattern
             .Replace(ELMarker.SourceName, sourceName);
            return path;
        }
    }
}
