//2014.10.06，czs, create in hailutu, 单点定位选项
//2015.10.15, czs, edit in 西安五路口袁记肉夹馍店, 增加延迟数量
//2016.05.01, czs, edit in hongqing, 更改为 Gnss 数据处理选项
//2016.08.02, czs, edit in fujian yongan, 增加固定参考站PPP
//2017.09.05, czs, edit in hongqing, 单独成立文件

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Geo.Times;
using Geo.Coordinates;
using Gnsser;
using Gnsser.Core;
using Geo;
using Gnsser.Service;
using Geo.IO;

namespace Gnsser
{
    /// <summary>
    /// GNSS解算器类型
    /// </summary>
    public class GnssSolverTypeHelper
    {
        static Log log = new Log(typeof(GnssSolverTypeHelper));

        /// <summary>
        /// 获取GNSS解算器类型
        /// </summary>
        /// <param name="SingleSiteSolverType"></param>
        /// <returns></returns>
        public static GnssSolverType GetGnssSolverType(SingleSiteGnssSolverType SingleSiteSolverType)
        {
            return (GnssSolverType)Enum.Parse(typeof(GnssSolverType), SingleSiteSolverType.ToString());
        } 
        /// <summary>
        /// 获取GNSS解算器类型
        /// </summary>
        /// <param name="SingleSiteSolverType"></param>
        /// <returns></returns>
        public static GnssSolverType GetGnssSolverType(TwoSiteSolverType SingleSiteSolverType)
        {
            return (GnssSolverType)Enum.Parse(typeof(TwoSiteSolverType), SingleSiteSolverType.ToString());
        } 
    }
     
}