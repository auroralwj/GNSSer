//2015.04, lly, create in zz, ERP 文件
//2015.05.12，czs, edit in namu, 面向对象重构
//2018.05.02，czs, edit in hmx, 优化差值算法，从重构为Dic



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gnsser.Service;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Gnsser.Times;
using Geo.Utils;
using Gnsser;
using Geo;
using Geo.Times;

namespace Gnsser.Data
{
    /// <summary>
    /// ERP 文件
    /// </summary>
    public class ErpFile : BaseDictionary<double, ErpItem>, IIgsProductFile
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ErpFile()
        {
        }  /// <summary>
           /// 代码
           /// </summary>
        public string SourceCode { get { return Name.Substring(0, 2); } }
        /// <summary>
        /// 名称
        /// </summary>
        //public string Name { get; set; } 

        /// <summary>
        /// 一周的ERP信息
        /// </summary>
        public Dictionary<double, ErpItem> Erps { get { return (Dictionary<double, ErpItem>)this.Data; } }
        BufferedTimePeriod timePeriod = null;
        /// <summary>
        /// 时段
        /// </summary>
        public BufferedTimePeriod TimePeriod
        {
            get
            {
                if(timePeriod!= null) { return timePeriod; }
                timePeriod =  new BufferedTimePeriod(new Time(this.First.Mjd, true), new Time(this.Last.Mjd, true), TimeSpan.FromDays(1.5));
                return timePeriod;
            }
        }

    }
}