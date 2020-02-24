//2017.10.22, czs, edit  in hongqing, 观测文件探测与修复选项。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Common;
using Geo.Coordinates;
using Geo;
using Geo.Times;

namespace Gnsser
{



    /// <summary>
    /// 观测文件探测与修复选项
    /// </summary>
    public class ObsFileFixOption : IOption
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ObsFileFixOption()
        {
            this.ObsCodes = new List<string>() { "P1" };
            this.TimePeriod = new TimePeriod(Time.MinValue, Time.MaxValue);
            this.IsEnableInterval = false;
            this.IsEnableMinObsCodeAppearRatio = true;
            this.MinObsCodeAppearRatio = 0.5;
            this.Version = 3.02;
            this.Interval = 30;
            this.SatelliteTypes = new List<SatelliteType>() { SatelliteType.G };
            ObsTypes = new List<ObsTypes>() { Gnsser.ObsTypes.C, Gnsser.ObsTypes.P, Gnsser.ObsTypes.L };
            NotVacantCodeList = new List<string>() { "L1", "L2" };
            this.EnabledSection = new EnableFloat(24 * 60, false);
            this.OnlyCodes = new List<string>() { "L1", "L2", "C1","P1" , "P2"};
            SatsToBeRemoved = new List<SatelliteNumber>();
            this.MinEpochCount = 10;
            this.IsEnableMinEpochCount = false;
            this.MaxBreakCount = 3;
            this.BufferSize = 50;

        }
        /// <summary>
        /// 缓存大小
        /// </summary>
        public int BufferSize { get; set; }
        /// <summary>
        /// 是否修复大周跳
        /// </summary>
        public bool IsAmendBigCs { get; set; }
        /// <summary>
        /// 启用相位对齐,采用第一次伪距对齐。
        /// 当时间断裂，将采用第二次对齐。
        /// </summary>
        public bool IsEnableAlignPhase { get; set; } 

        /// <summary>
        /// 启用最小历元数量过滤
        /// </summary>
        public bool IsEnableMinEpochCount { get; set; }
        /// <summary>
        /// 最小历元数量
        /// </summary>
        public int MinEpochCount { get; set; }
        /// <summary>
        /// 启用观测码按照出勤率过滤
        /// </summary>
        public bool IsEnableMinObsCodeAppearRatio { get; set; }
        /// <summary>
        /// 观测码出勤率
        /// </summary>
        public double MinObsCodeAppearRatio { get; set; }
        /// <summary>
        /// 是否将载波转成距离
        /// </summary>
        public bool IsConvertPhaseToLength { get; set; }
        /// <summary>
        /// 是否启用观测类型过滤
        /// </summary>
        public bool IsEnableObsTypes { get; set; }
        /// <summary>
        /// 观测类型过滤
        /// </summary>
        public List<ObsTypes> ObsTypes { get; set; }
        /// <summary>
        /// 是否移除指定卫星
        /// </summary>
        public bool IsEnableRemoveSats { get; set; }
        /// <summary>
        /// 待移除的卫星
        /// </summary>
        public List<SatelliteNumber> SatsToBeRemoved { get; set; }

        /// <summary>
        /// 是否过滤系统
        /// </summary>
        public bool IsEnableSatelliteTypes { get; set; }
        /// <summary>
        /// 系统
        /// </summary>
        public List<SatelliteType> SatelliteTypes { get; set; }
        /// <summary>
        /// 是否移除0值伪距卫星。
        /// </summary>
        public bool IsRemoveZeroRangeSat { get; set; }
        /// <summary>
        /// 是否移除0值载波卫星。
        /// </summary>
        public bool IsRemoveZeroPhaseSat { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public double Version { get; set; }
        /// <summary>
        /// 输出目录
        /// </summary>
        public string OutputDirectory { get; set; }
        /// <summary>
        /// 时段信息
        /// </summary>
        public TimePeriod TimePeriod { get; set; }
        /// <summary>
        /// 是否启用时段信息
        /// </summary>
        public bool IsEnableTimePeriod { get; set; }
        /// <summary>
        /// 允许的最大断裂历元数量
        /// </summary>
        public int MaxBreakCount { get; set; }
        /// <summary>
        /// 观测码
        /// </summary>
        public List<string> ObsCodes { get; set; }
        /// <summary>
        /// 启用观测码
        /// </summary>
        public bool IsEnableObsCodes { get; set; }
        /// <summary>
        /// 采样率，秒，注意：不可加密，直客稀疏，如果小于原始数据，则采样率不变。
        /// </summary>
        public double Interval { get; set; }
        /// <summary>
        /// 启用采样率
        /// </summary>
        public bool IsEnableInterval { get; set; }
        /// <summary>
        /// 删除数据为空的卫星
        /// </summary>
        public bool IsDeleteVacantSat { get; set; }
        /// <summary>
        /// 不可为空的观测码
        /// </summary>
        public string NotVacantCodes
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in NotVacantCodeList)
                {
                    sb.Append(item);
                    sb.Append(",");
                }

                return sb.ToString();

            }
            set
            {
                var array = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                this.NotVacantCodeList = new List<string>(array);

            }        
        }
        /// <summary>
        /// 不可为空的观测码
        /// </summary>
        public List<string> NotVacantCodeList { get; private set; }
        /// <summary>
        /// 观测分段，单位：分钟
        /// </summary>
        public EnableFloat EnabledSection { get; set; }


        /// <summary>
        /// 是否移除其它代码，即只保留指定的代码。
        /// </summary>
        public bool IsReomveOtherCodes { get; set; }

        /// <summary>
        /// 只保留指定的代码。
        /// </summary>
        public List<string> OnlyCodes  { get; private set; }

        /// <summary>
        /// 不可为空的观测码
        /// </summary>
        public string OnlyCodesString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in OnlyCodes)
                {
                    sb.Append(item);
                    sb.Append(",");
                }

                return sb.ToString();

            }
            set
            {
                var array = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                this.OnlyCodes = new List<string>(array);

            }
        }

     
    }

}
