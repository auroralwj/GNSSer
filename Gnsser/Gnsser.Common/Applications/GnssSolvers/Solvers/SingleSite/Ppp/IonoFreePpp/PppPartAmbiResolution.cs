//2014.08.30, czs, edit, 开始重构
//2014.08.31, czs, edit, 重构于 西安 到 沈阳 的航班上，春秋航空。

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo;
using Geo.Utils;
using Geo.Common;
using Gnsser.Domain;
using Geo.Algorithm.Adjust;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Gnsser.Service;
using Gnsser.Checkers;
using Geo.Algorithm;
using Geo.Times;

namespace Gnsser.Service
{
    // PPP， Ar=  AmbiguityResolution, PartAmbiResolution,
    //包含4类参数，测站位置（x,y,z），钟差（Cdt），对流程天顶距延迟(zpd)和非整的整周模糊度（N）。
     
    /// <summary>
    /// 精密单点定位。
    /// 此处采用观测值残差向量计算。
    /// 条件：必须是双频观测，且观测卫星数量大于5个。
    /// 参考：Jan Kouba and Pierre Héroux. GPS Precise Point Positioning Using IGS Orbit Products[J].2000,sep
    /// </summary>
    public class PppPartAmbiResolution : SingleSiteGnssSolver
    {
        #region 构造函数
        /// <summary>
        /// 精密单点定位。
        /// </summary>
        /// <param name="navFile">星历数据源</param>
        /// <param name="model">解算模型，数据输入模型</param>
        /// <param name="clockFile">钟差数据源</param>
        public PppPartAmbiResolution(DataSourceContext DataSourceContext, GnssProcessOption PositionOption)
            : base(DataSourceContext, PositionOption)
        {  
            this.IonoFreeAmbiguityMgr = new IonoFreeAmbiguityMgr();

           this.MatrixBuilder = new IonoFreePppMatrixBuilder(this.Option);
            //#region 周跳管理器   在AbstractEpochInfoBuilder.cs中添加，更早的探测周跳，尽可能保存已有数据，特别是低角度的数据
            ////需要一个维护卫星状态的类，避免新卫星收老数据的影响 
            //this.EpochInfoBuilder.AddPreProcessor(CycleSlipRemoveChainProcessor.Default());

            //#endregion 

            //SatAmbiguityManager CycleClipManager = new SatAmbiguityManager();
            //this.EpochInfoBuilder.AddPreProcessor(new AliningIonoFreePhaseProcessor(CycleClipManager));
        }
        #endregion

        #region 属性 
        /// <summary>
        /// 无电离层模糊度管理器
        /// </summary>
        protected IonoFreeAmbiguityMgr IonoFreeAmbiguityMgr { get; set; }

         
        #endregion

         
        /// <summary>
        /// 逐历元更新的，每颗卫星的MW平滑值
        /// </summary>
        private Dictionary<SatelliteNumber, SmoothValue> MwInfoDic = new Dictionary<SatelliteNumber, SmoothValue>();
     
         
        #region 卡尔曼滤波
        /// <summary>
        /// PPP 计算核心方法。 Kalmam滤波。
        /// 观测量的顺序是先伪距观测量，后载波观测量，观测量的总数为卫星数量的两倍。
        /// 参数数量为卫星数量加5,卫星数量对应各个模糊度，5为3个位置量xyz，1个接收机钟差量，1个对流程湿分量。
        /// </summary>
        /// <param name="recInfo">接收信息</param>
        /// <param name="option">解算选项</param> 
        /// <param name="lastPppResult">上次解算结果（用于 Kalman 滤波）,若为null则使用初始值计算</param>
        /// <returns></returns>
        public override SingleSiteGnssResult CaculateKalmanFilter(EpochInformation recInfo, SingleSiteGnssResult lastPppResult = null)
        { 
                PppResult last = null;
                if (lastPppResult != null) last = (PppResult)lastPppResult;
                //  ISatWeightProvider SatWeightProvider = new SatElevateAndRangeWeightProvider();
                //ISatWeightProvider SatWeightProvider = new SatElevateWeightProvider();
                 

                MatrixBuilder.SetMaterial(recInfo).SetPreviousProduct(lastPppResult);
                MatrixBuilder.Build();

                if (this.MatrixBuilder.ObsCount > 0)
                {

                //  this.Adjustment = new KalmanFilter( this.MatrixBuilder);
                this.Adjustment = this.RunAdjuster(BuildAdjustObsMatrix(this.CurrentMaterial));


                ////尝试固定模糊度  cuiyang 2015.07
                //int fixFlag = Ppp_AR.Process(recInfo, Adjustment);

                #region 具有条件的参数平差
                // this.Adjustment.restrictadjust();
                #endregion

                #region PPP模糊度固定
                foreach (var sat in recInfo.EnabledPrns)
                    {
                        if (recInfo.SlipedPrns.Contains(sat))//主要是针对 1.首次观测到某颗卫星 2.某颗卫星出现在第二个弧段，两个弧段的MW不相等
                        {
                            if (MwInfoDic.ContainsKey(sat)) MwInfoDic.Remove(sat);
                            continue;
                        }
                      
                        if (!MwInfoDic.ContainsKey(sat)){   //第一个没有周跳的历元
                            MwInfoDic[sat] = new SmoothValue(100, true) { Name = sat.ToString()};
                        }

                        MwInfoDic[sat].Regist(recInfo[sat].Combinations.MwRangeCombination.Value);
                    }

                    List<int> cyclesat = new List<int>(); //将有周跳的卫星挑出来
                    foreach (var sat in recInfo.SlipedPrns)
                    {
                        cyclesat.Add(sat.PRN);
                    }
                    if (MwInfoDic.Count > 0)
                    {
                        Dictionary<int, double> MWs = new Dictionary<int, double>();
                        foreach (var sat in recInfo.EnabledPrns)
                        {
                            if (!recInfo.SlipedPrns.Contains(sat))
                            {
                                int num = sat.PRN;
                                MWs.Add(num, MwInfoDic[sat].Value);//宽巷模糊度的历元平滑值
                            }
                        }
                        IMatrix new_estimated = this.Adjustment.PppArmbiResolve(MWs, cyclesat);
               
                    }
                    #endregion




                    PppResult result = new PppResult(recInfo, Adjustment, this.MatrixBuilder.GnssParamNameBuilder);
                  //  result.PreviousResult = lastPppResult;

                    //模糊度设置
                    IonoFreeAmbiguityMgr.SetIonoFreeCombination(result);


                    return result;
                }
                else
                    return null; 
        }
        #endregion   
        

    }
}
