//2014.08.26, czs, create, 抽象化单点定位计算
//2014.09.16, czs, refactor, 梳理各个过程，分为历元算前、算中和算后，增加初始化、检核等方法。
//2014.10.06，czs, edit in hailutu, 将EpochInfomation的构建独立开来，采用历元信息构建器IEpochInfoBuilder初始化
//2014.11.20，czs, edit in namu, 将PointPositioner命名为AbstractPointPositioner
//2016.03.10, czs, edit in hongqing, 重构设计
//2016.04.23, czs, edit in huoda, 分离数据源，名称修改为 StreamGnssService，意思为GNSS产品服务
//2018.12.31, czs, edit in hmx, 增加无电离层双差模糊度固定算法，新年快乐！

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Checkers;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo.Times;
using Geo.IO;

namespace Gnsser.Service
{
    /// <summary>
    /// 多站同历元解算器。
    /// </summary>
    public abstract class MultiSiteEpochSolver : AbstractGnssSolver<BaseGnssResult, MultiSiteEpochInfo>
    {
        protected Log log = new Log(typeof(MultiSiteEpochSolver));
      
          /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="option"></param> 
        public MultiSiteEpochSolver(DataSourceContext context,GnssProcessOption option) 
            : base(context, option)
        {
            this.IsDualIonoFreeComObservation = Option.IsDualIonoFreeComObservation;
        }

        #region 无电离层双差模糊度固定
        /// <summary>
        /// 是否双频电离层组合
        /// </summary>
        bool IsDualIonoFreeComObservation { get; set; }

        /// <summary>
        /// 执行无电离层双差模糊度固定
        /// </summary>
        /// <param name="rawFloatAmbiCycles"></param>
        /// <param name="isNetSolve">是否网解</param>
        /// <returns></returns>
        protected WeightedVector DoFixIonoFreeDoubleDifferAmbiguity(WeightedVector rawFloatAmbiCycles, bool isNetSolve)
        {
            if (!IsDualIonoFreeComObservation)
            {
                return base.DoFixAmbiguity(rawFloatAmbiCycles);
            }
            //-----------以下为无电离层组合模糊度固定算法--------------------
            if (this.DataSourceContext.SiteSatPeriodDataService == null)
            {
                log.Warn("必须开启时段数据服务，才能实现无电离层模糊度固定！");
                return new WeightedVector();
            }

            //指定系统的无电离层组合参数计算器
            var IonoFreeAmbiguitySolver =  IonoFreeAmbiguitySolverManager.GetOrCreate(CurrentBasePrn.SatelliteType);
            IonoFreeAmbiguitySolver.CheckOrInit(CurrentBasePrn, CurrentMaterial.ReceiverTime, !Option.IsLengthPhaseValue);
           
            //----------------------第一步 MW 宽巷双差 ------------------------ 
            NameRmsedNumeralVector intMwDoubleDiffers = GetIntMwDoubleDiffers(isNetSolve);
            
            //----------------------第二步 MW 宽巷和模糊度浮点解求窄巷模糊度--------
            var ambiFloatVal = rawFloatAmbiCycles.GetNameRmsedNumeralVector();
              
            //求窄巷模糊度浮点解//单位周
            var narrowFloat = IonoFreeAmbiguitySolver.GetNarrowFloatValue(intMwDoubleDiffers, ambiFloatVal);

            var narrowFloatVect = narrowFloat.GetWeightedVector();
            narrowFloatVect.InverseWeight = rawFloatAmbiCycles.GetWeightedVector(narrowFloatVect.ParamNames).InverseWeight; //追加系数阵，按照顺序------ 

            //方法1：
            var intNarrowVector = base.DoFixAmbiguity(narrowFloatVect);
            var intNarrow = intNarrowVector.GetNameRmsedNumeralVector();// ParseVector(intNarrowVector); 
            //方法2：直接取整
            //var intNarrow = narrowFloatVect.GetRound();//不推荐使用直接取整 

            //检核窄巷
            var intDiffer = intNarrow - narrowFloat;
            var toRemoves = intDiffer.GetAbsLargerThan(this.Option.MaxAmbiDifferOfIntAndFloat);
            intNarrow.Remove(toRemoves);//移除

            //判断是否超限
            //计算双差载波模糊度固定值
            var fixedVal = IonoFreeAmbiguitySolver.GetIonoFreeAmbiValue(intMwDoubleDiffers, intNarrow);

            var result = fixedVal.GetWeightedVector();

            return result;
        }

        /// <summary>
        /// 获取双差MW整数
        /// </summary>
        /// <param name="isNetSolve"></param>
        /// <returns></returns>
        public NameRmsedNumeralVector GetIntMwDoubleDiffers(bool isNetSolve)
        {
            List<string> toRemoves = new List<string>();
            NameRmsedNumeralVector mwDoubleDiffers = this.DataSourceContext.SiteSatPeriodDataService.GetMwCycleDoubleDifferValue(CurrentMaterial, CurrentBasePrn, isNetSolve);

            //移除RMS太大的数据，RMS太大可能发生了周跳。
            toRemoves = mwDoubleDiffers.GetRmsLargerThan(this.Option.MaxAllowedRmsOfMw);
            mwDoubleDiffers.Remove(toRemoves);//移除

            //直接四舍五入，求MW双差模糊度
            var intMwDoubleDiffers = mwDoubleDiffers.GetRound();

            ////求残差,由于前面采用四舍五入的方法求取，因此，大于0.5的判断是没有意义的
            if (Option.MaxRoundAmbiDifferOfIntAndFloat < 0.5)    //移除残差大于指定的数据
            {
                var residuals = mwDoubleDiffers - intMwDoubleDiffers; 
                toRemoves = residuals.GetAbsLargerThan(Option.MaxRoundAmbiDifferOfIntAndFloat);
                intMwDoubleDiffers.Remove(toRemoves);//移除
            }

            return intMwDoubleDiffers;
        }
        #endregion
    }
}