//2018.11.09, czs, create in HMX, 一条基线的平差结果
//2018.11.10, czs, create in hmx, 增加基线组合类
//2018.11.30, czs, create in hmx, 实现IToTabRow接口，用于规范输出,合并定义新的 BaseLineNet

using System;
using System.Collections.Generic;
using Gnsser.Domain;
using System.Text;
using System.Linq;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Filter;
using Gnsser.Checkers;
using Geo.Common;
using Geo;
using AnyInfo.Graphs.Structure;
using Geo.Times;
using AnyInfo.Graphs;

namespace Gnsser
{

    /// <summary>
    /// 独立基线选择算法
    /// </summary>
    public enum IndependentLineSelectType
    {
        闭合差最小,
        RMS最小,
        距离最短,
    }


    /// <summary>
    /// 基线权赋值
    /// </summary>
    public class BaseLineWeighter : AnyInfo.Graphs.AbstractSimpleEdgeWeighter
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="BaseLineNet">应该移除超限的基线</param>
        /// <param name="IndependentLineSelectType">选择算法</param>
        public BaseLineWeighter(BaseLineNet BaseLineNet, IndependentLineSelectType IndependentLineSelectType)
        {
            this.BaseLineNet = BaseLineNet;
            this.IndependentLineSelectType = IndependentLineSelectType;


            //生成所有可能的三角形，然后提取网络，计算闭合差 
            CurrentQualityManager = BaseLineNet.BuildTriangularClosureNets();
            if (CurrentQualityManager.Count == 0) { return; } 
        }
        /// <summary>
        /// 基线质量
        /// </summary>
        TriguilarNetManager CurrentQualityManager { get; set; }
        /// <summary>
        /// 当前所有基线集合，基线组成的网络
        /// </summary>
        BaseLineNet BaseLineNet { get; set; }
        IndependentLineSelectType IndependentLineSelectType;
        /// <summary>
        /// 计算权
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public override double GetWeight(INode start, INode end)
        {
            var lineName = new GnssBaseLineName(start.Id, end.Id);
            var est = BaseLineNet.GetOrReversed(lineName);
            if(est == null) { return double.MaxValue; }//此路不通

            switch (IndependentLineSelectType)
            {
                case IndependentLineSelectType.距离最短:
                    return est.EstimatedVector.Length;
                    break;
                case IndependentLineSelectType.RMS最小:
                    return est.StdDev;//.EstimatedRmsXyzOfRov.Rms.Length;
                    break;
                case IndependentLineSelectType.闭合差最小:
                    //如果没有，则返回一个大值
                    var quality = CurrentQualityManager.GetBest(lineName);
                    if(quality != null)
                    {
                        return quality.ClosureError.Value.Length;
                    }
                    return double.MaxValue;
                    break;
                default:
                    return est.EstimatedVector.Length;
                    break;
            } 
        } 
    }
}