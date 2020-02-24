//2018.10.24, czs, create in hmx, 非差轨道确定

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
using Geo.Times;

namespace Gnsser
{

    /// <summary>
    /// 非差轨道确定。
    ///  </summary>
    public class ZeroDifferOrbitSolver : MultiSiteEpochSolver
    {
        #region 构造函数
        /// <summary>
        /// 非差轨道确定
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="option"></param>
        public ZeroDifferOrbitSolver(DataSourceContext DataSourceContext, GnssProcessOption option)
            : base(DataSourceContext, option)
        {
            if(option.AdjustmentType == AdjustmentType.递归最小二乘)
            {
                this.MatrixBuilder = new RecursiveZeroDifferOrbitMatrixBuilder(option);
            }
            else
            {
                this.MatrixBuilder = new ZeroDifferOrbitMatrixBuilder(option);
            }
        }
        #endregion

        public override BaseGnssResult Produce(MultiSiteEpochInfo material)
        {
            //采用导航星历计算卫星初始坐标
            var ephService = GlobalNavEphemerisService.Instance;
            RangeOrbitResult result = null;
            RangeOrbitResult prevResult = null;
            double timeDiffer = 1;
            int loopCount = 0;
            double loopThreshold = 1000 / GnssConst.LIGHT_SPEED;
            do
            {
                prevResult = result;

                foreach (var site in material)
                {
                    foreach (var sat in site)
                    {
                        if (prevResult == null)
                        {
                            var emissionTime = sat.RecevingTime - sat.GetTransitTime();
                            var eph = ephService.Get(sat.Prn, emissionTime);//不改正相对论、钟差等
                            sat.Ephemeris = eph;
                        }
                        else
                        {
                            var ephResult = prevResult.EphemerisResults.Get(sat.Prn);
                            sat.Ephemeris = ephResult.Corrected;
                        }

                        //卫星钟差改正，根据计算的卫星钟差改正距离
                        new Gnsser.Correction.SatClockBiasCorrector().Correct(sat);
                    }
                    //直接计算，不用改正
                    //EpochEphemerisSetter.ReviseEphemerisOnly(site);
                }

                double old = 0d;
                if (prevResult != null)
                {
                    old = prevResult.MaterialObj.BaseEpochInfo.Time.Correction;
                }
                result = (ZeroDifferOrbitResult)base.Produce(material);

                if (prevResult != null)
                {
                    timeDiffer = old - result.MaterialObj.BaseEpochInfo.Time.Correction;
                    int ii = 0;
                }
                if (loopCount > 4)
                {
                    log.Error("迭代次数达到 " + loopCount + " 次，似乎不收敛！" + timeDiffer);
                }
                loopCount++;
            } while (loopCount < 8 && (prevResult == null || Math.Abs(timeDiffer) > loopThreshold));//|| prevResult.

            return result;

        }


        public override BaseGnssResult BuildResult()
        {
            var result = new ZeroDifferOrbitResult(this.CurrentMaterial, Adjustment, (ZeroDifferOrbitParamNameBuilder)this.MatrixBuilder.GnssParamNameBuilder);
           // result.PreviousResult = this.CurrentProduct;
            result.BasePrn = this.CurrentBasePrn;
            return result;
        } 

    }
}
