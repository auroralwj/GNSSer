//2017.07.15, czs, create in hongqing, 双差网解定位框架搭建

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
    /// 双差网解定位
    /// </summary>
    public class NetDoubleDifferMatrixBuilder:MultiSiteMatrixBuilder
    {
        /// <summary>
        /// 双差网解定位 构造函数。
        /// </summary>
        /// <param name="recInfo">观测信息</param>
        /// <param name="option">解算选项</param>
        public NetDoubleDifferMatrixBuilder(
            GnssProcessOption option)
            : base(option)
        {
            this.Option = option;
            this.SatWeightProvider = new SatElevateWeightProvider(option);
            this.ParamNameBuilder = new ClockParamNameBuilder(option);            
        }
        
        public int SiteCount { get { return CurrentMaterial.Count; } }

        #region 全局基础属性 

        public override bool IsParamsChanged
        {
            get
            {
                if (PreviousProduct != null)
                {
                    List<string> ParamNames1 = ParamNames;
                    List<string> ParamNames2 = PreviousProduct.ParamNames;
                    return !Geo.Utils.ListUtil.IsEqual(ParamNames1, ParamNames2);
                }
                return true;
            }
        }


        public override List<string> BuildParamNames()
        { 
            var nameBuilder = (ClockParamNameBuilder)this.ParamNameBuilder;
            nameBuilder.EpochInfos = CurrentMaterial;
            nameBuilder.SatelliteNumbers = EnabledPrns;
            this.ParamNames = nameBuilder.Build();
            return ParamNames;
        }
        
        public override void UpdateStateTransferModels()
        {
            var nameBuilder = (ClockParamNameBuilder)this.ParamNameBuilder;

            //Parallel.ForEach(CurrentMaterial, new Action<EpochInformation>(delegate(EpochInformation epoch)
            //{
            //    UpdateStateTransferModelsOfParam(nameBuilder, epoch);
            //}));

            foreach (var epoch in CurrentMaterial)
            {
                UpdateStateTransferModelsOfParam(nameBuilder, epoch);
            }
            foreach (var sat in EnabledPrns)
            {
                var satKey = nameBuilder.GetSatClockParamName(sat);
                if (!ParamStateTransferModelManager.Contains(satKey))
                {
                    ParamStateTransferModelManager.Add(satKey,  new WhiteNoiseModel(Option.StdDevOfRevClockWhiteNoiseModel));
                }
            }
        }

        private void UpdateStateTransferModelsOfParam(ClockParamNameBuilder nameBuilder, EpochInformation epoch)
        {
            var recKey = nameBuilder.GetReceiverClockParamName(epoch);
            if (!ParamStateTransferModelManager.Contains(recKey))
            {
                ParamStateTransferModelManager.Add(recKey,  new WhiteNoiseModel(Option.StdDevOfRevClockWhiteNoiseModel));
            }
            var tropKey = nameBuilder.GetSiteWetTropZpdName(epoch);
            if (!ParamStateTransferModelManager.Contains(tropKey))
            {
                ParamStateTransferModelManager.Add(tropKey, new RandomWalkModel(Option.StdDevOfTropoRandomWalkModel));
            }
            foreach (var s in epoch.EnabledSats)
            {
                var ambiKey = nameBuilder.GetSiteSatAmbiguityParamName(s);
                if (!ParamStateTransferModelManager.Contains(ambiKey))
                {
                    ParamStateTransferModelManager.Add(ambiKey, new SingleSatPhaseAmbiguityModel(s.Prn, Option.StdDevOfPhaseModel, Option.StdDevOfCycledPhaseModel));
                }
            }
        }

        /// <summary>
        /// 参数数量
        /// </summary>
        public override int ParamCount
        {
            get
            {
                int ParamCount = 0;
                ParamCount = ObsCount + 2 * CurrentMaterial.Count + EnabledPrns.Count;
                return ParamCount;
            }
        }
        public int BaseCount
        {
            get
            {
                int BaseCount = 2 * CurrentMaterial.Count + EnabledPrns.Count;
                return BaseCount;
            }
        }
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

       
        #region 参数先验信息
        /// <summary>
        /// 创建初始先验参数值和协方差阵。只会执行一次。
        /// </summary>
        /// <returns></returns>
        protected override WeightedVector CreateInitAprioriParam()
        {
            return ClockEstimationMatrixHelper.GetInitAprioriParam(this.ParamCount, this.CurrentMaterial.Count, EnabledPrns.Count);          
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
                Vector L = new Vector(ObsCount*2);
                //Parallel.ForEach(CurrentMaterial, new Action<EpochInformation>(delegate(EpochInformation epoch)
                //{
                //    GetEpochObservationVector(L1, epoch);
                //}));
                
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

        private void GetEpochObservationVector(Vector L, EpochInformation epoch)
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

                var SiteSatAmbiguityName = this.GnssParamNameBuilder.GetSiteSatAmbiguityParamName(item);
                int IndexOfSiteSatAmbiguityName = ParamNames.IndexOf(SiteSatAmbiguityName);

                int row = IndexOfSiteSatAmbiguityName - BaseCount;//行的索引号
                int next = row + ObsCount;

                L[row] = rangeVector[index];
                L[next] = phaseVector[index];
                index++;
            }
        }

        /// <summary>
        /// 观测量的权逆阵，一个对角阵。按照先伪距，后载波的顺序排列。
        /// 标准差由常数确定，载波比伪距标准差通常是 1:100，其方差比是 1:10000.
        /// 1.0/140*140=5.10e-5
        /// </summary> 
        /// <param name="factor">载波和伪距权逆阵因子（模糊度固定后，才采用默认值！）</param>
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
                    inverseWeight[ index1 + ObsCount] = inverseWeightOfSat * invFactorOfPhase;
                    index1++;
                }
            }

            //start = DateTime.Now;
            //int index = 0;
            //foreach (var epoch in this.CurrentMaterial)
            //{
            //    foreach (var prn in epoch.EnabledPrns)
            //    {
            //        EpochSatellite e = epoch[prn];
            //        double inverseWeightOfSat = SatWeightProvider.GetInverseWeight(e);
            //        //inverseWeight[index][index] = inverseWeightOfSat * invFactorOfRange;
            //        //inverseWeight[index + ObsCount][index + ObsCount] = inverseWeightOfSat * invFactorOfPhase;
            //        inverseWeight[index, index] = inverseWeightOfSat * invFactorOfRange;
            //        inverseWeight[index + ObsCount, index + ObsCount] = inverseWeightOfSat * invFactorOfPhase;
            //        index++;
            //    }
            //}
            //span = DateTime.Now - start;
            //Console.WriteLine(span.TotalMilliseconds + "ms计算Q-");


            //var ss = inverseWeight - inverseWeight1;
            return inverseWeight;
        }

        private void InverseWeightOfEpoch(double[] inverseWeightVector, double invFactorOfRange, double invFactorOfPhase, EpochInformation epoch)
        {
            foreach (var prn in epoch.EnabledPrns)
            {
                EpochSatellite e = epoch[prn];
                var SiteSatAmbiguityName = this.GnssParamNameBuilder.GetSiteSatAmbiguityParamName(e);
                int IndexOfSiteSatAmbiguityName = ParamNames.IndexOf(SiteSatAmbiguityName);

                int row = IndexOfSiteSatAmbiguityName - BaseCount;//行的索引号
                int next = row + ObsCount;

                double inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(e);
                inverseWeightVector[row] = inverseWeightOfSat * invFactorOfRange;
                inverseWeightVector[next] = inverseWeightOfSat * invFactorOfPhase;
            }
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
                //Parallel.ForEach(CurrentMaterial, new Action<EpochInformation>(delegate(EpochInformation epoch)
                //    {
                //        var wetTropName = this.GnssParamNameBuilder.GetReceiverWetTropParamName(epoch);
                //        int IndexOfwetTropName = ParamNames.IndexOf(wetTropName);

                //        GetEpochCoeffOfDesign(B, epoch, IndexOfwetTropName);
                //    }));
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

                        if (item.SiteName.ToUpper() == "AMC2" || item.SiteName.ToUpper() == "AJAC")// || key.SiteName.ToUpper() == "ALIC"|| key.SiteName.ToUpper() == "DAV1")//  i != 0)
                        {

                        }
                        else
                        {
                            A[row][i] = 1.0;//接收机钟差对应的距离 = clkError * 光速
                            A[next][i] = 1.0;//接收机钟差对应的距离 = clkError * 光速
                        }

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
               // return new SparseMatrix(A);
                return new Matrix(A);
            }
        }

        private void GetEpochCoeffOfDesign(double[][] A, EpochInformation epoch, int IndexOfwetTropName)
        {
            foreach (var prn in epoch.EnabledSats)// 一颗卫星2行
            {
                var receiverClockName = this.GnssParamNameBuilder.GetReceiverClockParamName(epoch);
                int IndexOfreceiverClockName = ParamNames.IndexOf(receiverClockName);

                var satelliteName = this.GnssParamNameBuilder.GetSatClockParamName(prn.Prn);
                int IndexOfsatelliteName = ParamNames.IndexOf(satelliteName);

                var SiteSatAmbiguityName = this.GnssParamNameBuilder.GetSiteSatAmbiguityParamName(prn);
                int IndexOfSiteSatAmbiguityName = ParamNames.IndexOf(SiteSatAmbiguityName);

                int row = IndexOfSiteSatAmbiguityName - BaseCount;//行的索引号
                int next = row + ObsCount;

                double wetMap = epoch[prn.Prn].WetMap;

                double[] A1 = A[row];
                double[] A2 = A[next];
                if (IndexOfreceiverClockName != 0)
                {
                    A1[IndexOfreceiverClockName] = 1.0;//接收机钟差对应的距离 = clkError * 光速
                    A2[IndexOfreceiverClockName] = 1.0;//接收机钟差对应的距离 = clkError * 光速
                }

                A1[IndexOfwetTropName] = wetMap;
                A1[IndexOfsatelliteName] = -1.0;//卫星钟差对应的距离 = clkError * 光速

                A2[IndexOfwetTropName] = wetMap;
                A2[IndexOfsatelliteName] = -1.0;//卫星钟差对应的距离 = clkError * 光速
                A2[IndexOfSiteSatAmbiguityName] = 1;//模糊度,保持以m为单位 

            }
        }

        #endregion


        #region 公共矩阵生成 备份

        /// <summary>
        /// 参数平差系数阵 A。前一半行数为伪距观测量，后一半为载波观测量。构建为稀疏矩阵
        /// </summary>
        //public override Matrix CoeffOfDesign
        //{
        //    get
        //    {
        //        int rowCount = ObsCount * 2;
        //        int colCount = ParamCount;
        //        double[][] A = Geo.Utils.MatrixUtil.Create(rowCount, colCount);
        //        int row = 0;//行的索引号

        //        int i = 0;
        //        Time time = CurrentMaterial.ReceiverTime;

        //        Parallel.ForEach(CurrentMaterial, new Action<EpochInformation>(delegate(EpochInformation EpochInformation1)
        //        {

        //        }));
        //        foreach (var key in CurrentMaterial)
        //        {

        //            var wetTropName = this.GnssParamNameBuilder.GetReceiverWetTropParamName(key);
        //            int IndexOfwetTropName = ParamNames.IndexOf(wetTropName);

        //            foreach (var prn in key.EnabledSats)// 一颗卫星2行
        //            {
        //                double wetMap = key[prn.Prn].WetMap;

        //                //double wetMap0 = key[prn.Prn].Vmf1WetMap;
        //                int next = row + ObsCount;
        //                double[] A1 = A[row];
        //                double[] A2 = A[next];
        //                if (i != 0)
        //                {
        //                    A1[i] = 1.0;//接收机钟差对应的距离 = clkError * 光速
        //                    A2[i] = 1.0;//接收机钟差对应的距离 = clkError * 光速
        //                }

        //                var satelliteName = this.GnssParamNameBuilder.GetSatClockParamName(prn.Prn);
        //                var SiteSatAmbiguityName = this.GnssParamNameBuilder.GetSiteSatAmbiguityParamName(prn);

        //                int IndexOfsatelliteName = ParamNames.IndexOf(satelliteName);

        //                A1[IndexOfwetTropName] = wetMap;
        //                A1[IndexOfsatelliteName] = -1.0;//卫星钟差对应的距离 = clkError * 光速

        //                A2[IndexOfwetTropName] = wetMap;
        //                A2[IndexOfsatelliteName] = -1.0;//卫星钟差对应的距离 = clkError * 光速
        //                A2[ParamNames.IndexOf(SiteSatAmbiguityName)] = 1;//模糊度,保持以m为单位 

        //                row++;
        //            }
        //            i++;
        //        }
        //        return new SparseMatrix(A);
        //    }
        //}

        #endregion
    }
  }
