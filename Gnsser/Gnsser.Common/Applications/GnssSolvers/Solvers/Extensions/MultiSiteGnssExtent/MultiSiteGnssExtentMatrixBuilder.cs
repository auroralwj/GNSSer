//2017.08.28, czs & kyc, create in hongqing, 多站单历元GNSS计算预留测试类
//2018.11.02, czs, edit in hmx, 整理代码，精简，方便扩展应用


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using Geo.Common;
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
using System.Threading.Tasks;

namespace Gnsser
{
    /// <summary>
    /// 多站单历元GNSS计算预留测试类
    /// </summary>
    public class MultiSiteGnssExtentMatrixBuilder : MultiSiteMatrixBuilder
    {
        /// <summary>
        /// 多站单历元GNSS计算预留测试类 构造函数。
        /// </summary>
        /// <param name="option">解算选项</param>
        public MultiSiteGnssExtentMatrixBuilder(
            GnssProcessOption option)
            : base(option)
        {
            this.Option = option;
            this.ParamNameBuilder = new MultiSiteGnssExtentParamNameBuilder(option);
        }


        #region 全局基础属性 

        public override int BaseClockCount
        {
            get
            {
                int BaseCount = CurrentMaterial.Count + EnabledPrns.Count;
                return BaseCount;
            }
        }

        /// <summary>
        /// 观测数量
        /// </summary>
        public override int ObsCount
        {
            get
            {
                int obsCount = 0;
                foreach (var EpochInfo in CurrentMaterial)
                {
                    obsCount += EpochInfo.EnabledSatCount;
                }
                return obsCount;
            }
        }
        #endregion


        #region 创建观测信息

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
                Vector L = new Vector(ObsCount * 2);
                int satIndex = 0;
                foreach (var epoch in CurrentMaterial)
                {
                    Vector rangeVector = epoch.GetAdjustVector(SatObsDataType.IonoFreeRange);
                    Vector phaseVector = null;
                    if (this.Option.IsAliningPhaseWithRange)
                    {
                        phaseVector = epoch.GetAdjustVector(SatObsDataType.AlignedIonoFreePhaseRange, true);
                    }
                    else
                    {
                        phaseVector = epoch.GetAdjustVector(SatObsDataType.IonoFreePhaseRange, true);
                    }
                    int index = 0;
                    foreach (var item in epoch.EnabledSats)
                    {
                        L[satIndex] = rangeVector[index];
                        L[satIndex + ObsCount] = phaseVector[index];
                        satIndex++;
                        index++;
                    }
                }

                return L;
            }
        }

        /// <summary>
        /// 观测量的权逆阵，一个对角阵。按照先伪距，后载波的顺序排列。
        /// 标准差由常数确定，载波比伪距标准差通常是 1:100，其方差比是 1:10000.
        /// 1.0/140*140=5.10e-5
        /// </summary> 
        /// <returns></returns>
        public IMatrix BulidInverseWeightOfObs()
        {
            //double[][] inverseWeight = Geo.Utils.MatrixUtil.Create(ObsCount * 2);
            DiagonalMatrix inverseWeight = new DiagonalMatrix(ObsCount * 2);
            double[] inverseWeightVector = inverseWeight.Vector;
            double invFactorOfRange = 1;
            double invFactorOfPhase = Option.PhaseCovaProportionToRange;

            int index1 = 0;
            foreach (var epoch in this.CurrentMaterial)
            {
                foreach (var prn in epoch.EnabledPrns)
                {
                    EpochSatellite e = epoch[prn];
                    double inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(e);
                    inverseWeight[index1] = inverseWeightOfSat * invFactorOfRange;
                    inverseWeight[index1 + ObsCount] = inverseWeightOfSat * invFactorOfPhase;
                    index1++;
                }
            }
            return inverseWeight;
        }

        #endregion

        #region 公共矩阵生成  以某一接收机钟为参考

        /// <summary>
        /// 参数平差系数阵 A。前一半行数为伪距观测量，后一半为载波观测量。构建为稀疏矩阵
        /// </summary>
        public override Matrix Coefficient
        {
            get
            {
                int rowCount = ObsCount * 2;
                int colCount = ParamCount;
                double[][] A = Geo.Utils.MatrixUtil.Create(rowCount, colCount);

                Time time = CurrentMaterial.ReceiverTime;

                DateTime start = DateTime.Now;
                int row = 0;//行的索引号
                int i = 0;
                foreach (var item in CurrentMaterial)
                {
                    var wetTropName = this.GnssParamNameBuilder.GetSiteWetTropZpdName(item);
                    int IndexOfwetTropName = ParamNames.IndexOf(wetTropName);

                    foreach (var prn in item.EnabledSats)// 一颗卫星2行
                    {
                        double wetMap = item[prn.Prn].WetMap;

                        //double wetMap0 = key[prn.Prn].Vmf1WetMap;
                        int next = row + ObsCount;

                        A[row][i] = 1.0;//接收机钟差对应的距离 = clkError * 光速
                        A[next][i] = 1.0;//接收机钟差对应的距离 = clkError * 光速

                        var satelliteName = this.GnssParamNameBuilder.GetSatClockParamName(prn.Prn);
                        int IndexOfsatelliteName = ParamNames.IndexOf(satelliteName);
                        var SiteSatAmbiguityName = this.GnssParamNameBuilder.GetSiteSatAmbiguityParamName(prn);
                        A[row][IndexOfwetTropName] = wetMap;
                        A[row][IndexOfsatelliteName] = -1.0;//卫星钟差对应的距离 = clkError * 光速

                        A[next][IndexOfwetTropName] = wetMap;
                        A[next][IndexOfsatelliteName] = -1.0;//卫星钟差对应的距离 = clkError * 光速
                        A[next][ParamNames.IndexOf(SiteSatAmbiguityName)] = 1;//模糊度,保持以m为单位 

                        row++;
                    }
                    i++;
                }
                //var span = DateTime.Now - start;
                //Console.WriteLine(span.TotalMilliseconds + "ms计算A");
                //var ss = new SparseMatrix(A).Minus(new SparseMatrix(A));
                //  return new SparseMatrix(A);
                return new Matrix(A);
            }
        }
        #endregion
    }
}
