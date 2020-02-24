//2014.09.10, czs, create, 管理随机模型
//2015.09.25, 可以采用字典类型。
//2016.03.10, czs, edit in hongqing, 重构，简化
//2017.09.02, czs, edit in hongqing, 增加双差等自动支持
//2017.09.02, lly, edit in zhegnzhou, 增加多系统PPP
//2017.09.05, czs, edit in hongqing, 去掉默认静态类，去掉数字检索，全部采用名字匹配
//2018.10.26, czs, edit in hmx, 支持测站-基准星模糊度

using System;
using System.Linq;
using System.Collections.Generic;
using Gnsser.Domain;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Checkers;
using Geo.Common;
using Geo;
using Gnsser.Models;

namespace Gnsser.Service
{
    /// <summary>
    /// 参数状态转移模型管理器。
    /// </summary>
    public class ParamStateTransferModelManager : BaseDictionary<string, BaseStateTransferModel>
    {
        /// <summary>
        /// 构造函数
        /// </summary> 
        public ParamStateTransferModelManager(GnssProcessOption Option)
        {
            this.Option = Option;
        }
        /// <summary>
        /// 选项
        /// </summary>
        public GnssProcessOption Option { get; set; }

        #region 方法
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="stochasticModel"></param>
        public void Add(BaseStateTransferModel stochasticModel)
        {
            this.Add(this.Count.ToString(), stochasticModel);
        }
        /// <summary>
        /// 检索
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public BaseStateTransferModel this[SatelliteNumber prn]
        {
            get { return this[prn.ToString()]; }
            set { this[prn.ToString()] = value; }
        }


        /// <summary>
        /// 更新所有的转移模型
        /// </summary>
        /// <param name="entity"></param>
        public void Update(ISiteSatObsInfo entity)
        {
            foreach (var item in this)
            {
                item.Init(entity);
            }
        }

        /// <summary>
        /// 默认的状态转移模型生成
        /// </summary>
        /// <param name="key">参数名称</param>
        /// <returns></returns>
        public override BaseStateTransferModel Create(string key)
        {
            if (
                key.Contains(ParamNames.Dx)
                || key.Contains(ParamNames.Dy)
                || key.Contains(ParamNames.Dz)
                )
            {
                if (Option.PositionType == PositionType.静态定位)
                {
                    return new StaticTransferModel(Option.StdDevOfStaticTransferModel);
                }
                else//动态定位
                {
                    return new WhiteNoiseModel(Option.StdDevOfWhiteNoiseOfDynamicPosition);// 50, lly edit in 2018.07.14
                    //return new RandomWalkModel(Option.StdDevOfRandomWalkModel);
                }
            }else if (
                key.Contains(ParamNames.Dvx)
                || key.Contains(ParamNames.Dvy)
                || key.Contains(ParamNames.Dvz)
                )
            {
                if (Option.PositionType == PositionType.静态定位)
                {
                    return new StaticTransferModel(Option.StdDevOfStaticTransferModel);
                }
                else//动态定位
                {
                    return new WhiteNoiseModel(Option.StdDevOfWhiteNoiseOfDynamicPosition);//50, lly edit in 2018.07.14
                    //return new RandomWalkModel(Option.StdDevOfRandomWalkModel);
                }
            }
            else if (
                key.Contains(ParamNames.RcvClkDriftDistance)
                ) {
                return new RandomWalkModel(Option.StdDevOfSysTimeRandomWalkModel);
            } 
            else if (key.Contains(ParamNames.SatClkErrDistance))
            {
                return new WhiteNoiseModel(Option.StdDevOfSatClockWhiteNoiseModel);
            }
            else if (
                key.Contains(ParamNames.RcvClkErrDistance)
                || key.Contains(ParamNames.cDt)
                )
            {
                return new WhiteNoiseModel(Option.StdDevOfRevClockWhiteNoiseModel);
            }
            else if (
                key.Contains(ParamNames.WetTropZpd)
                || key.Contains(ParamNames.Trop)
                )
            {
                return new RandomWalkModel(Option.StdDevOfTropoRandomWalkModel);
            } 
            else if (
                key.Contains(ParamNames.Iono)
                )
            {
                return new RandomWalkModel(Option.StdDevOfIonoRandomWalkModel);
            }
            else if (key.Contains(ParamNames.SysTimeDistDifferOf))
            {
                return new RandomWalkModel(Option.StdDevOfSysTimeRandomWalkModel);
            }
            else if (
                key.Contains(ParamNames.DifferAmbiguity)
                || key.Contains(ParamNames.DoubleDifferAmbiguity) //双星差分模糊度，具有基准星
                )
            {
                var prn = SatelliteNumber.TryParseFirst(key);//约定：第一个是当前卫星，第二个是基准星
                return new DueSatPhaseAmbiguityMode(prn, Option.StdDevOfPhaseModel, Option.StdDevOfCycledPhaseModel);
            }
            else if (
                key.Contains(ParamNames.PhaseLengthSuffix)
                || key.Contains(ParamNames.PhaseALengthSuffix)
                || key.Contains(ParamNames.PhaseBLengthSuffix)
                || key.Contains(ParamNames.PhaseCLengthSuffix)
                )
            { 
                if (key.Count(m => m == '_') >= 2)// 2 个以上分隔符，则认为是有测站
                {
                    var strs = key.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                    var siteName = strs[0];
                    var prn = SatelliteNumber.Parse(strs[1]);
                    if (prn == SatelliteNumber.Default)
                    {
                        log.Error("卫星 名称解析错误！！" + key);
                        throw new Exception("卫星 名称解析错误！！" + key);
                    }
                    return new SiteSatPhaseAmbiguityModel(siteName, prn, Option.StdDevOfPhaseModel, Option.StdDevOfCycledPhaseModel);
                }
                else
                {
                    var prn = SatelliteNumber.Parse(key);
                    if (prn == SatelliteNumber.Default)
                    {
                        log.Error("卫星 名称解析错误！！" + key);
                        throw new Exception("卫星 名称解析错误！！" + key);
                    }
                    return new SingleSatPhaseAmbiguityModel(prn, Option.StdDevOfPhaseModel, Option.StdDevOfCycledPhaseModel);
                }
            }
            else if (
                key.Contains(ParamNames.Dcb)
                )
            {
                var prn = SatelliteNumber.Parse(key);

                //      return new StaticTransferModel(Option.StdDevOfStaticTransferModel);
                return new WhiteNoiseModel(Option.StdDevOfRevClockWhiteNoiseModel);
                return new RandomWalkModel(0.001);
                return new SingleSatPhaseAmbiguityModel(prn, 0.001, Option.StdDevOfCycledPhaseModel);
            }

            return base.Create(key);
        }
        #endregion
          
    }
}
