//2014.09.10, czs, create, 周跳探测初步
//2017.08.13, czs, edit in hongiqng, 面向对象重构，参数可配置处理
//2017.10.26, czs, edit in hongqing, 重新整理命名周跳探测类型

using System;
using Gnsser.Domain;
using Geo;
using Geo.Times;
using Gnsser.Core;
using System.Text;

namespace Gnsser
{
    /// <summary>
    /// 周跳探测的类型
    /// </summary>
    public enum CycleSlipDetectorType
    { 
        /// <summary>
        /// 首次出现时间探测
        /// </summary>
        首次出现标记法,
        /// <summary>
        /// 通过平均数判断是否超限
        /// </summary>
        数值平均法,
        /// <summary>
        /// 多项式拟合
        /// </summary>
        多项式拟合法,
        /// <summary>
        /// 高次差
        /// </summary>
        高次差法,
        /// <summary>
        /// 灰色模型
        /// </summary>
        灰色模型法,

        #region  以下是数类型，不应该是方法
        /// <summary>
        /// 其实这只是一个数据 类型，不应该是探测方法
        /// </summary>
        LI组合,
        /// <summary>
        /// 其实这只是一个数据 类型，不应该是探测方法
        /// </summary>
        MW组合,
        /// <summary>
        /// 其实这只是一个数据 类型，不应该是探测方法
        /// </summary>
        三频GF1组合,
        /// <summary>
        /// 其实这只是一个数据 类型，不应该是探测方法
        /// </summary>
        三频GF2组合,
        /// <summary>
        /// 其实这只是一个数据 类型，不应该是探测方法
        /// </summary>
        三频MW组合,
        #endregion
    } 


    /// <summary>
    /// 周跳探测结果通用接口。CycleSlip 缩写为 CS。
    /// </summary>
    public interface ICycleSlipDetector : Geo.Namable
    {
        /// <summary>
        /// 周跳探测类型。
        /// </summary>
        CycleSlipDetectorType DetectorType { get; }

        /// <summary>
        /// 执行探测.如果找到了，返回true。
        /// </summary>
        bool Detect(EpochSatellite epochInfo);

        /// <summary>
        /// 周跳探测结果存储器
        /// </summary>
        InstantValueStorage CycleSlipStorage { get; set; }
        /// <summary>
        /// 当前卫星测量数据
        /// </summary>
        Gnsser.Domain.EpochSatellite EpochSat { get; set; }
        /// <summary>
        /// 是否保存结果到表
        /// </summary>
        bool IsSaveResultToTable { get; set; }
        /// <summary>
        /// 是否使用已经记录的周跳信息
        /// </summary>
        bool IsUsingRecordedCsInfo { get; set; }
        /// <summary>
        ///结果存储器
        /// </summary>
        Geo.ObjectTableManager TableObjectManager { get; set; }
        /// <summary>
        /// 当前探测结果
        /// </summary>
        bool CurrentResult { get; set; }
    }

}