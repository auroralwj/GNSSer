//2014.05.22, Cui Yang, created

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Geo.Coordinates;
using Gnsser;

namespace Gnsser
{
    /// <summary>
    /// 天线数据。按照频率存储。
    /// </summary>
    public class FrequencecyAntennaData
    {
        /// <summary>
        /// 天线数据。按照频率存储。
        /// </summary>
        public FrequencecyAntennaData()
        {
            this.NonAzimuthData = new List<double>();
            this.AzimuthDependentData = new Dictionary<double, List<double>>();
            this.NonAzimuthDataRms = new List<double>();
            this.AzimuthDependentDataRms = new Dictionary<double, List<double>>();
        }
        /// <summary>
        /// 相位中心偏差 PCO
        /// </summary>
        public NEU PhaseCenterEccentricities { get; set; }
        /// <summary>
        /// 不依赖方位角的数据，通常为卫星天线
        /// </summary>
        public List<double> NonAzimuthData { get; set; }
        /// <summary>
        /// 依赖方位角的数据. Key 为方位角， Value 是该方位角对应的高度角的值列表。测站天线。
        /// </summary>
        public Dictionary<double, List<double>> AzimuthDependentData { get; set; }
        /// <summary>
        /// 相位中心偏差均方根
        /// </summary>
        public NEU PhaseCenterEccentricitiesRms { get; set; }
        /// <summary>
        /// 无方位角数据均方根
        /// </summary>
        public List<double> NonAzimuthDataRms { get; set; }
        /// <summary>
        /// 依赖方位角的均方根
        /// </summary>
        public Dictionary<double, List<double>> AzimuthDependentDataRms { get; set; }

    }

}
