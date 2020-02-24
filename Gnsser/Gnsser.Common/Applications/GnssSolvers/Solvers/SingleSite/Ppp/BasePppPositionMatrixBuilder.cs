// 2014.09.03, czs, 独立于Ppp.cs
// 2014.09.05, czs, edit, 实现 IAdjustMatrixBuilder 接口
// 2016.03.11, czs, edit in hongqing, 提取基础的精密单点定位平差矩阵构建器

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using Geo.Common;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Times;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Data.Rinex;
using Gnsser.Checkers;
using Gnsser.Models;
using Geo.Times; 
using Geo;

namespace Gnsser.Service
{
    /// <summary>
    /// 基础的精密单点定位平差矩阵构建器
    /// </summary>
    public abstract class BasePppPositionMatrixBuilder : SingleSiteGnssMatrixBuilder
    {
        /// <summary>
        /// 先构造，再设置历元值。
        /// </summary>
        /// <param name="option">解算选项</param> 
        public BasePppPositionMatrixBuilder(GnssProcessOption option)
            : base(option)
        {
       
        }

        #region 全局基础属性 

        /// <summary>
        /// 卫星数量，观测值数量。
        /// </summary>
        public override int ObsCount { get { return EnabledSatCount * 2; } }
        #endregion


        #region 创建观测信息, 默认为无电离层组合

        /// <summary>
        /// 具有权值的观测值。
        /// </summary> 
        public override WeightedVector Observation
        {
            get
            {
                IMatrix inverseWeightOfObs = BulidInverseWeightOfObs();
                WeightedVector deltaObs = new WeightedVector(ObservationVector, inverseWeightOfObs);
                return deltaObs;
            }
        }

        /// <summary>
        /// 观测值。
        /// 自由项 l，观测值减去先验值或估计值。
        /// 常数项，观测误差方程的常数项,或称自由项
        /// </summary>
        public virtual IVector ObservationVector
        {
            get
            {
                Vector L = new Vector(ObsCount);
                int satIndex = 0;
                XYZ staXyz = CurrentMaterial.SiteInfo.EstimatedXyz;

                Vector rangeVector = CurrentMaterial.GetAdjustVector(SatObsDataType.IonoFreeRange);

                Vector phaseVector = null;
                if (this.Option.IsAliningPhaseWithRange)
                {
                    phaseVector = CurrentMaterial.GetAdjustVector(SatObsDataType.AlignedIonoFreePhaseRange, true);
                }
                else
                {
                    phaseVector = CurrentMaterial.GetAdjustVector(SatObsDataType.IonoFreePhaseRange, true);
                }
                foreach (var item in CurrentMaterial.EnabledPrns)
                {
                    L[satIndex] = rangeVector[satIndex]; 

                    int nextIndex = satIndex + EnabledSatCount;
                    L[nextIndex] = phaseVector[satIndex]; 

                    satIndex++;
                }
                L.ParamNames = BuildObsNames();
                return L;
            }
        }

        /// <summary>
        /// 构建观测名称
        /// </summary>
        /// <returns></returns>
        public List<string> BuildObsNames()
        {
            var paramNames = new List<string>();

            foreach (var item in CurrentMaterial.EnabledPrns)
            {
                paramNames.Add(item + "_" + Gnsser.ParamNames.LCode);
            }
            foreach (var item in CurrentMaterial.EnabledPrns)
            {
                paramNames.Add(item + "_" + Gnsser.ParamNames.PCode);
            }
            return paramNames;
        }

        /// <summary>
        /// 观测量的权逆阵，一个对角阵。按照先伪距，后载波的顺序排列。
        /// 标准差由常数确定，载波比伪距标准差通常是 1:100，其方差比是 1:10000.
        /// 1.0/140*140=5.10e-5
        /// </summary> 
        /// <returns></returns>
        protected virtual IMatrix BulidInverseWeightOfObs()
        {
            int satCount = EnabledSatCount;
            int row = satCount * 2;
            DiagonalMatrix inverseWeight = new DiagonalMatrix(row, 1);
            double[] inverseWeightVector = inverseWeight.Vector;
            double invFactorOfRange = 1;
            double invFactorOfPhase = this.PhaseCovaProportionToRange;//  0.003 * 0.003;
            int i = 0;
            foreach (var sat in CurrentMaterial.EnabledSats)// 一颗卫星1行
            {
                double inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(sat);
                inverseWeightVector[i] = inverseWeightOfSat * invFactorOfRange;
                inverseWeightVector[i + satCount] = inverseWeightOfSat * invFactorOfPhase;
                i++;
            }
            return (inverseWeight);
        }

        #endregion

    }

}