//2016.12.27 czs & cuiyang, created,  测站天线文件

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;
using Geo.Common;
using System.IO;
using Geo;
using Geo.Algorithm.Adjust;
using Geo.Coordinates;
using Geo.IO;
using Geo.Referencing;
using Geo.Times;


namespace Gnsser.Data
{  
    /// <summary>
    /// GNSSer测站天线文件,与SiteInfo的区别在于，此处为测站时段信息。
    /// </summary>
    public class StationInfo : IOrderedProperty
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StationInfo()
        {
            Init();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="siteInfo"></param>
        /// <param name="obsInfo"></param>
        public StationInfo(ISiteInfo siteInfo, IObsInfo obsInfo)
        {
            Init();
            this.AntHEN = siteInfo.Hen;
            this.AntennaType = siteInfo.AntennaType;
            this.AntennaNumber = siteInfo.AntennaNumber;
            this.ReceiverNumber = siteInfo.ReceiverNumber;
            this.ReceiverType = siteInfo.ReceiverType;
            this.TimePeriod = obsInfo.TimePeriod;
            this.SiteName = siteInfo.SiteName;
            this.StationName = siteInfo.SiteName;
        } 

        private void Init()
        {
            OrderedProperties = new List<string>() {  
                Geo.Utils.ObjectUtil.GetPropertyName<StationInfo>(m=>m.SiteName),
                Geo.Utils.ObjectUtil.GetPropertyName<StationInfo>(m=>m.StationName) ,
                Geo.Utils.ObjectUtil.GetPropertyName<StationInfo>(m=>m.TimeFrom) ,
                Geo.Utils.ObjectUtil.GetPropertyName<StationInfo>(m=>m.TimeTo) ,
                Geo.Utils.ObjectUtil.GetPropertyName<StationInfo>(m=>m.AntU) ,
                Geo.Utils.ObjectUtil.GetPropertyName<StationInfo>(m=>m.HeightCode) , //不一致
                Geo.Utils.ObjectUtil.GetPropertyName<StationInfo>(m=>m.AntN) ,
                Geo.Utils.ObjectUtil.GetPropertyName<StationInfo>(m=>m.AntE) ,
                Geo.Utils.ObjectUtil.GetPropertyName<StationInfo>(m=>m.ReceiverType) ,
                Geo.Utils.ObjectUtil.GetPropertyName<StationInfo>(m=>m.ReceiverNumber) ,
                Geo.Utils.ObjectUtil.GetPropertyName<StationInfo>(m=>m.AntennaType) ,
                Geo.Utils.ObjectUtil.GetPropertyName<StationInfo>(m=>m.AntennaNumber) 
            };
        } 

        /// <summary>
        /// 标识符,可以自定义，作为全局唯一标识。
        /// </summary>
        public virtual string Id { get { return SiteName.ToUpper() + Geo.Utils.StringUtil.SubString(AntennaType, 0, 4) + TimeFrom + TimeTo; } }
        /// <summary>
        /// 起始时间
        /// </summary>
        public Time TimeFrom { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public Time TimeTo { get; set; }
        /// <summary>
        /// 时段信息
        /// </summary>
        public TimePeriod TimePeriod { get { return new TimePeriod(TimeFrom, TimeTo); } set { this.TimeFrom = value.Start; this.TimeTo = value.End; } }
        /// <summary>
        /// 测站名称
        /// </summary>
        public string SiteName { get; set; }
        /// <summary>
        /// 站名称
        /// </summary>
        public string StationName { get; set; }
        /// <summary>
        /// 接收机名称
        /// </summary>
        public string ReceiverType { get; set; }
        /// <summary>
        /// 接收机编号
        /// </summary>
        public string ReceiverNumber { get; set; }
        /// <summary>
        /// 天线编号
        /// </summary>
        public string AntennaNumber { get; set; }
        /// <summary>
        /// 天线类型
        /// </summary>
        public string AntennaType { get; set; }
        /// <summary>
        /// 高度类型
        /// </summary>
        public string HeightCode { get; set; }
        /// <summary>
        /// 附加信息
        /// </summary>
        public string Info { get; set; }
        /// <summary>
        /// 天线北方向
        /// </summary>
        public double AntN { get; set; }
        /// <summary>
        /// 天线东方向
        /// </summary>
        public double AntE { get; set; }
        /// <summary>
        /// 天线上方向
        /// </summary>
        public double AntU { get; set; }
        /// <summary>
        /// 天线坐标
        /// </summary>
        public NEU AntNEU { get { return new NEU(AntN, AntE, AntU); } set { this.AntN = value.N; this.AntE = value.E; this.AntU = value.U; } }
        /// <summary>
        /// 以HEN标识的天线坐标
        /// </summary>
        public HEN AntHEN { get { return new HEN(AntNEU.U, AntNEU.E, AntNEU.N); } set { this.AntN = value.N; this.AntE = value.E; this.AntU = value.H; } }

        public List<string> OrderedProperties { get; set; }

        public List<ValueProperty> Properties { get; set; }

        public override bool Equals(object obj)
        {
            var o = obj as StationInfo;
            if (o == null) { return false; }
            return Id.Equals(o.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Id + TimePeriod.ToString() + ReceiverType + AntennaType;
        }
    }
}