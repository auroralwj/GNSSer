//2018.11.13, czs, create in hmx, 时段文件区分

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks; 
using Gnsser.Data;
using Geo.Times;
using Geo.IO;
using Gnsser.Data.Rinex;


namespace Gnsser
{

    /// <summary>
    /// 观测文件时段分组
    /// </summary>
    public class ObsFilePeriodDivider : Geo.AbstractProcess<string[], Dictionary<string, List<string>>>
    {
        Log log = new Log(typeof(ObsFilePeriodDivider));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="outputDirectory"></param>
        /// <param name="MinPhaseSpanMinutes"></param>
        /// <param name="SubDirectory"></param>
        /// <param name="NetName"></param>
        public ObsFilePeriodDivider(string outputDirectory, double MinPhaseSpanMinutes = 20, string SubDirectory = "", string NetName = "Net")
        {
            this.OutputDirectory = outputDirectory;
            this.SubDirectory = SubDirectory;
            this.MinPhaseSpanMinutes = MinPhaseSpanMinutes;
            this.NetName = NetName;
        }
        /// <summary>
        /// 是否移动到目录
        /// </summary>
        public bool IsMoveTo { get; set; }
        /// <summary>
        /// 网络名称前缀
        /// </summary>
        public string NetName { get; set; }
        /// <summary>
        /// 输出目录
        /// </summary>
        public string OutputDirectory { get; set; }
        /// <summary>
        /// 子目录
        /// </summary>
        public string SubDirectory { get; set; }
        /// <summary>
        /// 最小观测时段， 时段的区分，分钟
        /// </summary>
        public double MinPhaseSpanMinutes { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public Dictionary<string, List<string>> Result { get; set; }

        public override Dictionary<string, List<string>> Run(string[] inputs)
        {
            if (inputs == null || inputs.Length == 0) { return new Dictionary<string, List<string>>(); }

            var grouped = TimePeriodGroup(inputs, MinPhaseSpanMinutes);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("区分成了" + grouped.Count + " 个时段。");
            int i = 1;
            foreach (var item in grouped)
            {
                sb.AppendLine();
                sb.AppendLine(i + " " + item.Key + " 共 " + (item.Key.Span / 3600).ToString("0.00") + " 小时");
                foreach (var file in item.Value)
                {
                    sb.AppendLine(Path.GetFileName(file));
                }
                i++;
            }
            log.Info(sb.ToString());

            Dictionary<string, List<string>> result = CopyToNetDirectory(grouped);
            this.Result = result;
            return Result;
        }

        /// <summary>
        /// 复制到平差网目录
        /// </summary>
        /// <param name="grouped"></param>
        /// <returns></returns>
        private Dictionary<string, List<string>> CopyToNetDirectory(Dictionary<TimePeriod, List<string>> grouped)
        {
            //复制后的结果
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();

            //移动到对应的目录
            int i = 0;
            foreach (var kv in grouped)
            {
                i++;
                var time = kv.Key.Start;
                string netName = BuildGroupNetName(i, time);
                if (!result.ContainsKey(netName))
                {
                    result[netName] = new List<string>();
                }
                var list = result[netName];

                foreach (var oldPath in kv.Value)
                {
                    try
                    {
                        var fileName = Path.GetFileName(oldPath);

                        string path = BuildNewPath(netName, fileName);
                        ToNewPlace(oldPath, path);

                        //判断导航文件，也尝试移动
                        if (fileName.ToUpper().EndsWith("O"))
                        {

                            var oldDir = Path.GetDirectoryName(oldPath);
                            var navFileName = fileName.TrimEnd(new char[] { 'o', 'O' }) + "n";
                            var navPath = Path.Combine(oldDir, navFileName);
                            if (File.Exists(navPath))
                            {
                                path = BuildNewPath(netName, navFileName);
                                ToNewPlace(navPath, path);
                            }
                            else
                            {
                                navFileName = fileName.TrimEnd(new char[] { 'o', 'O' }) + "p";
                                navPath = Path.Combine(oldDir, navFileName);
                                if (File.Exists(navPath))
                                {
                                    path = BuildNewPath(netName, navFileName);
                                    ToNewPlace(navPath, path);
                                }
                            }
                        }

                        list.Add(path);
                    }
                    catch (Exception ex)
                    {
                        log.Error("复制文件出错：" + ex.Message);
                    }
                }
            }

            return result;
        }

        private string BuildNewPath(string netName, string fileName)
        {
            var dir = Path.Combine(OutputDirectory, netName);
            Geo.Utils.FileUtil.CheckOrCreateDirectory(dir);
            var path = Path.Combine(dir, SubDirectory, fileName);
            return path;
        }

        private void ToNewPlace(string oldPath, string path)
        {
            if (IsMoveTo)
            {
                Geo.Utils.FileUtil.MoveFile(oldPath, path, true);
            }
            else
            {
                Geo.Utils.FileUtil.CopyFile(oldPath, path, true);
            }
        }

        /// <summary>
        /// 构建网名
        /// </summary>
        /// <param name="i"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public string BuildGroupNetName(int i, Time time)
        {
            return NetName + i.ToString("00") + "_" + time.ToDateAndHourMinitePathString();
        }

        #region  时段分组
        /// <summary>
        /// 时段分组
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="MinPhaseSpanMinutes"></param>
        /// <returns></returns>
        public static Dictionary<TimePeriod, List<string>> TimePeriodGroup(string[] inputs, double MinPhaseSpanMinutes)
        {
            Dictionary<string, TimePeriod> periods = new Dictionary<string, TimePeriod>();
            foreach (var path in inputs)
            {
                if (!Geo.Utils.FileUtil.IsValid(path))
                {
                    continue;
                }
                var time = RinexObsFileReader.ReadPeriod(path);
                periods[path] = time;
            }

            Dictionary<TimePeriod, List<string>> grouped = TimePeriod.GroupToPeriods(periods, MinPhaseSpanMinutes);
            return grouped;
        }
        /// <summary>
        /// 时段分组
        /// </summary>
        /// <param name="sites"></param>
        /// <param name="MinPhaseSpanMinutes"></param>
        /// <returns></returns>
        public static Dictionary<TimePeriod, List<ObsSiteInfo>> TimePeriodGroup(List<ObsSiteInfo> sites, double MinPhaseSpanMinutes)
        {
            Dictionary<ObsSiteInfo, TimePeriod> periods = new Dictionary<ObsSiteInfo, TimePeriod>();
            foreach (var path in sites)
            { 
                periods[path] = path.NetPeriod;
            }

            Dictionary<TimePeriod, List<ObsSiteInfo>> grouped = TimePeriod.GroupToPeriods(periods, MinPhaseSpanMinutes);
            return grouped;
        }

        #endregion
    }

}
