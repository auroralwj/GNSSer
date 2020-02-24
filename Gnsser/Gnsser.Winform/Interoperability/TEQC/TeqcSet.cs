//2014.12.27, lh, create in 郑州, TEQC 互操作

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms.DataVisualization.Charting.Data;
using System.Windows.Forms.DataVisualization.Charting;

namespace Gnsser.Winform
{
    /// <summary>
    /// 程序数据设置
    /// </summary>
    public static class TeqcSet
    {
        /// <summary>
        /// 程序是否处于调试状态
        /// </summary>
        public static bool IsDebug = true;

        public static string ExeFolder = @".\Data\Exe\";
        public static string TeqcPath = ExeFolder + "teqc.exe";
    }

    //定义一个公共的批处理结果筛选项，用来传递
    public class BATScreeningParameter
    {
        public static double Percent { get; set; }
        public static double Mp1 { get; set; }
        public static double Mp2 { get; set; }
        public static double O_slps { get; set; }
        public static bool invert_selection { get; set; }

        public static bool percent_selection { get; set; }
    }

    //定义一个公共的SeriesList，用来传递
    public class BATSeriesList
    {
        public static List<Series> BATSeries { get; set; }
    }
}
