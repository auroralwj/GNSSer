//2016.04.19, czs, create in hongqing, 多个观测文件管理分析器
//2018.12.12, czs, edit in hmx, 单独提出

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.Times;
using Gnsser.Times;
using System.IO;

namespace Gnsser
{
    /// <summary>
    /// GNSS时段网
    /// </summary>
    public class GnssSurveyNet : BaseList<SiteObsBaseline>
    {
        public GnssSurveyNet()
        {

        }
        /// <summary>
        /// 共有时段
        /// </summary>
        public TimePeriod TimePeriod
        {
            get
            {
                if (this.Count == 0) return null;
                SiteObsBaseline first = this[0];
                TimePeriod timePeriod = first.TimePeriod;
                foreach (var item in this)
                {
                    if (item == first) continue;
                    var span = timePeriod.GetIntersect(item.TimePeriod);
                    if (span == null) return null;

                    timePeriod =new TimePeriod(span.Start, span.End);                    
                }
                return timePeriod;
            }

        }
        /// <summary>
        /// 获取所有的观测文件
        /// </summary>
        /// <returns></returns>
        public List<ObsSiteInfo> GetObsFileInfos()
        {
            List<ObsSiteInfo> list = new List<ObsSiteInfo>();
            foreach (var item in this)
            {
                if (!list.Contains(item.Start))
                {
                    list.Add(item.Start);
                }
                if (!list.Contains(item.End))
                {
                    list.Add(item.End);
                } 
            } 

            return list;
        }
        public override string ToString()
        {
            if (this.Count > 0)
            {
                return "GNSS网 "+ this.Count +"," +  TimePeriod;
            }
            return "空网";
        }
    }

}
