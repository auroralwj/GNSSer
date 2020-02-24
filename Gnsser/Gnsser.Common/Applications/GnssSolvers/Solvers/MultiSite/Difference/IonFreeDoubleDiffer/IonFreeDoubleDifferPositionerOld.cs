//2014.10.06, czs, create in hailutu, 差分定位
//2014.12.07, czs, edit in jinxinliaomao shaungliao, 名称改为 PppResidualDifferPositioner
//2015.05.17, cy,  修复bug，添加两个"历元信息构建器",分别为基准站和流动站的。
//2016.04.24, czs, edit in hongqing, 继承类重构，自 MultiSiteGnssSolver
//2018.12.10, cy, edit in chongqing, 调通无电离层组合

using System;
using System.Collections.Generic;
using Gnsser.Domain;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Filter;
using Gnsser.Checkers;
using Geo.Common;
using Gnsser;

namespace Gnsser.Service
{
    /// <summary>
    ///  无电离层双差，单历元解算。
    ///  方法：以一个站作为参考站，另一个作为流动站（待算站)
    /// </summary>
    public class IonFreeDoubleDifferPositionerOld : MultiSiteEpochSolver
    {
        #region 构造函数 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="GnssOption"></param>
        public IonFreeDoubleDifferPositionerOld(DataSourceContext DataSourceContext, GnssProcessOption GnssOption)
        : base(DataSourceContext, GnssOption)
        {
            //双差模糊度固定
            this.DoubleIonFreeAmReslution = new DoubleIonFreeAmbiguityReslution();
            this.ResidualsAnalysis = new ResidualsAnalysis();
            this.DualBandCycleSlipDetector = new DualBandCycleSlipDetector();
            this.Name = "无电离层组合双差";
            this.BaseParamCount = 5;

            this.IsBaseSatelliteRequried = true;//强制启用基准星

            var distanceOfBaseline = (DataSourceContext.ObservationDataSources.BaseDataSource.SiteInfo.ApproxXyz - DataSourceContext.ObservationDataSources.OtherDataSource.SiteInfo.ApproxXyz).Length;
            if (distanceOfBaseline <= GnssOption.MaxDistanceOfShortBaseLine)
            {
                this.BaseParamCount = 3;
            }
            else if (GnssOption.MaxDistanceOfShortBaseLine < distanceOfBaseline && distanceOfBaseline < GnssOption.MinDistanceOfLongBaseLine)
            {
                this.BaseParamCount = 4;
            }
            else
            {
                this.BaseParamCount = 5;
            }

            this.Option.IsBaseSatelliteRequried = true;//强制启用基准星 
            this.MatrixBuilder = new IonFreeDoubleDifferMatrixBuilder(Option, this.BaseParamCount);

            //双差模糊度固定
            this.DoubleIonFreeAmReslution = new DoubleIonFreeAmbiguityReslution(this.BaseParamCount);
            this.ResidualsAnalysis = new ResidualsAnalysis();
        }

        #endregion

        #region 属性  

        /// <summary>
        /// 双差模糊度固定
        /// </summary>
        public DoubleIonFreeAmbiguityReslution DoubleIonFreeAmReslution { get; set; }

        /// <summary>
        /// 残差分析,判断是否有未发现的周跳或粗差
        /// </summary>
        public ResidualsAnalysis ResidualsAnalysis { get; set; }

        /// <summary>
        /// 双频周跳探测
        /// </summary>
        public DualBandCycleSlipDetector DualBandCycleSlipDetector { get; set; }


        static object locker = new object();

        #endregion         

        #region 核心计算方法
        /// <summary>
        /// 构建矩阵
        /// </summary>
        public override void BuildAdjustMatrix()
        {
            var builder = (IonFreeDoubleDifferMatrixBuilder)MatrixBuilder;
            builder.SetMaterial(this.CurrentMaterial).SetPreviousProduct(this.CurrentProduct);
            builder.BaseParamCount = BaseParamCount;
            builder.SetBasePrn(this.CurrentBasePrn);
            builder.Build();
        }


