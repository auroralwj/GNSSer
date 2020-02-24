//2015.01.10, czs, create in namu, 观测数据分析专家接口

using System;

namespace Gnsser 
{
    /// <summary>
    ///  观测数据分析专家
    /// </summary>
    public  interface IObsDataAnalyst
    {

        /// <summary>
        /// 卫星周跳时段标记。
        /// </summary>
        SatPeriodInfoManager SatCycleSlipMaker { get; set; }

        /// <summary>
        /// 卫星选择器。
        /// </summary>
        SatelliteSelector SatelliteSelector { get; set; }

        /// <summary>
        /// 卫星可见性时段标记。
        /// </summary>
        SatPeriodInfoManager SatVisibleMaker { get; set; }
    }

    /// <summary>
    /// 观测文件分析者
    /// </summary>
    public class BaseObsDataAnalyst : IObsDataAnalyst
    {
        /// <summary>
        /// 观测文件分析者，构造函数
        /// </summary>
        public BaseObsDataAnalyst()
        { 
        }

        /// <summary>
        /// 卫星选择器。
        /// </summary>
        public SatelliteSelector SatelliteSelector { get; set; }

        /// <summary>
        /// 卫星可见性时段标记。
        /// </summary>
        public SatPeriodInfoManager SatVisibleMaker { get; set; }

        /// <summary>
        /// 卫星周跳时段标记。
        /// </summary>
        public SatPeriodInfoManager SatCycleSlipMaker { get; set; }

    } 
}
