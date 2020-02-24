//2015.01.18, czs, create in namu, 提供一种统一的命名方法，用以区别其他计算项目

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Times;
using Geo;
using Gnsser;
using Geo.Times; 
namespace Gnsser
{
    /// <summary>
    ///提供一种统一的命名方法，用以区别其他计算项目
    /// </summary>
    public class ProjectNameBuilder : AbstractBuilder<string> 
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="siteName">测站名称</param>
        /// <param name="ReceivingTime">观测时间</param> 
        public ProjectNameBuilder(string siteName, Time ReceivingTime)
            : this(siteName, ReceivingTime, DateTime.Now)
        { 
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="siteName">测站名称</param>
        /// <param name="ReceivingTime">观测时间</param>
        /// <param name="SolvingTime">计算时间</param>
        public ProjectNameBuilder(string siteName, Time ReceivingTime, DateTime SolvingTime)
        {
            this.SiteName = siteName;
            this.ReceivingTime = ReceivingTime;
            this.SolvingTime =  SolvingTime; 
        }
        /// <summary>
        /// 测站名称
        /// </summary>
        public string SiteName { get; set; }
        /// <summary>
        /// 观测文件的时间
        /// </summary>
        public Time ReceivingTime { get; set; }
        /// <summary>
        /// 解算时间
        /// </summary>
        public DateTime SolvingTime { get; set; }

        public override string Build()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(this.SiteName);
            sb.Append("_");
            sb.Append(this.ReceivingTime.ToDateString());
            sb.Append("_");
            sb.Append( Geo.Utils.DateTimeUtil.GetDateTimePathString( this.SolvingTime));

            return sb.ToString();

        }

    }
}