        #region 卡尔曼滤波
        /// <summary>
        /// PPP 计算核心方法。 Kalmam滤波。
        /// 观测量的顺序是先伪距观测量，后载波观测量，观测量的总数为卫星数量的两倍。
        /// 参数数量为卫星数量加5,卫星数量对应各个模糊度，5为3个位置量xyz，1个接收机钟差量，1个对流程湿分量。
        /// </summary>
        /// <param name="mSiteEpochInfo">接收信息</param> 
        /// <param name="lastResult">上次解算结果（用于 Kalman 滤波）,若为null则使用初始值计算</param>
        /// <returns></returns>
        public override BaseGnssResult CaculateKalmanFilter(MultiSiteEpochInfo mSiteEpochInfo, BaseGnssResult lastResult = null)
        {
            //return base.CaculateKalmanFilter(mSiteEpochInfo, lastResult);

            var refEpoch = mSiteEpochInfo.BaseEpochInfo;
            var rovEpoch = mSiteEpochInfo.OtherEpochInfo;

            IonFreeDoubleDifferPositionResult last = null;
            if (lastResult != null) last = (IonFreeDoubleDifferPositionResult)lastResult;

            //双差必须观测到2颗以上卫星，否则返回空
            if (refEpoch.EnabledPrns.Count < 2 && rovEpoch.EnabledPrns.Count < 2)
            {
                return null;
            }

            //随机噪声模型合适否？？？？？？

            //DualBandCycleSlipDetector.Dector(ref recInfo, ref currentRefInfo, CurrentBasePrn);

            //this.Adjustment = new KalmanFilter(MatrixBuilder);
            this.Adjustment = this.RunAdjuster(BuildAdjustObsMatrix(this.CurrentMaterial));//         BuildAdjustObsMatrix(this.CurrentMaterial));

            if (Adjustment.Estimated == null)
            {
                return null;
            }
            this.Adjustment.ResultType = ResultType.Float;

            ////验后残差分析，探测是否漏的周跳或粗差
            //bool isCS = ResidualsAnalysis.Detect(ref refEpoch, ref rovEpoch, Adjustment, CurrentBasePrn);

            //while (isCS)
            //{
            //    if (refEpoch.EnabledSatCount > 4) //1个基准星+n
            //    {
            //        this.MatrixBuilder = new IonFreeDoubleDifferMatrixBuilder(Option, BaseParamCount);
            //        //  this.MatrixBuilder.SetEpochInfo(recInfo).SetPreviousResult(lastPppResult);


            //        this.CurrentMaterial.BaseEpochInfo = refEpoch;
            //        this.CurrentMaterial.OtherEpochInfo = rovEpoch;
            //        this.MatrixBuilder.SetMaterial(this.CurrentMaterial).SetPreviousProduct(this.CurrentProduct);
            //        this.MatrixBuilder.SetBasePrn(this.CurrentBasePrn);
            //        this.MatrixBuilder.Build();

            //        //this.Adjustment = new KalmanFilter(MatrixBuilder);
            //        //this.Adjustment = new SimpleKalmanFilterOld(MatrixBuilder);
            //        //this.Adjustment.Process();
            //        this.Adjustment = this.RunAdjuster(BuildAdjustObsMatrix(this.CurrentMaterial));
            //        if (Adjustment.Estimated == null)
            //        {
            //            return null;
            //        }
            //        isCS = ResidualsAnalysis.Detect(ref refEpoch, ref rovEpoch, Adjustment, CurrentBasePrn);
            //    }
            //    else
            //    { isCS = false; }
            //}

            //模糊度固定解
            if (Option.IsFixingAmbiguity)
            {
                //尝试固定模糊度
                lock (locker)
                {
                    //尝试固定模糊度
                    var fixIonFreeAmbiCycles = DoubleIonFreeAmReslution.Process(rovEpoch, refEpoch, Adjustment, CurrentBasePrn);
                    //是否保存固定的双差LC模糊度信息，继承已固定的？ 建议不用， 始终动态更新      
                    // 对过去已固定，但当前历元却没有固定，是否继承下来？根据是否发生基准星变化、周跳等信息判断
                    WeightedVector preFixedVec = new WeightedVector(); //上一次固定结果，这里好像没有用，//2018.07.31， czs

                    //作为约束条件，重新进行平差计算
                    if (fixIonFreeAmbiCycles != null && fixIonFreeAmbiCycles.Count > 0)//
                    {
                        fixIonFreeAmbiCycles.InverseWeight = new Matrix(new DiagonalMatrix(fixIonFreeAmbiCycles.Count, 1e-10));

                        WeightedVector NewEstimated = Adjustment.SolveAmbiFixedResult(fixIonFreeAmbiCycles, preFixedVec, Option.IsFixParamByConditionOrHugeWeight);

                        XYZ newXYZ = new XYZ(NewEstimated[0], NewEstimated[1], NewEstimated[2]);
                        XYZ oldXYZ = new XYZ(Adjustment.Estimated[0], Adjustment.Estimated[1], Adjustment.Estimated[2]);
                        //模糊度固定错误的判断准则（参考李盼论文）
                        if ((newXYZ - oldXYZ).Length > 0.05 && newXYZ.Length / oldXYZ.Length > 1.5)
                        {
                            //模糊度固定失败,则不用
                        }
                        else
                        {
                            int nx = Adjustment.Estimated.Count;
                            //nx = 3;
                            for (int i = 0; i < nx; i++)
                            {
                                Adjustment.Estimated[i] = NewEstimated[i];
                                // for (int j = 0; j < nx ; j++) adjustment.Estimated.InverseWeight[i, j] = Estimated.InverseWeight[i, j];
                            }
                        }
                    }
                    this.Adjustment.ResultType = ResultType.Fixed;
                    //替换固定的模糊度参数，重新平差，依然不对啊  
                    #region 参考Ge论文给的Blewitt 1989论文思路
                    //if (fixIonFreeAmbiCycles.Count > 1)
                    //{
                    //    Vector newAprioriParam = this.MatrixBuilder.AprioriParam;
                    //    IMatrix newAprioriParamCovInverse = this.MatrixBuilder.AprioriParam.InverseWeight.Inversion;

                    //    for (int i = 0; i < fixIonFreeAmbiCycles.Count; i++)
                    //    {
                    //        //对所有的参数
                    //        var name = fixIonFreeAmbiCycles.ParamNames[i];
                    //        int j = this.MatrixBuilder.ParamNames.IndexOf(name);
                    //        newAprioriParam[j] = fixIonFreeAmbiCycles[i];
                    //        newAprioriParamCovInverse[j, j] = 1e28;
                    //    }

                    //    IMatrix TransferMatrix = MatrixBuilder.Transfer;
                    //    IMatrix AprioriParam = new Matrix(newAprioriParam);
                    //    IMatrix CovaOfAprioriParam = newAprioriParamCovInverse.GetInverse();
                    //    IMatrix InverseWeightOfTransfer = MatrixBuilder.Transfer.InverseWeight;
                    //    IMatrix PredictParam = TransferMatrix.Multiply(AprioriParam);//.Plus(controlMatrix.Multiply(controlInputVector));
                    //    IMatrix CovaOfPredictParam1 = AdjustmentUtil.BQBT(TransferMatrix, CovaOfAprioriParam).Plus(InverseWeightOfTransfer);

                    //    //简化字母表示
                    //    Matrix Q1 = new Matrix(CovaOfPredictParam1);
                    //    Matrix P1 = new Matrix(CovaOfPredictParam1.GetInverse());
                    //    Matrix X1 = new Matrix(PredictParam);
                    //    Matrix L = new Matrix(MatrixBuilder.Observation.Array);
                    //    Matrix A = new Matrix(MatrixBuilder.Coefficient);
                    //    Matrix AT = new Matrix(A.Transposition);
                    //    Matrix Po = new Matrix(MatrixBuilder.Observation.InverseWeight.GetInverse());
                    //    Matrix Qo = new Matrix(MatrixBuilder.Observation.InverseWeight);
                    //    Matrix ATPA = new Matrix(AdjustmentUtil.ATPA(A, Po));
                    //    //平差值Xk的权阵
                    //    Matrix PXk = null;
                    //    if (Po.IsDiagonal) { PXk = ATPA + P1; }
                    //    else { PXk = AT * Po * A + P1; }

                    //    //计算平差值的权逆阵
                    //    Matrix Qx = PXk.Inversion;
                    //    Matrix J = Qx * AT * Po;

                    //    //计算平差值
                    //    Matrix Vk1 = A * X1 - L;//计算新息向量
                    //    Matrix X = X1 - J * Vk1;
                    //    var Estimated = new WeightedVector(X, Qx) { ParamNames = MatrixBuilder.Observation.ParamNames };
                    //    WeightedVector NewAdjustment = Estimated;
                    //    int nx = Adjustment.Estimated.Count;
                    //    for (int i = 0; i < nx; i++)
                    //    {
                    //        Adjustment.Estimated[i] = NewAdjustment[i];
                    //        // for (int j = 0; j < nx ; j++) adjustment.Estimated.InverseWeight[i, j] = Estimated.InverseWeight[i, j];
                    //    }
                    //}

                    #endregion

                    //替换固定的模糊度参数，重新平差，依然不对啊  
                    #region 参考Dong 1989论文方法
                    //if (fixIonFreeAmbiCycles.Count > 0)
                    //{
                    //    WeightedVector estIonFreeAmbiVector = this.Adjustment.Estimated.GetWeightedVector(fixIonFreeAmbiCycles.ParamNames);
                    //    List<string> paramNames = new List<string>();
                    //    foreach (var item in this.Adjustment.Estimated.ParamNames)
                    //    {
                    //        if (!fixIonFreeAmbiCycles.ParamNames.Contains(item))
                    //        {
                    //            paramNames.Add(item);
                    //        }
                    //    }
                    //    foreach (var item in fixIonFreeAmbiCycles.ParamNames) paramNames.Add(item);


                    //    var Estimate = this.Adjustment.Estimated;

                    //    var orderEstimate = Estimate.SortByName(paramNames);

                    //    Matrix order = new Matrix(paramNames.Count, paramNames.Count);
                    //    for(int i=0;i<paramNames.Count;i++)
                    //    {
                    //        int j = Estimate.ParamNames.IndexOf(paramNames[i]);
                    //        order[i, j] = 1;
                    //    }

                    //    Matrix X1 = new Matrix(Estimate.Array);
                    //    Matrix QX1 = new Matrix(Estimate.InverseWeight);

                    //    Matrix newX1 = order * X1;
                    //    Matrix newX1Cov = order * QX1 * order.Transpose();

                    //    int n1 = Estimate.ParamNames.Count - fixIonFreeAmbiCycles.Count;
                    //    Matrix Q12 = newX1Cov.GetSub(0, n1, n1, fixIonFreeAmbiCycles.Count);
                    //    Matrix Q22 = newX1Cov.GetSub(n1, n1, fixIonFreeAmbiCycles.Count, fixIonFreeAmbiCycles.Count);

                    //    Matrix detX2 = new Matrix(fixIonFreeAmbiCycles.Count,1);
                    //    for(int i=0;i<fixIonFreeAmbiCycles.Count;i++)
                    //    {
                    //        int j = Estimate.ParamNames.IndexOf(fixIonFreeAmbiCycles.ParamNames[i]);
                    //        detX2[i, 0] = fixIonFreeAmbiCycles[i] - Estimate.Data[j];

                    //    }

                    //    Matrix X = Q12 * Q22.Inversion * detX2;

                    //    Vector newX = new Vector();
                    //    for (int i = 0; i < X.RowCount; i++) newX.Add(X[i, 0], paramNames[i]);
                    //    for (int i = 0; i < fixIonFreeAmbiCycles.Count; i++) newX.Add(detX2[i,0], fixIonFreeAmbiCycles.ParamNames[i]);

                    //    WeightedVector newEstrimate = new WeightedVector(newX);
                    //    newEstrimate.ParamNames = paramNames;


                    //    int nx = Adjustment.Estimated.Count;
                    //    for (int i = 0; i < 3; i++)
                    //    {
                    //        int j = newEstrimate.ParamNames.IndexOf(Adjustment.Estimated.ParamNames[i]);
                    //        Adjustment.Estimated[i] += newEstrimate[j];
                    //        // for (int j = 0; j < nx ; j++) adjustment.Estimated.InverseWeight[i, j] = Estimated.InverseWeight[i, j];
                    //    }
                    //}
                    #endregion

                }
            }

            if (Adjustment.Estimated != null)
            {
                var DDResidualDifferPositionResult = BuildResult();


                //if (Option.PositionType == PositionType.动态定位)
                //{
                //    mSiteEpochInfo.OtherEpochInfo.SiteInfo.EstimatedXyz = ((IonFreeDoubleDifferPositionResult)DDResidualDifferPositionResult).EstimatedXyzOfRov;
                //}

                //double[] v = PppResidualDifferPositionResult.Adjustment.PostfitResidual.OneDimArray;

                //int k = recInfo.EnabledPrns.IndexOf(CurrentBasePrn);

                //for (int i = 0; i < recInfo.EnabledSatCount; i++)
                //{
                //    SatelliteNumber prn = recInfo[i].Prn;

                //    if (prn != CurrentBasePrn)
                //    { 
                //        List<double> tmp = new List<double>();
                //        if (!posfit.ContainsKey(prn)) posfit.Add(prn, tmp);

                //        if (i < k)
                //        {
                //            posfit[prn].Add(v[i + recInfo.EnabledSatCount - 1]);
                //        }
                //        else
                //        { posfit[prn].Add(v[i - 1 + recInfo.EnabledSatCount - 1]); }
                //    }
                //}

             //   this.SetProduct(DDResidualDifferPositionResult);


                return DDResidualDifferPositionResult;
            }
            else
                return null;
        }
        #endregion


        /// <summary>
        /// 验后残差
        /// </summary>
        public Dictionary<SatelliteNumber, List<double>> posfit = new Dictionary<SatelliteNumber, List<double>>();

        #endregion 

        /// <summary>
        /// 结果
        /// </summary>
        /// <returns></returns>
        public override BaseGnssResult BuildResult()
        {
            var result = new IonFreeDoubleDifferPositionResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder, CurrentBasePrn, BaseParamCount);
            return result;
        }
    }
}
