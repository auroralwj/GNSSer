//2016.12.27 czs & cuiyang, created,  ��վ�����ļ�

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
    /// GNSSer��վ�����ļ�,��SiteInfo���������ڣ��˴�Ϊ��վʱ����Ϣ��
    /// </summary>
    public class StationInfo : IOrderedProperty
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        public StationInfo()
        {
            Init();
        }
        /// <summary>
        /// ���캯��
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
                Geo.Utils.ObjectUtil.GetPropertyName<StationInfo>(m=>m.HeightCode) , //��һ��
                Geo.Utils.ObjectUtil.GetPropertyName<StationInfo>(m=>m.AntN) ,
                Geo.Utils.ObjectUtil.GetPropertyName<StationInfo>(m=>m.AntE) ,
                Geo.Utils.ObjectUtil.GetPropertyName<StationInfo>(m=>m.ReceiverType) ,
                Geo.Utils.ObjectUtil.GetPropertyName<StationInfo>(m=>m.ReceiverNumber) ,
                Geo.Utils.ObjectUtil.GetPropertyName<StationInfo>(m=>m.AntennaType) ,
                Geo.Utils.ObjectUtil.GetPropertyName<StationInfo>(m=>m.AntennaNumber) 
            };
        } 

        /// <summary>
        /// ��ʶ��,�����Զ��壬��Ϊȫ��Ψһ��ʶ��
        /// </summary>
        public virtual string Id { get { return SiteName.ToUpper() + Geo.Utils.StringUtil.SubString(AntennaType, 0, 4) + TimeFrom + TimeTo; } }
        /// <summary>
        /// ��ʼʱ��
        /// </summary>
        public Time TimeFrom { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public Time TimeTo { get; set; }
        /// <summary>
        /// ʱ����Ϣ
        /// </summary>
        public TimePeriod TimePeriod { get { return new TimePeriod(TimeFrom, TimeTo); } set { this.TimeFrom = value.Start; this.TimeTo = value.End; } }
        /// <summary>
        /// ��վ����
        /// </summary>
        public string SiteName { get; set; }
        /// <summary>
        /// վ����
        /// </summary>
        public string StationName { get; set; }
        /// <summary>
        /// ���ջ�����
        /// </summary>
        public string ReceiverType { get; set; }
        /// <summary>
        /// ���ջ����
        /// </summary>
        public string ReceiverNumber { get; set; }
        /// <summary>
        /// ���߱��
        /// </summary>
        public string AntennaNumber { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public string AntennaType { get; set; }
        /// <summary>
        /// �߶�����
        /// </summary>
        public string HeightCode { get; set; }
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public string Info { get; set; }
        /// <summary>
        /// ���߱�����
        /// </summary>
        public double AntN { get; set; }
        /// <summary>
        /// ���߶�����
        /// </summary>
        public double AntE { get; set; }
        /// <summary>
        /// �����Ϸ���
        /// </summary>
        public double AntU { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public NEU AntNEU { get { return new NEU(AntN, AntE, AntU); } set { this.AntN = value.N; this.AntE = value.E; this.AntU = value.U; } }
        /// <summary>
        /// ��HEN��ʶ����������
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